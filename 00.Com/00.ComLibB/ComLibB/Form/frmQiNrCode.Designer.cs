namespace ComLibB
{
    partial class frmQiNrCode
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color383636307183287844031", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text447636307183287844031", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("CheckBox3969636307183288000304");
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType1 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmQiNrCode));
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Text4013636307183288000304");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("Text4057636307183288000304");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle6 = new FarPoint.Win.Spread.NamedStyle("Number31636307183288000304");
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType1 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle7 = new FarPoint.Win.Spread.NamedStyle("Static67636307183288000304");
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.Spread.ThreeColorScaleConditionalFormattingRule threeColorScaleConditionalFormattingRule1 = new FarPoint.Win.Spread.ThreeColorScaleConditionalFormattingRule(new FarPoint.Win.Spread.ConditionalFormattingColorValue(System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(105)))), ((int)(((byte)(107))))), null, FarPoint.Win.Spread.ConditionalFormattingValueType.Min), new FarPoint.Win.Spread.ConditionalFormattingColorValue(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(235)))), ((int)(((byte)(132))))), 50, FarPoint.Win.Spread.ConditionalFormattingValueType.Percentile), new FarPoint.Win.Spread.ConditionalFormattingColorValue(System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(190)))), ((int)(((byte)(123))))), null, FarPoint.Win.Spread.ConditionalFormattingValueType.Max), false);
            FarPoint.Win.Spread.ThreeColorScaleConditionalFormattingRule threeColorScaleConditionalFormattingRule2 = new FarPoint.Win.Spread.ThreeColorScaleConditionalFormattingRule(new FarPoint.Win.Spread.ConditionalFormattingColorValue(System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(190)))), ((int)(((byte)(123))))), null, FarPoint.Win.Spread.ConditionalFormattingValueType.Min), new FarPoint.Win.Spread.ConditionalFormattingColorValue(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(235)))), ((int)(((byte)(132))))), 50, FarPoint.Win.Spread.ConditionalFormattingValueType.Percentile), new FarPoint.Win.Spread.ConditionalFormattingColorValue(System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(105)))), ((int)(((byte)(107))))), null, FarPoint.Win.Spread.ConditionalFormattingValueType.Max), false);
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType2 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grbJong = new System.Windows.Forms.GroupBox();
            this.btnView = new System.Windows.Forms.Button();
            this.cboJong = new System.Windows.Forms.ComboBox();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panTitle0 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.ss1 = new FarPoint.Win.Spread.FpSpread();
            this.ss1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.grbJong.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panTitle0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Controls.Add(this.btnCancel);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 117);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(447, 27);
            this.panel3.TabIndex = 95;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(303, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 27);
            this.btnSave.TabIndex = 28;
            this.btnSave.Text = "등록(&S)";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(375, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 27);
            this.btnCancel.TabIndex = 23;
            this.btnCancel.Text = "취소(&C)";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(12, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 17);
            this.label3.TabIndex = 21;
            this.label3.Text = "조회 화면 및 등록";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grbJong);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 65);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(447, 52);
            this.panel1.TabIndex = 94;
            // 
            // grbJong
            // 
            this.grbJong.Controls.Add(this.btnView);
            this.grbJong.Controls.Add(this.cboJong);
            this.grbJong.Dock = System.Windows.Forms.DockStyle.Left;
            this.grbJong.Location = new System.Drawing.Point(0, 0);
            this.grbJong.Name = "grbJong";
            this.grbJong.Size = new System.Drawing.Size(289, 52);
            this.grbJong.TabIndex = 0;
            this.grbJong.TabStop = false;
            this.grbJong.Text = "코드종류";
            // 
            // btnView
            // 
            this.btnView.BackColor = System.Drawing.Color.Transparent;
            this.btnView.ForeColor = System.Drawing.Color.Black;
            this.btnView.Location = new System.Drawing.Point(211, 19);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(72, 27);
            this.btnView.TabIndex = 25;
            this.btnView.Text = "조회(&V)";
            this.btnView.UseVisualStyleBackColor = false;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // cboJong
            // 
            this.cboJong.FormattingEnabled = true;
            this.cboJong.Location = new System.Drawing.Point(8, 22);
            this.cboJong.Name = "cboJong";
            this.cboJong.Size = new System.Drawing.Size(197, 20);
            this.cboJong.TabIndex = 0;
            this.cboJong.Text = "cboJong";
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 38);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(447, 27);
            this.panTitleSub0.TabIndex = 93;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(12, 3);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(65, 17);
            this.lblTitleSub0.TabIndex = 21;
            this.lblTitleSub0.Text = "조회 조건";
            // 
            // panTitle0
            // 
            this.panTitle0.BackColor = System.Drawing.Color.White;
            this.panTitle0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle0.Controls.Add(this.btnExit);
            this.panTitle0.Controls.Add(this.lblTitle);
            this.panTitle0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle0.ForeColor = System.Drawing.Color.White;
            this.panTitle0.Location = new System.Drawing.Point(0, 0);
            this.panTitle0.Name = "panTitle0";
            this.panTitle0.Size = new System.Drawing.Size(447, 38);
            this.panTitle0.TabIndex = 92;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(361, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(79, 29);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "닫  기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(5, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(150, 21);
            this.lblTitle.TabIndex = 14;
            this.lblTitle.Text = "각종 기초코드 등록";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ss1
            // 
            this.ss1.AccessibleDescription = "";
            this.ss1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ss1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ss1.Location = new System.Drawing.Point(0, 144);
            this.ss1.Name = "ss1";
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
            checkBoxCellType1.Picture.False = ((System.Drawing.Image)(resources.GetObject("resource.False")));
            checkBoxCellType1.Picture.FalsePressed = ((System.Drawing.Image)(resources.GetObject("resource.FalsePressed")));
            checkBoxCellType1.Picture.Indeterminate = ((System.Drawing.Image)(resources.GetObject("resource.Indeterminate")));
            checkBoxCellType1.Picture.IndeterminatePressed = ((System.Drawing.Image)(resources.GetObject("resource.IndeterminatePressed")));
            checkBoxCellType1.Picture.True = ((System.Drawing.Image)(resources.GetObject("resource.True")));
            checkBoxCellType1.Picture.TruePressed = ((System.Drawing.Image)(resources.GetObject("resource.TruePressed")));
            namedStyle3.CellType = checkBoxCellType1;
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.Renderer = checkBoxCellType1;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            textCellType2.MaxLength = 8;
            namedStyle4.CellType = textCellType2;
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.Renderer = textCellType2;
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType3.MaxLength = 30;
            namedStyle5.CellType = textCellType3;
            namedStyle5.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle5.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle5.Renderer = textCellType3;
            namedStyle5.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            numberCellType1.MaximumValue = 9999D;
            numberCellType1.MinimumValue = 0D;
            namedStyle6.CellType = numberCellType1;
            namedStyle6.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle6.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle6.Renderer = numberCellType1;
            namedStyle6.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType4.Static = true;
            namedStyle7.CellType = textCellType4;
            namedStyle7.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle7.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle7.Renderer = textCellType4;
            namedStyle7.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4,
            namedStyle5,
            namedStyle6,
            namedStyle7});
            this.ss1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ss1_Sheet1});
            this.ss1.Size = new System.Drawing.Size(447, 423);
            this.ss1.TabIndex = 96;
            this.ss1.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ss1.TextTipAppearance = tipAppearance1;
            this.ss1.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ss1.LeaveCell += new FarPoint.Win.Spread.LeaveCellEventHandler(this.ss1_LeaveCell);
            // 
            // ss1_Sheet1
            // 
            this.ss1_Sheet1.Reset();
            this.ss1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ss1_Sheet1.ColumnCount = 8;
            this.ss1_Sheet1.RowCount = 1;
            threeColorScaleConditionalFormattingRule1.Priority = 2;
            this.ss1_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = " ";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "코드";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "코드명칭";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "SORT";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "ROWID";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "OldCode";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "OldName";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "OldSort";
            this.ss1_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ss1_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.Columns.Get(0).CellType = checkBoxCellType2;
            this.ss1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(0).Label = " ";
            this.ss1_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(0).Width = 28F;
            this.ss1_Sheet1.Columns.Get(1).Label = "코드";
            this.ss1_Sheet1.Columns.Get(1).StyleName = "Text4013636307183288000304";
            this.ss1_Sheet1.Columns.Get(1).Width = 70F;
            this.ss1_Sheet1.Columns.Get(2).Label = "코드명칭";
            this.ss1_Sheet1.Columns.Get(2).StyleName = "Text4057636307183288000304";
            this.ss1_Sheet1.Columns.Get(2).Width = 265F;
            this.ss1_Sheet1.Columns.Get(3).Label = "SORT";
            this.ss1_Sheet1.Columns.Get(3).StyleName = "Number31636307183288000304";
            this.ss1_Sheet1.Columns.Get(3).Width = 46F;
            this.ss1_Sheet1.Columns.Get(4).Label = "ROWID";
            this.ss1_Sheet1.Columns.Get(4).StyleName = "Static67636307183288000304";
            this.ss1_Sheet1.Columns.Get(4).Width = 58F;
            this.ss1_Sheet1.Columns.Get(5).Label = "OldCode";
            this.ss1_Sheet1.Columns.Get(5).StyleName = "Static67636307183288000304";
            this.ss1_Sheet1.Columns.Get(6).Label = "OldName";
            this.ss1_Sheet1.Columns.Get(6).StyleName = "Static67636307183288000304";
            this.ss1_Sheet1.Columns.Get(7).Label = "OldSort";
            this.ss1_Sheet1.Columns.Get(7).StyleName = "Static67636307183288000304";
            this.ss1_Sheet1.Columns.Get(7).Width = 58F;
            this.ss1_Sheet1.DefaultStyleName = "Text447636307183287844031";
            this.ss1_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ss1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ss1_Sheet1.Rows.Get(0).Height = 16F;
            this.ss1_Sheet1.SetConditionalFormatting(new FarPoint.Win.Spread.Model.CellRange[] {
            new FarPoint.Win.Spread.Model.CellRange(-1, 0, -1, 1)}, new FarPoint.Win.Spread.IConditionalFormattingRule[] {
            threeColorScaleConditionalFormattingRule1});
            this.ss1_Sheet1.SetConditionalFormatting(new FarPoint.Win.Spread.Model.CellRange[] {
            new FarPoint.Win.Spread.Model.CellRange(-1, 0, -1, 1)}, new FarPoint.Win.Spread.IConditionalFormattingRule[] {
            threeColorScaleConditionalFormattingRule2});
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmQiNrCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(447, 567);
            this.Controls.Add(this.ss1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle0);
            this.Name = "frmQiNrCode";
            this.Text = "각종 기초코드 등록";
            this.Load += new System.EventHandler(this.frmQiNrCode_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.grbJong.ResumeLayout(false);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panTitle0.ResumeLayout(false);
            this.panTitle0.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox grbJong;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.ComboBox cboJong;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panTitle0;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private FarPoint.Win.Spread.FpSpread ss1;
        private FarPoint.Win.Spread.SheetView ss1_Sheet1;
    }
}