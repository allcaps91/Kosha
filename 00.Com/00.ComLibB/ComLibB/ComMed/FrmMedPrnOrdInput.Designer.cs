namespace ComLibB
{
    partial class FrmMedPrnOrdInput
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblPinfo2 = new System.Windows.Forms.Label();
            this.lblPInfo = new System.Windows.Forms.Label();
            this.cboSub = new System.Windows.Forms.ComboBox();
            this.txtS2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtV1 = new System.Windows.Forms.TextBox();
            this.cboDosCode = new System.Windows.Forms.ComboBox();
            this.txtV2 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtV3 = new System.Windows.Forms.TextBox();
            this.lblV3 = new System.Windows.Forms.Label();
            this.cboNoti = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtV4 = new System.Windows.Forms.TextBox();
            this.txtS1 = new System.Windows.Forms.TextBox();
            this.lblV1 = new System.Windows.Forms.Label();
            this.lblV2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtV2_Min = new System.Windows.Forms.TextBox();
            this.txtV3_Max = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(3, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(287, 21);
            this.lblTitle.TabIndex = 5;
            this.lblTitle.Text = "필요시 처방(PRN) 처방 상세내용 입력";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(518, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 30);
            this.btnCancel.TabIndex = 27;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.BackColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(440, 9);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(72, 30);
            this.btnOK.TabIndex = 28;
            this.btnOK.Text = "확인";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblPinfo2
            // 
            this.lblPinfo2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblPinfo2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPinfo2.Location = new System.Drawing.Point(7, 47);
            this.lblPinfo2.Name = "lblPinfo2";
            this.lblPinfo2.Size = new System.Drawing.Size(103, 40);
            this.lblPinfo2.TabIndex = 29;
            this.lblPinfo2.Text = "일반약";
            this.lblPinfo2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPInfo
            // 
            this.lblPInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblPInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPInfo.Location = new System.Drawing.Point(114, 47);
            this.lblPInfo.Name = "lblPInfo";
            this.lblPInfo.Size = new System.Drawing.Size(473, 40);
            this.lblPInfo.TabIndex = 30;
            this.lblPInfo.Text = "[ 약품명 : 가딘주 ] [ 의약품 코드 : GADINA ]";
            this.lblPInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboSub
            // 
            this.cboSub.FormattingEnabled = true;
            this.cboSub.Location = new System.Drawing.Point(114, 96);
            this.cboSub.Name = "cboSub";
            this.cboSub.Size = new System.Drawing.Size(473, 20);
            this.cboSub.TabIndex = 34;
            this.cboSub.Visible = false;
            this.cboSub.SelectedIndexChanged += new System.EventHandler(this.cboSub_SelectedIndexChanged);
            // 
            // txtS2
            // 
            this.txtS2.Location = new System.Drawing.Point(114, 124);
            this.txtS2.Name = "txtS2";
            this.txtS2.Size = new System.Drawing.Size(473, 21);
            this.txtS2.TabIndex = 35;
            this.txtS2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtS2_KeyPress);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.RoyalBlue;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Font = new System.Drawing.Font("굴림체", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(7, 181);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 24);
            this.label3.TabIndex = 36;
            this.label3.Text = "일투량(1회용량)";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.RoyalBlue;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Font = new System.Drawing.Font("굴림체", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(149, 181);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(141, 24);
            this.label4.TabIndex = 37;
            this.label4.Text = "용법";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.RoyalBlue;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Font = new System.Drawing.Font("굴림체", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(292, 181);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(139, 24);
            this.label5.TabIndex = 38;
            this.label5.Text = "최소투여간격";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.RoyalBlue;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Font = new System.Drawing.Font("굴림체", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(433, 181);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(153, 24);
            this.label6.TabIndex = 39;
            this.label6.Text = "최대투여횟수";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtV1
            // 
            this.txtV1.Location = new System.Drawing.Point(7, 208);
            this.txtV1.Name = "txtV1";
            this.txtV1.Size = new System.Drawing.Size(140, 21);
            this.txtV1.TabIndex = 40;
            this.txtV1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtV1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtV1_KeyPress);
            // 
            // cboDosCode
            // 
            this.cboDosCode.FormattingEnabled = true;
            this.cboDosCode.Location = new System.Drawing.Point(149, 208);
            this.cboDosCode.Name = "cboDosCode";
            this.cboDosCode.Size = new System.Drawing.Size(141, 20);
            this.cboDosCode.TabIndex = 41;
            this.cboDosCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboDosCode_KeyPress);
            // 
            // txtV2
            // 
            this.txtV2.Location = new System.Drawing.Point(292, 207);
            this.txtV2.Name = "txtV2";
            this.txtV2.Size = new System.Drawing.Size(104, 21);
            this.txtV2.TabIndex = 42;
            this.txtV2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtV2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtV2_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(402, 211);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 43;
            this.label7.Text = "시간";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(541, 212);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 12);
            this.label8.TabIndex = 45;
            this.label8.Text = "회 까지";
            // 
            // txtV3
            // 
            this.txtV3.Location = new System.Drawing.Point(435, 207);
            this.txtV3.Name = "txtV3";
            this.txtV3.Size = new System.Drawing.Size(100, 21);
            this.txtV3.TabIndex = 44;
            this.txtV3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtV3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtV3_KeyPress);
            // 
            // lblV3
            // 
            this.lblV3.BackColor = System.Drawing.Color.RoyalBlue;
            this.lblV3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblV3.Font = new System.Drawing.Font("굴림체", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblV3.ForeColor = System.Drawing.Color.White;
            this.lblV3.Location = new System.Drawing.Point(7, 232);
            this.lblV3.Name = "lblV3";
            this.lblV3.Size = new System.Drawing.Size(580, 24);
            this.lblV3.TabIndex = 46;
            this.lblV3.Text = "Notifying 시기 (다음 중 선택후 내용입력하십시오)";
            this.lblV3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboNoti
            // 
            this.cboNoti.FormattingEnabled = true;
            this.cboNoti.Location = new System.Drawing.Point(7, 259);
            this.cboNoti.Name = "cboNoti";
            this.cboNoti.Size = new System.Drawing.Size(580, 20);
            this.cboNoti.TabIndex = 47;
            this.cboNoti.SelectedIndexChanged += new System.EventHandler(this.cboNoti_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Font = new System.Drawing.Font("굴림체", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(114, 150);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(473, 24);
            this.label11.TabIndex = 49;
            this.label11.Text = "실시기준(공통)은 수정 가능하며 참고 및 특이사항 입력하세요";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtV4
            // 
            this.txtV4.BackColor = System.Drawing.Color.PaleGreen;
            this.txtV4.Location = new System.Drawing.Point(7, 285);
            this.txtV4.Name = "txtV4";
            this.txtV4.Size = new System.Drawing.Size(580, 21);
            this.txtV4.TabIndex = 50;
            this.txtV4.Text = "Notifying 시기를 선택하거나 프리타이핑 하십시오 !!";
            // 
            // txtS1
            // 
            this.txtS1.Location = new System.Drawing.Point(115, 96);
            this.txtS1.Name = "txtS1";
            this.txtS1.Size = new System.Drawing.Size(471, 21);
            this.txtS1.TabIndex = 51;
            // 
            // lblV1
            // 
            this.lblV1.BackColor = System.Drawing.Color.RoyalBlue;
            this.lblV1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblV1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblV1.ForeColor = System.Drawing.Color.White;
            this.lblV1.Location = new System.Drawing.Point(7, 94);
            this.lblV1.Name = "lblV1";
            this.lblV1.Size = new System.Drawing.Size(103, 22);
            this.lblV1.TabIndex = 52;
            this.lblV1.Text = "적응증";
            this.lblV1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblV2
            // 
            this.lblV2.BackColor = System.Drawing.Color.RoyalBlue;
            this.lblV2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblV2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblV2.ForeColor = System.Drawing.Color.White;
            this.lblV2.Location = new System.Drawing.Point(7, 123);
            this.lblV2.Name = "lblV2";
            this.lblV2.Size = new System.Drawing.Size(103, 51);
            this.lblV2.TabIndex = 53;
            this.lblV2.Text = "실시기준(공통)";
            this.lblV2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(64, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 55;
            this.label1.Text = "시간";
            // 
            // txtV2_Min
            // 
            this.txtV2_Min.Location = new System.Drawing.Point(8, 29);
            this.txtV2_Min.Name = "txtV2_Min";
            this.txtV2_Min.Size = new System.Drawing.Size(56, 21);
            this.txtV2_Min.TabIndex = 54;
            this.txtV2_Min.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtV3_Max
            // 
            this.txtV3_Max.Location = new System.Drawing.Point(106, 29);
            this.txtV3_Max.Name = "txtV3_Max";
            this.txtV3_Max.Size = new System.Drawing.Size(56, 21);
            this.txtV3_Max.TabIndex = 44;
            this.txtV3_Max.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(162, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 12);
            this.label2.TabIndex = 45;
            this.label2.Text = "회 까지";
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.RoyalBlue;
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Font = new System.Drawing.Font("굴림체", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(3, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(100, 24);
            this.label9.TabIndex = 38;
            this.label9.Text = "최소투여간격";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.RoyalBlue;
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Font = new System.Drawing.Font("굴림체", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(105, 3);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 24);
            this.label10.TabIndex = 39;
            this.label10.Text = "최대투여횟수";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.txtV2_Min);
            this.panel1.Controls.Add(this.txtV3_Max);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(797, 147);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(214, 58);
            this.panel1.TabIndex = 56;
            this.panel1.Visible = false;
            // 
            // FrmMedPrnOrdInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(596, 315);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblV2);
            this.Controls.Add(this.lblV1);
            this.Controls.Add(this.txtS1);
            this.Controls.Add(this.txtV4);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.cboNoti);
            this.Controls.Add(this.lblV3);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtV3);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtV2);
            this.Controls.Add(this.cboDosCode);
            this.Controls.Add(this.txtV1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtS2);
            this.Controls.Add(this.cboSub);
            this.Controls.Add(this.lblPInfo);
            this.Controls.Add(this.lblPinfo2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblTitle);
            this.Name = "FrmMedPrnOrdInput";
            this.Text = "필요시 처방(PRN) 처방 상세내용 입력";
            this.Load += new System.EventHandler(this.FrmMedPrnOrdInput_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblPinfo2;
        private System.Windows.Forms.Label lblPInfo;
        private System.Windows.Forms.ComboBox cboSub;
        private System.Windows.Forms.TextBox txtS2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtV1;
        private System.Windows.Forms.ComboBox cboDosCode;
        private System.Windows.Forms.TextBox txtV2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtV3;
        private System.Windows.Forms.Label lblV3;
        private System.Windows.Forms.ComboBox cboNoti;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtV4;
        private System.Windows.Forms.TextBox txtS1;
        private System.Windows.Forms.Label lblV1;
        private System.Windows.Forms.Label lblV2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtV2_Min;
        private System.Windows.Forms.TextBox txtV3_Max;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panel1;
    }
}