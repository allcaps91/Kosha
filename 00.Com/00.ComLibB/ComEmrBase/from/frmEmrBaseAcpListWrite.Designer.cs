namespace ComEmrBase
{
    partial class frmEmrBaseAcpListWrite
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEmrBaseAcpListWrite));
            this.panViewEmrAcpDept = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.trvEmrForm = new System.Windows.Forms.TreeView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.mbtnSearch = new System.Windows.Forms.Button();
            this.txtFormName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.mbtnSave = new System.Windows.Forms.Button();
            this.optUser = new System.Windows.Forms.RadioButton();
            this.optAll = new System.Windows.Forms.RadioButton();
            this.mbtnCollapseAll = new System.Windows.Forms.Button();
            this.mbtnExpandAll = new System.Windows.Forms.Button();
            this.mbtnExit = new System.Windows.Forms.Button();
            this.panel14 = new System.Windows.Forms.Panel();
            this.ssViewEmrAcpDept = new FarPoint.Win.Spread.FpSpread();
            this.ssViewEmrAcpDept_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel13 = new System.Windows.Forms.Panel();
            this.btnSearchEmrDept = new System.Windows.Forms.Button();
            this.chkGikan = new System.Windows.Forms.CheckBox();
            this.dtpDateDeptE = new System.Windows.Forms.DateTimePicker();
            this.dtpDateDeptS = new System.Windows.Forms.DateTimePicker();
            this.optEmrInOutDeptI = new System.Windows.Forms.RadioButton();
            this.optEmrInOutDeptO = new System.Windows.Forms.RadioButton();
            this.optEmrInOutDeptA = new System.Windows.Forms.RadioButton();
            this.ImageList2 = new System.Windows.Forms.ImageList(this.components);
            this.panViewEmrAcpDept.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssViewEmrAcpDept)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssViewEmrAcpDept_Sheet1)).BeginInit();
            this.panel13.SuspendLayout();
            this.SuspendLayout();
            // 
            // panViewEmrAcpDept
            // 
            this.panViewEmrAcpDept.Controls.Add(this.panel1);
            this.panViewEmrAcpDept.Controls.Add(this.panel14);
            this.panViewEmrAcpDept.Controls.Add(this.ssViewEmrAcpDept);
            this.panViewEmrAcpDept.Controls.Add(this.panel13);
            this.panViewEmrAcpDept.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panViewEmrAcpDept.Location = new System.Drawing.Point(0, 0);
            this.panViewEmrAcpDept.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panViewEmrAcpDept.Name = "panViewEmrAcpDept";
            this.panViewEmrAcpDept.Size = new System.Drawing.Size(365, 771);
            this.panViewEmrAcpDept.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.trvEmrForm);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.panTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 356);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(365, 415);
            this.panel1.TabIndex = 51;
            // 
            // trvEmrForm
            // 
            this.trvEmrForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvEmrForm.Location = new System.Drawing.Point(0, 95);
            this.trvEmrForm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.trvEmrForm.Name = "trvEmrForm";
            this.trvEmrForm.Size = new System.Drawing.Size(365, 320);
            this.trvEmrForm.TabIndex = 21;
            this.trvEmrForm.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TrvEmrForm_NodeMouseClick);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.mbtnSearch);
            this.panel2.Controls.Add(this.txtFormName);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 37);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(365, 58);
            this.panel2.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(10, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(148, 17);
            this.label2.TabIndex = 72;
            this.label2.Text = "전체기록에서 조회 가능";
            // 
            // mbtnSearch
            // 
            this.mbtnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mbtnSearch.Location = new System.Drawing.Point(292, 1);
            this.mbtnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mbtnSearch.Name = "mbtnSearch";
            this.mbtnSearch.Size = new System.Drawing.Size(66, 30);
            this.mbtnSearch.TabIndex = 71;
            this.mbtnSearch.Text = "조회";
            this.mbtnSearch.UseVisualStyleBackColor = true;
            this.mbtnSearch.Click += new System.EventHandler(this.MbtnSearch_Click);
            // 
            // txtFormName
            // 
            this.txtFormName.Location = new System.Drawing.Point(69, 4);
            this.txtFormName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFormName.Name = "txtFormName";
            this.txtFormName.Size = new System.Drawing.Size(210, 25);
            this.txtFormName.TabIndex = 1;
            this.txtFormName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFormName_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "서식지명:";
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.SystemColors.Control;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.mbtnSave);
            this.panTitle.Controls.Add(this.optUser);
            this.panTitle.Controls.Add(this.optAll);
            this.panTitle.Controls.Add(this.mbtnCollapseAll);
            this.panTitle.Controls.Add(this.mbtnExpandAll);
            this.panTitle.Controls.Add(this.mbtnExit);
            this.panTitle.Cursor = System.Windows.Forms.Cursors.Default;
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitle.Name = "panTitle";
            this.panTitle.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panTitle.Size = new System.Drawing.Size(365, 37);
            this.panTitle.TabIndex = 5;
            this.panTitle.TabStop = true;
            // 
            // mbtnSave
            // 
            this.mbtnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mbtnSave.Location = new System.Drawing.Point(234, 2);
            this.mbtnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mbtnSave.Name = "mbtnSave";
            this.mbtnSave.Size = new System.Drawing.Size(56, 30);
            this.mbtnSave.TabIndex = 82;
            this.mbtnSave.Text = "등록";
            this.mbtnSave.UseVisualStyleBackColor = true;
            this.mbtnSave.Click += new System.EventHandler(this.MbtnSave_Click);
            // 
            // optUser
            // 
            this.optUser.AutoSize = true;
            this.optUser.Location = new System.Drawing.Point(64, 7);
            this.optUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optUser.Name = "optUser";
            this.optUser.Size = new System.Drawing.Size(52, 21);
            this.optUser.TabIndex = 81;
            this.optUser.Text = "개인";
            this.optUser.UseVisualStyleBackColor = true;
            this.optUser.CheckedChanged += new System.EventHandler(this.optUser_CheckedChanged);
            // 
            // optAll
            // 
            this.optAll.AutoSize = true;
            this.optAll.Location = new System.Drawing.Point(6, 7);
            this.optAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optAll.Name = "optAll";
            this.optAll.Size = new System.Drawing.Size(52, 21);
            this.optAll.TabIndex = 79;
            this.optAll.TabStop = true;
            this.optAll.Text = "전체";
            this.optAll.UseVisualStyleBackColor = true;
            this.optAll.CheckedChanged += new System.EventHandler(this.optAll_CheckedChanged);
            // 
            // mbtnCollapseAll
            // 
            this.mbtnCollapseAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mbtnCollapseAll.Location = new System.Drawing.Point(178, 2);
            this.mbtnCollapseAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mbtnCollapseAll.Name = "mbtnCollapseAll";
            this.mbtnCollapseAll.Size = new System.Drawing.Size(56, 30);
            this.mbtnCollapseAll.TabIndex = 70;
            this.mbtnCollapseAll.Text = "닫기";
            this.mbtnCollapseAll.UseVisualStyleBackColor = true;
            this.mbtnCollapseAll.Click += new System.EventHandler(this.MbtnCollapseAll_Click);
            // 
            // mbtnExpandAll
            // 
            this.mbtnExpandAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mbtnExpandAll.Location = new System.Drawing.Point(122, 2);
            this.mbtnExpandAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mbtnExpandAll.Name = "mbtnExpandAll";
            this.mbtnExpandAll.Size = new System.Drawing.Size(56, 30);
            this.mbtnExpandAll.TabIndex = 69;
            this.mbtnExpandAll.Text = "열기";
            this.mbtnExpandAll.UseVisualStyleBackColor = true;
            this.mbtnExpandAll.Click += new System.EventHandler(this.MbtnExpandAll_Click);
            // 
            // mbtnExit
            // 
            this.mbtnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mbtnExit.Location = new System.Drawing.Point(292, 2);
            this.mbtnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mbtnExit.Name = "mbtnExit";
            this.mbtnExit.Size = new System.Drawing.Size(67, 30);
            this.mbtnExit.TabIndex = 9;
            this.mbtnExit.Text = "닫 기";
            this.mbtnExit.UseVisualStyleBackColor = true;
            // 
            // panel14
            // 
            this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel14.Location = new System.Drawing.Point(0, 345);
            this.panel14.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(365, 11);
            this.panel14.TabIndex = 50;
            // 
            // ssViewEmrAcpDept
            // 
            this.ssViewEmrAcpDept.AccessibleDescription = "ssViewEmrAcpDept, Sheet1, Row 0, Column 0, ";
            this.ssViewEmrAcpDept.Dock = System.Windows.Forms.DockStyle.Top;
            this.ssViewEmrAcpDept.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssViewEmrAcpDept.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssViewEmrAcpDept.Location = new System.Drawing.Point(0, 34);
            this.ssViewEmrAcpDept.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ssViewEmrAcpDept.Name = "ssViewEmrAcpDept";
            this.ssViewEmrAcpDept.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssViewEmrAcpDept_Sheet1});
            this.ssViewEmrAcpDept.Size = new System.Drawing.Size(365, 311);
            this.ssViewEmrAcpDept.TabIndex = 49;
            this.ssViewEmrAcpDept.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssViewEmrAcpDept_CellClick);
            // 
            // ssViewEmrAcpDept_Sheet1
            // 
            this.ssViewEmrAcpDept_Sheet1.Reset();
            this.ssViewEmrAcpDept_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssViewEmrAcpDept_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssViewEmrAcpDept_Sheet1.ColumnCount = 12;
            this.ssViewEmrAcpDept_Sheet1.RowCount = 1;
            this.ssViewEmrAcpDept_Sheet1.Cells.Get(0, 1).Value = "9999-99-99";
            this.ssViewEmrAcpDept_Sheet1.Cells.Get(0, 3).Value = "IM";
            this.ssViewEmrAcpDept_Sheet1.Cells.Get(0, 8).Value = "홍길동";
            this.ssViewEmrAcpDept_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssViewEmrAcpDept_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssViewEmrAcpDept_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssViewEmrAcpDept_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssViewEmrAcpDept_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "I/O";
            this.ssViewEmrAcpDept_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "내원일자";
            this.ssViewEmrAcpDept_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "퇴원일자";
            this.ssViewEmrAcpDept_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "과";
            this.ssViewEmrAcpDept_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "진료과명";
            this.ssViewEmrAcpDept_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "MEDFRTIME";
            this.ssViewEmrAcpDept_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "MEDENDTIME";
            this.ssViewEmrAcpDept_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "MEDDRCD";
            this.ssViewEmrAcpDept_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "진료의";
            this.ssViewEmrAcpDept_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "퇴원일자";
            this.ssViewEmrAcpDept_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "MEDDEPTCD";
            this.ssViewEmrAcpDept_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "UPDATENO";
            this.ssViewEmrAcpDept_Sheet1.ColumnHeader.Rows.Get(0).Height = 28F;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(0).Label = "I/O";
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(0).Locked = true;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(0).Width = 30F;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(1).Label = "내원일자";
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(1).Locked = true;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(1).Width = 77F;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(2).Label = "퇴원일자";
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(2).Locked = true;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(2).Width = 77F;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(3).Label = "과";
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(3).Locked = true;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(3).Width = 32F;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(4).Label = "진료과명";
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(4).Locked = true;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(4).Width = 70F;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(5).Label = "MEDFRTIME";
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(5).Locked = false;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(5).Visible = false;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(5).Width = 103F;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(6).CellType = textCellType7;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(6).Label = "MEDENDTIME";
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(6).Locked = false;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(6).Visible = false;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(6).Width = 103F;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(7).CellType = textCellType8;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(7).Label = "MEDDRCD";
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(7).Locked = false;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(7).Visible = false;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(7).Width = 103F;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(8).CellType = textCellType9;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(8).Label = "진료의";
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(8).Width = 54F;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(9).CellType = textCellType10;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(9).Label = "퇴원일자";
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(9).Visible = false;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(9).Width = 77F;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(10).CellType = textCellType11;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(10).Label = "MEDDEPTCD";
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(10).Locked = false;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(10).Visible = false;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(10).Width = 103F;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(11).CellType = textCellType12;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(11).Label = "UPDATENO";
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(11).Locked = false;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(11).Visible = false;
            this.ssViewEmrAcpDept_Sheet1.Columns.Get(11).Width = 119F;
            this.ssViewEmrAcpDept_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssViewEmrAcpDept_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssViewEmrAcpDept_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssViewEmrAcpDept_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssViewEmrAcpDept_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssViewEmrAcpDept_Sheet1.RowHeader.Visible = false;
            this.ssViewEmrAcpDept_Sheet1.Rows.Get(0).Height = 24F;
            this.ssViewEmrAcpDept_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel13
            // 
            this.panel13.Controls.Add(this.btnSearchEmrDept);
            this.panel13.Controls.Add(this.chkGikan);
            this.panel13.Controls.Add(this.dtpDateDeptE);
            this.panel13.Controls.Add(this.dtpDateDeptS);
            this.panel13.Controls.Add(this.optEmrInOutDeptI);
            this.panel13.Controls.Add(this.optEmrInOutDeptO);
            this.panel13.Controls.Add(this.optEmrInOutDeptA);
            this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel13.Location = new System.Drawing.Point(0, 0);
            this.panel13.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(365, 34);
            this.panel13.TabIndex = 0;
            // 
            // btnSearchEmrDept
            // 
            this.btnSearchEmrDept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearchEmrDept.Location = new System.Drawing.Point(293, 2);
            this.btnSearchEmrDept.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearchEmrDept.Name = "btnSearchEmrDept";
            this.btnSearchEmrDept.Size = new System.Drawing.Size(67, 30);
            this.btnSearchEmrDept.TabIndex = 109;
            this.btnSearchEmrDept.Text = "조 회";
            this.btnSearchEmrDept.UseVisualStyleBackColor = true;
            this.btnSearchEmrDept.Click += new System.EventHandler(this.BtnSearchEmrDept_Click);
            // 
            // chkGikan
            // 
            this.chkGikan.AutoSize = true;
            this.chkGikan.Location = new System.Drawing.Point(7, 50);
            this.chkGikan.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkGikan.Name = "chkGikan";
            this.chkGikan.Size = new System.Drawing.Size(79, 21);
            this.chkGikan.TabIndex = 113;
            this.chkGikan.Text = "조회기간";
            this.chkGikan.UseVisualStyleBackColor = true;
            this.chkGikan.Visible = false;
            // 
            // dtpDateDeptE
            // 
            this.dtpDateDeptE.CustomFormat = "yyyy-MM-dd";
            this.dtpDateDeptE.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateDeptE.Location = new System.Drawing.Point(177, 47);
            this.dtpDateDeptE.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpDateDeptE.Name = "dtpDateDeptE";
            this.dtpDateDeptE.Size = new System.Drawing.Size(91, 25);
            this.dtpDateDeptE.TabIndex = 112;
            this.dtpDateDeptE.Visible = false;
            // 
            // dtpDateDeptS
            // 
            this.dtpDateDeptS.CustomFormat = "yyyy-MM-dd";
            this.dtpDateDeptS.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateDeptS.Location = new System.Drawing.Point(86, 47);
            this.dtpDateDeptS.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpDateDeptS.Name = "dtpDateDeptS";
            this.dtpDateDeptS.Size = new System.Drawing.Size(91, 25);
            this.dtpDateDeptS.TabIndex = 110;
            this.dtpDateDeptS.Visible = false;
            // 
            // optEmrInOutDeptI
            // 
            this.optEmrInOutDeptI.AutoSize = true;
            this.optEmrInOutDeptI.Location = new System.Drawing.Point(127, 7);
            this.optEmrInOutDeptI.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optEmrInOutDeptI.Name = "optEmrInOutDeptI";
            this.optEmrInOutDeptI.Size = new System.Drawing.Size(52, 21);
            this.optEmrInOutDeptI.TabIndex = 108;
            this.optEmrInOutDeptI.Text = "입원";
            this.optEmrInOutDeptI.UseVisualStyleBackColor = true;
            this.optEmrInOutDeptI.CheckedChanged += new System.EventHandler(this.optEmrInOutDeptA_CheckedChanged);
            // 
            // optEmrInOutDeptO
            // 
            this.optEmrInOutDeptO.AutoSize = true;
            this.optEmrInOutDeptO.Location = new System.Drawing.Point(67, 7);
            this.optEmrInOutDeptO.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optEmrInOutDeptO.Name = "optEmrInOutDeptO";
            this.optEmrInOutDeptO.Size = new System.Drawing.Size(52, 21);
            this.optEmrInOutDeptO.TabIndex = 107;
            this.optEmrInOutDeptO.Text = "외래";
            this.optEmrInOutDeptO.UseVisualStyleBackColor = true;
            this.optEmrInOutDeptO.CheckedChanged += new System.EventHandler(this.optEmrInOutDeptA_CheckedChanged);
            // 
            // optEmrInOutDeptA
            // 
            this.optEmrInOutDeptA.AutoSize = true;
            this.optEmrInOutDeptA.Checked = true;
            this.optEmrInOutDeptA.Location = new System.Drawing.Point(7, 7);
            this.optEmrInOutDeptA.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optEmrInOutDeptA.Name = "optEmrInOutDeptA";
            this.optEmrInOutDeptA.Size = new System.Drawing.Size(52, 21);
            this.optEmrInOutDeptA.TabIndex = 106;
            this.optEmrInOutDeptA.TabStop = true;
            this.optEmrInOutDeptA.Text = "전체";
            this.optEmrInOutDeptA.UseVisualStyleBackColor = true;
            this.optEmrInOutDeptA.CheckedChanged += new System.EventHandler(this.optEmrInOutDeptA_CheckedChanged);
            // 
            // ImageList2
            // 
            this.ImageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList2.ImageStream")));
            this.ImageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList2.Images.SetKeyName(0, "folder.png");
            this.ImageList2.Images.SetKeyName(1, "folder_accept.png");
            this.ImageList2.Images.SetKeyName(2, "note.png");
            this.ImageList2.Images.SetKeyName(3, "note_accept.png");
            // 
            // frmEmrBaseAcpListWrite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(365, 771);
            this.Controls.Add(this.panViewEmrAcpDept);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "frmEmrBaseAcpListWrite";
            this.Text = "frmEmrBaseAcpListWrite";
            this.Load += new System.EventHandler(this.FrmEmrBaseAcpListWrite_Load);
            this.panViewEmrAcpDept.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssViewEmrAcpDept)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssViewEmrAcpDept_Sheet1)).EndInit();
            this.panel13.ResumeLayout(false);
            this.panel13.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panViewEmrAcpDept;
        private FarPoint.Win.Spread.FpSpread ssViewEmrAcpDept;
        private FarPoint.Win.Spread.SheetView ssViewEmrAcpDept_Sheet1;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Button btnSearchEmrDept;
        private System.Windows.Forms.CheckBox chkGikan;
        private System.Windows.Forms.DateTimePicker dtpDateDeptE;
        private System.Windows.Forms.DateTimePicker dtpDateDeptS;
        private System.Windows.Forms.RadioButton optEmrInOutDeptI;
        private System.Windows.Forms.RadioButton optEmrInOutDeptO;
        private System.Windows.Forms.RadioButton optEmrInOutDeptA;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button mbtnSave;
        private System.Windows.Forms.RadioButton optUser;
        private System.Windows.Forms.RadioButton optAll;
        private System.Windows.Forms.Button mbtnCollapseAll;
        private System.Windows.Forms.Button mbtnExpandAll;
        private System.Windows.Forms.Button mbtnExit;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button mbtnSearch;
        private System.Windows.Forms.TextBox txtFormName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView trvEmrForm;
        internal System.Windows.Forms.ImageList ImageList2;
    }
}