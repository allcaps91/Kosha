namespace HC_OSHA
{
    partial class FrmExcelUpload2
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType1 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.FlatFocusIndicatorRenderer flatFocusIndicatorRenderer1 = new FarPoint.Win.Spread.FlatFocusIndicatorRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer1 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer2 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.TxtLtdcode = new System.Windows.Forms.TextBox();
            this.BtnSearchSite = new System.Windows.Forms.Button();
            this.btnJob4 = new System.Windows.Forms.Button();
            this.cboBangi = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboYear = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblLTD02 = new System.Windows.Forms.Label();
            this.btnJob3 = new System.Windows.Forms.Button();
            this.btnJob2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SSConv = new FarPoint.Win.Spread.FpSpread();
            this.SSConv_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnJob1 = new System.Windows.Forms.Button();
            this.SSExcel = new FarPoint.Win.Spread.FpSpread();
            this.SSExcel_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.SS1 = new FarPoint.Win.Spread.FpSpread();
            this.SS1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.btnJob5 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.SSConv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSConv_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSExcel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSExcel_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // TxtLtdcode
            // 
            this.TxtLtdcode.Location = new System.Drawing.Point(181, 6);
            this.TxtLtdcode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TxtLtdcode.Name = "TxtLtdcode";
            this.TxtLtdcode.Size = new System.Drawing.Size(136, 21);
            this.TxtLtdcode.TabIndex = 139;
            // 
            // BtnSearchSite
            // 
            this.BtnSearchSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSearchSite.Location = new System.Drawing.Point(320, 5);
            this.BtnSearchSite.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearchSite.Name = "BtnSearchSite";
            this.BtnSearchSite.Size = new System.Drawing.Size(40, 26);
            this.BtnSearchSite.TabIndex = 138;
            this.BtnSearchSite.Text = "검색";
            this.BtnSearchSite.UseVisualStyleBackColor = true;
            this.BtnSearchSite.Click += new System.EventHandler(this.BtnSearchSite_Click);
            // 
            // btnJob4
            // 
            this.btnJob4.Enabled = false;
            this.btnJob4.Location = new System.Drawing.Point(1051, -3);
            this.btnJob4.Name = "btnJob4";
            this.btnJob4.Size = new System.Drawing.Size(86, 33);
            this.btnJob4.TabIndex = 137;
            this.btnJob4.Text = "설정값 점검";
            this.btnJob4.UseVisualStyleBackColor = true;
            this.btnJob4.Click += new System.EventHandler(this.btnJob4_Click);
            // 
            // cboBangi
            // 
            this.cboBangi.FormattingEnabled = true;
            this.cboBangi.Location = new System.Drawing.Point(562, 6);
            this.cboBangi.Name = "cboBangi";
            this.cboBangi.Size = new System.Drawing.Size(52, 20);
            this.cboBangi.TabIndex = 136;
            this.cboBangi.SelectedIndexChanged += new System.EventHandler(this.cboBangi_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.LightBlue;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(522, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 21);
            this.label5.TabIndex = 135;
            this.label5.Text = "반기";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // cboYear
            // 
            this.cboYear.FormattingEnabled = true;
            this.cboYear.Location = new System.Drawing.Point(442, 9);
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new System.Drawing.Size(58, 20);
            this.cboYear.TabIndex = 134;
            this.cboYear.SelectedIndexChanged += new System.EventHandler(this.cboYear_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.LightBlue;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(375, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 22);
            this.label4.TabIndex = 133;
            this.label4.Text = "검진년도";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // lblLTD02
            // 
            this.lblLTD02.BackColor = System.Drawing.Color.LightBlue;
            this.lblLTD02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLTD02.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLTD02.Location = new System.Drawing.Point(137, 6);
            this.lblLTD02.Name = "lblLTD02";
            this.lblLTD02.Size = new System.Drawing.Size(42, 22);
            this.lblLTD02.TabIndex = 132;
            this.lblLTD02.Text = "회사";
            this.lblLTD02.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLTD02.Click += new System.EventHandler(this.lblLTD02_Click);
            // 
            // btnJob3
            // 
            this.btnJob3.Location = new System.Drawing.Point(615, 329);
            this.btnJob3.Name = "btnJob3";
            this.btnJob3.Size = new System.Drawing.Size(95, 25);
            this.btnJob3.TabIndex = 131;
            this.btnJob3.Text = "DB에 저장";
            this.btnJob3.UseVisualStyleBackColor = true;
            this.btnJob3.Click += new System.EventHandler(this.btnJob3_Click);
            // 
            // btnJob2
            // 
            this.btnJob2.Enabled = false;
            this.btnJob2.Location = new System.Drawing.Point(498, 329);
            this.btnJob2.Name = "btnJob2";
            this.btnJob2.Size = new System.Drawing.Size(111, 25);
            this.btnJob2.TabIndex = 130;
            this.btnJob2.Text = "표준 서식 변환";
            this.btnJob2.UseVisualStyleBackColor = true;
            this.btnJob2.Click += new System.EventHandler(this.btnJob2_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.RoyalBlue;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.ForeColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(722, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(192, 26);
            this.label3.TabIndex = 129;
            this.label3.Text = "엑셀파일을 표준서식 변환 설정값";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // SSConv
            // 
            this.SSConv.AccessibleDescription = "SSConv, Sheet1, Row 0, Column 0, ";
            this.SSConv.EditModeReplace = true;
            this.SSConv.Location = new System.Drawing.Point(723, 38);
            this.SSConv.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SSConv.Name = "SSConv";
            this.SSConv.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSConv_Sheet1});
            this.SSConv.Size = new System.Drawing.Size(428, 605);
            this.SSConv.TabIndex = 128;
            this.SSConv.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSConv_CellClick);
            // 
            // SSConv_Sheet1
            // 
            this.SSConv_Sheet1.Reset();
            this.SSConv_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSConv_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSConv_Sheet1.ColumnCount = 3;
            this.SSConv_Sheet1.Cells.Get(4, 2).Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SSConv_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSConv_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSConv_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSConv_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSConv_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "표순서식 제목";
            this.SSConv_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "엑셀칼럼번호";
            this.SSConv_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "엑셀 칼럼 데이타";
            this.SSConv_Sheet1.ColumnHeader.Rows.Get(0).Height = 43F;
            textCellType1.ReadOnly = true;
            this.SSConv_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.SSConv_Sheet1.Columns.Get(0).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SSConv_Sheet1.Columns.Get(0).Label = "표순서식 제목";
            this.SSConv_Sheet1.Columns.Get(0).Locked = true;
            this.SSConv_Sheet1.Columns.Get(0).Width = 121F;
            numberCellType1.DecimalPlaces = 0;
            numberCellType1.MaximumValue = 10000000D;
            numberCellType1.MinimumValue = 0D;
            this.SSConv_Sheet1.Columns.Get(1).CellType = numberCellType1;
            this.SSConv_Sheet1.Columns.Get(1).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SSConv_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSConv_Sheet1.Columns.Get(1).Label = "엑셀칼럼번호";
            this.SSConv_Sheet1.Columns.Get(1).Width = 57F;
            textCellType2.ReadOnly = true;
            this.SSConv_Sheet1.Columns.Get(2).CellType = textCellType2;
            this.SSConv_Sheet1.Columns.Get(2).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SSConv_Sheet1.Columns.Get(2).Label = "엑셀 칼럼 데이타";
            this.SSConv_Sheet1.Columns.Get(2).Locked = true;
            this.SSConv_Sheet1.Columns.Get(2).Width = 191F;
            this.SSConv_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSConv_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSConv_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSConv_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSConv_Sheet1.Protect = false;
            this.SSConv_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSConv_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.RoyalBlue;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(4, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 26);
            this.label2.TabIndex = 127;
            this.label2.Text = "등록할 엑셀파일";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.RoyalBlue;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(4, 328);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 26);
            this.label1.TabIndex = 126;
            this.label1.Text = "서버 DB용 표준 서식";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // btnJob1
            // 
            this.btnJob1.Location = new System.Drawing.Point(625, 3);
            this.btnJob1.Name = "btnJob1";
            this.btnJob1.Size = new System.Drawing.Size(90, 26);
            this.btnJob1.TabIndex = 125;
            this.btnJob1.Text = "엑셀파일 읽기";
            this.btnJob1.UseVisualStyleBackColor = true;
            this.btnJob1.Click += new System.EventHandler(this.btnJob1_Click);
            // 
            // SSExcel
            // 
            this.SSExcel.AccessibleDescription = "SSOshaList, Sheet1, Row 0, Column 0, HIC_USERS";
            this.SSExcel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSExcel.Location = new System.Drawing.Point(4, 38);
            this.SSExcel.Name = "SSExcel";
            this.SSExcel.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSExcel_Sheet1});
            this.SSExcel.Size = new System.Drawing.Size(713, 284);
            this.SSExcel.TabIndex = 123;
            this.SSExcel.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSExcel_CellClick);
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
            this.SS1.AccessibleDescription = "SS1, Sheet1, Row 0, Column 0, ";
            this.SS1.FocusRenderer = flatFocusIndicatorRenderer1;
            this.SS1.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SS1.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SS1.HorizontalScrollBar.Renderer = flatScrollBarRenderer1;
            this.SS1.HorizontalScrollBar.TabIndex = 220;
            this.SS1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SS1.Location = new System.Drawing.Point(2, 357);
            this.SS1.Name = "SS1";
            this.SS1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS1_Sheet1});
            this.SS1.Size = new System.Drawing.Size(713, 278);
            this.SS1.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.SS1.TabIndex = 140;
            this.SS1.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SS1.VerticalScrollBar.Name = "";
            flatScrollBarRenderer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SS1.VerticalScrollBar.Renderer = flatScrollBarRenderer2;
            this.SS1.VerticalScrollBar.TabIndex = 221;
            this.SS1.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SS1.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SS1_CellClick);
            this.SS1.SetViewportLeftColumn(0, 0, 2);
            this.SS1.SetActiveViewport(0, 0, -1);
            // 
            // SS1_Sheet1
            // 
            this.SS1_Sheet1.Reset();
            this.SS1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS1_Sheet1.ColumnCount = 33;
            this.SS1_Sheet1.RowCount = 50;
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "소속";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "성명";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "생년월일";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "검진일";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "검진병원";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "나이";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "성별";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "신장";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "체중";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "BMI";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "허리둘레";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "수축기혈압";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "이완기혈압";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "혈당";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "당화혈색소";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 15).Value = "총콜레스테롤";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 16).Value = "HDL";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 17).Value = "LDL";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 18).Value = "중성지방";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 19).Value = "단백뇨";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 20).Value = "크레아티닌";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 21).Value = "사구체여과율(GFR)";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 22).Value = "흉부X선";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 23).Value = "10년이내 심뇌혈관 발병 확률(%)";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 24).Value = "심뇌혈관나이";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 25).Value = "심뇌혈관발생위험";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 26).Value = "검진결과 평가";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 27).Value = "1단계";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 28).Value = "2단계";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 29).Value = "발병위험도평가";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 30).Value = "업무적합성판정";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 31).Value = "개선의견";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 32).Value = "최종평가";
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.Rows.Get(0).Height = 41F;
            this.SS1_Sheet1.Columns.Get(0).CellType = textCellType3;
            this.SS1_Sheet1.Columns.Get(0).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SS1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS1_Sheet1.Columns.Get(0).Label = "소속";
            this.SS1_Sheet1.Columns.Get(0).Locked = true;
            this.SS1_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(0).Width = 74F;
            this.SS1_Sheet1.Columns.Get(1).CellType = textCellType4;
            this.SS1_Sheet1.Columns.Get(1).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SS1_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(1).Label = "성명";
            this.SS1_Sheet1.Columns.Get(1).Locked = true;
            this.SS1_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(1).Width = 62F;
            this.SS1_Sheet1.Columns.Get(2).CellType = textCellType5;
            this.SS1_Sheet1.Columns.Get(2).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SS1_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(2).Label = "생년월일";
            this.SS1_Sheet1.Columns.Get(2).Locked = true;
            this.SS1_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(2).Width = 61F;
            this.SS1_Sheet1.Columns.Get(3).CellType = textCellType6;
            this.SS1_Sheet1.Columns.Get(3).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SS1_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(3).Label = "검진일";
            this.SS1_Sheet1.Columns.Get(3).Locked = true;
            this.SS1_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(3).Width = 76F;
            this.SS1_Sheet1.Columns.Get(4).CellType = textCellType7;
            this.SS1_Sheet1.Columns.Get(4).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SS1_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(4).Label = "검진병원";
            this.SS1_Sheet1.Columns.Get(4).Locked = true;
            this.SS1_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(4).Width = 80F;
            textCellType8.Multiline = true;
            textCellType8.WordWrap = true;
            this.SS1_Sheet1.Columns.Get(5).CellType = textCellType8;
            this.SS1_Sheet1.Columns.Get(5).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SS1_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS1_Sheet1.Columns.Get(5).Label = "나이";
            this.SS1_Sheet1.Columns.Get(5).Locked = true;
            this.SS1_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(5).Width = 35F;
            textCellType9.Multiline = true;
            textCellType9.WordWrap = true;
            this.SS1_Sheet1.Columns.Get(6).CellType = textCellType9;
            this.SS1_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(6).Label = "성별";
            this.SS1_Sheet1.Columns.Get(6).Locked = true;
            this.SS1_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(6).Width = 35F;
            this.SS1_Sheet1.Columns.Get(23).Label = "10년이내 심뇌혈관 발병 확률(%)";
            this.SS1_Sheet1.Columns.Get(23).Width = 77F;
            this.SS1_Sheet1.Columns.Get(25).Label = "심뇌혈관발생위험";
            this.SS1_Sheet1.Columns.Get(25).Width = 127F;
            this.SS1_Sheet1.Columns.Get(26).Label = "검진결과 평가";
            this.SS1_Sheet1.Columns.Get(26).Width = 96F;
            this.SS1_Sheet1.Columns.Get(29).Label = "발병위험도평가";
            this.SS1_Sheet1.Columns.Get(29).Width = 120F;
            this.SS1_Sheet1.Columns.Get(30).Label = "업무적합성판정";
            this.SS1_Sheet1.Columns.Get(30).Width = 92F;
            this.SS1_Sheet1.Columns.Get(31).Label = "개선의견";
            this.SS1_Sheet1.Columns.Get(31).Width = 91F;
            this.SS1_Sheet1.Columns.Get(32).Label = "최종평가";
            this.SS1_Sheet1.Columns.Get(32).Width = 117F;
            this.SS1_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.SS1_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.SS1_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.FrozenColumnCount = 2;
            this.SS1_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
            this.SS1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS1_Sheet1.RowHeader.Columns.Get(0).Width = 30F;
            this.SS1_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.SS1_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.SS1_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // btnJob5
            // 
            this.btnJob5.Enabled = false;
            this.btnJob5.Location = new System.Drawing.Point(920, -1);
            this.btnJob5.Name = "btnJob5";
            this.btnJob5.Size = new System.Drawing.Size(117, 31);
            this.btnJob5.TabIndex = 141;
            this.btnJob5.Text = "표준서식 칼럼 찾기";
            this.btnJob5.UseVisualStyleBackColor = true;
            this.btnJob5.Click += new System.EventHandler(this.btnJob5_Click);
            // 
            // FrmExcelUpload2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1149, 651);
            this.Controls.Add(this.btnJob5);
            this.Controls.Add(this.SS1);
            this.Controls.Add(this.TxtLtdcode);
            this.Controls.Add(this.BtnSearchSite);
            this.Controls.Add(this.btnJob4);
            this.Controls.Add(this.cboBangi);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cboYear);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblLTD02);
            this.Controls.Add(this.btnJob3);
            this.Controls.Add(this.btnJob2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.SSConv);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnJob1);
            this.Controls.Add(this.SSExcel);
            this.Name = "FrmExcelUpload2";
            this.Text = "뇌.심혈관계 발병 위험도 평가";
            this.Load += new System.EventHandler(this.FrmExcelUpload2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SSConv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSConv_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSExcel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSExcel_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TxtLtdcode;
        private System.Windows.Forms.Button BtnSearchSite;
        private System.Windows.Forms.Button btnJob4;
        private System.Windows.Forms.ComboBox cboBangi;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboYear;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblLTD02;
        private System.Windows.Forms.Button btnJob3;
        private System.Windows.Forms.Button btnJob2;
        private System.Windows.Forms.Label label3;
        private FarPoint.Win.Spread.FpSpread SSConv;
        private FarPoint.Win.Spread.SheetView SSConv_Sheet1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnJob1;
        private FarPoint.Win.Spread.FpSpread SSExcel;
        private FarPoint.Win.Spread.SheetView SSExcel_Sheet1;
        private FarPoint.Win.Spread.FpSpread SS1;
        private FarPoint.Win.Spread.SheetView SS1_Sheet1;
        private System.Windows.Forms.Button btnJob5;
    }
}