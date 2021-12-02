namespace ComEmrBase
{
    partial class frmAutoTextPT
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color370637252441755717578", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text460637252441755737461", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static564637252441755747434");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Color370637252441648747381", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("Text460637252441648782889", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle6 = new FarPoint.Win.Spread.NamedStyle("Static576637252441648802852");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance2 = new FarPoint.Win.Spread.TipAppearance();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkMy = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboGubun = new System.Windows.Forms.ComboBox();
            this.btnSave1 = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnSave2 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnText = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.txtText = new System.Windows.Forms.TextBox();
            this.SS2 = new FarPoint.Win.Spread.FpSpread();
            this.SS2_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.SS1 = new FarPoint.Win.Spread.FpSpread();
            this.SS1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panContent = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnVisible = new System.Windows.Forms.Button();
            this.btnSave3 = new System.Windows.Forms.Button();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS2_Sheet1)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).BeginInit();
            this.panContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkMy);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cboGubun);
            this.panel1.Controls.Add(this.btnSave1);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.btnSave2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(690, 58);
            this.panel1.TabIndex = 0;
            // 
            // chkMy
            // 
            this.chkMy.AutoSize = true;
            this.chkMy.Location = new System.Drawing.Point(14, 36);
            this.chkMy.Name = "chkMy";
            this.chkMy.Size = new System.Drawing.Size(156, 16);
            this.chkMy.TabIndex = 9;
            this.chkMy.Text = "내가 작성한 내용만 조회";
            this.chkMy.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "기록지 선택";
            // 
            // cboGubun
            // 
            this.cboGubun.FormattingEnabled = true;
            this.cboGubun.Location = new System.Drawing.Point(81, 10);
            this.cboGubun.Name = "cboGubun";
            this.cboGubun.Size = new System.Drawing.Size(281, 20);
            this.cboGubun.TabIndex = 7;
            this.cboGubun.SelectedIndexChanged += new System.EventHandler(this.cboGubun_SelectedIndexChanged);
            // 
            // btnSave1
            // 
            this.btnSave1.BackColor = System.Drawing.SystemColors.Control;
            this.btnSave1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave1.Location = new System.Drawing.Point(368, 4);
            this.btnSave1.Name = "btnSave1";
            this.btnSave1.Size = new System.Drawing.Size(98, 33);
            this.btnSave1.TabIndex = 6;
            this.btnSave1.Text = "구분추가하기";
            this.btnSave1.UseVisualStyleBackColor = false;
            this.btnSave1.Click += new System.EventHandler(this.btnSave1_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.SystemColors.Control;
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(625, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(61, 33);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.SystemColors.Control;
            this.btnSearch.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(564, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(61, 33);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnSave2
            // 
            this.btnSave2.BackColor = System.Drawing.SystemColors.Control;
            this.btnSave2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave2.Location = new System.Drawing.Point(466, 4);
            this.btnSave2.Name = "btnSave2";
            this.btnSave2.Size = new System.Drawing.Size(98, 33);
            this.btnSave2.TabIndex = 6;
            this.btnSave2.Text = "내용추가하기";
            this.btnSave2.UseVisualStyleBackColor = false;
            this.btnSave2.Click += new System.EventHandler(this.btnSave2_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 58);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(690, 469);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnText);
            this.panel3.Controls.Add(this.btnDel);
            this.panel3.Controls.Add(this.txtText);
            this.panel3.Controls.Add(this.SS2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(295, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(393, 469);
            this.panel3.TabIndex = 1;
            // 
            // btnText
            // 
            this.btnText.BackColor = System.Drawing.SystemColors.Control;
            this.btnText.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnText.Location = new System.Drawing.Point(330, 201);
            this.btnText.Name = "btnText";
            this.btnText.Size = new System.Drawing.Size(61, 33);
            this.btnText.TabIndex = 7;
            this.btnText.Text = "적용";
            this.btnText.UseVisualStyleBackColor = false;
            this.btnText.Click += new System.EventHandler(this.btnText_Click);
            // 
            // btnDel
            // 
            this.btnDel.BackColor = System.Drawing.SystemColors.Control;
            this.btnDel.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDel.Location = new System.Drawing.Point(269, 201);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(61, 33);
            this.btnDel.TabIndex = 8;
            this.btnDel.Text = "지우기";
            this.btnDel.UseVisualStyleBackColor = false;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // txtText
            // 
            this.txtText.Location = new System.Drawing.Point(3, 237);
            this.txtText.Multiline = true;
            this.txtText.Name = "txtText";
            this.txtText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtText.Size = new System.Drawing.Size(388, 208);
            this.txtText.TabIndex = 2;
            // 
            // SS2
            // 
            this.SS2.AccessibleDescription = "";
            this.SS2.Dock = System.Windows.Forms.DockStyle.Top;
            this.SS2.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.SS2.Location = new System.Drawing.Point(0, 0);
            this.SS2.Name = "SS2";
            namedStyle1.Font = new System.Drawing.Font("나눔고딕", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle2.Font = new System.Drawing.Font("나눔고딕", 9F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType1.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
            textCellType1.Static = true;
            textCellType1.WordWrap = true;
            namedStyle3.CellType = textCellType1;
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.Renderer = textCellType1;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS2.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3});
            this.SS2.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS2_Sheet1});
            this.SS2.Size = new System.Drawing.Size(393, 201);
            this.SS2.TabIndex = 1;
            this.SS2.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.SS2.TextTipAppearance = tipAppearance1;
            this.SS2.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SS2.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SS2_CellDoubleClick);
            // 
            // SS2_Sheet1
            // 
            this.SS2_Sheet1.Reset();
            this.SS2_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS2_Sheet1.ColumnCount = 3;
            this.SS2_Sheet1.RowCount = 20;
            this.SS2_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SS2_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "내용";
            this.SS2_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SS2_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SS2_Sheet1.Columns.Get(0).Label = "내용";
            this.SS2_Sheet1.Columns.Get(0).StyleName = "Static564637252441755747434";
            this.SS2_Sheet1.Columns.Get(0).Width = 337F;
            this.SS2_Sheet1.Columns.Get(1).Visible = false;
            this.SS2_Sheet1.Columns.Get(1).Width = 49F;
            this.SS2_Sheet1.Columns.Get(2).Visible = false;
            this.SS2_Sheet1.DefaultStyleName = "Text460637252441755737461";
            this.SS2_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            this.SS2_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS2_Sheet1.Rows.Default.Height = 25F;
            this.SS2_Sheet1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.SS2_Sheet1.SelectionStyle = FarPoint.Win.Spread.SelectionStyles.SelectionColors;
            this.SS2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.SS1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(295, 469);
            this.panel4.TabIndex = 10;
            // 
            // SS1
            // 
            this.SS1.AccessibleDescription = "";
            this.SS1.Dock = System.Windows.Forms.DockStyle.Top;
            this.SS1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.SS1.Location = new System.Drawing.Point(0, 0);
            this.SS1.Name = "SS1";
            namedStyle4.Font = new System.Drawing.Font("나눔고딕", 9F);
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.Parent = "DataAreaDefault";
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType2.MaxLength = 32000;
            namedStyle5.CellType = textCellType2;
            namedStyle5.Font = new System.Drawing.Font("나눔고딕", 9F);
            namedStyle5.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle5.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle5.Parent = "DataAreaDefault";
            namedStyle5.Renderer = textCellType2;
            namedStyle5.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType3.Static = true;
            namedStyle6.CellType = textCellType3;
            namedStyle6.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle6.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle6.Renderer = textCellType3;
            namedStyle6.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle4,
            namedStyle5,
            namedStyle6});
            this.SS1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS1_Sheet1});
            this.SS1.Size = new System.Drawing.Size(295, 448);
            this.SS1.TabIndex = 1;
            this.SS1.TabStripRatio = 0.6D;
            tipAppearance2.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance2.ForeColor = System.Drawing.SystemColors.InfoText;
            this.SS1.TextTipAppearance = tipAppearance2;
            this.SS1.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SS1_CellClick);
            // 
            // SS1_Sheet1
            // 
            this.SS1_Sheet1.Reset();
            this.SS1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS1_Sheet1.ColumnCount = 4;
            this.SS1_Sheet1.RowCount = 20;
            this.SS1_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "구분";
            this.SS1_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SS1_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SS1_Sheet1.Columns.Get(0).Label = "구분";
            this.SS1_Sheet1.Columns.Get(0).StyleName = "Static576637252441648802852";
            this.SS1_Sheet1.Columns.Get(0).Width = 235F;
            this.SS1_Sheet1.Columns.Get(1).Visible = false;
            this.SS1_Sheet1.Columns.Get(1).Width = 49F;
            this.SS1_Sheet1.Columns.Get(2).Visible = false;
            this.SS1_Sheet1.Columns.Get(3).Visible = false;
            this.SS1_Sheet1.DefaultStyleName = "Text460637252441648782889";
            this.SS1_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SS1_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            this.SS1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS1_Sheet1.Rows.Default.Height = 25F;
            this.SS1_Sheet1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.SS1_Sheet1.SelectionStyle = FarPoint.Win.Spread.SelectionStyles.SelectionColors;
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panContent
            // 
            this.panContent.Controls.Add(this.label2);
            this.panContent.Controls.Add(this.btnVisible);
            this.panContent.Controls.Add(this.btnSave3);
            this.panContent.Controls.Add(this.txtContent);
            this.panContent.Location = new System.Drawing.Point(12, 64);
            this.panContent.Name = "panContent";
            this.panContent.Size = new System.Drawing.Size(413, 299);
            this.panContent.TabIndex = 2;
            this.panContent.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("나눔고딕", 9F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(15, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 14);
            this.label2.TabIndex = 10;
            this.label2.Text = " 내용 입력 및 수정";
            // 
            // btnVisible
            // 
            this.btnVisible.BackColor = System.Drawing.SystemColors.Control;
            this.btnVisible.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnVisible.Location = new System.Drawing.Point(344, 245);
            this.btnVisible.Name = "btnVisible";
            this.btnVisible.Size = new System.Drawing.Size(61, 45);
            this.btnVisible.TabIndex = 9;
            this.btnVisible.Text = "닫기";
            this.btnVisible.UseVisualStyleBackColor = false;
            this.btnVisible.Click += new System.EventHandler(this.btnVisible_Click);
            // 
            // btnSave3
            // 
            this.btnSave3.BackColor = System.Drawing.SystemColors.Control;
            this.btnSave3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave3.Location = new System.Drawing.Point(17, 245);
            this.btnSave3.Name = "btnSave3";
            this.btnSave3.Size = new System.Drawing.Size(321, 45);
            this.btnSave3.TabIndex = 8;
            this.btnSave3.Text = "적용";
            this.btnSave3.UseVisualStyleBackColor = false;
            this.btnSave3.Click += new System.EventHandler(this.btnSave3_Click);
            // 
            // txtContent
            // 
            this.txtContent.Location = new System.Drawing.Point(17, 31);
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtContent.Size = new System.Drawing.Size(388, 208);
            this.txtContent.TabIndex = 3;
            // 
            // frmAutoTextPT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(690, 527);
            this.Controls.Add(this.panContent);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "frmAutoTextPT";
            this.Text = "물리치료실 상용구 등록 및 조회";
            this.Load += new System.EventHandler(this.frmAutoTextPT_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS2_Sheet1)).EndInit();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).EndInit();
            this.panContent.ResumeLayout(false);
            this.panContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private FarPoint.Win.Spread.FpSpread SS2;
        private FarPoint.Win.Spread.SheetView SS2_Sheet1;
        private System.Windows.Forms.Button btnSave2;
        private System.Windows.Forms.Button btnSave1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panel4;
        private FarPoint.Win.Spread.FpSpread SS1;
        private FarPoint.Win.Spread.SheetView SS1_Sheet1;
        private System.Windows.Forms.Button btnText;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.TextBox txtText;
        private System.Windows.Forms.CheckBox chkMy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboGubun;
        private System.Windows.Forms.Panel panContent;
        private System.Windows.Forms.Button btnVisible;
        private System.Windows.Forms.Button btnSave3;
        private System.Windows.Forms.TextBox txtContent;
        private System.Windows.Forms.Label label2;
    }
}