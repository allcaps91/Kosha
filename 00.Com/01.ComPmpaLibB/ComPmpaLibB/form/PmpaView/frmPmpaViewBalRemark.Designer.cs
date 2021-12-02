namespace ComPmpaLibB
{
    partial class frmPmpaViewBalRemark
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
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnText = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.rtbRemark = new System.Windows.Forms.RichTextBox();
            this.ssPrint = new FarPoint.Win.Spread.FpSpread();
            this.ssPrint_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssPrint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssPrint_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnText);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.btnPrint);
            this.panTitle.Controls.Add(this.btnSave);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(745, 34);
            this.panTitle.TabIndex = 12;
            // 
            // btnText
            // 
            this.btnText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnText.BackColor = System.Drawing.Color.Transparent;
            this.btnText.Location = new System.Drawing.Point(522, 0);
            this.btnText.Name = "btnText";
            this.btnText.Size = new System.Drawing.Size(72, 30);
            this.btnText.TabIndex = 27;
            this.btnText.Text = "글꼴";
            this.btnText.UseVisualStyleBackColor = false;
            this.btnText.Click += new System.EventHandler(this.btnText_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(220, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "발생주의 통계수치 자료 설명";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(666, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.Location = new System.Drawing.Point(594, 0);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 30);
            this.btnPrint.TabIndex = 22;
            this.btnPrint.Text = "출력";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.Location = new System.Drawing.Point(450, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 30);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // rtbRemark
            // 
            this.rtbRemark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbRemark.Location = new System.Drawing.Point(0, 34);
            this.rtbRemark.Name = "rtbRemark";
            this.rtbRemark.Size = new System.Drawing.Size(745, 385);
            this.rtbRemark.TabIndex = 19;
            this.rtbRemark.Text = "";
            // 
            // ssPrint
            // 
            this.ssPrint.AccessibleDescription = "";
            this.ssPrint.Location = new System.Drawing.Point(250, 12);
            this.ssPrint.Name = "ssPrint";
            this.ssPrint.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssPrint_Sheet1});
            this.ssPrint.Size = new System.Drawing.Size(100, 10);
            this.ssPrint.TabIndex = 20;
            this.ssPrint.Visible = false;
            // 
            // ssPrint_Sheet1
            // 
            this.ssPrint_Sheet1.Reset();
            this.ssPrint_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssPrint_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssPrint_Sheet1.ColumnCount = 1;
            this.ssPrint_Sheet1.RowCount = 1;
            textCellType1.Multiline = true;
            textCellType1.WordWrap = true;
            this.ssPrint_Sheet1.Cells.Get(0, 0).CellType = textCellType1;
            this.ssPrint_Sheet1.Cells.Get(0, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssPrint_Sheet1.Cells.Get(0, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.ssPrint_Sheet1.Columns.Get(0).CellType = textCellType2;
            this.ssPrint_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssPrint_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.ssPrint_Sheet1.Columns.Get(0).Width = 700F;
            this.ssPrint_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssPrint_Sheet1.Rows.Get(0).Height = 900F;
            this.ssPrint_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmPmpaViewBalRemark
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(745, 419);
            this.Controls.Add(this.ssPrint);
            this.Controls.Add(this.rtbRemark);
            this.Controls.Add(this.panTitle);
            this.Name = "frmPmpaViewBalRemark";
            this.Text = "발생주의 통계수치 자료 설명";
            this.Load += new System.EventHandler(this.frmPmpaViewBalRemark_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssPrint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssPrint_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnText;
        public System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.RichTextBox rtbRemark;
        private FarPoint.Win.Spread.FpSpread ssPrint;
        private FarPoint.Win.Spread.SheetView ssPrint_Sheet1;
    }
}