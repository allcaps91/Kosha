namespace ComEmrBase
{
    partial class frmEmrHealthGraph
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
            this.panTopMenu = new System.Windows.Forms.Panel();
            this.panChart = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panList = new System.Windows.Forms.Panel();
            this.ssList = new FarPoint.Win.Spread.FpSpread();
            this.ssList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkAsc = new System.Windows.Forms.CheckBox();
            this.mbtnPrint = new System.Windows.Forms.Button();
            this.mbtnSearch = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.dtpFrDate = new System.Windows.Forms.DateTimePicker();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.panWrite = new System.Windows.Forms.Panel();
            this.ssWrite = new FarPoint.Win.Spread.FpSpread();
            this.ssWrite_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panSearch = new System.Windows.Forms.Panel();
            this.mbtnSaveAll = new System.Windows.Forms.Button();
            this.mbtnHis = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.I0000037283 = new System.Windows.Forms.TextBox();
            this.I0000037282 = new System.Windows.Forms.TextBox();
            this.I0000037281 = new System.Windows.Forms.TextBox();
            this.mbtnSaveK = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panGraph = new System.Windows.Forms.Panel();
            this.mbtnGpPrint = new System.Windows.Forms.Button();
            this.panChart.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panWrite.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssWrite)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssWrite_Sheet1)).BeginInit();
            this.panSearch.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTopMenu
            // 
            this.panTopMenu.BackColor = System.Drawing.Color.White;
            this.panTopMenu.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTopMenu.Cursor = System.Windows.Forms.Cursors.Default;
            this.panTopMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTopMenu.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panTopMenu.Location = new System.Drawing.Point(0, 0);
            this.panTopMenu.Name = "panTopMenu";
            this.panTopMenu.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panTopMenu.Size = new System.Drawing.Size(686, 36);
            this.panTopMenu.TabIndex = 11;
            this.panTopMenu.TabStop = true;
            // 
            // panChart
            // 
            this.panChart.AutoScroll = true;
            this.panChart.BackColor = System.Drawing.SystemColors.Control;
            this.panChart.Controls.Add(this.tabControl1);
            this.panChart.Cursor = System.Windows.Forms.Cursors.Default;
            this.panChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panChart.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panChart.Location = new System.Drawing.Point(0, 36);
            this.panChart.Name = "panChart";
            this.panChart.Size = new System.Drawing.Size(686, 754);
            this.panChart.TabIndex = 13;
            this.panChart.TabStop = true;
            this.panChart.Scroll += new System.Windows.Forms.ScrollEventHandler(this.panChart_Scroll);
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(669, 789);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 28;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabPage2.Controls.Add(this.panList);
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Controls.Add(this.panWrite);
            this.tabPage2.Controls.Add(this.panSearch);
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(661, 760);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "표";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panList
            // 
            this.panList.Controls.Add(this.ssList);
            this.panList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panList.Location = new System.Drawing.Point(3, 172);
            this.panList.Name = "panList";
            this.panList.Size = new System.Drawing.Size(653, 583);
            this.panList.TabIndex = 26;
            // 
            // ssList
            // 
            this.ssList.AccessibleDescription = "ssList, Sheet1, Row 0, Column 0, 9999-99-99";
            this.ssList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssList.Location = new System.Drawing.Point(0, 0);
            this.ssList.Name = "ssList";
            this.ssList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList_Sheet1});
            this.ssList.Size = new System.Drawing.Size(653, 583);
            this.ssList.TabIndex = 1;
            this.ssList.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssList_CellDoubleClick);
            this.ssList.SetActiveViewport(0, -1, -1);
            // 
            // ssList_Sheet1
            // 
            this.ssList_Sheet1.Reset();
            this.ssList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssList_Sheet1.ColumnCount = 0;
            this.ssList_Sheet1.RowCount = 0;
            this.ssList_Sheet1.ActiveColumnIndex = -1;
            this.ssList_Sheet1.ActiveRowIndex = -1;
            this.ssList_Sheet1.ActiveSkin = new FarPoint.Win.Spread.SheetSkin("CustomSkin1", System.Drawing.Color.White, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.LightGray, FarPoint.Win.Spread.GridLines.Both, System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237))))), System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, false, false, false, false, false, true, false, true, "HeaderDefault", "HeaderDefault", "HeaderDefault", "DataAreaDefault", "HeaderDefault");
            this.ssList_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.ssList_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssList_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssList_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssList_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssList_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssList_Sheet1.ColumnHeader.Rows.Get(0).Height = 48F;
            this.ssList_Sheet1.ColumnHeader.Visible = false;
            this.ssList_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.ssList_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.FilterBar.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssList_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssList_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssList_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssList_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssList_Sheet1.RowHeader.Visible = false;
            this.ssList_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssList_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.SheetCornerStyle.Parent = "HeaderDefault";
            this.ssList_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.chkAsc);
            this.panel1.Controls.Add(this.mbtnPrint);
            this.panel1.Controls.Add(this.mbtnSearch);
            this.panel1.Controls.Add(this.Label1);
            this.panel1.Controls.Add(this.Label3);
            this.panel1.Controls.Add(this.dtpFrDate);
            this.panel1.Controls.Add(this.dtpEndDate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 136);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(653, 36);
            this.panel1.TabIndex = 26;
            // 
            // chkAsc
            // 
            this.chkAsc.AutoSize = true;
            this.chkAsc.Location = new System.Drawing.Point(584, 9);
            this.chkAsc.Name = "chkAsc";
            this.chkAsc.Size = new System.Drawing.Size(60, 16);
            this.chkAsc.TabIndex = 96;
            this.chkAsc.Text = "순정렬";
            this.chkAsc.UseVisualStyleBackColor = true;
            this.chkAsc.Visible = false;
            // 
            // mbtnPrint
            // 
            this.mbtnPrint.Location = new System.Drawing.Point(499, 2);
            this.mbtnPrint.Name = "mbtnPrint";
            this.mbtnPrint.Size = new System.Drawing.Size(83, 30);
            this.mbtnPrint.TabIndex = 88;
            this.mbtnPrint.Text = "출 력";
            this.mbtnPrint.UseVisualStyleBackColor = true;
            this.mbtnPrint.Click += new System.EventHandler(this.mbtnPrint_Click);
            // 
            // mbtnSearch
            // 
            this.mbtnSearch.BackColor = System.Drawing.SystemColors.Control;
            this.mbtnSearch.Cursor = System.Windows.Forms.Cursors.Default;
            this.mbtnSearch.ForeColor = System.Drawing.SystemColors.ControlText;
            this.mbtnSearch.Location = new System.Drawing.Point(416, 2);
            this.mbtnSearch.Name = "mbtnSearch";
            this.mbtnSearch.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.mbtnSearch.Size = new System.Drawing.Size(83, 30);
            this.mbtnSearch.TabIndex = 85;
            this.mbtnSearch.Text = "조  회";
            this.mbtnSearch.UseVisualStyleBackColor = true;
            this.mbtnSearch.Click += new System.EventHandler(this.mbtnSearchAll_Click);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.BackColor = System.Drawing.SystemColors.Control;
            this.Label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label1.Location = new System.Drawing.Point(178, 10);
            this.Label1.Name = "Label1";
            this.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label1.Size = new System.Drawing.Size(14, 12);
            this.Label1.TabIndex = 91;
            this.Label1.Text = "~";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.BackColor = System.Drawing.SystemColors.Control;
            this.Label3.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label3.Location = new System.Drawing.Point(15, 10);
            this.Label3.Name = "Label3";
            this.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label3.Size = new System.Drawing.Size(61, 12);
            this.Label3.TabIndex = 89;
            this.Label3.Text = "조회기간 :";
            // 
            // dtpFrDate
            // 
            this.dtpFrDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrDate.Location = new System.Drawing.Point(84, 6);
            this.dtpFrDate.Name = "dtpFrDate";
            this.dtpFrDate.Size = new System.Drawing.Size(86, 21);
            this.dtpFrDate.TabIndex = 86;
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new System.Drawing.Point(200, 6);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(86, 21);
            this.dtpEndDate.TabIndex = 87;
            // 
            // panWrite
            // 
            this.panWrite.Controls.Add(this.ssWrite);
            this.panWrite.Dock = System.Windows.Forms.DockStyle.Top;
            this.panWrite.Location = new System.Drawing.Point(3, 75);
            this.panWrite.Name = "panWrite";
            this.panWrite.Size = new System.Drawing.Size(653, 61);
            this.panWrite.TabIndex = 28;
            this.panWrite.Visible = false;
            // 
            // ssWrite
            // 
            this.ssWrite.AccessibleDescription = "ssWrite, Sheet1, Row 0, Column 0, I0000015052";
            this.ssWrite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssWrite.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssWrite.Location = new System.Drawing.Point(0, 0);
            this.ssWrite.Name = "ssWrite";
            this.ssWrite.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssWrite_Sheet1});
            this.ssWrite.Size = new System.Drawing.Size(653, 61);
            this.ssWrite.TabIndex = 0;
            this.ssWrite.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssWrite.SetActiveViewport(0, -1, -1);
            // 
            // ssWrite_Sheet1
            // 
            this.ssWrite_Sheet1.Reset();
            this.ssWrite_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssWrite_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssWrite_Sheet1.ColumnCount = 0;
            this.ssWrite_Sheet1.RowCount = 0;
            this.ssWrite_Sheet1.ActiveColumnIndex = -1;
            this.ssWrite_Sheet1.ActiveRowIndex = -1;
            this.ssWrite_Sheet1.ActiveSkin = new FarPoint.Win.Spread.SheetSkin("CustomSkin1", System.Drawing.Color.White, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.LightGray, FarPoint.Win.Spread.GridLines.Both, System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237))))), System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, false, false, false, false, false, true, false, true, "HeaderDefault", "HeaderDefault", "HeaderDefault", "DataAreaDefault", "HeaderDefault");
            this.ssWrite_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssWrite_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssWrite_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssWrite_Sheet1.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.ssWrite_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssWrite_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssWrite_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssWrite_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssWrite_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssWrite_Sheet1.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.ssWrite_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssWrite_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssWrite_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssWrite_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssWrite_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssWrite_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssWrite_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssWrite_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssWrite_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssWrite_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssWrite_Sheet1.ColumnHeader.Rows.Get(0).Height = 34F;
            this.ssWrite_Sheet1.ColumnHeader.Visible = false;
            this.ssWrite_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssWrite_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssWrite_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.ssWrite_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssWrite_Sheet1.FilterBar.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssWrite_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssWrite_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssWrite_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssWrite_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssWrite_Sheet1.FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssWrite_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssWrite_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssWrite_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssWrite_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssWrite_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssWrite_Sheet1.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssWrite_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssWrite_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssWrite_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssWrite_Sheet1.RowHeader.Visible = false;
            this.ssWrite_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssWrite_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssWrite_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssWrite_Sheet1.SheetCornerStyle.Parent = "HeaderDefault";
            this.ssWrite_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssWrite_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssWrite_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panSearch
            // 
            this.panSearch.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panSearch.Controls.Add(this.mbtnSaveAll);
            this.panSearch.Controls.Add(this.mbtnHis);
            this.panSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSearch.Location = new System.Drawing.Point(3, 39);
            this.panSearch.Name = "panSearch";
            this.panSearch.Size = new System.Drawing.Size(653, 36);
            this.panSearch.TabIndex = 27;
            this.panSearch.Visible = false;
            // 
            // mbtnSaveAll
            // 
            this.mbtnSaveAll.BackColor = System.Drawing.SystemColors.Control;
            this.mbtnSaveAll.Cursor = System.Windows.Forms.Cursors.Default;
            this.mbtnSaveAll.ForeColor = System.Drawing.SystemColors.ControlText;
            this.mbtnSaveAll.Location = new System.Drawing.Point(511, 2);
            this.mbtnSaveAll.Name = "mbtnSaveAll";
            this.mbtnSaveAll.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.mbtnSaveAll.Size = new System.Drawing.Size(83, 30);
            this.mbtnSaveAll.TabIndex = 94;
            this.mbtnSaveAll.Text = "저  장";
            this.mbtnSaveAll.UseVisualStyleBackColor = true;
            this.mbtnSaveAll.Click += new System.EventHandler(this.mbtnSaveAll_Click);
            // 
            // mbtnHis
            // 
            this.mbtnHis.Location = new System.Drawing.Point(428, 2);
            this.mbtnHis.Name = "mbtnHis";
            this.mbtnHis.Size = new System.Drawing.Size(83, 30);
            this.mbtnHis.TabIndex = 93;
            this.mbtnHis.Text = "이전내역";
            this.mbtnHis.UseVisualStyleBackColor = true;
            this.mbtnHis.Click += new System.EventHandler(this.mbtnHis_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.I0000037283);
            this.panel2.Controls.Add(this.I0000037282);
            this.panel2.Controls.Add(this.I0000037281);
            this.panel2.Controls.Add(this.mbtnSaveK);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(653, 36);
            this.panel2.TabIndex = 29;
            // 
            // I0000037283
            // 
            this.I0000037283.Location = new System.Drawing.Point(347, 7);
            this.I0000037283.Name = "I0000037283";
            this.I0000037283.ReadOnly = true;
            this.I0000037283.Size = new System.Drawing.Size(50, 21);
            this.I0000037283.TabIndex = 95;
            this.I0000037283.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // I0000037282
            // 
            this.I0000037282.Location = new System.Drawing.Point(196, 7);
            this.I0000037282.Name = "I0000037282";
            this.I0000037282.Size = new System.Drawing.Size(50, 21);
            this.I0000037282.TabIndex = 95;
            this.I0000037282.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.I0000037282.TextChanged += new System.EventHandler(this.I0000000562_1_TextChanged);
            // 
            // I0000037281
            // 
            this.I0000037281.Location = new System.Drawing.Point(74, 7);
            this.I0000037281.Name = "I0000037281";
            this.I0000037281.Size = new System.Drawing.Size(50, 21);
            this.I0000037281.TabIndex = 95;
            this.I0000037281.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.I0000037281.TextChanged += new System.EventHandler(this.I0000000562_1_TextChanged);
            // 
            // mbtnSaveK
            // 
            this.mbtnSaveK.BackColor = System.Drawing.SystemColors.Control;
            this.mbtnSaveK.Cursor = System.Windows.Forms.Cursors.Default;
            this.mbtnSaveK.ForeColor = System.Drawing.SystemColors.ControlText;
            this.mbtnSaveK.Location = new System.Drawing.Point(511, 2);
            this.mbtnSaveK.Name = "mbtnSaveK";
            this.mbtnSaveK.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.mbtnSaveK.Size = new System.Drawing.Size(83, 30);
            this.mbtnSaveK.TabIndex = 94;
            this.mbtnSaveK.Text = "저  장";
            this.mbtnSaveK.UseVisualStyleBackColor = true;
            this.mbtnSaveK.Click += new System.EventHandler(this.mbtnSaveK_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.SystemColors.Control;
            this.label5.Cursor = System.Windows.Forms.Cursors.Default;
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(305, 11);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label5.Size = new System.Drawing.Size(40, 12);
            this.label5.TabIndex = 89;
            this.label5.Text = "MPH :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.Control;
            this.label4.Cursor = System.Windows.Forms.Cursors.Default;
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(137, 11);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label4.Size = new System.Drawing.Size(55, 12);
            this.label4.TabIndex = 89;
            this.label4.Text = "모-신장 :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Control;
            this.label2.Cursor = System.Windows.Forms.Cursors.Default;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(15, 11);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label2.Size = new System.Drawing.Size(55, 12);
            this.label2.TabIndex = 89;
            this.label2.Text = "부-신장 :";
            // 
            // tabPage1
            // 
            this.tabPage1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabPage1.Controls.Add(this.panGraph);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(678, 760);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "그래프";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panGraph
            // 
            this.panGraph.BackColor = System.Drawing.Color.White;
            this.panGraph.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panGraph.Cursor = System.Windows.Forms.Cursors.Default;
            this.panGraph.Dock = System.Windows.Forms.DockStyle.Top;
            this.panGraph.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panGraph.Location = new System.Drawing.Point(0, 0);
            this.panGraph.Name = "panGraph";
            this.panGraph.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panGraph.Size = new System.Drawing.Size(676, 758);
            this.panGraph.TabIndex = 11;
            this.panGraph.TabStop = true;
            // 
            // mbtnGpPrint
            // 
            this.mbtnGpPrint.Location = new System.Drawing.Point(212, 36);
            this.mbtnGpPrint.Name = "mbtnGpPrint";
            this.mbtnGpPrint.Size = new System.Drawing.Size(75, 23);
            this.mbtnGpPrint.TabIndex = 14;
            this.mbtnGpPrint.Text = "출  력";
            this.mbtnGpPrint.UseVisualStyleBackColor = true;
            this.mbtnGpPrint.Click += new System.EventHandler(this.mbtnGpPrint_Click);
            // 
            // frmEmrHealthGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 790);
            this.Controls.Add(this.mbtnGpPrint);
            this.Controls.Add(this.panChart);
            this.Controls.Add(this.panTopMenu);
            this.Name = "frmEmrHealthGraph";
            this.Text = "frmEmrEF00100033";
            this.Load += new System.EventHandler(this.frmEmrEF00100033_Load);
            this.ResizeEnd += new System.EventHandler(this.frmEmrEF00100033_ResizeEnd);
            this.panChart.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panWrite.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssWrite)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssWrite_Sheet1)).EndInit();
            this.panSearch.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel panTopMenu;
        private System.Windows.Forms.Panel panChart;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panList;
        private FarPoint.Win.Spread.FpSpread ssList;
        private FarPoint.Win.Spread.SheetView ssList_Sheet1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkAsc;
        private System.Windows.Forms.Button mbtnPrint;
        public System.Windows.Forms.Button mbtnSearch;
        public System.Windows.Forms.Label Label1;
        public System.Windows.Forms.Label Label3;
        private System.Windows.Forms.DateTimePicker dtpFrDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Panel panWrite;
        private FarPoint.Win.Spread.FpSpread ssWrite;
        private FarPoint.Win.Spread.SheetView ssWrite_Sheet1;
        private System.Windows.Forms.Panel panSearch;
        public System.Windows.Forms.Button mbtnSaveAll;
        private System.Windows.Forms.Button mbtnHis;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox I0000037283;
        private System.Windows.Forms.TextBox I0000037282;
        private System.Windows.Forms.TextBox I0000037281;
        public System.Windows.Forms.Button mbtnSaveK;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabPage1;
        public System.Windows.Forms.Panel panGraph;
        private System.Windows.Forms.Button mbtnGpPrint;
    }
}