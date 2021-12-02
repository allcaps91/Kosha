namespace ComNurLibB
{
    partial class frmCertList
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panDisp = new System.Windows.Forms.Panel();
            this.dtpTDate = new System.Windows.Forms.DateTimePicker();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSNAME = new System.Windows.Forms.TextBox();
            this.txtPANO = new System.Windows.Forms.TextBox();
            this.dtpFDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ssCerti = new FarPoint.Win.Spread.FpSpread();
            this.ssCerti_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitleSub0.SuspendLayout();
            this.panDisp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssCerti)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssCerti_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 0);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(698, 28);
            this.panTitleSub0.TabIndex = 16;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(5, 1);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(150, 19);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "증명서 발급 내역 조회";
            // 
            // panDisp
            // 
            this.panDisp.Controls.Add(this.dtpTDate);
            this.panDisp.Controls.Add(this.btnExit);
            this.panDisp.Controls.Add(this.btnSearch);
            this.panDisp.Controls.Add(this.txtSNAME);
            this.panDisp.Controls.Add(this.txtPANO);
            this.panDisp.Controls.Add(this.dtpFDate);
            this.panDisp.Controls.Add(this.label1);
            this.panDisp.Controls.Add(this.label5);
            this.panDisp.Dock = System.Windows.Forms.DockStyle.Top;
            this.panDisp.Location = new System.Drawing.Point(0, 28);
            this.panDisp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panDisp.Name = "panDisp";
            this.panDisp.Size = new System.Drawing.Size(698, 38);
            this.panDisp.TabIndex = 24;
            // 
            // dtpTDate
            // 
            this.dtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTDate.Location = new System.Drawing.Point(173, 8);
            this.dtpTDate.Name = "dtpTDate";
            this.dtpTDate.Size = new System.Drawing.Size(98, 21);
            this.dtpTDate.TabIndex = 16;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(620, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(548, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 14;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSNAME
            // 
            this.txtSNAME.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.txtSNAME.Location = new System.Drawing.Point(444, 8);
            this.txtSNAME.Name = "txtSNAME";
            this.txtSNAME.Size = new System.Drawing.Size(94, 21);
            this.txtSNAME.TabIndex = 11;
            // 
            // txtPANO
            // 
            this.txtPANO.Location = new System.Drawing.Point(348, 8);
            this.txtPANO.Name = "txtPANO";
            this.txtPANO.Size = new System.Drawing.Size(94, 21);
            this.txtPANO.TabIndex = 11;
            this.txtPANO.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPANO_KeyDown);
            // 
            // dtpFDate
            // 
            this.dtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFDate.Location = new System.Drawing.Point(73, 8);
            this.dtpFDate.Name = "dtpFDate";
            this.dtpFDate.Size = new System.Drawing.Size(98, 21);
            this.dtpFDate.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 17);
            this.label1.TabIndex = 9;
            this.label1.Text = "조회기간";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(286, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 17);
            this.label5.TabIndex = 4;
            this.label5.Text = "병록번호";
            // 
            // ssCerti
            // 
            this.ssCerti.AccessibleDescription = "ssCerti, Sheet1, Row 0, Column 0, ";
            this.ssCerti.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssCerti.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssCerti.Location = new System.Drawing.Point(0, 66);
            this.ssCerti.Name = "ssCerti";
            this.ssCerti.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssCerti_Sheet1});
            this.ssCerti.Size = new System.Drawing.Size(698, 591);
            this.ssCerti.TabIndex = 25;
            // 
            // ssCerti_Sheet1
            // 
            this.ssCerti_Sheet1.Reset();
            this.ssCerti_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssCerti_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssCerti_Sheet1.ColumnCount = 12;
            this.ssCerti_Sheet1.RowCount = 1;
            this.ssCerti_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "등록번호";
            this.ssCerti_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "성명";
            this.ssCerti_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "나이";
            this.ssCerti_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "과";
            this.ssCerti_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "의사";
            this.ssCerti_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "내원(입원)일";
            this.ssCerti_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "증명서구분";
            this.ssCerti_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "발급일자";
            this.ssCerti_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "발급구분";
            this.ssCerti_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "구분코드";
            this.ssCerti_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "발급번호";
            this.ssCerti_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "의사명";
            this.ssCerti_Sheet1.ColumnHeader.Rows.Get(0).Height = 39F;
            this.ssCerti_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssCerti_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssCerti_Sheet1.Columns.Get(0).Label = "등록번호";
            this.ssCerti_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCerti_Sheet1.Columns.Get(0).Width = 80F;
            this.ssCerti_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssCerti_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssCerti_Sheet1.Columns.Get(1).Label = "성명";
            this.ssCerti_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCerti_Sheet1.Columns.Get(1).Width = 80F;
            this.ssCerti_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssCerti_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssCerti_Sheet1.Columns.Get(2).Label = "나이";
            this.ssCerti_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCerti_Sheet1.Columns.Get(2).Width = 40F;
            this.ssCerti_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssCerti_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssCerti_Sheet1.Columns.Get(3).Label = "과";
            this.ssCerti_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCerti_Sheet1.Columns.Get(3).Width = 40F;
            this.ssCerti_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.ssCerti_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssCerti_Sheet1.Columns.Get(4).Label = "의사";
            this.ssCerti_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCerti_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.ssCerti_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssCerti_Sheet1.Columns.Get(5).Label = "내원(입원)일";
            this.ssCerti_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCerti_Sheet1.Columns.Get(5).Width = 80F;
            this.ssCerti_Sheet1.Columns.Get(6).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(230)))), ((int)(((byte)(212)))));
            this.ssCerti_Sheet1.Columns.Get(6).CellType = textCellType7;
            this.ssCerti_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssCerti_Sheet1.Columns.Get(6).Label = "증명서구분";
            this.ssCerti_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCerti_Sheet1.Columns.Get(6).Width = 120F;
            this.ssCerti_Sheet1.Columns.Get(7).CellType = textCellType8;
            this.ssCerti_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssCerti_Sheet1.Columns.Get(7).Label = "발급일자";
            this.ssCerti_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCerti_Sheet1.Columns.Get(7).Width = 80F;
            this.ssCerti_Sheet1.Columns.Get(8).CellType = textCellType9;
            this.ssCerti_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssCerti_Sheet1.Columns.Get(8).Label = "발급구분";
            this.ssCerti_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCerti_Sheet1.Columns.Get(9).CellType = textCellType10;
            this.ssCerti_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssCerti_Sheet1.Columns.Get(9).Label = "구분코드";
            this.ssCerti_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCerti_Sheet1.Columns.Get(10).CellType = textCellType11;
            this.ssCerti_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssCerti_Sheet1.Columns.Get(10).Label = "발급번호";
            this.ssCerti_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCerti_Sheet1.Columns.Get(11).CellType = textCellType12;
            this.ssCerti_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssCerti_Sheet1.Columns.Get(11).Label = "의사명";
            this.ssCerti_Sheet1.Columns.Get(11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCerti_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssCerti_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmCertList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(698, 657);
            this.Controls.Add(this.ssCerti);
            this.Controls.Add(this.panDisp);
            this.Controls.Add(this.panTitleSub0);
            this.Name = "frmCertList";
            this.Text = "frmCertList";
            this.Load += new System.EventHandler(this.frmCertList_Load);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panDisp.ResumeLayout(false);
            this.panDisp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssCerti)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssCerti_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panDisp;
        private System.Windows.Forms.DateTimePicker dtpTDate;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSNAME;
        private System.Windows.Forms.TextBox txtPANO;
        private System.Windows.Forms.DateTimePicker dtpFDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private FarPoint.Win.Spread.FpSpread ssCerti;
        private FarPoint.Win.Spread.SheetView ssCerti_Sheet1;
    }
}