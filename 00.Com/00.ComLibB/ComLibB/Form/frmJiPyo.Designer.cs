namespace ComLibB
{
    partial class frmJiPyo
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
            FarPoint.Win.Spread.NamedStyle namedStyle6 = new FarPoint.Win.Spread.NamedStyle("Color355636716681977068754", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle7 = new FarPoint.Win.Spread.NamedStyle("Text437636716681977088804", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle8 = new FarPoint.Win.Spread.NamedStyle("CheckBox599636716681977590137");
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType2 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle9 = new FarPoint.Win.Spread.NamedStyle("Static635636716681977600161");
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle10 = new FarPoint.Win.Spread.NamedStyle("Static689636716681977620215");
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance2 = new FarPoint.Win.Spread.TipAppearance();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.pan0 = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.txtxx = new System.Windows.Forms.TextBox();
            this.cboYYMM = new System.Windows.Forms.ComboBox();
            this.lblitem0 = new System.Windows.Forms.Label();
            this.pan1 = new System.Windows.Forms.Panel();
            this.cboBuse = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pan2 = new System.Windows.Forms.Panel();
            this.ss1 = new FarPoint.Win.Spread.FpSpread();
            this.ss1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            this.pan0.SuspendLayout();
            this.pan1.SuspendLayout();
            this.pan2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnSave);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(541, 34);
            this.panTitle.TabIndex = 14;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.Location = new System.Drawing.Point(379, 1);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(84, 30);
            this.btnSave.TabIndex = 33;
            this.btnSave.Text = "작업시작(&S)";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(7, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(144, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "지표결과 자동생성";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(463, 1);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pan0
            // 
            this.pan0.BackColor = System.Drawing.Color.White;
            this.pan0.Controls.Add(this.progressBar1);
            this.pan0.Controls.Add(this.txtxx);
            this.pan0.Controls.Add(this.cboYYMM);
            this.pan0.Controls.Add(this.lblitem0);
            this.pan0.Dock = System.Windows.Forms.DockStyle.Top;
            this.pan0.Location = new System.Drawing.Point(0, 34);
            this.pan0.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pan0.Name = "pan0";
            this.pan0.Size = new System.Drawing.Size(541, 62);
            this.pan0.TabIndex = 16;
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 32);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(541, 30);
            this.progressBar1.TabIndex = 2;
            // 
            // txtxx
            // 
            this.txtxx.Location = new System.Drawing.Point(195, 4);
            this.txtxx.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtxx.Name = "txtxx";
            this.txtxx.Size = new System.Drawing.Size(318, 25);
            this.txtxx.TabIndex = 2;
            // 
            // cboYYMM
            // 
            this.cboYYMM.FormattingEnabled = true;
            this.cboYYMM.Location = new System.Drawing.Point(12, 4);
            this.cboYYMM.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboYYMM.Name = "cboYYMM";
            this.cboYYMM.Size = new System.Drawing.Size(121, 25);
            this.cboYYMM.TabIndex = 1;
            // 
            // lblitem0
            // 
            this.lblitem0.AutoSize = true;
            this.lblitem0.Location = new System.Drawing.Point(139, 9);
            this.lblitem0.Name = "lblitem0";
            this.lblitem0.Size = new System.Drawing.Size(34, 17);
            this.lblitem0.TabIndex = 0;
            this.lblitem0.Text = "년월";
            // 
            // pan1
            // 
            this.pan1.BackColor = System.Drawing.Color.White;
            this.pan1.Controls.Add(this.cboBuse);
            this.pan1.Controls.Add(this.label4);
            this.pan1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pan1.Location = new System.Drawing.Point(0, 96);
            this.pan1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pan1.Name = "pan1";
            this.pan1.Size = new System.Drawing.Size(541, 30);
            this.pan1.TabIndex = 17;
            // 
            // cboBuse
            // 
            this.cboBuse.FormattingEnabled = true;
            this.cboBuse.Location = new System.Drawing.Point(69, 3);
            this.cboBuse.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboBuse.Name = "cboBuse";
            this.cboBuse.Size = new System.Drawing.Size(160, 25);
            this.cboBuse.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "담당부서";
            // 
            // pan2
            // 
            this.pan2.Controls.Add(this.ss1);
            this.pan2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pan2.Location = new System.Drawing.Point(0, 126);
            this.pan2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pan2.Name = "pan2";
            this.pan2.Size = new System.Drawing.Size(541, 563);
            this.pan2.TabIndex = 18;
            // 
            // ss1
            // 
            this.ss1.AccessibleDescription = "ss1, Sheet1, Row 0, Column 0, ";
            this.ss1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ss1.Location = new System.Drawing.Point(0, 0);
            this.ss1.Name = "ss1";
            namedStyle6.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle6.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle6.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle6.Parent = "DataAreaDefault";
            namedStyle6.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType4.MaxLength = 32000;
            namedStyle7.CellType = textCellType4;
            namedStyle7.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle7.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle7.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle7.Parent = "DataAreaDefault";
            namedStyle7.Renderer = textCellType4;
            namedStyle7.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle8.CellType = checkBoxCellType2;
            namedStyle8.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle8.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle8.Renderer = checkBoxCellType2;
            namedStyle8.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType5.Static = true;
            namedStyle9.CellType = textCellType5;
            namedStyle9.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle9.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle9.Renderer = textCellType5;
            namedStyle9.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType6.Static = true;
            namedStyle10.CellType = textCellType6;
            namedStyle10.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle10.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle10.Renderer = textCellType6;
            namedStyle10.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle6,
            namedStyle7,
            namedStyle8,
            namedStyle9,
            namedStyle10});
            this.ss1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ss1_Sheet1});
            this.ss1.Size = new System.Drawing.Size(541, 563);
            this.ss1.TabIndex = 0;
            this.ss1.TabStripRatio = 0.6D;
            tipAppearance2.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance2.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ss1.TextTipAppearance = tipAppearance2;
            this.ss1.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ss1_CellClick);
            // 
            // ss1_Sheet1
            // 
            this.ss1_Sheet1.Reset();
            this.ss1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ss1_Sheet1.ColumnCount = 5;
            this.ss1_Sheet1.RowCount = 1;
            this.ss1_Sheet1.Cells.Get(0, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss1_Sheet1.Cells.Get(0, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Cells.Get(0, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ss1_Sheet1.Cells.Get(0, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "코드";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "지표명칭";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "시작";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "종료";
            this.ss1_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ss1_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ss1_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss1_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ss1_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss1_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.Columns.Get(0).StyleName = "CheckBox599636716681977590137";
            this.ss1_Sheet1.Columns.Get(0).Width = 19F;
            this.ss1_Sheet1.Columns.Get(1).Label = "코드";
            this.ss1_Sheet1.Columns.Get(1).StyleName = "Static635636716681977600161";
            this.ss1_Sheet1.Columns.Get(1).Width = 65F;
            this.ss1_Sheet1.Columns.Get(2).Label = "지표명칭";
            this.ss1_Sheet1.Columns.Get(2).StyleName = "Static635636716681977600161";
            this.ss1_Sheet1.Columns.Get(2).Width = 275F;
            this.ss1_Sheet1.Columns.Get(3).Label = "시작";
            this.ss1_Sheet1.Columns.Get(3).StyleName = "Static689636716681977620215";
            this.ss1_Sheet1.Columns.Get(4).Label = "종료";
            this.ss1_Sheet1.Columns.Get(4).StyleName = "Static689636716681977620215";
            this.ss1_Sheet1.DefaultStyleName = "Text437636716681977088804";
            this.ss1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ss1_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ss1_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ss1_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss1_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ss1_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss1_Sheet1.Rows.Default.Height = 17F;
            this.ss1_Sheet1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ss1_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ss1_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ss1_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss1_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ss1_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmJiPyo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 689);
            this.Controls.Add(this.pan2);
            this.Controls.Add(this.pan1);
            this.Controls.Add(this.pan0);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmJiPyo";
            this.Text = "frmJiPyo";
            this.Load += new System.EventHandler(this.frmJiPyo_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.pan0.ResumeLayout(false);
            this.pan0.PerformLayout();
            this.pan1.ResumeLayout(false);
            this.pan1.PerformLayout();
            this.pan2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel pan0;
        private System.Windows.Forms.TextBox txtxx;
        private System.Windows.Forms.ComboBox cboYYMM;
        private System.Windows.Forms.Label lblitem0;
        private System.Windows.Forms.Panel pan1;
        private System.Windows.Forms.ComboBox cboBuse;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pan2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private FarPoint.Win.Spread.FpSpread ss1;
        private FarPoint.Win.Spread.SheetView ss1_Sheet1;
    }
}