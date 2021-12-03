namespace ComEmrBase
{
    partial class frmItemUserFunction
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.ssFunc = new FarPoint.Win.Spread.FpSpread();
            this.ssFunc_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.txtFuncRmk1 = new System.Windows.Forms.TextBox();
            this.panTitleSub0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssFunc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssFunc_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.btnExit);
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 0);
            this.panTitleSub0.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(589, 38);
            this.panTitleSub0.TabIndex = 35;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(506, 0);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(73, 33);
            this.btnExit.TabIndex = 180;
            this.btnExit.Text = "닫 기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(4, 6);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(69, 20);
            this.lblTitleSub0.TabIndex = 14;
            this.lblTitleSub0.Text = "용어조회";
            this.lblTitleSub0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // splitContainer3
            // 
            this.splitContainer3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 38);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.ssFunc);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.txtFuncRmk1);
            this.splitContainer3.Size = new System.Drawing.Size(589, 577);
            this.splitContainer3.SplitterDistance = 474;
            this.splitContainer3.TabIndex = 36;
            // 
            // ssFunc
            // 
            this.ssFunc.AccessibleDescription = "ssFunc, Sheet1, Row 0, Column 0, ";
            this.ssFunc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssFunc.Location = new System.Drawing.Point(0, 0);
            this.ssFunc.Name = "ssFunc";
            this.ssFunc.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssFunc_Sheet1});
            this.ssFunc.Size = new System.Drawing.Size(589, 474);
            this.ssFunc.TabIndex = 1;
            this.ssFunc.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssFunc_CellClick);
            this.ssFunc.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssFunc_CellDoubleClick);
            // 
            // ssFunc_Sheet1
            // 
            this.ssFunc_Sheet1.Reset();
            this.ssFunc_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssFunc_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssFunc_Sheet1.ColumnCount = 3;
            this.ssFunc_Sheet1.RowCount = 1;
            this.ssFunc_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssFunc_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssFunc_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssFunc_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssFunc_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "함  수";
            this.ssFunc_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "함수명";
            this.ssFunc_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "설  명";
            this.ssFunc_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssFunc_Sheet1.Columns.Get(0).CellType = textCellType4;
            this.ssFunc_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssFunc_Sheet1.Columns.Get(0).Label = "함  수";
            this.ssFunc_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssFunc_Sheet1.Columns.Get(0).Width = 270F;
            this.ssFunc_Sheet1.Columns.Get(1).CellType = textCellType5;
            this.ssFunc_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssFunc_Sheet1.Columns.Get(1).Label = "함수명";
            this.ssFunc_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssFunc_Sheet1.Columns.Get(1).Width = 263F;
            textCellType6.MaxLength = 4000;
            textCellType6.Multiline = true;
            textCellType6.WordWrap = true;
            this.ssFunc_Sheet1.Columns.Get(2).CellType = textCellType6;
            this.ssFunc_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssFunc_Sheet1.Columns.Get(2).Label = "설  명";
            this.ssFunc_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssFunc_Sheet1.Columns.Get(2).Visible = false;
            this.ssFunc_Sheet1.Columns.Get(2).Width = 257F;
            this.ssFunc_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssFunc_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssFunc_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssFunc_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssFunc_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssFunc_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // txtFuncRmk1
            // 
            this.txtFuncRmk1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFuncRmk1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFuncRmk1.Location = new System.Drawing.Point(0, 0);
            this.txtFuncRmk1.Multiline = true;
            this.txtFuncRmk1.Name = "txtFuncRmk1";
            this.txtFuncRmk1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtFuncRmk1.Size = new System.Drawing.Size(589, 99);
            this.txtFuncRmk1.TabIndex = 11;
            // 
            // frmItemUserFunction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 615);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer3);
            this.Controls.Add(this.panTitleSub0);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmItemUserFunction";
            this.Text = "frmItemUserFunction";
            this.Load += new System.EventHandler(this.frmItemUserFunction_Load);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssFunc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssFunc_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private FarPoint.Win.Spread.FpSpread ssFunc;
        private FarPoint.Win.Spread.SheetView ssFunc_Sheet1;
        private System.Windows.Forms.TextBox txtFuncRmk1;
    }
}