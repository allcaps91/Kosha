namespace ComPmpaLibB
{
    partial class frmMcrtJobViewFav
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
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ssFav = new FarPoint.Win.Spread.FpSpread();
            this.ssFav_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtFavText = new System.Windows.Forms.TextBox();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssFav)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssFav_Sheet1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(424, 34);
            this.panTitle.TabIndex = 13;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(348, 0);
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
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(5, 4);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(150, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "◈ 상    용    구 ◈";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ssFav);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(424, 374);
            this.panel1.TabIndex = 15;
            // 
            // ssFav
            // 
            this.ssFav.AccessibleDescription = "";
            this.ssFav.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssFav.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssFav.Location = new System.Drawing.Point(0, 0);
            this.ssFav.Name = "ssFav";
            this.ssFav.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssFav_Sheet1});
            this.ssFav.Size = new System.Drawing.Size(424, 279);
            this.ssFav.TabIndex = 16;
            this.ssFav.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssFav_CellClick);
            this.ssFav.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssFav_CellDoubleClick);
            this.ssFav.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ssFav_KeyDown);
            this.ssFav.SetActiveViewport(0, -1, -1);
            // 
            // ssFav_Sheet1
            // 
            this.ssFav_Sheet1.Reset();
            this.ssFav_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssFav_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssFav_Sheet1.ColumnCount = 2;
            this.ssFav_Sheet1.ColumnHeader.RowCount = 0;
            this.ssFav_Sheet1.RowCount = 0;
            this.ssFav_Sheet1.ActiveColumnIndex = -1;
            this.ssFav_Sheet1.ActiveRowIndex = -1;
            textCellType1.Multiline = true;
            textCellType1.WordWrap = true;
            this.ssFav_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssFav_Sheet1.Columns.Get(0).Locked = true;
            this.ssFav_Sheet1.Columns.Get(0).Width = 400F;
            textCellType2.MaxLength = 9999;
            this.ssFav_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssFav_Sheet1.Columns.Get(1).Visible = false;
            this.ssFav_Sheet1.Columns.Get(1).Width = 200F;
            this.ssFav_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            this.ssFav_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssFav_Sheet1.RowHeader.Columns.Get(0).Width = 0F;
            this.ssFav_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtFavText);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 279);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(424, 95);
            this.panel2.TabIndex = 0;
            // 
            // txtFavText
            // 
            this.txtFavText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFavText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.txtFavText.Location = new System.Drawing.Point(0, 0);
            this.txtFavText.Multiline = true;
            this.txtFavText.Name = "txtFavText";
            this.txtFavText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtFavText.Size = new System.Drawing.Size(424, 95);
            this.txtFavText.TabIndex = 0;
            // 
            // frmMcrtJobViewFav
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(424, 408);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmMcrtJobViewFav";
            this.Text = "상용구";
            this.Load += new System.EventHandler(this.frmMcrtJobViewFav_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssFav)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssFav_Sheet1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private FarPoint.Win.Spread.FpSpread ssFav;
        private FarPoint.Win.Spread.SheetView ssFav_Sheet1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtFavText;
        private System.Windows.Forms.Button btnExit;
    }
}