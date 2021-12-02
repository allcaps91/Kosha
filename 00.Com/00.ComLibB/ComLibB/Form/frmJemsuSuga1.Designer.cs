namespace ComLibB
{
    partial class frmJemsuSuga1
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
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("Color341636459106695492005", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle6 = new FarPoint.Win.Spread.NamedStyle("Text405636459106695557162", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle7 = new FarPoint.Win.Spread.NamedStyle("Static509636459106695577230");
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle8 = new FarPoint.Win.Spread.NamedStyle("Static545636459106695582242");
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance2 = new FarPoint.Win.Spread.TipAppearance();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnSaveJob = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblJob = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblItem1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtTCode = new System.Windows.Forms.TextBox();
            this.txtFCode = new System.Windows.Forms.TextBox();
            this.lblItem0 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.btnSaveStart = new System.Windows.Forms.Button();
            this.btnCancelJob = new System.Windows.Forms.Button();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panTitleSub1 = new System.Windows.Forms.Panel();
            this.lblTitleSub1 = new System.Windows.Forms.Label();
            this.ss1 = new FarPoint.Win.Spread.FpSpread();
            this.ss1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panTitleSub1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnSaveJob);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(590, 41);
            this.panTitle.TabIndex = 15;
            // 
            // btnSaveJob
            // 
            this.btnSaveJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveJob.BackColor = System.Drawing.Color.Transparent;
            this.btnSaveJob.Location = new System.Drawing.Point(431, 3);
            this.btnSaveJob.Name = "btnSaveJob";
            this.btnSaveJob.Size = new System.Drawing.Size(72, 30);
            this.btnSaveJob.TabIndex = 31;
            this.btnSaveJob.Text = "JOB";
            this.btnSaveJob.UseVisualStyleBackColor = false;
            this.btnSaveJob.Click += new System.EventHandler(this.btnSaveJob_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(504, 3);
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
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(264, 16);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "상대가치 점수기준 수가 일괄변경";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.lblJob);
            this.panel3.Controls.Add(this.groupBox4);
            this.panel3.Controls.Add(this.groupBox3);
            this.panel3.Controls.Add(this.groupBox2);
            this.panel3.Controls.Add(this.groupBox1);
            this.panel3.Controls.Add(this.btnSaveStart);
            this.panel3.Controls.Add(this.btnCancelJob);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 69);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(590, 160);
            this.panel3.TabIndex = 19;
            // 
            // lblJob
            // 
            this.lblJob.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblJob.Location = new System.Drawing.Point(442, 124);
            this.lblJob.Name = "lblJob";
            this.lblJob.Size = new System.Drawing.Size(127, 30);
            this.lblJob.TabIndex = 37;
            this.lblJob.Text = "label5";
            this.lblJob.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.lblItem1);
            this.groupBox4.Location = new System.Drawing.Point(176, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(483, 115);
            this.groupBox4.TabIndex = 36;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "작업설명";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(321, 12);
            this.label2.TabIndex = 26;
            this.label2.Text = "변경수가로 변경을 됩니다. (변경수가가 0원인것은 제외됨)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(374, 12);
            this.label1.TabIndex = 26;
            this.label1.Text = "2.\"수가변경시작(O)\"를 클릭하면 수가코드중 표준코드가 동일한 것은";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(188, 12);
            this.label4.TabIndex = 26;
            this.label4.Text = "4.비급여(F항=1)은 일괄변경 않함.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(295, 12);
            this.label3.TabIndex = 26;
            this.label3.Text = "3.일반수가,자보수가는 기준에 의해 자동 변경 됩니다.";
            // 
            // lblItem1
            // 
            this.lblItem1.AutoSize = true;
            this.lblItem1.Location = new System.Drawing.Point(14, 19);
            this.lblItem1.Name = "lblItem1";
            this.lblItem1.Size = new System.Drawing.Size(337, 12);
            this.lblItem1.TabIndex = 26;
            this.lblItem1.Text = "1.상대가치 점수 Table의 적용일자1이 수가적용일과 동일한것";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtTCode);
            this.groupBox3.Controls.Add(this.txtFCode);
            this.groupBox3.Controls.Add(this.lblItem0);
            this.groupBox3.Location = new System.Drawing.Point(13, 108);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(157, 44);
            this.groupBox3.TabIndex = 35;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "작업코드";
            // 
            // txtTCode
            // 
            this.txtTCode.Location = new System.Drawing.Point(91, 17);
            this.txtTCode.Name = "txtTCode";
            this.txtTCode.Size = new System.Drawing.Size(51, 21);
            this.txtTCode.TabIndex = 35;
            // 
            // txtFCode
            // 
            this.txtFCode.Location = new System.Drawing.Point(13, 17);
            this.txtFCode.Name = "txtFCode";
            this.txtFCode.Size = new System.Drawing.Size(51, 21);
            this.txtFCode.TabIndex = 34;
            // 
            // lblItem0
            // 
            this.lblItem0.AutoSize = true;
            this.lblItem0.Location = new System.Drawing.Point(68, 21);
            this.lblItem0.Name = "lblItem0";
            this.lblItem0.Size = new System.Drawing.Size(19, 12);
            this.lblItem0.TabIndex = 33;
            this.lblItem0.Text = "=>";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtPrice);
            this.groupBox2.Location = new System.Drawing.Point(13, 58);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(157, 44);
            this.groupBox2.TabIndex = 34;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "상대가치 단가";
            // 
            // txtPrice
            // 
            this.txtPrice.Location = new System.Drawing.Point(13, 16);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(129, 21);
            this.txtPrice.TabIndex = 32;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtpDate);
            this.groupBox1.Location = new System.Drawing.Point(13, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(157, 44);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "수가적용일";
            // 
            // dtpDate
            // 
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(13, 17);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(129, 21);
            this.dtpDate.TabIndex = 0;
            // 
            // btnSaveStart
            // 
            this.btnSaveStart.BackColor = System.Drawing.Color.Transparent;
            this.btnSaveStart.Location = new System.Drawing.Point(176, 124);
            this.btnSaveStart.Name = "btnSaveStart";
            this.btnSaveStart.Size = new System.Drawing.Size(127, 30);
            this.btnSaveStart.TabIndex = 28;
            this.btnSaveStart.Text = "수가변경시작";
            this.btnSaveStart.UseVisualStyleBackColor = false;
            this.btnSaveStart.Click += new System.EventHandler(this.btnSaveStart_Click);
            // 
            // btnCancelJob
            // 
            this.btnCancelJob.BackColor = System.Drawing.Color.Transparent;
            this.btnCancelJob.Location = new System.Drawing.Point(309, 124);
            this.btnCancelJob.Name = "btnCancelJob";
            this.btnCancelJob.Size = new System.Drawing.Size(127, 30);
            this.btnCancelJob.TabIndex = 23;
            this.btnCancelJob.Text = "작업취소";
            this.btnCancelJob.UseVisualStyleBackColor = false;
            this.btnCancelJob.Click += new System.EventHandler(this.btnCancelJob_Click);
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 41);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(590, 28);
            this.panTitleSub0.TabIndex = 18;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 4);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(187, 15);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "상대가치 점수기준 수가 일괄변경";
            // 
            // panTitleSub1
            // 
            this.panTitleSub1.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub1.Controls.Add(this.lblTitleSub1);
            this.panTitleSub1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub1.Location = new System.Drawing.Point(0, 229);
            this.panTitleSub1.Name = "panTitleSub1";
            this.panTitleSub1.Size = new System.Drawing.Size(590, 28);
            this.panTitleSub1.TabIndex = 20;
            // 
            // lblTitleSub1
            // 
            this.lblTitleSub1.AutoSize = true;
            this.lblTitleSub1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub1.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub1.Location = new System.Drawing.Point(8, 4);
            this.lblTitleSub1.Name = "lblTitleSub1";
            this.lblTitleSub1.Size = new System.Drawing.Size(119, 15);
            this.lblTitleSub1.TabIndex = 1;
            this.lblTitleSub1.Text = "상대가치점수 테이블";
            // 
            // ss1
            // 
            this.ss1.AccessibleDescription = "";
            this.ss1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ss1.Location = new System.Drawing.Point(0, 257);
            this.ss1.Name = "ss1";
            namedStyle5.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle5.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle5.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle5.Parent = "DataAreaDefault";
            namedStyle5.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType4.MaxLength = 60;
            namedStyle6.CellType = textCellType4;
            namedStyle6.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle6.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle6.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle6.Parent = "DataAreaDefault";
            namedStyle6.Renderer = textCellType4;
            namedStyle6.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType5.Static = true;
            namedStyle7.CellType = textCellType5;
            namedStyle7.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle7.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle7.Renderer = textCellType5;
            namedStyle7.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType6.Static = true;
            namedStyle8.CellType = textCellType6;
            namedStyle8.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            namedStyle8.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle8.Renderer = textCellType6;
            namedStyle8.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle5,
            namedStyle6,
            namedStyle7,
            namedStyle8});
            this.ss1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ss1_Sheet1});
            this.ss1.Size = new System.Drawing.Size(590, 426);
            this.ss1.TabIndex = 21;
            this.ss1.TabStripRatio = 0.6D;
            tipAppearance2.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance2.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ss1.TextTipAppearance = tipAppearance2;
            // 
            // ss1_Sheet1
            // 
            this.ss1_Sheet1.Reset();
            this.ss1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ss1_Sheet1.ColumnCount = 5;
            this.ss1_Sheet1.RowCount = 50;
            this.ss1_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "표준수가";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "상대가치점수";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "보험수가";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "변경건수";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "비고";
            this.ss1_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.ColumnHeader.Rows.Get(0).Height = 19F;
            this.ss1_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.Columns.Get(0).Label = "표준수가";
            this.ss1_Sheet1.Columns.Get(0).StyleName = "Static509636459106695577230";
            this.ss1_Sheet1.Columns.Get(0).Width = 81F;
            this.ss1_Sheet1.Columns.Get(1).Label = "상대가치점수";
            this.ss1_Sheet1.Columns.Get(1).StyleName = "Static545636459106695582242";
            this.ss1_Sheet1.Columns.Get(1).Width = 95F;
            this.ss1_Sheet1.Columns.Get(2).Label = "보험수가";
            this.ss1_Sheet1.Columns.Get(2).StyleName = "Static545636459106695582242";
            this.ss1_Sheet1.Columns.Get(2).Width = 100F;
            this.ss1_Sheet1.Columns.Get(3).Label = "변경건수";
            this.ss1_Sheet1.Columns.Get(3).StyleName = "Static509636459106695577230";
            this.ss1_Sheet1.Columns.Get(3).Width = 131F;
            this.ss1_Sheet1.Columns.Get(4).Label = "비고";
            this.ss1_Sheet1.Columns.Get(4).StyleName = "Static509636459106695577230";
            this.ss1_Sheet1.Columns.Get(4).Width = 117F;
            this.ss1_Sheet1.DefaultStyleName = "Text405636459106695557162";
            this.ss1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmJemsuSuga1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 683);
            this.Controls.Add(this.ss1);
            this.Controls.Add(this.panTitleSub1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Name = "frmJemsuSuga1";
            this.Text = "상대가치 점수기준 수가 일괄변경";
            this.Load += new System.EventHandler(this.frmJemsuSuga1_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panTitleSub1.ResumeLayout(false);
            this.panTitleSub1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSaveJob;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnSaveStart;
        private System.Windows.Forms.Button btnCancelJob;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panTitleSub1;
        private System.Windows.Forms.Label lblTitleSub1;
        private System.Windows.Forms.Label lblJob;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblItem1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtTCode;
        private System.Windows.Forms.TextBox txtFCode;
        private System.Windows.Forms.Label lblItem0;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private FarPoint.Win.Spread.FpSpread ss1;
        private FarPoint.Win.Spread.SheetView ss1_Sheet1;
    }
}