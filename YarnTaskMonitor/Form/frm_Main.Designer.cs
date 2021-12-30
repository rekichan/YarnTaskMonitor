
namespace YarnTaskMonitor
{
    partial class frm_Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_Main));
            this.cht_Main = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ts_Main = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ico_Main = new System.Windows.Forms.NotifyIcon(this.components);
            this.cms_Ico = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmi_ShowMain = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.dgv_Trace = new System.Windows.Forms.DataGridView();
            this.cms_Dgv = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmi_CopyText = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_ObserveResource = new System.Windows.Forms.ToolStripMenuItem();
            this.mc_Main = new System.Windows.Forms.MonthCalendar();
            this.hsb_DivisionTasks = new System.Windows.Forms.HScrollBar();
            this.tsb_Collect = new System.Windows.Forms.ToolStripButton();
            this.tsb_AutoCollect = new System.Windows.Forms.ToolStripButton();
            this.tsb_ConnectDatabase = new System.Windows.Forms.ToolStripButton();
            this.tsb_Truncate = new System.Windows.Forms.ToolStripButton();
            this.tsb_GetTask = new System.Windows.Forms.ToolStripButton();
            this.tsb_ExecuteSQL = new System.Windows.Forms.ToolStripButton();
            this.tsb_Setting = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.cht_Main)).BeginInit();
            this.ts_Main.SuspendLayout();
            this.cms_Ico.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Trace)).BeginInit();
            this.cms_Dgv.SuspendLayout();
            this.SuspendLayout();
            // 
            // cht_Main
            // 
            this.cht_Main.AccessibleRole = System.Windows.Forms.AccessibleRole.Cursor;
            chartArea1.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.AxisY.Title = "Memory(GB)";
            chartArea1.CursorX.Interval = 0D;
            chartArea1.CursorX.IsUserEnabled = true;
            chartArea1.CursorX.IsUserSelectionEnabled = true;
            chartArea1.Name = "ca_Memory";
            chartArea2.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea2.AxisY.Title = "vCores";
            chartArea2.CursorX.Interval = 0D;
            chartArea2.CursorX.IsUserEnabled = true;
            chartArea2.CursorX.IsUserSelectionEnabled = true;
            chartArea2.Name = "ca_Cores";
            this.cht_Main.ChartAreas.Add(chartArea1);
            this.cht_Main.ChartAreas.Add(chartArea2);
            legend1.Name = "leg_Resource";
            legend1.Title = "Resource";
            this.cht_Main.Legends.Add(legend1);
            this.cht_Main.Location = new System.Drawing.Point(0, 28);
            this.cht_Main.Name = "cht_Main";
            series1.ChartArea = "ca_Cores";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Legend = "leg_Resource";
            series1.Name = "vCores";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
            series2.ChartArea = "ca_Memory";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Legend = "leg_Resource";
            series2.Name = "Memory";
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
            series2.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.cht_Main.Series.Add(series1);
            this.cht_Main.Series.Add(series2);
            this.cht_Main.Size = new System.Drawing.Size(1008, 538);
            this.cht_Main.TabIndex = 0;
            this.cht_Main.Text = "图标";
            title1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            title1.Name = "tlt_Main";
            title1.Text = "Yarn Resource Trace";
            this.cht_Main.Titles.Add(title1);
            this.cht_Main.CursorPositionChanged += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.CursorEventArgs>(this.cht_Main_CursorPositionChanged);
            this.cht_Main.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cht_Main_MouseClick);
            // 
            // ts_Main
            // 
            this.ts_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_Collect,
            this.tsb_AutoCollect,
            this.toolStripSeparator1,
            this.tsb_ConnectDatabase,
            this.tsb_Truncate,
            this.tsb_GetTask,
            this.tsb_ExecuteSQL,
            this.toolStripSeparator2,
            this.tsb_Setting});
            this.ts_Main.Location = new System.Drawing.Point(0, 0);
            this.ts_Main.Name = "ts_Main";
            this.ts_Main.Size = new System.Drawing.Size(1008, 25);
            this.ts_Main.TabIndex = 1;
            this.ts_Main.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // ico_Main
            // 
            this.ico_Main.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ico_Main.ContextMenuStrip = this.cms_Ico;
            this.ico_Main.Icon = ((System.Drawing.Icon)(resources.GetObject("ico_Main.Icon")));
            this.ico_Main.Text = "YarnTaskMonitor";
            this.ico_Main.Visible = true;
            this.ico_Main.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ico_Main_MouseDoubleClick);
            // 
            // cms_Ico
            // 
            this.cms_Ico.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_ShowMain,
            this.tsmi_Exit});
            this.cms_Ico.Name = "cms_Ico";
            this.cms_Ico.Size = new System.Drawing.Size(137, 48);
            // 
            // tsmi_ShowMain
            // 
            this.tsmi_ShowMain.Name = "tsmi_ShowMain";
            this.tsmi_ShowMain.Size = new System.Drawing.Size(136, 22);
            this.tsmi_ShowMain.Text = "显示主界面";
            this.tsmi_ShowMain.Click += new System.EventHandler(this.tsmi_ShowMain_Click);
            // 
            // tsmi_Exit
            // 
            this.tsmi_Exit.Name = "tsmi_Exit";
            this.tsmi_Exit.Size = new System.Drawing.Size(136, 22);
            this.tsmi_Exit.Text = "退出程序";
            this.tsmi_Exit.Click += new System.EventHandler(this.tsmi_Exit_Click);
            // 
            // dgv_Trace
            // 
            this.dgv_Trace.AllowUserToAddRows = false;
            this.dgv_Trace.AllowUserToDeleteRows = false;
            this.dgv_Trace.AllowUserToResizeColumns = false;
            this.dgv_Trace.AllowUserToResizeRows = false;
            this.dgv_Trace.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Trace.ContextMenuStrip = this.cms_Dgv;
            this.dgv_Trace.Location = new System.Drawing.Point(0, 570);
            this.dgv_Trace.MultiSelect = false;
            this.dgv_Trace.Name = "dgv_Trace";
            this.dgv_Trace.ReadOnly = true;
            this.dgv_Trace.RowHeadersVisible = false;
            this.dgv_Trace.RowTemplate.Height = 23;
            this.dgv_Trace.Size = new System.Drawing.Size(790, 159);
            this.dgv_Trace.TabIndex = 2;
            this.dgv_Trace.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_Trace_CellMouseDown);
            // 
            // cms_Dgv
            // 
            this.cms_Dgv.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_CopyText,
            this.tsmi_ObserveResource});
            this.cms_Dgv.Name = "cms_Dgv";
            this.cms_Dgv.Size = new System.Drawing.Size(125, 48);
            // 
            // tsmi_CopyText
            // 
            this.tsmi_CopyText.Name = "tsmi_CopyText";
            this.tsmi_CopyText.Size = new System.Drawing.Size(124, 22);
            this.tsmi_CopyText.Text = "复制本项";
            this.tsmi_CopyText.Click += new System.EventHandler(this.tsmi_CopyText_Click);
            // 
            // tsmi_ObserveResource
            // 
            this.tsmi_ObserveResource.Name = "tsmi_ObserveResource";
            this.tsmi_ObserveResource.Size = new System.Drawing.Size(124, 22);
            this.tsmi_ObserveResource.Text = "查看资源";
            this.tsmi_ObserveResource.Click += new System.EventHandler(this.tsmi_ObserveResource_Click);
            // 
            // mc_Main
            // 
            this.mc_Main.Location = new System.Drawing.Point(789, 559);
            this.mc_Main.Name = "mc_Main";
            this.mc_Main.ShowToday = false;
            this.mc_Main.TabIndex = 3;
            // 
            // hsb_DivisionTasks
            // 
            this.hsb_DivisionTasks.Location = new System.Drawing.Point(105, 546);
            this.hsb_DivisionTasks.Maximum = 230;
            this.hsb_DivisionTasks.MaximumSize = new System.Drawing.Size(753, 17);
            this.hsb_DivisionTasks.MinimumSize = new System.Drawing.Size(753, 17);
            this.hsb_DivisionTasks.Name = "hsb_DivisionTasks";
            this.hsb_DivisionTasks.Size = new System.Drawing.Size(753, 17);
            this.hsb_DivisionTasks.SmallChange = 10;
            this.hsb_DivisionTasks.TabIndex = 6;
            this.hsb_DivisionTasks.Visible = false;
            this.hsb_DivisionTasks.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsb_DivisionTasks_Scroll);
            // 
            // tsb_Collect
            // 
            this.tsb_Collect.Image = global::YarnTaskMonitor.Properties.Resources.icons8_扳手_30;
            this.tsb_Collect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Collect.Name = "tsb_Collect";
            this.tsb_Collect.Size = new System.Drawing.Size(76, 22);
            this.tsb_Collect.Text = "手动采集";
            this.tsb_Collect.Click += new System.EventHandler(this.tsb_Collect_Click);
            // 
            // tsb_AutoCollect
            // 
            this.tsb_AutoCollect.Image = global::YarnTaskMonitor.Properties.Resources.icons8_电动_30;
            this.tsb_AutoCollect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_AutoCollect.Name = "tsb_AutoCollect";
            this.tsb_AutoCollect.Size = new System.Drawing.Size(76, 22);
            this.tsb_AutoCollect.Text = "自动采集";
            this.tsb_AutoCollect.Click += new System.EventHandler(this.tsb_AutoCollect_Click);
            // 
            // tsb_ConnectDatabase
            // 
            this.tsb_ConnectDatabase.Image = global::YarnTaskMonitor.Properties.Resources.icons8_连接的_30;
            this.tsb_ConnectDatabase.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_ConnectDatabase.Name = "tsb_ConnectDatabase";
            this.tsb_ConnectDatabase.Size = new System.Drawing.Size(88, 22);
            this.tsb_ConnectDatabase.Text = "连接数据库";
            this.tsb_ConnectDatabase.Click += new System.EventHandler(this.tsb_ConnectDatabase_Click);
            // 
            // tsb_Truncate
            // 
            this.tsb_Truncate.Image = global::YarnTaskMonitor.Properties.Resources.icons8_清空回收站_30;
            this.tsb_Truncate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Truncate.Name = "tsb_Truncate";
            this.tsb_Truncate.Size = new System.Drawing.Size(88, 22);
            this.tsb_Truncate.Text = "清空数据表";
            this.tsb_Truncate.Click += new System.EventHandler(this.tsb_Truncate_Click);
            // 
            // tsb_GetTask
            // 
            this.tsb_GetTask.Image = global::YarnTaskMonitor.Properties.Resources.icons8_数据库导出_30;
            this.tsb_GetTask.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_GetTask.Name = "tsb_GetTask";
            this.tsb_GetTask.Size = new System.Drawing.Size(79, 22);
            this.tsb_GetTask.Text = "获取Task";
            this.tsb_GetTask.Click += new System.EventHandler(this.tsb_GetTask_Click);
            // 
            // tsb_ExecuteSQL
            // 
            this.tsb_ExecuteSQL.Image = global::YarnTaskMonitor.Properties.Resources.icons8_search_30;
            this.tsb_ExecuteSQL.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_ExecuteSQL.Name = "tsb_ExecuteSQL";
            this.tsb_ExecuteSQL.Size = new System.Drawing.Size(75, 22);
            this.tsb_ExecuteSQL.Text = "执行SQL";
            this.tsb_ExecuteSQL.Click += new System.EventHandler(this.tsb_ExecuteSQL_Click);
            // 
            // tsb_Setting
            // 
            this.tsb_Setting.Image = global::YarnTaskMonitor.Properties.Resources.icons8_服务_30;
            this.tsb_Setting.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Setting.Name = "tsb_Setting";
            this.tsb_Setting.Size = new System.Drawing.Size(52, 22);
            this.tsb_Setting.Text = "设置";
            this.tsb_Setting.Click += new System.EventHandler(this.tsb_Setting_Click);
            // 
            // frm_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.hsb_DivisionTasks);
            this.Controls.Add(this.dgv_Trace);
            this.Controls.Add(this.mc_Main);
            this.Controls.Add(this.ts_Main);
            this.Controls.Add(this.cht_Main);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1024, 768);
            this.MinimumSize = new System.Drawing.Size(1024, 768);
            this.Name = "frm_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "YarnTaskMonitor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_Main_FormClosing);
            this.Load += new System.EventHandler(this.frm_Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cht_Main)).EndInit();
            this.ts_Main.ResumeLayout(false);
            this.ts_Main.PerformLayout();
            this.cms_Ico.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Trace)).EndInit();
            this.cms_Dgv.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart cht_Main;
        private System.Windows.Forms.ToolStrip ts_Main;
        private System.Windows.Forms.ToolStripButton tsb_Collect;
        private System.Windows.Forms.ToolStripButton tsb_AutoCollect;
        private System.Windows.Forms.ToolStripButton tsb_Setting;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsb_ConnectDatabase;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsb_GetTask;
        private System.Windows.Forms.NotifyIcon ico_Main;
        private System.Windows.Forms.DataGridView dgv_Trace;
        private System.Windows.Forms.ContextMenuStrip cms_Ico;
        private System.Windows.Forms.ToolStripMenuItem tsmi_ShowMain;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Exit;
        private System.Windows.Forms.ContextMenuStrip cms_Dgv;
        private System.Windows.Forms.ToolStripMenuItem tsmi_CopyText;
        private System.Windows.Forms.ToolStripMenuItem tsmi_ObserveResource;
        private System.Windows.Forms.ToolStripButton tsb_Truncate;
        private System.Windows.Forms.MonthCalendar mc_Main;
        private System.Windows.Forms.HScrollBar hsb_DivisionTasks;
        private System.Windows.Forms.ToolStripButton tsb_ExecuteSQL;
    }
}

