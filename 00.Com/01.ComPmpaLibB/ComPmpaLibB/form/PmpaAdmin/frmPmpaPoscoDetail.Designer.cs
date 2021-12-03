namespace ComPmpaLibB
{
    partial class frmPmpaPoscoDetail
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
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType1 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer3 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer4 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.LineBorder lineBorder1 = new FarPoint.Win.LineBorder(System.Drawing.SystemColors.WindowFrame);
            FarPoint.Win.LineBorder lineBorder2 = new FarPoint.Win.LineBorder(System.Drawing.SystemColors.WindowFrame);
            FarPoint.Win.LineBorder lineBorder3 = new FarPoint.Win.LineBorder(System.Drawing.SystemColors.WindowFrame);
            FarPoint.Win.LineBorder lineBorder4 = new FarPoint.Win.LineBorder(System.Drawing.SystemColors.WindowFrame);
            FarPoint.Win.LineBorder lineBorder5 = new FarPoint.Win.LineBorder(System.Drawing.SystemColors.WindowFrame);
            FarPoint.Win.LineBorder lineBorder6 = new FarPoint.Win.LineBorder(System.Drawing.SystemColors.WindowFrame);
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SS_Posco = new FarPoint.Win.Spread.FpSpread();
            this.SS_Posco_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panSub02 = new System.Windows.Forms.Panel();
            this.lbl_Pos_Sts = new System.Windows.Forms.Label();
            this.lblAmt = new System.Windows.Forms.Label();
            this.chkSel = new System.Windows.Forms.CheckBox();
            this.btnSave2 = new System.Windows.Forms.Button();
            this.panTitleSub1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panSub01 = new System.Windows.Forms.Panel();
            this.SS1 = new FarPoint.Win.Spread.FpSpread();
            this.SS1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.CmdView = new System.Windows.Forms.Button();
            this.TxtSDate = new System.Windows.Forms.DateTimePicker();
            this.btnExit = new System.Windows.Forms.Button();
            this.TxtPano = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS_Posco)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS_Posco_Sheet1)).BeginInit();
            this.panSub02.SuspendLayout();
            this.panTitleSub1.SuspendLayout();
            this.panSub01.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.SS_Posco);
            this.panel1.Controls.Add(this.panSub02);
            this.panel1.Controls.Add(this.panTitleSub1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(512, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3);
            this.panel1.Size = new System.Drawing.Size(639, 544);
            this.panel1.TabIndex = 22;
            // 
            // SS_Posco
            // 
            this.SS_Posco.AccessibleDescription = "fpSpread1, Sheet1, Row 0, Column 0, ";
            this.SS_Posco.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SS_Posco.FocusRenderer = flatFocusIndicatorRenderer1;
            this.SS_Posco.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SS_Posco.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SS_Posco.HorizontalScrollBar.Renderer = flatScrollBarRenderer1;
            this.SS_Posco.HorizontalScrollBar.TabIndex = 52;
            this.SS_Posco.Location = new System.Drawing.Point(3, 84);
            this.SS_Posco.Name = "SS_Posco";
            this.SS_Posco.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS_Posco_Sheet1});
            this.SS_Posco.Size = new System.Drawing.Size(631, 455);
            this.SS_Posco.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.SS_Posco.TabIndex = 19;
            this.SS_Posco.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SS_Posco.VerticalScrollBar.Name = "";
            flatScrollBarRenderer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SS_Posco.VerticalScrollBar.Renderer = flatScrollBarRenderer2;
            this.SS_Posco.VerticalScrollBar.TabIndex = 53;
            this.SS_Posco.ButtonClicked += new FarPoint.Win.Spread.EditorNotifyEventHandler(this.SS_Posco_ButtonClicked);
            // 
            // SS_Posco_Sheet1
            // 
            this.SS_Posco_Sheet1.Reset();
            this.SS_Posco_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS_Posco_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS_Posco_Sheet1.ColumnCount = 8;
            this.SS_Posco_Sheet1.RowCount = 50;
            this.SS_Posco_Sheet1.Cells.Get(0, 1).Value = "AA176A";
            this.SS_Posco_Sheet1.Cells.Get(0, 2).Value = "진찰료-초진진찰료금액";
            this.SS_Posco_Sheet1.Cells.Get(0, 3).Value = "999,999,999";
            this.SS_Posco_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS_Posco_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS_Posco_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.SS_Posco_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS_Posco_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS_Posco_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS_Posco_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.SS_Posco_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS_Posco_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "선택";
            this.SS_Posco_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "항목";
            this.SS_Posco_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "명칭";
            this.SS_Posco_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "수가";
            this.SS_Posco_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "코드1";
            this.SS_Posco_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "코드2";
            this.SS_Posco_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "수정";
            this.SS_Posco_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "Rowid";
            this.SS_Posco_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS_Posco_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS_Posco_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.SS_Posco_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS_Posco_Sheet1.ColumnHeader.Rows.Get(0).Height = 25F;
            this.SS_Posco_Sheet1.Columns.Get(0).CellType = checkBoxCellType1;
            this.SS_Posco_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS_Posco_Sheet1.Columns.Get(0).Label = "선택";
            this.SS_Posco_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS_Posco_Sheet1.Columns.Get(0).Width = 36F;
            this.SS_Posco_Sheet1.Columns.Get(1).CellType = textCellType1;
            this.SS_Posco_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS_Posco_Sheet1.Columns.Get(1).Label = "항목";
            this.SS_Posco_Sheet1.Columns.Get(1).Locked = true;
            this.SS_Posco_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS_Posco_Sheet1.Columns.Get(1).Width = 71F;
            this.SS_Posco_Sheet1.Columns.Get(2).CellType = textCellType2;
            this.SS_Posco_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS_Posco_Sheet1.Columns.Get(2).Label = "명칭";
            this.SS_Posco_Sheet1.Columns.Get(2).Locked = true;
            this.SS_Posco_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS_Posco_Sheet1.Columns.Get(2).Width = 166F;
            this.SS_Posco_Sheet1.Columns.Get(3).CellType = textCellType3;
            this.SS_Posco_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS_Posco_Sheet1.Columns.Get(3).Label = "수가";
            this.SS_Posco_Sheet1.Columns.Get(3).Locked = true;
            this.SS_Posco_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS_Posco_Sheet1.Columns.Get(3).Width = 82F;
            this.SS_Posco_Sheet1.Columns.Get(4).CellType = textCellType4;
            this.SS_Posco_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS_Posco_Sheet1.Columns.Get(4).Label = "코드1";
            this.SS_Posco_Sheet1.Columns.Get(4).Locked = true;
            this.SS_Posco_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS_Posco_Sheet1.Columns.Get(4).Width = 84F;
            this.SS_Posco_Sheet1.Columns.Get(5).CellType = textCellType5;
            this.SS_Posco_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS_Posco_Sheet1.Columns.Get(5).Label = "코드2";
            this.SS_Posco_Sheet1.Columns.Get(5).Locked = true;
            this.SS_Posco_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS_Posco_Sheet1.Columns.Get(5).Width = 84F;
            this.SS_Posco_Sheet1.Columns.Get(6).CellType = textCellType6;
            this.SS_Posco_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS_Posco_Sheet1.Columns.Get(6).Label = "수정";
            this.SS_Posco_Sheet1.Columns.Get(6).Locked = true;
            this.SS_Posco_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS_Posco_Sheet1.Columns.Get(6).Width = 36F;
            this.SS_Posco_Sheet1.Columns.Get(7).CellType = textCellType7;
            this.SS_Posco_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS_Posco_Sheet1.Columns.Get(7).Label = "Rowid";
            this.SS_Posco_Sheet1.Columns.Get(7).Locked = true;
            this.SS_Posco_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS_Posco_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS_Posco_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS_Posco_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.SS_Posco_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS_Posco_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS_Posco_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS_Posco_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.SS_Posco_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS_Posco_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SS_Posco_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS_Posco_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS_Posco_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS_Posco_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.SS_Posco_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS_Posco_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS_Posco_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS_Posco_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.SS_Posco_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS_Posco_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SS_Posco_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panSub02
            // 
            this.panSub02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSub02.Controls.Add(this.lbl_Pos_Sts);
            this.panSub02.Controls.Add(this.lblAmt);
            this.panSub02.Controls.Add(this.chkSel);
            this.panSub02.Controls.Add(this.btnSave2);
            this.panSub02.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSub02.Location = new System.Drawing.Point(3, 30);
            this.panSub02.Name = "panSub02";
            this.panSub02.Size = new System.Drawing.Size(631, 54);
            this.panSub02.TabIndex = 17;
            // 
            // lbl_Pos_Sts
            // 
            this.lbl_Pos_Sts.AutoSize = true;
            this.lbl_Pos_Sts.Location = new System.Drawing.Point(18, 30);
            this.lbl_Pos_Sts.Name = "lbl_Pos_Sts";
            this.lbl_Pos_Sts.Size = new System.Drawing.Size(221, 12);
            this.lbl_Pos_Sts.TabIndex = 13;
            this.lbl_Pos_Sts.Text = "81000004  2999-12-31  고유번호: 999999";
            // 
            // lblAmt
            // 
            this.lblAmt.AutoSize = true;
            this.lblAmt.BackColor = System.Drawing.Color.Wheat;
            this.lblAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAmt.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblAmt.Location = new System.Drawing.Point(131, 6);
            this.lblAmt.Name = "lblAmt";
            this.lblAmt.Size = new System.Drawing.Size(72, 19);
            this.lblAmt.TabIndex = 12;
            this.lblAmt.Text = "9,999,999";
            // 
            // chkSel
            // 
            this.chkSel.AutoSize = true;
            this.chkSel.Location = new System.Drawing.Point(21, 5);
            this.chkSel.Name = "chkSel";
            this.chkSel.Size = new System.Drawing.Size(84, 16);
            this.chkSel.TabIndex = 11;
            this.chkSel.Text = "선택한것만";
            this.chkSel.UseVisualStyleBackColor = true;
            // 
            // btnSave2
            // 
            this.btnSave2.BackColor = System.Drawing.Color.Transparent;
            this.btnSave2.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave2.Location = new System.Drawing.Point(539, 0);
            this.btnSave2.Name = "btnSave2";
            this.btnSave2.Size = new System.Drawing.Size(90, 52);
            this.btnSave2.TabIndex = 10;
            this.btnSave2.Text = "저장";
            this.btnSave2.UseVisualStyleBackColor = false;
            // 
            // panTitleSub1
            // 
            this.panTitleSub1.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub1.Controls.Add(this.label1);
            this.panTitleSub1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub1.Location = new System.Drawing.Point(3, 3);
            this.panTitleSub1.Name = "panTitleSub1";
            this.panTitleSub1.Padding = new System.Windows.Forms.Padding(3);
            this.panTitleSub1.Size = new System.Drawing.Size(631, 27);
            this.panTitleSub1.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(223, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "포스코 위탁검사 비용 청구서 설정";
            // 
            // panSub01
            // 
            this.panSub01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSub01.Controls.Add(this.SS1);
            this.panSub01.Controls.Add(this.panel2);
            this.panSub01.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panSub01.Location = new System.Drawing.Point(0, 0);
            this.panSub01.Name = "panSub01";
            this.panSub01.Padding = new System.Windows.Forms.Padding(2);
            this.panSub01.Size = new System.Drawing.Size(512, 544);
            this.panSub01.TabIndex = 24;
            // 
            // SS1
            // 
            this.SS1.AccessibleDescription = "SS1, Sheet1, Row 0, Column 0, ";
            this.SS1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SS1.FocusRenderer = flatFocusIndicatorRenderer1;
            this.SS1.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SS1.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SS1.HorizontalScrollBar.Renderer = flatScrollBarRenderer3;
            this.SS1.HorizontalScrollBar.TabIndex = 69;
            this.SS1.Location = new System.Drawing.Point(2, 73);
            this.SS1.Name = "SS1";
            this.SS1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS1_Sheet1});
            this.SS1.Size = new System.Drawing.Size(506, 467);
            this.SS1.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.SS1.TabIndex = 26;
            this.SS1.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SS1.VerticalScrollBar.Name = "";
            flatScrollBarRenderer4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SS1.VerticalScrollBar.Renderer = flatScrollBarRenderer4;
            this.SS1.VerticalScrollBar.TabIndex = 70;
            this.SS1.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SS1_CellDoubleClick);
            // 
            // SS1_Sheet1
            // 
            this.SS1_Sheet1.Reset();
            this.SS1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS1_Sheet1.ColumnCount = 7;
            this.SS1_Sheet1.RowCount = 1;
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 0).Border = lineBorder1;
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "등록번호";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 1).Border = lineBorder2;
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "접수일자";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 2).Border = lineBorder3;
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "성명";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 3).Border = lineBorder4;
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "성별";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 4).Border = lineBorder5;
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "종류";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 5).Border = lineBorder6;
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "고유번호";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "rowid";
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.Columns.Get(0).CellType = textCellType8;
            this.SS1_Sheet1.Columns.Get(0).Label = "등록번호";
            this.SS1_Sheet1.Columns.Get(0).Width = 81F;
            this.SS1_Sheet1.Columns.Get(1).Label = "접수일자";
            this.SS1_Sheet1.Columns.Get(1).Width = 81F;
            this.SS1_Sheet1.Columns.Get(2).Label = "성명";
            this.SS1_Sheet1.Columns.Get(2).Width = 81F;
            this.SS1_Sheet1.Columns.Get(3).Label = "성별";
            this.SS1_Sheet1.Columns.Get(3).Width = 61F;
            this.SS1_Sheet1.Columns.Get(4).Label = "종류";
            this.SS1_Sheet1.Columns.Get(4).Width = 61F;
            this.SS1_Sheet1.Columns.Get(5).Label = "고유번호";
            this.SS1_Sheet1.Columns.Get(5).Width = 81F;
            this.SS1_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.SS1_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.SS1_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS1_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.SS1_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.SS1_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.CmdView);
            this.panel2.Controls.Add(this.TxtSDate);
            this.panel2.Controls.Add(this.btnExit);
            this.panel2.Controls.Add(this.TxtPano);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(2, 2);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(2);
            this.panel2.Size = new System.Drawing.Size(506, 71);
            this.panel2.TabIndex = 25;
            // 
            // CmdView
            // 
            this.CmdView.BackColor = System.Drawing.Color.Transparent;
            this.CmdView.Dock = System.Windows.Forms.DockStyle.Right;
            this.CmdView.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.CmdView.Location = new System.Drawing.Point(311, 2);
            this.CmdView.Name = "CmdView";
            this.CmdView.Size = new System.Drawing.Size(80, 65);
            this.CmdView.TabIndex = 14;
            this.CmdView.Text = "조회";
            this.CmdView.UseVisualStyleBackColor = false;
            // 
            // TxtSDate
            // 
            this.TxtSDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.TxtSDate.Location = new System.Drawing.Point(30, 41);
            this.TxtSDate.Name = "TxtSDate";
            this.TxtSDate.Size = new System.Drawing.Size(136, 21);
            this.TxtSDate.TabIndex = 13;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(391, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(111, 65);
            this.btnExit.TabIndex = 11;
            this.btnExit.Text = "닫기(&X)";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // TxtPano
            // 
            this.TxtPano.Location = new System.Drawing.Point(30, 8);
            this.TxtPano.Name = "TxtPano";
            this.TxtPano.Size = new System.Drawing.Size(69, 21);
            this.TxtPano.TabIndex = 10;
            this.TxtPano.Text = "81000004";
            this.TxtPano.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // frmPmpaPoscoDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1151, 544);
            this.Controls.Add(this.panSub01);
            this.Controls.Add(this.panel1);
            this.Name = "frmPmpaPoscoDetail";
            this.Text = "frmPmpaPoscoDetail";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SS_Posco)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS_Posco_Sheet1)).EndInit();
            this.panSub02.ResumeLayout(false);
            this.panSub02.PerformLayout();
            this.panTitleSub1.ResumeLayout(false);
            this.panTitleSub1.PerformLayout();
            this.panSub01.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private FarPoint.Win.Spread.FpSpread SS_Posco;
        private FarPoint.Win.Spread.SheetView SS_Posco_Sheet1;
        private System.Windows.Forms.Panel panSub02;
        private System.Windows.Forms.Label lbl_Pos_Sts;
        private System.Windows.Forms.Label lblAmt;
        private System.Windows.Forms.CheckBox chkSel;
        private System.Windows.Forms.Button btnSave2;
        private System.Windows.Forms.Panel panTitleSub1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panSub01;
        private FarPoint.Win.Spread.FpSpread SS1;
        private FarPoint.Win.Spread.SheetView SS1_Sheet1;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox TxtPano;
        private System.Windows.Forms.Button CmdView;
        private System.Windows.Forms.DateTimePicker TxtSDate;
    }
}