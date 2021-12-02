namespace ComMedLibB
{
    partial class frmMedDrugRepeateSayu
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
            this.panbtn1 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panheader4 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtEtcSayu = new System.Windows.Forms.TextBox();
            this.rdoGu2 = new System.Windows.Forms.RadioButton();
            this.rdoGu1 = new System.Windows.Forms.RadioButton();
            this.rdoGu0 = new System.Windows.Forms.RadioButton();
            this.panbtn1.SuspendLayout();
            this.panheader4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panbtn1
            // 
            this.panbtn1.Controls.Add(this.btnOK);
            this.panbtn1.Controls.Add(this.label1);
            this.panbtn1.Controls.Add(this.panel4);
            this.panbtn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panbtn1.Location = new System.Drawing.Point(0, 37);
            this.panbtn1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panbtn1.Name = "panbtn1";
            this.panbtn1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panbtn1.Size = new System.Drawing.Size(357, 42);
            this.panbtn1.TabIndex = 130;
            // 
            // btnOK
            // 
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnOK.Location = new System.Drawing.Point(282, 4);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(70, 34);
            this.btnOK.TabIndex = 29;
            this.btnOK.Text = "확인";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(10, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(195, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "추가처방 사유 선택";
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(352, 4);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(2, 34);
            this.panel4.TabIndex = 167;
            // 
            // panheader4
            // 
            this.panheader4.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panheader4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panheader4.Controls.Add(this.label2);
            this.panheader4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panheader4.Location = new System.Drawing.Point(0, 0);
            this.panheader4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panheader4.Name = "panheader4";
            this.panheader4.Size = new System.Drawing.Size(357, 37);
            this.panheader4.TabIndex = 131;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(4, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 17);
            this.label2.TabIndex = 41;
            this.label2.Text = "약 중복처방 사유 체크";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 79);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Size = new System.Drawing.Size(357, 132);
            this.panel1.TabIndex = 132;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtEtcSayu);
            this.panel2.Controls.Add(this.rdoGu2);
            this.panel2.Controls.Add(this.rdoGu1);
            this.panel2.Controls.Add(this.rdoGu0);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(351, 124);
            this.panel2.TabIndex = 1;
            // 
            // txtEtcSayu
            // 
            this.txtEtcSayu.Enabled = false;
            this.txtEtcSayu.Location = new System.Drawing.Point(74, 50);
            this.txtEtcSayu.Multiline = true;
            this.txtEtcSayu.Name = "txtEtcSayu";
            this.txtEtcSayu.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtEtcSayu.Size = new System.Drawing.Size(269, 49);
            this.txtEtcSayu.TabIndex = 3;
            // 
            // rdoGu2
            // 
            this.rdoGu2.AutoSize = true;
            this.rdoGu2.Location = new System.Drawing.Point(12, 52);
            this.rdoGu2.Name = "rdoGu2";
            this.rdoGu2.Size = new System.Drawing.Size(52, 21);
            this.rdoGu2.TabIndex = 2;
            this.rdoGu2.TabStop = true;
            this.rdoGu2.Text = "기타";
            this.rdoGu2.UseVisualStyleBackColor = true;
            this.rdoGu2.CheckedChanged += new System.EventHandler(this.rdoGu2_CheckedChanged);
            // 
            // rdoGu1
            // 
            this.rdoGu1.AutoSize = true;
            this.rdoGu1.Location = new System.Drawing.Point(118, 9);
            this.rdoGu1.Name = "rdoGu1";
            this.rdoGu1.Size = new System.Drawing.Size(96, 21);
            this.rdoGu1.TabIndex = 1;
            this.rdoGu1.TabStop = true;
            this.rdoGu1.Text = "조제약 분실";
            this.rdoGu1.UseVisualStyleBackColor = true;
            // 
            // rdoGu0
            // 
            this.rdoGu0.AutoSize = true;
            this.rdoGu0.Location = new System.Drawing.Point(12, 9);
            this.rdoGu0.Name = "rdoGu0";
            this.rdoGu0.Size = new System.Drawing.Size(83, 21);
            this.rdoGu0.TabIndex = 0;
            this.rdoGu0.TabStop = true;
            this.rdoGu0.Text = "용량 조절";
            this.rdoGu0.UseVisualStyleBackColor = true;
            // 
            // frmMedDrugRepeateSayu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(357, 208);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panbtn1);
            this.Controls.Add(this.panheader4);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmMedDrugRepeateSayu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmMedDrugRepeateSayu";
            this.Load += new System.EventHandler(this.frmMedDrugRepeateSayu_Load);
            this.panbtn1.ResumeLayout(false);
            this.panbtn1.PerformLayout();
            this.panheader4.ResumeLayout(false);
            this.panheader4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panbtn1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panheader4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtEtcSayu;
        private System.Windows.Forms.RadioButton rdoGu2;
        private System.Windows.Forms.RadioButton rdoGu1;
        private System.Windows.Forms.RadioButton rdoGu0;
    }
}