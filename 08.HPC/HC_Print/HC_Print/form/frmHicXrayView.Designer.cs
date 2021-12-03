namespace HC_Print
{
    partial class frmHicXrayView
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
            FarPoint.Win.Spread.FlatFocusIndicatorRenderer flatFocusIndicatorRenderer1 = new FarPoint.Win.Spread.FlatFocusIndicatorRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer1 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer2 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panSub01 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.ChkGbRe = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rdoPanGubun4 = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rdoPanGubun3 = new System.Windows.Forms.RadioButton();
            this.rdoPanGubun2 = new System.Windows.Forms.RadioButton();
            this.rdoPanGubun1 = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdoGubun3 = new System.Windows.Forms.RadioButton();
            this.rdoGubun2 = new System.Windows.Forms.RadioButton();
            this.rdoGubun1 = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.검진종류 = new System.Windows.Forms.GroupBox();
            this.cboJong = new System.Windows.Forms.ComboBox();
            this.panSub05 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblLtdName = new System.Windows.Forms.Label();
            this.btnLtdHelp = new System.Windows.Forms.Button();
            this.txtLtdCode = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.panSub02 = new System.Windows.Forms.Panel();
            this.dtpTDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFDate = new System.Windows.Forms.DateTimePicker();
            this.lblSub01 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.SSList = new FarPoint.Win.Spread.FpSpread();
            this.SSList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panTitle.SuspendLayout();
            this.panSub01.SuspendLayout();
            this.panel4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.검진종류.SuspendLayout();
            this.panSub05.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panSub02.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1479, 39);
            this.panTitle.TabIndex = 138;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(1395, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(82, 37);
            this.btnExit.TabIndex = 21;
            this.btnExit.Text = "닫 기(&X)";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(11, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(160, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "건진판독명단 리스트";
            // 
            // panSub01
            // 
            this.panSub01.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panSub01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSub01.Controls.Add(this.panel4);
            this.panSub01.Controls.Add(this.panel3);
            this.panSub01.Controls.Add(this.panel2);
            this.panSub01.Controls.Add(this.panel1);
            this.panSub01.Controls.Add(this.panSub05);
            this.panSub01.Controls.Add(this.btnSearch);
            this.panSub01.Controls.Add(this.btnPrint);
            this.panSub01.Controls.Add(this.panSub02);
            this.panSub01.Controls.Add(this.lblSub01);
            this.panSub01.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSub01.Location = new System.Drawing.Point(0, 39);
            this.panSub01.Name = "panSub01";
            this.panSub01.Padding = new System.Windows.Forms.Padding(1);
            this.panSub01.Size = new System.Drawing.Size(1479, 65);
            this.panSub01.TabIndex = 148;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.groupBox5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.panel4.Location = new System.Drawing.Point(1135, 1);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(1);
            this.panel4.Size = new System.Drawing.Size(114, 61);
            this.panel4.TabIndex = 59;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.ChkGbRe);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox5.Location = new System.Drawing.Point(1, 1);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(109, 59);
            this.groupBox5.TabIndex = 61;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "삭제구분";
            // 
            // ChkGbRe
            // 
            this.ChkGbRe.AutoSize = true;
            this.ChkGbRe.BackColor = System.Drawing.Color.Transparent;
            this.ChkGbRe.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ChkGbRe.ForeColor = System.Drawing.Color.Black;
            this.ChkGbRe.Location = new System.Drawing.Point(6, 26);
            this.ChkGbRe.Name = "ChkGbRe";
            this.ChkGbRe.Size = new System.Drawing.Size(74, 19);
            this.ChkGbRe.TabIndex = 160;
            this.ChkGbRe.Text = "삭제포함";
            this.ChkGbRe.UseVisualStyleBackColor = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.rdoPanGubun4);
            this.panel3.Controls.Add(this.groupBox4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.panel3.Location = new System.Drawing.Point(855, 1);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(1);
            this.panel3.Size = new System.Drawing.Size(280, 61);
            this.panel3.TabIndex = 58;
            // 
            // rdoPanGubun4
            // 
            this.rdoPanGubun4.AutoSize = true;
            this.rdoPanGubun4.Location = new System.Drawing.Point(207, 25);
            this.rdoPanGubun4.Name = "rdoPanGubun4";
            this.rdoPanGubun4.Size = new System.Drawing.Size(65, 21);
            this.rdoPanGubun4.TabIndex = 5;
            this.rdoPanGubun4.Text = "보류자";
            this.rdoPanGubun4.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rdoPanGubun3);
            this.groupBox4.Controls.Add(this.rdoPanGubun2);
            this.groupBox4.Controls.Add(this.rdoPanGubun1);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox4.Location = new System.Drawing.Point(1, 1);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(274, 59);
            this.groupBox4.TabIndex = 61;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "조건구분";
            // 
            // rdoPanGubun3
            // 
            this.rdoPanGubun3.AutoSize = true;
            this.rdoPanGubun3.Location = new System.Drawing.Point(135, 24);
            this.rdoPanGubun3.Name = "rdoPanGubun3";
            this.rdoPanGubun3.Size = new System.Drawing.Size(65, 21);
            this.rdoPanGubun3.TabIndex = 4;
            this.rdoPanGubun3.Text = "판독자";
            this.rdoPanGubun3.UseVisualStyleBackColor = true;
            // 
            // rdoPanGubun2
            // 
            this.rdoPanGubun2.AutoSize = true;
            this.rdoPanGubun2.Location = new System.Drawing.Point(64, 24);
            this.rdoPanGubun2.Name = "rdoPanGubun2";
            this.rdoPanGubun2.Size = new System.Drawing.Size(65, 21);
            this.rdoPanGubun2.TabIndex = 3;
            this.rdoPanGubun2.Text = "미판독";
            this.rdoPanGubun2.UseVisualStyleBackColor = true;
            // 
            // rdoPanGubun1
            // 
            this.rdoPanGubun1.AutoSize = true;
            this.rdoPanGubun1.Checked = true;
            this.rdoPanGubun1.Location = new System.Drawing.Point(6, 24);
            this.rdoPanGubun1.Name = "rdoPanGubun1";
            this.rdoPanGubun1.Size = new System.Drawing.Size(52, 21);
            this.rdoPanGubun1.TabIndex = 2;
            this.rdoPanGubun1.TabStop = true;
            this.rdoPanGubun1.Text = "전체";
            this.rdoPanGubun1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.panel2.Location = new System.Drawing.Point(684, 1);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(1);
            this.panel2.Size = new System.Drawing.Size(171, 61);
            this.panel2.TabIndex = 57;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rdoGubun3);
            this.groupBox3.Controls.Add(this.rdoGubun2);
            this.groupBox3.Controls.Add(this.rdoGubun1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox3.Location = new System.Drawing.Point(1, 1);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(164, 59);
            this.groupBox3.TabIndex = 61;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "출장구분";
            // 
            // rdoGubun3
            // 
            this.rdoGubun3.AutoSize = true;
            this.rdoGubun3.Location = new System.Drawing.Point(111, 24);
            this.rdoGubun3.Name = "rdoGubun3";
            this.rdoGubun3.Size = new System.Drawing.Size(52, 21);
            this.rdoGubun3.TabIndex = 3;
            this.rdoGubun3.Text = "출장";
            this.rdoGubun3.UseVisualStyleBackColor = true;
            // 
            // rdoGubun2
            // 
            this.rdoGubun2.AutoSize = true;
            this.rdoGubun2.Location = new System.Drawing.Point(58, 24);
            this.rdoGubun2.Name = "rdoGubun2";
            this.rdoGubun2.Size = new System.Drawing.Size(52, 21);
            this.rdoGubun2.TabIndex = 2;
            this.rdoGubun2.Text = "내원";
            this.rdoGubun2.UseVisualStyleBackColor = true;
            // 
            // rdoGubun1
            // 
            this.rdoGubun1.AutoSize = true;
            this.rdoGubun1.Checked = true;
            this.rdoGubun1.Location = new System.Drawing.Point(6, 24);
            this.rdoGubun1.Name = "rdoGubun1";
            this.rdoGubun1.Size = new System.Drawing.Size(52, 21);
            this.rdoGubun1.TabIndex = 1;
            this.rdoGubun1.TabStop = true;
            this.rdoGubun1.Text = "전체";
            this.rdoGubun1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.검진종류);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.panel1.Location = new System.Drawing.Point(499, 1);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(1);
            this.panel1.Size = new System.Drawing.Size(185, 61);
            this.panel1.TabIndex = 56;
            // 
            // 검진종류
            // 
            this.검진종류.Controls.Add(this.cboJong);
            this.검진종류.Dock = System.Windows.Forms.DockStyle.Left;
            this.검진종류.Location = new System.Drawing.Point(1, 1);
            this.검진종류.Name = "검진종류";
            this.검진종류.Size = new System.Drawing.Size(175, 59);
            this.검진종류.TabIndex = 61;
            this.검진종류.TabStop = false;
            this.검진종류.Text = "검진종류";
            // 
            // cboJong
            // 
            this.cboJong.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboJong.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboJong.FormattingEnabled = true;
            this.cboJong.Location = new System.Drawing.Point(6, 20);
            this.cboJong.Name = "cboJong";
            this.cboJong.Size = new System.Drawing.Size(163, 23);
            this.cboJong.TabIndex = 33;
            // 
            // panSub05
            // 
            this.panSub05.Controls.Add(this.groupBox1);
            this.panSub05.Dock = System.Windows.Forms.DockStyle.Left;
            this.panSub05.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.panSub05.Location = new System.Drawing.Point(264, 1);
            this.panSub05.Name = "panSub05";
            this.panSub05.Padding = new System.Windows.Forms.Padding(1);
            this.panSub05.Size = new System.Drawing.Size(235, 61);
            this.panSub05.TabIndex = 55;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblLtdName);
            this.groupBox1.Controls.Add(this.btnLtdHelp);
            this.groupBox1.Controls.Add(this.txtLtdCode);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(1, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(228, 59);
            this.groupBox1.TabIndex = 61;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "사업장명";
            // 
            // lblLtdName
            // 
            this.lblLtdName.BackColor = System.Drawing.Color.LightGray;
            this.lblLtdName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLtdName.Location = new System.Drawing.Point(87, 20);
            this.lblLtdName.Name = "lblLtdName";
            this.lblLtdName.Size = new System.Drawing.Size(137, 23);
            this.lblLtdName.TabIndex = 100;
            this.lblLtdName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnLtdHelp
            // 
            this.btnLtdHelp.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLtdHelp.Location = new System.Drawing.Point(56, 20);
            this.btnLtdHelp.Name = "btnLtdHelp";
            this.btnLtdHelp.Size = new System.Drawing.Size(25, 25);
            this.btnLtdHelp.TabIndex = 99;
            this.btnLtdHelp.Text = "&H";
            this.btnLtdHelp.UseVisualStyleBackColor = true;
            // 
            // txtLtdCode
            // 
            this.txtLtdCode.Location = new System.Drawing.Point(5, 20);
            this.txtLtdCode.Name = "txtLtdCode";
            this.txtLtdCode.Size = new System.Drawing.Size(49, 25);
            this.txtLtdCode.TabIndex = 98;
            this.txtLtdCode.Tag = "LTDCODE";
            this.txtLtdCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(1312, 1);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(82, 61);
            this.btnSearch.TabIndex = 54;
            this.btnSearch.Text = "조 회(&V)";
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.White;
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrint.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPrint.Location = new System.Drawing.Point(1394, 1);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(82, 61);
            this.btnPrint.TabIndex = 53;
            this.btnPrint.Text = "인 쇄(&P)";
            this.btnPrint.UseVisualStyleBackColor = false;
            // 
            // panSub02
            // 
            this.panSub02.Controls.Add(this.dtpTDate);
            this.panSub02.Controls.Add(this.dtpFDate);
            this.panSub02.Dock = System.Windows.Forms.DockStyle.Left;
            this.panSub02.Location = new System.Drawing.Point(64, 1);
            this.panSub02.Name = "panSub02";
            this.panSub02.Size = new System.Drawing.Size(200, 61);
            this.panSub02.TabIndex = 44;
            // 
            // dtpTDate
            // 
            this.dtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTDate.Location = new System.Drawing.Point(107, 16);
            this.dtpTDate.Name = "dtpTDate";
            this.dtpTDate.Size = new System.Drawing.Size(88, 25);
            this.dtpTDate.TabIndex = 1;
            // 
            // dtpFDate
            // 
            this.dtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFDate.Location = new System.Drawing.Point(3, 16);
            this.dtpFDate.Name = "dtpFDate";
            this.dtpFDate.Size = new System.Drawing.Size(88, 25);
            this.dtpFDate.TabIndex = 0;
            // 
            // lblSub01
            // 
            this.lblSub01.BackColor = System.Drawing.Color.LightBlue;
            this.lblSub01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSub01.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSub01.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSub01.Location = new System.Drawing.Point(1, 1);
            this.lblSub01.Name = "lblSub01";
            this.lblSub01.Size = new System.Drawing.Size(63, 61);
            this.lblSub01.TabIndex = 43;
            this.lblSub01.Text = "접수일자";
            this.lblSub01.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.SSList);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 104);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1479, 460);
            this.panel5.TabIndex = 149;
            // 
            // SSList
            // 
            this.SSList.AccessibleDescription = "SSList, Sheet1, Row 0, Column 0, ";
            this.SSList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSList.FocusRenderer = flatFocusIndicatorRenderer1;
            this.SSList.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SSList.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SSList.HorizontalScrollBar.Renderer = flatScrollBarRenderer1;
            this.SSList.HorizontalScrollBar.TabIndex = 132;
            this.SSList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SSList.Location = new System.Drawing.Point(0, 0);
            this.SSList.Name = "SSList";
            this.SSList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSList_Sheet1});
            this.SSList.Size = new System.Drawing.Size(1479, 460);
            this.SSList.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.SSList.TabIndex = 141;
            this.SSList.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SSList.VerticalScrollBar.Name = "";
            flatScrollBarRenderer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SSList.VerticalScrollBar.Renderer = flatScrollBarRenderer2;
            this.SSList.VerticalScrollBar.TabIndex = 133;
            this.SSList.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // SSList_Sheet1
            // 
            this.SSList_Sheet1.Reset();
            this.SSList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSList_Sheet1.ColumnCount = 19;
            this.SSList_Sheet1.RowCount = 1;
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "촬영번호";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "검진종류";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "회사명";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "성명";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "분류";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "분류명";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "소견";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "나이";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "판독";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "영상";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "촬영명칭";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "ROWID";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "PACSNO";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "분진구분";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "출장구분";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 15).Value = "삭제일자";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 18).Value = "외래번호";
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.Rows.Get(0).Height = 35F;
            this.SSList_Sheet1.Columns.Get(0).Label = "촬영번호";
            this.SSList_Sheet1.Columns.Get(0).Width = 152F;
            this.SSList_Sheet1.Columns.Get(1).Label = "검진종류";
            this.SSList_Sheet1.Columns.Get(1).Width = 90F;
            this.SSList_Sheet1.Columns.Get(2).Label = "회사명";
            this.SSList_Sheet1.Columns.Get(2).Width = 90F;
            this.SSList_Sheet1.Columns.Get(3).Label = "성명";
            this.SSList_Sheet1.Columns.Get(3).Width = 63F;
            this.SSList_Sheet1.Columns.Get(4).Label = "분류";
            this.SSList_Sheet1.Columns.Get(4).Width = 58F;
            this.SSList_Sheet1.Columns.Get(5).Label = "분류명";
            this.SSList_Sheet1.Columns.Get(5).Width = 96F;
            this.SSList_Sheet1.Columns.Get(6).Label = "소견";
            this.SSList_Sheet1.Columns.Get(6).Width = 182F;
            this.SSList_Sheet1.Columns.Get(7).Label = "나이";
            this.SSList_Sheet1.Columns.Get(7).Width = 25F;
            this.SSList_Sheet1.Columns.Get(8).Label = "판독";
            this.SSList_Sheet1.Columns.Get(8).Width = 25F;
            this.SSList_Sheet1.Columns.Get(9).Label = "영상";
            this.SSList_Sheet1.Columns.Get(9).Width = 25F;
            this.SSList_Sheet1.Columns.Get(10).Label = "촬영명칭";
            this.SSList_Sheet1.Columns.Get(10).Width = 182F;
            this.SSList_Sheet1.Columns.Get(11).Label = "ROWID";
            this.SSList_Sheet1.Columns.Get(11).Width = 66F;
            this.SSList_Sheet1.Columns.Get(12).Label = "PACSNO";
            this.SSList_Sheet1.Columns.Get(12).Width = 66F;
            this.SSList_Sheet1.Columns.Get(13).Label = "분진구분";
            this.SSList_Sheet1.Columns.Get(13).Width = 61F;
            this.SSList_Sheet1.Columns.Get(14).Label = "출장구분";
            this.SSList_Sheet1.Columns.Get(14).Width = 61F;
            this.SSList_Sheet1.Columns.Get(15).Label = "삭제일자";
            this.SSList_Sheet1.Columns.Get(15).Width = 61F;
            this.SSList_Sheet1.Columns.Get(16).Width = 24F;
            this.SSList_Sheet1.Columns.Get(17).Width = 24F;
            this.SSList_Sheet1.Columns.Get(18).Label = "외래번호";
            this.SSList_Sheet1.Columns.Get(18).Width = 61F;
            this.SSList_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.SSList_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.SSList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SSList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSList_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.SSList_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.Rows.Get(0).Height = 24F;
            this.SSList_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.SSList_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel6
            // 
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 564);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1479, 59);
            this.panel6.TabIndex = 150;
            // 
            // frmHicXrayView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1479, 650);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panSub01);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHicXrayView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmHicXrayView";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panSub01.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.검진종류.ResumeLayout(false);
            this.panSub05.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panSub02.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panSub01;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox ChkGbRe;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rdoPanGubun1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdoGubun3;
        private System.Windows.Forms.RadioButton rdoGubun2;
        private System.Windows.Forms.RadioButton rdoGubun1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox 검진종류;
        private System.Windows.Forms.Panel panSub05;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblLtdName;
        private System.Windows.Forms.Button btnLtdHelp;
        private System.Windows.Forms.TextBox txtLtdCode;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Panel panSub02;
        private System.Windows.Forms.DateTimePicker dtpTDate;
        private System.Windows.Forms.DateTimePicker dtpFDate;
        private System.Windows.Forms.Label lblSub01;
        private System.Windows.Forms.Panel panel5;
        private FarPoint.Win.Spread.FpSpread SSList;
        private FarPoint.Win.Spread.SheetView SSList_Sheet1;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.RadioButton rdoPanGubun4;
        private System.Windows.Forms.RadioButton rdoPanGubun3;
        private System.Windows.Forms.RadioButton rdoPanGubun2;
        private System.Windows.Forms.ComboBox cboJong;
    }
}