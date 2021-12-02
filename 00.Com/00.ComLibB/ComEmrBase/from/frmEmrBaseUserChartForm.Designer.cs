namespace ComEmrBase
{
    partial class frmEmrBaseUserChartForm
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
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("Color308635227296039987589", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle6 = new FarPoint.Win.Spread.NamedStyle("Text387635227296040187601", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle7 = new FarPoint.Win.Spread.NamedStyle("Text522635227296040457616");
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle8 = new FarPoint.Win.Spread.NamedStyle("Text589635227296040507619");
            FarPoint.Win.Spread.CellType.TextCellType textCellType13 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance2 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.Spread.CellType.TextCellType textCellType14 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType15 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType16 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType17 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType18 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType19 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType20 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ssMacro = new FarPoint.Win.Spread.FpSpread();
            this.ssMacro_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel5 = new System.Windows.Forms.Panel();
            this.opdAllL = new System.Windows.Forms.RadioButton();
            this.optUserL = new System.Windows.Forms.RadioButton();
            this.optDeptL = new System.Windows.Forms.RadioButton();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.btnExpend = new System.Windows.Forms.Button();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panEmr = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.optUserR = new System.Windows.Forms.RadioButton();
            this.optDeptR = new System.Windows.Forms.RadioButton();
            this.panTitleSub1 = new System.Windows.Forms.Panel();
            this.lblTitleSub1 = new System.Windows.Forms.Label();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssMacro)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMacro_Sheet1)).BeginInit();
            this.panel5.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panTitleSub1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(994, 34);
            this.panTitle.TabIndex = 26;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(166, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "사용자별 템플릿 관리";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(918, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 31;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ssMacro);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.panTitleSub0);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(318, 725);
            this.panel1.TabIndex = 27;
            // 
            // ssMacro
            // 
            this.ssMacro.AccessibleDescription = "ssMacro, Sheet1, Row 0, Column 0, ";
            this.ssMacro.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssMacro.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssMacro.Location = new System.Drawing.Point(0, 58);
            this.ssMacro.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ssMacro.Name = "ssMacro";
            namedStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            namedStyle5.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle5.Locked = true;
            namedStyle5.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle5.Parent = "DataAreaDefault";
            namedStyle5.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType11.MaxLength = 32000;
            namedStyle6.CellType = textCellType11;
            namedStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            namedStyle6.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle6.Locked = true;
            namedStyle6.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle6.Parent = "DataAreaDefault";
            namedStyle6.Renderer = textCellType11;
            namedStyle6.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType12.MaxLength = 32000;
            namedStyle7.CellType = textCellType12;
            namedStyle7.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle7.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle7.Renderer = textCellType12;
            namedStyle7.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType13.MaxLength = 32000;
            namedStyle8.CellType = textCellType13;
            namedStyle8.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle8.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle8.Renderer = textCellType13;
            namedStyle8.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMacro.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle5,
            namedStyle6,
            namedStyle7,
            namedStyle8});
            this.ssMacro.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssMacro_Sheet1});
            this.ssMacro.Size = new System.Drawing.Size(318, 667);
            this.ssMacro.TabIndex = 31;
            this.ssMacro.TabStripRatio = 0.6D;
            tipAppearance2.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance2.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssMacro.TextTipAppearance = tipAppearance2;
            this.ssMacro.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssMacro_CellClick);
            this.ssMacro.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssMacro_CellDoubleClick);
            // 
            // ssMacro_Sheet1
            // 
            this.ssMacro_Sheet1.Reset();
            this.ssMacro_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssMacro_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssMacro_Sheet1.ColumnCount = 7;
            this.ssMacro_Sheet1.RowCount = 1;
            this.ssMacro_Sheet1.ActiveSkin = new FarPoint.Win.Spread.SheetSkin("CustomSkin1", System.Drawing.Color.White, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.LightGray, FarPoint.Win.Spread.GridLines.Both, System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(220)))), ((int)(((byte)(227))))), System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, false, false, false, false, false, true, false, true, "HeaderDefault", "HeaderDefault", "HeaderDefault", "DataAreaDefault", "HeaderDefault");
            this.ssMacro_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssMacro_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssMacro_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMacro_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMacro_Sheet1.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.ssMacro_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMacro_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssMacro_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssMacro_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMacro_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMacro_Sheet1.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.ssMacro_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMacro_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssMacro_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "구분";
            this.ssMacro_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "상용구명";
            this.ssMacro_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "MACRONO";
            this.ssMacro_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "FORMNO";
            this.ssMacro_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "UPDATENO";
            this.ssMacro_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "USEGB";
            this.ssMacro_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "FORMNAME";
            this.ssMacro_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssMacro_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssMacro_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(220)))), ((int)(((byte)(227)))));
            this.ssMacro_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMacro_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMacro_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssMacro_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMacro_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssMacro_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssMacro_Sheet1.ColumnHeader.Rows.Get(0).Height = 28F;
            this.ssMacro_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            textCellType14.MaxLength = 32000;
            this.ssMacro_Sheet1.Columns.Get(0).CellType = textCellType14;
            this.ssMacro_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssMacro_Sheet1.Columns.Get(0).Label = "구분";
            this.ssMacro_Sheet1.Columns.Get(0).Locked = true;
            this.ssMacro_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMacro_Sheet1.Columns.Get(0).Width = 39F;
            textCellType15.MaxLength = 32000;
            this.ssMacro_Sheet1.Columns.Get(1).CellType = textCellType15;
            this.ssMacro_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssMacro_Sheet1.Columns.Get(1).Label = "상용구명";
            this.ssMacro_Sheet1.Columns.Get(1).Locked = true;
            this.ssMacro_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMacro_Sheet1.Columns.Get(1).Width = 258F;
            textCellType16.MaxLength = 99999;
            this.ssMacro_Sheet1.Columns.Get(2).CellType = textCellType16;
            this.ssMacro_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssMacro_Sheet1.Columns.Get(2).Label = "MACRONO";
            this.ssMacro_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.ssMacro_Sheet1.Columns.Get(2).Visible = false;
            this.ssMacro_Sheet1.Columns.Get(2).Width = 77F;
            this.ssMacro_Sheet1.Columns.Get(3).CellType = textCellType17;
            this.ssMacro_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssMacro_Sheet1.Columns.Get(3).Label = "FORMNO";
            this.ssMacro_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMacro_Sheet1.Columns.Get(3).Visible = false;
            this.ssMacro_Sheet1.Columns.Get(3).Width = 66F;
            this.ssMacro_Sheet1.Columns.Get(4).CellType = textCellType18;
            this.ssMacro_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssMacro_Sheet1.Columns.Get(4).Label = "UPDATENO";
            this.ssMacro_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMacro_Sheet1.Columns.Get(4).Visible = false;
            this.ssMacro_Sheet1.Columns.Get(4).Width = 78F;
            this.ssMacro_Sheet1.Columns.Get(5).CellType = textCellType19;
            this.ssMacro_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssMacro_Sheet1.Columns.Get(5).Label = "USEGB";
            this.ssMacro_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMacro_Sheet1.Columns.Get(5).Visible = false;
            this.ssMacro_Sheet1.Columns.Get(5).Width = 50F;
            this.ssMacro_Sheet1.Columns.Get(6).CellType = textCellType20;
            this.ssMacro_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssMacro_Sheet1.Columns.Get(6).Label = "FORMNAME";
            this.ssMacro_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMacro_Sheet1.Columns.Get(6).Visible = false;
            this.ssMacro_Sheet1.Columns.Get(6).Width = 84F;
            this.ssMacro_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMacro_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMacro_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.ssMacro_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMacro_Sheet1.FilterBar.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssMacro_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMacro_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMacro_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssMacro_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMacro_Sheet1.FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssMacro_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            this.ssMacro_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssMacro_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssMacro_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(220)))), ((int)(((byte)(227)))));
            this.ssMacro_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMacro_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMacro_Sheet1.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssMacro_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMacro_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssMacro_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssMacro_Sheet1.RowHeader.Visible = false;
            this.ssMacro_Sheet1.Rows.Get(0).Height = 21F;
            this.ssMacro_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(220)))), ((int)(((byte)(227)))));
            this.ssMacro_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMacro_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMacro_Sheet1.SheetCornerStyle.Parent = "HeaderDefault";
            this.ssMacro_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMacro_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssMacro_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.opdAllL);
            this.panel5.Controls.Add(this.optUserL);
            this.panel5.Controls.Add(this.optDeptL);
            this.panel5.Controls.Add(this.btnSearch);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 28);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(318, 30);
            this.panel5.TabIndex = 29;
            // 
            // opdAllL
            // 
            this.opdAllL.AutoSize = true;
            this.opdAllL.BackColor = System.Drawing.Color.Transparent;
            this.opdAllL.Cursor = System.Windows.Forms.Cursors.Default;
            this.opdAllL.ForeColor = System.Drawing.SystemColors.ControlText;
            this.opdAllL.Location = new System.Drawing.Point(13, 5);
            this.opdAllL.Name = "opdAllL";
            this.opdAllL.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.opdAllL.Size = new System.Drawing.Size(52, 21);
            this.opdAllL.TabIndex = 208;
            this.opdAllL.Text = "전체";
            this.opdAllL.UseVisualStyleBackColor = false;
            this.opdAllL.Visible = false;
            // 
            // optUserL
            // 
            this.optUserL.AutoSize = true;
            this.optUserL.BackColor = System.Drawing.Color.Transparent;
            this.optUserL.Checked = true;
            this.optUserL.Cursor = System.Windows.Forms.Cursors.Default;
            this.optUserL.ForeColor = System.Drawing.SystemColors.ControlText;
            this.optUserL.Location = new System.Drawing.Point(149, 5);
            this.optUserL.Name = "optUserL";
            this.optUserL.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.optUserL.Size = new System.Drawing.Size(52, 21);
            this.optUserL.TabIndex = 207;
            this.optUserL.TabStop = true;
            this.optUserL.Text = "개인";
            this.optUserL.UseVisualStyleBackColor = false;
            // 
            // optDeptL
            // 
            this.optDeptL.AutoSize = true;
            this.optDeptL.BackColor = System.Drawing.Color.Transparent;
            this.optDeptL.Cursor = System.Windows.Forms.Cursors.Default;
            this.optDeptL.ForeColor = System.Drawing.SystemColors.ControlText;
            this.optDeptL.Location = new System.Drawing.Point(81, 5);
            this.optDeptL.Name = "optDeptL";
            this.optDeptL.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.optDeptL.Size = new System.Drawing.Size(52, 21);
            this.optDeptL.TabIndex = 206;
            this.optDeptL.Text = "과별";
            this.optDeptL.UseVisualStyleBackColor = false;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Location = new System.Drawing.Point(251, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(61, 25);
            this.btnSearch.TabIndex = 34;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.btnExpend);
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 0);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(318, 28);
            this.panTitleSub0.TabIndex = 28;
            // 
            // btnExpend
            // 
            this.btnExpend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExpend.BackColor = System.Drawing.Color.Transparent;
            this.btnExpend.Location = new System.Drawing.Point(286, -1);
            this.btnExpend.Name = "btnExpend";
            this.btnExpend.Size = new System.Drawing.Size(26, 25);
            this.btnExpend.TabIndex = 33;
            this.btnExpend.Text = "▶";
            this.btnExpend.UseVisualStyleBackColor = false;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 4);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(83, 15);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "템플릿 리스트";
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(318, 34);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(6, 725);
            this.panel2.TabIndex = 28;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panEmr);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Controls.Add(this.panTitleSub1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(324, 34);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(670, 725);
            this.panel3.TabIndex = 29;
            // 
            // panEmr
            // 
            this.panEmr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panEmr.Location = new System.Drawing.Point(0, 63);
            this.panEmr.Name = "panEmr";
            this.panEmr.Size = new System.Drawing.Size(670, 662);
            this.panEmr.TabIndex = 32;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 58);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(670, 5);
            this.panel4.TabIndex = 31;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btnClear);
            this.panel6.Controls.Add(this.btnDelete);
            this.panel6.Controls.Add(this.txtTitle);
            this.panel6.Controls.Add(this.btnSave);
            this.panel6.Controls.Add(this.label2);
            this.panel6.Controls.Add(this.optUserR);
            this.panel6.Controls.Add(this.optDeptR);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 28);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(670, 30);
            this.panel6.TabIndex = 30;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.BackColor = System.Drawing.Color.Transparent;
            this.btnClear.Location = new System.Drawing.Point(466, 1);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(61, 25);
            this.btnClear.TabIndex = 37;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.BackColor = System.Drawing.Color.Transparent;
            this.btnDelete.Location = new System.Drawing.Point(604, 1);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(61, 25);
            this.btnDelete.TabIndex = 36;
            this.btnDelete.Text = "삭제";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(72, 2);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(206, 25);
            this.txtTitle.TabIndex = 211;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.Location = new System.Drawing.Point(535, 1);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(61, 25);
            this.btnSave.TabIndex = 35;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 17);
            this.label2.TabIndex = 210;
            this.label2.Text = "템플릿명";
            // 
            // optUserR
            // 
            this.optUserR.AutoSize = true;
            this.optUserR.BackColor = System.Drawing.Color.Transparent;
            this.optUserR.Checked = true;
            this.optUserR.Cursor = System.Windows.Forms.Cursors.Default;
            this.optUserR.ForeColor = System.Drawing.SystemColors.ControlText;
            this.optUserR.Location = new System.Drawing.Point(369, 3);
            this.optUserR.Name = "optUserR";
            this.optUserR.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.optUserR.Size = new System.Drawing.Size(52, 21);
            this.optUserR.TabIndex = 209;
            this.optUserR.TabStop = true;
            this.optUserR.Text = "개인";
            this.optUserR.UseVisualStyleBackColor = false;
            // 
            // optDeptR
            // 
            this.optDeptR.AutoSize = true;
            this.optDeptR.BackColor = System.Drawing.Color.Transparent;
            this.optDeptR.Cursor = System.Windows.Forms.Cursors.Default;
            this.optDeptR.ForeColor = System.Drawing.SystemColors.ControlText;
            this.optDeptR.Location = new System.Drawing.Point(301, 3);
            this.optDeptR.Name = "optDeptR";
            this.optDeptR.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.optDeptR.Size = new System.Drawing.Size(52, 21);
            this.optDeptR.TabIndex = 208;
            this.optDeptR.Text = "과별";
            this.optDeptR.UseVisualStyleBackColor = false;
            // 
            // panTitleSub1
            // 
            this.panTitleSub1.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub1.Controls.Add(this.lblTitleSub1);
            this.panTitleSub1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub1.Location = new System.Drawing.Point(0, 0);
            this.panTitleSub1.Name = "panTitleSub1";
            this.panTitleSub1.Size = new System.Drawing.Size(670, 28);
            this.panTitleSub1.TabIndex = 28;
            // 
            // lblTitleSub1
            // 
            this.lblTitleSub1.AutoSize = true;
            this.lblTitleSub1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub1.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub1.Location = new System.Drawing.Point(8, 4);
            this.lblTitleSub1.Name = "lblTitleSub1";
            this.lblTitleSub1.Size = new System.Drawing.Size(115, 15);
            this.lblTitleSub1.TabIndex = 0;
            this.lblTitleSub1.Text = "템플릿 조회 및 등록";
            // 
            // frmEmrBaseUserChartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(994, 759);
            this.ControlBox = false;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmEmrBaseUserChartForm";
            this.Text = "frmEmrBaseUserChartForm";
            this.Load += new System.EventHandler(this.frmEmrBaseUserChartForm_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssMacro)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMacro_Sheet1)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panTitleSub1.ResumeLayout(false);
            this.panTitleSub1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Button btnExpend;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panTitleSub1;
        private System.Windows.Forms.Label lblTitleSub1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        public System.Windows.Forms.RadioButton opdAllL;
        public System.Windows.Forms.RadioButton optUserL;
        public System.Windows.Forms.RadioButton optDeptL;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.RadioButton optUserR;
        public System.Windows.Forms.RadioButton optDeptR;
        private FarPoint.Win.Spread.FpSpread ssMacro;
        private FarPoint.Win.Spread.SheetView ssMacro_Sheet1;
        private System.Windows.Forms.Panel panEmr;
        private System.Windows.Forms.Panel panel4;
    }
}