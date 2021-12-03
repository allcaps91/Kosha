namespace ComPmpaLibB
{
    partial class frmPmpaViewName
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color396636402056716075326", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Static498636402056716075326", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Color617636402056716231591");
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Color744636402056716231591");
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("Color852636402056716231591");
            FarPoint.Win.Spread.NamedStyle namedStyle6 = new FarPoint.Win.Spread.NamedStyle("Static1754636402056716544131");
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.pan = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.txtSname = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPart = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.collapsibleSplitContainer1 = new DevComponents.DotNetBar.Controls.CollapsibleSplitContainer();
            this.ssList2 = new FarPoint.Win.Spread.FpSpread();
            this.ssList2_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.ssList1 = new FarPoint.Win.Spread.FpSpread();
            this.ssList1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.pan.SuspendLayout();
            this.panTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.collapsibleSplitContainer1)).BeginInit();
            this.collapsibleSplitContainer1.Panel1.SuspendLayout();
            this.collapsibleSplitContainer1.Panel2.SuspendLayout();
            this.collapsibleSplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList2_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // pan
            // 
            this.pan.BackColor = System.Drawing.Color.RoyalBlue;
            this.pan.Controls.Add(this.lblTitleSub0);
            this.pan.Dock = System.Windows.Forms.DockStyle.Top;
            this.pan.Location = new System.Drawing.Point(0, 40);
            this.pan.Name = "pan";
            this.pan.Size = new System.Drawing.Size(824, 28);
            this.pan.TabIndex = 118;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(12, 9);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(62, 12);
            this.lblTitleSub0.TabIndex = 22;
            this.lblTitleSub0.Text = "조회 결과";
            this.lblTitleSub0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.txtSname);
            this.panTitle.Controls.Add(this.label3);
            this.panTitle.Controls.Add(this.txtPart);
            this.panTitle.Controls.Add(this.label1);
            this.panTitle.Controls.Add(this.btnOK);
            this.panTitle.Controls.Add(this.label2);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(824, 40);
            this.panTitle.TabIndex = 117;
            // 
            // txtSname
            // 
            this.txtSname.Location = new System.Drawing.Point(312, 8);
            this.txtSname.Name = "txtSname";
            this.txtSname.Size = new System.Drawing.Size(92, 21);
            this.txtSname.TabIndex = 105;
            this.txtSname.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSname_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(243, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 17);
            this.label3.TabIndex = 104;
            this.label3.Text = "수진자명";
            // 
            // txtPart
            // 
            this.txtPart.Location = new System.Drawing.Point(475, 8);
            this.txtPart.Name = "txtPart";
            this.txtPart.Size = new System.Drawing.Size(38, 21);
            this.txtPart.TabIndex = 0;
            this.txtPart.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPart_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(419, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 17);
            this.label1.TabIndex = 102;
            this.label1.Text = "입력조";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.AutoSize = true;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(672, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(72, 30);
            this.btnOK.TabIndex = 101;
            this.btnOK.Text = "확인";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.btnOK_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(8, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(188, 21);
            this.label2.TabIndex = 81;
            this.label2.Text = "당일 이름별 수진자 조회";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(746, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // collapsibleSplitContainer1
            // 
            this.collapsibleSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.collapsibleSplitContainer1.Location = new System.Drawing.Point(0, 68);
            this.collapsibleSplitContainer1.Name = "collapsibleSplitContainer1";
            // 
            // collapsibleSplitContainer1.Panel1
            // 
            this.collapsibleSplitContainer1.Panel1.Controls.Add(this.ssList2);
            // 
            // collapsibleSplitContainer1.Panel2
            // 
            this.collapsibleSplitContainer1.Panel2.Controls.Add(this.ssList1);
            this.collapsibleSplitContainer1.Size = new System.Drawing.Size(824, 458);
            this.collapsibleSplitContainer1.SplitterDistance = 495;
            this.collapsibleSplitContainer1.SplitterWidth = 20;
            this.collapsibleSplitContainer1.TabIndex = 119;
            // 
            // ssList2
            // 
            this.ssList2.AccessibleDescription = "ssList2, Sheet1, Row 0, Column 0, ";
            this.ssList2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssList2.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssList2.Location = new System.Drawing.Point(0, 0);
            this.ssList2.Name = "ssList2";
            this.ssList2.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList2_Sheet1});
            this.ssList2.Size = new System.Drawing.Size(495, 458);
            this.ssList2.TabIndex = 0;
            this.ssList2.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssList2_CellDoubleClick);
            // 
            // ssList2_Sheet1
            // 
            this.ssList2_Sheet1.Reset();
            this.ssList2_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssList2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssList2_Sheet1.ColumnCount = 6;
            this.ssList2_Sheet1.RowCount = 1;
            this.ssList2_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "성명";
            this.ssList2_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "별록번호";
            this.ssList2_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "과";
            this.ssList2_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "초재구분";
            this.ssList2_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "의사명";
            this.ssList2_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "진찰료";
            this.ssList2_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssList2_Sheet1.Columns.Get(0).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssList2_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList2_Sheet1.Columns.Get(0).Label = "성명";
            this.ssList2_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList2_Sheet1.Columns.Get(0).Width = 70F;
            this.ssList2_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssList2_Sheet1.Columns.Get(1).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssList2_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList2_Sheet1.Columns.Get(1).Label = "별록번호";
            this.ssList2_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList2_Sheet1.Columns.Get(1).Width = 80F;
            this.ssList2_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssList2_Sheet1.Columns.Get(2).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssList2_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList2_Sheet1.Columns.Get(2).Label = "과";
            this.ssList2_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList2_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssList2_Sheet1.Columns.Get(3).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssList2_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList2_Sheet1.Columns.Get(3).Label = "초재구분";
            this.ssList2_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList2_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.ssList2_Sheet1.Columns.Get(4).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssList2_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList2_Sheet1.Columns.Get(4).Label = "의사명";
            this.ssList2_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList2_Sheet1.Columns.Get(4).Width = 70F;
            this.ssList2_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.ssList2_Sheet1.Columns.Get(5).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssList2_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList2_Sheet1.Columns.Get(5).Label = "진찰료";
            this.ssList2_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList2_Sheet1.Columns.Get(5).Width = 80F;
            this.ssList2_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // ssList1
            // 
            this.ssList1.AccessibleDescription = "ssList1, Sheet1, Row 0, Column 0, 수진자명";
            this.ssList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssList1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssList1.Location = new System.Drawing.Point(0, 0);
            this.ssList1.Name = "ssList1";
            namedStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            namedStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            namedStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            textCellType7.Static = true;
            namedStyle2.CellType = textCellType7;
            namedStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            namedStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType7;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            namedStyle5.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle5.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle5.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType8.Static = true;
            namedStyle6.CellType = textCellType8;
            namedStyle6.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            namedStyle6.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle6.Renderer = textCellType8;
            namedStyle6.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList1.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4,
            namedStyle5,
            namedStyle6});
            this.ssList1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList1_Sheet1});
            this.ssList1.Size = new System.Drawing.Size(309, 458);
            this.ssList1.TabIndex = 0;
            this.ssList1.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssList1.TextTipAppearance = tipAppearance1;
            this.ssList1.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            // 
            // ssList1_Sheet1
            // 
            this.ssList1_Sheet1.Reset();
            this.ssList1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssList1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssList1_Sheet1.ColumnCount = 3;
            this.ssList1_Sheet1.RowCount = 16;
            this.ssList1_Sheet1.Cells.Get(0, 0).ForeColor = System.Drawing.Color.Black;
            this.ssList1_Sheet1.Cells.Get(0, 0).Value = "수진자명";
            this.ssList1_Sheet1.Cells.Get(1, 0).ForeColor = System.Drawing.Color.Black;
            this.ssList1_Sheet1.Cells.Get(1, 0).Value = "병록번호";
            this.ssList1_Sheet1.Cells.Get(1, 2).StyleName = "Color852636402056716231591";
            this.ssList1_Sheet1.Cells.Get(2, 0).ForeColor = System.Drawing.Color.Black;
            this.ssList1_Sheet1.Cells.Get(2, 0).Value = "진료과목";
            this.ssList1_Sheet1.Cells.Get(3, 0).ForeColor = System.Drawing.Color.Black;
            this.ssList1_Sheet1.Cells.Get(3, 0).Value = "의사코드";
            this.ssList1_Sheet1.Cells.Get(4, 0).ForeColor = System.Drawing.Color.Black;
            this.ssList1_Sheet1.Cells.Get(4, 0).Value = "환자구분";
            this.ssList1_Sheet1.Cells.Get(5, 0).ForeColor = System.Drawing.Color.Black;
            this.ssList1_Sheet1.Cells.Get(5, 0).Value = "지역코드";
            this.ssList1_Sheet1.Cells.Get(6, 0).ForeColor = System.Drawing.Color.Black;
            this.ssList1_Sheet1.Cells.Get(6, 0).Value = "예약구분";
            this.ssList1_Sheet1.Cells.Get(7, 0).ForeColor = System.Drawing.Color.Black;
            this.ssList1_Sheet1.Cells.Get(7, 0).Value = "초재구분";
            this.ssList1_Sheet1.Cells.Get(8, 0).ForeColor = System.Drawing.Color.Black;
            this.ssList1_Sheet1.Cells.Get(8, 0).Value = "감액구분";
            this.ssList1_Sheet1.Cells.Get(9, 0).ForeColor = System.Drawing.Color.Black;
            this.ssList1_Sheet1.Cells.Get(9, 0).Value = "특진구분";
            this.ssList1_Sheet1.Cells.Get(10, 0).ForeColor = System.Drawing.Color.Black;
            this.ssList1_Sheet1.Cells.Get(10, 0).Value = "수납여부";
            this.ssList1_Sheet1.Cells.Get(11, 0).ForeColor = System.Drawing.Color.Black;
            this.ssList1_Sheet1.Cells.Get(11, 0).Value = "신환여부";
            this.ssList1_Sheet1.Cells.Get(12, 0).ForeColor = System.Drawing.Color.Black;
            this.ssList1_Sheet1.Cells.Get(12, 0).Value = "영수증발행";
            this.ssList1_Sheet1.Cells.Get(13, 0).ForeColor = System.Drawing.Color.Black;
            this.ssList1_Sheet1.Cells.Get(13, 0).Value = "접수시간";
            this.ssList1_Sheet1.Cells.Get(14, 0).ForeColor = System.Drawing.Color.Black;
            this.ssList1_Sheet1.Cells.Get(14, 0).Value = "수납시간";
            this.ssList1_Sheet1.Cells.Get(15, 0).ForeColor = System.Drawing.Color.Black;
            this.ssList1_Sheet1.Cells.Get(15, 0).Value = "영수금액";
            this.ssList1_Sheet1.Cells.Get(15, 1).StyleName = "Static1754636402056716544131";
            this.ssList1_Sheet1.Cells.Get(15, 2).StyleName = "Color852636402056716231591";
            this.ssList1_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssList1_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList1_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssList1_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList1_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList1_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssList1_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList1_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssList1_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList1_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssList1_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList1_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList1_Sheet1.ColumnHeader.Visible = false;
            this.ssList1_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssList1_Sheet1.Columns.Get(0).BackColor = System.Drawing.Color.White;
            this.ssList1_Sheet1.Columns.Get(0).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssList1_Sheet1.Columns.Get(0).Width = 87F;
            this.ssList1_Sheet1.Columns.Get(1).BackColor = System.Drawing.Color.White;
            this.ssList1_Sheet1.Columns.Get(1).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssList1_Sheet1.Columns.Get(1).ForeColor = System.Drawing.Color.Black;
            this.ssList1_Sheet1.Columns.Get(1).Width = 98F;
            this.ssList1_Sheet1.Columns.Get(2).BackColor = System.Drawing.Color.White;
            this.ssList1_Sheet1.Columns.Get(2).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssList1_Sheet1.Columns.Get(2).ForeColor = System.Drawing.Color.Black;
            this.ssList1_Sheet1.Columns.Get(2).Width = 106F;
            this.ssList1_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.ssList1_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.FilterBar.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList1_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssList1_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList1_Sheet1.LockBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssList1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList1_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList1_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssList1_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList1_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList1_Sheet1.RowHeader.Visible = false;
            this.ssList1_Sheet1.Rows.Get(0).Height = 27F;
            this.ssList1_Sheet1.Rows.Get(1).Height = 27F;
            this.ssList1_Sheet1.Rows.Get(2).Height = 27F;
            this.ssList1_Sheet1.Rows.Get(3).Height = 27F;
            this.ssList1_Sheet1.Rows.Get(4).Height = 27F;
            this.ssList1_Sheet1.Rows.Get(5).Height = 27F;
            this.ssList1_Sheet1.Rows.Get(6).Height = 27F;
            this.ssList1_Sheet1.Rows.Get(7).Height = 27F;
            this.ssList1_Sheet1.Rows.Get(8).Height = 27F;
            this.ssList1_Sheet1.Rows.Get(9).Height = 27F;
            this.ssList1_Sheet1.Rows.Get(10).Height = 27F;
            this.ssList1_Sheet1.Rows.Get(11).Height = 27F;
            this.ssList1_Sheet1.Rows.Get(12).Height = 27F;
            this.ssList1_Sheet1.Rows.Get(13).Height = 27F;
            this.ssList1_Sheet1.Rows.Get(14).Height = 27F;
            this.ssList1_Sheet1.Rows.Get(15).Height = 27F;
            this.ssList1_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssList1_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmPmpaViewName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(824, 526);
            this.Controls.Add(this.collapsibleSplitContainer1);
            this.Controls.Add(this.pan);
            this.Controls.Add(this.panTitle);
            this.Name = "frmPmpaViewName";
            this.Text = "당일 이름별 수진자 조회";
            this.pan.ResumeLayout(false);
            this.pan.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.collapsibleSplitContainer1.Panel1.ResumeLayout(false);
            this.collapsibleSplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.collapsibleSplitContainer1)).EndInit();
            this.collapsibleSplitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssList2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList2_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pan;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox txtSname;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPart;
        private System.Windows.Forms.Label label1;
        private DevComponents.DotNetBar.Controls.CollapsibleSplitContainer collapsibleSplitContainer1;
        private FarPoint.Win.Spread.FpSpread ssList2;
        private FarPoint.Win.Spread.SheetView ssList2_Sheet1;
        private FarPoint.Win.Spread.FpSpread ssList1;
        private FarPoint.Win.Spread.SheetView ssList1_Sheet1;
    }
}