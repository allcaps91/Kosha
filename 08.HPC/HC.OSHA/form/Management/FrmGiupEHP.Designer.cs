namespace HC_OSHA
{
    partial class FrmGiupEHP
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
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("ko-KR", false);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGiupEHP));
            FarPoint.Win.Spread.CellType.TextCellType textCellType16 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType17 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType18 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType19 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType20 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType21 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType22 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType23 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType24 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType25 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType26 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType27 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType28 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType29 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType30 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.TxtLtdcode = new System.Windows.Forms.TextBox();
            this.BtnSearchSite = new System.Windows.Forms.Button();
            this.SSExcel = new FarPoint.Win.Spread.FpSpread();
            this.SSExcel_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.SS1 = new FarPoint.Win.Spread.FpSpread();
            this.SS1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnChange = new System.Windows.Forms.Button();
            this.btnExcel = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.SSList = new FarPoint.Win.Spread.FpSpread();
            this.SSList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnExcelGet = new System.Windows.Forms.Button();
            this.btnMail = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.SSExcel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSExcel_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // TxtLtdcode
            // 
            this.TxtLtdcode.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TxtLtdcode.Location = new System.Drawing.Point(12, 7);
            this.TxtLtdcode.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.TxtLtdcode.Name = "TxtLtdcode";
            this.TxtLtdcode.Size = new System.Drawing.Size(153, 25);
            this.TxtLtdcode.TabIndex = 167;
            // 
            // BtnSearchSite
            // 
            this.BtnSearchSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSearchSite.Location = new System.Drawing.Point(171, 3);
            this.BtnSearchSite.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            this.BtnSearchSite.Name = "BtnSearchSite";
            this.BtnSearchSite.Size = new System.Drawing.Size(62, 29);
            this.BtnSearchSite.TabIndex = 166;
            this.BtnSearchSite.Text = "회사찾기";
            this.BtnSearchSite.UseVisualStyleBackColor = true;
            this.BtnSearchSite.Click += new System.EventHandler(this.BtnSearchSite_Click);
            // 
            // SSExcel
            // 
            this.SSExcel.AccessibleDescription = "SSOshaList, Sheet1, Row 0, Column 0, HIC_USERS";
            this.SSExcel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSExcel.Location = new System.Drawing.Point(12, 157);
            this.SSExcel.Name = "SSExcel";
            this.SSExcel.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSExcel_Sheet1});
            this.SSExcel.Size = new System.Drawing.Size(852, 682);
            this.SSExcel.TabIndex = 160;
            // 
            // SSExcel_Sheet1
            // 
            this.SSExcel_Sheet1.Reset();
            this.SSExcel_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSExcel_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSExcel_Sheet1.ColumnCount = 0;
            this.SSExcel_Sheet1.RowCount = 0;
            this.SSExcel_Sheet1.ActiveColumnIndex = -1;
            this.SSExcel_Sheet1.ActiveRowIndex = -1;
            this.SSExcel_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSExcel_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSExcel_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSExcel_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSExcel_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSExcel_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSExcel_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSExcel_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSExcel_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSExcel_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // SS1
            // 
            this.SS1.AccessibleDescription = "SS1, Sheet1, Row 0, Column 0, 2022-05-23";
            this.SS1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SS1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.SS1.Location = new System.Drawing.Point(171, 43);
            this.SS1.Name = "SS1";
            this.SS1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS1_Sheet1});
            this.SS1.Size = new System.Drawing.Size(692, 73);
            this.SS1.TabIndex = 171;
            // 
            // SS1_Sheet1
            // 
            this.SS1_Sheet1.Reset();
            this.SS1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS1_Sheet1.ColumnCount = 14;
            this.SS1_Sheet1.RowCount = 1;
            this.SS1_Sheet1.Cells.Get(0, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Cells.Get(0, 0).ParseFormatInfo = ((System.Globalization.DateTimeFormatInfo)(cultureInfo.DateTimeFormat.Clone()));
            this.SS1_Sheet1.Cells.Get(0, 0).ParseFormatString = "yyyy-MM-dd";
            this.SS1_Sheet1.Cells.Get(0, 0).Value = new System.DateTime(2022, 5, 23, 0, 0, 0, 0);
            this.SS1_Sheet1.Cells.Get(0, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Cells.Get(0, 11).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(0, 11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Cells.Get(0, 11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "등록일자";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "근로자수";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "장년(50세)";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "장시간(60hr/주)";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "교대(야간)";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "근골부담";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "고객응대";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "근골격질환";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "뇌심질환";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "검진미수검";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "유소견자";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "기업건강지수";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "ID";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "결과값";
            this.SS1_Sheet1.ColumnHeader.Rows.Get(0).Height = 38F;
            this.SS1_Sheet1.Columns.Get(0).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(0).Label = "등록일자";
            this.SS1_Sheet1.Columns.Get(0).Locked = true;
            this.SS1_Sheet1.Columns.Get(0).Width = 89F;
            textCellType16.WordWrap = true;
            this.SS1_Sheet1.Columns.Get(1).CellType = textCellType16;
            this.SS1_Sheet1.Columns.Get(1).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(1).Label = "근로자수";
            this.SS1_Sheet1.Columns.Get(1).Width = 52F;
            textCellType17.WordWrap = true;
            this.SS1_Sheet1.Columns.Get(2).CellType = textCellType17;
            this.SS1_Sheet1.Columns.Get(2).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(2).Label = "장년(50세)";
            this.SS1_Sheet1.Columns.Get(2).Width = 52F;
            textCellType18.WordWrap = true;
            this.SS1_Sheet1.Columns.Get(3).CellType = textCellType18;
            this.SS1_Sheet1.Columns.Get(3).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(3).Label = "장시간(60hr/주)";
            this.SS1_Sheet1.Columns.Get(3).Width = 61F;
            textCellType19.WordWrap = true;
            this.SS1_Sheet1.Columns.Get(4).CellType = textCellType19;
            this.SS1_Sheet1.Columns.Get(4).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(4).Label = "교대(야간)";
            this.SS1_Sheet1.Columns.Get(4).Width = 52F;
            textCellType20.WordWrap = true;
            this.SS1_Sheet1.Columns.Get(5).CellType = textCellType20;
            this.SS1_Sheet1.Columns.Get(5).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(5).Label = "근골부담";
            this.SS1_Sheet1.Columns.Get(5).Width = 52F;
            textCellType21.WordWrap = true;
            this.SS1_Sheet1.Columns.Get(6).CellType = textCellType21;
            this.SS1_Sheet1.Columns.Get(6).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(6).Label = "고객응대";
            this.SS1_Sheet1.Columns.Get(6).Width = 52F;
            textCellType22.WordWrap = true;
            this.SS1_Sheet1.Columns.Get(7).CellType = textCellType22;
            this.SS1_Sheet1.Columns.Get(7).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(7).Label = "근골격질환";
            this.SS1_Sheet1.Columns.Get(7).Width = 52F;
            textCellType23.WordWrap = true;
            this.SS1_Sheet1.Columns.Get(8).CellType = textCellType23;
            this.SS1_Sheet1.Columns.Get(8).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(8).Label = "뇌심질환";
            this.SS1_Sheet1.Columns.Get(8).Width = 52F;
            textCellType24.WordWrap = true;
            this.SS1_Sheet1.Columns.Get(9).CellType = textCellType24;
            this.SS1_Sheet1.Columns.Get(9).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(9).Label = "검진미수검";
            this.SS1_Sheet1.Columns.Get(9).Width = 52F;
            textCellType25.WordWrap = true;
            this.SS1_Sheet1.Columns.Get(10).CellType = textCellType25;
            this.SS1_Sheet1.Columns.Get(10).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(10).Label = "유소견자";
            this.SS1_Sheet1.Columns.Get(10).Width = 52F;
            textCellType26.ReadOnly = true;
            textCellType26.WordWrap = true;
            this.SS1_Sheet1.Columns.Get(11).CellType = textCellType26;
            this.SS1_Sheet1.Columns.Get(11).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(11).Label = "기업건강지수";
            this.SS1_Sheet1.Columns.Get(11).Width = 52F;
            textCellType27.ReadOnly = true;
            this.SS1_Sheet1.Columns.Get(12).CellType = textCellType27;
            this.SS1_Sheet1.Columns.Get(12).Label = "ID";
            this.SS1_Sheet1.Columns.Get(12).Width = 39F;
            textCellType28.ReadOnly = true;
            this.SS1_Sheet1.Columns.Get(13).CellType = textCellType28;
            this.SS1_Sheet1.Columns.Get(13).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Columns.Get(13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(13).Label = "결과값";
            this.SS1_Sheet1.Columns.Get(13).Width = 46F;
            this.SS1_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SS1_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS1_Sheet1.RowHeader.Visible = false;
            this.SS1_Sheet1.Rows.Get(0).Height = 31F;
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(794, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(69, 26);
            this.btnClose.TabIndex = 173;
            this.btnClose.Text = "닫기";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnChange
            // 
            this.btnChange.Location = new System.Drawing.Point(171, 122);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(120, 28);
            this.btnChange.TabIndex = 175;
            this.btnChange.Text = "입력값 엑셀에 적용";
            this.btnChange.UseVisualStyleBackColor = true;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.Location = new System.Drawing.Point(645, 123);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(67, 28);
            this.btnExcel.TabIndex = 176;
            this.btnExcel.Text = "엑셀저장";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(239, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(63, 29);
            this.btnSearch.TabIndex = 177;
            this.btnSearch.Text = "검색";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // SSList
            // 
            this.SSList.AccessibleDescription = "SSList, Sheet1, Row 0, Column 0, ";
            this.SSList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.SSList.Location = new System.Drawing.Point(12, 41);
            this.SSList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SSList.Name = "SSList";
            this.SSList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSList_Sheet1});
            this.SSList.Size = new System.Drawing.Size(153, 109);
            this.SSList.TabIndex = 178;
            this.SSList.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSList_CellDoubleClick);
            // 
            // SSList_Sheet1
            // 
            this.SSList_Sheet1.Reset();
            this.SSList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSList_Sheet1.ColumnCount = 2;
            this.SSList_Sheet1.RowCount = 10;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "작성일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "ID";
            textCellType29.ReadOnly = true;
            this.SSList_Sheet1.Columns.Get(0).CellType = textCellType29;
            this.SSList_Sheet1.Columns.Get(0).Font = new System.Drawing.Font("굴림", 9F);
            this.SSList_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(0).Label = "작성일";
            this.SSList_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(0).Width = 87F;
            textCellType30.ReadOnly = true;
            this.SSList_Sheet1.Columns.Get(1).CellType = textCellType30;
            this.SSList_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(1).Label = "ID";
            this.SSList_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(1).Width = 45F;
            this.SSList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSList_Sheet1.RowHeader.Visible = false;
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(572, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(63, 29);
            this.btnAdd.TabIndex = 180;
            this.btnAdd.Text = "신규작성";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(641, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(120, 29);
            this.button2.TabIndex = 181;
            this.button2.Text = "최근 결과 가져오기";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnExcelGet
            // 
            this.btnExcelGet.Location = new System.Drawing.Point(297, 123);
            this.btnExcelGet.Name = "btnExcelGet";
            this.btnExcelGet.Size = new System.Drawing.Size(142, 28);
            this.btnExcelGet.TabIndex = 182;
            this.btnExcelGet.Text = "엑셀값 시트로 가져오기";
            this.btnExcelGet.UseVisualStyleBackColor = true;
            this.btnExcelGet.Click += new System.EventHandler(this.btnExcelGet_Click);
            // 
            // btnMail
            // 
            this.btnMail.Location = new System.Drawing.Point(718, 122);
            this.btnMail.Name = "btnMail";
            this.btnMail.Size = new System.Drawing.Size(67, 28);
            this.btnMail.TabIndex = 183;
            this.btnMail.Text = "메일전송";
            this.btnMail.UseVisualStyleBackColor = true;
            this.btnMail.Click += new System.EventHandler(this.btnMail_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(796, 123);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(67, 28);
            this.btnSave.TabIndex = 184;
            this.btnSave.Text = "DB저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(572, 122);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(67, 28);
            this.btnDelete.TabIndex = 185;
            this.btnDelete.Text = "삭제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // FrmGiupEHP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 851);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnMail);
            this.Controls.Add(this.btnExcelGet);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.SSList);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnExcel);
            this.Controls.Add(this.btnChange);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.SS1);
            this.Controls.Add(this.TxtLtdcode);
            this.Controls.Add(this.BtnSearchSite);
            this.Controls.Add(this.SSExcel);
            this.Name = "FrmGiupEHP";
            this.Text = "기업건강증진지수 관리";
            this.Load += new System.EventHandler(this.FrmGiupEHP_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SSExcel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSExcel_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TxtLtdcode;
        private System.Windows.Forms.Button BtnSearchSite;
        private FarPoint.Win.Spread.FpSpread SSExcel;
        private FarPoint.Win.Spread.SheetView SSExcel_Sheet1;
        private FarPoint.Win.Spread.FpSpread SS1;
        private FarPoint.Win.Spread.SheetView SS1_Sheet1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnChange;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.Button btnSearch;
        private FarPoint.Win.Spread.FpSpread SSList;
        private FarPoint.Win.Spread.SheetView SSList_Sheet1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnExcelGet;
        private System.Windows.Forms.Button btnMail;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
    }
}