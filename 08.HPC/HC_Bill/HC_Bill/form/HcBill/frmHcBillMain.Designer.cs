namespace HC_Bill
{
    partial class frmHcBillMain
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
            this.menuMainMir = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu01_01 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.Menu01_02 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu01_03 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.Menu01_04 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu01_05 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu01_06 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu01_07 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu01_08 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.Menu01_09 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEtc = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu02_01 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu02_02 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu02_03 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu03_01 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu03_02 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu03_03 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu03_04 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOldMir = new System.Windows.Forms.ToolStripMenuItem();
            this.panMain = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.panMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuExit,
            this.menuMainMir,
            this.menuEtc,
            this.menuSearch,
            this.menuOldMir});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(58, 20);
            this.menuExit.Text = "종료(&X)";
            // 
            // menuMainMir
            // 
            this.menuMainMir.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu01_01,
            this.toolStripMenuItem1,
            this.Menu01_02,
            this.Menu01_03,
            this.toolStripMenuItem2,
            this.Menu01_04,
            this.Menu01_05,
            this.Menu01_06,
            this.Menu01_07,
            this.Menu01_08,
            this.toolStripMenuItem3,
            this.Menu01_09});
            this.menuMainMir.Name = "menuMainMir";
            this.menuMainMir.Size = new System.Drawing.Size(67, 20);
            this.menuMainMir.Text = "청구작업";
            // 
            // Menu01_01
            // 
            this.Menu01_01.Name = "Menu01_01";
            this.Menu01_01.Size = new System.Drawing.Size(240, 22);
            this.Menu01_01.Text = "1.청구자 제외명단 관리(통합)";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(237, 6);
            // 
            // Menu01_02
            // 
            this.Menu01_02.Name = "Menu01_02";
            this.Menu01_02.Size = new System.Drawing.Size(240, 22);
            this.Menu01_02.Text = "2.청구대상자 명단작성";
            // 
            // Menu01_03
            // 
            this.Menu01_03.Name = "Menu01_03";
            this.Menu01_03.Size = new System.Drawing.Size(240, 22);
            this.Menu01_03.Text = "3.청구대상자 점검 및 파일작성";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(237, 6);
            // 
            // Menu01_04
            // 
            this.Menu01_04.Name = "Menu01_04";
            this.Menu01_04.Size = new System.Drawing.Size(240, 22);
            this.Menu01_04.Text = "4.검진비용청구서";
            // 
            // Menu01_05
            // 
            this.Menu01_05.Name = "Menu01_05";
            this.Menu01_05.Size = new System.Drawing.Size(240, 22);
            this.Menu01_05.Text = "5.구강비용청구서";
            // 
            // Menu01_06
            // 
            this.Menu01_06.Name = "Menu01_06";
            this.Menu01_06.Size = new System.Drawing.Size(240, 22);
            this.Menu01_06.Text = "6.암검진비용청구서";
            // 
            // Menu01_07
            // 
            this.Menu01_07.Name = "Menu01_07";
            this.Menu01_07.Size = new System.Drawing.Size(240, 22);
            this.Menu01_07.Text = "7.수납집계표";
            // 
            // Menu01_08
            // 
            this.Menu01_08.Name = "Menu01_08";
            this.Menu01_08.Size = new System.Drawing.Size(240, 22);
            this.Menu01_08.Text = "8.건진청구 오류 수정의뢰 내역";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(237, 6);
            // 
            // Menu01_09
            // 
            this.Menu01_09.Name = "Menu01_09";
            this.Menu01_09.Size = new System.Drawing.Size(240, 22);
            this.Menu01_09.Text = "9.암검진 건수 점검";
            // 
            // menuEtc
            // 
            this.menuEtc.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu02_01,
            this.Menu02_02,
            this.Menu02_03});
            this.menuEtc.Name = "menuEtc";
            this.menuEtc.Size = new System.Drawing.Size(67, 20);
            this.menuEtc.Text = "기타업무";
            // 
            // Menu02_01
            // 
            this.Menu02_01.Name = "Menu02_01";
            this.Menu02_01.Size = new System.Drawing.Size(200, 22);
            this.Menu02_01.Text = "1.공무원 인원조회";
            // 
            // Menu02_02
            // 
            this.Menu02_02.Name = "Menu02_02";
            this.Menu02_02.Size = new System.Drawing.Size(200, 22);
            this.Menu02_02.Text = "2.암검진 회사청구 관리";
            // 
            // Menu02_03
            // 
            this.Menu02_03.Name = "Menu02_03";
            this.Menu02_03.Size = new System.Drawing.Size(200, 22);
            this.Menu02_03.Text = "3.결과통보서 발송대장";
            // 
            // menuSearch
            // 
            this.menuSearch.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu03_01,
            this.Menu03_02,
            this.Menu03_03,
            this.Menu03_04});
            this.menuSearch.Name = "menuSearch";
            this.menuSearch.Size = new System.Drawing.Size(67, 20);
            this.menuSearch.Text = "조회업무";
            // 
            // Menu03_01
            // 
            this.Menu03_01.Name = "Menu03_01";
            this.Menu03_01.Size = new System.Drawing.Size(232, 22);
            this.Menu03_01.Text = "1.청구금액조회(&V)";
            // 
            // Menu03_02
            // 
            this.Menu03_02.Name = "Menu03_02";
            this.Menu03_02.Size = new System.Drawing.Size(232, 22);
            this.Menu03_02.Text = "2.회사청구조회 및 청구작업";
            // 
            // Menu03_03
            // 
            this.Menu03_03.Name = "Menu03_03";
            this.Menu03_03.Size = new System.Drawing.Size(232, 22);
            this.Menu03_03.Text = "3.종검 및 건진 청구금액 조회";
            // 
            // Menu03_04
            // 
            this.Menu03_04.Name = "Menu03_04";
            this.Menu03_04.Size = new System.Drawing.Size(232, 22);
            this.Menu03_04.Text = "4.확진대상자 조회";
            // 
            // menuOldMir
            // 
            this.menuOldMir.Name = "menuOldMir";
            this.menuOldMir.Size = new System.Drawing.Size(67, 20);
            this.menuOldMir.Text = "과거자료";
            this.menuOldMir.Visible = false;
            // 
            // panMain
            // 
            this.panMain.Controls.Add(this.menuStrip1);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(0, 0);
            this.panMain.Name = "panMain";
            this.panMain.Size = new System.Drawing.Size(800, 637);
            this.panMain.TabIndex = 41;
            // 
            // frmHcBillMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 637);
            this.Controls.Add(this.panMain);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHcBillMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "청구메인";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panMain.ResumeLayout(false);
            this.panMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.ToolStripMenuItem menuMainMir;
        private System.Windows.Forms.ToolStripMenuItem menuEtc;
        private System.Windows.Forms.ToolStripMenuItem menuSearch;
        private System.Windows.Forms.ToolStripMenuItem menuOldMir;
        private System.Windows.Forms.Panel panMain;
        private System.Windows.Forms.ToolStripMenuItem Menu01_01;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem Menu01_02;
        private System.Windows.Forms.ToolStripMenuItem Menu01_03;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem Menu01_04;
        private System.Windows.Forms.ToolStripMenuItem Menu01_05;
        private System.Windows.Forms.ToolStripMenuItem Menu01_06;
        private System.Windows.Forms.ToolStripMenuItem Menu01_07;
        private System.Windows.Forms.ToolStripMenuItem Menu01_08;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem Menu01_09;
        private System.Windows.Forms.ToolStripMenuItem Menu02_01;
        private System.Windows.Forms.ToolStripMenuItem Menu02_02;
        private System.Windows.Forms.ToolStripMenuItem Menu02_03;
        private System.Windows.Forms.ToolStripMenuItem Menu03_01;
        private System.Windows.Forms.ToolStripMenuItem Menu03_02;
        private System.Windows.Forms.ToolStripMenuItem Menu03_03;
        private System.Windows.Forms.ToolStripMenuItem Menu03_04;
    }
}