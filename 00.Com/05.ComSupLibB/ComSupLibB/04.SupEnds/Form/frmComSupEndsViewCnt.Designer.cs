namespace ComSupLibB.SupEnds
{
    partial class frmComSupEndsViewCnt
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
            FarPoint.Win.EmptyBorder emptyBorder1 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder2 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder3 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder4 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder5 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder6 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder7 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder8 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder9 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder10 = new FarPoint.Win.EmptyBorder();
            this.panheader4 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dtpTDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.panbtn1 = new System.Windows.Forms.Panel();
            this.btnSearch1 = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.ssList = new FarPoint.Win.Spread.FpSpread();
            this.ssList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.expandableSplitter1 = new DevComponents.DotNetBar.ExpandableSplitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ssList2 = new FarPoint.Win.Spread.FpSpread();
            this.ssList2_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panheader4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panbtn1.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList2_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panheader4
            // 
            this.panheader4.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panheader4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panheader4.Controls.Add(this.label2);
            this.panheader4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panheader4.Location = new System.Drawing.Point(0, 0);
            this.panheader4.Name = "panheader4";
            this.panheader4.Size = new System.Drawing.Size(648, 33);
            this.panheader4.TabIndex = 140;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(4, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 12);
            this.label2.TabIndex = 41;
            this.label2.Text = "검사 및 주사 건수 조회";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panbtn1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 33);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(648, 37);
            this.panel1.TabIndex = 143;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dtpTDate);
            this.panel3.Controls.Add(this.dtpFDate);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(2, 5, 3, 5);
            this.panel3.Size = new System.Drawing.Size(253, 37);
            this.panel3.TabIndex = 31;
            // 
            // dtpTDate
            // 
            this.dtpTDate.CustomFormat = "yyyy-MM-dd";
            this.dtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTDate.Location = new System.Drawing.Point(156, 6);
            this.dtpTDate.Name = "dtpTDate";
            this.dtpTDate.Size = new System.Drawing.Size(82, 21);
            this.dtpTDate.TabIndex = 8;
            // 
            // dtpFDate
            // 
            this.dtpFDate.CustomFormat = "yyyy-MM-dd";
            this.dtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFDate.Location = new System.Drawing.Point(70, 6);
            this.dtpFDate.Name = "dtpFDate";
            this.dtpFDate.Size = new System.Drawing.Size(82, 21);
            this.dtpFDate.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "발행기간";
            // 
            // panbtn1
            // 
            this.panbtn1.Controls.Add(this.btnSearch1);
            this.panbtn1.Controls.Add(this.btnPrint);
            this.panbtn1.Controls.Add(this.btnExit);
            this.panbtn1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panbtn1.Location = new System.Drawing.Point(424, 0);
            this.panbtn1.Name = "panbtn1";
            this.panbtn1.Padding = new System.Windows.Forms.Padding(3);
            this.panbtn1.Size = new System.Drawing.Size(224, 37);
            this.panbtn1.TabIndex = 30;
            // 
            // btnSearch1
            // 
            this.btnSearch1.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch1.Location = new System.Drawing.Point(11, 3);
            this.btnSearch1.Name = "btnSearch1";
            this.btnSearch1.Size = new System.Drawing.Size(70, 31);
            this.btnSearch1.TabIndex = 34;
            this.btnSearch1.Text = "조회";
            this.btnSearch1.UseVisualStyleBackColor = true;
            // 
            // btnPrint
            // 
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrint.Location = new System.Drawing.Point(81, 3);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(70, 31);
            this.btnPrint.TabIndex = 30;
            this.btnPrint.Text = "인쇄";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Visible = false;
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(151, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(70, 31);
            this.btnExit.TabIndex = 29;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.ssList);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 70);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(648, 496);
            this.panel6.TabIndex = 145;
            // 
            // ssList
            // 
            this.ssList.AccessibleDescription = "";
            this.ssList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ssList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssList.Location = new System.Drawing.Point(0, 0);
            this.ssList.Name = "ssList";
            this.ssList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList_Sheet1});
            this.ssList.Size = new System.Drawing.Size(648, 496);
            this.ssList.TabIndex = 3;
            // 
            // ssList_Sheet1
            // 
            this.ssList_Sheet1.Reset();
            this.ssList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssList_Sheet1.ColumnCount = 5;
            this.ssList_Sheet1.RowCount = 1;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.Columns.Get(0).Border = emptyBorder1;
            this.ssList_Sheet1.Columns.Get(1).Border = emptyBorder2;
            this.ssList_Sheet1.Columns.Get(2).Border = emptyBorder3;
            this.ssList_Sheet1.Columns.Get(3).Border = emptyBorder4;
            this.ssList_Sheet1.Columns.Get(4).Border = emptyBorder5;
            this.ssList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ssList_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Silver);
            this.ssList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Silver);
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // expandableSplitter1
            // 
            this.expandableSplitter1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandableSplitter1.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.expandableSplitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.expandableSplitter1.ExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.ExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.GripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.GripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter1.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(151)))), ((int)(((byte)(61)))));
            this.expandableSplitter1.HotBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(184)))), ((int)(((byte)(94)))));
            this.expandableSplitter1.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this.expandableSplitter1.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this.expandableSplitter1.HotExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.HotExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.HotGripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.HotGripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter1.Location = new System.Drawing.Point(0, 566);
            this.expandableSplitter1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.expandableSplitter1.Name = "expandableSplitter1";
            this.expandableSplitter1.Size = new System.Drawing.Size(648, 7);
            this.expandableSplitter1.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007;
            this.expandableSplitter1.TabIndex = 147;
            this.expandableSplitter1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ssList2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 573);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(648, 30);
            this.panel2.TabIndex = 148;
            // 
            // ssList2
            // 
            this.ssList2.AccessibleDescription = "";
            this.ssList2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ssList2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssList2.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssList2.Location = new System.Drawing.Point(0, 0);
            this.ssList2.Name = "ssList2";
            this.ssList2.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList2_Sheet1});
            this.ssList2.Size = new System.Drawing.Size(648, 30);
            this.ssList2.TabIndex = 3;
            // 
            // ssList2_Sheet1
            // 
            this.ssList2_Sheet1.Reset();
            this.ssList2_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssList2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssList2_Sheet1.ColumnCount = 5;
            this.ssList2_Sheet1.RowCount = 1;
            this.ssList2_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList2_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList2_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssList2_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList2_Sheet1.Columns.Get(0).Border = emptyBorder6;
            this.ssList2_Sheet1.Columns.Get(1).Border = emptyBorder7;
            this.ssList2_Sheet1.Columns.Get(2).Border = emptyBorder8;
            this.ssList2_Sheet1.Columns.Get(3).Border = emptyBorder9;
            this.ssList2_Sheet1.Columns.Get(4).Border = emptyBorder10;
            this.ssList2_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList2_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList2_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssList2_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList2_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ssList2_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Silver);
            this.ssList2_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList2_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Silver);
            this.ssList2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmComSupEndsViewCnt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(648, 603);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.expandableSplitter1);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panheader4);
            this.Name = "frmComSupEndsViewCnt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmComSupEndsViewCnt";
            this.panheader4.ResumeLayout(false);
            this.panheader4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panbtn1.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssList2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList2_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panheader4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DateTimePicker dtpTDate;
        private System.Windows.Forms.DateTimePicker dtpFDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panbtn1;
        private System.Windows.Forms.Button btnSearch1;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel6;
        private DevComponents.DotNetBar.ExpandableSplitter expandableSplitter1;
        private System.Windows.Forms.Panel panel2;
        private FarPoint.Win.Spread.FpSpread ssList;
        private FarPoint.Win.Spread.SheetView ssList_Sheet1;
        private FarPoint.Win.Spread.FpSpread ssList2;
        private FarPoint.Win.Spread.SheetView ssList2_Sheet1;
    }
}