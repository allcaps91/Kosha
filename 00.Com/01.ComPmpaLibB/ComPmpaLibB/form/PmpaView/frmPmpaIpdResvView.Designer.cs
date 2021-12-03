namespace ComPmpaLibB
{
    partial class frmPmpaIpdResvView
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
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("ko-KR", false);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPmpaIpdResvView));
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panMsg = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkDel = new System.Windows.Forms.CheckBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.cboDept = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dtpDate2 = new System.Windows.Forms.DateTimePicker();
            this.dtpDate1 = new System.Windows.Forms.DateTimePicker();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rdoGbn2 = new System.Windows.Forms.RadioButton();
            this.rdoGbn1 = new System.Windows.Forms.RadioButton();
            this.SS1 = new FarPoint.Win.Spread.FpSpread();
            this.cntMnStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.입원예약증출력ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.임의DRG입원예약증출력ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.임의선택진료입원예약증출력ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.임의DSC입원예약증출력ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.입원예약증출력입원일선택ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SS1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panMsg.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).BeginInit();
            this.cntMnStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Padding = new System.Windows.Forms.Padding(3);
            this.panTitle.Size = new System.Drawing.Size(1147, 36);
            this.panTitle.TabIndex = 12;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(1029, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(111, 26);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(6, 7);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(160, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "입원예약자 명단조회";
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.btnSearch);
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 36);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Padding = new System.Windows.Forms.Padding(3);
            this.panTitleSub0.Size = new System.Drawing.Size(1147, 38);
            this.panTitleSub0.TabIndex = 13;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Location = new System.Drawing.Point(1029, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(111, 28);
            this.btnSearch.TabIndex = 18;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(10, 15);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(127, 12);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "입원예약자 명단조회";
            // 
            // panMsg
            // 
            this.panMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panMsg.Controls.Add(this.label4);
            this.panMsg.Controls.Add(this.label3);
            this.panMsg.Controls.Add(this.label1);
            this.panMsg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panMsg.Location = new System.Drawing.Point(0, 560);
            this.panMsg.Name = "panMsg";
            this.panMsg.Size = new System.Drawing.Size(1147, 37);
            this.panMsg.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label4.Location = new System.Drawing.Point(646, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(255, 15);
            this.label4.TabIndex = 2;
            this.label4.Text = "기타 다른 셀영역 더블클릭시 입원예약건 취소";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label3.Location = new System.Drawing.Point(347, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(247, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "입원희망일 더블클릭시 입원희망일 변경가능";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.Location = new System.Drawing.Point(11, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(279, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "명단을 마우스 우클릭시 입원의뢰서 인쇄목록 팝업";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkDel);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 74);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1147, 42);
            this.panel1.TabIndex = 15;
            // 
            // chkDel
            // 
            this.chkDel.AutoSize = true;
            this.chkDel.Location = new System.Drawing.Point(567, 12);
            this.chkDel.Name = "chkDel";
            this.chkDel.Size = new System.Drawing.Size(62, 19);
            this.chkDel.TabIndex = 5;
            this.chkDel.Text = "취소건";
            this.chkDel.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.cboDept);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(379, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(182, 42);
            this.panel4.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "진료과";
            // 
            // cboDept
            // 
            this.cboDept.FormattingEnabled = true;
            this.cboDept.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.cboDept.Location = new System.Drawing.Point(59, 8);
            this.cboDept.Name = "cboDept";
            this.cboDept.Size = new System.Drawing.Size(107, 23);
            this.cboDept.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dtpDate2);
            this.panel3.Controls.Add(this.dtpDate1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(168, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(211, 42);
            this.panel3.TabIndex = 3;
            // 
            // dtpDate2
            // 
            this.dtpDate2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate2.Location = new System.Drawing.Point(110, 8);
            this.dtpDate2.Name = "dtpDate2";
            this.dtpDate2.Size = new System.Drawing.Size(86, 23);
            this.dtpDate2.TabIndex = 4;
            // 
            // dtpDate1
            // 
            this.dtpDate1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate1.Location = new System.Drawing.Point(19, 8);
            this.dtpDate1.Name = "dtpDate1";
            this.dtpDate1.Size = new System.Drawing.Size(85, 23);
            this.dtpDate1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rdoGbn2);
            this.panel2.Controls.Add(this.rdoGbn1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(168, 42);
            this.panel2.TabIndex = 0;
            // 
            // rdoGbn2
            // 
            this.rdoGbn2.AutoSize = true;
            this.rdoGbn2.Location = new System.Drawing.Point(91, 11);
            this.rdoGbn2.Name = "rdoGbn2";
            this.rdoGbn2.Size = new System.Drawing.Size(73, 19);
            this.rdoGbn2.TabIndex = 3;
            this.rdoGbn2.Text = "예약일자";
            this.rdoGbn2.UseVisualStyleBackColor = true;
            // 
            // rdoGbn1
            // 
            this.rdoGbn1.AutoSize = true;
            this.rdoGbn1.Checked = true;
            this.rdoGbn1.Location = new System.Drawing.Point(12, 11);
            this.rdoGbn1.Name = "rdoGbn1";
            this.rdoGbn1.Size = new System.Drawing.Size(73, 19);
            this.rdoGbn1.TabIndex = 2;
            this.rdoGbn1.TabStop = true;
            this.rdoGbn1.Text = "신청일자";
            this.rdoGbn1.UseVisualStyleBackColor = true;
            // 
            // SS1
            // 
            this.SS1.AccessibleDescription = "SS1, Sheet1, Row 0, Column 0, 81000004";
            this.SS1.ContextMenuStrip = this.cntMnStrip;
            this.SS1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SS1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.SS1.Location = new System.Drawing.Point(0, 116);
            this.SS1.Name = "SS1";
            this.SS1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS1_Sheet1});
            this.SS1.Size = new System.Drawing.Size(1147, 444);
            this.SS1.TabIndex = 16;
            this.SS1.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SS1.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SS1_CellClick);
            this.SS1.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SS1_CellDoubleClick);
            // 
            // cntMnStrip
            // 
            this.cntMnStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.입원예약증출력ToolStripMenuItem,
            this.임의DRG입원예약증출력ToolStripMenuItem,
            this.임의선택진료입원예약증출력ToolStripMenuItem,
            this.임의DSC입원예약증출력ToolStripMenuItem,
            this.입원예약증출력입원일선택ToolStripMenuItem});
            this.cntMnStrip.Name = "cntMnStrip";
            this.cntMnStrip.Size = new System.Drawing.Size(319, 114);
            // 
            // 입원예약증출력ToolStripMenuItem
            // 
            this.입원예약증출력ToolStripMenuItem.Name = "입원예약증출력ToolStripMenuItem";
            this.입원예약증출력ToolStripMenuItem.Size = new System.Drawing.Size(318, 22);
            this.입원예약증출력ToolStripMenuItem.Text = "입원예약증출력";
            this.입원예약증출력ToolStripMenuItem.Click += new System.EventHandler(this.입원예약증출력ToolStripMenuItem_Click);
            // 
            // 임의DRG입원예약증출력ToolStripMenuItem
            // 
            this.임의DRG입원예약증출력ToolStripMenuItem.Name = "임의DRG입원예약증출력ToolStripMenuItem";
            this.임의DRG입원예약증출력ToolStripMenuItem.Size = new System.Drawing.Size(318, 22);
            this.임의DRG입원예약증출력ToolStripMenuItem.Text = "임의 DRG 입원예약증출력(자료저장없음)";
            this.임의DRG입원예약증출력ToolStripMenuItem.Click += new System.EventHandler(this.임의DRG입원예약증출력ToolStripMenuItem_Click);
            // 
            // 임의선택진료입원예약증출력ToolStripMenuItem
            // 
            this.임의선택진료입원예약증출력ToolStripMenuItem.Name = "임의선택진료입원예약증출력ToolStripMenuItem";
            this.임의선택진료입원예약증출력ToolStripMenuItem.Size = new System.Drawing.Size(318, 22);
            this.임의선택진료입원예약증출력ToolStripMenuItem.Text = "임의 선택진료 입원예약증출력(자료저장없음)";
            this.임의선택진료입원예약증출력ToolStripMenuItem.Click += new System.EventHandler(this.임의선택진료입원예약증출력ToolStripMenuItem_Click);
            // 
            // 임의DSC입원예약증출력ToolStripMenuItem
            // 
            this.임의DSC입원예약증출력ToolStripMenuItem.Name = "임의DSC입원예약증출력ToolStripMenuItem";
            this.임의DSC입원예약증출력ToolStripMenuItem.Size = new System.Drawing.Size(318, 22);
            this.임의DSC입원예약증출력ToolStripMenuItem.Text = "임의 DSC 입원예약증출력(자료저장없음)";
            this.임의DSC입원예약증출력ToolStripMenuItem.Click += new System.EventHandler(this.임의DSC입원예약증출력ToolStripMenuItem_Click);
            // 
            // 입원예약증출력입원일선택ToolStripMenuItem
            // 
            this.입원예약증출력입원일선택ToolStripMenuItem.Name = "입원예약증출력입원일선택ToolStripMenuItem";
            this.입원예약증출력입원일선택ToolStripMenuItem.Size = new System.Drawing.Size(318, 22);
            this.입원예약증출력입원일선택ToolStripMenuItem.Text = "입원예약증출력(입원일선택)";
            this.입원예약증출력입원일선택ToolStripMenuItem.Click += new System.EventHandler(this.입원예약증출력입원일선택ToolStripMenuItem_Click);
            // 
            // SS1_Sheet1
            // 
            this.SS1_Sheet1.Reset();
            this.SS1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS1_Sheet1.ColumnCount = 17;
            this.SS1_Sheet1.RowCount = 1;
            this.SS1_Sheet1.Cells.Get(0, 0).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(0, 0).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(0, 0).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.SS1_Sheet1.Cells.Get(0, 0).ParseFormatString = "n";
            this.SS1_Sheet1.Cells.Get(0, 0).Value = 81000004;
            this.SS1_Sheet1.Cells.Get(0, 1).Value = "김수한무거";
            this.SS1_Sheet1.Cells.Get(0, 2).Value = "123456-7890123";
            this.SS1_Sheet1.Cells.Get(0, 3).Value = "054-999-9999";
            this.SS1_Sheet1.Cells.Get(0, 4).Value = "010-9999-9999";
            this.SS1_Sheet1.Cells.Get(0, 5).ParseFormatInfo = ((System.Globalization.DateTimeFormatInfo)(cultureInfo.DateTimeFormat.Clone()));
            this.SS1_Sheet1.Cells.Get(0, 5).ParseFormatString = "yyyy-MM-dd";
            this.SS1_Sheet1.Cells.Get(0, 5).Value = new System.DateTime(2999, 12, 31, 0, 0, 0, 0);
            this.SS1_Sheet1.Cells.Get(0, 6).ParseFormatInfo = ((System.Globalization.DateTimeFormatInfo)(cultureInfo.DateTimeFormat.Clone()));
            this.SS1_Sheet1.Cells.Get(0, 6).ParseFormatString = "yyyy-MM-dd";
            this.SS1_Sheet1.Cells.Get(0, 6).Value = new System.DateTime(2999, 12, 31, 0, 0, 0, 0);
            this.SS1_Sheet1.Cells.Get(0, 7).Value = "MG";
            this.SS1_Sheet1.Cells.Get(0, 8).Value = "김수한무거";
            this.SS1_Sheet1.Cells.Get(0, 9).ParseFormatInfo = ((System.Globalization.DateTimeFormatInfo)(cultureInfo.DateTimeFormat.Clone()));
            this.SS1_Sheet1.Cells.Get(0, 9).ParseFormatString = "yyyy-MM-dd";
            this.SS1_Sheet1.Cells.Get(0, 9).Value = new System.DateTime(2999, 12, 31, 0, 0, 0, 0);
            this.SS1_Sheet1.Cells.Get(0, 10).Value = "가나다라마바사아자차카타파하";
            this.SS1_Sheet1.Cells.Get(0, 11).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(0, 11).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(0, 11).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.SS1_Sheet1.Cells.Get(0, 11).ParseFormatString = "n";
            this.SS1_Sheet1.Cells.Get(0, 11).Value = 123;
            this.SS1_Sheet1.Cells.Get(0, 12).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(0, 12).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(0, 12).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.SS1_Sheet1.Cells.Get(0, 12).ParseFormatString = "n";
            this.SS1_Sheet1.Cells.Get(0, 12).Value = 1324;
            this.SS1_Sheet1.Cells.Get(0, 13).Value = "D";
            this.SS1_Sheet1.Cells.Get(0, 14).Value = "Y";
            this.SS1_Sheet1.Cells.Get(0, 15).Value = "Y";
            this.SS1_Sheet1.Cells.Get(0, 16).Value = "Y";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "등록번호";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "환자명";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "주민번호";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "전화번호";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "휴대폰번호";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "진료일자";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "입원희망일";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "진료과";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "진료과장";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "예약원무확인";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "참고사항";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "ROWID";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "의사코드";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "DRG";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "특진";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 15).Value = "일일\r\n수술";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 16).Value = "60\r\n병동";
            this.SS1_Sheet1.ColumnHeader.Rows.Get(0).Height = 32F;
            this.SS1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(0).Label = "등록번호";
            this.SS1_Sheet1.Columns.Get(0).Locked = true;
            this.SS1_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(0).Width = 64F;
            this.SS1_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(1).Label = "환자명";
            this.SS1_Sheet1.Columns.Get(1).Locked = true;
            this.SS1_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(1).Width = 68F;
            this.SS1_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(2).Label = "주민번호";
            this.SS1_Sheet1.Columns.Get(2).Locked = true;
            this.SS1_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(2).Width = 104F;
            this.SS1_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(3).Label = "전화번호";
            this.SS1_Sheet1.Columns.Get(3).Locked = true;
            this.SS1_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(3).Width = 88F;
            this.SS1_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(4).Label = "휴대폰번호";
            this.SS1_Sheet1.Columns.Get(4).Locked = true;
            this.SS1_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(4).Width = 95F;
            this.SS1_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(5).Label = "진료일자";
            this.SS1_Sheet1.Columns.Get(5).Locked = true;
            this.SS1_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(5).Width = 74F;
            this.SS1_Sheet1.Columns.Get(6).BackColor = System.Drawing.Color.LemonChiffon;
            this.SS1_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(6).Label = "입원희망일";
            this.SS1_Sheet1.Columns.Get(6).Locked = true;
            this.SS1_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(6).Width = 74F;
            this.SS1_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(7).Label = "진료과";
            this.SS1_Sheet1.Columns.Get(7).Locked = true;
            this.SS1_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(7).Width = 45F;
            this.SS1_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(8).Label = "진료과장";
            this.SS1_Sheet1.Columns.Get(8).Locked = true;
            this.SS1_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(8).Width = 68F;
            this.SS1_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(9).Label = "예약원무확인";
            this.SS1_Sheet1.Columns.Get(9).Locked = true;
            this.SS1_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(9).Width = 81F;
            textCellType1.WordWrap = true;
            this.SS1_Sheet1.Columns.Get(10).CellType = textCellType1;
            this.SS1_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS1_Sheet1.Columns.Get(10).Label = "참고사항";
            this.SS1_Sheet1.Columns.Get(10).Locked = true;
            this.SS1_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(10).Width = 194F;
            this.SS1_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(11).Label = "ROWID";
            this.SS1_Sheet1.Columns.Get(11).Locked = true;
            this.SS1_Sheet1.Columns.Get(11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(11).Visible = false;
            this.SS1_Sheet1.Columns.Get(11).Width = 48F;
            this.SS1_Sheet1.Columns.Get(12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(12).Label = "의사코드";
            this.SS1_Sheet1.Columns.Get(12).Locked = true;
            this.SS1_Sheet1.Columns.Get(12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(12).Visible = false;
            this.SS1_Sheet1.Columns.Get(12).Width = 57F;
            this.SS1_Sheet1.Columns.Get(13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(13).Label = "DRG";
            this.SS1_Sheet1.Columns.Get(13).Locked = true;
            this.SS1_Sheet1.Columns.Get(13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(13).Width = 33F;
            this.SS1_Sheet1.Columns.Get(14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(14).Label = "특진";
            this.SS1_Sheet1.Columns.Get(14).Locked = true;
            this.SS1_Sheet1.Columns.Get(14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(14).Width = 33F;
            this.SS1_Sheet1.Columns.Get(15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(15).Label = "일일\r\n수술";
            this.SS1_Sheet1.Columns.Get(15).Locked = true;
            this.SS1_Sheet1.Columns.Get(15).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(15).Width = 33F;
            this.SS1_Sheet1.Columns.Get(16).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(16).Label = "60\r\n병동";
            this.SS1_Sheet1.Columns.Get(16).Locked = true;
            this.SS1_Sheet1.Columns.Get(16).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(16).Width = 33F;
            this.SS1_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SS1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS1_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmPmpaIpdResvView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1147, 597);
            this.Controls.Add(this.SS1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panMsg);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmPmpaIpdResvView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmPmpaIpdResvView_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panMsg.ResumeLayout(false);
            this.panMsg.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).EndInit();
            this.cntMnStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        public System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panMsg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkDel;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboDept;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DateTimePicker dtpDate2;
        private System.Windows.Forms.DateTimePicker dtpDate1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rdoGbn2;
        private System.Windows.Forms.RadioButton rdoGbn1;
        private FarPoint.Win.Spread.FpSpread SS1;
        private FarPoint.Win.Spread.SheetView SS1_Sheet1;
        private System.Windows.Forms.ContextMenuStrip cntMnStrip;
        private System.Windows.Forms.ToolStripMenuItem 입원예약증출력ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 임의DRG입원예약증출력ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 임의선택진료입원예약증출력ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 임의DSC입원예약증출력ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 입원예약증출력입원일선택ToolStripMenuItem;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
    }
}