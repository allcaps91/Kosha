namespace ComLibB
{
    partial class frmSanidView
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
            this.pan0 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpTdate = new System.Windows.Forms.DateTimePicker();
            this.dtpFdate = new System.Windows.Forms.DateTimePicker();
            this.btnPrint = new System.Windows.Forms.Button();
            this.rdoConiosis = new System.Windows.Forms.RadioButton();
            this.rdoSequela = new System.Windows.Forms.RadioButton();
            this.rdoTreat = new System.Windows.Forms.RadioButton();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblRadio = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.pan0.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            this.SuspendLayout();
            // 
            // pan0
            // 
            this.pan0.Controls.Add(this.ssView);
            this.pan0.Controls.Add(this.panel3);
            this.pan0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pan0.Location = new System.Drawing.Point(0, 0);
            this.pan0.Name = "pan0";
            this.pan0.Size = new System.Drawing.Size(1076, 543);
            this.pan0.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.lblTitle);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.dtpTdate);
            this.panel3.Controls.Add(this.dtpFdate);
            this.panel3.Controls.Add(this.btnPrint);
            this.panel3.Controls.Add(this.rdoConiosis);
            this.panel3.Controls.Add(this.rdoSequela);
            this.panel3.Controls.Add(this.rdoTreat);
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Controls.Add(this.lblRadio);
            this.panel3.Controls.Add(this.lblDate);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1076, 38);
            this.panel3.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(440, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 12);
            this.label1.TabIndex = 41;
            this.label1.Text = "~";
            // 
            // dtpTdate
            // 
            this.dtpTdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTdate.Location = new System.Drawing.Point(457, 7);
            this.dtpTdate.Name = "dtpTdate";
            this.dtpTdate.Size = new System.Drawing.Size(98, 21);
            this.dtpTdate.TabIndex = 40;
            // 
            // dtpFdate
            // 
            this.dtpFdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFdate.Location = new System.Drawing.Point(339, 7);
            this.dtpFdate.Name = "dtpFdate";
            this.dtpFdate.Size = new System.Drawing.Size(98, 21);
            this.dtpFdate.TabIndex = 40;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.Location = new System.Drawing.Point(919, 2);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 30);
            this.btnPrint.TabIndex = 23;
            this.btnPrint.Text = "출력";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // rdoConiosis
            // 
            this.rdoConiosis.AutoSize = true;
            this.rdoConiosis.Location = new System.Drawing.Point(745, 9);
            this.rdoConiosis.Name = "rdoConiosis";
            this.rdoConiosis.Size = new System.Drawing.Size(47, 16);
            this.rdoConiosis.TabIndex = 39;
            this.rdoConiosis.TabStop = true;
            this.rdoConiosis.Text = "진폐";
            this.rdoConiosis.UseVisualStyleBackColor = true;
            // 
            // rdoSequela
            // 
            this.rdoSequela.AutoSize = true;
            this.rdoSequela.Location = new System.Drawing.Point(692, 9);
            this.rdoSequela.Name = "rdoSequela";
            this.rdoSequela.Size = new System.Drawing.Size(47, 16);
            this.rdoSequela.TabIndex = 38;
            this.rdoSequela.TabStop = true;
            this.rdoSequela.Text = "후유";
            this.rdoSequela.UseVisualStyleBackColor = true;
            // 
            // rdoTreat
            // 
            this.rdoTreat.AutoSize = true;
            this.rdoTreat.Checked = true;
            this.rdoTreat.Location = new System.Drawing.Point(639, 9);
            this.rdoTreat.Name = "rdoTreat";
            this.rdoTreat.Size = new System.Drawing.Size(47, 16);
            this.rdoTreat.TabIndex = 37;
            this.rdoTreat.TabStop = true;
            this.rdoTreat.Text = "특진";
            this.rdoTreat.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Location = new System.Drawing.Point(841, 2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 27;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblRadio
            // 
            this.lblRadio.AutoSize = true;
            this.lblRadio.Location = new System.Drawing.Point(577, 11);
            this.lblRadio.Name = "lblRadio";
            this.lblRadio.Size = new System.Drawing.Size(59, 12);
            this.lblRadio.TabIndex = 25;
            this.lblRadio.Text = "특진/후유";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(280, 11);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(53, 12);
            this.lblDate.TabIndex = 24;
            this.lblDate.Text = "진료일자";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(3, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(211, 16);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "후유, 특진환자 조회 -외래";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(997, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 12;
            this.ssView_Sheet1.RowCount = 1;
            this.ssView_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssView_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "자격";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "특진/후유";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "진료일";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "진료과";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "등록번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "성명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "진료과1";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "진료과2";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "진료과3";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "재해발생일";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "진료개시일";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "치료결과";
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssView_Sheet1.Columns.Get(0).Label = "자격";
            this.ssView_Sheet1.Columns.Get(0).Locked = false;
            this.ssView_Sheet1.Columns.Get(0).Width = 80F;
            this.ssView_Sheet1.Columns.Get(1).Label = "특진/후유";
            this.ssView_Sheet1.Columns.Get(1).Locked = false;
            this.ssView_Sheet1.Columns.Get(1).Width = 80F;
            this.ssView_Sheet1.Columns.Get(2).Label = "진료일";
            this.ssView_Sheet1.Columns.Get(2).Locked = false;
            this.ssView_Sheet1.Columns.Get(2).Width = 120F;
            this.ssView_Sheet1.Columns.Get(3).Label = "진료과";
            this.ssView_Sheet1.Columns.Get(3).Locked = false;
            this.ssView_Sheet1.Columns.Get(3).Width = 80F;
            this.ssView_Sheet1.Columns.Get(4).Label = "등록번호";
            this.ssView_Sheet1.Columns.Get(4).Locked = false;
            this.ssView_Sheet1.Columns.Get(4).Width = 80F;
            this.ssView_Sheet1.Columns.Get(5).Label = "성명";
            this.ssView_Sheet1.Columns.Get(5).Locked = false;
            this.ssView_Sheet1.Columns.Get(5).Width = 80F;
            this.ssView_Sheet1.Columns.Get(6).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ssView_Sheet1.Columns.Get(6).Label = "진료과1";
            this.ssView_Sheet1.Columns.Get(6).Locked = false;
            this.ssView_Sheet1.Columns.Get(6).Width = 80F;
            this.ssView_Sheet1.Columns.Get(7).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ssView_Sheet1.Columns.Get(7).Label = "진료과2";
            this.ssView_Sheet1.Columns.Get(7).Locked = false;
            this.ssView_Sheet1.Columns.Get(7).Width = 80F;
            this.ssView_Sheet1.Columns.Get(8).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ssView_Sheet1.Columns.Get(8).Label = "진료과3";
            this.ssView_Sheet1.Columns.Get(8).Locked = false;
            this.ssView_Sheet1.Columns.Get(8).Width = 80F;
            this.ssView_Sheet1.Columns.Get(9).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ssView_Sheet1.Columns.Get(9).Label = "재해발생일";
            this.ssView_Sheet1.Columns.Get(9).Locked = false;
            this.ssView_Sheet1.Columns.Get(9).Width = 90F;
            this.ssView_Sheet1.Columns.Get(10).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ssView_Sheet1.Columns.Get(10).Label = "진료개시일";
            this.ssView_Sheet1.Columns.Get(10).Locked = false;
            this.ssView_Sheet1.Columns.Get(10).Width = 90F;
            this.ssView_Sheet1.Columns.Get(11).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ssView_Sheet1.Columns.Get(11).Label = "치료결과";
            this.ssView_Sheet1.Columns.Get(11).Locked = false;
            this.ssView_Sheet1.Columns.Get(11).Width = 80F;
            this.ssView_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.ssView_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.ssView_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssView_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssView_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssView_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "fpSpread1, Sheet1, Row 0, Column 0, ";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssView.Location = new System.Drawing.Point(0, 38);
            this.ssView.Name = "ssView";
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(1076, 505);
            this.ssView.TabIndex = 47;
            // 
            // frmSanidView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1076, 543);
            this.Controls.Add(this.pan0);
            this.Name = "frmSanidView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "후유, 특진환자 조회 -외래";
            this.Load += new System.EventHandler(this.frmSanidView_Load);
            this.pan0.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pan0;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpTdate;
        private System.Windows.Forms.DateTimePicker dtpFdate;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.RadioButton rdoConiosis;
        private System.Windows.Forms.RadioButton rdoSequela;
        private System.Windows.Forms.RadioButton rdoTreat;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblRadio;
        private System.Windows.Forms.Label lblDate;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
    }
}