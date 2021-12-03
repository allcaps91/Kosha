namespace ComLibB
{
    partial class frmDocu1
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.BtnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.TxtYear = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnSave = new System.Windows.Forms.Button();
            this.BtnDelete = new System.Windows.Forms.Button();
            this.LblEMP_GB = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.OptGB1 = new System.Windows.Forms.RadioButton();
            this.OptGB0 = new System.Windows.Forms.RadioButton();
            this.PanMain = new System.Windows.Forms.Panel();
            this.TxtDocuNo = new System.Windows.Forms.TextBox();
            this.BtnSEQ = new System.Windows.Forms.Button();
            this.CboDocuName = new System.Windows.Forms.ComboBox();
            this.CboPlaceName = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.CboBuse = new System.Windows.Forms.ComboBox();
            this.DtpWorkDay = new System.Windows.Forms.DateTimePicker();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtOutMan = new System.Windows.Forms.TextBox();
            this.TxtSeqNo = new System.Windows.Forms.TextBox();
            this.SsList = new FarPoint.Win.Spread.FpSpread();
            this.SsList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtPage = new System.Windows.Forms.TextBox();
            this.panTitle.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.PanMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SsList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SsList_Sheet1)).BeginInit();
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
            this.panTitle.Size = new System.Drawing.Size(1201, 34);
            this.panTitle.TabIndex = 16;
            // 
            // BtnExit
            // 
            this.BtnExit.AutoSize = true;
            this.BtnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BtnExit.Location = new System.Drawing.Point(1118, 0);
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
            this.lblTitle.Size = new System.Drawing.Size(106, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "원외발신공문";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.TxtYear);
            this.panel5.Controls.Add(this.label27);
            this.panel5.Controls.Add(this.panel3);
            this.panel5.Controls.Add(this.LblEMP_GB);
            this.panel5.Controls.Add(this.panel1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 34);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1201, 32);
            this.panel5.TabIndex = 35;
            // 
            // TxtYear
            // 
            this.TxtYear.Location = new System.Drawing.Point(83, 3);
            this.TxtYear.Name = "TxtYear";
            this.TxtYear.Size = new System.Drawing.Size(93, 25);
            this.TxtYear.TabIndex = 31;
            // 
            // label27
            // 
            this.label27.BackColor = System.Drawing.Color.LightGray;
            this.label27.Location = new System.Drawing.Point(7, 3);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(75, 25);
            this.label27.TabIndex = 27;
            this.label27.Tag = "";
            this.label27.Text = "조회기간";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.AutoSize = true;
            this.panel3.Controls.Add(this.BtnSearch);
            this.panel3.Controls.Add(this.BtnCancel);
            this.panel3.Controls.Add(this.BtnSave);
            this.panel3.Controls.Add(this.BtnDelete);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(883, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(1);
            this.panel3.Size = new System.Drawing.Size(318, 32);
            this.panel3.TabIndex = 17;
            // 
            // BtnSearch
            // 
            this.BtnSearch.AutoSize = true;
            this.BtnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BtnSearch.Location = new System.Drawing.Point(1, 1);
            this.BtnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(79, 30);
            this.BtnSearch.TabIndex = 19;
            this.BtnSearch.Text = "조  회";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.AutoSize = true;
            this.BtnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BtnCancel.Location = new System.Drawing.Point(80, 1);
            this.BtnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(79, 30);
            this.BtnCancel.TabIndex = 20;
            this.BtnCancel.Text = "취  소";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.AutoSize = true;
            this.BtnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BtnSave.Location = new System.Drawing.Point(159, 1);
            this.BtnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(79, 30);
            this.BtnSave.TabIndex = 17;
            this.BtnSave.Text = "저  장";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnDelete
            // 
            this.BtnDelete.AutoSize = true;
            this.BtnDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnDelete.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BtnDelete.Location = new System.Drawing.Point(238, 1);
            this.BtnDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(79, 30);
            this.BtnDelete.TabIndex = 16;
            this.BtnDelete.Text = "삭  제";
            this.BtnDelete.UseVisualStyleBackColor = true;
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // LblEMP_GB
            // 
            this.LblEMP_GB.BackColor = System.Drawing.Color.LightGray;
            this.LblEMP_GB.Location = new System.Drawing.Point(180, 3);
            this.LblEMP_GB.Name = "LblEMP_GB";
            this.LblEMP_GB.Size = new System.Drawing.Size(75, 25);
            this.LblEMP_GB.TabIndex = 27;
            this.LblEMP_GB.Tag = "";
            this.LblEMP_GB.Text = "구분";
            this.LblEMP_GB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.OptGB1);
            this.panel1.Controls.Add(this.OptGB0);
            this.panel1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.panel1.Location = new System.Drawing.Point(256, 3);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel1.Size = new System.Drawing.Size(124, 25);
            this.panel1.TabIndex = 30;
            // 
            // OptGB1
            // 
            this.OptGB1.AutoSize = true;
            this.OptGB1.Checked = true;
            this.OptGB1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.OptGB1.Location = new System.Drawing.Point(65, 1);
            this.OptGB1.Name = "OptGB1";
            this.OptGB1.Size = new System.Drawing.Size(52, 21);
            this.OptGB1.TabIndex = 0;
            this.OptGB1.TabStop = true;
            this.OptGB1.Text = "발송";
            this.OptGB1.UseVisualStyleBackColor = true;
            this.OptGB1.CheckedChanged += new System.EventHandler(this.OptGB1_CheckedChanged);
            // 
            // OptGB0
            // 
            this.OptGB0.AutoSize = true;
            this.OptGB0.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.OptGB0.Location = new System.Drawing.Point(5, 1);
            this.OptGB0.Name = "OptGB0";
            this.OptGB0.Size = new System.Drawing.Size(52, 21);
            this.OptGB0.TabIndex = 0;
            this.OptGB0.Text = "접수";
            this.OptGB0.UseVisualStyleBackColor = true;
            this.OptGB0.Visible = false;
            this.OptGB0.CheckedChanged += new System.EventHandler(this.OptGB0_CheckedChanged);
            // 
            // PanMain
            // 
            this.PanMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PanMain.Controls.Add(this.label1);
            this.PanMain.Controls.Add(this.TxtPage);
            this.PanMain.Controls.Add(this.TxtDocuNo);
            this.PanMain.Controls.Add(this.BtnSEQ);
            this.PanMain.Controls.Add(this.CboDocuName);
            this.PanMain.Controls.Add(this.CboPlaceName);
            this.PanMain.Controls.Add(this.label11);
            this.PanMain.Controls.Add(this.CboBuse);
            this.PanMain.Controls.Add(this.DtpWorkDay);
            this.PanMain.Controls.Add(this.label10);
            this.PanMain.Controls.Add(this.label8);
            this.PanMain.Controls.Add(this.label6);
            this.PanMain.Controls.Add(this.label9);
            this.PanMain.Controls.Add(this.label7);
            this.PanMain.Controls.Add(this.label3);
            this.PanMain.Controls.Add(this.TxtOutMan);
            this.PanMain.Controls.Add(this.TxtSeqNo);
            this.PanMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanMain.Location = new System.Drawing.Point(0, 66);
            this.PanMain.Name = "PanMain";
            this.PanMain.Size = new System.Drawing.Size(1201, 158);
            this.PanMain.TabIndex = 85;
            // 
            // TxtDocuNo
            // 
            this.TxtDocuNo.Location = new System.Drawing.Point(127, 33);
            this.TxtDocuNo.Name = "TxtDocuNo";
            this.TxtDocuNo.Size = new System.Drawing.Size(213, 25);
            this.TxtDocuNo.TabIndex = 44;
            // 
            // BtnSEQ
            // 
            this.BtnSEQ.AutoSize = true;
            this.BtnSEQ.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BtnSEQ.Location = new System.Drawing.Point(240, 5);
            this.BtnSEQ.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnSEQ.Name = "BtnSEQ";
            this.BtnSEQ.Size = new System.Drawing.Size(100, 26);
            this.BtnSEQ.TabIndex = 43;
            this.BtnSEQ.Text = "자동일련번호";
            this.BtnSEQ.UseVisualStyleBackColor = true;
            this.BtnSEQ.Click += new System.EventHandler(this.BtnSEQ_Click);
            // 
            // CboDocuName
            // 
            this.CboDocuName.FormattingEnabled = true;
            this.CboDocuName.Location = new System.Drawing.Point(127, 117);
            this.CboDocuName.Name = "CboDocuName";
            this.CboDocuName.Size = new System.Drawing.Size(549, 25);
            this.CboDocuName.TabIndex = 42;
            this.CboDocuName.Tag = "DOCUNAME";
            // 
            // CboPlaceName
            // 
            this.CboPlaceName.FormattingEnabled = true;
            this.CboPlaceName.Location = new System.Drawing.Point(127, 89);
            this.CboPlaceName.Name = "CboPlaceName";
            this.CboPlaceName.Size = new System.Drawing.Size(549, 25);
            this.CboPlaceName.TabIndex = 41;
            this.CboPlaceName.Tag = "PLACENAME";
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.LightGray;
            this.label11.Location = new System.Drawing.Point(5, 117);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(120, 25);
            this.label11.TabIndex = 35;
            this.label11.Tag = "";
            this.label11.Text = "공문명";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CboBuse
            // 
            this.CboBuse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CboBuse.FormattingEnabled = true;
            this.CboBuse.Location = new System.Drawing.Point(483, 33);
            this.CboBuse.Name = "CboBuse";
            this.CboBuse.Size = new System.Drawing.Size(116, 25);
            this.CboBuse.TabIndex = 34;
            this.CboBuse.Tag = "BUSE";
            // 
            // DtpWorkDay
            // 
            this.DtpWorkDay.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpWorkDay.Location = new System.Drawing.Point(127, 61);
            this.DtpWorkDay.Name = "DtpWorkDay";
            this.DtpWorkDay.Size = new System.Drawing.Size(212, 25);
            this.DtpWorkDay.TabIndex = 33;
            this.DtpWorkDay.Tag = "WORKDAY";
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.LightGray;
            this.label10.Location = new System.Drawing.Point(5, 89);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(120, 25);
            this.label10.TabIndex = 30;
            this.label10.Tag = "";
            this.label10.Text = "수신처";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.LightGray;
            this.label8.Location = new System.Drawing.Point(361, 33);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(120, 25);
            this.label8.TabIndex = 30;
            this.label8.Tag = "";
            this.label8.Text = "담당부서";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.LightGray;
            this.label6.Location = new System.Drawing.Point(5, 33);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 25);
            this.label6.TabIndex = 30;
            this.label6.Tag = "";
            this.label6.Text = "문서번호";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.LightGray;
            this.label9.Location = new System.Drawing.Point(361, 61);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(120, 25);
            this.label9.TabIndex = 30;
            this.label9.Tag = "";
            this.label9.Text = "담당자";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.LightGray;
            this.label7.Location = new System.Drawing.Point(5, 61);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(120, 25);
            this.label7.TabIndex = 30;
            this.label7.Tag = "";
            this.label7.Text = "접수/발송일자";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.LightGray;
            this.label3.Location = new System.Drawing.Point(5, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 25);
            this.label3.TabIndex = 30;
            this.label3.Tag = "";
            this.label3.Text = "일련번호";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TxtOutMan
            // 
            this.TxtOutMan.Location = new System.Drawing.Point(483, 61);
            this.TxtOutMan.Name = "TxtOutMan";
            this.TxtOutMan.Size = new System.Drawing.Size(193, 25);
            this.TxtOutMan.TabIndex = 31;
            this.TxtOutMan.Tag = "OUTMAN";
            // 
            // TxtSeqNo
            // 
            this.TxtSeqNo.Location = new System.Drawing.Point(127, 5);
            this.TxtSeqNo.Name = "TxtSeqNo";
            this.TxtSeqNo.ReadOnly = true;
            this.TxtSeqNo.Size = new System.Drawing.Size(110, 25);
            this.TxtSeqNo.TabIndex = 31;
            this.TxtSeqNo.Tag = "SEQNO";
            // 
            // SsList
            // 
            this.SsList.AccessibleDescription = "SsList, Sheet1, Row 0, Column 0, ";
            this.SsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SsList.Location = new System.Drawing.Point(0, 224);
            this.SsList.Name = "SsList";
            this.SsList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SsList_Sheet1});
            this.SsList.Size = new System.Drawing.Size(1201, 537);
            this.SsList.TabIndex = 87;
            this.SsList.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SsList_CellClick);
            // 
            // SsList_Sheet1
            // 
            this.SsList_Sheet1.Reset();
            this.SsList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SsList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SsList_Sheet1.ColumnCount = 6;
            this.SsList_Sheet1.RowCount = 1;
            this.SsList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SsList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SsList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SsList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SsList_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "일련번호";
            this.SsList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "문서번호";
            this.SsList_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "공문명";
            this.SsList_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "수신처";
            this.SsList_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "담당자";
            this.SsList_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "처리일자";
            this.SsList_Sheet1.ColumnHeader.Rows.Get(0).Height = 41F;
            this.SsList_Sheet1.Columns.Get(0).CellType = textCellType7;
            this.SsList_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SsList_Sheet1.Columns.Get(0).Label = "일련번호";
            this.SsList_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SsList_Sheet1.Columns.Get(0).Width = 44F;
            this.SsList_Sheet1.Columns.Get(1).CellType = textCellType8;
            this.SsList_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SsList_Sheet1.Columns.Get(1).Label = "문서번호";
            this.SsList_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SsList_Sheet1.Columns.Get(1).Width = 150F;
            this.SsList_Sheet1.Columns.Get(2).CellType = textCellType9;
            this.SsList_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SsList_Sheet1.Columns.Get(2).Label = "공문명";
            this.SsList_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SsList_Sheet1.Columns.Get(2).Width = 445F;
            this.SsList_Sheet1.Columns.Get(3).CellType = textCellType10;
            this.SsList_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SsList_Sheet1.Columns.Get(3).Label = "수신처";
            this.SsList_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SsList_Sheet1.Columns.Get(3).Width = 305F;
            this.SsList_Sheet1.Columns.Get(4).CellType = textCellType11;
            this.SsList_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SsList_Sheet1.Columns.Get(4).Label = "담당자";
            this.SsList_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SsList_Sheet1.Columns.Get(4).Width = 100F;
            this.SsList_Sheet1.Columns.Get(5).CellType = textCellType12;
            this.SsList_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SsList_Sheet1.Columns.Get(5).Label = "처리일자";
            this.SsList_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SsList_Sheet1.Columns.Get(5).Width = 100F;
            this.SsList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SsList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SsList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SsList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SsList_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
            this.SsList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SsList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LightGray;
            this.label1.Location = new System.Drawing.Point(692, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 25);
            this.label1.TabIndex = 47;
            this.label1.Tag = "";
            this.label1.Text = "총페이지";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TxtPage
            // 
            this.TxtPage.Location = new System.Drawing.Point(814, 61);
            this.TxtPage.Name = "TxtPage";
            this.TxtPage.Size = new System.Drawing.Size(82, 25);
            this.TxtPage.TabIndex = 48;
            this.TxtPage.Tag = "OUTMAN";
            // 
            // frmDocu1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1201, 761);
            this.Controls.Add(this.SsList);
            this.Controls.Add(this.PanMain);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmDocu1";
            this.Text = "frmDocu1";
            this.Activated += new System.EventHandler(this.frmDocu1_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDocu1_FormClosed);
            this.Load += new System.EventHandler(this.frmDocu1_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.PanMain.ResumeLayout(false);
            this.PanMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SsList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SsList_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Button BtnDelete;
        private System.Windows.Forms.Label LblEMP_GB;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton OptGB1;
        private System.Windows.Forms.RadioButton OptGB0;
        private System.Windows.Forms.Panel PanMain;
        private System.Windows.Forms.Button BtnSEQ;
        private System.Windows.Forms.ComboBox CboDocuName;
        private System.Windows.Forms.ComboBox CboPlaceName;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox CboBuse;
        private System.Windows.Forms.DateTimePicker DtpWorkDay;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtOutMan;
        private System.Windows.Forms.TextBox TxtSeqNo;
        private FarPoint.Win.Spread.FpSpread SsList;
        private FarPoint.Win.Spread.SheetView SsList_Sheet1;
        private System.Windows.Forms.TextBox TxtYear;
        private System.Windows.Forms.TextBox TxtDocuNo;
        public System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtPage;
    }
}