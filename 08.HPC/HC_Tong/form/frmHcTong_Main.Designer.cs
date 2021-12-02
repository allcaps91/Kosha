namespace HC_Tong
{
    partial class frmHcTong_Main
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.Job1 = new System.Windows.Forms.ToolStripMenuItem();
            this.Job1_1 = new System.Windows.Forms.ToolStripMenuItem();
            this.Job1_2 = new System.Windows.Forms.ToolStripMenuItem();
            this.Job1_3 = new System.Windows.Forms.ToolStripMenuItem();
            this.Job2 = new System.Windows.Forms.ToolStripMenuItem();
            this.Job2_1 = new System.Windows.Forms.ToolStripMenuItem();
            this.Job2_2 = new System.Windows.Forms.ToolStripMenuItem();
            this.Job2_3 = new System.Windows.Forms.ToolStripMenuItem();
            this.Job2_4 = new System.Windows.Forms.ToolStripMenuItem();
            this.Job_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.Job2_5 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Job1,
            this.Job2,
            this.Job_Exit});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(755, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // Job1
            // 
            this.Job1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Job1_1,
            this.Job1_2,
            this.Job1_3});
            this.Job1.Name = "Job1";
            this.Job1.Size = new System.Drawing.Size(102, 20);
            this.Job1.Text = "일별작업(Daily)";
            // 
            // Job1_1
            // 
            this.Job1_1.Name = "Job1_1";
            this.Job1_1.Size = new System.Drawing.Size(266, 22);
            this.Job1_1.Text = "건강검진 수납집계표(경리과통보용)";
            // 
            // Job1_2
            // 
            this.Job1_2.Name = "Job1_2";
            this.Job1_2.Size = new System.Drawing.Size(266, 22);
            this.Job1_2.Text = "일별 검진종류별 수입통계";
            // 
            // Job1_3
            // 
            this.Job1_3.Name = "Job1_3";
            this.Job1_3.Size = new System.Drawing.Size(266, 22);
            this.Job1_3.Text = "카드사별 수입집계";
            // 
            // Job2
            // 
            this.Job2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Job2_1,
            this.Job2_2,
            this.Job2_3,
            this.Job2_4,
            this.Job2_5});
            this.Job2.Name = "Job2";
            this.Job2.Size = new System.Drawing.Size(91, 20);
            this.Job2.Text = "검진종합통계";
            // 
            // Job2_1
            // 
            this.Job2_1.Name = "Job2_1";
            this.Job2_1.Size = new System.Drawing.Size(202, 22);
            this.Job2_1.Text = "전년도대비 인원통계";
            // 
            // Job2_2
            // 
            this.Job2_2.Name = "Job2_2";
            this.Job2_2.Size = new System.Drawing.Size(202, 22);
            this.Job2_2.Text = "암검사분류별 인원통계";
            // 
            // Job2_3
            // 
            this.Job2_3.Name = "Job2_3";
            this.Job2_3.Size = new System.Drawing.Size(202, 22);
            this.Job2_3.Text = "종합검진 직원소개 현황";
            // 
            // Job2_4
            // 
            this.Job2_4.Name = "Job2_4";
            this.Job2_4.Size = new System.Drawing.Size(202, 22);
            this.Job2_4.Text = "검진 업무일지";
            // 
            // Job_Exit
            // 
            this.Job_Exit.Name = "Job_Exit";
            this.Job_Exit.Size = new System.Drawing.Size(43, 20);
            this.Job_Exit.Text = "종료";
            // 
            // Job2_5
            // 
            this.Job2_5.Name = "Job2_5";
            this.Job2_5.Size = new System.Drawing.Size(202, 22);
            this.Job2_5.Text = "일반건진 업무일지";
            // 
            // frmHcTong_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(755, 452);
            this.Controls.Add(this.menuStrip1);
            this.Name = "frmHcTong_Main";
            this.Text = "건강진단 통계관리";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem Job1;
        private System.Windows.Forms.ToolStripMenuItem Job1_1;
        private System.Windows.Forms.ToolStripMenuItem Job1_2;
        private System.Windows.Forms.ToolStripMenuItem Job1_3;
        private System.Windows.Forms.ToolStripMenuItem Job2;
        private System.Windows.Forms.ToolStripMenuItem Job_Exit;
        private System.Windows.Forms.ToolStripMenuItem Job2_1;
        private System.Windows.Forms.ToolStripMenuItem Job2_2;
        private System.Windows.Forms.ToolStripMenuItem Job2_3;
        private System.Windows.Forms.ToolStripMenuItem Job2_4;
        private System.Windows.Forms.ToolStripMenuItem Job2_5;
    }
}