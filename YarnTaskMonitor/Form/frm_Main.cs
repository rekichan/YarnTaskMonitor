using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace YarnTaskMonitor
{
    public partial class frm_Main : Form
    {

        #region Properties
        private MySqlConnection conn;
        private cls_Config config;
        private cls_Logger logger;
        private bool auto;
        private bool forceClose;
        #endregion

        #region API
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void PostMessage(IntPtr hWnd, int msg, IntPtr wParam, int lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        #endregion

        #region Constructor
        public frm_Main()
        {
            InitializeComponent();
        }
        #endregion

        #region FormEvent
        private void frm_Main_Load(object sender, EventArgs e)
        {
            cls_Common.hwndFrmMain = this.Handle;
            logger = cls_Logger.Instance;

            InitConfig();

            if (!InitMySQL())
            {
                tsb_Collect.Enabled = false;
                tsb_AutoCollect.Enabled = false;
                tsb_ConnectDatabase.Enabled = false;
                MessageBox.Show("连接MySQL失败", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
                tsb_ConnectDatabase.Enabled = false;
        }

        private void frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!forceClose && cls_Common.taskBar == "true")
            {
                this.Visible = false;
                e.Cancel = true;
            }

            auto = false;

            if (conn != null)
                conn.Close();
        }
        #endregion

        #region Event
        private void tsb_Truncate_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要清空数据库?", "Q", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
                return;

            string sql = string.Format("truncate table {0}", cls_Common.table);
            MySqlCommand cmd = new MySqlCommand(sql, conn);//实例化MySQL指令对象
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        private void dgv_Trace_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                (sender as DataGridView).CurrentRow.Selected = false;
                (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
            }
        }

        private void tsmi_CopyText_Click(object sender, EventArgs e)
        {
            string text = (string)dgv_Trace.SelectedCells[0].Value;
            Clipboard.SetText(text);
        }

        private void tsmi_ObserveResource_Click(object sender, EventArgs e)
        {
            cht_Main.Series["Memory"].Points.Clear();
            cht_Main.Series["vCores"].Points.Clear();
            cht_Main.Series["Containers"].Points.Clear();

            int row = dgv_Trace.SelectedCells[0].RowIndex;
            string taskId = (string)dgv_Trace.Rows[row].Cells[0].Value;
            string taskName = (string)dgv_Trace.Rows[row].Cells[1].Value;
            string sql = "select containers,cores,memory,insertTime from " + cls_Common.table + " where taskId = '" + taskId + "' and taskName  = '" + taskName + "' order by insertTime asc";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            List<int> containers = new List<int>();
            List<int> cores = new List<int>();
            List<int> memory = new List<int>();
            List<DateTime> time = new List<DateTime>();
            while (reader.Read())
            {
                containers.Add((int)reader[0]);
                cores.Add((int)reader[1]);
                memory.Add((int)reader[2]);
                string buf = reader[3].ToString();
                time.Add(DateTime.Parse(buf));
            }

            cht_Main.Series["Memory"].Points.DataBindXY(time, memory);
            cht_Main.Series["vCores"].Points.DataBindXY(time, cores);
            cht_Main.Series["Containers"].Points.DataBindXY(time, containers);
            reader.Close();
        }

        private void ico_Main_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!this.Visible)
                this.Visible = true;
        }

        private void tsb_GetTask_Click(object sender, EventArgs e)
        {
            string sql = "select taskId,taskName,max(containers),avg(containers),max(cores),avg(cores),max(memory),avg(memory) from " + cls_Common.table + " group by taskId,taskName";
            MySqlDataAdapter mda = new MySqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            dgv_Trace.DataSource = dt;
            dgv_Trace.Columns[0].Width = 195;
            dgv_Trace.Columns[1].Width = 195;
            dgv_Trace.Columns[dgv_Trace.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            mda.Dispose();
        }

        private void tsmi_ShowMain_Click(object sender, EventArgs e)
        {
            this.Visible = true;
        }

        private void tsmi_Exit_Click(object sender, EventArgs e)
        {
            forceClose = true;
            auto = false;
            this.Close();
        }

        private void tsb_Setting_Click(object sender, EventArgs e)
        {
            frm_Setting setting = new frm_Setting();
            setting.ShowDialog();
        }

        private void tsb_AutoCollect_Click(object sender, EventArgs e)
        {
            if (auto)
                auto = false;
            else
            {
                int interval = Convert.ToInt32(cls_Common.autoInterval);
                if (interval <1000)
                {
                    MessageBox.Show("自动采集间隔应>=5000ms", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                auto = true;
                //实例化线程对象
                System.Threading.Thread threadWork = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(AutoCollect));
                threadWork.IsBackground = true;//设置后台线程
                threadWork.Start(interval);
            }
        }

        private void tsb_ConnectDatabase_Click(object sender, EventArgs e)
        {
            if (!InitMySQL())
            {
                tsb_Collect.Enabled = false;
                tsb_AutoCollect.Enabled = false;
                tsb_ConnectDatabase.Enabled = false;
                MessageBox.Show("连接MySQL失败", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
                tsb_ConnectDatabase.Enabled = false;
        }

        private void tsb_Collect_Click(object sender, EventArgs e)
        {
            ParseHtmlAndSaveMySQL(CatchHtml());
        }
        #endregion

        #region Function
        /// <summary>
        /// 初始化MySQL连接
        /// </summary>
        /// <returns></returns>
        private bool InitMySQL()
        {
            try
            {
                //实例化MySQL连接
                //server=127.0.0.1;port=3306;user=root;password=123456; database=hdp;
                conn = new MySqlConnection(cls_Common.connetionCmd);
                conn.Open();
            }
            catch (Exception ex)
            {
                logger.WriteExceptionLog(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 初始化配置信息
        /// </summary>
        private void InitConfig()
        {
            //获取配置文件
            config = cls_Config.getInstance(Application.StartupPath + @"\Parameter\config.ini");
            cls_Common.connetionCmd = config.IniReadValue("setting", "connetionCmd", "");
            cls_Common.table = config.IniReadValue("setting", "table", "");
            cls_Common.autoInterval = config.IniReadValue("setting", "autoInterval", "5000");
            cls_Common.taskBar = config.IniReadValue("setting", "taskBar", "false");
        }

        /// <summary>
        /// 时间戳转换日期
        /// </summary>
        /// <param name="ts">时间戳</param>
        /// <returns>日期</returns>
        private string Ts2Date(string ts)
        {
            DateTime dt = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0));
            return dt.AddMilliseconds(Convert.ToInt64(ts)).ToString();
        }

        /// <summary>
        /// 获取html网页信息
        /// </summary>
        /// <returns></returns>
        private string CatchHtml()
        {
            string url = "http://slave33.bl.bigdata:8088/cluster/apps/RUNNING";//指定yarn application url
            string html;
            try
            {
                WebRequest request = WebRequest.Create(url);//实例化WebRequest对象  
                WebResponse response = request.GetResponse();//创建WebResponse对象  
                Stream datastream = response.GetResponseStream();//创建流对象  
                Encoding ec = Encoding.UTF8;//指定html字符为utf8
                StreamReader reader = new StreamReader(datastream, ec);
                html = reader.ReadToEnd();//读取网页html
                reader.Close();
                reader.Dispose();
                datastream.Close();
                response.Close();
                GC.Collect();
            }
            catch (Exception ex)
            {
                logger.WriteExceptionLog(ex);
                return null;
            }
            return html;
        }

        /// <summary>
        /// 解析html信息并保存至MySQL
        /// </summary>
        /// <param name="html">html网页信息</param>
        /// <returns></returns>
        private bool ParseHtmlAndSaveMySQL(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return false;

            //html预处理
            string src = html;
            html = Regex.Split(html, "var appsTableData")[1];
            html = Regex.Split(html, "</script>")[0];
            html = html.Trim();
            html = html.Replace("\n", "");
            html = html.Substring(3);
            html = html.Substring(0, html.Length - 2);
            string[] htmls = Regex.Split(html, @"\],\[");
            src = Regex.Split(src, "Cluster Nodes Metrics")[0];
            src = Regex.Split(src, "<tbody class")[1];
            src = src.Replace("\n", "");
            src = src.Replace(" ", "");
            MatchCollection mc = Regex.Matches(src, @"<td>[A-Z a-z 0-9 .]+</td>");
            string curContainers = mc[4].Value.Replace("<td>", "").Replace("</td>", "");
            string curMemory = mc[5].Value.Replace("<td>", "").Replace("</td>", "").ToUpper();
            int curMem = Convert.ToInt32( Regex.Match(curMemory, @"[1-9]+").Value);
            if (curMemory.ToUpper().Contains("TB"))
                curMem = curMem * 1024;
            curMemory = curMem.ToString();
            string curCores = mc[8].Value.Replace("<td>", "").Replace("</td>", "");
            try
            {
                MySqlCommand cmd = new MySqlCommand();//实例化MySQL指令对象
                MySqlTransaction tx = conn.BeginTransaction();//启动事务
                cmd.Transaction = tx;
                cmd.Connection = conn;
                for (int i = 0; i < htmls.Length; i++)
                {
                    //html内容分割
                    //"<a href='/cluster/app/application_1639795714599_36499'>application_1639795714599_36499</a>",0
                    //"hive",1
                    //"SparkStreamingPrimaryKeyFmsdbdgMain",2
                    //"SPARK",3
                    //"root.users.supergroup",4
                    //"1640430607234",5
                    //"0",6
                    //"RUNNING",7
                    //"UNDEFINED",8
                    //"3",9
                    //"5",10
                    //"31744",11
                    //"<br title='10.0'> <div class='ui-progressbar ui-widget ui-widget-content ui-corner-all' title='10.0%'> <div class='ui-progressbar-value ui-widget-header ui-corner-left' style='width:10.0%'> </div> </div>","<a href='http://slave33.bl.bigdata:8088/proxy/application_1639795714599_36499/'>ApplicationMaster</a>"

                    //20211230 taskName引发的exception：因为分割','导致索引发生变化解析时间戳报错
                    //"CREATE TABLE recommendat...date,category_sid(Stage-1)"
                    string sub = htmls[i].Replace("\\", "");
                    string[] cols = Regex.Split(sub, "\",\"");
                    string buf = Regex.Match(cols[0], @">[a-z _ 0-9]+<").Value;//正则匹配taskId
                    string taskId = buf.Substring(1, buf.Length - 2);
                    string taskName = cols[2];
                    string taskType = cols[3];
                    string queue = cols[4];
                    string startTime = Ts2Date(cols[5]);
                    string containers = cols[9];
                    string cores = cols[10];
                    string memory = cols[11];
                    int mem = Convert.ToInt32(memory);
                    memory=Math.Round(mem / 1024.0, 4).ToString();
                    string sql = string.Format("insert into {9} values ('{0}','{1}','{2}','{3}','{4}',{5},{6},{7},'{8}')", taskId, taskName, taskType, queue, startTime, containers, cores, memory, DateTime.Now.ToString(),cls_Common.table);
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
                //写入一次Multi信息就行了,a1是为了保证第一位
                string multiSql = string.Format("insert into {9} values ('{0}','{1}','{2}','{3}','{4}',{5},{6},{7},'{8}')", "a1", "Multi", "Multi", "Multi", "Multi", curContainers, curCores, curMemory, DateTime.Now.ToString(), cls_Common.table);
                cmd.CommandText = multiSql;
                cmd.ExecuteNonQuery();
                tx.Commit();//使用事务主要是为了insert batch提高效率
            }
            catch (Exception ex)
            {
                logger.WriteExceptionLog(ex);
                //logger.WriteLog(src);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 线程睡眠
        /// </summary>
        /// <param name="ms">时长(ms)</param>
        private void Delay(int ms)
        {
            System.Threading.Thread.Sleep(ms);
        }

        /// <summary>
        /// 自动锁定/释放控件
        /// </summary>
        /// <param name="enable">锁定?</param>
        private void AutoLockUI(bool enable)
        {
            tsb_Collect.Enabled = enable;
            tsb_ConnectDatabase.Enabled = enable;
            tsb_Setting.Enabled = enable;
            tsb_Truncate.Enabled = enable;
            tsb_AutoCollect.Text = enable ? "自动采集" : "停止采集";
        }
        #endregion

        #region Thread
        private void AutoCollect(object para)//自动采集线程
        {
            PostMess(cls_Common.hwndFrmMain, cls_Common.AUTO_LOCK_UI);
            int interval = (int)para;
            while (auto)
            {
                if (!ParseHtmlAndSaveMySQL(CatchHtml()))
                    break;

                Delay(interval);
            }
            PostMess(cls_Common.hwndFrmMain, cls_Common.AUTO_RELEASE_UI);
        }
        #endregion

        #region Message
        /// <summary>
        /// PostMessage方法发送消息，带穿字符串内容
        /// </summary>
        /// <param name="Handle">目标句柄</param>
        /// <param name="msg">发送的消息</param>
        /// <param name="wParam">发送的内容</param>
        public void PostMess(IntPtr Handle, int msg, string wParam)
        {
            IntPtr p = System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(wParam);

            if (Handle != IntPtr.Zero)
            {
                PostMessage(Handle, msg, p, 0);
            }
        }

        /// <summary>
        /// PostMessage方法发送消息
        /// </summary>
        /// <param name="handle">目标句柄</param>
        /// <param name="msg">发送的消息</param>
        public void PostMess(IntPtr handle, int msg)
        {
            PostMessage(handle, msg, 0, 0);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case cls_Common.AUTO_LOCK_UI:
                    AutoLockUI(false);
                    break;

                case cls_Common.AUTO_RELEASE_UI:
                    AutoLockUI(true);
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        #endregion

    }
}
