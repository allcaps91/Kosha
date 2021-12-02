namespace ComEmrBase
{
    partial class frmEmrBaseSingularRemark
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEmrBaseSingularRemark));
            this.panTitle = new System.Windows.Forms.Panel();
            this.optSingularType1 = new System.Windows.Forms.RadioButton();
            this.optSingularType0 = new System.Windows.Forms.RadioButton();
            this.optSingularType2 = new System.Windows.Forms.RadioButton();
            this.txtPtNo = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ssSingularView = new FarPoint.Win.Spread.FpSpread();
            this.ssSingularView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.txtSingularRemark = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.optBed = new System.Windows.Forms.RadioButton();
            this.optGood = new System.Windows.Forms.RadioButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtSexAge = new System.Windows.Forms.TextBox();
            this.txtPtClsNm = new System.Windows.Forms.TextBox();
            this.txtPtName = new System.Windows.Forms.TextBox();
            this.dtpInpDate = new System.Windows.Forms.DateTimePicker();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssSingularView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssSingularView_Sheet1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.optSingularType1);
            this.panTitle.Controls.Add(this.optSingularType0);
            this.panTitle.Controls.Add(this.optSingularType2);
            this.panTitle.Controls.Add(this.txtPtNo);
            this.panTitle.Controls.Add(this.btnClear);
            this.panTitle.Controls.Add(this.btnSave);
            this.panTitle.Controls.Add(this.btnDelete);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(966, 33);
            this.panTitle.TabIndex = 13;
            // 
            // optSingularType1
            // 
            this.optSingularType1.AutoSize = true;
            this.optSingularType1.Location = new System.Drawing.Point(303, 5);
            this.optSingularType1.Name = "optSingularType1";
            this.optSingularType1.Size = new System.Drawing.Size(52, 21);
            this.optSingularType1.TabIndex = 35;
            this.optSingularType1.Text = "나쁨";
            this.optSingularType1.UseVisualStyleBackColor = true;
            // 
            // optSingularType0
            // 
            this.optSingularType0.AutoSize = true;
            this.optSingularType0.Location = new System.Drawing.Point(243, 5);
            this.optSingularType0.Name = "optSingularType0";
            this.optSingularType0.Size = new System.Drawing.Size(52, 21);
            this.optSingularType0.TabIndex = 34;
            this.optSingularType0.Text = "좋음";
            this.optSingularType0.UseVisualStyleBackColor = true;
            // 
            // optSingularType2
            // 
            this.optSingularType2.AutoSize = true;
            this.optSingularType2.Checked = true;
            this.optSingularType2.Location = new System.Drawing.Point(183, 5);
            this.optSingularType2.Name = "optSingularType2";
            this.optSingularType2.Size = new System.Drawing.Size(52, 21);
            this.optSingularType2.TabIndex = 14;
            this.optSingularType2.TabStop = true;
            this.optSingularType2.Text = "전체";
            this.optSingularType2.UseVisualStyleBackColor = true;
            // 
            // txtPtNo
            // 
            this.txtPtNo.Location = new System.Drawing.Point(79, 3);
            this.txtPtNo.Name = "txtPtNo";
            this.txtPtNo.Size = new System.Drawing.Size(82, 25);
            this.txtPtNo.TabIndex = 14;
            // 
            // btnClear
            // 
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClear.Location = new System.Drawing.Point(682, 0);
            this.btnClear.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(70, 29);
            this.btnClear.TabIndex = 33;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(752, 0);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(70, 29);
            this.btnSave.TabIndex = 32;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDelete.Location = new System.Drawing.Point(822, 0);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(70, 29);
            this.btnDelete.TabIndex = 31;
            this.btnDelete.Text = "삭제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(15, 7);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(60, 17);
            this.lblTitle.TabIndex = 14;
            this.lblTitle.Text = "등록번호";
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(892, 0);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(70, 29);
            this.btnExit.TabIndex = 30;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ssSingularView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 33);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(436, 548);
            this.panel1.TabIndex = 14;
            // 
            // ssSingularView
            // 
            this.ssSingularView.AccessibleDescription = "ssViewEmrAcpDept, Sheet1, Row 0, Column 0, 9999-99-99";
            this.ssSingularView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssSingularView.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssSingularView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssSingularView.Location = new System.Drawing.Point(0, 0);
            this.ssSingularView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ssSingularView.Name = "ssSingularView";
            this.ssSingularView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssSingularView_Sheet1});
            this.ssSingularView.Size = new System.Drawing.Size(436, 548);
            this.ssSingularView.TabIndex = 50;
            this.ssSingularView.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssSingularView_CellClick);
            // 
            // ssSingularView_Sheet1
            // 
            this.ssSingularView_Sheet1.Reset();
            this.ssSingularView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssSingularView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssSingularView_Sheet1.ColumnCount = 6;
            this.ssSingularView_Sheet1.RowCount = 1;
            this.ssSingularView_Sheet1.Cells.Get(0, 0).Value = "9999-99-99";
            this.ssSingularView_Sheet1.Cells.Get(0, 2).Value = "IM";
            this.ssSingularView_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssSingularView_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssSingularView_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssSingularView_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssSingularView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "내원일자";
            this.ssSingularView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "입력자";
            this.ssSingularView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "상태";
            this.ssSingularView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "내     용";
            this.ssSingularView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "USEID";
            this.ssSingularView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "상태코드";
            this.ssSingularView_Sheet1.ColumnHeader.Rows.Get(0).Height = 28F;
            this.ssSingularView_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssSingularView_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssSingularView_Sheet1.Columns.Get(0).Label = "내원일자";
            this.ssSingularView_Sheet1.Columns.Get(0).Locked = true;
            this.ssSingularView_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSingularView_Sheet1.Columns.Get(0).Width = 77F;
            this.ssSingularView_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssSingularView_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssSingularView_Sheet1.Columns.Get(1).Label = "입력자";
            this.ssSingularView_Sheet1.Columns.Get(1).Locked = true;
            this.ssSingularView_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSingularView_Sheet1.Columns.Get(1).Width = 63F;
            this.ssSingularView_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssSingularView_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSingularView_Sheet1.Columns.Get(2).Label = "상태";
            this.ssSingularView_Sheet1.Columns.Get(2).Locked = true;
            this.ssSingularView_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSingularView_Sheet1.Columns.Get(2).Width = 40F;
            this.ssSingularView_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssSingularView_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssSingularView_Sheet1.Columns.Get(3).Label = "내     용";
            this.ssSingularView_Sheet1.Columns.Get(3).Locked = true;
            this.ssSingularView_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSingularView_Sheet1.Columns.Get(3).Width = 235F;
            this.ssSingularView_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.ssSingularView_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSingularView_Sheet1.Columns.Get(4).Label = "USEID";
            this.ssSingularView_Sheet1.Columns.Get(4).Locked = false;
            this.ssSingularView_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSingularView_Sheet1.Columns.Get(4).Visible = false;
            this.ssSingularView_Sheet1.Columns.Get(4).Width = 103F;
            this.ssSingularView_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.ssSingularView_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSingularView_Sheet1.Columns.Get(5).Label = "상태코드";
            this.ssSingularView_Sheet1.Columns.Get(5).Locked = false;
            this.ssSingularView_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSingularView_Sheet1.Columns.Get(5).Visible = false;
            this.ssSingularView_Sheet1.Columns.Get(5).Width = 119F;
            this.ssSingularView_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssSingularView_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssSingularView_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssSingularView_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssSingularView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssSingularView_Sheet1.RowHeader.Visible = false;
            this.ssSingularView_Sheet1.Rows.Get(0).Height = 24F;
            this.ssSingularView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(436, 33);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(530, 548);
            this.panel2.TabIndex = 15;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.txtSingularRemark);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 78);
            this.panel5.Name = "panel5";
            this.panel5.Padding = new System.Windows.Forms.Padding(4);
            this.panel5.Size = new System.Drawing.Size(530, 470);
            this.panel5.TabIndex = 2;
            // 
            // txtSingularRemark
            // 
            this.txtSingularRemark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSingularRemark.Font = new System.Drawing.Font("굴림체", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtSingularRemark.Location = new System.Drawing.Point(4, 4);
            this.txtSingularRemark.Multiline = true;
            this.txtSingularRemark.Name = "txtSingularRemark";
            this.txtSingularRemark.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSingularRemark.Size = new System.Drawing.Size(522, 462);
            this.txtSingularRemark.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.pictureBox1);
            this.panel4.Controls.Add(this.pictureBox2);
            this.panel4.Controls.Add(this.optBed);
            this.panel4.Controls.Add(this.optGood);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 39);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(530, 39);
            this.panel4.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(42, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(40, 37);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 20;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(167, 1);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(40, 37);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 21;
            this.pictureBox2.TabStop = false;
            // 
            // optBed
            // 
            this.optBed.AutoSize = true;
            this.optBed.Location = new System.Drawing.Point(149, 10);
            this.optBed.Name = "optBed";
            this.optBed.Size = new System.Drawing.Size(97, 21);
            this.optBed.TabIndex = 23;
            this.optBed.TabStop = true;
            this.optBed.Text = "         나쁨";
            this.optBed.UseVisualStyleBackColor = true;
            // 
            // optGood
            // 
            this.optGood.AutoSize = true;
            this.optGood.Location = new System.Drawing.Point(16, 10);
            this.optGood.Name = "optGood";
            this.optGood.Size = new System.Drawing.Size(107, 21);
            this.optGood.TabIndex = 22;
            this.optGood.TabStop = true;
            this.optGood.Text = "           좋음";
            this.optGood.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtSexAge);
            this.panel3.Controls.Add(this.txtPtClsNm);
            this.panel3.Controls.Add(this.txtPtName);
            this.panel3.Controls.Add(this.dtpInpDate);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(530, 39);
            this.panel3.TabIndex = 0;
            // 
            // txtSexAge
            // 
            this.txtSexAge.Location = new System.Drawing.Point(213, 7);
            this.txtSexAge.Name = "txtSexAge";
            this.txtSexAge.Size = new System.Drawing.Size(75, 25);
            this.txtSexAge.TabIndex = 17;
            this.txtSexAge.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtPtClsNm
            // 
            this.txtPtClsNm.Location = new System.Drawing.Point(288, 7);
            this.txtPtClsNm.Name = "txtPtClsNm";
            this.txtPtClsNm.Size = new System.Drawing.Size(59, 25);
            this.txtPtClsNm.TabIndex = 16;
            this.txtPtClsNm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtPtName
            // 
            this.txtPtName.Location = new System.Drawing.Point(138, 7);
            this.txtPtName.Name = "txtPtName";
            this.txtPtName.Size = new System.Drawing.Size(75, 25);
            this.txtPtName.TabIndex = 15;
            this.txtPtName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dtpInpDate
            // 
            this.dtpInpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpInpDate.Location = new System.Drawing.Point(6, 7);
            this.dtpInpDate.Name = "dtpInpDate";
            this.dtpInpDate.Size = new System.Drawing.Size(109, 25);
            this.dtpInpDate.TabIndex = 14;
            // 
            // frmEmrBaseSingularRemark
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(966, 581);
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmEmrBaseSingularRemark";
            this.Text = "개인별 특이사항";
            this.Load += new System.EventHandler(this.frmEmrBaseSingularRemark_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssSingularView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssSingularView_Sheet1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.RadioButton optSingularType1;
        private System.Windows.Forms.RadioButton optSingularType0;
        private System.Windows.Forms.RadioButton optSingularType2;
        private System.Windows.Forms.TextBox txtPtNo;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private FarPoint.Win.Spread.FpSpread ssSingularView;
        private FarPoint.Win.Spread.SheetView ssSingularView_Sheet1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.RadioButton optBed;
        private System.Windows.Forms.RadioButton optGood;
        private System.Windows.Forms.TextBox txtSexAge;
        private System.Windows.Forms.TextBox txtPtClsNm;
        private System.Windows.Forms.TextBox txtPtName;
        private System.Windows.Forms.DateTimePicker dtpInpDate;
        private System.Windows.Forms.TextBox txtSingularRemark;
    }
}