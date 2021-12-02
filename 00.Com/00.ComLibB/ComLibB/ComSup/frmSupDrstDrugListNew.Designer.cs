namespace ComLibB
{
    partial class frmSupDrstDrugListNew
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
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.rdoSort5 = new System.Windows.Forms.RadioButton();
            this.rdoSort3 = new System.Windows.Forms.RadioButton();
            this.rdoSort2 = new System.Windows.Forms.RadioButton();
            this.rdoSort4 = new System.Windows.Forms.RadioButton();
            this.rdoSort1 = new System.Windows.Forms.RadioButton();
            this.rdoSort0 = new System.Windows.Forms.RadioButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdoGu2 = new System.Windows.Forms.RadioButton();
            this.rdoGu1 = new System.Windows.Forms.RadioButton();
            this.rdoGu0 = new System.Windows.Forms.RadioButton();
            this.cboGubun = new System.Windows.Forms.ComboBox();
            this.chkOK = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panTitleSub = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panTitleSub.SuspendLayout();
            this.SuspendLayout();
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 14;
            this.ssView_Sheet1.RowCount = 1;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "코드";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "약품명 및 성분";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "약품명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "성분명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "규격 및 단위";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "효능";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "제약회사";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "분류\r\n번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "제형";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "사용법";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "단가";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "등재일";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 12).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "저장방법\r\n(온도)";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 13).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "저장방법\r\n(차광)";
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssView_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssView_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(0).Label = "코드";
            this.ssView_Sheet1.Columns.Get(0).Locked = true;
            this.ssView_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(0).Width = 80F;
            this.ssView_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssView_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssView_Sheet1.Columns.Get(1).Label = "약품명 및 성분";
            this.ssView_Sheet1.Columns.Get(1).Locked = true;
            this.ssView_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(1).Width = 250F;
            this.ssView_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssView_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssView_Sheet1.Columns.Get(2).Label = "약품명";
            this.ssView_Sheet1.Columns.Get(2).Locked = true;
            this.ssView_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(2).Width = 150F;
            this.ssView_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssView_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssView_Sheet1.Columns.Get(3).Label = "성분명";
            this.ssView_Sheet1.Columns.Get(3).Locked = true;
            this.ssView_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(3).Width = 150F;
            this.ssView_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.ssView_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssView_Sheet1.Columns.Get(4).Label = "규격 및 단위";
            this.ssView_Sheet1.Columns.Get(4).Locked = true;
            this.ssView_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(4).Width = 150F;
            this.ssView_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.ssView_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssView_Sheet1.Columns.Get(5).Label = "효능";
            this.ssView_Sheet1.Columns.Get(5).Locked = true;
            this.ssView_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(5).Width = 150F;
            this.ssView_Sheet1.Columns.Get(6).CellType = textCellType7;
            this.ssView_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssView_Sheet1.Columns.Get(6).Label = "제약회사";
            this.ssView_Sheet1.Columns.Get(6).Locked = true;
            this.ssView_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(6).Width = 150F;
            this.ssView_Sheet1.Columns.Get(7).CellType = textCellType8;
            this.ssView_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(7).Label = "분류\r\n번호";
            this.ssView_Sheet1.Columns.Get(7).Locked = true;
            this.ssView_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(8).CellType = textCellType9;
            this.ssView_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(8).Label = "제형";
            this.ssView_Sheet1.Columns.Get(8).Locked = true;
            this.ssView_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(9).CellType = textCellType10;
            this.ssView_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssView_Sheet1.Columns.Get(9).Label = "사용법";
            this.ssView_Sheet1.Columns.Get(9).Locked = true;
            this.ssView_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(9).Width = 100F;
            this.ssView_Sheet1.Columns.Get(10).CellType = textCellType11;
            this.ssView_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssView_Sheet1.Columns.Get(10).Label = "단가";
            this.ssView_Sheet1.Columns.Get(10).Locked = true;
            this.ssView_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(10).Width = 80F;
            this.ssView_Sheet1.Columns.Get(11).CellType = textCellType12;
            this.ssView_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(11).Label = "등재일";
            this.ssView_Sheet1.Columns.Get(11).Locked = true;
            this.ssView_Sheet1.Columns.Get(11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(11).Width = 80F;
            this.ssView_Sheet1.Columns.Get(12).CellType = textCellType13;
            this.ssView_Sheet1.Columns.Get(12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssView_Sheet1.Columns.Get(12).Label = "저장방법\r\n(온도)";
            this.ssView_Sheet1.Columns.Get(12).Locked = true;
            this.ssView_Sheet1.Columns.Get(12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(12).Width = 80F;
            this.ssView_Sheet1.Columns.Get(13).CellType = textCellType14;
            this.ssView_Sheet1.Columns.Get(13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(13).Label = "저장방법\r\n(차광)";
            this.ssView_Sheet1.Columns.Get(13).Locked = true;
            this.ssView_Sheet1.Columns.Get(13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(13).Width = 80F;
            this.ssView_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // rdoSort5
            // 
            this.rdoSort5.AutoSize = true;
            this.rdoSort5.Location = new System.Drawing.Point(160, 45);
            this.rdoSort5.Name = "rdoSort5";
            this.rdoSort5.Size = new System.Drawing.Size(65, 21);
            this.rdoSort5.TabIndex = 0;
            this.rdoSort5.TabStop = true;
            this.rdoSort5.Text = "인증용";
            this.rdoSort5.UseVisualStyleBackColor = true;
            this.rdoSort5.CheckedChanged += new System.EventHandler(this.rdoSort_CheckedChanged);
            // 
            // rdoSort3
            // 
            this.rdoSort3.AutoSize = true;
            this.rdoSort3.Location = new System.Drawing.Point(85, 45);
            this.rdoSort3.Name = "rdoSort3";
            this.rdoSort3.Size = new System.Drawing.Size(65, 21);
            this.rdoSort3.TabIndex = 0;
            this.rdoSort3.TabStop = true;
            this.rdoSort3.Text = "성분순";
            this.rdoSort3.UseVisualStyleBackColor = true;
            this.rdoSort3.CheckedChanged += new System.EventHandler(this.rdoSort_CheckedChanged);
            // 
            // rdoSort2
            // 
            this.rdoSort2.AutoSize = true;
            this.rdoSort2.Location = new System.Drawing.Point(10, 45);
            this.rdoSort2.Name = "rdoSort2";
            this.rdoSort2.Size = new System.Drawing.Size(65, 21);
            this.rdoSort2.TabIndex = 0;
            this.rdoSort2.TabStop = true;
            this.rdoSort2.Text = "효능순";
            this.rdoSort2.UseVisualStyleBackColor = true;
            this.rdoSort2.CheckedChanged += new System.EventHandler(this.rdoSort_CheckedChanged);
            // 
            // rdoSort4
            // 
            this.rdoSort4.AutoSize = true;
            this.rdoSort4.Location = new System.Drawing.Point(160, 22);
            this.rdoSort4.Name = "rdoSort4";
            this.rdoSort4.Size = new System.Drawing.Size(72, 21);
            this.rdoSort4.TabIndex = 0;
            this.rdoSort4.TabStop = true;
            this.rdoSort4.Text = "효능순2";
            this.rdoSort4.UseVisualStyleBackColor = true;
            this.rdoSort4.CheckedChanged += new System.EventHandler(this.rdoSort_CheckedChanged);
            // 
            // rdoSort1
            // 
            this.rdoSort1.AutoSize = true;
            this.rdoSort1.Location = new System.Drawing.Point(85, 22);
            this.rdoSort1.Name = "rdoSort1";
            this.rdoSort1.Size = new System.Drawing.Size(65, 21);
            this.rdoSort1.TabIndex = 0;
            this.rdoSort1.TabStop = true;
            this.rdoSort1.Text = "품명순";
            this.rdoSort1.UseVisualStyleBackColor = true;
            this.rdoSort1.CheckedChanged += new System.EventHandler(this.rdoSort_CheckedChanged);
            // 
            // rdoSort0
            // 
            this.rdoSort0.AutoSize = true;
            this.rdoSort0.Location = new System.Drawing.Point(10, 22);
            this.rdoSort0.Name = "rdoSort0";
            this.rdoSort0.Size = new System.Drawing.Size(65, 21);
            this.rdoSort0.TabIndex = 0;
            this.rdoSort0.TabStop = true;
            this.rdoSort0.Text = "코드순";
            this.rdoSort0.UseVisualStyleBackColor = true;
            this.rdoSort0.CheckedChanged += new System.EventHandler(this.rdoSort_CheckedChanged);
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(243, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(10, 74);
            this.panel3.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoSort5);
            this.groupBox1.Controls.Add(this.rdoSort3);
            this.groupBox1.Controls.Add(this.rdoSort2);
            this.groupBox1.Controls.Add(this.rdoSort4);
            this.groupBox1.Controls.Add(this.rdoSort1);
            this.groupBox1.Controls.Add(this.rdoSort0);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(240, 74);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "정렬";
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "fpSpread1, Sheet1, Row 0, Column 0, ";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssView.Location = new System.Drawing.Point(0, 148);
            this.ssView.Name = "ssView";
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(966, 436);
            this.ssView.TabIndex = 37;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdoGu2);
            this.groupBox2.Controls.Add(this.rdoGu1);
            this.groupBox2.Controls.Add(this.rdoGu0);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.Location = new System.Drawing.Point(253, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(230, 74);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "작업";
            // 
            // rdoGu2
            // 
            this.rdoGu2.AutoSize = true;
            this.rdoGu2.Location = new System.Drawing.Point(132, 29);
            this.rdoGu2.Name = "rdoGu2";
            this.rdoGu2.Size = new System.Drawing.Size(91, 21);
            this.rdoGu2.TabIndex = 0;
            this.rdoGu2.TabStop = true;
            this.rdoGu2.Text = "원내외모두";
            this.rdoGu2.UseVisualStyleBackColor = true;
            // 
            // rdoGu1
            // 
            this.rdoGu1.AutoSize = true;
            this.rdoGu1.Location = new System.Drawing.Point(71, 29);
            this.rdoGu1.Name = "rdoGu1";
            this.rdoGu1.Size = new System.Drawing.Size(52, 21);
            this.rdoGu1.TabIndex = 0;
            this.rdoGu1.TabStop = true;
            this.rdoGu1.Text = "원외";
            this.rdoGu1.UseVisualStyleBackColor = true;
            // 
            // rdoGu0
            // 
            this.rdoGu0.AutoSize = true;
            this.rdoGu0.Location = new System.Drawing.Point(10, 29);
            this.rdoGu0.Name = "rdoGu0";
            this.rdoGu0.Size = new System.Drawing.Size(52, 21);
            this.rdoGu0.TabIndex = 0;
            this.rdoGu0.TabStop = true;
            this.rdoGu0.Text = "원내";
            this.rdoGu0.UseVisualStyleBackColor = true;
            // 
            // cboGubun
            // 
            this.cboGubun.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGubun.FormattingEnabled = true;
            this.cboGubun.Location = new System.Drawing.Point(10, 30);
            this.cboGubun.Name = "cboGubun";
            this.cboGubun.Size = new System.Drawing.Size(140, 25);
            this.cboGubun.TabIndex = 0;
            // 
            // chkOK
            // 
            this.chkOK.AutoSize = true;
            this.chkOK.Location = new System.Drawing.Point(680, 31);
            this.chkOK.Name = "chkOK";
            this.chkOK.Size = new System.Drawing.Size(123, 21);
            this.chkOK.TabIndex = 5;
            this.chkOK.Text = "영문명으로 보기";
            this.chkOK.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cboGubun);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox3.Location = new System.Drawing.Point(493, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(160, 74);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "구분";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chkOK);
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 68);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(3);
            this.panel2.Size = new System.Drawing.Size(966, 80);
            this.panel2.TabIndex = 36;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(483, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(10, 74);
            this.panel4.TabIndex = 3;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(865, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(100, 30);
            this.btnExit.TabIndex = 0;
            this.btnExit.Text = "닫  기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Location = new System.Drawing.Point(765, 2);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(100, 30);
            this.btnPrint.TabIndex = 0;
            this.btnPrint.Text = "출  력";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(665, 2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 30);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "조  회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(966, 34);
            this.panel1.TabIndex = 35;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(5, 5);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(90, 21);
            this.lblTitleSub0.TabIndex = 4;
            this.lblTitleSub0.Text = "약품리스트";
            // 
            // panTitleSub
            // 
            this.panTitleSub.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub.Controls.Add(this.lblTitleSub0);
            this.panTitleSub.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub.Location = new System.Drawing.Point(0, 0);
            this.panTitleSub.Name = "panTitleSub";
            this.panTitleSub.Size = new System.Drawing.Size(966, 34);
            this.panTitleSub.TabIndex = 34;
            // 
            // frmSupDrstDrugListNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(966, 584);
            this.Controls.Add(this.ssView);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitleSub);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmSupDrstDrugListNew";
            this.Text = "frmSupDrstDrugListNew";
            this.Load += new System.EventHandler(this.frmSupDrstDrugListNew_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panTitleSub.ResumeLayout(false);
            this.panTitleSub.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
        private System.Windows.Forms.RadioButton rdoSort5;
        private System.Windows.Forms.RadioButton rdoSort3;
        private System.Windows.Forms.RadioButton rdoSort2;
        private System.Windows.Forms.RadioButton rdoSort4;
        private System.Windows.Forms.RadioButton rdoSort1;
        private System.Windows.Forms.RadioButton rdoSort0;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox1;
        private FarPoint.Win.Spread.FpSpread ssView;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdoGu2;
        private System.Windows.Forms.RadioButton rdoGu1;
        private System.Windows.Forms.RadioButton rdoGu0;
        private System.Windows.Forms.ComboBox cboGubun;
        private System.Windows.Forms.CheckBox chkOK;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panTitleSub;
    }
}