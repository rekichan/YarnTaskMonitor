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
        private bool firstRun = true;
        private bool forceClose;
        private bool showDivisionTask;
        private string curSuffix;
        private string lastDay;
        private string selectedTaskId;
        private string selectedTaskName;
        private int lastInputVal;
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
            cls_Common.timeDivisionTask = new List<string>();
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
                return;
            }

            auto = false;

            if (conn != null)
                conn.Close();

            ico_Main.Visible = false;
        }
        #endregion

        #region Event
        private void tstb_AssignDivision_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar) || e.KeyChar == (char)8))
                e.Handled = true;
        }

        private void tsb_AssignDivision_Click(object sender, EventArgs e)
        {
            //先判断是否符合查询条件
            if (!cls_Common.timeDivisionTask.Contains(selectedTaskName) || string.IsNullOrWhiteSpace(selectedTaskName) || string.IsNullOrWhiteSpace(selectedTaskId))
                return;

            //判断输入的数值是否存在问题
            string assign = tstb_AssignDivision.Text;
            if (string.IsNullOrWhiteSpace(assign))
                return;
            int val = Convert.ToInt32(assign);
            if (val < 0 || val > 23 || lastInputVal == val)
                return;
            lastInputVal = val;

            tsb_AssignDivision.Enabled = false;

            //DateTime dt = DateTime.Now.Date;
            DateTime dt = mc_Main.SelectionStart;
            dt = dt.AddHours(val);

            //求出选择的日期
            DateTime day = mc_Main.SelectionEnd;
            string selectedDay = day.ToShortDateString().Replace("/", "");

            //转义防止报错
            selectedTaskName = selectedTaskName.Replace("'", "\\'");

            //规定查询时间范围(1hour)
            string prev = dt.ToString();
            string next = dt.AddHours(1).ToString();
            string sql = string.Format("select cores,memory,insertTime from {0} where taskId = '{1}' and taskName = '{2}' and insertTime >= '{3}' and insertTime < '{4}' order by insertTime asc", cls_Common.table + selectedDay, selectedTaskId, selectedTaskName, prev, next);
            ExecuteSQLFillUpChart(sql);

            tsb_AssignDivision.Enabled = true;
        }

        private void tsb_ExecuteSQL_Click(object sender, EventArgs e)
        {
            frm_SQLInput input = new frm_SQLInput();
            input.ShowDialog();
        }

        private void cht_Main_CursorPositionChanged(object sender, System.Windows.Forms.DataVisualization.Charting.CursorEventArgs e)
        {
            //用于控制图表同步放大
            System.Windows.Forms.DataVisualization.Charting.Chart cht = sender as System.Windows.Forms.DataVisualization.Charting.Chart;
            double pos;
            double size;
            if (cht.ChartAreas["ca_Memory"].AxisX.ScaleView.Position != double.NaN)
            {
                pos = cht.ChartAreas["ca_Memory"].AxisX.ScaleView.Position;
                size = cht.ChartAreas["ca_Memory"].AxisX.ScaleView.Size;
                cht.ChartAreas["ca_Cores"].AxisX.ScaleView.Position = pos;
                cht.ChartAreas["ca_Cores"].AxisX.ScaleView.Size = size;
            }
            else if (cht.ChartAreas["ca_Cores"].AxisX.ScaleView.Position != double.NaN)
            {
                pos = cht.ChartAreas["ca_Cores"].AxisX.ScaleView.Position;
                size = cht.ChartAreas["ca_Cores"].AxisX.ScaleView.Size;
                cht.ChartAreas["ca_Memory"].AxisX.ScaleView.Position = pos;
                cht.ChartAreas["ca_Memory"].AxisX.ScaleView.Size = size;
            }
        }

        private void cht_Main_MouseClick(object sender, MouseEventArgs e)
        {
            //用于控制图标同步最小化到初始状态
            System.Windows.Forms.DataVisualization.Charting.Chart cht = sender as System.Windows.Forms.DataVisualization.Charting.Chart;
            if (e.Button == MouseButtons.Right)
            {
                cht.ChartAreas["ca_Memory"].AxisX.ScaleView.ZoomReset(0);
                cht.ChartAreas["ca_Cores"].AxisX.ScaleView.ZoomReset(0);
            }
        }

        private void tsb_Truncate_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要清空数据库?", "Q", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
                return;

            DateTime dt = mc_Main.SelectionEnd;
            string selectedDay = dt.ToShortDateString().Replace("/", "");
            string sql = string.Format("truncate table {0}", cls_Common.table + selectedDay);
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                logger.WriteExceptionLog(ex);
                MessageBox.Show("表清空失败", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
            //取出右键选择项
            int row = dgv_Trace.SelectedCells[0].RowIndex;
            string taskId = (string)dgv_Trace.Rows[row].Cells[0].Value;
            string taskName = (string)dgv_Trace.Rows[row].Cells[1].Value;
            if(showDivisionTask)
            {
                //先检查是否在显示分时任务曲线，如果是再检测上次分时任务的id和taskName是否和本次相同，如果相同则不刷新曲线
                if (selectedTaskId.Equals(taskId) && selectedTaskName.Equals(taskName))
                    return;
            }
            selectedTaskId = "";
            selectedTaskName = "";
            lastInputVal = 0;

            DateTime dt = mc_Main.SelectionStart;
            string selectedDay = dt.ToShortDateString().Replace("/", "");

            //重绘时还原chart倍率，显示chart游标
            cht_Main.ChartAreas["ca_Memory"].AxisX.ScaleView.ZoomReset(0);
            cht_Main.ChartAreas["ca_Cores"].AxisX.ScaleView.ZoomReset(0);
            cht_Main.ChartAreas["ca_Memory"].CursorX.IsUserEnabled = true;
            cht_Main.ChartAreas["ca_Memory"].CursorX.IsUserSelectionEnabled = true;

            if (cls_Common.timeDivisionTask.Contains(taskName))
            {
                //如果是分时列表内的任务，则赋值taskId等内容，并显示scrollbar
                //分时任务选择时先显示0-1点的数据
                selectedTaskId = taskId;
                selectedTaskName = taskName;
                showDivisionTask = true;
                tstb_AssignDivision.Enabled = true;
                tsb_AssignDivision.Enabled = true;
                lastInputVal = 0;
                string divisionTaskSql = string.Format("select cores,memory,insertTime from {0} where taskId = '{1}' and taskName = '{2}' and insertTime >= '{3}' and insertTime < '{4}' order by insertTime asc", cls_Common.table + selectedDay, selectedTaskId, selectedTaskName, dt.ToString(), dt.AddHours(1).ToString());
                ExecuteSQLFillUpChart(divisionTaskSql);
                return;
            }

            //如果不是分时列表内的任务，则隐藏scrollbar
            showDivisionTask = false;
            tstb_AssignDivision.Enabled = false;
            tsb_AssignDivision.Enabled = false;

            //预处理，转义防止错误
            taskName = taskName.Replace("'", "\\'");
            string CommonSql = "select cores,memory,insertTime from " + cls_Common.table + selectedDay + " where taskId = '" + taskId + "' and taskName  = '" + taskName + "' order by insertTime asc";
            ExecuteSQLFillUpChart(CommonSql);
        }

        private void ico_Main_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!this.Visible)
                this.Visible = true;
        }

        private void tsb_GetTask_Click(object sender, EventArgs e)
        {
            DateTime dt = mc_Main.SelectionEnd;
            string selectedDay = dt.ToShortDateString().Replace("/", "");

            try
            {
                using (MySqlConnection queryConn = new MySqlConnection(cls_Common.connetionCmd))
                {
                    queryConn.Open();
                    string sql = "select " +
                        "taskId," +//0
                        "taskName," +//1
                        "avg(cores)," +//2
                        "avg(memory) as `avg(mem)`," +//3
                        "min(startTime) as startTime," +//4
                        "date_format(CONVERT_TZ(from_unixtime(UNIX_TIMESTAMP(max(insertTime))- UNIX_TIMESTAMP(min(startTime))),'+08:00','+00:00'),'%m/%d %H:%i:%s') as duration " +//5
                        "from " + cls_Common.table + selectedDay +
                        " group by taskId,taskName order by startTime desc";
                    using (MySqlDataAdapter mda = new MySqlDataAdapter(sql, queryConn))
                    {
                        DataTable data = new DataTable();
                        mda.Fill(data);
                        dgv_Trace.DataSource = data;
                        dgv_Trace.Columns[0].Width = 218;
                        dgv_Trace.Columns[1].Width = 186;
                        dgv_Trace.Columns[2].Width = 70;
                        dgv_Trace.Columns[3].Width = 70;
                        dgv_Trace.Columns[4].Width = 130;
                        dgv_Trace.Columns[dgv_Trace.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                    queryConn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.WriteExceptionLog(ex);
                MessageBox.Show("查询失败", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
                if (interval < 1000)
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
            //手动插入的话只插入到当日的数据表，不进行自动创建
            string suffix = DateTime.Now.ToShortDateString().Replace("/", "");
            ParseHtmlAndSaveMySQL(CatchHtml(), cls_Common.table + suffix);
        }
        #endregion

        #region Function
        /// <summary>
        /// 执行SQL填充Chart
        /// </summary>
        /// <param name="sql">SQL语句</param>
        private void ExecuteSQLFillUpChart(string sql)
        {
            try
            {
                //实例化链表存储数据
                List<int> cores = new List<int>();
                List<int> memory = new List<int>();
                List<DateTime> time = new List<DateTime>();

                using (MySqlConnection queryConn = new MySqlConnection(cls_Common.connetionCmd))
                {
                    queryConn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, queryConn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cores.Add((int)reader[0]);
                                memory.Add((int)reader[1]);
                                string buf = reader[2].ToString();
                                time.Add(DateTime.Parse(buf));
                            }
                            reader.Close();
                        }
                    }
                    queryConn.Close();
                }

                //清空表格数据
                cht_Main.Series["Memory"].Points.Clear();
                cht_Main.Series["vCores"].Points.Clear();

                //填充图表
                cht_Main.Series["Memory"].Points.DataBindXY(time, memory);
                cht_Main.Series["vCores"].Points.DataBindXY(time, cores);
            }
            catch (Exception ex)
            {
                logger.WriteExceptionLog(ex);
                MessageBox.Show("执行SQL失败", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 删除数据表
        /// </summary>
        /// <param name="tableName">数据表</param>
        /// <returns></returns>
        private bool DropMySQLTable(string tableName)
        {
            try
            {
                string sql = string.Format("drop table if exists {0}", tableName);
                using (MySqlCommand mda = new MySqlCommand(sql, conn))
                {
                    mda.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                logger.WriteExceptionLog(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <param name="tableName">数据表</param>
        /// <returns></returns>
        private bool CreateMySQLTable(string tableName)
        {
            try
            {
                string sql = string.Format("create table if not exists {0} (" +
                    "taskid varchar(50) ," +
                    "taskname varchar(100) ," +
                    "tasktype varchar(20) ," +
                    "queue varchar(30) ," +
                    "starttime timestamp ," +
                    "containers int ," +
                    "cores int ," +
                    "memory int ," +
                    "inserttime timestamp) " +
                    "engine=innodb default charset=utf8mb3", tableName);
                using (MySqlCommand mda = new MySqlCommand(sql, conn))
                {
                    mda.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                logger.WriteExceptionLog(ex);
                return false;
            }
            return true;
        }

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
            cls_Common.yarnWebUrl = config.IniReadValue("setting", "yarnWebUrl", "");
            string timeDivisionTasks = config.IniReadValue("setting", "divisionTasks", "");
            if (!string.IsNullOrWhiteSpace(timeDivisionTasks))
            {
                string[] divisionTasks = timeDivisionTasks.Split(',');
                for (int i = 0; i < divisionTasks.Length; i++)
                {
                    cls_Common.timeDivisionTask.Add(divisionTasks[i]);
                }
            }
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
            //"http://slave33.bl.bigdata:8088/cluster/apps/RUNNING"
            string url = cls_Common.yarnWebUrl;//指定yarn application url
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
                //GC.Collect(); //让系统自动GC
            }
            catch (Exception ex)
            {
                MessageBox.Show("爬取Web数据失败", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        private bool ParseHtmlAndSaveMySQL(string html, string tableName)
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
            double curMem = Convert.ToDouble(Regex.Match(curMemory, @"[0-9 .]+").Value);
            if (curMemory.Contains("TB"))
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

                    //>>>>
                    //20211230 taskName引发的exception：因为分割","导致索引发生变化解析时间戳报错
                    //"CREATE TABLE recommendat...date,category_sid(Stage-1)"

                    //20211231 taskName引发的exception：因为字段中存在"'"导致sql错误
                    //"SELECT order_no,req_Ext FROM sd...00:00:00')(Stage-1)"
                    //<<<<
                    string sub = htmls[i].Replace("\\", "");
                    string[] cols = Regex.Split(sub, "\",\"");
                    string buf = Regex.Match(cols[0], @">[a-z _ 0-9]+<").Value;//正则匹配taskId
                    string taskId = buf.Substring(1, buf.Length - 2);
                    string taskName = cols[2].Replace("'", "\\'");//防止插入字段中包含'
                    string taskType = cols[3];
                    string queue = cols[4];
                    string startTime = Ts2Date(cols[5]);
                    string containers = cols[9];
                    string cores = cols[10];
                    string memory = cols[11];
                    int mem = Convert.ToInt32(memory);
                    memory = Math.Round(mem / 1024.0, 4).ToString();
                    string sql = string.Format("insert into {9} values ('{0}','{1}','{2}','{3}','{4}',{5},{6},{7},'{8}')", taskId, taskName, taskType, queue, startTime, containers, cores, memory, DateTime.Now.ToString(), tableName);
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
                //写入一次Multi信息就行了,a1是为了保证第一位
                string multiSql = string.Format("insert into {9} values ('{0}','{1}','{2}','{3}','{4}',{5},{6},{7},'{8}')", "a1", "Multi", "Multi", "Multi", DateTime.Now.ToString(), curContainers, curCores, curMemory, DateTime.Now.ToString(), tableName);
                cmd.CommandText = multiSql;
                cmd.ExecuteNonQuery();
                tx.Commit();//使用事务主要是为了insert batch提高效率
                tx.Dispose();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                logger.WriteExceptionLog(ex);
                MessageBox.Show("插入数据表失败", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            //tsb_ConnectDatabase.Enabled = enable;
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
                if (firstRun)
                {
                    firstRun = false;
                    //如果是程序首次运行，无论如何都先创建一次数据表，利用create ... if not exists ... 避免exception
                    lastDay = DateTime.Now.ToShortDateString().Replace("/", "");
                    curSuffix = lastDay;
                    if (!CreateMySQLTable(cls_Common.table + curSuffix))
                        break;

                    Delay(interval);
                }

                if (lastDay != DateTime.Now.ToShortDateString().Replace("/", ""))
                {
                    //如果日期发生变化，删除旧的数据表，创建新的数据表
                    lastDay = DateTime.Now.ToShortDateString().Replace("/", "");
                    string last7Day = DateTime.Now.AddDays(-7).ToShortDateString().Replace("/", "");
                    curSuffix = lastDay;
                    //先删除7日之前的表
                    if (!DropMySQLTable(cls_Common.table + last7Day))
                        break;

                    //再创建今日的数据表，利用create ... if not exists ... 避免exception
                    if (!CreateMySQLTable(cls_Common.table + lastDay))
                        break;

                    //当日期发生变化时切换calendar控件的日期
                    PostMess(cls_Common.hwndFrmMain, cls_Common.CHANGE_DATE_SELECTED);
                }

                Delay(interval);

                if (!ParseHtmlAndSaveMySQL(CatchHtml(), cls_Common.table + curSuffix))
                    break;
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
                    this.Visible = true;
                    break;

                case cls_Common.MANUAL_EXECUTE_SQL:
                    string sql = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(m.WParam);
                    ExecuteSQLFillUpChart(sql);
                    //使用手动查询SQL时，关闭scrollbar
                    selectedTaskId = "";
                    selectedTaskName = "";
                    lastInputVal = 0;
                    showDivisionTask = false;
                    tsb_AssignDivision.Enabled = false;
                    tstb_AssignDivision.Enabled = false;
                    System.Runtime.InteropServices.Marshal.FreeHGlobal(m.WParam);
                    break;

                case cls_Common.CHANGE_DATE_SELECTED:
                    mc_Main.SelectionStart = DateTime.Now;
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        #endregion

    }
}
