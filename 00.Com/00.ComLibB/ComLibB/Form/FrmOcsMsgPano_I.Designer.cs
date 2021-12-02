namespace ComLibB
{
    partial class FrmOcsMsgPano_I
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType23 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType24 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType25 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType26 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType27 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType28 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType29 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType30 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType31 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType32 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType33 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtPano = new System.Windows.Forms.TextBox();
            this.cboSabun = new System.Windows.Forms.ComboBox();
            this.lblSabun = new System.Windows.Forms.Label();
            this.lblNum = new System.Windows.Forms.Label();
            this.dtpTDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFDate = new System.Windows.Forms.DateTimePicker();
            this.cboWard = new System.Windows.Forms.ComboBox();
            this.grbSORT = new System.Windows.Forms.GroupBox();
            this.rdoNum = new System.Windows.Forms.RadioButton();
            this.rdoName = new System.Windows.Forms.RadioButton();
            this.grbSearch = new System.Windows.Forms.GroupBox();
            this.rdoDays = new System.Windows.Forms.RadioButton();
            this.rdoJewon = new System.Windows.Forms.RadioButton();
            this.rdoRegis = new System.Windows.Forms.RadioButton();
            this.grbMsg = new System.Windows.Forms.GroupBox();
            this.ssMsg = new FarPoint.Win.Spread.FpSpread();
            this.ssMsg_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panMain = new System.Windows.Forms.Panel();
            this.txtInfo = new System.Windows.Forms.RichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnSpecial = new System.Windows.Forms.Button();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.grbSORT.SuspendLayout();
            this.grbSearch.SuspendLayout();
            this.grbMsg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssMsg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMsg_Sheet1)).BeginInit();
            this.panMain.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(997, 33);
            this.panTitle.TabIndex = 13;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(921, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 29);
            this.btnExit.TabIndex = 27;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(252, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "심사과 환자메세지등록(입원환자)";
            // 
            // btnNew
            // 
            this.btnNew.BackColor = System.Drawing.Color.Transparent;
            this.btnNew.Location = new System.Drawing.Point(276, 11);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(72, 29);
            this.btnNew.TabIndex = 31;
            this.btnNew.Text = "신규등록";
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Transparent;
            this.btnDelete.Location = new System.Drawing.Point(276, 71);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(72, 29);
            this.btnDelete.TabIndex = 29;
            this.btnDelete.Text = "삭제";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Location = new System.Drawing.Point(276, 41);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 29);
            this.btnCancel.TabIndex = 30;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.Location = new System.Drawing.Point(276, 101);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 29);
            this.btnSave.TabIndex = 28;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.ssView);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.txtName);
            this.panel1.Controls.Add(this.btnNew);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.txtPano);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.cboSabun);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.lblSabun);
            this.panel1.Controls.Add(this.lblNum);
            this.panel1.Controls.Add(this.dtpTDate);
            this.panel1.Controls.Add(this.dtpFDate);
            this.panel1.Controls.Add(this.cboWard);
            this.panel1.Controls.Add(this.grbSORT);
            this.panel1.Controls.Add(this.grbSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 33);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(353, 692);
            this.panel1.TabIndex = 32;
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "ssView, Sheet1, Row 0, Column 0, ";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ssView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssView.Location = new System.Drawing.Point(0, 167);
            this.ssView.Name = "ssView";
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(349, 521);
            this.ssView.TabIndex = 56;
            this.ssView.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssView_CellClick);
            this.ssView.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssView_CellDoubleClick);
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 4;
            this.ssView_Sheet1.RowCount = 1;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "등록번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "성명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "병동";
            this.ssView_Sheet1.Columns.Get(0).CellType = textCellType23;
            this.ssView_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(0).Label = "등록번호";
            this.ssView_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(0).Width = 100F;
            this.ssView_Sheet1.Columns.Get(1).CellType = textCellType24;
            this.ssView_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(1).Label = "성명";
            this.ssView_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(1).Width = 90F;
            this.ssView_Sheet1.Columns.Get(2).CellType = textCellType25;
            this.ssView_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(2).Label = "병동";
            this.ssView_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(2).Width = 80F;
            this.ssView_Sheet1.Columns.Get(3).CellType = textCellType26;
            this.ssView_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(3).Visible = false;
            this.ssView_Sheet1.Columns.Get(3).Width = 80F;
            this.ssView_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssView_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Location = new System.Drawing.Point(276, 131);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 29);
            this.btnSearch.TabIndex = 53;
            this.btnSearch.Text = "자료조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtName
            // 
            this.txtName.Enabled = false;
            this.txtName.Location = new System.Drawing.Point(179, 107);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(92, 25);
            this.txtName.TabIndex = 52;
            // 
            // txtPano
            // 
            this.txtPano.Location = new System.Drawing.Point(83, 107);
            this.txtPano.Name = "txtPano";
            this.txtPano.Size = new System.Drawing.Size(92, 25);
            this.txtPano.TabIndex = 51;
            this.txtPano.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPano_KeyDown);
            this.txtPano.Leave += new System.EventHandler(this.txtPano_Leave);
            // 
            // cboSabun
            // 
            this.cboSabun.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSabun.FormattingEnabled = true;
            this.cboSabun.Location = new System.Drawing.Point(83, 135);
            this.cboSabun.Name = "cboSabun";
            this.cboSabun.Size = new System.Drawing.Size(188, 25);
            this.cboSabun.TabIndex = 50;
            // 
            // lblSabun
            // 
            this.lblSabun.AutoSize = true;
            this.lblSabun.Location = new System.Drawing.Point(17, 139);
            this.lblSabun.Name = "lblSabun";
            this.lblSabun.Size = new System.Drawing.Size(60, 17);
            this.lblSabun.TabIndex = 49;
            this.lblSabun.Text = "등록자 : ";
            // 
            // lblNum
            // 
            this.lblNum.AutoSize = true;
            this.lblNum.Location = new System.Drawing.Point(4, 110);
            this.lblNum.Name = "lblNum";
            this.lblNum.Size = new System.Drawing.Size(73, 17);
            this.lblNum.TabIndex = 48;
            this.lblNum.Text = "등록번호 : ";
            // 
            // dtpTDate
            // 
            this.dtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTDate.Location = new System.Drawing.Point(179, 77);
            this.dtpTDate.Name = "dtpTDate";
            this.dtpTDate.Size = new System.Drawing.Size(92, 25);
            this.dtpTDate.TabIndex = 47;
            // 
            // dtpFDate
            // 
            this.dtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFDate.Location = new System.Drawing.Point(83, 77);
            this.dtpFDate.Name = "dtpFDate";
            this.dtpFDate.Size = new System.Drawing.Size(92, 25);
            this.dtpFDate.TabIndex = 46;
            // 
            // cboWard
            // 
            this.cboWard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWard.FormattingEnabled = true;
            this.cboWard.Location = new System.Drawing.Point(83, 45);
            this.cboWard.Name = "cboWard";
            this.cboWard.Size = new System.Drawing.Size(188, 25);
            this.cboWard.TabIndex = 45;
            // 
            // grbSORT
            // 
            this.grbSORT.Controls.Add(this.rdoNum);
            this.grbSORT.Controls.Add(this.rdoName);
            this.grbSORT.Location = new System.Drawing.Point(83, 5);
            this.grbSORT.Name = "grbSORT";
            this.grbSORT.Size = new System.Drawing.Size(190, 37);
            this.grbSORT.TabIndex = 43;
            this.grbSORT.TabStop = false;
            this.grbSORT.Text = "SORT";
            // 
            // rdoNum
            // 
            this.rdoNum.AutoSize = true;
            this.rdoNum.Checked = true;
            this.rdoNum.Location = new System.Drawing.Point(82, 12);
            this.rdoNum.Name = "rdoNum";
            this.rdoNum.Size = new System.Drawing.Size(91, 21);
            this.rdoNum.TabIndex = 40;
            this.rdoNum.TabStop = true;
            this.rdoNum.Text = "등록번호순";
            this.rdoNum.UseVisualStyleBackColor = true;
            // 
            // rdoName
            // 
            this.rdoName.AutoSize = true;
            this.rdoName.Location = new System.Drawing.Point(11, 12);
            this.rdoName.Name = "rdoName";
            this.rdoName.Size = new System.Drawing.Size(65, 21);
            this.rdoName.TabIndex = 39;
            this.rdoName.TabStop = true;
            this.rdoName.Text = "성명순";
            this.rdoName.UseVisualStyleBackColor = true;
            // 
            // grbSearch
            // 
            this.grbSearch.Controls.Add(this.rdoDays);
            this.grbSearch.Controls.Add(this.rdoJewon);
            this.grbSearch.Controls.Add(this.rdoRegis);
            this.grbSearch.Location = new System.Drawing.Point(3, 5);
            this.grbSearch.Name = "grbSearch";
            this.grbSearch.Size = new System.Drawing.Size(76, 100);
            this.grbSearch.TabIndex = 42;
            this.grbSearch.TabStop = false;
            this.grbSearch.Text = "찾기방법";
            // 
            // rdoDays
            // 
            this.rdoDays.AutoSize = true;
            this.rdoDays.Location = new System.Drawing.Point(7, 70);
            this.rdoDays.Name = "rdoDays";
            this.rdoDays.Size = new System.Drawing.Size(65, 21);
            this.rdoDays.TabIndex = 43;
            this.rdoDays.TabStop = true;
            this.rdoDays.Text = "일자별";
            this.rdoDays.UseVisualStyleBackColor = true;
            this.rdoDays.CheckedChanged += new System.EventHandler(this.rdoDays_CheckedChanged);
            // 
            // rdoJewon
            // 
            this.rdoJewon.AutoSize = true;
            this.rdoJewon.Checked = true;
            this.rdoJewon.Location = new System.Drawing.Point(7, 43);
            this.rdoJewon.Name = "rdoJewon";
            this.rdoJewon.Size = new System.Drawing.Size(65, 21);
            this.rdoJewon.TabIndex = 42;
            this.rdoJewon.TabStop = true;
            this.rdoJewon.Text = "재원자";
            this.rdoJewon.UseVisualStyleBackColor = true;
            // 
            // rdoRegis
            // 
            this.rdoRegis.AutoSize = true;
            this.rdoRegis.Location = new System.Drawing.Point(7, 16);
            this.rdoRegis.Name = "rdoRegis";
            this.rdoRegis.Size = new System.Drawing.Size(65, 21);
            this.rdoRegis.TabIndex = 41;
            this.rdoRegis.TabStop = true;
            this.rdoRegis.Text = "등록자";
            this.rdoRegis.UseVisualStyleBackColor = true;
            this.rdoRegis.Click += new System.EventHandler(this.rdoRegis_Click);
            // 
            // grbMsg
            // 
            this.grbMsg.Controls.Add(this.ssMsg);
            this.grbMsg.Dock = System.Windows.Forms.DockStyle.Top;
            this.grbMsg.Location = new System.Drawing.Point(353, 33);
            this.grbMsg.Name = "grbMsg";
            this.grbMsg.Size = new System.Drawing.Size(644, 131);
            this.grbMsg.TabIndex = 33;
            this.grbMsg.TabStop = false;
            this.grbMsg.Text = "과거 메세지 내역";
            // 
            // ssMsg
            // 
            this.ssMsg.AccessibleDescription = "ssMsg, Sheet1, Row 0, Column 0, ";
            this.ssMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssMsg.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssMsg.Location = new System.Drawing.Point(3, 21);
            this.ssMsg.Name = "ssMsg";
            this.ssMsg.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssMsg_Sheet1});
            this.ssMsg.Size = new System.Drawing.Size(638, 107);
            this.ssMsg.TabIndex = 1;
            this.ssMsg.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssMsg_CellClick);
            // 
            // ssMsg_Sheet1
            // 
            this.ssMsg_Sheet1.Reset();
            this.ssMsg_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssMsg_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssMsg_Sheet1.ColumnCount = 7;
            this.ssMsg_Sheet1.RowCount = 1;
            this.ssMsg_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMsg_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMsg_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssMsg_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMsg_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "등록일";
            this.ssMsg_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "삭제일";
            this.ssMsg_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "등록번호";
            this.ssMsg_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "성명";
            this.ssMsg_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "작성자";
            this.ssMsg_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "WRTNO";
            this.ssMsg_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "ROWID";
            this.ssMsg_Sheet1.Columns.Get(0).CellType = textCellType27;
            this.ssMsg_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssMsg_Sheet1.Columns.Get(0).Label = "등록일";
            this.ssMsg_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMsg_Sheet1.Columns.Get(0).Width = 120F;
            this.ssMsg_Sheet1.Columns.Get(1).CellType = textCellType28;
            this.ssMsg_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssMsg_Sheet1.Columns.Get(1).Label = "삭제일";
            this.ssMsg_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMsg_Sheet1.Columns.Get(1).Width = 120F;
            this.ssMsg_Sheet1.Columns.Get(2).CellType = textCellType29;
            this.ssMsg_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssMsg_Sheet1.Columns.Get(2).Label = "등록번호";
            this.ssMsg_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMsg_Sheet1.Columns.Get(2).Width = 110F;
            this.ssMsg_Sheet1.Columns.Get(3).CellType = textCellType30;
            this.ssMsg_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssMsg_Sheet1.Columns.Get(3).Label = "성명";
            this.ssMsg_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMsg_Sheet1.Columns.Get(3).Width = 110F;
            this.ssMsg_Sheet1.Columns.Get(4).CellType = textCellType31;
            this.ssMsg_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssMsg_Sheet1.Columns.Get(4).Label = "작성자";
            this.ssMsg_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMsg_Sheet1.Columns.Get(4).Width = 120F;
            this.ssMsg_Sheet1.Columns.Get(5).CellType = textCellType32;
            this.ssMsg_Sheet1.Columns.Get(5).Label = "WRTNO";
            this.ssMsg_Sheet1.Columns.Get(5).Visible = false;
            this.ssMsg_Sheet1.Columns.Get(6).CellType = textCellType33;
            this.ssMsg_Sheet1.Columns.Get(6).Label = "ROWID";
            this.ssMsg_Sheet1.Columns.Get(6).Visible = false;
            this.ssMsg_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMsg_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMsg_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssMsg_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMsg_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            this.ssMsg_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssMsg_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panMain
            // 
            this.panMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panMain.Controls.Add(this.txtInfo);
            this.panMain.Controls.Add(this.panel2);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(353, 164);
            this.panMain.Name = "panMain";
            this.panMain.Size = new System.Drawing.Size(644, 561);
            this.panMain.TabIndex = 34;
            // 
            // txtInfo
            // 
            this.txtInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfo.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtInfo.Location = new System.Drawing.Point(0, 33);
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.Size = new System.Drawing.Size(640, 524);
            this.txtInfo.TabIndex = 35;
            this.txtInfo.Text = "";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.btnSelect);
            this.panel2.Controls.Add(this.btnSpecial);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(640, 33);
            this.panel2.TabIndex = 36;
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.BackColor = System.Drawing.Color.Transparent;
            this.btnSelect.Location = new System.Drawing.Point(567, 2);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(72, 29);
            this.btnSelect.TabIndex = 33;
            this.btnSelect.Text = "글꼴선택";
            this.btnSelect.UseVisualStyleBackColor = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnSpecial
            // 
            this.btnSpecial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSpecial.BackColor = System.Drawing.Color.Transparent;
            this.btnSpecial.Location = new System.Drawing.Point(494, 2);
            this.btnSpecial.Name = "btnSpecial";
            this.btnSpecial.Size = new System.Drawing.Size(72, 29);
            this.btnSpecial.TabIndex = 34;
            this.btnSpecial.Text = "특수문자";
            this.btnSpecial.UseVisualStyleBackColor = false;
            this.btnSpecial.Click += new System.EventHandler(this.btnSpecial_Click);
            // 
            // fontDialog1
            // 
            this.fontDialog1.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.fontDialog1.ShowColor = true;
            // 
            // FrmOcsMsgPano_I
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(997, 725);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.grbMsg);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "FrmOcsMsgPano_I";
            this.Text = "심사과 환자메세지등록(입원환자)";
            this.Load += new System.EventHandler(this.FrmOcsMsgPano_I_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.grbSORT.ResumeLayout(false);
            this.grbSORT.PerformLayout();
            this.grbSearch.ResumeLayout(false);
            this.grbSearch.PerformLayout();
            this.grbMsg.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssMsg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMsg_Sheet1)).EndInit();
            this.panMain.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox grbSORT;
        private System.Windows.Forms.GroupBox grbSearch;
        private System.Windows.Forms.RadioButton rdoNum;
        private System.Windows.Forms.RadioButton rdoName;
        private System.Windows.Forms.RadioButton rdoDays;
        private System.Windows.Forms.RadioButton rdoJewon;
        private System.Windows.Forms.RadioButton rdoRegis;
        private System.Windows.Forms.ComboBox cboWard;
        private System.Windows.Forms.DateTimePicker dtpTDate;
        private System.Windows.Forms.DateTimePicker dtpFDate;
        private System.Windows.Forms.ComboBox cboSabun;
        private System.Windows.Forms.Label lblSabun;
        private System.Windows.Forms.Label lblNum;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtPano;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox grbMsg;
        private System.Windows.Forms.Panel panMain;
        private System.Windows.Forms.Button btnSpecial;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.RichTextBox txtInfo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.FontDialog fontDialog1;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
        private FarPoint.Win.Spread.FpSpread ssMsg;
        private FarPoint.Win.Spread.SheetView ssMsg_Sheet1;
    }
}