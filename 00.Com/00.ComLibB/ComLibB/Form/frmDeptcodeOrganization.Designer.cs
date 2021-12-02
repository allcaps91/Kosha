namespace ComLibB
{
    partial class frmDeptcodeOrganization
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder5 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder6 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder7 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder8 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.ss1 = new FarPoint.Win.Spread.FpSpread();
            this.ss1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).BeginInit();
            this.panTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // ss1
            // 
            this.ss1.AccessibleDescription = "ss1, Sheet1, Row 0, Column 0, ";
            this.ss1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ss1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ss1.Location = new System.Drawing.Point(0, 34);
            this.ss1.Name = "ss1";
            this.ss1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ss1_Sheet1});
            this.ss1.Size = new System.Drawing.Size(441, 499);
            this.ss1.TabIndex = 15;
            // 
            // ss1_Sheet1
            // 
            this.ss1_Sheet1.Reset();
            this.ss1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ss1_Sheet1.ColumnCount = 4;
            this.ss1_Sheet1.RowCount = 6;
            this.ss1_Sheet1.Cells.Get(0, 0).CellType = textCellType7;
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "대분류";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "중분류";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "상세분류";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "부서코드";
            this.ss1_Sheet1.ColumnHeader.Rows.Get(0).Height = 19F;
            this.ss1_Sheet1.Columns.Get(0).Border = complexBorder5;
            this.ss1_Sheet1.Columns.Get(0).CellType = textCellType8;
            this.ss1_Sheet1.Columns.Get(0).Label = "대분류";
            this.ss1_Sheet1.Columns.Get(0).Width = 79F;
            this.ss1_Sheet1.Columns.Get(1).Border = complexBorder6;
            this.ss1_Sheet1.Columns.Get(1).CellType = textCellType9;
            this.ss1_Sheet1.Columns.Get(1).Label = "중분류";
            this.ss1_Sheet1.Columns.Get(1).Width = 148F;
            this.ss1_Sheet1.Columns.Get(2).Border = complexBorder7;
            this.ss1_Sheet1.Columns.Get(2).CellType = textCellType10;
            this.ss1_Sheet1.Columns.Get(2).Label = "상세분류";
            this.ss1_Sheet1.Columns.Get(2).Width = 121F;
            this.ss1_Sheet1.Columns.Get(3).Border = complexBorder8;
            this.ss1_Sheet1.Columns.Get(3).CellType = textCellType11;
            this.ss1_Sheet1.Columns.Get(3).Label = "부서코드";
            this.ss1_Sheet1.Columns.Get(3).Width = 70F;
            this.ss1_Sheet1.DefaultStyle.CellType = textCellType12;
            this.ss1_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ss1_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss1_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.ss1_Sheet1.DefaultStyle.Renderer = textCellType12;
            this.ss1_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss1_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.None);
            this.ss1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ss1_Sheet1.RowHeader.Visible = false;
            this.ss1_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.None);
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 533);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(441, 23);
            this.progressBar1.TabIndex = 14;
            this.progressBar1.Value = 100;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnPrint);
            this.panTitle.Controls.Add(this.label1);
            this.panTitle.Controls.Add(this.btnClose);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(441, 34);
            this.panTitle.TabIndex = 16;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.Location = new System.Drawing.Point(289, 1);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 30);
            this.btnPrint.TabIndex = 5;
            this.btnPrint.Text = "인쇄";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(5, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "부서코드별 조직도";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Location = new System.Drawing.Point(361, 1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(72, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "닫기";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmDeptcodeOrganization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 556);
            this.Controls.Add(this.ss1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.panTitle);
            this.Name = "frmDeptcodeOrganization";
            this.Text = "부서코드별 조직도";
            this.Load += new System.EventHandler(this.frmDeptcodeOrganization_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).EndInit();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private FarPoint.Win.Spread.FpSpread ss1;
        private FarPoint.Win.Spread.SheetView ss1_Sheet1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
    }
}