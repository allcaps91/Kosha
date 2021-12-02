namespace HC_OSHA
{
    partial class SiteManagementCardForm
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
            HC.OSHA.Model.HC_ESTIMATE_MODEL hC_ESTIMATE_MODEL1 = new HC.OSHA.Model.HC_ESTIMATE_MODEL();
            HC.OSHA.Model.HC_OSHA_SITE_MODEL hC_OSHA_SITE_MODEL1 = new HC.OSHA.Model.HC_OSHA_SITE_MODEL();
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType1 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType13 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType14 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType15 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType16 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType17 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType2 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType3 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType18 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.formTItle1 = new ComBase.Mvc.UserControls.FormTItle();
            this.oshaSiteEstimateList1 = new HC_OSHA.OshaSiteEstimateList();
            this.oshaSiteList1 = new HC_OSHA.OshaSiteList();
            this.SSCardList = new FarPoint.Win.Spread.FpSpread();
            this.SSCardList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panBody = new System.Windows.Forms.Panel();
            this.panFrame = new System.Windows.Forms.Panel();
            this.panCardList = new System.Windows.Forms.Panel();
            this.panCardListPrint = new System.Windows.Forms.Panel();
            this.BtnPrint = new System.Windows.Forms.Button();
            this.panLeft = new System.Windows.Forms.Panel();
            this.horizSpace1 = new ComBase.Mvc.UserControls.HorizSpace();
            this.lblSiteName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSCardList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSCardList_Sheet1)).BeginInit();
            this.panBody.SuspendLayout();
            this.panCardList.SuspendLayout();
            this.panCardListPrint.SuspendLayout();
            this.panLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // formTItle1
            // 
            this.formTItle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.formTItle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.formTItle1.Location = new System.Drawing.Point(0, 0);
            this.formTItle1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.formTItle1.Name = "formTItle1";
            this.formTItle1.Size = new System.Drawing.Size(1264, 35);
            this.formTItle1.TabIndex = 0;
            this.formTItle1.TitleText = "사업장관리카드";
            // 
            // oshaSiteEstimateList1
            // 
            this.oshaSiteEstimateList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.oshaSiteEstimateList1.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            hC_ESTIMATE_MODEL1.ComboDisplay = null;
            hC_ESTIMATE_MODEL1.CONTRACTDATE = null;
            hC_ESTIMATE_MODEL1.CONTRACTENDDATE = null;
            hC_ESTIMATE_MODEL1.ContractPeriod = null;
            hC_ESTIMATE_MODEL1.CONTRACTSTARTDATE = null;
            hC_ESTIMATE_MODEL1.ESTIMATEDATE = null;
            hC_ESTIMATE_MODEL1.ID = ((long)(0));
            hC_ESTIMATE_MODEL1.ISCONTRACT = null;
            hC_ESTIMATE_MODEL1.RowStatus = ComBase.Mvc.RowStatus.None;
            hC_ESTIMATE_MODEL1.zTemp1 = null;
            hC_ESTIMATE_MODEL1.zTemp10 = null;
            hC_ESTIMATE_MODEL1.zTemp11 = null;
            hC_ESTIMATE_MODEL1.zTemp12 = null;
            hC_ESTIMATE_MODEL1.zTemp13 = null;
            hC_ESTIMATE_MODEL1.zTemp14 = null;
            hC_ESTIMATE_MODEL1.zTemp15 = null;
            hC_ESTIMATE_MODEL1.zTemp16 = null;
            hC_ESTIMATE_MODEL1.zTemp17 = null;
            hC_ESTIMATE_MODEL1.zTemp18 = null;
            hC_ESTIMATE_MODEL1.zTemp19 = null;
            hC_ESTIMATE_MODEL1.zTemp2 = null;
            hC_ESTIMATE_MODEL1.zTemp20 = null;
            hC_ESTIMATE_MODEL1.zTemp3 = null;
            hC_ESTIMATE_MODEL1.zTemp4 = null;
            hC_ESTIMATE_MODEL1.zTemp5 = null;
            hC_ESTIMATE_MODEL1.zTemp6 = null;
            hC_ESTIMATE_MODEL1.zTemp7 = null;
            hC_ESTIMATE_MODEL1.zTemp8 = null;
            hC_ESTIMATE_MODEL1.zTemp9 = null;
            this.oshaSiteEstimateList1.GetEstimateModel = hC_ESTIMATE_MODEL1;
            this.oshaSiteEstimateList1.Location = new System.Drawing.Point(0, 474);
            this.oshaSiteEstimateList1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.oshaSiteEstimateList1.Name = "oshaSiteEstimateList1";
            this.oshaSiteEstimateList1.Size = new System.Drawing.Size(194, 352);
            this.oshaSiteEstimateList1.TabIndex = 4;
            this.oshaSiteEstimateList1.CellDoubleClick += new HC_OSHA.OshaSiteEstimateList.CellDoubleClickEventHandler(this.oshaSiteEstimateList1_CellDoubleClick);
            // 
            // oshaSiteList1
            // 
            this.oshaSiteList1.Dock = System.Windows.Forms.DockStyle.Top;
            this.oshaSiteList1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
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
            hC_OSHA_SITE_MODEL1.MANAGEDOCTORCOUNT = ((long)(0));
            hC_OSHA_SITE_MODEL1.MANAGEDOCTORSTARTDATE = null;
            hC_OSHA_SITE_MODEL1.MANAGEENGINEERCOUNT = ((long)(0));
            hC_OSHA_SITE_MODEL1.MANAGEENGINEERSTARTDATE = null;
            hC_OSHA_SITE_MODEL1.MANAGENURSECOUNT = ((long)(0));
            hC_OSHA_SITE_MODEL1.MANAGENURSESTARTDATE = null;
            hC_OSHA_SITE_MODEL1.NAME = null;
            hC_OSHA_SITE_MODEL1.PARENTSITE_ID = ((long)(0));
            hC_OSHA_SITE_MODEL1.PARENTSITE_NAME = null;
            hC_OSHA_SITE_MODEL1.RowStatus = ComBase.Mvc.RowStatus.None;
            hC_OSHA_SITE_MODEL1.TEL = null;
            hC_OSHA_SITE_MODEL1.VISITDAY = ((long)(0));
            hC_OSHA_SITE_MODEL1.VISITWEEK = ((long)(0));
            hC_OSHA_SITE_MODEL1.WORKERTOTALCOUNT = ((long)(0));
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
            this.oshaSiteList1.GetSite = hC_OSHA_SITE_MODEL1;
            this.oshaSiteList1.Location = new System.Drawing.Point(0, 0);
            this.oshaSiteList1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.oshaSiteList1.Name = "oshaSiteList1";
            this.oshaSiteList1.Size = new System.Drawing.Size(194, 469);
            this.oshaSiteList1.TabIndex = 0;
            this.oshaSiteList1.CellDoubleClick += new HC_OSHA.OshaSiteList.CellDoubleClickEventHandler(this.oshaSiteList1_CellDoubleClick);
            // 
            // SSCardList
            // 
            this.SSCardList.AccessibleDescription = "SSCardList, Sheet1, Row 0, Column 0, False";
            this.SSCardList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSCardList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSCardList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SSCardList.Location = new System.Drawing.Point(5, 36);
            this.SSCardList.Name = "SSCardList";
            this.SSCardList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSCardList_Sheet1});
            this.SSCardList.Size = new System.Drawing.Size(241, 790);
            this.SSCardList.TabIndex = 0;
            this.SSCardList.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SSCardList.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSCardList_CellClick);
            this.SSCardList.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSCardList_CellDoubleClick);
            // 
            // SSCardList_Sheet1
            // 
            this.SSCardList_Sheet1.Reset();
            this.SSCardList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSCardList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSCardList_Sheet1.ColumnCount = 2;
            this.SSCardList_Sheet1.RowCount = 17;
            this.SSCardList_Sheet1.Cells.Get(0, 0).CellType = checkBoxCellType1;
            this.SSCardList_Sheet1.Cells.Get(0, 0).Value = false;
            textCellType1.Multiline = true;
            this.SSCardList_Sheet1.Cells.Get(0, 1).CellType = textCellType1;
            this.SSCardList_Sheet1.Cells.Get(0, 1).Locked = true;
            this.SSCardList_Sheet1.Cells.Get(0, 1).Value = "1. 사업자현황\r\n2. 안전,보건 담당자\r\n3. 관리책임자등 선임현황";
            textCellType2.Multiline = true;
            this.SSCardList_Sheet1.Cells.Get(1, 1).CellType = textCellType2;
            this.SSCardList_Sheet1.Cells.Get(1, 1).Locked = true;
            this.SSCardList_Sheet1.Cells.Get(1, 1).Value = "4. 업무(작업) 개요(1)";
            textCellType3.Multiline = true;
            this.SSCardList_Sheet1.Cells.Get(2, 1).CellType = textCellType3;
            this.SSCardList_Sheet1.Cells.Get(2, 1).Locked = true;
            this.SSCardList_Sheet1.Cells.Get(2, 1).Value = "4. 업무(작업) 개요(2)";
            textCellType4.Multiline = true;
            this.SSCardList_Sheet1.Cells.Get(3, 1).CellType = textCellType4;
            this.SSCardList_Sheet1.Cells.Get(3, 1).Locked = true;
            this.SSCardList_Sheet1.Cells.Get(3, 1).Value = "5. 입퇴사자 현황\r\n6. 산업재해 발생현황";
            this.SSCardList_Sheet1.Cells.Get(4, 1).CellType = textCellType5;
            this.SSCardList_Sheet1.Cells.Get(4, 1).Locked = true;
            this.SSCardList_Sheet1.Cells.Get(4, 1).Value = "   재해자별 현황";
            textCellType6.Multiline = true;
            this.SSCardList_Sheet1.Cells.Get(5, 1).CellType = textCellType6;
            this.SSCardList_Sheet1.Cells.Get(5, 1).Locked = true;
            this.SSCardList_Sheet1.Cells.Get(5, 1).Value = "7. 안전보건관리규정 제개정내용\r\n8. 산업안전보건위원회 운영";
            textCellType7.Multiline = true;
            this.SSCardList_Sheet1.Cells.Get(6, 1).CellType = textCellType7;
            this.SSCardList_Sheet1.Cells.Get(6, 1).Locked = true;
            this.SSCardList_Sheet1.Cells.Get(6, 1).Value = "9. 위험성평가\r\n10. 무재해운동추진";
            textCellType8.Multiline = true;
            this.SSCardList_Sheet1.Cells.Get(7, 1).CellType = textCellType8;
            this.SSCardList_Sheet1.Cells.Get(7, 1).Locked = true;
            this.SSCardList_Sheet1.Cells.Get(7, 1).Value = "11. 근로자 건강증진운동";
            textCellType9.Multiline = true;
            this.SSCardList_Sheet1.Cells.Get(8, 1).CellType = textCellType9;
            this.SSCardList_Sheet1.Cells.Get(8, 1).Locked = true;
            this.SSCardList_Sheet1.Cells.Get(8, 1).Value = "12. 위험기계기구 방호조치\r\n13. 보호구";
            textCellType10.Multiline = true;
            this.SSCardList_Sheet1.Cells.Get(9, 1).CellType = textCellType10;
            this.SSCardList_Sheet1.Cells.Get(9, 1).Locked = true;
            this.SSCardList_Sheet1.Cells.Get(9, 1).Value = "14. 안전검사";
            textCellType11.Multiline = true;
            this.SSCardList_Sheet1.Cells.Get(10, 1).CellType = textCellType11;
            this.SSCardList_Sheet1.Cells.Get(10, 1).Locked = true;
            this.SSCardList_Sheet1.Cells.Get(10, 1).Value = "15. 위험물질";
            textCellType12.Multiline = true;
            this.SSCardList_Sheet1.Cells.Get(11, 1).CellType = textCellType12;
            this.SSCardList_Sheet1.Cells.Get(11, 1).Locked = true;
            this.SSCardList_Sheet1.Cells.Get(11, 1).Value = "16. 유해물질";
            textCellType13.Multiline = true;
            this.SSCardList_Sheet1.Cells.Get(12, 1).CellType = textCellType13;
            this.SSCardList_Sheet1.Cells.Get(12, 1).Locked = true;
            this.SSCardList_Sheet1.Cells.Get(12, 1).Value = "17. 사업장 안전보건교육";
            textCellType14.Multiline = true;
            this.SSCardList_Sheet1.Cells.Get(13, 1).CellType = textCellType14;
            this.SSCardList_Sheet1.Cells.Get(13, 1).Locked = true;
            this.SSCardList_Sheet1.Cells.Get(13, 1).Value = "18. 근로자 건강상담";
            textCellType15.Multiline = true;
            this.SSCardList_Sheet1.Cells.Get(14, 1).CellType = textCellType15;
            this.SSCardList_Sheet1.Cells.Get(14, 1).Locked = true;
            this.SSCardList_Sheet1.Cells.Get(14, 1).Value = "19. 위탁업무수행 일지(총괄)";
            textCellType16.Multiline = true;
            this.SSCardList_Sheet1.Cells.Get(15, 1).CellType = textCellType16;
            this.SSCardList_Sheet1.Cells.Get(15, 1).Locked = true;
            this.SSCardList_Sheet1.Cells.Get(15, 1).Value = "20. 안전보건관리전문기관에\r\n대한 사업장의 만족";
            textCellType17.Multiline = true;
            this.SSCardList_Sheet1.Cells.Get(16, 1).CellType = textCellType17;
            this.SSCardList_Sheet1.Cells.Get(16, 1).Locked = true;
            this.SSCardList_Sheet1.Cells.Get(16, 1).Value = "21. 응급의료체계";
            this.SSCardList_Sheet1.ColumnHeader.Cells.Get(0, 0).CellType = checkBoxCellType2;
            this.SSCardList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "페이지";
            this.SSCardList_Sheet1.ColumnHeader.Rows.Get(0).Height = 37F;
            this.SSCardList_Sheet1.Columns.Get(0).CellType = checkBoxCellType3;
            this.SSCardList_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSCardList_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSCardList_Sheet1.Columns.Get(0).Width = 31F;
            textCellType18.Multiline = true;
            textCellType18.ReadOnly = true;
            this.SSCardList_Sheet1.Columns.Get(1).CellType = textCellType18;
            this.SSCardList_Sheet1.Columns.Get(1).Label = "페이지";
            this.SSCardList_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSCardList_Sheet1.Columns.Get(1).Width = 206F;
            this.SSCardList_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            this.SSCardList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSCardList_Sheet1.RowHeader.Visible = false;
            this.SSCardList_Sheet1.Rows.Get(0).Height = 65F;
            this.SSCardList_Sheet1.Rows.Get(3).Height = 52F;
            this.SSCardList_Sheet1.Rows.Get(4).Height = 31F;
            this.SSCardList_Sheet1.Rows.Get(5).Height = 59F;
            this.SSCardList_Sheet1.Rows.Get(6).Height = 54F;
            this.SSCardList_Sheet1.Rows.Get(7).Height = 32F;
            this.SSCardList_Sheet1.Rows.Get(8).Height = 58F;
            this.SSCardList_Sheet1.Rows.Get(9).Height = 30F;
            this.SSCardList_Sheet1.Rows.Get(10).Height = 30F;
            this.SSCardList_Sheet1.Rows.Get(11).Height = 30F;
            this.SSCardList_Sheet1.Rows.Get(12).Height = 30F;
            this.SSCardList_Sheet1.Rows.Get(13).Height = 30F;
            this.SSCardList_Sheet1.Rows.Get(14).Height = 30F;
            this.SSCardList_Sheet1.Rows.Get(15).Height = 46F;
            this.SSCardList_Sheet1.Rows.Get(16).Height = 32F;
            this.SSCardList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panBody
            // 
            this.panBody.BackColor = System.Drawing.SystemColors.Control;
            this.panBody.Controls.Add(this.panFrame);
            this.panBody.Controls.Add(this.panCardList);
            this.panBody.Controls.Add(this.panLeft);
            this.panBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBody.Location = new System.Drawing.Point(0, 35);
            this.panBody.Name = "panBody";
            this.panBody.Padding = new System.Windows.Forms.Padding(5);
            this.panBody.Size = new System.Drawing.Size(1264, 836);
            this.panBody.TabIndex = 2;
            // 
            // panFrame
            // 
            this.panFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panFrame.Location = new System.Drawing.Point(445, 5);
            this.panFrame.Name = "panFrame";
            this.panFrame.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.panFrame.Size = new System.Drawing.Size(814, 826);
            this.panFrame.TabIndex = 7;
            // 
            // panCardList
            // 
            this.panCardList.Controls.Add(this.SSCardList);
            this.panCardList.Controls.Add(this.panCardListPrint);
            this.panCardList.Dock = System.Windows.Forms.DockStyle.Left;
            this.panCardList.Location = new System.Drawing.Point(199, 5);
            this.panCardList.Name = "panCardList";
            this.panCardList.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.panCardList.Size = new System.Drawing.Size(246, 826);
            this.panCardList.TabIndex = 6;
            // 
            // panCardListPrint
            // 
            this.panCardListPrint.Controls.Add(this.BtnPrint);
            this.panCardListPrint.Dock = System.Windows.Forms.DockStyle.Top;
            this.panCardListPrint.Location = new System.Drawing.Point(5, 0);
            this.panCardListPrint.Name = "panCardListPrint";
            this.panCardListPrint.Size = new System.Drawing.Size(241, 36);
            this.panCardListPrint.TabIndex = 6;
            // 
            // BtnPrint
            // 
            this.BtnPrint.Location = new System.Drawing.Point(171, 4);
            this.BtnPrint.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.BtnPrint.Name = "BtnPrint";
            this.BtnPrint.Size = new System.Drawing.Size(65, 27);
            this.BtnPrint.TabIndex = 5;
            this.BtnPrint.Text = "인쇄(&P)";
            this.BtnPrint.UseVisualStyleBackColor = true;
            this.BtnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // panLeft
            // 
            this.panLeft.Controls.Add(this.oshaSiteEstimateList1);
            this.panLeft.Controls.Add(this.horizSpace1);
            this.panLeft.Controls.Add(this.oshaSiteList1);
            this.panLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panLeft.Location = new System.Drawing.Point(5, 5);
            this.panLeft.Name = "panLeft";
            this.panLeft.Size = new System.Drawing.Size(194, 826);
            this.panLeft.TabIndex = 5;
            // 
            // horizSpace1
            // 
            this.horizSpace1.Dock = System.Windows.Forms.DockStyle.Top;
            this.horizSpace1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.horizSpace1.Location = new System.Drawing.Point(0, 469);
            this.horizSpace1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.horizSpace1.Name = "horizSpace1";
            this.horizSpace1.Size = new System.Drawing.Size(194, 5);
            this.horizSpace1.TabIndex = 5;
            // 
            // lblSiteName
            // 
            this.lblSiteName.AutoSize = true;
            this.lblSiteName.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSiteName.Location = new System.Drawing.Point(206, 6);
            this.lblSiteName.Name = "lblSiteName";
            this.lblSiteName.Size = new System.Drawing.Size(96, 21);
            this.lblSiteName.TabIndex = 4;
            this.lblSiteName.Text = "사업장 이름";
            // 
            // SiteManagementCardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 871);
            this.Controls.Add(this.lblSiteName);
            this.Controls.Add(this.panBody);
            this.Controls.Add(this.formTItle1);
            this.Name = "SiteManagementCardForm";
            this.Text = "사업장 관리 카드";
            this.Load += new System.EventHandler(this.SiteManagementCardForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSCardList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSCardList_Sheet1)).EndInit();
            this.panBody.ResumeLayout(false);
            this.panCardList.ResumeLayout(false);
            this.panCardListPrint.ResumeLayout(false);
            this.panLeft.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComBase.Mvc.UserControls.FormTItle formTItle1;
        private HC_OSHA.OshaSiteEstimateList oshaSiteEstimateList1;
        private HC_OSHA.OshaSiteList oshaSiteList1;
        private FarPoint.Win.Spread.FpSpread SSCardList;
        private FarPoint.Win.Spread.SheetView SSCardList_Sheet1;
        private System.Windows.Forms.Panel panBody;
        private System.Windows.Forms.Panel panFrame;
        private System.Windows.Forms.Panel panCardList;
        private System.Windows.Forms.Panel panLeft;
        private ComBase.Mvc.UserControls.HorizSpace horizSpace1;
        private System.Windows.Forms.Button BtnPrint;
        private System.Windows.Forms.Panel panCardListPrint;
        private System.Windows.Forms.Label lblSiteName;
    }
}