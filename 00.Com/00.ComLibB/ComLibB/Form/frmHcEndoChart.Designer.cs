namespace ComLibB
{
    partial class frmHcEndoChart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHcEndoChart));
            this.button1 = new System.Windows.Forms.Button();
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pan8 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.DtpSDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SS1 = new FarPoint.Win.Spread.FpSpread();
            this.SS1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            this.pan8.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(545, 167);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(138, 37);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(664, 42);
            this.panTitle.TabIndex = 45;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(344, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "건진센터 내시경실용 챠트 작성 점검 프로그램";
            // 
            // pan8
            // 
            this.pan8.BackColor = System.Drawing.Color.Olive;
            this.pan8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pan8.Controls.Add(this.lblTitleSub0);
            this.pan8.Dock = System.Windows.Forms.DockStyle.Top;
            this.pan8.Location = new System.Drawing.Point(0, 42);
            this.pan8.Name = "pan8";
            this.pan8.Size = new System.Drawing.Size(664, 28);
            this.pan8.TabIndex = 46;
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
            this.panel1.Controls.Add(this.DtpSDate);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 70);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(664, 32);
            this.panel1.TabIndex = 47;
            // 
            // DtpSDate
            // 
            this.DtpSDate.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.DtpSDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpSDate.Location = new System.Drawing.Point(78, 3);
            this.DtpSDate.Name = "DtpSDate";
            this.DtpSDate.Size = new System.Drawing.Size(89, 25);
            this.DtpSDate.TabIndex = 45;
            this.DtpSDate.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.label2.Location = new System.Drawing.Point(12, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 17);
            this.label2.TabIndex = 44;
            this.label2.Text = "기준일자";
            this.label2.Visible = false;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(589, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 40;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Location = new System.Drawing.Point(518, 1);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 28;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Tan;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel5.Controls.Add(this.label6);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 102);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(664, 28);
            this.panel5.TabIndex = 48;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.ForeColor = System.Drawing.Color.Cornsilk;
            this.label6.Location = new System.Drawing.Point(8, 5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(322, 15);
            this.label6.TabIndex = 1;
            this.label6.Text = "해당쉘을 더블클릭하면 신규 챠트를 작성하실 수 있습니다.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(8, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 15);
            this.label4.TabIndex = 0;
            // 
            // SS1
            // 
            this.SS1.AccessibleDescription = "ssList, Sheet1, Row 0, Column 0, ";
            this.SS1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SS1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.SS1.Location = new System.Drawing.Point(0, 130);
            this.SS1.Name = "SS1";
            this.SS1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS1_Sheet1});
            this.SS1.Size = new System.Drawing.Size(664, 588);
            this.SS1.TabIndex = 50;
            this.SS1.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SS1_CellDoubleClick);
            // 
            // SS1_Sheet1
            // 
            this.SS1_Sheet1.Reset();
            this.SS1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS1_Sheet1.ColumnCount = 8;
            this.SS1_Sheet1.RowCount = 20;
            this.SS1_Sheet1.Cells.Get(0, 0).ParseFormatInfo = ((System.Globalization.DateTimeFormatInfo)(cultureInfo.DateTimeFormat.Clone()));
            this.SS1_Sheet1.Cells.Get(0, 0).ParseFormatString = "yyyy-MM-dd";
            this.SS1_Sheet1.Cells.Get(0, 0).Value = new System.DateTime(2021, 1, 1, 0, 0, 0, 0);
            this.SS1_Sheet1.Cells.Get(0, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Cells.Get(0, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(0, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(0, 2).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.SS1_Sheet1.Cells.Get(0, 2).ParseFormatString = "n";
            this.SS1_Sheet1.Cells.Get(0, 2).Value = 81000004;
            this.SS1_Sheet1.Cells.Get(0, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "진료일자";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "진료과";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "등록번호";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "환자명";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "성별/나이";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "진정평가";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "검사전 기록지";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "퇴실기록지";
            this.SS1_Sheet1.ColumnHeader.Rows.Get(0).Height = 35F;
            this.SS1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(0).Label = "진료일자";
            this.SS1_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(0).Width = 74F;
            this.SS1_Sheet1.Columns.Get(1).Label = "진료과";
            this.SS1_Sheet1.Columns.Get(1).Width = 52F;
            this.SS1_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(2).Label = "등록번호";
            this.SS1_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(2).Width = 74F;
            this.SS1_Sheet1.Columns.Get(5).Label = "진정평가";
            this.SS1_Sheet1.Columns.Get(5).Width = 87F;
            this.SS1_Sheet1.Columns.Get(6).Label = "검사전 기록지";
            this.SS1_Sheet1.Columns.Get(6).Width = 87F;
            this.SS1_Sheet1.Columns.Get(7).Label = "퇴실기록지";
            this.SS1_Sheet1.Columns.Get(7).Width = 87F;
            this.SS1_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SS1_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS1_Sheet1.Rows.Get(0).Height = 24F;
            this.SS1_Sheet1.Rows.Get(1).Height = 24F;
            this.SS1_Sheet1.Rows.Get(2).Height = 24F;
            this.SS1_Sheet1.Rows.Get(3).Height = 24F;
            this.SS1_Sheet1.Rows.Get(4).Height = 24F;
            this.SS1_Sheet1.Rows.Get(5).Height = 24F;
            this.SS1_Sheet1.Rows.Get(6).Height = 24F;
            this.SS1_Sheet1.Rows.Get(7).Height = 24F;
            this.SS1_Sheet1.Rows.Get(8).Height = 24F;
            this.SS1_Sheet1.Rows.Get(9).Height = 24F;
            this.SS1_Sheet1.Rows.Get(10).Height = 24F;
            this.SS1_Sheet1.Rows.Get(11).Height = 24F;
            this.SS1_Sheet1.Rows.Get(12).Height = 24F;
            this.SS1_Sheet1.Rows.Get(13).Height = 24F;
            this.SS1_Sheet1.Rows.Get(14).Height = 24F;
            this.SS1_Sheet1.Rows.Get(15).Height = 24F;
            this.SS1_Sheet1.Rows.Get(16).Height = 24F;
            this.SS1_Sheet1.Rows.Get(17).Height = 24F;
            this.SS1_Sheet1.Rows.Get(18).Height = 24F;
            this.SS1_Sheet1.Rows.Get(19).Height = 24F;
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmHcEndoChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(664, 718);
            this.Controls.Add(this.SS1);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pan8);
            this.Controls.Add(this.panTitle);
            this.Controls.Add(this.button1);
            this.Name = "frmHcEndoChart";
            this.Text = "frmHcEndoChart";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.pan8.ResumeLayout(false);
            this.pan8.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pan8;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker DtpSDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private FarPoint.Win.Spread.FpSpread SS1;
        private FarPoint.Win.Spread.SheetView SS1_Sheet1;
    }
}