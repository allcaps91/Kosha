namespace ComMirLibB.Com
{
    partial class frmComMirDtlCopy
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color410636651723553214350", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text482636651723553234399", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static586636651723553254453");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Static1000636651723553485068");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("Static1072636651723553505117");
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.abcd = new System.Windows.Forms.Panel();
            this.abcd1 = new System.Windows.Forms.Label();
            this.panOpt = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.optGB2 = new System.Windows.Forms.RadioButton();
            this.optGB1 = new System.Windows.Forms.RadioButton();
            this.optGB0 = new System.Windows.Forms.RadioButton();
            this.grbIO = new System.Windows.Forms.GroupBox();
            this.lblName = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblPano = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ss1 = new FarPoint.Win.Spread.FpSpread();
            this.ss1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ssList = new FarPoint.Win.Spread.FpSpread();
            this.ssList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.abcd.SuspendLayout();
            this.panOpt.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grbIO.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // abcd
            // 
            this.abcd.BackColor = System.Drawing.Color.RoyalBlue;
            this.abcd.Controls.Add(this.abcd1);
            this.abcd.Dock = System.Windows.Forms.DockStyle.Top;
            this.abcd.ForeColor = System.Drawing.Color.White;
            this.abcd.Location = new System.Drawing.Point(0, 0);
            this.abcd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.abcd.Name = "abcd";
            this.abcd.Padding = new System.Windows.Forms.Padding(1);
            this.abcd.Size = new System.Drawing.Size(1346, 38);
            this.abcd.TabIndex = 97;
            // 
            // abcd1
            // 
            this.abcd1.AutoSize = true;
            this.abcd1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.abcd1.Location = new System.Drawing.Point(6, 4);
            this.abcd1.Name = "abcd1";
            this.abcd1.Size = new System.Drawing.Size(172, 21);
            this.abcd1.TabIndex = 4;
            this.abcd1.Text = "청구내역 조회 및 복사";
            // 
            // panOpt
            // 
            this.panOpt.BackColor = System.Drawing.Color.White;
            this.panOpt.Controls.Add(this.groupBox1);
            this.panOpt.Controls.Add(this.grbIO);
            this.panOpt.Controls.Add(this.groupBox3);
            this.panOpt.Dock = System.Windows.Forms.DockStyle.Top;
            this.panOpt.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.panOpt.Location = new System.Drawing.Point(0, 38);
            this.panOpt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panOpt.Name = "panOpt";
            this.panOpt.Padding = new System.Windows.Forms.Padding(1);
            this.panOpt.Size = new System.Drawing.Size(1346, 60);
            this.panOpt.TabIndex = 98;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.optGB2);
            this.groupBox1.Controls.Add(this.optGB1);
            this.groupBox1.Controls.Add(this.optGB0);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(250, 1);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(231, 58);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "기간선정";
            // 
            // optGB2
            // 
            this.optGB2.AutoSize = true;
            this.optGB2.Dock = System.Windows.Forms.DockStyle.Left;
            this.optGB2.Location = new System.Drawing.Point(165, 20);
            this.optGB2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.optGB2.Name = "optGB2";
            this.optGB2.Size = new System.Drawing.Size(55, 36);
            this.optGB2.TabIndex = 2;
            this.optGB2.Text = "전체";
            this.optGB2.UseVisualStyleBackColor = true;
            // 
            // optGB1
            // 
            this.optGB1.AutoSize = true;
            this.optGB1.Dock = System.Windows.Forms.DockStyle.Left;
            this.optGB1.Location = new System.Drawing.Point(80, 20);
            this.optGB1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.optGB1.Name = "optGB1";
            this.optGB1.Size = new System.Drawing.Size(85, 36);
            this.optGB1.TabIndex = 1;
            this.optGB1.Text = "12개월전";
            this.optGB1.UseVisualStyleBackColor = true;
            // 
            // optGB0
            // 
            this.optGB0.AutoSize = true;
            this.optGB0.Checked = true;
            this.optGB0.Dock = System.Windows.Forms.DockStyle.Left;
            this.optGB0.Location = new System.Drawing.Point(3, 20);
            this.optGB0.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.optGB0.Name = "optGB0";
            this.optGB0.Size = new System.Drawing.Size(77, 36);
            this.optGB0.TabIndex = 0;
            this.optGB0.TabStop = true;
            this.optGB0.Text = "6개월전";
            this.optGB0.UseVisualStyleBackColor = true;
            // 
            // grbIO
            // 
            this.grbIO.BackColor = System.Drawing.SystemColors.Control;
            this.grbIO.Controls.Add(this.lblName);
            this.grbIO.Dock = System.Windows.Forms.DockStyle.Left;
            this.grbIO.Location = new System.Drawing.Point(136, 1);
            this.grbIO.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grbIO.Name = "grbIO";
            this.grbIO.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grbIO.Size = new System.Drawing.Size(114, 58);
            this.grbIO.TabIndex = 20;
            this.grbIO.TabStop = false;
            this.grbIO.Text = "성명";
            // 
            // lblName
            // 
            this.lblName.BackColor = System.Drawing.SystemColors.Control;
            this.lblName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblName.ForeColor = System.Drawing.Color.Blue;
            this.lblName.Location = new System.Drawing.Point(3, 20);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(108, 36);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "홍길동애기";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox3.Controls.Add(this.lblPano);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox3.Location = new System.Drawing.Point(1, 1);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Size = new System.Drawing.Size(135, 58);
            this.groupBox3.TabIndex = 23;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "등록번호";
            // 
            // lblPano
            // 
            this.lblPano.BackColor = System.Drawing.SystemColors.Control;
            this.lblPano.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPano.ForeColor = System.Drawing.Color.Blue;
            this.lblPano.Location = new System.Drawing.Point(3, 20);
            this.lblPano.Name = "lblPano";
            this.lblPano.Size = new System.Drawing.Size(129, 36);
            this.lblPano.TabIndex = 2;
            this.lblPano.Text = "12345678";
            this.lblPano.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnExit.Location = new System.Drawing.Point(20, 95);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(96, 39);
            this.btnExit.TabIndex = 25;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(20, 39);
            this.btnCopy.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(96, 43);
            this.btnCopy.TabIndex = 17;
            this.btnCopy.Text = "복사";
            this.btnCopy.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ss1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 98);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1346, 175);
            this.panel1.TabIndex = 99;
            // 
            // ss1
            // 
            this.ss1.AccessibleDescription = "ss1, Sheet1, Row 0, Column 0, ";
            this.ss1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ss1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ss1.Location = new System.Drawing.Point(0, 0);
            this.ss1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ss1.Name = "ss1";
            namedStyle1.Font = new System.Drawing.Font("굴림체", 11F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType1.MaxLength = 32000;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("굴림체", 11F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType2.Static = true;
            namedStyle3.CellType = textCellType2;
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.Renderer = textCellType2;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType3.Static = true;
            namedStyle4.CellType = textCellType3;
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.Renderer = textCellType3;
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType4.Static = true;
            namedStyle5.CellType = textCellType4;
            namedStyle5.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            namedStyle5.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle5.Renderer = textCellType4;
            namedStyle5.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4,
            namedStyle5});
            this.ss1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ss1_Sheet1});
            this.ss1.Size = new System.Drawing.Size(1209, 175);
            this.ss1.TabIndex = 1;
            this.ss1.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ss1.TextTipAppearance = tipAppearance1;
            this.ss1.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // ss1_Sheet1
            // 
            this.ss1_Sheet1.Reset();
            this.ss1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ss1_Sheet1.ColumnCount = 17;
            this.ss1_Sheet1.RowCount = 8;
            this.ss1_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "EDI여부";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "보류";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "청구구분";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "일련번호";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "상해외인";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "변경";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "청구번호";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "청구분야";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "진료과";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "진료개시일";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "본인부담";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "조합코드";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "증번호";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = " ";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "총진료비";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 15).Value = "조합부담금";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 16).Value = "본인부담금";
            this.ss1_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.ColumnHeader.Rows.Get(0).Height = 35F;
            this.ss1_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.Columns.Get(0).Label = "EDI여부";
            this.ss1_Sheet1.Columns.Get(0).StyleName = "Static586636651723553254453";
            this.ss1_Sheet1.Columns.Get(0).Width = 43F;
            this.ss1_Sheet1.Columns.Get(1).Label = "보류";
            this.ss1_Sheet1.Columns.Get(1).StyleName = "Static586636651723553254453";
            this.ss1_Sheet1.Columns.Get(1).Width = 31F;
            this.ss1_Sheet1.Columns.Get(2).Label = "청구구분";
            this.ss1_Sheet1.Columns.Get(2).StyleName = "Static586636651723553254453";
            this.ss1_Sheet1.Columns.Get(2).Width = 38F;
            this.ss1_Sheet1.Columns.Get(3).Label = "일련번호";
            this.ss1_Sheet1.Columns.Get(3).StyleName = "Static586636651723553254453";
            this.ss1_Sheet1.Columns.Get(3).Width = 70F;
            this.ss1_Sheet1.Columns.Get(4).Label = "상해외인";
            this.ss1_Sheet1.Columns.Get(4).StyleName = "Static586636651723553254453";
            this.ss1_Sheet1.Columns.Get(4).Width = 44F;
            this.ss1_Sheet1.Columns.Get(5).Label = "변경";
            this.ss1_Sheet1.Columns.Get(5).StyleName = "Static586636651723553254453";
            this.ss1_Sheet1.Columns.Get(5).Width = 29F;
            this.ss1_Sheet1.Columns.Get(6).Label = "청구번호";
            this.ss1_Sheet1.Columns.Get(6).Width = 75F;
            this.ss1_Sheet1.Columns.Get(7).Label = "청구분야";
            this.ss1_Sheet1.Columns.Get(7).StyleName = "Static586636651723553254453";
            this.ss1_Sheet1.Columns.Get(7).Width = 78F;
            this.ss1_Sheet1.Columns.Get(8).Label = "진료과";
            this.ss1_Sheet1.Columns.Get(8).StyleName = "Static586636651723553254453";
            this.ss1_Sheet1.Columns.Get(8).Width = 103F;
            this.ss1_Sheet1.Columns.Get(9).Label = "진료개시일";
            this.ss1_Sheet1.Columns.Get(9).StyleName = "Static586636651723553254453";
            this.ss1_Sheet1.Columns.Get(9).Width = 86F;
            this.ss1_Sheet1.Columns.Get(10).Label = "본인부담";
            this.ss1_Sheet1.Columns.Get(10).StyleName = "Static586636651723553254453";
            this.ss1_Sheet1.Columns.Get(10).Width = 45F;
            this.ss1_Sheet1.Columns.Get(11).Label = "조합코드";
            this.ss1_Sheet1.Columns.Get(11).StyleName = "Static586636651723553254453";
            this.ss1_Sheet1.Columns.Get(11).Width = 40F;
            this.ss1_Sheet1.Columns.Get(12).Label = "증번호";
            this.ss1_Sheet1.Columns.Get(12).StyleName = "Static1000636651723553485068";
            this.ss1_Sheet1.Columns.Get(12).Width = 106F;
            this.ss1_Sheet1.Columns.Get(13).Label = " ";
            this.ss1_Sheet1.Columns.Get(13).StyleName = "Static1000636651723553485068";
            this.ss1_Sheet1.Columns.Get(13).Width = 15F;
            this.ss1_Sheet1.Columns.Get(14).Label = "총진료비";
            this.ss1_Sheet1.Columns.Get(14).StyleName = "Static1072636651723553505117";
            this.ss1_Sheet1.Columns.Get(14).Width = 116F;
            this.ss1_Sheet1.Columns.Get(15).Label = "조합부담금";
            this.ss1_Sheet1.Columns.Get(15).StyleName = "Static1072636651723553505117";
            this.ss1_Sheet1.Columns.Get(15).Width = 116F;
            this.ss1_Sheet1.Columns.Get(16).Label = "본인부담금";
            this.ss1_Sheet1.Columns.Get(16).StyleName = "Static1072636651723553505117";
            this.ss1_Sheet1.Columns.Get(16).Width = 116F;
            this.ss1_Sheet1.DefaultStyleName = "Text482636651723553234399";
            this.ss1_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            this.ss1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ss1_Sheet1.Rows.Get(0).Height = 21F;
            this.ss1_Sheet1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ss1_Sheet1.SelectionStyle = FarPoint.Win.Spread.SelectionStyles.SelectionColors;
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnExit);
            this.panel2.Controls.Add(this.btnCopy);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(1209, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(137, 175);
            this.panel2.TabIndex = 0;
            // 
            // ssList
            // 
            this.ssList.AccessibleDescription = "";
            this.ssList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssList.Location = new System.Drawing.Point(0, 273);
            this.ssList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ssList.Name = "ssList";
            this.ssList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList_Sheet1});
            this.ssList.Size = new System.Drawing.Size(1346, 451);
            this.ssList.TabIndex = 100;
            // 
            // ssList_Sheet1
            // 
            this.ssList_Sheet1.Reset();
            this.ssList_Sheet1.SheetName = "Sheet1";
            // 
            // frmComMirDtlCopy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1346, 724);
            this.Controls.Add(this.ssList);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panOpt);
            this.Controls.Add(this.abcd);
            this.Font = new System.Drawing.Font("굴림", 9F);
            this.Name = "frmComMirDtlCopy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmComMirDtlCopy";
            this.abcd.ResumeLayout(false);
            this.abcd.PerformLayout();
            this.panOpt.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grbIO.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel abcd;
        private System.Windows.Forms.Label abcd1;
        private System.Windows.Forms.Panel panOpt;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.GroupBox grbIO;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel panel1;
        private FarPoint.Win.Spread.FpSpread ss1;
        private FarPoint.Win.Spread.SheetView ss1_Sheet1;
        private System.Windows.Forms.Panel panel2;
        private FarPoint.Win.Spread.FpSpread ssList;
        private FarPoint.Win.Spread.SheetView ssList_Sheet1;
        private System.Windows.Forms.RadioButton optGB2;
        private System.Windows.Forms.RadioButton optGB1;
        private System.Windows.Forms.RadioButton optGB0;
        private System.Windows.Forms.Label lblPano;
    }
}