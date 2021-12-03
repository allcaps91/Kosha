namespace ComEmrBase
{
    partial class frmEmrBaseUserChoRegOld
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ssFORM = new FarPoint.Win.Spread.FpSpread();
            this.ssFORM_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitleSub1 = new System.Windows.Forms.Panel();
            this.lblTitleSub1 = new System.Windows.Forms.Label();
            this.ssGRPFORM = new FarPoint.Win.Spread.FpSpread();
            this.ssGRPFORM_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ssUSERFORM = new FarPoint.Win.Spread.FpSpread();
            this.ssUSERFORM_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitleSub2 = new System.Windows.Forms.Panel();
            this.lblTitleSub2 = new System.Windows.Forms.Label();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssFORM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssFORM_Sheet1)).BeginInit();
            this.panTitleSub1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssGRPFORM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssGRPFORM_Sheet1)).BeginInit();
            this.panTitleSub0.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssUSERFORM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssUSERFORM_Sheet1)).BeginInit();
            this.panTitleSub2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Controls.Add(this.btnSearch);
            this.panTitle.Controls.Add(this.btnSave);
            this.panTitle.Controls.Add(this.btnDelete);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(919, 33);
            this.panTitle.TabIndex = 14;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(10, 4);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(134, 21);
            this.lblTitle.TabIndex = 34;
            this.lblTitle.Text = "상용 기록지 등록";
            // 
            // btnSearch
            // 
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Location = new System.Drawing.Point(635, 0);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(70, 29);
            this.btnSearch.TabIndex = 33;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(705, 0);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(70, 29);
            this.btnSave.TabIndex = 32;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDelete.Location = new System.Drawing.Point(775, 0);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(70, 29);
            this.btnDelete.TabIndex = 31;
            this.btnDelete.Text = "삭제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(845, 0);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(70, 29);
            this.btnExit.TabIndex = 30;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ssFORM);
            this.panel1.Controls.Add(this.panTitleSub1);
            this.panel1.Controls.Add(this.ssGRPFORM);
            this.panel1.Controls.Add(this.panTitleSub0);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 33);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(330, 630);
            this.panel1.TabIndex = 15;
            // 
            // ssFORM
            // 
            this.ssFORM.AccessibleDescription = "ssFORM, Sheet1, Row 0, Column 0, 9999-99-99";
            this.ssFORM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssFORM.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssFORM.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssFORM.Location = new System.Drawing.Point(0, 318);
            this.ssFORM.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ssFORM.Name = "ssFORM";
            this.ssFORM.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssFORM_Sheet1});
            this.ssFORM.Size = new System.Drawing.Size(330, 312);
            this.ssFORM.TabIndex = 53;
            this.ssFORM.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssFORM_CellClick);
            this.ssFORM.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssFORM_CellDoubleClick);
            // 
            // ssFORM_Sheet1
            // 
            this.ssFORM_Sheet1.Reset();
            this.ssFORM_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssFORM_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssFORM_Sheet1.ColumnCount = 4;
            this.ssFORM_Sheet1.RowCount = 1;
            this.ssFORM_Sheet1.Cells.Get(0, 0).Value = "9999-99-99";
            this.ssFORM_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssFORM_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssFORM_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssFORM_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssFORM_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "기록지명";
            this.ssFORM_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "기록지 그룹명";
            this.ssFORM_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "FORMNO";
            this.ssFORM_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "GRPFORMNO";
            this.ssFORM_Sheet1.ColumnHeader.Rows.Get(0).Height = 28F;
            this.ssFORM_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssFORM_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssFORM_Sheet1.Columns.Get(0).Label = "기록지명";
            this.ssFORM_Sheet1.Columns.Get(0).Locked = true;
            this.ssFORM_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssFORM_Sheet1.Columns.Get(0).Width = 308F;
            this.ssFORM_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssFORM_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssFORM_Sheet1.Columns.Get(1).Label = "기록지 그룹명";
            this.ssFORM_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssFORM_Sheet1.Columns.Get(1).Visible = false;
            this.ssFORM_Sheet1.Columns.Get(1).Width = 63F;
            this.ssFORM_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssFORM_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssFORM_Sheet1.Columns.Get(2).Label = "FORMNO";
            this.ssFORM_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssFORM_Sheet1.Columns.Get(2).Visible = false;
            this.ssFORM_Sheet1.Columns.Get(2).Width = 63F;
            this.ssFORM_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssFORM_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssFORM_Sheet1.Columns.Get(3).Label = "GRPFORMNO";
            this.ssFORM_Sheet1.Columns.Get(3).Locked = true;
            this.ssFORM_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssFORM_Sheet1.Columns.Get(3).Visible = false;
            this.ssFORM_Sheet1.Columns.Get(3).Width = 63F;
            this.ssFORM_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssFORM_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssFORM_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssFORM_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssFORM_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssFORM_Sheet1.RowHeader.Visible = false;
            this.ssFORM_Sheet1.Rows.Get(0).Height = 24F;
            this.ssFORM_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panTitleSub1
            // 
            this.panTitleSub1.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub1.Controls.Add(this.lblTitleSub1);
            this.panTitleSub1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub1.Location = new System.Drawing.Point(0, 290);
            this.panTitleSub1.Name = "panTitleSub1";
            this.panTitleSub1.Size = new System.Drawing.Size(330, 28);
            this.panTitleSub1.TabIndex = 52;
            // 
            // lblTitleSub1
            // 
            this.lblTitleSub1.AutoSize = true;
            this.lblTitleSub1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub1.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub1.Location = new System.Drawing.Point(8, 4);
            this.lblTitleSub1.Name = "lblTitleSub1";
            this.lblTitleSub1.Size = new System.Drawing.Size(43, 15);
            this.lblTitleSub1.TabIndex = 0;
            this.lblTitleSub1.Text = "기록지";
            // 
            // ssGRPFORM
            // 
            this.ssGRPFORM.AccessibleDescription = "ssViewEmrAcpDept, Sheet1, Row 0, Column 0, 9999-99-99";
            this.ssGRPFORM.Dock = System.Windows.Forms.DockStyle.Top;
            this.ssGRPFORM.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssGRPFORM.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssGRPFORM.Location = new System.Drawing.Point(0, 28);
            this.ssGRPFORM.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ssGRPFORM.Name = "ssGRPFORM";
            this.ssGRPFORM.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssGRPFORM_Sheet1});
            this.ssGRPFORM.Size = new System.Drawing.Size(330, 262);
            this.ssGRPFORM.TabIndex = 51;
            this.ssGRPFORM.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssGRPFORM_CellClick);
            // 
            // ssGRPFORM_Sheet1
            // 
            this.ssGRPFORM_Sheet1.Reset();
            this.ssGRPFORM_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssGRPFORM_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssGRPFORM_Sheet1.ColumnCount = 2;
            this.ssGRPFORM_Sheet1.RowCount = 1;
            this.ssGRPFORM_Sheet1.Cells.Get(0, 0).Value = "9999-99-99";
            this.ssGRPFORM_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssGRPFORM_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssGRPFORM_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssGRPFORM_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssGRPFORM_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "기록지 그룹명";
            this.ssGRPFORM_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "GRPFORMNO";
            this.ssGRPFORM_Sheet1.ColumnHeader.Rows.Get(0).Height = 28F;
            this.ssGRPFORM_Sheet1.Columns.Get(0).CellType = textCellType5;
            this.ssGRPFORM_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssGRPFORM_Sheet1.Columns.Get(0).Label = "기록지 그룹명";
            this.ssGRPFORM_Sheet1.Columns.Get(0).Locked = true;
            this.ssGRPFORM_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssGRPFORM_Sheet1.Columns.Get(0).Width = 308F;
            this.ssGRPFORM_Sheet1.Columns.Get(1).CellType = textCellType6;
            this.ssGRPFORM_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssGRPFORM_Sheet1.Columns.Get(1).Label = "GRPFORMNO";
            this.ssGRPFORM_Sheet1.Columns.Get(1).Locked = true;
            this.ssGRPFORM_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssGRPFORM_Sheet1.Columns.Get(1).Visible = false;
            this.ssGRPFORM_Sheet1.Columns.Get(1).Width = 63F;
            this.ssGRPFORM_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssGRPFORM_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssGRPFORM_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssGRPFORM_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssGRPFORM_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssGRPFORM_Sheet1.RowHeader.Visible = false;
            this.ssGRPFORM_Sheet1.Rows.Get(0).Height = 24F;
            this.ssGRPFORM_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 0);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(330, 28);
            this.panTitleSub0.TabIndex = 15;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 4);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(71, 15);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "기록지 그룹";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ssUSERFORM);
            this.panel2.Controls.Add(this.panTitleSub2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(330, 33);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(589, 630);
            this.panel2.TabIndex = 16;
            // 
            // ssUSERFORM
            // 
            this.ssUSERFORM.AccessibleDescription = "ssUSERFORM, Sheet1, Row 0, Column 0, 9999-99-99";
            this.ssUSERFORM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssUSERFORM.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssUSERFORM.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssUSERFORM.Location = new System.Drawing.Point(0, 28);
            this.ssUSERFORM.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ssUSERFORM.Name = "ssUSERFORM";
            this.ssUSERFORM.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssUSERFORM_Sheet1});
            this.ssUSERFORM.Size = new System.Drawing.Size(589, 602);
            this.ssUSERFORM.TabIndex = 51;
            this.ssUSERFORM.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssUSERFORM_CellClick);
            // 
            // ssUSERFORM_Sheet1
            // 
            this.ssUSERFORM_Sheet1.Reset();
            this.ssUSERFORM_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssUSERFORM_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssUSERFORM_Sheet1.ColumnCount = 6;
            this.ssUSERFORM_Sheet1.RowCount = 1;
            this.ssUSERFORM_Sheet1.Cells.Get(0, 0).Value = "9999-99-99";
            this.ssUSERFORM_Sheet1.Cells.Get(0, 2).Value = "IM";
            this.ssUSERFORM_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssUSERFORM_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssUSERFORM_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssUSERFORM_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssUSERFORM_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssUSERFORM_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssUSERFORM_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssUSERFORM_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssUSERFORM_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "기록지 그룹명";
            this.ssUSERFORM_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "기록지명";
            this.ssUSERFORM_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "순서";
            this.ssUSERFORM_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "FORMNO";
            this.ssUSERFORM_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "GRPFORMNO";
            this.ssUSERFORM_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "New";
            this.ssUSERFORM_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssUSERFORM_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssUSERFORM_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssUSERFORM_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssUSERFORM_Sheet1.ColumnHeader.Rows.Get(0).Height = 28F;
            this.ssUSERFORM_Sheet1.Columns.Get(0).CellType = textCellType7;
            this.ssUSERFORM_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssUSERFORM_Sheet1.Columns.Get(0).Label = "기록지 그룹명";
            this.ssUSERFORM_Sheet1.Columns.Get(0).Locked = true;
            this.ssUSERFORM_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssUSERFORM_Sheet1.Columns.Get(0).Width = 219F;
            this.ssUSERFORM_Sheet1.Columns.Get(1).CellType = textCellType8;
            this.ssUSERFORM_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssUSERFORM_Sheet1.Columns.Get(1).Label = "기록지명";
            this.ssUSERFORM_Sheet1.Columns.Get(1).Locked = true;
            this.ssUSERFORM_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssUSERFORM_Sheet1.Columns.Get(1).Width = 309F;
            this.ssUSERFORM_Sheet1.Columns.Get(2).CellType = textCellType9;
            this.ssUSERFORM_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssUSERFORM_Sheet1.Columns.Get(2).Label = "순서";
            this.ssUSERFORM_Sheet1.Columns.Get(2).Locked = false;
            this.ssUSERFORM_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssUSERFORM_Sheet1.Columns.Get(2).Width = 40F;
            this.ssUSERFORM_Sheet1.Columns.Get(3).CellType = textCellType10;
            this.ssUSERFORM_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssUSERFORM_Sheet1.Columns.Get(3).Label = "FORMNO";
            this.ssUSERFORM_Sheet1.Columns.Get(3).Locked = true;
            this.ssUSERFORM_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssUSERFORM_Sheet1.Columns.Get(3).Visible = false;
            this.ssUSERFORM_Sheet1.Columns.Get(3).Width = 65F;
            this.ssUSERFORM_Sheet1.Columns.Get(4).CellType = textCellType11;
            this.ssUSERFORM_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssUSERFORM_Sheet1.Columns.Get(4).Label = "GRPFORMNO";
            this.ssUSERFORM_Sheet1.Columns.Get(4).Locked = false;
            this.ssUSERFORM_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssUSERFORM_Sheet1.Columns.Get(4).Visible = false;
            this.ssUSERFORM_Sheet1.Columns.Get(4).Width = 103F;
            this.ssUSERFORM_Sheet1.Columns.Get(5).CellType = textCellType12;
            this.ssUSERFORM_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssUSERFORM_Sheet1.Columns.Get(5).Label = "New";
            this.ssUSERFORM_Sheet1.Columns.Get(5).Locked = false;
            this.ssUSERFORM_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssUSERFORM_Sheet1.Columns.Get(5).Visible = false;
            this.ssUSERFORM_Sheet1.Columns.Get(5).Width = 119F;
            this.ssUSERFORM_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssUSERFORM_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssUSERFORM_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.ssUSERFORM_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssUSERFORM_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssUSERFORM_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssUSERFORM_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.ssUSERFORM_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssUSERFORM_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssUSERFORM_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssUSERFORM_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssUSERFORM_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssUSERFORM_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssUSERFORM_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssUSERFORM_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssUSERFORM_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssUSERFORM_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssUSERFORM_Sheet1.RowHeader.Visible = false;
            this.ssUSERFORM_Sheet1.Rows.Get(0).Height = 24F;
            this.ssUSERFORM_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssUSERFORM_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssUSERFORM_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssUSERFORM_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssUSERFORM_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panTitleSub2
            // 
            this.panTitleSub2.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub2.Controls.Add(this.lblTitleSub2);
            this.panTitleSub2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub2.Location = new System.Drawing.Point(0, 0);
            this.panTitleSub2.Name = "panTitleSub2";
            this.panTitleSub2.Size = new System.Drawing.Size(589, 28);
            this.panTitleSub2.TabIndex = 15;
            // 
            // lblTitleSub2
            // 
            this.lblTitleSub2.AutoSize = true;
            this.lblTitleSub2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub2.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub2.Location = new System.Drawing.Point(8, 4);
            this.lblTitleSub2.Name = "lblTitleSub2";
            this.lblTitleSub2.Size = new System.Drawing.Size(71, 15);
            this.lblTitleSub2.TabIndex = 0;
            this.lblTitleSub2.Text = "상용 기록지";
            // 
            // frmEmrBaseUserChoRegOld
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(919, 663);
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmEmrBaseUserChoRegOld";
            this.Text = "frmEmrBaseUserChoRegOld";
            this.Load += new System.EventHandler(this.frmEmrBaseUserChoRegOld_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssFORM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssFORM_Sheet1)).EndInit();
            this.panTitleSub1.ResumeLayout(false);
            this.panTitleSub1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssGRPFORM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssGRPFORM_Sheet1)).EndInit();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssUSERFORM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssUSERFORM_Sheet1)).EndInit();
            this.panTitleSub2.ResumeLayout(false);
            this.panTitleSub2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panTitleSub2;
        private System.Windows.Forms.Label lblTitleSub2;
        private System.Windows.Forms.Panel panTitleSub1;
        private System.Windows.Forms.Label lblTitleSub1;
        private FarPoint.Win.Spread.FpSpread ssGRPFORM;
        private FarPoint.Win.Spread.SheetView ssGRPFORM_Sheet1;
        private FarPoint.Win.Spread.FpSpread ssFORM;
        private FarPoint.Win.Spread.SheetView ssFORM_Sheet1;
        private FarPoint.Win.Spread.FpSpread ssUSERFORM;
        private FarPoint.Win.Spread.SheetView ssUSERFORM_Sheet1;
    }
}