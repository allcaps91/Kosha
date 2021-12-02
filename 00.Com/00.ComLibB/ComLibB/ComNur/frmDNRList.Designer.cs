namespace ComLibB
{
    partial class frmDNRList
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
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("ko-KR", false);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDNRList));
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtEDate = new System.Windows.Forms.DateTimePicker();
            this.btnExit = new System.Windows.Forms.Button();
            this.TxtSDate = new System.Windows.Forms.DateTimePicker();
            this.btnView = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SS1 = new FarPoint.Win.Spread.FpSpread();
            this.SS1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(687, 34);
            this.panTitle.TabIndex = 23;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(149, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "[DNR] 대상자 명단";
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(687, 28);
            this.panTitleSub0.TabIndex = 24;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 4);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(59, 15);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "조회 조건";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtEDate);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.TxtSDate);
            this.panel1.Controls.Add(this.btnView);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(687, 31);
            this.panel1.TabIndex = 25;
            // 
            // txtEDate
            // 
            this.txtEDate.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtEDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtEDate.Location = new System.Drawing.Point(182, 3);
            this.txtEDate.Name = "txtEDate";
            this.txtEDate.Size = new System.Drawing.Size(97, 25);
            this.txtEDate.TabIndex = 33;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(612, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // TxtSDate
            // 
            this.TxtSDate.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TxtSDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.TxtSDate.Location = new System.Drawing.Point(79, 3);
            this.TxtSDate.Name = "TxtSDate";
            this.TxtSDate.Size = new System.Drawing.Size(97, 25);
            this.TxtSDate.TabIndex = 30;
            // 
            // btnView
            // 
            this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnView.BackColor = System.Drawing.Color.Transparent;
            this.btnView.Location = new System.Drawing.Point(541, 1);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(72, 30);
            this.btnView.TabIndex = 28;
            this.btnView.Text = "조회";
            this.btnView.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.label1.Location = new System.Drawing.Point(12, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "작업일자";
            // 
            // SS1
            // 
            this.SS1.AccessibleDescription = "SS1, Sheet1, Row 0, Column 0, 502";
            this.SS1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SS1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.SS1.Location = new System.Drawing.Point(0, 93);
            this.SS1.Name = "SS1";
            this.SS1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS1_Sheet1});
            this.SS1.Size = new System.Drawing.Size(687, 542);
            this.SS1.TabIndex = 26;
            // 
            // SS1_Sheet1
            // 
            this.SS1_Sheet1.Reset();
            this.SS1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS1_Sheet1.ColumnCount = 11;
            this.SS1_Sheet1.RowCount = 30;
            this.SS1_Sheet1.Cells.Get(0, 0).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(0, 0).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(0, 0).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.SS1_Sheet1.Cells.Get(0, 0).ParseFormatString = "n";
            this.SS1_Sheet1.Cells.Get(0, 0).Value = 502;
            this.SS1_Sheet1.Cells.Get(0, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(0, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(0, 1).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.SS1_Sheet1.Cells.Get(0, 1).ParseFormatString = "n";
            this.SS1_Sheet1.Cells.Get(0, 1).Value = 81000004;
            this.SS1_Sheet1.Cells.Get(0, 2).ParseFormatInfo = ((System.Globalization.DateTimeFormatInfo)(cultureInfo.DateTimeFormat.Clone()));
            this.SS1_Sheet1.Cells.Get(0, 2).ParseFormatString = "yyyy-MM-dd";
            this.SS1_Sheet1.Cells.Get(0, 2).Value = new System.DateTime(2019, 1, 1, 0, 0, 0, 0);
            this.SS1_Sheet1.Cells.Get(0, 3).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(0, 3).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(0, 3).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.SS1_Sheet1.Cells.Get(0, 3).ParseFormatString = "n";
            this.SS1_Sheet1.Cells.Get(0, 3).Value = "성명성명";
            this.SS1_Sheet1.Cells.Get(0, 4).Value = "F";
            this.SS1_Sheet1.Cells.Get(0, 5).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(0, 5).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(0, 5).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.SS1_Sheet1.Cells.Get(0, 5).ParseFormatString = "n";
            this.SS1_Sheet1.Cells.Get(0, 5).Value = 20;
            this.SS1_Sheet1.Cells.Get(0, 6).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(0, 6).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(0, 6).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.SS1_Sheet1.Cells.Get(0, 6).ParseFormatString = "n";
            this.SS1_Sheet1.Cells.Get(0, 6).Value = 20;
            this.SS1_Sheet1.Cells.Get(0, 7).Value = "응급실";
            this.SS1_Sheet1.Cells.Get(0, 8).ParseFormatInfo = ((System.Globalization.DateTimeFormatInfo)(cultureInfo.DateTimeFormat.Clone()));
            this.SS1_Sheet1.Cells.Get(0, 8).ParseFormatString = "yyyy-MM-dd";
            this.SS1_Sheet1.Cells.Get(0, 8).Value = new System.DateTime(2019, 1, 1, 0, 0, 0, 0);
            this.SS1_Sheet1.Cells.Get(0, 9).Value = "2019-01-01 44:33";
            this.SS1_Sheet1.Cells.Get(0, 10).Value = "2019-01-01(홍길동)";
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "호실";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "등록번호";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "입원일자";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "성명";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "성별";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "나이";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "진료과";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "진료의";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "입원일자";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "병동도착일시";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "[DNR]등록일";
            this.SS1_Sheet1.ColumnHeader.Rows.Get(0).Height = 35F;
            this.SS1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(0).Label = "호실";
            this.SS1_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(0).Width = 53F;
            this.SS1_Sheet1.Columns.Get(1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
            this.SS1_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(1).Label = "등록번호";
            this.SS1_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(1).Width = 72F;
            this.SS1_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(2).Label = "입원일자";
            this.SS1_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(2).Visible = false;
            this.SS1_Sheet1.Columns.Get(2).Width = 83F;
            this.SS1_Sheet1.Columns.Get(3).CellType = textCellType3;
            this.SS1_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(3).Label = "성명";
            this.SS1_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(3).Width = 70F;
            this.SS1_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(4).Label = "성별";
            this.SS1_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(4).Width = 41F;
            this.SS1_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(5).Label = "나이";
            this.SS1_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(5).Width = 41F;
            this.SS1_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(6).Label = "진료과";
            this.SS1_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(6).Width = 58F;
            this.SS1_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(7).Label = "진료의";
            this.SS1_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(7).Width = 66F;
            this.SS1_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(8).Label = "입원일자";
            this.SS1_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(8).Width = 82F;
            this.SS1_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(9).Label = "병동도착일시";
            this.SS1_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(9).Visible = false;
            this.SS1_Sheet1.Columns.Get(9).Width = 112F;
            this.SS1_Sheet1.Columns.Get(10).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
            textCellType4.WordWrap = true;
            this.SS1_Sheet1.Columns.Get(10).CellType = textCellType4;
            this.SS1_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(10).Label = "[DNR]등록일";
            this.SS1_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(10).Width = 140F;
            this.SS1_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(164)))), ((int)(((byte)(164))))));
            this.SS1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS1_Sheet1.Rows.Get(0).Height = 29F;
            this.SS1_Sheet1.Rows.Get(1).Height = 29F;
            this.SS1_Sheet1.Rows.Get(2).Height = 29F;
            this.SS1_Sheet1.Rows.Get(3).Height = 29F;
            this.SS1_Sheet1.Rows.Get(4).Height = 29F;
            this.SS1_Sheet1.Rows.Get(5).Height = 29F;
            this.SS1_Sheet1.Rows.Get(6).Height = 29F;
            this.SS1_Sheet1.Rows.Get(7).Height = 29F;
            this.SS1_Sheet1.Rows.Get(8).Height = 29F;
            this.SS1_Sheet1.Rows.Get(9).Height = 29F;
            this.SS1_Sheet1.Rows.Get(10).Height = 29F;
            this.SS1_Sheet1.Rows.Get(11).Height = 29F;
            this.SS1_Sheet1.Rows.Get(12).Height = 29F;
            this.SS1_Sheet1.Rows.Get(13).Height = 29F;
            this.SS1_Sheet1.Rows.Get(14).Height = 29F;
            this.SS1_Sheet1.Rows.Get(15).Height = 29F;
            this.SS1_Sheet1.Rows.Get(16).Height = 29F;
            this.SS1_Sheet1.Rows.Get(17).Height = 29F;
            this.SS1_Sheet1.Rows.Get(18).Height = 29F;
            this.SS1_Sheet1.Rows.Get(19).Height = 29F;
            this.SS1_Sheet1.Rows.Get(20).Height = 29F;
            this.SS1_Sheet1.Rows.Get(21).Height = 29F;
            this.SS1_Sheet1.Rows.Get(22).Height = 29F;
            this.SS1_Sheet1.Rows.Get(23).Height = 29F;
            this.SS1_Sheet1.Rows.Get(24).Height = 29F;
            this.SS1_Sheet1.Rows.Get(25).Height = 29F;
            this.SS1_Sheet1.Rows.Get(26).Height = 29F;
            this.SS1_Sheet1.Rows.Get(27).Height = 29F;
            this.SS1_Sheet1.Rows.Get(28).Height = 29F;
            this.SS1_Sheet1.Rows.Get(29).Height = 29F;
            this.SS1_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(164)))), ((int)(((byte)(164))))));
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmDNRList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(687, 635);
            this.Controls.Add(this.SS1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Name = "frmDNRList";
            this.Text = "frmDNRList";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker txtEDate;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.DateTimePicker TxtSDate;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Label label1;
        private FarPoint.Win.Spread.FpSpread SS1;
        private FarPoint.Win.Spread.SheetView SS1_Sheet1;
    }
}