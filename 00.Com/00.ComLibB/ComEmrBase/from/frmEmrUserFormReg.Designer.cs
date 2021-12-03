namespace ComEmrBase
{
    partial class frmEmrUserFormReg
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEmrUserFormReg));
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType1 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.panDept = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.cboDept = new System.Windows.Forms.ComboBox();
            this.mbtnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.trvEmrGroup = new System.Windows.Forms.TreeView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.mbtnCollapseAll = new System.Windows.Forms.Button();
            this.mbtnExpandAll = new System.Windows.Forms.Button();
            this.picIconBar0 = new System.Windows.Forms.PictureBox();
            this.lblTitleSub = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ssForm = new FarPoint.Win.Spread.FpSpread();
            this.ssForm_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.mbtnSearch = new System.Windows.Forms.Button();
            this.txtFormName = new System.Windows.Forms.TextBox();
            this.mbtnSave = new System.Windows.Forms.Button();
            this.picIconBar1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.trvUserForm = new System.Windows.Forms.TreeView();
            this.panel6 = new System.Windows.Forms.Panel();
            this.mbtnCollapseDept = new System.Windows.Forms.Button();
            this.mbtnExpandDept = new System.Windows.Forms.Button();
            this.mbtnDeleate = new System.Windows.Forms.Button();
            this.picIconBar2 = new System.Windows.Forms.PictureBox();
            this.lblGRPGB = new System.Windows.Forms.Label();
            this.ImageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panTitle.SuspendLayout();
            this.panDept.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIconBar0)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssForm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssForm_Sheet1)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIconBar1)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIconBar2)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.panDept);
            this.panTitle.Controls.Add(this.mbtnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1117, 38);
            this.panTitle.TabIndex = 3;
            // 
            // panDept
            // 
            this.panDept.Controls.Add(this.label2);
            this.panDept.Controls.Add(this.cboDept);
            this.panDept.Location = new System.Drawing.Point(807, 1);
            this.panDept.Name = "panDept";
            this.panDept.Size = new System.Drawing.Size(133, 32);
            this.panDept.TabIndex = 68;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(37, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "과";
            // 
            // cboDept
            // 
            this.cboDept.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDept.FormattingEnabled = true;
            this.cboDept.Location = new System.Drawing.Point(62, 6);
            this.cboDept.MaxDropDownItems = 16;
            this.cboDept.Name = "cboDept";
            this.cboDept.Size = new System.Drawing.Size(64, 20);
            this.cboDept.TabIndex = 20;
            this.cboDept.SelectedIndexChanged += new System.EventHandler(this.cboDept_SelectedIndexChanged);
            // 
            // mbtnExit
            // 
            this.mbtnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mbtnExit.Location = new System.Drawing.Point(1023, 3);
            this.mbtnExit.Name = "mbtnExit";
            this.mbtnExit.Size = new System.Drawing.Size(83, 30);
            this.mbtnExit.TabIndex = 19;
            this.mbtnExit.Text = "닫  기";
            this.mbtnExit.UseVisualStyleBackColor = true;
            this.mbtnExit.Click += new System.EventHandler(this.mbtnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(12, 11);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(139, 16);
            this.lblTitle.TabIndex = 14;
            this.lblTitle.Text = "상용 기록지 등록";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.trvEmrGroup);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 38);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3);
            this.panel1.Size = new System.Drawing.Size(312, 581);
            this.panel1.TabIndex = 4;
            // 
            // trvEmrGroup
            // 
            this.trvEmrGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvEmrGroup.Location = new System.Drawing.Point(3, 41);
            this.trvEmrGroup.Name = "trvEmrGroup";
            this.trvEmrGroup.Size = new System.Drawing.Size(306, 537);
            this.trvEmrGroup.TabIndex = 96;
            this.trvEmrGroup.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvEmrGroup_AfterSelect);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.mbtnCollapseAll);
            this.panel2.Controls.Add(this.mbtnExpandAll);
            this.panel2.Controls.Add(this.picIconBar0);
            this.panel2.Controls.Add(this.lblTitleSub);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(306, 38);
            this.panel2.TabIndex = 0;
            // 
            // mbtnCollapseAll
            // 
            this.mbtnCollapseAll.Location = new System.Drawing.Point(238, 2);
            this.mbtnCollapseAll.Name = "mbtnCollapseAll";
            this.mbtnCollapseAll.Size = new System.Drawing.Size(62, 30);
            this.mbtnCollapseAll.TabIndex = 68;
            this.mbtnCollapseAll.Text = "닫 기";
            this.mbtnCollapseAll.UseVisualStyleBackColor = true;
            this.mbtnCollapseAll.Click += new System.EventHandler(this.mbtnCollapseAll_Click);
            // 
            // mbtnExpandAll
            // 
            this.mbtnExpandAll.Location = new System.Drawing.Point(176, 2);
            this.mbtnExpandAll.Name = "mbtnExpandAll";
            this.mbtnExpandAll.Size = new System.Drawing.Size(62, 30);
            this.mbtnExpandAll.TabIndex = 67;
            this.mbtnExpandAll.Text = "열 기";
            this.mbtnExpandAll.UseVisualStyleBackColor = true;
            this.mbtnExpandAll.Click += new System.EventHandler(this.mbtnExpandAll_Click);
            // 
            // picIconBar0
            // 
            this.picIconBar0.Image = ((System.Drawing.Image)(resources.GetObject("picIconBar0.Image")));
            this.picIconBar0.InitialImage = null;
            this.picIconBar0.Location = new System.Drawing.Point(5, 10);
            this.picIconBar0.Name = "picIconBar0";
            this.picIconBar0.Size = new System.Drawing.Size(16, 16);
            this.picIconBar0.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picIconBar0.TabIndex = 64;
            this.picIconBar0.TabStop = false;
            // 
            // lblTitleSub
            // 
            this.lblTitleSub.AutoSize = true;
            this.lblTitleSub.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub.ForeColor = System.Drawing.Color.Black;
            this.lblTitleSub.Location = new System.Drawing.Point(26, 12);
            this.lblTitleSub.Name = "lblTitleSub";
            this.lblTitleSub.Size = new System.Drawing.Size(68, 13);
            this.lblTitleSub.TabIndex = 21;
            this.lblTitleSub.Text = "그룹 조회";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.ssForm);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(312, 38);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(414, 581);
            this.panel3.TabIndex = 5;
            // 
            // ssForm
            // 
            this.ssForm.AccessibleDescription = "ssForm, Sheet1, Row 0, Column 0, ";
            this.ssForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssForm.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssForm.Location = new System.Drawing.Point(0, 38);
            this.ssForm.Name = "ssForm";
            this.ssForm.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssForm_Sheet1});
            this.ssForm.Size = new System.Drawing.Size(414, 543);
            this.ssForm.TabIndex = 10;
            // 
            // ssForm_Sheet1
            // 
            this.ssForm_Sheet1.Reset();
            this.ssForm_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssForm_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            ssForm_Sheet1.ColumnCount = 5;
            ssForm_Sheet1.RowCount = 1;
            this.ssForm_Sheet1.ActiveSkin = new FarPoint.Win.Spread.SheetSkin("CustomSkin1", System.Drawing.Color.White, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.LightGray, FarPoint.Win.Spread.GridLines.Both, System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237))))), System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, false, false, false, false, false, true, false, true, "HeaderDefault", "HeaderDefault", "HeaderDefault", "DataAreaDefault", "HeaderDefault");
            this.ssForm_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssForm_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssForm_Sheet1.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.ssForm_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssForm_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssForm_Sheet1.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.ssForm_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = " ";
            this.ssForm_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "서식지번호";
            this.ssForm_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "기록지명";
            this.ssForm_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "인쇄명";
            this.ssForm_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "사용구분";
            this.ssForm_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssForm_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssForm_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssForm_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssForm_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssForm_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssForm_Sheet1.ColumnHeader.Rows.Get(0).Height = 30F;
            this.ssForm_Sheet1.Columns.Get(0).CellType = checkBoxCellType1;
            this.ssForm_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssForm_Sheet1.Columns.Get(0).Label = " ";
            this.ssForm_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssForm_Sheet1.Columns.Get(0).Width = 20F;
            this.ssForm_Sheet1.Columns.Get(1).CellType = textCellType1;
            this.ssForm_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssForm_Sheet1.Columns.Get(1).Label = "서식지번호";
            this.ssForm_Sheet1.Columns.Get(1).Locked = true;
            this.ssForm_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssForm_Sheet1.Columns.Get(1).Width = 77F;
            this.ssForm_Sheet1.Columns.Get(2).CellType = textCellType2;
            this.ssForm_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssForm_Sheet1.Columns.Get(2).Label = "기록지명";
            this.ssForm_Sheet1.Columns.Get(2).Locked = true;
            this.ssForm_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssForm_Sheet1.Columns.Get(2).Width = 260F;
            this.ssForm_Sheet1.Columns.Get(3).CellType = textCellType3;
            this.ssForm_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssForm_Sheet1.Columns.Get(3).Label = "인쇄명";
            this.ssForm_Sheet1.Columns.Get(3).Locked = true;
            this.ssForm_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssForm_Sheet1.Columns.Get(3).Visible = false;
            this.ssForm_Sheet1.Columns.Get(3).Width = 260F;
            this.ssForm_Sheet1.Columns.Get(4).CellType = textCellType4;
            this.ssForm_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssForm_Sheet1.Columns.Get(4).Label = "사용구분";
            this.ssForm_Sheet1.Columns.Get(4).Locked = true;
            this.ssForm_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssForm_Sheet1.Columns.Get(4).Visible = false;
            this.ssForm_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssForm_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssForm_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssForm_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssForm_Sheet1.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssForm_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssForm_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssForm_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.ssForm_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssForm_Sheet1.SheetCornerStyle.Parent = "HeaderDefault";
            this.ssForm_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssForm_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.mbtnSearch);
            this.panel4.Controls.Add(this.txtFormName);
            this.panel4.Controls.Add(this.mbtnSave);
            this.panel4.Controls.Add(this.picIconBar1);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(414, 38);
            this.panel4.TabIndex = 8;
            // 
            // mbtnSearch
            // 
            this.mbtnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mbtnSearch.Location = new System.Drawing.Point(254, 3);
            this.mbtnSearch.Name = "mbtnSearch";
            this.mbtnSearch.Size = new System.Drawing.Size(62, 30);
            this.mbtnSearch.TabIndex = 72;
            this.mbtnSearch.Text = "조회";
            this.mbtnSearch.UseVisualStyleBackColor = true;
            this.mbtnSearch.Click += new System.EventHandler(this.mbtnSearch_Click);
            // 
            // txtFormName
            // 
            this.txtFormName.Location = new System.Drawing.Point(76, 8);
            this.txtFormName.Name = "txtFormName";
            this.txtFormName.Size = new System.Drawing.Size(172, 21);
            this.txtFormName.TabIndex = 67;
            this.txtFormName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFormName_KeyDown);
            // 
            // mbtnSave
            // 
            this.mbtnSave.Location = new System.Drawing.Point(346, 3);
            this.mbtnSave.Name = "mbtnSave";
            this.mbtnSave.Size = new System.Drawing.Size(62, 30);
            this.mbtnSave.TabIndex = 66;
            this.mbtnSave.Text = "등 록";
            this.mbtnSave.UseVisualStyleBackColor = true;
            this.mbtnSave.Click += new System.EventHandler(this.mbtnSave_Click);
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
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "서식지";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.trvUserForm);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(726, 38);
            this.panel5.Name = "panel5";
            this.panel5.Padding = new System.Windows.Forms.Padding(3);
            this.panel5.Size = new System.Drawing.Size(391, 581);
            this.panel5.TabIndex = 6;
            // 
            // trvUserForm
            // 
            this.trvUserForm.CheckBoxes = true;
            this.trvUserForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvUserForm.Location = new System.Drawing.Point(3, 41);
            this.trvUserForm.Name = "trvUserForm";
            this.trvUserForm.Size = new System.Drawing.Size(385, 537);
            this.trvUserForm.TabIndex = 96;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel6.Controls.Add(this.mbtnCollapseDept);
            this.panel6.Controls.Add(this.mbtnExpandDept);
            this.panel6.Controls.Add(this.mbtnDeleate);
            this.panel6.Controls.Add(this.picIconBar2);
            this.panel6.Controls.Add(this.lblGRPGB);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(3, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(385, 38);
            this.panel6.TabIndex = 0;
            // 
            // mbtnCollapseDept
            // 
            this.mbtnCollapseDept.Location = new System.Drawing.Point(229, 2);
            this.mbtnCollapseDept.Name = "mbtnCollapseDept";
            this.mbtnCollapseDept.Size = new System.Drawing.Size(62, 30);
            this.mbtnCollapseDept.TabIndex = 70;
            this.mbtnCollapseDept.Text = "닫 기";
            this.mbtnCollapseDept.UseVisualStyleBackColor = true;
            this.mbtnCollapseDept.Click += new System.EventHandler(this.mbtnCollapseDept_Click);
            // 
            // mbtnExpandDept
            // 
            this.mbtnExpandDept.Location = new System.Drawing.Point(167, 2);
            this.mbtnExpandDept.Name = "mbtnExpandDept";
            this.mbtnExpandDept.Size = new System.Drawing.Size(62, 30);
            this.mbtnExpandDept.TabIndex = 69;
            this.mbtnExpandDept.Text = "열 기";
            this.mbtnExpandDept.UseVisualStyleBackColor = true;
            this.mbtnExpandDept.Click += new System.EventHandler(this.mbtnExpandDept_Click);
            // 
            // mbtnDeleate
            // 
            this.mbtnDeleate.Location = new System.Drawing.Point(295, 2);
            this.mbtnDeleate.Name = "mbtnDeleate";
            this.mbtnDeleate.Size = new System.Drawing.Size(83, 30);
            this.mbtnDeleate.TabIndex = 67;
            this.mbtnDeleate.Text = "삭 제";
            this.mbtnDeleate.UseVisualStyleBackColor = true;
            this.mbtnDeleate.Click += new System.EventHandler(this.mbtnDeleate_Click);
            // 
            // picIconBar2
            // 
            this.picIconBar2.Image = ((System.Drawing.Image)(resources.GetObject("picIconBar2.Image")));
            this.picIconBar2.InitialImage = null;
            this.picIconBar2.Location = new System.Drawing.Point(5, 10);
            this.picIconBar2.Name = "picIconBar2";
            this.picIconBar2.Size = new System.Drawing.Size(16, 16);
            this.picIconBar2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picIconBar2.TabIndex = 64;
            this.picIconBar2.TabStop = false;
            // 
            // lblGRPGB
            // 
            this.lblGRPGB.AutoSize = true;
            this.lblGRPGB.BackColor = System.Drawing.Color.Transparent;
            this.lblGRPGB.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblGRPGB.ForeColor = System.Drawing.Color.Black;
            this.lblGRPGB.Location = new System.Drawing.Point(26, 12);
            this.lblGRPGB.Name = "lblGRPGB";
            this.lblGRPGB.Size = new System.Drawing.Size(77, 13);
            this.lblGRPGB.TabIndex = 21;
            this.lblGRPGB.Text = "상용기록지";
            // 
            // ImageList1
            // 
            this.ImageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList1.ImageStream")));
            this.ImageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList1.Images.SetKeyName(0, "folder.png");
            this.ImageList1.Images.SetKeyName(1, "folder_accept.png");
            this.ImageList1.Images.SetKeyName(2, "note.png");
            this.ImageList1.Images.SetKeyName(3, "note_accept.png");
            // 
            // frmEmrUserFormReg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1117, 619);
            this.ControlBox = false;
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmEmrUserFormReg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "상용 기록지 등록";
            this.Load += new System.EventHandler(this.frmEmrUserFormReg_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panDept.ResumeLayout(false);
            this.panDept.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIconBar0)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssForm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssForm_Sheet1)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIconBar1)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIconBar2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button mbtnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.TreeView trvEmrGroup;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox picIconBar0;
        private System.Windows.Forms.Label lblTitleSub;
        private System.Windows.Forms.Panel panel3;
        private FarPoint.Win.Spread.FpSpread ssForm;
        private FarPoint.Win.Spread.SheetView ssForm_Sheet1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.PictureBox picIconBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel5;
        internal System.Windows.Forms.TreeView trvUserForm;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.PictureBox picIconBar2;
        private System.Windows.Forms.Label lblGRPGB;
        internal System.Windows.Forms.ImageList ImageList1;
        private System.Windows.Forms.ComboBox cboDept;
        private System.Windows.Forms.Button mbtnSave;
        private System.Windows.Forms.Button mbtnDeleate;
        private System.Windows.Forms.Button mbtnCollapseAll;
        private System.Windows.Forms.Button mbtnExpandAll;
        private System.Windows.Forms.Button mbtnCollapseDept;
        private System.Windows.Forms.Button mbtnExpandDept;
        private System.Windows.Forms.Panel panDept;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFormName;
        private System.Windows.Forms.Button mbtnSearch;
    }
}