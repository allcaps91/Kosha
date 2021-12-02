namespace ComSupLibB.SupXray
{
    partial class frmComSupXrayLIST04
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
            FarPoint.Win.Spread.EnhancedFocusIndicatorRenderer enhancedFocusIndicatorRenderer1 = new FarPoint.Win.Spread.EnhancedFocusIndicatorRenderer();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer1 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer2 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            FarPoint.Win.EmptyBorder emptyBorder1 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder2 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder3 = new FarPoint.Win.EmptyBorder();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.dtpTDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFDate = new System.Windows.Forms.DateTimePicker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.optIO3 = new System.Windows.Forms.RadioButton();
            this.optIO2 = new System.Windows.Forms.RadioButton();
            this.optIO1 = new System.Windows.Forms.RadioButton();
            this.optIO0 = new System.Windows.Forms.RadioButton();
            this.panbtn1 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel9 = new System.Windows.Forms.Panel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.panel8 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel20 = new System.Windows.Forms.Panel();
            this.line1 = new DevComponents.DotNetBar.Controls.Line();
            this.line2 = new DevComponents.DotNetBar.Controls.Line();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.exSpliter1 = new DevComponents.DotNetBar.ExpandableSplitter();
            this.panel4 = new System.Windows.Forms.Panel();
            this.ssList = new FarPoint.Win.Spread.FpSpread();
            this.ssList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.Progress = new DevComponents.DotNetBar.Controls.CircularProgress();
            this.panTitleSub0.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panbtn1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 0);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(1029, 36);
            this.panTitleSub0.TabIndex = 104;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 7);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(109, 17);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "근전도 검사 명단";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.panbtn1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 36);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1029, 46);
            this.panel2.TabIndex = 105;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.dtpTDate);
            this.panel6.Controls.Add(this.dtpFDate);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel6.Location = new System.Drawing.Point(271, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(190, 44);
            this.panel6.TabIndex = 39;
            // 
            // dtpTDate
            // 
            this.dtpTDate.CustomFormat = "yyyy-MM-dd";
            this.dtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTDate.Location = new System.Drawing.Point(97, 10);
            this.dtpTDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpTDate.Name = "dtpTDate";
            this.dtpTDate.Size = new System.Drawing.Size(87, 25);
            this.dtpTDate.TabIndex = 79;
            // 
            // dtpFDate
            // 
            this.dtpFDate.CustomFormat = "yyyy-MM-dd";
            this.dtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFDate.Location = new System.Drawing.Point(6, 10);
            this.dtpFDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpFDate.Name = "dtpFDate";
            this.dtpFDate.Size = new System.Drawing.Size(88, 25);
            this.dtpFDate.TabIndex = 78;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.optIO3);
            this.panel1.Controls.Add(this.optIO2);
            this.panel1.Controls.Add(this.optIO1);
            this.panel1.Controls.Add(this.optIO0);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(271, 44);
            this.panel1.TabIndex = 38;
            // 
            // optIO3
            // 
            this.optIO3.AutoSize = true;
            this.optIO3.Location = new System.Drawing.Point(179, 12);
            this.optIO3.Name = "optIO3";
            this.optIO3.Size = new System.Drawing.Size(52, 21);
            this.optIO3.TabIndex = 3;
            this.optIO3.Text = "응급";
            this.optIO3.UseVisualStyleBackColor = true;
            // 
            // optIO2
            // 
            this.optIO2.AutoSize = true;
            this.optIO2.Location = new System.Drawing.Point(121, 12);
            this.optIO2.Name = "optIO2";
            this.optIO2.Size = new System.Drawing.Size(52, 21);
            this.optIO2.TabIndex = 2;
            this.optIO2.Text = "외래";
            this.optIO2.UseVisualStyleBackColor = true;
            // 
            // optIO1
            // 
            this.optIO1.AutoSize = true;
            this.optIO1.Location = new System.Drawing.Point(63, 12);
            this.optIO1.Name = "optIO1";
            this.optIO1.Size = new System.Drawing.Size(52, 21);
            this.optIO1.TabIndex = 1;
            this.optIO1.Text = "입원";
            this.optIO1.UseVisualStyleBackColor = true;
            // 
            // optIO0
            // 
            this.optIO0.AutoSize = true;
            this.optIO0.Checked = true;
            this.optIO0.Location = new System.Drawing.Point(5, 12);
            this.optIO0.Name = "optIO0";
            this.optIO0.Size = new System.Drawing.Size(52, 21);
            this.optIO0.TabIndex = 0;
            this.optIO0.TabStop = true;
            this.optIO0.Text = "전체";
            this.optIO0.UseVisualStyleBackColor = true;
            // 
            // panbtn1
            // 
            this.panbtn1.Controls.Add(this.btnSearch);
            this.panbtn1.Controls.Add(this.panel9);
            this.panbtn1.Controls.Add(this.btnPrint);
            this.panbtn1.Controls.Add(this.panel8);
            this.panbtn1.Controls.Add(this.btnExit);
            this.panbtn1.Controls.Add(this.panel20);
            this.panbtn1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panbtn1.Location = new System.Drawing.Point(799, 0);
            this.panbtn1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panbtn1.Name = "panbtn1";
            this.panbtn1.Padding = new System.Windows.Forms.Padding(3);
            this.panbtn1.Size = new System.Drawing.Size(228, 44);
            this.panbtn1.TabIndex = 37;
            // 
            // btnSearch
            // 
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Location = new System.Drawing.Point(9, 3);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(70, 38);
            this.btnSearch.TabIndex = 34;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // panel9
            // 
            this.panel9.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel9.Location = new System.Drawing.Point(79, 3);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(2, 38);
            this.panel9.TabIndex = 168;
            // 
            // btnPrint
            // 
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrint.Location = new System.Drawing.Point(81, 3);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(70, 38);
            this.btnPrint.TabIndex = 30;
            this.btnPrint.Text = "출력";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // panel8
            // 
            this.panel8.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel8.Location = new System.Drawing.Point(151, 3);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(2, 38);
            this.panel8.TabIndex = 167;
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(153, 3);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(70, 38);
            this.btnExit.TabIndex = 29;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // panel20
            // 
            this.panel20.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel20.Location = new System.Drawing.Point(223, 3);
            this.panel20.Name = "panel20";
            this.panel20.Size = new System.Drawing.Size(2, 38);
            this.panel20.TabIndex = 166;
            // 
            // line1
            // 
            this.line1.Dock = System.Windows.Forms.DockStyle.Top;
            this.line1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.line1.Location = new System.Drawing.Point(0, 82);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(1029, 8);
            this.line1.TabIndex = 120;
            this.line1.Text = "line1";
            this.line1.Thickness = 5;
            // 
            // line2
            // 
            this.line2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.line2.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.line2.Location = new System.Drawing.Point(0, 771);
            this.line2.Name = "line2";
            this.line2.Size = new System.Drawing.Size(1029, 8);
            this.line2.TabIndex = 121;
            this.line2.Text = "line2";
            this.line2.Thickness = 5;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.exSpliter1);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 90);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1029, 681);
            this.panel3.TabIndex = 122;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.txtResult);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 445);
            this.panel5.Name = "panel5";
            this.panel5.Padding = new System.Windows.Forms.Padding(3);
            this.panel5.Size = new System.Drawing.Size(1029, 236);
            this.panel5.TabIndex = 110;
            // 
            // txtResult
            // 
            this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResult.Location = new System.Drawing.Point(3, 3);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(1023, 230);
            this.txtResult.TabIndex = 0;
            // 
            // exSpliter1
            // 
            this.exSpliter1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.exSpliter1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.exSpliter1.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.exSpliter1.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.exSpliter1.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.exSpliter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.exSpliter1.ExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.exSpliter1.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.exSpliter1.ExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.exSpliter1.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.exSpliter1.GripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.exSpliter1.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.exSpliter1.GripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.exSpliter1.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.exSpliter1.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(151)))), ((int)(((byte)(61)))));
            this.exSpliter1.HotBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(184)))), ((int)(((byte)(94)))));
            this.exSpliter1.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this.exSpliter1.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this.exSpliter1.HotExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.exSpliter1.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.exSpliter1.HotExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.exSpliter1.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.exSpliter1.HotGripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.exSpliter1.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.exSpliter1.HotGripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.exSpliter1.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.exSpliter1.Location = new System.Drawing.Point(0, 438);
            this.exSpliter1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.exSpliter1.Name = "exSpliter1";
            this.exSpliter1.Size = new System.Drawing.Size(1029, 7);
            this.exSpliter1.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007;
            this.exSpliter1.TabIndex = 109;
            this.exSpliter1.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.Progress);
            this.panel4.Controls.Add(this.ssList);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1029, 438);
            this.panel4.TabIndex = 0;
            // 
            // ssList
            // 
            this.ssList.AccessibleDescription = "";
            this.ssList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ssList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssList.FocusRenderer = enhancedFocusIndicatorRenderer1;
            this.ssList.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssList.HorizontalScrollBar.Name = "";
            this.ssList.HorizontalScrollBar.Renderer = enhancedScrollBarRenderer1;
            this.ssList.HorizontalScrollBar.TabIndex = 15;
            this.ssList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssList.Location = new System.Drawing.Point(0, 0);
            this.ssList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ssList.Name = "ssList";
            this.ssList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList_Sheet1});
            this.ssList.Size = new System.Drawing.Size(1029, 438);
            this.ssList.TabIndex = 8;
            this.ssList.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssList.VerticalScrollBar.Name = "";
            this.ssList.VerticalScrollBar.Renderer = enhancedScrollBarRenderer2;
            this.ssList.VerticalScrollBar.TabIndex = 16;
            // 
            // ssList_Sheet1
            // 
            this.ssList_Sheet1.Reset();
            this.ssList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssList_Sheet1.ColumnCount = 3;
            this.ssList_Sheet1.RowCount = 1;
            this.ssList_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterEnhanced";
            this.ssList_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerEnhanced";
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderEnhanced";
            this.ssList_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.Columns.Get(0).Border = emptyBorder1;
            this.ssList_Sheet1.Columns.Get(1).Border = emptyBorder2;
            this.ssList_Sheet1.Columns.Get(2).Border = emptyBorder3;
            this.ssList_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.ssList_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarEnhanced";
            this.ssList_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderEnhanced";
            this.ssList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ssList_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Black);
            this.ssList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderEnhanced";
            this.ssList_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.SheetCornerStyle.Parent = "CornerEnhanced";
            this.ssList_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Black);
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // Progress
            // 
            // 
            // 
            // 
            this.Progress.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.Progress.Location = new System.Drawing.Point(439, 144);
            this.Progress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Progress.Name = "Progress";
            this.Progress.ProgressBarType = DevComponents.DotNetBar.eCircularProgressType.Dot;
            this.Progress.ProgressColor = System.Drawing.Color.Gray;
            this.Progress.Size = new System.Drawing.Size(150, 150);
            this.Progress.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeXP;
            this.Progress.TabIndex = 129;
            this.Progress.Visible = false;
            // 
            // frmComSupXrayLIST04
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1029, 779);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.line2);
            this.Controls.Add(this.line1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panTitleSub0);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmComSupXrayLIST04";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmComSupXrayLIST04";
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panbtn1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panbtn1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel20;
        private DevComponents.DotNetBar.Controls.Line line1;
        private DevComponents.DotNetBar.Controls.Line line2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TextBox txtResult;
        private DevComponents.DotNetBar.ExpandableSplitter exSpliter1;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker dtpTDate;
        private System.Windows.Forms.DateTimePicker dtpFDate;
        private System.Windows.Forms.RadioButton optIO3;
        private System.Windows.Forms.RadioButton optIO2;
        private System.Windows.Forms.RadioButton optIO1;
        private System.Windows.Forms.RadioButton optIO0;
        private FarPoint.Win.Spread.FpSpread ssList;
        private FarPoint.Win.Spread.SheetView ssList_Sheet1;
        private DevComponents.DotNetBar.Controls.CircularProgress Progress;
    }
}