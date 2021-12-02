namespace ComEmrBase
{
    partial class frmEmrBaseViewVitalandActingItem
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType17 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType18 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType19 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType20 = new FarPoint.Win.Spread.CellType.TextCellType();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEmrBaseViewVitalandActingItem));
            FarPoint.Win.Spread.CellType.TextCellType textCellType21 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType22 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType23 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType24 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.optCvc = new System.Windows.Forms.RadioButton();
            this.optBedsore = new System.Windows.Forms.RadioButton();
            this.optWound = new System.Windows.Forms.RadioButton();
            this.optSpecial = new System.Windows.Forms.RadioButton();
            this.optAct = new System.Windows.Forms.RadioButton();
            this.optVital = new System.Windows.Forms.RadioButton();
            this.mbtnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ssForm = new FarPoint.Win.Spread.FpSpread();
            this.ssForm_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.picIconBar1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.ssFormUser = new FarPoint.Win.Spread.FpSpread();
            this.ssFormUser_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panUseGb = new System.Windows.Forms.Panel();
            this.optUser = new System.Windows.Forms.RadioButton();
            this.optDept = new System.Windows.Forms.RadioButton();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.optSpecial2 = new System.Windows.Forms.RadioButton();
            this.panTitle.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssForm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssForm_Sheet1)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIconBar1)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssFormUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssFormUser_Sheet1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panUseGb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.optCvc);
            this.panTitle.Controls.Add(this.optBedsore);
            this.panTitle.Controls.Add(this.optWound);
            this.panTitle.Controls.Add(this.optSpecial2);
            this.panTitle.Controls.Add(this.optSpecial);
            this.panTitle.Controls.Add(this.optAct);
            this.panTitle.Controls.Add(this.optVital);
            this.panTitle.Controls.Add(this.mbtnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(950, 38);
            this.panTitle.TabIndex = 11;
            // 
            // optCvc
            // 
            this.optCvc.AutoSize = true;
            this.optCvc.Location = new System.Drawing.Point(748, 9);
            this.optCvc.Name = "optCvc";
            this.optCvc.Size = new System.Drawing.Size(91, 21);
            this.optCvc.TabIndex = 70;
            this.optCvc.TabStop = true;
            this.optCvc.Text = "중심정맥관";
            this.optCvc.UseVisualStyleBackColor = true;
            this.optCvc.CheckedChanged += new System.EventHandler(this.optCvc_CheckedChanged);
            // 
            // optBedsore
            // 
            this.optBedsore.AutoSize = true;
            this.optBedsore.Location = new System.Drawing.Point(688, 9);
            this.optBedsore.Name = "optBedsore";
            this.optBedsore.Size = new System.Drawing.Size(52, 21);
            this.optBedsore.TabIndex = 70;
            this.optBedsore.TabStop = true;
            this.optBedsore.Text = "욕창";
            this.optBedsore.UseVisualStyleBackColor = true;
            this.optBedsore.CheckedChanged += new System.EventHandler(this.optBedsore_CheckedChanged);
            // 
            // optWound
            // 
            this.optWound.AutoSize = true;
            this.optWound.Location = new System.Drawing.Point(628, 9);
            this.optWound.Name = "optWound";
            this.optWound.Size = new System.Drawing.Size(52, 21);
            this.optWound.TabIndex = 70;
            this.optWound.TabStop = true;
            this.optWound.Text = "상처";
            this.optWound.UseVisualStyleBackColor = true;
            this.optWound.CheckedChanged += new System.EventHandler(this.optWound_CheckedChanged);
            // 
            // optSpecial
            // 
            this.optSpecial.AutoSize = true;
            this.optSpecial.Location = new System.Drawing.Point(363, 9);
            this.optSpecial.Name = "optSpecial";
            this.optSpecial.Size = new System.Drawing.Size(109, 21);
            this.optSpecial.TabIndex = 70;
            this.optSpecial.TabStop = true;
            this.optSpecial.Text = "호흡/인공기도";
            this.optSpecial.UseVisualStyleBackColor = true;
            this.optSpecial.CheckedChanged += new System.EventHandler(this.optSpecial_CheckedChanged);
            // 
            // optAct
            // 
            this.optAct.AutoSize = true;
            this.optAct.Location = new System.Drawing.Point(191, 9);
            this.optAct.Name = "optAct";
            this.optAct.Size = new System.Drawing.Size(78, 21);
            this.optAct.TabIndex = 70;
            this.optAct.TabStop = true;
            this.optAct.Text = "간호활동";
            this.optAct.UseVisualStyleBackColor = true;
            this.optAct.CheckedChanged += new System.EventHandler(this.optAct_CheckedChanged);
            // 
            // optVital
            // 
            this.optVital.AutoSize = true;
            this.optVital.Location = new System.Drawing.Point(277, 9);
            this.optVital.Name = "optVital";
            this.optVital.Size = new System.Drawing.Size(78, 21);
            this.optVital.TabIndex = 69;
            this.optVital.TabStop = true;
            this.optVital.Text = "임상관찰";
            this.optVital.UseVisualStyleBackColor = true;
            this.optVital.CheckedChanged += new System.EventHandler(this.optVital_CheckedChanged);
            // 
            // mbtnExit
            // 
            this.mbtnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mbtnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.mbtnExit.ForeColor = System.Drawing.Color.Blue;
            this.mbtnExit.Location = new System.Drawing.Point(854, 3);
            this.mbtnExit.Name = "mbtnExit";
            this.mbtnExit.Size = new System.Drawing.Size(92, 30);
            this.mbtnExit.TabIndex = 68;
            this.mbtnExit.Text = "닫기";
            this.mbtnExit.UseVisualStyleBackColor = true;
            this.mbtnExit.Click += new System.EventHandler(this.mbtnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(12, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(173, 16);
            this.lblTitle.TabIndex = 14;
            this.lblTitle.Text = "병동별 임상관찰 관리";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.ssForm);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 38);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(475, 657);
            this.panel3.TabIndex = 12;
            // 
            // ssForm
            // 
            this.ssForm.AccessibleDescription = "ssForm, Sheet1, Row 0, Column 0, ";
            this.ssForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssForm.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssForm.Location = new System.Drawing.Point(0, 40);
            this.ssForm.Name = "ssForm";
            this.ssForm.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssForm_Sheet1});
            this.ssForm.Size = new System.Drawing.Size(475, 617);
            this.ssForm.TabIndex = 13;
            this.ssForm.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssForm_CellDoubleClick);
            // 
            // ssForm_Sheet1
            // 
            this.ssForm_Sheet1.Reset();
            this.ssForm_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssForm_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssForm_Sheet1.ColumnCount = 3;
            this.ssForm_Sheet1.RowCount = 1;
            this.ssForm_Sheet1.ActiveSkin = new FarPoint.Win.Spread.SheetSkin("CustomSkin1", System.Drawing.Color.White, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.LightGray, FarPoint.Win.Spread.GridLines.Both, System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237))))), System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, false, false, false, false, false, true, false, true, "HeaderDefault", "HeaderDefault", "HeaderDefault", "DataAreaDefault", "HeaderDefault");
            this.ssForm_Sheet1.Cells.Get(0, 0).Locked = false;
            this.ssForm_Sheet1.Cells.Get(0, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssForm_Sheet1.Cells.Get(0, 1).Locked = false;
            this.ssForm_Sheet1.Cells.Get(0, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssForm_Sheet1.Cells.Get(0, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssForm_Sheet1.Cells.Get(0, 2).Locked = false;
            this.ssForm_Sheet1.Cells.Get(0, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssForm_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssForm_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssForm_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssForm_Sheet1.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.ssForm_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssForm_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssForm_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssForm_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssForm_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssForm_Sheet1.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.ssForm_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssForm_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssForm_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "ItemCode";
            this.ssForm_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "그  룹";
            this.ssForm_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "항 목";
            this.ssForm_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssForm_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssForm_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssForm_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssForm_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssForm_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssForm_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssForm_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssForm_Sheet1.ColumnHeader.Rows.Get(0).Height = 30F;
            this.ssForm_Sheet1.Columns.Get(0).CellType = textCellType17;
            this.ssForm_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssForm_Sheet1.Columns.Get(0).Label = "ItemCode";
            this.ssForm_Sheet1.Columns.Get(0).Locked = true;
            this.ssForm_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssForm_Sheet1.Columns.Get(0).Visible = false;
            this.ssForm_Sheet1.Columns.Get(0).Width = 166F;
            textCellType18.MaxLength = 400;
            textCellType18.Multiline = true;
            textCellType18.WordWrap = true;
            this.ssForm_Sheet1.Columns.Get(1).CellType = textCellType18;
            this.ssForm_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssForm_Sheet1.Columns.Get(1).Label = "그  룹";
            this.ssForm_Sheet1.Columns.Get(1).Locked = true;
            this.ssForm_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssForm_Sheet1.Columns.Get(1).Width = 135F;
            textCellType19.MaxLength = 400;
            textCellType19.Multiline = true;
            textCellType19.WordWrap = true;
            this.ssForm_Sheet1.Columns.Get(2).CellType = textCellType19;
            this.ssForm_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssForm_Sheet1.Columns.Get(2).Label = "항 목";
            this.ssForm_Sheet1.Columns.Get(2).Locked = true;
            this.ssForm_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssForm_Sheet1.Columns.Get(2).Width = 290F;
            this.ssForm_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssForm_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssForm_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.ssForm_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssForm_Sheet1.FilterBar.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssForm_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssForm_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssForm_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssForm_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssForm_Sheet1.FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssForm_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssForm_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssForm_Sheet1.RowHeader.Columns.Get(0).Width = 29F;
            this.ssForm_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssForm_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssForm_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssForm_Sheet1.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssForm_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssForm_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssForm_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssForm_Sheet1.Rows.Get(0).CellType = textCellType20;
            this.ssForm_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssForm_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssForm_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssForm_Sheet1.SheetCornerStyle.Parent = "HeaderDefault";
            this.ssForm_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssForm_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssForm_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.picIconBar1);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(475, 40);
            this.panel4.TabIndex = 8;
            // 
            // picIconBar1
            // 
            this.picIconBar1.Image = ((System.Drawing.Image)(resources.GetObject("picIconBar1.Image")));
            this.picIconBar1.InitialImage = null;
            this.picIconBar1.Location = new System.Drawing.Point(5, 10);
            this.picIconBar1.Name = "picIconBar1";
            this.picIconBar1.Size = new System.Drawing.Size(16, 16);
            this.picIconBar1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picIconBar1.TabIndex = 65;
            this.picIconBar1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(21, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "항목 조회";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.ssFormUser);
            this.panel5.Controls.Add(this.panel1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(475, 38);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(475, 657);
            this.panel5.TabIndex = 13;
            // 
            // ssFormUser
            // 
            this.ssFormUser.AccessibleDescription = "ssForm, Sheet1, Row 0, Column 0, ";
            this.ssFormUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssFormUser.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssFormUser.Location = new System.Drawing.Point(0, 40);
            this.ssFormUser.Name = "ssFormUser";
            this.ssFormUser.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssFormUser_Sheet1});
            this.ssFormUser.Size = new System.Drawing.Size(475, 617);
            this.ssFormUser.TabIndex = 14;
            this.ssFormUser.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssFormUser_CellDoubleClick);
            // 
            // ssFormUser_Sheet1
            // 
            this.ssFormUser_Sheet1.Reset();
            this.ssFormUser_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssFormUser_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssFormUser_Sheet1.ColumnCount = 3;
            this.ssFormUser_Sheet1.RowCount = 1;
            this.ssFormUser_Sheet1.ActiveSkin = new FarPoint.Win.Spread.SheetSkin("CustomSkin1", System.Drawing.Color.White, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.LightGray, FarPoint.Win.Spread.GridLines.Both, System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237))))), System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, false, false, false, false, false, true, false, true, "HeaderDefault", "HeaderDefault", "HeaderDefault", "DataAreaDefault", "HeaderDefault");
            this.ssFormUser_Sheet1.Cells.Get(0, 0).Locked = false;
            this.ssFormUser_Sheet1.Cells.Get(0, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssFormUser_Sheet1.Cells.Get(0, 1).Locked = false;
            this.ssFormUser_Sheet1.Cells.Get(0, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssFormUser_Sheet1.Cells.Get(0, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssFormUser_Sheet1.Cells.Get(0, 2).Locked = false;
            this.ssFormUser_Sheet1.Cells.Get(0, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssFormUser_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssFormUser_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssFormUser_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssFormUser_Sheet1.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.ssFormUser_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssFormUser_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssFormUser_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssFormUser_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssFormUser_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssFormUser_Sheet1.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.ssFormUser_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssFormUser_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssFormUser_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "ItemCode";
            this.ssFormUser_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "그  룹";
            this.ssFormUser_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "항 목";
            this.ssFormUser_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssFormUser_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssFormUser_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssFormUser_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssFormUser_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssFormUser_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssFormUser_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssFormUser_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssFormUser_Sheet1.ColumnHeader.Rows.Get(0).Height = 30F;
            this.ssFormUser_Sheet1.Columns.Get(0).CellType = textCellType21;
            this.ssFormUser_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssFormUser_Sheet1.Columns.Get(0).Label = "ItemCode";
            this.ssFormUser_Sheet1.Columns.Get(0).Locked = true;
            this.ssFormUser_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssFormUser_Sheet1.Columns.Get(0).Visible = false;
            this.ssFormUser_Sheet1.Columns.Get(0).Width = 166F;
            textCellType22.MaxLength = 400;
            textCellType22.Multiline = true;
            textCellType22.WordWrap = true;
            this.ssFormUser_Sheet1.Columns.Get(1).CellType = textCellType22;
            this.ssFormUser_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssFormUser_Sheet1.Columns.Get(1).Label = "그  룹";
            this.ssFormUser_Sheet1.Columns.Get(1).Locked = true;
            this.ssFormUser_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssFormUser_Sheet1.Columns.Get(1).Width = 135F;
            textCellType23.MaxLength = 400;
            textCellType23.Multiline = true;
            textCellType23.WordWrap = true;
            this.ssFormUser_Sheet1.Columns.Get(2).CellType = textCellType23;
            this.ssFormUser_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssFormUser_Sheet1.Columns.Get(2).Label = "항 목";
            this.ssFormUser_Sheet1.Columns.Get(2).Locked = true;
            this.ssFormUser_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssFormUser_Sheet1.Columns.Get(2).Width = 290F;
            this.ssFormUser_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssFormUser_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssFormUser_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.ssFormUser_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssFormUser_Sheet1.FilterBar.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssFormUser_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssFormUser_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssFormUser_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssFormUser_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssFormUser_Sheet1.FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssFormUser_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssFormUser_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssFormUser_Sheet1.RowHeader.Columns.Get(0).Width = 29F;
            this.ssFormUser_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssFormUser_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssFormUser_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssFormUser_Sheet1.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssFormUser_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssFormUser_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssFormUser_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssFormUser_Sheet1.Rows.Get(0).CellType = textCellType24;
            this.ssFormUser_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssFormUser_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssFormUser_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssFormUser_Sheet1.SheetCornerStyle.Parent = "HeaderDefault";
            this.ssFormUser_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssFormUser_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssFormUser_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.panUseGb);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(475, 40);
            this.panel1.TabIndex = 8;
            // 
            // panUseGb
            // 
            this.panUseGb.Controls.Add(this.optUser);
            this.panUseGb.Controls.Add(this.optDept);
            this.panUseGb.Location = new System.Drawing.Point(132, 3);
            this.panUseGb.Name = "panUseGb";
            this.panUseGb.Size = new System.Drawing.Size(197, 32);
            this.panUseGb.TabIndex = 66;
            this.panUseGb.Visible = false;
            // 
            // optUser
            // 
            this.optUser.AutoSize = true;
            this.optUser.Checked = true;
            this.optUser.Location = new System.Drawing.Point(19, 5);
            this.optUser.Name = "optUser";
            this.optUser.Size = new System.Drawing.Size(78, 21);
            this.optUser.TabIndex = 72;
            this.optUser.TabStop = true;
            this.optUser.Text = "사용자별";
            this.optUser.UseVisualStyleBackColor = true;
            this.optUser.CheckedChanged += new System.EventHandler(this.optUser_CheckedChanged);
            // 
            // optDept
            // 
            this.optDept.AutoSize = true;
            this.optDept.Location = new System.Drawing.Point(103, 5);
            this.optDept.Name = "optDept";
            this.optDept.Size = new System.Drawing.Size(65, 21);
            this.optDept.TabIndex = 71;
            this.optDept.TabStop = true;
            this.optDept.Text = "부서별";
            this.optDept.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.InitialImage = null;
            this.pictureBox2.Location = new System.Drawing.Point(5, 10);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(16, 16);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 65;
            this.pictureBox2.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(21, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "사용자별 항목";
            // 
            // optSpecial2
            // 
            this.optSpecial2.AutoSize = true;
            this.optSpecial2.Location = new System.Drawing.Point(480, 9);
            this.optSpecial2.Name = "optSpecial2";
            this.optSpecial2.Size = new System.Drawing.Size(140, 21);
            this.optSpecial2.TabIndex = 70;
            this.optSpecial2.TabStop = true;
            this.optSpecial2.Text = "산소/인공호흡/흡인";
            this.optSpecial2.UseVisualStyleBackColor = true;
            this.optSpecial2.CheckedChanged += new System.EventHandler(this.optSpecial_CheckedChanged);
            // 
            // frmEmrBaseViewVitalandActingItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 695);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmEmrBaseViewVitalandActingItem";
            this.Text = "frmEmrBaseViewVitalandActingItem";
            this.Load += new System.EventHandler(this.frmEmrBaseViewVitalandActingItem_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssForm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssForm_Sheet1)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIconBar1)).EndInit();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssFormUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssFormUser_Sheet1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panUseGb.ResumeLayout(false);
            this.panUseGb.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button mbtnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.PictureBox picIconBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton optAct;
        private System.Windows.Forms.RadioButton optVital;
        private FarPoint.Win.Spread.FpSpread ssForm;
        private FarPoint.Win.Spread.SheetView ssForm_Sheet1;
        private FarPoint.Win.Spread.FpSpread ssFormUser;
        private FarPoint.Win.Spread.SheetView ssFormUser_Sheet1;
        private System.Windows.Forms.Panel panUseGb;
        private System.Windows.Forms.RadioButton optUser;
        private System.Windows.Forms.RadioButton optDept;
        private System.Windows.Forms.RadioButton optSpecial;
        private System.Windows.Forms.RadioButton optWound;
        private System.Windows.Forms.RadioButton optCvc;
        private System.Windows.Forms.RadioButton optBedsore;
        private System.Windows.Forms.RadioButton optSpecial2;
    }
}