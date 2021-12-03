namespace ComEmrBase
{
    partial class frmAnFormMapping
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType13 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType14 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType15 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType16 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(847, 34);
            this.panTitle.TabIndex = 15;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(771, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 28;
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
            this.lblTitle.Size = new System.Drawing.Size(214, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "마취기록지 수술스케줄 매핑";
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssView.Location = new System.Drawing.Point(0, 34);
            this.ssView.Name = "ssView";
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(847, 234);
            this.ssView.TabIndex = 16;
            this.ssView.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssView_CellDoubleClick);
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 8;
            this.ssView_Sheet1.RowCount = 1;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "수술일자";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "진단명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "수술명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "수술의사";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "수술간호사";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "진료과";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "상태";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "WRTNO";
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 40F;
            this.ssView_Sheet1.Columns.Get(0).CellType = textCellType9;
            this.ssView_Sheet1.Columns.Get(0).Label = "수술일자";
            this.ssView_Sheet1.Columns.Get(0).Locked = true;
            this.ssView_Sheet1.Columns.Get(0).Width = 110F;
            this.ssView_Sheet1.Columns.Get(1).CellType = textCellType10;
            this.ssView_Sheet1.Columns.Get(1).Label = "진단명";
            this.ssView_Sheet1.Columns.Get(1).Locked = true;
            this.ssView_Sheet1.Columns.Get(1).Width = 200F;
            this.ssView_Sheet1.Columns.Get(2).CellType = textCellType11;
            this.ssView_Sheet1.Columns.Get(2).Label = "수술명";
            this.ssView_Sheet1.Columns.Get(2).Locked = true;
            this.ssView_Sheet1.Columns.Get(2).Width = 200F;
            this.ssView_Sheet1.Columns.Get(3).CellType = textCellType12;
            this.ssView_Sheet1.Columns.Get(3).Label = "수술의사";
            this.ssView_Sheet1.Columns.Get(3).Locked = true;
            this.ssView_Sheet1.Columns.Get(3).Width = 80F;
            this.ssView_Sheet1.Columns.Get(4).CellType = textCellType13;
            this.ssView_Sheet1.Columns.Get(4).Label = "수술간호사";
            this.ssView_Sheet1.Columns.Get(4).Locked = true;
            this.ssView_Sheet1.Columns.Get(4).Width = 80F;
            this.ssView_Sheet1.Columns.Get(5).CellType = textCellType14;
            this.ssView_Sheet1.Columns.Get(5).Label = "진료과";
            this.ssView_Sheet1.Columns.Get(5).Locked = true;
            this.ssView_Sheet1.Columns.Get(6).CellType = textCellType15;
            this.ssView_Sheet1.Columns.Get(6).Label = "상태";
            this.ssView_Sheet1.Columns.Get(6).Locked = true;
            this.ssView_Sheet1.Columns.Get(7).CellType = textCellType16;
            this.ssView_Sheet1.Columns.Get(7).Label = "WRTNO";
            this.ssView_Sheet1.Columns.Get(7).Visible = false;
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmAnFormMapping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 268);
            this.Controls.Add(this.ssView);
            this.Controls.Add(this.panTitle);
            this.Name = "frmAnFormMapping";
            this.Text = "frmAnFormMapping";
            this.Load += new System.EventHandler(this.frmAnFormMapping_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
    }
}