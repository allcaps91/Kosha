namespace ComEmrBase
{
    partial class frmEmrPatMemo
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.txtUseId = new System.Windows.Forms.TextBox();
            this.txtProbNo = new System.Windows.Forms.TextBox();
            this.panView = new System.Windows.Forms.Panel();
            this.ssProblemList = new FarPoint.Win.Spread.FpSpread();
            this.ssProblemList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.Panel13 = new System.Windows.Forms.Panel();
            this.chkDel = new System.Windows.Forms.CheckBox();
            this.mbtnSearch = new System.Windows.Forms.Button();
            this.mbtnExitView = new System.Windows.Forms.Button();
            this.chkPlistAll = new System.Windows.Forms.CheckBox();
            this.panReg = new System.Windows.Forms.Panel();
            this.txtReg = new System.Windows.Forms.TextBox();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.dtpIndate = new System.Windows.Forms.DateTimePicker();
            this.mbtnExitReg = new System.Windows.Forms.Button();
            this.mbtnClear = new System.Windows.Forms.Button();
            this.mbtnDelete = new System.Windows.Forms.Button();
            this.mbtnSave = new System.Windows.Forms.Button();
            this.panView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssProblemList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssProblemList_Sheet1)).BeginInit();
            this.Panel13.SuspendLayout();
            this.panReg.SuspendLayout();
            this.Panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtUseId
            // 
            this.txtUseId.Location = new System.Drawing.Point(218, 6);
            this.txtUseId.Name = "txtUseId";
            this.txtUseId.Size = new System.Drawing.Size(31, 21);
            this.txtUseId.TabIndex = 16;
            this.txtUseId.Visible = false;
            // 
            // txtProbNo
            // 
            this.txtProbNo.Location = new System.Drawing.Point(186, 5);
            this.txtProbNo.Name = "txtProbNo";
            this.txtProbNo.Size = new System.Drawing.Size(26, 21);
            this.txtProbNo.TabIndex = 15;
            this.txtProbNo.Visible = false;
            // 
            // panView
            // 
            this.panView.Controls.Add(this.ssProblemList);
            this.panView.Controls.Add(this.Panel13);
            this.panView.Dock = System.Windows.Forms.DockStyle.Top;
            this.panView.Location = new System.Drawing.Point(0, 0);
            this.panView.Name = "panView";
            this.panView.Size = new System.Drawing.Size(535, 261);
            this.panView.TabIndex = 14;
            // 
            // ssProblemList
            // 
            this.ssProblemList.AccessibleDescription = "ssProblemList, Sheet1, Row 0, Column 0, 2010-11-11";
            this.ssProblemList.BackColor = System.Drawing.SystemColors.Control;
            this.ssProblemList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssProblemList.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssProblemList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssProblemList.Location = new System.Drawing.Point(0, 32);
            this.ssProblemList.Name = "ssProblemList";
            this.ssProblemList.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ssProblemList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssProblemList_Sheet1});
            this.ssProblemList.Size = new System.Drawing.Size(535, 229);
            this.ssProblemList.TabIndex = 10;
            this.ssProblemList.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssProblemList_CellDoubleClick);
            // 
            // ssProblemList_Sheet1
            // 
            this.ssProblemList_Sheet1.Reset();
            this.ssProblemList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssProblemList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssProblemList_Sheet1.ColumnCount = 6;
            this.ssProblemList_Sheet1.RowCount = 1;
            this.ssProblemList_Sheet1.ActiveSkin = new FarPoint.Win.Spread.SheetSkin("CustomSkin1", System.Drawing.Color.White, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.LightGray, FarPoint.Win.Spread.GridLines.Both, System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237))))), System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, false, false, false, false, false, true, false, true, "HeaderDefault", "HeaderDefault", "HeaderDefault", "DataAreaDefault", "HeaderDefault");
            this.ssProblemList_Sheet1.Cells.Get(0, 0).Value = "2010-11-11";
            this.ssProblemList_Sheet1.Cells.Get(0, 2).Value = "가나다라";
            this.ssProblemList_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssProblemList_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssProblemList_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssProblemList_Sheet1.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.ssProblemList_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssProblemList_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssProblemList_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssProblemList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssProblemList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssProblemList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.ssProblemList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssProblemList_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssProblemList_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "작성일자";
            this.ssProblemList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "내용";
            this.ssProblemList_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "작성자";
            this.ssProblemList_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "작성자id";
            this.ssProblemList_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "ProblemNo";
            this.ssProblemList_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "MODIFY";
            this.ssProblemList_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssProblemList_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssProblemList_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssProblemList_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssProblemList_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssProblemList_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssProblemList_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssProblemList_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssProblemList_Sheet1.ColumnHeader.Rows.Get(0).Height = 24F;
            this.ssProblemList_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssProblemList_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssProblemList_Sheet1.Columns.Get(0).Label = "작성일자";
            this.ssProblemList_Sheet1.Columns.Get(0).Locked = true;
            this.ssProblemList_Sheet1.Columns.Get(0).MergePolicy = FarPoint.Win.Spread.Model.MergePolicy.Restricted;
            this.ssProblemList_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssProblemList_Sheet1.Columns.Get(0).Width = 72F;
            textCellType2.Multiline = true;
            textCellType2.WordWrap = true;
            this.ssProblemList_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssProblemList_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssProblemList_Sheet1.Columns.Get(1).Label = "내용";
            this.ssProblemList_Sheet1.Columns.Get(1).Locked = true;
            this.ssProblemList_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssProblemList_Sheet1.Columns.Get(1).Width = 385F;
            this.ssProblemList_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssProblemList_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssProblemList_Sheet1.Columns.Get(2).Label = "작성자";
            this.ssProblemList_Sheet1.Columns.Get(2).Locked = true;
            this.ssProblemList_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssProblemList_Sheet1.Columns.Get(2).Width = 54F;
            this.ssProblemList_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssProblemList_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssProblemList_Sheet1.Columns.Get(3).Label = "작성자id";
            this.ssProblemList_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssProblemList_Sheet1.Columns.Get(3).Visible = false;
            this.ssProblemList_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.ssProblemList_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssProblemList_Sheet1.Columns.Get(4).Label = "ProblemNo";
            this.ssProblemList_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssProblemList_Sheet1.Columns.Get(4).Visible = false;
            this.ssProblemList_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.ssProblemList_Sheet1.Columns.Get(5).Label = "MODIFY";
            this.ssProblemList_Sheet1.Columns.Get(5).Visible = false;
            this.ssProblemList_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssProblemList_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssProblemList_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.ssProblemList_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssProblemList_Sheet1.FilterBar.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssProblemList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssProblemList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssProblemList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssProblemList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssProblemList_Sheet1.FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssProblemList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssProblemList_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssProblemList_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssProblemList_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssProblemList_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssProblemList_Sheet1.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssProblemList_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssProblemList_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssProblemList_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssProblemList_Sheet1.RowHeader.Visible = false;
            this.ssProblemList_Sheet1.Rows.Get(0).Height = 94F;
            this.ssProblemList_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssProblemList_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssProblemList_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssProblemList_Sheet1.SheetCornerStyle.Parent = "HeaderDefault";
            this.ssProblemList_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssProblemList_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssProblemList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // Panel13
            // 
            this.Panel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.Panel13.Controls.Add(this.txtUseId);
            this.Panel13.Controls.Add(this.txtProbNo);
            this.Panel13.Controls.Add(this.chkDel);
            this.Panel13.Controls.Add(this.mbtnSearch);
            this.Panel13.Controls.Add(this.mbtnExitView);
            this.Panel13.Controls.Add(this.chkPlistAll);
            this.Panel13.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel13.Location = new System.Drawing.Point(0, 0);
            this.Panel13.Name = "Panel13";
            this.Panel13.Size = new System.Drawing.Size(535, 32);
            this.Panel13.TabIndex = 9;
            // 
            // chkDel
            // 
            this.chkDel.AutoSize = true;
            this.chkDel.Location = new System.Drawing.Point(108, 8);
            this.chkDel.Name = "chkDel";
            this.chkDel.Size = new System.Drawing.Size(72, 16);
            this.chkDel.TabIndex = 130;
            this.chkDel.Text = "삭제포함";
            this.chkDel.UseVisualStyleBackColor = true;
            // 
            // mbtnSearch
            // 
            this.mbtnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mbtnSearch.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.mbtnSearch.Location = new System.Drawing.Point(364, 3);
            this.mbtnSearch.Name = "mbtnSearch";
            this.mbtnSearch.Size = new System.Drawing.Size(75, 26);
            this.mbtnSearch.TabIndex = 128;
            this.mbtnSearch.Text = "조회";
            this.mbtnSearch.UseVisualStyleBackColor = true;
            this.mbtnSearch.Click += new System.EventHandler(this.mbtnSearch_Click);
            // 
            // mbtnExitView
            // 
            this.mbtnExitView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mbtnExitView.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.mbtnExitView.Location = new System.Drawing.Point(439, 3);
            this.mbtnExitView.Name = "mbtnExitView";
            this.mbtnExitView.Size = new System.Drawing.Size(75, 26);
            this.mbtnExitView.TabIndex = 4;
            this.mbtnExitView.Text = "닫기";
            this.mbtnExitView.UseVisualStyleBackColor = true;
            this.mbtnExitView.Click += new System.EventHandler(this.mbtnExitView_Click);
            // 
            // chkPlistAll
            // 
            this.chkPlistAll.AutoSize = true;
            this.chkPlistAll.Location = new System.Drawing.Point(13, 8);
            this.chkPlistAll.Name = "chkPlistAll";
            this.chkPlistAll.Size = new System.Drawing.Size(72, 16);
            this.chkPlistAll.TabIndex = 125;
            this.chkPlistAll.Text = "타과포함";
            this.chkPlistAll.UseVisualStyleBackColor = true;
            // 
            // panReg
            // 
            this.panReg.Controls.Add(this.txtReg);
            this.panReg.Controls.Add(this.Panel1);
            this.panReg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panReg.Location = new System.Drawing.Point(0, 261);
            this.panReg.Name = "panReg";
            this.panReg.Padding = new System.Windows.Forms.Padding(4);
            this.panReg.Size = new System.Drawing.Size(535, 148);
            this.panReg.TabIndex = 13;
            // 
            // txtReg
            // 
            this.txtReg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtReg.Location = new System.Drawing.Point(4, 37);
            this.txtReg.Multiline = true;
            this.txtReg.Name = "txtReg";
            this.txtReg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtReg.Size = new System.Drawing.Size(527, 107);
            this.txtReg.TabIndex = 1;
            // 
            // Panel1
            // 
            this.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Panel1.Controls.Add(this.button2);
            this.Panel1.Controls.Add(this.button1);
            this.Panel1.Controls.Add(this.Label1);
            this.Panel1.Controls.Add(this.dtpIndate);
            this.Panel1.Controls.Add(this.mbtnExitReg);
            this.Panel1.Controls.Add(this.mbtnClear);
            this.Panel1.Controls.Add(this.mbtnDelete);
            this.Panel1.Controls.Add(this.mbtnSave);
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel1.Location = new System.Drawing.Point(4, 4);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(527, 33);
            this.Panel1.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(278, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(29, 23);
            this.button2.TabIndex = 128;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(243, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(29, 23);
            this.button1.TabIndex = 127;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(7, 9);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(41, 12);
            this.Label1.TabIndex = 12;
            this.Label1.Text = "등록일";
            // 
            // dtpIndate
            // 
            this.dtpIndate.CalendarFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpIndate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpIndate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpIndate.Location = new System.Drawing.Point(52, 5);
            this.dtpIndate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpIndate.Name = "dtpIndate";
            this.dtpIndate.Size = new System.Drawing.Size(88, 21);
            this.dtpIndate.TabIndex = 126;
            // 
            // mbtnExitReg
            // 
            this.mbtnExitReg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mbtnExitReg.Location = new System.Drawing.Point(176, 3);
            this.mbtnExitReg.Name = "mbtnExitReg";
            this.mbtnExitReg.Size = new System.Drawing.Size(60, 24);
            this.mbtnExitReg.TabIndex = 5;
            this.mbtnExitReg.Text = "닫 기";
            this.mbtnExitReg.UseVisualStyleBackColor = true;
            this.mbtnExitReg.Visible = false;
            this.mbtnExitReg.Click += new System.EventHandler(this.mbtnExitReg_Click);
            // 
            // mbtnClear
            // 
            this.mbtnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mbtnClear.Location = new System.Drawing.Point(388, 1);
            this.mbtnClear.Name = "mbtnClear";
            this.mbtnClear.Size = new System.Drawing.Size(60, 27);
            this.mbtnClear.TabIndex = 4;
            this.mbtnClear.Text = "Clear";
            this.mbtnClear.UseVisualStyleBackColor = true;
            this.mbtnClear.Click += new System.EventHandler(this.mbtnClear_Click);
            // 
            // mbtnDelete
            // 
            this.mbtnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mbtnDelete.Location = new System.Drawing.Point(448, 1);
            this.mbtnDelete.Name = "mbtnDelete";
            this.mbtnDelete.Size = new System.Drawing.Size(60, 27);
            this.mbtnDelete.TabIndex = 3;
            this.mbtnDelete.Text = "삭 제";
            this.mbtnDelete.UseVisualStyleBackColor = true;
            this.mbtnDelete.Click += new System.EventHandler(this.mbtnDelete_Click);
            // 
            // mbtnSave
            // 
            this.mbtnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mbtnSave.Location = new System.Drawing.Point(328, 1);
            this.mbtnSave.Name = "mbtnSave";
            this.mbtnSave.Size = new System.Drawing.Size(60, 27);
            this.mbtnSave.TabIndex = 2;
            this.mbtnSave.Text = "등 록";
            this.mbtnSave.UseVisualStyleBackColor = true;
            this.mbtnSave.Click += new System.EventHandler(this.mbtnSave_Click);
            // 
            // frmEmrPatMemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 409);
            this.ControlBox = false;
            this.Controls.Add(this.panReg);
            this.Controls.Add(this.panView);
            this.Name = "frmEmrPatMemo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "문제목록";
            this.Load += new System.EventHandler(this.frmEmrPatMemo_Load);
            this.panView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssProblemList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssProblemList_Sheet1)).EndInit();
            this.Panel13.ResumeLayout(false);
            this.Panel13.PerformLayout();
            this.panReg.ResumeLayout(false);
            this.panReg.PerformLayout();
            this.Panel1.ResumeLayout(false);
            this.Panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.TextBox txtUseId;
        internal System.Windows.Forms.TextBox txtProbNo;
        internal System.Windows.Forms.Panel panView;
        internal FarPoint.Win.Spread.FpSpread ssProblemList;
        internal FarPoint.Win.Spread.SheetView ssProblemList_Sheet1;
        internal System.Windows.Forms.Panel Panel13;
        internal System.Windows.Forms.CheckBox chkDel;
        internal System.Windows.Forms.Button mbtnSearch;
        internal System.Windows.Forms.CheckBox chkPlistAll;
        internal System.Windows.Forms.Panel panReg;
        internal System.Windows.Forms.TextBox txtReg;
        internal System.Windows.Forms.Panel Panel1;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.DateTimePicker dtpIndate;
        internal System.Windows.Forms.Button mbtnExitReg;
        internal System.Windows.Forms.Button mbtnClear;
        internal System.Windows.Forms.Button mbtnDelete;
        internal System.Windows.Forms.Button mbtnSave;
        internal System.Windows.Forms.Button mbtnExitView;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;

    }
}