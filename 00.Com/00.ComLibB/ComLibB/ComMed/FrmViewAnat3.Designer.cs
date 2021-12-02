namespace ComLibB
{
    partial class FrmViewAnat3
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
            this.btnSave = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.rtxtRequest1 = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rtxtRequest2 = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnHelp = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panCP = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rdoCP1 = new System.Windows.Forms.RadioButton();
            this.rdoCP0 = new System.Windows.Forms.RadioButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cboCP = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panCP.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(316, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(77, 30);
            this.btnSave.TabIndex = 24;
            this.btnSave.Text = "확인(&O)";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(7, 13);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(120, 21);
            this.lblTitle.TabIndex = 37;
            this.lblTitle.Text = "PB smear 소견";
            // 
            // rtxtRequest1
            // 
            this.rtxtRequest1.Location = new System.Drawing.Point(7, 23);
            this.rtxtRequest1.Name = "rtxtRequest1";
            this.rtxtRequest1.Size = new System.Drawing.Size(386, 95);
            this.rtxtRequest1.TabIndex = 39;
            this.rtxtRequest1.Text = "";
            this.rtxtRequest1.Click += new System.EventHandler(this.rtxtRequest1_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(7, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(388, 17);
            this.label1.TabIndex = 38;
            this.label1.Text = "Clinical Diagnosis or Impression";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rtxtRequest2
            // 
            this.rtxtRequest2.Location = new System.Drawing.Point(7, 167);
            this.rtxtRequest2.Name = "rtxtRequest2";
            this.rtxtRequest2.Size = new System.Drawing.Size(386, 95);
            this.rtxtRequest2.TabIndex = 41;
            this.rtxtRequest2.Text = "";
            this.rtxtRequest2.Click += new System.EventHandler(this.rtxtRequest2_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(7, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(388, 17);
            this.label2.TabIndex = 40;
            this.label2.Text = "Specific previous Hematologic Disease";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(7, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(388, 17);
            this.label3.TabIndex = 42;
            this.label3.Text = "단. 과거력이 없으면 None 입력해주세요!!";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnHelp
            // 
            this.btnHelp.BackColor = System.Drawing.Color.White;
            this.btnHelp.Location = new System.Drawing.Point(223, 8);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(93, 30);
            this.btnHelp.TabIndex = 49;
            this.btnHelp.Text = "상용구";
            this.btnHelp.UseVisualStyleBackColor = false;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Controls.Add(this.btnHelp);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(402, 46);
            this.panel1.TabIndex = 50;
            // 
            // panCP
            // 
            this.panCP.Controls.Add(this.panel2);
            this.panCP.Controls.Add(this.panel3);
            this.panCP.Controls.Add(this.label6);
            this.panCP.Dock = System.Windows.Forms.DockStyle.Top;
            this.panCP.Location = new System.Drawing.Point(0, 46);
            this.panCP.Name = "panCP";
            this.panCP.Size = new System.Drawing.Size(402, 78);
            this.panCP.TabIndex = 57;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rdoCP1);
            this.panel2.Controls.Add(this.rdoCP0);
            this.panel2.Location = new System.Drawing.Point(9, 32);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(167, 32);
            this.panel2.TabIndex = 53;
            // 
            // rdoCP1
            // 
            this.rdoCP1.AutoSize = true;
            this.rdoCP1.Location = new System.Drawing.Point(93, 8);
            this.rdoCP1.Name = "rdoCP1";
            this.rdoCP1.Size = new System.Drawing.Size(64, 16);
            this.rdoCP1.TabIndex = 1;
            this.rdoCP1.Text = "CP사용";
            this.rdoCP1.UseVisualStyleBackColor = true;
            this.rdoCP1.CheckedChanged += new System.EventHandler(this.rdoCP1_CheckedChanged);
            // 
            // rdoCP0
            // 
            this.rdoCP0.AutoSize = true;
            this.rdoCP0.Checked = true;
            this.rdoCP0.Location = new System.Drawing.Point(11, 8);
            this.rdoCP0.Name = "rdoCP0";
            this.rdoCP0.Size = new System.Drawing.Size(76, 16);
            this.rdoCP0.TabIndex = 1;
            this.rdoCP0.TabStop = true;
            this.rdoCP0.Text = "CP미사용";
            this.rdoCP0.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.cboCP);
            this.panel3.Location = new System.Drawing.Point(176, 32);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(217, 32);
            this.panel3.TabIndex = 54;
            // 
            // cboCP
            // 
            this.cboCP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCP.FormattingEnabled = true;
            this.cboCP.Location = new System.Drawing.Point(5, 6);
            this.cboCP.Name = "cboCP";
            this.cboCP.Size = new System.Drawing.Size(209, 20);
            this.cboCP.TabIndex = 27;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(7, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(419, 17);
            this.label6.TabIndex = 55;
            this.label6.Text = "CP설정";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.rtxtRequest1);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.rtxtRequest2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 124);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(402, 269);
            this.panel4.TabIndex = 58;
            // 
            // FrmViewAnat3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(402, 397);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panCP);
            this.Controls.Add(this.panel1);
            this.Name = "FrmViewAnat3";
            this.Text = "PB smear 소견";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmViewAnat3_FormClosed);
            this.Load += new System.EventHandler(this.FrmViewAnat3_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panCP.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.RichTextBox rtxtRequest1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox rtxtRequest2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panCP;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rdoCP1;
        private System.Windows.Forms.RadioButton rdoCP0;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox cboCP;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel4;
    }
}