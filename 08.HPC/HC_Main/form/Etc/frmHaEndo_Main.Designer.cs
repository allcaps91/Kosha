namespace HC_Main
{
    partial class frmHaEndo_Main
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
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menu_00 = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_01 = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_02 = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_03 = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_08 = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_04 = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_05 = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_06 = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_07 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panMain = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_00,
            this.menu_01,
            this.menu_02,
            this.menu_03,
            this.menu_08,
            this.menu_04,
            this.menu_05,
            this.menu_06,
            this.menu_07});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1090, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menu_00
            // 
            this.menu_00.Name = "menu_00";
            this.menu_00.Size = new System.Drawing.Size(58, 20);
            this.menu_00.Text = "종료(&X)";
            // 
            // menu_01
            // 
            this.menu_01.Name = "menu_01";
            this.menu_01.Size = new System.Drawing.Size(103, 20);
            this.menu_01.Text = "내시경예약관리";
            this.menu_01.Visible = false;
            // 
            // menu_02
            // 
            this.menu_02.Name = "menu_02";
            this.menu_02.Size = new System.Drawing.Size(135, 20);
            this.menu_02.Text = "비상마약류 관리 대장";
            // 
            // menu_03
            // 
            this.menu_03.Name = "menu_03";
            this.menu_03.Size = new System.Drawing.Size(95, 20);
            this.menu_03.Text = "향정약품 관리";
            // 
            // menu_08
            // 
            this.menu_08.Name = "menu_08";
            this.menu_08.Size = new System.Drawing.Size(107, 20);
            this.menu_08.Text = "마약처방전 인쇄";
            // 
            // menu_04
            // 
            this.menu_04.Name = "menu_04";
            this.menu_04.Size = new System.Drawing.Size(135, 20);
            this.menu_04.Text = "내시경 검체장부 관리";
            this.menu_04.Visible = false;
            // 
            // menu_05
            // 
            this.menu_05.Name = "menu_05";
            this.menu_05.Size = new System.Drawing.Size(135, 20);
            this.menu_05.Text = "환경 미생물 오더확인";
            // 
            // menu_06
            // 
            this.menu_06.Name = "menu_06";
            this.menu_06.Size = new System.Drawing.Size(191, 20);
            this.menu_06.Text = "약물이상반응(ADR) 발생 보고서";
            // 
            // menu_07
            // 
            this.menu_07.Name = "menu_07";
            this.menu_07.Size = new System.Drawing.Size(67, 20);
            this.menu_07.Text = "약품정보";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // panMain
            // 
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.panMain.Location = new System.Drawing.Point(0, 24);
            this.panMain.Name = "panMain";
            this.panMain.Padding = new System.Windows.Forms.Padding(3);
            this.panMain.Size = new System.Drawing.Size(1090, 613);
            this.panMain.TabIndex = 266;
            // 
            // frmHaEndo_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1090, 637);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHaEndo_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "종합검진 내시경실 업무";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menu_01;
        private System.Windows.Forms.ToolStripMenuItem menu_02;
        private System.Windows.Forms.ToolStripMenuItem menu_03;
        private System.Windows.Forms.ToolStripMenuItem menu_04;
        private System.Windows.Forms.ToolStripMenuItem menu_05;
        private System.Windows.Forms.ToolStripMenuItem menu_06;
        private System.Windows.Forms.ToolStripMenuItem menu_07;
        private System.Windows.Forms.ToolStripMenuItem menu_08;
        private System.Windows.Forms.Panel panMain;
        private System.Windows.Forms.ToolStripMenuItem menu_00;
    }
}