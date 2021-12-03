namespace ComLibB
{
    partial class frmOvicuPatientSearch
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color396636310630467680854", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Static490636310630467680854", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static695636310630467837119");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.panTitle0 = new System.Windows.Forms.Panel();
            this.btnView = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.ssIPD = new FarPoint.Win.Spread.FpSpread();
            this.ssIPD_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssIPD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssIPD_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle0
            // 
            this.panTitle0.BackColor = System.Drawing.Color.White;
            this.panTitle0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle0.Controls.Add(this.btnView);
            this.panTitle0.Controls.Add(this.btnExit);
            this.panTitle0.Controls.Add(this.lblTitle);
            this.panTitle0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle0.ForeColor = System.Drawing.Color.White;
            this.panTitle0.Location = new System.Drawing.Point(0, 0);
            this.panTitle0.Name = "panTitle0";
            this.panTitle0.Size = new System.Drawing.Size(763, 38);
            this.panTitle0.TabIndex = 76;
            // 
            // btnView
            // 
            this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnView.AutoSize = true;
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnView.Location = new System.Drawing.Point(598, 3);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(79, 29);
            this.btnView.TabIndex = 16;
            this.btnView.Text = "조  회";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(677, 3);
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
            this.lblTitle.Size = new System.Drawing.Size(114, 21);
            this.lblTitle.TabIndex = 14;
            this.lblTitle.Text = "집 중 치 료 실";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ssIPD
            // 
            this.ssIPD.AccessibleDescription = "fpSpread1, Sheet1, Row 0, Column 0, ";
            this.ssIPD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssIPD.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssIPD.Location = new System.Drawing.Point(0, 38);
            this.ssIPD.Name = "ssIPD";
            namedStyle1.Font = new System.Drawing.Font("굴림체", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType1.Static = true;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("굴림체", 9F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType2.Static = true;
            namedStyle3.CellType = textCellType2;
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.Renderer = textCellType2;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssIPD.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3});
            this.ssIPD.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssIPD_Sheet1});
            this.ssIPD.Size = new System.Drawing.Size(763, 545);
            this.ssIPD.TabIndex = 77;
            this.ssIPD.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssIPD.TextTipAppearance = tipAppearance1;
            this.ssIPD.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // ssIPD_Sheet1
            // 
            this.ssIPD_Sheet1.Reset();
            this.ssIPD_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssIPD_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssIPD_Sheet1.ColumnCount = 9;
            this.ssIPD_Sheet1.RowCount = 1;
            this.ssIPD_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssIPD_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "병동";
            this.ssIPD_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "성 명";
            this.ssIPD_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "나이";
            this.ssIPD_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "성별";
            this.ssIPD_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "주민등록번호";
            this.ssIPD_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "주            소";
            this.ssIPD_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "진료과";
            this.ssIPD_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "전문의";
            this.ssIPD_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "보호자";
            this.ssIPD_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssIPD_Sheet1.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ssIPD_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssIPD_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssIPD_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssIPD_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssIPD_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssIPD_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssIPD_Sheet1.Columns.Get(0).Label = "병동";
            this.ssIPD_Sheet1.Columns.Get(0).Width = 63F;
            this.ssIPD_Sheet1.Columns.Get(1).Label = "성 명";
            this.ssIPD_Sheet1.Columns.Get(1).Width = 76F;
            this.ssIPD_Sheet1.Columns.Get(2).Label = "나이";
            this.ssIPD_Sheet1.Columns.Get(2).Width = 34F;
            this.ssIPD_Sheet1.Columns.Get(3).Label = "성별";
            this.ssIPD_Sheet1.Columns.Get(3).Width = 36F;
            this.ssIPD_Sheet1.Columns.Get(4).Label = "주민등록번호";
            this.ssIPD_Sheet1.Columns.Get(4).Width = 110F;
            this.ssIPD_Sheet1.Columns.Get(5).Label = "주            소";
            this.ssIPD_Sheet1.Columns.Get(5).StyleName = "Static695636310630467837119";
            this.ssIPD_Sheet1.Columns.Get(5).Width = 250F;
            this.ssIPD_Sheet1.Columns.Get(6).Label = "진료과";
            this.ssIPD_Sheet1.Columns.Get(6).Width = 48F;
            this.ssIPD_Sheet1.Columns.Get(7).Label = "전문의";
            this.ssIPD_Sheet1.Columns.Get(7).Width = 66F;
            this.ssIPD_Sheet1.Columns.Get(8).Label = "보호자";
            this.ssIPD_Sheet1.Columns.Get(8).Width = 76F;
            this.ssIPD_Sheet1.DefaultStyleName = "Static490636310630467680854";
            this.ssIPD_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ssIPD_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssIPD_Sheet1.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ssIPD_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssIPD_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssIPD_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssIPD_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssIPD_Sheet1.RowHeader.Visible = false;
            this.ssIPD_Sheet1.SheetCornerStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ssIPD_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssIPD_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssIPD_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssIPD_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssIPD_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmOvicuPatientSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 583);
            this.Controls.Add(this.ssIPD);
            this.Controls.Add(this.panTitle0);
            this.Name = "frmOvicuPatientSearch";
            this.Text = "집중치료실(입원환자)";
            this.Load += new System.EventHandler(this.frmOvicuPatientSearch_Load);
            this.panTitle0.ResumeLayout(false);
            this.panTitle0.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssIPD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssIPD_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle0;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnView;
        private FarPoint.Win.Spread.FpSpread ssIPD;
        private FarPoint.Win.Spread.SheetView ssIPD_Sheet1;
    }
}