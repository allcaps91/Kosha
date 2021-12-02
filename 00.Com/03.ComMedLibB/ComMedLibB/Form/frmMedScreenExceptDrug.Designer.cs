namespace ComMedLibB
{
    partial class frmMedScreenExceptDrug
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType14 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType15 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType16 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType17 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType18 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType19 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType20 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType21 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType22 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType23 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType24 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType25 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType26 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ssDrugSearch = new FarPoint.Win.Spread.FpSpread();
            this.ssDrugSearch_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.rdoEdiCode = new System.Windows.Forms.RadioButton();
            this.rdoHcode = new System.Windows.Forms.RadioButton();
            this.rdoJeyaksa = new System.Windows.Forms.RadioButton();
            this.rdoKorName = new System.Windows.Forms.RadioButton();
            this.rdoEnglengName = new System.Windows.Forms.RadioButton();
            this.rdoEngName = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblModuleNM = new System.Windows.Forms.Label();
            this.btnRowDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ssExceptDrug = new FarPoint.Win.Spread.FpSpread();
            this.ssExceptDrug_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.collapsibleSplitContainer1 = new DevComponents.DotNetBar.Controls.CollapsibleSplitContainer();
            this.panTitle.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssDrugSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssDrugSearch_Sheet1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssExceptDrug)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssExceptDrug_Sheet1)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.collapsibleSplitContainer1)).BeginInit();
            this.collapsibleSplitContainer1.Panel1.SuspendLayout();
            this.collapsibleSplitContainer1.Panel2.SuspendLayout();
            this.collapsibleSplitContainer1.SuspendLayout();
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
            this.panTitle.Size = new System.Drawing.Size(1160, 34);
            this.panTitle.TabIndex = 15;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(1065, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(91, 30);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(106, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "제외약품등록";
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(1160, 29);
            this.panTitleSub0.TabIndex = 20;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 4);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(155, 15);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "제외 약품 등록 및 약품검색";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 63);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1160, 84);
            this.panel1.TabIndex = 21;
            // 
            // ssDrugSearch
            // 
            this.ssDrugSearch.AccessibleDescription = "";
            this.ssDrugSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssDrugSearch.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssDrugSearch.Location = new System.Drawing.Point(0, 0);
            this.ssDrugSearch.Name = "ssDrugSearch";
            this.ssDrugSearch.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssDrugSearch_Sheet1});
            this.ssDrugSearch.Size = new System.Drawing.Size(1160, 160);
            this.ssDrugSearch.TabIndex = 1;
            this.ssDrugSearch.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssDrugSearch.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssDrugSearch_CellDoubleClick);
            // 
            // ssDrugSearch_Sheet1
            // 
            this.ssDrugSearch_Sheet1.Reset();
            this.ssDrugSearch_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssDrugSearch_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssDrugSearch_Sheet1.ColumnCount = 6;
            this.ssDrugSearch_Sheet1.RowCount = 1;
            this.ssDrugSearch_Sheet1.ColumnHeader.Cells.Get(0, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssDrugSearch_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "급여구분";
            this.ssDrugSearch_Sheet1.ColumnHeader.Cells.Get(0, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssDrugSearch_Sheet1.ColumnHeader.Cells.Get(0, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssDrugSearch_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "약품명";
            this.ssDrugSearch_Sheet1.ColumnHeader.Cells.Get(0, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssDrugSearch_Sheet1.ColumnHeader.Cells.Get(0, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssDrugSearch_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "약품코드";
            this.ssDrugSearch_Sheet1.ColumnHeader.Cells.Get(0, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssDrugSearch_Sheet1.ColumnHeader.Cells.Get(0, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssDrugSearch_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "영문상품명";
            this.ssDrugSearch_Sheet1.ColumnHeader.Cells.Get(0, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssDrugSearch_Sheet1.ColumnHeader.Cells.Get(0, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssDrugSearch_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "영문성분명";
            this.ssDrugSearch_Sheet1.ColumnHeader.Cells.Get(0, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssDrugSearch_Sheet1.ColumnHeader.Cells.Get(0, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssDrugSearch_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "제조회사";
            this.ssDrugSearch_Sheet1.ColumnHeader.Cells.Get(0, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssDrugSearch_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssDrugSearch_Sheet1.Columns.Get(0).CellType = textCellType14;
            this.ssDrugSearch_Sheet1.Columns.Get(0).Label = "급여구분";
            this.ssDrugSearch_Sheet1.Columns.Get(0).Locked = true;
            this.ssDrugSearch_Sheet1.Columns.Get(0).Width = 77F;
            this.ssDrugSearch_Sheet1.Columns.Get(1).CellType = textCellType15;
            this.ssDrugSearch_Sheet1.Columns.Get(1).Label = "약품명";
            this.ssDrugSearch_Sheet1.Columns.Get(1).Locked = true;
            this.ssDrugSearch_Sheet1.Columns.Get(1).Width = 204F;
            this.ssDrugSearch_Sheet1.Columns.Get(2).CellType = textCellType16;
            this.ssDrugSearch_Sheet1.Columns.Get(2).Label = "약품코드";
            this.ssDrugSearch_Sheet1.Columns.Get(2).Locked = true;
            this.ssDrugSearch_Sheet1.Columns.Get(2).Width = 97F;
            this.ssDrugSearch_Sheet1.Columns.Get(3).CellType = textCellType17;
            this.ssDrugSearch_Sheet1.Columns.Get(3).Label = "영문상품명";
            this.ssDrugSearch_Sheet1.Columns.Get(3).Locked = true;
            this.ssDrugSearch_Sheet1.Columns.Get(3).Width = 236F;
            this.ssDrugSearch_Sheet1.Columns.Get(4).CellType = textCellType18;
            this.ssDrugSearch_Sheet1.Columns.Get(4).Label = "영문성분명";
            this.ssDrugSearch_Sheet1.Columns.Get(4).Locked = true;
            this.ssDrugSearch_Sheet1.Columns.Get(4).Width = 178F;
            this.ssDrugSearch_Sheet1.Columns.Get(5).CellType = textCellType19;
            this.ssDrugSearch_Sheet1.Columns.Get(5).Label = "제조회사";
            this.ssDrugSearch_Sheet1.Columns.Get(5).Locked = true;
            this.ssDrugSearch_Sheet1.Columns.Get(5).Width = 153F;
            this.ssDrugSearch_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            this.ssDrugSearch_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssDrugSearch_Sheet1.RowHeader.Visible = false;
            this.ssDrugSearch_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkAll);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtSearch);
            this.groupBox1.Controls.Add(this.rdoEdiCode);
            this.groupBox1.Controls.Add(this.rdoHcode);
            this.groupBox1.Controls.Add(this.rdoJeyaksa);
            this.groupBox1.Controls.Add(this.rdoKorName);
            this.groupBox1.Controls.Add(this.rdoEnglengName);
            this.groupBox1.Controls.Add(this.rdoEngName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1160, 84);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "약품검색";
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Location = new System.Drawing.Point(811, 25);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(112, 23);
            this.chkAll.TabIndex = 10;
            this.chkAll.Text = "전체약품검색";
            this.chkAll.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(714, 20);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(79, 30);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(450, 24);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(258, 25);
            this.txtSearch.TabIndex = 7;
            this.txtSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSearch_KeyPress);
            // 
            // rdoEdiCode
            // 
            this.rdoEdiCode.AutoSize = true;
            this.rdoEdiCode.Location = new System.Drawing.Point(328, 49);
            this.rdoEdiCode.Name = "rdoEdiCode";
            this.rdoEdiCode.Size = new System.Drawing.Size(76, 23);
            this.rdoEdiCode.TabIndex = 6;
            this.rdoEdiCode.Text = "EDI코드";
            this.rdoEdiCode.UseVisualStyleBackColor = true;
            // 
            // rdoHcode
            // 
            this.rdoHcode.AutoSize = true;
            this.rdoHcode.Location = new System.Drawing.Point(214, 49);
            this.rdoHcode.Name = "rdoHcode";
            this.rdoHcode.Size = new System.Drawing.Size(83, 23);
            this.rdoHcode.TabIndex = 5;
            this.rdoHcode.Text = "병원코드";
            this.rdoHcode.UseVisualStyleBackColor = true;
            // 
            // rdoJeyaksa
            // 
            this.rdoJeyaksa.AutoSize = true;
            this.rdoJeyaksa.Location = new System.Drawing.Point(100, 49);
            this.rdoJeyaksa.Name = "rdoJeyaksa";
            this.rdoJeyaksa.Size = new System.Drawing.Size(97, 23);
            this.rdoJeyaksa.TabIndex = 4;
            this.rdoJeyaksa.Text = "제조회사명";
            this.rdoJeyaksa.UseVisualStyleBackColor = true;
            // 
            // rdoKorName
            // 
            this.rdoKorName.AutoSize = true;
            this.rdoKorName.Checked = true;
            this.rdoKorName.Location = new System.Drawing.Point(328, 21);
            this.rdoKorName.Name = "rdoKorName";
            this.rdoKorName.Size = new System.Drawing.Size(97, 23);
            this.rdoKorName.TabIndex = 3;
            this.rdoKorName.TabStop = true;
            this.rdoKorName.Text = "한글상품명";
            this.rdoKorName.UseVisualStyleBackColor = true;
            // 
            // rdoEnglengName
            // 
            this.rdoEnglengName.AutoSize = true;
            this.rdoEnglengName.Location = new System.Drawing.Point(214, 21);
            this.rdoEnglengName.Name = "rdoEnglengName";
            this.rdoEnglengName.Size = new System.Drawing.Size(97, 23);
            this.rdoEnglengName.TabIndex = 2;
            this.rdoEnglengName.Text = "영문성분명";
            this.rdoEnglengName.UseVisualStyleBackColor = true;
            // 
            // rdoEngName
            // 
            this.rdoEngName.AutoSize = true;
            this.rdoEngName.Location = new System.Drawing.Point(100, 21);
            this.rdoEngName.Name = "rdoEngName";
            this.rdoEngName.Size = new System.Drawing.Size(97, 23);
            this.rdoEngName.TabIndex = 1;
            this.rdoEngName.Text = "영문상품명";
            this.rdoEngName.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(25, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "조회구분:";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.lblModuleNM);
            this.panel2.Controls.Add(this.btnRowDelete);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1160, 29);
            this.panel2.TabIndex = 22;
            // 
            // lblModuleNM
            // 
            this.lblModuleNM.AutoSize = true;
            this.lblModuleNM.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblModuleNM.ForeColor = System.Drawing.Color.White;
            this.lblModuleNM.Location = new System.Drawing.Point(76, 4);
            this.lblModuleNM.Name = "lblModuleNM";
            this.lblModuleNM.Size = new System.Drawing.Size(43, 15);
            this.lblModuleNM.TabIndex = 8;
            this.lblModuleNM.Text = "모듈명";
            // 
            // btnRowDelete
            // 
            this.btnRowDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnRowDelete.Location = new System.Drawing.Point(974, 0);
            this.btnRowDelete.Name = "btnRowDelete";
            this.btnRowDelete.Size = new System.Drawing.Size(91, 25);
            this.btnRowDelete.TabIndex = 7;
            this.btnRowDelete.Text = "행삭제";
            this.btnRowDelete.UseVisualStyleBackColor = true;
            this.btnRowDelete.Click += new System.EventHandler(this.btnRowDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(1065, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(91, 25);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(8, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "검토모듈명:";
            // 
            // ssExceptDrug
            // 
            this.ssExceptDrug.AccessibleDescription = "ssExceptDrug, Sheet1, Row 0, Column 0, ";
            this.ssExceptDrug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssExceptDrug.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssExceptDrug.Location = new System.Drawing.Point(0, 29);
            this.ssExceptDrug.Name = "ssExceptDrug";
            this.ssExceptDrug.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssExceptDrug_Sheet1});
            this.ssExceptDrug.Size = new System.Drawing.Size(1160, 254);
            this.ssExceptDrug.TabIndex = 23;
            this.ssExceptDrug.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // ssExceptDrug_Sheet1
            // 
            this.ssExceptDrug_Sheet1.Reset();
            this.ssExceptDrug_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssExceptDrug_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssExceptDrug_Sheet1.ColumnCount = 7;
            this.ssExceptDrug_Sheet1.RowCount = 1;
            this.ssExceptDrug_Sheet1.Cells.Get(0, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Distributed;
            this.ssExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "MODULEID";
            this.ssExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "상태";
            this.ssExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "약품명";
            this.ssExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "약품코드";
            this.ssExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "성분명";
            this.ssExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "영문상품명";
            this.ssExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "등록일자";
            this.ssExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssExceptDrug_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssExceptDrug_Sheet1.Columns.Get(0).CellType = textCellType20;
            this.ssExceptDrug_Sheet1.Columns.Get(0).Label = "MODULEID";
            this.ssExceptDrug_Sheet1.Columns.Get(0).Locked = true;
            this.ssExceptDrug_Sheet1.Columns.Get(0).Width = 90F;
            this.ssExceptDrug_Sheet1.Columns.Get(1).CellType = textCellType21;
            this.ssExceptDrug_Sheet1.Columns.Get(1).Label = "상태";
            this.ssExceptDrug_Sheet1.Columns.Get(1).Locked = true;
            this.ssExceptDrug_Sheet1.Columns.Get(1).Width = 86F;
            this.ssExceptDrug_Sheet1.Columns.Get(2).CellType = textCellType22;
            this.ssExceptDrug_Sheet1.Columns.Get(2).Label = "약품명";
            this.ssExceptDrug_Sheet1.Columns.Get(2).Locked = true;
            this.ssExceptDrug_Sheet1.Columns.Get(2).Width = 255F;
            this.ssExceptDrug_Sheet1.Columns.Get(3).CellType = textCellType23;
            this.ssExceptDrug_Sheet1.Columns.Get(3).Label = "약품코드";
            this.ssExceptDrug_Sheet1.Columns.Get(3).Locked = true;
            this.ssExceptDrug_Sheet1.Columns.Get(3).Width = 138F;
            this.ssExceptDrug_Sheet1.Columns.Get(4).CellType = textCellType24;
            this.ssExceptDrug_Sheet1.Columns.Get(4).Label = "성분명";
            this.ssExceptDrug_Sheet1.Columns.Get(4).Locked = true;
            this.ssExceptDrug_Sheet1.Columns.Get(4).Width = 247F;
            this.ssExceptDrug_Sheet1.Columns.Get(5).CellType = textCellType25;
            this.ssExceptDrug_Sheet1.Columns.Get(5).Label = "영문상품명";
            this.ssExceptDrug_Sheet1.Columns.Get(5).Locked = true;
            this.ssExceptDrug_Sheet1.Columns.Get(5).Visible = false;
            this.ssExceptDrug_Sheet1.Columns.Get(5).Width = 178F;
            this.ssExceptDrug_Sheet1.Columns.Get(6).CellType = textCellType26;
            this.ssExceptDrug_Sheet1.Columns.Get(6).Label = "등록일자";
            this.ssExceptDrug_Sheet1.Columns.Get(6).Locked = true;
            this.ssExceptDrug_Sheet1.Columns.Get(6).Width = 212F;
            this.ssExceptDrug_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            this.ssExceptDrug_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssExceptDrug_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 253);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1160, 30);
            this.panel3.TabIndex = 24;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(1160, 30);
            this.label3.TabIndex = 0;
            this.label3.Text = "※SUPER USER가 지정한 제외 불가 약품은 DB에 저장되지 않습니다.";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Visible = false;
            // 
            // collapsibleSplitContainer1
            // 
            this.collapsibleSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.collapsibleSplitContainer1.Location = new System.Drawing.Point(0, 147);
            this.collapsibleSplitContainer1.Name = "collapsibleSplitContainer1";
            this.collapsibleSplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // collapsibleSplitContainer1.Panel1
            // 
            this.collapsibleSplitContainer1.Panel1.Controls.Add(this.ssDrugSearch);
            // 
            // collapsibleSplitContainer1.Panel2
            // 
            this.collapsibleSplitContainer1.Panel2.Controls.Add(this.panel3);
            this.collapsibleSplitContainer1.Panel2.Controls.Add(this.ssExceptDrug);
            this.collapsibleSplitContainer1.Panel2.Controls.Add(this.panel2);
            this.collapsibleSplitContainer1.Size = new System.Drawing.Size(1160, 463);
            this.collapsibleSplitContainer1.SplitterDistance = 160;
            this.collapsibleSplitContainer1.SplitterWidth = 20;
            this.collapsibleSplitContainer1.TabIndex = 6;
            // 
            // frmMedScreenExceptDrug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1160, 610);
            this.Controls.Add(this.collapsibleSplitContainer1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmMedScreenExceptDrug";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmMedScreenExceptDrug";
            this.Load += new System.EventHandler(this.frmMedScreenExceptDrug_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssDrugSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssDrugSearch_Sheet1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssExceptDrug)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssExceptDrug_Sheet1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.collapsibleSplitContainer1.Panel1.ResumeLayout(false);
            this.collapsibleSplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.collapsibleSplitContainer1)).EndInit();
            this.collapsibleSplitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panel1;
        private FarPoint.Win.Spread.FpSpread ssDrugSearch;
        private FarPoint.Win.Spread.SheetView ssDrugSearch_Sheet1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.RadioButton rdoEdiCode;
        private System.Windows.Forms.RadioButton rdoHcode;
        private System.Windows.Forms.RadioButton rdoJeyaksa;
        private System.Windows.Forms.RadioButton rdoKorName;
        private System.Windows.Forms.RadioButton rdoEnglengName;
        private System.Windows.Forms.RadioButton rdoEngName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnRowDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label2;
        private FarPoint.Win.Spread.FpSpread ssExceptDrug;
        private FarPoint.Win.Spread.SheetView ssExceptDrug_Sheet1;
        private System.Windows.Forms.Label lblModuleNM;
        private System.Windows.Forms.CheckBox chkAll;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private DevComponents.DotNetBar.Controls.CollapsibleSplitContainer collapsibleSplitContainer1;
    }
}