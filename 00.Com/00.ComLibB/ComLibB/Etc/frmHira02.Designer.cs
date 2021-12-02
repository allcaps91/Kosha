namespace ComLibB
{
    partial class frmHira02
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color361636626716483480584", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text425636626716483490605", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Color515636626716483500630");
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Static551636626716483510658");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("Static587636626716483520740");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ss1 = new FarPoint.Win.Spread.FpSpread();
            this.ss1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rdoGB3 = new System.Windows.Forms.RadioButton();
            this.rdoGB2 = new System.Windows.Forms.RadioButton();
            this.rdoGB1 = new System.Windows.Forms.RadioButton();
            this.rdoGB0 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.chkOPD = new System.Windows.Forms.CheckBox();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
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
            this.panTitle.Size = new System.Drawing.Size(828, 34);
            this.panTitle.TabIndex = 15;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(752, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 3;
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
            this.lblTitle.Size = new System.Drawing.Size(161, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "심평원자료-연령금기";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ss1);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(828, 487);
            this.panel1.TabIndex = 17;
            // 
            // ss1
            // 
            this.ss1.AccessibleDescription = "ss1, Sheet1, Row 0, Column 0, ";
            this.ss1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ss1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ss1.Location = new System.Drawing.Point(0, 30);
            this.ss1.Name = "ss1";
            namedStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType1.MaxLength = 32000;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            namedStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType2.Static = true;
            namedStyle4.CellType = textCellType2;
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.Renderer = textCellType2;
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType3.Static = true;
            namedStyle5.CellType = textCellType3;
            namedStyle5.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle5.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle5.Renderer = textCellType3;
            namedStyle5.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4,
            namedStyle5});
            this.ss1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ss1_Sheet1});
            this.ss1.Size = new System.Drawing.Size(828, 428);
            this.ss1.TabIndex = 2;
            this.ss1.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ss1.TextTipAppearance = tipAppearance1;
            this.ss1.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // ss1_Sheet1
            // 
            this.ss1_Sheet1.Reset();
            this.ss1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ss1_Sheet1.ColumnCount = 9;
            this.ss1_Sheet1.RowCount = 30;
            this.ss1_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "고지일자";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "수가코드";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "수가명";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "성분코드";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "제한연령";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "제한구분";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "적용일(부터)";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "적용일(까지)";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "수가삭제일";
            this.ss1_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.ColumnHeader.Rows.Get(0).Height = 27F;
            this.ss1_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.Columns.Get(0).Label = "고지일자";
            this.ss1_Sheet1.Columns.Get(0).StyleName = "Color515636626716483500630";
            this.ss1_Sheet1.Columns.Get(0).Width = 62F;
            this.ss1_Sheet1.Columns.Get(1).Label = "수가코드";
            this.ss1_Sheet1.Columns.Get(1).StyleName = "Static551636626716483510658";
            this.ss1_Sheet1.Columns.Get(1).Width = 66F;
            this.ss1_Sheet1.Columns.Get(2).Label = "수가명";
            this.ss1_Sheet1.Columns.Get(2).StyleName = "Static587636626716483520740";
            this.ss1_Sheet1.Columns.Get(2).Width = 193F;
            this.ss1_Sheet1.Columns.Get(3).Label = "성분코드";
            this.ss1_Sheet1.Columns.Get(3).StyleName = "Static551636626716483510658";
            this.ss1_Sheet1.Columns.Get(3).Width = 73F;
            this.ss1_Sheet1.Columns.Get(4).Label = "제한연령";
            this.ss1_Sheet1.Columns.Get(4).StyleName = "Static551636626716483510658";
            this.ss1_Sheet1.Columns.Get(4).Width = 62F;
            this.ss1_Sheet1.Columns.Get(5).Label = "제한구분";
            this.ss1_Sheet1.Columns.Get(5).StyleName = "Static551636626716483510658";
            this.ss1_Sheet1.Columns.Get(5).Width = 70F;
            this.ss1_Sheet1.Columns.Get(6).Label = "적용일(부터)";
            this.ss1_Sheet1.Columns.Get(6).StyleName = "Static551636626716483510658";
            this.ss1_Sheet1.Columns.Get(6).Width = 83F;
            this.ss1_Sheet1.Columns.Get(7).Label = "적용일(까지)";
            this.ss1_Sheet1.Columns.Get(7).StyleName = "Static551636626716483510658";
            this.ss1_Sheet1.Columns.Get(7).Width = 83F;
            this.ss1_Sheet1.Columns.Get(8).Label = "수가삭제일";
            this.ss1_Sheet1.Columns.Get(8).StyleName = "Static551636626716483510658";
            this.ss1_Sheet1.Columns.Get(8).Width = 75F;
            this.ss1_Sheet1.DefaultStyleName = "Text425636626716483490605";
            this.ss1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 458);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(828, 29);
            this.panel4.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(828, 29);
            this.label3.TabIndex = 2;
            this.label3.Text = "이자료는 심평원자료와 자동 연동하여 본원 수가로 조회 됩니다.";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCopy);
            this.panel2.Controls.Add(this.btnPrint);
            this.panel2.Controls.Add(this.btnSearch);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.chkOPD);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(828, 30);
            this.panel2.TabIndex = 0;
            // 
            // btnCopy
            // 
            this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopy.AutoSize = true;
            this.btnCopy.BackColor = System.Drawing.Color.Transparent;
            this.btnCopy.Location = new System.Drawing.Point(591, 0);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(153, 30);
            this.btnCopy.TabIndex = 3;
            this.btnCopy.Text = "외래제한사항으로 복사";
            this.btnCopy.UseVisualStyleBackColor = false;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.Location = new System.Drawing.Point(519, 0);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 30);
            this.btnPrint.TabIndex = 3;
            this.btnPrint.Text = "출력";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Location = new System.Drawing.Point(447, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.rdoGB3);
            this.panel3.Controls.Add(this.rdoGB2);
            this.panel3.Controls.Add(this.rdoGB1);
            this.panel3.Controls.Add(this.rdoGB0);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Location = new System.Drawing.Point(109, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(318, 30);
            this.panel3.TabIndex = 1;
            // 
            // rdoGB3
            // 
            this.rdoGB3.AutoSize = true;
            this.rdoGB3.Location = new System.Drawing.Point(253, 3);
            this.rdoGB3.Name = "rdoGB3";
            this.rdoGB3.Size = new System.Drawing.Size(52, 21);
            this.rdoGB3.TabIndex = 1;
            this.rdoGB3.Text = "미만";
            this.rdoGB3.UseVisualStyleBackColor = true;
            // 
            // rdoGB2
            // 
            this.rdoGB2.AutoSize = true;
            this.rdoGB2.Location = new System.Drawing.Point(193, 3);
            this.rdoGB2.Name = "rdoGB2";
            this.rdoGB2.Size = new System.Drawing.Size(52, 21);
            this.rdoGB2.TabIndex = 1;
            this.rdoGB2.Text = "이하";
            this.rdoGB2.UseVisualStyleBackColor = true;
            // 
            // rdoGB1
            // 
            this.rdoGB1.AutoSize = true;
            this.rdoGB1.Location = new System.Drawing.Point(133, 3);
            this.rdoGB1.Name = "rdoGB1";
            this.rdoGB1.Size = new System.Drawing.Size(52, 21);
            this.rdoGB1.TabIndex = 1;
            this.rdoGB1.Text = "이상";
            this.rdoGB1.UseVisualStyleBackColor = true;
            // 
            // rdoGB0
            // 
            this.rdoGB0.AutoSize = true;
            this.rdoGB0.Checked = true;
            this.rdoGB0.Location = new System.Drawing.Point(73, 3);
            this.rdoGB0.Name = "rdoGB0";
            this.rdoGB0.Size = new System.Drawing.Size(52, 21);
            this.rdoGB0.TabIndex = 1;
            this.rdoGB0.TabStop = true;
            this.rdoGB0.Text = "전체";
            this.rdoGB0.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "제한구분";
            // 
            // chkOPD
            // 
            this.chkOPD.AutoSize = true;
            this.chkOPD.Checked = true;
            this.chkOPD.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOPD.Location = new System.Drawing.Point(24, 5);
            this.chkOPD.Name = "chkOPD";
            this.chkOPD.Size = new System.Drawing.Size(79, 21);
            this.chkOPD.TabIndex = 0;
            this.chkOPD.Text = "병원적용";
            this.chkOPD.UseVisualStyleBackColor = true;
            // 
            // frmHira02
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(828, 521);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHira02";
            this.Text = "frmHira02";
            this.Load += new System.EventHandler(this.frmHira02_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox chkOPD;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton rdoGB3;
        private System.Windows.Forms.RadioButton rdoGB2;
        private System.Windows.Forms.RadioButton rdoGB1;
        private System.Windows.Forms.RadioButton rdoGB0;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label3;
        private FarPoint.Win.Spread.FpSpread ss1;
        private FarPoint.Win.Spread.SheetView ss1_Sheet1;
    }
}