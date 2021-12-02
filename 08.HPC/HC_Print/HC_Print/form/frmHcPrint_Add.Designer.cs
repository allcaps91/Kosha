namespace HC_Print
{
    partial class frmHcPrint_Add
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
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType3 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panSub01 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.radioButton9 = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel16 = new System.Windows.Forms.Panel();
            this.panel21 = new System.Windows.Forms.Panel();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.lblLtdName = new System.Windows.Forms.Label();
            this.btnLtdHelp = new System.Windows.Forms.Button();
            this.txtLtdCode = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ChkGbRe = new System.Windows.Forms.CheckBox();
            this.panSub05 = new System.Windows.Forms.Panel();
            this.panel12 = new System.Windows.Forms.Panel();
            this.panel20 = new System.Windows.Forms.Panel();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.txtSname = new System.Windows.Forms.TextBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtWrtno = new System.Windows.Forms.TextBox();
            this.panSub02 = new System.Windows.Forms.Panel();
            this.dtpTDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFDate = new System.Windows.Forms.DateTimePicker();
            this.lblSub01 = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.SSList = new FarPoint.Win.Spread.FpSpread();
            this.SSList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnWebSearch = new System.Windows.Forms.Button();
            this.btnWebPrint = new System.Windows.Forms.Button();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.panel11 = new System.Windows.Forms.Panel();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnAllOk = new System.Windows.Forms.Button();
            this.ChkAll = new System.Windows.Forms.CheckBox();
            this.panTitle.SuspendLayout();
            this.panSub01.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel16.SuspendLayout();
            this.panel21.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panSub05.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel20.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.panel7.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panSub02.SuspendLayout();
            this.panel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.panel11.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1335, 39);
            this.panTitle.TabIndex = 143;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(1251, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(82, 37);
            this.btnExit.TabIndex = 21;
            this.btnExit.Text = "닫 기(&X)";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(11, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(160, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "회사추가검진 결과지";
            // 
            // panSub01
            // 
            this.panSub01.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panSub01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSub01.Controls.Add(this.panel10);
            this.panSub01.Controls.Add(this.panel11);
            this.panSub01.Controls.Add(this.panel8);
            this.panSub01.Controls.Add(this.panel6);
            this.panSub01.Controls.Add(this.panel5);
            this.panSub01.Controls.Add(this.panel1);
            this.panSub01.Controls.Add(this.panel4);
            this.panSub01.Controls.Add(this.panel3);
            this.panSub01.Controls.Add(this.panel2);
            this.panSub01.Controls.Add(this.panSub05);
            this.panSub01.Controls.Add(this.panSub02);
            this.panSub01.Controls.Add(this.lblSub01);
            this.panSub01.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSub01.Location = new System.Drawing.Point(0, 39);
            this.panSub01.Name = "panSub01";
            this.panSub01.Padding = new System.Windows.Forms.Padding(1);
            this.panSub01.Size = new System.Drawing.Size(1335, 115);
            this.panSub01.TabIndex = 144;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.panel4.Location = new System.Drawing.Point(864, 1);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(1);
            this.panel4.Size = new System.Drawing.Size(10, 111);
            this.panel4.TabIndex = 59;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.panel3.Location = new System.Drawing.Point(750, 1);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(1);
            this.panel3.Size = new System.Drawing.Size(114, 111);
            this.panel3.TabIndex = 58;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.radioButton8);
            this.groupBox4.Controls.Add(this.radioButton9);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox4.Location = new System.Drawing.Point(1, 1);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(109, 109);
            this.groupBox4.TabIndex = 61;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "매수";
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Location = new System.Drawing.Point(58, 24);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(46, 21);
            this.radioButton8.TabIndex = 2;
            this.radioButton8.TabStop = true;
            this.radioButton8.Text = "2장";
            this.radioButton8.UseVisualStyleBackColor = true;
            // 
            // radioButton9
            // 
            this.radioButton9.AutoSize = true;
            this.radioButton9.Checked = true;
            this.radioButton9.Location = new System.Drawing.Point(6, 24);
            this.radioButton9.Name = "radioButton9";
            this.radioButton9.Size = new System.Drawing.Size(46, 21);
            this.radioButton9.TabIndex = 1;
            this.radioButton9.TabStop = true;
            this.radioButton9.Text = "1장";
            this.radioButton9.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel16);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.panel2.Location = new System.Drawing.Point(499, 1);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(1);
            this.panel2.Size = new System.Drawing.Size(251, 111);
            this.panel2.TabIndex = 57;
            // 
            // panel16
            // 
            this.panel16.Controls.Add(this.panel21);
            this.panel16.Controls.Add(this.groupBox2);
            this.panel16.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel16.Location = new System.Drawing.Point(1, 1);
            this.panel16.Name = "panel16";
            this.panel16.Size = new System.Drawing.Size(228, 109);
            this.panel16.TabIndex = 11;
            // 
            // panel21
            // 
            this.panel21.Controls.Add(this.groupBox8);
            this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel21.Location = new System.Drawing.Point(0, 50);
            this.panel21.Name = "panel21";
            this.panel21.Size = new System.Drawing.Size(228, 53);
            this.panel21.TabIndex = 4;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.lblLtdName);
            this.groupBox8.Controls.Add(this.btnLtdHelp);
            this.groupBox8.Controls.Add(this.txtLtdCode);
            this.groupBox8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox8.Location = new System.Drawing.Point(0, 0);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(228, 53);
            this.groupBox8.TabIndex = 15;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "사업자명";
            // 
            // lblLtdName
            // 
            this.lblLtdName.BackColor = System.Drawing.Color.LightGray;
            this.lblLtdName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLtdName.Location = new System.Drawing.Point(87, 20);
            this.lblLtdName.Name = "lblLtdName";
            this.lblLtdName.Size = new System.Drawing.Size(137, 23);
            this.lblLtdName.TabIndex = 100;
            this.lblLtdName.Text = "현대제철";
            this.lblLtdName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnLtdHelp
            // 
            this.btnLtdHelp.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLtdHelp.Location = new System.Drawing.Point(56, 20);
            this.btnLtdHelp.Name = "btnLtdHelp";
            this.btnLtdHelp.Size = new System.Drawing.Size(25, 25);
            this.btnLtdHelp.TabIndex = 99;
            this.btnLtdHelp.Text = "&H";
            this.btnLtdHelp.UseVisualStyleBackColor = true;
            // 
            // txtLtdCode
            // 
            this.txtLtdCode.Location = new System.Drawing.Point(5, 20);
            this.txtLtdCode.Name = "txtLtdCode";
            this.txtLtdCode.Size = new System.Drawing.Size(49, 25);
            this.txtLtdCode.TabIndex = 98;
            this.txtLtdCode.Tag = "LTDCODE";
            this.txtLtdCode.Text = "01234";
            this.txtLtdCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ChkGbRe);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(228, 50);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "구분";
            // 
            // ChkGbRe
            // 
            this.ChkGbRe.AutoSize = true;
            this.ChkGbRe.BackColor = System.Drawing.Color.Transparent;
            this.ChkGbRe.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ChkGbRe.ForeColor = System.Drawing.Color.Black;
            this.ChkGbRe.Location = new System.Drawing.Point(6, 20);
            this.ChkGbRe.Name = "ChkGbRe";
            this.ChkGbRe.Size = new System.Drawing.Size(62, 19);
            this.ChkGbRe.TabIndex = 161;
            this.ChkGbRe.Text = "재발행";
            this.ChkGbRe.UseVisualStyleBackColor = false;
            // 
            // panSub05
            // 
            this.panSub05.Controls.Add(this.panel12);
            this.panSub05.Dock = System.Windows.Forms.DockStyle.Left;
            this.panSub05.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.panSub05.Location = new System.Drawing.Point(264, 1);
            this.panSub05.Name = "panSub05";
            this.panSub05.Padding = new System.Windows.Forms.Padding(1);
            this.panSub05.Size = new System.Drawing.Size(235, 111);
            this.panSub05.TabIndex = 55;
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.panel20);
            this.panel12.Controls.Add(this.panel7);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel12.Location = new System.Drawing.Point(1, 1);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(229, 109);
            this.panel12.TabIndex = 8;
            // 
            // panel20
            // 
            this.panel20.Controls.Add(this.groupBox7);
            this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel20.Location = new System.Drawing.Point(0, 53);
            this.panel20.Name = "panel20";
            this.panel20.Size = new System.Drawing.Size(229, 46);
            this.panel20.TabIndex = 1;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.txtSname);
            this.groupBox7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox7.Location = new System.Drawing.Point(0, 0);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(229, 46);
            this.groupBox7.TabIndex = 16;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "성명";
            // 
            // txtSname
            // 
            this.txtSname.Location = new System.Drawing.Point(6, 18);
            this.txtSname.Name = "txtSname";
            this.txtSname.Size = new System.Drawing.Size(216, 25);
            this.txtSname.TabIndex = 97;
            this.txtSname.Tag = "";
            this.txtSname.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.groupBox1);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(229, 53);
            this.panel7.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtWrtno);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(229, 53);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "접수번호";
            // 
            // txtWrtno
            // 
            this.txtWrtno.Location = new System.Drawing.Point(6, 18);
            this.txtWrtno.Name = "txtWrtno";
            this.txtWrtno.Size = new System.Drawing.Size(216, 25);
            this.txtWrtno.TabIndex = 98;
            this.txtWrtno.Tag = "";
            this.txtWrtno.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panSub02
            // 
            this.panSub02.Controls.Add(this.dtpTDate);
            this.panSub02.Controls.Add(this.dtpFDate);
            this.panSub02.Dock = System.Windows.Forms.DockStyle.Left;
            this.panSub02.Location = new System.Drawing.Point(64, 1);
            this.panSub02.Name = "panSub02";
            this.panSub02.Size = new System.Drawing.Size(200, 111);
            this.panSub02.TabIndex = 44;
            // 
            // dtpTDate
            // 
            this.dtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTDate.Location = new System.Drawing.Point(107, 19);
            this.dtpTDate.Name = "dtpTDate";
            this.dtpTDate.Size = new System.Drawing.Size(88, 25);
            this.dtpTDate.TabIndex = 1;
            // 
            // dtpFDate
            // 
            this.dtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFDate.Location = new System.Drawing.Point(3, 19);
            this.dtpFDate.Name = "dtpFDate";
            this.dtpFDate.Size = new System.Drawing.Size(88, 25);
            this.dtpFDate.TabIndex = 0;
            // 
            // lblSub01
            // 
            this.lblSub01.BackColor = System.Drawing.Color.LightBlue;
            this.lblSub01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSub01.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSub01.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSub01.Location = new System.Drawing.Point(1, 1);
            this.lblSub01.Name = "lblSub01";
            this.lblSub01.Size = new System.Drawing.Size(63, 111);
            this.lblSub01.TabIndex = 43;
            this.lblSub01.Text = "접수일자";
            this.lblSub01.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.ChkAll);
            this.panel9.Controls.Add(this.SSList);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(0, 154);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(1335, 588);
            this.panel9.TabIndex = 145;
            // 
            // SSList
            // 
            this.SSList.AccessibleDescription = "SSList, Sheet1, Row 0, Column 0, ";
            this.SSList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSList.Location = new System.Drawing.Point(0, 0);
            this.SSList.Name = "SSList";
            this.SSList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSList_Sheet1});
            this.SSList.Size = new System.Drawing.Size(1335, 588);
            this.SSList.TabIndex = 0;
            // 
            // SSList_Sheet1
            // 
            this.SSList_Sheet1.Reset();
            this.SSList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSList_Sheet1.ColumnCount = 11;
            this.SSList_Sheet1.RowCount = 1;
            this.SSList_Sheet1.Cells.Get(0, 0).TextIndent = 4;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "성명";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "검진종류";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "근무회사명";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "검진일자";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "주민번호";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "통보일자";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "접수번호";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "취급물질";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "웹결과지";
            this.SSList_Sheet1.Columns.Get(0).CellType = checkBoxCellType3;
            this.SSList_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(0).Width = 25F;
            this.SSList_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(1).Label = "성명";
            this.SSList_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(1).Width = 130F;
            this.SSList_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(2).Label = "검진종류";
            this.SSList_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(2).Width = 131F;
            this.SSList_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(3).Label = "근무회사명";
            this.SSList_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(3).Width = 226F;
            this.SSList_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(4).Label = "검진일자";
            this.SSList_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(4).Width = 140F;
            this.SSList_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(5).Label = "주민번호";
            this.SSList_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(5).Width = 154F;
            this.SSList_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(6).Label = "통보일자";
            this.SSList_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(6).Width = 140F;
            this.SSList_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(7).Label = "접수번호";
            this.SSList_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(7).Width = 107F;
            this.SSList_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(8).Label = "취급물질";
            this.SSList_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(8).Width = 218F;
            this.SSList_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(9).Visible = false;
            this.SSList_Sheet1.Columns.Get(9).Width = 19F;
            this.SSList_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(10).Label = "웹결과지";
            this.SSList_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(10).Visible = false;
            this.SSList_Sheet1.Columns.Get(10).Width = 106F;
            this.SSList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(874, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(106, 111);
            this.panel1.TabIndex = 60;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnSearch);
            this.groupBox3.Controls.Add(this.btnPrint);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(106, 111);
            this.groupBox3.TabIndex = 19;
            this.groupBox3.TabStop = false;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(6, 19);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(91, 34);
            this.btnSearch.TabIndex = 15;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(7, 59);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(91, 34);
            this.btnPrint.TabIndex = 14;
            this.btnPrint.Text = "인쇄";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel5.Location = new System.Drawing.Point(980, 1);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(10, 111);
            this.panel5.TabIndex = 61;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.groupBox5);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel6.Location = new System.Drawing.Point(990, 1);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(101, 111);
            this.panel6.TabIndex = 62;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnWebSearch);
            this.groupBox5.Controls.Add(this.btnWebPrint);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(0, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(101, 111);
            this.groupBox5.TabIndex = 13;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "웹결과지";
            // 
            // btnWebSearch
            // 
            this.btnWebSearch.Location = new System.Drawing.Point(6, 20);
            this.btnWebSearch.Name = "btnWebSearch";
            this.btnWebSearch.Size = new System.Drawing.Size(88, 34);
            this.btnWebSearch.TabIndex = 6;
            this.btnWebSearch.Text = "조회";
            this.btnWebSearch.UseVisualStyleBackColor = true;
            // 
            // btnWebPrint
            // 
            this.btnWebPrint.Location = new System.Drawing.Point(6, 59);
            this.btnWebPrint.Name = "btnWebPrint";
            this.btnWebPrint.Size = new System.Drawing.Size(88, 34);
            this.btnWebPrint.TabIndex = 5;
            this.btnWebPrint.Text = "전송";
            this.btnWebPrint.UseVisualStyleBackColor = true;
            // 
            // panel8
            // 
            this.panel8.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel8.Location = new System.Drawing.Point(1091, 1);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(10, 111);
            this.panel8.TabIndex = 63;
            // 
            // panel10
            // 
            this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel10.Location = new System.Drawing.Point(1202, 1);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(10, 111);
            this.panel10.TabIndex = 65;
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.groupBox6);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel11.Location = new System.Drawing.Point(1101, 1);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(101, 111);
            this.panel11.TabIndex = 64;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnAllOk);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(0, 0);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(101, 111);
            this.groupBox6.TabIndex = 13;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "일괄적용";
            // 
            // btnAllOk
            // 
            this.btnAllOk.Location = new System.Drawing.Point(6, 20);
            this.btnAllOk.Name = "btnAllOk";
            this.btnAllOk.Size = new System.Drawing.Size(88, 34);
            this.btnAllOk.TabIndex = 6;
            this.btnAllOk.Text = "일괄적용";
            this.btnAllOk.UseVisualStyleBackColor = true;
            // 
            // ChkAll
            // 
            this.ChkAll.AutoSize = true;
            this.ChkAll.BackColor = System.Drawing.Color.Transparent;
            this.ChkAll.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ChkAll.ForeColor = System.Drawing.Color.Black;
            this.ChkAll.Location = new System.Drawing.Point(40, 6);
            this.ChkAll.Name = "ChkAll";
            this.ChkAll.Size = new System.Drawing.Size(15, 14);
            this.ChkAll.TabIndex = 164;
            this.ChkAll.UseVisualStyleBackColor = false;
            // 
            // frmHcPrint_Add
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1335, 742);
            this.Controls.Add(this.panel9);
            this.Controls.Add(this.panSub01);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHcPrint_Add";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmHcPrint_Add";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panSub01.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel16.ResumeLayout(false);
            this.panel21.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panSub05.ResumeLayout(false);
            this.panel12.ResumeLayout(false);
            this.panel20.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panSub02.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.panel11.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panSub01;
        private System.Windows.Forms.Panel panSub05;
        private System.Windows.Forms.Panel panSub02;
        private System.Windows.Forms.DateTimePicker dtpTDate;
        private System.Windows.Forms.DateTimePicker dtpFDate;
        private System.Windows.Forms.Label lblSub01;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.RadioButton radioButton9;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel9;
        private FarPoint.Win.Spread.FpSpread SSList;
        private FarPoint.Win.Spread.SheetView SSList_Sheet1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel20;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox txtSname;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtWrtno;
        private System.Windows.Forms.Panel panel16;
        private System.Windows.Forms.Panel panel21;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label lblLtdName;
        private System.Windows.Forms.Button btnLtdHelp;
        private System.Windows.Forms.TextBox txtLtdCode;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox ChkGbRe;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnAllOk;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnWebSearch;
        private System.Windows.Forms.Button btnWebPrint;
        private System.Windows.Forms.CheckBox ChkAll;
    }
}