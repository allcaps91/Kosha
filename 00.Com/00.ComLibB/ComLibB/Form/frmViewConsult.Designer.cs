namespace ComLibB
{
    partial class frmViewConsult
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
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.lblTitle0 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.pan0 = new System.Windows.Forms.Panel();
            this.grb3 = new System.Windows.Forms.GroupBox();
            this.opt24back = new System.Windows.Forms.RadioButton();
            this.opt24go = new System.Windows.Forms.RadioButton();
            this.optAll = new System.Windows.Forms.RadioButton();
            this.grb2 = new System.Windows.Forms.GroupBox();
            this.optER2 = new System.Windows.Forms.RadioButton();
            this.optER1 = new System.Windows.Forms.RadioButton();
            this.optER0 = new System.Windows.Forms.RadioButton();
            this.orb1 = new System.Windows.Forms.GroupBox();
            this.cboDept = new System.Windows.Forms.ComboBox();
            this.orb0 = new System.Windows.Forms.GroupBox();
            this.cboYear = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSuch = new System.Windows.Forms.Button();
            this.panTitleSub0.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.pan0.SuspendLayout();
            this.grb3.SuspendLayout();
            this.grb2.SuspendLayout();
            this.orb1.SuspendLayout();
            this.orb0.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
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
            this.panTitleSub0.Size = new System.Drawing.Size(914, 28);
            this.panTitleSub0.TabIndex = 14;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 4);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(83, 17);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "임상 질 지표";
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnSuch);
            this.panTitle.Controls.Add(this.btnPrint);
            this.panTitle.Controls.Add(this.lblTitle0);
            this.panTitle.Controls.Add(this.btnClose);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(914, 34);
            this.panTitle.TabIndex = 13;
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrint.Location = new System.Drawing.Point(766, 0);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 30);
            this.btnPrint.TabIndex = 29;
            this.btnPrint.Text = "출력";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // lblTitle0
            // 
            this.lblTitle0.AutoSize = true;
            this.lblTitle0.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle0.Location = new System.Drawing.Point(6, 5);
            this.lblTitle0.Name = "lblTitle0";
            this.lblTitle0.Size = new System.Drawing.Size(74, 21);
            this.lblTitle0.TabIndex = 4;
            this.lblTitle0.Text = "협의진료";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.Location = new System.Drawing.Point(838, 0);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(72, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "닫기";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(914, 691);
            this.panel1.TabIndex = 15;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.ssView);
            this.panel3.Controls.Add(this.pan0);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(914, 691);
            this.panel3.TabIndex = 1;
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "fpSpread1, Sheet1, Row 0, Column 0, ";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssView.Location = new System.Drawing.Point(0, 78);
            this.ssView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ssView.Name = "ssView";
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(914, 613);
            this.ssView.TabIndex = 1;
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 14;
            this.ssView_Sheet1.RowCount = 8;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "진료의사";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "1월";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "2월";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "3월";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "4월";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "5월";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "6월";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "7월";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "8월";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "9월";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "10월";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "11월";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "12월";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "합계";
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssView_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(0).Label = "진료의사";
            this.ssView_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(0).Width = 78F;
            this.ssView_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(1).Label = "1월";
            this.ssView_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(2).Label = "2월";
            this.ssView_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(3).Label = "3월";
            this.ssView_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(4).Label = "4월";
            this.ssView_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(5).Label = "5월";
            this.ssView_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(6).Label = "6월";
            this.ssView_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(7).Label = "7월";
            this.ssView_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(8).Label = "8월";
            this.ssView_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(9).Label = "9월";
            this.ssView_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(10).Label = "10월";
            this.ssView_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(11).Label = "11월";
            this.ssView_Sheet1.Columns.Get(11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(12).Label = "12월";
            this.ssView_Sheet1.Columns.Get(12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(13).Label = "합계";
            this.ssView_Sheet1.Columns.Get(13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // pan0
            // 
            this.pan0.BackColor = System.Drawing.Color.White;
            this.pan0.Controls.Add(this.grb3);
            this.pan0.Controls.Add(this.grb2);
            this.pan0.Controls.Add(this.orb1);
            this.pan0.Controls.Add(this.orb0);
            this.pan0.Dock = System.Windows.Forms.DockStyle.Top;
            this.pan0.Location = new System.Drawing.Point(0, 0);
            this.pan0.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pan0.Name = "pan0";
            this.pan0.Size = new System.Drawing.Size(914, 78);
            this.pan0.TabIndex = 2;
            // 
            // grb3
            // 
            this.grb3.Controls.Add(this.opt24back);
            this.grb3.Controls.Add(this.opt24go);
            this.grb3.Controls.Add(this.optAll);
            this.grb3.Location = new System.Drawing.Point(410, 8);
            this.grb3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grb3.Name = "grb3";
            this.grb3.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grb3.Size = new System.Drawing.Size(229, 61);
            this.grb3.TabIndex = 45;
            this.grb3.TabStop = false;
            this.grb3.Text = "24시간";
            // 
            // opt24back
            // 
            this.opt24back.AutoSize = true;
            this.opt24back.Location = new System.Drawing.Point(149, 24);
            this.opt24back.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.opt24back.Name = "opt24back";
            this.opt24back.Size = new System.Drawing.Size(75, 21);
            this.opt24back.TabIndex = 42;
            this.opt24back.TabStop = true;
            this.opt24back.Text = "24H이후";
            this.opt24back.UseVisualStyleBackColor = true;
            // 
            // opt24go
            // 
            this.opt24go.AutoSize = true;
            this.opt24go.Location = new System.Drawing.Point(66, 25);
            this.opt24go.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.opt24go.Name = "opt24go";
            this.opt24go.Size = new System.Drawing.Size(75, 21);
            this.opt24go.TabIndex = 41;
            this.opt24go.TabStop = true;
            this.opt24go.Text = "24H이내";
            this.opt24go.UseVisualStyleBackColor = true;
            // 
            // optAll
            // 
            this.optAll.AutoSize = true;
            this.optAll.Location = new System.Drawing.Point(6, 24);
            this.optAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optAll.Name = "optAll";
            this.optAll.Size = new System.Drawing.Size(52, 21);
            this.optAll.TabIndex = 40;
            this.optAll.TabStop = true;
            this.optAll.Text = "전체";
            this.optAll.UseVisualStyleBackColor = true;
            // 
            // grb2
            // 
            this.grb2.Controls.Add(this.optER2);
            this.grb2.Controls.Add(this.optER1);
            this.grb2.Controls.Add(this.optER0);
            this.grb2.Location = new System.Drawing.Point(210, 8);
            this.grb2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grb2.Name = "grb2";
            this.grb2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grb2.Size = new System.Drawing.Size(197, 61);
            this.grb2.TabIndex = 44;
            this.grb2.TabStop = false;
            this.grb2.Text = "응급여부";
            // 
            // optER2
            // 
            this.optER2.AutoSize = true;
            this.optER2.Location = new System.Drawing.Point(126, 24);
            this.optER2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optER2.Name = "optER2";
            this.optER2.Size = new System.Drawing.Size(65, 21);
            this.optER2.TabIndex = 42;
            this.optER2.TabStop = true;
            this.optER2.Text = "비응급";
            this.optER2.UseVisualStyleBackColor = true;
            // 
            // optER1
            // 
            this.optER1.AutoSize = true;
            this.optER1.Location = new System.Drawing.Point(66, 25);
            this.optER1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optER1.Name = "optER1";
            this.optER1.Size = new System.Drawing.Size(52, 21);
            this.optER1.TabIndex = 41;
            this.optER1.TabStop = true;
            this.optER1.Text = "응급";
            this.optER1.UseVisualStyleBackColor = true;
            // 
            // optER0
            // 
            this.optER0.AutoSize = true;
            this.optER0.Location = new System.Drawing.Point(6, 24);
            this.optER0.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optER0.Name = "optER0";
            this.optER0.Size = new System.Drawing.Size(52, 21);
            this.optER0.TabIndex = 40;
            this.optER0.TabStop = true;
            this.optER0.Text = "전체";
            this.optER0.UseVisualStyleBackColor = true;
            // 
            // orb1
            // 
            this.orb1.Controls.Add(this.cboDept);
            this.orb1.Location = new System.Drawing.Point(111, 8);
            this.orb1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.orb1.Name = "orb1";
            this.orb1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.orb1.Size = new System.Drawing.Size(93, 61);
            this.orb1.TabIndex = 43;
            this.orb1.TabStop = false;
            this.orb1.Text = "진료과";
            // 
            // cboDept
            // 
            this.cboDept.FormattingEnabled = true;
            this.cboDept.Location = new System.Drawing.Point(6, 24);
            this.cboDept.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboDept.Name = "cboDept";
            this.cboDept.Size = new System.Drawing.Size(81, 25);
            this.cboDept.TabIndex = 1;
            // 
            // orb0
            // 
            this.orb0.Controls.Add(this.cboYear);
            this.orb0.Location = new System.Drawing.Point(12, 8);
            this.orb0.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.orb0.Name = "orb0";
            this.orb0.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.orb0.Size = new System.Drawing.Size(93, 61);
            this.orb0.TabIndex = 42;
            this.orb0.TabStop = false;
            this.orb0.Text = "작업년도";
            // 
            // cboYear
            // 
            this.cboYear.FormattingEnabled = true;
            this.cboYear.Location = new System.Drawing.Point(6, 24);
            this.cboYear.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new System.Drawing.Size(81, 25);
            this.cboYear.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 782);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(914, 28);
            this.panel2.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(3, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(227, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "* 토요일, 일요일, 공휴일 제외";
            // 
            // btnSuch
            // 
            this.btnSuch.BackColor = System.Drawing.Color.Transparent;
            this.btnSuch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSuch.Location = new System.Drawing.Point(694, 0);
            this.btnSuch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSuch.Name = "btnSuch";
            this.btnSuch.Size = new System.Drawing.Size(72, 30);
            this.btnSuch.TabIndex = 30;
            this.btnSuch.Text = "조회";
            this.btnSuch.UseVisualStyleBackColor = false;
            this.btnSuch.Click += new System.EventHandler(this.btnSuch_Click);
            // 
            // frmViewConsult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 810);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmViewConsult";
            this.Text = "frmViewConsult";
            this.Load += new System.EventHandler(this.frmViewConsult_Load);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.pan0.ResumeLayout(false);
            this.grb3.ResumeLayout(false);
            this.grb3.PerformLayout();
            this.grb2.ResumeLayout(false);
            this.grb2.PerformLayout();
            this.orb1.ResumeLayout(false);
            this.orb0.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle0;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pan0;
        private System.Windows.Forms.GroupBox orb0;
        private System.Windows.Forms.GroupBox orb1;
        private System.Windows.Forms.ComboBox cboDept;
        private System.Windows.Forms.ComboBox cboYear;
        private System.Windows.Forms.GroupBox grb2;
        private System.Windows.Forms.GroupBox grb3;
        private System.Windows.Forms.RadioButton opt24back;
        private System.Windows.Forms.RadioButton opt24go;
        private System.Windows.Forms.RadioButton optAll;
        private System.Windows.Forms.RadioButton optER2;
        private System.Windows.Forms.RadioButton optER1;
        private System.Windows.Forms.RadioButton optER0;
        private System.Windows.Forms.Button btnSuch;
    }
}