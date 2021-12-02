namespace ComLibB
{
    partial class frmRadiationSlipView
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
            FarPoint.Win.Spread.NamedStyle namedStyle29 = new FarPoint.Win.Spread.NamedStyle("Color383636312208186253940", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle30 = new FarPoint.Win.Spread.NamedStyle("Text447636312208186263983", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType22 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle31 = new FarPoint.Win.Spread.NamedStyle("Static551636312208186284041");
            FarPoint.Win.Spread.CellType.TextCellType textCellType23 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle32 = new FarPoint.Win.Spread.NamedStyle("Static695636312208186314126");
            FarPoint.Win.Spread.CellType.TextCellType textCellType24 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance8 = new FarPoint.Win.Spread.TipAppearance();
            this.panTitle0 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnView = new System.Windows.Forms.Button();
            this.grbDate = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpTDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFDate = new System.Windows.Forms.DateTimePicker();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnPrint = new System.Windows.Forms.Button();
            this.txtPano = new System.Windows.Forms.TextBox();
            this.lblSName = new System.Windows.Forms.Label();
            this.ss1 = new FarPoint.Win.Spread.FpSpread();
            this.ss1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle0.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grbDate.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle0
            // 
            this.panTitle0.BackColor = System.Drawing.Color.White;
            this.panTitle0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle0.Controls.Add(this.btnExit);
            this.panTitle0.Controls.Add(this.lblTitle);
            this.panTitle0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle0.ForeColor = System.Drawing.Color.White;
            this.panTitle0.Location = new System.Drawing.Point(0, 0);
            this.panTitle0.Name = "panTitle0";
            this.panTitle0.Size = new System.Drawing.Size(801, 38);
            this.panTitle0.TabIndex = 77;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(715, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(79, 29);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "닫  기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(5, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(220, 21);
            this.lblTitle.TabIndex = 14;
            this.lblTitle.Text = "환자별 방사선 수납내역 조회";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panTitleSub0.Controls.Add(this.btnPrint);
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Controls.Add(this.btnView);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 38);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(801, 27);
            this.panTitleSub0.TabIndex = 89;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(12, 3);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(65, 17);
            this.lblTitleSub0.TabIndex = 21;
            this.lblTitleSub0.Text = "조회 조건";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grbDate);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 65);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(801, 52);
            this.panel1.TabIndex = 90;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblSName);
            this.groupBox1.Controls.Add(this.txtPano);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(227, 52);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "등록번호";
            // 
            // btnView
            // 
            this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnView.BackColor = System.Drawing.Color.Transparent;
            this.btnView.ForeColor = System.Drawing.Color.Black;
            this.btnView.Location = new System.Drawing.Point(655, 0);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(72, 27);
            this.btnView.TabIndex = 25;
            this.btnView.Text = "조  회";
            this.btnView.UseVisualStyleBackColor = false;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // grbDate
            // 
            this.grbDate.Controls.Add(this.label1);
            this.grbDate.Controls.Add(this.dtpTDate);
            this.grbDate.Controls.Add(this.dtpFDate);
            this.grbDate.Dock = System.Windows.Forms.DockStyle.Left;
            this.grbDate.Location = new System.Drawing.Point(227, 0);
            this.grbDate.Name = "grbDate";
            this.grbDate.Size = new System.Drawing.Size(289, 52);
            this.grbDate.TabIndex = 0;
            this.grbDate.TabStop = false;
            this.grbDate.Text = "검색기간";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 12F);
            this.label1.Location = new System.Drawing.Point(132, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "~";
            // 
            // dtpTDate
            // 
            this.dtpTDate.CalendarFont = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtpTDate.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTDate.Location = new System.Drawing.Point(152, 20);
            this.dtpTDate.Name = "dtpTDate";
            this.dtpTDate.Size = new System.Drawing.Size(125, 25);
            this.dtpTDate.TabIndex = 1;
            this.dtpTDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dtpTDate_KeyPress);
            // 
            // dtpFDate
            // 
            this.dtpFDate.CalendarFont = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtpFDate.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFDate.Location = new System.Drawing.Point(7, 20);
            this.dtpFDate.Name = "dtpFDate";
            this.dtpFDate.Size = new System.Drawing.Size(125, 25);
            this.dtpFDate.TabIndex = 0;
            this.dtpFDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dtpFDate_KeyPress);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 117);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(801, 27);
            this.panel3.TabIndex = 94;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(12, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 17);
            this.label3.TabIndex = 21;
            this.label3.Text = "조회 화면";
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.ForeColor = System.Drawing.Color.Black;
            this.btnPrint.Location = new System.Drawing.Point(728, 0);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 27);
            this.btnPrint.TabIndex = 27;
            this.btnPrint.Text = "인  쇄";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // txtPano
            // 
            this.txtPano.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtPano.Location = new System.Drawing.Point(6, 19);
            this.txtPano.Name = "txtPano";
            this.txtPano.Size = new System.Drawing.Size(71, 21);
            this.txtPano.TabIndex = 0;
            this.txtPano.Text = "12345678";
            this.txtPano.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPano_KeyPress);
            this.txtPano.Leave += new System.EventHandler(this.txtPano_Leave);
            // 
            // lblSName
            // 
            this.lblSName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblSName.Location = new System.Drawing.Point(95, 20);
            this.lblSName.Name = "lblSName";
            this.lblSName.Size = new System.Drawing.Size(110, 23);
            this.lblSName.TabIndex = 1;
            this.lblSName.Text = "홍길동애기";
            this.lblSName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ss1
            // 
            this.ss1.AccessibleDescription = "";
            this.ss1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ss1.Location = new System.Drawing.Point(0, 144);
            this.ss1.Name = "ss1";
            namedStyle29.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle29.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle29.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle29.Parent = "DataAreaDefault";
            namedStyle29.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType22.MaxLength = 60;
            namedStyle30.CellType = textCellType22;
            namedStyle30.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle30.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle30.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle30.Parent = "DataAreaDefault";
            namedStyle30.Renderer = textCellType22;
            namedStyle30.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType23.Static = true;
            namedStyle31.CellType = textCellType23;
            namedStyle31.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle31.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle31.Renderer = textCellType23;
            namedStyle31.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType24.Static = true;
            namedStyle32.CellType = textCellType24;
            namedStyle32.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle32.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle32.Renderer = textCellType24;
            namedStyle32.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle29,
            namedStyle30,
            namedStyle31,
            namedStyle32});
            this.ss1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ss1_Sheet1});
            this.ss1.Size = new System.Drawing.Size(801, 407);
            this.ss1.TabIndex = 95;
            this.ss1.TabStripRatio = 0.6D;
            tipAppearance8.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance8.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ss1.TextTipAppearance = tipAppearance8;
            // 
            // ss1_Sheet1
            // 
            this.ss1_Sheet1.Reset();
            this.ss1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ss1_Sheet1.ColumnCount = 9;
            this.ss1_Sheet1.RowCount = 1;
            this.ss1_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "처방일자";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "구분";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "과";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "의사";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "수가코드";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "수량";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "오 더 명 칭";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "전송";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "호실";
            this.ss1_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ss1_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.Columns.Get(0).Label = "처방일자";
            this.ss1_Sheet1.Columns.Get(0).StyleName = "Static551636312208186284041";
            this.ss1_Sheet1.Columns.Get(0).Width = 92F;
            this.ss1_Sheet1.Columns.Get(1).Label = "구분";
            this.ss1_Sheet1.Columns.Get(1).StyleName = "Static551636312208186284041";
            this.ss1_Sheet1.Columns.Get(1).Width = 42F;
            this.ss1_Sheet1.Columns.Get(2).Label = "과";
            this.ss1_Sheet1.Columns.Get(2).StyleName = "Static551636312208186284041";
            this.ss1_Sheet1.Columns.Get(2).Width = 36F;
            this.ss1_Sheet1.Columns.Get(3).Label = "의사";
            this.ss1_Sheet1.Columns.Get(3).StyleName = "Static551636312208186284041";
            this.ss1_Sheet1.Columns.Get(3).Width = 63F;
            this.ss1_Sheet1.Columns.Get(4).Label = "수가코드";
            this.ss1_Sheet1.Columns.Get(4).StyleName = "Static695636312208186314126";
            this.ss1_Sheet1.Columns.Get(4).Width = 70F;
            this.ss1_Sheet1.Columns.Get(5).Label = "수량";
            this.ss1_Sheet1.Columns.Get(5).StyleName = "Static551636312208186284041";
            this.ss1_Sheet1.Columns.Get(5).Width = 45F;
            this.ss1_Sheet1.Columns.Get(6).Label = "오 더 명 칭";
            this.ss1_Sheet1.Columns.Get(6).StyleName = "Static695636312208186314126";
            this.ss1_Sheet1.Columns.Get(6).Width = 309F;
            this.ss1_Sheet1.Columns.Get(7).Label = "전송";
            this.ss1_Sheet1.Columns.Get(7).StyleName = "Static551636312208186284041";
            this.ss1_Sheet1.Columns.Get(7).Width = 43F;
            this.ss1_Sheet1.Columns.Get(8).Label = "호실";
            this.ss1_Sheet1.Columns.Get(8).StyleName = "Static551636312208186284041";
            this.ss1_Sheet1.Columns.Get(8).Width = 44F;
            this.ss1_Sheet1.DefaultStyleName = "Text447636312208186263983";
            this.ss1_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ss1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmRadiationSlipView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(801, 551);
            this.Controls.Add(this.ss1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle0);
            this.Name = "frmRadiationSlipView";
            this.Text = "환자별 방사선 수납내역 조회";
            this.Load += new System.EventHandler(this.frmRadiationSlipView_Load);
            this.panTitle0.ResumeLayout(false);
            this.panTitle0.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grbDate.ResumeLayout(false);
            this.grbDate.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle0;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.GroupBox grbDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpTDate;
        private System.Windows.Forms.DateTimePicker dtpFDate;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label lblSName;
        private System.Windows.Forms.TextBox txtPano;
        private FarPoint.Win.Spread.FpSpread ss1;
        private FarPoint.Win.Spread.SheetView ss1_Sheet1;
    }
}