namespace ComLibB
{
    partial class FrmOrdSugaInfo
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlSuga = new System.Windows.Forms.Panel();
            this.lblOrdCode = new System.Windows.Forms.Label();
            this.txtSuCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblSugaF = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSuNamek = new System.Windows.Forms.TextBox();
            this.txtSuNameE = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.rtxtInfo = new System.Windows.Forms.RichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.fpSpread2 = new FarPoint.Win.Spread.FpSpread();
            this.sheetView1 = new FarPoint.Win.Spread.SheetView();
            this.fpSpread1 = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.fpSpread3 = new FarPoint.Win.Spread.FpSpread();
            this.sheetView2 = new FarPoint.Win.Spread.SheetView();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.panHelp = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnPanExit = new System.Windows.Forms.Button();
            this.fpSpread4 = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread4_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.pnlSuga.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sheetView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sheetView2)).BeginInit();
            this.panHelp.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread4_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(2, 3);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(74, 21);
            this.lblTitle.TabIndex = 5;
            this.lblTitle.Text = "수가정보";
            // 
            // pnlSuga
            // 
            this.pnlSuga.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSuga.Controls.Add(this.lblOrdCode);
            this.pnlSuga.Controls.Add(this.txtSuCode);
            this.pnlSuga.Controls.Add(this.label2);
            this.pnlSuga.Controls.Add(this.label1);
            this.pnlSuga.Location = new System.Drawing.Point(6, 36);
            this.pnlSuga.Name = "pnlSuga";
            this.pnlSuga.Size = new System.Drawing.Size(621, 45);
            this.pnlSuga.TabIndex = 8;
            // 
            // lblOrdCode
            // 
            this.lblOrdCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblOrdCode.Location = new System.Drawing.Point(276, 11);
            this.lblOrdCode.Name = "lblOrdCode";
            this.lblOrdCode.Size = new System.Drawing.Size(159, 22);
            this.lblOrdCode.TabIndex = 12;
            this.lblOrdCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtSuCode
            // 
            this.txtSuCode.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtSuCode.Location = new System.Drawing.Point(69, 12);
            this.txtSuCode.Name = "txtSuCode";
            this.txtSuCode.Size = new System.Drawing.Size(135, 21);
            this.txtSuCode.TabIndex = 11;
            this.txtSuCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSuCode_KeyPress);
            this.txtSuCode.Leave += new System.EventHandler(this.txtSuCode_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(217, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "오더코드";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "수가코드";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.LavenderBlush;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(6, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(699, 22);
            this.label4.TabIndex = 13;
            this.label4.Text = "환자 자격에 따라 부담율은 다를 수 있습니다.";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSugaF
            // 
            this.lblSugaF.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblSugaF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSugaF.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSugaF.Location = new System.Drawing.Point(6, 111);
            this.lblSugaF.Name = "lblSugaF";
            this.lblSugaF.Size = new System.Drawing.Size(699, 59);
            this.lblSugaF.TabIndex = 14;
            this.lblSugaF.Text = "TEST";
            this.lblSugaF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 180);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 15;
            this.label6.Text = "한글명칭";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 207);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 16;
            this.label7.Text = "영문명칭";
            // 
            // txtSuNamek
            // 
            this.txtSuNamek.Location = new System.Drawing.Point(63, 176);
            this.txtSuNamek.Name = "txtSuNamek";
            this.txtSuNamek.Size = new System.Drawing.Size(642, 21);
            this.txtSuNamek.TabIndex = 17;
            // 
            // txtSuNameE
            // 
            this.txtSuNameE.Location = new System.Drawing.Point(63, 203);
            this.txtSuNameE.Name = "txtSuNameE";
            this.txtSuNameE.Size = new System.Drawing.Size(642, 21);
            this.txtSuNameE.TabIndex = 18;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(7, 239);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 18);
            this.label8.TabIndex = 19;
            this.label8.Text = "심사정보";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rtxtInfo
            // 
            this.rtxtInfo.Location = new System.Drawing.Point(6, 256);
            this.rtxtInfo.Name = "rtxtInfo";
            this.rtxtInfo.Size = new System.Drawing.Size(699, 369);
            this.rtxtInfo.TabIndex = 20;
            this.rtxtInfo.Text = "";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.fpSpread2);
            this.panel2.Controls.Add(this.fpSpread1);
            this.panel2.Location = new System.Drawing.Point(7, 658);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1095, 331);
            this.panel2.TabIndex = 23;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.RoyalBlue;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(4, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(1080, 26);
            this.label3.TabIndex = 25;
            this.label3.Text = "PanSugaMsg";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fpSpread2
            // 
            this.fpSpread2.AccessibleDescription = "";
            this.fpSpread2.Location = new System.Drawing.Point(3, 182);
            this.fpSpread2.Name = "fpSpread2";
            this.fpSpread2.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.sheetView1});
            this.fpSpread2.Size = new System.Drawing.Size(1081, 142);
            this.fpSpread2.TabIndex = 24;
            // 
            // sheetView1
            // 
            this.sheetView1.Reset();
            this.sheetView1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.sheetView1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.sheetView1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.sheetView1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.sheetView1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.sheetView1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.sheetView1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.sheetView1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.sheetView1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.sheetView1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.sheetView1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // fpSpread1
            // 
            this.fpSpread1.AccessibleDescription = "";
            this.fpSpread1.Location = new System.Drawing.Point(5, 5);
            this.fpSpread1.Name = "fpSpread1";
            this.fpSpread1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread1_Sheet1});
            this.fpSpread1.Size = new System.Drawing.Size(1081, 142);
            this.fpSpread1.TabIndex = 23;
            // 
            // fpSpread1_Sheet1
            // 
            this.fpSpread1_Sheet1.Reset();
            this.fpSpread1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpread1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpSpread1_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.fpSpread1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.fpSpread1_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.fpSpread1_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.fpSpread1_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread1_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.fpSpread1_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.fpSpread1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // fpSpread3
            // 
            this.fpSpread3.AccessibleDescription = "";
            this.fpSpread3.Location = new System.Drawing.Point(7, 881);
            this.fpSpread3.Name = "fpSpread3";
            this.fpSpread3.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.sheetView2});
            this.fpSpread3.Size = new System.Drawing.Size(517, 101);
            this.fpSpread3.TabIndex = 25;
            // 
            // sheetView2
            // 
            this.sheetView2.Reset();
            this.sheetView2.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.sheetView2.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.sheetView2.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.sheetView2.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.sheetView2.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.sheetView2.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.sheetView2.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.sheetView2.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.sheetView2.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.sheetView2.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.sheetView2.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(530, 896);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 26;
            this.label5.Text = "분류";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(530, 921);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 27;
            this.label9.Text = "누적";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(569, 892);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(41, 21);
            this.textBox1.TabIndex = 28;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(569, 918);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(41, 21);
            this.textBox2.TabIndex = 29;
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Location = new System.Drawing.Point(614, 892);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(118, 21);
            this.label10.TabIndex = 30;
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Location = new System.Drawing.Point(614, 919);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(118, 21);
            this.label11.TabIndex = 31;
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(633, 35);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 47);
            this.btnExit.TabIndex = 32;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHelp.BackColor = System.Drawing.Color.White;
            this.btnHelp.Location = new System.Drawing.Point(524, 0);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(181, 35);
            this.btnHelp.TabIndex = 33;
            this.btnHelp.Text = "CT, MRI 보험 본인부담율";
            this.btnHelp.UseVisualStyleBackColor = false;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // panHelp
            // 
            this.panHelp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panHelp.Controls.Add(this.fpSpread4);
            this.panHelp.Controls.Add(this.panel3);
            this.panHelp.Location = new System.Drawing.Point(414, 207);
            this.panHelp.Name = "panHelp";
            this.panHelp.Size = new System.Drawing.Size(284, 406);
            this.panHelp.TabIndex = 34;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnPanExit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(282, 41);
            this.panel3.TabIndex = 0;
            // 
            // btnPanExit
            // 
            this.btnPanExit.BackColor = System.Drawing.Color.White;
            this.btnPanExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPanExit.Location = new System.Drawing.Point(210, 0);
            this.btnPanExit.Name = "btnPanExit";
            this.btnPanExit.Size = new System.Drawing.Size(72, 41);
            this.btnPanExit.TabIndex = 33;
            this.btnPanExit.Text = "닫기";
            this.btnPanExit.UseVisualStyleBackColor = false;
            this.btnPanExit.Click += new System.EventHandler(this.btnPanExit_Click);
            // 
            // fpSpread4
            // 
            this.fpSpread4.AccessibleDescription = "fpSpread4, Sheet1, Row 0, Column 0, CT, MRI, 보험 본인부담율";
            this.fpSpread4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpread4.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.fpSpread4.Location = new System.Drawing.Point(0, 41);
            this.fpSpread4.Name = "fpSpread4";
            this.fpSpread4.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread4_Sheet1});
            this.fpSpread4.Size = new System.Drawing.Size(282, 363);
            this.fpSpread4.TabIndex = 1;
            this.fpSpread4.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            // 
            // fpSpread4_Sheet1
            // 
            this.fpSpread4_Sheet1.Reset();
            this.fpSpread4_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpread4_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpSpread4_Sheet1.ColumnCount = 3;
            this.fpSpread4_Sheet1.RowCount = 12;
            this.fpSpread4_Sheet1.Cells.Get(0, 0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.fpSpread4_Sheet1.Cells.Get(0, 0).Font = new System.Drawing.Font("굴림", 10F);
            this.fpSpread4_Sheet1.Cells.Get(0, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Justify;
            this.fpSpread4_Sheet1.Cells.Get(0, 0).Value = "CT, MRI, 보험 본인부담율";
            this.fpSpread4_Sheet1.Cells.Get(0, 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.fpSpread4_Sheet1.Cells.Get(0, 1).Font = new System.Drawing.Font("굴림", 10F);
            this.fpSpread4_Sheet1.Cells.Get(0, 1).Value = "입원";
            this.fpSpread4_Sheet1.Cells.Get(0, 2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.fpSpread4_Sheet1.Cells.Get(0, 2).Font = new System.Drawing.Font("굴림", 10F);
            this.fpSpread4_Sheet1.Cells.Get(0, 2).Value = "외래";
            this.fpSpread4_Sheet1.Cells.Get(1, 0).Value = "건강보험";
            this.fpSpread4_Sheet1.Cells.Get(1, 1).Value = "50%";
            this.fpSpread4_Sheet1.Cells.Get(1, 2).Value = "50%";
            this.fpSpread4_Sheet1.Cells.Get(2, 0).Value = "차상위 E";
            this.fpSpread4_Sheet1.Cells.Get(2, 1).Value = "14%";
            this.fpSpread4_Sheet1.Cells.Get(2, 2).Value = "14%";
            this.fpSpread4_Sheet1.Cells.Get(3, 0).Value = "차상위 C, F";
            this.fpSpread4_Sheet1.Cells.Get(3, 1).Value = "0%";
            this.fpSpread4_Sheet1.Cells.Get(3, 2).Value = "0%";
            this.fpSpread4_Sheet1.Cells.Get(4, 0).Value = "암";
            this.fpSpread4_Sheet1.Cells.Get(4, 1).Value = "5%";
            this.fpSpread4_Sheet1.Cells.Get(4, 2).Value = "5%";
            this.fpSpread4_Sheet1.Cells.Get(5, 0).Value = "산정특례";
            this.fpSpread4_Sheet1.Cells.Get(5, 1).Value = "10%";
            this.fpSpread4_Sheet1.Cells.Get(5, 2).Value = "10%";
            this.fpSpread4_Sheet1.Cells.Get(6, 0).Value = "소아 15세이하";
            this.fpSpread4_Sheet1.Cells.Get(6, 1).Value = "5%";
            this.fpSpread4_Sheet1.Cells.Get(7, 0).Value = "소아 1세미만";
            this.fpSpread4_Sheet1.Cells.Get(7, 2).Value = "15%";
            this.fpSpread4_Sheet1.Cells.Get(8, 0).Value = "소아 1~6세미만";
            this.fpSpread4_Sheet1.Cells.Get(8, 2).Value = "35%";
            this.fpSpread4_Sheet1.Cells.Get(9, 0).Value = "소아 6세이상";
            this.fpSpread4_Sheet1.Cells.Get(9, 2).Value = "50%";
            this.fpSpread4_Sheet1.Cells.Get(10, 0).Value = "의료급여 1종";
            this.fpSpread4_Sheet1.Cells.Get(10, 1).Value = "0%";
            this.fpSpread4_Sheet1.Cells.Get(10, 2).Value = "5%";
            this.fpSpread4_Sheet1.Cells.Get(11, 0).Value = "의료급여 2종";
            this.fpSpread4_Sheet1.Cells.Get(11, 1).Value = "10%";
            this.fpSpread4_Sheet1.Cells.Get(11, 2).Value = "15%";
            this.fpSpread4_Sheet1.ColumnHeader.Visible = false;
            this.fpSpread4_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.fpSpread4_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpSpread4_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpSpread4_Sheet1.Columns.Get(0).Width = 159F;
            this.fpSpread4_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.fpSpread4_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpSpread4_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpSpread4_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.fpSpread4_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpSpread4_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpSpread4_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
            this.fpSpread4_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpSpread4_Sheet1.RowHeader.Visible = false;
            this.fpSpread4_Sheet1.Rows.Get(0).Height = 30F;
            this.fpSpread4_Sheet1.Rows.Get(1).Height = 30F;
            this.fpSpread4_Sheet1.Rows.Get(2).Height = 30F;
            this.fpSpread4_Sheet1.Rows.Get(3).Height = 30F;
            this.fpSpread4_Sheet1.Rows.Get(4).Height = 30F;
            this.fpSpread4_Sheet1.Rows.Get(5).Height = 30F;
            this.fpSpread4_Sheet1.Rows.Get(6).Height = 30F;
            this.fpSpread4_Sheet1.Rows.Get(7).Height = 30F;
            this.fpSpread4_Sheet1.Rows.Get(8).Height = 30F;
            this.fpSpread4_Sheet1.Rows.Get(9).Height = 30F;
            this.fpSpread4_Sheet1.Rows.Get(10).Height = 30F;
            this.fpSpread4_Sheet1.Rows.Get(11).Height = 30F;
            this.fpSpread4_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // FrmOrdSugaInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(710, 631);
            this.Controls.Add(this.panHelp);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.fpSpread3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.rtxtInfo);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtSuNameE);
            this.Controls.Add(this.txtSuNamek);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblSugaF);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pnlSuga);
            this.Controls.Add(this.lblTitle);
            this.Name = "FrmOrdSugaInfo";
            this.Text = "수가정보";
            this.Load += new System.EventHandler(this.FrmOrdSugaInfo_Load);
            this.pnlSuga.ResumeLayout(false);
            this.pnlSuga.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sheetView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sheetView2)).EndInit();
            this.panHelp.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread4_Sheet1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlSuga;
        private System.Windows.Forms.Label lblOrdCode;
        private System.Windows.Forms.TextBox txtSuCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblSugaF;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSuNamek;
        private System.Windows.Forms.TextBox txtSuNameE;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RichTextBox rtxtInfo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private FarPoint.Win.Spread.FpSpread fpSpread2;
        private FarPoint.Win.Spread.SheetView sheetView1;
        private FarPoint.Win.Spread.FpSpread fpSpread1;
        private FarPoint.Win.Spread.SheetView fpSpread1_Sheet1;
        private FarPoint.Win.Spread.FpSpread fpSpread3;
        private FarPoint.Win.Spread.SheetView sheetView2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Panel panHelp;
        private FarPoint.Win.Spread.FpSpread fpSpread4;
        private FarPoint.Win.Spread.SheetView fpSpread4_Sheet1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnPanExit;
    }
}