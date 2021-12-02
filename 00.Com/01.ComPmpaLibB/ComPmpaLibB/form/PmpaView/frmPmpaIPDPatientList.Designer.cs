namespace ComPmpaLibB
{
    partial class frmPmpaIPDPatientList
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("BorderEx351636464618523149313", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder1 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text517636464618523249341", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder2 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static632636464618523269350");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Static943636464618523379400");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.rdoOptSORT2 = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.rdoOptSORT1 = new System.Windows.Forms.RadioButton();
            this.cboGubun = new System.Windows.Forms.ComboBox();
            this.rdoOptSORT0 = new System.Windows.Forms.RadioButton();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.pan = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnView = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.SS1 = new FarPoint.Win.Spread.FpSpread();
            this.SS1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pan.SuspendLayout();
            this.panTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 97);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(921, 28);
            this.panel2.TabIndex = 130;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(6, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 17);
            this.label1.TabIndex = 22;
            this.label1.Text = "조회 결과";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.rdoOptSORT2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.rdoOptSORT1);
            this.panel1.Controls.Add(this.cboGubun);
            this.panel1.Controls.Add(this.rdoOptSORT0);
            this.panel1.Controls.Add(this.dtpDate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(921, 35);
            this.panel1.TabIndex = 129;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(512, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 23);
            this.label5.TabIndex = 96;
            this.label5.Text = "환자구분";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rdoOptSORT2
            // 
            this.rdoOptSORT2.AutoSize = true;
            this.rdoOptSORT2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rdoOptSORT2.ForeColor = System.Drawing.Color.Black;
            this.rdoOptSORT2.Location = new System.Drawing.Point(424, 7);
            this.rdoOptSORT2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoOptSORT2.Name = "rdoOptSORT2";
            this.rdoOptSORT2.Size = new System.Drawing.Size(81, 21);
            this.rdoOptSORT2.TabIndex = 95;
            this.rdoOptSORT2.Text = "과,의사순";
            this.rdoOptSORT2.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(194, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 23);
            this.label4.TabIndex = 94;
            this.label4.Text = "정렬순서";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(3, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 23);
            this.label3.TabIndex = 93;
            this.label3.Text = "입원일자";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rdoOptSORT1
            // 
            this.rdoOptSORT1.AutoSize = true;
            this.rdoOptSORT1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rdoOptSORT1.ForeColor = System.Drawing.Color.Black;
            this.rdoOptSORT1.Location = new System.Drawing.Point(333, 7);
            this.rdoOptSORT1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoOptSORT1.Name = "rdoOptSORT1";
            this.rdoOptSORT1.Size = new System.Drawing.Size(91, 21);
            this.rdoOptSORT1.TabIndex = 92;
            this.rdoOptSORT1.Text = "뱡동병실순";
            this.rdoOptSORT1.UseVisualStyleBackColor = true;
            // 
            // cboGubun
            // 
            this.cboGubun.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboGubun.FormattingEnabled = true;
            this.cboGubun.Location = new System.Drawing.Point(586, 5);
            this.cboGubun.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboGubun.Name = "cboGubun";
            this.cboGubun.Size = new System.Drawing.Size(89, 25);
            this.cboGubun.TabIndex = 0;
            // 
            // rdoOptSORT0
            // 
            this.rdoOptSORT0.AutoSize = true;
            this.rdoOptSORT0.Checked = true;
            this.rdoOptSORT0.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rdoOptSORT0.ForeColor = System.Drawing.Color.Black;
            this.rdoOptSORT0.Location = new System.Drawing.Point(268, 7);
            this.rdoOptSORT0.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoOptSORT0.Name = "rdoOptSORT0";
            this.rdoOptSORT0.Size = new System.Drawing.Size(65, 21);
            this.rdoOptSORT0.TabIndex = 91;
            this.rdoOptSORT0.TabStop = true;
            this.rdoOptSORT0.Text = "성명순";
            this.rdoOptSORT0.UseVisualStyleBackColor = true;
            // 
            // dtpDate
            // 
            this.dtpDate.CustomFormat = "yyyy-MM-dd";
            this.dtpDate.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDate.Location = new System.Drawing.Point(77, 5);
            this.dtpDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(105, 25);
            this.dtpDate.TabIndex = 0;
            // 
            // pan
            // 
            this.pan.BackColor = System.Drawing.Color.RoyalBlue;
            this.pan.Controls.Add(this.lblTitleSub0);
            this.pan.Dock = System.Windows.Forms.DockStyle.Top;
            this.pan.Location = new System.Drawing.Point(0, 34);
            this.pan.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pan.Name = "pan";
            this.pan.Size = new System.Drawing.Size(921, 28);
            this.pan.TabIndex = 128;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(6, 6);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(65, 17);
            this.lblTitleSub0.TabIndex = 22;
            this.lblTitleSub0.Text = "조회 옵션";
            this.lblTitleSub0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnView);
            this.panTitle.Controls.Add(this.label2);
            this.panTitle.Controls.Add(this.btnPrint);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(921, 34);
            this.panTitle.TabIndex = 127;
            // 
            // btnView
            // 
            this.btnView.AutoSize = true;
            this.btnView.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnView.Location = new System.Drawing.Point(701, 0);
            this.btnView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(72, 30);
            this.btnView.TabIndex = 84;
            this.btnView.Text = "조회";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(3, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 21);
            this.label2.TabIndex = 81;
            this.label2.Text = "입원환자 명부";
            // 
            // btnPrint
            // 
            this.btnPrint.AutoSize = true;
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPrint.Location = new System.Drawing.Point(773, 0);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 30);
            this.btnPrint.TabIndex = 77;
            this.btnPrint.Text = "인쇄";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnExit
            // 
            this.btnExit.AutoSize = true;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(845, 0);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // SS1
            // 
            this.SS1.AccessibleDescription = "";
            this.SS1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SS1.Location = new System.Drawing.Point(0, 125);
            this.SS1.Name = "SS1";
            namedStyle1.Border = complexBorder1;
            namedStyle1.Font = new System.Drawing.Font("굴림체", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle2.Border = complexBorder2;
            textCellType1.MaxLength = 60;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("굴림체", 9F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType2.Static = true;
            namedStyle3.CellType = textCellType2;
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.Renderer = textCellType2;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType3.Static = true;
            namedStyle4.CellType = textCellType3;
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.Renderer = textCellType3;
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4});
            this.SS1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS1_Sheet1});
            this.SS1.Size = new System.Drawing.Size(921, 605);
            this.SS1.TabIndex = 131;
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
            this.SS1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS1_Sheet1.ColumnCount = 17;
            this.SS1_Sheet1.RowCount = 1;
            this.SS1_Sheet1.Cells.Get(0, 0).Value = "1";
            this.SS1_Sheet1.Cells.Get(0, 1).Value = "12345678";
            this.SS1_Sheet1.Cells.Get(0, 2).Value = "이이이이이";
            this.SS1_Sheet1.Cells.Get(0, 3).Value = "이이리리";
            this.SS1_Sheet1.Cells.Get(0, 4).Value = "MR";
            this.SS1_Sheet1.Cells.Get(0, 5).Value = "이비인후과Rg";
            this.SS1_Sheet1.Cells.Get(0, 6).Value = "123";
            this.SS1_Sheet1.Cells.Get(0, 7).Value = "F/35";
            this.SS1_Sheet1.Cells.Get(0, 8).Value = "이이이이이";
            this.SS1_Sheet1.Cells.Get(0, 9).Value = "123456 - 1234567";
            this.SS1_Sheet1.Cells.Get(0, 10).Value = "이이이이이이이이이이이이이이이이이이";
            this.SS1_Sheet1.Cells.Get(0, 12).Value = "01234-456-7898";
            this.SS1_Sheet1.Cells.Get(0, 13).Value = "99-99-99";
            this.SS1_Sheet1.ColumnFooter.Columns.Default.Resizable = true;
            this.SS1_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SS1_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.SS1_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "순위";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "병록번호";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "수진자명";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "보험종류";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "과";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "의사";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "호실";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "나이";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "종교";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "주민번호";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "주소";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "작업조";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "전화번호";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "퇴원일";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "비고";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 15).Value = "입원번호";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 16).Value = "감액";
            this.SS1_Sheet1.ColumnHeader.Columns.Default.Resizable = true;
            this.SS1_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SS1_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.SS1_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.SS1_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.SS1_Sheet1.Columns.Default.Resizable = true;
            this.SS1_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SS1_Sheet1.Columns.Get(0).Label = "순위";
            this.SS1_Sheet1.Columns.Get(0).StyleName = "Static632636464618523269350";
            this.SS1_Sheet1.Columns.Get(0).Width = 37F;
            this.SS1_Sheet1.Columns.Get(1).Label = "병록번호";
            this.SS1_Sheet1.Columns.Get(1).StyleName = "Static632636464618523269350";
            this.SS1_Sheet1.Columns.Get(1).Width = 91F;
            this.SS1_Sheet1.Columns.Get(2).Label = "수진자명";
            this.SS1_Sheet1.Columns.Get(2).StyleName = "Static632636464618523269350";
            this.SS1_Sheet1.Columns.Get(2).Width = 107F;
            this.SS1_Sheet1.Columns.Get(3).Label = "보험종류";
            this.SS1_Sheet1.Columns.Get(3).StyleName = "Static632636464618523269350";
            this.SS1_Sheet1.Columns.Get(3).Width = 77F;
            this.SS1_Sheet1.Columns.Get(4).Label = "과";
            this.SS1_Sheet1.Columns.Get(4).StyleName = "Static632636464618523269350";
            this.SS1_Sheet1.Columns.Get(4).Width = 31F;
            this.SS1_Sheet1.Columns.Get(5).Label = "의사";
            this.SS1_Sheet1.Columns.Get(5).StyleName = "Static632636464618523269350";
            this.SS1_Sheet1.Columns.Get(5).Width = 54F;
            this.SS1_Sheet1.Columns.Get(6).Label = "호실";
            this.SS1_Sheet1.Columns.Get(6).StyleName = "Static632636464618523269350";
            this.SS1_Sheet1.Columns.Get(6).Width = 39F;
            this.SS1_Sheet1.Columns.Get(7).Label = "나이";
            this.SS1_Sheet1.Columns.Get(7).StyleName = "Static632636464618523269350";
            this.SS1_Sheet1.Columns.Get(7).Width = 39F;
            this.SS1_Sheet1.Columns.Get(8).Label = "종교";
            this.SS1_Sheet1.Columns.Get(8).StyleName = "Static943636464618523379400";
            this.SS1_Sheet1.Columns.Get(8).Width = 53F;
            this.SS1_Sheet1.Columns.Get(9).Label = "주민번호";
            this.SS1_Sheet1.Columns.Get(9).StyleName = "Static632636464618523269350";
            this.SS1_Sheet1.Columns.Get(9).Width = 105F;
            this.SS1_Sheet1.Columns.Get(10).Label = "주소";
            this.SS1_Sheet1.Columns.Get(10).StyleName = "Static943636464618523379400";
            this.SS1_Sheet1.Columns.Get(10).Width = 295F;
            this.SS1_Sheet1.Columns.Get(11).Label = "작업조";
            this.SS1_Sheet1.Columns.Get(11).StyleName = "Static632636464618523269350";
            this.SS1_Sheet1.Columns.Get(11).Width = 52F;
            this.SS1_Sheet1.Columns.Get(12).Label = "전화번호";
            this.SS1_Sheet1.Columns.Get(12).StyleName = "Static943636464618523379400";
            this.SS1_Sheet1.Columns.Get(12).Width = 89F;
            this.SS1_Sheet1.Columns.Get(13).Label = "퇴원일";
            this.SS1_Sheet1.Columns.Get(13).StyleName = "Static632636464618523269350";
            this.SS1_Sheet1.Columns.Get(13).Width = 78F;
            this.SS1_Sheet1.Columns.Get(14).Label = "비고";
            this.SS1_Sheet1.Columns.Get(14).StyleName = "Static943636464618523379400";
            this.SS1_Sheet1.Columns.Get(14).Width = 108F;
            this.SS1_Sheet1.Columns.Get(15).Label = "입원번호";
            this.SS1_Sheet1.Columns.Get(15).StyleName = "Static632636464618523269350";
            this.SS1_Sheet1.Columns.Get(15).Width = 83F;
            this.SS1_Sheet1.Columns.Get(16).Label = "감액";
            this.SS1_Sheet1.Columns.Get(16).StyleName = "Static632636464618523269350";
            this.SS1_Sheet1.Columns.Get(16).Width = 51F;
            this.SS1_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.SS1_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.FilterBar.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.SS1_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SS1_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.SS1_Sheet1.FrozenColumnCount = 3;
            this.SS1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS1_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.SS1_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SS1_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.SS1_Sheet1.RowHeader.Rows.Default.Resizable = false;
            this.SS1_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.SS1_Sheet1.RowHeader.Visible = false;
            this.SS1_Sheet1.Rows.Default.Height = 23F;
            this.SS1_Sheet1.Rows.Default.Resizable = false;
            this.SS1_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.SS1_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmPmpaIPDPatientList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(921, 730);
            this.Controls.Add(this.SS1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pan);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmPmpaIPDPatientList";
            this.Text = "입원환자 명부";
            this.Load += new System.EventHandler(this.frmPmpaIPDPatientList_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pan.ResumeLayout(false);
            this.pan.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.ComboBox cboGubun;
        private System.Windows.Forms.RadioButton rdoOptSORT1;
        private System.Windows.Forms.RadioButton rdoOptSORT0;
        private System.Windows.Forms.Panel pan;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton rdoOptSORT2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private FarPoint.Win.Spread.FpSpread SS1;
        private FarPoint.Win.Spread.SheetView SS1_Sheet1;
    }
}