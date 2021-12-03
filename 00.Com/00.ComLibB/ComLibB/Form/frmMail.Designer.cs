namespace ComLibB
{
    partial class frmMail
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color394636316680010809915", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text458636316680010819936", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static562636316680010829961");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Static598636316680010839986");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnView = new System.Windows.Forms.Button();
            this.txtData = new System.Windows.Forms.TextBox();
            this.grbMethod = new System.Windows.Forms.GroupBox();
            this.optDong = new System.Windows.Forms.RadioButton();
            this.optNum = new System.Windows.Forms.RadioButton();
            this.optJuso = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ssMail = new FarPoint.Win.Spread.FpSpread();
            this.ssMail_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitleSub0.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.grbMethod.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssMail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMail_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(574, 28);
            this.panTitleSub0.TabIndex = 84;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(12, 9);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(88, 12);
            this.lblTitleSub0.TabIndex = 22;
            this.lblTitleSub0.Text = "우편번호 조회";
            this.lblTitleSub0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnView);
            this.panTitle.Controls.Add(this.txtData);
            this.panTitle.Controls.Add(this.grbMethod);
            this.panTitle.Controls.Add(this.btnCancel);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(574, 34);
            this.panTitle.TabIndex = 83;
            // 
            // btnView
            // 
            this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnView.AutoSize = true;
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnView.Location = new System.Drawing.Point(417, 1);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(72, 30);
            this.btnView.TabIndex = 92;
            this.btnView.Text = "찾 기";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // txtData
            // 
            this.txtData.Location = new System.Drawing.Point(231, 7);
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(182, 21);
            this.txtData.TabIndex = 91;
            this.txtData.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtData_KeyPress);
            // 
            // grbMethod
            // 
            this.grbMethod.Controls.Add(this.optDong);
            this.grbMethod.Controls.Add(this.optNum);
            this.grbMethod.Controls.Add(this.optJuso);
            this.grbMethod.Location = new System.Drawing.Point(3, -1);
            this.grbMethod.Name = "grbMethod";
            this.grbMethod.Size = new System.Drawing.Size(200, 36);
            this.grbMethod.TabIndex = 88;
            this.grbMethod.TabStop = false;
            this.grbMethod.Text = "찾기방법";
            // 
            // optDong
            // 
            this.optDong.AutoSize = true;
            this.optDong.Checked = true;
            this.optDong.ForeColor = System.Drawing.Color.Black;
            this.optDong.Location = new System.Drawing.Point(7, 14);
            this.optDong.Name = "optDong";
            this.optDong.Size = new System.Drawing.Size(59, 16);
            this.optDong.TabIndex = 85;
            this.optDong.TabStop = true;
            this.optDong.Text = "동명칭";
            this.optDong.UseVisualStyleBackColor = true;
            // 
            // optNum
            // 
            this.optNum.AutoSize = true;
            this.optNum.ForeColor = System.Drawing.Color.Black;
            this.optNum.Location = new System.Drawing.Point(123, 14);
            this.optNum.Name = "optNum";
            this.optNum.Size = new System.Drawing.Size(71, 16);
            this.optNum.TabIndex = 87;
            this.optNum.Text = "우편번호";
            this.optNum.UseVisualStyleBackColor = true;
            // 
            // optJuso
            // 
            this.optJuso.AutoSize = true;
            this.optJuso.ForeColor = System.Drawing.Color.Black;
            this.optJuso.Location = new System.Drawing.Point(71, 14);
            this.optJuso.Name = "optJuso";
            this.optJuso.Size = new System.Drawing.Size(47, 16);
            this.optJuso.TabIndex = 86;
            this.optJuso.Text = "주소";
            this.optJuso.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.AutoSize = true;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(495, 1);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 30);
            this.btnCancel.TabIndex = 83;
            this.btnCancel.Text = "닫 기";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ssMail);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(574, 207);
            this.panel1.TabIndex = 85;
            // 
            // ssMail
            // 
            this.ssMail.AccessibleDescription = "";
            this.ssMail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssMail.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssMail.Location = new System.Drawing.Point(0, 0);
            this.ssMail.Name = "ssMail";
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
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.Renderer = textCellType3;
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMail.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4});
            this.ssMail.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssMail_Sheet1});
            this.ssMail.Size = new System.Drawing.Size(574, 207);
            this.ssMail.TabIndex = 0;
            this.ssMail.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssMail.TextTipAppearance = tipAppearance1;
            this.ssMail.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssMail.LeaveCell += new FarPoint.Win.Spread.LeaveCellEventHandler(this.ssMail_LeaveCell);
            this.ssMail.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssMail_CellDoubleClick);
            this.ssMail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ssMail_KeyPress);
            // 
            // ssMail_Sheet1
            // 
            this.ssMail_Sheet1.Reset();
            this.ssMail_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssMail_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssMail_Sheet1.ColumnCount = 4;
            this.ssMail_Sheet1.RowCount = 1;
            this.ssMail_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssMail_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "우편번호";
            this.ssMail_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "주 소";
            this.ssMail_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "동명칭";
            this.ssMail_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "지역";
            this.ssMail_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssMail_Sheet1.ColumnHeader.Rows.Get(0).Height = 19F;
            this.ssMail_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssMail_Sheet1.Columns.Get(0).Label = "우편번호";
            this.ssMail_Sheet1.Columns.Get(0).StyleName = "Static562636316680010829961";
            this.ssMail_Sheet1.Columns.Get(0).Width = 63F;
            this.ssMail_Sheet1.Columns.Get(1).Label = "주 소";
            this.ssMail_Sheet1.Columns.Get(1).StyleName = "Static598636316680010839986";
            this.ssMail_Sheet1.Columns.Get(1).Width = 225F;
            this.ssMail_Sheet1.Columns.Get(2).Label = "동명칭";
            this.ssMail_Sheet1.Columns.Get(2).StyleName = "Static598636316680010839986";
            this.ssMail_Sheet1.Columns.Get(2).Width = 130F;
            this.ssMail_Sheet1.Columns.Get(3).Label = "지역";
            this.ssMail_Sheet1.Columns.Get(3).StyleName = "Static562636316680010829961";
            this.ssMail_Sheet1.Columns.Get(3).Width = 43F;
            this.ssMail_Sheet1.DefaultStyleName = "Text458636316680010819936";
            this.ssMail_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssMail_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmMail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(574, 269);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Name = "frmMail";
            this.Text = "우편번호찾기";
            this.Load += new System.EventHandler(this.frmMail_Load);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.grbMethod.ResumeLayout(false);
            this.grbMethod.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssMail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMail_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.GroupBox grbMethod;
        private System.Windows.Forms.RadioButton optDong;
        private System.Windows.Forms.RadioButton optNum;
        private System.Windows.Forms.RadioButton optJuso;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel1;
        private FarPoint.Win.Spread.FpSpread ssMail;
        private FarPoint.Win.Spread.SheetView ssMail_Sheet1;
    }
}