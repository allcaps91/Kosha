namespace ComEmrBase
{
    partial class frmCertTest_PhohangSM
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
            this.label9 = new System.Windows.Forms.Label();
            this.IDC_EDIT_ID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.IDC_EDIT_CERTPASS = new System.Windows.Forms.TextBox();
            this.APIINIT = new System.Windows.Forms.Button();
            this.IDC_BUTTON_ROAMING2 = new System.Windows.Forms.Button();
            this.IDC_BUTTON_ROAMING = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSABUN = new System.Windows.Forms.TextBox();
            this.IDC_EDIT_EMR_DATA_OUTPUT = new System.Windows.Forms.TextBox();
            this.IDC_EDIT_EMR_DATA = new System.Windows.Forms.TextBox();
            this.IDC_EDIT_EMR_SIGNED_DATA2 = new System.Windows.Forms.TextBox();
            this.IDC_BUTTON_HASHSIGN = new System.Windows.Forms.Button();
            this.APIRELEASE = new System.Windows.Forms.Button();
            this.btnCertAll = new System.Windows.Forms.Button();
            this.btnSearchSabun = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(380, 69);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(78, 17);
            this.label9.TabIndex = 4;
            this.label9.Text = "인증서 암호";
            // 
            // IDC_EDIT_ID
            // 
            this.IDC_EDIT_ID.Location = new System.Drawing.Point(257, 65);
            this.IDC_EDIT_ID.Name = "IDC_EDIT_ID";
            this.IDC_EDIT_ID.Size = new System.Drawing.Size(117, 25);
            this.IDC_EDIT_ID.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(185, 69);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 17);
            this.label6.TabIndex = 3;
            this.label6.Text = "식별 번호";
            // 
            // IDC_EDIT_CERTPASS
            // 
            this.IDC_EDIT_CERTPASS.Location = new System.Drawing.Point(465, 65);
            this.IDC_EDIT_CERTPASS.Name = "IDC_EDIT_CERTPASS";
            this.IDC_EDIT_CERTPASS.Size = new System.Drawing.Size(117, 25);
            this.IDC_EDIT_CERTPASS.TabIndex = 6;
            // 
            // APIINIT
            // 
            this.APIINIT.Location = new System.Drawing.Point(20, 12);
            this.APIINIT.Name = "APIINIT";
            this.APIINIT.Size = new System.Drawing.Size(131, 26);
            this.APIINIT.TabIndex = 7;
            this.APIINIT.Text = "API 초기화";
            this.APIINIT.UseVisualStyleBackColor = true;
            this.APIINIT.Click += new System.EventHandler(this.APIINIT_Click);
            // 
            // IDC_BUTTON_ROAMING2
            // 
            this.IDC_BUTTON_ROAMING2.Location = new System.Drawing.Point(20, 82);
            this.IDC_BUTTON_ROAMING2.Name = "IDC_BUTTON_ROAMING2";
            this.IDC_BUTTON_ROAMING2.Size = new System.Drawing.Size(131, 26);
            this.IDC_BUTTON_ROAMING2.TabIndex = 9;
            this.IDC_BUTTON_ROAMING2.Text = "인증서 로그인(창X)";
            this.IDC_BUTTON_ROAMING2.UseVisualStyleBackColor = true;
            this.IDC_BUTTON_ROAMING2.Click += new System.EventHandler(this.IDC_BUTTON_ROAMING2_Click);
            // 
            // IDC_BUTTON_ROAMING
            // 
            this.IDC_BUTTON_ROAMING.Location = new System.Drawing.Point(20, 54);
            this.IDC_BUTTON_ROAMING.Name = "IDC_BUTTON_ROAMING";
            this.IDC_BUTTON_ROAMING.Size = new System.Drawing.Size(131, 26);
            this.IDC_BUTTON_ROAMING.TabIndex = 8;
            this.IDC_BUTTON_ROAMING.Text = "인증서 로그인";
            this.IDC_BUTTON_ROAMING.UseVisualStyleBackColor = true;
            this.IDC_BUTTON_ROAMING.Click += new System.EventHandler(this.IDC_BUTTON_ROAMING_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(172, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 17);
            this.label1.TabIndex = 10;
            this.label1.Text = "사용자 ID";
            // 
            // txtSABUN
            // 
            this.txtSABUN.Location = new System.Drawing.Point(257, 24);
            this.txtSABUN.Name = "txtSABUN";
            this.txtSABUN.Size = new System.Drawing.Size(117, 25);
            this.txtSABUN.TabIndex = 11;
            this.txtSABUN.Text = "29519";
            // 
            // IDC_EDIT_EMR_DATA_OUTPUT
            // 
            this.IDC_EDIT_EMR_DATA_OUTPUT.Location = new System.Drawing.Point(442, 113);
            this.IDC_EDIT_EMR_DATA_OUTPUT.Multiline = true;
            this.IDC_EDIT_EMR_DATA_OUTPUT.Name = "IDC_EDIT_EMR_DATA_OUTPUT";
            this.IDC_EDIT_EMR_DATA_OUTPUT.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.IDC_EDIT_EMR_DATA_OUTPUT.Size = new System.Drawing.Size(252, 59);
            this.IDC_EDIT_EMR_DATA_OUTPUT.TabIndex = 12;
            // 
            // IDC_EDIT_EMR_DATA
            // 
            this.IDC_EDIT_EMR_DATA.Location = new System.Drawing.Point(175, 113);
            this.IDC_EDIT_EMR_DATA.Multiline = true;
            this.IDC_EDIT_EMR_DATA.Name = "IDC_EDIT_EMR_DATA";
            this.IDC_EDIT_EMR_DATA.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.IDC_EDIT_EMR_DATA.Size = new System.Drawing.Size(252, 59);
            this.IDC_EDIT_EMR_DATA.TabIndex = 13;
            this.IDC_EDIT_EMR_DATA.Tag = "";
            this.IDC_EDIT_EMR_DATA.Text = "전자서명 할 처방데이터입니다.";
            // 
            // IDC_EDIT_EMR_SIGNED_DATA2
            // 
            this.IDC_EDIT_EMR_SIGNED_DATA2.Location = new System.Drawing.Point(175, 286);
            this.IDC_EDIT_EMR_SIGNED_DATA2.Multiline = true;
            this.IDC_EDIT_EMR_SIGNED_DATA2.Name = "IDC_EDIT_EMR_SIGNED_DATA2";
            this.IDC_EDIT_EMR_SIGNED_DATA2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.IDC_EDIT_EMR_SIGNED_DATA2.Size = new System.Drawing.Size(252, 59);
            this.IDC_EDIT_EMR_SIGNED_DATA2.TabIndex = 14;
            // 
            // IDC_BUTTON_HASHSIGN
            // 
            this.IDC_BUTTON_HASHSIGN.Location = new System.Drawing.Point(442, 287);
            this.IDC_BUTTON_HASHSIGN.Name = "IDC_BUTTON_HASHSIGN";
            this.IDC_BUTTON_HASHSIGN.Size = new System.Drawing.Size(97, 21);
            this.IDC_BUTTON_HASHSIGN.TabIndex = 15;
            this.IDC_BUTTON_HASHSIGN.Text = "Hash Sign";
            this.IDC_BUTTON_HASHSIGN.UseVisualStyleBackColor = true;
            // 
            // APIRELEASE
            // 
            this.APIRELEASE.Location = new System.Drawing.Point(20, 502);
            this.APIRELEASE.Name = "APIRELEASE";
            this.APIRELEASE.Size = new System.Drawing.Size(131, 26);
            this.APIRELEASE.TabIndex = 16;
            this.APIRELEASE.Text = "API 자원 해제";
            this.APIRELEASE.UseVisualStyleBackColor = true;
            this.APIRELEASE.Click += new System.EventHandler(this.APIRELEASE_Click);
            // 
            // btnCertAll
            // 
            this.btnCertAll.Location = new System.Drawing.Point(465, 349);
            this.btnCertAll.Name = "btnCertAll";
            this.btnCertAll.Size = new System.Drawing.Size(184, 106);
            this.btnCertAll.TabIndex = 17;
            this.btnCertAll.Text = "전자인증 All";
            this.btnCertAll.UseVisualStyleBackColor = true;
            this.btnCertAll.Click += new System.EventHandler(this.btnCertAll_Click);
            // 
            // btnSearchSabun
            // 
            this.btnSearchSabun.Location = new System.Drawing.Point(408, 22);
            this.btnSearchSabun.Name = "btnSearchSabun";
            this.btnSearchSabun.Size = new System.Drawing.Size(131, 26);
            this.btnSearchSabun.TabIndex = 18;
            this.btnSearchSabun.Text = "사용자 정보";
            this.btnSearchSabun.UseVisualStyleBackColor = true;
            this.btnSearchSabun.Click += new System.EventHandler(this.btnSearchSabun_Click);
            // 
            // frmCertTest_PhohangSM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 669);
            this.Controls.Add(this.btnSearchSabun);
            this.Controls.Add(this.btnCertAll);
            this.Controls.Add(this.APIRELEASE);
            this.Controls.Add(this.IDC_EDIT_EMR_SIGNED_DATA2);
            this.Controls.Add(this.IDC_BUTTON_HASHSIGN);
            this.Controls.Add(this.IDC_EDIT_EMR_DATA_OUTPUT);
            this.Controls.Add(this.IDC_EDIT_EMR_DATA);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSABUN);
            this.Controls.Add(this.IDC_BUTTON_ROAMING2);
            this.Controls.Add(this.IDC_BUTTON_ROAMING);
            this.Controls.Add(this.APIINIT);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.IDC_EDIT_ID);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.IDC_EDIT_CERTPASS);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmCertTest_PhohangSM";
            this.Text = "frmCertTest_PhohangSM";
            this.Load += new System.EventHandler(this.frmCertTest_PhohangSM_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox IDC_EDIT_ID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox IDC_EDIT_CERTPASS;
        private System.Windows.Forms.Button APIINIT;
        private System.Windows.Forms.Button IDC_BUTTON_ROAMING2;
        private System.Windows.Forms.Button IDC_BUTTON_ROAMING;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSABUN;
        private System.Windows.Forms.TextBox IDC_EDIT_EMR_DATA_OUTPUT;
        private System.Windows.Forms.TextBox IDC_EDIT_EMR_DATA;
        private System.Windows.Forms.TextBox IDC_EDIT_EMR_SIGNED_DATA2;
        private System.Windows.Forms.Button IDC_BUTTON_HASHSIGN;
        private System.Windows.Forms.Button APIRELEASE;
        private System.Windows.Forms.Button btnCertAll;
        private System.Windows.Forms.Button btnSearchSabun;
    }
}