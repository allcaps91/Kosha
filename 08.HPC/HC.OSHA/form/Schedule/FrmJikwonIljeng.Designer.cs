namespace HC_OSHA
{
    partial class FrmJikwonIljeng
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
            FarPoint.Win.Spread.CellType.GeneralCellType generalCellType1 = new FarPoint.Win.Spread.CellType.GeneralCellType();
            this.SSList = new FarPoint.Win.Spread.FpSpread();
            this.SSList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panSearch = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnExcel1 = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnExcel = new System.Windows.Forms.Button();
            this.cboYYMM = new System.Windows.Forms.ComboBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
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
            this.SSList.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SSList.Location = new System.Drawing.Point(0, 86);
            this.SSList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SSList.Name = "SSList";
            this.SSList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSList_Sheet1});
            this.SSList.Size = new System.Drawing.Size(1169, 734);
            this.SSList.TabIndex = 19;
            this.SSList.SetViewportLeftColumn(0, 0, 2);
            this.SSList.SetActiveViewport(0, -1, -1);
            // 
            // SSList_Sheet1
            // 
            this.SSList_Sheet1.Reset();
            this.SSList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSList_Sheet1.ColumnCount = 3;
            this.SSList_Sheet1.RowCount = 0;
            this.SSList_Sheet1.ActiveColumnIndex = -1;
            this.SSList_Sheet1.ActiveRowIndex = -1;
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "구분";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "성명";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "3/1 (월)";
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            textCellType1.ReadOnly = true;
            this.SSList_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.SSList_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(0).Label = "구분";
            this.SSList_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(0).Width = 56F;
            textCellType2.ReadOnly = true;
            this.SSList_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.SSList_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(1).Label = "성명";
            this.SSList_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(1).Width = 57F;
            generalCellType1.ReadOnly = true;
            generalCellType1.WordWrap = true;
            this.SSList_Sheet1.Columns.Get(2).CellType = generalCellType1;
            this.SSList_Sheet1.Columns.Get(2).Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.SSList_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SSList_Sheet1.Columns.Get(2).Label = "3/1 (월)";
            this.SSList_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(2).Width = 123F;
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
            this.SSList_Sheet1.FrozenColumnCount = 2;
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
            this.panSearch.Controls.Add(this.btnExit);
            this.panSearch.Controls.Add(this.btnExcel1);
            this.panSearch.Controls.Add(this.btnSave);
            this.panSearch.Controls.Add(this.btnExcel);
            this.panSearch.Controls.Add(this.cboYYMM);
            this.panSearch.Controls.Add(this.btnPrint);
            this.panSearch.Controls.Add(this.btnSearch);
            this.panSearch.Controls.Add(this.label15);
            this.panSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSearch.Location = new System.Drawing.Point(0, 43);
            this.panSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panSearch.Name = "panSearch";
            this.panSearch.Size = new System.Drawing.Size(1169, 43);
            this.panSearch.TabIndex = 20;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(1069, 5);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 28);
            this.btnExit.TabIndex = 81;
            this.btnExit.Text = "닫기(&X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnExcel1
            // 
            this.btnExcel1.Location = new System.Drawing.Point(966, 5);
            this.btnExcel1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.btnExcel1.Name = "btnExcel1";
            this.btnExcel1.Size = new System.Drawing.Size(75, 28);
            this.btnExcel1.TabIndex = 80;
            this.btnExcel1.Text = "엑셀저장";
            this.btnExcel1.UseVisualStyleBackColor = true;
            this.btnExcel1.Click += new System.EventHandler(this.btnExcel1_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(804, 3);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 28);
            this.btnSave.TabIndex = 79;
            this.btnSave.Text = "저장(&S)";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.Location = new System.Drawing.Point(1159, 8);
            this.btnExcel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 40);
            this.btnExcel.TabIndex = 4;
            this.btnExcel.Text = "엑셀";
            this.btnExcel.UseVisualStyleBackColor = true;
            // 
            // cboYYMM
            // 
            this.cboYYMM.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.cboYYMM.FormattingEnabled = true;
            this.cboYYMM.Location = new System.Drawing.Point(85, 8);
            this.cboYYMM.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboYYMM.Name = "cboYYMM";
            this.cboYYMM.Size = new System.Drawing.Size(94, 25);
            this.cboYYMM.TabIndex = 1;
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(885, 5);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 28);
            this.btnPrint.TabIndex = 3;
            this.btnPrint.Text = "인쇄(&P)";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(197, 8);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 25);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "검색(&F)";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label15.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.Location = new System.Drawing.Point(11, 8);
            this.label15.Name = "label15";
            this.label15.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label15.Size = new System.Drawing.Size(68, 27);
            this.label15.TabIndex = 78;
            this.label15.Text = "월별";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // formTItle1
            // 
            this.formTItle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.formTItle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.formTItle1.Location = new System.Drawing.Point(0, 0);
            this.formTItle1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.formTItle1.Name = "formTItle1";
            this.formTItle1.Size = new System.Drawing.Size(1169, 43);
            this.formTItle1.TabIndex = 21;
            this.formTItle1.TitleText = "직원 일정 등록";
            // 
            // FrmJikwonIljeng
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1169, 820);
            this.Controls.Add(this.SSList);
            this.Controls.Add(this.panSearch);
            this.Controls.Add(this.formTItle1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmJikwonIljeng";
            this.Text = "직원일정등록";
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).EndInit();
            this.panSearch.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FarPoint.Win.Spread.FpSpread SSList;
        private FarPoint.Win.Spread.SheetView SSList_Sheet1;
        private System.Windows.Forms.Panel panSearch;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnExcel1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.ComboBox cboYYMM;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label15;
        private ComBase.Mvc.UserControls.FormTItle formTItle1;
    }
}