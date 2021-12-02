namespace ComNurLibB
{
    partial class frmDrSchInput
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
            FarPoint.Win.EmptyBorder emptyBorder6 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.EmptyBorder emptyBorder7 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.EmptyBorder emptyBorder8 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.EmptyBorder emptyBorder9 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.EmptyBorder emptyBorder10 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType13 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType14 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblmst = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSel = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cboYYMM = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cboDept = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.cboDr3 = new System.Windows.Forms.ComboBox();
            this.cboDept3 = new System.Windows.Forms.ComboBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.cboDr2 = new System.Windows.Forms.ComboBox();
            this.cboDept2 = new System.Windows.Forms.ComboBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.cboDr1 = new System.Windows.Forms.ComboBox();
            this.cboDept1 = new System.Windows.Forms.ComboBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.ssColor = new FarPoint.Win.Spread.FpSpread();
            this.ssColor_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnRegist = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ssList = new FarPoint.Win.Spread.FpSpread();
            this.ssList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitleSub0.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssColor_Sheet1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblmst);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 0);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(1109, 32);
            this.panTitleSub0.TabIndex = 16;
            // 
            // lblmst
            // 
            this.lblmst.AutoSize = true;
            this.lblmst.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblmst.ForeColor = System.Drawing.Color.White;
            this.lblmst.Location = new System.Drawing.Point(8, 7);
            this.lblmst.Name = "lblmst";
            this.lblmst.Size = new System.Drawing.Size(163, 12);
            this.lblmst.TabIndex = 0;
            this.lblmst.Text = "진료과별 의사 진료 스케쥴";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSel);
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.ssColor);
            this.groupBox1.Controls.Add(this.btnExit);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.btnPrint);
            this.groupBox1.Controls.Add(this.btnRegist);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1109, 80);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            // 
            // btnSel
            // 
            this.btnSel.Location = new System.Drawing.Point(279, 27);
            this.btnSel.Name = "btnSel";
            this.btnSel.Size = new System.Drawing.Size(101, 23);
            this.btnSel.TabIndex = 74;
            this.btnSel.Text = "조건검색보기";
            this.btnSel.UseVisualStyleBackColor = true;
            this.btnSel.Click += new System.EventHandler(this.btnSel_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cboYYMM);
            this.groupBox5.Location = new System.Drawing.Point(6, 18);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(110, 52);
            this.groupBox5.TabIndex = 70;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "작업년월";
            // 
            // cboYYMM
            // 
            this.cboYYMM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboYYMM.FormattingEnabled = true;
            this.cboYYMM.Location = new System.Drawing.Point(10, 22);
            this.cboYYMM.Name = "cboYYMM";
            this.cboYYMM.Size = new System.Drawing.Size(88, 20);
            this.cboYYMM.TabIndex = 58;
            this.cboYYMM.SelectedIndexChanged += new System.EventHandler(this.cboYYMM_SelectedIndexChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cboDept);
            this.groupBox4.Location = new System.Drawing.Point(122, 19);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(151, 51);
            this.groupBox4.TabIndex = 71;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "진료과";
            // 
            // cboDept
            // 
            this.cboDept.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDept.FormattingEnabled = true;
            this.cboDept.Location = new System.Drawing.Point(6, 20);
            this.cboDept.Name = "cboDept";
            this.cboDept.Size = new System.Drawing.Size(136, 20);
            this.cboDept.TabIndex = 57;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox8);
            this.groupBox3.Controls.Add(this.groupBox7);
            this.groupBox3.Controls.Add(this.groupBox6);
            this.groupBox3.Controls.Add(this.txtSearch);
            this.groupBox3.Location = new System.Drawing.Point(6, 84);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(698, 68);
            this.groupBox3.TabIndex = 73;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "조건검색";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.cboDr3);
            this.groupBox8.Controls.Add(this.cboDept3);
            this.groupBox8.Location = new System.Drawing.Point(498, 11);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(186, 51);
            this.groupBox8.TabIndex = 74;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "진료과/의사명";
            // 
            // cboDr3
            // 
            this.cboDr3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDr3.FormattingEnabled = true;
            this.cboDr3.Location = new System.Drawing.Point(57, 20);
            this.cboDr3.Name = "cboDr3";
            this.cboDr3.Size = new System.Drawing.Size(119, 20);
            this.cboDr3.TabIndex = 58;
            // 
            // cboDept3
            // 
            this.cboDept3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDept3.FormattingEnabled = true;
            this.cboDept3.Location = new System.Drawing.Point(6, 20);
            this.cboDept3.Name = "cboDept3";
            this.cboDept3.Size = new System.Drawing.Size(46, 20);
            this.cboDept3.TabIndex = 57;
            this.cboDept3.SelectedIndexChanged += new System.EventHandler(this.cboDept3_SelectedIndexChanged);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.cboDr2);
            this.groupBox7.Controls.Add(this.cboDept2);
            this.groupBox7.Location = new System.Drawing.Point(306, 11);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(186, 51);
            this.groupBox7.TabIndex = 73;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "진료과/의사명";
            // 
            // cboDr2
            // 
            this.cboDr2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDr2.FormattingEnabled = true;
            this.cboDr2.Location = new System.Drawing.Point(57, 20);
            this.cboDr2.Name = "cboDr2";
            this.cboDr2.Size = new System.Drawing.Size(119, 20);
            this.cboDr2.TabIndex = 58;
            // 
            // cboDept2
            // 
            this.cboDept2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDept2.FormattingEnabled = true;
            this.cboDept2.Location = new System.Drawing.Point(6, 20);
            this.cboDept2.Name = "cboDept2";
            this.cboDept2.Size = new System.Drawing.Size(46, 20);
            this.cboDept2.TabIndex = 57;
            this.cboDept2.SelectedIndexChanged += new System.EventHandler(this.cboDept2_SelectedIndexChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.cboDr1);
            this.groupBox6.Controls.Add(this.cboDept1);
            this.groupBox6.Location = new System.Drawing.Point(115, 11);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(186, 51);
            this.groupBox6.TabIndex = 72;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "진료과/의사명";
            // 
            // cboDr1
            // 
            this.cboDr1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDr1.FormattingEnabled = true;
            this.cboDr1.Location = new System.Drawing.Point(57, 20);
            this.cboDr1.Name = "cboDr1";
            this.cboDr1.Size = new System.Drawing.Size(119, 20);
            this.cboDr1.TabIndex = 58;
            // 
            // cboDept1
            // 
            this.cboDept1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDept1.FormattingEnabled = true;
            this.cboDept1.Location = new System.Drawing.Point(6, 20);
            this.cboDept1.Name = "cboDept1";
            this.cboDept1.Size = new System.Drawing.Size(46, 20);
            this.cboDept1.TabIndex = 57;
            this.cboDept1.SelectedIndexChanged += new System.EventHandler(this.cboDept1_SelectedIndexChanged);
            // 
            // txtSearch
            // 
            this.txtSearch.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.txtSearch.Location = new System.Drawing.Point(9, 31);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(84, 21);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ssColor
            // 
            this.ssColor.AccessibleDescription = "ssColor, Sheet1, Row 0, Column 0, 1.진료";
            this.ssColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ssColor.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssColor.Location = new System.Drawing.Point(383, 18);
            this.ssColor.Name = "ssColor";
            this.ssColor.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssColor_Sheet1});
            this.ssColor.Size = new System.Drawing.Size(256, 39);
            this.ssColor.TabIndex = 72;
            this.ssColor.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            // 
            // ssColor_Sheet1
            // 
            this.ssColor_Sheet1.Reset();
            this.ssColor_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssColor_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssColor_Sheet1.ColumnCount = 6;
            this.ssColor_Sheet1.RowCount = 2;
            this.ssColor_Sheet1.Cells.Get(0, 0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ssColor_Sheet1.Cells.Get(0, 0).Value = "1.진료";
            this.ssColor_Sheet1.Cells.Get(0, 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ssColor_Sheet1.Cells.Get(0, 1).Value = "2.수술";
            this.ssColor_Sheet1.Cells.Get(0, 2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.ssColor_Sheet1.Cells.Get(0, 2).Value = "3.특검";
            this.ssColor_Sheet1.Cells.Get(0, 3).Value = "4.휴진";
            this.ssColor_Sheet1.Cells.Get(0, 4).Value = "5.학회";
            this.ssColor_Sheet1.Cells.Get(0, 5).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ssColor_Sheet1.Cells.Get(0, 5).Value = "A.협진";
            this.ssColor_Sheet1.Cells.Get(1, 0).Value = "6.휴가";
            this.ssColor_Sheet1.Cells.Get(1, 1).Value = "7.출장";
            this.ssColor_Sheet1.Cells.Get(1, 2).Value = "8.기타";
            this.ssColor_Sheet1.Cells.Get(1, 3).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.ssColor_Sheet1.Cells.Get(1, 3).Value = "9.OFF";
            this.ssColor_Sheet1.Cells.Get(1, 4).Value = "D.교육";
            this.ssColor_Sheet1.Cells.Get(1, 5).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.ssColor_Sheet1.Cells.Get(1, 5).Value = "[선]";
            this.ssColor_Sheet1.ColumnHeader.Visible = false;
            this.ssColor_Sheet1.Columns.Get(0).Border = emptyBorder6;
            this.ssColor_Sheet1.Columns.Get(0).CellType = textCellType8;
            this.ssColor_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssColor_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssColor_Sheet1.Columns.Get(0).Width = 51F;
            this.ssColor_Sheet1.Columns.Get(1).Border = emptyBorder7;
            this.ssColor_Sheet1.Columns.Get(1).CellType = textCellType9;
            this.ssColor_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssColor_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssColor_Sheet1.Columns.Get(1).Width = 51F;
            this.ssColor_Sheet1.Columns.Get(2).Border = emptyBorder8;
            this.ssColor_Sheet1.Columns.Get(2).CellType = textCellType10;
            this.ssColor_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssColor_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssColor_Sheet1.Columns.Get(2).Width = 51F;
            this.ssColor_Sheet1.Columns.Get(3).Border = emptyBorder9;
            this.ssColor_Sheet1.Columns.Get(3).CellType = textCellType11;
            this.ssColor_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssColor_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssColor_Sheet1.Columns.Get(3).Width = 51F;
            this.ssColor_Sheet1.Columns.Get(4).Border = emptyBorder10;
            this.ssColor_Sheet1.Columns.Get(4).CellType = textCellType12;
            this.ssColor_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssColor_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssColor_Sheet1.Columns.Get(4).Width = 51F;
            this.ssColor_Sheet1.Columns.Get(5).CellType = textCellType13;
            this.ssColor_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssColor_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssColor_Sheet1.Columns.Get(5).Width = 50F;
            this.ssColor_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Black);
            this.ssColor_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
            this.ssColor_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssColor_Sheet1.RowHeader.Visible = false;
            this.ssColor_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Black);
            this.ssColor_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(1025, 27);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 65;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Location = new System.Drawing.Point(714, 27);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 69;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.Location = new System.Drawing.Point(947, 27);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 30);
            this.btnPrint.TabIndex = 66;
            this.btnPrint.Text = "출력";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnRegist
            // 
            this.btnRegist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRegist.BackColor = System.Drawing.Color.Transparent;
            this.btnRegist.Location = new System.Drawing.Point(792, 27);
            this.btnRegist.Name = "btnRegist";
            this.btnRegist.Size = new System.Drawing.Size(72, 30);
            this.btnRegist.TabIndex = 68;
            this.btnRegist.Text = "등록";
            this.btnRegist.UseVisualStyleBackColor = false;
            this.btnRegist.Click += new System.EventHandler(this.btnRegist_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Location = new System.Drawing.Point(870, 27);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 30);
            this.btnCancel.TabIndex = 67;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ssList);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 112);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1109, 453);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "스케쥴내역";
            // 
            // ssList
            // 
            this.ssList.AccessibleDescription = "fpSpread1, Sheet1, Row 0, Column 0, ";
            this.ssList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssList.Location = new System.Drawing.Point(3, 17);
            this.ssList.Name = "ssList";
            this.ssList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList_Sheet1});
            this.ssList.Size = new System.Drawing.Size(1103, 433);
            this.ssList.TabIndex = 0;
            this.ssList.Change += new FarPoint.Win.Spread.ChangeEventHandler(this.ssList_Change);
            // 
            // ssList_Sheet1
            // 
            this.ssList_Sheet1.Reset();
            this.ssList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssList_Sheet1.ColumnCount = 1;
            this.ssList_Sheet1.RowCount = 1;
            this.ssList_Sheet1.Columns.Get(0).CellType = textCellType14;
            this.ssList_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Black);
            this.ssList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Black);
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmDrSchInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1109, 565);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panTitleSub0);
            this.Name = "frmDrSchInput";
            this.Text = "의사스케쥴관리";
            this.Load += new System.EventHandler(this.frmDoctorSch_Load);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssColor_Sheet1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblmst;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private FarPoint.Win.Spread.FpSpread ssList;
        private FarPoint.Win.Spread.SheetView ssList_Sheet1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnRegist;
        private System.Windows.Forms.Button btnCancel;
        private FarPoint.Win.Spread.FpSpread ssColor;
        private FarPoint.Win.Spread.SheetView ssColor_Sheet1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.ComboBox cboDr3;
        private System.Windows.Forms.ComboBox cboDept3;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.ComboBox cboDr2;
        private System.Windows.Forms.ComboBox cboDept2;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ComboBox cboDr1;
        private System.Windows.Forms.ComboBox cboDept1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox cboYYMM;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox cboDept;
        private System.Windows.Forms.Button btnSel;
    }
}