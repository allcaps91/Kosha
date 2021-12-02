namespace ComSupLibB.SupFnEx
{
    partial class frmComSupFnExSCH02
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("BorderEx360636282841800973033", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder1 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))));
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text454636282841801063038", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder2 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))));
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Text1003636282841801203046");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.EmptyBorder emptyBorder1 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder2 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder3 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder4 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder5 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder6 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder7 = new FarPoint.Win.EmptyBorder();
            this.panheader4 = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panbtn1 = new System.Windows.Forms.Panel();
            this.btnSearch1 = new System.Windows.Forms.Button();
            this.panel20 = new System.Windows.Forms.Panel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cboYYMM = new System.Windows.Forms.ComboBox();
            this.btnSearch3 = new System.Windows.Forms.Button();
            this.btnSearch2 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ssList = new FarPoint.Win.Spread.FpSpread();
            this.ssList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panheader4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panbtn1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panheader4
            // 
            this.panheader4.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panheader4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panheader4.Controls.Add(this.lblTitle);
            this.panheader4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panheader4.Location = new System.Drawing.Point(0, 0);
            this.panheader4.Name = "panheader4";
            this.panheader4.Size = new System.Drawing.Size(1246, 33);
            this.panheader4.TabIndex = 115;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(4, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(108, 17);
            this.lblTitle.TabIndex = 41;
            this.lblTitle.Text = "ABR 스케쥴 조회";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panbtn1);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 33);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1246, 38);
            this.panel1.TabIndex = 116;
            // 
            // panbtn1
            // 
            this.panbtn1.Controls.Add(this.btnSearch1);
            this.panbtn1.Controls.Add(this.panel20);
            this.panbtn1.Controls.Add(this.btnPrint);
            this.panbtn1.Controls.Add(this.panel5);
            this.panbtn1.Controls.Add(this.btnExit);
            this.panbtn1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panbtn1.Location = new System.Drawing.Point(764, 0);
            this.panbtn1.Name = "panbtn1";
            this.panbtn1.Padding = new System.Windows.Forms.Padding(3);
            this.panbtn1.Size = new System.Drawing.Size(483, 38);
            this.panbtn1.TabIndex = 30;
            // 
            // btnSearch1
            // 
            this.btnSearch1.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch1.Location = new System.Drawing.Point(266, 3);
            this.btnSearch1.Name = "btnSearch1";
            this.btnSearch1.Size = new System.Drawing.Size(70, 32);
            this.btnSearch1.TabIndex = 34;
            this.btnSearch1.Text = "조회";
            this.btnSearch1.UseVisualStyleBackColor = true;
            // 
            // panel20
            // 
            this.panel20.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel20.Location = new System.Drawing.Point(336, 3);
            this.panel20.Name = "panel20";
            this.panel20.Size = new System.Drawing.Size(2, 32);
            this.panel20.TabIndex = 166;
            // 
            // btnPrint
            // 
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrint.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPrint.Location = new System.Drawing.Point(338, 3);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(70, 32);
            this.btnPrint.TabIndex = 30;
            this.btnPrint.Text = "인쇄";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(408, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(2, 32);
            this.panel5.TabIndex = 167;
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(410, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(70, 32);
            this.btnExit.TabIndex = 29;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnSave);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(182, 0);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(3);
            this.panel4.Size = new System.Drawing.Size(582, 38);
            this.panel4.TabIndex = 32;
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave.Location = new System.Drawing.Point(428, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(151, 32);
            this.btnSave.TabIndex = 31;
            this.btnSave.Text = "ABR 검사 스케쥴 등록";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.cboYYMM);
            this.panel3.Controls.Add(this.btnSearch3);
            this.panel3.Controls.Add(this.btnSearch2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(2, 5, 3, 5);
            this.panel3.Size = new System.Drawing.Size(182, 38);
            this.panel3.TabIndex = 35;
            // 
            // cboYYMM
            // 
            this.cboYYMM.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboYYMM.FormattingEnabled = true;
            this.cboYYMM.Location = new System.Drawing.Point(34, 5);
            this.cboYYMM.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboYYMM.Name = "cboYYMM";
            this.cboYYMM.Size = new System.Drawing.Size(117, 25);
            this.cboYYMM.TabIndex = 174;
            // 
            // btnSearch3
            // 
            this.btnSearch3.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch3.Font = new System.Drawing.Font("굴림체", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch3.Location = new System.Drawing.Point(154, 5);
            this.btnSearch3.Name = "btnSearch3";
            this.btnSearch3.Size = new System.Drawing.Size(25, 28);
            this.btnSearch3.TabIndex = 7;
            this.btnSearch3.Text = "▶";
            this.btnSearch3.UseVisualStyleBackColor = true;
            // 
            // btnSearch2
            // 
            this.btnSearch2.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSearch2.Font = new System.Drawing.Font("굴림체", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch2.Location = new System.Drawing.Point(2, 5);
            this.btnSearch2.Name = "btnSearch2";
            this.btnSearch2.Size = new System.Drawing.Size(28, 28);
            this.btnSearch2.TabIndex = 6;
            this.btnSearch2.Text = "◀";
            this.btnSearch2.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ssList);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 71);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1246, 668);
            this.panel2.TabIndex = 117;
            // 
            // ssList
            // 
            this.ssList.AccessibleDescription = "ssList, Sheet1, Row 0, Column 0, ";
            this.ssList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssList.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssList.Location = new System.Drawing.Point(0, 0);
            this.ssList.Name = "ssList";
            namedStyle1.Border = complexBorder1;
            namedStyle1.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle2.Border = complexBorder2;
            textCellType1.MaxLength = 120;
            textCellType1.Multiline = true;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType2.MaxLength = 500;
            textCellType2.Multiline = true;
            namedStyle3.CellType = textCellType2;
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.Renderer = textCellType2;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.ssList.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3});
            this.ssList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList_Sheet1});
            this.ssList.Size = new System.Drawing.Size(1246, 668);
            this.ssList.TabIndex = 87;
            this.ssList.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssList.TextTipAppearance = tipAppearance1;
            this.ssList.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Fixed;
            this.ssList.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // ssList_Sheet1
            // 
            this.ssList_Sheet1.Reset();
            this.ssList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssList_Sheet1.ColumnCount = 7;
            this.ssList_Sheet1.RowCount = 5;
            this.ssList_Sheet1.Cells.Get(0, 1).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(0, 2).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(0, 3).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(0, 4).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(0, 5).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(0, 6).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(1, 1).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(1, 2).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(1, 3).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(1, 4).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(1, 5).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(1, 6).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(2, 1).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(2, 2).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(2, 3).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(2, 4).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(2, 5).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(2, 6).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(3, 1).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(3, 2).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(3, 3).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(3, 4).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(3, 5).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(3, 6).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(4, 1).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(4, 2).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(4, 3).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(4, 4).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(4, 5).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.Cells.Get(4, 6).StyleName = "Text1003636282841801203046";
            this.ssList_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "Sun";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "Mon";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "Tue";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "Wed";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "Thu";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "Fri";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "Sat";
            this.ssList_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssList_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssList_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssList_Sheet1.Columns.Get(0).Border = emptyBorder1;
            this.ssList_Sheet1.Columns.Get(0).Label = "Sun";
            this.ssList_Sheet1.Columns.Get(0).Width = 30F;
            this.ssList_Sheet1.Columns.Get(1).Border = emptyBorder2;
            this.ssList_Sheet1.Columns.Get(1).Label = "Mon";
            this.ssList_Sheet1.Columns.Get(1).Width = 200F;
            this.ssList_Sheet1.Columns.Get(2).Border = emptyBorder3;
            this.ssList_Sheet1.Columns.Get(2).Label = "Tue";
            this.ssList_Sheet1.Columns.Get(2).Width = 200F;
            this.ssList_Sheet1.Columns.Get(3).Border = emptyBorder4;
            this.ssList_Sheet1.Columns.Get(3).Label = "Wed";
            this.ssList_Sheet1.Columns.Get(3).Width = 200F;
            this.ssList_Sheet1.Columns.Get(4).Border = emptyBorder5;
            this.ssList_Sheet1.Columns.Get(4).Label = "Thu";
            this.ssList_Sheet1.Columns.Get(4).Width = 200F;
            this.ssList_Sheet1.Columns.Get(5).Border = emptyBorder6;
            this.ssList_Sheet1.Columns.Get(5).Label = "Fri";
            this.ssList_Sheet1.Columns.Get(5).Width = 200F;
            this.ssList_Sheet1.Columns.Get(6).Border = emptyBorder7;
            this.ssList_Sheet1.Columns.Get(6).Label = "Sat";
            this.ssList_Sheet1.Columns.Get(6).Width = 200F;
            this.ssList_Sheet1.DefaultStyleName = "Text454636282841801063038";
            this.ssList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ssList_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Black);
            this.ssList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList_Sheet1.RowHeader.Visible = false;
            this.ssList_Sheet1.Rows.Get(0).Height = 125F;
            this.ssList_Sheet1.Rows.Get(1).Height = 125F;
            this.ssList_Sheet1.Rows.Get(2).Height = 125F;
            this.ssList_Sheet1.Rows.Get(3).Height = 125F;
            this.ssList_Sheet1.Rows.Get(4).Height = 125F;
            this.ssList_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Black);
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmComSupFnExSCH02
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1246, 739);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panheader4);
            this.Name = "frmComSupFnExSCH02";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmComSupFnExSchABR";
            this.panheader4.ResumeLayout(false);
            this.panheader4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panbtn1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panheader4;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        public FarPoint.Win.Spread.FpSpread ssList;
        private FarPoint.Win.Spread.SheetView ssList_Sheet1;
        private System.Windows.Forms.Panel panbtn1;
        private System.Windows.Forms.Button btnSearch1;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panel20;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox cboYYMM;
        private System.Windows.Forms.Button btnSearch3;
        private System.Windows.Forms.Button btnSearch2;
    }
}