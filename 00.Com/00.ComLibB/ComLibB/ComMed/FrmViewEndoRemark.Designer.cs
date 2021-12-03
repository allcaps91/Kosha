namespace ComLibB
{
    partial class FrmViewEndoRemark
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
            this.grb = new System.Windows.Forms.GroupBox();
            this.cboSRemark = new System.Windows.Forms.ComboBox();
            this.cboSelect = new System.Windows.Forms.ComboBox();
            this.lblItem0 = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rtxtRemark2 = new System.Windows.Forms.TextBox();
            this.rtxtRemark1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnHelp = new System.Windows.Forms.Button();
            this.grpCP = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rdoCP0 = new System.Windows.Forms.RadioButton();
            this.rdoCP1 = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cboCP = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.grb.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpCP.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // grb
            // 
            this.grb.Controls.Add(this.cboSRemark);
            this.grb.Controls.Add(this.cboSelect);
            this.grb.Controls.Add(this.lblItem0);
            this.grb.Location = new System.Drawing.Point(7, 40);
            this.grb.Name = "grb";
            this.grb.Size = new System.Drawing.Size(503, 53);
            this.grb.TabIndex = 42;
            this.grb.TabStop = false;
            this.grb.Text = "상용 소견 선택";
            // 
            // cboSRemark
            // 
            this.cboSRemark.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSRemark.FormattingEnabled = true;
            this.cboSRemark.Location = new System.Drawing.Point(287, 21);
            this.cboSRemark.Name = "cboSRemark";
            this.cboSRemark.Size = new System.Drawing.Size(197, 20);
            this.cboSRemark.TabIndex = 28;
            this.cboSRemark.Click += new System.EventHandler(this.cboSRemark_Click);
            this.cboSRemark.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboSRemark_KeyPress);
            // 
            // cboSelect
            // 
            this.cboSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelect.FormattingEnabled = true;
            this.cboSelect.Location = new System.Drawing.Point(84, 21);
            this.cboSelect.Name = "cboSelect";
            this.cboSelect.Size = new System.Drawing.Size(197, 20);
            this.cboSelect.TabIndex = 26;
            this.cboSelect.Click += new System.EventHandler(this.cboSelect_Click);
            this.cboSelect.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboSelect_KeyPress);
            // 
            // lblItem0
            // 
            this.lblItem0.AutoSize = true;
            this.lblItem0.Location = new System.Drawing.Point(21, 24);
            this.lblItem0.Name = "lblItem0";
            this.lblItem0.Size = new System.Drawing.Size(57, 12);
            this.lblItem0.TabIndex = 25;
            this.lblItem0.Text = "소견 선택";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(3, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(96, 21);
            this.lblTitle.TabIndex = 43;
            this.lblTitle.Text = "내시경 소견";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(594, 47);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 46);
            this.btnExit.TabIndex = 44;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(516, 47);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 46);
            this.btnSave.TabIndex = 45;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rtxtRemark2);
            this.groupBox1.Controls.Add(this.rtxtRemark1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 164);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(677, 217);
            this.groupBox1.TabIndex = 46;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "소견 등록(항목이동은 Tab Key로 하세요)";
            // 
            // rtxtRemark2
            // 
            this.rtxtRemark2.Location = new System.Drawing.Point(14, 146);
            this.rtxtRemark2.Multiline = true;
            this.rtxtRemark2.Name = "rtxtRemark2";
            this.rtxtRemark2.Size = new System.Drawing.Size(641, 60);
            this.rtxtRemark2.TabIndex = 30;
            this.rtxtRemark2.Click += new System.EventHandler(this.rtxtRemark2_Click);
            // 
            // rtxtRemark1
            // 
            this.rtxtRemark1.Location = new System.Drawing.Point(12, 45);
            this.rtxtRemark1.Multiline = true;
            this.rtxtRemark1.Name = "rtxtRemark1";
            this.rtxtRemark1.Size = new System.Drawing.Size(641, 74);
            this.rtxtRemark1.TabIndex = 29;
            this.rtxtRemark1.Click += new System.EventHandler(this.rtxtRemark1_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(10, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(649, 20);
            this.label2.TabIndex = 28;
            this.label2.Text = "Clinical Diagnosis";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(649, 20);
            this.label1.TabIndex = 26;
            this.label1.Text = "Chief Complaints";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHelp.BackColor = System.Drawing.Color.White;
            this.btnHelp.Location = new System.Drawing.Point(516, 13);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(150, 28);
            this.btnHelp.TabIndex = 47;
            this.btnHelp.Text = "Help(도움말)";
            this.btnHelp.UseVisualStyleBackColor = false;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // grpCP
            // 
            this.grpCP.Controls.Add(this.panel2);
            this.grpCP.Controls.Add(this.panel1);
            this.grpCP.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCP.Location = new System.Drawing.Point(0, 102);
            this.grpCP.Name = "grpCP";
            this.grpCP.Padding = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.grpCP.Size = new System.Drawing.Size(677, 62);
            this.grpCP.TabIndex = 48;
            this.grpCP.TabStop = false;
            this.grpCP.Text = "CP설정";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rdoCP1);
            this.panel1.Controls.Add(this.rdoCP0);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(3, 17);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(167, 35);
            this.panel1.TabIndex = 0;
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
            // panel2
            // 
            this.panel2.Controls.Add(this.cboCP);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(170, 17);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(489, 35);
            this.panel2.TabIndex = 1;
            // 
            // cboCP
            // 
            this.cboCP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCP.FormattingEnabled = true;
            this.cboCP.Location = new System.Drawing.Point(6, 6);
            this.cboCP.Name = "cboCP";
            this.cboCP.Size = new System.Drawing.Size(477, 20);
            this.cboCP.TabIndex = 27;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblTitle);
            this.panel3.Controls.Add(this.grb);
            this.panel3.Controls.Add(this.btnHelp);
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(677, 102);
            this.panel3.TabIndex = 49;
            // 
            // FrmViewEndoRemark
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(677, 383);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpCP);
            this.Controls.Add(this.panel3);
            this.Name = "FrmViewEndoRemark";
            this.Text = "내시경소견";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmViewEndoRemark_FormClosed);
            this.Load += new System.EventHandler(this.FrmViewEndoRemark_Load);
            this.grb.ResumeLayout(false);
            this.grb.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpCP.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grb;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblItem0;
        private System.Windows.Forms.ComboBox cboSRemark;
        private System.Windows.Forms.ComboBox cboSelect;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.TextBox rtxtRemark2;
        private System.Windows.Forms.TextBox rtxtRemark1;
        private System.Windows.Forms.GroupBox grpCP;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cboCP;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rdoCP1;
        private System.Windows.Forms.RadioButton rdoCP0;
        private System.Windows.Forms.Panel panel3;
    }
}