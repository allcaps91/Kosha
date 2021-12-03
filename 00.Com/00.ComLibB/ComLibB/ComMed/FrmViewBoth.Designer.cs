namespace ComLibB
{
    partial class FrmViewBoth
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
            FarPoint.Win.Spread.FlatFocusIndicatorRenderer flatFocusIndicatorRenderer2 = new FarPoint.Win.Spread.FlatFocusIndicatorRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer3 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer4 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.ssXrayPart = new FarPoint.Win.Spread.FpSpread();
            this.ssXrayPart_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panTitleSub = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.ssXrayPart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssXrayPart_Sheet1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panTitleSub.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ssXrayPart
            // 
            this.ssXrayPart.AccessibleDescription = "ssSecondDiv, Sheet1, Row 0, Column 0, ";
            this.ssXrayPart.BackColor = System.Drawing.Color.White;
            this.ssXrayPart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssXrayPart.FocusRenderer = flatFocusIndicatorRenderer2;
            this.ssXrayPart.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssXrayPart.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssXrayPart.HorizontalScrollBar.Renderer = flatScrollBarRenderer3;
            this.ssXrayPart.HorizontalScrollBar.TabIndex = 48;
            this.ssXrayPart.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssXrayPart.Location = new System.Drawing.Point(0, 67);
            this.ssXrayPart.Name = "ssXrayPart";
            this.ssXrayPart.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ssXrayPart.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssXrayPart_Sheet1});
            this.ssXrayPart.Size = new System.Drawing.Size(247, 433);
            this.ssXrayPart.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.ssXrayPart.TabIndex = 12;
            this.ssXrayPart.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssXrayPart.VerticalScrollBar.Name = "";
            flatScrollBarRenderer4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssXrayPart.VerticalScrollBar.Renderer = flatScrollBarRenderer4;
            this.ssXrayPart.VerticalScrollBar.TabIndex = 49;
            this.ssXrayPart.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssXrayPart.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssXrayPart_CellClick);
            this.ssXrayPart.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssXrayPart_CellDoubleClick);
            this.ssXrayPart.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ssXrayPart_KeyUp);
            // 
            // ssXrayPart_Sheet1
            // 
            this.ssXrayPart_Sheet1.Reset();
            this.ssXrayPart_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssXrayPart_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssXrayPart_Sheet1.ColumnCount = 2;
            this.ssXrayPart_Sheet1.RowCount = 11;
            this.ssXrayPart_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssXrayPart_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssXrayPart_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.ssXrayPart_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssXrayPart_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssXrayPart_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssXrayPart_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.ssXrayPart_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssXrayPart_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "부위";
            this.ssXrayPart_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "Sucode";
            this.ssXrayPart_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssXrayPart_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssXrayPart_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.ssXrayPart_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssXrayPart_Sheet1.ColumnHeader.Rows.Get(0).Height = 24F;
            this.ssXrayPart_Sheet1.Columns.Get(0).CellType = textCellType3;
            this.ssXrayPart_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssXrayPart_Sheet1.Columns.Get(0).Label = "부위";
            this.ssXrayPart_Sheet1.Columns.Get(0).Locked = true;
            this.ssXrayPart_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssXrayPart_Sheet1.Columns.Get(0).Width = 243F;
            this.ssXrayPart_Sheet1.Columns.Get(1).CellType = textCellType4;
            this.ssXrayPart_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssXrayPart_Sheet1.Columns.Get(1).Label = "Sucode";
            this.ssXrayPart_Sheet1.Columns.Get(1).Locked = true;
            this.ssXrayPart_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssXrayPart_Sheet1.Columns.Get(1).Width = 243F;
            this.ssXrayPart_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssXrayPart_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssXrayPart_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.ssXrayPart_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssXrayPart_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssXrayPart_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssXrayPart_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.ssXrayPart_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssXrayPart_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ssXrayPart_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.None);
            this.ssXrayPart_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            this.ssXrayPart_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssXrayPart_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssXrayPart_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssXrayPart_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.ssXrayPart_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssXrayPart_Sheet1.RowHeader.Visible = false;
            this.ssXrayPart_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssXrayPart_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssXrayPart_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.ssXrayPart_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssXrayPart_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.None);
            this.ssXrayPart_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(155, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 26);
            this.btnOK.TabIndex = 13;
            this.btnOK.Text = "확인(&O)";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(3, 5);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(149, 21);
            this.txtSearch.TabIndex = 14;
            this.txtSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSearch_KeyPress);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ssXrayPart);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.panTitleSub);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(247, 500);
            this.panel1.TabIndex = 15;
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
            this.panTitleSub.Size = new System.Drawing.Size(247, 35);
            this.panTitleSub.TabIndex = 16;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(5, 7);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(87, 17);
            this.lblTitleSub0.TabIndex = 1;
            this.lblTitleSub0.Text = "OrderCode : ";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtSearch);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 35);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(247, 32);
            this.panel2.TabIndex = 17;
            // 
            // FrmViewBoth
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(247, 500);
            this.Controls.Add(this.panel1);
            this.Name = "FrmViewBoth";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "항목선택";
            this.Load += new System.EventHandler(this.FrmViewBoth_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ssXrayPart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssXrayPart_Sheet1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panTitleSub.ResumeLayout(false);
            this.panTitleSub.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private FarPoint.Win.Spread.FpSpread ssXrayPart;
        private FarPoint.Win.Spread.SheetView ssXrayPart_Sheet1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panTitleSub;
        private System.Windows.Forms.Label lblTitleSub0;
    }
}