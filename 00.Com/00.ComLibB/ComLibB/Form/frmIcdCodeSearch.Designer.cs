namespace ComLibB
{
    partial class frmIcdCodeSearch
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
            FarPoint.Win.Spread.CellType.EnhancedColumnHeaderRenderer enhancedColumnHeaderRenderer1 = new FarPoint.Win.Spread.CellType.EnhancedColumnHeaderRenderer();
            FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer enhancedRowHeaderRenderer1 = new FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmIcdCodeSearch));
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("FilterBarDefaultEnhanced");
            FarPoint.Win.Spread.CellType.FilterBarCellType filterBarCellType1 = new FarPoint.Win.Spread.CellType.FilterBarCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("ColumnHeaderDefaultEnhanced");
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("RowHeaderDefaultEnhanced");
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("CornerDefaultEnhanced");
            FarPoint.Win.Spread.CellType.FlatCornerHeaderRenderer flatCornerHeaderRenderer1 = new FarPoint.Win.Spread.CellType.FlatCornerHeaderRenderer();
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("DataAreaDefault");
            FarPoint.Win.Spread.CellType.GeneralCellType generalCellType1 = new FarPoint.Win.Spread.CellType.GeneralCellType();
            FarPoint.Win.Spread.SpreadSkin spreadSkin1 = new FarPoint.Win.Spread.SpreadSkin();
            FarPoint.Win.Spread.EnhancedInterfaceRenderer enhancedInterfaceRenderer1 = new FarPoint.Win.Spread.EnhancedInterfaceRenderer();
            FarPoint.Win.Spread.StatusBarSkin statusBarSkin1 = new FarPoint.Win.Spread.StatusBarSkin();
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panTitle0 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ssList = new FarPoint.Win.Spread.FpSpread();
            this.ssList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panTitleSub0.SuspendLayout();
            this.panTitle0.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            enhancedColumnHeaderRenderer1.ActiveBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedColumnHeaderRenderer1.Name = "enhancedColumnHeaderRenderer1";
            enhancedColumnHeaderRenderer1.NormalGridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            enhancedColumnHeaderRenderer1.PictureZoomEffect = false;
            enhancedColumnHeaderRenderer1.SelectedActiveBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedColumnHeaderRenderer1.SelectedBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(240)))), ((int)(((byte)(248)))));
            enhancedColumnHeaderRenderer1.SelectedGridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            enhancedColumnHeaderRenderer1.TextRotationAngle = 0D;
            enhancedColumnHeaderRenderer1.ZoomFactor = 1F;
            enhancedRowHeaderRenderer1.ActiveBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedRowHeaderRenderer1.Name = "enhancedRowHeaderRenderer1";
            enhancedRowHeaderRenderer1.NormalGridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            enhancedRowHeaderRenderer1.PictureZoomEffect = false;
            enhancedRowHeaderRenderer1.SelectedActiveBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedRowHeaderRenderer1.SelectedBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(240)))), ((int)(((byte)(248)))));
            enhancedRowHeaderRenderer1.SelectedGridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            enhancedRowHeaderRenderer1.TextRotationAngle = 0D;
            enhancedRowHeaderRenderer1.ZoomFactor = 1F;
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 38);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(723, 27);
            this.panTitleSub0.TabIndex = 93;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(12, 5);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(92, 17);
            this.lblTitleSub0.TabIndex = 21;
            this.lblTitleSub0.Text = "ICD 코드 검색";
            // 
            // panTitle0
            // 
            this.panTitle0.BackColor = System.Drawing.Color.White;
            this.panTitle0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle0.Controls.Add(this.btnExit);
            this.panTitle0.Controls.Add(this.lblTitle);
            this.panTitle0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle0.ForeColor = System.Drawing.Color.White;
            this.panTitle0.Location = new System.Drawing.Point(0, 0);
            this.panTitle0.Name = "panTitle0";
            this.panTitle0.Size = new System.Drawing.Size(723, 38);
            this.panTitle0.TabIndex = 92;
            // 
            // btnExit
            // 
            this.btnExit.AutoSize = true;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(647, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 34);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "닫  기";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(5, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(113, 21);
            this.lblTitle.TabIndex = 14;
            this.lblTitle.Text = "ICD 코드 검색";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ssList);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 65);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(723, 394);
            this.panel1.TabIndex = 94;
            // 
            // ssList
            // 
            this.ssList.AccessibleDescription = "ssList, Sheet1, Row 0, Column 0, ";
            this.ssList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssList.FocusRenderer = new FarPoint.Win.Spread.CustomFocusIndicatorRenderer(((System.Drawing.Bitmap)(resources.GetObject("ssList.FocusRenderer"))), 1);
            this.ssList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssList.Location = new System.Drawing.Point(0, 32);
            this.ssList.Name = "ssList";
            namedStyle1.BackColor = System.Drawing.Color.White;
            filterBarCellType1.FormatString = "";
            namedStyle1.CellType = filterBarCellType1;
            namedStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Renderer = filterBarCellType1;
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle1.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            namedStyle2.BackColor = System.Drawing.Color.White;
            namedStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Renderer = enhancedColumnHeaderRenderer1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle2.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            namedStyle3.BackColor = System.Drawing.Color.White;
            namedStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.Renderer = enhancedRowHeaderRenderer1;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle3.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            namedStyle4.BackColor = System.Drawing.Color.White;
            namedStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            flatCornerHeaderRenderer1.ActiveForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(213)))), ((int)(((byte)(213)))));
            flatCornerHeaderRenderer1.ActiveMouseOverForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(213)))), ((int)(((byte)(213)))));
            flatCornerHeaderRenderer1.GridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            flatCornerHeaderRenderer1.NormalTriangleColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(213)))), ((int)(((byte)(213)))));
            namedStyle4.Renderer = flatCornerHeaderRenderer1;
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle4.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            namedStyle5.BackColor = System.Drawing.SystemColors.Window;
            namedStyle5.CellType = generalCellType1;
            namedStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            namedStyle5.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle5.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle5.Renderer = generalCellType1;
            namedStyle5.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle5.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4,
            namedStyle5});
            this.ssList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList_Sheet1});
            this.ssList.Size = new System.Drawing.Size(723, 362);
            spreadSkin1.ColumnFooterDefaultStyle = namedStyle2;
            spreadSkin1.ColumnHeaderDefaultStyle = namedStyle2;
            spreadSkin1.CornerDefaultStyle = namedStyle4;
            spreadSkin1.DefaultStyle = namedStyle5;
            spreadSkin1.FilterBarDefaultStyle = namedStyle1;
            spreadSkin1.FilterBarHeaderDefaultStyle = namedStyle3;
            spreadSkin1.FocusRenderer = new FarPoint.Win.Spread.CustomFocusIndicatorRenderer(((System.Drawing.Bitmap)(resources.GetObject("spreadSkin1.FocusRenderer"))), 1);
            enhancedInterfaceRenderer1.ArrowColorDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(178)))), ((int)(((byte)(178)))));
            enhancedInterfaceRenderer1.ArrowColorEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(172)))), ((int)(((byte)(179)))));
            enhancedInterfaceRenderer1.GrayAreaColor = System.Drawing.Color.White;
            enhancedInterfaceRenderer1.RangeGroupBackgroundColor = System.Drawing.Color.White;
            enhancedInterfaceRenderer1.RangeGroupButtonBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            enhancedInterfaceRenderer1.RangeGroupLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            enhancedInterfaceRenderer1.ScrollBoxBackgroundColor = System.Drawing.Color.White;
            enhancedInterfaceRenderer1.SheetTabBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(172)))), ((int)(((byte)(179)))));
            enhancedInterfaceRenderer1.SheetTabLowerActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedInterfaceRenderer1.SheetTabLowerNormalColor = System.Drawing.Color.White;
            enhancedInterfaceRenderer1.SheetTabUpperActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedInterfaceRenderer1.SheetTabUpperNormalColor = System.Drawing.Color.White;
            enhancedInterfaceRenderer1.SplitBarBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(240)))));
            enhancedInterfaceRenderer1.SplitBarDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(240)))));
            enhancedInterfaceRenderer1.SplitBarLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(227)))), ((int)(((byte)(240)))));
            enhancedInterfaceRenderer1.SplitBoxBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(213)))), ((int)(((byte)(213)))));
            enhancedInterfaceRenderer1.SplitBoxBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(213)))), ((int)(((byte)(213)))));
            enhancedInterfaceRenderer1.TabStripBackgroundColor = System.Drawing.Color.White;
            enhancedInterfaceRenderer1.TabStripButtonBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(172)))), ((int)(((byte)(179)))));
            enhancedInterfaceRenderer1.TabStripButtonFlatStyle = System.Windows.Forms.FlatStyle.Flat;
            enhancedInterfaceRenderer1.TabStripButtonLowerActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedInterfaceRenderer1.TabStripButtonLowerNormalColor = System.Drawing.Color.White;
            enhancedInterfaceRenderer1.TabStripButtonLowerPressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedInterfaceRenderer1.TabStripButtonStyle = FarPoint.Win.Spread.EnhancedInterfaceRenderer.ButtonStyles.Flat;
            enhancedInterfaceRenderer1.TabStripButtonUpperActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            enhancedInterfaceRenderer1.TabStripButtonUpperNormalColor = System.Drawing.Color.White;
            enhancedInterfaceRenderer1.TabStripButtonUpperPressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(234)))), ((int)(((byte)(253)))));
            spreadSkin1.InterfaceRenderer = enhancedInterfaceRenderer1;
            spreadSkin1.Name = "CustomSkin1";
            spreadSkin1.RowHeaderDefaultStyle = namedStyle3;
            spreadSkin1.SelectionRenderer = new FarPoint.Win.Spread.FlatSelectionRenderer();
            statusBarSkin1.BackColor = System.Drawing.Color.White;
            statusBarSkin1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            statusBarSkin1.ForeColor = System.Drawing.Color.Black;
            statusBarSkin1.Name = "Default";
            statusBarSkin1.ZoomButtonHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            statusBarSkin1.ZoomSliderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            statusBarSkin1.ZoomSliderHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            statusBarSkin1.ZoomSliderTrackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            spreadSkin1.StatusBarSkin = statusBarSkin1;
            this.ssList.Skin = spreadSkin1;
            this.ssList.TabIndex = 19;
            this.ssList.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssList_CellClick);
            // 
            // ssList_Sheet1
            // 
            this.ssList_Sheet1.Reset();
            this.ssList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssList_Sheet1.ColumnCount = 7;
            this.ssList_Sheet1.RowCount = 1;
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "ICDCODE";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "ICDCLASS";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "한글명";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "영어명";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "KCDCODE";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "SDATE";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "EDATE";
            this.ssList_Sheet1.ColumnHeader.Rows.Get(0).Height = 29F;
            textCellType1.Static = true;
            this.ssList_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssList_Sheet1.Columns.Get(0).Label = "ICDCODE";
            this.ssList_Sheet1.Columns.Get(0).Width = 65F;
            textCellType2.Static = true;
            this.ssList_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssList_Sheet1.Columns.Get(1).Label = "ICDCLASS";
            this.ssList_Sheet1.Columns.Get(1).Visible = false;
            this.ssList_Sheet1.Columns.Get(1).Width = 68F;
            textCellType3.Static = true;
            this.ssList_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssList_Sheet1.Columns.Get(2).Label = "한글명";
            this.ssList_Sheet1.Columns.Get(2).Width = 300F;
            textCellType4.Static = true;
            this.ssList_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssList_Sheet1.Columns.Get(3).Label = "영어명";
            this.ssList_Sheet1.Columns.Get(3).Width = 300F;
            textCellType5.Static = true;
            this.ssList_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.ssList_Sheet1.Columns.Get(4).Label = "KCDCODE";
            this.ssList_Sheet1.Columns.Get(4).Visible = false;
            this.ssList_Sheet1.Columns.Get(4).Width = 75F;
            textCellType6.Static = true;
            this.ssList_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.ssList_Sheet1.Columns.Get(5).Label = "SDATE";
            this.ssList_Sheet1.Columns.Get(5).Visible = false;
            this.ssList_Sheet1.Columns.Get(5).Width = 65F;
            textCellType7.Static = true;
            this.ssList_Sheet1.Columns.Get(6).CellType = textCellType7;
            this.ssList_Sheet1.Columns.Get(6).Label = "EDATE";
            this.ssList_Sheet1.Columns.Get(6).Visible = false;
            this.ssList_Sheet1.Columns.Get(6).Width = 65F;
            this.ssList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
            this.ssList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnSearch);
            this.panel2.Controls.Add(this.txtSearch);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(723, 32);
            this.panel2.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.AutoSize = true;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Location = new System.Drawing.Point(192, 1);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 16;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(65, 3);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(123, 25);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "검색어";
            // 
            // frmIcdCodeSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 459);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle0);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmIcdCodeSearch";
            this.Text = "frmIcdCodeSearch";
            this.Load += new System.EventHandler(this.frmIcdCodeSearch_Load);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panTitle0.ResumeLayout(false);
            this.panTitle0.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panTitle0;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label1;
        private FarPoint.Win.Spread.FpSpread ssList;
        private FarPoint.Win.Spread.SheetView ssList_Sheet1;
    }
}