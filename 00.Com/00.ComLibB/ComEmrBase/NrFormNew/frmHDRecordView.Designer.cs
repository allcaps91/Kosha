namespace ComEmrBase
{
    partial class frmHDRecordView
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panGraphSub = new System.Windows.Forms.Panel();
            this.chartVital = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ssList1 = new FarPoint.Win.Spread.FpSpread();
            this.ssList1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dtpEDate1 = new System.Windows.Forms.DateTimePicker();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dtpSDate1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ssList2 = new FarPoint.Win.Spread.FpSpread();
            this.ssList2_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dtpEDate2 = new System.Windows.Forms.DateTimePicker();
            this.btnSearch2 = new System.Windows.Forms.Button();
            this.dtpSDate2 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panGraphSub.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartVital)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList1_Sheet1)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList2_Sheet1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(914, 540);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panGraphSub);
            this.tabPage1.Controls.Add(this.ssList1);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(906, 514);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "AF 조회";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panGraphSub
            // 
            this.panGraphSub.Controls.Add(this.chartVital);
            this.panGraphSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panGraphSub.Location = new System.Drawing.Point(282, 39);
            this.panGraphSub.Name = "panGraphSub";
            this.panGraphSub.Size = new System.Drawing.Size(621, 472);
            this.panGraphSub.TabIndex = 35;
            // 
            // chartVital
            // 
            chartArea1.Name = "ChartArea1";
            this.chartVital.ChartAreas.Add(chartArea1);
            this.chartVital.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chartVital.Legends.Add(legend1);
            this.chartVital.Location = new System.Drawing.Point(0, 0);
            this.chartVital.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chartVital.Name = "chartVital";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series1.Legend = "Legend1";
            series1.MarkerColor = System.Drawing.Color.Red;
            series1.MarkerSize = 7;
            series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series1.Name = "Series1";
            series1.YValuesPerPoint = 6;
            this.chartVital.Series.Add(series1);
            this.chartVital.Size = new System.Drawing.Size(621, 472);
            this.chartVital.TabIndex = 33;
            this.chartVital.Text = "chart1";
            // 
            // ssList1
            // 
            this.ssList1.AccessibleDescription = "ssList1, Sheet1, Row 0, Column 0, 2020-01-01";
            this.ssList1.Dock = System.Windows.Forms.DockStyle.Left;
            this.ssList1.Location = new System.Drawing.Point(3, 39);
            this.ssList1.Name = "ssList1";
            this.ssList1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList1_Sheet1});
            this.ssList1.Size = new System.Drawing.Size(279, 472);
            this.ssList1.TabIndex = 3;
            // 
            // ssList1_Sheet1
            // 
            this.ssList1_Sheet1.Reset();
            this.ssList1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssList1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssList1_Sheet1.ColumnCount = 3;
            this.ssList1_Sheet1.RowCount = 1;
            this.ssList1_Sheet1.Cells.Get(0, 0).Value = "2020-01-01";
            this.ssList1_Sheet1.Cells.Get(0, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList1_Sheet1.Cells.Get(0, 1).Value = "11:11";
            this.ssList1_Sheet1.Cells.Get(0, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "일자";
            this.ssList1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "시간";
            this.ssList1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "수치";
            this.ssList1_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssList1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList1_Sheet1.Columns.Get(0).Label = "일자";
            this.ssList1_Sheet1.Columns.Get(0).Locked = true;
            this.ssList1_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList1_Sheet1.Columns.Get(0).Width = 100F;
            this.ssList1_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssList1_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList1_Sheet1.Columns.Get(1).Label = "시간";
            this.ssList1_Sheet1.Columns.Get(1).Locked = true;
            this.ssList1_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList1_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssList1_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList1_Sheet1.Columns.Get(2).Label = "수치";
            this.ssList1_Sheet1.Columns.Get(2).Locked = true;
            this.ssList1_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.dtpEDate1);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.dtpSDate1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(900, 36);
            this.panel1.TabIndex = 2;
            // 
            // dtpEDate1
            // 
            this.dtpEDate1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEDate1.Location = new System.Drawing.Point(115, 7);
            this.dtpEDate1.Name = "dtpEDate1";
            this.dtpEDate1.Size = new System.Drawing.Size(97, 21);
            this.dtpEDate1.TabIndex = 78;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(214, 2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(62, 30);
            this.btnSearch.TabIndex = 79;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dtpSDate1
            // 
            this.dtpSDate1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpSDate1.Location = new System.Drawing.Point(4, 7);
            this.dtpSDate1.Name = "dtpSDate1";
            this.dtpSDate1.Size = new System.Drawing.Size(97, 21);
            this.dtpSDate1.TabIndex = 80;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(101, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 12);
            this.label2.TabIndex = 81;
            this.label2.Text = "~";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ssList2);
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(906, 514);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Spkt/V 조회";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ssList2
            // 
            this.ssList2.AccessibleDescription = "ssList1, Sheet1, Row 0, Column 0, 2020-01-01";
            this.ssList2.Dock = System.Windows.Forms.DockStyle.Left;
            this.ssList2.Location = new System.Drawing.Point(3, 39);
            this.ssList2.Name = "ssList2";
            this.ssList2.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList2_Sheet1});
            this.ssList2.Size = new System.Drawing.Size(279, 472);
            this.ssList2.TabIndex = 4;
            // 
            // ssList2_Sheet1
            // 
            this.ssList2_Sheet1.Reset();
            this.ssList2_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssList2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssList2_Sheet1.ColumnCount = 3;
            this.ssList2_Sheet1.RowCount = 1;
            this.ssList2_Sheet1.Cells.Get(0, 0).Value = "2020-01-01";
            this.ssList2_Sheet1.Cells.Get(0, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList2_Sheet1.Cells.Get(0, 1).Value = "11:11";
            this.ssList2_Sheet1.Cells.Get(0, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList2_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList2_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList2_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssList2_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList2_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "일자";
            this.ssList2_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "시간";
            this.ssList2_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "수치";
            this.ssList2_Sheet1.Columns.Get(0).CellType = textCellType4;
            this.ssList2_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList2_Sheet1.Columns.Get(0).Label = "일자";
            this.ssList2_Sheet1.Columns.Get(0).Locked = true;
            this.ssList2_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList2_Sheet1.Columns.Get(0).Width = 100F;
            this.ssList2_Sheet1.Columns.Get(1).CellType = textCellType5;
            this.ssList2_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList2_Sheet1.Columns.Get(1).Label = "시간";
            this.ssList2_Sheet1.Columns.Get(1).Locked = true;
            this.ssList2_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList2_Sheet1.Columns.Get(2).CellType = textCellType6;
            this.ssList2_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList2_Sheet1.Columns.Get(2).Label = "수치";
            this.ssList2_Sheet1.Columns.Get(2).Locked = true;
            this.ssList2_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList2_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList2_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList2_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssList2_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList2_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.dtpEDate2);
            this.panel2.Controls.Add(this.btnSearch2);
            this.panel2.Controls.Add(this.dtpSDate2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(900, 36);
            this.panel2.TabIndex = 3;
            // 
            // dtpEDate2
            // 
            this.dtpEDate2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEDate2.Location = new System.Drawing.Point(115, 7);
            this.dtpEDate2.Name = "dtpEDate2";
            this.dtpEDate2.Size = new System.Drawing.Size(97, 21);
            this.dtpEDate2.TabIndex = 78;
            // 
            // btnSearch2
            // 
            this.btnSearch2.Location = new System.Drawing.Point(214, 2);
            this.btnSearch2.Name = "btnSearch2";
            this.btnSearch2.Size = new System.Drawing.Size(62, 30);
            this.btnSearch2.TabIndex = 79;
            this.btnSearch2.Text = "조회";
            this.btnSearch2.UseVisualStyleBackColor = true;
            this.btnSearch2.Click += new System.EventHandler(this.btnSearch2_Click);
            // 
            // dtpSDate2
            // 
            this.dtpSDate2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpSDate2.Location = new System.Drawing.Point(4, 7);
            this.dtpSDate2.Name = "dtpSDate2";
            this.dtpSDate2.Size = new System.Drawing.Size(97, 21);
            this.dtpSDate2.TabIndex = 80;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(101, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 12);
            this.label1.TabIndex = 81;
            this.label1.Text = "~";
            // 
            // frmHDRecordView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 540);
            this.Controls.Add(this.tabControl1);
            this.Name = "frmHDRecordView";
            this.Text = "frmHDRecordView";
            this.Load += new System.EventHandler(this.frmHDRecordView_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panGraphSub.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartVital)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList1_Sheet1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssList2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList2_Sheet1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker dtpEDate1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DateTimePicker dtpSDate1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabPage2;
        private FarPoint.Win.Spread.FpSpread ssList1;
        private FarPoint.Win.Spread.SheetView ssList1_Sheet1;
        private FarPoint.Win.Spread.FpSpread ssList2;
        private FarPoint.Win.Spread.SheetView ssList2_Sheet1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DateTimePicker dtpEDate2;
        private System.Windows.Forms.Button btnSearch2;
        private System.Windows.Forms.DateTimePicker dtpSDate2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panGraphSub;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartVital;
    }
}