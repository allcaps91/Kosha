namespace ComEmrBase
{
    partial class frmNrIONew2
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType13 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType14 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType15 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkAsc = new System.Windows.Forms.CheckBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.dtpFrDateTot2 = new System.Windows.Forms.DateTimePicker();
            this.dtpFrDateTot = new System.Windows.Forms.DateTimePicker();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearchTot = new System.Windows.Forms.Button();
            this.ssIoTot = new FarPoint.Win.Spread.FpSpread();
            this.ssIoTot_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssIoTot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssIoTot_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkAsc);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.dtpFrDateTot2);
            this.panel1.Controls.Add(this.dtpFrDateTot);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnSearchTot);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(523, 71);
            this.panel1.TabIndex = 3;
            // 
            // chkAsc
            // 
            this.chkAsc.AutoSize = true;
            this.chkAsc.Location = new System.Drawing.Point(244, 52);
            this.chkAsc.Name = "chkAsc";
            this.chkAsc.Size = new System.Drawing.Size(60, 16);
            this.chkAsc.TabIndex = 99;
            this.chkAsc.Text = "순정렬";
            this.chkAsc.UseVisualStyleBackColor = true;
            this.chkAsc.CheckedChanged += new System.EventHandler(this.chkAsc_CheckedChanged);
            // 
            // btnPrint
            // 
            this.btnPrint.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPrint.ForeColor = System.Drawing.Color.Black;
            this.btnPrint.Location = new System.Drawing.Point(404, 21);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(80, 26);
            this.btnPrint.TabIndex = 98;
            this.btnPrint.Text = "출력";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Visible = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // dtpFrDateTot2
            // 
            this.dtpFrDateTot2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtpFrDateTot2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrDateTot2.Location = new System.Drawing.Point(129, 21);
            this.dtpFrDateTot2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpFrDateTot2.Name = "dtpFrDateTot2";
            this.dtpFrDateTot2.Size = new System.Drawing.Size(113, 25);
            this.dtpFrDateTot2.TabIndex = 93;
            // 
            // dtpFrDateTot
            // 
            this.dtpFrDateTot.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtpFrDateTot.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrDateTot.Location = new System.Drawing.Point(7, 21);
            this.dtpFrDateTot.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpFrDateTot.Name = "dtpFrDateTot";
            this.dtpFrDateTot.Size = new System.Drawing.Size(116, 25);
            this.dtpFrDateTot.TabIndex = 93;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(324, 21);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(80, 26);
            this.btnExit.TabIndex = 33;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSearchTot
            // 
            this.btnSearchTot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnSearchTot.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSearchTot.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSearchTot.Location = new System.Drawing.Point(244, 21);
            this.btnSearchTot.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearchTot.Name = "btnSearchTot";
            this.btnSearchTot.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnSearchTot.Size = new System.Drawing.Size(80, 26);
            this.btnSearchTot.TabIndex = 96;
            this.btnSearchTot.Text = "조회";
            this.btnSearchTot.UseVisualStyleBackColor = false;
            this.btnSearchTot.Click += new System.EventHandler(this.btnSearchTot_Click);
            // 
            // ssIoTot
            // 
            this.ssIoTot.AccessibleDescription = "ssIoTot, Sheet1, Row 0, Column 0, 날짜";
            this.ssIoTot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssIoTot.Location = new System.Drawing.Point(0, 71);
            this.ssIoTot.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ssIoTot.Name = "ssIoTot";
            this.ssIoTot.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssIoTot_Sheet1});
            this.ssIoTot.Size = new System.Drawing.Size(523, 970);
            this.ssIoTot.TabIndex = 15;
            // 
            // ssIoTot_Sheet1
            // 
            this.ssIoTot_Sheet1.Reset();
            this.ssIoTot_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssIoTot_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssIoTot_Sheet1.ColumnCount = 8;
            this.ssIoTot_Sheet1.RowCount = 2;
            this.ssIoTot_Sheet1.Cells.Get(0, 0).BackColor = System.Drawing.Color.LightBlue;
            textCellType1.ReadOnly = true;
            this.ssIoTot_Sheet1.Cells.Get(0, 0).CellType = textCellType1;
            this.ssIoTot_Sheet1.Cells.Get(0, 0).Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.ssIoTot_Sheet1.Cells.Get(0, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssIoTot_Sheet1.Cells.Get(0, 0).Locked = true;
            this.ssIoTot_Sheet1.Cells.Get(0, 0).RowSpan = 2;
            this.ssIoTot_Sheet1.Cells.Get(0, 0).Value = "날짜";
            this.ssIoTot_Sheet1.Cells.Get(0, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssIoTot_Sheet1.Cells.Get(0, 1).BackColor = System.Drawing.Color.LightBlue;
            textCellType2.MaxLength = 3000;
            textCellType2.Multiline = true;
            textCellType2.ReadOnly = true;
            textCellType2.WordWrap = true;
            this.ssIoTot_Sheet1.Cells.Get(0, 1).CellType = textCellType2;
            this.ssIoTot_Sheet1.Cells.Get(0, 1).Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.ssIoTot_Sheet1.Cells.Get(0, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssIoTot_Sheet1.Cells.Get(0, 1).Locked = true;
            this.ssIoTot_Sheet1.Cells.Get(0, 1).Value = "구분";
            this.ssIoTot_Sheet1.Cells.Get(0, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssIoTot_Sheet1.Cells.Get(0, 2).BackColor = System.Drawing.Color.LightBlue;
            this.ssIoTot_Sheet1.Cells.Get(0, 2).CellType = textCellType3;
            this.ssIoTot_Sheet1.Cells.Get(0, 2).Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.ssIoTot_Sheet1.Cells.Get(0, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssIoTot_Sheet1.Cells.Get(0, 2).Locked = true;
            this.ssIoTot_Sheet1.Cells.Get(0, 2).RowSpan = 2;
            this.ssIoTot_Sheet1.Cells.Get(0, 2).Value = "대분류";
            this.ssIoTot_Sheet1.Cells.Get(0, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssIoTot_Sheet1.Cells.Get(0, 3).BackColor = System.Drawing.Color.LightBlue;
            textCellType4.MaxLength = 300;
            textCellType4.Multiline = true;
            textCellType4.WordWrap = true;
            this.ssIoTot_Sheet1.Cells.Get(0, 3).CellType = textCellType4;
            this.ssIoTot_Sheet1.Cells.Get(0, 3).Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.ssIoTot_Sheet1.Cells.Get(0, 3).Locked = true;
            this.ssIoTot_Sheet1.Cells.Get(0, 3).RowSpan = 2;
            this.ssIoTot_Sheet1.Cells.Get(0, 3).Value = "아이템";
            this.ssIoTot_Sheet1.Cells.Get(0, 4).BackColor = System.Drawing.Color.LightBlue;
            textCellType5.ReadOnly = true;
            this.ssIoTot_Sheet1.Cells.Get(0, 4).CellType = textCellType5;
            this.ssIoTot_Sheet1.Cells.Get(0, 4).Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.ssIoTot_Sheet1.Cells.Get(0, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssIoTot_Sheet1.Cells.Get(0, 4).Locked = true;
            this.ssIoTot_Sheet1.Cells.Get(0, 4).RowSpan = 2;
            this.ssIoTot_Sheet1.Cells.Get(0, 4).Value = "Day";
            this.ssIoTot_Sheet1.Cells.Get(0, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssIoTot_Sheet1.Cells.Get(0, 5).BackColor = System.Drawing.Color.LightBlue;
            textCellType6.ReadOnly = true;
            this.ssIoTot_Sheet1.Cells.Get(0, 5).CellType = textCellType6;
            this.ssIoTot_Sheet1.Cells.Get(0, 5).Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.ssIoTot_Sheet1.Cells.Get(0, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssIoTot_Sheet1.Cells.Get(0, 5).Locked = true;
            this.ssIoTot_Sheet1.Cells.Get(0, 5).RowSpan = 2;
            this.ssIoTot_Sheet1.Cells.Get(0, 5).Value = "Eve";
            this.ssIoTot_Sheet1.Cells.Get(0, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssIoTot_Sheet1.Cells.Get(0, 6).BackColor = System.Drawing.Color.LightBlue;
            textCellType7.ReadOnly = true;
            this.ssIoTot_Sheet1.Cells.Get(0, 6).CellType = textCellType7;
            this.ssIoTot_Sheet1.Cells.Get(0, 6).Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.ssIoTot_Sheet1.Cells.Get(0, 6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssIoTot_Sheet1.Cells.Get(0, 6).Locked = true;
            this.ssIoTot_Sheet1.Cells.Get(0, 6).RowSpan = 2;
            this.ssIoTot_Sheet1.Cells.Get(0, 6).Value = "Night";
            this.ssIoTot_Sheet1.Cells.Get(0, 6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssIoTot_Sheet1.Cells.Get(0, 7).BackColor = System.Drawing.Color.LightBlue;
            textCellType8.ReadOnly = true;
            this.ssIoTot_Sheet1.Cells.Get(0, 7).CellType = textCellType8;
            this.ssIoTot_Sheet1.Cells.Get(0, 7).Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.ssIoTot_Sheet1.Cells.Get(0, 7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssIoTot_Sheet1.Cells.Get(0, 7).Locked = true;
            this.ssIoTot_Sheet1.Cells.Get(0, 7).RowSpan = 2;
            this.ssIoTot_Sheet1.Cells.Get(0, 7).Value = "Tot";
            this.ssIoTot_Sheet1.Cells.Get(0, 7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssIoTot_Sheet1.Cells.Get(1, 0).Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.ssIoTot_Sheet1.Cells.Get(1, 0).Locked = true;
            this.ssIoTot_Sheet1.Cells.Get(1, 1).BackColor = System.Drawing.Color.LightBlue;
            textCellType9.Multiline = true;
            textCellType9.WordWrap = true;
            this.ssIoTot_Sheet1.Cells.Get(1, 1).CellType = textCellType9;
            this.ssIoTot_Sheet1.Cells.Get(1, 1).Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.ssIoTot_Sheet1.Cells.Get(1, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssIoTot_Sheet1.Cells.Get(1, 1).Locked = true;
            this.ssIoTot_Sheet1.Cells.Get(1, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssIoTot_Sheet1.Cells.Get(1, 2).BackColor = System.Drawing.Color.LightBlue;
            this.ssIoTot_Sheet1.Cells.Get(1, 2).Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.ssIoTot_Sheet1.Cells.Get(1, 2).Locked = true;
            this.ssIoTot_Sheet1.Cells.Get(1, 3).CellType = textCellType10;
            this.ssIoTot_Sheet1.Cells.Get(1, 3).Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.ssIoTot_Sheet1.Cells.Get(1, 3).Locked = true;
            this.ssIoTot_Sheet1.Cells.Get(1, 4).BackColor = System.Drawing.Color.LightBlue;
            this.ssIoTot_Sheet1.Cells.Get(1, 4).Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.ssIoTot_Sheet1.Cells.Get(1, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssIoTot_Sheet1.Cells.Get(1, 4).Locked = true;
            this.ssIoTot_Sheet1.Cells.Get(1, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssIoTot_Sheet1.Cells.Get(1, 5).BackColor = System.Drawing.Color.LightBlue;
            this.ssIoTot_Sheet1.Cells.Get(1, 5).Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.ssIoTot_Sheet1.Cells.Get(1, 5).Locked = true;
            this.ssIoTot_Sheet1.Cells.Get(1, 6).BackColor = System.Drawing.Color.LightBlue;
            this.ssIoTot_Sheet1.Cells.Get(1, 6).Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.ssIoTot_Sheet1.Cells.Get(1, 6).Locked = true;
            this.ssIoTot_Sheet1.Cells.Get(1, 7).BackColor = System.Drawing.Color.LightBlue;
            this.ssIoTot_Sheet1.Cells.Get(1, 7).Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.ssIoTot_Sheet1.Cells.Get(1, 7).Locked = true;
            this.ssIoTot_Sheet1.ColumnHeader.Visible = false;
            this.ssIoTot_Sheet1.Columns.Get(0).BackColor = System.Drawing.Color.White;
            this.ssIoTot_Sheet1.Columns.Get(0).Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.ssIoTot_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssIoTot_Sheet1.Columns.Get(0).Locked = true;
            this.ssIoTot_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssIoTot_Sheet1.Columns.Get(0).Width = 80F;
            this.ssIoTot_Sheet1.Columns.Get(1).BackColor = System.Drawing.Color.White;
            this.ssIoTot_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssIoTot_Sheet1.Columns.Get(1).Locked = true;
            this.ssIoTot_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssIoTot_Sheet1.Columns.Get(1).Visible = false;
            this.ssIoTot_Sheet1.Columns.Get(1).Width = 35F;
            this.ssIoTot_Sheet1.Columns.Get(2).BackColor = System.Drawing.Color.White;
            this.ssIoTot_Sheet1.Columns.Get(2).CellType = textCellType11;
            this.ssIoTot_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssIoTot_Sheet1.Columns.Get(2).Locked = true;
            this.ssIoTot_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssIoTot_Sheet1.Columns.Get(2).Width = 80F;
            this.ssIoTot_Sheet1.Columns.Get(3).BackColor = System.Drawing.Color.White;
            this.ssIoTot_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssIoTot_Sheet1.Columns.Get(3).Locked = true;
            this.ssIoTot_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssIoTot_Sheet1.Columns.Get(3).Width = 100F;
            this.ssIoTot_Sheet1.Columns.Get(4).BackColor = System.Drawing.Color.White;
            textCellType12.ReadOnly = true;
            this.ssIoTot_Sheet1.Columns.Get(4).CellType = textCellType12;
            this.ssIoTot_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssIoTot_Sheet1.Columns.Get(4).Locked = true;
            this.ssIoTot_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssIoTot_Sheet1.Columns.Get(5).BackColor = System.Drawing.Color.White;
            textCellType13.ReadOnly = true;
            this.ssIoTot_Sheet1.Columns.Get(5).CellType = textCellType13;
            this.ssIoTot_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssIoTot_Sheet1.Columns.Get(5).Locked = true;
            this.ssIoTot_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssIoTot_Sheet1.Columns.Get(6).BackColor = System.Drawing.Color.White;
            textCellType14.ReadOnly = true;
            this.ssIoTot_Sheet1.Columns.Get(6).CellType = textCellType14;
            this.ssIoTot_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssIoTot_Sheet1.Columns.Get(6).Locked = true;
            this.ssIoTot_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssIoTot_Sheet1.Columns.Get(7).BackColor = System.Drawing.Color.White;
            textCellType15.ReadOnly = true;
            this.ssIoTot_Sheet1.Columns.Get(7).CellType = textCellType15;
            this.ssIoTot_Sheet1.Columns.Get(7).Font = new System.Drawing.Font("굴림", 9F);
            this.ssIoTot_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssIoTot_Sheet1.Columns.Get(7).Locked = true;
            this.ssIoTot_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssIoTot_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssIoTot_Sheet1.RowHeader.Visible = false;
            this.ssIoTot_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmNrIONew2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 1041);
            this.Controls.Add(this.ssIoTot);
            this.Controls.Add(this.panel1);
            this.Name = "frmNrIONew2";
            this.Text = "frmNrIONew2";
            this.Load += new System.EventHandler(this.frmNrIONew_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssIoTot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssIoTot_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.DateTimePicker dtpFrDateTot;
        public System.Windows.Forms.Button btnSearchTot;
        private FarPoint.Win.Spread.FpSpread ssIoTot;
        private FarPoint.Win.Spread.SheetView ssIoTot_Sheet1;
        private System.Windows.Forms.DateTimePicker dtpFrDateTot2;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.CheckBox chkAsc;
    }
}