namespace HC_Act
{
    partial class frmHcExResultSend
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color352637081267356626617", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Static432637081267356646559", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static565637081267356656536");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Static619637081267356666506");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("Text771637081267356686495");
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblFormTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SS1 = new FarPoint.Win.Spread.FpSpread();
            this.SS1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSave_Send = new System.Windows.Forms.Button();
            this.btnDSel_All = new System.Windows.Forms.Button();
            this.btnSel_All = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpTDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.btnView = new System.Windows.Forms.Button();
            this.panWork = new System.Windows.Forms.Panel();
            this.SS2 = new FarPoint.Win.Spread.FpSpread();
            this.SS2_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel6 = new System.Windows.Forms.Panel();
            this.rdoGbn2 = new System.Windows.Forms.RadioButton();
            this.rdoGbn1 = new System.Windows.Forms.RadioButton();
            this.panel5 = new System.Windows.Forms.Panel();
            this.rdoJob3 = new System.Windows.Forms.RadioButton();
            this.rdoJob2 = new System.Windows.Forms.RadioButton();
            this.rdoJob1 = new System.Windows.Forms.RadioButton();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panWork.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS2_Sheet1)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.btnExit);
            this.panel4.Controls.Add(this.lblFormTitle);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1047, 36);
            this.panel4.TabIndex = 5;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(956, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(89, 34);
            this.btnExit.TabIndex = 9;
            this.btnExit.Text = "닫기(&X)";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.AutoSize = true;
            this.lblFormTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblFormTitle.Location = new System.Drawing.Point(7, 7);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(182, 21);
            this.lblFormTitle.TabIndex = 7;
            this.lblFormTitle.Text = "임상병리 검사결과 전송";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.SS1);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 36);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1047, 285);
            this.panel1.TabIndex = 8;
            // 
            // SS1
            // 
            this.SS1.AccessibleDescription = "fpSpread1, Sheet1, Row 0, Column 0, ";
            this.SS1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SS1.FocusRenderer = flatFocusIndicatorRenderer1;
            this.SS1.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SS1.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SS1.HorizontalScrollBar.Renderer = flatScrollBarRenderer1;
            this.SS1.HorizontalScrollBar.TabIndex = 109;
            this.SS1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SS1.Location = new System.Drawing.Point(204, 41);
            this.SS1.Name = "SS1";
            this.SS1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS1_Sheet1});
            this.SS1.Size = new System.Drawing.Size(841, 242);
            this.SS1.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.SS1.TabIndex = 146;
            this.SS1.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SS1.VerticalScrollBar.Name = "";
            flatScrollBarRenderer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SS1.VerticalScrollBar.Renderer = flatScrollBarRenderer2;
            this.SS1.VerticalScrollBar.TabIndex = 110;
            this.SS1.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // SS1_Sheet1
            // 
            this.SS1_Sheet1.Reset();
            this.SS1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS1_Sheet1.ColumnCount = 5;
            this.SS1_Sheet1.RowCount = 1;
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.SS1_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.SS1_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SS1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS1_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.SS1_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.Rows.Get(0).Height = 24F;
            this.SS1_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.SS1_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.btnSave_Send);
            this.panel3.Controls.Add(this.btnDSel_All);
            this.panel3.Controls.Add(this.btnSel_All);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(204, 0);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(2);
            this.panel3.Size = new System.Drawing.Size(841, 41);
            this.panel3.TabIndex = 1;
            // 
            // btnSave_Send
            // 
            this.btnSave_Send.BackColor = System.Drawing.Color.White;
            this.btnSave_Send.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave_Send.Location = new System.Drawing.Point(751, 2);
            this.btnSave_Send.Name = "btnSave_Send";
            this.btnSave_Send.Size = new System.Drawing.Size(86, 35);
            this.btnSave_Send.TabIndex = 2;
            this.btnSave_Send.Text = "결과전송";
            this.btnSave_Send.UseVisualStyleBackColor = false;
            // 
            // btnDSel_All
            // 
            this.btnDSel_All.BackColor = System.Drawing.Color.White;
            this.btnDSel_All.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnDSel_All.Location = new System.Drawing.Point(73, 2);
            this.btnDSel_All.Name = "btnDSel_All";
            this.btnDSel_All.Size = new System.Drawing.Size(71, 35);
            this.btnDSel_All.TabIndex = 1;
            this.btnDSel_All.Text = "선택취소";
            this.btnDSel_All.UseVisualStyleBackColor = false;
            // 
            // btnSel_All
            // 
            this.btnSel_All.BackColor = System.Drawing.Color.White;
            this.btnSel_All.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSel_All.Location = new System.Drawing.Point(2, 2);
            this.btnSel_All.Name = "btnSel_All";
            this.btnSel_All.Size = new System.Drawing.Size(71, 35);
            this.btnSel_All.TabIndex = 0;
            this.btnSel_All.Text = "전체선택";
            this.btnSel_All.UseVisualStyleBackColor = false;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.txtSName);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.dtpTDate);
            this.panel2.Controls.Add(this.dtpFDate);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.btnView);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(204, 283);
            this.panel2.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 183);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 17);
            this.label4.TabIndex = 12;
            this.label4.Text = "수검자명";
            // 
            // txtSName
            // 
            this.txtSName.Location = new System.Drawing.Point(14, 203);
            this.txtSName.Name = "txtSName";
            this.txtSName.Size = new System.Drawing.Size(157, 25);
            this.txtSName.TabIndex = 11;
            this.txtSName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(137, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 17);
            this.label3.TabIndex = 10;
            this.label3.Text = "까지";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(137, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 17);
            this.label2.TabIndex = 9;
            this.label2.Text = "부터";
            // 
            // dtpTDate
            // 
            this.dtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTDate.Location = new System.Drawing.Point(14, 138);
            this.dtpTDate.Name = "dtpTDate";
            this.dtpTDate.Size = new System.Drawing.Size(117, 25);
            this.dtpTDate.TabIndex = 8;
            // 
            // dtpFDate
            // 
            this.dtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFDate.Location = new System.Drawing.Point(14, 107);
            this.dtpFDate.Name = "dtpFDate";
            this.dtpFDate.Size = new System.Drawing.Size(117, 25);
            this.dtpFDate.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "검색일자";
            // 
            // btnView
            // 
            this.btnView.BackColor = System.Drawing.Color.White;
            this.btnView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnView.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnView.Location = new System.Drawing.Point(0, 241);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(202, 40);
            this.btnView.TabIndex = 5;
            this.btnView.Text = "조 회";
            this.btnView.UseVisualStyleBackColor = false;
            // 
            // panWork
            // 
            this.panWork.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panWork.Controls.Add(this.SS2);
            this.panWork.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panWork.Location = new System.Drawing.Point(0, 321);
            this.panWork.Name = "panWork";
            this.panWork.Size = new System.Drawing.Size(1047, 384);
            this.panWork.TabIndex = 9;
            // 
            // SS2
            // 
            this.SS2.AccessibleDescription = "fpSpread1, Sheet1, Row 0, Column 0, ";
            this.SS2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SS2.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.SS2.Location = new System.Drawing.Point(0, 0);
            this.SS2.Name = "SS2";
            namedStyle1.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType1.Static = true;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
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
            textCellType4.MaxLength = 10;
            namedStyle5.CellType = textCellType4;
            namedStyle5.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle5.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle5.Renderer = textCellType4;
            namedStyle5.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS2.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4,
            namedStyle5});
            this.SS2.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS2_Sheet1});
            this.SS2.Size = new System.Drawing.Size(1045, 382);
            this.SS2.TabIndex = 147;
            this.SS2.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.SS2.TextTipAppearance = tipAppearance1;
            this.SS2.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // SS2_Sheet1
            // 
            this.SS2_Sheet1.Reset();
            this.SS2_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS2_Sheet1.ColumnCount = 11;
            this.SS2_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SS2_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "접수번호";
            this.SS2_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "성 명";
            this.SS2_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "검사코드명";
            this.SS2_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "결과형태";
            this.SS2_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "검사명";
            this.SS2_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "결과";
            this.SS2_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "변환결과";
            this.SS2_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "변환상태";
            this.SS2_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "검진항목";
            this.SS2_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "검사코드";
            this.SS2_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "접수일자";
            this.SS2_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SS2_Sheet1.ColumnHeader.Rows.Get(0).Height = 21F;
            this.SS2_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SS2_Sheet1.Columns.Get(0).Label = "접수번호";
            this.SS2_Sheet1.Columns.Get(0).StyleName = "Static565637081267356656536";
            this.SS2_Sheet1.Columns.Get(0).Width = 71F;
            this.SS2_Sheet1.Columns.Get(1).Label = "성 명";
            this.SS2_Sheet1.Columns.Get(1).StyleName = "Static565637081267356656536";
            this.SS2_Sheet1.Columns.Get(2).Label = "검사코드명";
            this.SS2_Sheet1.Columns.Get(2).StyleName = "Static619637081267356666506";
            this.SS2_Sheet1.Columns.Get(2).Width = 89F;
            this.SS2_Sheet1.Columns.Get(3).Label = "결과형태";
            this.SS2_Sheet1.Columns.Get(3).StyleName = "Static619637081267356666506";
            this.SS2_Sheet1.Columns.Get(3).Width = 65F;
            this.SS2_Sheet1.Columns.Get(4).Label = "검사명";
            this.SS2_Sheet1.Columns.Get(4).StyleName = "Static619637081267356666506";
            this.SS2_Sheet1.Columns.Get(4).Width = 122F;
            this.SS2_Sheet1.Columns.Get(5).Label = "결과";
            this.SS2_Sheet1.Columns.Get(5).StyleName = "Static619637081267356666506";
            this.SS2_Sheet1.Columns.Get(5).Width = 137F;
            this.SS2_Sheet1.Columns.Get(6).Label = "변환결과";
            this.SS2_Sheet1.Columns.Get(6).StyleName = "Text771637081267356686495";
            this.SS2_Sheet1.Columns.Get(6).Width = 111F;
            this.SS2_Sheet1.Columns.Get(7).Label = "변환상태";
            this.SS2_Sheet1.Columns.Get(7).StyleName = "Static565637081267356656536";
            this.SS2_Sheet1.Columns.Get(7).Width = 61F;
            this.SS2_Sheet1.Columns.Get(8).Label = "검진항목";
            this.SS2_Sheet1.Columns.Get(8).StyleName = "Static565637081267356656536";
            this.SS2_Sheet1.Columns.Get(9).Label = "검사코드";
            this.SS2_Sheet1.Columns.Get(9).StyleName = "Static565637081267356656536";
            this.SS2_Sheet1.Columns.Get(10).Label = "접수일자";
            this.SS2_Sheet1.Columns.Get(10).StyleName = "Static565637081267356656536";
            this.SS2_Sheet1.Columns.Get(10).Width = 87F;
            this.SS2_Sheet1.DefaultStyleName = "Static432637081267356646559";
            this.SS2_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.None);
            this.SS2_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS2_Sheet1.RowHeader.Columns.Get(0).Width = 0F;
            this.SS2_Sheet1.RowHeader.Visible = false;
            this.SS2_Sheet1.Rows.Get(0).Height = 17F;
            this.SS2_Sheet1.Rows.Get(1).Height = 17F;
            this.SS2_Sheet1.Rows.Get(2).Height = 17F;
            this.SS2_Sheet1.Rows.Get(3).Height = 17F;
            this.SS2_Sheet1.Rows.Get(4).Height = 17F;
            this.SS2_Sheet1.Rows.Get(5).Height = 17F;
            this.SS2_Sheet1.Rows.Get(6).Height = 17F;
            this.SS2_Sheet1.Rows.Get(7).Height = 17F;
            this.SS2_Sheet1.Rows.Get(8).Height = 17F;
            this.SS2_Sheet1.Rows.Get(9).Height = 17F;
            this.SS2_Sheet1.Rows.Get(10).Height = 17F;
            this.SS2_Sheet1.Rows.Get(11).Height = 17F;
            this.SS2_Sheet1.Rows.Get(12).Height = 17F;
            this.SS2_Sheet1.Rows.Get(13).Height = 17F;
            this.SS2_Sheet1.Rows.Get(14).Height = 17F;
            this.SS2_Sheet1.Rows.Get(15).Height = 17F;
            this.SS2_Sheet1.Rows.Get(16).Height = 17F;
            this.SS2_Sheet1.Rows.Get(17).Height = 17F;
            this.SS2_Sheet1.Rows.Get(18).Height = 17F;
            this.SS2_Sheet1.Rows.Get(19).Height = 17F;
            this.SS2_Sheet1.Rows.Get(20).Height = 17F;
            this.SS2_Sheet1.Rows.Get(21).Height = 17F;
            this.SS2_Sheet1.Rows.Get(22).Height = 17F;
            this.SS2_Sheet1.Rows.Get(23).Height = 17F;
            this.SS2_Sheet1.Rows.Get(24).Height = 17F;
            this.SS2_Sheet1.Rows.Get(25).Height = 17F;
            this.SS2_Sheet1.Rows.Get(26).Height = 17F;
            this.SS2_Sheet1.Rows.Get(27).Height = 17F;
            this.SS2_Sheet1.Rows.Get(28).Height = 17F;
            this.SS2_Sheet1.Rows.Get(29).Height = 17F;
            this.SS2_Sheet1.Rows.Get(30).Height = 17F;
            this.SS2_Sheet1.Rows.Get(31).Height = 17F;
            this.SS2_Sheet1.Rows.Get(32).Height = 17F;
            this.SS2_Sheet1.Rows.Get(33).Height = 17F;
            this.SS2_Sheet1.Rows.Get(34).Height = 17F;
            this.SS2_Sheet1.Rows.Get(35).Height = 17F;
            this.SS2_Sheet1.Rows.Get(36).Height = 17F;
            this.SS2_Sheet1.Rows.Get(37).Height = 17F;
            this.SS2_Sheet1.Rows.Get(38).Height = 17F;
            this.SS2_Sheet1.Rows.Get(39).Height = 17F;
            this.SS2_Sheet1.Rows.Get(40).Height = 17F;
            this.SS2_Sheet1.Rows.Get(41).Height = 17F;
            this.SS2_Sheet1.Rows.Get(42).Height = 17F;
            this.SS2_Sheet1.Rows.Get(43).Height = 17F;
            this.SS2_Sheet1.Rows.Get(44).Height = 17F;
            this.SS2_Sheet1.Rows.Get(45).Height = 17F;
            this.SS2_Sheet1.Rows.Get(46).Height = 17F;
            this.SS2_Sheet1.Rows.Get(47).Height = 17F;
            this.SS2_Sheet1.Rows.Get(48).Height = 17F;
            this.SS2_Sheet1.Rows.Get(49).Height = 17F;
            this.SS2_Sheet1.Rows.Get(50).Height = 17F;
            this.SS2_Sheet1.Rows.Get(51).Height = 17F;
            this.SS2_Sheet1.Rows.Get(52).Height = 17F;
            this.SS2_Sheet1.Rows.Get(53).Height = 17F;
            this.SS2_Sheet1.Rows.Get(54).Height = 17F;
            this.SS2_Sheet1.Rows.Get(55).Height = 17F;
            this.SS2_Sheet1.Rows.Get(56).Height = 17F;
            this.SS2_Sheet1.Rows.Get(57).Height = 17F;
            this.SS2_Sheet1.Rows.Get(58).Height = 17F;
            this.SS2_Sheet1.Rows.Get(59).Height = 17F;
            this.SS2_Sheet1.Rows.Get(60).Height = 17F;
            this.SS2_Sheet1.Rows.Get(61).Height = 17F;
            this.SS2_Sheet1.Rows.Get(62).Height = 17F;
            this.SS2_Sheet1.Rows.Get(63).Height = 17F;
            this.SS2_Sheet1.Rows.Get(64).Height = 17F;
            this.SS2_Sheet1.Rows.Get(65).Height = 17F;
            this.SS2_Sheet1.Rows.Get(66).Height = 17F;
            this.SS2_Sheet1.Rows.Get(67).Height = 17F;
            this.SS2_Sheet1.Rows.Get(68).Height = 17F;
            this.SS2_Sheet1.Rows.Get(69).Height = 17F;
            this.SS2_Sheet1.Rows.Get(70).Height = 17F;
            this.SS2_Sheet1.Rows.Get(71).Height = 17F;
            this.SS2_Sheet1.Rows.Get(72).Height = 17F;
            this.SS2_Sheet1.Rows.Get(73).Height = 17F;
            this.SS2_Sheet1.Rows.Get(74).Height = 17F;
            this.SS2_Sheet1.Rows.Get(75).Height = 17F;
            this.SS2_Sheet1.Rows.Get(76).Height = 17F;
            this.SS2_Sheet1.Rows.Get(77).Height = 17F;
            this.SS2_Sheet1.Rows.Get(78).Height = 17F;
            this.SS2_Sheet1.Rows.Get(79).Height = 17F;
            this.SS2_Sheet1.Rows.Get(80).Height = 17F;
            this.SS2_Sheet1.Rows.Get(81).Height = 17F;
            this.SS2_Sheet1.Rows.Get(82).Height = 17F;
            this.SS2_Sheet1.Rows.Get(83).Height = 17F;
            this.SS2_Sheet1.Rows.Get(84).Height = 17F;
            this.SS2_Sheet1.Rows.Get(85).Height = 17F;
            this.SS2_Sheet1.Rows.Get(86).Height = 17F;
            this.SS2_Sheet1.Rows.Get(87).Height = 17F;
            this.SS2_Sheet1.Rows.Get(88).Height = 17F;
            this.SS2_Sheet1.Rows.Get(89).Height = 17F;
            this.SS2_Sheet1.Rows.Get(90).Height = 17F;
            this.SS2_Sheet1.Rows.Get(91).Height = 17F;
            this.SS2_Sheet1.Rows.Get(92).Height = 17F;
            this.SS2_Sheet1.Rows.Get(93).Height = 17F;
            this.SS2_Sheet1.Rows.Get(94).Height = 17F;
            this.SS2_Sheet1.Rows.Get(95).Height = 17F;
            this.SS2_Sheet1.Rows.Get(96).Height = 17F;
            this.SS2_Sheet1.Rows.Get(97).Height = 17F;
            this.SS2_Sheet1.Rows.Get(98).Height = 17F;
            this.SS2_Sheet1.Rows.Get(99).Height = 17F;
            this.SS2_Sheet1.Rows.Get(100).Height = 17F;
            this.SS2_Sheet1.Rows.Get(101).Height = 17F;
            this.SS2_Sheet1.Rows.Get(102).Height = 17F;
            this.SS2_Sheet1.Rows.Get(103).Height = 17F;
            this.SS2_Sheet1.Rows.Get(104).Height = 17F;
            this.SS2_Sheet1.Rows.Get(105).Height = 17F;
            this.SS2_Sheet1.Rows.Get(106).Height = 17F;
            this.SS2_Sheet1.Rows.Get(107).Height = 17F;
            this.SS2_Sheet1.Rows.Get(108).Height = 17F;
            this.SS2_Sheet1.Rows.Get(109).Height = 17F;
            this.SS2_Sheet1.Rows.Get(110).Height = 17F;
            this.SS2_Sheet1.Rows.Get(111).Height = 17F;
            this.SS2_Sheet1.Rows.Get(112).Height = 17F;
            this.SS2_Sheet1.Rows.Get(113).Height = 17F;
            this.SS2_Sheet1.Rows.Get(114).Height = 17F;
            this.SS2_Sheet1.Rows.Get(115).Height = 17F;
            this.SS2_Sheet1.Rows.Get(116).Height = 17F;
            this.SS2_Sheet1.Rows.Get(117).Height = 17F;
            this.SS2_Sheet1.Rows.Get(118).Height = 17F;
            this.SS2_Sheet1.Rows.Get(119).Height = 17F;
            this.SS2_Sheet1.Rows.Get(120).Height = 17F;
            this.SS2_Sheet1.Rows.Get(121).Height = 17F;
            this.SS2_Sheet1.Rows.Get(122).Height = 17F;
            this.SS2_Sheet1.Rows.Get(123).Height = 17F;
            this.SS2_Sheet1.Rows.Get(124).Height = 17F;
            this.SS2_Sheet1.Rows.Get(125).Height = 17F;
            this.SS2_Sheet1.Rows.Get(126).Height = 17F;
            this.SS2_Sheet1.Rows.Get(127).Height = 17F;
            this.SS2_Sheet1.Rows.Get(128).Height = 17F;
            this.SS2_Sheet1.Rows.Get(129).Height = 17F;
            this.SS2_Sheet1.Rows.Get(130).Height = 17F;
            this.SS2_Sheet1.Rows.Get(131).Height = 17F;
            this.SS2_Sheet1.Rows.Get(132).Height = 17F;
            this.SS2_Sheet1.Rows.Get(133).Height = 17F;
            this.SS2_Sheet1.Rows.Get(134).Height = 17F;
            this.SS2_Sheet1.Rows.Get(135).Height = 17F;
            this.SS2_Sheet1.Rows.Get(136).Height = 17F;
            this.SS2_Sheet1.Rows.Get(137).Height = 17F;
            this.SS2_Sheet1.Rows.Get(138).Height = 17F;
            this.SS2_Sheet1.Rows.Get(139).Height = 17F;
            this.SS2_Sheet1.Rows.Get(140).Height = 17F;
            this.SS2_Sheet1.Rows.Get(141).Height = 17F;
            this.SS2_Sheet1.Rows.Get(142).Height = 17F;
            this.SS2_Sheet1.Rows.Get(143).Height = 17F;
            this.SS2_Sheet1.Rows.Get(144).Height = 17F;
            this.SS2_Sheet1.Rows.Get(145).Height = 17F;
            this.SS2_Sheet1.Rows.Get(146).Height = 17F;
            this.SS2_Sheet1.Rows.Get(147).Height = 17F;
            this.SS2_Sheet1.Rows.Get(148).Height = 17F;
            this.SS2_Sheet1.Rows.Get(149).Height = 17F;
            this.SS2_Sheet1.Rows.Get(150).Height = 17F;
            this.SS2_Sheet1.Rows.Get(151).Height = 17F;
            this.SS2_Sheet1.Rows.Get(152).Height = 17F;
            this.SS2_Sheet1.Rows.Get(153).Height = 17F;
            this.SS2_Sheet1.Rows.Get(154).Height = 17F;
            this.SS2_Sheet1.Rows.Get(155).Height = 17F;
            this.SS2_Sheet1.Rows.Get(156).Height = 17F;
            this.SS2_Sheet1.Rows.Get(157).Height = 17F;
            this.SS2_Sheet1.Rows.Get(158).Height = 17F;
            this.SS2_Sheet1.Rows.Get(159).Height = 17F;
            this.SS2_Sheet1.Rows.Get(160).Height = 17F;
            this.SS2_Sheet1.Rows.Get(161).Height = 17F;
            this.SS2_Sheet1.Rows.Get(162).Height = 17F;
            this.SS2_Sheet1.Rows.Get(163).Height = 17F;
            this.SS2_Sheet1.Rows.Get(164).Height = 17F;
            this.SS2_Sheet1.Rows.Get(165).Height = 17F;
            this.SS2_Sheet1.Rows.Get(166).Height = 17F;
            this.SS2_Sheet1.Rows.Get(167).Height = 17F;
            this.SS2_Sheet1.Rows.Get(168).Height = 17F;
            this.SS2_Sheet1.Rows.Get(169).Height = 17F;
            this.SS2_Sheet1.Rows.Get(170).Height = 17F;
            this.SS2_Sheet1.Rows.Get(171).Height = 17F;
            this.SS2_Sheet1.Rows.Get(172).Height = 17F;
            this.SS2_Sheet1.Rows.Get(173).Height = 17F;
            this.SS2_Sheet1.Rows.Get(174).Height = 17F;
            this.SS2_Sheet1.Rows.Get(175).Height = 17F;
            this.SS2_Sheet1.Rows.Get(176).Height = 17F;
            this.SS2_Sheet1.Rows.Get(177).Height = 17F;
            this.SS2_Sheet1.Rows.Get(178).Height = 17F;
            this.SS2_Sheet1.Rows.Get(179).Height = 17F;
            this.SS2_Sheet1.Rows.Get(180).Height = 17F;
            this.SS2_Sheet1.Rows.Get(181).Height = 17F;
            this.SS2_Sheet1.Rows.Get(182).Height = 17F;
            this.SS2_Sheet1.Rows.Get(183).Height = 17F;
            this.SS2_Sheet1.Rows.Get(184).Height = 17F;
            this.SS2_Sheet1.Rows.Get(185).Height = 17F;
            this.SS2_Sheet1.Rows.Get(186).Height = 17F;
            this.SS2_Sheet1.Rows.Get(187).Height = 17F;
            this.SS2_Sheet1.Rows.Get(188).Height = 17F;
            this.SS2_Sheet1.Rows.Get(189).Height = 17F;
            this.SS2_Sheet1.Rows.Get(190).Height = 17F;
            this.SS2_Sheet1.Rows.Get(191).Height = 17F;
            this.SS2_Sheet1.Rows.Get(192).Height = 17F;
            this.SS2_Sheet1.Rows.Get(193).Height = 17F;
            this.SS2_Sheet1.Rows.Get(194).Height = 17F;
            this.SS2_Sheet1.Rows.Get(195).Height = 17F;
            this.SS2_Sheet1.Rows.Get(196).Height = 17F;
            this.SS2_Sheet1.Rows.Get(197).Height = 17F;
            this.SS2_Sheet1.Rows.Get(198).Height = 17F;
            this.SS2_Sheet1.Rows.Get(199).Height = 17F;
            this.SS2_Sheet1.Rows.Get(200).Height = 17F;
            this.SS2_Sheet1.Rows.Get(201).Height = 17F;
            this.SS2_Sheet1.Rows.Get(202).Height = 17F;
            this.SS2_Sheet1.Rows.Get(203).Height = 17F;
            this.SS2_Sheet1.Rows.Get(204).Height = 17F;
            this.SS2_Sheet1.Rows.Get(205).Height = 17F;
            this.SS2_Sheet1.Rows.Get(206).Height = 17F;
            this.SS2_Sheet1.Rows.Get(207).Height = 17F;
            this.SS2_Sheet1.Rows.Get(208).Height = 17F;
            this.SS2_Sheet1.Rows.Get(209).Height = 17F;
            this.SS2_Sheet1.Rows.Get(210).Height = 17F;
            this.SS2_Sheet1.Rows.Get(211).Height = 17F;
            this.SS2_Sheet1.Rows.Get(212).Height = 17F;
            this.SS2_Sheet1.Rows.Get(213).Height = 17F;
            this.SS2_Sheet1.Rows.Get(214).Height = 17F;
            this.SS2_Sheet1.Rows.Get(215).Height = 17F;
            this.SS2_Sheet1.Rows.Get(216).Height = 17F;
            this.SS2_Sheet1.Rows.Get(217).Height = 17F;
            this.SS2_Sheet1.Rows.Get(218).Height = 17F;
            this.SS2_Sheet1.Rows.Get(219).Height = 17F;
            this.SS2_Sheet1.Rows.Get(220).Height = 17F;
            this.SS2_Sheet1.Rows.Get(221).Height = 17F;
            this.SS2_Sheet1.Rows.Get(222).Height = 17F;
            this.SS2_Sheet1.Rows.Get(223).Height = 17F;
            this.SS2_Sheet1.Rows.Get(224).Height = 17F;
            this.SS2_Sheet1.Rows.Get(225).Height = 17F;
            this.SS2_Sheet1.Rows.Get(226).Height = 17F;
            this.SS2_Sheet1.Rows.Get(227).Height = 17F;
            this.SS2_Sheet1.Rows.Get(228).Height = 17F;
            this.SS2_Sheet1.Rows.Get(229).Height = 17F;
            this.SS2_Sheet1.Rows.Get(230).Height = 17F;
            this.SS2_Sheet1.Rows.Get(231).Height = 17F;
            this.SS2_Sheet1.Rows.Get(232).Height = 17F;
            this.SS2_Sheet1.Rows.Get(233).Height = 17F;
            this.SS2_Sheet1.Rows.Get(234).Height = 17F;
            this.SS2_Sheet1.Rows.Get(235).Height = 17F;
            this.SS2_Sheet1.Rows.Get(236).Height = 17F;
            this.SS2_Sheet1.Rows.Get(237).Height = 17F;
            this.SS2_Sheet1.Rows.Get(238).Height = 17F;
            this.SS2_Sheet1.Rows.Get(239).Height = 17F;
            this.SS2_Sheet1.Rows.Get(240).Height = 17F;
            this.SS2_Sheet1.Rows.Get(241).Height = 17F;
            this.SS2_Sheet1.Rows.Get(242).Height = 17F;
            this.SS2_Sheet1.Rows.Get(243).Height = 17F;
            this.SS2_Sheet1.Rows.Get(244).Height = 17F;
            this.SS2_Sheet1.Rows.Get(245).Height = 17F;
            this.SS2_Sheet1.Rows.Get(246).Height = 17F;
            this.SS2_Sheet1.Rows.Get(247).Height = 17F;
            this.SS2_Sheet1.Rows.Get(248).Height = 17F;
            this.SS2_Sheet1.Rows.Get(249).Height = 17F;
            this.SS2_Sheet1.Rows.Get(250).Height = 17F;
            this.SS2_Sheet1.Rows.Get(251).Height = 17F;
            this.SS2_Sheet1.Rows.Get(252).Height = 17F;
            this.SS2_Sheet1.Rows.Get(253).Height = 17F;
            this.SS2_Sheet1.Rows.Get(254).Height = 17F;
            this.SS2_Sheet1.Rows.Get(255).Height = 17F;
            this.SS2_Sheet1.Rows.Get(256).Height = 17F;
            this.SS2_Sheet1.Rows.Get(257).Height = 17F;
            this.SS2_Sheet1.Rows.Get(258).Height = 17F;
            this.SS2_Sheet1.Rows.Get(259).Height = 17F;
            this.SS2_Sheet1.Rows.Get(260).Height = 17F;
            this.SS2_Sheet1.Rows.Get(261).Height = 17F;
            this.SS2_Sheet1.Rows.Get(262).Height = 17F;
            this.SS2_Sheet1.Rows.Get(263).Height = 17F;
            this.SS2_Sheet1.Rows.Get(264).Height = 17F;
            this.SS2_Sheet1.Rows.Get(265).Height = 17F;
            this.SS2_Sheet1.Rows.Get(266).Height = 17F;
            this.SS2_Sheet1.Rows.Get(267).Height = 17F;
            this.SS2_Sheet1.Rows.Get(268).Height = 17F;
            this.SS2_Sheet1.Rows.Get(269).Height = 17F;
            this.SS2_Sheet1.Rows.Get(270).Height = 17F;
            this.SS2_Sheet1.Rows.Get(271).Height = 17F;
            this.SS2_Sheet1.Rows.Get(272).Height = 17F;
            this.SS2_Sheet1.Rows.Get(273).Height = 17F;
            this.SS2_Sheet1.Rows.Get(274).Height = 17F;
            this.SS2_Sheet1.Rows.Get(275).Height = 17F;
            this.SS2_Sheet1.Rows.Get(276).Height = 17F;
            this.SS2_Sheet1.Rows.Get(277).Height = 17F;
            this.SS2_Sheet1.Rows.Get(278).Height = 17F;
            this.SS2_Sheet1.Rows.Get(279).Height = 17F;
            this.SS2_Sheet1.Rows.Get(280).Height = 17F;
            this.SS2_Sheet1.Rows.Get(281).Height = 17F;
            this.SS2_Sheet1.Rows.Get(282).Height = 17F;
            this.SS2_Sheet1.Rows.Get(283).Height = 17F;
            this.SS2_Sheet1.Rows.Get(284).Height = 17F;
            this.SS2_Sheet1.Rows.Get(285).Height = 17F;
            this.SS2_Sheet1.Rows.Get(286).Height = 17F;
            this.SS2_Sheet1.Rows.Get(287).Height = 17F;
            this.SS2_Sheet1.Rows.Get(288).Height = 17F;
            this.SS2_Sheet1.Rows.Get(289).Height = 17F;
            this.SS2_Sheet1.Rows.Get(290).Height = 17F;
            this.SS2_Sheet1.Rows.Get(291).Height = 17F;
            this.SS2_Sheet1.Rows.Get(292).Height = 17F;
            this.SS2_Sheet1.Rows.Get(293).Height = 17F;
            this.SS2_Sheet1.Rows.Get(294).Height = 17F;
            this.SS2_Sheet1.Rows.Get(295).Height = 17F;
            this.SS2_Sheet1.Rows.Get(296).Height = 17F;
            this.SS2_Sheet1.Rows.Get(297).Height = 17F;
            this.SS2_Sheet1.Rows.Get(298).Height = 17F;
            this.SS2_Sheet1.Rows.Get(299).Height = 17F;
            this.SS2_Sheet1.Rows.Get(300).Height = 17F;
            this.SS2_Sheet1.Rows.Get(301).Height = 17F;
            this.SS2_Sheet1.Rows.Get(302).Height = 17F;
            this.SS2_Sheet1.Rows.Get(303).Height = 17F;
            this.SS2_Sheet1.Rows.Get(304).Height = 17F;
            this.SS2_Sheet1.Rows.Get(305).Height = 17F;
            this.SS2_Sheet1.Rows.Get(306).Height = 17F;
            this.SS2_Sheet1.Rows.Get(307).Height = 17F;
            this.SS2_Sheet1.Rows.Get(308).Height = 17F;
            this.SS2_Sheet1.Rows.Get(309).Height = 17F;
            this.SS2_Sheet1.Rows.Get(310).Height = 17F;
            this.SS2_Sheet1.Rows.Get(311).Height = 17F;
            this.SS2_Sheet1.Rows.Get(312).Height = 17F;
            this.SS2_Sheet1.Rows.Get(313).Height = 17F;
            this.SS2_Sheet1.Rows.Get(314).Height = 17F;
            this.SS2_Sheet1.Rows.Get(315).Height = 17F;
            this.SS2_Sheet1.Rows.Get(316).Height = 17F;
            this.SS2_Sheet1.Rows.Get(317).Height = 17F;
            this.SS2_Sheet1.Rows.Get(318).Height = 17F;
            this.SS2_Sheet1.Rows.Get(319).Height = 17F;
            this.SS2_Sheet1.Rows.Get(320).Height = 17F;
            this.SS2_Sheet1.Rows.Get(321).Height = 17F;
            this.SS2_Sheet1.Rows.Get(322).Height = 17F;
            this.SS2_Sheet1.Rows.Get(323).Height = 17F;
            this.SS2_Sheet1.Rows.Get(324).Height = 17F;
            this.SS2_Sheet1.Rows.Get(325).Height = 17F;
            this.SS2_Sheet1.Rows.Get(326).Height = 17F;
            this.SS2_Sheet1.Rows.Get(327).Height = 17F;
            this.SS2_Sheet1.Rows.Get(328).Height = 17F;
            this.SS2_Sheet1.Rows.Get(329).Height = 17F;
            this.SS2_Sheet1.Rows.Get(330).Height = 17F;
            this.SS2_Sheet1.Rows.Get(331).Height = 17F;
            this.SS2_Sheet1.Rows.Get(332).Height = 17F;
            this.SS2_Sheet1.Rows.Get(333).Height = 17F;
            this.SS2_Sheet1.Rows.Get(334).Height = 17F;
            this.SS2_Sheet1.Rows.Get(335).Height = 17F;
            this.SS2_Sheet1.Rows.Get(336).Height = 17F;
            this.SS2_Sheet1.Rows.Get(337).Height = 17F;
            this.SS2_Sheet1.Rows.Get(338).Height = 17F;
            this.SS2_Sheet1.Rows.Get(339).Height = 17F;
            this.SS2_Sheet1.Rows.Get(340).Height = 17F;
            this.SS2_Sheet1.Rows.Get(341).Height = 17F;
            this.SS2_Sheet1.Rows.Get(342).Height = 17F;
            this.SS2_Sheet1.Rows.Get(343).Height = 17F;
            this.SS2_Sheet1.Rows.Get(344).Height = 17F;
            this.SS2_Sheet1.Rows.Get(345).Height = 17F;
            this.SS2_Sheet1.Rows.Get(346).Height = 17F;
            this.SS2_Sheet1.Rows.Get(347).Height = 17F;
            this.SS2_Sheet1.Rows.Get(348).Height = 17F;
            this.SS2_Sheet1.Rows.Get(349).Height = 17F;
            this.SS2_Sheet1.Rows.Get(350).Height = 17F;
            this.SS2_Sheet1.Rows.Get(351).Height = 17F;
            this.SS2_Sheet1.Rows.Get(352).Height = 17F;
            this.SS2_Sheet1.Rows.Get(353).Height = 17F;
            this.SS2_Sheet1.Rows.Get(354).Height = 17F;
            this.SS2_Sheet1.Rows.Get(355).Height = 17F;
            this.SS2_Sheet1.Rows.Get(356).Height = 17F;
            this.SS2_Sheet1.Rows.Get(357).Height = 17F;
            this.SS2_Sheet1.Rows.Get(358).Height = 17F;
            this.SS2_Sheet1.Rows.Get(359).Height = 17F;
            this.SS2_Sheet1.Rows.Get(360).Height = 17F;
            this.SS2_Sheet1.Rows.Get(361).Height = 17F;
            this.SS2_Sheet1.Rows.Get(362).Height = 17F;
            this.SS2_Sheet1.Rows.Get(363).Height = 17F;
            this.SS2_Sheet1.Rows.Get(364).Height = 17F;
            this.SS2_Sheet1.Rows.Get(365).Height = 17F;
            this.SS2_Sheet1.Rows.Get(366).Height = 17F;
            this.SS2_Sheet1.Rows.Get(367).Height = 17F;
            this.SS2_Sheet1.Rows.Get(368).Height = 17F;
            this.SS2_Sheet1.Rows.Get(369).Height = 17F;
            this.SS2_Sheet1.Rows.Get(370).Height = 17F;
            this.SS2_Sheet1.Rows.Get(371).Height = 17F;
            this.SS2_Sheet1.Rows.Get(372).Height = 17F;
            this.SS2_Sheet1.Rows.Get(373).Height = 17F;
            this.SS2_Sheet1.Rows.Get(374).Height = 17F;
            this.SS2_Sheet1.Rows.Get(375).Height = 17F;
            this.SS2_Sheet1.Rows.Get(376).Height = 17F;
            this.SS2_Sheet1.Rows.Get(377).Height = 17F;
            this.SS2_Sheet1.Rows.Get(378).Height = 17F;
            this.SS2_Sheet1.Rows.Get(379).Height = 17F;
            this.SS2_Sheet1.Rows.Get(380).Height = 17F;
            this.SS2_Sheet1.Rows.Get(381).Height = 17F;
            this.SS2_Sheet1.Rows.Get(382).Height = 17F;
            this.SS2_Sheet1.Rows.Get(383).Height = 17F;
            this.SS2_Sheet1.Rows.Get(384).Height = 17F;
            this.SS2_Sheet1.Rows.Get(385).Height = 17F;
            this.SS2_Sheet1.Rows.Get(386).Height = 17F;
            this.SS2_Sheet1.Rows.Get(387).Height = 17F;
            this.SS2_Sheet1.Rows.Get(388).Height = 17F;
            this.SS2_Sheet1.Rows.Get(389).Height = 17F;
            this.SS2_Sheet1.Rows.Get(390).Height = 17F;
            this.SS2_Sheet1.Rows.Get(391).Height = 17F;
            this.SS2_Sheet1.Rows.Get(392).Height = 17F;
            this.SS2_Sheet1.Rows.Get(393).Height = 17F;
            this.SS2_Sheet1.Rows.Get(394).Height = 17F;
            this.SS2_Sheet1.Rows.Get(395).Height = 17F;
            this.SS2_Sheet1.Rows.Get(396).Height = 17F;
            this.SS2_Sheet1.Rows.Get(397).Height = 17F;
            this.SS2_Sheet1.Rows.Get(398).Height = 17F;
            this.SS2_Sheet1.Rows.Get(399).Height = 17F;
            this.SS2_Sheet1.Rows.Get(400).Height = 17F;
            this.SS2_Sheet1.Rows.Get(401).Height = 17F;
            this.SS2_Sheet1.Rows.Get(402).Height = 17F;
            this.SS2_Sheet1.Rows.Get(403).Height = 17F;
            this.SS2_Sheet1.Rows.Get(404).Height = 17F;
            this.SS2_Sheet1.Rows.Get(405).Height = 17F;
            this.SS2_Sheet1.Rows.Get(406).Height = 17F;
            this.SS2_Sheet1.Rows.Get(407).Height = 17F;
            this.SS2_Sheet1.Rows.Get(408).Height = 17F;
            this.SS2_Sheet1.Rows.Get(409).Height = 17F;
            this.SS2_Sheet1.Rows.Get(410).Height = 17F;
            this.SS2_Sheet1.Rows.Get(411).Height = 17F;
            this.SS2_Sheet1.Rows.Get(412).Height = 17F;
            this.SS2_Sheet1.Rows.Get(413).Height = 17F;
            this.SS2_Sheet1.Rows.Get(414).Height = 17F;
            this.SS2_Sheet1.Rows.Get(415).Height = 17F;
            this.SS2_Sheet1.Rows.Get(416).Height = 17F;
            this.SS2_Sheet1.Rows.Get(417).Height = 17F;
            this.SS2_Sheet1.Rows.Get(418).Height = 17F;
            this.SS2_Sheet1.Rows.Get(419).Height = 17F;
            this.SS2_Sheet1.Rows.Get(420).Height = 17F;
            this.SS2_Sheet1.Rows.Get(421).Height = 17F;
            this.SS2_Sheet1.Rows.Get(422).Height = 17F;
            this.SS2_Sheet1.Rows.Get(423).Height = 17F;
            this.SS2_Sheet1.Rows.Get(424).Height = 17F;
            this.SS2_Sheet1.Rows.Get(425).Height = 17F;
            this.SS2_Sheet1.Rows.Get(426).Height = 17F;
            this.SS2_Sheet1.Rows.Get(427).Height = 17F;
            this.SS2_Sheet1.Rows.Get(428).Height = 17F;
            this.SS2_Sheet1.Rows.Get(429).Height = 17F;
            this.SS2_Sheet1.Rows.Get(430).Height = 17F;
            this.SS2_Sheet1.Rows.Get(431).Height = 17F;
            this.SS2_Sheet1.Rows.Get(432).Height = 17F;
            this.SS2_Sheet1.Rows.Get(433).Height = 17F;
            this.SS2_Sheet1.Rows.Get(434).Height = 17F;
            this.SS2_Sheet1.Rows.Get(435).Height = 17F;
            this.SS2_Sheet1.Rows.Get(436).Height = 17F;
            this.SS2_Sheet1.Rows.Get(437).Height = 17F;
            this.SS2_Sheet1.Rows.Get(438).Height = 17F;
            this.SS2_Sheet1.Rows.Get(439).Height = 17F;
            this.SS2_Sheet1.Rows.Get(440).Height = 17F;
            this.SS2_Sheet1.Rows.Get(441).Height = 17F;
            this.SS2_Sheet1.Rows.Get(442).Height = 17F;
            this.SS2_Sheet1.Rows.Get(443).Height = 17F;
            this.SS2_Sheet1.Rows.Get(444).Height = 17F;
            this.SS2_Sheet1.Rows.Get(445).Height = 17F;
            this.SS2_Sheet1.Rows.Get(446).Height = 17F;
            this.SS2_Sheet1.Rows.Get(447).Height = 17F;
            this.SS2_Sheet1.Rows.Get(448).Height = 17F;
            this.SS2_Sheet1.Rows.Get(449).Height = 17F;
            this.SS2_Sheet1.Rows.Get(450).Height = 17F;
            this.SS2_Sheet1.Rows.Get(451).Height = 17F;
            this.SS2_Sheet1.Rows.Get(452).Height = 17F;
            this.SS2_Sheet1.Rows.Get(453).Height = 17F;
            this.SS2_Sheet1.Rows.Get(454).Height = 17F;
            this.SS2_Sheet1.Rows.Get(455).Height = 17F;
            this.SS2_Sheet1.Rows.Get(456).Height = 17F;
            this.SS2_Sheet1.Rows.Get(457).Height = 17F;
            this.SS2_Sheet1.Rows.Get(458).Height = 17F;
            this.SS2_Sheet1.Rows.Get(459).Height = 17F;
            this.SS2_Sheet1.Rows.Get(460).Height = 17F;
            this.SS2_Sheet1.Rows.Get(461).Height = 17F;
            this.SS2_Sheet1.Rows.Get(462).Height = 17F;
            this.SS2_Sheet1.Rows.Get(463).Height = 17F;
            this.SS2_Sheet1.Rows.Get(464).Height = 17F;
            this.SS2_Sheet1.Rows.Get(465).Height = 17F;
            this.SS2_Sheet1.Rows.Get(466).Height = 17F;
            this.SS2_Sheet1.Rows.Get(467).Height = 17F;
            this.SS2_Sheet1.Rows.Get(468).Height = 17F;
            this.SS2_Sheet1.Rows.Get(469).Height = 17F;
            this.SS2_Sheet1.Rows.Get(470).Height = 17F;
            this.SS2_Sheet1.Rows.Get(471).Height = 17F;
            this.SS2_Sheet1.Rows.Get(472).Height = 17F;
            this.SS2_Sheet1.Rows.Get(473).Height = 17F;
            this.SS2_Sheet1.Rows.Get(474).Height = 17F;
            this.SS2_Sheet1.Rows.Get(475).Height = 17F;
            this.SS2_Sheet1.Rows.Get(476).Height = 17F;
            this.SS2_Sheet1.Rows.Get(477).Height = 17F;
            this.SS2_Sheet1.Rows.Get(478).Height = 17F;
            this.SS2_Sheet1.Rows.Get(479).Height = 17F;
            this.SS2_Sheet1.Rows.Get(480).Height = 17F;
            this.SS2_Sheet1.Rows.Get(481).Height = 17F;
            this.SS2_Sheet1.Rows.Get(482).Height = 17F;
            this.SS2_Sheet1.Rows.Get(483).Height = 17F;
            this.SS2_Sheet1.Rows.Get(484).Height = 17F;
            this.SS2_Sheet1.Rows.Get(485).Height = 17F;
            this.SS2_Sheet1.Rows.Get(486).Height = 17F;
            this.SS2_Sheet1.Rows.Get(487).Height = 17F;
            this.SS2_Sheet1.Rows.Get(488).Height = 17F;
            this.SS2_Sheet1.Rows.Get(489).Height = 17F;
            this.SS2_Sheet1.Rows.Get(490).Height = 17F;
            this.SS2_Sheet1.Rows.Get(491).Height = 17F;
            this.SS2_Sheet1.Rows.Get(492).Height = 17F;
            this.SS2_Sheet1.Rows.Get(493).Height = 17F;
            this.SS2_Sheet1.Rows.Get(494).Height = 17F;
            this.SS2_Sheet1.Rows.Get(495).Height = 17F;
            this.SS2_Sheet1.Rows.Get(496).Height = 17F;
            this.SS2_Sheet1.Rows.Get(497).Height = 17F;
            this.SS2_Sheet1.Rows.Get(498).Height = 17F;
            this.SS2_Sheet1.Rows.Get(499).Height = 17F;
            this.SS2_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192))))));
            this.SS2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.rdoGbn2);
            this.panel6.Controls.Add(this.rdoGbn1);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(202, 37);
            this.panel6.TabIndex = 13;
            // 
            // rdoGbn2
            // 
            this.rdoGbn2.AutoSize = true;
            this.rdoGbn2.Location = new System.Drawing.Point(93, 7);
            this.rdoGbn2.Name = "rdoGbn2";
            this.rdoGbn2.Size = new System.Drawing.Size(78, 21);
            this.rdoGbn2.TabIndex = 5;
            this.rdoGbn2.Text = "해부병리";
            this.rdoGbn2.UseVisualStyleBackColor = true;
            // 
            // rdoGbn1
            // 
            this.rdoGbn1.AutoSize = true;
            this.rdoGbn1.Checked = true;
            this.rdoGbn1.Location = new System.Drawing.Point(9, 7);
            this.rdoGbn1.Name = "rdoGbn1";
            this.rdoGbn1.Size = new System.Drawing.Size(78, 21);
            this.rdoGbn1.TabIndex = 4;
            this.rdoGbn1.TabStop = true;
            this.rdoGbn1.Text = "임상병리";
            this.rdoGbn1.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.rdoJob3);
            this.panel5.Controls.Add(this.rdoJob2);
            this.panel5.Controls.Add(this.rdoJob1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 37);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(202, 37);
            this.panel5.TabIndex = 14;
            // 
            // rdoJob3
            // 
            this.rdoJob3.AutoSize = true;
            this.rdoJob3.Location = new System.Drawing.Point(138, 7);
            this.rdoJob3.Name = "rdoJob3";
            this.rdoJob3.Size = new System.Drawing.Size(52, 21);
            this.rdoJob3.TabIndex = 7;
            this.rdoJob3.Text = "전체";
            this.rdoJob3.UseVisualStyleBackColor = true;
            // 
            // rdoJob2
            // 
            this.rdoJob2.AutoSize = true;
            this.rdoJob2.Location = new System.Drawing.Point(80, 7);
            this.rdoJob2.Name = "rdoJob2";
            this.rdoJob2.Size = new System.Drawing.Size(52, 21);
            this.rdoJob2.TabIndex = 5;
            this.rdoJob2.Text = "전송";
            this.rdoJob2.UseVisualStyleBackColor = true;
            // 
            // rdoJob1
            // 
            this.rdoJob1.AutoSize = true;
            this.rdoJob1.Checked = true;
            this.rdoJob1.Location = new System.Drawing.Point(9, 7);
            this.rdoJob1.Name = "rdoJob1";
            this.rdoJob1.Size = new System.Drawing.Size(65, 21);
            this.rdoJob1.TabIndex = 4;
            this.rdoJob1.TabStop = true;
            this.rdoJob1.Text = "미전송";
            this.rdoJob1.UseVisualStyleBackColor = true;
            // 
            // frmHcExResultSend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1047, 705);
            this.Controls.Add(this.panWork);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHcExResultSend";
            this.Text = "검진센터 임상병리 검사 결과전송";
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panWork.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SS2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS2_Sheet1)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panWork;
        private FarPoint.Win.Spread.FpSpread SS1;
        private FarPoint.Win.Spread.SheetView SS1_Sheet1;
        private System.Windows.Forms.Button btnSave_Send;
        private System.Windows.Forms.Button btnDSel_All;
        private System.Windows.Forms.Button btnSel_All;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpTDate;
        private System.Windows.Forms.DateTimePicker dtpFDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnView;
        private FarPoint.Win.Spread.FpSpread SS2;
        private FarPoint.Win.Spread.SheetView SS2_Sheet1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.RadioButton rdoJob3;
        private System.Windows.Forms.RadioButton rdoJob2;
        private System.Windows.Forms.RadioButton rdoJob1;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.RadioButton rdoGbn2;
        private System.Windows.Forms.RadioButton rdoGbn1;
    }
}