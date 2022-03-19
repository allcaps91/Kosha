namespace HC_OSHA
{
    partial class FrmSanjeReport
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.SSList = new FarPoint.Win.Spread.FpSpread();
            this.SSList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panSearch = new System.Windows.Forms.Panel();
            this.btnExcel = new System.Windows.Forms.Button();
            this.cboYear = new System.Windows.Forms.ComboBox();
            this.BtnPrint = new System.Windows.Forms.Button();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.formTItle1 = new ComBase.Mvc.UserControls.FormTItle();
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cboSanje = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).BeginInit();
            this.panSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // SSList
            // 
            this.SSList.AccessibleDescription = "SSList, Sheet1, Row 0, Column 0, ";
            this.SSList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSList.Location = new System.Drawing.Point(0, 79);
            this.SSList.Name = "SSList";
            this.SSList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSList_Sheet1});
            this.SSList.Size = new System.Drawing.Size(1094, 582);
            this.SSList.TabIndex = 12;
            this.SSList.SetViewportLeftColumn(0, 0, 3);
            this.SSList.SetActiveViewport(0, 0, -1);
            // 
            // SSList_Sheet1
            // 
            this.SSList_Sheet1.Reset();
            this.SSList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSList_Sheet1.ColumnCount = 9;
            this.SSList_Sheet1.RowCount = 1;
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "발생일자";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "산재구분";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "이름";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "주민번호";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "사업장명";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "산재요양신청일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "승인일자";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "상 병";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "재해 발생 경위";
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            textCellType1.ReadOnly = true;
            this.SSList_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.SSList_Sheet1.Columns.Get(0).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SSList_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(0).Label = "발생일자";
            this.SSList_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(0).Width = 83F;
            textCellType2.ReadOnly = true;
            this.SSList_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.SSList_Sheet1.Columns.Get(1).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SSList_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(1).Label = "산재구분";
            this.SSList_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType3.ReadOnly = true;
            this.SSList_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.SSList_Sheet1.Columns.Get(2).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SSList_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(2).Label = "이름";
            this.SSList_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(2).Width = 65F;
            this.SSList_Sheet1.Columns.Get(3).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SSList_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(3).Label = "주민번호";
            this.SSList_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(3).Width = 122F;
            textCellType4.ReadOnly = true;
            this.SSList_Sheet1.Columns.Get(4).CellType = textCellType4;
            this.SSList_Sheet1.Columns.Get(4).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SSList_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SSList_Sheet1.Columns.Get(4).Label = "사업장명";
            this.SSList_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(4).Width = 102F;
            textCellType5.ReadOnly = true;
            this.SSList_Sheet1.Columns.Get(5).CellType = textCellType5;
            this.SSList_Sheet1.Columns.Get(5).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SSList_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(5).Label = "산재요양신청일";
            this.SSList_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(5).Width = 98F;
            textCellType6.ReadOnly = true;
            this.SSList_Sheet1.Columns.Get(6).CellType = textCellType6;
            this.SSList_Sheet1.Columns.Get(6).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SSList_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(6).Label = "승인일자";
            this.SSList_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(6).Width = 82F;
            textCellType7.ReadOnly = true;
            textCellType7.WordWrap = true;
            this.SSList_Sheet1.Columns.Get(7).CellType = textCellType7;
            this.SSList_Sheet1.Columns.Get(7).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SSList_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.Columns.Get(7).Label = "상 병";
            this.SSList_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(7).Width = 203F;
            textCellType8.ReadOnly = true;
            textCellType8.WordWrap = true;
            this.SSList_Sheet1.Columns.Get(8).CellType = textCellType8;
            this.SSList_Sheet1.Columns.Get(8).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SSList_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.Columns.Get(8).Label = "재해 발생 경위";
            this.SSList_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(8).Width = 219F;
            this.SSList_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.SSList_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.SSList_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.FrozenColumnCount = 3;
            this.SSList_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
            this.SSList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSList_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSList_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.SSList_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panSearch
            // 
            this.panSearch.BackColor = System.Drawing.Color.White;
            this.panSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSearch.Controls.Add(this.cboSanje);
            this.panSearch.Controls.Add(this.label1);
            this.panSearch.Controls.Add(this.btnExcel);
            this.panSearch.Controls.Add(this.cboYear);
            this.panSearch.Controls.Add(this.BtnPrint);
            this.panSearch.Controls.Add(this.BtnSearch);
            this.panSearch.Controls.Add(this.label15);
            this.panSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSearch.Location = new System.Drawing.Point(0, 35);
            this.panSearch.Name = "panSearch";
            this.panSearch.Size = new System.Drawing.Size(1094, 44);
            this.panSearch.TabIndex = 10;
            // 
            // btnExcel
            // 
            this.btnExcel.Location = new System.Drawing.Point(891, 6);
            this.btnExcel.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 28);
            this.btnExcel.TabIndex = 4;
            this.btnExcel.Text = "엑셀";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // cboYear
            // 
            this.cboYear.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.cboYear.FormattingEnabled = true;
            this.cboYear.Location = new System.Drawing.Point(62, 8);
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new System.Drawing.Size(66, 25);
            this.cboYear.TabIndex = 0;
            // 
            // BtnPrint
            // 
            this.BtnPrint.Location = new System.Drawing.Point(810, 6);
            this.BtnPrint.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnPrint.Name = "BtnPrint";
            this.BtnPrint.Size = new System.Drawing.Size(75, 28);
            this.BtnPrint.TabIndex = 3;
            this.BtnPrint.Text = "인쇄(&P)";
            this.BtnPrint.UseVisualStyleBackColor = true;
            this.BtnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(719, 8);
            this.BtnSearch.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(75, 28);
            this.BtnSearch.TabIndex = 2;
            this.BtnSearch.Text = "검색(&F)";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label15.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.Location = new System.Drawing.Point(11, 6);
            this.label15.Name = "label15";
            this.label15.Padding = new System.Windows.Forms.Padding(3);
            this.label15.Size = new System.Drawing.Size(45, 28);
            this.label15.TabIndex = 78;
            this.label15.Text = "년도";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // formTItle1
            // 
            this.formTItle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.formTItle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.formTItle1.Location = new System.Drawing.Point(0, 0);
            this.formTItle1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.formTItle1.Name = "formTItle1";
            this.formTItle1.Size = new System.Drawing.Size(1094, 35);
            this.formTItle1.TabIndex = 11;
            this.formTItle1.TitleText = "산재현황 대장";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(1010, 0);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 35);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "닫기";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(143, 8);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(3);
            this.label1.Size = new System.Drawing.Size(70, 28);
            this.label1.TabIndex = 146;
            this.label1.Text = "산재구분";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboSanje
            // 
            this.cboSanje.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.cboSanje.FormattingEnabled = true;
            this.cboSanje.Location = new System.Drawing.Point(219, 8);
            this.cboSanje.Name = "cboSanje";
            this.cboSanje.Size = new System.Drawing.Size(79, 25);
            this.cboSanje.TabIndex = 1;
            // 
            // FrmSanjeReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1094, 661);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.SSList);
            this.Controls.Add(this.panSearch);
            this.Controls.Add(this.formTItle1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmSanjeReport";
            this.Text = "산재현황대장";
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).EndInit();
            this.panSearch.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FarPoint.Win.Spread.FpSpread SSList;
        private FarPoint.Win.Spread.SheetView SSList_Sheet1;
        private System.Windows.Forms.Panel panSearch;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.ComboBox cboYear;
        private System.Windows.Forms.Button BtnPrint;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.Label label15;
        private ComBase.Mvc.UserControls.FormTItle formTItle1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ComboBox cboSanje;
        private System.Windows.Forms.Label label1;
    }
}