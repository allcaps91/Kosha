namespace ComEmrBase
{
    partial class frmActingRecord
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType23 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType24 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType25 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType26 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType27 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType28 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType29 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType30 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType31 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType32 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType33 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cboWard = new System.Windows.Forms.ComboBox();
            this.dtpBDate = new System.Windows.Forms.DateTimePicker();
            this.chkBun0 = new System.Windows.Forms.CheckBox();
            this.chkBun1 = new System.Windows.Forms.CheckBox();
            this.cboJob = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.ssList = new FarPoint.Win.Spread.FpSpread();
            this.ssList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpDate1 = new System.Windows.Forms.DateTimePicker();
            this.dtpDate2 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.chkDate = new System.Windows.Forms.CheckBox();
            this.btnActing = new System.Windows.Forms.Button();
            this.ssActing = new FarPoint.Win.Spread.FpSpread();
            this.ssActing_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.lblName = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssActing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssActing_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ssList);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(327, 664);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblName);
            this.panel2.Controls.Add(this.btnActing);
            this.panel2.Controls.Add(this.chkDate);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.dtpDate2);
            this.panel2.Controls.Add(this.dtpDate1);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(327, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(708, 58);
            this.panel2.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboWard);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(83, 71);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "병동";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dtpBDate);
            this.groupBox2.Location = new System.Drawing.Point(90, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(105, 71);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "처방일자";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkBun1);
            this.groupBox3.Controls.Add(this.chkBun0);
            this.groupBox3.Location = new System.Drawing.Point(199, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(96, 71);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "구분";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cboJob);
            this.groupBox4.Location = new System.Drawing.Point(3, 80);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(192, 53);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "조회방법";
            // 
            // cboWard
            // 
            this.cboWard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboWard.FormattingEnabled = true;
            this.cboWard.Location = new System.Drawing.Point(3, 21);
            this.cboWard.Name = "cboWard";
            this.cboWard.Size = new System.Drawing.Size(77, 25);
            this.cboWard.TabIndex = 0;
            // 
            // dtpBDate
            // 
            this.dtpBDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpBDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpBDate.Location = new System.Drawing.Point(3, 21);
            this.dtpBDate.Name = "dtpBDate";
            this.dtpBDate.Size = new System.Drawing.Size(99, 25);
            this.dtpBDate.TabIndex = 0;
            // 
            // chkBun0
            // 
            this.chkBun0.AutoSize = true;
            this.chkBun0.Checked = true;
            this.chkBun0.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBun0.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkBun0.Location = new System.Drawing.Point(3, 21);
            this.chkBun0.Name = "chkBun0";
            this.chkBun0.Size = new System.Drawing.Size(90, 23);
            this.chkBun0.TabIndex = 0;
            this.chkBun0.Text = "내복,외용";
            this.chkBun0.UseVisualStyleBackColor = true;
            // 
            // chkBun1
            // 
            this.chkBun1.AutoSize = true;
            this.chkBun1.Checked = true;
            this.chkBun1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBun1.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkBun1.Location = new System.Drawing.Point(3, 44);
            this.chkBun1.Name = "chkBun1";
            this.chkBun1.Size = new System.Drawing.Size(90, 23);
            this.chkBun1.TabIndex = 1;
            this.chkBun1.Text = "주사";
            this.chkBun1.UseVisualStyleBackColor = true;
            // 
            // cboJob
            // 
            this.cboJob.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboJob.FormattingEnabled = true;
            this.cboJob.Location = new System.Drawing.Point(3, 21);
            this.cboJob.Name = "cboJob";
            this.cboJob.Size = new System.Drawing.Size(186, 25);
            this.cboJob.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(202, 91);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(79, 35);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "목록조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // ssList
            // 
            this.ssList.AccessibleDescription = "ssList, Sheet1, Row 0, Column 0, ";
            this.ssList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssList.Location = new System.Drawing.Point(3, 139);
            this.ssList.Name = "ssList";
            this.ssList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList_Sheet1});
            this.ssList.Size = new System.Drawing.Size(318, 522);
            this.ssList.TabIndex = 5;
            this.ssList.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // ssList_Sheet1
            // 
            this.ssList_Sheet1.Reset();
            this.ssList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssList_Sheet1.ColumnCount = 5;
            this.ssList_Sheet1.RowCount = 1;
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "호실";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "등록번호";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "성명";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "성별\r\n나이";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "의사명";
            this.ssList_Sheet1.ColumnHeader.Rows.Get(0).Height = 41F;
            this.ssList_Sheet1.Columns.Get(0).CellType = textCellType23;
            this.ssList_Sheet1.Columns.Get(0).Label = "호실";
            this.ssList_Sheet1.Columns.Get(0).Locked = true;
            this.ssList_Sheet1.Columns.Get(0).Width = 42F;
            this.ssList_Sheet1.Columns.Get(1).CellType = textCellType24;
            this.ssList_Sheet1.Columns.Get(1).Label = "등록번호";
            this.ssList_Sheet1.Columns.Get(1).Locked = true;
            this.ssList_Sheet1.Columns.Get(1).Width = 70F;
            this.ssList_Sheet1.Columns.Get(2).CellType = textCellType25;
            this.ssList_Sheet1.Columns.Get(2).Label = "성명";
            this.ssList_Sheet1.Columns.Get(2).Locked = true;
            this.ssList_Sheet1.Columns.Get(2).Width = 62F;
            this.ssList_Sheet1.Columns.Get(3).CellType = textCellType26;
            this.ssList_Sheet1.Columns.Get(3).Label = "성별\r\n나이";
            this.ssList_Sheet1.Columns.Get(3).Locked = true;
            this.ssList_Sheet1.Columns.Get(3).Width = 40F;
            this.ssList_Sheet1.Columns.Get(4).CellType = textCellType27;
            this.ssList_Sheet1.Columns.Get(4).Label = "의사명";
            this.ssList_Sheet1.Columns.Get(4).Locked = true;
            this.ssList_Sheet1.Columns.Get(4).Width = 61F;
            this.ssList_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            this.ssList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList_Sheet1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssList_Sheet1.SelectionStyle = FarPoint.Win.Spread.SelectionStyles.SelectionColors;
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "액팅일자:";
            // 
            // dtpDate1
            // 
            this.dtpDate1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate1.Location = new System.Drawing.Point(71, 16);
            this.dtpDate1.Name = "dtpDate1";
            this.dtpDate1.Size = new System.Drawing.Size(99, 25);
            this.dtpDate1.TabIndex = 1;
            // 
            // dtpDate2
            // 
            this.dtpDate2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate2.Location = new System.Drawing.Point(189, 16);
            this.dtpDate2.Name = "dtpDate2";
            this.dtpDate2.Size = new System.Drawing.Size(99, 25);
            this.dtpDate2.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(170, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "~";
            // 
            // chkDate
            // 
            this.chkDate.AutoSize = true;
            this.chkDate.ForeColor = System.Drawing.Color.Blue;
            this.chkDate.Location = new System.Drawing.Point(294, 17);
            this.chkDate.Name = "chkDate";
            this.chkDate.Size = new System.Drawing.Size(112, 23);
            this.chkDate.TabIndex = 4;
            this.chkDate.Text = "임의시간설정";
            this.chkDate.UseVisualStyleBackColor = true;
            this.chkDate.CheckedChanged += new System.EventHandler(this.chkDate_CheckedChanged);
            // 
            // btnActing
            // 
            this.btnActing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnActing.Location = new System.Drawing.Point(617, 12);
            this.btnActing.Name = "btnActing";
            this.btnActing.Size = new System.Drawing.Size(79, 35);
            this.btnActing.TabIndex = 5;
            this.btnActing.Text = "조회";
            this.btnActing.UseVisualStyleBackColor = true;
            this.btnActing.Click += new System.EventHandler(this.btnActing_Click);
            // 
            // ssActing
            // 
            this.ssActing.AccessibleDescription = "";
            this.ssActing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssActing.Enabled = false;
            this.ssActing.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssActing.Location = new System.Drawing.Point(327, 58);
            this.ssActing.Name = "ssActing";
            this.ssActing.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssActing_Sheet1});
            this.ssActing.Size = new System.Drawing.Size(708, 606);
            this.ssActing.TabIndex = 2;
            this.ssActing.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // ssActing_Sheet1
            // 
            this.ssActing_Sheet1.Reset();
            this.ssActing_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssActing_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssActing_Sheet1.ColumnCount = 6;
            this.ssActing_Sheet1.RowCount = 1;
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "액팅일자";
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "처방코드";
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "Order Name";
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "용법 및 검체";
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "횟수";
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "Acting";
            this.ssActing_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssActing_Sheet1.Columns.Get(0).CellType = textCellType28;
            this.ssActing_Sheet1.Columns.Get(0).Label = "액팅일자";
            this.ssActing_Sheet1.Columns.Get(0).Locked = true;
            this.ssActing_Sheet1.Columns.Get(0).Width = 67F;
            this.ssActing_Sheet1.Columns.Get(1).CellType = textCellType29;
            this.ssActing_Sheet1.Columns.Get(1).Label = "처방코드";
            this.ssActing_Sheet1.Columns.Get(1).Locked = true;
            this.ssActing_Sheet1.Columns.Get(1).Width = 67F;
            this.ssActing_Sheet1.Columns.Get(2).CellType = textCellType30;
            this.ssActing_Sheet1.Columns.Get(2).Label = "Order Name";
            this.ssActing_Sheet1.Columns.Get(2).Locked = true;
            this.ssActing_Sheet1.Columns.Get(2).Width = 89F;
            this.ssActing_Sheet1.Columns.Get(3).CellType = textCellType31;
            this.ssActing_Sheet1.Columns.Get(3).Label = "용법 및 검체";
            this.ssActing_Sheet1.Columns.Get(3).Locked = true;
            this.ssActing_Sheet1.Columns.Get(3).Width = 91F;
            this.ssActing_Sheet1.Columns.Get(4).CellType = textCellType32;
            this.ssActing_Sheet1.Columns.Get(4).Label = "횟수";
            this.ssActing_Sheet1.Columns.Get(4).Locked = true;
            this.ssActing_Sheet1.Columns.Get(4).Width = 39F;
            this.ssActing_Sheet1.Columns.Get(5).CellType = textCellType33;
            this.ssActing_Sheet1.Columns.Get(5).Label = "Acting";
            this.ssActing_Sheet1.Columns.Get(5).Locked = true;
            this.ssActing_Sheet1.Columns.Get(5).Width = 315F;
            this.ssActing_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssActing_Sheet1.Rows.Get(0).Height = 22F;
            this.ssActing_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(403, 19);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(60, 19);
            this.lblName.TabIndex = 6;
            this.lblName.Text = "lblName";
            // 
            // frmActingRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1035, 664);
            this.Controls.Add(this.ssActing);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmActingRecord";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "투약기록지";
            this.Load += new System.EventHandler(this.frmActingRecord_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssActing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssActing_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private FarPoint.Win.Spread.FpSpread ssList;
        private FarPoint.Win.Spread.SheetView ssList_Sheet1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox cboJob;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkBun1;
        private System.Windows.Forms.CheckBox chkBun0;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DateTimePicker dtpBDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cboWard;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnActing;
        private System.Windows.Forms.CheckBox chkDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpDate2;
        private System.Windows.Forms.DateTimePicker dtpDate1;
        private System.Windows.Forms.Label label1;
        private FarPoint.Win.Spread.FpSpread ssActing;
        private FarPoint.Win.Spread.SheetView ssActing_Sheet1;
        private System.Windows.Forms.Label lblName;
    }
}