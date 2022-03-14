namespace HC_OSHA.form.Visit
{
    partial class FrmVisitCheck
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType13 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType14 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType15 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType16 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType17 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType18 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType19 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType20 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType21 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.SSList = new FarPoint.Win.Spread.FpSpread();
            this.SSList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panSearch = new System.Windows.Forms.Panel();
            this.BtnPrint = new System.Windows.Forms.Button();
            this.chk정상방문 = new System.Windows.Forms.CheckBox();
            this.cboVisit = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboYear = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.contentTitle1 = new ComBase.Mvc.UserControls.ContentTitle();
            this.BtnExit = new System.Windows.Forms.Button();
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
            this.SSList.Location = new System.Drawing.Point(0, 80);
            this.SSList.Name = "SSList";
            this.SSList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSList_Sheet1});
            this.SSList.Size = new System.Drawing.Size(1037, 580);
            this.SSList.TabIndex = 4;
            this.SSList.SetViewportLeftColumn(0, 0, 2);
            this.SSList.SetActiveViewport(0, 0, -1);
            // 
            // SSList_Sheet1
            // 
            this.SSList_Sheet1.Reset();
            this.SSList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSList_Sheet1.ColumnCount = 21;
            this.SSList_Sheet1.RowCount = 10;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "코드";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "회사명";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "계약시작일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "계약종료일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "담당자";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "인원";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "주기";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "방문횟수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "1월";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "2월";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "3월";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "4월";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "5월";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "6월";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "7월";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 15).Value = "8월";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 16).Value = "9월";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 17).Value = "10월";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 18).Value = "11월";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 19).Value = "12월";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 20).Value = "비고";
            this.SSList_Sheet1.ColumnHeader.Rows.Get(0).Height = 35F;
            this.SSList_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.SSList_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(0).Label = "코드";
            this.SSList_Sheet1.Columns.Get(0).Width = 48F;
            this.SSList_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.SSList_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SSList_Sheet1.Columns.Get(1).Label = "회사명";
            this.SSList_Sheet1.Columns.Get(1).Width = 86F;
            this.SSList_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.SSList_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(2).Label = "계약시작일";
            this.SSList_Sheet1.Columns.Get(2).Width = 73F;
            this.SSList_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.SSList_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(3).Label = "계약종료일";
            this.SSList_Sheet1.Columns.Get(3).Width = 72F;
            this.SSList_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.SSList_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(4).Label = "담당자";
            this.SSList_Sheet1.Columns.Get(4).Width = 54F;
            this.SSList_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.SSList_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(5).Label = "인원";
            this.SSList_Sheet1.Columns.Get(5).Width = 37F;
            this.SSList_Sheet1.Columns.Get(6).CellType = textCellType7;
            this.SSList_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(6).Label = "주기";
            this.SSList_Sheet1.Columns.Get(6).Width = 38F;
            this.SSList_Sheet1.Columns.Get(7).CellType = textCellType8;
            this.SSList_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(7).Label = "방문횟수";
            this.SSList_Sheet1.Columns.Get(7).Width = 38F;
            this.SSList_Sheet1.Columns.Get(8).CellType = textCellType9;
            this.SSList_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(8).Label = "1월";
            this.SSList_Sheet1.Columns.Get(8).Width = 38F;
            this.SSList_Sheet1.Columns.Get(9).CellType = textCellType10;
            this.SSList_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(9).Label = "2월";
            this.SSList_Sheet1.Columns.Get(9).Width = 38F;
            this.SSList_Sheet1.Columns.Get(10).CellType = textCellType11;
            this.SSList_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(10).Label = "3월";
            this.SSList_Sheet1.Columns.Get(10).Width = 38F;
            this.SSList_Sheet1.Columns.Get(11).CellType = textCellType12;
            this.SSList_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(11).Label = "4월";
            this.SSList_Sheet1.Columns.Get(11).Width = 38F;
            this.SSList_Sheet1.Columns.Get(12).CellType = textCellType13;
            this.SSList_Sheet1.Columns.Get(12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(12).Label = "5월";
            this.SSList_Sheet1.Columns.Get(12).Width = 38F;
            this.SSList_Sheet1.Columns.Get(13).CellType = textCellType14;
            this.SSList_Sheet1.Columns.Get(13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(13).Label = "6월";
            this.SSList_Sheet1.Columns.Get(13).Width = 38F;
            this.SSList_Sheet1.Columns.Get(14).CellType = textCellType15;
            this.SSList_Sheet1.Columns.Get(14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(14).Label = "7월";
            this.SSList_Sheet1.Columns.Get(14).Width = 38F;
            this.SSList_Sheet1.Columns.Get(15).CellType = textCellType16;
            this.SSList_Sheet1.Columns.Get(15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(15).Label = "8월";
            this.SSList_Sheet1.Columns.Get(15).Width = 38F;
            this.SSList_Sheet1.Columns.Get(16).CellType = textCellType17;
            this.SSList_Sheet1.Columns.Get(16).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(16).Label = "9월";
            this.SSList_Sheet1.Columns.Get(16).Width = 38F;
            this.SSList_Sheet1.Columns.Get(17).CellType = textCellType18;
            this.SSList_Sheet1.Columns.Get(17).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(17).Label = "10월";
            this.SSList_Sheet1.Columns.Get(17).Width = 38F;
            this.SSList_Sheet1.Columns.Get(18).CellType = textCellType19;
            this.SSList_Sheet1.Columns.Get(18).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(18).Label = "11월";
            this.SSList_Sheet1.Columns.Get(18).Width = 38F;
            this.SSList_Sheet1.Columns.Get(19).CellType = textCellType20;
            this.SSList_Sheet1.Columns.Get(19).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(19).Label = "12월";
            this.SSList_Sheet1.Columns.Get(19).Width = 38F;
            this.SSList_Sheet1.Columns.Get(20).CellType = textCellType21;
            this.SSList_Sheet1.Columns.Get(20).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SSList_Sheet1.Columns.Get(20).Label = "비고";
            this.SSList_Sheet1.Columns.Get(20).Width = 71F;
            this.SSList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.FrozenColumnCount = 2;
            this.SSList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panSearch
            // 
            this.panSearch.BackColor = System.Drawing.Color.White;
            this.panSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSearch.Controls.Add(this.BtnPrint);
            this.panSearch.Controls.Add(this.chk정상방문);
            this.panSearch.Controls.Add(this.cboVisit);
            this.panSearch.Controls.Add(this.label1);
            this.panSearch.Controls.Add(this.cboYear);
            this.panSearch.Controls.Add(this.label15);
            this.panSearch.Controls.Add(this.BtnSearch);
            this.panSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSearch.Location = new System.Drawing.Point(0, 38);
            this.panSearch.Name = "panSearch";
            this.panSearch.Size = new System.Drawing.Size(1037, 42);
            this.panSearch.TabIndex = 5;
            // 
            // BtnPrint
            // 
            this.BtnPrint.Location = new System.Drawing.Point(697, 6);
            this.BtnPrint.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnPrint.Name = "BtnPrint";
            this.BtnPrint.Size = new System.Drawing.Size(75, 28);
            this.BtnPrint.TabIndex = 83;
            this.BtnPrint.Text = "인쇄(&P)";
            this.BtnPrint.UseVisualStyleBackColor = true;
            this.BtnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // chk정상방문
            // 
            this.chk정상방문.AutoSize = true;
            this.chk정상방문.Location = new System.Drawing.Point(390, 14);
            this.chk정상방문.Name = "chk정상방문";
            this.chk정상방문.Size = new System.Drawing.Size(184, 16);
            this.chk정상방문.TabIndex = 3;
            this.chk정상방문.Text = "방문 누락이 없으면 표시 제외";
            this.chk정상방문.UseVisualStyleBackColor = true;
            // 
            // cboVisit
            // 
            this.cboVisit.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.cboVisit.FormattingEnabled = true;
            this.cboVisit.Location = new System.Drawing.Point(203, 7);
            this.cboVisit.Name = "cboVisit";
            this.cboVisit.Size = new System.Drawing.Size(81, 25);
            this.cboVisit.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(134, 5);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(3);
            this.label1.Size = new System.Drawing.Size(63, 28);
            this.label1.TabIndex = 82;
            this.label1.Text = "방문자";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboYear
            // 
            this.cboYear.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.cboYear.FormattingEnabled = true;
            this.cboYear.Location = new System.Drawing.Point(62, 9);
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new System.Drawing.Size(66, 25);
            this.cboYear.TabIndex = 0;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label15.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.Location = new System.Drawing.Point(11, 7);
            this.label15.Name = "label15";
            this.label15.Padding = new System.Windows.Forms.Padding(3);
            this.label15.Size = new System.Drawing.Size(45, 28);
            this.label15.TabIndex = 80;
            this.label15.Text = "년도";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(300, 6);
            this.BtnSearch.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(75, 28);
            this.BtnSearch.TabIndex = 2;
            this.BtnSearch.Text = "검색";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // contentTitle1
            // 
            this.contentTitle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.contentTitle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.contentTitle1.Location = new System.Drawing.Point(0, 0);
            this.contentTitle1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.contentTitle1.Name = "contentTitle1";
            this.contentTitle1.Size = new System.Drawing.Size(1037, 38);
            this.contentTitle1.TabIndex = 4;
            this.contentTitle1.TitleText = "방문날짜 점검 리스트";
            // 
            // BtnExit
            // 
            this.BtnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnExit.Location = new System.Drawing.Point(950, 2);
            this.BtnExit.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(75, 28);
            this.BtnExit.TabIndex = 18;
            this.BtnExit.Text = "닫 기(&S)";
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // FrmVisitCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1037, 660);
            this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.SSList);
            this.Controls.Add(this.panSearch);
            this.Controls.Add(this.contentTitle1);
            this.Name = "FrmVisitCheck";
            this.Text = "FrmVisitCheck";
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
        private System.Windows.Forms.Button BtnSearch;
        private ComBase.Mvc.UserControls.ContentTitle contentTitle1;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.CheckBox chk정상방문;
        private System.Windows.Forms.ComboBox cboVisit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboYear;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button BtnPrint;
    }
}