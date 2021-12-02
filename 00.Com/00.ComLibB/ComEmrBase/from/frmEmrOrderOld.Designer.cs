namespace ComEmrBase
{
    partial class frmEmrOrderOld
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType41 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType42 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType43 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType44 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType45 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType46 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType47 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType48 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType49 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType50 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.mbtnPrint = new System.Windows.Forms.Button();
            this.spcChart = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.mbtnSearch = new System.Windows.Forms.Button();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.spcChart)).BeginInit();
            this.spcChart.Panel2.SuspendLayout();
            this.spcChart.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mbtnPrint
            // 
            this.mbtnPrint.Location = new System.Drawing.Point(354, 1);
            this.mbtnPrint.Name = "mbtnPrint";
            this.mbtnPrint.Size = new System.Drawing.Size(62, 28);
            this.mbtnPrint.TabIndex = 37;
            this.mbtnPrint.Text = "출  력";
            this.mbtnPrint.UseVisualStyleBackColor = true;
            this.mbtnPrint.Click += new System.EventHandler(this.mbtnPrint_Click);
            // 
            // spcChart
            // 
            this.spcChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcChart.Location = new System.Drawing.Point(0, 0);
            this.spcChart.Name = "spcChart";
            this.spcChart.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.spcChart.Panel1Collapsed = true;
            // 
            // spcChart.Panel2
            // 
            this.spcChart.Panel2.Controls.Add(this.panel2);
            this.spcChart.Size = new System.Drawing.Size(686, 790);
            this.spcChart.SplitterDistance = 159;
            this.spcChart.SplitterWidth = 10;
            this.spcChart.TabIndex = 108;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ssView);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(686, 790);
            this.panel2.TabIndex = 4;
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "ssView, Sheet1, Row 0, Column 0, ";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.Location = new System.Drawing.Point(0, 34);
            this.ssView.Name = "ssView";
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(686, 756);
            this.ssView.TabIndex = 5;
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 10;
            this.ssView_Sheet1.RowCount = 1;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "처방일자";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "처방코드";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "처방명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "일용량";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "일투량";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "용법/검체";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "Mix";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "횟수";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "일수";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "Sign";
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 40F;
            this.ssView_Sheet1.Columns.Get(0).CellType = textCellType41;
            this.ssView_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssView_Sheet1.Columns.Get(0).Label = "처방일자";
            this.ssView_Sheet1.Columns.Get(0).Width = 80F;
            this.ssView_Sheet1.Columns.Get(1).CellType = textCellType42;
            this.ssView_Sheet1.Columns.Get(1).Label = "처방코드";
            this.ssView_Sheet1.Columns.Get(2).CellType = textCellType43;
            this.ssView_Sheet1.Columns.Get(2).Label = "처방명";
            this.ssView_Sheet1.Columns.Get(2).Width = 260F;
            this.ssView_Sheet1.Columns.Get(3).CellType = textCellType44;
            this.ssView_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(3).Label = "일용량";
            this.ssView_Sheet1.Columns.Get(3).Width = 50F;
            this.ssView_Sheet1.Columns.Get(4).CellType = textCellType45;
            this.ssView_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(4).Label = "일투량";
            this.ssView_Sheet1.Columns.Get(4).Width = 50F;
            this.ssView_Sheet1.Columns.Get(5).CellType = textCellType46;
            this.ssView_Sheet1.Columns.Get(5).Label = "용법/검체";
            this.ssView_Sheet1.Columns.Get(5).Width = 80F;
            this.ssView_Sheet1.Columns.Get(6).CellType = textCellType47;
            this.ssView_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(6).Label = "Mix";
            this.ssView_Sheet1.Columns.Get(6).Width = 30F;
            this.ssView_Sheet1.Columns.Get(7).CellType = textCellType48;
            this.ssView_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(7).Label = "횟수";
            this.ssView_Sheet1.Columns.Get(7).Width = 28F;
            this.ssView_Sheet1.Columns.Get(8).CellType = textCellType49;
            this.ssView_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(8).Label = "일수";
            this.ssView_Sheet1.Columns.Get(8).Width = 28F;
            this.ssView_Sheet1.Columns.Get(9).CellType = textCellType50;
            this.ssView_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(9).Label = "Sign";
            this.ssView_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssView_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.mbtnSearch);
            this.panel1.Controls.Add(this.dateTimePicker2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.dateTimePicker1);
            this.panel1.Controls.Add(this.mbtnPrint);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(686, 34);
            this.panel1.TabIndex = 4;
            // 
            // mbtnSearch
            // 
            this.mbtnSearch.Location = new System.Drawing.Point(289, 1);
            this.mbtnSearch.Name = "mbtnSearch";
            this.mbtnSearch.Size = new System.Drawing.Size(62, 28);
            this.mbtnSearch.TabIndex = 39;
            this.mbtnSearch.Text = "조  회";
            this.mbtnSearch.UseVisualStyleBackColor = true;
            this.mbtnSearch.Click += new System.EventHandler(this.mbtnSearch_Click);
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(185, 5);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(98, 21);
            this.dateTimePicker2.TabIndex = 38;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 37;
            this.label1.Text = "작성일자";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(75, 5);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(98, 21);
            this.dateTimePicker1.TabIndex = 36;
            // 
            // frmEmrOrderOld
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 790);
            this.Controls.Add(this.spcChart);
            this.Name = "frmEmrOrderOld";
            this.Text = "frmEmrOrderOld";
            this.Load += new System.EventHandler(this.frmEmrOrderOld_Load);
            this.spcChart.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spcChart)).EndInit();
            this.spcChart.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.Button mbtnPrint;
        private System.Windows.Forms.SplitContainer spcChart;
        private System.Windows.Forms.Panel panel2;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Button mbtnSearch;
        public System.Windows.Forms.DateTimePicker dateTimePicker2;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.DateTimePicker dateTimePicker1;
    }
}