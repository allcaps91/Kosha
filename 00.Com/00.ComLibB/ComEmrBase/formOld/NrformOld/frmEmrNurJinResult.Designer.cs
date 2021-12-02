namespace ComEmrBase
{
    partial class frmEmrNurJinResult
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
            FarPoint.Win.Spread.NamedStyle namedStyle16 = new FarPoint.Win.Spread.NamedStyle("Color374637107176713349399", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle17 = new FarPoint.Win.Spread.NamedStyle("Text464637107176713399275", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle18 = new FarPoint.Win.Spread.NamedStyle("CheckBox637637107176713429185");
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType4 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle19 = new FarPoint.Win.Spread.NamedStyle("Static714637107176713449140");
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle20 = new FarPoint.Win.Spread.NamedStyle("Static761637107176713469087");
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance4 = new FarPoint.Win.Spread.TipAppearance();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Label6 = new System.Windows.Forms.Label();
            this.SS6 = new FarPoint.Win.Spread.FpSpread();
            this.SS6_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS6_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(334, 34);
            this.panel1.TabIndex = 16;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(261, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 0;
            this.btnExit.Text = "닫  기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(189, 2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(72, 30);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "적  용";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(117, 2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "조  회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.Label6);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 34);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(334, 34);
            this.panel2.TabIndex = 17;
            // 
            // Label6
            // 
            this.Label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Label6.Font = new System.Drawing.Font("나눔고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Label6.Location = new System.Drawing.Point(0, 0);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(334, 34);
            this.Label6.TabIndex = 0;
            this.Label6.Text = "Result";
            this.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SS6
            // 
            this.SS6.AccessibleDescription = "";
            this.SS6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SS6.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.SS6.Location = new System.Drawing.Point(0, 68);
            this.SS6.Name = "SS6";
            namedStyle16.Font = new System.Drawing.Font("나눔고딕", 9F);
            namedStyle16.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle16.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle16.Parent = "DataAreaDefault";
            namedStyle16.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType10.MaxLength = 32000;
            namedStyle17.CellType = textCellType10;
            namedStyle17.Font = new System.Drawing.Font("나눔고딕", 9F);
            namedStyle17.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle17.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle17.Parent = "DataAreaDefault";
            namedStyle17.Renderer = textCellType10;
            namedStyle17.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle18.CellType = checkBoxCellType4;
            namedStyle18.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle18.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle18.Renderer = checkBoxCellType4;
            namedStyle18.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType11.Static = true;
            namedStyle19.CellType = textCellType11;
            namedStyle19.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle19.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle19.Renderer = textCellType11;
            namedStyle19.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType12.Static = true;
            textCellType12.WordWrap = true;
            namedStyle20.CellType = textCellType12;
            namedStyle20.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle20.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle20.Renderer = textCellType12;
            namedStyle20.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS6.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle16,
            namedStyle17,
            namedStyle18,
            namedStyle19,
            namedStyle20});
            this.SS6.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS6_Sheet1});
            this.SS6.Size = new System.Drawing.Size(334, 382);
            this.SS6.TabIndex = 18;
            this.SS6.TabStripRatio = 0.6D;
            tipAppearance4.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance4.ForeColor = System.Drawing.SystemColors.InfoText;
            this.SS6.TextTipAppearance = tipAppearance4;
            this.SS6.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SS6.VerticalScrollBarWidth = 11;
            // 
            // SS6_Sheet1
            // 
            this.SS6_Sheet1.Reset();
            this.SS6_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS6_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS6_Sheet1.ColumnCount = 3;
            this.SS6_Sheet1.RowCount = 1;
            this.SS6_Sheet1.ColumnFooter.Columns.Default.Resizable = false;
            this.SS6_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SS6_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = " ";
            this.SS6_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "코드";
            this.SS6_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "내용";
            this.SS6_Sheet1.ColumnHeader.Columns.Default.Resizable = false;
            this.SS6_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SS6_Sheet1.Columns.Default.Resizable = false;
            this.SS6_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SS6_Sheet1.Columns.Get(0).Label = " ";
            this.SS6_Sheet1.Columns.Get(0).StyleName = "CheckBox637637107176713429185";
            this.SS6_Sheet1.Columns.Get(0).Width = 28F;
            this.SS6_Sheet1.Columns.Get(1).Label = "코드";
            this.SS6_Sheet1.Columns.Get(1).StyleName = "Static714637107176713449140";
            this.SS6_Sheet1.Columns.Get(1).Visible = false;
            this.SS6_Sheet1.Columns.Get(1).Width = 75F;
            this.SS6_Sheet1.Columns.Get(2).Label = "내용";
            this.SS6_Sheet1.Columns.Get(2).StyleName = "Static761637107176713469087";
            this.SS6_Sheet1.Columns.Get(2).Width = 283F;
            this.SS6_Sheet1.DefaultStyleName = "Text464637107176713399275";
            this.SS6_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS6_Sheet1.RowHeader.Rows.Default.Resizable = false;
            this.SS6_Sheet1.RowHeader.Visible = false;
            this.SS6_Sheet1.Rows.Default.Resizable = false;
            this.SS6_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmEmrNurJinResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 450);
            this.Controls.Add(this.SS6);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "frmEmrNurJinResult";
            this.Text = "간호진단 코드 - Result";
            this.Load += new System.EventHandler(this.frmEmrNurJinResult_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SS6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS6_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label Label6;
        private FarPoint.Win.Spread.FpSpread SS6;
        private FarPoint.Win.Spread.SheetView SS6_Sheet1;
    }
}