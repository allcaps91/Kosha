namespace HC_OSHA.StatusReport
{
    partial class WorkerHealthCheckMacroForm
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
            this.contentTitle1 = new ComBase.Mvc.UserControls.ContentTitle();
            this.PanMacroword11 = new System.Windows.Forms.Panel();
            this.PanMacroword = new System.Windows.Forms.Panel();
            this.NumDispSeq = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtTitle = new System.Windows.Forms.TextBox();
            this.TxtContent = new System.Windows.Forms.TextBox();
            this.SSList = new FarPoint.Win.Spread.FpSpread();
            this.SSList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.RdoMacroType2 = new System.Windows.Forms.RadioButton();
            this.RdoMacroType1 = new System.Windows.Forms.RadioButton();
            this.RdoMacroType0 = new System.Windows.Forms.RadioButton();
            this.BtnNew = new System.Windows.Forms.Button();
            this.BtnSave = new System.Windows.Forms.Button();
            this.BtnDelete = new System.Windows.Forms.Button();
            this.PanMacroword22 = new System.Windows.Forms.Panel();
            this.PanMacroword2 = new System.Windows.Forms.Panel();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.TxtSugesstion = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.SSList2 = new FarPoint.Win.Spread.FpSpread();
            this.SSList2_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.RdoMacro2Type2 = new System.Windows.Forms.RadioButton();
            this.RdoMacro2Type1 = new System.Windows.Forms.RadioButton();
            this.RdoMacro2Type0 = new System.Windows.Forms.RadioButton();
            this.BtnNew2 = new System.Windows.Forms.Button();
            this.BtnSave2 = new System.Windows.Forms.Button();
            this.BtnDelete2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.PanMacroword11.SuspendLayout();
            this.PanMacroword.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumDispSeq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).BeginInit();
            this.panel1.SuspendLayout();
            this.PanMacroword22.SuspendLayout();
            this.PanMacroword2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList2_Sheet1)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentTitle1
            // 
            this.contentTitle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.contentTitle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.contentTitle1.Location = new System.Drawing.Point(0, 0);
            this.contentTitle1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.contentTitle1.Name = "contentTitle1";
            this.contentTitle1.Size = new System.Drawing.Size(1315, 38);
            this.contentTitle1.TabIndex = 0;
            this.contentTitle1.TitleText = "근로자 건강관리 상용구";
            // 
            // PanMacroword11
            // 
            this.PanMacroword11.BackColor = System.Drawing.SystemColors.Window;
            this.PanMacroword11.Controls.Add(this.PanMacroword);
            this.PanMacroword11.Controls.Add(this.SSList);
            this.PanMacroword11.Controls.Add(this.panel1);
            this.PanMacroword11.Dock = System.Windows.Forms.DockStyle.Left;
            this.PanMacroword11.Location = new System.Drawing.Point(0, 38);
            this.PanMacroword11.Name = "PanMacroword11";
            this.PanMacroword11.Size = new System.Drawing.Size(592, 493);
            this.PanMacroword11.TabIndex = 1;
            this.PanMacroword11.Paint += new System.Windows.Forms.PaintEventHandler(this.PanMacroword_Paint);
            // 
            // PanMacroword
            // 
            this.PanMacroword.Controls.Add(this.NumDispSeq);
            this.PanMacroword.Controls.Add(this.label2);
            this.PanMacroword.Controls.Add(this.label1);
            this.PanMacroword.Controls.Add(this.label3);
            this.PanMacroword.Controls.Add(this.TxtTitle);
            this.PanMacroword.Controls.Add(this.TxtContent);
            this.PanMacroword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanMacroword.Location = new System.Drawing.Point(254, 49);
            this.PanMacroword.Name = "PanMacroword";
            this.PanMacroword.Size = new System.Drawing.Size(338, 444);
            this.PanMacroword.TabIndex = 140;
            // 
            // NumDispSeq
            // 
            this.NumDispSeq.Location = new System.Drawing.Point(120, 43);
            this.NumDispSeq.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.NumDispSeq.Name = "NumDispSeq";
            this.NumDispSeq.Size = new System.Drawing.Size(184, 25);
            this.NumDispSeq.TabIndex = 149;
            this.NumDispSeq.Tag = "DISPSEQ";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(26, 42);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label2.Size = new System.Drawing.Size(88, 25);
            this.label2.TabIndex = 148;
            this.label2.Text = "표시순서";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(26, 12);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label1.Size = new System.Drawing.Size(88, 25);
            this.label1.TabIndex = 146;
            this.label1.Text = "상용구 제목";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(26, 79);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label3.Size = new System.Drawing.Size(278, 25);
            this.label3.TabIndex = 144;
            this.label3.Text = "상담(지도)내용";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TxtTitle
            // 
            this.TxtTitle.Location = new System.Drawing.Point(120, 12);
            this.TxtTitle.Name = "TxtTitle";
            this.TxtTitle.Size = new System.Drawing.Size(184, 25);
            this.TxtTitle.TabIndex = 135;
            this.TxtTitle.Tag = "TITLE";
            // 
            // TxtContent
            // 
            this.TxtContent.Location = new System.Drawing.Point(26, 107);
            this.TxtContent.Multiline = true;
            this.TxtContent.Name = "TxtContent";
            this.TxtContent.Size = new System.Drawing.Size(278, 125);
            this.TxtContent.TabIndex = 132;
            this.TxtContent.Tag = "CONTENT";
            // 
            // SSList
            // 
            this.SSList.AccessibleDescription = "";
            this.SSList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSList.Dock = System.Windows.Forms.DockStyle.Left;
            this.SSList.Location = new System.Drawing.Point(0, 49);
            this.SSList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SSList.Name = "SSList";
            this.SSList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSList_Sheet1});
            this.SSList.Size = new System.Drawing.Size(254, 444);
            this.SSList.TabIndex = 134;
            this.SSList.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSList_CellClick);
            this.SSList.SetActiveViewport(0, -1, -1);
            // 
            // SSList_Sheet1
            // 
            this.SSList_Sheet1.Reset();
            this.SSList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSList_Sheet1.ColumnCount = 0;
            this.SSList_Sheet1.RowCount = 0;
            this.SSList_Sheet1.ActiveColumnIndex = -1;
            this.SSList_Sheet1.ActiveRowIndex = -1;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.RdoMacroType2);
            this.panel1.Controls.Add(this.RdoMacroType1);
            this.panel1.Controls.Add(this.RdoMacroType0);
            this.panel1.Controls.Add(this.BtnNew);
            this.panel1.Controls.Add(this.BtnSave);
            this.panel1.Controls.Add(this.BtnDelete);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(592, 49);
            this.panel1.TabIndex = 139;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel1_Paint);
            // 
            // RdoMacroType2
            // 
            this.RdoMacroType2.AutoSize = true;
            this.RdoMacroType2.Location = new System.Drawing.Point(149, 15);
            this.RdoMacroType2.Name = "RdoMacroType2";
            this.RdoMacroType2.Size = new System.Drawing.Size(65, 21);
            this.RdoMacroType2.TabIndex = 140;
            this.RdoMacroType2.TabStop = true;
            this.RdoMacroType2.Text = "간호사";
            this.RdoMacroType2.UseVisualStyleBackColor = true;
            this.RdoMacroType2.CheckedChanged += new System.EventHandler(this.RdoMacroType0_CheckedChanged);
            // 
            // RdoMacroType1
            // 
            this.RdoMacroType1.AutoSize = true;
            this.RdoMacroType1.Location = new System.Drawing.Point(82, 15);
            this.RdoMacroType1.Name = "RdoMacroType1";
            this.RdoMacroType1.Size = new System.Drawing.Size(52, 21);
            this.RdoMacroType1.TabIndex = 139;
            this.RdoMacroType1.TabStop = true;
            this.RdoMacroType1.Text = "의사";
            this.RdoMacroType1.UseVisualStyleBackColor = true;
            this.RdoMacroType1.CheckedChanged += new System.EventHandler(this.RdoMacroType0_CheckedChanged);
            // 
            // RdoMacroType0
            // 
            this.RdoMacroType0.AutoSize = true;
            this.RdoMacroType0.Checked = true;
            this.RdoMacroType0.Location = new System.Drawing.Point(22, 15);
            this.RdoMacroType0.Name = "RdoMacroType0";
            this.RdoMacroType0.Size = new System.Drawing.Size(52, 21);
            this.RdoMacroType0.TabIndex = 138;
            this.RdoMacroType0.TabStop = true;
            this.RdoMacroType0.Text = "개인";
            this.RdoMacroType0.UseVisualStyleBackColor = true;
            this.RdoMacroType0.CheckedChanged += new System.EventHandler(this.RdoMacroType0_CheckedChanged);
            // 
            // BtnNew
            // 
            this.BtnNew.Location = new System.Drawing.Point(402, 15);
            this.BtnNew.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnNew.Name = "BtnNew";
            this.BtnNew.Size = new System.Drawing.Size(75, 27);
            this.BtnNew.TabIndex = 137;
            this.BtnNew.Text = "화면정리";
            this.BtnNew.UseVisualStyleBackColor = true;
            this.BtnNew.Click += new System.EventHandler(this.BtnNew_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(483, 15);
            this.BtnSave.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 27);
            this.BtnSave.TabIndex = 133;
            this.BtnSave.Text = "저장";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnDelete
            // 
            this.BtnDelete.Location = new System.Drawing.Point(321, 15);
            this.BtnDelete.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(75, 27);
            this.BtnDelete.TabIndex = 136;
            this.BtnDelete.Text = "삭제";
            this.BtnDelete.UseVisualStyleBackColor = true;
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // PanMacroword22
            // 
            this.PanMacroword22.BackColor = System.Drawing.SystemColors.Window;
            this.PanMacroword22.Controls.Add(this.PanMacroword2);
            this.PanMacroword22.Controls.Add(this.SSList2);
            this.PanMacroword22.Controls.Add(this.panel4);
            this.PanMacroword22.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanMacroword22.Location = new System.Drawing.Point(592, 38);
            this.PanMacroword22.Name = "PanMacroword22";
            this.PanMacroword22.Size = new System.Drawing.Size(723, 493);
            this.PanMacroword22.TabIndex = 141;
            // 
            // PanMacroword2
            // 
            this.PanMacroword2.Controls.Add(this.numericUpDown1);
            this.PanMacroword2.Controls.Add(this.label5);
            this.PanMacroword2.Controls.Add(this.label6);
            this.PanMacroword2.Controls.Add(this.label8);
            this.PanMacroword2.Controls.Add(this.TxtSugesstion);
            this.PanMacroword2.Controls.Add(this.textBox2);
            this.PanMacroword2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanMacroword2.Location = new System.Drawing.Point(266, 49);
            this.PanMacroword2.Name = "PanMacroword2";
            this.PanMacroword2.Size = new System.Drawing.Size(457, 444);
            this.PanMacroword2.TabIndex = 140;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(120, 43);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(184, 25);
            this.numericUpDown1.TabIndex = 149;
            this.numericUpDown1.Tag = "DISPSEQ";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(26, 42);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label5.Size = new System.Drawing.Size(88, 25);
            this.label5.TabIndex = 148;
            this.label5.Text = "표시순서";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label6.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(26, 12);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label6.Size = new System.Drawing.Size(88, 25);
            this.label6.TabIndex = 146;
            this.label6.Text = "상용구 제목";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label8.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(26, 79);
            this.label8.Name = "label8";
            this.label8.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label8.Size = new System.Drawing.Size(278, 25);
            this.label8.TabIndex = 145;
            this.label8.Text = "상담 후 건의사항";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TxtSugesstion
            // 
            this.TxtSugesstion.Location = new System.Drawing.Point(26, 108);
            this.TxtSugesstion.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TxtSugesstion.Multiline = true;
            this.TxtSugesstion.Name = "TxtSugesstion";
            this.TxtSugesstion.Size = new System.Drawing.Size(278, 125);
            this.TxtSugesstion.TabIndex = 143;
            this.TxtSugesstion.Tag = "SUGESSTION";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(120, 12);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(184, 25);
            this.textBox2.TabIndex = 135;
            this.textBox2.Tag = "TITLE";
            // 
            // SSList2
            // 
            this.SSList2.AccessibleDescription = "";
            this.SSList2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSList2.Dock = System.Windows.Forms.DockStyle.Left;
            this.SSList2.Location = new System.Drawing.Point(0, 49);
            this.SSList2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SSList2.Name = "SSList2";
            this.SSList2.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSList2_Sheet1});
            this.SSList2.Size = new System.Drawing.Size(266, 444);
            this.SSList2.TabIndex = 134;
            this.SSList2.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSList2_CellClick);
            // 
            // SSList2_Sheet1
            // 
            this.SSList2_Sheet1.Reset();
            this.SSList2_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSList2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSList2_Sheet1.ColumnCount = 0;
            this.SSList2_Sheet1.RowCount = 0;
            this.SSList2_Sheet1.ActiveColumnIndex = -1;
            this.SSList2_Sheet1.ActiveRowIndex = -1;
            this.SSList2_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList2_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList2_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSList2_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList2_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList2_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList2_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSList2_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList2_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSList2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.RdoMacro2Type2);
            this.panel4.Controls.Add(this.RdoMacro2Type1);
            this.panel4.Controls.Add(this.RdoMacro2Type0);
            this.panel4.Controls.Add(this.BtnNew2);
            this.panel4.Controls.Add(this.BtnSave2);
            this.panel4.Controls.Add(this.BtnDelete2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(723, 49);
            this.panel4.TabIndex = 139;
            // 
            // RdoMacro2Type2
            // 
            this.RdoMacro2Type2.AutoSize = true;
            this.RdoMacro2Type2.Location = new System.Drawing.Point(177, 15);
            this.RdoMacro2Type2.Name = "RdoMacro2Type2";
            this.RdoMacro2Type2.Size = new System.Drawing.Size(65, 21);
            this.RdoMacro2Type2.TabIndex = 143;
            this.RdoMacro2Type2.TabStop = true;
            this.RdoMacro2Type2.Text = "간호사";
            this.RdoMacro2Type2.UseVisualStyleBackColor = true;
            this.RdoMacro2Type2.CheckedChanged += new System.EventHandler(this.RdoMacro2Type0_CheckedChanged);
            // 
            // RdoMacro2Type1
            // 
            this.RdoMacro2Type1.AutoSize = true;
            this.RdoMacro2Type1.Location = new System.Drawing.Point(110, 15);
            this.RdoMacro2Type1.Name = "RdoMacro2Type1";
            this.RdoMacro2Type1.Size = new System.Drawing.Size(52, 21);
            this.RdoMacro2Type1.TabIndex = 142;
            this.RdoMacro2Type1.TabStop = true;
            this.RdoMacro2Type1.Text = "의사";
            this.RdoMacro2Type1.UseVisualStyleBackColor = true;
            this.RdoMacro2Type1.CheckedChanged += new System.EventHandler(this.RdoMacro2Type0_CheckedChanged);
            // 
            // RdoMacro2Type0
            // 
            this.RdoMacro2Type0.AutoSize = true;
            this.RdoMacro2Type0.Checked = true;
            this.RdoMacro2Type0.Location = new System.Drawing.Point(50, 15);
            this.RdoMacro2Type0.Name = "RdoMacro2Type0";
            this.RdoMacro2Type0.Size = new System.Drawing.Size(52, 21);
            this.RdoMacro2Type0.TabIndex = 141;
            this.RdoMacro2Type0.TabStop = true;
            this.RdoMacro2Type0.Text = "개인";
            this.RdoMacro2Type0.UseVisualStyleBackColor = true;
            this.RdoMacro2Type0.CheckedChanged += new System.EventHandler(this.RdoMacro2Type0_CheckedChanged);
            // 
            // BtnNew2
            // 
            this.BtnNew2.Location = new System.Drawing.Point(414, 11);
            this.BtnNew2.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnNew2.Name = "BtnNew2";
            this.BtnNew2.Size = new System.Drawing.Size(75, 27);
            this.BtnNew2.TabIndex = 137;
            this.BtnNew2.Text = "화면정리";
            this.BtnNew2.UseVisualStyleBackColor = true;
            this.BtnNew2.Click += new System.EventHandler(this.BtnNew2_Click);
            // 
            // BtnSave2
            // 
            this.BtnSave2.Location = new System.Drawing.Point(495, 11);
            this.BtnSave2.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSave2.Name = "BtnSave2";
            this.BtnSave2.Size = new System.Drawing.Size(75, 27);
            this.BtnSave2.TabIndex = 133;
            this.BtnSave2.Text = "저장";
            this.BtnSave2.UseVisualStyleBackColor = true;
            this.BtnSave2.Click += new System.EventHandler(this.BtnSave2_Click);
            // 
            // BtnDelete2
            // 
            this.BtnDelete2.Location = new System.Drawing.Point(333, 11);
            this.BtnDelete2.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnDelete2.Name = "BtnDelete2";
            this.BtnDelete2.Size = new System.Drawing.Size(75, 27);
            this.BtnDelete2.TabIndex = 136;
            this.BtnDelete2.Text = "삭제";
            this.BtnDelete2.UseVisualStyleBackColor = true;
            this.BtnDelete2.Click += new System.EventHandler(this.BtnDelete2_Click);
            // 
            // WorkerHealthCheckMacroForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1315, 531);
            this.Controls.Add(this.PanMacroword22);
            this.Controls.Add(this.PanMacroword11);
            this.Controls.Add(this.contentTitle1);
            this.Name = "WorkerHealthCheckMacroForm";
            this.Text = "근로자 건강상담 상용구 관리";
            this.Load += new System.EventHandler(this.MacrowordForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.PanMacroword11.ResumeLayout(false);
            this.PanMacroword.ResumeLayout(false);
            this.PanMacroword.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumDispSeq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.PanMacroword22.ResumeLayout(false);
            this.PanMacroword2.ResumeLayout(false);
            this.PanMacroword2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList2_Sheet1)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ComBase.Mvc.UserControls.ContentTitle contentTitle1;
        private System.Windows.Forms.Panel PanMacroword11;
        private System.Windows.Forms.Button BtnNew;
        private System.Windows.Forms.Button BtnDelete;
        private FarPoint.Win.Spread.FpSpread SSList;
        private FarPoint.Win.Spread.SheetView SSList_Sheet1;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel PanMacroword;
        private System.Windows.Forms.TextBox TxtTitle;
        private System.Windows.Forms.TextBox TxtContent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown NumDispSeq;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel PanMacroword22;
        private System.Windows.Forms.Panel PanMacroword2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TxtSugesstion;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button BtnNew2;
        private System.Windows.Forms.Button BtnSave2;
        private System.Windows.Forms.Button BtnDelete2;
        private FarPoint.Win.Spread.FpSpread SSList2;
        private FarPoint.Win.Spread.SheetView SSList2_Sheet1;
        private System.Windows.Forms.RadioButton RdoMacroType2;
        private System.Windows.Forms.RadioButton RdoMacroType1;
        private System.Windows.Forms.RadioButton RdoMacroType0;
        private System.Windows.Forms.RadioButton RdoMacro2Type2;
        private System.Windows.Forms.RadioButton RdoMacro2Type1;
        private System.Windows.Forms.RadioButton RdoMacro2Type0;
    }
}