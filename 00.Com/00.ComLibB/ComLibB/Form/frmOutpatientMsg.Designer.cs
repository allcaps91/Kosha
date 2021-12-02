namespace ComLibB
{
    partial class frmOutpatientMsg
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
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.grb1 = new System.Windows.Forms.GroupBox();
            this.txtSendTime = new System.Windows.Forms.TextBox();
            this.txt2 = new System.Windows.Forms.TextBox();
            this.txt1 = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grb2 = new System.Windows.Forms.GroupBox();
            this.btnMonth12 = new System.Windows.Forms.Button();
            this.btnMonth11 = new System.Windows.Forms.Button();
            this.btnMonth10 = new System.Windows.Forms.Button();
            this.btnMonth09 = new System.Windows.Forms.Button();
            this.btnMonth08 = new System.Windows.Forms.Button();
            this.btnMonth07 = new System.Windows.Forms.Button();
            this.btnMonth06 = new System.Windows.Forms.Button();
            this.btnMonth05 = new System.Windows.Forms.Button();
            this.btnMonth04 = new System.Windows.Forms.Button();
            this.btnMonth03 = new System.Windows.Forms.Button();
            this.btnMonth02 = new System.Windows.Forms.Button();
            this.btnMonth01 = new System.Windows.Forms.Button();
            this.cboYYYY = new System.Windows.Forms.ComboBox();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.grb1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.grb2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(701, 34);
            this.panTitle.TabIndex = 12;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(622, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(156, 16);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "외래 안부문자 예약";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.btnDel);
            this.panel1.Controls.Add(this.grb1);
            this.panel1.Controls.Add(this.txt2);
            this.panel1.Controls.Add(this.txt1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(701, 60);
            this.panel1.TabIndex = 13;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Location = new System.Drawing.Point(622, 11);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 41);
            this.btnSearch.TabIndex = 28;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnDel
            // 
            this.btnDel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDel.BackColor = System.Drawing.Color.Transparent;
            this.btnDel.Location = new System.Drawing.Point(544, 11);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(72, 41);
            this.btnDel.TabIndex = 24;
            this.btnDel.Text = "예약취소";
            this.btnDel.UseVisualStyleBackColor = false;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // grb1
            // 
            this.grb1.Controls.Add(this.txtSendTime);
            this.grb1.Location = new System.Drawing.Point(310, 4);
            this.grb1.Name = "grb1";
            this.grb1.Size = new System.Drawing.Size(228, 49);
            this.grb1.TabIndex = 1;
            this.grb1.TabStop = false;
            this.grb1.Text = "전송 예약 시간";
            // 
            // txtSendTime
            // 
            this.txtSendTime.Location = new System.Drawing.Point(6, 20);
            this.txtSendTime.Name = "txtSendTime";
            this.txtSendTime.Size = new System.Drawing.Size(216, 21);
            this.txtSendTime.TabIndex = 32;
            // 
            // txt2
            // 
            this.txt2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt2.ForeColor = System.Drawing.Color.Red;
            this.txt2.Location = new System.Drawing.Point(3, 31);
            this.txt2.Name = "txt2";
            this.txt2.Size = new System.Drawing.Size(301, 21);
            this.txt2.TabIndex = 0;
            this.txt2.Text = "월에 붉은색 글씨[예약 전송 월]";
            this.txt2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txt1
            // 
            this.txt1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt1.ForeColor = System.Drawing.Color.Blue;
            this.txt1.Location = new System.Drawing.Point(3, 4);
            this.txt1.Name = "txt1";
            this.txt1.Size = new System.Drawing.Size(301, 21);
            this.txt1.TabIndex = 0;
            this.txt1.Text = "전송년월을 선택하면 기존 예약일이 자동 갱신됨";
            this.txt1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.grb2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 94);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(701, 67);
            this.panel2.TabIndex = 14;
            // 
            // grb2
            // 
            this.grb2.Controls.Add(this.btnMonth12);
            this.grb2.Controls.Add(this.btnMonth11);
            this.grb2.Controls.Add(this.btnMonth10);
            this.grb2.Controls.Add(this.btnMonth09);
            this.grb2.Controls.Add(this.btnMonth08);
            this.grb2.Controls.Add(this.btnMonth07);
            this.grb2.Controls.Add(this.btnMonth06);
            this.grb2.Controls.Add(this.btnMonth05);
            this.grb2.Controls.Add(this.btnMonth04);
            this.grb2.Controls.Add(this.btnMonth03);
            this.grb2.Controls.Add(this.btnMonth02);
            this.grb2.Controls.Add(this.btnMonth01);
            this.grb2.Controls.Add(this.cboYYYY);
            this.grb2.Location = new System.Drawing.Point(3, 4);
            this.grb2.Name = "grb2";
            this.grb2.Size = new System.Drawing.Size(691, 58);
            this.grb2.TabIndex = 0;
            this.grb2.TabStop = false;
            this.grb2.Text = "전송년월 선택";
            // 
            // btnMonth12
            // 
            this.btnMonth12.Location = new System.Drawing.Point(642, 14);
            this.btnMonth12.Name = "btnMonth12";
            this.btnMonth12.Size = new System.Drawing.Size(40, 30);
            this.btnMonth12.TabIndex = 2;
            this.btnMonth12.Text = "12월";
            this.btnMonth12.UseVisualStyleBackColor = true;
            this.btnMonth12.Click += new System.EventHandler(this.btnMonth_Click);
            // 
            // btnMonth11
            // 
            this.btnMonth11.Location = new System.Drawing.Point(596, 14);
            this.btnMonth11.Name = "btnMonth11";
            this.btnMonth11.Size = new System.Drawing.Size(40, 30);
            this.btnMonth11.TabIndex = 2;
            this.btnMonth11.Text = "11월";
            this.btnMonth11.UseVisualStyleBackColor = true;
            this.btnMonth11.Click += new System.EventHandler(this.btnMonth_Click);
            // 
            // btnMonth10
            // 
            this.btnMonth10.Location = new System.Drawing.Point(550, 14);
            this.btnMonth10.Name = "btnMonth10";
            this.btnMonth10.Size = new System.Drawing.Size(40, 30);
            this.btnMonth10.TabIndex = 2;
            this.btnMonth10.Text = "10월";
            this.btnMonth10.UseVisualStyleBackColor = true;
            this.btnMonth10.Click += new System.EventHandler(this.btnMonth_Click);
            // 
            // btnMonth09
            // 
            this.btnMonth09.Location = new System.Drawing.Point(504, 14);
            this.btnMonth09.Name = "btnMonth09";
            this.btnMonth09.Size = new System.Drawing.Size(40, 30);
            this.btnMonth09.TabIndex = 2;
            this.btnMonth09.Text = "9월";
            this.btnMonth09.UseVisualStyleBackColor = true;
            this.btnMonth09.Click += new System.EventHandler(this.btnMonth_Click);
            // 
            // btnMonth08
            // 
            this.btnMonth08.Location = new System.Drawing.Point(458, 14);
            this.btnMonth08.Name = "btnMonth08";
            this.btnMonth08.Size = new System.Drawing.Size(40, 30);
            this.btnMonth08.TabIndex = 2;
            this.btnMonth08.Text = "8월";
            this.btnMonth08.UseVisualStyleBackColor = true;
            this.btnMonth08.Click += new System.EventHandler(this.btnMonth_Click);
            // 
            // btnMonth07
            // 
            this.btnMonth07.Location = new System.Drawing.Point(412, 14);
            this.btnMonth07.Name = "btnMonth07";
            this.btnMonth07.Size = new System.Drawing.Size(40, 30);
            this.btnMonth07.TabIndex = 2;
            this.btnMonth07.Text = "7월";
            this.btnMonth07.UseVisualStyleBackColor = true;
            this.btnMonth07.Click += new System.EventHandler(this.btnMonth_Click);
            // 
            // btnMonth06
            // 
            this.btnMonth06.Location = new System.Drawing.Point(366, 14);
            this.btnMonth06.Name = "btnMonth06";
            this.btnMonth06.Size = new System.Drawing.Size(40, 30);
            this.btnMonth06.TabIndex = 2;
            this.btnMonth06.Text = "6월";
            this.btnMonth06.UseVisualStyleBackColor = true;
            this.btnMonth06.Click += new System.EventHandler(this.btnMonth_Click);
            // 
            // btnMonth05
            // 
            this.btnMonth05.Location = new System.Drawing.Point(320, 14);
            this.btnMonth05.Name = "btnMonth05";
            this.btnMonth05.Size = new System.Drawing.Size(40, 30);
            this.btnMonth05.TabIndex = 2;
            this.btnMonth05.Text = "5월";
            this.btnMonth05.UseVisualStyleBackColor = true;
            this.btnMonth05.Click += new System.EventHandler(this.btnMonth_Click);
            // 
            // btnMonth04
            // 
            this.btnMonth04.Location = new System.Drawing.Point(274, 14);
            this.btnMonth04.Name = "btnMonth04";
            this.btnMonth04.Size = new System.Drawing.Size(40, 30);
            this.btnMonth04.TabIndex = 2;
            this.btnMonth04.Text = "4월";
            this.btnMonth04.UseVisualStyleBackColor = true;
            this.btnMonth04.Click += new System.EventHandler(this.btnMonth_Click);
            // 
            // btnMonth03
            // 
            this.btnMonth03.Location = new System.Drawing.Point(228, 14);
            this.btnMonth03.Name = "btnMonth03";
            this.btnMonth03.Size = new System.Drawing.Size(40, 30);
            this.btnMonth03.TabIndex = 2;
            this.btnMonth03.Text = "3월";
            this.btnMonth03.UseVisualStyleBackColor = true;
            this.btnMonth03.Click += new System.EventHandler(this.btnMonth_Click);
            // 
            // btnMonth02
            // 
            this.btnMonth02.Location = new System.Drawing.Point(182, 14);
            this.btnMonth02.Name = "btnMonth02";
            this.btnMonth02.Size = new System.Drawing.Size(40, 30);
            this.btnMonth02.TabIndex = 2;
            this.btnMonth02.Text = "2월";
            this.btnMonth02.UseVisualStyleBackColor = true;
            this.btnMonth02.Click += new System.EventHandler(this.btnMonth_Click);
            // 
            // btnMonth01
            // 
            this.btnMonth01.Location = new System.Drawing.Point(136, 14);
            this.btnMonth01.Name = "btnMonth01";
            this.btnMonth01.Size = new System.Drawing.Size(40, 30);
            this.btnMonth01.TabIndex = 2;
            this.btnMonth01.Text = "1월";
            this.btnMonth01.UseVisualStyleBackColor = true;
            this.btnMonth01.Click += new System.EventHandler(this.btnMonth_Click);
            // 
            // cboYYYY
            // 
            this.cboYYYY.FormattingEnabled = true;
            this.cboYYYY.Location = new System.Drawing.Point(9, 20);
            this.cboYYYY.Name = "cboYYYY";
            this.cboYYYY.Size = new System.Drawing.Size(121, 20);
            this.cboYYYY.TabIndex = 1;
            this.cboYYYY.Click += new System.EventHandler(this.cboYYYY_Click);
            // 
            // frmOutpatientMsg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 161);
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Name = "frmOutpatientMsg";
            this.Text = "외래 안부문자 예약";
            this.Load += new System.EventHandler(this.frmOutpatientMsg_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.grb1.ResumeLayout(false);
            this.grb1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.grb2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox txt2;
        private System.Windows.Forms.TextBox txt1;
        private System.Windows.Forms.GroupBox grb1;
        private System.Windows.Forms.TextBox txtSendTime;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox grb2;
        private System.Windows.Forms.Button btnMonth12;
        private System.Windows.Forms.Button btnMonth11;
        private System.Windows.Forms.Button btnMonth10;
        private System.Windows.Forms.Button btnMonth09;
        private System.Windows.Forms.Button btnMonth08;
        private System.Windows.Forms.Button btnMonth07;
        private System.Windows.Forms.Button btnMonth06;
        private System.Windows.Forms.Button btnMonth05;
        private System.Windows.Forms.Button btnMonth04;
        private System.Windows.Forms.Button btnMonth03;
        private System.Windows.Forms.Button btnMonth02;
        private System.Windows.Forms.Button btnMonth01;
        private System.Windows.Forms.ComboBox cboYYYY;
    }
}