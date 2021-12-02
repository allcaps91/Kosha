namespace ComLibB
{
    partial class frmMemo
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
            this.panTitleSub = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panOption = new System.Windows.Forms.Panel();
            this.cboDept1 = new System.Windows.Forms.ComboBox();
            this.txtPaname = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPano = new System.Windows.Forms.TextBox();
            this.btnDelete1 = new System.Windows.Forms.Button();
            this.btnSave1 = new System.Windows.Forms.Button();
            this.btnSearch1 = new System.Windows.Forms.Button();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtMemo = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtMemo2 = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cboDept2 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnDelete2 = new System.Windows.Forms.Button();
            this.btnSave2 = new System.Windows.Forms.Button();
            this.btnSearch2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.txtMemo3 = new System.Windows.Forms.TextBox();
            this.panTitleSub.SuspendLayout();
            this.panOption.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitleSub
            // 
            this.panTitleSub.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub.Controls.Add(this.btnExit);
            this.panTitleSub.Controls.Add(this.lblTitle);
            this.panTitleSub.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub.Location = new System.Drawing.Point(0, 0);
            this.panTitleSub.Name = "panTitleSub";
            this.panTitleSub.Size = new System.Drawing.Size(610, 34);
            this.panTitleSub.TabIndex = 12;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(527, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(166, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "환자별 참고사항 관리";
            // 
            // panOption
            // 
            this.panOption.BackColor = System.Drawing.Color.White;
            this.panOption.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panOption.Controls.Add(this.cboDept1);
            this.panOption.Controls.Add(this.txtPaname);
            this.panOption.Controls.Add(this.label3);
            this.panOption.Controls.Add(this.txtPano);
            this.panOption.Controls.Add(this.btnDelete1);
            this.panOption.Controls.Add(this.btnSave1);
            this.panOption.Controls.Add(this.btnSearch1);
            this.panOption.Controls.Add(this.lblTitleSub0);
            this.panOption.Dock = System.Windows.Forms.DockStyle.Top;
            this.panOption.Location = new System.Drawing.Point(0, 34);
            this.panOption.Name = "panOption";
            this.panOption.Size = new System.Drawing.Size(610, 34);
            this.panOption.TabIndex = 13;
            // 
            // cboDept1
            // 
            this.cboDept1.FormattingEnabled = true;
            this.cboDept1.Location = new System.Drawing.Point(294, 5);
            this.cboDept1.Name = "cboDept1";
            this.cboDept1.Size = new System.Drawing.Size(74, 20);
            this.cboDept1.TabIndex = 46;
            // 
            // txtPaname
            // 
            this.txtPaname.Location = new System.Drawing.Point(156, 5);
            this.txtPaname.Name = "txtPaname";
            this.txtPaname.Size = new System.Drawing.Size(90, 21);
            this.txtPaname.TabIndex = 45;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(249, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 17);
            this.label3.TabIndex = 44;
            this.label3.Text = "진료과";
            // 
            // txtPano
            // 
            this.txtPano.Location = new System.Drawing.Point(65, 5);
            this.txtPano.Name = "txtPano";
            this.txtPano.Size = new System.Drawing.Size(90, 21);
            this.txtPano.TabIndex = 43;
            this.txtPano.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPano_KeyDown);
            // 
            // btnDelete1
            // 
            this.btnDelete1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete1.BackColor = System.Drawing.Color.Transparent;
            this.btnDelete1.Location = new System.Drawing.Point(527, 0);
            this.btnDelete1.Name = "btnDelete1";
            this.btnDelete1.Size = new System.Drawing.Size(72, 30);
            this.btnDelete1.TabIndex = 42;
            this.btnDelete1.Text = "삭제";
            this.btnDelete1.UseVisualStyleBackColor = false;
            this.btnDelete1.Click += new System.EventHandler(this.btnDelete1_Click);
            // 
            // btnSave1
            // 
            this.btnSave1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave1.BackColor = System.Drawing.Color.Transparent;
            this.btnSave1.Location = new System.Drawing.Point(455, 0);
            this.btnSave1.Name = "btnSave1";
            this.btnSave1.Size = new System.Drawing.Size(72, 30);
            this.btnSave1.TabIndex = 41;
            this.btnSave1.Text = "저장";
            this.btnSave1.UseVisualStyleBackColor = false;
            this.btnSave1.Click += new System.EventHandler(this.btnSave1_Click);
            // 
            // btnSearch1
            // 
            this.btnSearch1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch1.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch1.Location = new System.Drawing.Point(383, 0);
            this.btnSearch1.Name = "btnSearch1";
            this.btnSearch1.Size = new System.Drawing.Size(72, 30);
            this.btnSearch1.TabIndex = 40;
            this.btnSearch1.Text = "조회";
            this.btnSearch1.UseVisualStyleBackColor = false;
            this.btnSearch1.Click += new System.EventHandler(this.btnSearch1_Click);
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.Black;
            this.lblTitleSub0.Location = new System.Drawing.Point(7, 7);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(60, 17);
            this.lblTitleSub0.TabIndex = 39;
            this.lblTitleSub0.Text = "등록번호";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.txtMemo);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 68);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(610, 201);
            this.panel3.TabIndex = 18;
            // 
            // txtMemo
            // 
            this.txtMemo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMemo.Location = new System.Drawing.Point(0, 0);
            this.txtMemo.Multiline = true;
            this.txtMemo.Name = "txtMemo";
            this.txtMemo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMemo.Size = new System.Drawing.Size(610, 201);
            this.txtMemo.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 269);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(610, 28);
            this.panel1.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(8, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "퇴원시 병동에서 전달 메세지";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.txtMemo2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 297);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(610, 160);
            this.panel2.TabIndex = 20;
            // 
            // txtMemo2
            // 
            this.txtMemo2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMemo2.Location = new System.Drawing.Point(0, 0);
            this.txtMemo2.Multiline = true;
            this.txtMemo2.Name = "txtMemo2";
            this.txtMemo2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMemo2.Size = new System.Drawing.Size(610, 160);
            this.txtMemo2.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.cboDept2);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.btnDelete2);
            this.panel4.Controls.Add(this.btnSave2);
            this.panel4.Controls.Add(this.btnSearch2);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 457);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(610, 28);
            this.panel4.TabIndex = 21;
            // 
            // cboDept2
            // 
            this.cboDept2.FormattingEnabled = true;
            this.cboDept2.Location = new System.Drawing.Point(289, 1);
            this.cboDept2.Name = "cboDept2";
            this.cboDept2.Size = new System.Drawing.Size(74, 20);
            this.cboDept2.TabIndex = 40;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(244, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 15);
            this.label4.TabIndex = 39;
            this.label4.Text = "진료과";
            // 
            // btnDelete2
            // 
            this.btnDelete2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete2.BackColor = System.Drawing.Color.Transparent;
            this.btnDelete2.Location = new System.Drawing.Point(524, -2);
            this.btnDelete2.Name = "btnDelete2";
            this.btnDelete2.Size = new System.Drawing.Size(72, 26);
            this.btnDelete2.TabIndex = 31;
            this.btnDelete2.Text = "삭제";
            this.btnDelete2.UseVisualStyleBackColor = false;
            this.btnDelete2.Click += new System.EventHandler(this.btnDelete2_Click);
            // 
            // btnSave2
            // 
            this.btnSave2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave2.BackColor = System.Drawing.Color.Transparent;
            this.btnSave2.Location = new System.Drawing.Point(453, -2);
            this.btnSave2.Name = "btnSave2";
            this.btnSave2.Size = new System.Drawing.Size(72, 26);
            this.btnSave2.TabIndex = 30;
            this.btnSave2.Text = "저장";
            this.btnSave2.UseVisualStyleBackColor = false;
            this.btnSave2.Click += new System.EventHandler(this.btnSave2_Click);
            // 
            // btnSearch2
            // 
            this.btnSearch2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch2.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch2.Location = new System.Drawing.Point(382, -2);
            this.btnSearch2.Name = "btnSearch2";
            this.btnSearch2.Size = new System.Drawing.Size(72, 26);
            this.btnSearch2.TabIndex = 29;
            this.btnSearch2.Text = "조회";
            this.btnSearch2.UseVisualStyleBackColor = false;
            this.btnSearch2.Click += new System.EventHandler(this.btnSearch2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(8, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "타과전달 메세지";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.White;
            this.panel5.Controls.Add(this.txtMemo3);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 485);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(610, 115);
            this.panel5.TabIndex = 22;
            // 
            // txtMemo3
            // 
            this.txtMemo3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMemo3.Location = new System.Drawing.Point(0, 0);
            this.txtMemo3.Multiline = true;
            this.txtMemo3.Name = "txtMemo3";
            this.txtMemo3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMemo3.Size = new System.Drawing.Size(610, 115);
            this.txtMemo3.TabIndex = 1;
            // 
            // frmMemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 600);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panOption);
            this.Controls.Add(this.panTitleSub);
            this.Name = "frmMemo";
            this.Text = "frmMemo";
            this.Load += new System.EventHandler(this.frmMemo_Load);
            this.panTitleSub.ResumeLayout(false);
            this.panTitleSub.PerformLayout();
            this.panOption.ResumeLayout(false);
            this.panOption.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panOption;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtMemo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtMemo2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ComboBox cboDept2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnDelete2;
        private System.Windows.Forms.Button btnSave2;
        private System.Windows.Forms.Button btnSearch2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TextBox txtMemo3;
        private System.Windows.Forms.ComboBox cboDept1;
        private System.Windows.Forms.TextBox txtPaname;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPano;
        private System.Windows.Forms.Button btnDelete1;
        private System.Windows.Forms.Button btnSave1;
        private System.Windows.Forms.Button btnSearch1;
        private System.Windows.Forms.Label lblTitleSub0;
    }
}