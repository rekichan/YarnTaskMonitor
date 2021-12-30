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
    public partial class frm_Setting : Form
    {

        #region Properties
        cls_Config config;
        #endregion

        #region Constructor
        public frm_Setting()
        {
            InitializeComponent();
        }
        #endregion

        #region FormEvent
        private void frm_Setting_Load(object sender, EventArgs e)
        {
            config = cls_Config.getInstance();
            txb_ConnectionCmd.Text = cls_Common.connetionCmd;
            txb_Table.Text = cls_Common.table;
            txb_AutoInterval.Text = cls_Common.autoInterval;
            chk_TaskBar.Checked = cls_Common.taskBar.Equals("true");
            txb_YarnWebUrl.Text = cls_Common.yarnWebUrl;
        }
        #endregion

        #region Event
        private void btn_Save_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要保存参数?", "Q", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
                return;

            cls_Common.connetionCmd = txb_ConnectionCmd.Text;
            cls_Common.table = txb_Table.Text;
            cls_Common.autoInterval = txb_AutoInterval.Text;
            cls_Common.taskBar = chk_TaskBar.Checked ? "true" : "false";
            cls_Common.yarnWebUrl = txb_YarnWebUrl.Text;

            config.IniWriteValue("setting", "connetionCmd", cls_Common.connetionCmd);
            config.IniWriteValue("setting", "table", cls_Common.table);
            config.IniWriteValue("setting", "autoInterval", cls_Common.autoInterval);
            config.IniWriteValue("setting", "taskBar", cls_Common.taskBar);
            config.IniWriteValue("setting", "yarnWebUrl", cls_Common.yarnWebUrl);
            MessageBox.Show("保存成功", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

    }
}
