namespace ComBase
{
    partial class frmCalendar
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
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.TxtYear = new System.Windows.Forms.TextBox();
            this.UpDown1 = new System.Windows.Forms.HScrollBar();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Calendar = new System.Windows.Forms.MonthCalendar();
            this.panTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnSave);
            this.panTitle.Controls.Add(this.TxtYear);
            this.panTitle.Controls.Add(this.UpDown1);
            this.panTitle.Controls.Add(this.Label1);
            this.panTitle.Controls.Add(this.Label2);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(449, 34);
            this.panTitle.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(362, 1);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 30);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "확인";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // TxtYear
            // 
            this.TxtYear.AcceptsReturn = true;
            this.TxtYear.BackColor = System.Drawing.SystemColors.Window;
            this.TxtYear.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TxtYear.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TxtYear.ForeColor = System.Drawing.SystemColors.WindowText;
            this.TxtYear.Location = new System.Drawing.Point(14, 5);
            this.TxtYear.MaxLength = 4;
            this.TxtYear.Name = "TxtYear";
            this.TxtYear.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TxtYear.Size = new System.Drawing.Size(49, 22);
            this.TxtYear.TabIndex = 7;
            this.TxtYear.Text = "2012";
            this.TxtYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtYear.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtYear_KeyPress);
            // 
            // UpDown1
            // 
            this.UpDown1.Cursor = System.Windows.Forms.Cursors.Default;
            this.UpDown1.LargeChange = 1;
            this.UpDown1.Location = new System.Drawing.Point(62, 5);
            this.UpDown1.Maximum = 9999;
            this.UpDown1.Minimum = 1753;
            this.UpDown1.Name = "UpDown1";
            this.UpDown1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UpDown1.Size = new System.Drawing.Size(31, 22);
            this.UpDown1.TabIndex = 6;
            this.UpDown1.TabStop = true;
            this.UpDown1.Value = 1753;
            this.UpDown1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.UpDown1_Scroll);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.BackColor = System.Drawing.SystemColors.Control;
            this.Label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.Label1.Location = new System.Drawing.Point(154, 10);
            this.Label1.Name = "Label1";
            this.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label1.Size = new System.Drawing.Size(177, 13);
            this.Label1.TabIndex = 9;
            this.Label1.Text = "◆ 날짜를 더블클릭 하세요 ◆";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.BackColor = System.Drawing.Color.Transparent;
            this.Label2.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label2.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.Label2.Location = new System.Drawing.Point(97, 10);
            this.Label2.Name = "Label2";
            this.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label2.Size = new System.Drawing.Size(35, 13);
            this.Label2.TabIndex = 8;
            this.Label2.Text = "년도";
            // 
            // Calendar
            // 
            this.Calendar.CalendarDimensions = new System.Drawing.Size(2, 1);
            this.Calendar.Location = new System.Drawing.Point(2, 36);
            this.Calendar.Name = "Calendar";
            this.Calendar.TabIndex = 2;
            this.Calendar.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.Calendar_DateChanged);
            // 
            // frmCalendar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 199);
            this.Controls.Add(this.Calendar);
            this.Controls.Add(this.panTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmCalendar";
            this.Text = "달력(날짜선택)";
            this.Load += new System.EventHandler(this.frmCalendar_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnSave;
        public System.Windows.Forms.TextBox TxtYear;
        public System.Windows.Forms.HScrollBar UpDown1;
        public System.Windows.Forms.Label Label1;
        public System.Windows.Forms.Label Label2;
        private System.Windows.Forms.MonthCalendar Calendar;
    }
}