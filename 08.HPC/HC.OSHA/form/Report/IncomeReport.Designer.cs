namespace HC_OSHA.form.Report
{
    partial class IncomeReport
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
            HC.OSHA.Model.HC_OSHA_SITE_MODEL hC_OSHA_SITE_MODEL1 = new HC.OSHA.Model.HC_OSHA_SITE_MODEL();
            this.SSList = new FarPoint.Win.Spread.FpSpread();
            this.SSList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panSearch = new System.Windows.Forms.Panel();
            this.ChkIsHisotry = new System.Windows.Forms.CheckBox();
            this.ChkAll = new System.Windows.Forms.CheckBox();
            this.DtpEnd = new System.Windows.Forms.DateTimePicker();
            this.BtnPrint = new System.Windows.Forms.Button();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.DtpStart = new System.Windows.Forms.DateTimePicker();
            this.label15 = new System.Windows.Forms.Label();
            this.TxtSiteName = new System.Windows.Forms.Label();
            this.formTItle1 = new ComBase.Mvc.UserControls.FormTItle();
            this.BtnClose = new System.Windows.Forms.Button();
            this.oshaSiteLastTree1 = new HC_OSHA.OshaSiteLastTree();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).BeginInit();
            this.panSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // SSList
            // 
            this.SSList.AccessibleDescription = "";
            this.SSList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSList.Location = new System.Drawing.Point(200, 79);
            this.SSList.Name = "SSList";
            this.SSList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSList_Sheet1});
            this.SSList.Size = new System.Drawing.Size(703, 371);
            this.SSList.TabIndex = 9;
            this.SSList.SetActiveViewport(0, -1, -1);
            // 
            // SSList_Sheet1
            // 
            this.SSList_Sheet1.Reset();
            this.SSList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSList_Sheet1.ColumnCount = 0;
            this.SSList_Sheet1.RowCount = 0;
            this.SSList_Sheet1.ActiveColumnIndex = -1;
            this.SSList_Sheet1.ActiveRowIndex = -1;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panSearch
            // 
            this.panSearch.BackColor = System.Drawing.Color.White;
            this.panSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSearch.Controls.Add(this.ChkIsHisotry);
            this.panSearch.Controls.Add(this.ChkAll);
            this.panSearch.Controls.Add(this.DtpEnd);
            this.panSearch.Controls.Add(this.BtnPrint);
            this.panSearch.Controls.Add(this.BtnSearch);
            this.panSearch.Controls.Add(this.DtpStart);
            this.panSearch.Controls.Add(this.label15);
            this.panSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSearch.Location = new System.Drawing.Point(0, 35);
            this.panSearch.Name = "panSearch";
            this.panSearch.Size = new System.Drawing.Size(903, 44);
            this.panSearch.TabIndex = 7;
            // 
            // ChkIsHisotry
            // 
            this.ChkIsHisotry.AutoSize = true;
            this.ChkIsHisotry.Location = new System.Drawing.Point(424, 11);
            this.ChkIsHisotry.Name = "ChkIsHisotry";
            this.ChkIsHisotry.Size = new System.Drawing.Size(125, 21);
            this.ChkIsHisotry.TabIndex = 89;
            this.ChkIsHisotry.Text = "Hisotry 내역포함";
            this.ChkIsHisotry.UseVisualStyleBackColor = true;
            // 
            // ChkAll
            // 
            this.ChkAll.AutoSize = true;
            this.ChkAll.Checked = true;
            this.ChkAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkAll.Location = new System.Drawing.Point(338, 12);
            this.ChkAll.Name = "ChkAll";
            this.ChkAll.Size = new System.Drawing.Size(53, 21);
            this.ChkAll.TabIndex = 88;
            this.ChkAll.Text = "전체";
            this.ChkAll.UseVisualStyleBackColor = true;
            this.ChkAll.CheckedChanged += new System.EventHandler(this.ChkAll_CheckedChanged);
            // 
            // DtpEnd
            // 
            this.DtpEnd.CustomFormat = "yyyy-MM-dd";
            this.DtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpEnd.Location = new System.Drawing.Point(205, 9);
            this.DtpEnd.Name = "DtpEnd";
            this.DtpEnd.Size = new System.Drawing.Size(107, 25);
            this.DtpEnd.TabIndex = 87;
            // 
            // BtnPrint
            // 
            this.BtnPrint.Location = new System.Drawing.Point(754, 10);
            this.BtnPrint.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnPrint.Name = "BtnPrint";
            this.BtnPrint.Size = new System.Drawing.Size(75, 28);
            this.BtnPrint.TabIndex = 85;
            this.BtnPrint.Text = "인쇄";
            this.BtnPrint.UseVisualStyleBackColor = true;
            this.BtnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(605, 7);
            this.BtnSearch.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(75, 28);
            this.BtnSearch.TabIndex = 80;
            this.BtnSearch.Text = "검색";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // DtpStart
            // 
            this.DtpStart.CustomFormat = "yyyy-MM-dd";
            this.DtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpStart.Location = new System.Drawing.Point(92, 9);
            this.DtpStart.Name = "DtpStart";
            this.DtpStart.Size = new System.Drawing.Size(107, 25);
            this.DtpStart.TabIndex = 84;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label15.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.Location = new System.Drawing.Point(11, 8);
            this.label15.Name = "label15";
            this.label15.Padding = new System.Windows.Forms.Padding(3);
            this.label15.Size = new System.Drawing.Size(75, 25);
            this.label15.TabIndex = 78;
            this.label15.Text = "회계일자";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TxtSiteName
            // 
            this.TxtSiteName.AutoSize = true;
            this.TxtSiteName.Location = new System.Drawing.Point(100, 9);
            this.TxtSiteName.Name = "TxtSiteName";
            this.TxtSiteName.Size = new System.Drawing.Size(60, 17);
            this.TxtSiteName.TabIndex = 89;
            this.TxtSiteName.Text = "사업장명";
            // 
            // formTItle1
            // 
            this.formTItle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.formTItle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.formTItle1.Location = new System.Drawing.Point(0, 0);
            this.formTItle1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.formTItle1.Name = "formTItle1";
            this.formTItle1.Size = new System.Drawing.Size(903, 35);
            this.formTItle1.TabIndex = 8;
            this.formTItle1.TitleText = "수입통계";
            // 
            // BtnClose
            // 
            this.BtnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnClose.Location = new System.Drawing.Point(818, 2);
            this.BtnClose.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(75, 28);
            this.BtnClose.TabIndex = 84;
            this.BtnClose.Text = "닫기";
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // oshaSiteLastTree1
            // 
            this.oshaSiteLastTree1.Dock = System.Windows.Forms.DockStyle.Left;
            this.oshaSiteLastTree1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            hC_OSHA_SITE_MODEL1.ADDRESS = null;
            hC_OSHA_SITE_MODEL1.BIZCREATEDATE = null;
            hC_OSHA_SITE_MODEL1.BIZJIDOWON = null;
            hC_OSHA_SITE_MODEL1.BIZJONG = null;
            hC_OSHA_SITE_MODEL1.BIZKIHO = null;
            hC_OSHA_SITE_MODEL1.BIZNUMBER = null;
            hC_OSHA_SITE_MODEL1.BIZTYPE = null;
            hC_OSHA_SITE_MODEL1.CEONAME = null;
            hC_OSHA_SITE_MODEL1.ComboDisplay = null;
            hC_OSHA_SITE_MODEL1.EMAIL = null;
            hC_OSHA_SITE_MODEL1.FAX = null;
            hC_OSHA_SITE_MODEL1.HASCHILD = null;
            hC_OSHA_SITE_MODEL1.ID = ((long)(0));
            hC_OSHA_SITE_MODEL1.INDUSTRIALNUMBER = null;
            hC_OSHA_SITE_MODEL1.INSURANCE = null;
            hC_OSHA_SITE_MODEL1.ISACTIVE = null;
            hC_OSHA_SITE_MODEL1.LABOR = null;
            hC_OSHA_SITE_MODEL1.LASTMODIFIED = null;
            hC_OSHA_SITE_MODEL1.NAME = null;
            hC_OSHA_SITE_MODEL1.PARENTSITE_ID = ((long)(0));
            hC_OSHA_SITE_MODEL1.PARENTSITE_NAME = null;
            hC_OSHA_SITE_MODEL1.RowStatus = ComBase.Mvc.RowStatus.None;
            hC_OSHA_SITE_MODEL1.TEL = null;
            hC_OSHA_SITE_MODEL1.zTemp1 = null;
            hC_OSHA_SITE_MODEL1.zTemp10 = null;
            hC_OSHA_SITE_MODEL1.zTemp11 = null;
            hC_OSHA_SITE_MODEL1.zTemp12 = null;
            hC_OSHA_SITE_MODEL1.zTemp13 = null;
            hC_OSHA_SITE_MODEL1.zTemp14 = null;
            hC_OSHA_SITE_MODEL1.zTemp15 = null;
            hC_OSHA_SITE_MODEL1.zTemp16 = null;
            hC_OSHA_SITE_MODEL1.zTemp17 = null;
            hC_OSHA_SITE_MODEL1.zTemp18 = null;
            hC_OSHA_SITE_MODEL1.zTemp19 = null;
            hC_OSHA_SITE_MODEL1.zTemp2 = null;
            hC_OSHA_SITE_MODEL1.zTemp20 = null;
            hC_OSHA_SITE_MODEL1.zTemp3 = null;
            hC_OSHA_SITE_MODEL1.zTemp4 = null;
            hC_OSHA_SITE_MODEL1.zTemp5 = null;
            hC_OSHA_SITE_MODEL1.zTemp6 = null;
            hC_OSHA_SITE_MODEL1.zTemp7 = null;
            hC_OSHA_SITE_MODEL1.zTemp8 = null;
            hC_OSHA_SITE_MODEL1.zTemp9 = null;
            this.oshaSiteLastTree1.GetSite = hC_OSHA_SITE_MODEL1;
            this.oshaSiteLastTree1.IsCheckbox = false;
            this.oshaSiteLastTree1.Location = new System.Drawing.Point(0, 79);
            this.oshaSiteLastTree1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.oshaSiteLastTree1.Name = "oshaSiteLastTree1";
            this.oshaSiteLastTree1.Size = new System.Drawing.Size(200, 371);
            this.oshaSiteLastTree1.TabIndex = 85;
            this.oshaSiteLastTree1.NodeClick += new HC_OSHA.OshaSiteLastTree.SiteTreeViewNodeMouseClickEventHandler(this.oshaSiteLastTree1_NodeClick);
            // 
            // IncomeReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(903, 450);
            this.Controls.Add(this.TxtSiteName);
            this.Controls.Add(this.SSList);
            this.Controls.Add(this.oshaSiteLastTree1);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.panSearch);
            this.Controls.Add(this.formTItle1);
            this.Name = "IncomeReport";
            this.Text = "수입일보";
            this.Load += new System.EventHandler(this.IncomeReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).EndInit();
            this.panSearch.ResumeLayout(false);
            this.panSearch.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FarPoint.Win.Spread.FpSpread SSList;
        private FarPoint.Win.Spread.SheetView SSList_Sheet1;
        private System.Windows.Forms.Panel panSearch;
        private System.Windows.Forms.DateTimePicker DtpEnd;
        private System.Windows.Forms.Button BtnPrint;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.DateTimePicker DtpStart;
        private System.Windows.Forms.Label label15;
        private ComBase.Mvc.UserControls.FormTItle formTItle1;
        private System.Windows.Forms.Button BtnClose;
        private OshaSiteLastTree oshaSiteLastTree1;
        private System.Windows.Forms.Label TxtSiteName;
        private System.Windows.Forms.CheckBox ChkAll;
        private System.Windows.Forms.CheckBox ChkIsHisotry;
    }
}