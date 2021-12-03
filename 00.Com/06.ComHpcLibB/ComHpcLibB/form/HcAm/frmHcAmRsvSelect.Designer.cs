namespace ComHpcLibB
{
    partial class frmHcAmRsvSelect
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
            FarPoint.Win.Spread.FlatFocusIndicatorRenderer flatFocusIndicatorRenderer1 = new FarPoint.Win.Spread.FlatFocusIndicatorRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer1 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer2 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.Panel9999 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.ssRsv = new FarPoint.Win.Spread.FpSpread();
            this.ssRsv_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.Panel9999.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssRsv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssRsv_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // Panel9999
            // 
            this.Panel9999.BackColor = System.Drawing.Color.White;
            this.Panel9999.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel9999.Controls.Add(this.btnExit);
            this.Panel9999.Controls.Add(this.lblTitle);
            this.Panel9999.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel9999.Location = new System.Drawing.Point(0, 0);
            this.Panel9999.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Panel9999.Name = "Panel9999";
            this.Panel9999.Size = new System.Drawing.Size(825, 35);
            this.Panel9999.TabIndex = 31;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(734, 0);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(89, 33);
            this.btnExit.TabIndex = 20;
            this.btnExit.Text = "닫 기(&X)";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(3, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(112, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "예약일자 선택";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.ssRsv);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(825, 263);
            this.panel1.TabIndex = 32;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LemonChiffon;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(6, 227);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(811, 29);
            this.label1.TabIndex = 1;
            this.label1.Text = "수정 하실 예약일자를 더블클릭 하십시오!";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ssRsv
            // 
            this.ssRsv.AccessibleDescription = "ssRsv, Sheet1, Row 0, Column 0, ";
            this.ssRsv.FocusRenderer = flatFocusIndicatorRenderer1;
            this.ssRsv.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssRsv.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssRsv.HorizontalScrollBar.Renderer = flatScrollBarRenderer1;
            this.ssRsv.HorizontalScrollBar.TabIndex = 40;
            this.ssRsv.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssRsv.Location = new System.Drawing.Point(6, 6);
            this.ssRsv.Name = "ssRsv";
            this.ssRsv.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssRsv_Sheet1});
            this.ssRsv.Size = new System.Drawing.Size(812, 215);
            this.ssRsv.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.ssRsv.TabIndex = 0;
            this.ssRsv.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssRsv.VerticalScrollBar.Name = "";
            flatScrollBarRenderer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssRsv.VerticalScrollBar.Renderer = flatScrollBarRenderer2;
            this.ssRsv.VerticalScrollBar.TabIndex = 41;
            // 
            // ssRsv_Sheet1
            // 
            this.ssRsv_Sheet1.Reset();
            this.ssRsv_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssRsv_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssRsv_Sheet1.ColumnCount = 14;
            this.ssRsv_Sheet1.RowCount = 1;
            this.ssRsv_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssRsv_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssRsv_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.ssRsv_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssRsv_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssRsv_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssRsv_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.ssRsv_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssRsv_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "성 명";
            this.ssRsv_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "예약일자";
            this.ssRsv_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "rowid";
            this.ssRsv_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "UGI";
            this.ssRsv_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "GFS";
            this.ssRsv_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "GFS(종검)";
            this.ssRsv_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "유방";
            this.ssRsv_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "CT";
            this.ssRsv_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "분변";
            this.ssRsv_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "COLON";
            this.ssRsv_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "SONO";
            this.ssRsv_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "자궁경부";
            this.ssRsv_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "건진1차";
            this.ssRsv_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "CT 사후상담";
            this.ssRsv_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssRsv_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssRsv_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.ssRsv_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssRsv_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssRsv_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssRsv_Sheet1.Columns.Get(0).Label = "성 명";
            this.ssRsv_Sheet1.Columns.Get(0).Locked = true;
            this.ssRsv_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssRsv_Sheet1.Columns.Get(0).Width = 75F;
            this.ssRsv_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssRsv_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssRsv_Sheet1.Columns.Get(1).Label = "예약일자";
            this.ssRsv_Sheet1.Columns.Get(1).Locked = true;
            this.ssRsv_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssRsv_Sheet1.Columns.Get(1).Width = 182F;
            this.ssRsv_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssRsv_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssRsv_Sheet1.Columns.Get(2).Label = "rowid";
            this.ssRsv_Sheet1.Columns.Get(2).Locked = true;
            this.ssRsv_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssRsv_Sheet1.Columns.Get(2).Visible = false;
            this.ssRsv_Sheet1.Columns.Get(2).Width = 234F;
            this.ssRsv_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssRsv_Sheet1.Columns.Get(3).Label = "UGI";
            this.ssRsv_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssRsv_Sheet1.Columns.Get(3).Width = 27F;
            this.ssRsv_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssRsv_Sheet1.Columns.Get(4).Label = "GFS";
            this.ssRsv_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssRsv_Sheet1.Columns.Get(4).Width = 31F;
            this.ssRsv_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssRsv_Sheet1.Columns.Get(5).Label = "GFS(종검)";
            this.ssRsv_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssRsv_Sheet1.Columns.Get(5).Width = 77F;
            this.ssRsv_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssRsv_Sheet1.Columns.Get(6).Label = "유방";
            this.ssRsv_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssRsv_Sheet1.Columns.Get(6).Width = 31F;
            this.ssRsv_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssRsv_Sheet1.Columns.Get(7).Label = "CT";
            this.ssRsv_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssRsv_Sheet1.Columns.Get(7).Width = 24F;
            this.ssRsv_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssRsv_Sheet1.Columns.Get(8).Label = "분변";
            this.ssRsv_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssRsv_Sheet1.Columns.Get(8).Width = 31F;
            this.ssRsv_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssRsv_Sheet1.Columns.Get(9).Label = "COLON";
            this.ssRsv_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssRsv_Sheet1.Columns.Get(9).Width = 50F;
            this.ssRsv_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssRsv_Sheet1.Columns.Get(10).Label = "SONO";
            this.ssRsv_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssRsv_Sheet1.Columns.Get(10).Width = 42F;
            this.ssRsv_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssRsv_Sheet1.Columns.Get(11).Label = "자궁경부";
            this.ssRsv_Sheet1.Columns.Get(11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssRsv_Sheet1.Columns.Get(11).Width = 55F;
            this.ssRsv_Sheet1.Columns.Get(12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssRsv_Sheet1.Columns.Get(12).Label = "건진1차";
            this.ssRsv_Sheet1.Columns.Get(12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssRsv_Sheet1.Columns.Get(12).Width = 56F;
            this.ssRsv_Sheet1.Columns.Get(13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssRsv_Sheet1.Columns.Get(13).Label = "CT 사후상담";
            this.ssRsv_Sheet1.Columns.Get(13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssRsv_Sheet1.Columns.Get(13).Width = 76F;
            this.ssRsv_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssRsv_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssRsv_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.ssRsv_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssRsv_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssRsv_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssRsv_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.ssRsv_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssRsv_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
            this.ssRsv_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssRsv_Sheet1.RowHeader.Columns.Get(0).Width = 34F;
            this.ssRsv_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssRsv_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssRsv_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.ssRsv_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssRsv_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssRsv_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssRsv_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.ssRsv_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssRsv_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmHcAmRsvSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(825, 298);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Panel9999);
            this.Name = "frmHcAmRsvSelect";
            this.Text = "frmHcAmRsvSelect";
            this.Panel9999.ResumeLayout(false);
            this.Panel9999.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssRsv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssRsv_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Panel9999;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private FarPoint.Win.Spread.FpSpread ssRsv;
        private FarPoint.Win.Spread.SheetView ssRsv_Sheet1;
        private System.Windows.Forms.Label label1;
    }
}