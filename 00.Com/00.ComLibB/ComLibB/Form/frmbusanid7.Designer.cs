namespace ComLibB
{
    partial class frmbusanid7
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose ();
            }
            base.Dispose (disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color376636341972529019335", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text454636341972529329352", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static558636341972529359366");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Static630636341972529450982");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("Font682636341972529460980");
            FarPoint.Win.Spread.NamedStyle namedStyle6 = new FarPoint.Win.Spread.NamedStyle("Static718636341972529480992");
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle7 = new FarPoint.Win.Spread.NamedStyle("Static898636341972529541008");
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.pan0 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblBottom = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnDePastPrint = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnPrintJob = new System.Windows.Forms.Button();
            this.btnDOSPrint = new System.Windows.Forms.Button();
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pan0.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.panel3.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // pan0
            // 
            this.pan0.Controls.Add(this.panel2);
            this.pan0.Controls.Add(this.panTitle);
            this.pan0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pan0.Location = new System.Drawing.Point(0, 0);
            this.pan0.Name = "pan0";
            this.pan0.Size = new System.Drawing.Size(1063, 657);
            this.pan0.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 34);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1063, 623);
            this.panel2.TabIndex = 13;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.Controls.Add(this.lblBottom);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 594);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1059, 25);
            this.panel4.TabIndex = 19;
            // 
            // lblBottom
            // 
            this.lblBottom.AutoSize = true;
            this.lblBottom.Location = new System.Drawing.Point(140, 7);
            this.lblBottom.Name = "lblBottom";
            this.lblBottom.Size = new System.Drawing.Size(597, 12);
            this.lblBottom.TabIndex = 0;
            this.lblBottom.Text = "◈ 더블클릭시  ①수가코드:수가등록,  ②품명코드:표준코드조회 ③분류:구입신고 ④명칭:의약품실구입신고 ◈";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.ssView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1059, 585);
            this.panel1.TabIndex = 18;
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "ssView, Sheet1, Row 0, Column 0, ";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssView.Location = new System.Drawing.Point(0, 0);
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
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.Renderer = textCellType2;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType3.Static = true;
            namedStyle4.CellType = textCellType3;
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.Renderer = textCellType3;
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle5.Font = new System.Drawing.Font("굴림체", 9F);
            namedStyle5.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle5.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle5.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType4.Static = true;
            namedStyle6.CellType = textCellType4;
            namedStyle6.Font = new System.Drawing.Font("굴림체", 9F);
            namedStyle6.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle6.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle6.Renderer = textCellType4;
            namedStyle6.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType5.Static = true;
            namedStyle7.CellType = textCellType5;
            namedStyle7.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            namedStyle7.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle7.Renderer = textCellType5;
            namedStyle7.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4,
            namedStyle5,
            namedStyle6,
            namedStyle7});
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(1059, 585);
            this.ssView.TabIndex = 46;
            this.ssView.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssView.TextTipAppearance = tipAppearance1;
            this.ssView.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssView.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssView_CellDoubleClick);
            this.ssView.SetViewportLeftColumn(0, 0, 2);
            this.ssView.SetActiveViewport(0, 0, -1);
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 19;
            this.ssView_Sheet1.RowCount = 1;
            this.ssView_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "수가코드";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "품명코드";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "T";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "분류";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "ABCDEFGHIJKLMNOPQRSTUV";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "SS";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "BI";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "수량";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "수 가 명 칭";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "보험수가";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "자보수가";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "일반수가";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "변경일자";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "종전보험";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "종전자보";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 15).Value = "종전일반";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 16).Value = "단위";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 17).Value = "한글코드";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 18).Value = "보험코드";
            this.ssView_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssView_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.Columns.Get(0).Label = "수가코드";
            this.ssView_Sheet1.Columns.Get(0).StyleName = "Static558636341972529359366";
            this.ssView_Sheet1.Columns.Get(0).Width = 62F;
            this.ssView_Sheet1.Columns.Get(1).Label = "품명코드";
            this.ssView_Sheet1.Columns.Get(1).StyleName = "Static558636341972529359366";
            this.ssView_Sheet1.Columns.Get(1).Width = 62F;
            this.ssView_Sheet1.Columns.Get(2).Label = "T";
            this.ssView_Sheet1.Columns.Get(2).StyleName = "Static630636341972529450982";
            this.ssView_Sheet1.Columns.Get(2).Width = 18F;
            this.ssView_Sheet1.Columns.Get(3).Label = "분류";
            this.ssView_Sheet1.Columns.Get(3).StyleName = "Static630636341972529450982";
            this.ssView_Sheet1.Columns.Get(3).Width = 42F;
            this.ssView_Sheet1.Columns.Get(4).Label = "ABCDEFGHIJKLMNOPQRSTUV";
            this.ssView_Sheet1.Columns.Get(4).StyleName = "Static718636341972529480992";
            this.ssView_Sheet1.Columns.Get(4).Width = 221F;
            this.ssView_Sheet1.Columns.Get(5).Label = "SS";
            this.ssView_Sheet1.Columns.Get(5).StyleName = "Static630636341972529450982";
            this.ssView_Sheet1.Columns.Get(5).Width = 24F;
            this.ssView_Sheet1.Columns.Get(6).Label = "BI";
            this.ssView_Sheet1.Columns.Get(6).StyleName = "Static630636341972529450982";
            this.ssView_Sheet1.Columns.Get(6).Width = 20F;
            this.ssView_Sheet1.Columns.Get(7).Label = "수량";
            this.ssView_Sheet1.Columns.Get(7).StyleName = "Static630636341972529450982";
            this.ssView_Sheet1.Columns.Get(7).Width = 22F;
            this.ssView_Sheet1.Columns.Get(8).Label = "수 가 명 칭";
            this.ssView_Sheet1.Columns.Get(8).StyleName = "Static558636341972529359366";
            this.ssView_Sheet1.Columns.Get(8).Width = 280F;
            this.ssView_Sheet1.Columns.Get(9).Label = "보험수가";
            this.ssView_Sheet1.Columns.Get(9).StyleName = "Static898636341972529541008";
            this.ssView_Sheet1.Columns.Get(9).Width = 61F;
            this.ssView_Sheet1.Columns.Get(10).Label = "자보수가";
            this.ssView_Sheet1.Columns.Get(10).StyleName = "Static898636341972529541008";
            this.ssView_Sheet1.Columns.Get(10).Width = 62F;
            this.ssView_Sheet1.Columns.Get(11).Label = "일반수가";
            this.ssView_Sheet1.Columns.Get(11).StyleName = "Static898636341972529541008";
            this.ssView_Sheet1.Columns.Get(11).Width = 61F;
            this.ssView_Sheet1.Columns.Get(12).Label = "변경일자";
            this.ssView_Sheet1.Columns.Get(12).StyleName = "Static630636341972529450982";
            this.ssView_Sheet1.Columns.Get(12).Width = 81F;
            this.ssView_Sheet1.Columns.Get(13).Label = "종전보험";
            this.ssView_Sheet1.Columns.Get(13).StyleName = "Static898636341972529541008";
            this.ssView_Sheet1.Columns.Get(13).Width = 61F;
            this.ssView_Sheet1.Columns.Get(14).Label = "종전자보";
            this.ssView_Sheet1.Columns.Get(14).StyleName = "Static898636341972529541008";
            this.ssView_Sheet1.Columns.Get(15).Label = "종전일반";
            this.ssView_Sheet1.Columns.Get(15).StyleName = "Static898636341972529541008";
            this.ssView_Sheet1.Columns.Get(15).Width = 61F;
            this.ssView_Sheet1.Columns.Get(16).Label = "단위";
            this.ssView_Sheet1.Columns.Get(16).StyleName = "Static558636341972529359366";
            this.ssView_Sheet1.Columns.Get(16).Width = 46F;
            this.ssView_Sheet1.Columns.Get(17).Label = "한글코드";
            this.ssView_Sheet1.Columns.Get(17).StyleName = "Static558636341972529359366";
            this.ssView_Sheet1.Columns.Get(17).Width = 62F;
            this.ssView_Sheet1.Columns.Get(18).Label = "보험코드";
            this.ssView_Sheet1.Columns.Get(18).StyleName = "Static558636341972529359366";
            this.ssView_Sheet1.Columns.Get(18).Width = 71F;
            this.ssView_Sheet1.DefaultStyleName = "Text454636341972529329352";
            this.ssView_Sheet1.FrozenColumnCount = 2;
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.btnDePastPrint);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Controls.Add(this.btnPrintJob);
            this.panel3.Controls.Add(this.btnDOSPrint);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1059, 34);
            this.panel3.TabIndex = 17;
            // 
            // btnDePastPrint
            // 
            this.btnDePastPrint.Location = new System.Drawing.Point(571, 2);
            this.btnDePastPrint.Name = "btnDePastPrint";
            this.btnDePastPrint.Size = new System.Drawing.Size(120, 30);
            this.btnDePastPrint.TabIndex = 29;
            this.btnDePastPrint.Text = "전산실 고속프린트";
            this.btnDePastPrint.UseVisualStyleBackColor = true;
            this.btnDePastPrint.Click += new System.EventHandler(this.btnDePastPrint_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(977, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Location = new System.Drawing.Point(2, 2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(177, 30);
            this.btnSearch.TabIndex = 27;
            this.btnSearch.Text = "검색조건 지정 및 자료조회(&Y)";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnPrintJob
            // 
            this.btnPrintJob.BackColor = System.Drawing.Color.Transparent;
            this.btnPrintJob.Location = new System.Drawing.Point(329, 2);
            this.btnPrintJob.Name = "btnPrintJob";
            this.btnPrintJob.Size = new System.Drawing.Size(119, 30);
            this.btnPrintJob.TabIndex = 28;
            this.btnPrintJob.Text = "인쇄작업(&P)";
            this.btnPrintJob.UseVisualStyleBackColor = false;
            this.btnPrintJob.Click += new System.EventHandler(this.btnPrintJob_Click);
            // 
            // btnDOSPrint
            // 
            this.btnDOSPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnDOSPrint.Location = new System.Drawing.Point(448, 2);
            this.btnDOSPrint.Name = "btnDOSPrint";
            this.btnDOSPrint.Size = new System.Drawing.Size(123, 30);
            this.btnDOSPrint.TabIndex = 21;
            this.btnDOSPrint.Text = "DOS모드로 인쇄(&D)";
            this.btnDOSPrint.UseVisualStyleBackColor = false;
            this.btnDOSPrint.Click += new System.EventHandler(this.btnDOSPrint_Click);
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1063, 34);
            this.panTitle.TabIndex = 11;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(236, 16);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "수가코드 조회 및 코드집 인쇄";
            // 
            // frmbusanid7
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1063, 657);
            this.Controls.Add(this.pan0);
            this.Name = "frmbusanid7";
            this.Text = "수가코드 조회 및 코드집 인쇄";
            this.Load += new System.EventHandler(this.frmbusanid7_Load);
            this.pan0.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pan0;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel1;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnPrintJob;
        private System.Windows.Forms.Button btnDOSPrint;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblBottom;
        private System.Windows.Forms.Button btnDePastPrint;
    }
}