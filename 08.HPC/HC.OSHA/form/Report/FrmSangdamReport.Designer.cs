namespace HC_OSHA
{
    partial class FrmSangdamReport
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.SSList = new FarPoint.Win.Spread.FpSpread();
            this.SSList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panSearch = new System.Windows.Forms.Panel();
            this.TxtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboYear = new System.Windows.Forms.ComboBox();
            this.TxtLtdcode = new System.Windows.Forms.TextBox();
            this.BtnSearchSite = new System.Windows.Forms.Button();
            this.lblLTD02 = new System.Windows.Forms.Label();
            this.BtnPrint = new System.Windows.Forms.Button();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.formTItle1 = new ComBase.Mvc.UserControls.FormTItle();
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
            this.SSList.Size = new System.Drawing.Size(1023, 602);
            this.SSList.TabIndex = 6;
            // 
            // SSList_Sheet1
            // 
            this.SSList_Sheet1.Reset();
            this.SSList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSList_Sheet1.ColumnCount = 10;
            this.SSList_Sheet1.RowCount = 50;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "코드";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "회사명";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "직원ID";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "근로자명";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "부서";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "성별연령";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "일자";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "상담 지도 내용";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "상담 후 건의사항";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "상담자";
            this.SSList_Sheet1.ColumnHeader.Rows.Get(0).Height = 29F;
            textCellType1.ReadOnly = true;
            textCellType1.WordWrap = true;
            this.SSList_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.SSList_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(0).Label = "코드";
            this.SSList_Sheet1.Columns.Get(0).Width = 46F;
            textCellType2.ReadOnly = true;
            textCellType2.WordWrap = true;
            this.SSList_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.SSList_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(1).Label = "회사명";
            this.SSList_Sheet1.Columns.Get(1).Width = 88F;
            textCellType3.ReadOnly = true;
            textCellType3.WordWrap = true;
            this.SSList_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.SSList_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(2).Label = "직원ID";
            this.SSList_Sheet1.Columns.Get(2).Width = 51F;
            textCellType4.ReadOnly = true;
            textCellType4.WordWrap = true;
            this.SSList_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.SSList_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(3).Label = "근로자명";
            this.SSList_Sheet1.Columns.Get(3).Width = 64F;
            textCellType5.ReadOnly = true;
            textCellType5.WordWrap = true;
            this.SSList_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.SSList_Sheet1.Columns.Get(4).Label = "부서";
            this.SSList_Sheet1.Columns.Get(4).Width = 65F;
            textCellType6.ReadOnly = true;
            textCellType6.WordWrap = true;
            this.SSList_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.SSList_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(5).Label = "성별연령";
            this.SSList_Sheet1.Columns.Get(5).Width = 39F;
            textCellType7.ReadOnly = true;
            textCellType7.WordWrap = true;
            this.SSList_Sheet1.Columns.Get(6).CellType = textCellType7;
            this.SSList_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(6).Label = "일자";
            this.SSList_Sheet1.Columns.Get(6).Width = 47F;
            textCellType8.WordWrap = true;
            this.SSList_Sheet1.Columns.Get(7).CellType = textCellType8;
            this.SSList_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SSList_Sheet1.Columns.Get(7).Label = "상담 지도 내용";
            this.SSList_Sheet1.Columns.Get(7).Width = 262F;
            textCellType9.WordWrap = true;
            this.SSList_Sheet1.Columns.Get(8).CellType = textCellType9;
            this.SSList_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SSList_Sheet1.Columns.Get(8).Label = "상담 후 건의사항";
            this.SSList_Sheet1.Columns.Get(8).Width = 240F;
            textCellType10.ReadOnly = true;
            textCellType10.WordWrap = true;
            this.SSList_Sheet1.Columns.Get(9).CellType = textCellType10;
            this.SSList_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(9).Label = "상담자";
            this.SSList_Sheet1.Columns.Get(9).Width = 58F;
            this.SSList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panSearch
            // 
            this.panSearch.BackColor = System.Drawing.Color.White;
            this.panSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSearch.Controls.Add(this.TxtName);
            this.panSearch.Controls.Add(this.label1);
            this.panSearch.Controls.Add(this.cboYear);
            this.panSearch.Controls.Add(this.TxtLtdcode);
            this.panSearch.Controls.Add(this.BtnSearchSite);
            this.panSearch.Controls.Add(this.lblLTD02);
            this.panSearch.Controls.Add(this.BtnPrint);
            this.panSearch.Controls.Add(this.BtnSearch);
            this.panSearch.Controls.Add(this.label15);
            this.panSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSearch.Location = new System.Drawing.Point(0, 35);
            this.panSearch.Name = "panSearch";
            this.panSearch.Size = new System.Drawing.Size(1023, 44);
            this.panSearch.TabIndex = 4;
            // 
            // TxtName
            // 
            this.TxtName.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.TxtName.Location = new System.Drawing.Point(541, 9);
            this.TxtName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TxtName.Name = "TxtName";
            this.TxtName.Size = new System.Drawing.Size(90, 25);
            this.TxtName.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LightBlue;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.label1.Location = new System.Drawing.Point(444, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 28);
            this.label1.TabIndex = 144;
            this.label1.Text = "근로자 성명";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            // TxtLtdcode
            // 
            this.TxtLtdcode.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.TxtLtdcode.Location = new System.Drawing.Point(202, 8);
            this.TxtLtdcode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TxtLtdcode.Name = "TxtLtdcode";
            this.TxtLtdcode.Size = new System.Drawing.Size(136, 25);
            this.TxtLtdcode.TabIndex = 1;
            // 
            // BtnSearchSite
            // 
            this.BtnSearchSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSearchSite.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.BtnSearchSite.Location = new System.Drawing.Point(344, 5);
            this.BtnSearchSite.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearchSite.Name = "BtnSearchSite";
            this.BtnSearchSite.Size = new System.Drawing.Size(50, 28);
            this.BtnSearchSite.TabIndex = 2;
            this.BtnSearchSite.Text = "찾기";
            this.BtnSearchSite.UseVisualStyleBackColor = true;
            this.BtnSearchSite.Click += new System.EventHandler(this.BtnSearchSite_Click);
            // 
            // lblLTD02
            // 
            this.lblLTD02.BackColor = System.Drawing.Color.LightBlue;
            this.lblLTD02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLTD02.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.lblLTD02.Location = new System.Drawing.Point(148, 6);
            this.lblLTD02.Name = "lblLTD02";
            this.lblLTD02.Size = new System.Drawing.Size(50, 28);
            this.lblLTD02.TabIndex = 140;
            this.lblLTD02.Text = "회사";
            this.lblLTD02.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnPrint
            // 
            this.BtnPrint.Location = new System.Drawing.Point(810, 6);
            this.BtnPrint.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnPrint.Name = "BtnPrint";
            this.BtnPrint.Size = new System.Drawing.Size(75, 28);
            this.BtnPrint.TabIndex = 6;
            this.BtnPrint.Text = "인쇄(&P)";
            this.BtnPrint.UseVisualStyleBackColor = true;
            this.BtnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(647, 8);
            this.BtnSearch.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(75, 28);
            this.BtnSearch.TabIndex = 4;
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
            this.formTItle1.Size = new System.Drawing.Size(1023, 35);
            this.formTItle1.TabIndex = 5;
            this.formTItle1.TitleText = "사업장별 상담대장";
            // 
            // FrmSangdamReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1023, 681);
            this.Controls.Add(this.SSList);
            this.Controls.Add(this.panSearch);
            this.Controls.Add(this.formTItle1);
            this.Name = "FrmSangdamReport";
            this.Text = "FrmSangdamReport";
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).EndInit();
            this.panSearch.ResumeLayout(false);
            this.panSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private FarPoint.Win.Spread.FpSpread SSList;
        private FarPoint.Win.Spread.SheetView SSList_Sheet1;
        private System.Windows.Forms.Panel panSearch;
        private System.Windows.Forms.Button BtnPrint;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.Label label15;
        private ComBase.Mvc.UserControls.FormTItle formTItle1;
        private System.Windows.Forms.TextBox TxtLtdcode;
        private System.Windows.Forms.Button BtnSearchSite;
        private System.Windows.Forms.Label lblLTD02;
        private System.Windows.Forms.TextBox TxtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboYear;
    }
}