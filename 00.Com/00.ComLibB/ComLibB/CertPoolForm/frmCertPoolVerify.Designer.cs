namespace ComLibB
{
    partial class frmCertPoolVerify
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType64 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType65 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType66 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType67 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType68 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType69 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType70 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType15 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType16 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType71 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType72 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.panel11 = new System.Windows.Forms.Panel();
            this.BtnView = new System.Windows.Forms.Button();
            this.BtnOk = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.TxtName = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.DtpEDate = new System.Windows.Forms.DateTimePicker();
            this.DtpSDate = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RdoGubun_1 = new System.Windows.Forms.RadioButton();
            this.RdoGubun_0 = new System.Windows.Forms.RadioButton();
            this.pan00 = new System.Windows.Forms.Panel();
            this.lbl00 = new System.Windows.Forms.Label();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.panel4.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel11.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pan00.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel10);
            this.panel4.Controls.Add(this.groupBox3);
            this.panel4.Controls.Add(this.groupBox2);
            this.panel4.Controls.Add(this.groupBox1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(3);
            this.panel4.Size = new System.Drawing.Size(892, 49);
            this.panel4.TabIndex = 38;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.panel11);
            this.panel10.Controls.Add(this.btnPrint);
            this.panel10.Controls.Add(this.btnExit);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel10.Location = new System.Drawing.Point(451, 3);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(438, 43);
            this.panel10.TabIndex = 3;
            // 
            // panel11
            // 
            this.panel11.AutoSize = true;
            this.panel11.Controls.Add(this.BtnView);
            this.panel11.Controls.Add(this.BtnOk);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel11.Location = new System.Drawing.Point(58, 0);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(190, 43);
            this.panel11.TabIndex = 36;
            // 
            // BtnView
            // 
            this.BtnView.BackColor = System.Drawing.Color.Transparent;
            this.BtnView.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnView.Location = new System.Drawing.Point(0, 0);
            this.BtnView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnView.Name = "BtnView";
            this.BtnView.Size = new System.Drawing.Size(95, 43);
            this.BtnView.TabIndex = 37;
            this.BtnView.Text = "조회";
            this.BtnView.UseVisualStyleBackColor = false;
            this.BtnView.Click += new System.EventHandler(this.BtnView_Click);
            // 
            // BtnOk
            // 
            this.BtnOk.BackColor = System.Drawing.Color.Transparent;
            this.BtnOk.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnOk.Location = new System.Drawing.Point(95, 0);
            this.BtnOk.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(95, 43);
            this.BtnOk.TabIndex = 36;
            this.BtnOk.Text = "인증서검증";
            this.BtnOk.UseVisualStyleBackColor = false;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.TxtName);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox3.Location = new System.Drawing.Point(350, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(101, 43);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "성명";
            // 
            // TxtName
            // 
            this.TxtName.Location = new System.Drawing.Point(6, 16);
            this.TxtName.Name = "TxtName";
            this.TxtName.Size = new System.Drawing.Size(90, 21);
            this.TxtName.TabIndex = 43;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.DtpEDate);
            this.groupBox2.Controls.Add(this.DtpSDate);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.Location = new System.Drawing.Point(127, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(223, 43);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "조회기간";
            // 
            // DtpEDate
            // 
            this.DtpEDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpEDate.Location = new System.Drawing.Point(111, 16);
            this.DtpEDate.Name = "DtpEDate";
            this.DtpEDate.Size = new System.Drawing.Size(107, 21);
            this.DtpEDate.TabIndex = 3;
            // 
            // DtpSDate
            // 
            this.DtpSDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpSDate.Location = new System.Drawing.Point(4, 16);
            this.DtpSDate.Name = "DtpSDate";
            this.DtpSDate.Size = new System.Drawing.Size(107, 21);
            this.DtpSDate.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RdoGubun_1);
            this.groupBox1.Controls.Add(this.RdoGubun_0);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(124, 43);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "구분";
            // 
            // RdoGubun_1
            // 
            this.RdoGubun_1.AutoSize = true;
            this.RdoGubun_1.Location = new System.Drawing.Point(64, 18);
            this.RdoGubun_1.Name = "RdoGubun_1";
            this.RdoGubun_1.Size = new System.Drawing.Size(59, 16);
            this.RdoGubun_1.TabIndex = 2;
            this.RdoGubun_1.Text = "검증일";
            this.RdoGubun_1.UseVisualStyleBackColor = true;
            // 
            // RdoGubun_0
            // 
            this.RdoGubun_0.AutoSize = true;
            this.RdoGubun_0.Checked = true;
            this.RdoGubun_0.Location = new System.Drawing.Point(5, 18);
            this.RdoGubun_0.Name = "RdoGubun_0";
            this.RdoGubun_0.Size = new System.Drawing.Size(59, 16);
            this.RdoGubun_0.TabIndex = 1;
            this.RdoGubun_0.TabStop = true;
            this.RdoGubun_0.Text = "등록일";
            this.RdoGubun_0.UseVisualStyleBackColor = true;
            // 
            // pan00
            // 
            this.pan00.BackColor = System.Drawing.Color.RoyalBlue;
            this.pan00.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pan00.Controls.Add(this.lbl00);
            this.pan00.Dock = System.Windows.Forms.DockStyle.Top;
            this.pan00.Location = new System.Drawing.Point(0, 49);
            this.pan00.Name = "pan00";
            this.pan00.Size = new System.Drawing.Size(892, 28);
            this.pan00.TabIndex = 39;
            // 
            // lbl00
            // 
            this.lbl00.AutoSize = true;
            this.lbl00.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl00.ForeColor = System.Drawing.Color.White;
            this.lbl00.Location = new System.Drawing.Point(8, 4);
            this.lbl00.Name = "lbl00";
            this.lbl00.Size = new System.Drawing.Size(115, 15);
            this.lbl00.TabIndex = 0;
            this.lbl00.Text = "인증서 등록 및 검증";
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "fpSpread2, Sheet1, Row 0, Column 0, 정신의료사회사업실";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssView.Location = new System.Drawing.Point(0, 77);
            this.ssView.Name = "ssView";
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(892, 574);
            this.ssView.TabIndex = 40;
            this.ssView.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssView_CellDoubleClick);
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 11;
            this.ssView_Sheet1.RowCount = 1;
            this.ssView_Sheet1.Cells.Get(0, 0).Value = "정신의료사회사업실";
            this.ssView_Sheet1.Cells.Get(0, 4).Value = "2020-04-10";
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "부서";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "사번";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "성명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "주민번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "등록일자";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "검증일자";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "검증결과";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "명단빼기";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "ROWID";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "인증패스";
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 39F;
            this.ssView_Sheet1.Columns.Get(0).CellType = textCellType64;
            this.ssView_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssView_Sheet1.Columns.Get(0).Label = "부서";
            this.ssView_Sheet1.Columns.Get(0).Locked = true;
            this.ssView_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(0).Width = 200F;
            this.ssView_Sheet1.Columns.Get(1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(230)))), ((int)(((byte)(212)))));
            this.ssView_Sheet1.Columns.Get(1).CellType = textCellType65;
            this.ssView_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(1).Label = "사번";
            this.ssView_Sheet1.Columns.Get(1).Locked = true;
            this.ssView_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(1).Width = 80F;
            this.ssView_Sheet1.Columns.Get(2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(230)))), ((int)(((byte)(212)))));
            this.ssView_Sheet1.Columns.Get(2).CellType = textCellType66;
            this.ssView_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(2).Label = "성명";
            this.ssView_Sheet1.Columns.Get(2).Locked = true;
            this.ssView_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(2).Width = 80F;
            this.ssView_Sheet1.Columns.Get(3).CellType = textCellType67;
            this.ssView_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(3).Label = "주민번호";
            this.ssView_Sheet1.Columns.Get(3).Locked = true;
            this.ssView_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(3).Width = 120F;
            this.ssView_Sheet1.Columns.Get(4).CellType = textCellType68;
            this.ssView_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(4).Label = "등록일자";
            this.ssView_Sheet1.Columns.Get(4).Locked = true;
            this.ssView_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(4).Width = 90F;
            this.ssView_Sheet1.Columns.Get(5).CellType = textCellType69;
            this.ssView_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(5).Label = "검증일자";
            this.ssView_Sheet1.Columns.Get(5).Locked = true;
            this.ssView_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(5).Width = 90F;
            this.ssView_Sheet1.Columns.Get(6).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
            this.ssView_Sheet1.Columns.Get(6).CellType = textCellType70;
            this.ssView_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(6).Label = "검증결과";
            this.ssView_Sheet1.Columns.Get(6).Locked = true;
            this.ssView_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(6).Width = 80F;
            this.ssView_Sheet1.Columns.Get(7).CellType = checkBoxCellType15;
            this.ssView_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(7).Width = 30F;
            this.ssView_Sheet1.Columns.Get(8).CellType = checkBoxCellType16;
            this.ssView_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(8).Label = "명단빼기";
            this.ssView_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(8).Width = 62F;
            this.ssView_Sheet1.Columns.Get(9).CellType = textCellType71;
            this.ssView_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(9).Label = "ROWID";
            this.ssView_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(9).Visible = false;
            this.ssView_Sheet1.Columns.Get(10).CellType = textCellType72;
            this.ssView_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(10).Label = "인증패스";
            this.ssView_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(10).Visible = false;
            this.ssView_Sheet1.Columns.Get(10).Width = 62F;
            this.ssView_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssView_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(343, 0);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(95, 43);
            this.btnExit.TabIndex = 37;
            this.btnExit.Text = "종료";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrint.Location = new System.Drawing.Point(248, 0);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(95, 43);
            this.btnPrint.TabIndex = 38;
            this.btnPrint.Text = "출력";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // frmCertPoolVerify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(892, 651);
            this.Controls.Add(this.ssView);
            this.Controls.Add(this.pan00);
            this.Controls.Add(this.panel4);
            this.Name = "frmCertPoolVerify";
            this.Text = "frmCertPoolVerify";
            this.Load += new System.EventHandler(this.frmCertPoolVerify_Load);
            this.panel4.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pan00.ResumeLayout(false);
            this.pan00.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Button BtnView;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox TxtName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DateTimePicker DtpEDate;
        private System.Windows.Forms.DateTimePicker DtpSDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton RdoGubun_1;
        private System.Windows.Forms.RadioButton RdoGubun_0;
        private System.Windows.Forms.Panel pan00;
        private System.Windows.Forms.Label lbl00;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnExit;
    }
}