namespace HC_OSHA
{
    partial class FrmSahusogen
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
            FarPoint.Win.Spread.FlatFocusIndicatorRenderer flatFocusIndicatorRenderer1 = new FarPoint.Win.Spread.FlatFocusIndicatorRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer1 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer2 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSahusogen));
            this.panSearch = new System.Windows.Forms.Panel();
            this.cboJong = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtLtdcode = new System.Windows.Forms.TextBox();
            this.BtnSearchSite = new System.Windows.Forms.Button();
            this.lblLTD02 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboPanjeng = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.CboYear = new System.Windows.Forms.ComboBox();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.BtnExportExcel = new System.Windows.Forms.Button();
            this.panTitle = new System.Windows.Forms.Panel();
            this.LblSite = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SSHealthCheck = new FarPoint.Win.Spread.FpSpread();
            this.SSHealthCheck_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.btnDelete = new System.Windows.Forms.Button();
            this.panSearch.SuspendLayout();
            this.panTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSHealthCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSHealthCheck_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panSearch
            // 
            this.panSearch.Controls.Add(this.btnDelete);
            this.panSearch.Controls.Add(this.cboJong);
            this.panSearch.Controls.Add(this.label3);
            this.panSearch.Controls.Add(this.TxtLtdcode);
            this.panSearch.Controls.Add(this.BtnSearchSite);
            this.panSearch.Controls.Add(this.lblLTD02);
            this.panSearch.Controls.Add(this.txtName);
            this.panSearch.Controls.Add(this.label2);
            this.panSearch.Controls.Add(this.label1);
            this.panSearch.Controls.Add(this.cboPanjeng);
            this.panSearch.Controls.Add(this.label4);
            this.panSearch.Controls.Add(this.CboYear);
            this.panSearch.Controls.Add(this.BtnSearch);
            this.panSearch.Controls.Add(this.BtnExportExcel);
            this.panSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSearch.Location = new System.Drawing.Point(0, 54);
            this.panSearch.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.panSearch.Name = "panSearch";
            this.panSearch.Size = new System.Drawing.Size(1373, 46);
            this.panSearch.TabIndex = 30;
            this.panSearch.Paint += new System.Windows.Forms.PaintEventHandler(this.panSearch_Paint);
            // 
            // cboJong
            // 
            this.cboJong.FormattingEnabled = true;
            this.cboJong.Location = new System.Drawing.Point(551, 9);
            this.cboJong.Name = "cboJong";
            this.cboJong.Size = new System.Drawing.Size(91, 25);
            this.cboJong.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.LightBlue;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(487, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 23);
            this.label3.TabIndex = 143;
            this.label3.Text = "검진";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TxtLtdcode
            // 
            this.TxtLtdcode.Location = new System.Drawing.Point(253, 10);
            this.TxtLtdcode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TxtLtdcode.Name = "TxtLtdcode";
            this.TxtLtdcode.Size = new System.Drawing.Size(140, 25);
            this.TxtLtdcode.TabIndex = 1;
            // 
            // BtnSearchSite
            // 
            this.BtnSearchSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSearchSite.Location = new System.Drawing.Point(399, 11);
            this.BtnSearchSite.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearchSite.Name = "BtnSearchSite";
            this.BtnSearchSite.Size = new System.Drawing.Size(56, 26);
            this.BtnSearchSite.TabIndex = 1;
            this.BtnSearchSite.Text = "검색";
            this.BtnSearchSite.UseVisualStyleBackColor = true;
            this.BtnSearchSite.Click += new System.EventHandler(this.BtnSearchSite_Click);
            // 
            // lblLTD02
            // 
            this.lblLTD02.BackColor = System.Drawing.Color.LightBlue;
            this.lblLTD02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLTD02.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLTD02.Location = new System.Drawing.Point(189, 11);
            this.lblLTD02.Name = "lblLTD02";
            this.lblLTD02.Size = new System.Drawing.Size(58, 23);
            this.lblLTD02.TabIndex = 140;
            this.lblLTD02.Text = "회사";
            this.lblLTD02.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(877, 10);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(92, 25);
            this.txtName.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.LightBlue;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(817, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 28);
            this.label2.TabIndex = 138;
            this.label2.Text = "상명";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LightBlue;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(666, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 27);
            this.label1.TabIndex = 136;
            this.label1.Text = "판정";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboPanjeng
            // 
            this.cboPanjeng.FormattingEnabled = true;
            this.cboPanjeng.Location = new System.Drawing.Point(728, 12);
            this.cboPanjeng.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cboPanjeng.Name = "cboPanjeng";
            this.cboPanjeng.Size = new System.Drawing.Size(72, 25);
            this.cboPanjeng.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.LightBlue;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(12, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 27);
            this.label4.TabIndex = 134;
            this.label4.Text = "검진년도";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CboYear
            // 
            this.CboYear.FormattingEnabled = true;
            this.CboYear.Location = new System.Drawing.Point(89, 11);
            this.CboYear.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.CboYear.Name = "CboYear";
            this.CboYear.Size = new System.Drawing.Size(87, 25);
            this.CboYear.TabIndex = 0;
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(993, 9);
            this.BtnSearch.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(75, 30);
            this.BtnSearch.TabIndex = 5;
            this.BtnSearch.Text = "검색";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // BtnExportExcel
            // 
            this.BtnExportExcel.Location = new System.Drawing.Point(1074, 9);
            this.BtnExportExcel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.BtnExportExcel.Name = "BtnExportExcel";
            this.BtnExportExcel.Size = new System.Drawing.Size(79, 30);
            this.BtnExportExcel.TabIndex = 6;
            this.BtnExportExcel.Text = "엑셀파일";
            this.BtnExportExcel.UseVisualStyleBackColor = true;
            this.BtnExportExcel.Click += new System.EventHandler(this.BtnExportExcel_Click);
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panTitle.Controls.Add(this.LblSite);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1373, 54);
            this.panTitle.TabIndex = 29;
            // 
            // LblSite
            // 
            this.LblSite.AutoSize = true;
            this.LblSite.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LblSite.Location = new System.Drawing.Point(184, 16);
            this.LblSite.Name = "LblSite";
            this.LblSite.Size = new System.Drawing.Size(0, 21);
            this.LblSite.TabIndex = 30;
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(1289, 0);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(82, 52);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "닫 기(&X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(11, 16);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(122, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "사후관리소견서";
            // 
            // SSHealthCheck
            // 
            this.SSHealthCheck.AccessibleDescription = "SSHealthCheck, Sheet1, Row 0, Column 0, ";
            this.SSHealthCheck.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSHealthCheck.FocusRenderer = flatFocusIndicatorRenderer1;
            this.SSHealthCheck.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SSHealthCheck.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SSHealthCheck.HorizontalScrollBar.Renderer = flatScrollBarRenderer1;
            this.SSHealthCheck.HorizontalScrollBar.TabIndex = 191;
            this.SSHealthCheck.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SSHealthCheck.Location = new System.Drawing.Point(0, 100);
            this.SSHealthCheck.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.SSHealthCheck.Name = "SSHealthCheck";
            this.SSHealthCheck.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSHealthCheck_Sheet1});
            this.SSHealthCheck.Size = new System.Drawing.Size(1373, 732);
            this.SSHealthCheck.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.SSHealthCheck.TabIndex = 166;
            this.SSHealthCheck.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SSHealthCheck.VerticalScrollBar.Name = "";
            flatScrollBarRenderer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SSHealthCheck.VerticalScrollBar.Renderer = flatScrollBarRenderer2;
            this.SSHealthCheck.VerticalScrollBar.TabIndex = 192;
            this.SSHealthCheck.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SSHealthCheck.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSHealthCheck_CellClick);
            this.SSHealthCheck.SetViewportTopRow(0, 0, 3);
            // 
            // SSHealthCheck_Sheet1
            // 
            this.SSHealthCheck_Sheet1.Reset();
            this.SSHealthCheck_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSHealthCheck_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSHealthCheck_Sheet1.ColumnCount = 15;
            this.SSHealthCheck_Sheet1.RowCount = 50;
            this.SSHealthCheck_Sheet1.Models = ((FarPoint.Win.Spread.SheetView.DocumentModels)(resources.GetObject("SSHealthCheck_Sheet1.Models")));
            this.SSHealthCheck_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(1261, 10);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 30);
            this.btnDelete.TabIndex = 144;
            this.btnDelete.Text = "선택한것 삭제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // FrmSahusogen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1373, 832);
            this.Controls.Add(this.SSHealthCheck);
            this.Controls.Add(this.panSearch);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmSahusogen";
            this.Text = "사후관리소견서";
            this.panSearch.ResumeLayout(false);
            this.panSearch.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSHealthCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSHealthCheck_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panSearch;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.Button BtnExportExcel;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label LblSite;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ComboBox CboYear;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboPanjeng;
        private System.Windows.Forms.Label label4;
        private FarPoint.Win.Spread.FpSpread SSHealthCheck;
        private FarPoint.Win.Spread.SheetView SSHealthCheck_Sheet1;
        private System.Windows.Forms.TextBox TxtLtdcode;
        private System.Windows.Forms.Button BtnSearchSite;
        private System.Windows.Forms.Label lblLTD02;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboJong;
        private System.Windows.Forms.Button btnDelete;
    }
}