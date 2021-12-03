namespace ComSupLibB.SupLbEx
{
    partial class frmComSupLbExVIEW07
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
            FarPoint.Win.Spread.EnhancedFocusIndicatorRenderer enhancedFocusIndicatorRenderer1 = new FarPoint.Win.Spread.EnhancedFocusIndicatorRenderer();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer1 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer2 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer3 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer4 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("ko-KR", false);
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblDateTitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panMain2 = new System.Windows.Forms.Panel();
            this.circProgress = new DevComponents.DotNetBar.Controls.CircularProgress();
            this.ssResult = new FarPoint.Win.Spread.FpSpread();
            this.ssResult_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panMain = new System.Windows.Forms.Panel();
            this.ssMain = new FarPoint.Win.Spread.FpSpread();
            this.ssMain_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panTitle.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panMain2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssResult_Sheet1)).BeginInit();
            this.panMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMain_Sheet1)).BeginInit();
            this.panTitleSub0.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 100);
            this.panel2.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(0, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(296, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 16;
            this.label3.Text = "WS구분";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(164, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = " ~ ";
            // 
            // lblDateTitle
            // 
            this.lblDateTitle.Location = new System.Drawing.Point(0, 0);
            this.lblDateTitle.Name = "lblDateTitle";
            this.lblDateTitle.Size = new System.Drawing.Size(100, 23);
            this.lblDateTitle.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(6, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(167, 16);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "검체번호별 상세정보";
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(776, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 32);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.Controls.Add(this.btnPrint);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Padding = new System.Windows.Forms.Padding(1);
            this.panTitle.Size = new System.Drawing.Size(849, 34);
            this.panTitle.TabIndex = 13;
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrint.Location = new System.Drawing.Point(704, 1);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 32);
            this.btnPrint.TabIndex = 5;
            this.btnPrint.Text = "출력";
            this.btnPrint.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(8, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "검사정보";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 109);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(849, 28);
            this.panel4.TabIndex = 91;
            // 
            // panMain2
            // 
            this.panMain2.Controls.Add(this.circProgress);
            this.panMain2.Controls.Add(this.ssResult);
            this.panMain2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain2.Location = new System.Drawing.Point(0, 137);
            this.panMain2.Name = "panMain2";
            this.panMain2.Size = new System.Drawing.Size(849, 359);
            this.panMain2.TabIndex = 92;
            // 
            // circProgress
            // 
            this.circProgress.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.circProgress.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.circProgress.Location = new System.Drawing.Point(262, 58);
            this.circProgress.Name = "circProgress";
            this.circProgress.Size = new System.Drawing.Size(259, 256);
            this.circProgress.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeXP;
            this.circProgress.TabIndex = 8;
            this.circProgress.Visible = false;
            // 
            // ssResult
            // 
            this.ssResult.AccessibleDescription = "ssMain, Sheet1, Row 0, Column 0, ";
            this.ssResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssResult.FocusRenderer = enhancedFocusIndicatorRenderer1;
            this.ssResult.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssResult.HorizontalScrollBar.Name = "";
            this.ssResult.HorizontalScrollBar.Renderer = enhancedScrollBarRenderer1;
            this.ssResult.HorizontalScrollBar.TabIndex = 8;
            this.ssResult.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssResult.Location = new System.Drawing.Point(0, 0);
            this.ssResult.Name = "ssResult";
            this.ssResult.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssResult_Sheet1});
            this.ssResult.Size = new System.Drawing.Size(849, 359);
            this.ssResult.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;
            this.ssResult.TabIndex = 7;
            this.ssResult.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssResult.VerticalScrollBar.Name = "";
            this.ssResult.VerticalScrollBar.Renderer = enhancedScrollBarRenderer2;
            this.ssResult.VerticalScrollBar.TabIndex = 9;
            this.ssResult.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssResult.SetViewportLeftColumn(0, 0, 2);
            this.ssResult.SetActiveViewport(0, 0, -1);
            // 
            // ssResult_Sheet1
            // 
            this.ssResult_Sheet1.Reset();
            this.ssResult_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssResult_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssResult_Sheet1.ColumnCount = 12;
            this.ssResult_Sheet1.RowCount = 1;
            this.ssResult_Sheet1.Cells.Get(0, 8).Value = "170705-11:05";
            this.ssResult_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssResult_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssResult_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterEnhanced";
            this.ssResult_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssResult_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssResult_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssResult_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerEnhanced";
            this.ssResult_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssResult_Sheet1.ColumnHeader.AutoText = FarPoint.Win.Spread.HeaderAutoText.Numbers;
            this.ssResult_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "검사명";
            this.ssResult_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "결과";
            this.ssResult_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "R";
            this.ssResult_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "P";
            this.ssResult_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "D";
            this.ssResult_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "S";
            this.ssResult_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "Unit";
            this.ssResult_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "참고치";
            this.ssResult_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "결과일시";
            this.ssResult_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "검사자";
            this.ssResult_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "검사코드";
            this.ssResult_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "SUB코드";
            this.ssResult_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssResult_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssResult_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderEnhanced";
            this.ssResult_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssResult_Sheet1.Columns.Get(0).Label = "검사명";
            this.ssResult_Sheet1.Columns.Get(0).Width = 177F;
            this.ssResult_Sheet1.Columns.Get(1).Label = "결과";
            this.ssResult_Sheet1.Columns.Get(1).Width = 103F;
            this.ssResult_Sheet1.Columns.Get(2).Label = "R";
            this.ssResult_Sheet1.Columns.Get(2).Width = 15F;
            this.ssResult_Sheet1.Columns.Get(3).Label = "P";
            this.ssResult_Sheet1.Columns.Get(3).Width = 15F;
            this.ssResult_Sheet1.Columns.Get(4).Label = "D";
            this.ssResult_Sheet1.Columns.Get(4).Width = 15F;
            this.ssResult_Sheet1.Columns.Get(5).Label = "S";
            this.ssResult_Sheet1.Columns.Get(5).Width = 15F;
            this.ssResult_Sheet1.Columns.Get(6).Label = "Unit";
            this.ssResult_Sheet1.Columns.Get(6).Width = 59F;
            this.ssResult_Sheet1.Columns.Get(7).Label = "참고치";
            this.ssResult_Sheet1.Columns.Get(7).Width = 103F;
            this.ssResult_Sheet1.Columns.Get(8).Label = "결과일시";
            this.ssResult_Sheet1.Columns.Get(8).Width = 76F;
            this.ssResult_Sheet1.Columns.Get(9).Label = "검사자";
            this.ssResult_Sheet1.Columns.Get(9).Width = 71F;
            this.ssResult_Sheet1.Columns.Get(10).Label = "검사코드";
            this.ssResult_Sheet1.Columns.Get(10).Width = 79F;
            this.ssResult_Sheet1.Columns.Get(11).Label = "SUB코드";
            this.ssResult_Sheet1.Columns.Get(11).Width = 81F;
            this.ssResult_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssResult_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssResult_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.ssResult_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssResult_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssResult_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssResult_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarEnhanced";
            this.ssResult_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssResult_Sheet1.FilterBarHeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(236)))), ((int)(((byte)(247)))));
            this.ssResult_Sheet1.FilterBarHeaderStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ssResult_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssResult_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssResult_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderEnhanced";
            this.ssResult_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssResult_Sheet1.FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssResult_Sheet1.FrozenColumnCount = 2;
            this.ssResult_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ssResult_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
            this.ssResult_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssResult_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssResult_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssResult_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderEnhanced";
            this.ssResult_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssResult_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssResult_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssResult_Sheet1.SheetCornerStyle.Parent = "CornerEnhanced";
            this.ssResult_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssResult_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panMain
            // 
            this.panMain.Controls.Add(this.ssMain);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.panMain.Location = new System.Drawing.Point(0, 62);
            this.panMain.Name = "panMain";
            this.panMain.Padding = new System.Windows.Forms.Padding(1);
            this.panMain.Size = new System.Drawing.Size(849, 47);
            this.panMain.TabIndex = 90;
            // 
            // ssMain
            // 
            this.ssMain.AccessibleDescription = "ssMain, Sheet1, Row 0, Column 0, 1234567890";
            this.ssMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssMain.FocusRenderer = enhancedFocusIndicatorRenderer1;
            this.ssMain.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssMain.HorizontalScrollBar.Name = "";
            this.ssMain.HorizontalScrollBar.Renderer = enhancedScrollBarRenderer3;
            this.ssMain.HorizontalScrollBar.TabIndex = 3;
            this.ssMain.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssMain.Location = new System.Drawing.Point(1, 1);
            this.ssMain.Name = "ssMain";
            this.ssMain.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssMain_Sheet1});
            this.ssMain.Size = new System.Drawing.Size(847, 45);
            this.ssMain.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;
            this.ssMain.TabIndex = 0;
            this.ssMain.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssMain.VerticalScrollBar.Name = "";
            this.ssMain.VerticalScrollBar.Renderer = enhancedScrollBarRenderer4;
            this.ssMain.VerticalScrollBar.TabIndex = 4;
            this.ssMain.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            // 
            // ssMain_Sheet1
            // 
            this.ssMain_Sheet1.Reset();
            this.ssMain_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssMain_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssMain_Sheet1.ColumnCount = 10;
            this.ssMain_Sheet1.RowCount = 1;
            this.ssMain_Sheet1.RowHeader.ColumnCount = 0;
            this.ssMain_Sheet1.Cells.Get(0, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssMain_Sheet1.Cells.Get(0, 0).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.ssMain_Sheet1.Cells.Get(0, 0).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.ssMain_Sheet1.Cells.Get(0, 0).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.ssMain_Sheet1.Cells.Get(0, 0).ParseFormatString = "n";
            this.ssMain_Sheet1.Cells.Get(0, 0).Value = 1234567890;
            this.ssMain_Sheet1.Cells.Get(0, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMain_Sheet1.Cells.Get(0, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssMain_Sheet1.Cells.Get(0, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.ssMain_Sheet1.Cells.Get(0, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.ssMain_Sheet1.Cells.Get(0, 1).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.ssMain_Sheet1.Cells.Get(0, 1).ParseFormatString = "n";
            this.ssMain_Sheet1.Cells.Get(0, 1).Value = 1234567890;
            this.ssMain_Sheet1.Cells.Get(0, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMain_Sheet1.Cells.Get(0, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssMain_Sheet1.Cells.Get(0, 2).Value = "M/5";
            this.ssMain_Sheet1.Cells.Get(0, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMain_Sheet1.Cells.Get(0, 3).Value = "가나다라마바사";
            this.ssMain_Sheet1.Cells.Get(0, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssMain_Sheet1.Cells.Get(0, 4).Value = "56/123";
            this.ssMain_Sheet1.Cells.Get(0, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMain_Sheet1.Cells.Get(0, 5).Value = "소아청소년과";
            this.ssMain_Sheet1.Cells.Get(0, 6).Value = "가나다라마바사";
            this.ssMain_Sheet1.Cells.Get(0, 7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssMain_Sheet1.Cells.Get(0, 7).Value = "17-07-05-11:05";
            this.ssMain_Sheet1.Cells.Get(0, 7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMain_Sheet1.Cells.Get(0, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssMain_Sheet1.Cells.Get(0, 8).Value = "17-07-05-11:05";
            this.ssMain_Sheet1.Cells.Get(0, 8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMain_Sheet1.Cells.Get(0, 9).Value = "진행완료";
            this.ssMain_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMain_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMain_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterEnhanced";
            this.ssMain_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMain_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMain_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMain_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerEnhanced";
            this.ssMain_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMain_Sheet1.ColumnHeader.AutoText = FarPoint.Win.Spread.HeaderAutoText.Numbers;
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "검체번호";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "등록번호";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "성별/나이";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "성명";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "병동/병실";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "진료과";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "의사명";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "채혈일시";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "접수일시";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "진행상태";
            this.ssMain_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMain_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMain_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderEnhanced";
            this.ssMain_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMain_Sheet1.Columns.Get(0).Label = "검체번호";
            this.ssMain_Sheet1.Columns.Get(0).Width = 66F;
            this.ssMain_Sheet1.Columns.Get(1).Label = "등록번호";
            this.ssMain_Sheet1.Columns.Get(1).Width = 66F;
            this.ssMain_Sheet1.Columns.Get(2).Label = "성별/나이";
            this.ssMain_Sheet1.Columns.Get(2).Width = 61F;
            this.ssMain_Sheet1.Columns.Get(3).Label = "성명";
            this.ssMain_Sheet1.Columns.Get(3).Width = 90F;
            this.ssMain_Sheet1.Columns.Get(4).Label = "병동/병실";
            this.ssMain_Sheet1.Columns.Get(4).Width = 61F;
            this.ssMain_Sheet1.Columns.Get(5).Label = "진료과";
            this.ssMain_Sheet1.Columns.Get(5).Width = 86F;
            this.ssMain_Sheet1.Columns.Get(6).Label = "의사명";
            this.ssMain_Sheet1.Columns.Get(6).Width = 90F;
            this.ssMain_Sheet1.Columns.Get(7).Label = "채혈일시";
            this.ssMain_Sheet1.Columns.Get(7).Width = 88F;
            this.ssMain_Sheet1.Columns.Get(8).Label = "접수일시";
            this.ssMain_Sheet1.Columns.Get(8).Width = 100F;
            this.ssMain_Sheet1.Columns.Get(9).Label = "진행상태";
            this.ssMain_Sheet1.Columns.Get(9).Width = 55F;
            this.ssMain_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMain_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMain_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.ssMain_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMain_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMain_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMain_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarEnhanced";
            this.ssMain_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMain_Sheet1.FilterBarHeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(236)))), ((int)(((byte)(247)))));
            this.ssMain_Sheet1.FilterBarHeaderStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ssMain_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssMain_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMain_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderEnhanced";
            this.ssMain_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMain_Sheet1.FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssMain_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ssMain_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
            this.ssMain_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssMain_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMain_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMain_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderEnhanced";
            this.ssMain_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMain_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMain_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMain_Sheet1.SheetCornerStyle.Parent = "CornerEnhanced";
            this.ssMain_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMain_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 6);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(83, 12);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "환자종합정보";
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.panel3);
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(849, 28);
            this.panTitleSub0.TabIndex = 89;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(-88, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(933, 24);
            this.panel3.TabIndex = 1;
            // 
            // frmComSupLbExVIEW07
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(849, 496);
            this.Controls.Add(this.panMain2);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Name = "frmComSupLbExVIEW07";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmComSupLbExSpecInfo";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panMain2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssResult_Sheet1)).EndInit();
            this.panMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMain_Sheet1)).EndInit();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblDateTitle;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panMain2;
        private DevComponents.DotNetBar.Controls.CircularProgress circProgress;
        private FarPoint.Win.Spread.FpSpread ssResult;
        private FarPoint.Win.Spread.SheetView ssResult_Sheet1;
        private System.Windows.Forms.Panel panMain;
        private FarPoint.Win.Spread.FpSpread ssMain;
        private FarPoint.Win.Spread.SheetView ssMain_Sheet1;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Panel panel3;
    }
}