namespace HC_Main
{
    partial class frmNarcoticPrint
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
            this.components = new System.ComponentModel.Container();
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("ko-KR", false);
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.SS1 = new FarPoint.Win.Spread.FpSpread();
            this.SS1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panTitle.Controls.Add(this.btnPrint);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(295, 31);
            this.panTitle.TabIndex = 19;
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.White;
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrint.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPrint.Location = new System.Drawing.Point(164, 0);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(66, 29);
            this.btnPrint.TabIndex = 24;
            this.btnPrint.Text = "인쇄(P)";
            this.btnPrint.UseVisualStyleBackColor = false;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(230, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(63, 29);
            this.btnExit.TabIndex = 20;
            this.btnExit.Text = "닫기(&X)";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(3, 4);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(154, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "향정마약처방전인쇄";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.SS1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(295, 39);
            this.panel1.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(9, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "향정.마약 처방전 인쇄 중 ...";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SS1
            // 
            this.SS1.AccessibleDescription = "SS1, Sheet1, Row 0, Column 0, ";
            this.SS1.Location = new System.Drawing.Point(12, 54);
            this.SS1.Name = "SS1";
            this.SS1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS1_Sheet1});
            this.SS1.Size = new System.Drawing.Size(357, 163);
            this.SS1.TabIndex = 0;
            // 
            // SS1_Sheet1
            // 
            this.SS1_Sheet1.Reset();
            this.SS1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS1_Sheet1.ColumnCount = 11;
            this.SS1_Sheet1.RowCount = 14;
            this.SS1_Sheet1.Cells.Get(0, 2).ColumnSpan = 3;
            this.SS1_Sheet1.Cells.Get(0, 2).Font = new System.Drawing.Font("맑은 고딕", 14F);
            this.SS1_Sheet1.Cells.Get(0, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Cells.Get(0, 2).Value = "향 정";
            this.SS1_Sheet1.Cells.Get(0, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Cells.Get(1, 0).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(1, 1).ColumnSpan = 2;
            this.SS1_Sheet1.Cells.Get(1, 1).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(1, 1).Value = "병동호실";
            this.SS1_Sheet1.Cells.Get(1, 2).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(1, 3).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(1, 4).ColumnSpan = 2;
            this.SS1_Sheet1.Cells.Get(1, 4).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(1, 4).Value = "홍길동애기";
            this.SS1_Sheet1.Cells.Get(1, 5).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(1, 6).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(1, 7).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(1, 8).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(1, 8).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(1, 8).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(1, 8).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.SS1_Sheet1.Cells.Get(1, 8).ParseFormatString = "n";
            this.SS1_Sheet1.Cells.Get(1, 8).Value = 12345678;
            this.SS1_Sheet1.Cells.Get(1, 9).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(1, 10).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(1, 10).Value = "종합건진";
            this.SS1_Sheet1.Cells.Get(2, 0).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(2, 1).ColumnSpan = 2;
            this.SS1_Sheet1.Cells.Get(2, 1).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(2, 1).Value = "오더번호";
            this.SS1_Sheet1.Cells.Get(2, 2).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(2, 3).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(2, 4).ColumnSpan = 2;
            this.SS1_Sheet1.Cells.Get(2, 4).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(2, 4).Value = "F/26";
            this.SS1_Sheet1.Cells.Get(2, 5).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(2, 6).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(2, 7).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(2, 8).ColumnSpan = 3;
            this.SS1_Sheet1.Cells.Get(2, 8).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(2, 8).Value = "123456-1234567";
            this.SS1_Sheet1.Cells.Get(2, 9).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(2, 10).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(3, 0).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(3, 1).ColumnSpan = 10;
            this.SS1_Sheet1.Cells.Get(3, 1).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(3, 1).Value = "주소";
            this.SS1_Sheet1.Cells.Get(3, 2).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(3, 3).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(3, 4).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(3, 5).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(3, 6).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(3, 7).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(3, 8).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(3, 9).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(3, 10).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(4, 0).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(4, 1).ColumnSpan = 10;
            this.SS1_Sheet1.Cells.Get(4, 1).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(4, 1).Value = "상병명";
            this.SS1_Sheet1.Cells.Get(4, 2).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(4, 3).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(4, 4).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(4, 5).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(4, 6).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(4, 7).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(4, 8).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(4, 9).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(4, 10).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(5, 0).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(5, 1).ColumnSpan = 10;
            this.SS1_Sheet1.Cells.Get(5, 1).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(5, 1).Value = "주요증상";
            this.SS1_Sheet1.Cells.Get(5, 2).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(5, 3).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(5, 4).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(5, 5).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(5, 6).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(5, 7).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(5, 8).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(5, 9).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(5, 10).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(6, 0).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(6, 1).ColumnSpan = 4;
            this.SS1_Sheet1.Cells.Get(6, 1).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(6, 1).Value = "처방일";
            this.SS1_Sheet1.Cells.Get(6, 2).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(6, 3).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(6, 4).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(6, 5).ColumnSpan = 3;
            this.SS1_Sheet1.Cells.Get(6, 5).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(6, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Cells.Get(6, 5).Value = "검 사 일";
            this.SS1_Sheet1.Cells.Get(6, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.SS1_Sheet1.Cells.Get(6, 6).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(6, 7).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(6, 8).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(6, 9).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(6, 10).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(7, 0).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(7, 1).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(7, 2).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(7, 3).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(7, 4).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(7, 5).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(7, 6).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(7, 7).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(7, 8).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(7, 9).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(7, 10).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(8, 0).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(8, 0).Value = "A-POL2";
            this.SS1_Sheet1.Cells.Get(8, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.SS1_Sheet1.Cells.Get(8, 1).ColumnSpan = 2;
            this.SS1_Sheet1.Cells.Get(8, 1).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(8, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS1_Sheet1.Cells.Get(8, 1).Value = "포리포폴";
            this.SS1_Sheet1.Cells.Get(8, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.SS1_Sheet1.Cells.Get(8, 2).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(8, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.SS1_Sheet1.Cells.Get(8, 3).ColumnSpan = 2;
            this.SS1_Sheet1.Cells.Get(8, 3).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(8, 3).Value = "1ml";
            this.SS1_Sheet1.Cells.Get(8, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.SS1_Sheet1.Cells.Get(8, 4).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(8, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.SS1_Sheet1.Cells.Get(8, 5).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(8, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.SS1_Sheet1.Cells.Get(8, 6).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(8, 6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Cells.Get(8, 6).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(8, 6).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(8, 6).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.SS1_Sheet1.Cells.Get(8, 6).ParseFormatString = "n";
            this.SS1_Sheet1.Cells.Get(8, 6).Value = 1;
            this.SS1_Sheet1.Cells.Get(8, 6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.SS1_Sheet1.Cells.Get(8, 7).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(8, 7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Cells.Get(8, 7).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(8, 7).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(8, 7).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.SS1_Sheet1.Cells.Get(8, 7).ParseFormatString = "n";
            this.SS1_Sheet1.Cells.Get(8, 7).Value = 1;
            this.SS1_Sheet1.Cells.Get(8, 7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.SS1_Sheet1.Cells.Get(8, 8).ColumnSpan = 3;
            this.SS1_Sheet1.Cells.Get(8, 8).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(8, 8).Value = "IV";
            this.SS1_Sheet1.Cells.Get(8, 8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.SS1_Sheet1.Cells.Get(8, 9).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(8, 9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.SS1_Sheet1.Cells.Get(8, 10).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(8, 10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.SS1_Sheet1.Cells.Get(9, 0).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(9, 1).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(9, 2).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(9, 3).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(9, 4).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(9, 5).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(9, 6).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(9, 7).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(9, 8).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(9, 9).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(9, 10).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(10, 0).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(10, 1).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(10, 2).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(10, 3).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(10, 4).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(10, 5).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(10, 6).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(10, 7).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(10, 8).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(10, 9).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(10, 10).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(11, 0).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(11, 1).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(11, 2).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(11, 3).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(11, 4).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(11, 5).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(11, 6).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(11, 7).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(11, 8).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(11, 9).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(11, 10).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(12, 0).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(12, 1).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(12, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS1_Sheet1.Cells.Get(12, 1).Value = "이주령";
            this.SS1_Sheet1.Cells.Get(12, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Cells.Get(12, 2).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(12, 3).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(12, 4).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(12, 5).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(12, 6).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(12, 7).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(12, 8).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(12, 9).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(12, 10).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(13, 0).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(13, 1).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(13, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS1_Sheet1.Cells.Get(13, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(13, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(13, 1).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.SS1_Sheet1.Cells.Get(13, 1).ParseFormatString = "n";
            this.SS1_Sheet1.Cells.Get(13, 1).Value = 12345678;
            this.SS1_Sheet1.Cells.Get(13, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Cells.Get(13, 2).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(13, 3).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(13, 4).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(13, 5).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(13, 6).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(13, 7).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(13, 8).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(13, 9).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Cells.Get(13, 10).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS1_Sheet1.Columns.Get(0).Width = 81F;
            this.SS1_Sheet1.Columns.Get(1).Width = 57F;
            this.SS1_Sheet1.Columns.Get(2).Width = 103F;
            this.SS1_Sheet1.Columns.Get(3).Width = 67F;
            this.SS1_Sheet1.Columns.Get(6).Width = 30F;
            this.SS1_Sheet1.Columns.Get(7).Width = 47F;
            this.SS1_Sheet1.Columns.Get(8).Width = 77F;
            this.SS1_Sheet1.Columns.Get(9).Width = 82F;
            this.SS1_Sheet1.Columns.Get(10).Width = 64F;
            this.SS1_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
            this.SS1_Sheet1.RestrictColumns = true;
            this.SS1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS1_Sheet1.Rows.Get(0).Height = 39F;
            this.SS1_Sheet1.Rows.Get(1).Height = 29F;
            this.SS1_Sheet1.Rows.Get(2).Height = 29F;
            this.SS1_Sheet1.Rows.Get(3).Height = 29F;
            this.SS1_Sheet1.Rows.Get(4).Height = 29F;
            this.SS1_Sheet1.Rows.Get(5).Height = 29F;
            this.SS1_Sheet1.Rows.Get(6).Height = 28F;
            this.SS1_Sheet1.Rows.Get(7).Height = 25F;
            this.SS1_Sheet1.Rows.Get(8).Height = 26F;
            this.SS1_Sheet1.Rows.Get(9).Height = 26F;
            this.SS1_Sheet1.Rows.Get(10).Height = 26F;
            this.SS1_Sheet1.Rows.Get(11).Height = 35F;
            this.SS1_Sheet1.Rows.Get(12).Height = 35F;
            this.SS1_Sheet1.Rows.Get(13).Height = 35F;
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frmNarcoticPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(295, 70);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Name = "frmNarcoticPrint";
            this.Text = "향정마약처방전인쇄";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnPrint;
        private FarPoint.Win.Spread.FpSpread SS1;
        private FarPoint.Win.Spread.SheetView SS1_Sheet1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
    }
}