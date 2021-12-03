namespace ComBase
{
    partial class frmCalendarTime
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
            this.panTitle0 = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.monthDate = new System.Windows.Forms.MonthCalendar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblYYMMDD = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMM = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtHH = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panTitle0.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle0
            // 
            this.panTitle0.BackColor = System.Drawing.Color.White;
            this.panTitle0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle0.Controls.Add(this.lblTitle);
            this.panTitle0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle0.ForeColor = System.Drawing.Color.White;
            this.panTitle0.Location = new System.Drawing.Point(0, 0);
            this.panTitle0.Name = "panTitle0";
            this.panTitle0.Size = new System.Drawing.Size(293, 38);
            this.panTitle0.TabIndex = 80;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(5, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(74, 21);
            this.lblTitle.TabIndex = 14;
            this.lblTitle.Text = "예약설정";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // monthDate
            // 
            this.monthDate.Location = new System.Drawing.Point(11, 50);
            this.monthDate.MaxSelectionCount = 1;
            this.monthDate.Name = "monthDate";
            this.monthDate.TabIndex = 82;
            this.monthDate.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.monthDate_DateChanged);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblYYMMDD);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtMM);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.txtHH);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 224);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(219, 145);
            this.panel1.TabIndex = 83;
            // 
            // lblYYMMDD
            // 
            this.lblYYMMDD.BackColor = System.Drawing.Color.LightGray;
            this.lblYYMMDD.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblYYMMDD.Location = new System.Drawing.Point(89, 14);
            this.lblYYMMDD.Name = "lblYYMMDD";
            this.lblYYMMDD.Size = new System.Drawing.Size(111, 21);
            this.lblYYMMDD.TabIndex = 24;
            this.lblYYMMDD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.LightGray;
            this.label4.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(17, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 21);
            this.label4.TabIndex = 23;
            this.label4.Text = "선택일자";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.LightGray;
            this.label3.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(148, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 25);
            this.label3.TabIndex = 22;
            this.label3.Text = "분";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.LightGray;
            this.label2.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(83, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 25);
            this.label2.TabIndex = 21;
            this.label2.Text = "시";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMM
            // 
            this.txtMM.Location = new System.Drawing.Point(123, 46);
            this.txtMM.Name = "txtMM";
            this.txtMM.Size = new System.Drawing.Size(25, 25);
            this.txtMM.TabIndex = 20;
            this.txtMM.Text = "23";
            this.txtMM.Enter += new System.EventHandler(this.txtMM_Enter);
            this.txtMM.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMM_KeyPress);
            this.txtMM.Leave += new System.EventHandler(this.txtMM_Leave);
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSize = true;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(110, 89);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(79, 29);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "취  소";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.AutoSize = true;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSave.Location = new System.Drawing.Point(25, 89);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(79, 29);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "확  인";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtHH
            // 
            this.txtHH.Location = new System.Drawing.Point(58, 46);
            this.txtHH.Name = "txtHH";
            this.txtHH.Size = new System.Drawing.Size(25, 25);
            this.txtHH.TabIndex = 1;
            this.txtHH.Text = "23";
            this.txtHH.Enter += new System.EventHandler(this.txtHH_Enter);
            this.txtHH.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtHH_KeyPress);
            this.txtHH.Leave += new System.EventHandler(this.txtHH_Leave);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LightGray;
            this.label1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(18, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "시간:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmCalendarTime
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(293, 388);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.monthDate);
            this.Controls.Add(this.panTitle0);
            this.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.Name = "frmCalendarTime";
            this.Text = "예약설정";
            this.Load += new System.EventHandler(this.frmCalendarTime_Load);
            this.panTitle0.ResumeLayout(false);
            this.panTitle0.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle0;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.MonthCalendar monthDate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblYYMMDD;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMM;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtHH;
        private System.Windows.Forms.Label label1;
    }
}