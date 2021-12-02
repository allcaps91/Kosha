namespace ComLibB
{
    partial class frmOrdersView
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
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType3 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.FlatFocusIndicatorRenderer flatFocusIndicatorRenderer3 = new FarPoint.Win.Spread.FlatFocusIndicatorRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer5 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer6 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.lblInfo = new System.Windows.Forms.Label();
            this.ssOrder = new FarPoint.Win.Spread.FpSpread();
            this.ssOrder_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.ssPrint = new FarPoint.Win.Spread.FpSpread();
            this.ssPrint_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssOrder_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssPrint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssPrint_Sheet1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlInfo
            // 
            this.pnlInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlInfo.Controls.Add(this.lblInfo);
            this.pnlInfo.Location = new System.Drawing.Point(1, 1);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(772, 71);
            this.pnlInfo.TabIndex = 0;
            // 
            // lblInfo
            // 
            this.lblInfo.Location = new System.Drawing.Point(5, 5);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(760, 59);
            this.lblInfo.TabIndex = 0;
            // 
            // ssOrder
            // 
            this.ssOrder.AccessibleDescription = "ssOrder, Sheet1, Row 0, Column 0, ";
            this.ssOrder.FocusRenderer = flatFocusIndicatorRenderer2;
            this.ssOrder.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssOrder.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssOrder.HorizontalScrollBar.Renderer = flatScrollBarRenderer3;
            this.ssOrder.HorizontalScrollBar.TabIndex = 110;
            this.ssOrder.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssOrder.Location = new System.Drawing.Point(0, 77);
            this.ssOrder.Name = "ssOrder";
            this.ssOrder.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssOrder_Sheet1});
            this.ssOrder.Size = new System.Drawing.Size(1037, 570);
            this.ssOrder.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.ssOrder.TabIndex = 5;
            this.ssOrder.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssOrder.VerticalScrollBar.Name = "";
            flatScrollBarRenderer4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssOrder.VerticalScrollBar.Renderer = flatScrollBarRenderer4;
            this.ssOrder.VerticalScrollBar.TabIndex = 111;
            this.ssOrder.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // ssOrder_Sheet1
            // 
            this.ssOrder_Sheet1.Reset();
            this.ssOrder_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssOrder_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssOrder_Sheet1.ColumnCount = 37;
            this.ssOrder_Sheet1.RowCount = 100;
            this.ssOrder_Sheet1.RowHeader.ColumnCount = 0;
            this.ssOrder_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssOrder_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssOrder_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.ssOrder_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssOrder_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssOrder_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssOrder_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.ssOrder_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 0).BackColor = System.Drawing.Color.White;
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "D";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "일자";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "구분";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "처방코드";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "처방명 [F5]";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "용법/검체";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "용량";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "수량";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "횟수";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "일수";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "의사";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "N";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "E";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "S";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "R";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 15).Value = "M";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 16).Value = "SUCODE";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 17).Value = "BUN";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 18).Value = "SLIPNO";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 19).Value = "QTY";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 20).Value = "DOSAGE";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 21).Value = "GBBOTH";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 22).Value = "GBINFO";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 23).Value = "REMARK";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 24).Value = "DISPRGB";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 25).Value = "GBBOTH";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 26).Value = "GBINFO";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 27).Value = "GBQTY";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 28).Value = "GBDOSAGE";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 29).Value = "NEXTCODE";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 30).Value = "GBIMIV";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 31).Value = "ORDERNO";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 32).Value = "ROWID";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 33).Value = "SORT";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 34).Value = "GBQTY";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 35).Value = "BCONTENTS";
            this.ssOrder_Sheet1.ColumnHeader.Cells.Get(0, 36).Value = "의사";
            this.ssOrder_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssOrder_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssOrder_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.ssOrder_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssOrder_Sheet1.ColumnHeader.Rows.Get(0).Height = 23F;
            this.ssOrder_Sheet1.Columns.Get(0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.ssOrder_Sheet1.Columns.Get(0).CellType = checkBoxCellType3;
            this.ssOrder_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssOrder_Sheet1.Columns.Get(0).Label = "D";
            this.ssOrder_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssOrder_Sheet1.Columns.Get(0).Width = 24F;
            this.ssOrder_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssOrder_Sheet1.Columns.Get(1).Label = "일자";
            this.ssOrder_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssOrder_Sheet1.Columns.Get(1).Width = 80F;
            this.ssOrder_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssOrder_Sheet1.Columns.Get(2).Label = "구분";
            this.ssOrder_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssOrder_Sheet1.Columns.Get(2).Width = 31F;
            this.ssOrder_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssOrder_Sheet1.Columns.Get(3).Label = "처방코드";
            this.ssOrder_Sheet1.Columns.Get(3).Width = 71F;
            this.ssOrder_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssOrder_Sheet1.Columns.Get(4).Label = "처방명 [F5]";
            this.ssOrder_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssOrder_Sheet1.Columns.Get(4).Width = 290F;
            this.ssOrder_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssOrder_Sheet1.Columns.Get(5).Label = "용법/검체";
            this.ssOrder_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssOrder_Sheet1.Columns.Get(5).Width = 140F;
            this.ssOrder_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssOrder_Sheet1.Columns.Get(6).Label = "용량";
            this.ssOrder_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssOrder_Sheet1.Columns.Get(6).Width = 59F;
            this.ssOrder_Sheet1.Columns.Get(7).Label = "수량";
            this.ssOrder_Sheet1.Columns.Get(7).Width = 32F;
            this.ssOrder_Sheet1.Columns.Get(8).Label = "횟수";
            this.ssOrder_Sheet1.Columns.Get(8).Width = 32F;
            this.ssOrder_Sheet1.Columns.Get(9).Label = "일수";
            this.ssOrder_Sheet1.Columns.Get(9).Width = 32F;
            this.ssOrder_Sheet1.Columns.Get(10).Label = "의사";
            this.ssOrder_Sheet1.Columns.Get(10).Width = 87F;
            this.ssOrder_Sheet1.Columns.Get(11).Label = "N";
            this.ssOrder_Sheet1.Columns.Get(11).Width = 24F;
            this.ssOrder_Sheet1.Columns.Get(12).Label = "E";
            this.ssOrder_Sheet1.Columns.Get(12).Width = 24F;
            this.ssOrder_Sheet1.Columns.Get(13).Label = "S";
            this.ssOrder_Sheet1.Columns.Get(13).Width = 24F;
            this.ssOrder_Sheet1.Columns.Get(14).Label = "R";
            this.ssOrder_Sheet1.Columns.Get(14).Width = 24F;
            this.ssOrder_Sheet1.Columns.Get(15).Label = "M";
            this.ssOrder_Sheet1.Columns.Get(15).Width = 24F;
            this.ssOrder_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssOrder_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssOrder_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.ssOrder_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssOrder_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssOrder_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssOrder_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.ssOrder_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssOrder_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            this.ssOrder_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssOrder_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssOrder_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssOrder_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.ssOrder_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssOrder_Sheet1.SelectionBackColor = System.Drawing.Color.White;
            this.ssOrder_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssOrder_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssOrder_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.ssOrder_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssOrder_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // ssPrint
            // 
            this.ssPrint.AccessibleDescription = "ssPrint, Sheet1, Row 0, Column 0, ";
            this.ssPrint.FocusRenderer = flatFocusIndicatorRenderer3;
            this.ssPrint.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssPrint.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssPrint.HorizontalScrollBar.Renderer = flatScrollBarRenderer5;
            this.ssPrint.HorizontalScrollBar.TabIndex = 142;
            this.ssPrint.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssPrint.Location = new System.Drawing.Point(44, 268);
            this.ssPrint.Name = "ssPrint";
            this.ssPrint.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssPrint_Sheet1});
            this.ssPrint.Size = new System.Drawing.Size(776, 570);
            this.ssPrint.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.ssPrint.TabIndex = 7;
            this.ssPrint.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssPrint.VerticalScrollBar.Name = "";
            flatScrollBarRenderer6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssPrint.VerticalScrollBar.Renderer = flatScrollBarRenderer6;
            this.ssPrint.VerticalScrollBar.TabIndex = 143;
            this.ssPrint.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssPrint.Visible = false;
            // 
            // ssPrint_Sheet1
            // 
            this.ssPrint_Sheet1.Reset();
            this.ssPrint_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssPrint_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssPrint_Sheet1.ColumnCount = 10;
            this.ssPrint_Sheet1.RowCount = 100;
            this.ssPrint_Sheet1.RowHeader.ColumnCount = 0;
            this.ssPrint_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPrint_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPrint_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.ssPrint_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPrint_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPrint_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPrint_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.ssPrint_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPrint_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "E";
            this.ssPrint_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "처방코드";
            this.ssPrint_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "처방명";
            this.ssPrint_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "일용량";
            this.ssPrint_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "일투량";
            this.ssPrint_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "용법/경로/검체";
            this.ssPrint_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "Mix";
            this.ssPrint_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "횟수";
            this.ssPrint_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "일수";
            this.ssPrint_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "T/P/M";
            this.ssPrint_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPrint_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPrint_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.ssPrint_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPrint_Sheet1.Columns.Get(0).Label = "E";
            this.ssPrint_Sheet1.Columns.Get(0).Width = 33F;
            this.ssPrint_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Columns.Get(1).Label = "처방코드";
            this.ssPrint_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Columns.Get(1).Width = 74F;
            this.ssPrint_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssPrint_Sheet1.Columns.Get(2).Label = "처방명";
            this.ssPrint_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Columns.Get(2).Width = 251F;
            this.ssPrint_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssPrint_Sheet1.Columns.Get(3).Label = "일용량";
            this.ssPrint_Sheet1.Columns.Get(3).Width = 48F;
            this.ssPrint_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Columns.Get(4).Label = "일투량";
            this.ssPrint_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Columns.Get(4).Width = 46F;
            this.ssPrint_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrint_Sheet1.Columns.Get(5).Label = "용법/경로/검체";
            this.ssPrint_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Columns.Get(5).Width = 156F;
            this.ssPrint_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssPrint_Sheet1.Columns.Get(6).Label = "Mix";
            this.ssPrint_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrint_Sheet1.Columns.Get(6).Width = 27F;
            this.ssPrint_Sheet1.Columns.Get(7).Label = "횟수";
            this.ssPrint_Sheet1.Columns.Get(7).Width = 34F;
            this.ssPrint_Sheet1.Columns.Get(8).Label = "일수";
            this.ssPrint_Sheet1.Columns.Get(8).Width = 34F;
            this.ssPrint_Sheet1.Columns.Get(9).Label = "T/P/M";
            this.ssPrint_Sheet1.Columns.Get(9).Width = 53F;
            this.ssPrint_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPrint_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPrint_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.ssPrint_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPrint_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPrint_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPrint_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.ssPrint_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPrint_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.None);
            this.ssPrint_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            this.ssPrint_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssPrint_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPrint_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPrint_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.ssPrint_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPrint_Sheet1.SelectionBackColor = System.Drawing.Color.White;
            this.ssPrint_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPrint_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPrint_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.ssPrint_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPrint_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.Location = new System.Drawing.Point(84, 20);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(84, 30);
            this.btnPrint.TabIndex = 29;
            this.btnPrint.Text = "오더지 출력";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Location = new System.Drawing.Point(6, 20);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 30;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(174, 20);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 28;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Location = new System.Drawing.Point(779, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(256, 71);
            this.panel1.TabIndex = 6;
            // 
            // frmOrdersView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1038, 649);
            this.Controls.Add(this.ssPrint);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ssOrder);
            this.Controls.Add(this.pnlInfo);
            this.Name = "frmOrdersView";
            this.Text = "입원환자 추가오더 조회";
            this.Load += new System.EventHandler(this.frmOrdersView_Load);
            this.pnlInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssOrder_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssPrint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssPrint_Sheet1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlInfo;
        private FarPoint.Win.Spread.FpSpread ssOrder;
        private FarPoint.Win.Spread.SheetView ssOrder_Sheet1;
        private System.Windows.Forms.Label lblInfo;
        private FarPoint.Win.Spread.FpSpread ssPrint;
        private FarPoint.Win.Spread.SheetView ssPrint_Sheet1;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel1;
    }
}