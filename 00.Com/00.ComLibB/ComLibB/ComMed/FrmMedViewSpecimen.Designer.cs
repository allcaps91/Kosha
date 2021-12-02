namespace ComLibB
{
    partial class FrmMedViewSpecimen
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
            this.ssSpecimen = new FarPoint.Win.Spread.FpSpread();
            this.ssSpecimen_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.btnOK = new System.Windows.Forms.Button();
            this.panTitleSub = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.ssSpecimen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssSpecimen_Sheet1)).BeginInit();
            this.panTitleSub.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // ssSpecimen
            // 
            this.ssSpecimen.AccessibleDescription = "ssSpecimen, Sheet1, Row 0, Column 0, ";
            this.ssSpecimen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssSpecimen.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssSpecimen.Location = new System.Drawing.Point(0, 0);
            this.ssSpecimen.Name = "ssSpecimen";
            this.ssSpecimen.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssSpecimen_Sheet1});
            this.ssSpecimen.Size = new System.Drawing.Size(331, 410);
            this.ssSpecimen.TabIndex = 0;
            this.ssSpecimen.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssSpecimen.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssSpecimen_CellDoubleClick);
            // 
            // ssSpecimen_Sheet1
            // 
            this.ssSpecimen_Sheet1.Reset();
            this.ssSpecimen_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssSpecimen_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssSpecimen_Sheet1.ColumnCount = 2;
            this.ssSpecimen_Sheet1.RowCount = 30;
            this.ssSpecimen_Sheet1.RowHeader.ColumnCount = 0;
            this.ssSpecimen_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "검체명";
            this.ssSpecimen_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "검체코드";
            this.ssSpecimen_Sheet1.Columns.Get(0).CellType = textCellType9;
            this.ssSpecimen_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssSpecimen_Sheet1.Columns.Get(0).Label = "검체명";
            this.ssSpecimen_Sheet1.Columns.Get(0).Locked = true;
            this.ssSpecimen_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSpecimen_Sheet1.Columns.Get(0).Width = 232F;
            this.ssSpecimen_Sheet1.Columns.Get(1).CellType = textCellType10;
            this.ssSpecimen_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssSpecimen_Sheet1.Columns.Get(1).Label = "검체코드";
            this.ssSpecimen_Sheet1.Columns.Get(1).Locked = true;
            this.ssSpecimen_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSpecimen_Sheet1.Columns.Get(1).Width = 74F;
            this.ssSpecimen_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            this.ssSpecimen_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssSpecimen_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.None);
            this.ssSpecimen_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(222, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(81, 33);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "확인";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // panTitleSub
            // 
            this.panTitleSub.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub.Controls.Add(this.lblTitleSub0);
            this.panTitleSub.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub.Location = new System.Drawing.Point(0, 0);
            this.panTitleSub.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitleSub.Name = "panTitleSub";
            this.panTitleSub.Size = new System.Drawing.Size(331, 35);
            this.panTitleSub.TabIndex = 15;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(5, 6);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(100, 20);
            this.lblTitleSub0.TabIndex = 1;
            this.lblTitleSub0.Text = "OrderCode : ";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(331, 451);
            this.panel1.TabIndex = 16;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ssSpecimen);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(331, 410);
            this.panel2.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnOK);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 410);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(331, 41);
            this.panel3.TabIndex = 1;
            // 
            // FrmMedViewSpecimen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(331, 487);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitleSub);
            this.Name = "FrmMedViewSpecimen";
            this.Text = "FrmMedViewSpecimen";
            this.Load += new System.EventHandler(this.FrmMedViewSpecimen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ssSpecimen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssSpecimen_Sheet1)).EndInit();
            this.panTitleSub.ResumeLayout(false);
            this.panTitleSub.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FarPoint.Win.Spread.FpSpread ssSpecimen;
        private FarPoint.Win.Spread.SheetView ssSpecimen_Sheet1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel panTitleSub;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
    }
}