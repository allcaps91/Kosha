namespace HC_Act
{
    partial class frmHcActMemoSave
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType13 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType14 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.EnhancedFocusIndicatorRenderer enhancedFocusIndicatorRenderer2 = new FarPoint.Win.Spread.EnhancedFocusIndicatorRenderer();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer3 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer4 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType2 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType15 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType16 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnMemoSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtWrtNo = new System.Windows.Forms.TextBox();
            this.ssPatInfo = new FarPoint.Win.Spread.FpSpread();
            this.ssPatInfo_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel12 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.SS_ETC = new FarPoint.Win.Spread.FpSpread();
            this.SS_ETC_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssPatInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssPatInfo_Sheet1)).BeginInit();
            this.panel12.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS_ETC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS_ETC_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(633, 35);
            this.panTitle.TabIndex = 24;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(549, 0);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(82, 33);
            this.btnExit.TabIndex = 20;
            this.btnExit.Text = "닫 기(&X)";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(3, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(144, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "검사결과 메모입력";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnMemoSave);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.ssPatInfo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(633, 61);
            this.panel1.TabIndex = 25;
            // 
            // btnMemoSave
            // 
            this.btnMemoSave.BackColor = System.Drawing.Color.White;
            this.btnMemoSave.Location = new System.Drawing.Point(552, 4);
            this.btnMemoSave.Name = "btnMemoSave";
            this.btnMemoSave.Size = new System.Drawing.Size(74, 50);
            this.btnMemoSave.TabIndex = 48;
            this.btnMemoSave.Text = "메모저장";
            this.btnMemoSave.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtWrtNo);
            this.groupBox1.Location = new System.Drawing.Point(7, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(109, 50);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "접수번호";
            // 
            // txtWrtNo
            // 
            this.txtWrtNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtWrtNo.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtWrtNo.Location = new System.Drawing.Point(9, 17);
            this.txtWrtNo.Name = "txtWrtNo";
            this.txtWrtNo.Size = new System.Drawing.Size(92, 22);
            this.txtWrtNo.TabIndex = 25;
            this.txtWrtNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ssPatInfo
            // 
            this.ssPatInfo.AccessibleDescription = "ssPatInfo, Sheet1, Row 0, Column 0, ";
            this.ssPatInfo.FocusRenderer = flatFocusIndicatorRenderer2;
            this.ssPatInfo.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssPatInfo.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssPatInfo.HorizontalScrollBar.Renderer = flatScrollBarRenderer3;
            this.ssPatInfo.HorizontalScrollBar.TabIndex = 81;
            this.ssPatInfo.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssPatInfo.Location = new System.Drawing.Point(122, 6);
            this.ssPatInfo.Name = "ssPatInfo";
            this.ssPatInfo.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssPatInfo_Sheet1});
            this.ssPatInfo.Size = new System.Drawing.Size(426, 49);
            this.ssPatInfo.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.ssPatInfo.TabIndex = 27;
            this.ssPatInfo.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssPatInfo.VerticalScrollBar.Name = "";
            flatScrollBarRenderer4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssPatInfo.VerticalScrollBar.Renderer = flatScrollBarRenderer4;
            this.ssPatInfo.VerticalScrollBar.TabIndex = 82;
            this.ssPatInfo.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // ssPatInfo_Sheet1
            // 
            this.ssPatInfo_Sheet1.Reset();
            this.ssPatInfo_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssPatInfo_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssPatInfo_Sheet1.ColumnCount = 6;
            this.ssPatInfo_Sheet1.RowCount = 1;
            this.ssPatInfo_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPatInfo_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPatInfo_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.ssPatInfo_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPatInfo_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPatInfo_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPatInfo_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.ssPatInfo_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPatInfo_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "등록번호";
            this.ssPatInfo_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "성명";
            this.ssPatInfo_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "나이";
            this.ssPatInfo_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "사업체명";
            this.ssPatInfo_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "접수일자";
            this.ssPatInfo_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "검진종류";
            this.ssPatInfo_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPatInfo_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPatInfo_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.ssPatInfo_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPatInfo_Sheet1.ColumnHeader.Rows.Get(0).Height = 25F;
            this.ssPatInfo_Sheet1.Columns.Get(0).CellType = textCellType9;
            this.ssPatInfo_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(0).Label = "등록번호";
            this.ssPatInfo_Sheet1.Columns.Get(0).Locked = true;
            this.ssPatInfo_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(0).Width = 74F;
            this.ssPatInfo_Sheet1.Columns.Get(1).CellType = textCellType10;
            this.ssPatInfo_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(1).Label = "성명";
            this.ssPatInfo_Sheet1.Columns.Get(1).Locked = true;
            this.ssPatInfo_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(2).CellType = textCellType11;
            this.ssPatInfo_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(2).Label = "나이";
            this.ssPatInfo_Sheet1.Columns.Get(2).Locked = true;
            this.ssPatInfo_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(2).Width = 35F;
            this.ssPatInfo_Sheet1.Columns.Get(3).CellType = textCellType12;
            this.ssPatInfo_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssPatInfo_Sheet1.Columns.Get(3).Label = "사업체명";
            this.ssPatInfo_Sheet1.Columns.Get(3).Locked = true;
            this.ssPatInfo_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(3).Width = 86F;
            this.ssPatInfo_Sheet1.Columns.Get(4).CellType = textCellType13;
            this.ssPatInfo_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(4).Label = "접수일자";
            this.ssPatInfo_Sheet1.Columns.Get(4).Locked = true;
            this.ssPatInfo_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(4).Width = 82F;
            this.ssPatInfo_Sheet1.Columns.Get(5).CellType = textCellType14;
            this.ssPatInfo_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssPatInfo_Sheet1.Columns.Get(5).Label = "검진종류";
            this.ssPatInfo_Sheet1.Columns.Get(5).Locked = true;
            this.ssPatInfo_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(5).Width = 88F;
            this.ssPatInfo_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPatInfo_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPatInfo_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.ssPatInfo_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPatInfo_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPatInfo_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPatInfo_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.ssPatInfo_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPatInfo_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
            this.ssPatInfo_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssPatInfo_Sheet1.RowHeader.Columns.Get(0).Width = 28F;
            this.ssPatInfo_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPatInfo_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPatInfo_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.ssPatInfo_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPatInfo_Sheet1.RowHeader.Visible = false;
            this.ssPatInfo_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPatInfo_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPatInfo_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.ssPatInfo_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPatInfo_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel12
            // 
            this.panel12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel12.Controls.Add(this.panel3);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel12.Location = new System.Drawing.Point(0, 96);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(633, 324);
            this.panel12.TabIndex = 26;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.SS_ETC);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(631, 322);
            this.panel3.TabIndex = 49;
            // 
            // SS_ETC
            // 
            this.SS_ETC.AccessibleDescription = "SS_ETC, Sheet1, Row 0, Column 0, ";
            this.SS_ETC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SS_ETC.FocusRenderer = enhancedFocusIndicatorRenderer2;
            this.SS_ETC.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SS_ETC.HorizontalScrollBar.Name = "";
            this.SS_ETC.HorizontalScrollBar.Renderer = enhancedScrollBarRenderer3;
            this.SS_ETC.HorizontalScrollBar.TabIndex = 3;
            this.SS_ETC.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.SS_ETC.Location = new System.Drawing.Point(0, 0);
            this.SS_ETC.Name = "SS_ETC";
            this.SS_ETC.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS_ETC_Sheet1});
            this.SS_ETC.Size = new System.Drawing.Size(631, 322);
            this.SS_ETC.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;
            this.SS_ETC.TabIndex = 48;
            this.SS_ETC.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SS_ETC.VerticalScrollBar.Name = "";
            this.SS_ETC.VerticalScrollBar.Renderer = enhancedScrollBarRenderer4;
            this.SS_ETC.VerticalScrollBar.TabIndex = 4;
            // 
            // SS_ETC_Sheet1
            // 
            this.SS_ETC_Sheet1.Reset();
            this.SS_ETC_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS_ETC_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS_ETC_Sheet1.ColumnCount = 5;
            this.SS_ETC_Sheet1.RowCount = 30;
            this.SS_ETC_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS_ETC_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS_ETC_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterEnhanced";
            this.SS_ETC_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS_ETC_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS_ETC_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS_ETC_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerEnhanced";
            this.SS_ETC_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS_ETC_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "D";
            this.SS_ETC_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "입력시간";
            this.SS_ETC_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "내용";
            this.SS_ETC_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "작업자";
            this.SS_ETC_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "RowId";
            this.SS_ETC_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS_ETC_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS_ETC_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderEnhanced";
            this.SS_ETC_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS_ETC_Sheet1.Columns.Get(0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.SS_ETC_Sheet1.Columns.Get(0).CellType = checkBoxCellType2;
            this.SS_ETC_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS_ETC_Sheet1.Columns.Get(0).Label = "D";
            this.SS_ETC_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS_ETC_Sheet1.Columns.Get(0).Width = 29F;
            this.SS_ETC_Sheet1.Columns.Get(1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.SS_ETC_Sheet1.Columns.Get(1).CellType = textCellType15;
            this.SS_ETC_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS_ETC_Sheet1.Columns.Get(1).Label = "입력시간";
            this.SS_ETC_Sheet1.Columns.Get(1).Locked = true;
            this.SS_ETC_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS_ETC_Sheet1.Columns.Get(1).Width = 173F;
            this.SS_ETC_Sheet1.Columns.Get(2).CellType = textCellType16;
            this.SS_ETC_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS_ETC_Sheet1.Columns.Get(2).Label = "내용";
            this.SS_ETC_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS_ETC_Sheet1.Columns.Get(2).Width = 300F;
            this.SS_ETC_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS_ETC_Sheet1.Columns.Get(3).Label = "작업자";
            this.SS_ETC_Sheet1.Columns.Get(3).Locked = true;
            this.SS_ETC_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS_ETC_Sheet1.Columns.Get(3).Width = 72F;
            this.SS_ETC_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS_ETC_Sheet1.Columns.Get(4).Label = "RowId";
            this.SS_ETC_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS_ETC_Sheet1.Columns.Get(4).Visible = false;
            this.SS_ETC_Sheet1.Columns.Get(4).Width = 72F;
            this.SS_ETC_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS_ETC_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS_ETC_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarEnhanced";
            this.SS_ETC_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS_ETC_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS_ETC_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS_ETC_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderEnhanced";
            this.SS_ETC_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS_ETC_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.SS_ETC_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS_ETC_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS_ETC_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS_ETC_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderEnhanced";
            this.SS_ETC_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS_ETC_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS_ETC_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS_ETC_Sheet1.SheetCornerStyle.Parent = "CornerEnhanced";
            this.SS_ETC_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS_ETC_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmHcActMemoSave
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 420);
            this.ControlBox = false;
            this.Controls.Add(this.panel12);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Name = "frmHcActMemoSave";
            this.Text = "frmHcActMemoSave";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssPatInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssPatInfo_Sheet1)).EndInit();
            this.panel12.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SS_ETC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS_ETC_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtWrtNo;
        private FarPoint.Win.Spread.FpSpread ssPatInfo;
        private FarPoint.Win.Spread.SheetView ssPatInfo_Sheet1;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel3;
        private FarPoint.Win.Spread.FpSpread SS_ETC;
        private FarPoint.Win.Spread.SheetView SS_ETC_Sheet1;
        private System.Windows.Forms.Button btnMemoSave;
    }
}