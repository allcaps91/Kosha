namespace ComEmrBase
{
    partial class frmEmrBaseViewVitalandActing
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType37 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType38 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType39 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType40 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType41 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType42 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType43 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType44 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType45 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType46 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType47 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType48 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnIO = new System.Windows.Forms.Button();
            this.btnRegItem = new System.Windows.Forms.Button();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFrDate = new System.Windows.Forms.DateTimePicker();
            this.btnSearchFilter = new System.Windows.Forms.Button();
            this.btnSearchAll = new System.Windows.Forms.Button();
            this.chkAsc = new System.Windows.Forms.CheckBox();
            this.Label9 = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabActing = new System.Windows.Forms.TabPage();
            this.ssAct = new FarPoint.Win.Spread.FpSpread();
            this.ssAct_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.tabVital = new System.Windows.Forms.TabPage();
            this.ssVital = new FarPoint.Win.Spread.FpSpread();
            this.ssVital_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.tabSpo2RR = new System.Windows.Forms.TabPage();
            this.ssSpo2RR = new FarPoint.Win.Spread.FpSpread();
            this.ssSpo2RR_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.tabRecord3 = new System.Windows.Forms.TabPage();
            this.ssRecord3 = new FarPoint.Win.Spread.FpSpread();
            this.ssRecord3_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.btnViewInpUser = new System.Windows.Forms.Button();
            this.panTitle.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabActing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssAct)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssAct_Sheet1)).BeginInit();
            this.tabVital.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssVital)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssVital_Sheet1)).BeginInit();
            this.tabSpo2RR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssSpo2RR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssSpo2RR_Sheet1)).BeginInit();
            this.tabRecord3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssRecord3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssRecord3_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.SystemColors.Control;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnViewInpUser);
            this.panTitle.Controls.Add(this.btnPrint);
            this.panTitle.Controls.Add(this.btnIO);
            this.panTitle.Controls.Add(this.btnRegItem);
            this.panTitle.Controls.Add(this.dtpEndDate);
            this.panTitle.Controls.Add(this.dtpFrDate);
            this.panTitle.Controls.Add(this.btnSearchFilter);
            this.panTitle.Controls.Add(this.btnSearchAll);
            this.panTitle.Controls.Add(this.chkAsc);
            this.panTitle.Controls.Add(this.Label9);
            this.panTitle.Controls.Add(this.Label8);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1084, 39);
            this.panTitle.TabIndex = 89;
            // 
            // btnPrint
            // 
            this.btnPrint.Enabled = false;
            this.btnPrint.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPrint.ForeColor = System.Drawing.Color.Black;
            this.btnPrint.Location = new System.Drawing.Point(666, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 30);
            this.btnPrint.TabIndex = 97;
            this.btnPrint.Text = "출력";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnIO
            // 
            this.btnIO.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnIO.ForeColor = System.Drawing.Color.Black;
            this.btnIO.Location = new System.Drawing.Point(594, 4);
            this.btnIO.Name = "btnIO";
            this.btnIO.Size = new System.Drawing.Size(72, 30);
            this.btnIO.TabIndex = 96;
            this.btnIO.Text = "IO합계";
            this.btnIO.UseVisualStyleBackColor = true;
            this.btnIO.Click += new System.EventHandler(this.btnIO_Click);
            // 
            // btnRegItem
            // 
            this.btnRegItem.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRegItem.ForeColor = System.Drawing.Color.Black;
            this.btnRegItem.Location = new System.Drawing.Point(522, 4);
            this.btnRegItem.Name = "btnRegItem";
            this.btnRegItem.Size = new System.Drawing.Size(72, 30);
            this.btnRegItem.TabIndex = 96;
            this.btnRegItem.Text = "아이템";
            this.btnRegItem.UseVisualStyleBackColor = true;
            this.btnRegItem.Click += new System.EventHandler(this.btnRegItem_Click);
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new System.Drawing.Point(190, 7);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(103, 25);
            this.dtpEndDate.TabIndex = 95;
            // 
            // dtpFrDate
            // 
            this.dtpFrDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrDate.Location = new System.Drawing.Point(70, 7);
            this.dtpFrDate.Name = "dtpFrDate";
            this.dtpFrDate.Size = new System.Drawing.Size(103, 25);
            this.dtpFrDate.TabIndex = 94;
            // 
            // btnSearchFilter
            // 
            this.btnSearchFilter.BackColor = System.Drawing.SystemColors.Control;
            this.btnSearchFilter.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSearchFilter.Font = new System.Drawing.Font("굴림", 9F);
            this.btnSearchFilter.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSearchFilter.Location = new System.Drawing.Point(443, 4);
            this.btnSearchFilter.Name = "btnSearchFilter";
            this.btnSearchFilter.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnSearchFilter.Size = new System.Drawing.Size(79, 30);
            this.btnSearchFilter.TabIndex = 91;
            this.btnSearchFilter.Text = "필터조회";
            this.btnSearchFilter.UseVisualStyleBackColor = true;
            this.btnSearchFilter.Click += new System.EventHandler(this.btnSearchAll_Click);
            // 
            // btnSearchAll
            // 
            this.btnSearchAll.BackColor = System.Drawing.SystemColors.Control;
            this.btnSearchAll.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSearchAll.Font = new System.Drawing.Font("굴림", 9F);
            this.btnSearchAll.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSearchAll.Location = new System.Drawing.Point(364, 4);
            this.btnSearchAll.Name = "btnSearchAll";
            this.btnSearchAll.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnSearchAll.Size = new System.Drawing.Size(79, 30);
            this.btnSearchAll.TabIndex = 91;
            this.btnSearchAll.Text = "조회";
            this.btnSearchAll.UseVisualStyleBackColor = true;
            this.btnSearchAll.Click += new System.EventHandler(this.btnSearchAll_Click);
            // 
            // chkAsc
            // 
            this.chkAsc.AutoSize = true;
            this.chkAsc.BackColor = System.Drawing.Color.Transparent;
            this.chkAsc.Cursor = System.Windows.Forms.Cursors.Default;
            this.chkAsc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkAsc.ForeColor = System.Drawing.SystemColors.WindowText;
            this.chkAsc.Location = new System.Drawing.Point(301, 9);
            this.chkAsc.Name = "chkAsc";
            this.chkAsc.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chkAsc.Size = new System.Drawing.Size(63, 21);
            this.chkAsc.TabIndex = 90;
            this.chkAsc.Text = "순정렬";
            this.chkAsc.UseVisualStyleBackColor = false;
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.BackColor = System.Drawing.Color.Transparent;
            this.Label9.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label9.Location = new System.Drawing.Point(173, 11);
            this.Label9.Name = "Label9";
            this.Label9.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label9.Size = new System.Drawing.Size(17, 17);
            this.Label9.TabIndex = 93;
            this.Label9.Text = "~";
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.BackColor = System.Drawing.Color.Transparent;
            this.Label8.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label8.Location = new System.Drawing.Point(10, 11);
            this.Label8.Name = "Label8";
            this.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label8.Size = new System.Drawing.Size(60, 17);
            this.Label8.TabIndex = 92;
            this.Label8.Text = "조회기간";
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(738, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabActing);
            this.tabControl1.Controls.Add(this.tabVital);
            this.tabControl1.Controls.Add(this.tabSpo2RR);
            this.tabControl1.Controls.Add(this.tabRecord3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 39);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1084, 702);
            this.tabControl1.TabIndex = 90;
            // 
            // tabActing
            // 
            this.tabActing.Controls.Add(this.ssAct);
            this.tabActing.Location = new System.Drawing.Point(4, 26);
            this.tabActing.Name = "tabActing";
            this.tabActing.Padding = new System.Windows.Forms.Padding(3);
            this.tabActing.Size = new System.Drawing.Size(1076, 672);
            this.tabActing.TabIndex = 1;
            this.tabActing.Text = "    간호활동    ";
            this.tabActing.UseVisualStyleBackColor = true;
            // 
            // ssAct
            // 
            this.ssAct.AccessibleDescription = "ssAct, Sheet1, Row 0, Column 0, ";
            this.ssAct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssAct.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssAct.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssAct.Location = new System.Drawing.Point(3, 3);
            this.ssAct.Name = "ssAct";
            this.ssAct.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssAct_Sheet1});
            this.ssAct.Size = new System.Drawing.Size(1070, 666);
            this.ssAct.TabIndex = 161;
            this.ssAct.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Floating;
            this.ssAct.SetViewportLeftColumn(0, 0, 3);
            this.ssAct.SetActiveViewport(0, 0, -1);
            // 
            // ssAct_Sheet1
            // 
            this.ssAct_Sheet1.Reset();
            this.ssAct_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssAct_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssAct_Sheet1.ColumnCount = 3;
            this.ssAct_Sheet1.ColumnHeader.RowCount = 2;
            this.ssAct_Sheet1.RowCount = 1;
            this.ssAct_Sheet1.ActiveSkin = new FarPoint.Win.Spread.SheetSkin("CustomSkin1", System.Drawing.Color.White, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.LightGray, FarPoint.Win.Spread.GridLines.Both, System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237))))), System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, false, false, false, false, false, true, false, true, "HeaderDefault", "HeaderDefault", "HeaderDefault", "DataAreaDefault", "HeaderDefault");
            this.ssAct_Sheet1.AutoFilterMode = FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu;
            this.ssAct_Sheet1.Cells.Get(0, 1).Value = "기본간호활동";
            this.ssAct_Sheet1.Cells.Get(0, 2).Value = "기본간호활동";
            this.ssAct_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssAct_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssAct_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssAct_Sheet1.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.ssAct_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssAct_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssAct_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssAct_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssAct_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssAct_Sheet1.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.ssAct_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssAct_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssAct_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "ItemCode";
            this.ssAct_Sheet1.ColumnHeader.Cells.Get(0, 1).RowSpan = 2;
            this.ssAct_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "구분";
            this.ssAct_Sheet1.ColumnHeader.Cells.Get(0, 2).RowSpan = 2;
            this.ssAct_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "항목";
            this.ssAct_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssAct_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssAct_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssAct_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssAct_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssAct_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssAct_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssAct_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssAct_Sheet1.Columns.Get(0).CellType = textCellType37;
            this.ssAct_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssAct_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssAct_Sheet1.Columns.Get(0).Visible = false;
            this.ssAct_Sheet1.Columns.Get(0).Width = 128F;
            this.ssAct_Sheet1.Columns.Get(1).AllowAutoFilter = true;
            this.ssAct_Sheet1.Columns.Get(1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            textCellType38.MaxLength = 400;
            textCellType38.Multiline = true;
            textCellType38.WordWrap = true;
            this.ssAct_Sheet1.Columns.Get(1).CellType = textCellType38;
            this.ssAct_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssAct_Sheet1.Columns.Get(1).Locked = true;
            this.ssAct_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssAct_Sheet1.Columns.Get(1).Width = 80F;
            this.ssAct_Sheet1.Columns.Get(2).AllowAutoFilter = true;
            this.ssAct_Sheet1.Columns.Get(2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            textCellType39.MaxLength = 400;
            textCellType39.Multiline = true;
            textCellType39.WordWrap = true;
            this.ssAct_Sheet1.Columns.Get(2).CellType = textCellType39;
            this.ssAct_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssAct_Sheet1.Columns.Get(2).Locked = true;
            this.ssAct_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssAct_Sheet1.Columns.Get(2).Width = 215F;
            this.ssAct_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssAct_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssAct_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.ssAct_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssAct_Sheet1.FilterBar.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssAct_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssAct_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssAct_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssAct_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssAct_Sheet1.FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssAct_Sheet1.FrozenColumnCount = 3;
            this.ssAct_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssAct_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssAct_Sheet1.RowHeader.Columns.Get(0).Width = 29F;
            this.ssAct_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssAct_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssAct_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssAct_Sheet1.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssAct_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssAct_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssAct_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssAct_Sheet1.RowHeader.Visible = false;
            this.ssAct_Sheet1.Rows.Get(0).Height = 38F;
            this.ssAct_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssAct_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssAct_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssAct_Sheet1.SheetCornerStyle.Parent = "HeaderDefault";
            this.ssAct_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssAct_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssAct_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // tabVital
            // 
            this.tabVital.Controls.Add(this.ssVital);
            this.tabVital.Location = new System.Drawing.Point(4, 26);
            this.tabVital.Name = "tabVital";
            this.tabVital.Padding = new System.Windows.Forms.Padding(3);
            this.tabVital.Size = new System.Drawing.Size(1076, 672);
            this.tabVital.TabIndex = 0;
            this.tabVital.Text = "    임상관찰    ";
            this.tabVital.UseVisualStyleBackColor = true;
            // 
            // ssVital
            // 
            this.ssVital.AccessibleDescription = "ssVital, Sheet1, Row 0, Column 0, ";
            this.ssVital.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssVital.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssVital.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssVital.Location = new System.Drawing.Point(3, 3);
            this.ssVital.Name = "ssVital";
            this.ssVital.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssVital_Sheet1});
            this.ssVital.Size = new System.Drawing.Size(1070, 666);
            this.ssVital.TabIndex = 160;
            this.ssVital.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Floating;
            this.ssVital.SetViewportLeftColumn(0, 0, 3);
            this.ssVital.SetActiveViewport(0, 0, -1);
            // 
            // ssVital_Sheet1
            // 
            this.ssVital_Sheet1.Reset();
            this.ssVital_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssVital_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssVital_Sheet1.ColumnCount = 3;
            this.ssVital_Sheet1.ColumnHeader.RowCount = 2;
            this.ssVital_Sheet1.RowCount = 1;
            this.ssVital_Sheet1.ActiveSkin = new FarPoint.Win.Spread.SheetSkin("CustomSkin1", System.Drawing.Color.White, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.LightGray, FarPoint.Win.Spread.GridLines.Both, System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237))))), System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, false, false, false, false, false, true, false, true, "HeaderDefault", "HeaderDefault", "HeaderDefault", "DataAreaDefault", "HeaderDefault");
            this.ssVital_Sheet1.AutoFilterMode = FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu;
            this.ssVital_Sheet1.Cells.Get(0, 1).Value = "기본간호활동";
            this.ssVital_Sheet1.Cells.Get(0, 2).Value = "기본간호활동";
            this.ssVital_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssVital_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssVital_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssVital_Sheet1.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.ssVital_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssVital_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssVital_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssVital_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssVital_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssVital_Sheet1.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.ssVital_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssVital_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssVital_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "ItemCode";
            this.ssVital_Sheet1.ColumnHeader.Cells.Get(0, 1).RowSpan = 2;
            this.ssVital_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "구분";
            this.ssVital_Sheet1.ColumnHeader.Cells.Get(0, 2).RowSpan = 2;
            this.ssVital_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "항목";
            this.ssVital_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssVital_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssVital_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssVital_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssVital_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssVital_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssVital_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssVital_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssVital_Sheet1.ColumnHeader.Rows.Get(0).Height = 19F;
            this.ssVital_Sheet1.Columns.Get(0).CellType = textCellType40;
            this.ssVital_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssVital_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssVital_Sheet1.Columns.Get(0).Visible = false;
            this.ssVital_Sheet1.Columns.Get(0).Width = 128F;
            this.ssVital_Sheet1.Columns.Get(1).AllowAutoFilter = true;
            this.ssVital_Sheet1.Columns.Get(1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            textCellType41.MaxLength = 400;
            textCellType41.Multiline = true;
            textCellType41.WordWrap = true;
            this.ssVital_Sheet1.Columns.Get(1).CellType = textCellType41;
            this.ssVital_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssVital_Sheet1.Columns.Get(1).Locked = true;
            this.ssVital_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssVital_Sheet1.Columns.Get(1).Width = 80F;
            this.ssVital_Sheet1.Columns.Get(2).AllowAutoFilter = true;
            this.ssVital_Sheet1.Columns.Get(2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            textCellType42.MaxLength = 400;
            textCellType42.Multiline = true;
            textCellType42.WordWrap = true;
            this.ssVital_Sheet1.Columns.Get(2).CellType = textCellType42;
            this.ssVital_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssVital_Sheet1.Columns.Get(2).Locked = true;
            this.ssVital_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssVital_Sheet1.Columns.Get(2).Width = 215F;
            this.ssVital_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssVital_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssVital_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.ssVital_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssVital_Sheet1.FilterBar.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssVital_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssVital_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssVital_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssVital_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssVital_Sheet1.FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssVital_Sheet1.FrozenColumnCount = 3;
            this.ssVital_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssVital_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssVital_Sheet1.RowHeader.Columns.Get(0).Width = 29F;
            this.ssVital_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssVital_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssVital_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssVital_Sheet1.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssVital_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssVital_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssVital_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssVital_Sheet1.RowHeader.Visible = false;
            this.ssVital_Sheet1.Rows.Get(0).Height = 38F;
            this.ssVital_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssVital_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssVital_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssVital_Sheet1.SheetCornerStyle.Parent = "HeaderDefault";
            this.ssVital_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssVital_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssVital_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // tabSpo2RR
            // 
            this.tabSpo2RR.Controls.Add(this.ssSpo2RR);
            this.tabSpo2RR.Location = new System.Drawing.Point(4, 26);
            this.tabSpo2RR.Name = "tabSpo2RR";
            this.tabSpo2RR.Size = new System.Drawing.Size(1076, 672);
            this.tabSpo2RR.TabIndex = 2;
            this.tabSpo2RR.Text = "산소/인공호흡";
            this.tabSpo2RR.UseVisualStyleBackColor = true;
            // 
            // ssSpo2RR
            // 
            this.ssSpo2RR.AccessibleDescription = "ssAct, Sheet1, Row 0, Column 0, ";
            this.ssSpo2RR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssSpo2RR.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssSpo2RR.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssSpo2RR.Location = new System.Drawing.Point(0, 0);
            this.ssSpo2RR.Name = "ssSpo2RR";
            this.ssSpo2RR.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssSpo2RR_Sheet1});
            this.ssSpo2RR.Size = new System.Drawing.Size(1076, 672);
            this.ssSpo2RR.TabIndex = 162;
            this.ssSpo2RR.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Floating;
            this.ssSpo2RR.SetViewportLeftColumn(0, 0, 3);
            this.ssSpo2RR.SetActiveViewport(0, 0, -1);
            // 
            // ssSpo2RR_Sheet1
            // 
            this.ssSpo2RR_Sheet1.Reset();
            this.ssSpo2RR_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssSpo2RR_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssSpo2RR_Sheet1.ColumnCount = 3;
            this.ssSpo2RR_Sheet1.ColumnHeader.RowCount = 2;
            this.ssSpo2RR_Sheet1.RowCount = 1;
            this.ssSpo2RR_Sheet1.ActiveSkin = new FarPoint.Win.Spread.SheetSkin("CustomSkin1", System.Drawing.Color.White, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.LightGray, FarPoint.Win.Spread.GridLines.Both, System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237))))), System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, false, false, false, false, false, true, false, true, "HeaderDefault", "HeaderDefault", "HeaderDefault", "DataAreaDefault", "HeaderDefault");
            this.ssSpo2RR_Sheet1.AutoFilterMode = FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu;
            this.ssSpo2RR_Sheet1.Cells.Get(0, 1).Value = "기본간호활동";
            this.ssSpo2RR_Sheet1.Cells.Get(0, 2).Value = "기본간호활동";
            this.ssSpo2RR_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssSpo2RR_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssSpo2RR_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssSpo2RR_Sheet1.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.ssSpo2RR_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssSpo2RR_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssSpo2RR_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssSpo2RR_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssSpo2RR_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssSpo2RR_Sheet1.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.ssSpo2RR_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssSpo2RR_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssSpo2RR_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "ItemCode";
            this.ssSpo2RR_Sheet1.ColumnHeader.Cells.Get(0, 1).RowSpan = 2;
            this.ssSpo2RR_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "구분";
            this.ssSpo2RR_Sheet1.ColumnHeader.Cells.Get(0, 2).RowSpan = 2;
            this.ssSpo2RR_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "항목";
            this.ssSpo2RR_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssSpo2RR_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssSpo2RR_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssSpo2RR_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssSpo2RR_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssSpo2RR_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssSpo2RR_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssSpo2RR_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssSpo2RR_Sheet1.Columns.Get(0).CellType = textCellType43;
            this.ssSpo2RR_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSpo2RR_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSpo2RR_Sheet1.Columns.Get(0).Visible = false;
            this.ssSpo2RR_Sheet1.Columns.Get(0).Width = 128F;
            this.ssSpo2RR_Sheet1.Columns.Get(1).AllowAutoFilter = true;
            this.ssSpo2RR_Sheet1.Columns.Get(1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            textCellType44.MaxLength = 400;
            textCellType44.Multiline = true;
            textCellType44.WordWrap = true;
            this.ssSpo2RR_Sheet1.Columns.Get(1).CellType = textCellType44;
            this.ssSpo2RR_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssSpo2RR_Sheet1.Columns.Get(1).Locked = true;
            this.ssSpo2RR_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSpo2RR_Sheet1.Columns.Get(1).Width = 80F;
            this.ssSpo2RR_Sheet1.Columns.Get(2).AllowAutoFilter = true;
            this.ssSpo2RR_Sheet1.Columns.Get(2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            textCellType45.MaxLength = 400;
            textCellType45.Multiline = true;
            textCellType45.WordWrap = true;
            this.ssSpo2RR_Sheet1.Columns.Get(2).CellType = textCellType45;
            this.ssSpo2RR_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssSpo2RR_Sheet1.Columns.Get(2).Locked = true;
            this.ssSpo2RR_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSpo2RR_Sheet1.Columns.Get(2).Width = 215F;
            this.ssSpo2RR_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssSpo2RR_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssSpo2RR_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.ssSpo2RR_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssSpo2RR_Sheet1.FilterBar.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssSpo2RR_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssSpo2RR_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssSpo2RR_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssSpo2RR_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssSpo2RR_Sheet1.FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssSpo2RR_Sheet1.FrozenColumnCount = 3;
            this.ssSpo2RR_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssSpo2RR_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssSpo2RR_Sheet1.RowHeader.Columns.Get(0).Width = 29F;
            this.ssSpo2RR_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssSpo2RR_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssSpo2RR_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssSpo2RR_Sheet1.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssSpo2RR_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssSpo2RR_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssSpo2RR_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssSpo2RR_Sheet1.RowHeader.Visible = false;
            this.ssSpo2RR_Sheet1.Rows.Get(0).Height = 38F;
            this.ssSpo2RR_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssSpo2RR_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssSpo2RR_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssSpo2RR_Sheet1.SheetCornerStyle.Parent = "HeaderDefault";
            this.ssSpo2RR_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssSpo2RR_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssSpo2RR_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // tabRecord3
            // 
            this.tabRecord3.Controls.Add(this.ssRecord3);
            this.tabRecord3.Location = new System.Drawing.Point(4, 26);
            this.tabRecord3.Name = "tabRecord3";
            this.tabRecord3.Size = new System.Drawing.Size(1076, 672);
            this.tabRecord3.TabIndex = 3;
            this.tabRecord3.Text = "상처/욕창/중심정맥관";
            this.tabRecord3.UseVisualStyleBackColor = true;
            // 
            // ssRecord3
            // 
            this.ssRecord3.AccessibleDescription = "ssAct, Sheet1, Row 0, Column 0, ";
            this.ssRecord3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssRecord3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssRecord3.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssRecord3.Location = new System.Drawing.Point(0, 0);
            this.ssRecord3.Name = "ssRecord3";
            this.ssRecord3.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssRecord3_Sheet1});
            this.ssRecord3.Size = new System.Drawing.Size(1076, 672);
            this.ssRecord3.TabIndex = 163;
            this.ssRecord3.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Floating;
            this.ssRecord3.SetViewportLeftColumn(0, 0, 3);
            this.ssRecord3.SetActiveViewport(0, 0, -1);
            // 
            // ssRecord3_Sheet1
            // 
            this.ssRecord3_Sheet1.Reset();
            this.ssRecord3_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssRecord3_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssRecord3_Sheet1.ColumnCount = 3;
            this.ssRecord3_Sheet1.ColumnHeader.RowCount = 2;
            this.ssRecord3_Sheet1.RowCount = 1;
            this.ssRecord3_Sheet1.ActiveSkin = new FarPoint.Win.Spread.SheetSkin("CustomSkin1", System.Drawing.Color.White, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.LightGray, FarPoint.Win.Spread.GridLines.Both, System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237))))), System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, false, false, false, false, false, true, false, true, "HeaderDefault", "HeaderDefault", "HeaderDefault", "DataAreaDefault", "HeaderDefault");
            this.ssRecord3_Sheet1.AutoFilterMode = FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu;
            this.ssRecord3_Sheet1.Cells.Get(0, 1).Value = "기본간호활동";
            this.ssRecord3_Sheet1.Cells.Get(0, 2).Value = "기본간호활동";
            this.ssRecord3_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssRecord3_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssRecord3_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssRecord3_Sheet1.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.ssRecord3_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssRecord3_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssRecord3_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssRecord3_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssRecord3_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssRecord3_Sheet1.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.ssRecord3_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssRecord3_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssRecord3_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "ItemCode";
            this.ssRecord3_Sheet1.ColumnHeader.Cells.Get(0, 1).RowSpan = 2;
            this.ssRecord3_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "구분";
            this.ssRecord3_Sheet1.ColumnHeader.Cells.Get(0, 2).RowSpan = 2;
            this.ssRecord3_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "항목";
            this.ssRecord3_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssRecord3_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssRecord3_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssRecord3_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssRecord3_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssRecord3_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssRecord3_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssRecord3_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssRecord3_Sheet1.Columns.Get(0).CellType = textCellType46;
            this.ssRecord3_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssRecord3_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssRecord3_Sheet1.Columns.Get(0).Visible = false;
            this.ssRecord3_Sheet1.Columns.Get(0).Width = 128F;
            this.ssRecord3_Sheet1.Columns.Get(1).AllowAutoFilter = true;
            this.ssRecord3_Sheet1.Columns.Get(1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            textCellType47.MaxLength = 400;
            textCellType47.Multiline = true;
            textCellType47.WordWrap = true;
            this.ssRecord3_Sheet1.Columns.Get(1).CellType = textCellType47;
            this.ssRecord3_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssRecord3_Sheet1.Columns.Get(1).Locked = true;
            this.ssRecord3_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssRecord3_Sheet1.Columns.Get(1).Width = 80F;
            this.ssRecord3_Sheet1.Columns.Get(2).AllowAutoFilter = true;
            this.ssRecord3_Sheet1.Columns.Get(2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            textCellType48.MaxLength = 400;
            textCellType48.Multiline = true;
            textCellType48.WordWrap = true;
            this.ssRecord3_Sheet1.Columns.Get(2).CellType = textCellType48;
            this.ssRecord3_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssRecord3_Sheet1.Columns.Get(2).Locked = true;
            this.ssRecord3_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssRecord3_Sheet1.Columns.Get(2).Width = 215F;
            this.ssRecord3_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssRecord3_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssRecord3_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.ssRecord3_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssRecord3_Sheet1.FilterBar.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssRecord3_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssRecord3_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssRecord3_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssRecord3_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssRecord3_Sheet1.FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssRecord3_Sheet1.FrozenColumnCount = 3;
            this.ssRecord3_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssRecord3_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssRecord3_Sheet1.RowHeader.Columns.Get(0).Width = 29F;
            this.ssRecord3_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssRecord3_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssRecord3_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssRecord3_Sheet1.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssRecord3_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssRecord3_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssRecord3_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssRecord3_Sheet1.RowHeader.Visible = false;
            this.ssRecord3_Sheet1.Rows.Get(0).Height = 38F;
            this.ssRecord3_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssRecord3_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssRecord3_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssRecord3_Sheet1.SheetCornerStyle.Parent = "HeaderDefault";
            this.ssRecord3_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssRecord3_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssRecord3_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // btnViewInpUser
            // 
            this.btnViewInpUser.BackColor = System.Drawing.SystemColors.Control;
            this.btnViewInpUser.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnViewInpUser.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnViewInpUser.Location = new System.Drawing.Point(982, 6);
            this.btnViewInpUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnViewInpUser.Name = "btnViewInpUser";
            this.btnViewInpUser.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnViewInpUser.Size = new System.Drawing.Size(88, 26);
            this.btnViewInpUser.TabIndex = 115;
            this.btnViewInpUser.Text = "작성자 보기";
            this.btnViewInpUser.UseVisualStyleBackColor = true;
            this.btnViewInpUser.Click += new System.EventHandler(this.btnViewInpUser_Click);
            // 
            // frmEmrBaseViewVitalandActing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 741);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmEmrBaseViewVitalandActing";
            this.Text = "frmEmrBaseViewVitalandActing";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmEmrBaseViewVitalandActing_FormClosed);
            this.Load += new System.EventHandler(this.frmEmrBaseViewVitalandActing_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabActing.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssAct)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssAct_Sheet1)).EndInit();
            this.tabVital.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssVital)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssVital_Sheet1)).EndInit();
            this.tabSpo2RR.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssSpo2RR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssSpo2RR_Sheet1)).EndInit();
            this.tabRecord3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssRecord3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssRecord3_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.DateTimePicker dtpFrDate;
        public System.Windows.Forms.Button btnSearchAll;
        public System.Windows.Forms.CheckBox chkAsc;
        public System.Windows.Forms.Label Label9;
        public System.Windows.Forms.Label Label8;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabVital;
        private System.Windows.Forms.TabPage tabActing;
        private System.Windows.Forms.Button btnRegItem;
        private FarPoint.Win.Spread.FpSpread ssVital;
        private FarPoint.Win.Spread.SheetView ssVital_Sheet1;
        private FarPoint.Win.Spread.FpSpread ssAct;
        private FarPoint.Win.Spread.SheetView ssAct_Sheet1;
        private System.Windows.Forms.Button btnIO;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.TabPage tabSpo2RR;
        private FarPoint.Win.Spread.FpSpread ssSpo2RR;
        private FarPoint.Win.Spread.SheetView ssSpo2RR_Sheet1;
        private System.Windows.Forms.TabPage tabRecord3;
        private FarPoint.Win.Spread.FpSpread ssRecord3;
        private FarPoint.Win.Spread.SheetView ssRecord3_Sheet1;
        public System.Windows.Forms.Button btnSearchFilter;
        public System.Windows.Forms.Button btnViewInpUser;
    }
}