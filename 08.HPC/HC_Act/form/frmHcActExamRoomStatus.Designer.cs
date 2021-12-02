namespace HC_Act
{
    partial class frmHcActExamRoomStatus
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
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("ko-KR", false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.ButtonCellType buttonCellType1 = new FarPoint.Win.Spread.CellType.ButtonCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer3 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer4 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.ButtonCellType buttonCellType2 = new FarPoint.Win.Spread.CellType.ButtonCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblFormTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.ssChk_Hic = new FarPoint.Win.Spread.FpSpread();
            this.sheetView1 = new FarPoint.Win.Spread.SheetView();
            this.ssChk = new FarPoint.Win.Spread.FpSpread();
            this.ssChk_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnlHicGubun = new System.Windows.Forms.Panel();
            this.rdoJepsuGubun3 = new System.Windows.Forms.RadioButton();
            this.rdoJepsuGubun2 = new System.Windows.Forms.RadioButton();
            this.rdoJepsuGubun1 = new System.Windows.Forms.RadioButton();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rdoGubun2 = new System.Windows.Forms.RadioButton();
            this.rdoGubun1 = new System.Windows.Forms.RadioButton();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssChk_Hic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sheetView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssChk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssChk_Sheet1)).BeginInit();
            this.panel2.SuspendLayout();
            this.pnlHicGubun.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblFormTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(451, 37);
            this.panTitle.TabIndex = 44;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(372, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 33);
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "닫기(&X)";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblFormTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblFormTitle.Location = new System.Drawing.Point(0, 0);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(225, 33);
            this.lblFormTitle.TabIndex = 6;
            this.lblFormTitle.Text = "일반검진/종합검진 대기현황";
            this.lblFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 37);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(451, 829);
            this.panel1.TabIndex = 45;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.ssChk_Hic);
            this.panel4.Controls.Add(this.ssChk);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 45);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(451, 784);
            this.panel4.TabIndex = 1;
            // 
            // ssChk_Hic
            // 
            this.ssChk_Hic.AccessibleDescription = "ssChk_Hic, Sheet1, Row 0, Column 0, 혈액검사";
            this.ssChk_Hic.FocusRenderer = flatFocusIndicatorRenderer1;
            this.ssChk_Hic.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssChk_Hic.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssChk_Hic.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssChk_Hic.HorizontalScrollBar.Renderer = flatScrollBarRenderer1;
            this.ssChk_Hic.HorizontalScrollBar.TabIndex = 278;
            this.ssChk_Hic.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssChk_Hic.Location = new System.Drawing.Point(2, 1);
            this.ssChk_Hic.Name = "ssChk_Hic";
            this.ssChk_Hic.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.sheetView1});
            this.ssChk_Hic.Size = new System.Drawing.Size(446, 783);
            this.ssChk_Hic.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.ssChk_Hic.TabIndex = 13;
            this.ssChk_Hic.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssChk_Hic.VerticalScrollBar.Name = "";
            flatScrollBarRenderer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssChk_Hic.VerticalScrollBar.Renderer = flatScrollBarRenderer2;
            this.ssChk_Hic.VerticalScrollBar.TabIndex = 279;
            // 
            // sheetView1
            // 
            this.sheetView1.Reset();
            this.sheetView1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.sheetView1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.sheetView1.ColumnCount = 9;
            this.sheetView1.RowCount = 30;
            this.sheetView1.Cells.Get(0, 0).Value = "혈액검사";
            this.sheetView1.Cells.Get(0, 1).Value = "완료";
            this.sheetView1.Cells.Get(0, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(0, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(0, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.sheetView1.Cells.Get(0, 2).ParseFormatString = "n";
            this.sheetView1.Cells.Get(0, 2).Value = 0;
            this.sheetView1.Cells.Get(1, 0).Value = "소변검사";
            this.sheetView1.Cells.Get(1, 1).Value = "완료";
            this.sheetView1.Cells.Get(1, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(1, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(1, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.sheetView1.Cells.Get(1, 2).ParseFormatString = "n";
            this.sheetView1.Cells.Get(1, 2).Value = 1;
            this.sheetView1.Cells.Get(2, 0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.sheetView1.Cells.Get(2, 0).Value = "구강검사";
            this.sheetView1.Cells.Get(2, 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.sheetView1.Cells.Get(2, 1).Value = "완료";
            this.sheetView1.Cells.Get(2, 2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.sheetView1.Cells.Get(2, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(2, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(2, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.sheetView1.Cells.Get(2, 2).ParseFormatString = "n";
            this.sheetView1.Cells.Get(2, 2).Value = 0;
            this.sheetView1.Cells.Get(2, 3).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.sheetView1.Cells.Get(3, 0).Value = "혈압";
            this.sheetView1.Cells.Get(3, 1).Value = "완료";
            this.sheetView1.Cells.Get(3, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(3, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(3, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.sheetView1.Cells.Get(3, 2).ParseFormatString = "n";
            this.sheetView1.Cells.Get(3, 2).Value = 0;
            this.sheetView1.Cells.Get(4, 0).Value = "체성분";
            this.sheetView1.Cells.Get(4, 1).Value = "완료";
            this.sheetView1.Cells.Get(4, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(4, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(4, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.sheetView1.Cells.Get(4, 2).ParseFormatString = "n";
            this.sheetView1.Cells.Get(4, 2).Value = 0;
            this.sheetView1.Cells.Get(5, 0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.sheetView1.Cells.Get(5, 0).Value = "심전도검사";
            this.sheetView1.Cells.Get(5, 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.sheetView1.Cells.Get(5, 1).Value = "완료";
            this.sheetView1.Cells.Get(5, 2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.sheetView1.Cells.Get(5, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(5, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(5, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.sheetView1.Cells.Get(5, 2).ParseFormatString = "n";
            this.sheetView1.Cells.Get(5, 2).Value = 0;
            this.sheetView1.Cells.Get(5, 3).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.sheetView1.Cells.Get(6, 0).Value = "폐기능";
            this.sheetView1.Cells.Get(6, 1).Value = "완료";
            this.sheetView1.Cells.Get(6, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(6, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(6, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.sheetView1.Cells.Get(6, 2).ParseFormatString = "n";
            this.sheetView1.Cells.Get(6, 2).Value = 0;
            this.sheetView1.Cells.Get(7, 0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.sheetView1.Cells.Get(7, 0).Value = "시력";
            this.sheetView1.Cells.Get(7, 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.sheetView1.Cells.Get(7, 1).Value = "완료";
            this.sheetView1.Cells.Get(7, 2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.sheetView1.Cells.Get(7, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(7, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(7, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.sheetView1.Cells.Get(7, 2).ParseFormatString = "n";
            this.sheetView1.Cells.Get(7, 2).Value = 2;
            this.sheetView1.Cells.Get(7, 3).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.sheetView1.Cells.Get(8, 0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.sheetView1.Cells.Get(8, 0).Value = "안압 검사";
            this.sheetView1.Cells.Get(8, 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.sheetView1.Cells.Get(8, 1).Value = "완료";
            this.sheetView1.Cells.Get(8, 2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.sheetView1.Cells.Get(8, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(8, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(8, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.sheetView1.Cells.Get(8, 2).ParseFormatString = "n";
            this.sheetView1.Cells.Get(8, 2).Value = 0;
            this.sheetView1.Cells.Get(8, 3).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.sheetView1.Cells.Get(9, 0).Value = "청력검사";
            this.sheetView1.Cells.Get(9, 1).Value = "완료";
            this.sheetView1.Cells.Get(9, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(9, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(9, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.sheetView1.Cells.Get(9, 2).ParseFormatString = "n";
            this.sheetView1.Cells.Get(9, 2).Value = 1;
            this.sheetView1.Cells.Get(10, 0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.sheetView1.Cells.Get(10, 0).Value = "흉부촬영";
            this.sheetView1.Cells.Get(10, 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.sheetView1.Cells.Get(10, 1).Value = "완료";
            this.sheetView1.Cells.Get(10, 2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.sheetView1.Cells.Get(10, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(10, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(10, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.sheetView1.Cells.Get(10, 2).ParseFormatString = "n";
            this.sheetView1.Cells.Get(10, 2).Value = 2;
            this.sheetView1.Cells.Get(10, 3).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.sheetView1.Cells.Get(11, 0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.sheetView1.Cells.Get(11, 0).Value = "상복부초음파";
            this.sheetView1.Cells.Get(11, 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.sheetView1.Cells.Get(11, 1).Value = "완료";
            this.sheetView1.Cells.Get(11, 2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.sheetView1.Cells.Get(11, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(11, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(11, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.sheetView1.Cells.Get(11, 2).ParseFormatString = "n";
            this.sheetView1.Cells.Get(11, 2).Value = 0;
            this.sheetView1.Cells.Get(11, 3).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.sheetView1.Cells.Get(12, 0).Value = "수면-위내시경";
            this.sheetView1.Cells.Get(12, 1).Value = "완료";
            this.sheetView1.Cells.Get(12, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(12, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(12, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.sheetView1.Cells.Get(12, 2).ParseFormatString = "n";
            this.sheetView1.Cells.Get(12, 2).Value = 1;
            this.sheetView1.Cells.Get(13, 0).Value = "분변검사";
            this.sheetView1.Cells.Get(13, 1).Value = "완료";
            this.sheetView1.Cells.Get(13, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(13, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(13, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.sheetView1.Cells.Get(13, 2).ParseFormatString = "n";
            this.sheetView1.Cells.Get(13, 2).Value = 7;
            this.sheetView1.Cells.Get(14, 0).Value = "저선량 흉부 CT";
            this.sheetView1.Cells.Get(14, 1).Value = "완료";
            this.sheetView1.Cells.Get(14, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(14, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(14, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.sheetView1.Cells.Get(14, 2).ParseFormatString = "n";
            this.sheetView1.Cells.Get(14, 2).Value = 0;
            this.sheetView1.Cells.Get(15, 0).Value = "폐기능";
            this.sheetView1.Cells.Get(15, 1).Value = "완료";
            this.sheetView1.Cells.Get(15, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(15, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.sheetView1.Cells.Get(15, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.sheetView1.Cells.Get(15, 2).ParseFormatString = "n";
            this.sheetView1.Cells.Get(15, 2).Value = 0;
            this.sheetView1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.sheetView1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.sheetView1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.sheetView1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.sheetView1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.sheetView1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.sheetView1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.sheetView1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.sheetView1.ColumnHeader.Cells.Get(0, 0).Value = "구 분";
            this.sheetView1.ColumnHeader.Cells.Get(0, 1).Value = "상태";
            this.sheetView1.ColumnHeader.Cells.Get(0, 2).Value = "대기";
            this.sheetView1.ColumnHeader.Cells.Get(0, 3).Value = "검사실";
            this.sheetView1.ColumnHeader.Cells.Get(0, 4).Value = "구분";
            this.sheetView1.ColumnHeader.Cells.Get(0, 5).Value = "대기명단";
            this.sheetView1.ColumnHeader.Cells.Get(0, 6).Value = "검사실";
            this.sheetView1.ColumnHeader.Cells.Get(0, 7).Value = "Part";
            this.sheetView1.ColumnHeader.Cells.Get(0, 8).Value = "접수번호";
            this.sheetView1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(244)))), ((int)(((byte)(206)))));
            this.sheetView1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.sheetView1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.sheetView1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.sheetView1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.sheetView1.ColumnHeader.Rows.Get(0).Height = 42F;
            this.sheetView1.Columns.Get(0).CellType = textCellType1;
            this.sheetView1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sheetView1.Columns.Get(0).Label = "구 분";
            this.sheetView1.Columns.Get(0).Locked = true;
            this.sheetView1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sheetView1.Columns.Get(0).Width = 315F;
            this.sheetView1.Columns.Get(1).CellType = textCellType2;
            this.sheetView1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sheetView1.Columns.Get(1).Label = "상태";
            this.sheetView1.Columns.Get(1).Locked = true;
            this.sheetView1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sheetView1.Columns.Get(1).Width = 51F;
            this.sheetView1.Columns.Get(2).CellType = textCellType3;
            this.sheetView1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sheetView1.Columns.Get(2).Label = "대기";
            this.sheetView1.Columns.Get(2).Locked = true;
            this.sheetView1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sheetView1.Columns.Get(3).CellType = textCellType4;
            this.sheetView1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sheetView1.Columns.Get(3).Label = "검사실";
            this.sheetView1.Columns.Get(3).Locked = true;
            this.sheetView1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sheetView1.Columns.Get(3).Visible = false;
            this.sheetView1.Columns.Get(3).Width = 62F;
            buttonCellType1.ButtonColor2 = System.Drawing.SystemColors.ButtonFace;
            buttonCellType1.Text = "보기";
            this.sheetView1.Columns.Get(4).CellType = buttonCellType1;
            this.sheetView1.Columns.Get(4).Font = new System.Drawing.Font("맑은 고딕", 11.25F);
            this.sheetView1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sheetView1.Columns.Get(4).Label = "구분";
            this.sheetView1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sheetView1.Columns.Get(4).Visible = false;
            this.sheetView1.Columns.Get(4).Width = 45F;
            this.sheetView1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sheetView1.Columns.Get(5).Label = "대기명단";
            this.sheetView1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sheetView1.Columns.Get(5).Visible = false;
            this.sheetView1.Columns.Get(5).Width = 89F;
            this.sheetView1.Columns.Get(6).CellType = textCellType5;
            this.sheetView1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sheetView1.Columns.Get(6).Label = "검사실";
            this.sheetView1.Columns.Get(6).Locked = true;
            this.sheetView1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sheetView1.Columns.Get(6).Visible = false;
            this.sheetView1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sheetView1.Columns.Get(7).Label = "Part";
            this.sheetView1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sheetView1.Columns.Get(7).Visible = false;
            this.sheetView1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sheetView1.Columns.Get(8).Label = "접수번호";
            this.sheetView1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sheetView1.Columns.Get(8).Visible = false;
            this.sheetView1.Columns.Get(8).Width = 80F;
            this.sheetView1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.sheetView1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.sheetView1.DefaultStyle.Parent = "DataAreaDefault";
            this.sheetView1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.sheetView1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.sheetView1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.sheetView1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.sheetView1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.sheetView1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.sheetView1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.sheetView1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.sheetView1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.sheetView1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.sheetView1.RowHeader.Columns.Default.Resizable = false;
            this.sheetView1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.sheetView1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.sheetView1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.sheetView1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.sheetView1.RowHeader.Visible = false;
            this.sheetView1.Rows.Get(0).Height = 32F;
            this.sheetView1.Rows.Get(1).Height = 32F;
            this.sheetView1.Rows.Get(2).Height = 32F;
            this.sheetView1.Rows.Get(3).Height = 32F;
            this.sheetView1.Rows.Get(4).Height = 32F;
            this.sheetView1.Rows.Get(5).Height = 32F;
            this.sheetView1.Rows.Get(6).Height = 32F;
            this.sheetView1.Rows.Get(7).Height = 32F;
            this.sheetView1.Rows.Get(8).Height = 32F;
            this.sheetView1.Rows.Get(9).Height = 32F;
            this.sheetView1.Rows.Get(10).Height = 32F;
            this.sheetView1.Rows.Get(11).Height = 32F;
            this.sheetView1.Rows.Get(12).Height = 32F;
            this.sheetView1.Rows.Get(13).Height = 32F;
            this.sheetView1.Rows.Get(14).Height = 32F;
            this.sheetView1.Rows.Get(15).Height = 32F;
            this.sheetView1.Rows.Get(16).Height = 32F;
            this.sheetView1.Rows.Get(17).Height = 32F;
            this.sheetView1.Rows.Get(18).Height = 32F;
            this.sheetView1.Rows.Get(19).Height = 32F;
            this.sheetView1.Rows.Get(20).Height = 32F;
            this.sheetView1.Rows.Get(21).Height = 32F;
            this.sheetView1.Rows.Get(22).Height = 32F;
            this.sheetView1.Rows.Get(23).Height = 32F;
            this.sheetView1.Rows.Get(24).Height = 32F;
            this.sheetView1.Rows.Get(25).Height = 32F;
            this.sheetView1.Rows.Get(26).Height = 32F;
            this.sheetView1.Rows.Get(27).Height = 32F;
            this.sheetView1.Rows.Get(28).Height = 32F;
            this.sheetView1.Rows.Get(29).Height = 32F;
            this.sheetView1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.sheetView1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.sheetView1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.sheetView1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.sheetView1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.sheetView1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // ssChk
            // 
            this.ssChk.AccessibleDescription = "ssChk, Sheet1, Row 0, Column 0, 혈액검사";
            this.ssChk.FocusRenderer = flatFocusIndicatorRenderer1;
            this.ssChk.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssChk.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssChk.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssChk.HorizontalScrollBar.Renderer = flatScrollBarRenderer3;
            this.ssChk.HorizontalScrollBar.TabIndex = 226;
            this.ssChk.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssChk.Location = new System.Drawing.Point(3, 2);
            this.ssChk.Name = "ssChk";
            this.ssChk.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssChk_Sheet1});
            this.ssChk.Size = new System.Drawing.Size(447, 782);
            this.ssChk.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.ssChk.TabIndex = 9;
            this.ssChk.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssChk.VerticalScrollBar.Name = "";
            flatScrollBarRenderer4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssChk.VerticalScrollBar.Renderer = flatScrollBarRenderer4;
            this.ssChk.VerticalScrollBar.TabIndex = 227;
            // 
            // ssChk_Sheet1
            // 
            this.ssChk_Sheet1.Reset();
            this.ssChk_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssChk_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssChk_Sheet1.ColumnCount = 9;
            this.ssChk_Sheet1.RowCount = 30;
            this.ssChk_Sheet1.Cells.Get(0, 0).Value = "혈액검사";
            this.ssChk_Sheet1.Cells.Get(0, 1).Value = "완료";
            this.ssChk_Sheet1.Cells.Get(0, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(0, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(0, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.ssChk_Sheet1.Cells.Get(0, 2).ParseFormatString = "n";
            this.ssChk_Sheet1.Cells.Get(0, 2).Value = 0;
            this.ssChk_Sheet1.Cells.Get(1, 0).Value = "소변검사";
            this.ssChk_Sheet1.Cells.Get(1, 1).Value = "완료";
            this.ssChk_Sheet1.Cells.Get(1, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(1, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(1, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.ssChk_Sheet1.Cells.Get(1, 2).ParseFormatString = "n";
            this.ssChk_Sheet1.Cells.Get(1, 2).Value = 1;
            this.ssChk_Sheet1.Cells.Get(2, 0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.ssChk_Sheet1.Cells.Get(2, 0).Value = "구강검사";
            this.ssChk_Sheet1.Cells.Get(2, 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.ssChk_Sheet1.Cells.Get(2, 1).Value = "완료";
            this.ssChk_Sheet1.Cells.Get(2, 2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.ssChk_Sheet1.Cells.Get(2, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(2, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(2, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.ssChk_Sheet1.Cells.Get(2, 2).ParseFormatString = "n";
            this.ssChk_Sheet1.Cells.Get(2, 2).Value = 0;
            this.ssChk_Sheet1.Cells.Get(2, 3).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.ssChk_Sheet1.Cells.Get(3, 0).Value = "혈압";
            this.ssChk_Sheet1.Cells.Get(3, 1).Value = "완료";
            this.ssChk_Sheet1.Cells.Get(3, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(3, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(3, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.ssChk_Sheet1.Cells.Get(3, 2).ParseFormatString = "n";
            this.ssChk_Sheet1.Cells.Get(3, 2).Value = 0;
            this.ssChk_Sheet1.Cells.Get(4, 0).Value = "체성분";
            this.ssChk_Sheet1.Cells.Get(4, 1).Value = "완료";
            this.ssChk_Sheet1.Cells.Get(4, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(4, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(4, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.ssChk_Sheet1.Cells.Get(4, 2).ParseFormatString = "n";
            this.ssChk_Sheet1.Cells.Get(4, 2).Value = 0;
            this.ssChk_Sheet1.Cells.Get(5, 0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.ssChk_Sheet1.Cells.Get(5, 0).Value = "심전도검사";
            this.ssChk_Sheet1.Cells.Get(5, 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.ssChk_Sheet1.Cells.Get(5, 1).Value = "완료";
            this.ssChk_Sheet1.Cells.Get(5, 2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.ssChk_Sheet1.Cells.Get(5, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(5, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(5, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.ssChk_Sheet1.Cells.Get(5, 2).ParseFormatString = "n";
            this.ssChk_Sheet1.Cells.Get(5, 2).Value = 0;
            this.ssChk_Sheet1.Cells.Get(5, 3).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.ssChk_Sheet1.Cells.Get(6, 0).Value = "폐기능";
            this.ssChk_Sheet1.Cells.Get(6, 1).Value = "완료";
            this.ssChk_Sheet1.Cells.Get(6, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(6, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(6, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.ssChk_Sheet1.Cells.Get(6, 2).ParseFormatString = "n";
            this.ssChk_Sheet1.Cells.Get(6, 2).Value = 0;
            this.ssChk_Sheet1.Cells.Get(7, 0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.ssChk_Sheet1.Cells.Get(7, 0).Value = "시력";
            this.ssChk_Sheet1.Cells.Get(7, 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.ssChk_Sheet1.Cells.Get(7, 1).Value = "완료";
            this.ssChk_Sheet1.Cells.Get(7, 2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.ssChk_Sheet1.Cells.Get(7, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(7, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(7, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.ssChk_Sheet1.Cells.Get(7, 2).ParseFormatString = "n";
            this.ssChk_Sheet1.Cells.Get(7, 2).Value = 2;
            this.ssChk_Sheet1.Cells.Get(7, 3).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.ssChk_Sheet1.Cells.Get(8, 0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.ssChk_Sheet1.Cells.Get(8, 0).Value = "안압 검사";
            this.ssChk_Sheet1.Cells.Get(8, 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.ssChk_Sheet1.Cells.Get(8, 1).Value = "완료";
            this.ssChk_Sheet1.Cells.Get(8, 2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.ssChk_Sheet1.Cells.Get(8, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(8, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(8, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.ssChk_Sheet1.Cells.Get(8, 2).ParseFormatString = "n";
            this.ssChk_Sheet1.Cells.Get(8, 2).Value = 0;
            this.ssChk_Sheet1.Cells.Get(8, 3).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.ssChk_Sheet1.Cells.Get(9, 0).Value = "청력검사";
            this.ssChk_Sheet1.Cells.Get(9, 1).Value = "완료";
            this.ssChk_Sheet1.Cells.Get(9, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(9, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(9, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.ssChk_Sheet1.Cells.Get(9, 2).ParseFormatString = "n";
            this.ssChk_Sheet1.Cells.Get(9, 2).Value = 1;
            this.ssChk_Sheet1.Cells.Get(10, 0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.ssChk_Sheet1.Cells.Get(10, 0).Value = "흉부촬영";
            this.ssChk_Sheet1.Cells.Get(10, 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.ssChk_Sheet1.Cells.Get(10, 1).Value = "완료";
            this.ssChk_Sheet1.Cells.Get(10, 2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.ssChk_Sheet1.Cells.Get(10, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(10, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(10, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.ssChk_Sheet1.Cells.Get(10, 2).ParseFormatString = "n";
            this.ssChk_Sheet1.Cells.Get(10, 2).Value = 2;
            this.ssChk_Sheet1.Cells.Get(10, 3).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.ssChk_Sheet1.Cells.Get(11, 0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.ssChk_Sheet1.Cells.Get(11, 0).Value = "상복부초음파";
            this.ssChk_Sheet1.Cells.Get(11, 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.ssChk_Sheet1.Cells.Get(11, 1).Value = "완료";
            this.ssChk_Sheet1.Cells.Get(11, 2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.ssChk_Sheet1.Cells.Get(11, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(11, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(11, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.ssChk_Sheet1.Cells.Get(11, 2).ParseFormatString = "n";
            this.ssChk_Sheet1.Cells.Get(11, 2).Value = 0;
            this.ssChk_Sheet1.Cells.Get(11, 3).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.ssChk_Sheet1.Cells.Get(12, 0).Value = "수면-위내시경";
            this.ssChk_Sheet1.Cells.Get(12, 1).Value = "완료";
            this.ssChk_Sheet1.Cells.Get(12, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(12, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(12, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.ssChk_Sheet1.Cells.Get(12, 2).ParseFormatString = "n";
            this.ssChk_Sheet1.Cells.Get(12, 2).Value = 1;
            this.ssChk_Sheet1.Cells.Get(13, 0).Value = "분변검사";
            this.ssChk_Sheet1.Cells.Get(13, 1).Value = "완료";
            this.ssChk_Sheet1.Cells.Get(13, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(13, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(13, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.ssChk_Sheet1.Cells.Get(13, 2).ParseFormatString = "n";
            this.ssChk_Sheet1.Cells.Get(13, 2).Value = 7;
            this.ssChk_Sheet1.Cells.Get(14, 0).Value = "저선량 흉부 CT";
            this.ssChk_Sheet1.Cells.Get(14, 1).Value = "완료";
            this.ssChk_Sheet1.Cells.Get(14, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(14, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(14, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.ssChk_Sheet1.Cells.Get(14, 2).ParseFormatString = "n";
            this.ssChk_Sheet1.Cells.Get(14, 2).Value = 0;
            this.ssChk_Sheet1.Cells.Get(15, 0).Value = "폐기능";
            this.ssChk_Sheet1.Cells.Get(15, 1).Value = "완료";
            this.ssChk_Sheet1.Cells.Get(15, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(15, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.ssChk_Sheet1.Cells.Get(15, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.ssChk_Sheet1.Cells.Get(15, 2).ParseFormatString = "n";
            this.ssChk_Sheet1.Cells.Get(15, 2).Value = 0;
            this.ssChk_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssChk_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssChk_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.ssChk_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssChk_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssChk_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssChk_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.ssChk_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssChk_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "구 분";
            this.ssChk_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "상태";
            this.ssChk_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "접수인원";
            this.ssChk_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "검사실";
            this.ssChk_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "구분";
            this.ssChk_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "대기명단";
            this.ssChk_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "검사실";
            this.ssChk_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "Part";
            this.ssChk_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "접수번호";
            this.ssChk_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(244)))), ((int)(((byte)(206)))));
            this.ssChk_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssChk_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssChk_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.ssChk_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssChk_Sheet1.ColumnHeader.Rows.Get(0).Height = 42F;
            this.ssChk_Sheet1.Columns.Get(0).CellType = textCellType6;
            this.ssChk_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssChk_Sheet1.Columns.Get(0).Label = "구 분";
            this.ssChk_Sheet1.Columns.Get(0).Locked = true;
            this.ssChk_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssChk_Sheet1.Columns.Get(0).Width = 141F;
            this.ssChk_Sheet1.Columns.Get(1).CellType = textCellType7;
            this.ssChk_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssChk_Sheet1.Columns.Get(1).Label = "상태";
            this.ssChk_Sheet1.Columns.Get(1).Locked = true;
            this.ssChk_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssChk_Sheet1.Columns.Get(1).Width = 43F;
            this.ssChk_Sheet1.Columns.Get(2).CellType = textCellType8;
            this.ssChk_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssChk_Sheet1.Columns.Get(2).Label = "접수인원";
            this.ssChk_Sheet1.Columns.Get(2).Locked = true;
            this.ssChk_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssChk_Sheet1.Columns.Get(2).Width = 46F;
            this.ssChk_Sheet1.Columns.Get(3).CellType = textCellType9;
            this.ssChk_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssChk_Sheet1.Columns.Get(3).Label = "검사실";
            this.ssChk_Sheet1.Columns.Get(3).Locked = true;
            this.ssChk_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssChk_Sheet1.Columns.Get(3).Width = 62F;
            buttonCellType2.ButtonColor2 = System.Drawing.SystemColors.ButtonFace;
            buttonCellType2.Text = "보기";
            this.ssChk_Sheet1.Columns.Get(4).CellType = buttonCellType2;
            this.ssChk_Sheet1.Columns.Get(4).Font = new System.Drawing.Font("맑은 고딕", 11.25F);
            this.ssChk_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssChk_Sheet1.Columns.Get(4).Label = "구분";
            this.ssChk_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssChk_Sheet1.Columns.Get(4).Width = 45F;
            this.ssChk_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssChk_Sheet1.Columns.Get(5).Label = "대기명단";
            this.ssChk_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssChk_Sheet1.Columns.Get(5).Width = 89F;
            this.ssChk_Sheet1.Columns.Get(6).CellType = textCellType10;
            this.ssChk_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssChk_Sheet1.Columns.Get(6).Label = "검사실";
            this.ssChk_Sheet1.Columns.Get(6).Locked = true;
            this.ssChk_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssChk_Sheet1.Columns.Get(6).Visible = false;
            this.ssChk_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssChk_Sheet1.Columns.Get(7).Label = "Part";
            this.ssChk_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssChk_Sheet1.Columns.Get(7).Visible = false;
            this.ssChk_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssChk_Sheet1.Columns.Get(8).Label = "접수번호";
            this.ssChk_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssChk_Sheet1.Columns.Get(8).Visible = false;
            this.ssChk_Sheet1.Columns.Get(8).Width = 80F;
            this.ssChk_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssChk_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssChk_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.ssChk_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssChk_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssChk_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssChk_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.ssChk_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssChk_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssChk_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssChk_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.ssChk_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssChk_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.ssChk_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssChk_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssChk_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssChk_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.ssChk_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssChk_Sheet1.RowHeader.Visible = false;
            this.ssChk_Sheet1.Rows.Get(0).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(1).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(2).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(3).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(4).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(5).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(6).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(7).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(8).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(9).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(10).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(11).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(12).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(13).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(14).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(15).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(16).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(17).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(18).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(19).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(20).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(21).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(22).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(23).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(24).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(25).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(26).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(27).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(28).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(29).Height = 32F;
            this.ssChk_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssChk_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssChk_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.ssChk_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssChk_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.ssChk_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.pnlHicGubun);
            this.panel2.Controls.Add(this.btnSearch);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(451, 45);
            this.panel2.TabIndex = 0;
            // 
            // pnlHicGubun
            // 
            this.pnlHicGubun.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHicGubun.Controls.Add(this.rdoJepsuGubun3);
            this.pnlHicGubun.Controls.Add(this.rdoJepsuGubun2);
            this.pnlHicGubun.Controls.Add(this.rdoJepsuGubun1);
            this.pnlHicGubun.Location = new System.Drawing.Point(276, 6);
            this.pnlHicGubun.Name = "pnlHicGubun";
            this.pnlHicGubun.Size = new System.Drawing.Size(170, 31);
            this.pnlHicGubun.TabIndex = 44;
            this.pnlHicGubun.Visible = false;
            // 
            // rdoJepsuGubun3
            // 
            this.rdoJepsuGubun3.AutoSize = true;
            this.rdoJepsuGubun3.Location = new System.Drawing.Point(114, 6);
            this.rdoJepsuGubun3.Name = "rdoJepsuGubun3";
            this.rdoJepsuGubun3.Size = new System.Drawing.Size(47, 16);
            this.rdoJepsuGubun3.TabIndex = 2;
            this.rdoJepsuGubun3.Text = "출장";
            this.rdoJepsuGubun3.UseVisualStyleBackColor = true;
            // 
            // rdoJepsuGubun2
            // 
            this.rdoJepsuGubun2.AutoSize = true;
            this.rdoJepsuGubun2.Checked = true;
            this.rdoJepsuGubun2.Location = new System.Drawing.Point(61, 6);
            this.rdoJepsuGubun2.Name = "rdoJepsuGubun2";
            this.rdoJepsuGubun2.Size = new System.Drawing.Size(47, 16);
            this.rdoJepsuGubun2.TabIndex = 1;
            this.rdoJepsuGubun2.TabStop = true;
            this.rdoJepsuGubun2.Text = "내원";
            this.rdoJepsuGubun2.UseVisualStyleBackColor = true;
            // 
            // rdoJepsuGubun1
            // 
            this.rdoJepsuGubun1.AutoSize = true;
            this.rdoJepsuGubun1.Location = new System.Drawing.Point(8, 6);
            this.rdoJepsuGubun1.Name = "rdoJepsuGubun1";
            this.rdoJepsuGubun1.Size = new System.Drawing.Size(47, 16);
            this.rdoJepsuGubun1.TabIndex = 0;
            this.rdoJepsuGubun1.Text = "전체";
            this.rdoJepsuGubun1.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(180, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(65, 33);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "조회(&V)";
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.rdoGubun2);
            this.panel3.Controls.Add(this.rdoGubun1);
            this.panel3.Location = new System.Drawing.Point(5, 6);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(172, 31);
            this.panel3.TabIndex = 0;
            // 
            // rdoGubun2
            // 
            this.rdoGubun2.AutoSize = true;
            this.rdoGubun2.Location = new System.Drawing.Point(91, 6);
            this.rdoGubun2.Name = "rdoGubun2";
            this.rdoGubun2.Size = new System.Drawing.Size(71, 16);
            this.rdoGubun2.TabIndex = 1;
            this.rdoGubun2.Text = "종합검진";
            this.rdoGubun2.UseVisualStyleBackColor = true;
            // 
            // rdoGubun1
            // 
            this.rdoGubun1.AutoSize = true;
            this.rdoGubun1.Checked = true;
            this.rdoGubun1.Location = new System.Drawing.Point(7, 7);
            this.rdoGubun1.Name = "rdoGubun1";
            this.rdoGubun1.Size = new System.Drawing.Size(71, 16);
            this.rdoGubun1.TabIndex = 0;
            this.rdoGubun1.TabStop = true;
            this.rdoGubun1.Text = "일반검진";
            this.rdoGubun1.UseVisualStyleBackColor = true;
            // 
            // frmHcActExamRoomStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(451, 866);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Name = "frmHcActExamRoomStatus";
            this.Text = "frmHcActExamRoomStatus";
            this.panTitle.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssChk_Hic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sheetView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssChk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssChk_Sheet1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.pnlHicGubun.ResumeLayout(false);
            this.pnlHicGubun.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton rdoGubun2;
        private System.Windows.Forms.RadioButton rdoGubun1;
        private System.Windows.Forms.Panel panel4;
        private FarPoint.Win.Spread.FpSpread ssChk;
        private FarPoint.Win.Spread.SheetView ssChk_Sheet1;
        private System.Windows.Forms.Button btnSearch;
        private FarPoint.Win.Spread.FpSpread ssChk_Hic;
        private FarPoint.Win.Spread.SheetView sheetView1;
        private System.Windows.Forms.Panel pnlHicGubun;
        private System.Windows.Forms.RadioButton rdoJepsuGubun3;
        private System.Windows.Forms.RadioButton rdoJepsuGubun2;
        private System.Windows.Forms.RadioButton rdoJepsuGubun1;
    }
}