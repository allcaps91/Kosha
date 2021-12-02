namespace ComLibB
{
    partial class frmDocu_01
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
            this.panTitle = new System.Windows.Forms.Panel();
            this.BtnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtnShow = new System.Windows.Forms.Button();
            this.BtnDocuName = new System.Windows.Forms.Button();
            this.BtnPlaceName = new System.Windows.Forms.Button();
            this.BtnDocuNo = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.OptGB1 = new System.Windows.Forms.RadioButton();
            this.OptGB0 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtYear = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnDelete = new System.Windows.Forms.Button();
            this.BtnSave = new System.Windows.Forms.Button();
            this.BtnDocuName_ADD = new System.Windows.Forms.Button();
            this.BtnPlaceName_ADD = new System.Windows.Forms.Button();
            this.TxtOutMan = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.CboBuse = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.BtnDocuNo_ADD = new System.Windows.Forms.Button();
            this.BtnSeqNoShow = new System.Windows.Forms.Button();
            this.CboDocuName = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.CboPlaceName = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.CboDocuNo = new System.Windows.Forms.ComboBox();
            this.TxtWorkDay = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.TxtSeqNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.ss1 = new FarPoint.Win.Spread.FpSpread();
            this.ss1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.label10 = new System.Windows.Forms.Label();
            this.TxtPage = new System.Windows.Forms.TextBox();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.BtnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1264, 34);
            this.panTitle.TabIndex = 13;
            // 
            // BtnExit
            // 
            this.BtnExit.AutoSize = true;
            this.BtnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BtnExit.Location = new System.Drawing.Point(1181, 0);
            this.BtnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(79, 30);
            this.BtnExit.TabIndex = 19;
            this.BtnExit.Text = "닫기";
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(74, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "문서관리";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.BtnShow);
            this.panel1.Controls.Add(this.BtnDocuName);
            this.panel1.Controls.Add(this.BtnPlaceName);
            this.panel1.Controls.Add(this.BtnDocuNo);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.TxtYear);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1264, 33);
            this.panel1.TabIndex = 14;
            // 
            // BtnShow
            // 
            this.BtnShow.Location = new System.Drawing.Point(740, 4);
            this.BtnShow.Name = "BtnShow";
            this.BtnShow.Size = new System.Drawing.Size(165, 27);
            this.BtnShow.TabIndex = 7;
            this.BtnShow.Text = "기존문서조회";
            this.BtnShow.UseVisualStyleBackColor = true;
            this.BtnShow.Click += new System.EventHandler(this.BtnShow_Click);
            // 
            // BtnDocuName
            // 
            this.BtnDocuName.Location = new System.Drawing.Point(625, 4);
            this.BtnDocuName.Name = "BtnDocuName";
            this.BtnDocuName.Size = new System.Drawing.Size(116, 27);
            this.BtnDocuName.TabIndex = 6;
            this.BtnDocuName.Text = "공문명 관리";
            this.BtnDocuName.UseVisualStyleBackColor = true;
            this.BtnDocuName.Click += new System.EventHandler(this.BtnDocuName_Click);
            // 
            // BtnPlaceName
            // 
            this.BtnPlaceName.Location = new System.Drawing.Point(509, 4);
            this.BtnPlaceName.Name = "BtnPlaceName";
            this.BtnPlaceName.Size = new System.Drawing.Size(116, 27);
            this.BtnPlaceName.TabIndex = 5;
            this.BtnPlaceName.Text = "기관명 관리";
            this.BtnPlaceName.UseVisualStyleBackColor = true;
            this.BtnPlaceName.Click += new System.EventHandler(this.BtnPlaceName_Click);
            // 
            // BtnDocuNo
            // 
            this.BtnDocuNo.Location = new System.Drawing.Point(393, 4);
            this.BtnDocuNo.Name = "BtnDocuNo";
            this.BtnDocuNo.Size = new System.Drawing.Size(116, 27);
            this.BtnDocuNo.TabIndex = 4;
            this.BtnDocuNo.Text = "문서번호 관리";
            this.BtnDocuNo.UseVisualStyleBackColor = true;
            this.BtnDocuNo.Click += new System.EventHandler(this.BtnDocuNo_Click);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.OptGB1);
            this.panel3.Controls.Add(this.OptGB0);
            this.panel3.Location = new System.Drawing.Point(280, 5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(109, 25);
            this.panel3.TabIndex = 3;
            // 
            // OptGB1
            // 
            this.OptGB1.AutoSize = true;
            this.OptGB1.Location = new System.Drawing.Point(55, 1);
            this.OptGB1.Name = "OptGB1";
            this.OptGB1.Size = new System.Drawing.Size(52, 21);
            this.OptGB1.TabIndex = 1;
            this.OptGB1.TabStop = true;
            this.OptGB1.Text = "발송";
            this.OptGB1.UseVisualStyleBackColor = true;
            // 
            // OptGB0
            // 
            this.OptGB0.AutoSize = true;
            this.OptGB0.Location = new System.Drawing.Point(3, 1);
            this.OptGB0.Name = "OptGB0";
            this.OptGB0.Size = new System.Drawing.Size(52, 21);
            this.OptGB0.TabIndex = 0;
            this.OptGB0.TabStop = true;
            this.OptGB0.Text = "접수";
            this.OptGB0.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Gainsboro;
            this.label2.Location = new System.Drawing.Point(197, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "구분";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TxtYear
            // 
            this.TxtYear.Location = new System.Drawing.Point(91, 5);
            this.TxtYear.Name = "TxtYear";
            this.TxtYear.Size = new System.Drawing.Size(100, 25);
            this.TxtYear.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Gainsboro;
            this.label1.Location = new System.Drawing.Point(8, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "작업년도";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.TxtPage);
            this.panel2.Controls.Add(this.BtnCancel);
            this.panel2.Controls.Add(this.BtnDelete);
            this.panel2.Controls.Add(this.BtnSave);
            this.panel2.Controls.Add(this.BtnDocuName_ADD);
            this.panel2.Controls.Add(this.BtnPlaceName_ADD);
            this.panel2.Controls.Add(this.TxtOutMan);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.CboBuse);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.BtnDocuNo_ADD);
            this.panel2.Controls.Add(this.BtnSeqNoShow);
            this.panel2.Controls.Add(this.CboDocuName);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.CboPlaceName);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.CboDocuNo);
            this.panel2.Controls.Add(this.TxtWorkDay);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.TxtSeqNo);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 67);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1264, 152);
            this.panel2.TabIndex = 15;
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(1134, 78);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(97, 37);
            this.BtnCancel.TabIndex = 22;
            this.BtnCancel.Text = "취 소";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnDelete
            // 
            this.BtnDelete.Location = new System.Drawing.Point(1134, 41);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(97, 37);
            this.BtnDelete.TabIndex = 21;
            this.BtnDelete.Text = "삭 제";
            this.BtnDelete.UseVisualStyleBackColor = true;
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(1134, 4);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(97, 37);
            this.BtnSave.TabIndex = 20;
            this.BtnSave.Text = "등 록";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnDocuName_ADD
            // 
            this.BtnDocuName_ADD.Location = new System.Drawing.Point(871, 116);
            this.BtnDocuName_ADD.Name = "BtnDocuName_ADD";
            this.BtnDocuName_ADD.Size = new System.Drawing.Size(148, 27);
            this.BtnDocuName_ADD.TabIndex = 19;
            this.BtnDocuName_ADD.Text = "현재목록 자동추가";
            this.BtnDocuName_ADD.UseVisualStyleBackColor = true;
            this.BtnDocuName_ADD.Click += new System.EventHandler(this.BtnDocuName_ADD_Click);
            // 
            // BtnPlaceName_ADD
            // 
            this.BtnPlaceName_ADD.Location = new System.Drawing.Point(557, 88);
            this.BtnPlaceName_ADD.Name = "BtnPlaceName_ADD";
            this.BtnPlaceName_ADD.Size = new System.Drawing.Size(148, 27);
            this.BtnPlaceName_ADD.TabIndex = 18;
            this.BtnPlaceName_ADD.Text = "현재목록 자동추가";
            this.BtnPlaceName_ADD.UseVisualStyleBackColor = true;
            this.BtnPlaceName_ADD.Click += new System.EventHandler(this.BtnPlaceName_ADD_Click);
            // 
            // TxtOutMan
            // 
            this.TxtOutMan.Location = new System.Drawing.Point(871, 33);
            this.TxtOutMan.Name = "TxtOutMan";
            this.TxtOutMan.Size = new System.Drawing.Size(243, 25);
            this.TxtOutMan.TabIndex = 17;
            this.TxtOutMan.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtOutMan_KeyDown);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Gainsboro;
            this.label9.Location = new System.Drawing.Point(711, 33);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(159, 25);
            this.label9.TabIndex = 16;
            this.label9.Text = "담   당   자";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CboBuse
            // 
            this.CboBuse.FormattingEnabled = true;
            this.CboBuse.Location = new System.Drawing.Point(871, 5);
            this.CboBuse.Name = "CboBuse";
            this.CboBuse.Size = new System.Drawing.Size(243, 25);
            this.CboBuse.TabIndex = 15;
            this.CboBuse.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CboBuse_KeyDown);
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Gainsboro;
            this.label8.Location = new System.Drawing.Point(711, 5);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(159, 25);
            this.label8.TabIndex = 14;
            this.label8.Text = "담   당   부   서";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnDocuNo_ADD
            // 
            this.BtnDocuNo_ADD.Location = new System.Drawing.Point(409, 32);
            this.BtnDocuNo_ADD.Name = "BtnDocuNo_ADD";
            this.BtnDocuNo_ADD.Size = new System.Drawing.Size(148, 27);
            this.BtnDocuNo_ADD.TabIndex = 13;
            this.BtnDocuNo_ADD.Text = "현재목록 자동추가";
            this.BtnDocuNo_ADD.UseVisualStyleBackColor = true;
            this.BtnDocuNo_ADD.Click += new System.EventHandler(this.BtnDocuNo_ADD_Click);
            // 
            // BtnSeqNoShow
            // 
            this.BtnSeqNoShow.Location = new System.Drawing.Point(291, 4);
            this.BtnSeqNoShow.Name = "BtnSeqNoShow";
            this.BtnSeqNoShow.Size = new System.Drawing.Size(118, 27);
            this.BtnSeqNoShow.TabIndex = 12;
            this.BtnSeqNoShow.Text = "자동일련번호";
            this.BtnSeqNoShow.UseVisualStyleBackColor = true;
            this.BtnSeqNoShow.Click += new System.EventHandler(this.BtnSeqNoShow_Click);
            // 
            // CboDocuName
            // 
            this.CboDocuName.FormattingEnabled = true;
            this.CboDocuName.Location = new System.Drawing.Point(166, 117);
            this.CboDocuName.Name = "CboDocuName";
            this.CboDocuName.Size = new System.Drawing.Size(704, 25);
            this.CboDocuName.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Gainsboro;
            this.label7.Location = new System.Drawing.Point(6, 117);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(159, 25);
            this.label7.TabIndex = 10;
            this.label7.Text = "공      문      명";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CboPlaceName
            // 
            this.CboPlaceName.FormattingEnabled = true;
            this.CboPlaceName.Location = new System.Drawing.Point(166, 89);
            this.CboPlaceName.Name = "CboPlaceName";
            this.CboPlaceName.Size = new System.Drawing.Size(391, 25);
            this.CboPlaceName.TabIndex = 9;
            this.CboPlaceName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CboPlaceName_KeyDown);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Gainsboro;
            this.label6.Location = new System.Drawing.Point(6, 89);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(159, 25);
            this.label6.TabIndex = 8;
            this.label6.Text = "기      관      명";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CboDocuNo
            // 
            this.CboDocuNo.FormattingEnabled = true;
            this.CboDocuNo.Location = new System.Drawing.Point(166, 33);
            this.CboDocuNo.Name = "CboDocuNo";
            this.CboDocuNo.Size = new System.Drawing.Size(243, 25);
            this.CboDocuNo.TabIndex = 7;
            this.CboDocuNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CboDocuNo_KeyDown);
            // 
            // TxtWorkDay
            // 
            this.TxtWorkDay.Location = new System.Drawing.Point(166, 61);
            this.TxtWorkDay.Name = "TxtWorkDay";
            this.TxtWorkDay.Size = new System.Drawing.Size(124, 25);
            this.TxtWorkDay.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Gainsboro;
            this.label5.Location = new System.Drawing.Point(6, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(159, 25);
            this.label5.TabIndex = 5;
            this.label5.Text = "처   리   일   자";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Gainsboro;
            this.label4.Location = new System.Drawing.Point(6, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(159, 25);
            this.label4.TabIndex = 3;
            this.label4.Text = "문   서   번   호";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TxtSeqNo
            // 
            this.TxtSeqNo.BackColor = System.Drawing.Color.LightYellow;
            this.TxtSeqNo.Location = new System.Drawing.Point(166, 5);
            this.TxtSeqNo.Name = "TxtSeqNo";
            this.TxtSeqNo.Size = new System.Drawing.Size(124, 25);
            this.TxtSeqNo.TabIndex = 2;
            this.TxtSeqNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtSeqNo_KeyDown);
            this.TxtSeqNo.Leave += new System.EventHandler(this.TxtSeqNo_Leave);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Gainsboro;
            this.label3.Location = new System.Drawing.Point(6, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(159, 25);
            this.label3.TabIndex = 1;
            this.label3.Text = "일   련   번   호";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 219);
            this.panTitleSub0.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(1264, 28);
            this.panTitleSub0.TabIndex = 16;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 6);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(43, 15);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "리스트";
            // 
            // ss1
            // 
            this.ss1.AccessibleDescription = "";
            this.ss1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ss1.Location = new System.Drawing.Point(0, 247);
            this.ss1.Name = "ss1";
            this.ss1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ss1_Sheet1});
            this.ss1.Size = new System.Drawing.Size(1264, 361);
            this.ss1.TabIndex = 17;
            this.ss1.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ss1_CellDoubleClick);
            // 
            // ss1_Sheet1
            // 
            this.ss1_Sheet1.Reset();
            this.ss1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ss1_Sheet1.ColumnCount = 4;
            this.ss1_Sheet1.RowCount = 1;
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "일련번호";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "처리일자";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "문서번호";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "문서내용";
            this.ss1_Sheet1.ColumnHeader.Rows.Get(0).Height = 41F;
            this.ss1_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ss1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(0).Label = "일련번호";
            this.ss1_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(0).Width = 62F;
            this.ss1_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ss1_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(1).Label = "처리일자";
            this.ss1_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(1).Width = 120F;
            this.ss1_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ss1_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ss1_Sheet1.Columns.Get(2).Label = "문서번호";
            this.ss1_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(2).Width = 300F;
            this.ss1_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ss1_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ss1_Sheet1.Columns.Get(3).Label = "문서내용";
            this.ss1_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(3).Width = 600F;
            this.ss1_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
            this.ss1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.LightGray;
            this.label10.Location = new System.Drawing.Point(296, 61);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(113, 25);
            this.label10.TabIndex = 47;
            this.label10.Tag = "";
            this.label10.Text = "총페이지";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TxtPage
            // 
            this.TxtPage.Location = new System.Drawing.Point(410, 61);
            this.TxtPage.Name = "TxtPage";
            this.TxtPage.Size = new System.Drawing.Size(82, 25);
            this.TxtPage.TabIndex = 48;
            this.TxtPage.Tag = "OUTMAN";
            // 
            // frmDocu_01
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1264, 608);
            this.Controls.Add(this.ss1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmDocu_01";
            this.Text = "frmDocu_01";
            this.Activated += new System.EventHandler(this.frmDocu_01_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDocu_01_FormClosed);
            this.Load += new System.EventHandler(this.frmDocu_01_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button BtnShow;
        private System.Windows.Forms.Button BtnDocuName;
        private System.Windows.Forms.Button BtnPlaceName;
        private System.Windows.Forms.Button BtnDocuNo;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton OptGB1;
        private System.Windows.Forms.RadioButton OptGB0;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnSeqNoShow;
        private System.Windows.Forms.ComboBox CboDocuName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox CboPlaceName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox CboDocuNo;
        private System.Windows.Forms.TextBox TxtWorkDay;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TxtSeqNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnDelete;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Button BtnDocuName_ADD;
        private System.Windows.Forms.Button BtnPlaceName_ADD;
        private System.Windows.Forms.TextBox TxtOutMan;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox CboBuse;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button BtnDocuNo_ADD;
        private FarPoint.Win.Spread.FpSpread ss1;
        private FarPoint.Win.Spread.SheetView ss1_Sheet1;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox TxtPage;
    }
}