namespace ComSupLibB.SupLbEx
{
    partial class frmComSupLbExVIEW04
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdoNoGrowh = new System.Windows.Forms.RadioButton();
            this.rdoGrowth = new System.Windows.Forms.RadioButton();
            this.rdoGrowthAll = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoO = new System.Windows.Forms.RadioButton();
            this.rdoI = new System.Windows.Forms.RadioButton();
            this.rdoIOAll = new System.Windows.Forms.RadioButton();
            this.userPtInfo = new ComSupLibB.UcSupComPtSearch();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dtpTDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpFDate = new System.Windows.Forms.DateTimePicker();
            this.lblDateTitle = new System.Windows.Forms.Label();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panMain = new System.Windows.Forms.Panel();
            this.ssMain = new FarPoint.Win.Spread.FpSpread();
            this.ssMain_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMain_Sheet1)).BeginInit();
            this.SuspendLayout();
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
            this.panTitle.Size = new System.Drawing.Size(1079, 34);
            this.panTitle.TabIndex = 13;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(1006, 1);
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
            this.lblTitle.Size = new System.Drawing.Size(135, 16);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "MIC 검사 리스트";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.userPtInfo);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.dtpTDate);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.dtpFDate);
            this.panel1.Controls.Add(this.lblDateTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(1);
            this.panel1.Size = new System.Drawing.Size(1079, 50);
            this.panel1.TabIndex = 88;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdoNoGrowh);
            this.groupBox2.Controls.Add(this.rdoGrowth);
            this.groupBox2.Controls.Add(this.rdoGrowthAll);
            this.groupBox2.Location = new System.Drawing.Point(763, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(213, 41);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "결과";
            // 
            // rdoNoGrowh
            // 
            this.rdoNoGrowh.AutoSize = true;
            this.rdoNoGrowh.Location = new System.Drawing.Point(124, 17);
            this.rdoNoGrowh.Name = "rdoNoGrowh";
            this.rdoNoGrowh.Size = new System.Drawing.Size(83, 16);
            this.rdoNoGrowh.TabIndex = 2;
            this.rdoNoGrowh.Text = "No Growth";
            this.rdoNoGrowh.UseVisualStyleBackColor = true;
            // 
            // rdoGrowth
            // 
            this.rdoGrowth.AutoSize = true;
            this.rdoGrowth.Location = new System.Drawing.Point(59, 17);
            this.rdoGrowth.Name = "rdoGrowth";
            this.rdoGrowth.Size = new System.Drawing.Size(63, 16);
            this.rdoGrowth.TabIndex = 1;
            this.rdoGrowth.Text = "Growth";
            this.rdoGrowth.UseVisualStyleBackColor = true;
            // 
            // rdoGrowthAll
            // 
            this.rdoGrowthAll.AutoSize = true;
            this.rdoGrowthAll.Checked = true;
            this.rdoGrowthAll.Location = new System.Drawing.Point(9, 17);
            this.rdoGrowthAll.Name = "rdoGrowthAll";
            this.rdoGrowthAll.Size = new System.Drawing.Size(47, 16);
            this.rdoGrowthAll.TabIndex = 0;
            this.rdoGrowthAll.TabStop = true;
            this.rdoGrowthAll.Text = "전체";
            this.rdoGrowthAll.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoO);
            this.groupBox1.Controls.Add(this.rdoI);
            this.groupBox1.Controls.Add(this.rdoIOAll);
            this.groupBox1.Location = new System.Drawing.Point(582, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(177, 41);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "입원/외래";
            // 
            // rdoO
            // 
            this.rdoO.AutoSize = true;
            this.rdoO.Location = new System.Drawing.Point(108, 17);
            this.rdoO.Name = "rdoO";
            this.rdoO.Size = new System.Drawing.Size(47, 16);
            this.rdoO.TabIndex = 2;
            this.rdoO.Text = "외래";
            this.rdoO.UseVisualStyleBackColor = true;
            // 
            // rdoI
            // 
            this.rdoI.AutoSize = true;
            this.rdoI.Location = new System.Drawing.Point(59, 17);
            this.rdoI.Name = "rdoI";
            this.rdoI.Size = new System.Drawing.Size(47, 16);
            this.rdoI.TabIndex = 1;
            this.rdoI.Text = "입원";
            this.rdoI.UseVisualStyleBackColor = true;
            // 
            // rdoIOAll
            // 
            this.rdoIOAll.AutoSize = true;
            this.rdoIOAll.Checked = true;
            this.rdoIOAll.Location = new System.Drawing.Point(9, 17);
            this.rdoIOAll.Name = "rdoIOAll";
            this.rdoIOAll.Size = new System.Drawing.Size(47, 16);
            this.rdoIOAll.TabIndex = 0;
            this.rdoIOAll.TabStop = true;
            this.rdoIOAll.Text = "전체";
            this.rdoIOAll.UseVisualStyleBackColor = true;
            // 
            // userPtInfo
            // 
            this.userPtInfo.BackColor = System.Drawing.Color.White;
            this.userPtInfo.Location = new System.Drawing.Point(292, 9);
            this.userPtInfo.pPSMH_LPoint = new System.Drawing.Point(0, 0);
            this.userPtInfo.Name = "userPtInfo";
            this.userPtInfo.PSMH_TYPE = ComSupLibB.UcSupComPtSearch.enmType.PTINFO;
            this.userPtInfo.Size = new System.Drawing.Size(286, 29);
            this.userPtInfo.TabIndex = 21;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnSearch);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(998, 1);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5, 8, 1, 8);
            this.panel2.Size = new System.Drawing.Size(80, 48);
            this.panel2.TabIndex = 20;
            // 
            // btnSearch
            // 
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Location = new System.Drawing.Point(7, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 32);
            this.btnSearch.TabIndex = 15;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // dtpTDate
            // 
            this.dtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTDate.Location = new System.Drawing.Point(185, 14);
            this.dtpTDate.Name = "dtpTDate";
            this.dtpTDate.Size = new System.Drawing.Size(106, 21);
            this.dtpTDate.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(160, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = " ~ ";
            // 
            // dtpFDate
            // 
            this.dtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFDate.Location = new System.Drawing.Point(63, 14);
            this.dtpFDate.Name = "dtpFDate";
            this.dtpFDate.Size = new System.Drawing.Size(95, 21);
            this.dtpFDate.TabIndex = 1;
            // 
            // lblDateTitle
            // 
            this.lblDateTitle.AutoSize = true;
            this.lblDateTitle.Location = new System.Drawing.Point(11, 19);
            this.lblDateTitle.Name = "lblDateTitle";
            this.lblDateTitle.Size = new System.Drawing.Size(53, 12);
            this.lblDateTitle.TabIndex = 0;
            this.lblDateTitle.Text = "결과일자";
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 84);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(1079, 28);
            this.panTitleSub0.TabIndex = 89;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 6);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(70, 12);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "리스트내역";
            // 
            // panMain
            // 
            this.panMain.Controls.Add(this.ssMain);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(0, 112);
            this.panMain.Name = "panMain";
            this.panMain.Padding = new System.Windows.Forms.Padding(1);
            this.panMain.Size = new System.Drawing.Size(1079, 667);
            this.panMain.TabIndex = 90;
            // 
            // ssMain
            // 
            this.ssMain.AccessibleDescription = "";
            this.ssMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssMain.Location = new System.Drawing.Point(1, 1);
            this.ssMain.Name = "ssMain";
            this.ssMain.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssMain_Sheet1});
            this.ssMain.Size = new System.Drawing.Size(1077, 665);
            this.ssMain.TabIndex = 0;
            // 
            // ssMain_Sheet1
            // 
            this.ssMain_Sheet1.Reset();
            this.ssMain_Sheet1.SheetName = "Sheet1";
            // 
            // frmComSupLbExMIC01
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1079, 779);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Name = "frmComSupLbExMIC01";
            this.Text = "frmComSupLbExMIC01";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMain_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DateTimePicker dtpTDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpFDate;
        private System.Windows.Forms.Label lblDateTitle;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdoNoGrowh;
        private System.Windows.Forms.RadioButton rdoGrowth;
        private System.Windows.Forms.RadioButton rdoGrowthAll;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoO;
        private System.Windows.Forms.RadioButton rdoI;
        private System.Windows.Forms.RadioButton rdoIOAll;
        private UcSupComPtSearch userPtInfo;
        private System.Windows.Forms.Panel panMain;
        private FarPoint.Win.Spread.FpSpread ssMain;
        private FarPoint.Win.Spread.SheetView ssMain_Sheet1;
        private System.Windows.Forms.Button btnSearch;
    }
}