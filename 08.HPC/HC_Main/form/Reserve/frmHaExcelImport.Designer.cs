namespace HC_Main
{
    partial class frmHaExcelImport
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color351637250470935179578", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text429637250470935199530", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Text553637250470935209501");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.Spread.FlatFocusIndicatorRenderer flatFocusIndicatorRenderer1 = new FarPoint.Win.Spread.FlatFocusIndicatorRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer1 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer2 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnConvert = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblFormTitle = new System.Windows.Forms.Label();
            this.panSub01 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnDialog = new System.Windows.Forms.Button();
            this.txtFile1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cboExcelType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnLtdHelp = new System.Windows.Forms.Button();
            this.txtLtdName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cboYYMM = new System.Windows.Forms.ComboBox();
            this.lblJONG1 = new System.Windows.Forms.Label();
            this.panExcel = new System.Windows.Forms.Panel();
            this.SS1 = new FarPoint.Win.Spread.FpSpread();
            this.SS1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.label8 = new System.Windows.Forms.Label();
            this.panPrg = new System.Windows.Forms.Panel();
            this.prgBar = new System.Windows.Forms.ProgressBar();
            this.lblMsg = new System.Windows.Forms.Label();
            this.panSub02 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnSave2 = new System.Windows.Forms.Button();
            this.panel7 = new System.Windows.Forms.Panel();
            this.txtBirth = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.txtSName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.cboLtd = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panMain = new System.Windows.Forms.Panel();
            this.SS2 = new FarPoint.Win.Spread.FpSpread();
            this.SS2_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.label9 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panTitle.SuspendLayout();
            this.panSub01.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panExcel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).BeginInit();
            this.panPrg.SuspendLayout();
            this.panSub02.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS2_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panTitle.Controls.Add(this.btnConvert);
            this.panTitle.Controls.Add(this.btnSave);
            this.panTitle.Controls.Add(this.label3);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblFormTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1264, 38);
            this.panTitle.TabIndex = 11;
            // 
            // btnConvert
            // 
            this.btnConvert.BackColor = System.Drawing.Color.White;
            this.btnConvert.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnConvert.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnConvert.Location = new System.Drawing.Point(689, 0);
            this.btnConvert.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(139, 36);
            this.btnConvert.TabIndex = 167;
            this.btnConvert.Text = "DB형식으로 변환";
            this.btnConvert.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave.Location = new System.Drawing.Point(828, 0);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(108, 36);
            this.btnSave.TabIndex = 166;
            this.btnSave.Text = "DB로 저장(&S)";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            this.label3.AutoEllipsis = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Dock = System.Windows.Forms.DockStyle.Right;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(936, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(246, 36);
            this.label3.TabIndex = 165;
            this.label3.Text = "1. 엑셀파일 유형을 선택   2. 엑셀파일 읽기\r\n3. [DB형식으로 변환]       4. [DB에 저장]\r\n";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(1182, 0);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(80, 36);
            this.btnExit.TabIndex = 9;
            this.btnExit.Text = "닫기(&X)";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.AutoSize = true;
            this.lblFormTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblFormTitle.Location = new System.Drawing.Point(7, 8);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(224, 21);
            this.lblFormTitle.TabIndex = 7;
            this.lblFormTitle.Text = "종검예약 EXCEL 파일 DB저장";
            // 
            // panSub01
            // 
            this.panSub01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSub01.Controls.Add(this.panel4);
            this.panSub01.Controls.Add(this.label2);
            this.panSub01.Controls.Add(this.panel3);
            this.panSub01.Controls.Add(this.label1);
            this.panSub01.Controls.Add(this.panel2);
            this.panSub01.Controls.Add(this.label4);
            this.panSub01.Controls.Add(this.panel1);
            this.panSub01.Controls.Add(this.lblJONG1);
            this.panSub01.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSub01.Location = new System.Drawing.Point(0, 38);
            this.panSub01.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panSub01.Name = "panSub01";
            this.panSub01.Size = new System.Drawing.Size(1264, 34);
            this.panSub01.TabIndex = 12;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.btnImport);
            this.panel4.Controls.Add(this.btnDialog);
            this.panel4.Controls.Add(this.txtFile1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(828, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(434, 32);
            this.panel4.TabIndex = 165;
            // 
            // btnImport
            // 
            this.btnImport.BackColor = System.Drawing.Color.White;
            this.btnImport.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnImport.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnImport.Location = new System.Drawing.Point(306, 0);
            this.btnImport.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(126, 30);
            this.btnImport.TabIndex = 164;
            this.btnImport.Text = "엑셀파일읽기";
            this.btnImport.UseVisualStyleBackColor = false;
            // 
            // btnDialog
            // 
            this.btnDialog.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDialog.Location = new System.Drawing.Point(275, 3);
            this.btnDialog.Name = "btnDialog";
            this.btnDialog.Size = new System.Drawing.Size(28, 25);
            this.btnDialog.TabIndex = 163;
            this.btnDialog.Text = "...";
            this.btnDialog.UseVisualStyleBackColor = true;
            // 
            // txtFile1
            // 
            this.txtFile1.Location = new System.Drawing.Point(5, 3);
            this.txtFile1.MaxLength = 0;
            this.txtFile1.Name = "txtFile1";
            this.txtFile1.Size = new System.Drawing.Size(267, 25);
            this.txtFile1.TabIndex = 162;
            this.txtFile1.Tag = "";
            // 
            // label2
            // 
            this.label2.AutoEllipsis = true;
            this.label2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(728, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 32);
            this.label2.TabIndex = 164;
            this.label2.Text = "엑셀파일 경로";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.cboExcelType);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(559, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(169, 32);
            this.panel3.TabIndex = 163;
            // 
            // cboExcelType
            // 
            this.cboExcelType.FormattingEnabled = true;
            this.cboExcelType.Location = new System.Drawing.Point(3, 2);
            this.cboExcelType.Name = "cboExcelType";
            this.cboExcelType.Size = new System.Drawing.Size(161, 25);
            this.cboExcelType.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoEllipsis = true;
            this.label1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(459, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 32);
            this.label1.TabIndex = 162;
            this.label1.Text = "엑셀파일 유형";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnLtdHelp);
            this.panel2.Controls.Add(this.txtLtdName);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(277, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(182, 32);
            this.panel2.TabIndex = 161;
            // 
            // btnLtdHelp
            // 
            this.btnLtdHelp.Image = global::HC_Main.Properties.Resources.find;
            this.btnLtdHelp.Location = new System.Drawing.Point(148, 1);
            this.btnLtdHelp.Name = "btnLtdHelp";
            this.btnLtdHelp.Size = new System.Drawing.Size(30, 28);
            this.btnLtdHelp.TabIndex = 162;
            this.btnLtdHelp.UseVisualStyleBackColor = true;
            // 
            // txtLtdName
            // 
            this.txtLtdName.Location = new System.Drawing.Point(3, 2);
            this.txtLtdName.MaxLength = 6;
            this.txtLtdName.Name = "txtLtdName";
            this.txtLtdName.Size = new System.Drawing.Size(143, 25);
            this.txtLtdName.TabIndex = 161;
            this.txtLtdName.Tag = "";
            // 
            // label4
            // 
            this.label4.AutoEllipsis = true;
            this.label4.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(185, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 32);
            this.label4.TabIndex = 159;
            this.label4.Text = "사업장명";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.cboYYMM);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(89, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(96, 32);
            this.panel1.TabIndex = 157;
            // 
            // cboYYMM
            // 
            this.cboYYMM.FormattingEnabled = true;
            this.cboYYMM.Location = new System.Drawing.Point(5, 3);
            this.cboYYMM.Name = "cboYYMM";
            this.cboYYMM.Size = new System.Drawing.Size(85, 25);
            this.cboYYMM.TabIndex = 1;
            // 
            // lblJONG1
            // 
            this.lblJONG1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lblJONG1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblJONG1.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblJONG1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblJONG1.Location = new System.Drawing.Point(0, 0);
            this.lblJONG1.Name = "lblJONG1";
            this.lblJONG1.Size = new System.Drawing.Size(89, 32);
            this.lblJONG1.TabIndex = 156;
            this.lblJONG1.Text = "검진연도";
            this.lblJONG1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panExcel
            // 
            this.panExcel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panExcel.Controls.Add(this.SS1);
            this.panExcel.Controls.Add(this.label8);
            this.panExcel.Controls.Add(this.panPrg);
            this.panExcel.Dock = System.Windows.Forms.DockStyle.Top;
            this.panExcel.Location = new System.Drawing.Point(0, 72);
            this.panExcel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panExcel.Name = "panExcel";
            this.panExcel.Size = new System.Drawing.Size(1264, 295);
            this.panExcel.TabIndex = 13;
            // 
            // SS1
            // 
            this.SS1.AccessibleDescription = "SS1, SS1, Row 0, Column 0, 1";
            this.SS1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SS1.Location = new System.Drawing.Point(0, 22);
            this.SS1.Name = "SS1";
            namedStyle1.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType1.MaxLength = 32000;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType2.MaxLength = 32000;
            namedStyle3.CellType = textCellType2;
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.Renderer = textCellType2;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3});
            this.SS1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS1_Sheet1});
            this.SS1.Size = new System.Drawing.Size(1262, 249);
            this.SS1.TabIndex = 158;
            this.SS1.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.SS1.TextTipAppearance = tipAppearance1;
            this.SS1.SetViewportLeftColumn(0, 0, 3);
            this.SS1.SetActiveViewport(0, 0, -1);
            // 
            // SS1_Sheet1
            // 
            this.SS1_Sheet1.Reset();
            this.SS1_Sheet1.SheetName = "SS1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS1_Sheet1.ColumnCount = 21;
            this.SS1_Sheet1.RowCount = 5;
            this.SS1_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "부서명";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "사번";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "직원성명";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "직원주민등록";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "직원연락처";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "가족명";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "관계";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "가족주민등록";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "가족연락처";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "본인희망일";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "가족희망일";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "본인유형";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "가족유형";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "병원";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "공단검진";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 15).Value = "특수검진항목";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 16).Value = "직원 회사부담 추가검사";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 17).Value = "직원 본인부담 추가검사";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 18).Value = "가족 회사부담 추가검사";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 19).Value = "가족 본인부담 추가검사";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 20).Value = "참고사항";
            this.SS1_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SS1_Sheet1.ColumnHeader.Rows.Get(0).Height = 29F;
            this.SS1_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SS1_Sheet1.Columns.Get(0).Label = "부서명";
            this.SS1_Sheet1.Columns.Get(0).StyleName = "Text553637250470935209501";
            this.SS1_Sheet1.Columns.Get(0).Width = 93F;
            this.SS1_Sheet1.Columns.Get(1).Label = "사번";
            this.SS1_Sheet1.Columns.Get(1).StyleName = "Text553637250470935209501";
            this.SS1_Sheet1.Columns.Get(2).Label = "직원성명";
            this.SS1_Sheet1.Columns.Get(2).StyleName = "Text553637250470935209501";
            this.SS1_Sheet1.Columns.Get(2).Width = 66F;
            this.SS1_Sheet1.Columns.Get(3).Label = "직원주민등록";
            this.SS1_Sheet1.Columns.Get(3).StyleName = "Text553637250470935209501";
            this.SS1_Sheet1.Columns.Get(3).Width = 94F;
            this.SS1_Sheet1.Columns.Get(4).Label = "직원연락처";
            this.SS1_Sheet1.Columns.Get(4).StyleName = "Text553637250470935209501";
            this.SS1_Sheet1.Columns.Get(4).Width = 87F;
            this.SS1_Sheet1.Columns.Get(5).Label = "가족명";
            this.SS1_Sheet1.Columns.Get(5).Width = 59F;
            this.SS1_Sheet1.Columns.Get(6).Label = "관계";
            this.SS1_Sheet1.Columns.Get(6).Width = 49F;
            this.SS1_Sheet1.Columns.Get(7).Label = "가족주민등록";
            this.SS1_Sheet1.Columns.Get(7).Width = 92F;
            this.SS1_Sheet1.Columns.Get(8).Label = "가족연락처";
            this.SS1_Sheet1.Columns.Get(8).Width = 83F;
            this.SS1_Sheet1.Columns.Get(9).Label = "본인희망일";
            this.SS1_Sheet1.Columns.Get(9).Width = 75F;
            this.SS1_Sheet1.Columns.Get(10).Label = "가족희망일";
            this.SS1_Sheet1.Columns.Get(10).Width = 74F;
            this.SS1_Sheet1.Columns.Get(11).Label = "본인유형";
            this.SS1_Sheet1.Columns.Get(11).StyleName = "Text553637250470935209501";
            this.SS1_Sheet1.Columns.Get(11).Width = 73F;
            this.SS1_Sheet1.Columns.Get(12).Label = "가족유형";
            this.SS1_Sheet1.Columns.Get(12).StyleName = "Text553637250470935209501";
            this.SS1_Sheet1.Columns.Get(12).Width = 67F;
            this.SS1_Sheet1.Columns.Get(13).Label = "병원";
            this.SS1_Sheet1.Columns.Get(13).StyleName = "Text553637250470935209501";
            this.SS1_Sheet1.Columns.Get(13).Width = 49F;
            this.SS1_Sheet1.Columns.Get(14).Label = "공단검진";
            this.SS1_Sheet1.Columns.Get(14).StyleName = "Text553637250470935209501";
            this.SS1_Sheet1.Columns.Get(14).Width = 62F;
            this.SS1_Sheet1.Columns.Get(15).Label = "특수검진항목";
            this.SS1_Sheet1.Columns.Get(15).StyleName = "Text553637250470935209501";
            this.SS1_Sheet1.Columns.Get(15).Width = 120F;
            this.SS1_Sheet1.Columns.Get(16).Label = "직원 회사부담 추가검사";
            this.SS1_Sheet1.Columns.Get(16).StyleName = "Text553637250470935209501";
            this.SS1_Sheet1.Columns.Get(16).Width = 150F;
            this.SS1_Sheet1.Columns.Get(17).Label = "직원 본인부담 추가검사";
            this.SS1_Sheet1.Columns.Get(17).StyleName = "Text553637250470935209501";
            this.SS1_Sheet1.Columns.Get(17).Width = 156F;
            this.SS1_Sheet1.Columns.Get(18).Label = "가족 회사부담 추가검사";
            this.SS1_Sheet1.Columns.Get(18).StyleName = "Text553637250470935209501";
            this.SS1_Sheet1.Columns.Get(18).Width = 154F;
            this.SS1_Sheet1.Columns.Get(19).Label = "가족 본인부담 추가검사";
            this.SS1_Sheet1.Columns.Get(19).StyleName = "Text553637250470935209501";
            this.SS1_Sheet1.Columns.Get(19).Width = 150F;
            this.SS1_Sheet1.Columns.Get(20).Label = "참고사항";
            this.SS1_Sheet1.Columns.Get(20).StyleName = "Text553637250470935209501";
            this.SS1_Sheet1.Columns.Get(20).Width = 228F;
            this.SS1_Sheet1.DefaultStyleName = "Text429637250470935199530";
            this.SS1_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SS1_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.FrozenColumnCount = 3;
            this.SS1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.LightGray;
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Dock = System.Windows.Forms.DockStyle.Top;
            this.label8.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(0, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(1262, 22);
            this.label8.TabIndex = 157;
            this.label8.Text = "  <Excel File>";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panPrg
            // 
            this.panPrg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panPrg.Controls.Add(this.prgBar);
            this.panPrg.Controls.Add(this.lblMsg);
            this.panPrg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panPrg.Location = new System.Drawing.Point(0, 271);
            this.panPrg.Name = "panPrg";
            this.panPrg.Size = new System.Drawing.Size(1262, 22);
            this.panPrg.TabIndex = 72;
            // 
            // prgBar
            // 
            this.prgBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prgBar.Location = new System.Drawing.Point(158, 0);
            this.prgBar.Name = "prgBar";
            this.prgBar.Size = new System.Drawing.Size(1102, 20);
            this.prgBar.TabIndex = 160;
            this.prgBar.Value = 50;
            // 
            // lblMsg
            // 
            this.lblMsg.BackColor = System.Drawing.Color.LightGray;
            this.lblMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMsg.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblMsg.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblMsg.Location = new System.Drawing.Point(0, 0);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(158, 20);
            this.lblMsg.TabIndex = 159;
            this.lblMsg.Text = " 주민번호 점검중 ...";
            this.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panSub02
            // 
            this.panSub02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSub02.Controls.Add(this.btnSearch);
            this.panSub02.Controls.Add(this.btnSave2);
            this.panSub02.Controls.Add(this.panel7);
            this.panSub02.Controls.Add(this.label7);
            this.panSub02.Controls.Add(this.panel6);
            this.panSub02.Controls.Add(this.label6);
            this.panSub02.Controls.Add(this.panel5);
            this.panSub02.Controls.Add(this.label5);
            this.panSub02.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSub02.Location = new System.Drawing.Point(0, 367);
            this.panSub02.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panSub02.Name = "panSub02";
            this.panSub02.Size = new System.Drawing.Size(1264, 36);
            this.panSub02.TabIndex = 14;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(1038, 0);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(112, 34);
            this.btnSearch.TabIndex = 169;
            this.btnSearch.Text = "조회(&V)";
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // btnSave2
            // 
            this.btnSave2.BackColor = System.Drawing.Color.White;
            this.btnSave2.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave2.Location = new System.Drawing.Point(1150, 0);
            this.btnSave2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave2.Name = "btnSave2";
            this.btnSave2.Size = new System.Drawing.Size(112, 34);
            this.btnSave2.TabIndex = 168;
            this.btnSave2.Text = "명단저장";
            this.btnSave2.UseVisualStyleBackColor = false;
            // 
            // panel7
            // 
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.txtBirth);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel7.Location = new System.Drawing.Point(577, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(90, 34);
            this.panel7.TabIndex = 163;
            // 
            // txtBirth
            // 
            this.txtBirth.Location = new System.Drawing.Point(5, 4);
            this.txtBirth.MaxLength = 6;
            this.txtBirth.Name = "txtBirth";
            this.txtBirth.Size = new System.Drawing.Size(79, 25);
            this.txtBirth.TabIndex = 162;
            this.txtBirth.Tag = "";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Dock = System.Windows.Forms.DockStyle.Left;
            this.label7.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(504, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 34);
            this.label7.TabIndex = 162;
            this.label7.Text = "생년월일";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.txtSName);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel6.Location = new System.Drawing.Point(348, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(156, 34);
            this.panel6.TabIndex = 161;
            // 
            // txtSName
            // 
            this.txtSName.Location = new System.Drawing.Point(5, 4);
            this.txtSName.MaxLength = 6;
            this.txtSName.Name = "txtSName";
            this.txtSName.Size = new System.Drawing.Size(143, 25);
            this.txtSName.TabIndex = 162;
            this.txtSName.Tag = "";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Dock = System.Windows.Forms.DockStyle.Left;
            this.label6.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(292, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 34);
            this.label6.TabIndex = 160;
            this.label6.Text = "성명";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.cboLtd);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel5.Location = new System.Drawing.Point(89, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(203, 34);
            this.panel5.TabIndex = 159;
            // 
            // cboLtd
            // 
            this.cboLtd.FormattingEnabled = true;
            this.cboLtd.Location = new System.Drawing.Point(5, 3);
            this.cboLtd.Name = "cboLtd";
            this.cboLtd.Size = new System.Drawing.Size(191, 25);
            this.cboLtd.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Dock = System.Windows.Forms.DockStyle.Left;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 34);
            this.label5.TabIndex = 158;
            this.label5.Text = "검색할회사";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panMain
            // 
            this.panMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panMain.Controls.Add(this.SS2);
            this.panMain.Controls.Add(this.label9);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(0, 403);
            this.panMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panMain.Name = "panMain";
            this.panMain.Size = new System.Drawing.Size(1264, 558);
            this.panMain.TabIndex = 15;
            // 
            // SS2
            // 
            this.SS2.AccessibleDescription = "SS2, Sheet1, Row 0, Column 0, ";
            this.SS2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SS2.FocusRenderer = flatFocusIndicatorRenderer1;
            this.SS2.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SS2.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SS2.HorizontalScrollBar.Renderer = flatScrollBarRenderer1;
            this.SS2.HorizontalScrollBar.TabIndex = 257;
            this.SS2.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SS2.Location = new System.Drawing.Point(0, 22);
            this.SS2.Name = "SS2";
            this.SS2.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS2_Sheet1});
            this.SS2.Size = new System.Drawing.Size(1262, 534);
            this.SS2.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.SS2.TabIndex = 159;
            this.SS2.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SS2.VerticalScrollBar.Name = "";
            flatScrollBarRenderer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SS2.VerticalScrollBar.Renderer = flatScrollBarRenderer2;
            this.SS2.VerticalScrollBar.TabIndex = 258;
            this.SS2.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // SS2_Sheet1
            // 
            this.SS2_Sheet1.Reset();
            this.SS2_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS2_Sheet1.ColumnCount = 5;
            this.SS2_Sheet1.RowCount = 1;
            this.SS2_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS2_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS2_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.SS2_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS2_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS2_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS2_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.SS2_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS2_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS2_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS2_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.SS2_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS2_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS2_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS2_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.SS2_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS2_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS2_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS2_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.SS2_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS2_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SS2_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS2_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS2_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS2_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.SS2_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS2_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS2_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS2_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.SS2_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS2_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SS2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.LightGray;
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Dock = System.Windows.Forms.DockStyle.Top;
            this.label9.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(0, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(1262, 22);
            this.label9.TabIndex = 158;
            this.label9.Text = "  <DB List>";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // frmHaExcelImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 961);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.panSub02);
            this.Controls.Add(this.panExcel);
            this.Controls.Add(this.panSub01);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHaExcelImport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "종검예약 EXCEL 파일 DB저장";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panSub01.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panExcel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).EndInit();
            this.panPrg.ResumeLayout(false);
            this.panSub02.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SS2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS2_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Panel panSub01;
        private System.Windows.Forms.Panel panExcel;
        private System.Windows.Forms.Panel panSub02;
        private System.Windows.Forms.Panel panMain;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblJONG1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnLtdHelp;
        private System.Windows.Forms.TextBox txtLtdName;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnDialog;
        private System.Windows.Forms.TextBox txtFile1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox cboExcelType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboYYMM;
        private System.Windows.Forms.Panel panPrg;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ComboBox cboLtd;
        private System.Windows.Forms.Label label5;
        private FarPoint.Win.Spread.FpSpread SS1;
        private FarPoint.Win.Spread.SheetView SS1_Sheet1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.TextBox txtBirth;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TextBox txtSName;
        private System.Windows.Forms.Label label6;
        private FarPoint.Win.Spread.FpSpread SS2;
        private FarPoint.Win.Spread.SheetView SS2_Sheet1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnSave2;
        private System.Windows.Forms.ProgressBar prgBar;
        private System.Windows.Forms.Label lblMsg;
    }
}