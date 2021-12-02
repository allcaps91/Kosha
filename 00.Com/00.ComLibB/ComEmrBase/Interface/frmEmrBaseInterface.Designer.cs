namespace ComEmrBase
{
    partial class frmEmrBaseInterface
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
            this.components = new System.ComponentModel.Container();
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color319637342141326300781", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text383637342141326330698", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("CheckBox556637342141326360619");
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType1 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Static592637342141326370593");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("Static664637342141326410497");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.Spread.NamedStyle namedStyle6 = new FarPoint.Win.Spread.NamedStyle("Color308637285056697859048", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle7 = new FarPoint.Win.Spread.NamedStyle("Text372637285056697878996", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance2 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.chkTO = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panEMG2 = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.txtPanoSearch = new System.Windows.Forms.TextBox();
            this.lblCnt = new System.Windows.Forms.Label();
            this.chkNo_DB = new System.Windows.Forms.CheckBox();
            this.chkHic = new System.Windows.Forms.CheckBox();
            this.ChkEtc = new System.Windows.Forms.CheckBox();
            this.chkEMR = new System.Windows.Forms.CheckBox();
            this.txtPano = new System.Windows.Forms.TextBox();
            this.cboMIN = new System.Windows.Forms.ComboBox();
            this.chkPostpay = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panEMG = new System.Windows.Forms.Panel();
            this.ss2 = new FarPoint.Win.Spread.FpSpread();
            this.ss2_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.ss1 = new FarPoint.Win.Spread.FpSpread();
            this.ss1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panEMG2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panEMG.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss2_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnStop);
            this.panTitle.Controls.Add(this.btnSave);
            this.panTitle.Controls.Add(this.btnClear);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1109, 34);
            this.panTitle.TabIndex = 12;
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.BackColor = System.Drawing.Color.Transparent;
            this.btnStop.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnStop.Location = new System.Drawing.Point(1033, 1);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(72, 30);
            this.btnStop.TabIndex = 7;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave.Location = new System.Drawing.Point(961, 1);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 30);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Start";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.BackColor = System.Drawing.Color.Transparent;
            this.btnClear.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnClear.Location = new System.Drawing.Point(889, 1);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(72, 30);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "청소";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(817, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Visible = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(236, 16);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "ABI 결과 자동 영상 EMR 등록";
            // 
            // chkTO
            // 
            this.chkTO.AutoSize = true;
            this.chkTO.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkTO.Location = new System.Drawing.Point(499, 5);
            this.chkTO.Name = "chkTO";
            this.chkTO.Size = new System.Drawing.Size(76, 16);
            this.chkTO.TabIndex = 5;
            this.chkTO.Text = "종검포함";
            this.chkTO.UseVisualStyleBackColor = true;
            this.chkTO.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panEMG2);
            this.panel1.Controls.Add(this.lblCnt);
            this.panel1.Controls.Add(this.chkNo_DB);
            this.panel1.Controls.Add(this.chkHic);
            this.panel1.Controls.Add(this.ChkEtc);
            this.panel1.Controls.Add(this.chkEMR);
            this.panel1.Controls.Add(this.txtPano);
            this.panel1.Controls.Add(this.cboMIN);
            this.panel1.Controls.Add(this.chkPostpay);
            this.panel1.Controls.Add(this.chkTO);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1109, 42);
            this.panel1.TabIndex = 13;
            // 
            // panEMG2
            // 
            this.panEMG2.Controls.Add(this.btnDelete);
            this.panEMG2.Controls.Add(this.btnSearch);
            this.panEMG2.Controls.Add(this.label1);
            this.panEMG2.Controls.Add(this.lblName);
            this.panEMG2.Controls.Add(this.txtPanoSearch);
            this.panEMG2.Location = new System.Drawing.Point(585, 2);
            this.panEMG2.Name = "panEMG2";
            this.panEMG2.Size = new System.Drawing.Size(455, 39);
            this.panEMG2.TabIndex = 12;
            this.panEMG2.Visible = false;
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDelete.ForeColor = System.Drawing.Color.Navy;
            this.btnDelete.Location = new System.Drawing.Point(391, 5);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(61, 30);
            this.btnDelete.TabIndex = 13;
            this.btnDelete.Text = "삭제(&D)";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.ForeColor = System.Drawing.Color.Navy;
            this.btnSearch.Location = new System.Drawing.Point(328, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(61, 30);
            this.btnSearch.TabIndex = 13;
            this.btnSearch.Text = "조회(&V)";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(5, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 16);
            this.label1.TabIndex = 12;
            this.label1.Text = "등록번호";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblName
            // 
            this.lblName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblName.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblName.Location = new System.Drawing.Point(177, 8);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(145, 21);
            this.lblName.TabIndex = 12;
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPanoSearch
            // 
            this.txtPanoSearch.Location = new System.Drawing.Point(87, 8);
            this.txtPanoSearch.Name = "txtPanoSearch";
            this.txtPanoSearch.Size = new System.Drawing.Size(84, 21);
            this.txtPanoSearch.TabIndex = 0;
            this.txtPanoSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPanoSearch_KeyDown);
            // 
            // lblCnt
            // 
            this.lblCnt.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCnt.Location = new System.Drawing.Point(385, 4);
            this.lblCnt.Name = "lblCnt";
            this.lblCnt.Size = new System.Drawing.Size(108, 32);
            this.lblCnt.TabIndex = 11;
            this.lblCnt.Text = "00";
            this.lblCnt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkNo_DB
            // 
            this.chkNo_DB.AutoSize = true;
            this.chkNo_DB.Checked = true;
            this.chkNo_DB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNo_DB.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkNo_DB.ForeColor = System.Drawing.Color.Blue;
            this.chkNo_DB.Location = new System.Drawing.Point(84, 27);
            this.chkNo_DB.Name = "chkNo_DB";
            this.chkNo_DB.Size = new System.Drawing.Size(133, 16);
            this.chkNo_DB.TabIndex = 10;
            this.chkNo_DB.Text = "이미지DB저장안함";
            this.chkNo_DB.UseVisualStyleBackColor = true;
            this.chkNo_DB.Visible = false;
            // 
            // chkHic
            // 
            this.chkHic.AutoSize = true;
            this.chkHic.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkHic.ForeColor = System.Drawing.Color.Blue;
            this.chkHic.Location = new System.Drawing.Point(223, 26);
            this.chkHic.Name = "chkHic";
            this.chkHic.Size = new System.Drawing.Size(78, 16);
            this.chkHic.TabIndex = 9;
            this.chkHic.Text = "건진EKG";
            this.chkHic.UseVisualStyleBackColor = true;
            this.chkHic.Visible = false;
            // 
            // ChkEtc
            // 
            this.ChkEtc.AutoSize = true;
            this.ChkEtc.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ChkEtc.ForeColor = System.Drawing.Color.Blue;
            this.ChkEtc.Location = new System.Drawing.Point(292, 5);
            this.ChkEtc.Name = "ChkEtc";
            this.ChkEtc.Size = new System.Drawing.Size(87, 16);
            this.ChkEtc.TabIndex = 8;
            this.ChkEtc.Text = "E6910단독";
            this.ChkEtc.UseVisualStyleBackColor = true;
            this.ChkEtc.Visible = false;
            // 
            // chkEMR
            // 
            this.chkEMR.AutoSize = true;
            this.chkEMR.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkEMR.ForeColor = System.Drawing.Color.Blue;
            this.chkEMR.Location = new System.Drawing.Point(84, 5);
            this.chkEMR.Name = "chkEMR";
            this.chkEMR.Size = new System.Drawing.Size(116, 16);
            this.chkEMR.TabIndex = 7;
            this.chkEMR.Text = "EMR 차트 제외";
            this.chkEMR.UseVisualStyleBackColor = true;
            this.chkEMR.Visible = false;
            // 
            // txtPano
            // 
            this.txtPano.Location = new System.Drawing.Point(200, 3);
            this.txtPano.Name = "txtPano";
            this.txtPano.Size = new System.Drawing.Size(86, 21);
            this.txtPano.TabIndex = 6;
            this.txtPano.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPano.Visible = false;
            // 
            // cboMIN
            // 
            this.cboMIN.FormattingEnabled = true;
            this.cboMIN.Location = new System.Drawing.Point(10, 3);
            this.cboMIN.Name = "cboMIN";
            this.cboMIN.Size = new System.Drawing.Size(70, 20);
            this.cboMIN.TabIndex = 0;
            // 
            // chkPostpay
            // 
            this.chkPostpay.AutoSize = true;
            this.chkPostpay.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkPostpay.Location = new System.Drawing.Point(12, 26);
            this.chkPostpay.Name = "chkPostpay";
            this.chkPostpay.Size = new System.Drawing.Size(153, 16);
            this.chkPostpay.TabIndex = 5;
            this.chkPostpay.Text = "후불/예약자 자동체크";
            this.chkPostpay.UseVisualStyleBackColor = true;
            this.chkPostpay.Visible = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panEMG);
            this.panel2.Controls.Add(this.ss1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 76);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1109, 527);
            this.panel2.TabIndex = 14;
            // 
            // panEMG
            // 
            this.panEMG.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.panEMG.Controls.Add(this.ss2);
            this.panEMG.Dock = System.Windows.Forms.DockStyle.Left;
            this.panEMG.Location = new System.Drawing.Point(601, 0);
            this.panEMG.Name = "panEMG";
            this.panEMG.Size = new System.Drawing.Size(508, 527);
            this.panEMG.TabIndex = 1;
            this.panEMG.Visible = false;
            // 
            // ss2
            // 
            this.ss2.AccessibleDescription = "ss1, Sheet1, Row 0, Column 0, ";
            this.ss2.Dock = System.Windows.Forms.DockStyle.Left;
            this.ss2.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ss2.Location = new System.Drawing.Point(0, 0);
            this.ss2.Name = "ss2";
            namedStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType1.MaxLength = 32000;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle3.CellType = checkBoxCellType1;
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.Renderer = checkBoxCellType1;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType2.Static = true;
            namedStyle4.CellType = textCellType2;
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.Renderer = textCellType2;
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType3.Static = true;
            namedStyle5.CellType = textCellType3;
            namedStyle5.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle5.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle5.Renderer = textCellType3;
            namedStyle5.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss2.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4,
            namedStyle5});
            this.ss2.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ss2_Sheet1});
            this.ss2.Size = new System.Drawing.Size(403, 527);
            this.ss2.TabIndex = 13;
            this.ss2.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ss2.TextTipAppearance = tipAppearance1;
            this.ss2.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ss2.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ss2_CellDoubleClick);
            // 
            // ss2_Sheet1
            // 
            this.ss2_Sheet1.Reset();
            this.ss2_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ss2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ss2_Sheet1.ColumnCount = 7;
            this.ss2_Sheet1.RowCount = 1;
            this.ss2_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss2_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = " ";
            this.ss2_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "처방일";
            this.ss2_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "검사일";
            this.ss2_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "검사명";
            this.ss2_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "결과";
            this.ss2_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "ROWID";
            this.ss2_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "rowid";
            this.ss2_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss2_Sheet1.ColumnHeader.Rows.Get(0).Height = 21F;
            this.ss2_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss2_Sheet1.Columns.Get(0).Label = " ";
            this.ss2_Sheet1.Columns.Get(0).StyleName = "CheckBox556637342141326360619";
            this.ss2_Sheet1.Columns.Get(0).Width = 29F;
            this.ss2_Sheet1.Columns.Get(1).Label = "처방일";
            this.ss2_Sheet1.Columns.Get(1).StyleName = "Static592637342141326370593";
            this.ss2_Sheet1.Columns.Get(1).Width = 71F;
            this.ss2_Sheet1.Columns.Get(2).Label = "검사일";
            this.ss2_Sheet1.Columns.Get(2).StyleName = "Static592637342141326370593";
            this.ss2_Sheet1.Columns.Get(2).Width = 70F;
            this.ss2_Sheet1.Columns.Get(3).Label = "검사명";
            this.ss2_Sheet1.Columns.Get(3).StyleName = "Static664637342141326410497";
            this.ss2_Sheet1.Columns.Get(3).Width = 173F;
            this.ss2_Sheet1.Columns.Get(4).Label = "결과";
            this.ss2_Sheet1.Columns.Get(4).StyleName = "Static592637342141326370593";
            this.ss2_Sheet1.Columns.Get(4).Width = 34F;
            this.ss2_Sheet1.Columns.Get(5).Label = "ROWID";
            this.ss2_Sheet1.Columns.Get(5).Visible = false;
            this.ss2_Sheet1.Columns.Get(6).Label = "rowid";
            this.ss2_Sheet1.Columns.Get(6).StyleName = "Static592637342141326370593";
            this.ss2_Sheet1.Columns.Get(6).Visible = false;
            this.ss2_Sheet1.DefaultStyleName = "Text383637342141326330698";
            this.ss2_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ss2_Sheet1.RowHeader.Visible = false;
            this.ss2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // ss1
            // 
            this.ss1.AccessibleDescription = "ss1, Sheet1, Row 0, Column 0, ";
            this.ss1.Dock = System.Windows.Forms.DockStyle.Left;
            this.ss1.Location = new System.Drawing.Point(0, 0);
            this.ss1.Name = "ss1";
            namedStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            namedStyle6.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle6.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle6.Parent = "DataAreaDefault";
            namedStyle6.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType4.MaxLength = 32000;
            namedStyle7.CellType = textCellType4;
            namedStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            namedStyle7.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle7.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle7.Parent = "DataAreaDefault";
            namedStyle7.Renderer = textCellType4;
            namedStyle7.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss1.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle6,
            namedStyle7});
            this.ss1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ss1_Sheet1});
            this.ss1.Size = new System.Drawing.Size(601, 527);
            this.ss1.TabIndex = 0;
            this.ss1.TabStripRatio = 0.6D;
            tipAppearance2.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance2.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ss1.TextTipAppearance = tipAppearance2;
            this.ss1.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ss1_CellClick);
            // 
            // ss1_Sheet1
            // 
            this.ss1_Sheet1.Reset();
            this.ss1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ss1_Sheet1.ColumnCount = 5;
            this.ss1_Sheet1.RowCount = 1;
            this.ss1_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "처방일자";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "등록번호";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "화일명";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "완료여부";
            this.ss1_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.Columns.Get(0).CellType = textCellType5;
            this.ss1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(0).Label = "처방일자";
            this.ss1_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(0).Width = 93F;
            this.ss1_Sheet1.Columns.Get(1).CellType = textCellType6;
            this.ss1_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(1).Label = "등록번호";
            this.ss1_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(1).Width = 89F;
            textCellType7.MaxLength = 30000;
            textCellType7.Multiline = true;
            textCellType7.WordWrap = true;
            this.ss1_Sheet1.Columns.Get(2).CellType = textCellType7;
            this.ss1_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ss1_Sheet1.Columns.Get(2).Label = "화일명";
            this.ss1_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(2).Width = 274F;
            this.ss1_Sheet1.Columns.Get(3).CellType = textCellType8;
            this.ss1_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(3).Visible = false;
            this.ss1_Sheet1.Columns.Get(4).CellType = textCellType9;
            this.ss1_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(4).Label = "완료여부";
            this.ss1_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(4).Width = 87F;
            this.ss1_Sheet1.DefaultStyleName = "Text372637285056697878996";
            this.ss1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frmEmrBaseInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1109, 603);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Name = "frmEmrBaseInterface";
            this.Text = "frmEmrBaseInterface";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmEmrBaseInterface_FormClosed);
            this.Load += new System.EventHandler(this.frmEmrBaseInterface_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panEMG2.ResumeLayout(false);
            this.panEMG2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panEMG.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ss2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss2_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox chkTO;
        private System.Windows.Forms.ComboBox cboMIN;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Timer timer1;
        private FarPoint.Win.Spread.FpSpread ss1;
        private FarPoint.Win.Spread.SheetView ss1_Sheet1;
        private System.Windows.Forms.CheckBox chkEMR;
        private System.Windows.Forms.TextBox txtPano;
        private System.Windows.Forms.CheckBox ChkEtc;
        private System.Windows.Forms.CheckBox chkHic;
        private System.Windows.Forms.CheckBox chkNo_DB;
        private System.Windows.Forms.Label lblCnt;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.CheckBox chkPostpay;
        private System.Windows.Forms.Panel panEMG;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPanoSearch;
        private System.Windows.Forms.Label lblName;
        private FarPoint.Win.Spread.FpSpread ss2;
        private FarPoint.Win.Spread.SheetView ss2_Sheet1;
        private System.Windows.Forms.Panel panEMG2;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnDelete;
    }
}