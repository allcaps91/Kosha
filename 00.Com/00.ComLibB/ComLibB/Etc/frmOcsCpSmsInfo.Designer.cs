namespace ComLibB
{
    partial class frmOcsCpSmsInfo
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color341636707454079265005", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Static415636707454079284975", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static549636707454079304889");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Static812636707454079384675");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblPatInfo = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cboCpCode = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCallact = new System.Windows.Forms.TextBox();
            this.txtCalldeacti = new System.Windows.Forms.TextBox();
            this.txtCallacti = new System.Windows.Forms.TextBox();
            this.txtCallwarm = new System.Windows.Forms.TextBox();
            this.panel8 = new System.Windows.Forms.Panel();
            this.ssSMS = new FarPoint.Win.Spread.FpSpread();
            this.ssSMS_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panTitle.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssSMS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssSMS_Sheet1)).BeginInit();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Controls.Add(this.btnSearch);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(495, 34);
            this.panTitle.TabIndex = 14;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(406, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(80, 30);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "닫  기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(106, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "CP 처방 관리";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(326, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 30);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "조  회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(495, 28);
            this.panTitleSub0.TabIndex = 15;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 4);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(55, 15);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "환자정보";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblPatInfo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(2);
            this.panel1.Size = new System.Drawing.Size(495, 34);
            this.panel1.TabIndex = 16;
            // 
            // lblPatInfo
            // 
            this.lblPatInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblPatInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPatInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPatInfo.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lblPatInfo.Location = new System.Drawing.Point(2, 2);
            this.lblPatInfo.Name = "lblPatInfo";
            this.lblPatInfo.Size = new System.Drawing.Size(491, 30);
            this.lblPatInfo.TabIndex = 0;
            this.lblPatInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 96);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(495, 28);
            this.panel3.TabIndex = 22;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(8, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "CP SMS Call 정보";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.cboCpCode);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 124);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(495, 33);
            this.panel4.TabIndex = 23;
            // 
            // cboCpCode
            // 
            this.cboCpCode.FormattingEnabled = true;
            this.cboCpCode.Location = new System.Drawing.Point(69, 4);
            this.cboCpCode.Name = "cboCpCode";
            this.cboCpCode.Size = new System.Drawing.Size(414, 25);
            this.cboCpCode.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 17);
            this.label1.TabIndex = 17;
            this.label1.Text = "CP 명칭";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.txtCallact);
            this.panel2.Controls.Add(this.txtCalldeacti);
            this.panel2.Controls.Add(this.txtCallacti);
            this.panel2.Controls.Add(this.txtCallwarm);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 157);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(495, 135);
            this.panel2.TabIndex = 24;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 101);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 17);
            this.label7.TabIndex = 37;
            this.label7.Text = "시술 Call";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(125, 17);
            this.label6.TabIndex = 36;
            this.label6.Text = "CP deactivation Call";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(110, 17);
            this.label5.TabIndex = 35;
            this.label5.Text = "CP activation Call";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 17);
            this.label4.TabIndex = 1;
            this.label4.Text = "예비 CP Call";
            // 
            // txtCallact
            // 
            this.txtCallact.Location = new System.Drawing.Point(172, 103);
            this.txtCallact.Name = "txtCallact";
            this.txtCallact.ReadOnly = true;
            this.txtCallact.Size = new System.Drawing.Size(310, 25);
            this.txtCallact.TabIndex = 34;
            // 
            // txtCalldeacti
            // 
            this.txtCalldeacti.Location = new System.Drawing.Point(172, 71);
            this.txtCalldeacti.Name = "txtCalldeacti";
            this.txtCalldeacti.ReadOnly = true;
            this.txtCalldeacti.Size = new System.Drawing.Size(310, 25);
            this.txtCalldeacti.TabIndex = 33;
            // 
            // txtCallacti
            // 
            this.txtCallacti.Location = new System.Drawing.Point(172, 39);
            this.txtCallacti.Name = "txtCallacti";
            this.txtCallacti.ReadOnly = true;
            this.txtCallacti.Size = new System.Drawing.Size(310, 25);
            this.txtCallacti.TabIndex = 32;
            // 
            // txtCallwarm
            // 
            this.txtCallwarm.Location = new System.Drawing.Point(172, 7);
            this.txtCallwarm.Name = "txtCallwarm";
            this.txtCallwarm.ReadOnly = true;
            this.txtCallwarm.Size = new System.Drawing.Size(310, 25);
            this.txtCallwarm.TabIndex = 31;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.ssSMS);
            this.panel8.Controls.Add(this.panel5);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(0, 292);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(495, 304);
            this.panel8.TabIndex = 25;
            // 
            // ssSMS
            // 
            this.ssSMS.AccessibleDescription = "ssSMS, Sheet1, Row 0, Column 0, ";
            this.ssSMS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssSMS.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssSMS.Location = new System.Drawing.Point(0, 28);
            this.ssSMS.Name = "ssSMS";
            namedStyle1.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType1.Static = true;
            textCellType1.WordWrap = true;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType2.Static = true;
            textCellType2.WordWrap = true;
            namedStyle3.CellType = textCellType2;
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.Renderer = textCellType2;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType3.Static = true;
            textCellType3.WordWrap = true;
            namedStyle4.CellType = textCellType3;
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.Renderer = textCellType3;
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSMS.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4});
            this.ssSMS.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssSMS_Sheet1});
            this.ssSMS.Size = new System.Drawing.Size(495, 276);
            this.ssSMS.TabIndex = 24;
            this.ssSMS.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssSMS.TextTipAppearance = tipAppearance1;
            // 
            // ssSMS_Sheet1
            // 
            this.ssSMS_Sheet1.Reset();
            this.ssSMS_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssSMS_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssSMS_Sheet1.ColumnCount = 8;
            this.ssSMS_Sheet1.RowCount = 1;
            this.ssSMS_Sheet1.Cells.Get(0, 5).StyleName = "Static549636707454079304889";
            this.ssSMS_Sheet1.Cells.Get(0, 5).Value = "2018-07-17 11:12";
            this.ssSMS_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssSMS_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssSMS_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssSMS_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssSMS_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssSMS_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "휴대폰번호";
            this.ssSMS_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "의사명";
            this.ssSMS_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "과";
            this.ssSMS_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "등록번호";
            this.ssSMS_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "구분";
            this.ssSMS_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "CALL TIME";
            this.ssSMS_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "전송일자";
            this.ssSMS_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "문자내용";
            this.ssSMS_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssSMS_Sheet1.ColumnHeader.Rows.Get(0).Height = 28F;
            this.ssSMS_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssSMS_Sheet1.Columns.Get(0).Label = "휴대폰번호";
            this.ssSMS_Sheet1.Columns.Get(0).StyleName = "Static549636707454079304889";
            this.ssSMS_Sheet1.Columns.Get(0).Width = 100F;
            this.ssSMS_Sheet1.Columns.Get(1).Label = "의사명";
            this.ssSMS_Sheet1.Columns.Get(1).StyleName = "Static549636707454079304889";
            this.ssSMS_Sheet1.Columns.Get(1).Width = 59F;
            this.ssSMS_Sheet1.Columns.Get(2).Label = "과";
            this.ssSMS_Sheet1.Columns.Get(2).StyleName = "Static549636707454079304889";
            this.ssSMS_Sheet1.Columns.Get(2).Width = 30F;
            this.ssSMS_Sheet1.Columns.Get(3).Label = "등록번호";
            this.ssSMS_Sheet1.Columns.Get(3).StyleName = "Static549636707454079304889";
            this.ssSMS_Sheet1.Columns.Get(3).Width = 79F;
            this.ssSMS_Sheet1.Columns.Get(4).Label = "구분";
            this.ssSMS_Sheet1.Columns.Get(4).StyleName = "Static549636707454079304889";
            this.ssSMS_Sheet1.Columns.Get(4).Width = 68F;
            this.ssSMS_Sheet1.Columns.Get(5).Label = "CALL TIME";
            this.ssSMS_Sheet1.Columns.Get(5).StyleName = "Static549636707454079304889";
            this.ssSMS_Sheet1.Columns.Get(5).Width = 106F;
            this.ssSMS_Sheet1.Columns.Get(6).Label = "전송일자";
            this.ssSMS_Sheet1.Columns.Get(6).StyleName = "Static549636707454079304889";
            this.ssSMS_Sheet1.Columns.Get(6).Visible = false;
            this.ssSMS_Sheet1.Columns.Get(6).Width = 102F;
            this.ssSMS_Sheet1.Columns.Get(7).Label = "문자내용";
            this.ssSMS_Sheet1.Columns.Get(7).StyleName = "Static812636707454079384675";
            this.ssSMS_Sheet1.Columns.Get(7).Visible = false;
            this.ssSMS_Sheet1.Columns.Get(7).Width = 264F;
            this.ssSMS_Sheet1.DefaultStyleName = "Static415636707454079284975";
            this.ssSMS_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssSMS_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssSMS_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssSMS_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssSMS_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssSMS_Sheet1.RowHeader.Columns.Get(0).Width = 32F;
            this.ssSMS_Sheet1.Rows.Default.Height = 18F;
            this.ssSMS_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel5.Controls.Add(this.label2);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(495, 28);
            this.panel5.TabIndex = 23;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(8, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "CP SMS Call List";
            // 
            // frmOcsCpSmsInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(495, 596);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmOcsCpSmsInfo";
            this.Text = "frmOcsCpSmsInfo";
            this.Load += new System.EventHandler(this.frmOcsCpSmsInfo_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssSMS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssSMS_Sheet1)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblPatInfo;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ComboBox cboCpCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panel8;
        private FarPoint.Win.Spread.FpSpread ssSMS;
        private FarPoint.Win.Spread.SheetView ssSMS_Sheet1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCallact;
        private System.Windows.Forms.TextBox txtCalldeacti;
        private System.Windows.Forms.TextBox txtCallacti;
        private System.Windows.Forms.TextBox txtCallwarm;
    }
}