namespace HC_OSHA
{
    partial class FrmExcelUpload1
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
            FarPoint.Win.Spread.FlatFocusIndicatorRenderer flatFocusIndicatorRenderer2 = new FarPoint.Win.Spread.FlatFocusIndicatorRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer3 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer4 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.CellType.TextCellType textCellType16 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType17 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType18 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType19 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType4 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType20 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.SS1 = new FarPoint.Win.Spread.FpSpread();
            this.SS1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.TxtLtdcode = new System.Windows.Forms.TextBox();
            this.lblLTD02 = new System.Windows.Forms.Label();
            this.btnJob3 = new System.Windows.Forms.Button();
            this.btnJob2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnJob1 = new System.Windows.Forms.Button();
            this.SSExcel = new FarPoint.Win.Spread.FpSpread();
            this.SSExcel_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.btnJob4 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SSConv = new FarPoint.Win.Spread.FpSpread();
            this.SSConv_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.btnJob5 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSExcel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSExcel_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSConv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSConv_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // SS1
            // 
            this.SS1.AccessibleDescription = "SS1, Sheet1, Row 0, Column 0, ";
            this.SS1.FocusRenderer = flatFocusIndicatorRenderer2;
            this.SS1.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SS1.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SS1.HorizontalScrollBar.Renderer = flatScrollBarRenderer3;
            this.SS1.HorizontalScrollBar.TabIndex = 224;
            this.SS1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SS1.Location = new System.Drawing.Point(-3, 359);
            this.SS1.Name = "SS1";
            this.SS1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS1_Sheet1});
            this.SS1.Size = new System.Drawing.Size(503, 278);
            this.SS1.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.SS1.TabIndex = 154;
            this.SS1.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SS1.VerticalScrollBar.Name = "";
            flatScrollBarRenderer4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SS1.VerticalScrollBar.Renderer = flatScrollBarRenderer4;
            this.SS1.VerticalScrollBar.TabIndex = 225;
            this.SS1.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SS1.SetViewportLeftColumn(0, 0, 2);
            this.SS1.SetActiveViewport(0, 0, -1);
            // 
            // SS1_Sheet1
            // 
            this.SS1_Sheet1.Reset();
            this.SS1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS1_Sheet1.ColumnCount = 3;
            this.SS1_Sheet1.RowCount = 50;
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "이름";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "생년월일";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "부서";
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.Rows.Get(0).Height = 41F;
            this.SS1_Sheet1.Columns.Get(0).CellType = textCellType16;
            this.SS1_Sheet1.Columns.Get(0).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SS1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS1_Sheet1.Columns.Get(0).Label = "이름";
            this.SS1_Sheet1.Columns.Get(0).Locked = true;
            this.SS1_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(0).Width = 86F;
            this.SS1_Sheet1.Columns.Get(1).CellType = textCellType17;
            this.SS1_Sheet1.Columns.Get(1).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SS1_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(1).Label = "생년월일";
            this.SS1_Sheet1.Columns.Get(1).Locked = true;
            this.SS1_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(1).Width = 80F;
            this.SS1_Sheet1.Columns.Get(2).CellType = textCellType18;
            this.SS1_Sheet1.Columns.Get(2).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SS1_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(2).Label = "부서";
            this.SS1_Sheet1.Columns.Get(2).Locked = true;
            this.SS1_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(2).Width = 123F;
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
            // TxtLtdcode
            // 
            this.TxtLtdcode.Location = new System.Drawing.Point(191, 8);
            this.TxtLtdcode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TxtLtdcode.Name = "TxtLtdcode";
            this.TxtLtdcode.Size = new System.Drawing.Size(130, 25);
            this.TxtLtdcode.TabIndex = 153;
            // 
            // lblLTD02
            // 
            this.lblLTD02.BackColor = System.Drawing.Color.LightBlue;
            this.lblLTD02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLTD02.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLTD02.Location = new System.Drawing.Point(132, 8);
            this.lblLTD02.Name = "lblLTD02";
            this.lblLTD02.Size = new System.Drawing.Size(53, 25);
            this.lblLTD02.TabIndex = 147;
            this.lblLTD02.Text = "회사";
            this.lblLTD02.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnJob3
            // 
            this.btnJob3.Location = new System.Drawing.Point(405, 331);
            this.btnJob3.Name = "btnJob3";
            this.btnJob3.Size = new System.Drawing.Size(95, 25);
            this.btnJob3.TabIndex = 146;
            this.btnJob3.Text = "DB에 저장";
            this.btnJob3.UseVisualStyleBackColor = true;
            this.btnJob3.Click += new System.EventHandler(this.btnJob3_Click);
            // 
            // btnJob2
            // 
            this.btnJob2.Enabled = false;
            this.btnJob2.Location = new System.Drawing.Point(288, 331);
            this.btnJob2.Name = "btnJob2";
            this.btnJob2.Size = new System.Drawing.Size(111, 25);
            this.btnJob2.TabIndex = 145;
            this.btnJob2.Text = "표준 서식 변환";
            this.btnJob2.UseVisualStyleBackColor = true;
            this.btnJob2.Click += new System.EventHandler(this.btnJob2_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.RoyalBlue;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(-1, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 26);
            this.label2.TabIndex = 144;
            this.label2.Text = "등록할 엑셀파일";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.RoyalBlue;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(-1, 330);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 26);
            this.label1.TabIndex = 143;
            this.label1.Text = "서버 DB용 표준 서식";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnJob1
            // 
            this.btnJob1.Location = new System.Drawing.Point(394, 9);
            this.btnJob1.Name = "btnJob1";
            this.btnJob1.Size = new System.Drawing.Size(106, 26);
            this.btnJob1.TabIndex = 142;
            this.btnJob1.Text = "엑셀파일 읽기";
            this.btnJob1.UseVisualStyleBackColor = true;
            this.btnJob1.Click += new System.EventHandler(this.btnJob1_Click);
            // 
            // SSExcel
            // 
            this.SSExcel.AccessibleDescription = "SSOshaList, Sheet1, Row 0, Column 0, HIC_USERS";
            this.SSExcel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSExcel.Location = new System.Drawing.Point(-1, 40);
            this.SSExcel.Name = "SSExcel";
            this.SSExcel.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSExcel_Sheet1});
            this.SSExcel.Size = new System.Drawing.Size(501, 284);
            this.SSExcel.TabIndex = 141;
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
            // btnJob4
            // 
            this.btnJob4.Enabled = false;
            this.btnJob4.Location = new System.Drawing.Point(816, 5);
            this.btnJob4.Name = "btnJob4";
            this.btnJob4.Size = new System.Drawing.Size(86, 30);
            this.btnJob4.TabIndex = 157;
            this.btnJob4.Text = "설정값 점검";
            this.btnJob4.UseVisualStyleBackColor = true;
            this.btnJob4.Click += new System.EventHandler(this.btnJob4_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.RoyalBlue;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.ForeColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(515, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 26);
            this.label3.TabIndex = 156;
            this.label3.Text = "표준서식 변환 설정값";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SSConv
            // 
            this.SSConv.AccessibleDescription = "SSConv, Sheet1, Row 0, Column 0, ";
            this.SSConv.EditModeReplace = true;
            this.SSConv.Location = new System.Drawing.Point(516, 40);
            this.SSConv.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SSConv.Name = "SSConv";
            this.SSConv.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSConv_Sheet1});
            this.SSConv.Size = new System.Drawing.Size(386, 605);
            this.SSConv.TabIndex = 155;
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
            textCellType19.ReadOnly = true;
            this.SSConv_Sheet1.Columns.Get(0).CellType = textCellType19;
            this.SSConv_Sheet1.Columns.Get(0).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SSConv_Sheet1.Columns.Get(0).Label = "표순서식 제목";
            this.SSConv_Sheet1.Columns.Get(0).Locked = true;
            this.SSConv_Sheet1.Columns.Get(0).Width = 102F;
            numberCellType4.DecimalPlaces = 0;
            numberCellType4.MaximumValue = 10000000D;
            numberCellType4.MinimumValue = 0D;
            this.SSConv_Sheet1.Columns.Get(1).CellType = numberCellType4;
            this.SSConv_Sheet1.Columns.Get(1).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SSConv_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSConv_Sheet1.Columns.Get(1).Label = "엑셀칼럼번호";
            this.SSConv_Sheet1.Columns.Get(1).Width = 57F;
            textCellType20.ReadOnly = true;
            this.SSConv_Sheet1.Columns.Get(2).CellType = textCellType20;
            this.SSConv_Sheet1.Columns.Get(2).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SSConv_Sheet1.Columns.Get(2).Label = "엑셀 칼럼 데이타";
            this.SSConv_Sheet1.Columns.Get(2).Locked = true;
            this.SSConv_Sheet1.Columns.Get(2).Width = 168F;
            this.SSConv_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSConv_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSConv_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSConv_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSConv_Sheet1.Protect = false;
            this.SSConv_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSConv_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // btnJob5
            // 
            this.btnJob5.Enabled = false;
            this.btnJob5.Location = new System.Drawing.Point(670, 3);
            this.btnJob5.Name = "btnJob5";
            this.btnJob5.Size = new System.Drawing.Size(140, 31);
            this.btnJob5.TabIndex = 159;
            this.btnJob5.Text = "표준서식 칼럼 찾기";
            this.btnJob5.UseVisualStyleBackColor = true;
            this.btnJob5.Click += new System.EventHandler(this.btnJob5_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(327, 9);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(48, 26);
            this.button1.TabIndex = 158;
            this.button1.Text = "검색";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FrmExcelUpload1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 651);
            this.Controls.Add(this.btnJob5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnJob4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.SSConv);
            this.Controls.Add(this.SS1);
            this.Controls.Add(this.TxtLtdcode);
            this.Controls.Add(this.lblLTD02);
            this.Controls.Add(this.btnJob3);
            this.Controls.Add(this.btnJob2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnJob1);
            this.Controls.Add(this.SSExcel);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmExcelUpload1";
            this.Text = "직원명단 엑셀파일 업로드";
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSExcel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSExcel_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSConv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSConv_Sheet1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FarPoint.Win.Spread.FpSpread SS1;
        private FarPoint.Win.Spread.SheetView SS1_Sheet1;
        private System.Windows.Forms.TextBox TxtLtdcode;
        private System.Windows.Forms.Label lblLTD02;
        private System.Windows.Forms.Button btnJob3;
        private System.Windows.Forms.Button btnJob2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnJob1;
        private FarPoint.Win.Spread.FpSpread SSExcel;
        private FarPoint.Win.Spread.SheetView SSExcel_Sheet1;
        private System.Windows.Forms.Button btnJob4;
        private System.Windows.Forms.Label label3;
        private FarPoint.Win.Spread.FpSpread SSConv;
        private FarPoint.Win.Spread.SheetView SSConv_Sheet1;
        private System.Windows.Forms.Button btnJob5;
        private System.Windows.Forms.Button button1;
    }
}