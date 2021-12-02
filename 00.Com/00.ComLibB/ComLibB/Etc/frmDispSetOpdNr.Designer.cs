namespace ComLibB
{
    partial class frmDispSetOpdNr
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
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType4 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panDisp = new System.Windows.Forms.Panel();
            this.cboDoct = new System.Windows.Forms.ComboBox();
            this.cboDept = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.optJob_0 = new System.Windows.Forms.RadioButton();
            this.optJob_1 = new System.Windows.Forms.RadioButton();
            this.optJob_2 = new System.Windows.Forms.RadioButton();
            this.optJob_3 = new System.Windows.Forms.RadioButton();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.chkCon = new System.Windows.Forms.CheckBox();
            this.ssView2 = new FarPoint.Win.Spread.FpSpread();
            this.ssView2_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.ssView1 = new FarPoint.Win.Spread.FpSpread();
            this.ssView1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panPOPUP = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panInfo = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSeqRTime = new System.Windows.Forms.TextBox();
            this.btnSave2 = new System.Windows.Forms.Button();
            this.btnExit2 = new System.Windows.Forms.Button();
            this.pan_Doct = new System.Windows.Forms.Panel();
            this.cboDoct2 = new System.Windows.Forms.ComboBox();
            this.cboDept2 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panTitleSub0.SuspendLayout();
            this.panDisp.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView2_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView1_Sheet1)).BeginInit();
            this.panPOPUP.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pan_Doct.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 0);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(1032, 28);
            this.panTitleSub0.TabIndex = 14;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(5, 1);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(126, 19);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "대기순번 기타작업";
            // 
            // panDisp
            // 
            this.panDisp.Controls.Add(this.chkCon);
            this.panDisp.Controls.Add(this.dtpDate);
            this.panDisp.Controls.Add(this.label1);
            this.panDisp.Controls.Add(this.btnSave);
            this.panDisp.Controls.Add(this.btnSearch);
            this.panDisp.Controls.Add(this.btnExit);
            this.panDisp.Controls.Add(this.panel4);
            this.panDisp.Controls.Add(this.cboDoct);
            this.panDisp.Controls.Add(this.cboDept);
            this.panDisp.Controls.Add(this.label5);
            this.panDisp.Controls.Add(this.label2);
            this.panDisp.Dock = System.Windows.Forms.DockStyle.Top;
            this.panDisp.Location = new System.Drawing.Point(0, 28);
            this.panDisp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panDisp.Name = "panDisp";
            this.panDisp.Size = new System.Drawing.Size(1032, 67);
            this.panDisp.TabIndex = 22;
            // 
            // cboDoct
            // 
            this.cboDoct.FormattingEnabled = true;
            this.cboDoct.Location = new System.Drawing.Point(500, 8);
            this.cboDoct.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboDoct.Name = "cboDoct";
            this.cboDoct.Size = new System.Drawing.Size(132, 20);
            this.cboDoct.TabIndex = 4;
            // 
            // cboDept
            // 
            this.cboDept.FormattingEnabled = true;
            this.cboDept.Location = new System.Drawing.Point(409, 8);
            this.cboDept.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboDept.Name = "cboDept";
            this.cboDept.Size = new System.Drawing.Size(85, 20);
            this.cboDept.TabIndex = 3;
            this.cboDept.SelectedIndexChanged += new System.EventHandler(this.cboDept_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(343, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 17);
            this.label5.TabIndex = 4;
            this.label5.Text = "진료의사";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(18, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "작업구분";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 95);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1032, 516);
            this.panel1.TabIndex = 23;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panPOPUP);
            this.panel2.Controls.Add(this.ssView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(480, 516);
            this.panel2.TabIndex = 23;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.ssView2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(480, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(552, 516);
            this.panel3.TabIndex = 24;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.optJob_3);
            this.panel4.Controls.Add(this.optJob_2);
            this.panel4.Controls.Add(this.optJob_1);
            this.panel4.Controls.Add(this.optJob_0);
            this.panel4.Location = new System.Drawing.Point(84, 6);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(225, 26);
            this.panel4.TabIndex = 5;
            // 
            // optJob_0
            // 
            this.optJob_0.AutoSize = true;
            this.optJob_0.Location = new System.Drawing.Point(5, 5);
            this.optJob_0.Name = "optJob_0";
            this.optJob_0.Size = new System.Drawing.Size(47, 16);
            this.optJob_0.TabIndex = 1;
            this.optJob_0.TabStop = true;
            this.optJob_0.Text = "협진";
            this.optJob_0.UseVisualStyleBackColor = true;
            // 
            // optJob_1
            // 
            this.optJob_1.AutoSize = true;
            this.optJob_1.Location = new System.Drawing.Point(58, 5);
            this.optJob_1.Name = "optJob_1";
            this.optJob_1.Size = new System.Drawing.Size(47, 16);
            this.optJob_1.TabIndex = 2;
            this.optJob_1.TabStop = true;
            this.optJob_1.Text = "검진";
            this.optJob_1.UseVisualStyleBackColor = true;
            // 
            // optJob_2
            // 
            this.optJob_2.AutoSize = true;
            this.optJob_2.Location = new System.Drawing.Point(111, 5);
            this.optJob_2.Name = "optJob_2";
            this.optJob_2.Size = new System.Drawing.Size(59, 16);
            this.optJob_2.TabIndex = 3;
            this.optJob_2.TabStop = true;
            this.optJob_2.Text = "근전도";
            this.optJob_2.UseVisualStyleBackColor = true;
            // 
            // optJob_3
            // 
            this.optJob_3.AutoSize = true;
            this.optJob_3.Location = new System.Drawing.Point(176, 5);
            this.optJob_3.Name = "optJob_3";
            this.optJob_3.Size = new System.Drawing.Size(47, 16);
            this.optJob_3.TabIndex = 4;
            this.optJob_3.TabStop = true;
            this.optJob_3.Text = "입원";
            this.optJob_3.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(947, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(801, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave.Location = new System.Drawing.Point(874, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 30);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "완료";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(18, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 17);
            this.label1.TabIndex = 9;
            this.label1.Text = "일     자";
            // 
            // dtpDate
            // 
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(84, 39);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(107, 21);
            this.dtpDate.TabIndex = 10;
            // 
            // chkCon
            // 
            this.chkCon.AutoSize = true;
            this.chkCon.Location = new System.Drawing.Point(223, 44);
            this.chkCon.Name = "chkCon";
            this.chkCon.Size = new System.Drawing.Size(84, 16);
            this.chkCon.TabIndex = 11;
            this.chkCon.Text = "협진완료만";
            this.chkCon.UseVisualStyleBackColor = true;
            // 
            // ssView2
            // 
            this.ssView2.AccessibleDescription = "ssView2, Sheet1, Row 0, Column 0, ";
            this.ssView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView2.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssView2.Location = new System.Drawing.Point(0, 0);
            this.ssView2.Name = "ssView2";
            this.ssView2.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView2_Sheet1});
            this.ssView2.Size = new System.Drawing.Size(552, 516);
            this.ssView2.TabIndex = 0;
            this.ssView2.LeaveCell += new FarPoint.Win.Spread.LeaveCellEventHandler(this.ssView2_LeaveCell);
            this.ssView2.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssView2_CellClick);
            // 
            // ssView2_Sheet1
            // 
            this.ssView2_Sheet1.Reset();
            this.ssView2_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView2_Sheet1.ColumnCount = 10;
            this.ssView2_Sheet1.RowCount = 1;
            this.ssView2_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "등록번호";
            this.ssView2_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "성명";
            this.ssView2_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "과";
            this.ssView2_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "의사명";
            this.ssView2_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "도착시간";
            this.ssView2_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "진료예상시간";
            this.ssView2_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "구분";
            this.ssView2_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "의사코드";
            this.ssView2_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "ROWID";
            this.ssView2_Sheet1.ColumnHeader.Rows.Get(0).Height = 41F;
            this.ssView2_Sheet1.Columns.Get(0).CellType = checkBoxCellType4;
            this.ssView2_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(0).Width = 25F;
            this.ssView2_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(1).Label = "등록번호";
            this.ssView2_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(1).Width = 80F;
            this.ssView2_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(2).Label = "성명";
            this.ssView2_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(2).Width = 80F;
            this.ssView2_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(3).Label = "과";
            this.ssView2_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(3).Width = 40F;
            this.ssView2_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(4).Label = "의사명";
            this.ssView2_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(4).Width = 80F;
            this.ssView2_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(5).Label = "도착시간";
            this.ssView2_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(6).Label = "진료예상시간";
            this.ssView2_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(7).Label = "구분";
            this.ssView2_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(8).Label = "의사코드";
            this.ssView2_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(8).Visible = false;
            this.ssView2_Sheet1.Columns.Get(9).Label = "ROWID";
            this.ssView2_Sheet1.Columns.Get(9).Visible = false;
            this.ssView2_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // ssView1
            // 
            this.ssView1.AccessibleDescription = "ssView1, Sheet1, Row 0, Column 0, ";
            this.ssView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssView1.Location = new System.Drawing.Point(0, 0);
            this.ssView1.Name = "ssView1";
            this.ssView1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView1_Sheet1});
            this.ssView1.Size = new System.Drawing.Size(480, 516);
            this.ssView1.TabIndex = 1;
            this.ssView1.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssView1_CellDoubleClick);
            // 
            // ssView1_Sheet1
            // 
            this.ssView1_Sheet1.Reset();
            this.ssView1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView1_Sheet1.ColumnCount = 8;
            this.ssView1_Sheet1.RowCount = 1;
            this.ssView1_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView1_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "등록번호";
            this.ssView1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "성명";
            this.ssView1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "과";
            this.ssView1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "의사명";
            this.ssView1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "나이";
            this.ssView1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "성별";
            this.ssView1_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "구분";
            this.ssView1_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "의사코드";
            this.ssView1_Sheet1.ColumnHeader.Rows.Get(0).Height = 39F;
            this.ssView1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView1_Sheet1.Columns.Get(0).Label = "등록번호";
            this.ssView1_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView1_Sheet1.Columns.Get(0).Width = 80F;
            this.ssView1_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView1_Sheet1.Columns.Get(1).Label = "성명";
            this.ssView1_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView1_Sheet1.Columns.Get(1).Width = 80F;
            this.ssView1_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView1_Sheet1.Columns.Get(2).Label = "과";
            this.ssView1_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView1_Sheet1.Columns.Get(2).Width = 40F;
            this.ssView1_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView1_Sheet1.Columns.Get(3).Label = "의사명";
            this.ssView1_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView1_Sheet1.Columns.Get(3).Width = 80F;
            this.ssView1_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView1_Sheet1.Columns.Get(4).Label = "나이";
            this.ssView1_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView1_Sheet1.Columns.Get(4).Width = 40F;
            this.ssView1_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView1_Sheet1.Columns.Get(5).Label = "성별";
            this.ssView1_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView1_Sheet1.Columns.Get(5).Width = 40F;
            this.ssView1_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView1_Sheet1.Columns.Get(6).Label = "구분";
            this.ssView1_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView1_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView1_Sheet1.Columns.Get(7).Label = "의사코드";
            this.ssView1_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView1_Sheet1.Columns.Get(7).Visible = false;
            this.ssView1_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView1_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView1_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panPOPUP
            // 
            this.panPOPUP.Controls.Add(this.groupBox1);
            this.panPOPUP.Location = new System.Drawing.Point(51, 157);
            this.panPOPUP.Name = "panPOPUP";
            this.panPOPUP.Size = new System.Drawing.Size(334, 238);
            this.panPOPUP.TabIndex = 1;
            this.panPOPUP.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pan_Doct);
            this.groupBox1.Controls.Add(this.btnSave2);
            this.groupBox1.Controls.Add(this.btnExit2);
            this.groupBox1.Controls.Add(this.txtSeqRTime);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.panInfo);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(334, 238);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "대기순번 임의 추가작업";
            // 
            // panInfo
            // 
            this.panInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panInfo.Location = new System.Drawing.Point(35, 37);
            this.panInfo.Name = "panInfo";
            this.panInfo.Size = new System.Drawing.Size(266, 30);
            this.panInfo.TabIndex = 0;
            this.panInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(43, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "진료예상시간";
            // 
            // txtSeqRTime
            // 
            this.txtSeqRTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.txtSeqRTime.Location = new System.Drawing.Point(135, 96);
            this.txtSeqRTime.Name = "txtSeqRTime";
            this.txtSeqRTime.Size = new System.Drawing.Size(70, 21);
            this.txtSeqRTime.TabIndex = 6;
            // 
            // btnSave2
            // 
            this.btnSave2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave2.BackColor = System.Drawing.Color.Transparent;
            this.btnSave2.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave2.Location = new System.Drawing.Point(107, 187);
            this.btnSave2.Name = "btnSave2";
            this.btnSave2.Size = new System.Drawing.Size(72, 30);
            this.btnSave2.TabIndex = 14;
            this.btnSave2.Text = "설정";
            this.btnSave2.UseVisualStyleBackColor = false;
            this.btnSave2.Click += new System.EventHandler(this.btnSave2_Click);
            // 
            // btnExit2
            // 
            this.btnExit2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit2.BackColor = System.Drawing.Color.Transparent;
            this.btnExit2.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit2.Location = new System.Drawing.Point(180, 187);
            this.btnExit2.Name = "btnExit2";
            this.btnExit2.Size = new System.Drawing.Size(72, 30);
            this.btnExit2.TabIndex = 13;
            this.btnExit2.Text = "취소";
            this.btnExit2.UseVisualStyleBackColor = false;
            this.btnExit2.Click += new System.EventHandler(this.btnExit2_Click);
            // 
            // pan_Doct
            // 
            this.pan_Doct.Controls.Add(this.cboDoct2);
            this.pan_Doct.Controls.Add(this.cboDept2);
            this.pan_Doct.Controls.Add(this.label4);
            this.pan_Doct.Location = new System.Drawing.Point(35, 136);
            this.pan_Doct.Name = "pan_Doct";
            this.pan_Doct.Size = new System.Drawing.Size(266, 28);
            this.pan_Doct.TabIndex = 15;
            // 
            // cboDoct2
            // 
            this.cboDoct2.FormattingEnabled = true;
            this.cboDoct2.Location = new System.Drawing.Point(151, 4);
            this.cboDoct2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboDoct2.Name = "cboDoct2";
            this.cboDoct2.Size = new System.Drawing.Size(106, 20);
            this.cboDoct2.TabIndex = 14;
            // 
            // cboDept2
            // 
            this.cboDept2.FormattingEnabled = true;
            this.cboDept2.Location = new System.Drawing.Point(75, 4);
            this.cboDept2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboDept2.Name = "cboDept2";
            this.cboDept2.Size = new System.Drawing.Size(70, 20);
            this.cboDept2.TabIndex = 13;
            this.cboDept2.SelectedIndexChanged += new System.EventHandler(this.cboDept2_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(9, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 17);
            this.label4.TabIndex = 15;
            this.label4.Text = "진료의사";
            // 
            // frmDispSetOpdNr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1032, 611);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panDisp);
            this.Controls.Add(this.panTitleSub0);
            this.Name = "frmDispSetOpdNr";
            this.Text = "frmDispSetOpdNr";
            this.Load += new System.EventHandler(this.frmDispSetOpdNr_Load);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panDisp.ResumeLayout(false);
            this.panDisp.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView2_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView1_Sheet1)).EndInit();
            this.panPOPUP.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pan_Doct.ResumeLayout(false);
            this.pan_Doct.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panDisp;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.RadioButton optJob_3;
        private System.Windows.Forms.RadioButton optJob_2;
        private System.Windows.Forms.RadioButton optJob_1;
        private System.Windows.Forms.RadioButton optJob_0;
        private System.Windows.Forms.ComboBox cboDoct;
        private System.Windows.Forms.ComboBox cboDept;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox chkCon;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnExit;
        private FarPoint.Win.Spread.FpSpread ssView2;
        private FarPoint.Win.Spread.SheetView ssView2_Sheet1;
        private FarPoint.Win.Spread.FpSpread ssView1;
        private FarPoint.Win.Spread.SheetView ssView1_Sheet1;
        private System.Windows.Forms.Panel panPOPUP;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSave2;
        private System.Windows.Forms.Button btnExit2;
        private System.Windows.Forms.TextBox txtSeqRTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label panInfo;
        private System.Windows.Forms.Panel pan_Doct;
        private System.Windows.Forms.ComboBox cboDoct2;
        private System.Windows.Forms.ComboBox cboDept2;
        private System.Windows.Forms.Label label4;
    }
}