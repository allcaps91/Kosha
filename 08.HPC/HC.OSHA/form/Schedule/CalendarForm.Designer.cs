namespace HC_OSHA
{
    partial class CalendarForm
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
            this.panCalendar = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.formTItle1 = new ComBase.Mvc.UserControls.FormTItle();
            this.panSearch = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dgdf = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.RdoCalendarSearchType_Visit = new System.Windows.Forms.RadioButton();
            this.RdoCalendarSearchType_Unvisit = new System.Windows.Forms.RadioButton();
            this.RdoCalendarSearchType_All = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.CboManager = new System.Windows.Forms.ComboBox();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // panCalendar
            // 
            this.panCalendar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panCalendar.Location = new System.Drawing.Point(0, 84);
            this.panCalendar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panCalendar.Name = "panCalendar";
            this.panCalendar.Size = new System.Drawing.Size(1264, 787);
            this.panCalendar.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1119, 6);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 33);
            this.button1.TabIndex = 0;
            this.button1.Text = "Developer Tool";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // formTItle1
            // 
            this.formTItle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.formTItle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.formTItle1.Location = new System.Drawing.Point(0, 0);
            this.formTItle1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.formTItle1.Name = "formTItle1";
            this.formTItle1.Size = new System.Drawing.Size(1264, 35);
            this.formTItle1.TabIndex = 1;
            this.formTItle1.TitleText = "방문 일정";
            // 
            // panSearch
            // 
            this.panSearch.BackColor = System.Drawing.Color.White;
            this.panSearch.Controls.Add(this.button2);
            this.panSearch.Controls.Add(this.label5);
            this.panSearch.Controls.Add(this.label6);
            this.panSearch.Controls.Add(this.label4);
            this.panSearch.Controls.Add(this.dgdf);
            this.panSearch.Controls.Add(this.label3);
            this.panSearch.Controls.Add(this.label2);
            this.panSearch.Controls.Add(this.RdoCalendarSearchType_Visit);
            this.panSearch.Controls.Add(this.RdoCalendarSearchType_Unvisit);
            this.panSearch.Controls.Add(this.RdoCalendarSearchType_All);
            this.panSearch.Controls.Add(this.label1);
            this.panSearch.Controls.Add(this.CboManager);
            this.panSearch.Controls.Add(this.BtnSearch);
            this.panSearch.Controls.Add(this.button1);
            this.panSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSearch.Location = new System.Drawing.Point(0, 35);
            this.panSearch.Name = "panSearch";
            this.panSearch.Size = new System.Drawing.Size(1264, 49);
            this.panSearch.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(630, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 17);
            this.label5.TabIndex = 13;
            this.label5.Text = "수수료발생";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Green;
            this.label6.Location = new System.Drawing.Point(591, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 17);
            this.label6.TabIndex = 12;
            this.label6.Text = "     ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(454, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "방문";
            // 
            // dgdf
            // 
            this.dgdf.AutoSize = true;
            this.dgdf.Location = new System.Drawing.Point(533, 12);
            this.dgdf.Name = "dgdf";
            this.dgdf.Size = new System.Drawing.Size(47, 17);
            this.dgdf.TabIndex = 10;
            this.dgdf.Text = "미방문";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Gray;
            this.label3.Location = new System.Drawing.Point(494, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "     ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(414, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "     ";
            // 
            // RdoCalendarSearchType_Visit
            // 
            this.RdoCalendarSearchType_Visit.AutoSize = true;
            this.RdoCalendarSearchType_Visit.Location = new System.Drawing.Point(343, 10);
            this.RdoCalendarSearchType_Visit.Name = "RdoCalendarSearchType_Visit";
            this.RdoCalendarSearchType_Visit.Size = new System.Drawing.Size(52, 21);
            this.RdoCalendarSearchType_Visit.TabIndex = 7;
            this.RdoCalendarSearchType_Visit.Text = "방문";
            this.RdoCalendarSearchType_Visit.UseVisualStyleBackColor = true;
            this.RdoCalendarSearchType_Visit.Click += new System.EventHandler(this.RdoCalendarSearchType_All_Click);
            // 
            // RdoCalendarSearchType_Unvisit
            // 
            this.RdoCalendarSearchType_Unvisit.AutoSize = true;
            this.RdoCalendarSearchType_Unvisit.Location = new System.Drawing.Point(272, 10);
            this.RdoCalendarSearchType_Unvisit.Name = "RdoCalendarSearchType_Unvisit";
            this.RdoCalendarSearchType_Unvisit.Size = new System.Drawing.Size(65, 21);
            this.RdoCalendarSearchType_Unvisit.TabIndex = 6;
            this.RdoCalendarSearchType_Unvisit.Text = "미방문";
            this.RdoCalendarSearchType_Unvisit.UseVisualStyleBackColor = true;
            this.RdoCalendarSearchType_Unvisit.Click += new System.EventHandler(this.RdoCalendarSearchType_All_Click);
            // 
            // RdoCalendarSearchType_All
            // 
            this.RdoCalendarSearchType_All.AutoSize = true;
            this.RdoCalendarSearchType_All.Checked = true;
            this.RdoCalendarSearchType_All.Location = new System.Drawing.Point(214, 10);
            this.RdoCalendarSearchType_All.Name = "RdoCalendarSearchType_All";
            this.RdoCalendarSearchType_All.Size = new System.Drawing.Size(52, 21);
            this.RdoCalendarSearchType_All.TabIndex = 5;
            this.RdoCalendarSearchType_All.TabStop = true;
            this.RdoCalendarSearchType_All.Text = "전체";
            this.RdoCalendarSearchType_All.UseVisualStyleBackColor = true;
            this.RdoCalendarSearchType_All.Click += new System.EventHandler(this.RdoCalendarSearchType_All_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "담당자";
            // 
            // CboManager
            // 
            this.CboManager.FormattingEnabled = true;
            this.CboManager.Location = new System.Drawing.Point(65, 9);
            this.CboManager.Name = "CboManager";
            this.CboManager.Size = new System.Drawing.Size(121, 25);
            this.CboManager.TabIndex = 3;
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(709, 4);
            this.BtnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(70, 33);
            this.BtnSearch.TabIndex = 0;
            this.BtnSearch.Text = "검색";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(785, 4);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(67, 33);
            this.button2.TabIndex = 14;
            this.button2.Text = "인쇄";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // CalendarForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 871);
            this.Controls.Add(this.panCalendar);
            this.Controls.Add(this.panSearch);
            this.Controls.Add(this.formTItle1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "CalendarForm";
            this.Text = "방문일정";
            this.Load += new System.EventHandler(this.CalendarForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panSearch.ResumeLayout(false);
            this.panSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panCalendar;
        private System.Windows.Forms.Button button1;
        private ComBase.Mvc.UserControls.FormTItle formTItle1;
        private System.Windows.Forms.Panel panSearch;
        private System.Windows.Forms.RadioButton RdoCalendarSearchType_All;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CboManager;
        private System.Windows.Forms.RadioButton RdoCalendarSearchType_Visit;
        private System.Windows.Forms.RadioButton RdoCalendarSearchType_Unvisit;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label dgdf;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button2;
    }
}