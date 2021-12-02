namespace ComLibB
{
    partial class frmYGuipView
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color394636316567721121096", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text458636316567721131166", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static562636316567721141149");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Static598636316567721161206");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtBun = new System.Windows.Forms.TextBox();
            this.txtDanwi = new System.Windows.Forms.TextBox();
            this.txtJejo = new System.Windows.Forms.TextBox();
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.lbl5 = new System.Windows.Forms.Label();
            this.lbl4 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lbl3 = new System.Windows.Forms.Label();
            this.lbl2 = new System.Windows.Forms.Label();
            this.lbl1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtData = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ssYGuipView = new FarPoint.Win.Spread.FpSpread();
            this.ssYGuipView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitleSub0.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssYGuipView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssYGuipView_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.Controls.Add(this.label1);
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(612, 28);
            this.panTitleSub0.TabIndex = 84;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(278, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(248, 12);
            this.label1.TabIndex = 23;
            this.label1.Text = "구입신고내역 삭제: 해당 DATA 더블클릭";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(12, 9);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(57, 12);
            this.lblTitleSub0.TabIndex = 22;
            this.lblTitleSub0.Text = "신고내역";
            this.lblTitleSub0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Controls.Add(this.btnHelp);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(612, 34);
            this.panTitle.TabIndex = 83;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(3, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(116, 16);
            this.lblTitle.TabIndex = 14;
            this.lblTitle.Text = "신고내역 조회";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHelp.AutoSize = true;
            this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnHelp.Location = new System.Drawing.Point(455, 1);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(72, 30);
            this.btnHelp.TabIndex = 85;
            this.btnHelp.Text = "코드찾기";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(533, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 83;
            this.btnExit.Text = "닫 기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.AutoSize = true;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(28, 80);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 30);
            this.btnCancel.TabIndex = 85;
            this.btnCancel.Text = "취 소";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.txtBun);
            this.panel1.Controls.Add(this.txtDanwi);
            this.panel1.Controls.Add(this.txtJejo);
            this.panel1.Controls.Add(this.txtSpec);
            this.panel1.Controls.Add(this.lbl5);
            this.panel1.Controls.Add(this.lbl4);
            this.panel1.Controls.Add(this.txtName);
            this.panel1.Controls.Add(this.lbl3);
            this.panel1.Controls.Add(this.lbl2);
            this.panel1.Controls.Add(this.lbl1);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(0, 64);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(612, 153);
            this.panel1.TabIndex = 85;
            // 
            // txtBun
            // 
            this.txtBun.Location = new System.Drawing.Point(436, 90);
            this.txtBun.Name = "txtBun";
            this.txtBun.Size = new System.Drawing.Size(100, 21);
            this.txtBun.TabIndex = 10;
            // 
            // txtDanwi
            // 
            this.txtDanwi.Location = new System.Drawing.Point(436, 57);
            this.txtDanwi.Name = "txtDanwi";
            this.txtDanwi.Size = new System.Drawing.Size(100, 21);
            this.txtDanwi.TabIndex = 9;
            // 
            // txtJejo
            // 
            this.txtJejo.Location = new System.Drawing.Point(214, 90);
            this.txtJejo.Name = "txtJejo";
            this.txtJejo.Size = new System.Drawing.Size(165, 21);
            this.txtJejo.TabIndex = 8;
            // 
            // txtSpec
            // 
            this.txtSpec.Location = new System.Drawing.Point(214, 57);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.Size = new System.Drawing.Size(165, 21);
            this.txtSpec.TabIndex = 7;
            // 
            // lbl5
            // 
            this.lbl5.AutoSize = true;
            this.lbl5.Location = new System.Drawing.Point(385, 94);
            this.lbl5.Name = "lbl5";
            this.lbl5.Size = new System.Drawing.Size(45, 12);
            this.lbl5.TabIndex = 6;
            this.lbl5.Text = "분 류 : ";
            // 
            // lbl4
            // 
            this.lbl4.AutoSize = true;
            this.lbl4.Location = new System.Drawing.Point(385, 61);
            this.lbl4.Name = "lbl4";
            this.lbl4.Size = new System.Drawing.Size(45, 12);
            this.lbl4.TabIndex = 5;
            this.lbl4.Text = "단 위 : ";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(214, 24);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(322, 21);
            this.txtName.TabIndex = 4;
            // 
            // lbl3
            // 
            this.lbl3.AutoSize = true;
            this.lbl3.Location = new System.Drawing.Point(163, 94);
            this.lbl3.Name = "lbl3";
            this.lbl3.Size = new System.Drawing.Size(45, 12);
            this.lbl3.TabIndex = 3;
            this.lbl3.Text = "제 조 : ";
            // 
            // lbl2
            // 
            this.lbl2.AutoSize = true;
            this.lbl2.Location = new System.Drawing.Point(163, 61);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(45, 12);
            this.lbl2.TabIndex = 2;
            this.lbl2.Text = "규 격 : ";
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Location = new System.Drawing.Point(163, 28);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(45, 12);
            this.lbl1.TabIndex = 1;
            this.lbl1.Text = "품 명 : ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtData);
            this.groupBox1.Location = new System.Drawing.Point(8, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(113, 49);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "표준코드";
            // 
            // txtData
            // 
            this.txtData.Location = new System.Drawing.Point(6, 20);
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(100, 21);
            this.txtData.TabIndex = 1;
            this.txtData.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtData_KeyPress);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ssYGuipView);
            this.panel2.Location = new System.Drawing.Point(0, 219);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(612, 383);
            this.panel2.TabIndex = 86;
            // 
            // ssYGuipView
            // 
            this.ssYGuipView.AccessibleDescription = "";
            this.ssYGuipView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssYGuipView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssYGuipView.Location = new System.Drawing.Point(0, 0);
            this.ssYGuipView.Name = "ssYGuipView";
            namedStyle1.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType1.MaxLength = 60;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType2.Static = true;
            namedStyle3.CellType = textCellType2;
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.Renderer = textCellType2;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType3.Static = true;
            namedStyle4.CellType = textCellType3;
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.Renderer = textCellType3;
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssYGuipView.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4});
            this.ssYGuipView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssYGuipView_Sheet1});
            this.ssYGuipView.Size = new System.Drawing.Size(612, 383);
            this.ssYGuipView.TabIndex = 0;
            this.ssYGuipView.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssYGuipView.TextTipAppearance = tipAppearance1;
            this.ssYGuipView.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssYGuipView.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssYGuipView_CellDoubleClick);
            // 
            // ssYGuipView_Sheet1
            // 
            this.ssYGuipView_Sheet1.Reset();
            this.ssYGuipView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssYGuipView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssYGuipView_Sheet1.ColumnCount = 8;
            this.ssYGuipView_Sheet1.RowCount = 1;
            this.ssYGuipView_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssYGuipView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "적용일자";
            this.ssYGuipView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "수량";
            this.ssYGuipView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "금 액";
            this.ssYGuipView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "단 가";
            this.ssYGuipView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "최초";
            this.ssYGuipView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "신고일자";
            this.ssYGuipView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "비 고";
            this.ssYGuipView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "rowid";
            this.ssYGuipView_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssYGuipView_Sheet1.ColumnHeader.Rows.Get(0).Height = 19F;
            this.ssYGuipView_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssYGuipView_Sheet1.Columns.Get(0).Label = "적용일자";
            this.ssYGuipView_Sheet1.Columns.Get(0).StyleName = "Static562636316567721141149";
            this.ssYGuipView_Sheet1.Columns.Get(0).Width = 88F;
            this.ssYGuipView_Sheet1.Columns.Get(1).Label = "수량";
            this.ssYGuipView_Sheet1.Columns.Get(1).StyleName = "Static598636316567721161206";
            this.ssYGuipView_Sheet1.Columns.Get(1).Width = 70F;
            this.ssYGuipView_Sheet1.Columns.Get(2).Label = "금 액";
            this.ssYGuipView_Sheet1.Columns.Get(2).StyleName = "Static598636316567721161206";
            this.ssYGuipView_Sheet1.Columns.Get(2).Width = 113F;
            this.ssYGuipView_Sheet1.Columns.Get(3).Label = "단 가";
            this.ssYGuipView_Sheet1.Columns.Get(3).StyleName = "Static598636316567721161206";
            this.ssYGuipView_Sheet1.Columns.Get(3).Width = 81F;
            this.ssYGuipView_Sheet1.Columns.Get(4).Label = "최초";
            this.ssYGuipView_Sheet1.Columns.Get(4).StyleName = "Static562636316567721141149";
            this.ssYGuipView_Sheet1.Columns.Get(4).Width = 37F;
            this.ssYGuipView_Sheet1.Columns.Get(5).Label = "신고일자";
            this.ssYGuipView_Sheet1.Columns.Get(5).StyleName = "Static562636316567721141149";
            this.ssYGuipView_Sheet1.Columns.Get(5).Width = 103F;
            this.ssYGuipView_Sheet1.Columns.Get(6).Label = "비 고";
            this.ssYGuipView_Sheet1.Columns.Get(6).StyleName = "Static562636316567721141149";
            this.ssYGuipView_Sheet1.Columns.Get(6).Width = 85F;
            this.ssYGuipView_Sheet1.Columns.Get(7).Label = "rowid";
            this.ssYGuipView_Sheet1.Columns.Get(7).Visible = false;
            this.ssYGuipView_Sheet1.DefaultStyleName = "Text458636316567721131166";
            this.ssYGuipView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssYGuipView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmYGuipView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(612, 603);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Name = "frmYGuipView";
            this.Text = "의약품 실구입 신고내역 조회";
            this.Load += new System.EventHandler(this.frmYGuipView_Load);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssYGuipView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssYGuipView_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.Label lbl5;
        private System.Windows.Forms.Label lbl4;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lbl3;
        private System.Windows.Forms.Label lbl2;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.TextBox txtBun;
        private System.Windows.Forms.TextBox txtDanwi;
        private System.Windows.Forms.TextBox txtJejo;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Panel panel2;
        private FarPoint.Win.Spread.FpSpread ssYGuipView;
        private FarPoint.Win.Spread.SheetView ssYGuipView_Sheet1;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button btnExit;
    }
}