namespace ComLibB
{
    partial class frmComSupADR3_1
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
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rdorelation1 = new System.Windows.Forms.RadioButton();
            this.rdorelation2 = new System.Windows.Forms.RadioButton();
            this.rdorelation3 = new System.Windows.Forms.RadioButton();
            this.rdorelation4 = new System.Windows.Forms.RadioButton();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.rdoclass1 = new System.Windows.Forms.RadioButton();
            this.rdoclass3 = new System.Windows.Forms.RadioButton();
            this.rdoclass2 = new System.Windows.Forms.RadioButton();
            this.rdoclass4 = new System.Windows.Forms.RadioButton();
            this.panel5 = new System.Windows.Forms.Panel();
            this.txtrelationMemo = new System.Windows.Forms.TextBox();
            this.panAll = new System.Windows.Forms.Panel();
            this.panTitleSub.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panAll.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitleSub
            // 
            this.panTitleSub.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub.Controls.Add(this.lblTitleSub0);
            this.panTitleSub.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub.Location = new System.Drawing.Point(0, 0);
            this.panTitleSub.Name = "panTitleSub";
            this.panTitleSub.Size = new System.Drawing.Size(470, 34);
            this.panTitleSub.TabIndex = 23;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(5, 5);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(283, 21);
            this.lblTitleSub0.TabIndex = 4;
            this.lblTitleSub0.Text = "약물이상반응에 대한 2차 평가자 의견";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(470, 48);
            this.panel3.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(159, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "① 약물이상반응과 의심약물\r\n    과의 인과관계 평가";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.rdorelation4);
            this.panel2.Controls.Add(this.rdorelation3);
            this.panel2.Controls.Add(this.rdorelation2);
            this.panel2.Controls.Add(this.rdorelation1);
            this.panel2.Location = new System.Drawing.Point(180, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(285, 44);
            this.panel2.TabIndex = 1;
            // 
            // rdorelation1
            // 
            this.rdorelation1.AutoSize = true;
            this.rdorelation1.Location = new System.Drawing.Point(7, 2);
            this.rdorelation1.Name = "rdorelation1";
            this.rdorelation1.Size = new System.Drawing.Size(107, 19);
            this.rdorelation1.TabIndex = 1;
            this.rdorelation1.TabStop = true;
            this.rdorelation1.Text = "확실함(Certain)";
            this.rdorelation1.UseVisualStyleBackColor = true;
            // 
            // rdorelation2
            // 
            this.rdorelation2.AutoSize = true;
            this.rdorelation2.Location = new System.Drawing.Point(125, 2);
            this.rdorelation2.Name = "rdorelation2";
            this.rdorelation2.Size = new System.Drawing.Size(156, 19);
            this.rdorelation2.TabIndex = 1;
            this.rdorelation2.TabStop = true;
            this.rdorelation2.Text = "상당히 확실함(Probable)";
            this.rdorelation2.UseVisualStyleBackColor = true;
            // 
            // rdorelation3
            // 
            this.rdorelation3.AutoSize = true;
            this.rdorelation3.Location = new System.Drawing.Point(7, 21);
            this.rdorelation3.Name = "rdorelation3";
            this.rdorelation3.Size = new System.Drawing.Size(112, 19);
            this.rdorelation3.TabIndex = 1;
            this.rdorelation3.TabStop = true;
            this.rdorelation3.Text = "가능함(Possible)";
            this.rdorelation3.UseVisualStyleBackColor = true;
            // 
            // rdorelation4
            // 
            this.rdorelation4.AutoSize = true;
            this.rdorelation4.Location = new System.Drawing.Point(125, 21);
            this.rdorelation4.Name = "rdorelation4";
            this.rdorelation4.Size = new System.Drawing.Size(139, 19);
            this.rdorelation4.TabIndex = 1;
            this.rdorelation4.TabStop = true;
            this.rdorelation4.Text = "가능성 적음(Unlikely)";
            this.rdorelation4.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.panel4);
            this.panel6.Controls.Add(this.label2);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 48);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(470, 29);
            this.panel6.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(175, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "② 약물이상반응의 중증도 분류";
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.rdoclass4);
            this.panel4.Controls.Add(this.rdoclass2);
            this.panel4.Controls.Add(this.rdoclass3);
            this.panel4.Controls.Add(this.rdoclass1);
            this.panel4.Location = new System.Drawing.Point(180, 2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(285, 25);
            this.panel4.TabIndex = 1;
            // 
            // rdoclass1
            // 
            this.rdoclass1.AutoSize = true;
            this.rdoclass1.Location = new System.Drawing.Point(7, 2);
            this.rdoclass1.Name = "rdoclass1";
            this.rdoclass1.Size = new System.Drawing.Size(49, 19);
            this.rdoclass1.TabIndex = 1;
            this.rdoclass1.TabStop = true;
            this.rdoclass1.Text = "mild";
            this.rdoclass1.UseVisualStyleBackColor = true;
            // 
            // rdoclass3
            // 
            this.rdoclass3.AutoSize = true;
            this.rdoclass3.Location = new System.Drawing.Point(144, 2);
            this.rdoclass3.Name = "rdoclass3";
            this.rdoclass3.Size = new System.Drawing.Size(58, 19);
            this.rdoclass3.TabIndex = 1;
            this.rdoclass3.TabStop = true;
            this.rdoclass3.Text = "severe";
            this.rdoclass3.UseVisualStyleBackColor = true;
            // 
            // rdoclass2
            // 
            this.rdoclass2.AutoSize = true;
            this.rdoclass2.Location = new System.Drawing.Point(62, 2);
            this.rdoclass2.Name = "rdoclass2";
            this.rdoclass2.Size = new System.Drawing.Size(76, 19);
            this.rdoclass2.TabIndex = 1;
            this.rdoclass2.TabStop = true;
            this.rdoclass2.Text = "moderate";
            this.rdoclass2.UseVisualStyleBackColor = true;
            // 
            // rdoclass4
            // 
            this.rdoclass4.AutoSize = true;
            this.rdoclass4.Location = new System.Drawing.Point(208, 2);
            this.rdoclass4.Name = "rdoclass4";
            this.rdoclass4.Size = new System.Drawing.Size(62, 19);
            this.rdoclass4.TabIndex = 1;
            this.rdoclass4.TabStop = true;
            this.rdoclass4.Text = "serious";
            this.rdoclass4.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.txtrelationMemo);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 77);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(470, 61);
            this.panel5.TabIndex = 2;
            // 
            // txtrelationMemo
            // 
            this.txtrelationMemo.Location = new System.Drawing.Point(8, 3);
            this.txtrelationMemo.Multiline = true;
            this.txtrelationMemo.Name = "txtrelationMemo";
            this.txtrelationMemo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtrelationMemo.Size = new System.Drawing.Size(454, 55);
            this.txtrelationMemo.TabIndex = 0;
            // 
            // panAll
            // 
            this.panAll.AutoScroll = true;
            this.panAll.AutoSize = true;
            this.panAll.Controls.Add(this.panel5);
            this.panAll.Controls.Add(this.panel6);
            this.panAll.Controls.Add(this.panel3);
            this.panAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panAll.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.panAll.Location = new System.Drawing.Point(0, 34);
            this.panAll.Name = "panAll";
            this.panAll.Size = new System.Drawing.Size(470, 138);
            this.panAll.TabIndex = 24;
            // 
            // frmComSupADR3_1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(470, 172);
            this.Controls.Add(this.panAll);
            this.Controls.Add(this.panTitleSub);
            this.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmComSupADR3_1";
            this.Text = "약물이상반응에 대한 2차 평가자 의견";
            this.Load += new System.EventHandler(this.frmComSupADR3_1_Load);
            this.panTitleSub.ResumeLayout(false);
            this.panTitleSub.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panAll.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rdorelation4;
        private System.Windows.Forms.RadioButton rdorelation3;
        private System.Windows.Forms.RadioButton rdorelation2;
        private System.Windows.Forms.RadioButton rdorelation1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.RadioButton rdoclass4;
        private System.Windows.Forms.RadioButton rdoclass2;
        private System.Windows.Forms.RadioButton rdoclass3;
        private System.Windows.Forms.RadioButton rdoclass1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TextBox txtrelationMemo;
        private System.Windows.Forms.Panel panAll;
    }
}