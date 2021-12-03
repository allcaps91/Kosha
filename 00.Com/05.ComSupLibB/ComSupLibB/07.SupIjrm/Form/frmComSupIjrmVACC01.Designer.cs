namespace ComSupLibB.SupIjrm
{
    partial class frmComSupIjrmVACC01
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
            FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer enhancedRowHeaderRenderer5 = new FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer();
            FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer enhancedRowHeaderRenderer1 = new FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panMain = new System.Windows.Forms.Panel();
            this.circProgress = new DevComponents.DotNetBar.Controls.CircularProgress();
            this.webBr = new System.Windows.Forms.WebBrowser();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.line1 = new DevComponents.DotNetBar.Controls.Line();
            this.panVaccine = new System.Windows.Forms.Panel();
            this.ucSupComPtSearch1 = new ComSupLibB.UcSupComPtSearch();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtJUMIN2 = new System.Windows.Forms.TextBox();
            this.txtJUMIN1 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.panTitle.SuspendLayout();
            this.panMain.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panVaccine.SuspendLayout();
            this.SuspendLayout();
            enhancedRowHeaderRenderer5.BackColor = System.Drawing.SystemColors.Control;
            enhancedRowHeaderRenderer5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            enhancedRowHeaderRenderer5.ForeColor = System.Drawing.SystemColors.ControlText;
            enhancedRowHeaderRenderer5.Name = "enhancedRowHeaderRenderer5";
            enhancedRowHeaderRenderer5.PictureZoomEffect = false;
            enhancedRowHeaderRenderer5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            enhancedRowHeaderRenderer5.TextRotationAngle = 0D;
            enhancedRowHeaderRenderer5.ZoomFactor = 1F;
            enhancedRowHeaderRenderer1.Name = "enhancedRowHeaderRenderer1";
            enhancedRowHeaderRenderer1.PictureZoomEffect = false;
            enhancedRowHeaderRenderer1.TextRotationAngle = 0D;
            enhancedRowHeaderRenderer1.ZoomFactor = 1F;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Padding = new System.Windows.Forms.Padding(1);
            this.panTitle.Size = new System.Drawing.Size(1284, 34);
            this.panTitle.TabIndex = 14;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(1211, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 32);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(6, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(224, 16);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "질병관리본부 예방접종 내역";
            // 
            // panMain
            // 
            this.panMain.Controls.Add(this.circProgress);
            this.panMain.Controls.Add(this.webBr);
            this.panMain.Controls.Add(this.panTitleSub0);
            this.panMain.Controls.Add(this.line1);
            this.panMain.Controls.Add(this.panVaccine);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(0, 34);
            this.panMain.Name = "panMain";
            this.panMain.Padding = new System.Windows.Forms.Padding(1);
            this.panMain.Size = new System.Drawing.Size(1284, 780);
            this.panMain.TabIndex = 91;
            // 
            // circProgress
            // 
            this.circProgress.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.circProgress.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.circProgress.Location = new System.Drawing.Point(539, 195);
            this.circProgress.Name = "circProgress";
            this.circProgress.Size = new System.Drawing.Size(259, 264);
            this.circProgress.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeXP;
            this.circProgress.TabIndex = 106;
            this.circProgress.Visible = false;
            // 
            // webBr
            // 
            this.webBr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBr.Location = new System.Drawing.Point(1, 71);
            this.webBr.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBr.Name = "webBr";
            this.webBr.Size = new System.Drawing.Size(1282, 708);
            this.webBr.TabIndex = 105;
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(1, 43);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(1282, 28);
            this.panTitleSub0.TabIndex = 103;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 6);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(101, 12);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "접종내역 리스트";
            // 
            // line1
            // 
            this.line1.Dock = System.Windows.Forms.DockStyle.Top;
            this.line1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.line1.Location = new System.Drawing.Point(1, 33);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(1282, 10);
            this.line1.TabIndex = 100;
            this.line1.Text = "line1";
            this.line1.Thickness = 3;
            // 
            // panVaccine
            // 
            this.panVaccine.Controls.Add(this.ucSupComPtSearch1);
            this.panVaccine.Controls.Add(this.btnSearch);
            this.panVaccine.Controls.Add(this.txtJUMIN2);
            this.panVaccine.Controls.Add(this.txtJUMIN1);
            this.panVaccine.Controls.Add(this.label9);
            this.panVaccine.Dock = System.Windows.Forms.DockStyle.Top;
            this.panVaccine.Location = new System.Drawing.Point(1, 1);
            this.panVaccine.Name = "panVaccine";
            this.panVaccine.Padding = new System.Windows.Forms.Padding(1);
            this.panVaccine.Size = new System.Drawing.Size(1282, 32);
            this.panVaccine.TabIndex = 98;
            // 
            // ucSupComPtSearch1
            // 
            this.ucSupComPtSearch1.AutoSize = true;
            this.ucSupComPtSearch1.BackColor = System.Drawing.Color.White;
            this.ucSupComPtSearch1.Location = new System.Drawing.Point(9, 4);
            this.ucSupComPtSearch1.Name = "ucSupComPtSearch1";
            this.ucSupComPtSearch1.pPSMH_LPoint = new System.Drawing.Point(0, 0);
            this.ucSupComPtSearch1.PSMH_TITLE_VISIBLE = true;
            this.ucSupComPtSearch1.PSMH_TYPE = ComSupLibB.UcSupComPtSearch.enmType.PTINFO;
            this.ucSupComPtSearch1.PSMH_UNITKEY = false;
            this.ucSupComPtSearch1.Size = new System.Drawing.Size(251, 30);
            this.ucSupComPtSearch1.TabIndex = 24;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Location = new System.Drawing.Point(1209, 1);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 23;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // txtJUMIN2
            // 
            this.txtJUMIN2.Location = new System.Drawing.Point(406, 7);
            this.txtJUMIN2.Name = "txtJUMIN2";
            this.txtJUMIN2.ReadOnly = true;
            this.txtJUMIN2.Size = new System.Drawing.Size(76, 21);
            this.txtJUMIN2.TabIndex = 22;
            this.txtJUMIN2.Text = "12345678";
            // 
            // txtJUMIN1
            // 
            this.txtJUMIN1.Location = new System.Drawing.Point(327, 7);
            this.txtJUMIN1.Name = "txtJUMIN1";
            this.txtJUMIN1.ReadOnly = true;
            this.txtJUMIN1.Size = new System.Drawing.Size(73, 21);
            this.txtJUMIN1.TabIndex = 14;
            this.txtJUMIN1.Text = "12345678";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(275, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 12;
            this.label9.Text = "주민번호";
            // 
            // frmComSupIjrmVACC01
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1284, 814);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.panTitle);
            this.Name = "frmComSupIjrmVACC01";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panMain.ResumeLayout(false);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panVaccine.ResumeLayout(false);
            this.panVaccine.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panMain;
        private DevComponents.DotNetBar.Controls.Line line1;
        private System.Windows.Forms.WebBrowser webBr;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panVaccine;
        private ComSupLibB.UcSupComPtSearch ucSupComPtSearch1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtJUMIN2;
        private System.Windows.Forms.TextBox txtJUMIN1;
        private System.Windows.Forms.Label label9;
        private DevComponents.DotNetBar.Controls.CircularProgress circProgress;
    }
}