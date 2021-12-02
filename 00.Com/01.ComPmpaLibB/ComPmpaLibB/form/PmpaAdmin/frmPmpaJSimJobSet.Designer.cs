namespace ComPmpaLibB
{
    partial class frmPmpaJSimJobSet
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
            this.pnlHead = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rdbColor2 = new System.Windows.Forms.RadioButton();
            this.rdbColor1 = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdbMy2 = new System.Windows.Forms.RadioButton();
            this.rdbMy1 = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdbNext2 = new System.Windows.Forms.RadioButton();
            this.rdbNext1 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdbDisp2 = new System.Windows.Forms.RadioButton();
            this.rdbDisp1 = new System.Windows.Forms.RadioButton();
            this.pnlHead.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHead
            // 
            this.pnlHead.BackColor = System.Drawing.Color.RoyalBlue;
            this.pnlHead.Controls.Add(this.label7);
            this.pnlHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHead.ForeColor = System.Drawing.Color.White;
            this.pnlHead.Location = new System.Drawing.Point(0, 0);
            this.pnlHead.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlHead.Name = "pnlHead";
            this.pnlHead.Size = new System.Drawing.Size(405, 38);
            this.pnlHead.TabIndex = 178;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(14, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "재원심사 환경설정";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 38);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3);
            this.panel1.Size = new System.Drawing.Size(405, 37);
            this.panel1.TabIndex = 179;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.SystemColors.Window;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(232, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(85, 31);
            this.btnSave.TabIndex = 152;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.SystemColors.Window;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(317, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(85, 31);
            this.btnExit.TabIndex = 151;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox4);
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 75);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(10);
            this.panel2.Size = new System.Drawing.Size(405, 251);
            this.panel2.TabIndex = 180;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rdbColor2);
            this.groupBox4.Controls.Add(this.rdbColor1);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.Location = new System.Drawing.Point(10, 178);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(385, 56);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "수가별 색상 표시";
            // 
            // rdbColor2
            // 
            this.rdbColor2.AutoSize = true;
            this.rdbColor2.Checked = true;
            this.rdbColor2.Location = new System.Drawing.Point(193, 22);
            this.rdbColor2.Name = "rdbColor2";
            this.rdbColor2.Size = new System.Drawing.Size(73, 19);
            this.rdbColor2.TabIndex = 1;
            this.rdbColor2.TabStop = true;
            this.rdbColor2.Text = "표시않함";
            this.rdbColor2.UseVisualStyleBackColor = true;
            // 
            // rdbColor1
            // 
            this.rdbColor1.AutoSize = true;
            this.rdbColor1.Location = new System.Drawing.Point(51, 22);
            this.rdbColor1.Name = "rdbColor1";
            this.rdbColor1.Size = new System.Drawing.Size(49, 19);
            this.rdbColor1.TabIndex = 0;
            this.rdbColor1.Text = "표시";
            this.rdbColor1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rdbMy2);
            this.groupBox3.Controls.Add(this.rdbMy1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(10, 122);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(385, 56);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "내환자 자동 설정";
            // 
            // rdbMy2
            // 
            this.rdbMy2.AutoSize = true;
            this.rdbMy2.Checked = true;
            this.rdbMy2.Location = new System.Drawing.Point(193, 22);
            this.rdbMy2.Name = "rdbMy2";
            this.rdbMy2.Size = new System.Drawing.Size(145, 19);
            this.rdbMy2.TabIndex = 1;
            this.rdbMy2.TabStop = true;
            this.rdbMy2.Text = "내환자 자동 추가 않함";
            this.rdbMy2.UseVisualStyleBackColor = true;
            // 
            // rdbMy1
            // 
            this.rdbMy1.AutoSize = true;
            this.rdbMy1.Location = new System.Drawing.Point(51, 22);
            this.rdbMy1.Name = "rdbMy1";
            this.rdbMy1.Size = new System.Drawing.Size(117, 19);
            this.rdbMy1.TabIndex = 0;
            this.rdbMy1.Text = "내환자 자동 추가";
            this.rdbMy1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdbNext2);
            this.groupBox2.Controls.Add(this.rdbNext1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(10, 66);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(385, 56);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "다음환자 자동선택 설정";
            // 
            // rdbNext2
            // 
            this.rdbNext2.AutoSize = true;
            this.rdbNext2.Checked = true;
            this.rdbNext2.Location = new System.Drawing.Point(193, 22);
            this.rdbNext2.Name = "rdbNext2";
            this.rdbNext2.Size = new System.Drawing.Size(101, 19);
            this.rdbNext2.TabIndex = 1;
            this.rdbNext2.TabStop = true;
            this.rdbNext2.Text = "자동선택 않함";
            this.rdbNext2.UseVisualStyleBackColor = true;
            // 
            // rdbNext1
            // 
            this.rdbNext1.AutoSize = true;
            this.rdbNext1.Location = new System.Drawing.Point(51, 22);
            this.rdbNext1.Name = "rdbNext1";
            this.rdbNext1.Size = new System.Drawing.Size(73, 19);
            this.rdbNext1.TabIndex = 0;
            this.rdbNext1.Text = "자동선택";
            this.rdbNext1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbDisp2);
            this.groupBox1.Controls.Add(this.rdbDisp1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(385, 56);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "재원심사 완료시 자동 재원심사 화면표시 설정";
            // 
            // rdbDisp2
            // 
            this.rdbDisp2.AutoSize = true;
            this.rdbDisp2.Location = new System.Drawing.Point(193, 22);
            this.rdbDisp2.Name = "rdbDisp2";
            this.rdbDisp2.Size = new System.Drawing.Size(125, 19);
            this.rdbDisp2.TabIndex = 1;
            this.rdbDisp2.Text = "자동목록 표시않함";
            this.rdbDisp2.UseVisualStyleBackColor = true;
            // 
            // rdbDisp1
            // 
            this.rdbDisp1.AutoSize = true;
            this.rdbDisp1.Checked = true;
            this.rdbDisp1.Location = new System.Drawing.Point(51, 22);
            this.rdbDisp1.Name = "rdbDisp1";
            this.rdbDisp1.Size = new System.Drawing.Size(101, 19);
            this.rdbDisp1.TabIndex = 0;
            this.rdbDisp1.TabStop = true;
            this.rdbDisp1.Text = "자동목록 표시";
            this.rdbDisp1.UseVisualStyleBackColor = true;
            // 
            // frmPmpaJSimJobSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 326);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlHead);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "frmPmpaJSimJobSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "재원심사 환경설정";
            this.Load += new System.EventHandler(this.frmPmpaJSimJobSet_Load);
            this.pnlHead.ResumeLayout(false);
            this.pnlHead.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHead;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSave;
        public System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rdbColor2;
        private System.Windows.Forms.RadioButton rdbColor1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdbMy2;
        private System.Windows.Forms.RadioButton rdbMy1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdbNext2;
        private System.Windows.Forms.RadioButton rdbNext1;
        private System.Windows.Forms.RadioButton rdbDisp2;
        private System.Windows.Forms.RadioButton rdbDisp1;
    }
}