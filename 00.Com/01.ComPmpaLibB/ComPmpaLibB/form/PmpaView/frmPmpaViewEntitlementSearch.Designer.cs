namespace ComPmpaLibB
{
    partial class frmPmpaViewEntitlementSearch
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtJumin2 = new System.Windows.Forms.TextBox();
            this.txtPano = new System.Windows.Forms.TextBox();
            this.txtJumin1 = new System.Windows.Forms.TextBox();
            this.txtSname = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblItem1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblItem0 = new System.Windows.Forms.Label();
            this.panTitle.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(374, 34);
            this.panTitle.TabIndex = 12;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(128, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "보호자 자격조회";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(295, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.txtJumin2);
            this.panel3.Controls.Add(this.txtPano);
            this.panel3.Controls.Add(this.txtJumin1);
            this.panel3.Controls.Add(this.txtSname);
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.lblItem1);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.lblItem0);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 34);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(374, 109);
            this.panel3.TabIndex = 18;
            // 
            // txtJumin2
            // 
            this.txtJumin2.Location = new System.Drawing.Point(180, 40);
            this.txtJumin2.MaxLength = 7;
            this.txtJumin2.Name = "txtJumin2";
            this.txtJumin2.Size = new System.Drawing.Size(100, 25);
            this.txtJumin2.TabIndex = 3;
            this.txtJumin2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtJumin2_KeyDown);
            // 
            // txtPano
            // 
            this.txtPano.BackColor = System.Drawing.Color.LightGray;
            this.txtPano.Enabled = false;
            this.txtPano.Location = new System.Drawing.Point(67, 72);
            this.txtPano.Name = "txtPano";
            this.txtPano.Size = new System.Drawing.Size(100, 25);
            this.txtPano.TabIndex = 32;
            this.txtPano.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPano_KeyDown);
            // 
            // txtJumin1
            // 
            this.txtJumin1.Location = new System.Drawing.Point(67, 40);
            this.txtJumin1.MaxLength = 6;
            this.txtJumin1.Name = "txtJumin1";
            this.txtJumin1.Size = new System.Drawing.Size(100, 25);
            this.txtJumin1.TabIndex = 2;
            this.txtJumin1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtJumin1_KeyDown);
            // 
            // txtSname
            // 
            this.txtSname.Location = new System.Drawing.Point(67, 8);
            this.txtSname.Name = "txtSname";
            this.txtSname.Size = new System.Drawing.Size(100, 25);
            this.txtSname.TabIndex = 1;
            this.txtSname.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSname_KeyDown);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Location = new System.Drawing.Point(292, 37);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 17);
            this.label1.TabIndex = 25;
            this.label1.Text = "등록번호";
            // 
            // lblItem1
            // 
            this.lblItem1.AutoSize = true;
            this.lblItem1.Location = new System.Drawing.Point(7, 44);
            this.lblItem1.Name = "lblItem1";
            this.lblItem1.Size = new System.Drawing.Size(60, 17);
            this.lblItem1.TabIndex = 25;
            this.lblItem1.Text = "주민번호";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(167, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 17);
            this.label2.TabIndex = 24;
            this.label2.Text = "-";
            // 
            // lblItem0
            // 
            this.lblItem0.AutoSize = true;
            this.lblItem0.Location = new System.Drawing.Point(7, 12);
            this.lblItem0.Name = "lblItem0";
            this.lblItem0.Size = new System.Drawing.Size(34, 17);
            this.lblItem0.TabIndex = 24;
            this.lblItem0.Text = "성명";
            // 
            // frmPmpaViewEntitlementSearch
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(374, 143);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "frmPmpaViewEntitlementSearch";
            this.Text = "보호자 자격조회";
            this.Load += new System.EventHandler(this.frmPmpaViewEntitlementSearch_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        public System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtJumin2;
        private System.Windows.Forms.TextBox txtJumin1;
        private System.Windows.Forms.TextBox txtSname;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblItem1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblItem0;
        private System.Windows.Forms.TextBox txtPano;
        private System.Windows.Forms.Label label1;
    }
}