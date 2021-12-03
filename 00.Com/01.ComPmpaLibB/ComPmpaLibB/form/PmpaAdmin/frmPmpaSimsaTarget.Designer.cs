namespace ComPmpaLibB
{
    partial class frmPmpaSimsaTarget
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
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("ko-KR", false);
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType1 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType2 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType3 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            this.pnlHead = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.panList = new System.Windows.Forms.Panel();
            this.SSList = new FarPoint.Win.Spread.FpSpread();
            this.SSList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panMain = new System.Windows.Forms.Panel();
            this.panCtrl = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.panSet = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.SSSeq = new FarPoint.Win.Spread.FpSpread();
            this.SSSeq_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.SSDept = new FarPoint.Win.Spread.FpSpread();
            this.SSDept_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.SSWard = new FarPoint.Win.Spread.FpSpread();
            this.SSWard_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SSBi = new FarPoint.Win.Spread.FpSpread();
            this.SSBi_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panHead = new System.Windows.Forms.Panel();
            this.lblName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlHead.SuspendLayout();
            this.panList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).BeginInit();
            this.panMain.SuspendLayout();
            this.panCtrl.SuspendLayout();
            this.panSet.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSSeq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSSeq_Sheet1)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSDept)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSDept_Sheet1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSWard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSWard_Sheet1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSBi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSBi_Sheet1)).BeginInit();
            this.panHead.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHead
            // 
            this.pnlHead.BackColor = System.Drawing.Color.RoyalBlue;
            this.pnlHead.Controls.Add(this.label1);
            this.pnlHead.Controls.Add(this.label7);
            this.pnlHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHead.ForeColor = System.Drawing.Color.White;
            this.pnlHead.Location = new System.Drawing.Point(0, 0);
            this.pnlHead.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            this.pnlHead.Name = "pnlHead";
            this.pnlHead.Size = new System.Drawing.Size(578, 42);
            this.pnlHead.TabIndex = 181;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(14, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(133, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "심사자별 심사대상 세팅";
            // 
            // panList
            // 
            this.panList.Controls.Add(this.SSList);
            this.panList.Dock = System.Windows.Forms.DockStyle.Left;
            this.panList.Location = new System.Drawing.Point(0, 42);
            this.panList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panList.Name = "panList";
            this.panList.Padding = new System.Windows.Forms.Padding(3);
            this.panList.Size = new System.Drawing.Size(135, 521);
            this.panList.TabIndex = 182;
            // 
            // SSList
            // 
            this.SSList.AccessibleDescription = "SSList, Sheet1, Row 0, Column 0, 99999";
            this.SSList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.SSList.Location = new System.Drawing.Point(3, 3);
            this.SSList.Name = "SSList";
            this.SSList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSList_Sheet1});
            this.SSList.Size = new System.Drawing.Size(129, 515);
            this.SSList.TabIndex = 1;
            this.SSList.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.SSList.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSList_CellDoubleClick);
            // 
            // SSList_Sheet1
            // 
            this.SSList_Sheet1.Reset();
            this.SSList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSList_Sheet1.ColumnCount = 2;
            this.SSList_Sheet1.RowCount = 1;
            this.SSList_Sheet1.Cells.Get(0, 0).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SSList_Sheet1.Cells.Get(0, 0).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.SSList_Sheet1.Cells.Get(0, 0).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.SSList_Sheet1.Cells.Get(0, 0).ParseFormatString = "n";
            this.SSList_Sheet1.Cells.Get(0, 0).Value = 99999;
            this.SSList_Sheet1.Cells.Get(0, 1).Value = "김수한무거";
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "사번";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "이름";
            this.SSList_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(0).Label = "사번";
            this.SSList_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(0).Width = 52F;
            this.SSList_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(1).Label = "이름";
            this.SSList_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(1).Width = 73F;
            this.SSList_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.DefaultStyle.Locked = true;
            this.SSList_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.SSList_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SSList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSList_Sheet1.RowHeader.Visible = false;
            this.SSList_Sheet1.Rows.Get(0).Height = 24F;
            this.SSList_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panMain
            // 
            this.panMain.Controls.Add(this.panCtrl);
            this.panMain.Controls.Add(this.panSet);
            this.panMain.Controls.Add(this.panHead);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(135, 42);
            this.panMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panMain.Name = "panMain";
            this.panMain.Size = new System.Drawing.Size(443, 521);
            this.panMain.TabIndex = 183;
            // 
            // panCtrl
            // 
            this.panCtrl.Controls.Add(this.btnExit);
            this.panCtrl.Controls.Add(this.btnDelete);
            this.panCtrl.Controls.Add(this.btnSave);
            this.panCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panCtrl.Location = new System.Drawing.Point(0, 476);
            this.panCtrl.Name = "panCtrl";
            this.panCtrl.Size = new System.Drawing.Size(443, 45);
            this.panCtrl.TabIndex = 3;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(247, 6);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(114, 32);
            this.btnExit.TabIndex = 10;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(127, 6);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(114, 32);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Text = "삭제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(7, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(114, 32);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panSet
            // 
            this.panSet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSet.Controls.Add(this.groupBox4);
            this.panSet.Controls.Add(this.groupBox3);
            this.panSet.Controls.Add(this.groupBox2);
            this.panSet.Controls.Add(this.groupBox1);
            this.panSet.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSet.Location = new System.Drawing.Point(0, 44);
            this.panSet.Name = "panSet";
            this.panSet.Padding = new System.Windows.Forms.Padding(3);
            this.panSet.Size = new System.Drawing.Size(443, 432);
            this.panSet.TabIndex = 2;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnAdd);
            this.groupBox4.Controls.Add(this.SSSeq);
            this.groupBox4.Location = new System.Drawing.Point(5, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(66, 174);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "세팅순번";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(1, 146);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(65, 28);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "신규추가";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // SSSeq
            // 
            this.SSSeq.AccessibleDescription = "";
            this.SSSeq.Dock = System.Windows.Forms.DockStyle.Top;
            this.SSSeq.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.SSSeq.Location = new System.Drawing.Point(3, 19);
            this.SSSeq.Name = "SSSeq";
            this.SSSeq.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSSeq_Sheet1});
            this.SSSeq.Size = new System.Drawing.Size(60, 124);
            this.SSSeq.TabIndex = 2;
            this.SSSeq.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SSSeq.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSSeq_CellDoubleClick);
            // 
            // SSSeq_Sheet1
            // 
            this.SSSeq_Sheet1.Reset();
            this.SSSeq_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSSeq_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSSeq_Sheet1.ColumnCount = 1;
            this.SSSeq_Sheet1.RowCount = 1;
            this.SSSeq_Sheet1.Cells.Get(0, 0).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SSSeq_Sheet1.Cells.Get(0, 0).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.SSSeq_Sheet1.Cells.Get(0, 0).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.SSSeq_Sheet1.Cells.Get(0, 0).ParseFormatString = "n";
            this.SSSeq_Sheet1.Cells.Get(0, 0).Value = 11;
            this.SSSeq_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSSeq_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSSeq_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSSeq_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSSeq_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "순번";
            this.SSSeq_Sheet1.Columns.Get(0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.SSSeq_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSSeq_Sheet1.Columns.Get(0).Label = "순번";
            this.SSSeq_Sheet1.Columns.Get(0).Locked = true;
            this.SSSeq_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSSeq_Sheet1.Columns.Get(0).Width = 39F;
            this.SSSeq_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSSeq_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSSeq_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSSeq_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSSeq_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SSSeq_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSSeq_Sheet1.RowHeader.Visible = false;
            this.SSSeq_Sheet1.Rows.Get(0).Height = 24F;
            this.SSSeq_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SSSeq_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.SSDept);
            this.groupBox3.Location = new System.Drawing.Point(332, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(100, 418);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "진료과";
            // 
            // SSDept
            // 
            this.SSDept.AccessibleDescription = "";
            this.SSDept.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSDept.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.SSDept.Location = new System.Drawing.Point(3, 19);
            this.SSDept.Name = "SSDept";
            this.SSDept.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSDept_Sheet1});
            this.SSDept.Size = new System.Drawing.Size(94, 396);
            this.SSDept.TabIndex = 1;
            // 
            // SSDept_Sheet1
            // 
            this.SSDept_Sheet1.Reset();
            this.SSDept_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSDept_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSDept_Sheet1.ColumnCount = 2;
            this.SSDept_Sheet1.RowCount = 1;
            this.SSDept_Sheet1.Cells.Get(0, 0).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SSDept_Sheet1.Cells.Get(0, 0).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.SSDept_Sheet1.Cells.Get(0, 0).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.SSDept_Sheet1.Cells.Get(0, 0).ParseFormatString = "n";
            this.SSDept_Sheet1.Cells.Get(0, 0).Value = 11;
            this.SSDept_Sheet1.Cells.Get(0, 1).Value = false;
            this.SSDept_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSDept_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSDept_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSDept_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSDept_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "구분";
            this.SSDept_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "선택";
            this.SSDept_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSDept_Sheet1.Columns.Get(0).Label = "구분";
            this.SSDept_Sheet1.Columns.Get(0).Locked = true;
            this.SSDept_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSDept_Sheet1.Columns.Get(0).Width = 39F;
            this.SSDept_Sheet1.Columns.Get(1).CellType = checkBoxCellType1;
            this.SSDept_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSDept_Sheet1.Columns.Get(1).Label = "선택";
            this.SSDept_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSDept_Sheet1.Columns.Get(1).Width = 33F;
            this.SSDept_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSDept_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSDept_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSDept_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSDept_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SSDept_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSDept_Sheet1.RowHeader.Visible = false;
            this.SSDept_Sheet1.Rows.Get(0).Height = 24F;
            this.SSDept_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SSDept_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.SSWard);
            this.groupBox2.Location = new System.Drawing.Point(183, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(143, 418);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "병동";
            // 
            // SSWard
            // 
            this.SSWard.AccessibleDescription = "";
            this.SSWard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSWard.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.SSWard.Location = new System.Drawing.Point(3, 19);
            this.SSWard.Name = "SSWard";
            this.SSWard.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSWard_Sheet1});
            this.SSWard.Size = new System.Drawing.Size(137, 396);
            this.SSWard.TabIndex = 1;
            // 
            // SSWard_Sheet1
            // 
            this.SSWard_Sheet1.Reset();
            this.SSWard_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSWard_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSWard_Sheet1.ColumnCount = 3;
            this.SSWard_Sheet1.RowCount = 1;
            this.SSWard_Sheet1.Cells.Get(0, 0).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SSWard_Sheet1.Cells.Get(0, 0).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.SSWard_Sheet1.Cells.Get(0, 0).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.SSWard_Sheet1.Cells.Get(0, 0).ParseFormatString = "n";
            this.SSWard_Sheet1.Cells.Get(0, 0).Value = 11;
            this.SSWard_Sheet1.Cells.Get(0, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SSWard_Sheet1.Cells.Get(0, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.SSWard_Sheet1.Cells.Get(0, 1).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.SSWard_Sheet1.Cells.Get(0, 1).ParseFormatString = "n";
            this.SSWard_Sheet1.Cells.Get(0, 1).Value = 703;
            this.SSWard_Sheet1.Cells.Get(0, 2).Value = false;
            this.SSWard_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSWard_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSWard_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSWard_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSWard_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "병동";
            this.SSWard_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "호실";
            this.SSWard_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "선택";
            this.SSWard_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSWard_Sheet1.Columns.Get(0).Label = "병동";
            this.SSWard_Sheet1.Columns.Get(0).Locked = true;
            this.SSWard_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSWard_Sheet1.Columns.Get(0).Width = 39F;
            this.SSWard_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSWard_Sheet1.Columns.Get(1).Label = "호실";
            this.SSWard_Sheet1.Columns.Get(1).Locked = true;
            this.SSWard_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSWard_Sheet1.Columns.Get(1).Width = 43F;
            this.SSWard_Sheet1.Columns.Get(2).CellType = checkBoxCellType2;
            this.SSWard_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSWard_Sheet1.Columns.Get(2).Label = "선택";
            this.SSWard_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSWard_Sheet1.Columns.Get(2).Width = 33F;
            this.SSWard_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSWard_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSWard_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSWard_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSWard_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SSWard_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSWard_Sheet1.RowHeader.Visible = false;
            this.SSWard_Sheet1.Rows.Get(0).Height = 24F;
            this.SSWard_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SSWard_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SSBi);
            this.groupBox1.Location = new System.Drawing.Point(77, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(100, 418);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "환자자격";
            // 
            // SSBi
            // 
            this.SSBi.AccessibleDescription = "";
            this.SSBi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSBi.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.SSBi.Location = new System.Drawing.Point(3, 19);
            this.SSBi.Name = "SSBi";
            this.SSBi.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSBi_Sheet1});
            this.SSBi.Size = new System.Drawing.Size(94, 396);
            this.SSBi.TabIndex = 1;
            // 
            // SSBi_Sheet1
            // 
            this.SSBi_Sheet1.Reset();
            this.SSBi_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSBi_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSBi_Sheet1.ColumnCount = 2;
            this.SSBi_Sheet1.RowCount = 1;
            this.SSBi_Sheet1.Cells.Get(0, 0).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SSBi_Sheet1.Cells.Get(0, 0).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.SSBi_Sheet1.Cells.Get(0, 0).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.SSBi_Sheet1.Cells.Get(0, 0).ParseFormatString = "n";
            this.SSBi_Sheet1.Cells.Get(0, 0).Value = 11;
            this.SSBi_Sheet1.Cells.Get(0, 1).Value = false;
            this.SSBi_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSBi_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSBi_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSBi_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSBi_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "구분";
            this.SSBi_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "선택";
            this.SSBi_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSBi_Sheet1.Columns.Get(0).Label = "구분";
            this.SSBi_Sheet1.Columns.Get(0).Locked = true;
            this.SSBi_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSBi_Sheet1.Columns.Get(0).Width = 39F;
            this.SSBi_Sheet1.Columns.Get(1).CellType = checkBoxCellType3;
            this.SSBi_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSBi_Sheet1.Columns.Get(1).Label = "선택";
            this.SSBi_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSBi_Sheet1.Columns.Get(1).Width = 33F;
            this.SSBi_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSBi_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSBi_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSBi_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSBi_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SSBi_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSBi_Sheet1.RowHeader.Visible = false;
            this.SSBi_Sheet1.Rows.Get(0).Height = 24F;
            this.SSBi_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SSBi_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panHead
            // 
            this.panHead.BackColor = System.Drawing.Color.Bisque;
            this.panHead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panHead.Controls.Add(this.lblName);
            this.panHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHead.Location = new System.Drawing.Point(0, 0);
            this.panHead.Name = "panHead";
            this.panHead.Size = new System.Drawing.Size(443, 44);
            this.panHead.TabIndex = 0;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblName.Location = new System.Drawing.Point(18, 13);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(187, 17);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "대상자 : 김수한무거  1번 세팅";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(349, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(219, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "셋팅순번은 0부터시작되어야합니다.";
            // 
            // frmPmpaSimsaTarget
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 563);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.panList);
            this.Controls.Add(this.pnlHead);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmPmpaSimsaTarget";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmPmpaSimsaTarget_Load);
            this.pnlHead.ResumeLayout(false);
            this.pnlHead.PerformLayout();
            this.panList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).EndInit();
            this.panMain.ResumeLayout(false);
            this.panCtrl.ResumeLayout(false);
            this.panSet.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSSeq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSSeq_Sheet1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSDept)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSDept_Sheet1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSWard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSWard_Sheet1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSBi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSBi_Sheet1)).EndInit();
            this.panHead.ResumeLayout(false);
            this.panHead.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHead;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panList;
        private FarPoint.Win.Spread.FpSpread SSList;
        private FarPoint.Win.Spread.SheetView SSList_Sheet1;
        private System.Windows.Forms.Panel panMain;
        private System.Windows.Forms.Panel panHead;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Panel panSet;
        private System.Windows.Forms.GroupBox groupBox3;
        private FarPoint.Win.Spread.FpSpread SSDept;
        private FarPoint.Win.Spread.SheetView SSDept_Sheet1;
        private System.Windows.Forms.GroupBox groupBox2;
        private FarPoint.Win.Spread.FpSpread SSWard;
        private FarPoint.Win.Spread.SheetView SSWard_Sheet1;
        private System.Windows.Forms.GroupBox groupBox1;
        private FarPoint.Win.Spread.FpSpread SSBi;
        private FarPoint.Win.Spread.SheetView SSBi_Sheet1;
        private System.Windows.Forms.Panel panCtrl;
        public System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnAdd;
        private FarPoint.Win.Spread.FpSpread SSSeq;
        private FarPoint.Win.Spread.SheetView SSSeq_Sheet1;
        private System.Windows.Forms.Label label1;
    }
}