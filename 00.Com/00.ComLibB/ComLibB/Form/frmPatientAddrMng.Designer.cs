namespace ComLibB
{
    partial class frmPatientAddrMng
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
            this.btnRegist = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtPostCode = new System.Windows.Forms.TextBox();
            this.txtSNAME = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtHPhone = new System.Windows.Forms.TextBox();
            this.txtPtno = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBuildNo = new System.Windows.Forms.TextBox();
            this.txtJuso1 = new System.Windows.Forms.TextBox();
            this.btnPost = new System.Windows.Forms.Button();
            this.txtJuso2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnRegist);
            this.panTitle.Controls.Add(this.btnSearch);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(478, 34);
            this.panTitle.TabIndex = 13;
            // 
            // btnRegist
            // 
            this.btnRegist.BackColor = System.Drawing.Color.Transparent;
            this.btnRegist.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnRegist.Location = new System.Drawing.Point(258, 0);
            this.btnRegist.Name = "btnRegist";
            this.btnRegist.Size = new System.Drawing.Size(72, 30);
            this.btnRegist.TabIndex = 553;
            this.btnRegist.Text = "등록(&O)";
            this.btnRegist.UseVisualStyleBackColor = false;
            this.btnRegist.Click += new System.EventHandler(this.btnRegist_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Location = new System.Drawing.Point(330, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 552;
            this.btnSearch.Text = "조회(&V)";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(402, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 550;
            this.btnExit.Text = "닫기(&D)";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(122, 17);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "환저 주소정보 수정";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.txtPostCode);
            this.panel1.Controls.Add(this.txtSNAME);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtHPhone);
            this.panel1.Controls.Add(this.txtPtno);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtBuildNo);
            this.panel1.Controls.Add(this.txtJuso1);
            this.panel1.Controls.Add(this.btnPost);
            this.panel1.Controls.Add(this.txtJuso2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(478, 154);
            this.panel1.TabIndex = 15;
            // 
            // txtPostCode
            // 
            this.txtPostCode.Location = new System.Drawing.Point(368, 5);
            this.txtPostCode.Name = "txtPostCode";
            this.txtPostCode.Size = new System.Drawing.Size(54, 25);
            this.txtPostCode.TabIndex = 546;
            this.txtPostCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtSNAME
            // 
            this.txtSNAME.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtSNAME.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtSNAME.Location = new System.Drawing.Point(219, 5);
            this.txtSNAME.Name = "txtSNAME";
            this.txtSNAME.Size = new System.Drawing.Size(78, 25);
            this.txtSNAME.TabIndex = 545;
            this.txtSNAME.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Window;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(158, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 17);
            this.label2.TabIndex = 544;
            this.label2.Text = "환자이름";
            // 
            // txtHPhone
            // 
            this.txtHPhone.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtHPhone.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtHPhone.Location = new System.Drawing.Point(73, 121);
            this.txtHPhone.Name = "txtHPhone";
            this.txtHPhone.Size = new System.Drawing.Size(122, 25);
            this.txtHPhone.TabIndex = 545;
            this.txtHPhone.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtPtno
            // 
            this.txtPtno.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtPtno.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtPtno.Location = new System.Drawing.Point(73, 5);
            this.txtPtno.Name = "txtPtno";
            this.txtPtno.Size = new System.Drawing.Size(78, 25);
            this.txtPtno.TabIndex = 545;
            this.txtPtno.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Window;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 17);
            this.label1.TabIndex = 544;
            this.label1.Text = "등록번호";
            // 
            // txtBuildNo
            // 
            this.txtBuildNo.BackColor = System.Drawing.SystemColors.Info;
            this.txtBuildNo.Location = new System.Drawing.Point(491, 9);
            this.txtBuildNo.Name = "txtBuildNo";
            this.txtBuildNo.Size = new System.Drawing.Size(82, 25);
            this.txtBuildNo.TabIndex = 218;
            this.txtBuildNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBuildNo.Visible = false;
            // 
            // txtJuso1
            // 
            this.txtJuso1.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.txtJuso1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtJuso1.Location = new System.Drawing.Point(73, 38);
            this.txtJuso1.Multiline = true;
            this.txtJuso1.Name = "txtJuso1";
            this.txtJuso1.Size = new System.Drawing.Size(394, 47);
            this.txtJuso1.TabIndex = 215;
            // 
            // btnPost
            // 
            this.btnPost.BackColor = System.Drawing.Color.Transparent;
            this.btnPost.Location = new System.Drawing.Point(424, 5);
            this.btnPost.Name = "btnPost";
            this.btnPost.Size = new System.Drawing.Size(43, 25);
            this.btnPost.TabIndex = 214;
            this.btnPost.Text = "(&H)";
            this.btnPost.UseVisualStyleBackColor = false;
            this.btnPost.Click += new System.EventHandler(this.btnPost_Click);
            // 
            // txtJuso2
            // 
            this.txtJuso2.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.txtJuso2.Location = new System.Drawing.Point(73, 90);
            this.txtJuso2.Name = "txtJuso2";
            this.txtJuso2.Size = new System.Drawing.Size(394, 25);
            this.txtJuso2.TabIndex = 213;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(12, 125);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 17);
            this.label3.TabIndex = 212;
            this.label3.Text = "전화번호";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.Location = new System.Drawing.Point(25, 53);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(34, 17);
            this.label15.TabIndex = 212;
            this.label15.Text = "주소";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.SystemColors.Window;
            this.label14.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.Location = new System.Drawing.Point(303, 9);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(60, 17);
            this.label14.TabIndex = 211;
            this.label14.Text = "우편번호";
            // 
            // frmPatientAddrMng
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 188);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmPatientAddrMng";
            this.Text = "frmPatientAddrMng";
            this.Load += new System.EventHandler(this.frmPatientAddrMng_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtBuildNo;
        private System.Windows.Forms.TextBox txtJuso1;
        private System.Windows.Forms.Button btnPost;
        private System.Windows.Forms.TextBox txtJuso2;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPostCode;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnRegist;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtHPhone;
        private System.Windows.Forms.TextBox txtSNAME;
        private System.Windows.Forms.TextBox txtPtno;
    }
}