namespace ComHpcLibB
{
    partial class frmHcSchedule
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panSearch = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panSub01 = new System.Windows.Forms.Panel();
            this.rdoView2 = new System.Windows.Forms.RadioButton();
            this.rdoView1 = new System.Windows.Forms.RadioButton();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            this.panDate = new System.Windows.Forms.Panel();
            this.lblSub2 = new System.Windows.Forms.Label();
            this.lblMonth = new System.Windows.Forms.Label();
            this.lblSub1 = new System.Windows.Forms.Label();
            this.lblYear = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnLeft = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblFormTitle = new System.Windows.Forms.Label();
            this.panMain = new System.Windows.Forms.Panel();
            this.ssCal = new FarPoint.Win.Spread.FpSpread();
            this.ssCal_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panSearch.SuspendLayout();
            this.panSub01.SuspendLayout();
            this.panDate.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssCal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssCal_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panSearch
            // 
            this.panSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSearch.Controls.Add(this.label3);
            this.panSearch.Controls.Add(this.panSub01);
            this.panSearch.Controls.Add(this.btnSearch);
            this.panSearch.Controls.Add(this.btnPrint);
            this.panSearch.Controls.Add(this.btnRight);
            this.panSearch.Controls.Add(this.panDate);
            this.panSearch.Controls.Add(this.btnLeft);
            this.panSearch.Controls.Add(this.panel1);
            this.panSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSearch.Location = new System.Drawing.Point(0, 33);
            this.panSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panSearch.Name = "panSearch";
            this.panSearch.Size = new System.Drawing.Size(1229, 38);
            this.panSearch.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.LightBlue;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Dock = System.Windows.Forms.DockStyle.Right;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(786, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 36);
            this.label3.TabIndex = 146;
            this.label3.Text = "구분";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panSub01
            // 
            this.panSub01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSub01.Controls.Add(this.rdoView2);
            this.panSub01.Controls.Add(this.rdoView1);
            this.panSub01.Dock = System.Windows.Forms.DockStyle.Right;
            this.panSub01.Location = new System.Drawing.Point(866, 0);
            this.panSub01.Name = "panSub01";
            this.panSub01.Padding = new System.Windows.Forms.Padding(3);
            this.panSub01.Size = new System.Drawing.Size(201, 36);
            this.panSub01.TabIndex = 145;
            // 
            // rdoView2
            // 
            this.rdoView2.AutoSize = true;
            this.rdoView2.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoView2.Location = new System.Drawing.Point(81, 3);
            this.rdoView2.Name = "rdoView2";
            this.rdoView2.Size = new System.Drawing.Size(113, 28);
            this.rdoView2.TabIndex = 1;
            this.rdoView2.Text = "내원검진+학생";
            this.rdoView2.UseVisualStyleBackColor = true;
            // 
            // rdoView1
            // 
            this.rdoView1.AutoSize = true;
            this.rdoView1.Checked = true;
            this.rdoView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoView1.Location = new System.Drawing.Point(3, 3);
            this.rdoView1.Name = "rdoView1";
            this.rdoView1.Size = new System.Drawing.Size(78, 28);
            this.rdoView1.TabIndex = 0;
            this.rdoView1.TabStop = true;
            this.rdoView1.Text = "출장검진";
            this.rdoView1.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(1067, 0);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 36);
            this.btnSearch.TabIndex = 144;
            this.btnSearch.Text = "조회(&S)";
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.White;
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrint.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPrint.Location = new System.Drawing.Point(1147, 0);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(80, 36);
            this.btnPrint.TabIndex = 141;
            this.btnPrint.Text = "출력(&P)";
            this.btnPrint.UseVisualStyleBackColor = false;
            // 
            // btnRight
            // 
            this.btnRight.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnRight.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRight.Location = new System.Drawing.Point(341, 0);
            this.btnRight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(42, 36);
            this.btnRight.TabIndex = 140;
            this.btnRight.Text = "▶";
            this.btnRight.UseVisualStyleBackColor = true;
            // 
            // panDate
            // 
            this.panDate.BackColor = System.Drawing.Color.PapayaWhip;
            this.panDate.Controls.Add(this.lblSub2);
            this.panDate.Controls.Add(this.lblMonth);
            this.panDate.Controls.Add(this.lblSub1);
            this.panDate.Controls.Add(this.lblYear);
            this.panDate.Controls.Add(this.panel3);
            this.panDate.Dock = System.Windows.Forms.DockStyle.Left;
            this.panDate.Location = new System.Drawing.Point(84, 0);
            this.panDate.Name = "panDate";
            this.panDate.Size = new System.Drawing.Size(257, 36);
            this.panDate.TabIndex = 139;
            // 
            // lblSub2
            // 
            this.lblSub2.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSub2.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSub2.Location = new System.Drawing.Point(182, 0);
            this.lblSub2.Name = "lblSub2";
            this.lblSub2.Size = new System.Drawing.Size(34, 36);
            this.lblSub2.TabIndex = 134;
            this.lblSub2.Text = "월";
            this.lblSub2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMonth
            // 
            this.lblMonth.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblMonth.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblMonth.Location = new System.Drawing.Point(145, 0);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(37, 36);
            this.lblMonth.TabIndex = 133;
            this.lblMonth.Text = "05";
            this.lblMonth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSub1
            // 
            this.lblSub1.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSub1.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSub1.Location = new System.Drawing.Point(111, 0);
            this.lblSub1.Name = "lblSub1";
            this.lblSub1.Size = new System.Drawing.Size(34, 36);
            this.lblSub1.TabIndex = 132;
            this.lblSub1.Text = "년";
            this.lblSub1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblYear
            // 
            this.lblYear.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblYear.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblYear.Location = new System.Drawing.Point(39, 0);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(72, 36);
            this.lblYear.TabIndex = 131;
            this.lblYear.Text = "2020";
            this.lblYear.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(39, 36);
            this.panel3.TabIndex = 130;
            // 
            // btnLeft
            // 
            this.btnLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnLeft.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLeft.Location = new System.Drawing.Point(42, 0);
            this.btnLeft.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(42, 36);
            this.btnLeft.TabIndex = 130;
            this.btnLeft.Text = "◀";
            this.btnLeft.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(42, 36);
            this.panel1.TabIndex = 129;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.btnExit);
            this.panel4.Controls.Add(this.lblFormTitle);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1229, 33);
            this.panel4.TabIndex = 14;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(1147, 0);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(80, 31);
            this.btnExit.TabIndex = 9;
            this.btnExit.Text = "닫기(&X)";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.AutoSize = true;
            this.lblFormTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblFormTitle.Location = new System.Drawing.Point(7, 5);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(150, 21);
            this.lblFormTitle.TabIndex = 7;
            this.lblFormTitle.Text = "일반검진 예약 달력";
            // 
            // panMain
            // 
            this.panMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panMain.Controls.Add(this.ssCal);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(0, 71);
            this.panMain.Name = "panMain";
            this.panMain.Size = new System.Drawing.Size(1229, 870);
            this.panMain.TabIndex = 15;
            // 
            // ssCal
            // 
            this.ssCal.AccessibleDescription = "ssCal, Sheet1, Row 0, Column 0, ";
            this.ssCal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssCal.FocusRenderer = flatFocusIndicatorRenderer1;
            this.ssCal.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssCal.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssCal.HorizontalScrollBar.Renderer = flatScrollBarRenderer1;
            this.ssCal.HorizontalScrollBar.TabIndex = 110;
            this.ssCal.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssCal.Location = new System.Drawing.Point(0, 0);
            this.ssCal.Name = "ssCal";
            this.ssCal.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssCal_Sheet1});
            this.ssCal.Size = new System.Drawing.Size(1227, 868);
            this.ssCal.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.ssCal.TabIndex = 5;
            this.ssCal.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssCal.VerticalScrollBar.Name = "";
            flatScrollBarRenderer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssCal.VerticalScrollBar.Renderer = flatScrollBarRenderer2;
            this.ssCal.VerticalScrollBar.TabIndex = 111;
            this.ssCal.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // ssCal_Sheet1
            // 
            this.ssCal_Sheet1.Reset();
            this.ssCal_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssCal_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssCal_Sheet1.ColumnCount = 7;
            this.ssCal_Sheet1.RowCount = 5;
            this.ssCal_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssCal_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssCal_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.ssCal_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssCal_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssCal_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssCal_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.ssCal_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssCal_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "일";
            this.ssCal_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "월 MON";
            this.ssCal_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "화 TUE";
            this.ssCal_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "수 WED";
            this.ssCal_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "목 THU";
            this.ssCal_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "금 FRI";
            this.ssCal_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "토 SAT";
            this.ssCal_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssCal_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssCal_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.ssCal_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssCal_Sheet1.ColumnHeader.Rows.Get(0).Height = 25F;
            this.ssCal_Sheet1.Columns.Get(0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            textCellType1.Multiline = true;
            textCellType1.WordWrap = true;
            this.ssCal_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssCal_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssCal_Sheet1.Columns.Get(0).Label = "일";
            this.ssCal_Sheet1.Columns.Get(0).Locked = true;
            this.ssCal_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.ssCal_Sheet1.Columns.Get(0).Width = 39F;
            textCellType2.Multiline = true;
            textCellType2.WordWrap = true;
            this.ssCal_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssCal_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssCal_Sheet1.Columns.Get(1).Label = "월 MON";
            this.ssCal_Sheet1.Columns.Get(1).Locked = true;
            this.ssCal_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.ssCal_Sheet1.Columns.Get(1).Width = 197F;
            textCellType3.Multiline = true;
            textCellType3.WordWrap = true;
            this.ssCal_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssCal_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssCal_Sheet1.Columns.Get(2).Label = "화 TUE";
            this.ssCal_Sheet1.Columns.Get(2).Locked = true;
            this.ssCal_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.ssCal_Sheet1.Columns.Get(2).Width = 197F;
            textCellType4.Multiline = true;
            textCellType4.WordWrap = true;
            this.ssCal_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssCal_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssCal_Sheet1.Columns.Get(3).Label = "수 WED";
            this.ssCal_Sheet1.Columns.Get(3).Locked = true;
            this.ssCal_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.ssCal_Sheet1.Columns.Get(3).Width = 197F;
            textCellType5.Multiline = true;
            textCellType5.WordWrap = true;
            this.ssCal_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.ssCal_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssCal_Sheet1.Columns.Get(4).Label = "목 THU";
            this.ssCal_Sheet1.Columns.Get(4).Locked = true;
            this.ssCal_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.ssCal_Sheet1.Columns.Get(4).Width = 197F;
            textCellType6.Multiline = true;
            textCellType6.WordWrap = true;
            this.ssCal_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.ssCal_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssCal_Sheet1.Columns.Get(5).Label = "금 FRI";
            this.ssCal_Sheet1.Columns.Get(5).Locked = true;
            this.ssCal_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.ssCal_Sheet1.Columns.Get(5).Width = 197F;
            textCellType7.Multiline = true;
            textCellType7.WordWrap = true;
            this.ssCal_Sheet1.Columns.Get(6).CellType = textCellType7;
            this.ssCal_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssCal_Sheet1.Columns.Get(6).Label = "토 SAT";
            this.ssCal_Sheet1.Columns.Get(6).Locked = true;
            this.ssCal_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.ssCal_Sheet1.Columns.Get(6).Width = 197F;
            this.ssCal_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssCal_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssCal_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.ssCal_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssCal_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssCal_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssCal_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.ssCal_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssCal_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.ssCal_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssCal_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssCal_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssCal_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.ssCal_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssCal_Sheet1.RowHeader.Visible = false;
            this.ssCal_Sheet1.Rows.Get(0).Height = 131F;
            this.ssCal_Sheet1.Rows.Get(1).Height = 131F;
            this.ssCal_Sheet1.Rows.Get(2).Height = 131F;
            this.ssCal_Sheet1.Rows.Get(3).Height = 131F;
            this.ssCal_Sheet1.Rows.Get(4).Height = 131F;
            this.ssCal_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssCal_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssCal_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.ssCal_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssCal_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.ssCal_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmHcSchedule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1229, 941);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.panSearch);
            this.Controls.Add(this.panel4);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHcSchedule";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "일반검진 예약달력";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panSearch.ResumeLayout(false);
            this.panSub01.ResumeLayout(false);
            this.panSub01.PerformLayout();
            this.panDate.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssCal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssCal_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panSearch;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Panel panMain;
        private FarPoint.Win.Spread.FpSpread ssCal;
        private FarPoint.Win.Spread.SheetView ssCal_Sheet1;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Panel panDate;
        private System.Windows.Forms.Label lblSub2;
        private System.Windows.Forms.Label lblMonth;
        private System.Windows.Forms.Label lblSub1;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panSub01;
        private System.Windows.Forms.RadioButton rdoView2;
        private System.Windows.Forms.RadioButton rdoView1;
        private System.Windows.Forms.Button btnSearch;
    }
}