using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YarnTaskMonitor
{
    public partial class frm_SQLInput : Form
    {

        #region API
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void PostMessage(IntPtr hWnd, int msg, IntPtr wParam, int lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        #endregion

        #region Constructor
        public frm_SQLInput()
        {
            InitializeComponent();
        }
        #endregion

        #region FormEvent
        private void frm_SQLInput_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cls_Common.lastExecuteSQL))
                txb_Input.Text = cls_Common.lastExecuteSQL;
        }
        #endregion

        #region Event
        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Execute_Click(object sender, EventArgs e)
        {
            //select cores,memory,insertTime from {0} where taskId = '{1}' and taskName = '{2}' and insertTime >= '{3}' and insertTime < '{4}' order by insertTime asc
            string[] sqls = txb_Input.Lines;
            string save = txb_Input.Text;
            string sql = "";
            for(int i =0;i< sqls.Length; i++)
            {
                string line = sqls[i].Trim();
                sql += line + " ";
            }
            cls_Common.lastExecuteSQL = save;//记录之前的SQL
            sql = sql.Replace(System.Environment.NewLine, "");

            //稍微过滤一下，不让全表扫描
            if (sql.Contains("*"))
                return;
            if (!sql.Contains("where"))
                return;

            //传送字符串到主界面执行
            PostMess(cls_Common.hwndFrmMain, cls_Common.MANUAL_EXECUTE_SQL, sql);

            this.Close();
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
        #endregion

    }
}
