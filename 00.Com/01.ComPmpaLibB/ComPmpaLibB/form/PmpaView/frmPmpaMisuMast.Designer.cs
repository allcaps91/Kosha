using System.Windows.Forms;

namespace ComPmpaLibB
{
    partial class frmPmpaMisuMast
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

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Keys key = keyData & ~(Keys.Shift | Keys.Control);

            switch (key)
            {
                case Keys.F2: //신규등록
                    eMenuClick(Menu1_1, null);
                    return true;
                case Keys.F3: //ID사항변경
                    eMenuClick(Menu1_2, null);
                    return true;
                case Keys.F4: //월별, 조합별 명단찾기
                    eMenuClick(Menu1_3, null);
                    return true;
                case Keys.F5: //미수번호별 명단찾기
                    eMenuClick(Menu1_4, null);
                    return true;

            }

            return base.ProcessCmdKey(ref msg, keyData);
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("FilterBarDefaultEnhanced");
            FarPoint.Win.Spread.CellType.FilterBarCellType filterBarCellType1 = new FarPoint.Win.Spread.CellType.FilterBarCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("ColumnHeaderDefaultEnhanced");
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("RowHeaderDefaultEnhanced");
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("CornerDefaultEnhanced");
            FarPoint.Win.Spread.CellType.FlatCornerHeaderRenderer flatCornerHeaderRenderer1 = new FarPoint.Win.Spread.CellType.FlatCornerHeaderRenderer();
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("DataAreaDefault");
            FarPoint.Win.Spread.CellType.GeneralCellType generalCellType1 = new FarPoint.Win.Spread.CellType.GeneralCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle6 = new FarPoint.Win.Spread.NamedStyle("FilterBarGrayscale");
            FarPoint.Win.Spread.CellType.FilterBarCellType filterBarCellType2 = new FarPoint.Win.Spread.CellType.FilterBarCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle7 = new FarPoint.Win.Spread.NamedStyle("FilterBarHeaderFlatOffice2016DarkGray");
            FarPoint.Win.Spread.NamedStyle namedStyle8 = new FarPoint.Win.Spread.NamedStyle("CornerHeaderFlatOffice2016DarkGray");
            FarPoint.Win.Spread.CellType.FlatCornerHeaderRenderer flatCornerHeaderRenderer2 = new FarPoint.Win.Spread.CellType.FlatCornerHeaderRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer2 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.InputMap ssList2_InputMapWhenFocusedNormal;
            FarPoint.Win.Spread.InputMap ssList2_InputMapWhenFocusedReadOnly;
            FarPoint.Win.Spread.InputMap ssList2_InputMapWhenFocusedRowMode;
            FarPoint.Win.Spread.InputMap ssList2_InputMapWhenFocusedSingleSelect;
            FarPoint.Win.Spread.InputMap ssList2_InputMapWhenFocusedMultiSelect;
            FarPoint.Win.Spread.InputMap ssList2_InputMapWhenFocusedExtendedSelect;
            FarPoint.Win.Spread.InputMap ssList2_InputMapWhenAncestorOfFocusedNormal;
            FarPoint.Win.Spread.InputMap ssList2_InputMapWhenAncestorOfFocusedReadOnly;
            FarPoint.Win.Spread.InputMap ssList2_InputMapWhenAncestorOfFocusedRowMode;
            FarPoint.Win.Spread.InputMap ssList2_InputMapWhenAncestorOfFocusedSingleSelect;
            FarPoint.Win.Spread.InputMap ssList2_InputMapWhenAncestorOfFocusedMultiSelect;
            FarPoint.Win.Spread.InputMap ssList2_InputMapWhenAncestorOfFocusedExtendedSelect;
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.JobMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu1_1 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu1_2 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu1_3 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu1_4 = new System.Windows.Forms.ToolStripMenuItem();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pan = new System.Windows.Forms.Panel();
            this.panel12 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.chkCash = new System.Windows.Forms.CheckBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cboClass = new System.Windows.Forms.ComboBox();
            this.lblItem0 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.PanelMain = new System.Windows.Forms.Panel();
            this.cboDept = new System.Windows.Forms.ComboBox();
            this.dtpTDate = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.dtpFDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.btnHelp = new System.Windows.Forms.Button();
            this.panel13 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.cboMgrRank = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.GbGubun = new System.Windows.Forms.GroupBox();
            this.cboGubun = new System.Windows.Forms.ComboBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.ssList1 = new FarPoint.Win.Spread.FpSpread();
            this.ssList1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.TxtMirYYMM = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cboIO = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpBDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.cboBun = new System.Windows.Forms.ComboBox();
            this.lblMiaName = new System.Windows.Forms.Label();
            this.TxtGelCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblWRTNO = new System.Windows.Forms.Label();
            this.lblSname = new System.Windows.Forms.Label();
            this.TxtMisuID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel10 = new System.Windows.Forms.Panel();
            this.panel11 = new System.Windows.Forms.Panel();
            this.ssList3 = new FarPoint.Win.Spread.FpSpread();
            this.ssList3_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.ssList2 = new FarPoint.Win.Spread.FpSpread();
            this.ssList2_Sheet1 = new FarPoint.Win.Spread.SheetView();
            ssList2_InputMapWhenFocusedNormal = new FarPoint.Win.Spread.InputMap();
            ssList2_InputMapWhenFocusedReadOnly = new FarPoint.Win.Spread.InputMap();
            ssList2_InputMapWhenFocusedRowMode = new FarPoint.Win.Spread.InputMap();
            ssList2_InputMapWhenFocusedSingleSelect = new FarPoint.Win.Spread.InputMap();
            ssList2_InputMapWhenFocusedMultiSelect = new FarPoint.Win.Spread.InputMap();
            ssList2_InputMapWhenFocusedExtendedSelect = new FarPoint.Win.Spread.InputMap();
            ssList2_InputMapWhenAncestorOfFocusedNormal = new FarPoint.Win.Spread.InputMap();
            ssList2_InputMapWhenAncestorOfFocusedReadOnly = new FarPoint.Win.Spread.InputMap();
            ssList2_InputMapWhenAncestorOfFocusedRowMode = new FarPoint.Win.Spread.InputMap();
            ssList2_InputMapWhenAncestorOfFocusedSingleSelect = new FarPoint.Win.Spread.InputMap();
            ssList2_InputMapWhenAncestorOfFocusedMultiSelect = new FarPoint.Win.Spread.InputMap();
            ssList2_InputMapWhenAncestorOfFocusedExtendedSelect = new FarPoint.Win.Spread.InputMap();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.PanelMain.SuspendLayout();
            this.panel13.SuspendLayout();
            this.GbGubun.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList1_Sheet1)).BeginInit();
            this.panel10.SuspendLayout();
            this.panel11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList3_Sheet1)).BeginInit();
            this.panel8.SuspendLayout();
            this.panel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList2_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(948, 21);
            this.panel1.TabIndex = 1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.JobMenu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(948, 21);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // JobMenu
            // 
            this.JobMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu1_1,
            this.Menu1_2,
            this.Menu1_3,
            this.Menu1_4});
            this.JobMenu.Name = "JobMenu";
            this.JobMenu.Size = new System.Drawing.Size(37, 17);
            this.JobMenu.Text = "Job";
            // 
            // Menu1_1
            // 
            this.Menu1_1.Name = "Menu1_1";
            this.Menu1_1.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.Menu1_1.Size = new System.Drawing.Size(223, 22);
            this.Menu1_1.Text = "1. 신규등록";
            // 
            // Menu1_2
            // 
            this.Menu1_2.Name = "Menu1_2";
            this.Menu1_2.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.Menu1_2.Size = new System.Drawing.Size(223, 22);
            this.Menu1_2.Text = "2. ID사항변경";
            // 
            // Menu1_3
            // 
            this.Menu1_3.Name = "Menu1_3";
            this.Menu1_3.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.Menu1_3.Size = new System.Drawing.Size(223, 22);
            this.Menu1_3.Text = "3. 월별,조합별 명단찾기";
            // 
            // Menu1_4
            // 
            this.Menu1_4.Name = "Menu1_4";
            this.Menu1_4.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.Menu1_4.Size = new System.Drawing.Size(223, 22);
            this.Menu1_4.Text = "4. 미수번호별 명단찾기";
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.label1);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 21);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(948, 31);
            this.panTitle.TabIndex = 171;
            // 
            // btnExit
            // 
            this.btnExit.AutoSize = true;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(872, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 27);
            this.btnExit.TabIndex = 82;
            this.btnExit.Text = "닫기(&X)";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 21);
            this.label1.TabIndex = 81;
            this.label1.Text = "미수 원장 관리";
            // 
            // pan
            // 
            this.pan.BackColor = System.Drawing.Color.RoyalBlue;
            this.pan.Dock = System.Windows.Forms.DockStyle.Top;
            this.pan.Location = new System.Drawing.Point(0, 52);
            this.pan.Name = "pan";
            this.pan.Size = new System.Drawing.Size(948, 10);
            this.pan.TabIndex = 180;
            // 
            // panel12
            // 
            this.panel12.BackColor = System.Drawing.Color.White;
            this.panel12.Controls.Add(this.panel3);
            this.panel12.Controls.Add(this.panel6);
            this.panel12.Controls.Add(this.panel4);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel12.Location = new System.Drawing.Point(0, 62);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(948, 40);
            this.panel12.TabIndex = 181;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnOK);
            this.panel3.Controls.Add(this.btnDel);
            this.panel3.Controls.Add(this.btnNext);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(687, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(262, 40);
            this.panel3.TabIndex = 32;
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.Location = new System.Drawing.Point(16, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(72, 30);
            this.btnOK.TabIndex = 31;
            this.btnOK.Text = "등록(&O)";
            this.btnOK.UseVisualStyleBackColor = false;
            // 
            // btnDel
            // 
            this.btnDel.BackColor = System.Drawing.Color.Transparent;
            this.btnDel.Location = new System.Drawing.Point(172, 5);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(72, 30);
            this.btnDel.TabIndex = 29;
            this.btnDel.Text = "삭제(&D)";
            this.btnDel.UseVisualStyleBackColor = false;
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.Transparent;
            this.btnNext.Location = new System.Drawing.Point(94, 5);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(72, 30);
            this.btnNext.TabIndex = 28;
            this.btnNext.Text = "취소(&C)";
            this.btnNext.UseVisualStyleBackColor = false;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.chkCash);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel6.Location = new System.Drawing.Point(332, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(355, 40);
            this.panel6.TabIndex = 31;
            // 
            // chkCash
            // 
            this.chkCash.AutoSize = true;
            this.chkCash.Location = new System.Drawing.Point(273, 16);
            this.chkCash.Name = "chkCash";
            this.chkCash.Size = new System.Drawing.Size(79, 21);
            this.chkCash.TabIndex = 34;
            this.chkCash.Text = "현금승인";
            this.chkCash.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.cboClass);
            this.panel4.Controls.Add(this.lblItem0);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(332, 40);
            this.panel4.TabIndex = 30;
            // 
            // cboClass
            // 
            this.cboClass.FormattingEnabled = true;
            this.cboClass.Location = new System.Drawing.Point(120, 9);
            this.cboClass.Name = "cboClass";
            this.cboClass.Size = new System.Drawing.Size(206, 25);
            this.cboClass.TabIndex = 0;
            this.cboClass.Text = "cboClass";
            // 
            // lblItem0
            // 
            this.lblItem0.AutoSize = true;
            this.lblItem0.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblItem0.Location = new System.Drawing.Point(44, 10);
            this.lblItem0.Name = "lblItem0";
            this.lblItem0.Size = new System.Drawing.Size(69, 20);
            this.lblItem0.TabIndex = 25;
            this.lblItem0.Text = "미수구분";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.PanelMain);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 102);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(948, 210);
            this.panel5.TabIndex = 182;
            // 
            // PanelMain
            // 
            this.PanelMain.BackColor = System.Drawing.SystemColors.Menu;
            this.PanelMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PanelMain.Controls.Add(this.cboDept);
            this.PanelMain.Controls.Add(this.dtpTDate);
            this.PanelMain.Controls.Add(this.label9);
            this.PanelMain.Controls.Add(this.dtpFDate);
            this.PanelMain.Controls.Add(this.label2);
            this.PanelMain.Controls.Add(this.btnHelp);
            this.PanelMain.Controls.Add(this.panel13);
            this.PanelMain.Controls.Add(this.cboMgrRank);
            this.PanelMain.Controls.Add(this.panel2);
            this.PanelMain.Controls.Add(this.label10);
            this.PanelMain.Controls.Add(this.GbGubun);
            this.PanelMain.Controls.Add(this.panel7);
            this.PanelMain.Controls.Add(this.TxtMirYYMM);
            this.PanelMain.Controls.Add(this.label12);
            this.PanelMain.Controls.Add(this.cboIO);
            this.PanelMain.Controls.Add(this.label4);
            this.PanelMain.Controls.Add(this.dtpBDate);
            this.PanelMain.Controls.Add(this.label3);
            this.PanelMain.Controls.Add(this.cboBun);
            this.PanelMain.Controls.Add(this.lblMiaName);
            this.PanelMain.Controls.Add(this.TxtGelCode);
            this.PanelMain.Controls.Add(this.label5);
            this.PanelMain.Controls.Add(this.label8);
            this.PanelMain.Controls.Add(this.lblWRTNO);
            this.PanelMain.Controls.Add(this.lblSname);
            this.PanelMain.Controls.Add(this.TxtMisuID);
            this.PanelMain.Controls.Add(this.label6);
            this.PanelMain.Controls.Add(this.label7);
            this.PanelMain.Location = new System.Drawing.Point(38, 5);
            this.PanelMain.Name = "PanelMain";
            this.PanelMain.Size = new System.Drawing.Size(877, 202);
            this.PanelMain.TabIndex = 11;
            // 
            // cboDept
            // 
            this.cboDept.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cboDept.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboDept.FormattingEnabled = true;
            this.cboDept.ItemHeight = 17;
            this.cboDept.Location = new System.Drawing.Point(599, 41);
            this.cboDept.Name = "cboDept";
            this.cboDept.Size = new System.Drawing.Size(138, 25);
            this.cboDept.TabIndex = 11;
            this.cboDept.Text = "cboDept";
            // 
            // dtpTDate
            // 
            this.dtpTDate.CustomFormat = "yyyy-MM-dd";
            this.dtpTDate.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTDate.Location = new System.Drawing.Point(708, 11);
            this.dtpTDate.Name = "dtpTDate";
            this.dtpTDate.Size = new System.Drawing.Size(103, 25);
            this.dtpTDate.TabIndex = 10;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(527, 43);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 20);
            this.label9.TabIndex = 127;
            this.label9.Text = "진 료 과";
            // 
            // dtpFDate
            // 
            this.dtpFDate.CustomFormat = "yyyy-MM-dd";
            this.dtpFDate.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFDate.Location = new System.Drawing.Point(599, 11);
            this.dtpFDate.Name = "dtpFDate";
            this.dtpFDate.Size = new System.Drawing.Size(103, 25);
            this.dtpFDate.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(527, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 20);
            this.label2.TabIndex = 125;
            this.label2.Text = "청구기간";
            // 
            // btnHelp
            // 
            this.btnHelp.BackColor = System.Drawing.Color.Transparent;
            this.btnHelp.Location = new System.Drawing.Point(198, 34);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(47, 27);
            this.btnHelp.TabIndex = 122;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = false;
            // 
            // panel13
            // 
            this.panel13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel13.Controls.Add(this.label11);
            this.panel13.Controls.Add(this.label13);
            this.panel13.Location = new System.Drawing.Point(599, 126);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(267, 64);
            this.panel13.TabIndex = 120;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.Location = new System.Drawing.Point(8, 35);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(245, 20);
            this.label11.TabIndex = 119;
            this.label11.Text = "25.기타수입  32.반송  99.삭감계산";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.Location = new System.Drawing.Point(7, 9);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(230, 20);
            this.label13.TabIndex = 20;
            this.label13.Text = "11.청구미수  21.입금  31.삭감액";
            // 
            // cboMgrRank
            // 
            this.cboMgrRank.FormattingEnabled = true;
            this.cboMgrRank.Location = new System.Drawing.Point(679, 94);
            this.cboMgrRank.Name = "cboMgrRank";
            this.cboMgrRank.Size = new System.Drawing.Size(122, 25);
            this.cboMgrRank.TabIndex = 120;
            this.cboMgrRank.Text = "cboMgrRank";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new System.Drawing.Point(803, 8);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(64, 72);
            this.panel2.TabIndex = 117;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(607, 96);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(69, 20);
            this.label10.TabIndex = 121;
            this.label10.Text = "미수등급";
            // 
            // GbGubun
            // 
            this.GbGubun.Controls.Add(this.cboGubun);
            this.GbGubun.Location = new System.Drawing.Point(425, 72);
            this.GbGubun.Name = "GbGubun";
            this.GbGubun.Size = new System.Drawing.Size(145, 51);
            this.GbGubun.TabIndex = 116;
            this.GbGubun.TabStop = false;
            this.GbGubun.Text = "지역구분(치매)";
            // 
            // cboGubun
            // 
            this.cboGubun.FormattingEnabled = true;
            this.cboGubun.Location = new System.Drawing.Point(11, 18);
            this.cboGubun.Name = "cboGubun";
            this.cboGubun.Size = new System.Drawing.Size(124, 25);
            this.cboGubun.TabIndex = 5;
            this.cboGubun.Text = "cboGubun";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.ssList1);
            this.panel7.Location = new System.Drawing.Point(27, 128);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(560, 62);
            this.panel7.TabIndex = 115;
            // 
            // ssList1
            // 
            this.ssList1.AccessibleDescription = "ssList1, Sheet1, Row 0, Column 0, ";
            this.ssList1.AutoScrollWhenKeyboardShowing = false;
            this.ssList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssList1.FocusRenderer = flatFocusIndicatorRenderer1;
            this.ssList1.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssList1.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer1.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            flatScrollBarRenderer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            flatScrollBarRenderer1.TrackBarBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(219)))), ((int)(((byte)(219)))));
            this.ssList1.HorizontalScrollBar.Renderer = flatScrollBarRenderer1;
            this.ssList1.HorizontalScrollBar.TabIndex = 113;
            this.ssList1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssList1.Location = new System.Drawing.Point(0, 0);
            this.ssList1.Name = "ssList1";
            namedStyle1.BackColor = System.Drawing.Color.White;
            filterBarCellType1.FormatString = "";
            namedStyle1.CellType = filterBarCellType1;
            namedStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Renderer = filterBarCellType1;
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle1.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            namedStyle2.BackColor = System.Drawing.Color.White;
            namedStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle2.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            namedStyle3.BackColor = System.Drawing.Color.White;
            namedStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle3.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            namedStyle4.BackColor = System.Drawing.Color.White;
            namedStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            flatCornerHeaderRenderer1.ActiveForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(213)))), ((int)(((byte)(213)))));
            flatCornerHeaderRenderer1.ActiveMouseOverForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(213)))), ((int)(((byte)(213)))));
            flatCornerHeaderRenderer1.GridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            flatCornerHeaderRenderer1.NormalTriangleColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(213)))), ((int)(((byte)(213)))));
            namedStyle4.Renderer = flatCornerHeaderRenderer1;
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle4.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            namedStyle5.BackColor = System.Drawing.SystemColors.Window;
            namedStyle5.CellType = generalCellType1;
            namedStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            namedStyle5.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle5.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle5.Renderer = generalCellType1;
            namedStyle5.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle5.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            namedStyle6.BackColor = System.Drawing.Color.DimGray;
            filterBarCellType2.FormatString = "";
            namedStyle6.CellType = filterBarCellType2;
            namedStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            namedStyle6.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle6.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle6.Renderer = filterBarCellType2;
            namedStyle6.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle6.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            namedStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(106)))), ((int)(((byte)(106)))));
            namedStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            namedStyle7.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle7.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle7.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle7.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            namedStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(106)))), ((int)(((byte)(106)))));
            namedStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            namedStyle8.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle8.NoteIndicatorColor = System.Drawing.Color.Red;
            flatCornerHeaderRenderer2.ActiveForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(240)))), ((int)(((byte)(224)))));
            flatCornerHeaderRenderer2.ActiveMouseOverForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            flatCornerHeaderRenderer2.NormalTriangleColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            namedStyle8.Renderer = flatCornerHeaderRenderer2;
            namedStyle8.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle8.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssList1.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4,
            namedStyle5,
            namedStyle6,
            namedStyle7,
            namedStyle8});
            this.ssList1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList1_Sheet1});
            this.ssList1.Size = new System.Drawing.Size(560, 62);
            this.ssList1.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2016Colorful;
            this.ssList1.TabIndex = 21;
            this.ssList1.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssList1.VerticalScrollBar.Name = "";
            flatScrollBarRenderer2.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            flatScrollBarRenderer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            flatScrollBarRenderer2.TrackBarBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(219)))), ((int)(((byte)(219)))));
            this.ssList1.VerticalScrollBar.Renderer = flatScrollBarRenderer2;
            this.ssList1.VerticalScrollBar.TabIndex = 114;
            this.ssList1.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            // 
            // ssList1_Sheet1
            // 
            this.ssList1_Sheet1.Reset();
            this.ssList1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssList1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssList1_Sheet1.ColumnCount = 5;
            this.ssList1_Sheet1.RowCount = 1;
            this.ssList1_Sheet1.Cells.Get(0, 0).BackColor = System.Drawing.Color.Yellow;
            this.ssList1_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlatOffice2016Colorful";
            this.ssList1_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlatOffice2016Colorful";
            this.ssList1_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "미수금액";
            this.ssList1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "입금금액";
            this.ssList1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "삭감금액";
            this.ssList1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "기    타";
            this.ssList1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "현재잔액";
            this.ssList1_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlatOffice2016Colorful";
            this.ssList1_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.ColumnHeader.Rows.Get(0).Height = 30F;
            this.ssList1_Sheet1.Columns.Get(0).CanFocus = false;
            this.ssList1_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssList1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssList1_Sheet1.Columns.Get(0).Label = "미수금액";
            this.ssList1_Sheet1.Columns.Get(0).Resizable = false;
            this.ssList1_Sheet1.Columns.Get(0).TabStop = false;
            this.ssList1_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList1_Sheet1.Columns.Get(0).Width = 120F;
            this.ssList1_Sheet1.Columns.Get(1).CanFocus = false;
            this.ssList1_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssList1_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssList1_Sheet1.Columns.Get(1).Label = "입금금액";
            this.ssList1_Sheet1.Columns.Get(1).Resizable = false;
            this.ssList1_Sheet1.Columns.Get(1).TabStop = false;
            this.ssList1_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList1_Sheet1.Columns.Get(1).Width = 120F;
            this.ssList1_Sheet1.Columns.Get(2).CanFocus = false;
            this.ssList1_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssList1_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssList1_Sheet1.Columns.Get(2).Label = "삭감금액";
            this.ssList1_Sheet1.Columns.Get(2).Resizable = false;
            this.ssList1_Sheet1.Columns.Get(2).TabStop = false;
            this.ssList1_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList1_Sheet1.Columns.Get(2).Width = 95F;
            this.ssList1_Sheet1.Columns.Get(3).CanFocus = false;
            this.ssList1_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssList1_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssList1_Sheet1.Columns.Get(3).Label = "기    타";
            this.ssList1_Sheet1.Columns.Get(3).Resizable = false;
            this.ssList1_Sheet1.Columns.Get(3).TabStop = false;
            this.ssList1_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList1_Sheet1.Columns.Get(3).Width = 103F;
            this.ssList1_Sheet1.Columns.Get(4).CanFocus = false;
            this.ssList1_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.ssList1_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssList1_Sheet1.Columns.Get(4).Label = "현재잔액";
            this.ssList1_Sheet1.Columns.Get(4).Resizable = false;
            this.ssList1_Sheet1.Columns.Get(4).TabStop = false;
            this.ssList1_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList1_Sheet1.Columns.Get(4).Width = 120F;
            this.ssList1_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.ssList1_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlatOffice2016Colorful";
            this.ssList1_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlatOffice2016Colorful";
            this.ssList1_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.LockBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(213)))));
            this.ssList1_Sheet1.RowHeader.Cells.Get(0, 0).Value = "1";
            this.ssList1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList1_Sheet1.RowHeader.Columns.Get(0).Width = 21F;
            this.ssList1_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlatOffice2016Colorful";
            this.ssList1_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.RowHeader.Visible = false;
            this.ssList1_Sheet1.Rows.Get(0).CanFocus = false;
            this.ssList1_Sheet1.Rows.Get(0).Height = 30F;
            this.ssList1_Sheet1.Rows.Get(0).LockBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ssList1_Sheet1.Rows.Get(0).Locked = true;
            this.ssList1_Sheet1.Rows.Get(0).Resizable = false;
            this.ssList1_Sheet1.Rows.Get(0).TabStop = false;
            this.ssList1_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlatOffice2016Colorful";
            this.ssList1_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // TxtMirYYMM
            // 
            this.TxtMirYYMM.Location = new System.Drawing.Point(299, 94);
            this.TxtMirYYMM.Name = "TxtMirYYMM";
            this.TxtMirYYMM.Size = new System.Drawing.Size(86, 25);
            this.TxtMirYYMM.TabIndex = 8;
            this.TxtMirYYMM.Text = "TxtMirYYMM";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.Location = new System.Drawing.Point(223, 96);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(69, 20);
            this.label12.TabIndex = 28;
            this.label12.Text = "통계년월";
            // 
            // cboIO
            // 
            this.cboIO.FormattingEnabled = true;
            this.cboIO.Location = new System.Drawing.Point(299, 65);
            this.cboIO.Name = "cboIO";
            this.cboIO.Size = new System.Drawing.Size(86, 25);
            this.cboIO.TabIndex = 4;
            this.cboIO.Text = "cboIO";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(224, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 20);
            this.label4.TabIndex = 17;
            this.label4.Text = "외래입원";
            // 
            // dtpBDate
            // 
            this.dtpBDate.CustomFormat = "yyyy-MM-dd";
            this.dtpBDate.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtpBDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpBDate.Location = new System.Drawing.Point(92, 65);
            this.dtpBDate.Name = "dtpBDate";
            this.dtpBDate.Size = new System.Drawing.Size(119, 25);
            this.dtpBDate.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(20, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 20);
            this.label3.TabIndex = 18;
            this.label3.Text = "청구일자";
            // 
            // cboBun
            // 
            this.cboBun.FormattingEnabled = true;
            this.cboBun.Location = new System.Drawing.Point(91, 94);
            this.cboBun.Name = "cboBun";
            this.cboBun.Size = new System.Drawing.Size(120, 25);
            this.cboBun.TabIndex = 6;
            this.cboBun.Text = "cboBun";
            // 
            // lblMiaName
            // 
            this.lblMiaName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMiaName.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblMiaName.ForeColor = System.Drawing.Color.Black;
            this.lblMiaName.Location = new System.Drawing.Point(248, 37);
            this.lblMiaName.Name = "lblMiaName";
            this.lblMiaName.Size = new System.Drawing.Size(176, 25);
            this.lblMiaName.TabIndex = 15;
            this.lblMiaName.Text = "lblMiaName";
            this.lblMiaName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TxtGelCode
            // 
            this.TxtGelCode.Location = new System.Drawing.Point(92, 36);
            this.TxtGelCode.Name = "TxtGelCode";
            this.TxtGelCode.Size = new System.Drawing.Size(100, 25);
            this.TxtGelCode.TabIndex = 2;
            this.TxtGelCode.Text = "TxtGelCode";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(20, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 20);
            this.label5.TabIndex = 10;
            this.label5.Text = "부담구분";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(20, 38);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 20);
            this.label8.TabIndex = 12;
            this.label8.Text = "계 약 처";
            // 
            // lblWRTNO
            // 
            this.lblWRTNO.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWRTNO.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblWRTNO.ForeColor = System.Drawing.Color.Black;
            this.lblWRTNO.Location = new System.Drawing.Point(379, 8);
            this.lblWRTNO.Name = "lblWRTNO";
            this.lblWRTNO.Size = new System.Drawing.Size(139, 25);
            this.lblWRTNO.TabIndex = 8;
            this.lblWRTNO.Text = "lblWRNTO";
            this.lblWRTNO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSname
            // 
            this.lblSname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSname.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSname.ForeColor = System.Drawing.Color.Black;
            this.lblSname.Location = new System.Drawing.Point(198, 8);
            this.lblSname.Name = "lblSname";
            this.lblSname.Size = new System.Drawing.Size(103, 25);
            this.lblSname.TabIndex = 16;
            this.lblSname.Text = "lblSname";
            this.lblSname.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TxtMisuID
            // 
            this.TxtMisuID.Location = new System.Drawing.Point(92, 8);
            this.TxtMisuID.Name = "TxtMisuID";
            this.TxtMisuID.Size = new System.Drawing.Size(100, 25);
            this.TxtMisuID.TabIndex = 1;
            this.TxtMisuID.Text = "TxtMisuID";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(310, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 20);
            this.label6.TabIndex = 9;
            this.label6.Text = "WRNTO";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(18, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 20);
            this.label7.TabIndex = 13;
            this.label7.Text = "병원번호";
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.panel11);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel10.Location = new System.Drawing.Point(0, 525);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(948, 173);
            this.panel10.TabIndex = 184;
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.ssList3);
            this.panel11.Location = new System.Drawing.Point(38, 5);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(876, 164);
            this.panel11.TabIndex = 0;
            // 
            // ssList3
            // 
            this.ssList3.AccessibleDescription = "";
            this.ssList3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssList3.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssList3.Location = new System.Drawing.Point(0, 0);
            this.ssList3.Name = "ssList3";
            this.ssList3.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList3_Sheet1});
            this.ssList3.Size = new System.Drawing.Size(876, 164);
            this.ssList3.TabIndex = 0;
            // 
            // ssList3_Sheet1
            // 
            this.ssList3_Sheet1.Reset();
            this.ssList3_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssList3_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssList3_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList3_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList3_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssList3_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList3_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList3_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList3_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssList3_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList3_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList3_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList3_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssList3_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList3_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList3_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList3_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.ssList3_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList3_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList3_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList3_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.ssList3_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList3_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList3_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList3_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssList3_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList3_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList3_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList3_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList3_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssList3_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList3_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList3_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList3_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssList3_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList3_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.panel9);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 312);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(948, 213);
            this.panel8.TabIndex = 183;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.ssList2);
            this.panel9.Location = new System.Drawing.Point(38, 5);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(876, 204);
            this.panel9.TabIndex = 0;
            // 
            // ssList2
            // 
            this.ssList2.AccessibleDescription = "";
            this.ssList2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssList2.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssList2.Location = new System.Drawing.Point(0, 0);
            this.ssList2.Name = "ssList2";
            this.ssList2.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList2_Sheet1});
            this.ssList2.Size = new System.Drawing.Size(876, 204);
            this.ssList2.TabIndex = 2;
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Back, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.StartEditing);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Return, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextCellThenControl);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke('='), FarPoint.Win.Spread.SpreadActions.StartEditingFormula);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.X, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardCut);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Insert, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardCopy);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Delete, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardCut);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Insert, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardPasteAll);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.F4, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.ShowSubEditor);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Space, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.SelectRow);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Z, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.Undo);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Y, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.Redo);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Tab, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.None);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Tab, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Left, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToPreviousCellThenControl);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Right, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextCellThenControl);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Left, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Right, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Left, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Right, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Left, ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Right, ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.C, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardCopyValues);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.V, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardPasteValues);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Up, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToPreviousRow);
            ssList2_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Down, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextRow);
            this.ssList2.SetInputMap(FarPoint.Win.Spread.InputMapMode.WhenFocused, FarPoint.Win.Spread.OperationMode.Normal, ssList2_InputMapWhenFocusedNormal);
            ssList2_InputMapWhenFocusedReadOnly.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Return, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextColumn);
            ssList2_InputMapWhenFocusedReadOnly.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Tab, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.None);
            ssList2_InputMapWhenFocusedReadOnly.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Tab, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            this.ssList2.SetInputMap(FarPoint.Win.Spread.InputMapMode.WhenFocused, FarPoint.Win.Spread.OperationMode.ReadOnly, ssList2_InputMapWhenFocusedReadOnly);
            ssList2_InputMapWhenFocusedRowMode.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Return, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextColumn);
            ssList2_InputMapWhenFocusedRowMode.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Left, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.None);
            ssList2_InputMapWhenFocusedRowMode.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Right, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.None);
            ssList2_InputMapWhenFocusedRowMode.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Left, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            ssList2_InputMapWhenFocusedRowMode.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Right, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            ssList2_InputMapWhenFocusedRowMode.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Left, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            ssList2_InputMapWhenFocusedRowMode.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Right, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            ssList2_InputMapWhenFocusedRowMode.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Left, ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            ssList2_InputMapWhenFocusedRowMode.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Right, ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            this.ssList2.SetInputMap(FarPoint.Win.Spread.InputMapMode.WhenFocused, FarPoint.Win.Spread.OperationMode.RowMode, ssList2_InputMapWhenFocusedRowMode);
            ssList2_InputMapWhenFocusedSingleSelect.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Return, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextColumn);
            this.ssList2.SetInputMap(FarPoint.Win.Spread.InputMapMode.WhenFocused, FarPoint.Win.Spread.OperationMode.SingleSelect, ssList2_InputMapWhenFocusedSingleSelect);
            ssList2_InputMapWhenFocusedMultiSelect.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Return, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextColumn);
            this.ssList2.SetInputMap(FarPoint.Win.Spread.InputMapMode.WhenFocused, FarPoint.Win.Spread.OperationMode.MultiSelect, ssList2_InputMapWhenFocusedMultiSelect);
            ssList2_InputMapWhenFocusedExtendedSelect.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Return, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextColumn);
            this.ssList2.SetInputMap(FarPoint.Win.Spread.InputMapMode.WhenFocused, FarPoint.Win.Spread.OperationMode.ExtendedSelect, ssList2_InputMapWhenFocusedExtendedSelect);
            ssList2_InputMapWhenAncestorOfFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Return, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextColumn);
            this.ssList2.SetInputMap(FarPoint.Win.Spread.InputMapMode.WhenAncestorOfFocused, FarPoint.Win.Spread.OperationMode.Normal, ssList2_InputMapWhenAncestorOfFocusedNormal);
            ssList2_InputMapWhenAncestorOfFocusedReadOnly.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Return, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextColumn);
            ssList2_InputMapWhenAncestorOfFocusedReadOnly.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Tab, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.None);
            ssList2_InputMapWhenAncestorOfFocusedReadOnly.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Tab, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            this.ssList2.SetInputMap(FarPoint.Win.Spread.InputMapMode.WhenAncestorOfFocused, FarPoint.Win.Spread.OperationMode.ReadOnly, ssList2_InputMapWhenAncestorOfFocusedReadOnly);
            ssList2_InputMapWhenAncestorOfFocusedRowMode.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Return, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextColumn);
            this.ssList2.SetInputMap(FarPoint.Win.Spread.InputMapMode.WhenAncestorOfFocused, FarPoint.Win.Spread.OperationMode.RowMode, ssList2_InputMapWhenAncestorOfFocusedRowMode);
            ssList2_InputMapWhenAncestorOfFocusedSingleSelect.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Return, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextColumn);
            this.ssList2.SetInputMap(FarPoint.Win.Spread.InputMapMode.WhenAncestorOfFocused, FarPoint.Win.Spread.OperationMode.SingleSelect, ssList2_InputMapWhenAncestorOfFocusedSingleSelect);
            ssList2_InputMapWhenAncestorOfFocusedMultiSelect.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Return, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextColumn);
            this.ssList2.SetInputMap(FarPoint.Win.Spread.InputMapMode.WhenAncestorOfFocused, FarPoint.Win.Spread.OperationMode.MultiSelect, ssList2_InputMapWhenAncestorOfFocusedMultiSelect);
            ssList2_InputMapWhenAncestorOfFocusedExtendedSelect.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Return, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextColumn);
            this.ssList2.SetInputMap(FarPoint.Win.Spread.InputMapMode.WhenAncestorOfFocused, FarPoint.Win.Spread.OperationMode.ExtendedSelect, ssList2_InputMapWhenAncestorOfFocusedExtendedSelect);
            // 
            // ssList2_Sheet1
            // 
            this.ssList2_Sheet1.Reset();
            this.ssList2_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssList2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssList2_Sheet1.ColumnCount = 1;
            this.ssList2_Sheet1.RowCount = 1;
            this.ssList2_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList2_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList2_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssList2_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList2_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList2_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList2_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssList2_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList2_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList2_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList2_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssList2_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList2_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList2_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList2_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.ssList2_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList2_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList2_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList2_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.ssList2_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList2_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList2_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList2_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssList2_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList2_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList2_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList2_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList2_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssList2_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList2_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList2_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList2_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssList2_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmPmpaMisuMast
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(948, 699);
            this.Controls.Add(this.panel10);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel12);
            this.Controls.Add(this.pan);
            this.Controls.Add(this.panTitle);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmPmpaMisuMast";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "미수원장관리";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel12.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.PanelMain.ResumeLayout(false);
            this.PanelMain.PerformLayout();
            this.panel13.ResumeLayout(false);
            this.panel13.PerformLayout();
            this.GbGubun.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList1_Sheet1)).EndInit();
            this.panel10.ResumeLayout(false);
            this.panel11.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssList3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList3_Sheet1)).EndInit();
            this.panel8.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssList2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList2_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem JobMenu;
        private System.Windows.Forms.ToolStripMenuItem Menu1_1;
        private System.Windows.Forms.ToolStripMenuItem Menu1_2;
        private System.Windows.Forms.ToolStripMenuItem Menu1_3;
        private System.Windows.Forms.ToolStripMenuItem Menu1_4;
        private System.Windows.Forms.Panel panTitle;
        public System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pan;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ComboBox cboClass;
        private System.Windows.Forms.Label lblItem0;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel PanelMain;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.TextBox TxtMirYYMM;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cboIO;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpBDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboBun;
        private System.Windows.Forms.Label lblMiaName;
        private System.Windows.Forms.TextBox TxtGelCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblWRTNO;
        private System.Windows.Forms.Label lblSname;
        private System.Windows.Forms.TextBox TxtMisuID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private FarPoint.Win.Spread.FpSpread ssList1;
        private FarPoint.Win.Spread.SheetView ssList1_Sheet1;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cboMgrRank;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox GbGubun;
        private System.Windows.Forms.ComboBox cboGubun;
        private System.Windows.Forms.CheckBox chkCash;
        private Panel panel10;
        private Panel panel11;
        private FarPoint.Win.Spread.FpSpread ssList3;
        private FarPoint.Win.Spread.SheetView ssList3_Sheet1;
        private Panel panel8;
        private Panel panel9;
        private FarPoint.Win.Spread.FpSpread ssList2;
        private FarPoint.Win.Spread.SheetView ssList2_Sheet1;
        private Button btnHelp;
        private ComboBox cboDept;
        private DateTimePicker dtpTDate;
        private Label label9;
        private DateTimePicker dtpFDate;
        private Label label2;
    }
}