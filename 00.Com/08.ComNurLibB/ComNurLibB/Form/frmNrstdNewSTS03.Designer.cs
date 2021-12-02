namespace ComNurLibB
{
    partial class frmNrstdNewSTS03
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color375636530107937439656", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Static457636530107937469675", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("BorderEx563636530107937509679");
            FarPoint.Win.ComplexBorder complexBorder1 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.ComplexBorder complexBorder2 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblSts = new System.Windows.Forms.Label();
            this.panYYMM = new System.Windows.Forms.Panel();
            this.ComboYYMM = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panYear = new System.Windows.Forms.Panel();
            this.ComboYear = new System.Windows.Forms.ComboBox();
            this.lblItem0 = new System.Windows.Forms.Label();
            this.OptTong1 = new System.Windows.Forms.RadioButton();
            this.OptTong0 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.ComboWard = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SS1 = new FarPoint.Win.Spread.FpSpread();
            this.SS1_Sheet0 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panYYMM.SuspendLayout();
            this.panYear.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet0)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1184, 34);
            this.panTitle.TabIndex = 14;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.btnExit.Location = new System.Drawing.Point(1108, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(172, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "월별 과별 중증도 통계";
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(1184, 28);
            this.panTitleSub0.TabIndex = 15;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 4);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(59, 15);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "조건 검색";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.lblSts);
            this.panel1.Controls.Add(this.panYYMM);
            this.panel1.Controls.Add(this.panYear);
            this.panel1.Controls.Add(this.OptTong1);
            this.panel1.Controls.Add(this.OptTong0);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.ComboWard);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1184, 32);
            this.panel1.TabIndex = 16;
            // 
            // lblSts
            // 
            this.lblSts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSts.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.lblSts.Location = new System.Drawing.Point(552, 5);
            this.lblSts.Name = "lblSts";
            this.lblSts.Size = new System.Drawing.Size(227, 23);
            this.lblSts.TabIndex = 39;
            this.lblSts.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panYYMM
            // 
            this.panYYMM.Controls.Add(this.ComboYYMM);
            this.panYYMM.Controls.Add(this.label3);
            this.panYYMM.Dock = System.Windows.Forms.DockStyle.Left;
            this.panYYMM.Location = new System.Drawing.Point(188, 0);
            this.panYYMM.Name = "panYYMM";
            this.panYYMM.Size = new System.Drawing.Size(163, 32);
            this.panYYMM.TabIndex = 38;
            // 
            // ComboYYMM
            // 
            this.ComboYYMM.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.ComboYYMM.FormattingEnabled = true;
            this.ComboYYMM.Location = new System.Drawing.Point(73, 4);
            this.ComboYYMM.Name = "ComboYYMM";
            this.ComboYYMM.Size = new System.Drawing.Size(87, 25);
            this.ComboYYMM.TabIndex = 28;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.label3.Location = new System.Drawing.Point(6, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 23);
            this.label3.TabIndex = 27;
            this.label3.Text = "작업월";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panYear
            // 
            this.panYear.Controls.Add(this.ComboYear);
            this.panYear.Controls.Add(this.lblItem0);
            this.panYear.Dock = System.Windows.Forms.DockStyle.Left;
            this.panYear.Location = new System.Drawing.Point(0, 0);
            this.panYear.Name = "panYear";
            this.panYear.Size = new System.Drawing.Size(188, 32);
            this.panYear.TabIndex = 37;
            // 
            // ComboYear
            // 
            this.ComboYear.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.ComboYear.FormattingEnabled = true;
            this.ComboYear.Location = new System.Drawing.Point(73, 4);
            this.ComboYear.Name = "ComboYear";
            this.ComboYear.Size = new System.Drawing.Size(111, 25);
            this.ComboYear.TabIndex = 28;
            // 
            // lblItem0
            // 
            this.lblItem0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblItem0.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.lblItem0.Location = new System.Drawing.Point(6, 5);
            this.lblItem0.Name = "lblItem0";
            this.lblItem0.Size = new System.Drawing.Size(66, 23);
            this.lblItem0.TabIndex = 27;
            this.lblItem0.Text = "조회년도";
            this.lblItem0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OptTong1
            // 
            this.OptTong1.AutoSize = true;
            this.OptTong1.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.OptTong1.Location = new System.Drawing.Point(481, 6);
            this.OptTong1.Name = "OptTong1";
            this.OptTong1.Size = new System.Drawing.Size(65, 21);
            this.OptTong1.TabIndex = 36;
            this.OptTong1.Text = "년통계";
            this.OptTong1.UseVisualStyleBackColor = true;
            this.OptTong1.CheckedChanged += new System.EventHandler(this.OptTong_CheckedChanged);
            // 
            // OptTong0
            // 
            this.OptTong0.AutoSize = true;
            this.OptTong0.Checked = true;
            this.OptTong0.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.OptTong0.Location = new System.Drawing.Point(416, 6);
            this.OptTong0.Name = "OptTong0";
            this.OptTong0.Size = new System.Drawing.Size(65, 21);
            this.OptTong0.TabIndex = 35;
            this.OptTong0.TabStop = true;
            this.OptTong0.Text = "월통계";
            this.OptTong0.UseVisualStyleBackColor = true;
            this.OptTong0.CheckedChanged += new System.EventHandler(this.OptTong_CheckedChanged);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.label2.Location = new System.Drawing.Point(346, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 23);
            this.label2.TabIndex = 34;
            this.label2.Text = "조회구분";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnSearch);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(1034, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(150, 32);
            this.panel4.TabIndex = 33;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.btnSearch.Location = new System.Drawing.Point(75, 1);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 32;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // ComboWard
            // 
            this.ComboWard.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.ComboWard.FormattingEnabled = true;
            this.ComboWard.Location = new System.Drawing.Point(217, 4);
            this.ComboWard.Name = "ComboWard";
            this.ComboWard.Size = new System.Drawing.Size(121, 25);
            this.ComboWard.TabIndex = 28;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.label1.Location = new System.Drawing.Point(168, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 23);
            this.label1.TabIndex = 27;
            this.label1.Text = "병동";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SS1
            // 
            this.SS1.AccessibleDescription = "SS1, 욕창통계, Row 0, Column 0, ";
            this.SS1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SS1.Location = new System.Drawing.Point(0, 94);
            this.SS1.Name = "SS1";
            namedStyle1.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType1.Static = true;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle3.Border = complexBorder1;
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3});
            this.SS1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS1_Sheet0});
            this.SS1.Size = new System.Drawing.Size(1184, 474);
            this.SS1.TabIndex = 17;
            this.SS1.TabStrip.DefaultSheetTab.Font = new System.Drawing.Font("굴림", 9F);
            this.SS1.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.SS1.TextTipAppearance = tipAppearance1;
            this.SS1.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SS1_CellDoubleClick);
            this.SS1.SetViewportLeftColumn(0, 0, 2);
            this.SS1.SetActiveViewport(0, 0, -1);
            // 
            // SS1_Sheet0
            // 
            this.SS1_Sheet0.Reset();
            this.SS1_Sheet0.SheetName = "욕창통계";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS1_Sheet0.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS1_Sheet0.ColumnCount = 3;
            this.SS1_Sheet0.RowCount = 1;
            this.SS1_Sheet0.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SS1_Sheet0.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet0.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet0.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SS1_Sheet0.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet0.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SS1_Sheet0.ColumnHeader.Rows.Get(0).Height = 28F;
            this.SS1_Sheet0.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SS1_Sheet0.Columns.Get(0).Border = complexBorder2;
            this.SS1_Sheet0.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet0.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet0.Columns.Get(0).Width = 63F;
            this.SS1_Sheet0.Columns.Get(1).Visible = false;
            this.SS1_Sheet0.DefaultStyleName = "Static457636530107937469675";
            this.SS1_Sheet0.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet0.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet0.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SS1_Sheet0.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet0.FrozenColumnCount = 2;
            this.SS1_Sheet0.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.None);
            this.SS1_Sheet0.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            this.SS1_Sheet0.RowHeader.Columns.Default.Resizable = false;
            this.SS1_Sheet0.Rows.Get(0).Height = 17F;
            this.SS1_Sheet0.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.None);
            this.SS1_Sheet0.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmNrstdNewSTS03
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 568);
            this.Controls.Add(this.SS1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Name = "frmNrstdNewSTS03";
            this.Text = "frmNrstdNewSTS03";
            this.Load += new System.EventHandler(this.frmNrstdNewSTS03_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panYYMM.ResumeLayout(false);
            this.panYear.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet0)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblSts;
        private System.Windows.Forms.Panel panYYMM;
        private System.Windows.Forms.ComboBox ComboYYMM;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panYear;
        private System.Windows.Forms.ComboBox ComboYear;
        private System.Windows.Forms.Label lblItem0;
        private System.Windows.Forms.RadioButton OptTong1;
        private System.Windows.Forms.RadioButton OptTong0;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox ComboWard;
        private System.Windows.Forms.Label label1;
        private FarPoint.Win.Spread.FpSpread SS1;
        private FarPoint.Win.Spread.SheetView SS1_Sheet0;
    }
}