namespace ComLibB
{
    partial class frmBcodeMng
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
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType1 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType1 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType2 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType3 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType4 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType5 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.cboCode = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.ssGichoCode = new FarPoint.Win.Spread.FpSpread();
            this.ssGichoCode_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel2.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssGichoCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssGichoCode_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panel2.Controls.Add(this.lblTitleSub0);
            this.panel2.Controls.Add(this.cboCode);
            this.panel2.Controls.Add(this.btnAdd);
            this.panel2.Controls.Add(this.btnSearch);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.btnDelete);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 34);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1261, 28);
            this.panel2.TabIndex = 85;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(9, 9);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(88, 12);
            this.lblTitleSub0.TabIndex = 23;
            this.lblTitleSub0.Text = "기초코드 항목";
            this.lblTitleSub0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboCode
            // 
            this.cboCode.FormattingEnabled = true;
            this.cboCode.Location = new System.Drawing.Point(102, 4);
            this.cboCode.Name = "cboCode";
            this.cboCode.Size = new System.Drawing.Size(285, 20);
            this.cboCode.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAdd.ForeColor = System.Drawing.Color.Black;
            this.btnAdd.Location = new System.Drawing.Point(1113, 1);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(72, 26);
            this.btnAdd.TabIndex = 16;
            this.btnAdd.Text = "추 가";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.Location = new System.Drawing.Point(969, 1);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 26);
            this.btnSearch.TabIndex = 14;
            this.btnSearch.Text = "조 회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Location = new System.Drawing.Point(1041, 1);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 26);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "저 장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Location = new System.Drawing.Point(1185, 1);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(72, 26);
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Text = "삭 제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1261, 34);
            this.panTitle.TabIndex = 84;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(1184, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "닫 기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(9, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(181, 16);
            this.lblTitle.TabIndex = 15;
            this.lblTitle.Text = "기초코드 관리(관리자)";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 653);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1261, 30);
            this.panel1.TabIndex = 86;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(6, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(556, 16);
            this.label1.TabIndex = 16;
            this.label1.Text = "1. 수술실 업무 자료사전 등록금지( OPR_CODE : 수술실기코드 사용중)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ssGichoCode
            // 
            this.ssGichoCode.AccessibleDescription = "ssGichoCode, Sheet1, Row 0, Column 0, ";
            this.ssGichoCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssGichoCode.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssGichoCode.Location = new System.Drawing.Point(0, 62);
            this.ssGichoCode.Name = "ssGichoCode";
            this.ssGichoCode.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssGichoCode_Sheet1});
            this.ssGichoCode.Size = new System.Drawing.Size(1261, 591);
            this.ssGichoCode.TabIndex = 88;
            this.ssGichoCode.EditChange += new FarPoint.Win.Spread.EditorNotifyEventHandler(this.ssGichoCode_EditChange);
            // 
            // ssGichoCode_Sheet1
            // 
            this.ssGichoCode_Sheet1.Reset();
            this.ssGichoCode_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssGichoCode_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssGichoCode_Sheet1.ColumnCount = 17;
            this.ssGichoCode_Sheet1.RowCount = 1;
            this.ssGichoCode_Sheet1.Cells.Get(0, 13).Value = "9999-99-99";
            this.ssGichoCode_Sheet1.Cells.Get(0, 14).Value = "9999-99-99";
            this.ssGichoCode_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssGichoCode_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssGichoCode_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssGichoCode_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "삭제";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "코드";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "내용";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "GUBUN2";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "GUBUN3";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "GUBUN4";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "GUBUN5";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "GUNUM1";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "GUNUM2";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "GUNUM3";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "PART";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "CNT";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "SORT";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "적용일자";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "삭제일자";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 15).Value = "ROWID";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 16).Value = "변경여부";
            this.ssGichoCode_Sheet1.ColumnHeader.Rows.Get(0).Height = 24F;
            this.ssGichoCode_Sheet1.Columns.Get(0).CellType = checkBoxCellType1;
            this.ssGichoCode_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssGichoCode_Sheet1.Columns.Get(0).Label = "삭제";
            this.ssGichoCode_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssGichoCode_Sheet1.Columns.Get(0).Width = 32F;
            this.ssGichoCode_Sheet1.Columns.Get(1).CellType = textCellType1;
            this.ssGichoCode_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssGichoCode_Sheet1.Columns.Get(1).Label = "코드";
            this.ssGichoCode_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssGichoCode_Sheet1.Columns.Get(1).Width = 184F;
            this.ssGichoCode_Sheet1.Columns.Get(2).CellType = textCellType2;
            this.ssGichoCode_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssGichoCode_Sheet1.Columns.Get(2).Label = "내용";
            this.ssGichoCode_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssGichoCode_Sheet1.Columns.Get(2).Width = 240F;
            this.ssGichoCode_Sheet1.Columns.Get(3).CellType = textCellType3;
            this.ssGichoCode_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssGichoCode_Sheet1.Columns.Get(3).Label = "GUBUN2";
            this.ssGichoCode_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssGichoCode_Sheet1.Columns.Get(3).Width = 65F;
            this.ssGichoCode_Sheet1.Columns.Get(4).CellType = textCellType4;
            this.ssGichoCode_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssGichoCode_Sheet1.Columns.Get(4).Label = "GUBUN3";
            this.ssGichoCode_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssGichoCode_Sheet1.Columns.Get(4).Width = 65F;
            this.ssGichoCode_Sheet1.Columns.Get(5).CellType = textCellType5;
            this.ssGichoCode_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssGichoCode_Sheet1.Columns.Get(5).Label = "GUBUN4";
            this.ssGichoCode_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssGichoCode_Sheet1.Columns.Get(5).Width = 65F;
            this.ssGichoCode_Sheet1.Columns.Get(6).CellType = textCellType6;
            this.ssGichoCode_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssGichoCode_Sheet1.Columns.Get(6).Label = "GUBUN5";
            this.ssGichoCode_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssGichoCode_Sheet1.Columns.Get(6).Width = 65F;
            this.ssGichoCode_Sheet1.Columns.Get(7).CellType = numberCellType1;
            this.ssGichoCode_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssGichoCode_Sheet1.Columns.Get(7).Label = "GUNUM1";
            this.ssGichoCode_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssGichoCode_Sheet1.Columns.Get(7).Width = 62F;
            this.ssGichoCode_Sheet1.Columns.Get(8).CellType = numberCellType2;
            this.ssGichoCode_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssGichoCode_Sheet1.Columns.Get(8).Label = "GUNUM2";
            this.ssGichoCode_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssGichoCode_Sheet1.Columns.Get(8).Width = 62F;
            this.ssGichoCode_Sheet1.Columns.Get(9).CellType = numberCellType3;
            this.ssGichoCode_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssGichoCode_Sheet1.Columns.Get(9).Label = "GUNUM3";
            this.ssGichoCode_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssGichoCode_Sheet1.Columns.Get(9).Width = 62F;
            this.ssGichoCode_Sheet1.Columns.Get(10).CellType = textCellType7;
            this.ssGichoCode_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssGichoCode_Sheet1.Columns.Get(10).Label = "PART";
            this.ssGichoCode_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssGichoCode_Sheet1.Columns.Get(10).Width = 63F;
            numberCellType4.MaximumValue = 9999999D;
            numberCellType4.MinimumValue = -9999999D;
            this.ssGichoCode_Sheet1.Columns.Get(11).CellType = numberCellType4;
            this.ssGichoCode_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssGichoCode_Sheet1.Columns.Get(11).Label = "CNT";
            this.ssGichoCode_Sheet1.Columns.Get(11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssGichoCode_Sheet1.Columns.Get(11).Width = 44F;
            numberCellType5.MaximumValue = 9999999D;
            numberCellType5.MinimumValue = -9999999D;
            this.ssGichoCode_Sheet1.Columns.Get(12).CellType = numberCellType5;
            this.ssGichoCode_Sheet1.Columns.Get(12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssGichoCode_Sheet1.Columns.Get(12).Label = "SORT";
            this.ssGichoCode_Sheet1.Columns.Get(12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssGichoCode_Sheet1.Columns.Get(12).Width = 46F;
            this.ssGichoCode_Sheet1.Columns.Get(13).CellType = textCellType8;
            this.ssGichoCode_Sheet1.Columns.Get(13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssGichoCode_Sheet1.Columns.Get(13).Label = "적용일자";
            this.ssGichoCode_Sheet1.Columns.Get(13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssGichoCode_Sheet1.Columns.Get(13).Width = 75F;
            this.ssGichoCode_Sheet1.Columns.Get(14).CellType = textCellType9;
            this.ssGichoCode_Sheet1.Columns.Get(14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssGichoCode_Sheet1.Columns.Get(14).Label = "삭제일자";
            this.ssGichoCode_Sheet1.Columns.Get(14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssGichoCode_Sheet1.Columns.Get(14).Width = 75F;
            this.ssGichoCode_Sheet1.Columns.Get(15).CellType = textCellType10;
            this.ssGichoCode_Sheet1.Columns.Get(15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssGichoCode_Sheet1.Columns.Get(15).Label = "ROWID";
            this.ssGichoCode_Sheet1.Columns.Get(15).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssGichoCode_Sheet1.Columns.Get(15).Visible = false;
            this.ssGichoCode_Sheet1.Columns.Get(15).Width = 91F;
            this.ssGichoCode_Sheet1.Columns.Get(16).CellType = textCellType11;
            this.ssGichoCode_Sheet1.Columns.Get(16).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssGichoCode_Sheet1.Columns.Get(16).Label = "변경여부";
            this.ssGichoCode_Sheet1.Columns.Get(16).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssGichoCode_Sheet1.Columns.Get(16).Visible = false;
            this.ssGichoCode_Sheet1.Columns.Get(16).Width = 65F;
            this.ssGichoCode_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssGichoCode_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssGichoCode_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssGichoCode_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssGichoCode_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ssGichoCode_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssGichoCode_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmBcodeMng
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1261, 683);
            this.ControlBox = false;
            this.Controls.Add(this.ssGichoCode);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panTitle);
            this.Name = "frmBcodeMng";
            this.Text = "기초코드 관리(관리자)";
            this.Load += new System.EventHandler(this.frmBcodeMng_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssGichoCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssGichoCode_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboCode;
        private FarPoint.Win.Spread.FpSpread ssGichoCode;
        private FarPoint.Win.Spread.SheetView ssGichoCode_Sheet1;
        private System.Windows.Forms.Label lblTitleSub0;
    }
}