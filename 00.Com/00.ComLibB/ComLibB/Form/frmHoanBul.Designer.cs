namespace ComLibB
{
    partial class frmHoanBul
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color352636457337099953802", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text434636457337099978847", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static550636457337099998897");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblROWID = new System.Windows.Forms.Label();
            this.btnSaveMonth = new System.Windows.Forms.Button();
            this.optMonth_6 = new System.Windows.Forms.RadioButton();
            this.optMonth_5 = new System.Windows.Forms.RadioButton();
            this.optMonth_4 = new System.Windows.Forms.RadioButton();
            this.optMonth_3 = new System.Windows.Forms.RadioButton();
            this.optMonth_2 = new System.Windows.Forms.RadioButton();
            this.optMonth_1 = new System.Windows.Forms.RadioButton();
            this.optMonth_0 = new System.Windows.Forms.RadioButton();
            this.lblTitleSub1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.optBun_1 = new System.Windows.Forms.RadioButton();
            this.optBun_0 = new System.Windows.Forms.RadioButton();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.lblPaName = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.txtPano = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTitleSub2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtPano1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ss1 = new FarPoint.Win.Spread.FpSpread();
            this.ss1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panel1.SuspendLayout();
            this.lblTitleSub1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.lblTitleSub2.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).BeginInit();
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
            this.panTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(587, 34);
            this.panTitle.TabIndex = 14;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(510, 1);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 42);
            this.btnExit.TabIndex = 27;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 7);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(90, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "환불재정산";
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(587, 28);
            this.panTitleSub0.TabIndex = 15;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 4);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(31, 15);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "환불";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.lblROWID);
            this.panel1.Controls.Add(this.btnSaveMonth);
            this.panel1.Controls.Add(this.optMonth_6);
            this.panel1.Controls.Add(this.optMonth_5);
            this.panel1.Controls.Add(this.optMonth_4);
            this.panel1.Controls.Add(this.optMonth_3);
            this.panel1.Controls.Add(this.optMonth_2);
            this.panel1.Controls.Add(this.optMonth_1);
            this.panel1.Controls.Add(this.optMonth_0);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(587, 46);
            this.panel1.TabIndex = 16;
            // 
            // lblROWID
            // 
            this.lblROWID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.lblROWID.Location = new System.Drawing.Point(9, 29);
            this.lblROWID.Name = "lblROWID";
            this.lblROWID.Size = new System.Drawing.Size(49, 10);
            this.lblROWID.TabIndex = 19;
            // 
            // btnSaveMonth
            // 
            this.btnSaveMonth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveMonth.BackColor = System.Drawing.Color.Transparent;
            this.btnSaveMonth.Location = new System.Drawing.Point(510, 6);
            this.btnSaveMonth.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSaveMonth.Name = "btnSaveMonth";
            this.btnSaveMonth.Size = new System.Drawing.Size(72, 30);
            this.btnSaveMonth.TabIndex = 18;
            this.btnSaveMonth.Text = "월저장";
            this.btnSaveMonth.UseVisualStyleBackColor = false;
            this.btnSaveMonth.Click += new System.EventHandler(this.btnSaveMonth_Click);
            // 
            // optMonth_6
            // 
            this.optMonth_6.AutoSize = true;
            this.optMonth_6.Location = new System.Drawing.Point(441, 5);
            this.optMonth_6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optMonth_6.Name = "optMonth_6";
            this.optMonth_6.Size = new System.Drawing.Size(59, 21);
            this.optMonth_6.TabIndex = 0;
            this.optMonth_6.TabStop = true;
            this.optMonth_6.Text = "9개월";
            this.optMonth_6.UseVisualStyleBackColor = true;
            // 
            // optMonth_5
            // 
            this.optMonth_5.AutoSize = true;
            this.optMonth_5.Location = new System.Drawing.Point(369, 5);
            this.optMonth_5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optMonth_5.Name = "optMonth_5";
            this.optMonth_5.Size = new System.Drawing.Size(59, 21);
            this.optMonth_5.TabIndex = 0;
            this.optMonth_5.TabStop = true;
            this.optMonth_5.Text = "8개월";
            this.optMonth_5.UseVisualStyleBackColor = true;
            // 
            // optMonth_4
            // 
            this.optMonth_4.AutoSize = true;
            this.optMonth_4.Location = new System.Drawing.Point(297, 5);
            this.optMonth_4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optMonth_4.Name = "optMonth_4";
            this.optMonth_4.Size = new System.Drawing.Size(59, 21);
            this.optMonth_4.TabIndex = 0;
            this.optMonth_4.TabStop = true;
            this.optMonth_4.Text = "7개월";
            this.optMonth_4.UseVisualStyleBackColor = true;
            // 
            // optMonth_3
            // 
            this.optMonth_3.AutoSize = true;
            this.optMonth_3.Location = new System.Drawing.Point(225, 5);
            this.optMonth_3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optMonth_3.Name = "optMonth_3";
            this.optMonth_3.Size = new System.Drawing.Size(59, 21);
            this.optMonth_3.TabIndex = 0;
            this.optMonth_3.TabStop = true;
            this.optMonth_3.Text = "6개월";
            this.optMonth_3.UseVisualStyleBackColor = true;
            // 
            // optMonth_2
            // 
            this.optMonth_2.AutoSize = true;
            this.optMonth_2.Location = new System.Drawing.Point(153, 5);
            this.optMonth_2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optMonth_2.Name = "optMonth_2";
            this.optMonth_2.Size = new System.Drawing.Size(59, 21);
            this.optMonth_2.TabIndex = 0;
            this.optMonth_2.TabStop = true;
            this.optMonth_2.Text = "5개월";
            this.optMonth_2.UseVisualStyleBackColor = true;
            // 
            // optMonth_1
            // 
            this.optMonth_1.AutoSize = true;
            this.optMonth_1.Location = new System.Drawing.Point(81, 5);
            this.optMonth_1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optMonth_1.Name = "optMonth_1";
            this.optMonth_1.Size = new System.Drawing.Size(59, 21);
            this.optMonth_1.TabIndex = 0;
            this.optMonth_1.TabStop = true;
            this.optMonth_1.Text = "4개월";
            this.optMonth_1.UseVisualStyleBackColor = true;
            // 
            // optMonth_0
            // 
            this.optMonth_0.AutoSize = true;
            this.optMonth_0.Location = new System.Drawing.Point(9, 5);
            this.optMonth_0.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optMonth_0.Name = "optMonth_0";
            this.optMonth_0.Size = new System.Drawing.Size(59, 21);
            this.optMonth_0.TabIndex = 0;
            this.optMonth_0.TabStop = true;
            this.optMonth_0.Text = "3개월";
            this.optMonth_0.UseVisualStyleBackColor = true;
            // 
            // lblTitleSub1
            // 
            this.lblTitleSub1.BackColor = System.Drawing.Color.RoyalBlue;
            this.lblTitleSub1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTitleSub1.Controls.Add(this.label1);
            this.lblTitleSub1.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitleSub1.Location = new System.Drawing.Point(0, 108);
            this.lblTitleSub1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblTitleSub1.Name = "lblTitleSub1";
            this.lblTitleSub1.Size = new System.Drawing.Size(587, 28);
            this.lblTitleSub1.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(8, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "개인별";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Controls.Add(this.optBun_1);
            this.panel3.Controls.Add(this.optBun_0);
            this.panel3.Controls.Add(this.dtpDate);
            this.panel3.Controls.Add(this.lblPaName);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.txtRemark);
            this.panel3.Controls.Add(this.txtPano);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 136);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(587, 80);
            this.panel3.TabIndex = 18;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.Location = new System.Drawing.Point(510, 1);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 30);
            this.btnSave.TabIndex = 19;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // optBun_1
            // 
            this.optBun_1.AutoSize = true;
            this.optBun_1.Location = new System.Drawing.Point(422, 6);
            this.optBun_1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optBun_1.Name = "optBun_1";
            this.optBun_1.Size = new System.Drawing.Size(78, 21);
            this.optBun_1.TabIndex = 5;
            this.optBun_1.TabStop = true;
            this.optBun_1.Text = "환불취소";
            this.optBun_1.UseVisualStyleBackColor = true;
            // 
            // optBun_0
            // 
            this.optBun_0.AutoSize = true;
            this.optBun_0.Location = new System.Drawing.Point(359, 6);
            this.optBun_0.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optBun_0.Name = "optBun_0";
            this.optBun_0.Size = new System.Drawing.Size(52, 21);
            this.optBun_0.TabIndex = 6;
            this.optBun_0.TabStop = true;
            this.optBun_0.Text = "환불";
            this.optBun_0.UseVisualStyleBackColor = true;
            // 
            // dtpDate
            // 
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(252, 5);
            this.dtpDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(101, 25);
            this.dtpDate.TabIndex = 4;
            this.dtpDate.ValueChanged += new System.EventHandler(this.dtpDate_ValueChanged);
            // 
            // lblPaName
            // 
            this.lblPaName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPaName.Location = new System.Drawing.Point(164, 5);
            this.lblPaName.Name = "lblPaName";
            this.lblPaName.Size = new System.Drawing.Size(82, 25);
            this.lblPaName.TabIndex = 3;
            this.lblPaName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "사      유";
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(79, 47);
            this.txtRemark.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(422, 25);
            this.txtRemark.TabIndex = 1;
            // 
            // txtPano
            // 
            this.txtPano.Location = new System.Drawing.Point(79, 5);
            this.txtPano.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPano.Name = "txtPano";
            this.txtPano.Size = new System.Drawing.Size(82, 25);
            this.txtPano.TabIndex = 1;
            this.txtPano.Enter += new System.EventHandler(this.txtPano_Enter);
            this.txtPano.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPano_KeyDown);
            this.txtPano.Leave += new System.EventHandler(this.txtPano_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "등록번호";
            // 
            // lblTitleSub2
            // 
            this.lblTitleSub2.BackColor = System.Drawing.Color.RoyalBlue;
            this.lblTitleSub2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTitleSub2.Controls.Add(this.label2);
            this.lblTitleSub2.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitleSub2.Location = new System.Drawing.Point(0, 216);
            this.lblTitleSub2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblTitleSub2.Name = "lblTitleSub2";
            this.lblTitleSub2.Size = new System.Drawing.Size(587, 28);
            this.lblTitleSub2.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(8, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "조회";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.White;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel5.Controls.Add(this.btnCancel);
            this.panel5.Controls.Add(this.btnSearch);
            this.panel5.Controls.Add(this.txtPano1);
            this.panel5.Controls.Add(this.label5);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 244);
            this.panel5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(587, 36);
            this.panel5.TabIndex = 20;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Location = new System.Drawing.Point(510, 1);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 30);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Location = new System.Drawing.Point(434, 1);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 20;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtPano1
            // 
            this.txtPano1.Location = new System.Drawing.Point(341, 4);
            this.txtPano1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPano1.Name = "txtPano1";
            this.txtPano1.Size = new System.Drawing.Size(82, 25);
            this.txtPano1.TabIndex = 3;
            this.txtPano1.Enter += new System.EventHandler(this.txtPano1_Enter);
            this.txtPano1.Leave += new System.EventHandler(this.txtPano1_Leave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(280, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 17);
            this.label5.TabIndex = 2;
            this.label5.Text = "등록번호";
            // 
            // ss1
            // 
            this.ss1.AccessibleDescription = "";
            this.ss1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ss1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ss1.Location = new System.Drawing.Point(0, 280);
            this.ss1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ss1.Name = "ss1";
            namedStyle1.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType1.MaxLength = 60;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType2.Static = true;
            namedStyle3.CellType = textCellType2;
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.Renderer = textCellType2;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3});
            this.ss1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ss1_Sheet1});
            this.ss1.Size = new System.Drawing.Size(587, 625);
            this.ss1.TabIndex = 21;
            this.ss1.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ss1.TextTipAppearance = tipAppearance1;
            this.ss1.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ss1.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ss1_CellDoubleClick);
            // 
            // ss1_Sheet1
            // 
            this.ss1_Sheet1.Reset();
            this.ss1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ss1_Sheet1.ColumnCount = 4;
            this.ss1_Sheet1.RowCount = 50;
            this.ss1_Sheet1.Cells.Get(0, 0).Value = "81000004";
            this.ss1_Sheet1.Cells.Get(0, 1).StyleName = "Static550636457337099998897";
            this.ss1_Sheet1.Cells.Get(0, 1).Value = "2003-01-01";
            this.ss1_Sheet1.Cells.Get(0, 3).StyleName = "Static550636457337099998897";
            this.ss1_Sheet1.Cells.Get(0, 3).Value = "환불";
            this.ss1_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "등록번호";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "일 자";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "내 용";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "구분";
            this.ss1_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.ColumnHeader.Rows.Get(0).Height = 21F;
            this.ss1_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.Columns.Get(0).Label = "등록번호";
            this.ss1_Sheet1.Columns.Get(0).StyleName = "Static550636457337099998897";
            this.ss1_Sheet1.Columns.Get(0).Width = 74F;
            this.ss1_Sheet1.Columns.Get(1).Label = "일 자";
            this.ss1_Sheet1.Columns.Get(1).StyleName = "Static550636457337099998897";
            this.ss1_Sheet1.Columns.Get(1).Width = 86F;
            this.ss1_Sheet1.Columns.Get(2).Label = "내 용";
            this.ss1_Sheet1.Columns.Get(2).Width = 217F;
            this.ss1_Sheet1.Columns.Get(3).Label = "구분";
            this.ss1_Sheet1.Columns.Get(3).StyleName = "Static550636457337099998897";
            this.ss1_Sheet1.Columns.Get(3).Width = 37F;
            this.ss1_Sheet1.DefaultStyleName = "Text434636457337099978847";
            this.ss1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ss1_Sheet1.Rows.Default.Height = 18F;
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmHoanBul
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 905);
            this.Controls.Add(this.ss1);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.lblTitleSub2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.lblTitleSub1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHoanBul";
            this.Text = "환불재정산";
            this.Load += new System.EventHandler(this.frmHoanBul_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.lblTitleSub1.ResumeLayout(false);
            this.lblTitleSub1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.lblTitleSub2.ResumeLayout(false);
            this.lblTitleSub2.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton optMonth_6;
        private System.Windows.Forms.RadioButton optMonth_5;
        private System.Windows.Forms.RadioButton optMonth_4;
        private System.Windows.Forms.RadioButton optMonth_3;
        private System.Windows.Forms.RadioButton optMonth_2;
        private System.Windows.Forms.RadioButton optMonth_1;
        private System.Windows.Forms.RadioButton optMonth_0;
        private System.Windows.Forms.Panel lblTitleSub1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel lblTitleSub2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnSaveMonth;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.RadioButton optBun_1;
        private System.Windows.Forms.RadioButton optBun_0;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label lblPaName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.TextBox txtPano;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtPano1;
        private System.Windows.Forms.Label label5;
        private FarPoint.Win.Spread.FpSpread ss1;
        private FarPoint.Win.Spread.SheetView ss1_Sheet1;
        private System.Windows.Forms.Label lblROWID;
    }
}