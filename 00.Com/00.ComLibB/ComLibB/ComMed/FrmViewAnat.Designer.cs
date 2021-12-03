namespace ComLibB
{
    partial class FrmViewAnat
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtNature = new System.Windows.Forms.TextBox();
            this.txtDiagnosis = new System.Windows.Forms.TextBox();
            this.txtClinicalHis = new System.Windows.Forms.TextBox();
            this.txtInformation = new System.Windows.Forms.TextBox();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panCP = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rdoCP1 = new System.Windows.Forms.RadioButton();
            this.rdoCP0 = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cboCP = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblOrder = new System.Windows.Forms.Label();
            this.panel3.SuspendLayout();
            this.panCP.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(7, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(419, 17);
            this.label1.TabIndex = 19;
            this.label1.Text = "Nature _Source of Specimen";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(7, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(419, 17);
            this.label2.TabIndex = 20;
            this.label2.Text = "Clinical Diagnosis";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(7, 239);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(419, 17);
            this.label3.TabIndex = 21;
            this.label3.Text = "Clinical History && Information";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(7, 372);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(419, 17);
            this.label4.TabIndex = 22;
            this.label4.Text = "Information on previou Cytology Examination";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtNature
            // 
            this.txtNature.Location = new System.Drawing.Point(6, 25);
            this.txtNature.Multiline = true;
            this.txtNature.Name = "txtNature";
            this.txtNature.Size = new System.Drawing.Size(418, 73);
            this.txtNature.TabIndex = 49;
            this.txtNature.Click += new System.EventHandler(this.txtNature_Click);
            // 
            // txtDiagnosis
            // 
            this.txtDiagnosis.Location = new System.Drawing.Point(6, 126);
            this.txtDiagnosis.Multiline = true;
            this.txtDiagnosis.Name = "txtDiagnosis";
            this.txtDiagnosis.Size = new System.Drawing.Size(418, 106);
            this.txtDiagnosis.TabIndex = 50;
            this.txtDiagnosis.Click += new System.EventHandler(this.txtDiagnosis_Click);
            // 
            // txtClinicalHis
            // 
            this.txtClinicalHis.Location = new System.Drawing.Point(6, 260);
            this.txtClinicalHis.Multiline = true;
            this.txtClinicalHis.Name = "txtClinicalHis";
            this.txtClinicalHis.Size = new System.Drawing.Size(418, 106);
            this.txtClinicalHis.TabIndex = 51;
            this.txtClinicalHis.Click += new System.EventHandler(this.txtClinicalHis_Click);
            // 
            // txtInformation
            // 
            this.txtInformation.Location = new System.Drawing.Point(6, 394);
            this.txtInformation.Multiline = true;
            this.txtInformation.Name = "txtInformation";
            this.txtInformation.Size = new System.Drawing.Size(418, 106);
            this.txtInformation.TabIndex = 52;
            this.txtInformation.Click += new System.EventHandler(this.txtInformation_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.BackColor = System.Drawing.Color.White;
            this.btnHelp.Location = new System.Drawing.Point(244, 10);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(93, 30);
            this.btnHelp.TabIndex = 48;
            this.btnHelp.Text = "상용구";
            this.btnHelp.UseVisualStyleBackColor = false;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(337, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 30);
            this.btnSave.TabIndex = 18;
            this.btnSave.Text = "확인(&O)";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(11, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(116, 21);
            this.lblTitle.TabIndex = 37;
            this.lblTitle.Text = "Cytology 소견";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblOrder);
            this.panel3.Controls.Add(this.lblTitle);
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Controls.Add(this.btnHelp);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(432, 71);
            this.panel3.TabIndex = 53;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(152, 10);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(93, 30);
            this.btnSearch.TabIndex = 48;
            this.btnSearch.Text = "가져오기";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panCP
            // 
            this.panCP.Controls.Add(this.panel1);
            this.panCP.Controls.Add(this.panel2);
            this.panCP.Controls.Add(this.label6);
            this.panCP.Dock = System.Windows.Forms.DockStyle.Top;
            this.panCP.Location = new System.Drawing.Point(0, 71);
            this.panCP.Name = "panCP";
            this.panCP.Size = new System.Drawing.Size(432, 78);
            this.panCP.TabIndex = 57;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rdoCP1);
            this.panel1.Controls.Add(this.rdoCP0);
            this.panel1.Location = new System.Drawing.Point(6, 32);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(167, 32);
            this.panel1.TabIndex = 53;
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
            // panel2
            // 
            this.panel2.Controls.Add(this.cboCP);
            this.panel2.Location = new System.Drawing.Point(173, 32);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(256, 32);
            this.panel2.TabIndex = 54;
            // 
            // cboCP
            // 
            this.cboCP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCP.FormattingEnabled = true;
            this.cboCP.Location = new System.Drawing.Point(5, 6);
            this.cboCP.Name = "cboCP";
            this.cboCP.Size = new System.Drawing.Size(246, 20);
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
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.txtInformation);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.txtClinicalHis);
            this.panel4.Controls.Add(this.txtNature);
            this.panel4.Controls.Add(this.txtDiagnosis);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 149);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(432, 505);
            this.panel4.TabIndex = 58;
            // 
            // lblOrder
            // 
            this.lblOrder.AutoSize = true;
            this.lblOrder.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblOrder.ForeColor = System.Drawing.Color.Navy;
            this.lblOrder.Location = new System.Drawing.Point(13, 43);
            this.lblOrder.Name = "lblOrder";
            this.lblOrder.Size = new System.Drawing.Size(126, 21);
            this.lblOrder.TabIndex = 49;
            this.lblOrder.Text = "Pathology 소견";
            // 
            // FrmViewAnat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(432, 652);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panCP);
            this.Controls.Add(this.panel3);
            this.Name = "FrmViewAnat";
            this.Text = "Cytology 소견";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmViewAnat_FormClosed);
            this.Load += new System.EventHandler(this.FrmViewAnat_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panCP.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtNature;
        private System.Windows.Forms.TextBox txtDiagnosis;
        private System.Windows.Forms.TextBox txtClinicalHis;
        private System.Windows.Forms.TextBox txtInformation;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panCP;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rdoCP1;
        private System.Windows.Forms.RadioButton rdoCP0;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cboCP;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblOrder;
    }
}