﻿namespace ComPmpaLibB
{
    partial class frmPmpaViewNotJinList2
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("BorderEx402636404992078192999", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder1 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Static506636404992078192999", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder2 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static694636404992078192999");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.EmptyBorder emptyBorder1 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder2 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder3 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder4 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder5 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.EmptyBorder emptyBorder6 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder7 = new FarPoint.Win.EmptyBorder();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dtpTDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.pan = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.ssList = new FarPoint.Win.Spread.FpSpread();
            this.ssList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.pan.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 68);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(655, 58);
            this.panel1.TabIndex = 123;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dtpTDate);
            this.groupBox2.Controls.Add(this.dtpFDate);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox2.Location = new System.Drawing.Point(14, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(292, 46);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "조회년월";
            // 
            // dtpTDate
            // 
            this.dtpTDate.CustomFormat = "yyyy-MM-dd";
            this.dtpTDate.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTDate.Location = new System.Drawing.Point(161, 16);
            this.dtpTDate.Name = "dtpTDate";
            this.dtpTDate.Size = new System.Drawing.Size(121, 25);
            this.dtpTDate.TabIndex = 125;
            // 
            // dtpFDate
            // 
            this.dtpFDate.CustomFormat = "yyyy-MM-dd";
            this.dtpFDate.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFDate.Location = new System.Drawing.Point(11, 16);
            this.dtpFDate.Name = "dtpFDate";
            this.dtpFDate.Size = new System.Drawing.Size(121, 25);
            this.dtpFDate.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(138, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 17);
            this.label1.TabIndex = 124;
            this.label1.Text = "~";
            // 
            // pan
            // 
            this.pan.BackColor = System.Drawing.Color.RoyalBlue;
            this.pan.Controls.Add(this.lblTitleSub0);
            this.pan.Dock = System.Windows.Forms.DockStyle.Top;
            this.pan.Location = new System.Drawing.Point(0, 40);
            this.pan.Name = "pan";
            this.pan.Size = new System.Drawing.Size(655, 28);
            this.pan.TabIndex = 122;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(12, 9);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(62, 12);
            this.lblTitleSub0.TabIndex = 22;
            this.lblTitleSub0.Text = "조회 옵션";
            this.lblTitleSub0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnCancel);
            this.panTitle.Controls.Add(this.btnPrint);
            this.panTitle.Controls.Add(this.btnView);
            this.panTitle.Controls.Add(this.label2);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(655, 40);
            this.panTitle.TabIndex = 121;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.AutoSize = true;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCancel.Location = new System.Drawing.Point(428, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 30);
            this.btnCancel.TabIndex = 118;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.AutoSize = true;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPrint.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPrint.Location = new System.Drawing.Point(502, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 30);
            this.btnPrint.TabIndex = 117;
            this.btnPrint.Text = "인쇄";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // btnView
            // 
            this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnView.AutoSize = true;
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnView.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnView.Location = new System.Drawing.Point(354, 4);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(72, 30);
            this.btnView.TabIndex = 116;
            this.btnView.Text = "조회";
            this.btnView.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(8, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(296, 21);
            this.label2.TabIndex = 81;
            this.label2.Text = "당일 접수비 수납후 미진료 환자 리스트";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(576, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel2.Controls.Add(this.label3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 126);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(655, 28);
            this.panel2.TabIndex = 124;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 12);
            this.label3.TabIndex = 22;
            this.label3.Text = "조회 결과";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ssList
            // 
            this.ssList.AccessibleDescription = "ssList, Sheet1, Row 0, Column 0, ";
            this.ssList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssList.Location = new System.Drawing.Point(0, 154);
            this.ssList.Name = "ssList";
            namedStyle1.Border = complexBorder1;
            namedStyle1.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle2.Border = complexBorder2;
            textCellType1.Static = true;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType2.Static = true;
            namedStyle3.CellType = textCellType2;
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.Renderer = textCellType2;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3});
            this.ssList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList_Sheet1});
            this.ssList.Size = new System.Drawing.Size(655, 386);
            this.ssList.TabIndex = 125;
            this.ssList.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssList.TextTipAppearance = tipAppearance1;
            this.ssList.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // ssList_Sheet1
            // 
            this.ssList_Sheet1.Reset();
            this.ssList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssList_Sheet1.ColumnCount = 7;
            this.ssList_Sheet1.RowCount = 1;
            this.ssList_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssList_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssList_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "취소일자";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "등록번호";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "성명";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "진료과";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "취소금액";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "취소사번";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "비고";
            this.ssList_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssList_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssList_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList_Sheet1.ColumnHeader.Rows.Get(0).Height = 34F;
            this.ssList_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssList_Sheet1.Columns.Get(0).Border = emptyBorder1;
            this.ssList_Sheet1.Columns.Get(0).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssList_Sheet1.Columns.Get(0).Label = "취소일자";
            this.ssList_Sheet1.Columns.Get(0).Width = 125F;
            this.ssList_Sheet1.Columns.Get(1).Border = emptyBorder2;
            this.ssList_Sheet1.Columns.Get(1).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssList_Sheet1.Columns.Get(1).Label = "등록번호";
            this.ssList_Sheet1.Columns.Get(1).Width = 69F;
            this.ssList_Sheet1.Columns.Get(2).Border = emptyBorder3;
            this.ssList_Sheet1.Columns.Get(2).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssList_Sheet1.Columns.Get(2).Label = "성명";
            this.ssList_Sheet1.Columns.Get(2).Width = 65F;
            this.ssList_Sheet1.Columns.Get(3).Border = emptyBorder4;
            this.ssList_Sheet1.Columns.Get(3).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssList_Sheet1.Columns.Get(3).Label = "진료과";
            this.ssList_Sheet1.Columns.Get(3).Width = 46F;
            this.ssList_Sheet1.Columns.Get(4).Border = emptyBorder5;
            textCellType3.Static = true;
            this.ssList_Sheet1.Columns.Get(4).CellType = textCellType3;
            this.ssList_Sheet1.Columns.Get(4).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssList_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssList_Sheet1.Columns.Get(4).Label = "취소금액";
            this.ssList_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(4).Width = 101F;
            this.ssList_Sheet1.Columns.Get(5).Border = emptyBorder6;
            this.ssList_Sheet1.Columns.Get(5).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssList_Sheet1.Columns.Get(5).Label = "취소사번";
            this.ssList_Sheet1.Columns.Get(5).Width = 73F;
            this.ssList_Sheet1.Columns.Get(6).Border = emptyBorder7;
            this.ssList_Sheet1.Columns.Get(6).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssList_Sheet1.Columns.Get(6).Label = "비고";
            this.ssList_Sheet1.Columns.Get(6).Width = 114F;
            this.ssList_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.ssList_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.FilterBar.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssList_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList_Sheet1.Rows.Default.Height = 18F;
            this.ssList_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssList_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmPmpaViewNotJinList2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(655, 540);
            this.Controls.Add(this.ssList);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pan);
            this.Controls.Add(this.panTitle);
            this.Name = "frmPmpaViewNotJinList2";
            this.Text = "당일 접수비 수납후 미진료 환자 리스트";
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.pan.ResumeLayout(false);
            this.pan.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pan;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private FarPoint.Win.Spread.FpSpread ssList;
        private FarPoint.Win.Spread.SheetView ssList_Sheet1;
        private System.Windows.Forms.DateTimePicker dtpTDate;
        private System.Windows.Forms.DateTimePicker dtpFDate;
    }
}