namespace ComEmrBase
{
    partial class frmEmrNrCatheterView
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
            this.ssChart = new FarPoint.Win.Spread.FpSpread();
            this.ssChart_Sheet1 = new FarPoint.Win.Spread.SheetView();
            ((System.ComponentModel.ISupportInitialize)(this.ssChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssChart_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // ssChart
            // 
            this.ssChart.AccessibleDescription = "ssChart, Sheet1, Row 0, Column 0, 기본간호활동";
            this.ssChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssChart.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssChart.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssChart.Location = new System.Drawing.Point(0, 0);
            this.ssChart.Name = "ssChart";
            this.ssChart.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssChart_Sheet1});
            this.ssChart.Size = new System.Drawing.Size(775, 513);
            this.ssChart.TabIndex = 160;
            this.ssChart.SetViewportLeftColumn(0, 0, 2);
            this.ssChart.SetActiveViewport(0, 0, -1);
            // 
            // ssChart_Sheet1
            // 
            this.ssChart_Sheet1.Reset();
            this.ssChart_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssChart_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssChart_Sheet1.ColumnCount = 5;
            this.ssChart_Sheet1.RowCount = 1;
            this.ssChart_Sheet1.ActiveSkin = new FarPoint.Win.Spread.SheetSkin("CustomSkin1", System.Drawing.Color.White, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.LightGray, FarPoint.Win.Spread.GridLines.Both, System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237))))), System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, false, false, false, false, false, true, false, true, "HeaderDefault", "HeaderDefault", "HeaderDefault", "DataAreaDefault", "HeaderDefault");
            this.ssChart_Sheet1.Cells.Get(0, 0).Value = "기본간호활동";
            this.ssChart_Sheet1.Cells.Get(0, 2).Value = "기본간호활동";
            this.ssChart_Sheet1.Cells.Get(0, 3).Value = "1";
            this.ssChart_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssChart_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssChart_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssChart_Sheet1.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.ssChart_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssChart_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssChart_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssChart_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssChart_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssChart_Sheet1.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.ssChart_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssChart_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssChart_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "항목";
            this.ssChart_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "종류";
            this.ssChart_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "삽입일";
            this.ssChart_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "유지일";
            this.ssChart_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "최근 제거일";
            this.ssChart_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssChart_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssChart_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssChart_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssChart_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssChart_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssChart_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssChart_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssChart_Sheet1.ColumnHeader.Rows.Get(0).Height = 30F;
            textCellType1.MaxLength = 400;
            textCellType1.Multiline = true;
            textCellType1.WordWrap = true;
            this.ssChart_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssChart_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssChart_Sheet1.Columns.Get(0).Label = "항목";
            this.ssChart_Sheet1.Columns.Get(0).Locked = true;
            this.ssChart_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssChart_Sheet1.Columns.Get(0).Width = 250F;
            this.ssChart_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssChart_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssChart_Sheet1.Columns.Get(1).Label = "종류";
            this.ssChart_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssChart_Sheet1.Columns.Get(1).Width = 200F;
            textCellType3.MaxLength = 400;
            textCellType3.Multiline = true;
            textCellType3.WordWrap = true;
            this.ssChart_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssChart_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssChart_Sheet1.Columns.Get(2).Label = "삽입일";
            this.ssChart_Sheet1.Columns.Get(2).Locked = true;
            this.ssChart_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssChart_Sheet1.Columns.Get(2).Width = 100F;
            this.ssChart_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssChart_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssChart_Sheet1.Columns.Get(3).Label = "유지일";
            this.ssChart_Sheet1.Columns.Get(3).Locked = true;
            this.ssChart_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssChart_Sheet1.Columns.Get(3).Width = 100F;
            this.ssChart_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.ssChart_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssChart_Sheet1.Columns.Get(4).Label = "최근 제거일";
            this.ssChart_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssChart_Sheet1.Columns.Get(4).Width = 100F;
            this.ssChart_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssChart_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssChart_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.ssChart_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssChart_Sheet1.FilterBar.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssChart_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssChart_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssChart_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssChart_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssChart_Sheet1.FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssChart_Sheet1.FrozenColumnCount = 2;
            this.ssChart_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssChart_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssChart_Sheet1.RowHeader.Columns.Get(0).Width = 29F;
            this.ssChart_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssChart_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssChart_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssChart_Sheet1.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssChart_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssChart_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssChart_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssChart_Sheet1.RowHeader.Visible = false;
            this.ssChart_Sheet1.Rows.Get(0).Height = 38F;
            this.ssChart_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssChart_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssChart_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssChart_Sheet1.SheetCornerStyle.Parent = "HeaderDefault";
            this.ssChart_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssChart_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssChart_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmEmrNrCatheterView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 513);
            this.Controls.Add(this.ssChart);
            this.Name = "frmEmrNrCatheterView";
            this.Text = "frmEmrNrCatheterView";
            this.Load += new System.EventHandler(this.frmEmrNrCatheterView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ssChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssChart_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private FarPoint.Win.Spread.FpSpread ssChart;
        private FarPoint.Win.Spread.SheetView ssChart_Sheet1;
    }
}