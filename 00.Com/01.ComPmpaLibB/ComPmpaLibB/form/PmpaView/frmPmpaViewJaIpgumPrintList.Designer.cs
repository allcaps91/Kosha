namespace ComPmpaLibB
{
    partial class frmPmpaViewJaIpgumPrintList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose ();
            }
            base.Dispose (disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color343636437536659321069", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text465636437536659351088", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static598636437536659391096");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Currency895636437536660041604");
            FarPoint.Win.Spread.CellType.CurrencyCellType currencyCellType1 = new FarPoint.Win.Spread.CellType.CurrencyCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("Static1057636437536660091610");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType1 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType2 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType3 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType4 = new FarPoint.Win.Spread.CellType.NumberCellType();
            this.panSSmain = new System.Windows.Forms.Panel();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.label1 = new System.Windows.Forms.Label();
            this.panTitleSub1 = new System.Windows.Forms.Panel();
            this.panSub = new System.Windows.Forms.Panel();
            this.pnaHeddnTrue = new System.Windows.Forms.Panel();
            this.rdoIO2 = new System.Windows.Forms.RadioButton();
            this.rdoIO1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.lblGubun2 = new System.Windows.Forms.Label();
            this.panHidden1 = new System.Windows.Forms.Panel();
            this.cboYYMM = new System.Windows.Forms.ComboBox();
            this.lblyyyy = new System.Windows.Forms.Label();
            this.panHidden0 = new System.Windows.Forms.Panel();
            this.dtpTDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFDate = new System.Windows.Forms.DateTimePicker();
            this.lblyymm = new System.Windows.Forms.Label();
            this.lbldoc = new System.Windows.Forms.Label();
            this.lblWorkgubun = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.lblgubun0 = new System.Windows.Forms.Panel();
            this.rdoGB1 = new System.Windows.Forms.RadioButton();
            this.rdoGB0 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.lblsubTile = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panmain = new System.Windows.Forms.Panel();
            this.panSSmain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.panTitleSub1.SuspendLayout();
            this.panSub.SuspendLayout();
            this.pnaHeddnTrue.SuspendLayout();
            this.panHidden1.SuspendLayout();
            this.panHidden0.SuspendLayout();
            this.lblWorkgubun.SuspendLayout();
            this.lblgubun0.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.panmain.SuspendLayout();
            this.SuspendLayout();
            // 
            // panSSmain
            // 
            this.panSSmain.Controls.Add(this.ssView);
            this.panSSmain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panSSmain.Location = new System.Drawing.Point(0, 122);
            this.panSSmain.Name = "panSSmain";
            this.panSSmain.Size = new System.Drawing.Size(1184, 539);
            this.panSSmain.TabIndex = 209;
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "ssView, Sheet1, Row 0, Column 0, ";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssView.Location = new System.Drawing.Point(0, 0);
            this.ssView.Name = "ssView";
            namedStyle1.Font = new System.Drawing.Font("굴림체", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType1.MaxLength = 60;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("굴림체", 9F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType2.Static = true;
            namedStyle3.CellType = textCellType2;
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.Renderer = textCellType2;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            currencyCellType1.DecimalPlaces = 0;
            currencyCellType1.MaximumValue = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            currencyCellType1.MinimumValue = new decimal(new int[] {
            9999999,
            0,
            0,
            -2147483648});
            currencyCellType1.ShowCurrencySymbol = false;
            currencyCellType1.ShowSeparator = true;
            namedStyle4.CellType = currencyCellType1;
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.Renderer = currencyCellType1;
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            textCellType3.Static = true;
            namedStyle5.CellType = textCellType3;
            namedStyle5.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            namedStyle5.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle5.Renderer = textCellType3;
            namedStyle5.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4,
            namedStyle5});
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(1184, 539);
            this.ssView.TabIndex = 166;
            this.ssView.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssView.TextTipAppearance = tipAppearance1;
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 15;
            this.ssView_Sheet1.RowCount = 0;
            this.ssView_Sheet1.ActiveColumnIndex = -1;
            this.ssView_Sheet1.ActiveRowIndex = -1;
            this.ssView_Sheet1.ColumnFooter.Columns.Default.Resizable = true;
            this.ssView_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "계 약 처";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "입금일";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "청구일";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "회수기간";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "구분";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "병록번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "성 명 ";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "진료기간";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "과목";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "청구액";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "입금액";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "삭감액 ";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "삭감율 ";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "현잔액";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "비고";
            this.ssView_Sheet1.ColumnHeader.Columns.Default.Resizable = true;
            this.ssView_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssView_Sheet1.Columns.Default.Resizable = true;
            this.ssView_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.Columns.Get(0).Label = "계 약 처";
            this.ssView_Sheet1.Columns.Get(0).Width = 137F;
            this.ssView_Sheet1.Columns.Get(1).Label = "입금일";
            this.ssView_Sheet1.Columns.Get(1).StyleName = "Static598636437536659391096";
            this.ssView_Sheet1.Columns.Get(1).Width = 72F;
            this.ssView_Sheet1.Columns.Get(2).Label = "청구일";
            this.ssView_Sheet1.Columns.Get(2).StyleName = "Static598636437536659391096";
            this.ssView_Sheet1.Columns.Get(2).Width = 72F;
            this.ssView_Sheet1.Columns.Get(3).Label = "회수기간";
            this.ssView_Sheet1.Columns.Get(3).StyleName = "Static598636437536659391096";
            this.ssView_Sheet1.Columns.Get(3).Width = 62F;
            this.ssView_Sheet1.Columns.Get(4).Label = "구분";
            this.ssView_Sheet1.Columns.Get(4).StyleName = "Static598636437536659391096";
            this.ssView_Sheet1.Columns.Get(4).Width = 40F;
            this.ssView_Sheet1.Columns.Get(5).Label = "병록번호";
            this.ssView_Sheet1.Columns.Get(5).StyleName = "Static598636437536659391096";
            this.ssView_Sheet1.Columns.Get(5).Width = 70F;
            this.ssView_Sheet1.Columns.Get(6).Label = "성 명 ";
            this.ssView_Sheet1.Columns.Get(6).StyleName = "Static598636437536659391096";
            this.ssView_Sheet1.Columns.Get(6).Width = 59F;
            this.ssView_Sheet1.Columns.Get(7).Label = "진료기간";
            this.ssView_Sheet1.Columns.Get(7).StyleName = "Static598636437536659391096";
            this.ssView_Sheet1.Columns.Get(7).Width = 142F;
            this.ssView_Sheet1.Columns.Get(8).Label = "과목";
            this.ssView_Sheet1.Columns.Get(8).StyleName = "Static598636437536659391096";
            this.ssView_Sheet1.Columns.Get(8).Width = 35F;
            numberCellType1.DecimalPlaces = 1;
            numberCellType1.FixedPoint = false;
            numberCellType1.ShowSeparator = true;
            this.ssView_Sheet1.Columns.Get(9).CellType = numberCellType1;
            this.ssView_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssView_Sheet1.Columns.Get(9).Label = "청구액";
            this.ssView_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.ssView_Sheet1.Columns.Get(9).Width = 86F;
            numberCellType2.DecimalPlaces = 1;
            numberCellType2.FixedPoint = false;
            numberCellType2.ShowSeparator = true;
            this.ssView_Sheet1.Columns.Get(10).CellType = numberCellType2;
            this.ssView_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssView_Sheet1.Columns.Get(10).Label = "입금액";
            this.ssView_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.ssView_Sheet1.Columns.Get(10).Width = 85F;
            numberCellType3.DecimalPlaces = 1;
            numberCellType3.FixedPoint = false;
            numberCellType3.ShowSeparator = true;
            this.ssView_Sheet1.Columns.Get(11).CellType = numberCellType3;
            this.ssView_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssView_Sheet1.Columns.Get(11).Label = "삭감액 ";
            this.ssView_Sheet1.Columns.Get(11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.ssView_Sheet1.Columns.Get(11).Width = 80F;
            this.ssView_Sheet1.Columns.Get(12).Label = "삭감율 ";
            this.ssView_Sheet1.Columns.Get(12).StyleName = "Static1057636437536660091610";
            this.ssView_Sheet1.Columns.Get(12).Width = 49F;
            numberCellType4.DecimalPlaces = 1;
            numberCellType4.FixedPoint = false;
            numberCellType4.ShowSeparator = true;
            this.ssView_Sheet1.Columns.Get(13).CellType = numberCellType4;
            this.ssView_Sheet1.Columns.Get(13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssView_Sheet1.Columns.Get(13).Label = "현잔액";
            this.ssView_Sheet1.Columns.Get(13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.ssView_Sheet1.Columns.Get(13).Width = 86F;
            this.ssView_Sheet1.Columns.Get(14).Label = "비고";
            this.ssView_Sheet1.Columns.Get(14).Width = 49F;
            this.ssView_Sheet1.DefaultStyleName = "Text465636437536659351088";
            this.ssView_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.None);
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ssView_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssView_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.RowHeader.Rows.Default.Resizable = false;
            this.ssView_Sheet1.RowHeader.Visible = false;
            this.ssView_Sheet1.Rows.Default.Resizable = false;
            this.ssView_Sheet1.SheetCornerStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ssView_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssView_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192))))));
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(4, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 17);
            this.label1.TabIndex = 22;
            this.label1.Text = "결과";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitleSub1
            // 
            this.panTitleSub1.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub1.Controls.Add(this.label1);
            this.panTitleSub1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub1.Location = new System.Drawing.Point(0, 94);
            this.panTitleSub1.Name = "panTitleSub1";
            this.panTitleSub1.Size = new System.Drawing.Size(1184, 28);
            this.panTitleSub1.TabIndex = 208;
            // 
            // panSub
            // 
            this.panSub.Controls.Add(this.pnaHeddnTrue);
            this.panSub.Controls.Add(this.panHidden1);
            this.panSub.Controls.Add(this.panHidden0);
            this.panSub.Dock = System.Windows.Forms.DockStyle.Left;
            this.panSub.Location = new System.Drawing.Point(193, 0);
            this.panSub.Name = "panSub";
            this.panSub.Size = new System.Drawing.Size(737, 32);
            this.panSub.TabIndex = 90;
            // 
            // pnaHeddnTrue
            // 
            this.pnaHeddnTrue.Controls.Add(this.rdoIO2);
            this.pnaHeddnTrue.Controls.Add(this.rdoIO1);
            this.pnaHeddnTrue.Controls.Add(this.radioButton2);
            this.pnaHeddnTrue.Controls.Add(this.lblGubun2);
            this.pnaHeddnTrue.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnaHeddnTrue.Location = new System.Drawing.Point(474, 0);
            this.pnaHeddnTrue.Name = "pnaHeddnTrue";
            this.pnaHeddnTrue.Size = new System.Drawing.Size(220, 32);
            this.pnaHeddnTrue.TabIndex = 176;
            // 
            // rdoIO2
            // 
            this.rdoIO2.AutoSize = true;
            this.rdoIO2.Location = new System.Drawing.Point(163, 8);
            this.rdoIO2.Name = "rdoIO2";
            this.rdoIO2.Size = new System.Drawing.Size(47, 16);
            this.rdoIO2.TabIndex = 102;
            this.rdoIO2.Text = "외래";
            this.rdoIO2.UseVisualStyleBackColor = true;
            // 
            // rdoIO1
            // 
            this.rdoIO1.AutoSize = true;
            this.rdoIO1.Location = new System.Drawing.Point(116, 8);
            this.rdoIO1.Name = "rdoIO1";
            this.rdoIO1.Size = new System.Drawing.Size(47, 16);
            this.rdoIO1.TabIndex = 101;
            this.rdoIO1.Text = "입원";
            this.rdoIO1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Location = new System.Drawing.Point(69, 8);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(47, 16);
            this.radioButton2.TabIndex = 100;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "전체";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // lblGubun2
            // 
            this.lblGubun2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblGubun2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblGubun2.ForeColor = System.Drawing.Color.Black;
            this.lblGubun2.Location = new System.Drawing.Point(6, 5);
            this.lblGubun2.Name = "lblGubun2";
            this.lblGubun2.Size = new System.Drawing.Size(58, 23);
            this.lblGubun2.TabIndex = 98;
            this.lblGubun2.Text = "입/외래";
            this.lblGubun2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panHidden1
            // 
            this.panHidden1.Controls.Add(this.cboYYMM);
            this.panHidden1.Controls.Add(this.lblyyyy);
            this.panHidden1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panHidden1.Location = new System.Drawing.Point(274, 0);
            this.panHidden1.Name = "panHidden1";
            this.panHidden1.Size = new System.Drawing.Size(200, 32);
            this.panHidden1.TabIndex = 175;
            // 
            // cboYYMM
            // 
            this.cboYYMM.FormattingEnabled = true;
            this.cboYYMM.Location = new System.Drawing.Point(71, 6);
            this.cboYYMM.Name = "cboYYMM";
            this.cboYYMM.Size = new System.Drawing.Size(121, 20);
            this.cboYYMM.TabIndex = 175;
            // 
            // lblyyyy
            // 
            this.lblyyyy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblyyyy.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblyyyy.ForeColor = System.Drawing.Color.Black;
            this.lblyyyy.Location = new System.Drawing.Point(6, 5);
            this.lblyyyy.Name = "lblyyyy";
            this.lblyyyy.Size = new System.Drawing.Size(58, 23);
            this.lblyyyy.TabIndex = 174;
            this.lblyyyy.Text = "작업년월";
            this.lblyyyy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panHidden0
            // 
            this.panHidden0.Controls.Add(this.dtpTDate);
            this.panHidden0.Controls.Add(this.dtpFDate);
            this.panHidden0.Controls.Add(this.lblyymm);
            this.panHidden0.Controls.Add(this.lbldoc);
            this.panHidden0.Dock = System.Windows.Forms.DockStyle.Left;
            this.panHidden0.Location = new System.Drawing.Point(0, 0);
            this.panHidden0.Name = "panHidden0";
            this.panHidden0.Size = new System.Drawing.Size(274, 32);
            this.panHidden0.TabIndex = 173;
            // 
            // dtpTDate
            // 
            this.dtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTDate.Location = new System.Drawing.Point(181, 6);
            this.dtpTDate.Name = "dtpTDate";
            this.dtpTDate.Size = new System.Drawing.Size(86, 21);
            this.dtpTDate.TabIndex = 178;
            // 
            // dtpFDate
            // 
            this.dtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFDate.Location = new System.Drawing.Point(68, 6);
            this.dtpFDate.Name = "dtpFDate";
            this.dtpFDate.Size = new System.Drawing.Size(86, 21);
            this.dtpFDate.TabIndex = 177;
            // 
            // lblyymm
            // 
            this.lblyymm.AutoSize = true;
            this.lblyymm.Location = new System.Drawing.Point(161, 10);
            this.lblyymm.Name = "lblyymm";
            this.lblyymm.Size = new System.Drawing.Size(14, 12);
            this.lblyymm.TabIndex = 176;
            this.lblyymm.Text = "~";
            // 
            // lbldoc
            // 
            this.lbldoc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbldoc.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbldoc.ForeColor = System.Drawing.Color.Black;
            this.lbldoc.Location = new System.Drawing.Point(4, 5);
            this.lbldoc.Name = "lbldoc";
            this.lbldoc.Size = new System.Drawing.Size(58, 23);
            this.lbldoc.TabIndex = 173;
            this.lbldoc.Text = "작업년월";
            this.lbldoc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWorkgubun
            // 
            this.lblWorkgubun.BackColor = System.Drawing.SystemColors.Window;
            this.lblWorkgubun.Controls.Add(this.btnSearch);
            this.lblWorkgubun.Controls.Add(this.btnPrint);
            this.lblWorkgubun.Controls.Add(this.panSub);
            this.lblWorkgubun.Controls.Add(this.lblgubun0);
            this.lblWorkgubun.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblWorkgubun.Location = new System.Drawing.Point(0, 62);
            this.lblWorkgubun.Name = "lblWorkgubun";
            this.lblWorkgubun.Size = new System.Drawing.Size(1184, 32);
            this.lblWorkgubun.TabIndex = 207;
            // 
            // btnSearch
            // 
            this.btnSearch.AutoSize = true;
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Location = new System.Drawing.Point(1040, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 32);
            this.btnSearch.TabIndex = 77;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.AutoSize = true;
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPrint.Location = new System.Drawing.Point(1112, 0);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 32);
            this.btnPrint.TabIndex = 80;
            this.btnPrint.Text = "인쇄";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // lblgubun0
            // 
            this.lblgubun0.Controls.Add(this.rdoGB1);
            this.lblgubun0.Controls.Add(this.rdoGB0);
            this.lblgubun0.Controls.Add(this.label2);
            this.lblgubun0.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblgubun0.Location = new System.Drawing.Point(0, 0);
            this.lblgubun0.Name = "lblgubun0";
            this.lblgubun0.Size = new System.Drawing.Size(193, 32);
            this.lblgubun0.TabIndex = 91;
            // 
            // rdoGB1
            // 
            this.rdoGB1.AutoSize = true;
            this.rdoGB1.Location = new System.Drawing.Point(136, 8);
            this.rdoGB1.Name = "rdoGB1";
            this.rdoGB1.Size = new System.Drawing.Size(47, 16);
            this.rdoGB1.TabIndex = 99;
            this.rdoGB1.Text = "월별";
            this.rdoGB1.UseVisualStyleBackColor = true;
            this.rdoGB1.CheckedChanged += new System.EventHandler(this.rdoGB_CheckedChanged);
            // 
            // rdoGB0
            // 
            this.rdoGB0.AutoSize = true;
            this.rdoGB0.Checked = true;
            this.rdoGB0.Location = new System.Drawing.Point(77, 8);
            this.rdoGB0.Name = "rdoGB0";
            this.rdoGB0.Size = new System.Drawing.Size(59, 16);
            this.rdoGB0.TabIndex = 98;
            this.rdoGB0.TabStop = true;
            this.rdoGB0.Text = "일자별";
            this.rdoGB0.UseVisualStyleBackColor = true;
            this.rdoGB0.CheckedChanged += new System.EventHandler(this.rdoGB_CheckedChanged);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(12, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 23);
            this.label2.TabIndex = 97;
            this.label2.Text = "작업구분";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblsubTile
            // 
            this.lblsubTile.AutoSize = true;
            this.lblsubTile.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblsubTile.ForeColor = System.Drawing.Color.Black;
            this.lblsubTile.Location = new System.Drawing.Point(3, 5);
            this.lblsubTile.Name = "lblsubTile";
            this.lblsubTile.Size = new System.Drawing.Size(172, 21);
            this.lblsubTile.TabIndex = 83;
            this.lblsubTile.Text = "자보 진료비 입금 내역";
            this.lblsubTile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnExit
            // 
            this.btnExit.AutoSize = true;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(1108, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.lblsubTile);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1184, 34);
            this.panTitle.TabIndex = 205;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(4, 6);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(34, 17);
            this.lblTitleSub0.TabIndex = 22;
            this.lblTitleSub0.Text = "조건";
            this.lblTitleSub0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panmain
            // 
            this.panmain.BackColor = System.Drawing.Color.RoyalBlue;
            this.panmain.Controls.Add(this.lblTitleSub0);
            this.panmain.Dock = System.Windows.Forms.DockStyle.Top;
            this.panmain.Location = new System.Drawing.Point(0, 34);
            this.panmain.Name = "panmain";
            this.panmain.Size = new System.Drawing.Size(1184, 28);
            this.panmain.TabIndex = 206;
            // 
            // frmPmpaViewJaIpgumPrintList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 661);
            this.Controls.Add(this.panSSmain);
            this.Controls.Add(this.panTitleSub1);
            this.Controls.Add(this.lblWorkgubun);
            this.Controls.Add(this.panmain);
            this.Controls.Add(this.panTitle);
            this.Name = "frmPmpaViewJaIpgumPrintList";
            this.Text = "자보 진료비 입금 내역";
            this.Load += new System.EventHandler(this.frmPmpaViewJaIpgumPrintList_Load);
            this.panSSmain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.panTitleSub1.ResumeLayout(false);
            this.panTitleSub1.PerformLayout();
            this.panSub.ResumeLayout(false);
            this.pnaHeddnTrue.ResumeLayout(false);
            this.pnaHeddnTrue.PerformLayout();
            this.panHidden1.ResumeLayout(false);
            this.panHidden0.ResumeLayout(false);
            this.panHidden0.PerformLayout();
            this.lblWorkgubun.ResumeLayout(false);
            this.lblWorkgubun.PerformLayout();
            this.lblgubun0.ResumeLayout(false);
            this.lblgubun0.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panmain.ResumeLayout(false);
            this.panmain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panSSmain;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panTitleSub1;
        private System.Windows.Forms.Panel panSub;
        private System.Windows.Forms.Panel lblWorkgubun;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label lblsubTile;
        public System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panmain;
        private System.Windows.Forms.Panel lblgubun0;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdoGB1;
        private System.Windows.Forms.RadioButton rdoGB0;
        private System.Windows.Forms.Panel panHidden0;
        private System.Windows.Forms.Label lblyymm;
        private System.Windows.Forms.Label lbldoc;
        private System.Windows.Forms.Panel panHidden1;
        private System.Windows.Forms.ComboBox cboYYMM;
        private System.Windows.Forms.Label lblyyyy;
        private System.Windows.Forms.Panel pnaHeddnTrue;
        private System.Windows.Forms.RadioButton rdoIO2;
        private System.Windows.Forms.RadioButton rdoIO1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Label lblGubun2;
        private System.Windows.Forms.DateTimePicker dtpTDate;
        private System.Windows.Forms.DateTimePicker dtpFDate;
    }
}