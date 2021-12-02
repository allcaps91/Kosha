namespace ComLibB
{
    partial class FrmOcsMsgPano_O
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType34 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType35 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType36 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType37 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType38 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType39 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType40 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType41 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType42 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType43 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType44 = new FarPoint.Win.Spread.CellType.TextCellType();
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
            this.panel4 = new System.Windows.Forms.Panel();
            this.rdoRegis = new System.Windows.Forms.RadioButton();
            this.rdoOut = new System.Windows.Forms.RadioButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rdoName = new System.Windows.Forms.RadioButton();
            this.rdoNum = new System.Windows.Forms.RadioButton();
            this.lblDay = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtPano = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblNum = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.grbMsg = new System.Windows.Forms.GroupBox();
            this.ssMsg = new FarPoint.Win.Spread.FpSpread();
            this.ssMsg_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panMain = new System.Windows.Forms.Panel();
            this.txtInfo = new System.Windows.Forms.RichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSelect = new System.Windows.Forms.Button();
            this.cboDept = new System.Windows.Forms.ComboBox();
            this.btnSpecial = new System.Windows.Forms.Button();
            this.lblDept = new System.Windows.Forms.Label();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
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
            this.panTitle.Size = new System.Drawing.Size(980, 34);
            this.panTitle.TabIndex = 13;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(904, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
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
            this.lblTitle.Size = new System.Drawing.Size(204, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "외래환자(개인별 심사과용)";
            // 
            // btnNew
            // 
            this.btnNew.BackColor = System.Drawing.Color.Transparent;
            this.btnNew.Location = new System.Drawing.Point(251, 11);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(72, 30);
            this.btnNew.TabIndex = 31;
            this.btnNew.Text = "신규등록";
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Transparent;
            this.btnDelete.Location = new System.Drawing.Point(251, 71);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(72, 30);
            this.btnDelete.TabIndex = 29;
            this.btnDelete.Text = "삭제";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Location = new System.Drawing.Point(251, 41);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 30);
            this.btnCancel.TabIndex = 30;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.Location = new System.Drawing.Point(251, 101);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 30);
            this.btnSave.TabIndex = 28;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.ssView);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.lblDay);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.txtName);
            this.panel1.Controls.Add(this.btnNew);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.txtPano);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lblNum);
            this.panel1.Controls.Add(this.dtpDate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(331, 692);
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
            this.ssView.Size = new System.Drawing.Size(327, 521);
            this.ssView.TabIndex = 57;
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
            this.ssView_Sheet1.Columns.Get(0).CellType = textCellType34;
            this.ssView_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(0).Label = "등록번호";
            this.ssView_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(0).Width = 100F;
            this.ssView_Sheet1.Columns.Get(1).CellType = textCellType35;
            this.ssView_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(1).Label = "성명";
            this.ssView_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(1).Width = 90F;
            this.ssView_Sheet1.Columns.Get(2).CellType = textCellType36;
            this.ssView_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(2).Label = "병동";
            this.ssView_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(2).Width = 80F;
            this.ssView_Sheet1.Columns.Get(3).CellType = textCellType37;
            this.ssView_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(3).Visible = false;
            this.ssView_Sheet1.Columns.Get(3).Width = 80F;
            this.ssView_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssView_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.rdoRegis);
            this.panel4.Controls.Add(this.rdoOut);
            this.panel4.Location = new System.Drawing.Point(79, 13);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(166, 29);
            this.panel4.TabIndex = 39;
            // 
            // rdoRegis
            // 
            this.rdoRegis.AutoSize = true;
            this.rdoRegis.Location = new System.Drawing.Point(5, 3);
            this.rdoRegis.Name = "rdoRegis";
            this.rdoRegis.Size = new System.Drawing.Size(65, 21);
            this.rdoRegis.TabIndex = 41;
            this.rdoRegis.Text = "등록자";
            this.rdoRegis.UseVisualStyleBackColor = true;
            this.rdoRegis.CheckedChanged += new System.EventHandler(this.rdoRegis_CheckedChanged);
            // 
            // rdoOut
            // 
            this.rdoOut.AutoSize = true;
            this.rdoOut.Checked = true;
            this.rdoOut.Location = new System.Drawing.Point(70, 3);
            this.rdoOut.Name = "rdoOut";
            this.rdoOut.Size = new System.Drawing.Size(52, 21);
            this.rdoOut.TabIndex = 42;
            this.rdoOut.TabStop = true;
            this.rdoOut.Text = "외래";
            this.rdoOut.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.rdoName);
            this.panel3.Controls.Add(this.rdoNum);
            this.panel3.Location = new System.Drawing.Point(79, 45);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(166, 26);
            this.panel3.TabIndex = 39;
            // 
            // rdoName
            // 
            this.rdoName.AutoSize = true;
            this.rdoName.Location = new System.Drawing.Point(5, 2);
            this.rdoName.Name = "rdoName";
            this.rdoName.Size = new System.Drawing.Size(65, 21);
            this.rdoName.TabIndex = 39;
            this.rdoName.Text = "성명순";
            this.rdoName.UseVisualStyleBackColor = true;
            // 
            // rdoNum
            // 
            this.rdoNum.AutoSize = true;
            this.rdoNum.Checked = true;
            this.rdoNum.Location = new System.Drawing.Point(70, 2);
            this.rdoNum.Name = "rdoNum";
            this.rdoNum.Size = new System.Drawing.Size(91, 21);
            this.rdoNum.TabIndex = 40;
            this.rdoNum.TabStop = true;
            this.rdoNum.Text = "등록번호순";
            this.rdoNum.UseVisualStyleBackColor = true;
            // 
            // lblDay
            // 
            this.lblDay.AutoSize = true;
            this.lblDay.Location = new System.Drawing.Point(10, 138);
            this.lblDay.Name = "lblDay";
            this.lblDay.Size = new System.Drawing.Size(73, 17);
            this.lblDay.TabIndex = 54;
            this.lblDay.Text = "진료일자 : ";
            this.lblDay.Visible = false;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Location = new System.Drawing.Point(251, 131);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 53;
            this.btnSearch.Text = "자료조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(115, 101);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(105, 25);
            this.txtName.TabIndex = 52;
            // 
            // txtPano
            // 
            this.txtPano.Location = new System.Drawing.Point(6, 101);
            this.txtPano.Name = "txtPano";
            this.txtPano.Size = new System.Drawing.Size(107, 25);
            this.txtPano.TabIndex = 51;
            this.txtPano.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPano_KeyDown);
            this.txtPano.Leave += new System.EventHandler(this.txtPano_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 17);
            this.label2.TabIndex = 48;
            this.label2.Text = "찾기방법 : ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 17);
            this.label1.TabIndex = 48;
            this.label1.Text = "정렬방법 : ";
            // 
            // lblNum
            // 
            this.lblNum.AutoSize = true;
            this.lblNum.Location = new System.Drawing.Point(6, 78);
            this.lblNum.Name = "lblNum";
            this.lblNum.Size = new System.Drawing.Size(73, 17);
            this.lblNum.TabIndex = 48;
            this.lblNum.Text = "등록번호 : ";
            // 
            // dtpDate
            // 
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(83, 134);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(92, 25);
            this.dtpDate.TabIndex = 46;
            this.dtpDate.Visible = false;
            // 
            // grbMsg
            // 
            this.grbMsg.Controls.Add(this.ssMsg);
            this.grbMsg.Dock = System.Windows.Forms.DockStyle.Top;
            this.grbMsg.Location = new System.Drawing.Point(331, 34);
            this.grbMsg.Name = "grbMsg";
            this.grbMsg.Size = new System.Drawing.Size(649, 133);
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
            this.ssMsg.Size = new System.Drawing.Size(643, 109);
            this.ssMsg.TabIndex = 2;
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
            this.ssMsg_Sheet1.Columns.Get(0).CellType = textCellType38;
            this.ssMsg_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssMsg_Sheet1.Columns.Get(0).Label = "등록일";
            this.ssMsg_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMsg_Sheet1.Columns.Get(0).Width = 120F;
            this.ssMsg_Sheet1.Columns.Get(1).CellType = textCellType39;
            this.ssMsg_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssMsg_Sheet1.Columns.Get(1).Label = "삭제일";
            this.ssMsg_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMsg_Sheet1.Columns.Get(1).Width = 120F;
            this.ssMsg_Sheet1.Columns.Get(2).CellType = textCellType40;
            this.ssMsg_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssMsg_Sheet1.Columns.Get(2).Label = "등록번호";
            this.ssMsg_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMsg_Sheet1.Columns.Get(2).Width = 110F;
            this.ssMsg_Sheet1.Columns.Get(3).CellType = textCellType41;
            this.ssMsg_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssMsg_Sheet1.Columns.Get(3).Label = "성명";
            this.ssMsg_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMsg_Sheet1.Columns.Get(3).Width = 110F;
            this.ssMsg_Sheet1.Columns.Get(4).CellType = textCellType42;
            this.ssMsg_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssMsg_Sheet1.Columns.Get(4).Label = "작성자";
            this.ssMsg_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMsg_Sheet1.Columns.Get(4).Width = 120F;
            this.ssMsg_Sheet1.Columns.Get(5).CellType = textCellType43;
            this.ssMsg_Sheet1.Columns.Get(5).Label = "WRTNO";
            this.ssMsg_Sheet1.Columns.Get(5).Visible = false;
            this.ssMsg_Sheet1.Columns.Get(6).CellType = textCellType44;
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
            this.panMain.Location = new System.Drawing.Point(331, 167);
            this.panMain.Name = "panMain";
            this.panMain.Size = new System.Drawing.Size(649, 559);
            this.panMain.TabIndex = 34;
            // 
            // txtInfo
            // 
            this.txtInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfo.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtInfo.Location = new System.Drawing.Point(0, 34);
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.Size = new System.Drawing.Size(645, 521);
            this.txtInfo.TabIndex = 38;
            this.txtInfo.Text = "";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnSelect);
            this.panel2.Controls.Add(this.cboDept);
            this.panel2.Controls.Add(this.btnSpecial);
            this.panel2.Controls.Add(this.lblDept);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(645, 34);
            this.panel2.TabIndex = 37;
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.BackColor = System.Drawing.Color.Transparent;
            this.btnSelect.Location = new System.Drawing.Point(79, 2);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(72, 30);
            this.btnSelect.TabIndex = 33;
            this.btnSelect.Text = "글꼴선택";
            this.btnSelect.UseVisualStyleBackColor = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // cboDept
            // 
            this.cboDept.FormattingEnabled = true;
            this.cboDept.Location = new System.Drawing.Point(217, 5);
            this.cboDept.Name = "cboDept";
            this.cboDept.Size = new System.Drawing.Size(121, 25);
            this.cboDept.TabIndex = 36;
            // 
            // btnSpecial
            // 
            this.btnSpecial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSpecial.BackColor = System.Drawing.Color.Transparent;
            this.btnSpecial.Location = new System.Drawing.Point(7, 2);
            this.btnSpecial.Name = "btnSpecial";
            this.btnSpecial.Size = new System.Drawing.Size(72, 30);
            this.btnSpecial.TabIndex = 34;
            this.btnSpecial.Text = "특수문자";
            this.btnSpecial.UseVisualStyleBackColor = false;
            this.btnSpecial.Click += new System.EventHandler(this.btnSpecial_Click);
            // 
            // lblDept
            // 
            this.lblDept.AutoSize = true;
            this.lblDept.Location = new System.Drawing.Point(157, 9);
            this.lblDept.Name = "lblDept";
            this.lblDept.Size = new System.Drawing.Size(60, 17);
            this.lblDept.TabIndex = 35;
            this.lblDept.Text = "진료과 : ";
            // 
            // fontDialog1
            // 
            this.fontDialog1.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.fontDialog1.ShowColor = true;
            // 
            // FrmOcsMsgPano_O
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(980, 726);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.grbMsg);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmOcsMsgPano_O";
            this.Text = "외래환자(개인별 심사과정)";
            this.Load += new System.EventHandler(this.FrmOcsMsgPano_O_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.grbMsg.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssMsg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMsg_Sheet1)).EndInit();
            this.panMain.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
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
        private System.Windows.Forms.RadioButton rdoNum;
        private System.Windows.Forms.RadioButton rdoName;
        private System.Windows.Forms.RadioButton rdoOut;
        private System.Windows.Forms.RadioButton rdoRegis;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label lblNum;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtPano;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox grbMsg;
        private System.Windows.Forms.Panel panMain;
        private System.Windows.Forms.Button btnSpecial;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Label lblDept;
        private System.Windows.Forms.ComboBox cboDept;
        private System.Windows.Forms.Label lblDay;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RichTextBox txtInfo;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
        private FarPoint.Win.Spread.FpSpread ssMsg;
        private FarPoint.Win.Spread.SheetView ssMsg_Sheet1;
    }
}