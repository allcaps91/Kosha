namespace ComLibB
{
    partial class frmMSelf
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
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType1 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.DateTimeCellType dateTimeCellType1 = new FarPoint.Win.Spread.CellType.DateTimeCellType();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMSelf));
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.pan0 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.grb = new System.Windows.Forms.GroupBox();
            this.btnImBu = new System.Windows.Forms.Button();
            this.btnLow = new System.Windows.Forms.Button();
            this.btnMax = new System.Windows.Forms.Button();
            this.btnStability = new System.Windows.Forms.Button();
            this.btnAge = new System.Windows.Forms.Button();
            this.btnBY = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.lblItem0 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cboJob = new System.Windows.Forms.ComboBox();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pan0.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.panel3.SuspendLayout();
            this.grb.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // pan0
            // 
            this.pan0.Controls.Add(this.panel2);
            this.pan0.Controls.Add(this.panTitle);
            this.pan0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pan0.Location = new System.Drawing.Point(0, 0);
            this.pan0.Name = "pan0";
            this.pan0.Size = new System.Drawing.Size(680, 636);
            this.pan0.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.ssView);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 34);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(680, 602);
            this.panel2.TabIndex = 13;
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "ssView, Sheet1, Row 0, Column 0, ";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssView.Location = new System.Drawing.Point(0, 109);
            this.ssView.Name = "ssView";
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(676, 489);
            this.ssView.TabIndex = 46;
            this.ssView.Change += new FarPoint.Win.Spread.ChangeEventHandler(this.ssView_Change);
            this.ssView.LeaveCell += new FarPoint.Win.Spread.LeaveCellEventHandler(this.ssView_LeaveCell);
            this.ssView.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssView_CellDoubleClick);
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 10;
            this.ssView_Sheet1.RowCount = 1;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = " ";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "수가코드";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "나이";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = " ";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "등록일자";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "수가명칭";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "ROWID";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "변경";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "제외";
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssView_Sheet1.Columns.Get(0).CellType = checkBoxCellType1;
            this.ssView_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(0).Label = " ";
            this.ssView_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(0).Width = 20F;
            this.ssView_Sheet1.Columns.Get(1).AllowAutoSort = true;
            this.ssView_Sheet1.Columns.Get(1).CellType = textCellType1;
            this.ssView_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssView_Sheet1.Columns.Get(1).Label = "수가코드";
            this.ssView_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(1).Width = 80F;
            this.ssView_Sheet1.Columns.Get(2).AllowAutoSort = true;
            this.ssView_Sheet1.Columns.Get(2).CellType = textCellType2;
            this.ssView_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssView_Sheet1.Columns.Get(2).Label = "나이";
            this.ssView_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(2).Width = 70F;
            this.ssView_Sheet1.Columns.Get(3).AllowAutoSort = true;
            this.ssView_Sheet1.Columns.Get(3).CellType = textCellType3;
            this.ssView_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssView_Sheet1.Columns.Get(3).Label = " ";
            this.ssView_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(3).Width = 70F;
            this.ssView_Sheet1.Columns.Get(4).AllowAutoSort = true;
            dateTimeCellType1.Calendar = new System.Globalization.GregorianCalendar(System.Globalization.GregorianCalendarTypes.Localized);
            dateTimeCellType1.CalendarSurroundingDaysColor = System.Drawing.SystemColors.GrayText;
            dateTimeCellType1.MaximumTime = System.TimeSpan.Parse("23:59:59.9999999");
            dateTimeCellType1.TimeDefault = new System.DateTime(2020, 3, 31, 14, 34, 39, 0);
            this.ssView_Sheet1.Columns.Get(4).CellType = dateTimeCellType1;
            this.ssView_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(4).Label = "등록일자";
            this.ssView_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(4).VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.Columns.Get(4).Width = 100F;
            this.ssView_Sheet1.Columns.Get(5).AllowAutoSort = true;
            this.ssView_Sheet1.Columns.Get(5).CellType = textCellType4;
            this.ssView_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssView_Sheet1.Columns.Get(5).Label = "수가명칭";
            this.ssView_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(5).Width = 280F;
            this.ssView_Sheet1.Columns.Get(6).CellType = textCellType5;
            this.ssView_Sheet1.Columns.Get(6).Label = "ROWID";
            this.ssView_Sheet1.Columns.Get(6).Visible = false;
            this.ssView_Sheet1.Columns.Get(6).Width = 70F;
            this.ssView_Sheet1.Columns.Get(7).CellType = textCellType6;
            this.ssView_Sheet1.Columns.Get(7).Label = "변경";
            this.ssView_Sheet1.Columns.Get(7).Visible = false;
            this.ssView_Sheet1.Columns.Get(7).Width = 70F;
            this.ssView_Sheet1.Columns.Get(8).CellType = textCellType7;
            this.ssView_Sheet1.Columns.Get(8).Label = "제외";
            this.ssView_Sheet1.Columns.Get(8).Visible = false;
            this.ssView_Sheet1.Columns.Get(9).CellType = textCellType8;
            this.ssView_Sheet1.Columns.Get(9).Visible = false;
            this.ssView_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssView_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.button1);
            this.panel3.Controls.Add(this.grb);
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Controls.Add(this.btnPrint);
            this.panel3.Controls.Add(this.lblItem0);
            this.panel3.Controls.Add(this.btnCancel);
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Controls.Add(this.cboJob);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(676, 109);
            this.panel3.TabIndex = 17;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.Location = new System.Drawing.Point(504, 65);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 30);
            this.button1.TabIndex = 27;
            this.button1.Text = "외래 주상병 금지";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // grb
            // 
            this.grb.Controls.Add(this.btnImBu);
            this.grb.Controls.Add(this.btnLow);
            this.grb.Controls.Add(this.btnMax);
            this.grb.Controls.Add(this.btnStability);
            this.grb.Controls.Add(this.btnAge);
            this.grb.Controls.Add(this.btnBY);
            this.grb.Location = new System.Drawing.Point(65, 46);
            this.grb.Name = "grb";
            this.grb.Size = new System.Drawing.Size(431, 55);
            this.grb.TabIndex = 38;
            this.grb.TabStop = false;
            this.grb.Text = "심평원 자료조회";
            // 
            // btnImBu
            // 
            this.btnImBu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImBu.BackColor = System.Drawing.Color.Transparent;
            this.btnImBu.Location = new System.Drawing.Point(356, 19);
            this.btnImBu.Name = "btnImBu";
            this.btnImBu.Size = new System.Drawing.Size(71, 30);
            this.btnImBu.TabIndex = 27;
            this.btnImBu.Text = "임부금기";
            this.btnImBu.UseVisualStyleBackColor = false;
            this.btnImBu.Click += new System.EventHandler(this.btnImBu_Click);
            // 
            // btnLow
            // 
            this.btnLow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLow.BackColor = System.Drawing.Color.Transparent;
            this.btnLow.Location = new System.Drawing.Point(291, 19);
            this.btnLow.Name = "btnLow";
            this.btnLow.Size = new System.Drawing.Size(65, 30);
            this.btnLow.TabIndex = 27;
            this.btnLow.Text = "저함량";
            this.btnLow.UseVisualStyleBackColor = false;
            this.btnLow.Click += new System.EventHandler(this.btnLow_Click);
            // 
            // btnMax
            // 
            this.btnMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMax.BackColor = System.Drawing.Color.Transparent;
            this.btnMax.Location = new System.Drawing.Point(216, 19);
            this.btnMax.Name = "btnMax";
            this.btnMax.Size = new System.Drawing.Size(75, 30);
            this.btnMax.TabIndex = 27;
            this.btnMax.Text = "최대용량";
            this.btnMax.UseVisualStyleBackColor = false;
            this.btnMax.Click += new System.EventHandler(this.btnMax_Click);
            // 
            // btnStability
            // 
            this.btnStability.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStability.BackColor = System.Drawing.Color.Transparent;
            this.btnStability.Location = new System.Drawing.Point(151, 19);
            this.btnStability.Name = "btnStability";
            this.btnStability.Size = new System.Drawing.Size(65, 30);
            this.btnStability.TabIndex = 27;
            this.btnStability.Text = "안정성";
            this.btnStability.UseVisualStyleBackColor = false;
            this.btnStability.Click += new System.EventHandler(this.btnStability_Click);
            // 
            // btnAge
            // 
            this.btnAge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAge.BackColor = System.Drawing.Color.Transparent;
            this.btnAge.Location = new System.Drawing.Point(77, 19);
            this.btnAge.Name = "btnAge";
            this.btnAge.Size = new System.Drawing.Size(74, 30);
            this.btnAge.TabIndex = 27;
            this.btnAge.Text = "연령금기";
            this.btnAge.UseVisualStyleBackColor = false;
            this.btnAge.Click += new System.EventHandler(this.btnAge_Click);
            // 
            // btnBY
            // 
            this.btnBY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBY.BackColor = System.Drawing.Color.Transparent;
            this.btnBY.Location = new System.Drawing.Point(4, 19);
            this.btnBY.Name = "btnBY";
            this.btnBY.Size = new System.Drawing.Size(73, 30);
            this.btnBY.TabIndex = 27;
            this.btnBY.Text = "병용금기";
            this.btnBY.UseVisualStyleBackColor = false;
            this.btnBY.Click += new System.EventHandler(this.btnBY_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Location = new System.Drawing.Point(316, 13);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 27;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.Location = new System.Drawing.Point(550, 13);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 30);
            this.btnPrint.TabIndex = 22;
            this.btnPrint.Text = "출력";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // lblItem0
            // 
            this.lblItem0.AutoSize = true;
            this.lblItem0.Location = new System.Drawing.Point(62, 20);
            this.lblItem0.Name = "lblItem0";
            this.lblItem0.Size = new System.Drawing.Size(34, 17);
            this.lblItem0.TabIndex = 24;
            this.lblItem0.Text = "종류";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Location = new System.Drawing.Point(472, 13);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 30);
            this.btnCancel.TabIndex = 23;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.Location = new System.Drawing.Point(394, 13);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 30);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cboJob
            // 
            this.cboJob.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboJob.FormattingEnabled = true;
            this.cboJob.Location = new System.Drawing.Point(97, 15);
            this.cboJob.Name = "cboJob";
            this.cboJob.Size = new System.Drawing.Size(202, 25);
            this.cboJob.TabIndex = 36;
            this.cboJob.SelectedIndexChanged += new System.EventHandler(this.cboJob_SelectedIndexChanged);
            this.cboJob.Click += new System.EventHandler(this.cboJob_Click);
            this.cboJob.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboJob_KeyDown);
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
            this.panTitle.Size = new System.Drawing.Size(680, 34);
            this.panTitle.TabIndex = 11;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(601, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(156, 16);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "외래 제한사항 등록";
            // 
            // frmMSelf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(680, 636);
            this.Controls.Add(this.pan0);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "frmMSelf";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "외래 제한사항 등록";
            this.Load += new System.EventHandler(this.frmMSelf_Load);
            this.pan0.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.grb.ResumeLayout(false);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pan0;
        private System.Windows.Forms.Panel panel2;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
        private System.Windows.Forms.ComboBox cboJob;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label lblItem0;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox grb;
        private System.Windows.Forms.Button btnImBu;
        private System.Windows.Forms.Button btnLow;
        private System.Windows.Forms.Button btnMax;
        private System.Windows.Forms.Button btnStability;
        private System.Windows.Forms.Button btnAge;
        private System.Windows.Forms.Button btnBY;
    }
}