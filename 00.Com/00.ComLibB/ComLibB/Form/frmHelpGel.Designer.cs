namespace ComLibB
{
    partial class frmHelpGel
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
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grbDate = new System.Windows.Forms.GroupBox();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.btnView = new System.Windows.Forms.Button();
            this.optCho1 = new System.Windows.Forms.RadioButton();
            this.optCho0 = new System.Windows.Forms.RadioButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.libGel = new System.Windows.Forms.ListBox();
            this.panTitle0.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panel1.SuspendLayout();
            this.grbDate.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle0
            // 
            this.panTitle0.BackColor = System.Drawing.Color.White;
            this.panTitle0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle0.Controls.Add(this.btnExit);
            this.panTitle0.Controls.Add(this.lblTitle);
            this.panTitle0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle0.ForeColor = System.Drawing.Color.White;
            this.panTitle0.Location = new System.Drawing.Point(0, 0);
            this.panTitle0.Name = "panTitle0";
            this.panTitle0.Size = new System.Drawing.Size(655, 38);
            this.panTitle0.TabIndex = 77;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(569, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(79, 29);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "닫  기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(5, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(122, 21);
            this.lblTitle.TabIndex = 14;
            this.lblTitle.Text = "거래처코드조회";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 38);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(655, 27);
            this.panTitleSub0.TabIndex = 89;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(12, 3);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(65, 17);
            this.lblTitleSub0.TabIndex = 21;
            this.lblTitleSub0.Text = "조회 조건";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grbDate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 65);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(655, 59);
            this.panel1.TabIndex = 90;
            // 
            // grbDate
            // 
            this.grbDate.Controls.Add(this.txtInput);
            this.grbDate.Controls.Add(this.btnView);
            this.grbDate.Controls.Add(this.optCho1);
            this.grbDate.Controls.Add(this.optCho0);
            this.grbDate.Dock = System.Windows.Forms.DockStyle.Left;
            this.grbDate.Location = new System.Drawing.Point(0, 0);
            this.grbDate.Name = "grbDate";
            this.grbDate.Size = new System.Drawing.Size(331, 59);
            this.grbDate.TabIndex = 0;
            this.grbDate.TabStop = false;
            this.grbDate.Text = "조건";
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(94, 21);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(152, 21);
            this.txtInput.TabIndex = 95;
            this.txtInput.Enter += new System.EventHandler(this.txtInput_Enter);
            this.txtInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInput_KeyPress);
            this.txtInput.Leave += new System.EventHandler(this.txtInput_Leave);
            // 
            // btnView
            // 
            this.btnView.BackColor = System.Drawing.Color.Transparent;
            this.btnView.ForeColor = System.Drawing.Color.Black;
            this.btnView.Location = new System.Drawing.Point(252, 18);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(72, 27);
            this.btnView.TabIndex = 25;
            this.btnView.Text = "조  회";
            this.btnView.UseVisualStyleBackColor = false;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // optCho1
            // 
            this.optCho1.AutoSize = true;
            this.optCho1.Location = new System.Drawing.Point(6, 33);
            this.optCho1.Name = "optCho1";
            this.optCho1.Size = new System.Drawing.Size(83, 16);
            this.optCho1.TabIndex = 96;
            this.optCho1.Text = "거래처코드";
            this.optCho1.UseVisualStyleBackColor = true;
            // 
            // optCho0
            // 
            this.optCho0.AutoSize = true;
            this.optCho0.Checked = true;
            this.optCho0.Location = new System.Drawing.Point(6, 15);
            this.optCho0.Name = "optCho0";
            this.optCho0.Size = new System.Drawing.Size(71, 16);
            this.optCho0.TabIndex = 95;
            this.optCho0.TabStop = true;
            this.optCho0.Text = "거래처명";
            this.optCho0.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 124);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(655, 27);
            this.panel3.TabIndex = 94;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(12, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 17);
            this.label3.TabIndex = 21;
            this.label3.Text = "조회 화면";
            // 
            // libGel
            // 
            this.libGel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.libGel.FormattingEnabled = true;
            this.libGel.ItemHeight = 12;
            this.libGel.Items.AddRange(new object[] {
            "libGel"});
            this.libGel.Location = new System.Drawing.Point(0, 151);
            this.libGel.Name = "libGel";
            this.libGel.Size = new System.Drawing.Size(655, 371);
            this.libGel.TabIndex = 95;
            this.libGel.DoubleClick += new System.EventHandler(this.libGel_DoubleClick);
            this.libGel.Enter += new System.EventHandler(this.libGel_Enter);
            this.libGel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.libGel_KeyPress);
            // 
            // frmHelpGel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(655, 522);
            this.Controls.Add(this.libGel);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle0);
            this.Name = "frmHelpGel";
            this.Text = "거래처코드조회";
            this.Load += new System.EventHandler(this.frmHelpGel_Load);
            this.panTitle0.ResumeLayout(false);
            this.panTitle0.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.grbDate.ResumeLayout(false);
            this.grbDate.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle0;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.GroupBox grbDate;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.RadioButton optCho1;
        private System.Windows.Forms.RadioButton optCho0;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox libGel;
    }
}