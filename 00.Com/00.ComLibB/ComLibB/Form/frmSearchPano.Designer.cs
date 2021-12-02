namespace ComLibB
{
    partial class frmSearchPano
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color367636314183485601124", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text461636314183485601124", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static565636314183485601124");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtFind = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.optView4 = new System.Windows.Forms.RadioButton();
            this.optView2 = new System.Windows.Forms.RadioButton();
            this.optView3 = new System.Windows.Forms.RadioButton();
            this.optView0 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.optIO5 = new System.Windows.Forms.RadioButton();
            this.optIO3 = new System.Windows.Forms.RadioButton();
            this.optIO2 = new System.Windows.Forms.RadioButton();
            this.optIO4 = new System.Windows.Forms.RadioButton();
            this.optIO1 = new System.Windows.Forms.RadioButton();
            this.optIO0 = new System.Windows.Forms.RadioButton();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(801, 34);
            this.panTitle.TabIndex = 86;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(724, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(9, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(156, 16);
            this.lblTitle.TabIndex = 15;
            this.lblTitle.Text = "환자 등록번호 찾기";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.Location = new System.Drawing.Point(639, 23);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 25);
            this.btnSearch.TabIndex = 16;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.txtFind);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(4);
            this.panel1.Size = new System.Drawing.Size(801, 74);
            this.panel1.TabIndex = 87;
            // 
            // txtFind
            // 
            this.txtFind.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtFind.Location = new System.Drawing.Point(533, 23);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new System.Drawing.Size(100, 25);
            this.txtFind.TabIndex = 3;
            this.txtFind.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFind_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(470, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "찾을 자료";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.optView4);
            this.groupBox2.Controls.Add(this.optView2);
            this.groupBox2.Controls.Add(this.optView3);
            this.groupBox2.Controls.Add(this.optView0);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.Location = new System.Drawing.Point(284, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(180, 62);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "찾기방법";
            // 
            // optView4
            // 
            this.optView4.AutoSize = true;
            this.optView4.Location = new System.Drawing.Point(93, 43);
            this.optView4.Name = "optView4";
            this.optView4.Size = new System.Drawing.Size(71, 16);
            this.optView4.TabIndex = 5;
            this.optView4.Text = "등록번호";
            this.optView4.UseVisualStyleBackColor = true;
            this.optView4.CheckedChanged += new System.EventHandler(this.optView_CheckedChanged);
            // 
            // optView2
            // 
            this.optView2.AutoSize = true;
            this.optView2.Location = new System.Drawing.Point(6, 43);
            this.optView2.Name = "optView2";
            this.optView2.Size = new System.Drawing.Size(71, 16);
            this.optView2.TabIndex = 4;
            this.optView2.Text = "병동코드";
            this.optView2.UseVisualStyleBackColor = true;
            this.optView2.CheckedChanged += new System.EventHandler(this.optView_CheckedChanged);
            // 
            // optView3
            // 
            this.optView3.AutoSize = true;
            this.optView3.Location = new System.Drawing.Point(93, 21);
            this.optView3.Name = "optView3";
            this.optView3.Size = new System.Drawing.Size(71, 16);
            this.optView3.TabIndex = 3;
            this.optView3.Text = "접수일자";
            this.optView3.UseVisualStyleBackColor = true;
            this.optView3.CheckedChanged += new System.EventHandler(this.optView_CheckedChanged);
            // 
            // optView0
            // 
            this.optView0.AutoSize = true;
            this.optView0.Checked = true;
            this.optView0.Location = new System.Drawing.Point(6, 21);
            this.optView0.Name = "optView0";
            this.optView0.Size = new System.Drawing.Size(71, 16);
            this.optView0.TabIndex = 2;
            this.optView0.TabStop = true;
            this.optView0.Text = "환자성명";
            this.optView0.UseVisualStyleBackColor = true;
            this.optView0.CheckedChanged += new System.EventHandler(this.optView_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.optIO5);
            this.groupBox1.Controls.Add(this.optIO3);
            this.groupBox1.Controls.Add(this.optIO2);
            this.groupBox1.Controls.Add(this.optIO4);
            this.groupBox1.Controls.Add(this.optIO1);
            this.groupBox1.Controls.Add(this.optIO0);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 62);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "외래 / 입원";
            // 
            // optIO5
            // 
            this.optIO5.AutoSize = true;
            this.optIO5.Location = new System.Drawing.Point(103, 43);
            this.optIO5.Name = "optIO5";
            this.optIO5.Size = new System.Drawing.Size(71, 16);
            this.optIO5.TabIndex = 4;
            this.optIO5.Text = "일반건진";
            this.optIO5.UseVisualStyleBackColor = true;
            this.optIO5.CheckedChanged += new System.EventHandler(this.optIO_CheckedChanged);
            // 
            // optIO3
            // 
            this.optIO3.AutoSize = true;
            this.optIO3.Location = new System.Drawing.Point(16, 43);
            this.optIO3.Name = "optIO3";
            this.optIO3.Size = new System.Drawing.Size(71, 16);
            this.optIO3.TabIndex = 3;
            this.optIO3.Text = "종합건진";
            this.optIO3.UseVisualStyleBackColor = true;
            this.optIO3.CheckedChanged += new System.EventHandler(this.optIO_CheckedChanged);
            // 
            // optIO2
            // 
            this.optIO2.AutoSize = true;
            this.optIO2.Checked = true;
            this.optIO2.Location = new System.Drawing.Point(190, 21);
            this.optIO2.Name = "optIO2";
            this.optIO2.Size = new System.Drawing.Size(83, 16);
            this.optIO2.TabIndex = 2;
            this.optIO2.TabStop = true;
            this.optIO2.Text = "환자마스타";
            this.optIO2.UseVisualStyleBackColor = true;
            this.optIO2.CheckedChanged += new System.EventHandler(this.optIO_CheckedChanged);
            // 
            // optIO4
            // 
            this.optIO4.AutoSize = true;
            this.optIO4.Location = new System.Drawing.Point(190, 43);
            this.optIO4.Name = "optIO4";
            this.optIO4.Size = new System.Drawing.Size(47, 16);
            this.optIO4.TabIndex = 1;
            this.optIO4.Text = "수탁";
            this.optIO4.UseVisualStyleBackColor = true;
            this.optIO4.CheckedChanged += new System.EventHandler(this.optIO_CheckedChanged);
            // 
            // optIO1
            // 
            this.optIO1.AutoSize = true;
            this.optIO1.Location = new System.Drawing.Point(103, 21);
            this.optIO1.Name = "optIO1";
            this.optIO1.Size = new System.Drawing.Size(47, 16);
            this.optIO1.TabIndex = 1;
            this.optIO1.Text = "외래";
            this.optIO1.UseVisualStyleBackColor = true;
            this.optIO1.CheckedChanged += new System.EventHandler(this.optIO_CheckedChanged);
            // 
            // optIO0
            // 
            this.optIO0.AutoSize = true;
            this.optIO0.Location = new System.Drawing.Point(16, 21);
            this.optIO0.Name = "optIO0";
            this.optIO0.Size = new System.Drawing.Size(59, 16);
            this.optIO0.TabIndex = 0;
            this.optIO0.Text = "재원자";
            this.optIO0.UseVisualStyleBackColor = true;
            this.optIO0.CheckedChanged += new System.EventHandler(this.optIO_CheckedChanged);
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "ssView, Sheet1, Row 0, Column 0, ";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssView.Location = new System.Drawing.Point(0, 108);
            this.ssView.Name = "ssView";
            namedStyle1.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType1.MaxLength = 60;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("굴림", 9F);
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
            this.ssView.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3});
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(801, 450);
            this.ssView.TabIndex = 88;
            this.ssView.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssView.TextTipAppearance = tipAppearance1;
            this.ssView.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssView_CellClick);
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 12;
            this.ssView_Sheet1.RowCount = 50;
            this.ssView_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnFooter.Rows.Get(0).Height = 26F;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "등록번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "성 명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "병동";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "호실";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "주민번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "나이";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "성별";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "종류";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "과";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "의사";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "의사명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "수납일";
            this.ssView_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.Columns.Get(0).Label = "등록번호";
            this.ssView_Sheet1.Columns.Get(0).StyleName = "Static565636314183485601124";
            this.ssView_Sheet1.Columns.Get(0).Width = 89F;
            this.ssView_Sheet1.Columns.Get(1).Label = "성 명";
            this.ssView_Sheet1.Columns.Get(1).StyleName = "Static565636314183485601124";
            this.ssView_Sheet1.Columns.Get(1).Width = 77F;
            this.ssView_Sheet1.Columns.Get(2).Label = "병동";
            this.ssView_Sheet1.Columns.Get(2).StyleName = "Static565636314183485601124";
            this.ssView_Sheet1.Columns.Get(2).Width = 39F;
            this.ssView_Sheet1.Columns.Get(3).Label = "호실";
            this.ssView_Sheet1.Columns.Get(3).StyleName = "Static565636314183485601124";
            this.ssView_Sheet1.Columns.Get(3).Width = 38F;
            this.ssView_Sheet1.Columns.Get(4).Label = "주민번호";
            this.ssView_Sheet1.Columns.Get(4).StyleName = "Static565636314183485601124";
            this.ssView_Sheet1.Columns.Get(4).Width = 100F;
            this.ssView_Sheet1.Columns.Get(5).Label = "나이";
            this.ssView_Sheet1.Columns.Get(5).StyleName = "Static565636314183485601124";
            this.ssView_Sheet1.Columns.Get(5).Width = 43F;
            this.ssView_Sheet1.Columns.Get(6).Label = "성별";
            this.ssView_Sheet1.Columns.Get(6).StyleName = "Static565636314183485601124";
            this.ssView_Sheet1.Columns.Get(6).Width = 41F;
            this.ssView_Sheet1.Columns.Get(7).Label = "종류";
            this.ssView_Sheet1.Columns.Get(7).StyleName = "Static565636314183485601124";
            this.ssView_Sheet1.Columns.Get(7).Width = 39F;
            this.ssView_Sheet1.Columns.Get(8).Label = "과";
            this.ssView_Sheet1.Columns.Get(8).StyleName = "Static565636314183485601124";
            this.ssView_Sheet1.Columns.Get(8).Width = 31F;
            this.ssView_Sheet1.Columns.Get(9).Label = "의사";
            this.ssView_Sheet1.Columns.Get(9).StyleName = "Static565636314183485601124";
            this.ssView_Sheet1.Columns.Get(9).Width = 57F;
            this.ssView_Sheet1.Columns.Get(10).Label = "의사명";
            this.ssView_Sheet1.Columns.Get(10).StyleName = "Static565636314183485601124";
            this.ssView_Sheet1.Columns.Get(10).Width = 73F;
            this.ssView_Sheet1.Columns.Get(11).Label = "수납일";
            this.ssView_Sheet1.Columns.Get(11).Width = 111F;
            this.ssView_Sheet1.DefaultStyleName = "Text461636314183485601124";
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.Rows.Default.Height = 18F;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmSearchPano
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(801, 558);
            this.ControlBox = false;
            this.Controls.Add(this.ssView);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Name = "frmSearchPano";
            this.Text = "환자 등록번호 찾기";
            this.Load += new System.EventHandler(this.frmPanoSearch_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtFind;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton optView4;
        private System.Windows.Forms.RadioButton optView2;
        private System.Windows.Forms.RadioButton optView3;
        private System.Windows.Forms.RadioButton optView0;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton optIO5;
        private System.Windows.Forms.RadioButton optIO3;
        private System.Windows.Forms.RadioButton optIO2;
        private System.Windows.Forms.RadioButton optIO4;
        private System.Windows.Forms.RadioButton optIO1;
        private System.Windows.Forms.RadioButton optIO0;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
    }
}