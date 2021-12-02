namespace HC_Sangdam
{
    partial class frmHcSangdamMain
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
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu01 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu01_01 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.Menu01_02 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu01_03 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.Menu01_04 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu02 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu03 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu04 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu05 = new System.Windows.Forms.ToolStripMenuItem();
            this.panMain = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.menuStrip1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuExit,
            this.Menu01,
            this.Menu02,
            this.Menu03,
            this.Menu04,
            this.Menu05});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1295, 25);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(62, 21);
            this.menuExit.Text = "종료(&X)";
            // 
            // Menu01
            // 
            this.Menu01.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu01_01,
            this.toolStripMenuItem1,
            this.Menu01_02,
            this.Menu01_03,
            this.toolStripMenuItem2,
            this.Menu01_04});
            this.Menu01.Name = "Menu01";
            this.Menu01.Size = new System.Drawing.Size(98, 21);
            this.Menu01.Text = "건강검진상담";
            // 
            // Menu01_01
            // 
            this.Menu01_01.Name = "Menu01_01";
            this.Menu01_01.Size = new System.Drawing.Size(185, 22);
            this.Menu01_01.Text = "통합상담프로그램";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(182, 6);
            // 
            // Menu01_02
            // 
            this.Menu01_02.Name = "Menu01_02";
            this.Menu01_02.Size = new System.Drawing.Size(185, 22);
            this.Menu01_02.Text = "구강상담(일반)";
            // 
            // Menu01_03
            // 
            this.Menu01_03.Name = "Menu01_03";
            this.Menu01_03.Size = new System.Drawing.Size(185, 22);
            this.Menu01_03.Text = "구강상담(학생)";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(182, 6);
            // 
            // Menu01_04
            // 
            this.Menu01_04.Name = "Menu01_04";
            this.Menu01_04.Size = new System.Drawing.Size(185, 22);
            this.Menu01_04.Text = "종합검진 구강상담";
            // 
            // Menu02
            // 
            this.Menu02.Name = "Menu02";
            this.Menu02.Size = new System.Drawing.Size(72, 21);
            this.Menu02.Text = "향정승인";
            // 
            // Menu03
            // 
            this.Menu03.Name = "Menu03";
            this.Menu03.Size = new System.Drawing.Size(111, 21);
            this.Menu03.Text = "내시경처방전송";
            // 
            // Menu04
            // 
            this.Menu04.Name = "Menu04";
            this.Menu04.Size = new System.Drawing.Size(98, 21);
            this.Menu04.Text = "인터넷문진표";
            // 
            // Menu05
            // 
            this.Menu05.Name = "Menu05";
            this.Menu05.Size = new System.Drawing.Size(165, 21);
            this.Menu05.Text = "생활습관 개선 상담 수정";
            // 
            // panMain
            // 
            this.panMain.BackColor = System.Drawing.SystemColors.Control;
            this.panMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(0, 25);
            this.panMain.Name = "panMain";
            this.panMain.Size = new System.Drawing.Size(1295, 769);
            this.panMain.TabIndex = 5;
            // 
            // frmHcSangdamMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1295, 794);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHcSangdamMain";
            this.Text = "건강검진 상담";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.ToolStripMenuItem Menu01;
        private System.Windows.Forms.ToolStripMenuItem Menu02;
        private System.Windows.Forms.ToolStripMenuItem Menu03;
        private System.Windows.Forms.ToolStripMenuItem Menu04;
        private System.Windows.Forms.Panel panMain;
        private System.Windows.Forms.ToolStripMenuItem Menu05;
        private System.Windows.Forms.ToolStripMenuItem Menu01_01;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem Menu01_02;
        private System.Windows.Forms.ToolStripMenuItem Menu01_03;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem Menu01_04;
    }
}