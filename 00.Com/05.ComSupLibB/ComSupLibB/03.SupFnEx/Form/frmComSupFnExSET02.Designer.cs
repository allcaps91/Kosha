namespace ComSupLibB.SupFnEx
{
    partial class frmComSupFnExSET02
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panSubTitle01 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.panPAT = new System.Windows.Forms.Panel();
            this.dtpJDate = new System.Windows.Forms.DateTimePicker();
            this.dtpSDate = new System.Windows.Forms.DateTimePicker();
            this.txtPtno = new System.Windows.Forms.TextBox();
            this.txtSAge = new System.Windows.Forms.TextBox();
            this.txtSName = new System.Windows.Forms.TextBox();
            this.txtExName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panRead1 = new System.Windows.Forms.Panel();
            this.txtPanResult = new System.Windows.Forms.TextBox();
            this.panSubTitle02 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panRead2 = new System.Windows.Forms.Panel();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.panSubTitle03 = new System.Windows.Forms.Panel();
            this.btnResultNew = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panTitle.SuspendLayout();
            this.panSubTitle01.SuspendLayout();
            this.panPAT.SuspendLayout();
            this.panRead1.SuspendLayout();
            this.panSubTitle02.SuspendLayout();
            this.panRead2.SuspendLayout();
            this.panSubTitle03.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnSave);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Controls.Add(this.panel2);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(890, 34);
            this.panTitle.TabIndex = 101;
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(741, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(70, 30);
            this.btnSave.TabIndex = 168;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(811, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(70, 30);
            this.btnExit.TabIndex = 30;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(150, 16);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "종검판독결과 입력";
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(881, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 30);
            this.panel2.TabIndex = 167;
            // 
            // panSubTitle01
            // 
            this.panSubTitle01.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panSubTitle01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSubTitle01.Controls.Add(this.label12);
            this.panSubTitle01.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSubTitle01.Location = new System.Drawing.Point(0, 34);
            this.panSubTitle01.Name = "panSubTitle01";
            this.panSubTitle01.Size = new System.Drawing.Size(890, 29);
            this.panSubTitle01.TabIndex = 102;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(7, 5);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(78, 17);
            this.label12.TabIndex = 0;
            this.label12.Text = "수검자 정보";
            // 
            // panPAT
            // 
            this.panPAT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panPAT.Controls.Add(this.dtpJDate);
            this.panPAT.Controls.Add(this.dtpSDate);
            this.panPAT.Controls.Add(this.txtPtno);
            this.panPAT.Controls.Add(this.txtSAge);
            this.panPAT.Controls.Add(this.txtSName);
            this.panPAT.Controls.Add(this.txtExName);
            this.panPAT.Controls.Add(this.label8);
            this.panPAT.Controls.Add(this.label7);
            this.panPAT.Controls.Add(this.label6);
            this.panPAT.Controls.Add(this.label5);
            this.panPAT.Controls.Add(this.label4);
            this.panPAT.Controls.Add(this.label3);
            this.panPAT.Dock = System.Windows.Forms.DockStyle.Top;
            this.panPAT.Location = new System.Drawing.Point(0, 63);
            this.panPAT.Name = "panPAT";
            this.panPAT.Size = new System.Drawing.Size(890, 83);
            this.panPAT.TabIndex = 103;
            // 
            // dtpJDate
            // 
            this.dtpJDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpJDate.Location = new System.Drawing.Point(744, 43);
            this.dtpJDate.Name = "dtpJDate";
            this.dtpJDate.Size = new System.Drawing.Size(108, 25);
            this.dtpJDate.TabIndex = 10;
            // 
            // dtpSDate
            // 
            this.dtpSDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpSDate.Location = new System.Drawing.Point(563, 42);
            this.dtpSDate.Name = "dtpSDate";
            this.dtpSDate.Size = new System.Drawing.Size(104, 25);
            this.dtpSDate.TabIndex = 10;
            // 
            // txtPtno
            // 
            this.txtPtno.Location = new System.Drawing.Point(384, 43);
            this.txtPtno.Name = "txtPtno";
            this.txtPtno.Size = new System.Drawing.Size(102, 25);
            this.txtPtno.TabIndex = 9;
            this.txtPtno.Text = "81000004";
            this.txtPtno.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtSAge
            // 
            this.txtSAge.Location = new System.Drawing.Point(239, 43);
            this.txtSAge.Name = "txtSAge";
            this.txtSAge.Size = new System.Drawing.Size(68, 25);
            this.txtSAge.TabIndex = 8;
            this.txtSAge.Text = "M/999";
            this.txtSAge.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtSName
            // 
            this.txtSName.Location = new System.Drawing.Point(69, 43);
            this.txtSName.Name = "txtSName";
            this.txtSName.Size = new System.Drawing.Size(87, 25);
            this.txtSName.TabIndex = 7;
            this.txtSName.Text = "홍길동전홍";
            this.txtSName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtExName
            // 
            this.txtExName.Location = new System.Drawing.Point(69, 11);
            this.txtExName.Name = "txtExName";
            this.txtExName.Size = new System.Drawing.Size(783, 25);
            this.txtExName.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(678, 47);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 17);
            this.label8.TabIndex = 5;
            this.label8.Text = "수검일자";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(497, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 17);
            this.label7.TabIndex = 4;
            this.label7.Text = "검사일자";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(318, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 17);
            this.label6.TabIndex = 3;
            this.label6.Text = "등록번호";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(167, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 17);
            this.label5.TabIndex = 2;
            this.label5.Text = "나이/성별";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(29, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 17);
            this.label4.TabIndex = 1;
            this.label4.Text = "성명";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(16, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "검사명";
            // 
            // panRead1
            // 
            this.panRead1.Controls.Add(this.txtPanResult);
            this.panRead1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panRead1.Location = new System.Drawing.Point(0, 175);
            this.panRead1.Name = "panRead1";
            this.panRead1.Size = new System.Drawing.Size(890, 320);
            this.panRead1.TabIndex = 105;
            // 
            // txtPanResult
            // 
            this.txtPanResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPanResult.Location = new System.Drawing.Point(0, 0);
            this.txtPanResult.Multiline = true;
            this.txtPanResult.Name = "txtPanResult";
            this.txtPanResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPanResult.Size = new System.Drawing.Size(890, 320);
            this.txtPanResult.TabIndex = 0;
            // 
            // panSubTitle02
            // 
            this.panSubTitle02.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panSubTitle02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSubTitle02.Controls.Add(this.label1);
            this.panSubTitle02.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSubTitle02.Location = new System.Drawing.Point(0, 146);
            this.panSubTitle02.Name = "panSubTitle02";
            this.panSubTitle02.Size = new System.Drawing.Size(890, 29);
            this.panSubTitle02.TabIndex = 104;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(7, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "판독결과";
            // 
            // panRead2
            // 
            this.panRead2.Controls.Add(this.txtResult);
            this.panRead2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panRead2.Location = new System.Drawing.Point(0, 524);
            this.panRead2.Name = "panRead2";
            this.panRead2.Size = new System.Drawing.Size(890, 297);
            this.panRead2.TabIndex = 107;
            // 
            // txtResult
            // 
            this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResult.Location = new System.Drawing.Point(0, 0);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(890, 297);
            this.txtResult.TabIndex = 1;
            // 
            // panSubTitle03
            // 
            this.panSubTitle03.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panSubTitle03.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSubTitle03.Controls.Add(this.btnResultNew);
            this.panSubTitle03.Controls.Add(this.label2);
            this.panSubTitle03.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSubTitle03.Location = new System.Drawing.Point(0, 495);
            this.panSubTitle03.Name = "panSubTitle03";
            this.panSubTitle03.Size = new System.Drawing.Size(890, 29);
            this.panSubTitle03.TabIndex = 106;
            // 
            // btnResultNew
            // 
            this.btnResultNew.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnResultNew.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnResultNew.Location = new System.Drawing.Point(707, 0);
            this.btnResultNew.Name = "btnResultNew";
            this.btnResultNew.Size = new System.Drawing.Size(181, 27);
            this.btnResultNew.TabIndex = 31;
            this.btnResultNew.Text = "상용문구 불러오기";
            this.btnResultNew.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(7, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "종검결과";
            // 
            // frmComSupFnExSET02
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 821);
            this.Controls.Add(this.panRead2);
            this.Controls.Add(this.panSubTitle03);
            this.Controls.Add(this.panRead1);
            this.Controls.Add(this.panSubTitle02);
            this.Controls.Add(this.panPAT);
            this.Controls.Add(this.panSubTitle01);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "frmComSupFnExSET02";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmComSupFnExSET02";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panSubTitle01.ResumeLayout(false);
            this.panSubTitle01.PerformLayout();
            this.panPAT.ResumeLayout(false);
            this.panPAT.PerformLayout();
            this.panRead1.ResumeLayout(false);
            this.panRead1.PerformLayout();
            this.panSubTitle02.ResumeLayout(false);
            this.panSubTitle02.PerformLayout();
            this.panRead2.ResumeLayout(false);
            this.panRead2.PerformLayout();
            this.panSubTitle03.ResumeLayout(false);
            this.panSubTitle03.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panSubTitle01;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel panPAT;
        private System.Windows.Forms.Panel panRead1;
        private System.Windows.Forms.Panel panSubTitle02;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panRead2;
        private System.Windows.Forms.Panel panSubTitle03;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpJDate;
        private System.Windows.Forms.DateTimePicker dtpSDate;
        private System.Windows.Forms.TextBox txtPtno;
        private System.Windows.Forms.TextBox txtSAge;
        private System.Windows.Forms.TextBox txtSName;
        private System.Windows.Forms.TextBox txtExName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPanResult;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Button btnResultNew;
    }
}