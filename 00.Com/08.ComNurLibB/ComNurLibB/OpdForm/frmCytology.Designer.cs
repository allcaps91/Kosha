namespace ComNurLibB
{
    partial class frmCytology
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color341636524858353403523", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Static405636524858356386446", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static563636524858356421541");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Static617636524858356441597");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panDisp = new System.Windows.Forms.Panel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.txtPtno = new System.Windows.Forms.TextBox();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.panMain = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtRemark3 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtRemark2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtRemark4 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRemark1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.lblInfo = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panTitleSub0.SuspendLayout();
            this.panDisp.SuspendLayout();
            this.panMain.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.panel3.SuspendLayout();
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
            this.panTitleSub0.Size = new System.Drawing.Size(869, 28);
            this.panTitleSub0.TabIndex = 15;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(5, 1);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(195, 19);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "Cytology 검사 의뢰지 재출력";
            // 
            // panDisp
            // 
            this.panDisp.Controls.Add(this.panel3);
            this.panDisp.Controls.Add(this.btnPrint);
            this.panDisp.Controls.Add(this.txtPtno);
            this.panDisp.Controls.Add(this.dtpDate);
            this.panDisp.Controls.Add(this.label1);
            this.panDisp.Controls.Add(this.btnSave);
            this.panDisp.Controls.Add(this.label5);
            this.panDisp.Dock = System.Windows.Forms.DockStyle.Top;
            this.panDisp.Location = new System.Drawing.Point(0, 28);
            this.panDisp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panDisp.Name = "panDisp";
            this.panDisp.Size = new System.Drawing.Size(869, 38);
            this.panDisp.TabIndex = 23;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPrint.Location = new System.Drawing.Point(767, 3);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 30);
            this.btnPrint.TabIndex = 12;
            this.btnPrint.Text = "출력";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Visible = false;
            // 
            // txtPtno
            // 
            this.txtPtno.Location = new System.Drawing.Point(294, 8);
            this.txtPtno.Name = "txtPtno";
            this.txtPtno.Size = new System.Drawing.Size(123, 21);
            this.txtPtno.TabIndex = 11;
            // 
            // dtpDate
            // 
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(88, 8);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(107, 21);
            this.dtpDate.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(22, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 17);
            this.label1.TabIndex = 9;
            this.label1.Text = "의뢰일자";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave.Location = new System.Drawing.Point(693, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 30);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(227, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 17);
            this.label5.TabIndex = 4;
            this.label5.Text = "병록번호";
            // 
            // panMain
            // 
            this.panMain.Controls.Add(this.panel2);
            this.panMain.Controls.Add(this.panel1);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(0, 66);
            this.panMain.Name = "panMain";
            this.panMain.Size = new System.Drawing.Size(869, 427);
            this.panMain.TabIndex = 24;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblInfo);
            this.panel2.Controls.Add(this.txtRemark3);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.txtRemark2);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.txtRemark4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.txtRemark1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(544, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(325, 427);
            this.panel2.TabIndex = 1;
            // 
            // txtRemark3
            // 
            this.txtRemark3.Location = new System.Drawing.Point(9, 346);
            this.txtRemark3.Multiline = true;
            this.txtRemark3.Name = "txtRemark3";
            this.txtRemark3.Size = new System.Drawing.Size(307, 70);
            this.txtRemark3.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 328);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(270, 12);
            this.label6.TabIndex = 7;
            this.label6.Text = "Information on Previous  Cytology Examination";
            // 
            // txtRemark2
            // 
            this.txtRemark2.Location = new System.Drawing.Point(9, 249);
            this.txtRemark2.Multiline = true;
            this.txtRemark2.Name = "txtRemark2";
            this.txtRemark2.Size = new System.Drawing.Size(307, 70);
            this.txtRemark2.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 231);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(159, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "Clinical history & Information";
            // 
            // txtRemark4
            // 
            this.txtRemark4.Location = new System.Drawing.Point(9, 149);
            this.txtRemark4.Multiline = true;
            this.txtRemark4.Name = "txtRemark4";
            this.txtRemark4.Size = new System.Drawing.Size(307, 70);
            this.txtRemark4.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "Clinical Dianosis";
            // 
            // txtRemark1
            // 
            this.txtRemark1.Location = new System.Drawing.Point(9, 54);
            this.txtRemark1.Multiline = true;
            this.txtRemark1.Name = "txtRemark1";
            this.txtRemark1.Size = new System.Drawing.Size(307, 70);
            this.txtRemark1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "Nature && Source of Specimen";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ssView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(544, 427);
            this.panel1.TabIndex = 0;
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "ssView, Sheet1, Row 0, Column 0, ";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.Location = new System.Drawing.Point(0, 0);
            this.ssView.Name = "ssView";
            namedStyle1.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType1.Static = true;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType2.Static = true;
            namedStyle3.CellType = textCellType2;
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.Renderer = textCellType2;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType3.Static = true;
            namedStyle4.CellType = textCellType3;
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.Renderer = textCellType3;
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4});
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(544, 427);
            this.ssView.TabIndex = 0;
            this.ssView.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssView.TextTipAppearance = tipAppearance1;
            this.ssView.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssView_CellClick);
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 9;
            this.ssView_Sheet1.RowCount = 1;
            this.ssView_Sheet1.ColumnFooter.Columns.Default.Resizable = false;
            this.ssView_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "등록번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "성   명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "과";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "의사";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "검사명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "검체번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "병리번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "RowID";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "오더No.";
            this.ssView_Sheet1.ColumnHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 22F;
            this.ssView_Sheet1.Columns.Default.Resizable = false;
            this.ssView_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.Columns.Get(2).Label = "과";
            this.ssView_Sheet1.Columns.Get(2).Width = 26F;
            this.ssView_Sheet1.Columns.Get(3).Label = "의사";
            this.ssView_Sheet1.Columns.Get(3).Width = 47F;
            this.ssView_Sheet1.Columns.Get(4).Label = "검사명";
            this.ssView_Sheet1.Columns.Get(4).StyleName = "Static563636524858356421541";
            this.ssView_Sheet1.Columns.Get(4).Width = 142F;
            this.ssView_Sheet1.Columns.Get(5).Label = "검체번호";
            this.ssView_Sheet1.Columns.Get(5).Width = 80F;
            this.ssView_Sheet1.Columns.Get(6).Label = "병리번호";
            this.ssView_Sheet1.Columns.Get(6).Width = 74F;
            this.ssView_Sheet1.Columns.Get(7).Label = "RowID";
            this.ssView_Sheet1.Columns.Get(7).Visible = false;
            this.ssView_Sheet1.Columns.Get(8).Label = "오더No.";
            this.ssView_Sheet1.Columns.Get(8).StyleName = "Static617636524858356441597";
            this.ssView_Sheet1.DefaultStyleName = "Static405636524858356386446";
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.RowHeader.Columns.Get(0).Width = 30F;
            this.ssView_Sheet1.RowHeader.Rows.Default.Resizable = false;
            this.ssView_Sheet1.Rows.Default.Resizable = false;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblInfo.Location = new System.Drawing.Point(9, 12);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(0, 12);
            this.lblInfo.TabIndex = 9;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Location = new System.Drawing.Point(693, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(164, 33);
            this.panel3.TabIndex = 13;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(6, 1);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(78, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 10;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // frmCytology
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(869, 493);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.panDisp);
            this.Controls.Add(this.panTitleSub0);
            this.Name = "frmCytology";
            this.Text = "frmCytology";
            this.Load += new System.EventHandler(this.frmCytology_Load);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panDisp.ResumeLayout(false);
            this.panDisp.PerformLayout();
            this.panMain.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panDisp;
        private System.Windows.Forms.TextBox txtPtno;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panMain;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtRemark3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRemark2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtRemark4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRemark1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearch;
    }
}