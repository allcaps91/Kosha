namespace ComPmpaLibB
{
    partial class frmPmpaViewResJepsuTel
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
            this.pnlBody = new System.Windows.Forms.Panel();
            this.ssList = new FarPoint.Win.Spread.FpSpread();
            this.ssList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dtpFdate = new System.Windows.Forms.DateTimePicker();
            this.dtpTdate = new System.Windows.Forms.DateTimePicker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtPtno = new System.Windows.Forms.TextBox();
            this.txtSname = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.optSort0 = new System.Windows.Forms.RadioButton();
            this.optSort2 = new System.Windows.Forms.RadioButton();
            this.optSort1 = new System.Windows.Forms.RadioButton();
            this.optSort3 = new System.Windows.Forms.RadioButton();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlHead = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.pnlBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnlHead.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBody
            // 
            this.pnlBody.BackColor = System.Drawing.SystemColors.Window;
            this.pnlBody.Controls.Add(this.ssList);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(0, 100);
            this.pnlBody.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(1154, 833);
            this.pnlBody.TabIndex = 202;
            // 
            // ssList
            // 
            this.ssList.AccessibleDescription = "ssList, Sheet1, Row 0, Column 0, ";
            this.ssList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssList.Location = new System.Drawing.Point(0, 0);
            this.ssList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ssList.Name = "ssList";
            this.ssList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList_Sheet1});
            this.ssList.Size = new System.Drawing.Size(1154, 833);
            this.ssList.TabIndex = 166;
            this.ssList.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssList_CellClick);
            // 
            // ssList_Sheet1
            // 
            this.ssList_Sheet1.Reset();
            this.ssList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssList_Sheet1.ColumnCount = 13;
            this.ssList_Sheet1.RowCount = 1;
            this.ssList_Sheet1.Cells.Get(0, 6).Value = "테스트";
            this.ssList_Sheet1.Cells.Get(0, 11).Value = "010-1234-5678";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "접수구분";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "등록번호";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "성명";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "과";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "의사명";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "예약일시";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "접수자";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "접수일시";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "삭제자";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "삭제일시";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "진료유무";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "휴대폰번호";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "비고";
            this.ssList_Sheet1.ColumnHeader.Rows.Get(0).Height = 35F;
            textCellType1.Static = true;
            this.ssList_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssList_Sheet1.Columns.Get(0).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.ssList_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(0).Label = "접수구분";
            this.ssList_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(0).Width = 62F;
            textCellType2.Static = true;
            this.ssList_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssList_Sheet1.Columns.Get(1).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.ssList_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(1).Label = "등록번호";
            this.ssList_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(1).Width = 80F;
            textCellType3.Static = true;
            this.ssList_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssList_Sheet1.Columns.Get(2).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.ssList_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(2).Label = "성명";
            this.ssList_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(2).Width = 80F;
            textCellType4.Static = true;
            this.ssList_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssList_Sheet1.Columns.Get(3).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.ssList_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(3).Label = "과";
            this.ssList_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(3).Width = 40F;
            textCellType5.Static = true;
            this.ssList_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.ssList_Sheet1.Columns.Get(4).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.ssList_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(4).Label = "의사명";
            this.ssList_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(4).Width = 80F;
            this.ssList_Sheet1.Columns.Get(5).BackColor = System.Drawing.Color.AliceBlue;
            textCellType6.Static = true;
            this.ssList_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.ssList_Sheet1.Columns.Get(5).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.ssList_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(5).Label = "예약일시";
            this.ssList_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(5).Width = 130F;
            textCellType7.Static = true;
            this.ssList_Sheet1.Columns.Get(6).CellType = textCellType7;
            this.ssList_Sheet1.Columns.Get(6).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.ssList_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(6).Label = "접수자";
            this.ssList_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(6).Width = 70F;
            textCellType8.Static = true;
            this.ssList_Sheet1.Columns.Get(7).CellType = textCellType8;
            this.ssList_Sheet1.Columns.Get(7).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.ssList_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(7).Label = "접수일시";
            this.ssList_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(7).Width = 130F;
            this.ssList_Sheet1.Columns.Get(8).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            textCellType9.Static = true;
            this.ssList_Sheet1.Columns.Get(8).CellType = textCellType9;
            this.ssList_Sheet1.Columns.Get(8).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.ssList_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(8).Label = "삭제자";
            this.ssList_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(8).Width = 70F;
            this.ssList_Sheet1.Columns.Get(9).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            textCellType10.Static = true;
            this.ssList_Sheet1.Columns.Get(9).CellType = textCellType10;
            this.ssList_Sheet1.Columns.Get(9).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.ssList_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(9).Label = "삭제일시";
            this.ssList_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(9).Width = 130F;
            textCellType11.Static = true;
            this.ssList_Sheet1.Columns.Get(10).CellType = textCellType11;
            this.ssList_Sheet1.Columns.Get(10).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.ssList_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(10).Label = "진료유무";
            this.ssList_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(10).Width = 45F;
            textCellType12.Static = true;
            this.ssList_Sheet1.Columns.Get(11).CellType = textCellType12;
            this.ssList_Sheet1.Columns.Get(11).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.ssList_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(11).Label = "휴대폰번호";
            this.ssList_Sheet1.Columns.Get(11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(11).Width = 115F;
            textCellType13.Static = true;
            this.ssList_Sheet1.Columns.Get(12).CellType = textCellType13;
            this.ssList_Sheet1.Columns.Get(12).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.ssList_Sheet1.Columns.Get(12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(12).Label = "비고";
            this.ssList_Sheet1.Columns.Get(12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.Silver;
            this.pnlTop.Controls.Add(this.groupBox3);
            this.pnlTop.Controls.Add(this.groupBox2);
            this.pnlTop.Controls.Add(this.groupBox1);
            this.pnlTop.Controls.Add(this.btnSearch);
            this.pnlTop.Controls.Add(this.btnCancel);
            this.pnlTop.Controls.Add(this.btnPrint);
            this.pnlTop.Controls.Add(this.btnExit);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 28);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(10, 14, 10, 14);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.pnlTop.Size = new System.Drawing.Size(1154, 72);
            this.pnlTop.TabIndex = 201;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dtpFdate);
            this.groupBox3.Controls.Add(this.dtpTdate);
            this.groupBox3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.groupBox3.Location = new System.Drawing.Point(447, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(243, 67);
            this.groupBox3.TabIndex = 227;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "예약일자";
            // 
            // dtpFdate
            // 
            this.dtpFdate.Font = new System.Drawing.Font("맑은 고딕", 10.75F, System.Drawing.FontStyle.Bold);
            this.dtpFdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFdate.Location = new System.Drawing.Point(6, 25);
            this.dtpFdate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpFdate.Name = "dtpFdate";
            this.dtpFdate.Size = new System.Drawing.Size(113, 27);
            this.dtpFdate.TabIndex = 200;
            // 
            // dtpTdate
            // 
            this.dtpTdate.Font = new System.Drawing.Font("맑은 고딕", 10.75F, System.Drawing.FontStyle.Bold);
            this.dtpTdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTdate.Location = new System.Drawing.Point(124, 25);
            this.dtpTdate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpTdate.Name = "dtpTdate";
            this.dtpTdate.Size = new System.Drawing.Size(113, 27);
            this.dtpTdate.TabIndex = 202;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtPtno);
            this.groupBox2.Controls.Add(this.txtSname);
            this.groupBox2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(237, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(204, 67);
            this.groupBox2.TabIndex = 227;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "등록번호";
            // 
            // txtPtno
            // 
            this.txtPtno.BackColor = System.Drawing.SystemColors.Window;
            this.txtPtno.Font = new System.Drawing.Font("맑은 고딕", 10.75F);
            this.txtPtno.Location = new System.Drawing.Point(7, 25);
            this.txtPtno.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPtno.MaxLength = 8;
            this.txtPtno.Name = "txtPtno";
            this.txtPtno.Size = new System.Drawing.Size(85, 27);
            this.txtPtno.TabIndex = 219;
            this.txtPtno.Text = "99999999";
            this.txtPtno.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtSname
            // 
            this.txtSname.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.txtSname.Enabled = false;
            this.txtSname.Font = new System.Drawing.Font("맑은 고딕", 10.75F);
            this.txtSname.Location = new System.Drawing.Point(96, 25);
            this.txtSname.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSname.Name = "txtSname";
            this.txtSname.Size = new System.Drawing.Size(96, 27);
            this.txtSname.TabIndex = 220;
            this.txtSname.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.optSort0);
            this.groupBox1.Controls.Add(this.optSort2);
            this.groupBox1.Controls.Add(this.optSort1);
            this.groupBox1.Controls.Add(this.optSort3);
            this.groupBox1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(17, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(214, 67);
            this.groupBox1.TabIndex = 226;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "정렬기준";
            // 
            // optSort0
            // 
            this.optSort0.AutoSize = true;
            this.optSort0.Checked = true;
            this.optSort0.Font = new System.Drawing.Font("맑은 고딕", 10.75F);
            this.optSort0.Location = new System.Drawing.Point(6, 15);
            this.optSort0.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optSort0.Name = "optSort0";
            this.optSort0.Size = new System.Drawing.Size(57, 24);
            this.optSort0.TabIndex = 222;
            this.optSort0.TabStop = true;
            this.optSort0.Text = "성명";
            this.optSort0.UseVisualStyleBackColor = true;
            // 
            // optSort2
            // 
            this.optSort2.AutoSize = true;
            this.optSort2.Font = new System.Drawing.Font("맑은 고딕", 10.75F);
            this.optSort2.Location = new System.Drawing.Point(6, 39);
            this.optSort2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optSort2.Name = "optSort2";
            this.optSort2.Size = new System.Drawing.Size(87, 24);
            this.optSort2.TabIndex = 223;
            this.optSort2.Text = "등록번호";
            this.optSort2.UseVisualStyleBackColor = true;
            // 
            // optSort1
            // 
            this.optSort1.AutoSize = true;
            this.optSort1.Font = new System.Drawing.Font("맑은 고딕", 10.75F);
            this.optSort1.Location = new System.Drawing.Point(101, 15);
            this.optSort1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optSort1.Name = "optSort1";
            this.optSort1.Size = new System.Drawing.Size(110, 24);
            this.optSort1.TabIndex = 224;
            this.optSort1.Text = "진료과, 시각";
            this.optSort1.UseVisualStyleBackColor = true;
            // 
            // optSort3
            // 
            this.optSort3.AutoSize = true;
            this.optSort3.Font = new System.Drawing.Font("맑은 고딕", 10.75F);
            this.optSort3.Location = new System.Drawing.Point(101, 39);
            this.optSort3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optSort3.Name = "optSort3";
            this.optSort3.Size = new System.Drawing.Size(72, 24);
            this.optSort3.TabIndex = 223;
            this.optSort3.Text = "접수자";
            this.optSort3.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.SystemColors.Window;
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Location = new System.Drawing.Point(862, 6);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 60);
            this.btnSearch.TabIndex = 221;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.SystemColors.Window;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(934, 6);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 60);
            this.btnCancel.TabIndex = 198;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.SystemColors.Window;
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrint.Location = new System.Drawing.Point(1006, 6);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 60);
            this.btnPrint.TabIndex = 193;
            this.btnPrint.Text = "출력";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.SystemColors.Window;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(1078, 6);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 60);
            this.btnExit.TabIndex = 191;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // pnlHead
            // 
            this.pnlHead.BackColor = System.Drawing.Color.RoyalBlue;
            this.pnlHead.Controls.Add(this.label7);
            this.pnlHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHead.ForeColor = System.Drawing.Color.White;
            this.pnlHead.ImeMode = System.Windows.Forms.ImeMode.KatakanaHalf;
            this.pnlHead.Location = new System.Drawing.Point(0, 0);
            this.pnlHead.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlHead.Name = "pnlHead";
            this.pnlHead.Size = new System.Drawing.Size(1154, 28);
            this.pnlHead.TabIndex = 203;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(14, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 15);
            this.label7.TabIndex = 0;
            this.label7.Text = "접수현황 통합조회";
            // 
            // frmPmpaViewResJepsuTel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1154, 933);
            this.ControlBox = false;
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlHead);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmPmpaViewResJepsuTel";
            this.Text = " ";
            this.pnlBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnlHead.ResumeLayout(false);
            this.pnlHead.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBody;
        private FarPoint.Win.Spread.FpSpread ssList;
        private FarPoint.Win.Spread.SheetView ssList_Sheet1;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.DateTimePicker dtpTdate;
        private System.Windows.Forms.DateTimePicker dtpFdate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnPrint;
        public System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel pnlHead;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSname;
        private System.Windows.Forms.TextBox txtPtno;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.RadioButton optSort1;
        private System.Windows.Forms.RadioButton optSort2;
        private System.Windows.Forms.RadioButton optSort0;
        private System.Windows.Forms.RadioButton optSort3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}