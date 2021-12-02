namespace ComEmrBase
{
    partial class frmNrActingSetNew
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType13 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType14 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType15 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType16 = new FarPoint.Win.Spread.CellType.TextCellType();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNrActingSetNew));
            FarPoint.Win.Spread.CellType.TextCellType textCellType17 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.ComboBoxCellType comboBoxCellType3 = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType18 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType19 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType20 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType21 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType22 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.ComboBoxCellType comboBoxCellType4 = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType23 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType24 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnSaveSet = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.dtpVitalDate = new System.Windows.Forms.DateTimePicker();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ssForm = new FarPoint.Win.Spread.FpSpread();
            this.ssForm_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cboWard = new System.Windows.Forms.ComboBox();
            this.picIconBar1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.ssActForm = new FarPoint.Win.Spread.FpSpread();
            this.ssActForm_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnDeSelect = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panTitle.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssForm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssForm_Sheet1)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIconBar1)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssActForm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssActForm_Sheet1)).BeginInit();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnSaveSet);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Controls.Add(this.dtpVitalDate);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1010, 38);
            this.panTitle.TabIndex = 8;
            // 
            // btnSaveSet
            // 
            this.btnSaveSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveSet.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSaveSet.ForeColor = System.Drawing.Color.Blue;
            this.btnSaveSet.Location = new System.Drawing.Point(911, 3);
            this.btnSaveSet.Name = "btnSaveSet";
            this.btnSaveSet.Size = new System.Drawing.Size(92, 30);
            this.btnSaveSet.TabIndex = 66;
            this.btnSaveSet.Text = "닫기";
            this.btnSaveSet.UseVisualStyleBackColor = true;
            this.btnSaveSet.Click += new System.EventHandler(this.btnSaveSet_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(637, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(92, 30);
            this.btnExit.TabIndex = 19;
            this.btnExit.Text = "닫  기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Visible = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(12, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(106, 21);
            this.lblTitle.TabIndex = 14;
            this.lblTitle.Text = "간호활동관리";
            // 
            // dtpVitalDate
            // 
            this.dtpVitalDate.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtpVitalDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpVitalDate.Location = new System.Drawing.Point(471, 7);
            this.dtpVitalDate.Name = "dtpVitalDate";
            this.dtpVitalDate.Size = new System.Drawing.Size(112, 21);
            this.dtpVitalDate.TabIndex = 67;
            this.dtpVitalDate.ValueChanged += new System.EventHandler(this.dtpVitalDate_ValueChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.ssForm);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 38);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(455, 691);
            this.panel3.TabIndex = 10;
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
            this.ssForm.Size = new System.Drawing.Size(455, 651);
            this.ssForm.TabIndex = 10;
            this.ssForm.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssForm_CellClick);
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
            this.ssForm_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "항  목";
            this.ssForm_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssForm_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssForm_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssForm_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssForm_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssForm_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssForm_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssForm_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssForm_Sheet1.ColumnHeader.Rows.Get(0).Height = 30F;
            this.ssForm_Sheet1.Columns.Get(0).CellType = textCellType13;
            this.ssForm_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssForm_Sheet1.Columns.Get(0).Label = "ItemCode";
            this.ssForm_Sheet1.Columns.Get(0).Locked = true;
            this.ssForm_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssForm_Sheet1.Columns.Get(0).Visible = false;
            this.ssForm_Sheet1.Columns.Get(0).Width = 166F;
            textCellType14.Multiline = true;
            textCellType14.WordWrap = true;
            this.ssForm_Sheet1.Columns.Get(1).CellType = textCellType14;
            this.ssForm_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssForm_Sheet1.Columns.Get(1).Label = "그  룹";
            this.ssForm_Sheet1.Columns.Get(1).Locked = true;
            this.ssForm_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssForm_Sheet1.Columns.Get(1).Width = 100F;
            textCellType15.MaxLength = 400;
            textCellType15.Multiline = true;
            textCellType15.WordWrap = true;
            this.ssForm_Sheet1.Columns.Get(2).CellType = textCellType15;
            this.ssForm_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssForm_Sheet1.Columns.Get(2).Label = "항  목";
            this.ssForm_Sheet1.Columns.Get(2).Locked = true;
            this.ssForm_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssForm_Sheet1.Columns.Get(2).Width = 305F;
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
            this.ssForm_Sheet1.Rows.Get(0).CellType = textCellType16;
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
            this.panel4.Controls.Add(this.btnSearch);
            this.panel4.Controls.Add(this.txtSearch);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.cboWard);
            this.panel4.Controls.Add(this.picIconBar1);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(455, 40);
            this.panel4.TabIndex = 8;
            // 
            // cboWard
            // 
            this.cboWard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWard.FormattingEnabled = true;
            this.cboWard.Location = new System.Drawing.Point(132, 7);
            this.cboWard.Name = "cboWard";
            this.cboWard.Size = new System.Drawing.Size(84, 25);
            this.cboWard.TabIndex = 201;
            this.cboWard.SelectedIndexChanged += new System.EventHandler(this.cboWard_SelectedIndexChanged);
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
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(21, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 17);
            this.label1.TabIndex = 21;
            this.label1.Text = "항목 조회";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.ssActForm);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(455, 38);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(555, 691);
            this.panel5.TabIndex = 11;
            // 
            // ssActForm
            // 
            this.ssActForm.AccessibleDescription = "ssActForm, Sheet1, Row 0, Column 0, ";
            this.ssActForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssActForm.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssActForm.Location = new System.Drawing.Point(0, 40);
            this.ssActForm.Name = "ssActForm";
            this.ssActForm.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssActForm_Sheet1});
            this.ssActForm.Size = new System.Drawing.Size(555, 651);
            this.ssActForm.TabIndex = 11;
            this.ssActForm.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssActForm_CellDoubleClick);
            this.ssActForm.ComboCloseUp += new FarPoint.Win.Spread.EditorNotifyEventHandler(this.ssActForm_ComboCloseUp);
            // 
            // ssActForm_Sheet1
            // 
            this.ssActForm_Sheet1.Reset();
            this.ssActForm_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssActForm_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssActForm_Sheet1.ColumnCount = 6;
            this.ssActForm_Sheet1.RowCount = 1;
            this.ssActForm_Sheet1.ActiveSkin = new FarPoint.Win.Spread.SheetSkin("CustomSkin1", System.Drawing.Color.White, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.LightGray, FarPoint.Win.Spread.GridLines.Both, System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237))))), System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, false, false, false, false, false, true, false, true, "HeaderDefault", "HeaderDefault", "HeaderDefault", "DataAreaDefault", "HeaderDefault");
            this.ssActForm_Sheet1.Cells.Get(0, 0).Locked = false;
            this.ssActForm_Sheet1.Cells.Get(0, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssActForm_Sheet1.Cells.Get(0, 1).Locked = false;
            this.ssActForm_Sheet1.Cells.Get(0, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssActForm_Sheet1.Cells.Get(0, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssActForm_Sheet1.Cells.Get(0, 2).Locked = false;
            this.ssActForm_Sheet1.Cells.Get(0, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssActForm_Sheet1.Cells.Get(0, 3).CellType = textCellType17;
            this.ssActForm_Sheet1.Cells.Get(0, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssActForm_Sheet1.Cells.Get(0, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            comboBoxCellType3.AllowEditorVerticalAlign = true;
            comboBoxCellType3.ButtonAlign = FarPoint.Win.ButtonAlign.Right;
            this.ssActForm_Sheet1.Cells.Get(0, 4).CellType = comboBoxCellType3;
            this.ssActForm_Sheet1.Cells.Get(0, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssActForm_Sheet1.Cells.Get(0, 4).Value = "12회";
            this.ssActForm_Sheet1.Cells.Get(0, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssActForm_Sheet1.Cells.Get(0, 5).CellType = textCellType18;
            this.ssActForm_Sheet1.Cells.Get(0, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssActForm_Sheet1.Cells.Get(0, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssActForm_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssActForm_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssActForm_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssActForm_Sheet1.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.ssActForm_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssActForm_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssActForm_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssActForm_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssActForm_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssActForm_Sheet1.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.ssActForm_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssActForm_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssActForm_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "ItemCode";
            this.ssActForm_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "그  룹";
            this.ssActForm_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "항  목";
            this.ssActForm_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "간격";
            this.ssActForm_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "구분";
            this.ssActForm_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "횟수";
            this.ssActForm_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssActForm_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssActForm_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssActForm_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssActForm_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssActForm_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssActForm_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssActForm_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssActForm_Sheet1.ColumnHeader.Rows.Get(0).Height = 30F;
            this.ssActForm_Sheet1.Columns.Get(0).CellType = textCellType19;
            this.ssActForm_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssActForm_Sheet1.Columns.Get(0).Label = "ItemCode";
            this.ssActForm_Sheet1.Columns.Get(0).Locked = true;
            this.ssActForm_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssActForm_Sheet1.Columns.Get(0).Visible = false;
            this.ssActForm_Sheet1.Columns.Get(0).Width = 166F;
            textCellType20.Multiline = true;
            textCellType20.WordWrap = true;
            this.ssActForm_Sheet1.Columns.Get(1).CellType = textCellType20;
            this.ssActForm_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssActForm_Sheet1.Columns.Get(1).Label = "그  룹";
            this.ssActForm_Sheet1.Columns.Get(1).Locked = true;
            this.ssActForm_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssActForm_Sheet1.Columns.Get(1).Width = 106F;
            textCellType21.MaxLength = 400;
            textCellType21.Multiline = true;
            textCellType21.WordWrap = true;
            this.ssActForm_Sheet1.Columns.Get(2).CellType = textCellType21;
            this.ssActForm_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssActForm_Sheet1.Columns.Get(2).Label = "항  목";
            this.ssActForm_Sheet1.Columns.Get(2).Locked = true;
            this.ssActForm_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssActForm_Sheet1.Columns.Get(2).Width = 258F;
            this.ssActForm_Sheet1.Columns.Get(3).CellType = textCellType22;
            this.ssActForm_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssActForm_Sheet1.Columns.Get(3).Label = "간격";
            this.ssActForm_Sheet1.Columns.Get(3).Locked = false;
            this.ssActForm_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssActForm_Sheet1.Columns.Get(3).Width = 40F;
            comboBoxCellType4.AllowEditorVerticalAlign = true;
            comboBoxCellType4.ButtonAlign = FarPoint.Win.ButtonAlign.Right;
            this.ssActForm_Sheet1.Columns.Get(4).CellType = comboBoxCellType4;
            this.ssActForm_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssActForm_Sheet1.Columns.Get(4).Label = "구분";
            this.ssActForm_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssActForm_Sheet1.Columns.Get(4).Width = 61F;
            this.ssActForm_Sheet1.Columns.Get(5).CellType = textCellType23;
            this.ssActForm_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssActForm_Sheet1.Columns.Get(5).Label = "횟수";
            this.ssActForm_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssActForm_Sheet1.Columns.Get(5).Width = 40F;
            this.ssActForm_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssActForm_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssActForm_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.ssActForm_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssActForm_Sheet1.FilterBar.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssActForm_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssActForm_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssActForm_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssActForm_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssActForm_Sheet1.FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssActForm_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssActForm_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssActForm_Sheet1.RowHeader.Columns.Get(0).Width = 29F;
            this.ssActForm_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssActForm_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssActForm_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssActForm_Sheet1.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssActForm_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssActForm_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssActForm_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssActForm_Sheet1.Rows.Get(0).CellType = textCellType24;
            this.ssActForm_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssActForm_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssActForm_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssActForm_Sheet1.SheetCornerStyle.Parent = "HeaderDefault";
            this.ssActForm_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssActForm_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssActForm_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel6.Controls.Add(this.btnDeSelect);
            this.panel6.Controls.Add(this.btnSelect);
            this.panel6.Controls.Add(this.btnDelete);
            this.panel6.Controls.Add(this.pictureBox1);
            this.panel6.Controls.Add(this.label2);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(555, 40);
            this.panel6.TabIndex = 8;
            // 
            // btnDeSelect
            // 
            this.btnDeSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeSelect.Location = new System.Drawing.Point(354, 3);
            this.btnDeSelect.Name = "btnDeSelect";
            this.btnDeSelect.Size = new System.Drawing.Size(73, 30);
            this.btnDeSelect.TabIndex = 72;
            this.btnDeSelect.Text = "전체해제";
            this.btnDeSelect.UseVisualStyleBackColor = true;
            this.btnDeSelect.Visible = false;
            this.btnDeSelect.Click += new System.EventHandler(this.btnDeSelect_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.Location = new System.Drawing.Point(281, 3);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(73, 30);
            this.btnSelect.TabIndex = 71;
            this.btnSelect.Text = "전체선택";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Visible = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.ForeColor = System.Drawing.Color.Red;
            this.btnDelete.Location = new System.Drawing.Point(456, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(92, 30);
            this.btnDelete.TabIndex = 66;
            this.btnDelete.Text = "삭 제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Visible = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(5, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 65;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(21, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 17);
            this.label2.TabIndex = 21;
            this.label2.Text = "가변항목";
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnSearch.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.ForeColor = System.Drawing.Color.Blue;
            this.btnSearch.Location = new System.Drawing.Point(379, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 204;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtSearch.Location = new System.Drawing.Point(287, 7);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(88, 25);
            this.txtSearch.TabIndex = 203;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(235, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 202;
            this.label3.Text = "검색어";
            // 
            // frmNrActingSetNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1010, 729);
            this.ControlBox = false;
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmNrActingSetNew";
            this.Text = "frmNrActingSetNew";
            this.Load += new System.EventHandler(this.frmNrActingSetNew_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssForm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssForm_Sheet1)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIconBar1)).EndInit();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssActForm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssActForm_Sheet1)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnSaveSet;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        public System.Windows.Forms.DateTimePicker dtpVitalDate;
        private System.Windows.Forms.Panel panel3;
        private FarPoint.Win.Spread.FpSpread ssForm;
        private FarPoint.Win.Spread.SheetView ssForm_Sheet1;
        private System.Windows.Forms.Panel panel4;
        internal System.Windows.Forms.ComboBox cboWard;
        private System.Windows.Forms.PictureBox picIconBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel5;
        private FarPoint.Win.Spread.FpSpread ssActForm;
        private FarPoint.Win.Spread.SheetView ssActForm_Sheet1;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btnDeSelect;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label3;
    }
}