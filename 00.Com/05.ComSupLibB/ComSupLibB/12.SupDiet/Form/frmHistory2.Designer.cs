namespace ComSupLibB
{
    partial class frmHistory2
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rdo0 = new System.Windows.Forms.RadioButton();
            this.rdo1 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtTDate = new System.Windows.Forms.DateTimePicker();
            this.TxtFDate = new System.Windows.Forms.DateTimePicker();
            this.txtPano = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.lblItem1 = new System.Windows.Forms.Label();
            this.lblItem0 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.ss1 = new FarPoint.Win.Spread.FpSpread();
            this.ss1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.ss2 = new FarPoint.Win.Spread.FpSpread();
            this.ss2_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel3.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss2_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.rdo0);
            this.panel3.Controls.Add(this.rdo1);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.TxtTDate);
            this.panel3.Controls.Add(this.TxtFDate);
            this.panel3.Controls.Add(this.txtPano);
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Controls.Add(this.btnPrint);
            this.panel3.Controls.Add(this.lblItem1);
            this.panel3.Controls.Add(this.lblItem0);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 62);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(830, 35);
            this.panel3.TabIndex = 20;
            // 
            // rdo0
            // 
            this.rdo0.AutoSize = true;
            this.rdo0.Location = new System.Drawing.Point(414, 9);
            this.rdo0.Name = "rdo0";
            this.rdo0.Size = new System.Drawing.Size(90, 16);
            this.rdo0.TabIndex = 36;
            this.rdo0.Text = "변경 History";
            this.rdo0.UseVisualStyleBackColor = true;
            this.rdo0.CheckedChanged += new System.EventHandler(this.rdo_CheckedChanged);
            // 
            // rdo1
            // 
            this.rdo1.AutoSize = true;
            this.rdo1.Location = new System.Drawing.Point(504, 9);
            this.rdo1.Name = "rdo1";
            this.rdo1.Size = new System.Drawing.Size(139, 16);
            this.rdo1.TabIndex = 35;
            this.rdo1.Text = "환자별 식이 내역조회";
            this.rdo1.UseVisualStyleBackColor = true;
            this.rdo1.CheckedChanged += new System.EventHandler(this.rdo_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(308, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 12);
            this.label1.TabIndex = 34;
            this.label1.Text = "-";
            // 
            // TxtTDate
            // 
            this.TxtTDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.TxtTDate.Location = new System.Drawing.Point(319, 7);
            this.TxtTDate.Name = "TxtTDate";
            this.TxtTDate.Size = new System.Drawing.Size(84, 21);
            this.TxtTDate.TabIndex = 33;
            // 
            // TxtFDate
            // 
            this.TxtFDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.TxtFDate.Location = new System.Drawing.Point(224, 7);
            this.TxtFDate.Name = "TxtFDate";
            this.TxtFDate.Size = new System.Drawing.Size(84, 21);
            this.TxtFDate.TabIndex = 32;
            // 
            // txtPano
            // 
            this.txtPano.Location = new System.Drawing.Point(58, 7);
            this.txtPano.Name = "txtPano";
            this.txtPano.Size = new System.Drawing.Size(100, 21);
            this.txtPano.TabIndex = 31;
            this.txtPano.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPano_KeyDown);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Location = new System.Drawing.Point(684, 2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 27;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.Location = new System.Drawing.Point(756, 2);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 30);
            this.btnPrint.TabIndex = 22;
            this.btnPrint.Text = "출력";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // lblItem1
            // 
            this.lblItem1.AutoSize = true;
            this.lblItem1.Location = new System.Drawing.Point(170, 11);
            this.lblItem1.Name = "lblItem1";
            this.lblItem1.Size = new System.Drawing.Size(53, 12);
            this.lblItem1.TabIndex = 25;
            this.lblItem1.Text = "식이기간";
            // 
            // lblItem0
            // 
            this.lblItem0.AutoSize = true;
            this.lblItem0.Location = new System.Drawing.Point(5, 11);
            this.lblItem0.Name = "lblItem0";
            this.lblItem0.Size = new System.Drawing.Size(53, 12);
            this.lblItem0.TabIndex = 24;
            this.lblItem0.Text = "등록번호";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(754, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(830, 28);
            this.panTitleSub0.TabIndex = 19;
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
            this.lblTitleSub0.Text = "검색 조건";
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(830, 34);
            this.panTitle.TabIndex = 18;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(102, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "변경 History";
            // 
            // ss1
            // 
            this.ss1.AccessibleDescription = "";
            this.ss1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ss1.Location = new System.Drawing.Point(40, 258);
            this.ss1.Name = "ss1";
            this.ss1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ss1_Sheet1});
            this.ss1.Size = new System.Drawing.Size(192, 133);
            this.ss1.TabIndex = 21;
            // 
            // ss1_Sheet1
            // 
            this.ss1_Sheet1.Reset();
            this.ss1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ss1_Sheet1.ColumnCount = 6;
            this.ss1_Sheet1.RowCount = 1;
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "오더일자";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "병동";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "호실";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "DAY";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "변경일자";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "식이형태";
            this.ss1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(0).Label = "오더일자";
            this.ss1_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(0).Width = 100F;
            this.ss1_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(1).Label = "병동";
            this.ss1_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(2).Label = "호실";
            this.ss1_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(3).Label = "DAY";
            this.ss1_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(4).Label = "변경일자";
            this.ss1_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(4).Width = 200F;
            this.ss1_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(5).Label = "식이형태";
            this.ss1_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(5).Width = 200F;
            this.ss1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // ss2
            // 
            this.ss2.AccessibleDescription = "";
            this.ss2.Location = new System.Drawing.Point(319, 258);
            this.ss2.Name = "ss2";
            this.ss2.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ss2_Sheet1});
            this.ss2.Size = new System.Drawing.Size(143, 100);
            this.ss2.TabIndex = 22;
            // 
            // ss2_Sheet1
            // 
            this.ss2_Sheet1.Reset();
            this.ss2_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ss2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ss2_Sheet1.ColumnCount = 6;
            this.ss2_Sheet1.RowCount = 1;
            this.ss2_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "일자";
            this.ss2_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "병동";
            this.ss2_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "호실";
            this.ss2_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "아침";
            this.ss2_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "점심";
            this.ss2_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "저녁";
            textCellType1.Multiline = true;
            textCellType1.WordWrap = true;
            this.ss2_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ss2_Sheet1.Columns.Get(0).Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ss2_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss2_Sheet1.Columns.Get(0).Label = "일자";
            this.ss2_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss2_Sheet1.Columns.Get(0).Width = 120F;
            textCellType2.Multiline = true;
            textCellType2.WordWrap = true;
            this.ss2_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ss2_Sheet1.Columns.Get(1).Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ss2_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss2_Sheet1.Columns.Get(1).Label = "병동";
            this.ss2_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType3.Multiline = true;
            textCellType3.WordWrap = true;
            this.ss2_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ss2_Sheet1.Columns.Get(2).Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ss2_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss2_Sheet1.Columns.Get(2).Label = "호실";
            this.ss2_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType4.Multiline = true;
            textCellType4.WordWrap = true;
            this.ss2_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ss2_Sheet1.Columns.Get(3).Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ss2_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss2_Sheet1.Columns.Get(3).Label = "아침";
            this.ss2_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss2_Sheet1.Columns.Get(3).Width = 120F;
            textCellType5.Multiline = true;
            textCellType5.WordWrap = true;
            this.ss2_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.ss2_Sheet1.Columns.Get(4).Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ss2_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss2_Sheet1.Columns.Get(4).Label = "점심";
            this.ss2_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss2_Sheet1.Columns.Get(4).Width = 120F;
            textCellType6.Multiline = true;
            textCellType6.WordWrap = true;
            this.ss2_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.ss2_Sheet1.Columns.Get(5).Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ss2_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss2_Sheet1.Columns.Get(5).Label = "저녁";
            this.ss2_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss2_Sheet1.Columns.Get(5).Width = 120F;
            this.ss2_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ss2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmHistory2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 669);
            this.Controls.Add(this.ss2);
            this.Controls.Add(this.ss1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Name = "frmHistory2";
            this.Text = "frmHistory2";
            this.Load += new System.EventHandler(this.frmHistory2_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss2_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker TxtTDate;
        private System.Windows.Forms.DateTimePicker TxtFDate;
        private System.Windows.Forms.TextBox txtPano;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label lblItem1;
        private System.Windows.Forms.Label lblItem0;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private FarPoint.Win.Spread.FpSpread ss1;
        private FarPoint.Win.Spread.SheetView ss1_Sheet1;
        private System.Windows.Forms.RadioButton rdo0;
        private System.Windows.Forms.RadioButton rdo1;
        private FarPoint.Win.Spread.FpSpread ss2;
        private FarPoint.Win.Spread.SheetView ss2_Sheet1;
    }
}