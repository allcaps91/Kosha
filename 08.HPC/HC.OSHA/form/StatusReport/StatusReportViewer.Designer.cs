namespace HC_OSHA.StatusReport
{
    partial class StatusReportViewer
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panTitle = new System.Windows.Forms.Panel();
            this.LblMailSendDate = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.Rdo4 = new System.Windows.Forms.RadioButton();
            this.Rdo3 = new System.Windows.Forms.RadioButton();
            this.ChkOSHA = new System.Windows.Forms.CheckBox();
            this.BtnSign = new System.Windows.Forms.Button();
            this.BtnClose = new System.Windows.Forms.Button();
            this.BtnSendMail = new System.Windows.Forms.Button();
            this.BtnPdf = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDevTool = new System.Windows.Forms.Button();
            this.panTitle.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 71);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(866, 890);
            this.panel1.TabIndex = 0;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.LblMailSendDate);
            this.panTitle.Controls.Add(this.label2);
            this.panTitle.Controls.Add(this.panel2);
            this.panTitle.Controls.Add(this.ChkOSHA);
            this.panTitle.Controls.Add(this.BtnSign);
            this.panTitle.Controls.Add(this.BtnClose);
            this.panTitle.Controls.Add(this.BtnSendMail);
            this.panTitle.Controls.Add(this.BtnPdf);
            this.panTitle.Controls.Add(this.btnSave);
            this.panTitle.Controls.Add(this.btnDevTool);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(866, 71);
            this.panTitle.TabIndex = 42;
            // 
            // LblMailSendDate
            // 
            this.LblMailSendDate.AutoSize = true;
            this.LblMailSendDate.Location = new System.Drawing.Point(110, 48);
            this.LblMailSendDate.Name = "LblMailSendDate";
            this.LblMailSendDate.Size = new System.Drawing.Size(0, 12);
            this.LblMailSendDate.TabIndex = 46;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 12);
            this.label2.TabIndex = 45;
            this.label2.Text = "최종 메일 발송일";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.Rdo4);
            this.panel2.Controls.Add(this.Rdo3);
            this.panel2.Location = new System.Drawing.Point(240, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(148, 30);
            this.panel2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "결재란";
            // 
            // Rdo4
            // 
            this.Rdo4.AutoSize = true;
            this.Rdo4.Location = new System.Drawing.Point(103, 7);
            this.Rdo4.Name = "Rdo4";
            this.Rdo4.Size = new System.Drawing.Size(41, 16);
            this.Rdo4.TabIndex = 46;
            this.Rdo4.TabStop = true;
            this.Rdo4.Text = "4칸";
            this.Rdo4.UseVisualStyleBackColor = true;
            this.Rdo4.CheckedChanged += new System.EventHandler(this.Rdo3_CheckedChanged);
            // 
            // Rdo3
            // 
            this.Rdo3.AutoSize = true;
            this.Rdo3.Location = new System.Drawing.Point(56, 7);
            this.Rdo3.Name = "Rdo3";
            this.Rdo3.Size = new System.Drawing.Size(41, 16);
            this.Rdo3.TabIndex = 45;
            this.Rdo3.TabStop = true;
            this.Rdo3.Text = "3칸";
            this.Rdo3.UseVisualStyleBackColor = true;
            this.Rdo3.CheckedChanged += new System.EventHandler(this.Rdo3_CheckedChanged);
            // 
            // ChkOSHA
            // 
            this.ChkOSHA.AutoSize = true;
            this.ChkOSHA.Location = new System.Drawing.Point(154, 14);
            this.ChkOSHA.Name = "ChkOSHA";
            this.ChkOSHA.Size = new System.Drawing.Size(84, 16);
            this.ChkOSHA.TabIndex = 44;
            this.ChkOSHA.Text = "기관보관용";
            this.ChkOSHA.UseVisualStyleBackColor = true;
            this.ChkOSHA.CheckedChanged += new System.EventHandler(this.ChkOSHA_CheckedChanged);
            // 
            // BtnSign
            // 
            this.BtnSign.BackColor = System.Drawing.Color.Transparent;
            this.BtnSign.Location = new System.Drawing.Point(2, 5);
            this.BtnSign.Name = "BtnSign";
            this.BtnSign.Size = new System.Drawing.Size(72, 30);
            this.BtnSign.TabIndex = 43;
            this.BtnSign.Text = "서명";
            this.BtnSign.UseVisualStyleBackColor = false;
            this.BtnSign.Click += new System.EventHandler(this.BtnSign_Click);
            // 
            // BtnClose
            // 
            this.BtnClose.BackColor = System.Drawing.Color.Transparent;
            this.BtnClose.Location = new System.Drawing.Point(782, 5);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(79, 30);
            this.BtnClose.TabIndex = 42;
            this.BtnClose.Text = "닫기";
            this.BtnClose.UseVisualStyleBackColor = false;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // BtnSendMail
            // 
            this.BtnSendMail.BackColor = System.Drawing.Color.Transparent;
            this.BtnSendMail.Location = new System.Drawing.Point(542, 5);
            this.BtnSendMail.Name = "BtnSendMail";
            this.BtnSendMail.Size = new System.Drawing.Size(79, 30);
            this.BtnSendMail.TabIndex = 41;
            this.BtnSendMail.Text = "메일발송";
            this.BtnSendMail.UseVisualStyleBackColor = false;
            this.BtnSendMail.Click += new System.EventHandler(this.BtnSendMail_Click);
            // 
            // BtnPdf
            // 
            this.BtnPdf.BackColor = System.Drawing.Color.Transparent;
            this.BtnPdf.Location = new System.Drawing.Point(622, 5);
            this.BtnPdf.Name = "BtnPdf";
            this.BtnPdf.Size = new System.Drawing.Size(79, 30);
            this.BtnPdf.TabIndex = 40;
            this.BtnPdf.Text = "PDF 저장";
            this.BtnPdf.UseVisualStyleBackColor = false;
            this.BtnPdf.Click += new System.EventHandler(this.BtnPdf_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.Location = new System.Drawing.Point(702, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(79, 30);
            this.btnSave.TabIndex = 39;
            this.btnSave.Text = "인쇄";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDevTool
            // 
            this.btnDevTool.BackColor = System.Drawing.Color.Transparent;
            this.btnDevTool.Location = new System.Drawing.Point(77, 5);
            this.btnDevTool.Name = "btnDevTool";
            this.btnDevTool.Size = new System.Drawing.Size(72, 30);
            this.btnDevTool.TabIndex = 33;
            this.btnDevTool.Text = "개발자";
            this.btnDevTool.UseVisualStyleBackColor = false;
            this.btnDevTool.Click += new System.EventHandler(this.btnDevTool_Click);
            // 
            // StatusReportViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(866, 961);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Name = "StatusReportViewer";
            this.Text = "상태보고서 인쇄";
            this.Load += new System.EventHandler(this.StatusReportViewer_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDevTool;
        private System.Windows.Forms.Button BtnSendMail;
        private System.Windows.Forms.Button BtnPdf;
        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.Button BtnSign;
        private System.Windows.Forms.CheckBox ChkOSHA;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton Rdo4;
        private System.Windows.Forms.RadioButton Rdo3;
        private System.Windows.Forms.Label LblMailSendDate;
        private System.Windows.Forms.Label label2;
    }
}