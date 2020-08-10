namespace TEAM_07
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.CB_SO = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TB_NOP = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TB_TQ = new System.Windows.Forms.TextBox();
            this.BT_APPLY = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.CB_SN = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BT_ASSIGN = new System.Windows.Forms.Button();
            this.TB_BT = new System.Windows.Forms.TextBox();
            this.TB_AT = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.BT_START = new System.Windows.Forms.Button();
            this.BT_STEP = new System.Windows.Forms.Button();
            this.BT_JUMP = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.LV_RESULT = new System.Windows.Forms.ListView();
            this.NUMBER = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AT = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BT = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.WT = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TT = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NT = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BT_RESET = new System.Windows.Forms.Button();
            this.CT_GANTT = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.LV_EXP = new System.Windows.Forms.ListView();
            this.EXP_NUMBER = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LV_ENP = new System.Windows.Forms.ListView();
            this.ENP_NUMBER = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LB_CT = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.CB_QUIZ = new System.Windows.Forms.ComboBox();
            this.BT_QUIZ = new System.Windows.Forms.Button();
            this.RB_HARD = new System.Windows.Forms.RadioButton();
            this.RB_EASY = new System.Windows.Forms.RadioButton();
            this.RB_NONE = new System.Windows.Forms.RadioButton();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CT_GANTT)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // CB_SO
            // 
            this.CB_SO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_SO.FormattingEnabled = true;
            this.CB_SO.Items.AddRange(new object[] {
            "FCFS",
            "RR",
            "SPN",
            "SRTN",
            "HRRN"});
            this.CB_SO.Location = new System.Drawing.Point(168, 12);
            this.CB_SO.Name = "CB_SO";
            this.CB_SO.Size = new System.Drawing.Size(83, 20);
            this.CB_SO.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Scheduling Option";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Number of Process(es)";
            // 
            // TB_NOP
            // 
            this.TB_NOP.Location = new System.Drawing.Point(168, 41);
            this.TB_NOP.Name = "TB_NOP";
            this.TB_NOP.Size = new System.Drawing.Size(83, 21);
            this.TB_NOP.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Time Quantum";
            // 
            // TB_TQ
            // 
            this.TB_TQ.Enabled = false;
            this.TB_TQ.Location = new System.Drawing.Point(168, 121);
            this.TB_TQ.Name = "TB_TQ";
            this.TB_TQ.Size = new System.Drawing.Size(83, 21);
            this.TB_TQ.TabIndex = 3;
            // 
            // BT_APPLY
            // 
            this.BT_APPLY.Location = new System.Drawing.Point(66, 82);
            this.BT_APPLY.Name = "BT_APPLY";
            this.BT_APPLY.Size = new System.Drawing.Size(122, 23);
            this.BT_APPLY.TabIndex = 2;
            this.BT_APPLY.Text = "APPLY OPTION";
            this.BT_APPLY.UseVisualStyleBackColor = true;
            this.BT_APPLY.Click += new System.EventHandler(this.BT_APPLY_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "Select Number";
            // 
            // CB_SN
            // 
            this.CB_SN.Enabled = false;
            this.CB_SN.FormattingEnabled = true;
            this.CB_SN.Location = new System.Drawing.Point(132, 29);
            this.CB_SN.Name = "CB_SN";
            this.CB_SN.Size = new System.Drawing.Size(83, 20);
            this.CB_SN.TabIndex = 0;
            this.CB_SN.SelectedIndexChanged += new System.EventHandler(this.CB_SN_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BT_ASSIGN);
            this.groupBox1.Controls.Add(this.TB_BT);
            this.groupBox1.Controls.Add(this.TB_AT);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.CB_SN);
            this.groupBox1.Location = new System.Drawing.Point(14, 153);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(237, 174);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Process Setting";
            // 
            // BT_ASSIGN
            // 
            this.BT_ASSIGN.Enabled = false;
            this.BT_ASSIGN.Location = new System.Drawing.Point(79, 137);
            this.BT_ASSIGN.Name = "BT_ASSIGN";
            this.BT_ASSIGN.Size = new System.Drawing.Size(75, 23);
            this.BT_ASSIGN.TabIndex = 3;
            this.BT_ASSIGN.Text = "ASSIGN";
            this.BT_ASSIGN.UseVisualStyleBackColor = true;
            this.BT_ASSIGN.Click += new System.EventHandler(this.BT_ASSIGN_Click);
            // 
            // TB_BT
            // 
            this.TB_BT.Enabled = false;
            this.TB_BT.Location = new System.Drawing.Point(132, 103);
            this.TB_BT.Name = "TB_BT";
            this.TB_BT.Size = new System.Drawing.Size(83, 21);
            this.TB_BT.TabIndex = 2;
            // 
            // TB_AT
            // 
            this.TB_AT.Enabled = false;
            this.TB_AT.Location = new System.Drawing.Point(132, 73);
            this.TB_AT.Name = "TB_AT";
            this.TB_AT.Size = new System.Drawing.Size(83, 21);
            this.TB_AT.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 109);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "Burst Time";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "Arrival Time";
            // 
            // BT_START
            // 
            this.BT_START.Enabled = false;
            this.BT_START.Location = new System.Drawing.Point(14, 338);
            this.BT_START.Name = "BT_START";
            this.BT_START.Size = new System.Drawing.Size(105, 48);
            this.BT_START.TabIndex = 5;
            this.BT_START.Text = "START";
            this.BT_START.UseVisualStyleBackColor = true;
            this.BT_START.Click += new System.EventHandler(this.BT_START_Click);
            // 
            // BT_STEP
            // 
            this.BT_STEP.Enabled = false;
            this.BT_STEP.Location = new System.Drawing.Point(145, 338);
            this.BT_STEP.Name = "BT_STEP";
            this.BT_STEP.Size = new System.Drawing.Size(106, 48);
            this.BT_STEP.TabIndex = 6;
            this.BT_STEP.Text = "STEP";
            this.BT_STEP.UseVisualStyleBackColor = true;
            this.BT_STEP.Click += new System.EventHandler(this.BT_STEP_Click);
            // 
            // BT_JUMP
            // 
            this.BT_JUMP.Enabled = false;
            this.BT_JUMP.Location = new System.Drawing.Point(145, 392);
            this.BT_JUMP.Name = "BT_JUMP";
            this.BT_JUMP.Size = new System.Drawing.Size(106, 48);
            this.BT_JUMP.TabIndex = 7;
            this.BT_JUMP.Text = "JUMP";
            this.BT_JUMP.UseVisualStyleBackColor = true;
            this.BT_JUMP.Click += new System.EventHandler(this.BT_JUMP_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.LV_RESULT);
            this.groupBox2.Location = new System.Drawing.Point(267, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(505, 130);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Result Table";
            // 
            // LV_RESULT
            // 
            this.LV_RESULT.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NUMBER,
            this.AT,
            this.BT,
            this.WT,
            this.TT,
            this.NT});
            this.LV_RESULT.Location = new System.Drawing.Point(6, 20);
            this.LV_RESULT.Name = "LV_RESULT";
            this.LV_RESULT.Size = new System.Drawing.Size(493, 104);
            this.LV_RESULT.TabIndex = 0;
            this.LV_RESULT.UseCompatibleStateImageBehavior = false;
            this.LV_RESULT.View = System.Windows.Forms.View.Details;
            // 
            // NUMBER
            // 
            this.NUMBER.Text = "NUMBER";
            this.NUMBER.Width = 90;
            // 
            // AT
            // 
            this.AT.Text = "AT";
            // 
            // BT
            // 
            this.BT.Text = "BT";
            // 
            // WT
            // 
            this.WT.Text = "WT";
            // 
            // TT
            // 
            this.TT.Text = "TT";
            // 
            // NT
            // 
            this.NT.Text = "NT";
            this.NT.Width = 120;
            // 
            // BT_RESET
            // 
            this.BT_RESET.Location = new System.Drawing.Point(14, 392);
            this.BT_RESET.Name = "BT_RESET";
            this.BT_RESET.Size = new System.Drawing.Size(105, 48);
            this.BT_RESET.TabIndex = 8;
            this.BT_RESET.Text = "RESET";
            this.BT_RESET.UseVisualStyleBackColor = true;
            this.BT_RESET.Click += new System.EventHandler(this.BT_RESET_Click);
            // 
            // CT_GANTT
            // 
            chartArea2.AxisX.Interval = 1D;
            chartArea2.AxisX.LabelStyle.Interval = 0D;
            chartArea2.AxisX.LabelStyle.IntervalOffset = 0D;
            chartArea2.AxisX.LabelStyle.IsEndLabelVisible = false;
            chartArea2.AxisX.MajorGrid.Enabled = false;
            chartArea2.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea2.AxisX.Title = "Process Number";
            chartArea2.AxisY.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea2.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea2.AxisY.Title = "Time";
            chartArea2.Name = "GanttArea";
            this.CT_GANTT.ChartAreas.Add(chartArea2);
            this.CT_GANTT.Location = new System.Drawing.Point(6, 20);
            this.CT_GANTT.Name = "CT_GANTT";
            series2.ChartArea = "GanttArea";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.RangeBar;
            series2.Name = "NOP";
            series2.YValuesPerPoint = 2;
            this.CT_GANTT.Series.Add(series2);
            this.CT_GANTT.Size = new System.Drawing.Size(493, 267);
            this.CT_GANTT.TabIndex = 16;
            this.CT_GANTT.Text = "chart1";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.LV_EXP);
            this.groupBox3.Controls.Add(this.LV_ENP);
            this.groupBox3.Controls.Add(this.LB_CT);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.CT_GANTT);
            this.groupBox3.Location = new System.Drawing.Point(267, 153);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(505, 396);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Gantt Chart";
            // 
            // LV_EXP
            // 
            this.LV_EXP.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.EXP_NUMBER});
            this.LV_EXP.Location = new System.Drawing.Point(333, 326);
            this.LV_EXP.Name = "LV_EXP";
            this.LV_EXP.Size = new System.Drawing.Size(121, 58);
            this.LV_EXP.TabIndex = 21;
            this.LV_EXP.UseCompatibleStateImageBehavior = false;
            this.LV_EXP.View = System.Windows.Forms.View.Details;
            // 
            // EXP_NUMBER
            // 
            this.EXP_NUMBER.Text = "NUMBER";
            this.EXP_NUMBER.Width = 100;
            // 
            // LV_ENP
            // 
            this.LV_ENP.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ENP_NUMBER});
            this.LV_ENP.Location = new System.Drawing.Point(169, 326);
            this.LV_ENP.Name = "LV_ENP";
            this.LV_ENP.Size = new System.Drawing.Size(121, 58);
            this.LV_ENP.TabIndex = 21;
            this.LV_ENP.UseCompatibleStateImageBehavior = false;
            this.LV_ENP.View = System.Windows.Forms.View.Details;
            // 
            // ENP_NUMBER
            // 
            this.ENP_NUMBER.Text = "NUMBER";
            this.ENP_NUMBER.Width = 100;
            // 
            // LB_CT
            // 
            this.LB_CT.AutoSize = true;
            this.LB_CT.Font = new System.Drawing.Font("굴림", 13F);
            this.LB_CT.Location = new System.Drawing.Point(104, 341);
            this.LB_CT.Name = "LB_CT";
            this.LB_CT.Size = new System.Drawing.Size(18, 18);
            this.LB_CT.TabIndex = 20;
            this.LB_CT.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(342, 302);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(94, 12);
            this.label9.TabIndex = 19;
            this.label9.Text = "Exiting Process";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(178, 302);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(102, 12);
            this.label8.TabIndex = 18;
            this.label8.Text = "Entering Process";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(43, 302);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 12);
            this.label7.TabIndex = 17;
            this.label7.Text = "Current Time";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.CB_QUIZ);
            this.groupBox4.Controls.Add(this.BT_QUIZ);
            this.groupBox4.Controls.Add(this.RB_HARD);
            this.groupBox4.Controls.Add(this.RB_EASY);
            this.groupBox4.Controls.Add(this.RB_NONE);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Location = new System.Drawing.Point(12, 449);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(239, 100);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Quiz";
            // 
            // CB_QUIZ
            // 
            this.CB_QUIZ.Enabled = false;
            this.CB_QUIZ.FormattingEnabled = true;
            this.CB_QUIZ.Location = new System.Drawing.Point(29, 65);
            this.CB_QUIZ.Name = "CB_QUIZ";
            this.CB_QUIZ.Size = new System.Drawing.Size(96, 20);
            this.CB_QUIZ.TabIndex = 23;
            // 
            // BT_QUIZ
            // 
            this.BT_QUIZ.Enabled = false;
            this.BT_QUIZ.Location = new System.Drawing.Point(139, 64);
            this.BT_QUIZ.Name = "BT_QUIZ";
            this.BT_QUIZ.Size = new System.Drawing.Size(75, 23);
            this.BT_QUIZ.TabIndex = 1;
            this.BT_QUIZ.Text = "SUBMIT";
            this.BT_QUIZ.UseVisualStyleBackColor = true;
            this.BT_QUIZ.Click += new System.EventHandler(this.BT_QUIZ_Click);
            // 
            // RB_HARD
            // 
            this.RB_HARD.AutoSize = true;
            this.RB_HARD.Enabled = false;
            this.RB_HARD.Location = new System.Drawing.Point(188, 36);
            this.RB_HARD.Name = "RB_HARD";
            this.RB_HARD.Size = new System.Drawing.Size(14, 13);
            this.RB_HARD.TabIndex = 4;
            this.RB_HARD.TabStop = true;
            this.RB_HARD.UseVisualStyleBackColor = true;
            this.RB_HARD.CheckedChanged += new System.EventHandler(this.RB_HARD_CheckedChanged);
            // 
            // RB_EASY
            // 
            this.RB_EASY.AutoSize = true;
            this.RB_EASY.Enabled = false;
            this.RB_EASY.Location = new System.Drawing.Point(110, 36);
            this.RB_EASY.Name = "RB_EASY";
            this.RB_EASY.Size = new System.Drawing.Size(14, 13);
            this.RB_EASY.TabIndex = 3;
            this.RB_EASY.TabStop = true;
            this.RB_EASY.UseVisualStyleBackColor = true;
            this.RB_EASY.CheckedChanged += new System.EventHandler(this.RB_EASY_CheckedChanged);
            // 
            // RB_NONE
            // 
            this.RB_NONE.AutoSize = true;
            this.RB_NONE.Checked = true;
            this.RB_NONE.Enabled = false;
            this.RB_NONE.Location = new System.Drawing.Point(31, 36);
            this.RB_NONE.Name = "RB_NONE";
            this.RB_NONE.Size = new System.Drawing.Size(14, 13);
            this.RB_NONE.TabIndex = 2;
            this.RB_NONE.TabStop = true;
            this.RB_NONE.UseVisualStyleBackColor = true;
            this.RB_NONE.CheckedChanged += new System.EventHandler(this.RB_NONE_CheckedChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(177, 17);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(37, 12);
            this.label12.TabIndex = 22;
            this.label12.Text = "HARD";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(99, 18);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(37, 12);
            this.label11.TabIndex = 21;
            this.label11.Text = "EASY";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(19, 17);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(40, 12);
            this.label10.TabIndex = 20;
            this.label10.Text = "NONE";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.BT_RESET);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.BT_JUMP);
            this.Controls.Add(this.BT_STEP);
            this.Controls.Add(this.BT_START);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BT_APPLY);
            this.Controls.Add(this.TB_TQ);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TB_NOP);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CB_SO);
            this.Controls.Add(this.groupBox3);
            this.Name = "Form1";
            this.Text = "TEAM_07";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CT_GANTT)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CB_SO;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TB_NOP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TB_TQ;
        private System.Windows.Forms.Button BT_APPLY;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox CB_SN;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button BT_ASSIGN;
        private System.Windows.Forms.TextBox TB_BT;
        private System.Windows.Forms.TextBox TB_AT;
        private System.Windows.Forms.Button BT_START;
        private System.Windows.Forms.Button BT_STEP;
        private System.Windows.Forms.Button BT_JUMP;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView LV_RESULT;
        private System.Windows.Forms.Button BT_RESET;
        private System.Windows.Forms.DataVisualization.Charting.Chart CT_GANTT;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label LB_CT;
        private System.Windows.Forms.ListView LV_EXP;
        private System.Windows.Forms.ListView LV_ENP;
        private System.Windows.Forms.RadioButton RB_NONE;
        private System.Windows.Forms.RadioButton RB_HARD;
        private System.Windows.Forms.RadioButton RB_EASY;
        private System.Windows.Forms.Button BT_QUIZ;
        private System.Windows.Forms.ComboBox CB_QUIZ;
        private System.Windows.Forms.ColumnHeader NUMBER;
        private System.Windows.Forms.ColumnHeader AT;
        private System.Windows.Forms.ColumnHeader BT;
        private System.Windows.Forms.ColumnHeader WT;
        private System.Windows.Forms.ColumnHeader TT;
        private System.Windows.Forms.ColumnHeader NT;
        private System.Windows.Forms.ColumnHeader ENP_NUMBER;
        private System.Windows.Forms.ColumnHeader EXP_NUMBER;
    }
}

