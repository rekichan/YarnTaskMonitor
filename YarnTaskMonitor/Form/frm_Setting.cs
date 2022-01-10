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
            lsb_TimeDivisionTask.Items.AddRange(cls_Common.timeDivisionTask.ToArray());
        }
        #endregion

        #region Event
        private void lsb_TimeDivisionTask_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = lsb_TimeDivisionTask.IndexFromPoint(e.Location);
                if (index >= 0)
                    lsb_TimeDivisionTask.SelectedIndex = index;
            }
        }

        private void btn_AddTimeDivisionTask_Click(object sender, EventArgs e)
        {
            string taskName = txb_TimeDivisionTask.Text;
            if (string.IsNullOrWhiteSpace(taskName))
                return;

            if (lsb_TimeDivisionTask.Items.Contains(taskName))
                return;

            lsb_TimeDivisionTask.Items.Add(taskName);
        }

        private void tsmi_Delete_Click(object sender, EventArgs e)
        {
            if (lsb_TimeDivisionTask.SelectedIndex < 0)
                return;

            DialogResult dr = MessageBox.Show("确定要删除本项?", "Q", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
                return;

            string item = lsb_TimeDivisionTask.SelectedItem.ToString();
            lsb_TimeDivisionTask.Items.Remove(item);
        }

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
            cls_Common.timeDivisionTask.Clear();

            string divisionTasks = "";
            for (int i = 0; i < lsb_TimeDivisionTask.Items.Count; i++)
            {
                string divisionTask = lsb_TimeDivisionTask.Items[i].ToString();
                cls_Common.timeDivisionTask.Add(divisionTask);
                divisionTasks += divisionTask;
                if (i != lsb_TimeDivisionTask.Items.Count - 1)
                    divisionTasks += ",";
            }

            config.IniWriteValue("setting", "connetionCmd", cls_Common.connetionCmd);
            config.IniWriteValue("setting", "table", cls_Common.table);
            config.IniWriteValue("setting", "autoInterval", cls_Common.autoInterval);
            config.IniWriteValue("setting", "taskBar", cls_Common.taskBar);
            config.IniWriteValue("setting", "yarnWebUrl", cls_Common.yarnWebUrl);
            config.IniWriteValue("setting", "divisionTasks", divisionTasks);
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
