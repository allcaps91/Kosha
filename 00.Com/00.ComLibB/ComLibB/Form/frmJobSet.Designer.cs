namespace ComLibB
{
    partial class frmJobSet
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.optPrt0 = new System.Windows.Forms.RadioButton();
            this.optPrt1 = new System.Windows.Forms.RadioButton();
            this.optSave0 = new System.Windows.Forms.RadioButton();
            this.optSave1 = new System.Windows.Forms.RadioButton();
            this.optSave2 = new System.Windows.Forms.RadioButton();
            this.optCancel0 = new System.Windows.Forms.RadioButton();
            this.optCancel1 = new System.Windows.Forms.RadioButton();
            this.btnSave = new System.Windows.Forms.Button();
            this.panTitle.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.Controls.Add(this.btnSave);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(304, 34);
            this.panTitle.TabIndex = 90;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(228, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(9, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(116, 16);
            this.lblTitle.TabIndex = 15;
            this.lblTitle.Text = "작업환경 설정";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 34);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(169, 156);
            this.panel2.TabIndex = 92;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(169, 34);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(4);
            this.panel3.Size = new System.Drawing.Size(135, 156);
            this.panel3.TabIndex = 93;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.optSave2);
            this.groupBox1.Controls.Add(this.optSave1);
            this.groupBox1.Controls.Add(this.optSave0);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(127, 148);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "저장시";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.groupBox2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(4);
            this.panel4.Size = new System.Drawing.Size(169, 78);
            this.panel4.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.groupBox3);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 78);
            this.panel5.Name = "panel5";
            this.panel5.Padding = new System.Windows.Forms.Padding(4);
            this.panel5.Size = new System.Drawing.Size(169, 78);
            this.panel5.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.optPrt1);
            this.groupBox2.Controls.Add(this.optPrt0);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(161, 70);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "판독결과 저장시 자동인쇄";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.optCancel1);
            this.groupBox3.Controls.Add(this.optCancel0);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(4, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(161, 70);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "취소시 경고 문구 표시";
            // 
            // optPrt0
            // 
            this.optPrt0.AutoSize = true;
            this.optPrt0.Checked = true;
            this.optPrt0.Location = new System.Drawing.Point(10, 20);
            this.optPrt0.Name = "optPrt0";
            this.optPrt0.Size = new System.Drawing.Size(71, 16);
            this.optPrt0.TabIndex = 0;
            this.optPrt0.TabStop = true;
            this.optPrt0.Text = "자동인쇄";
            this.optPrt0.UseVisualStyleBackColor = true;
            // 
            // optPrt1
            // 
            this.optPrt1.AutoSize = true;
            this.optPrt1.Location = new System.Drawing.Point(10, 47);
            this.optPrt1.Name = "optPrt1";
            this.optPrt1.Size = new System.Drawing.Size(71, 16);
            this.optPrt1.TabIndex = 1;
            this.optPrt1.Text = "인쇄않함";
            this.optPrt1.UseVisualStyleBackColor = true;
            // 
            // optSave0
            // 
            this.optSave0.AutoSize = true;
            this.optSave0.Checked = true;
            this.optSave0.Location = new System.Drawing.Point(10, 40);
            this.optSave0.Name = "optSave0";
            this.optSave0.Size = new System.Drawing.Size(87, 16);
            this.optSave0.TabIndex = 1;
            this.optSave0.TabStop = true;
            this.optSave0.Text = "일자별 명단";
            this.optSave0.UseVisualStyleBackColor = true;
            // 
            // optSave1
            // 
            this.optSave1.AutoSize = true;
            this.optSave1.Location = new System.Drawing.Point(10, 72);
            this.optSave1.Name = "optSave1";
            this.optSave1.Size = new System.Drawing.Size(83, 16);
            this.optSave1.TabIndex = 2;
            this.optSave1.Text = "등록번호별";
            this.optSave1.UseVisualStyleBackColor = true;
            // 
            // optSave2
            // 
            this.optSave2.AutoSize = true;
            this.optSave2.Location = new System.Drawing.Point(10, 104);
            this.optSave2.Name = "optSave2";
            this.optSave2.Size = new System.Drawing.Size(71, 16);
            this.optSave2.TabIndex = 3;
            this.optSave2.Text = "현재화며";
            this.optSave2.UseVisualStyleBackColor = true;
            // 
            // optCancel0
            // 
            this.optCancel0.AutoSize = true;
            this.optCancel0.Checked = true;
            this.optCancel0.Location = new System.Drawing.Point(10, 20);
            this.optCancel0.Name = "optCancel0";
            this.optCancel0.Size = new System.Drawing.Size(59, 16);
            this.optCancel0.TabIndex = 1;
            this.optCancel0.TabStop = true;
            this.optCancel0.Text = "표시함";
            this.optCancel0.UseVisualStyleBackColor = true;
            // 
            // optCancel1
            // 
            this.optCancel1.AutoSize = true;
            this.optCancel1.Location = new System.Drawing.Point(10, 47);
            this.optCancel1.Name = "optCancel1";
            this.optCancel1.Size = new System.Drawing.Size(71, 16);
            this.optCancel1.TabIndex = 2;
            this.optCancel1.Text = "표시안함";
            this.optCancel1.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Location = new System.Drawing.Point(156, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 30);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmJobSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(304, 190);
            this.ControlBox = false;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panTitle);
            this.Name = "frmJobSet";
            this.Text = "작업환경 설정";
            this.Load += new System.EventHandler(this.frmJobSet_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton optCancel1;
        private System.Windows.Forms.RadioButton optCancel0;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton optPrt1;
        private System.Windows.Forms.RadioButton optPrt0;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton optSave2;
        private System.Windows.Forms.RadioButton optSave1;
        private System.Windows.Forms.RadioButton optSave0;
    }
}