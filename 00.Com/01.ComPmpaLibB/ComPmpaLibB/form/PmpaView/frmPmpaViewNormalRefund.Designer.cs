namespace ComPmpaLibB
{
    partial class frmPmpaViewNormalRefund
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color396636397868238106031", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Static470636397868238106031", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static705636397868238106031");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Static777636397868238106031");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("Static1262636397868238106031");
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkNotRef = new System.Windows.Forms.CheckBox();
            this.txtGunsu = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboYYMM = new System.Windows.Forms.ComboBox();
            this.dtpTDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.optGbn0 = new System.Windows.Forms.RadioButton();
            this.optGbn1 = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.optDate0 = new System.Windows.Forms.RadioButton();
            this.optDate1 = new System.Windows.Forms.RadioButton();
            this.pan = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.ssList = new FarPoint.Win.Spread.FpSpread();
            this.ssList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.pan.SuspendLayout();
            this.panTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel2.Controls.Add(this.label3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 140);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(907, 28);
            this.panel2.TabIndex = 113;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 12);
            this.label3.TabIndex = 22;
            this.label3.Text = "조회 결과";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkNotRef);
            this.panel1.Controls.Add(this.txtGunsu);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cboYYMM);
            this.panel1.Controls.Add(this.dtpTDate);
            this.panel1.Controls.Add(this.dtpFDate);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(907, 78);
            this.panel1.TabIndex = 112;
            // 
            // chkNotRef
            // 
            this.chkNotRef.AutoSize = true;
            this.chkNotRef.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkNotRef.Location = new System.Drawing.Point(652, 25);
            this.chkNotRef.Name = "chkNotRef";
            this.chkNotRef.Size = new System.Drawing.Size(105, 21);
            this.chkNotRef.TabIndex = 105;
            this.chkNotRef.Text = "미환불건조회";
            this.chkNotRef.UseVisualStyleBackColor = true;
            // 
            // txtGunsu
            // 
            this.txtGunsu.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtGunsu.Location = new System.Drawing.Point(546, 23);
            this.txtGunsu.Name = "txtGunsu";
            this.txtGunsu.Size = new System.Drawing.Size(100, 25);
            this.txtGunsu.TabIndex = 104;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(506, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 17);
            this.label4.TabIndex = 103;
            this.label4.Text = "건수";
            // 
            // cboYYMM
            // 
            this.cboYYMM.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboYYMM.FormattingEnabled = true;
            this.cboYYMM.Location = new System.Drawing.Point(261, 22);
            this.cboYYMM.Name = "cboYYMM";
            this.cboYYMM.Size = new System.Drawing.Size(121, 25);
            this.cboYYMM.TabIndex = 102;
            // 
            // dtpTDate
            // 
            this.dtpTDate.CustomFormat = "yyyy-MM-dd";
            this.dtpTDate.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTDate.Location = new System.Drawing.Point(357, 22);
            this.dtpTDate.Name = "dtpTDate";
            this.dtpTDate.Size = new System.Drawing.Size(92, 25);
            this.dtpTDate.TabIndex = 101;
            // 
            // dtpFDate
            // 
            this.dtpFDate.CustomFormat = "yyyy-MM-dd";
            this.dtpFDate.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFDate.Location = new System.Drawing.Point(259, 22);
            this.dtpFDate.Name = "dtpFDate";
            this.dtpFDate.Size = new System.Drawing.Size(92, 25);
            this.dtpFDate.TabIndex = 100;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(195, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 17);
            this.label1.TabIndex = 99;
            this.label1.Text = "조회기간";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.optGbn0);
            this.groupBox2.Controls.Add(this.optGbn1);
            this.groupBox2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox2.Location = new System.Drawing.Point(105, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(84, 66);
            this.groupBox2.TabIndex = 98;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "작업구분";
            // 
            // optGbn0
            // 
            this.optGbn0.AutoSize = true;
            this.optGbn0.ForeColor = System.Drawing.Color.Black;
            this.optGbn0.Location = new System.Drawing.Point(6, 14);
            this.optGbn0.Name = "optGbn0";
            this.optGbn0.Size = new System.Drawing.Size(65, 21);
            this.optGbn0.TabIndex = 91;
            this.optGbn0.TabStop = true;
            this.optGbn0.Text = "보관금";
            this.optGbn0.UseVisualStyleBackColor = true;
            // 
            // optGbn1
            // 
            this.optGbn1.AutoSize = true;
            this.optGbn1.ForeColor = System.Drawing.Color.Black;
            this.optGbn1.Location = new System.Drawing.Point(6, 36);
            this.optGbn1.Name = "optGbn1";
            this.optGbn1.Size = new System.Drawing.Size(65, 21);
            this.optGbn1.TabIndex = 92;
            this.optGbn1.TabStop = true;
            this.optGbn1.Text = "환불금";
            this.optGbn1.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.optDate0);
            this.groupBox4.Controls.Add(this.optDate1);
            this.groupBox4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox4.Location = new System.Drawing.Point(14, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(85, 66);
            this.groupBox4.TabIndex = 96;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "구분";
            // 
            // optDate0
            // 
            this.optDate0.AutoSize = true;
            this.optDate0.ForeColor = System.Drawing.Color.Black;
            this.optDate0.Location = new System.Drawing.Point(6, 14);
            this.optDate0.Name = "optDate0";
            this.optDate0.Size = new System.Drawing.Size(65, 21);
            this.optDate0.TabIndex = 91;
            this.optDate0.TabStop = true;
            this.optDate0.Text = "일자별";
            this.optDate0.UseVisualStyleBackColor = true;
            // 
            // optDate1
            // 
            this.optDate1.AutoSize = true;
            this.optDate1.ForeColor = System.Drawing.Color.Black;
            this.optDate1.Location = new System.Drawing.Point(6, 36);
            this.optDate1.Name = "optDate1";
            this.optDate1.Size = new System.Drawing.Size(52, 21);
            this.optDate1.TabIndex = 92;
            this.optDate1.TabStop = true;
            this.optDate1.Text = "월별";
            this.optDate1.UseVisualStyleBackColor = true;
            // 
            // pan
            // 
            this.pan.BackColor = System.Drawing.Color.RoyalBlue;
            this.pan.Controls.Add(this.lblTitleSub0);
            this.pan.Dock = System.Windows.Forms.DockStyle.Top;
            this.pan.Location = new System.Drawing.Point(0, 34);
            this.pan.Name = "pan";
            this.pan.Size = new System.Drawing.Size(907, 28);
            this.pan.TabIndex = 111;
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
            this.lblTitleSub0.Text = "조회 옵션";
            this.lblTitleSub0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.label2);
            this.panTitle.Controls.Add(this.btnPrint);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.btnView);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(907, 34);
            this.panTitle.TabIndex = 110;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(8, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 21);
            this.label2.TabIndex = 81;
            this.label2.Text = "일반환불 기간별 조회";
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.AutoSize = true;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPrint.Location = new System.Drawing.Point(756, 1);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 30);
            this.btnPrint.TabIndex = 80;
            this.btnPrint.Text = "인쇄";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(829, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // btnView
            // 
            this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnView.AutoSize = true;
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnView.Location = new System.Drawing.Point(683, 1);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(72, 30);
            this.btnView.TabIndex = 77;
            this.btnView.Text = "조회";
            this.btnView.UseVisualStyleBackColor = true;
            // 
            // ssList
            // 
            this.ssList.AccessibleDescription = "ssList, Sheet1, Row 0, Column 0, ";
            this.ssList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssList.Location = new System.Drawing.Point(0, 168);
            this.ssList.Name = "ssList";
            namedStyle1.Font = new System.Drawing.Font("굴림체", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType1.Static = true;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("굴림체", 9F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType2.Static = true;
            namedStyle3.CellType = textCellType2;
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.Renderer = textCellType2;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType3.Static = true;
            namedStyle4.CellType = textCellType3;
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.Renderer = textCellType3;
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType4.Static = true;
            namedStyle5.CellType = textCellType4;
            namedStyle5.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle5.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle5.Renderer = textCellType4;
            namedStyle5.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4,
            namedStyle5});
            this.ssList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList_Sheet1});
            this.ssList.Size = new System.Drawing.Size(907, 395);
            this.ssList.TabIndex = 114;
            this.ssList.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssList.TextTipAppearance = tipAppearance1;
            this.ssList.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // ssList_Sheet1
            // 
            this.ssList_Sheet1.Reset();
            this.ssList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssList_Sheet1.ColumnCount = 12;
            this.ssList_Sheet1.RowCount = 50;
            this.ssList_Sheet1.Cells.Get(0, 5).StyleName = "Static1262636397868238106031";
            this.ssList_Sheet1.Cells.Get(0, 5).Value = "6";
            this.ssList_Sheet1.Cells.Get(0, 8).StyleName = "Static777636397868238106031";
            this.ssList_Sheet1.Cells.Get(0, 8).Value = "9";
            this.ssList_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "발생일자";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "등록번호";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "성명";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "자격";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "과";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "진료의사";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "보관금액";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "환불금액";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "반환사유";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "미환불 사유";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "ROWID";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "3년이후";
            this.ssList_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssList_Sheet1.ColumnHeader.Rows.Get(0).Height = 28F;
            this.ssList_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssList_Sheet1.Columns.Get(0).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssList_Sheet1.Columns.Get(0).Label = "발생일자";
            this.ssList_Sheet1.Columns.Get(0).Width = 69F;
            this.ssList_Sheet1.Columns.Get(1).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssList_Sheet1.Columns.Get(1).Label = "등록번호";
            this.ssList_Sheet1.Columns.Get(1).Width = 70F;
            this.ssList_Sheet1.Columns.Get(2).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssList_Sheet1.Columns.Get(2).Label = "성명";
            this.ssList_Sheet1.Columns.Get(2).Width = 73F;
            this.ssList_Sheet1.Columns.Get(3).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssList_Sheet1.Columns.Get(3).Label = "자격";
            this.ssList_Sheet1.Columns.Get(3).Width = 28F;
            this.ssList_Sheet1.Columns.Get(4).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssList_Sheet1.Columns.Get(4).Label = "과";
            this.ssList_Sheet1.Columns.Get(4).Width = 49F;
            this.ssList_Sheet1.Columns.Get(5).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssList_Sheet1.Columns.Get(5).Label = "진료의사";
            this.ssList_Sheet1.Columns.Get(5).Width = 54F;
            textCellType5.Static = true;
            this.ssList_Sheet1.Columns.Get(6).CellType = textCellType5;
            this.ssList_Sheet1.Columns.Get(6).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssList_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssList_Sheet1.Columns.Get(6).Label = "보관금액";
            this.ssList_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(6).Width = 95F;
            textCellType6.Static = true;
            this.ssList_Sheet1.Columns.Get(7).CellType = textCellType6;
            this.ssList_Sheet1.Columns.Get(7).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssList_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssList_Sheet1.Columns.Get(7).Label = "환불금액";
            this.ssList_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(7).Width = 95F;
            textCellType7.Static = true;
            this.ssList_Sheet1.Columns.Get(8).CellType = textCellType7;
            this.ssList_Sheet1.Columns.Get(8).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssList_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssList_Sheet1.Columns.Get(8).Label = "반환사유";
            this.ssList_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(8).Width = 166F;
            textCellType8.Static = true;
            this.ssList_Sheet1.Columns.Get(9).CellType = textCellType8;
            this.ssList_Sheet1.Columns.Get(9).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssList_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssList_Sheet1.Columns.Get(9).Label = "미환불 사유";
            this.ssList_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(9).Width = 151F;
            this.ssList_Sheet1.Columns.Get(10).Label = "ROWID";
            this.ssList_Sheet1.Columns.Get(10).Visible = false;
            this.ssList_Sheet1.Columns.Get(11).Label = "3년이후";
            this.ssList_Sheet1.Columns.Get(11).Visible = false;
            this.ssList_Sheet1.Columns.Get(11).Width = 29F;
            this.ssList_Sheet1.DefaultStyleName = "Static470636397868238106031";
            this.ssList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList_Sheet1.Rows.Default.Height = 16F;
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmPmpaViewNormalRefund
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(907, 563);
            this.Controls.Add(this.ssList);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pan);
            this.Controls.Add(this.panTitle);
            this.Name = "frmPmpaViewNormalRefund";
            this.Text = "환불대상자 기간별조회";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.pan.ResumeLayout(false);
            this.pan.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton optDate0;
        private System.Windows.Forms.RadioButton optDate1;
        private System.Windows.Forms.Panel pan;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnPrint;
        public System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.CheckBox chkNotRef;
        private System.Windows.Forms.TextBox txtGunsu;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboYYMM;
        private System.Windows.Forms.DateTimePicker dtpTDate;
        private System.Windows.Forms.DateTimePicker dtpFDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton optGbn0;
        private System.Windows.Forms.RadioButton optGbn1;
        private FarPoint.Win.Spread.FpSpread ssList;
        private FarPoint.Win.Spread.SheetView ssList_Sheet1;
    }
}