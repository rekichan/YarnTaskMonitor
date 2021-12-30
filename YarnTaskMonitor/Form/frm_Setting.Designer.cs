
namespace YarnTaskMonitor
{
    partial class frm_Setting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_Setting));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txb_ConnectionCmd = new System.Windows.Forms.TextBox();
            this.btn_Save = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.txb_Table = new System.Windows.Forms.TextBox();
            this.txb_AutoInterval = new System.Windows.Forms.TextBox();
            this.chk_TaskBar = new System.Windows.Forms.CheckBox();
            this.txb_YarnWebUrl = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "自动采集间隔(ms)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "MySQL连接";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(135, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "数据表";
            // 
            // txb_ConnectionCmd
            // 
            this.txb_ConnectionCmd.Location = new System.Drawing.Point(12, 116);
            this.txb_ConnectionCmd.Multiline = true;
            this.txb_ConnectionCmd.Name = "txb_ConnectionCmd";
            this.txb_ConnectionCmd.Size = new System.Drawing.Size(360, 55);
            this.txb_ConnectionCmd.TabIndex = 3;
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(297, 12);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(75, 35);
            this.btn_Save.TabIndex = 4;
            this.btn_Save.Text = "保存";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(297, 59);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 35);
            this.btn_Cancel.TabIndex = 5;
            this.btn_Cancel.Text = "取消";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // txb_Table
            // 
            this.txb_Table.Location = new System.Drawing.Point(135, 32);
            this.txb_Table.Name = "txb_Table";
            this.txb_Table.Size = new System.Drawing.Size(145, 23);
            this.txb_Table.TabIndex = 6;
            // 
            // txb_AutoInterval
            // 
            this.txb_AutoInterval.Location = new System.Drawing.Point(12, 32);
            this.txb_AutoInterval.Name = "txb_AutoInterval";
            this.txb_AutoInterval.Size = new System.Drawing.Size(100, 23);
            this.txb_AutoInterval.TabIndex = 7;
            // 
            // chk_TaskBar
            // 
            this.chk_TaskBar.AutoSize = true;
            this.chk_TaskBar.Location = new System.Drawing.Point(135, 67);
            this.chk_TaskBar.Name = "chk_TaskBar";
            this.chk_TaskBar.Size = new System.Drawing.Size(123, 21);
            this.chk_TaskBar.TabIndex = 8;
            this.chk_TaskBar.Text = "最小化到系统托盘";
            this.chk_TaskBar.UseVisualStyleBackColor = true;
            // 
            // txb_YarnWebUrl
            // 
            this.txb_YarnWebUrl.Location = new System.Drawing.Point(12, 194);
            this.txb_YarnWebUrl.Multiline = true;
            this.txb_YarnWebUrl.Name = "txb_YarnWebUrl";
            this.txb_YarnWebUrl.Size = new System.Drawing.Size(360, 55);
            this.txb_YarnWebUrl.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 174);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "Yarn Running Web Url";
            // 
            // frm_Setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 261);
            this.Controls.Add(this.txb_YarnWebUrl);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chk_TaskBar);
            this.Controls.Add(this.txb_AutoInterval);
            this.Controls.Add(this.txb_Table);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.txb_ConnectionCmd);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(400, 300);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "frm_Setting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Setting";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frm_Setting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txb_ConnectionCmd;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.TextBox txb_Table;
        private System.Windows.Forms.TextBox txb_AutoInterval;
        private System.Windows.Forms.CheckBox chk_TaskBar;
        private System.Windows.Forms.TextBox txb_YarnWebUrl;
        private System.Windows.Forms.Label label4;
    }
}