namespace ComPmpaLibB
{
    partial class frmPmpaViewSearchSuga
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color437636414533272515814", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text501636414533272515814", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static605636414533272515814");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Font621636414533272515814");
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("Static657636414533272515814");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle6 = new FarPoint.Win.Spread.NamedStyle("Number729636414533272515814");
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType1 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnView = new System.Windows.Forms.Button();
            this.ssList = new FarPoint.Win.Spread.FpSpread();
            this.ssList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.optGbn0 = new System.Windows.Forms.RadioButton();
            this.optGbn1 = new System.Windows.Forms.RadioButton();
            this.txtSuga = new System.Windows.Forms.TextBox();
            this.panel3.SuspendLayout();
            this.panTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 40);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(864, 28);
            this.panel3.TabIndex = 140;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 17);
            this.label1.TabIndex = 22;
            this.label1.Text = "조회 결과";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.txtSuga);
            this.panTitle.Controls.Add(this.groupBox1);
            this.panTitle.Controls.Add(this.btnView);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.label2);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(864, 40);
            this.panTitle.TabIndex = 139;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(784, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(74, 30);
            this.btnExit.TabIndex = 123;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(8, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 21);
            this.label2.TabIndex = 81;
            this.label2.Text = "수가조회";
            // 
            // btnView
            // 
            this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnView.AutoSize = true;
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnView.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnView.Location = new System.Drawing.Point(708, 4);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(74, 30);
            this.btnView.TabIndex = 141;
            this.btnView.Text = "조회";
            this.btnView.UseVisualStyleBackColor = true;
            // 
            // ssList
            // 
            this.ssList.AccessibleDescription = "ssList, Sheet1, Row 0, Column 0, ";
            this.ssList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssList.Location = new System.Drawing.Point(0, 68);
            this.ssList.Name = "ssList";
            namedStyle1.Font = new System.Drawing.Font("맑은 고딕", 11F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType1.MaxLength = 32000;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("맑은 고딕", 11F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType2.Static = true;
            namedStyle3.CellType = textCellType2;
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.Renderer = textCellType2;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle4.Font = new System.Drawing.Font("맑은 고딕", 9F);
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType3.Static = true;
            namedStyle5.CellType = textCellType3;
            namedStyle5.Font = new System.Drawing.Font("맑은 고딕", 9F);
            namedStyle5.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle5.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle5.Renderer = textCellType3;
            namedStyle5.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            numberCellType1.DecimalPlaces = 0;
            numberCellType1.DecimalSeparator = ".";
            numberCellType1.MaximumValue = 99999999D;
            numberCellType1.MinimumValue = -99999999D;
            numberCellType1.Separator = ",";
            numberCellType1.ShowSeparator = true;
            namedStyle6.CellType = numberCellType1;
            namedStyle6.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            namedStyle6.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle6.Renderer = numberCellType1;
            namedStyle6.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4,
            namedStyle5,
            namedStyle6});
            this.ssList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList_Sheet1});
            this.ssList.Size = new System.Drawing.Size(864, 231);
            this.ssList.TabIndex = 141;
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
            this.ssList_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "수가코드";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "한글수가명칭";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "보험금액";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "자보산재금액";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "일반금액";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "분류";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "누적";
            this.ssList_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssList_Sheet1.ColumnHeader.Rows.Get(0).Height = 23F;
            this.ssList_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssList_Sheet1.Columns.Get(0).Label = "수가코드";
            this.ssList_Sheet1.Columns.Get(0).StyleName = "Static605636414533272515814";
            this.ssList_Sheet1.Columns.Get(0).Width = 113F;
            this.ssList_Sheet1.Columns.Get(1).Label = "한글수가명칭";
            this.ssList_Sheet1.Columns.Get(1).StyleName = "Static657636414533272515814";
            this.ssList_Sheet1.Columns.Get(1).Width = 333F;
            this.ssList_Sheet1.Columns.Get(2).Label = "보험금액";
            this.ssList_Sheet1.Columns.Get(2).StyleName = "Number729636414533272515814";
            this.ssList_Sheet1.Columns.Get(2).Width = 96F;
            this.ssList_Sheet1.Columns.Get(3).Label = "자보산재금액";
            this.ssList_Sheet1.Columns.Get(3).StyleName = "Number729636414533272515814";
            this.ssList_Sheet1.Columns.Get(3).Width = 96F;
            this.ssList_Sheet1.Columns.Get(4).Label = "일반금액";
            this.ssList_Sheet1.Columns.Get(4).StyleName = "Number729636414533272515814";
            this.ssList_Sheet1.Columns.Get(4).Width = 96F;
            this.ssList_Sheet1.Columns.Get(5).Label = "분류";
            this.ssList_Sheet1.Columns.Get(5).StyleName = "Static605636414533272515814";
            this.ssList_Sheet1.Columns.Get(5).Width = 36F;
            this.ssList_Sheet1.Columns.Get(6).Label = "누적";
            this.ssList_Sheet1.Columns.Get(6).StyleName = "Static605636414533272515814";
            this.ssList_Sheet1.Columns.Get(6).Width = 36F;
            this.ssList_Sheet1.DefaultStyleName = "Text501636414533272515814";
            this.ssList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.optGbn1);
            this.groupBox1.Controls.Add(this.optGbn0);
            this.groupBox1.Location = new System.Drawing.Point(462, -1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(119, 34);
            this.groupBox1.TabIndex = 142;
            this.groupBox1.TabStop = false;
            // 
            // optGbn0
            // 
            this.optGbn0.AutoSize = true;
            this.optGbn0.BackColor = System.Drawing.Color.White;
            this.optGbn0.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.optGbn0.ForeColor = System.Drawing.Color.Black;
            this.optGbn0.Location = new System.Drawing.Point(6, 10);
            this.optGbn0.Name = "optGbn0";
            this.optGbn0.Size = new System.Drawing.Size(52, 21);
            this.optGbn0.TabIndex = 0;
            this.optGbn0.TabStop = true;
            this.optGbn0.Text = "코드";
            this.optGbn0.UseVisualStyleBackColor = false;
            // 
            // optGbn1
            // 
            this.optGbn1.AutoSize = true;
            this.optGbn1.BackColor = System.Drawing.Color.White;
            this.optGbn1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.optGbn1.ForeColor = System.Drawing.Color.Black;
            this.optGbn1.Location = new System.Drawing.Point(64, 10);
            this.optGbn1.Name = "optGbn1";
            this.optGbn1.Size = new System.Drawing.Size(52, 21);
            this.optGbn1.TabIndex = 1;
            this.optGbn1.TabStop = true;
            this.optGbn1.Text = "명칭";
            this.optGbn1.UseVisualStyleBackColor = false;
            // 
            // txtSuga
            // 
            this.txtSuga.Location = new System.Drawing.Point(598, 9);
            this.txtSuga.Name = "txtSuga";
            this.txtSuga.Size = new System.Drawing.Size(100, 21);
            this.txtSuga.TabIndex = 143;
            // 
            // frmPmpaViewSearchSuga
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(864, 299);
            this.Controls.Add(this.ssList);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panTitle);
            this.Name = "frmPmpaViewSearchSuga";
            this.Text = "수가조회";
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panTitle;
        public System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnView;
        private FarPoint.Win.Spread.FpSpread ssList;
        private FarPoint.Win.Spread.SheetView ssList_Sheet1;
        private System.Windows.Forms.TextBox txtSuga;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton optGbn1;
        private System.Windows.Forms.RadioButton optGbn0;
    }
}