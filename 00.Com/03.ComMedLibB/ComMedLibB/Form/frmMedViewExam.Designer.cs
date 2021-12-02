namespace ComMedLibB
{
    partial class frmMedViewExam
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
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rdoExam3 = new System.Windows.Forms.RadioButton();
            this.rdoExam2 = new System.Windows.Forms.RadioButton();
            this.rdoExam1 = new System.Windows.Forms.RadioButton();
            this.rdoExam0 = new System.Windows.Forms.RadioButton();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.btnSearch);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(846, 34);
            this.panTitle.TabIndex = 12;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(752, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(90, 30);
            this.btnExit.TabIndex = 27;
            this.btnExit.Text = "닫  기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(663, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(90, 30);
            this.btnSearch.TabIndex = 27;
            this.btnSearch.Text = "조  회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(232, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "미시행 검사 내역 조회 및 정리";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rdoExam3);
            this.panel1.Controls.Add(this.rdoExam2);
            this.panel1.Controls.Add(this.rdoExam1);
            this.panel1.Controls.Add(this.rdoExam0);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(846, 34);
            this.panel1.TabIndex = 13;
            // 
            // rdoExam3
            // 
            this.rdoExam3.AutoSize = true;
            this.rdoExam3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.rdoExam3.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.rdoExam3.ForeColor = System.Drawing.Color.Black;
            this.rdoExam3.Location = new System.Drawing.Point(332, 6);
            this.rdoExam3.Name = "rdoExam3";
            this.rdoExam3.Size = new System.Drawing.Size(116, 23);
            this.rdoExam3.TabIndex = 16;
            this.rdoExam3.Text = "외래 입원처방";
            this.rdoExam3.UseVisualStyleBackColor = false;
            this.rdoExam3.CheckedChanged += new System.EventHandler(this.rdoExam_CheckedChanged);
            // 
            // rdoExam2
            // 
            this.rdoExam2.AutoSize = true;
            this.rdoExam2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.rdoExam2.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.rdoExam2.ForeColor = System.Drawing.Color.Black;
            this.rdoExam2.Location = new System.Drawing.Point(227, 6);
            this.rdoExam2.Name = "rdoExam2";
            this.rdoExam2.Size = new System.Drawing.Size(97, 23);
            this.rdoExam2.TabIndex = 15;
            this.rdoExam2.Text = "결과값없음";
            this.rdoExam2.UseVisualStyleBackColor = false;
            this.rdoExam2.CheckedChanged += new System.EventHandler(this.rdoExam_CheckedChanged);
            // 
            // rdoExam1
            // 
            this.rdoExam1.AutoSize = true;
            this.rdoExam1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.rdoExam1.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.rdoExam1.ForeColor = System.Drawing.Color.Black;
            this.rdoExam1.Location = new System.Drawing.Point(122, 6);
            this.rdoExam1.Name = "rdoExam1";
            this.rdoExam1.Size = new System.Drawing.Size(97, 23);
            this.rdoExam1.TabIndex = 14;
            this.rdoExam1.Text = "결과값있음";
            this.rdoExam1.UseVisualStyleBackColor = false;
            this.rdoExam1.CheckedChanged += new System.EventHandler(this.rdoExam_CheckedChanged);
            // 
            // rdoExam0
            // 
            this.rdoExam0.AutoSize = true;
            this.rdoExam0.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rdoExam0.Checked = true;
            this.rdoExam0.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.rdoExam0.ForeColor = System.Drawing.Color.Black;
            this.rdoExam0.Location = new System.Drawing.Point(17, 6);
            this.rdoExam0.Name = "rdoExam0";
            this.rdoExam0.Size = new System.Drawing.Size(97, 23);
            this.rdoExam0.TabIndex = 13;
            this.rdoExam0.TabStop = true;
            this.rdoExam0.Text = "미시행검사";
            this.rdoExam0.UseVisualStyleBackColor = false;
            this.rdoExam0.CheckedChanged += new System.EventHandler(this.rdoExam_CheckedChanged);
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(846, 28);
            this.panTitleSub0.TabIndex = 14;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 1);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(65, 19);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "조회조건";
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "ssView, Sheet1, Row 0, Column 0, ";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssView.Location = new System.Drawing.Point(0, 96);
            this.ssView.Name = "ssView";
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(846, 416);
            this.ssView.TabIndex = 16;
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 8;
            this.ssView_Sheet1.RowCount = 1;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "처방일";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "시행일";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "검 사 명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "검체";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "SpecNo";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "workSts";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "결과시간";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "처방구분";
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssView_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssView_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(0).Label = "처방일";
            this.ssView_Sheet1.Columns.Get(0).Locked = true;
            this.ssView_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(0).Width = 85F;
            this.ssView_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssView_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(1).Label = "시행일";
            this.ssView_Sheet1.Columns.Get(1).Locked = true;
            this.ssView_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(1).Width = 85F;
            this.ssView_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssView_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssView_Sheet1.Columns.Get(2).Label = "검 사 명";
            this.ssView_Sheet1.Columns.Get(2).Locked = true;
            this.ssView_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(2).Width = 300F;
            this.ssView_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssView_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssView_Sheet1.Columns.Get(3).Label = "검체";
            this.ssView_Sheet1.Columns.Get(3).Locked = true;
            this.ssView_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(3).Width = 100F;
            this.ssView_Sheet1.Columns.Get(4).Label = "SpecNo";
            this.ssView_Sheet1.Columns.Get(4).Visible = false;
            this.ssView_Sheet1.Columns.Get(5).Label = "workSts";
            this.ssView_Sheet1.Columns.Get(5).Visible = false;
            this.ssView_Sheet1.Columns.Get(6).CellType = textCellType5;
            this.ssView_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(6).Label = "결과시간";
            this.ssView_Sheet1.Columns.Get(6).Locked = true;
            this.ssView_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(6).Width = 140F;
            this.ssView_Sheet1.Columns.Get(7).CellType = textCellType6;
            this.ssView_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(7).Label = "처방구분";
            this.ssView_Sheet1.Columns.Get(7).Locked = true;
            this.ssView_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(7).Width = 80F;
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmMedViewExam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(846, 512);
            this.Controls.Add(this.ssView);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmMedViewExam";
            this.Text = "frmMedViewExam";
            this.Load += new System.EventHandler(this.frmMedViewExam_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.RadioButton rdoExam3;
        private System.Windows.Forms.RadioButton rdoExam2;
        private System.Windows.Forms.RadioButton rdoExam1;
        private System.Windows.Forms.RadioButton rdoExam0;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
    }
}