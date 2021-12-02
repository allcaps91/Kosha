namespace HC_OSHA.form.Etc
{
    partial class frmBrainRiskEvalution
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color341637214498074922131", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text419637214498074962021", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static535637214498075021855");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Static582637214498075041803");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.panTitle = new System.Windows.Forms.Panel();
            this.LblSite = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panBody = new System.Windows.Forms.Panel();
            this.SSList = new FarPoint.Win.Spread.FpSpread();
            this.SSList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panSearch = new System.Windows.Forms.Panel();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.ChkMore = new System.Windows.Forms.CheckBox();
            this.BtnExportExcel = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.DtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.DtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.TxtSiteIdOrName = new System.Windows.Forms.TextBox();
            this.BtnSearchSite = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CboYear = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panTitle.SuspendLayout();
            this.panBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).BeginInit();
            this.panSearch.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panTitle.Controls.Add(this.LblSite);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1029, 47);
            this.panTitle.TabIndex = 25;
            // 
            // LblSite
            // 
            this.LblSite.AutoSize = true;
            this.LblSite.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LblSite.Location = new System.Drawing.Point(184, 11);
            this.LblSite.Name = "LblSite";
            this.LblSite.Size = new System.Drawing.Size(160, 21);
            this.LblSite.TabIndex = 30;
            this.LblSite.Text = "뇌심혈관질환 위험도";
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(945, 0);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(82, 45);
            this.btnExit.TabIndex = 29;
            this.btnExit.Text = "닫 기(&X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(11, 11);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(160, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "뇌심혈관질환 위험도";
            // 
            // panBody
            // 
            this.panBody.Controls.Add(this.SSList);
            this.panBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBody.Location = new System.Drawing.Point(0, 115);
            this.panBody.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panBody.Name = "panBody";
            this.panBody.Size = new System.Drawing.Size(1029, 522);
            this.panBody.TabIndex = 26;
            // 
            // SSList
            // 
            this.SSList.AccessibleDescription = "";
            this.SSList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSList.Location = new System.Drawing.Point(0, 0);
            this.SSList.Name = "SSList";
            namedStyle1.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType1.MaxLength = 32000;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType2.Static = true;
            namedStyle3.CellType = textCellType2;
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.Renderer = textCellType2;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType3.Static = true;
            namedStyle4.CellType = textCellType3;
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.Renderer = textCellType3;
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4});
            this.SSList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSList_Sheet1});
            this.SSList.Size = new System.Drawing.Size(1029, 522);
            this.SSList.TabIndex = 4;
            this.SSList.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.SSList.TextTipAppearance = tipAppearance1;
            this.SSList.SetViewportLeftColumn(0, 0, 4);
            this.SSList.SetActiveViewport(0, 0, -1);
            // 
            // SSList_Sheet1
            // 
            this.SSList_Sheet1.Reset();
            this.SSList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSList_Sheet1.ColumnCount = 58;
            this.SSList_Sheet1.RowCount = 60;
            this.SSList_Sheet1.Cells.Get(0, 0).StyleName = "Static535637214498075021855";
            this.SSList_Sheet1.Cells.Get(0, 0).Value = "1";
            this.SSList_Sheet1.Cells.Get(0, 9).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Cells.Get(0, 9).Value = "10";
            this.SSList_Sheet1.Cells.Get(0, 19).Value = "20";
            this.SSList_Sheet1.Cells.Get(0, 28).Value = "29";
            this.SSList_Sheet1.Cells.Get(0, 45).Value = "46";
            this.SSList_Sheet1.Cells.Get(0, 46).Value = "47";
            this.SSList_Sheet1.Cells.Get(0, 47).Value = "48";
            this.SSList_Sheet1.Cells.Get(0, 48).Value = "49";
            this.SSList_Sheet1.Cells.Get(0, 49).Value = "50";
            this.SSList_Sheet1.Cells.Get(0, 50).Value = "51";
            this.SSList_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "소속";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "성명";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "주민번호";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "검진일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "검진병원";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "나이";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "성별";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "신장";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "체중";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "BMI";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "허리둘레";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "수축기 혈압";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "이완기 혈압";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "혈당";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "당화혈색소";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 15).Value = "총콜레스테롤";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 16).Value = "HDL";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 17).Value = "LDL";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 18).Value = "중성지방";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 19).Value = "단백뇨";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 20).Value = "크레아티닌";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 21).Value = "사구체여과율(GFR)";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 22).Value = "흉부-X선";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 23).Value = "심전도";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 24).Value = "안저";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 25).Value = "좌심실비대";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 26).Value = "맥파전달속도";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 27).Value = "발목위팔협압지수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 28).Value = "고혈압성망막";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 29).Value = "연령";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 30).Value = "혈당";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 31).Value = "비만,허리";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 32).Value = "HDL";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 33).Value = "콜레스테롤4종";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 34).Value = "신장3종";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 35).Value = "흡연";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 36).Value = "가족력";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 37).Value = "임시";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 38).Value = "임시";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 39).Value = "죽상동맥경화";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 40).Value = "고혈압성망막";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 41).Value = "당뇨";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 42).Value = "뇌졸증";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 43).Value = "심장질환";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 44).Value = "말초혈관질환";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 45).Value = "신장질환";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 46).Value = "만성콩판병";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 47).Value = "10년이내 심뇌혈관 발병 확률(%)";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 48).Value = "심뇌혈관나이";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 49).Value = "심뇌혈관발생위험";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 50).Value = "검진결과 평가";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 51).Value = "1단계";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 52).Value = "2단계";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 53).Value = "발병위험도평가";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 54).Value = "업무적합성판정";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 55).Value = "개선의견";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 56).Value = "주민등록";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 57).Value = "최종평가";
            this.SSList_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SSList_Sheet1.ColumnHeader.Rows.Get(0).Height = 54F;
            this.SSList_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SSList_Sheet1.Columns.Get(0).Label = "소속";
            this.SSList_Sheet1.Columns.Get(0).StyleName = "Static535637214498075021855";
            this.SSList_Sheet1.Columns.Get(0).Width = 100F;
            this.SSList_Sheet1.Columns.Get(1).Label = "성명";
            this.SSList_Sheet1.Columns.Get(1).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(1).Width = 58F;
            this.SSList_Sheet1.Columns.Get(2).Label = "주민번호";
            this.SSList_Sheet1.Columns.Get(2).StyleName = "Static535637214498075021855";
            this.SSList_Sheet1.Columns.Get(2).Width = 63F;
            this.SSList_Sheet1.Columns.Get(3).Label = "검진일";
            this.SSList_Sheet1.Columns.Get(3).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(3).Width = 72F;
            this.SSList_Sheet1.Columns.Get(4).Label = "검진병원";
            this.SSList_Sheet1.Columns.Get(4).StyleName = "Static535637214498075021855";
            this.SSList_Sheet1.Columns.Get(4).Width = 76F;
            this.SSList_Sheet1.Columns.Get(5).Label = "나이";
            this.SSList_Sheet1.Columns.Get(5).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(5).Width = 35F;
            this.SSList_Sheet1.Columns.Get(6).Label = "성별";
            this.SSList_Sheet1.Columns.Get(6).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(6).Width = 32F;
            this.SSList_Sheet1.Columns.Get(7).Label = "신장";
            this.SSList_Sheet1.Columns.Get(7).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(7).Width = 38F;
            this.SSList_Sheet1.Columns.Get(8).Label = "체중";
            this.SSList_Sheet1.Columns.Get(8).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(8).Width = 36F;
            this.SSList_Sheet1.Columns.Get(9).Label = "BMI";
            this.SSList_Sheet1.Columns.Get(9).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(9).Width = 36F;
            this.SSList_Sheet1.Columns.Get(10).Label = "허리둘레";
            this.SSList_Sheet1.Columns.Get(10).Width = 36F;
            this.SSList_Sheet1.Columns.Get(11).Label = "수축기 혈압";
            this.SSList_Sheet1.Columns.Get(11).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(11).Width = 48F;
            this.SSList_Sheet1.Columns.Get(12).Label = "이완기 혈압";
            this.SSList_Sheet1.Columns.Get(12).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(12).Width = 51F;
            this.SSList_Sheet1.Columns.Get(13).Label = "혈당";
            this.SSList_Sheet1.Columns.Get(13).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(13).Width = 36F;
            this.SSList_Sheet1.Columns.Get(15).Label = "총콜레스테롤";
            this.SSList_Sheet1.Columns.Get(15).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(15).Width = 45F;
            this.SSList_Sheet1.Columns.Get(16).Label = "HDL";
            this.SSList_Sheet1.Columns.Get(16).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(16).Width = 40F;
            this.SSList_Sheet1.Columns.Get(17).Label = "LDL";
            this.SSList_Sheet1.Columns.Get(17).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(17).Width = 38F;
            this.SSList_Sheet1.Columns.Get(18).Label = "중성지방";
            this.SSList_Sheet1.Columns.Get(18).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(18).Width = 40F;
            this.SSList_Sheet1.Columns.Get(19).Label = "단백뇨";
            this.SSList_Sheet1.Columns.Get(19).Width = 52F;
            this.SSList_Sheet1.Columns.Get(20).Label = "크레아티닌";
            this.SSList_Sheet1.Columns.Get(20).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(20).Width = 46F;
            this.SSList_Sheet1.Columns.Get(22).Label = "흉부-X선";
            this.SSList_Sheet1.Columns.Get(22).Width = 63F;
            this.SSList_Sheet1.Columns.Get(23).Label = "심전도";
            this.SSList_Sheet1.Columns.Get(23).Visible = false;
            this.SSList_Sheet1.Columns.Get(24).Label = "안저";
            this.SSList_Sheet1.Columns.Get(24).Visible = false;
            this.SSList_Sheet1.Columns.Get(25).Label = "좌심실비대";
            this.SSList_Sheet1.Columns.Get(25).Visible = false;
            this.SSList_Sheet1.Columns.Get(26).Label = "맥파전달속도";
            this.SSList_Sheet1.Columns.Get(26).Visible = false;
            this.SSList_Sheet1.Columns.Get(27).Label = "발목위팔협압지수";
            this.SSList_Sheet1.Columns.Get(27).Visible = false;
            this.SSList_Sheet1.Columns.Get(28).Label = "고혈압성망막";
            this.SSList_Sheet1.Columns.Get(28).Visible = false;
            this.SSList_Sheet1.Columns.Get(29).Label = "연령";
            this.SSList_Sheet1.Columns.Get(29).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(29).Width = 30F;
            this.SSList_Sheet1.Columns.Get(30).Label = "혈당";
            this.SSList_Sheet1.Columns.Get(30).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(30).Width = 31F;
            this.SSList_Sheet1.Columns.Get(31).Label = "비만,허리";
            this.SSList_Sheet1.Columns.Get(31).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(31).Width = 45F;
            this.SSList_Sheet1.Columns.Get(32).Label = "HDL";
            this.SSList_Sheet1.Columns.Get(32).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(32).Width = 43F;
            this.SSList_Sheet1.Columns.Get(33).Label = "콜레스테롤4종";
            this.SSList_Sheet1.Columns.Get(33).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(33).Width = 32F;
            this.SSList_Sheet1.Columns.Get(34).Label = "신장3종";
            this.SSList_Sheet1.Columns.Get(34).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(34).Width = 29F;
            this.SSList_Sheet1.Columns.Get(35).Label = "흡연";
            this.SSList_Sheet1.Columns.Get(35).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(35).Width = 43F;
            this.SSList_Sheet1.Columns.Get(36).Label = "가족력";
            this.SSList_Sheet1.Columns.Get(36).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(36).Width = 36F;
            this.SSList_Sheet1.Columns.Get(37).Label = "임시";
            this.SSList_Sheet1.Columns.Get(37).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(37).Visible = false;
            this.SSList_Sheet1.Columns.Get(37).Width = 39F;
            this.SSList_Sheet1.Columns.Get(38).Label = "임시";
            this.SSList_Sheet1.Columns.Get(38).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(38).Visible = false;
            this.SSList_Sheet1.Columns.Get(38).Width = 41F;
            this.SSList_Sheet1.Columns.Get(39).Label = "죽상동맥경화";
            this.SSList_Sheet1.Columns.Get(39).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(39).Visible = false;
            this.SSList_Sheet1.Columns.Get(39).Width = 39F;
            this.SSList_Sheet1.Columns.Get(40).Label = "고혈압성망막";
            this.SSList_Sheet1.Columns.Get(40).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(40).Visible = false;
            this.SSList_Sheet1.Columns.Get(40).Width = 41F;
            this.SSList_Sheet1.Columns.Get(41).Label = "당뇨";
            this.SSList_Sheet1.Columns.Get(41).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(41).Width = 34F;
            this.SSList_Sheet1.Columns.Get(42).Label = "뇌졸증";
            this.SSList_Sheet1.Columns.Get(42).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(42).Width = 52F;
            this.SSList_Sheet1.Columns.Get(43).Label = "심장질환";
            this.SSList_Sheet1.Columns.Get(43).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(43).Width = 34F;
            this.SSList_Sheet1.Columns.Get(44).Label = "말초혈관질환";
            this.SSList_Sheet1.Columns.Get(44).StyleName = "Static582637214498075041803";
            this.SSList_Sheet1.Columns.Get(44).Visible = false;
            this.SSList_Sheet1.Columns.Get(44).Width = 44F;
            this.SSList_Sheet1.Columns.Get(45).Label = "신장질환";
            this.SSList_Sheet1.Columns.Get(45).Visible = false;
            this.SSList_Sheet1.Columns.Get(45).Width = 41F;
            this.SSList_Sheet1.Columns.Get(46).Label = "만성콩판병";
            this.SSList_Sheet1.Columns.Get(46).Visible = false;
            this.SSList_Sheet1.Columns.Get(46).Width = 40F;
            this.SSList_Sheet1.Columns.Get(51).Label = "1단계";
            this.SSList_Sheet1.Columns.Get(51).StyleName = "Static535637214498075021855";
            this.SSList_Sheet1.Columns.Get(51).Width = 52F;
            this.SSList_Sheet1.Columns.Get(52).Label = "2단계";
            this.SSList_Sheet1.Columns.Get(52).StyleName = "Static535637214498075021855";
            this.SSList_Sheet1.Columns.Get(52).Width = 41F;
            this.SSList_Sheet1.Columns.Get(53).Label = "발병위험도평가";
            this.SSList_Sheet1.Columns.Get(53).StyleName = "Static535637214498075021855";
            this.SSList_Sheet1.Columns.Get(53).Width = 95F;
            this.SSList_Sheet1.Columns.Get(54).Label = "업무적합성판정";
            this.SSList_Sheet1.Columns.Get(54).StyleName = "Static535637214498075021855";
            this.SSList_Sheet1.Columns.Get(54).Width = 99F;
            this.SSList_Sheet1.Columns.Get(55).Label = "개선의견";
            this.SSList_Sheet1.Columns.Get(55).StyleName = "Static535637214498075021855";
            this.SSList_Sheet1.Columns.Get(55).Width = 104F;
            this.SSList_Sheet1.Columns.Get(56).Label = "주민등록";
            this.SSList_Sheet1.Columns.Get(56).StyleName = "Static535637214498075021855";
            this.SSList_Sheet1.Columns.Get(56).Visible = false;
            this.SSList_Sheet1.DefaultStyleName = "Text419637214498074962021";
            this.SSList_Sheet1.FrozenColumnCount = 4;
            this.SSList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panSearch
            // 
            this.panSearch.Controls.Add(this.BtnSearch);
            this.panSearch.Controls.Add(this.ChkMore);
            this.panSearch.Controls.Add(this.BtnExportExcel);
            this.panSearch.Controls.Add(this.groupBox3);
            this.panSearch.Controls.Add(this.groupBox2);
            this.panSearch.Controls.Add(this.groupBox1);
            this.panSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSearch.Location = new System.Drawing.Point(0, 47);
            this.panSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panSearch.Name = "panSearch";
            this.panSearch.Size = new System.Drawing.Size(1029, 68);
            this.panSearch.TabIndex = 27;
            this.panSearch.Paint += new System.Windows.Forms.PaintEventHandler(this.panSearch_Paint);
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(713, 24);
            this.BtnSearch.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(75, 28);
            this.BtnSearch.TabIndex = 121;
            this.BtnSearch.Text = "검색";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // ChkMore
            // 
            this.ChkMore.AutoSize = true;
            this.ChkMore.Location = new System.Drawing.Point(623, 29);
            this.ChkMore.Name = "ChkMore";
            this.ChkMore.Size = new System.Drawing.Size(84, 21);
            this.ChkMore.TabIndex = 122;
            this.ChkMore.Text = "종검 보기";
            this.ChkMore.UseVisualStyleBackColor = true;
            // 
            // BtnExportExcel
            // 
            this.BtnExportExcel.Location = new System.Drawing.Point(794, 25);
            this.BtnExportExcel.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnExportExcel.Name = "BtnExportExcel";
            this.BtnExportExcel.Size = new System.Drawing.Size(75, 28);
            this.BtnExportExcel.TabIndex = 121;
            this.BtnExportExcel.Text = "엑셀파일생성";
            this.BtnExportExcel.UseVisualStyleBackColor = true;
            this.BtnExportExcel.Click += new System.EventHandler(this.BtnExportExcel_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.DtpEndDate);
            this.groupBox3.Controls.Add(this.DtpStartDate);
            this.groupBox3.Location = new System.Drawing.Point(376, 6);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox3.Size = new System.Drawing.Size(241, 54);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "작업기간";
            // 
            // DtpEndDate
            // 
            this.DtpEndDate.CustomFormat = "yyyy-MM-dd";
            this.DtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpEndDate.Location = new System.Drawing.Point(120, 22);
            this.DtpEndDate.Name = "DtpEndDate";
            this.DtpEndDate.Size = new System.Drawing.Size(108, 25);
            this.DtpEndDate.TabIndex = 4;
            // 
            // DtpStartDate
            // 
            this.DtpStartDate.CustomFormat = "yyyy-MM-dd";
            this.DtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpStartDate.Location = new System.Drawing.Point(6, 22);
            this.DtpStartDate.Name = "DtpStartDate";
            this.DtpStartDate.Size = new System.Drawing.Size(108, 25);
            this.DtpStartDate.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.TxtSiteIdOrName);
            this.groupBox2.Controls.Add(this.BtnSearchSite);
            this.groupBox2.Location = new System.Drawing.Point(125, 6);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(220, 54);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "회사명(코드)";
            this.groupBox2.Visible = false;
            // 
            // TxtSiteIdOrName
            // 
            this.TxtSiteIdOrName.Location = new System.Drawing.Point(6, 21);
            this.TxtSiteIdOrName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TxtSiteIdOrName.Name = "TxtSiteIdOrName";
            this.TxtSiteIdOrName.Size = new System.Drawing.Size(127, 25);
            this.TxtSiteIdOrName.TabIndex = 0;
            // 
            // BtnSearchSite
            // 
            this.BtnSearchSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSearchSite.Location = new System.Drawing.Point(139, 20);
            this.BtnSearchSite.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearchSite.Name = "BtnSearchSite";
            this.BtnSearchSite.Size = new System.Drawing.Size(75, 28);
            this.BtnSearchSite.TabIndex = 120;
            this.BtnSearchSite.Text = "검색";
            this.BtnSearchSite.UseVisualStyleBackColor = true;
            this.BtnSearchSite.Click += new System.EventHandler(this.BtnSearchSite_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CboYear);
            this.groupBox1.Location = new System.Drawing.Point(16, 6);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(103, 54);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "건진년도";
            // 
            // CboYear
            // 
            this.CboYear.FormattingEnabled = true;
            this.CboYear.Location = new System.Drawing.Point(6, 21);
            this.CboYear.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CboYear.Name = "CboYear";
            this.CboYear.Size = new System.Drawing.Size(87, 25);
            this.CboYear.TabIndex = 0;
            this.CboYear.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // frmBrainRiskEvalution
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1029, 637);
            this.Controls.Add(this.panBody);
            this.Controls.Add(this.panSearch);
            this.Controls.Add(this.panTitle);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmBrainRiskEvalution";
            this.Text = "frmBrainRiskEvalution";
            this.Load += new System.EventHandler(this.frmBrainRiskEvalution_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).EndInit();
            this.panSearch.ResumeLayout(false);
            this.panSearch.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panBody;
        private System.Windows.Forms.Panel panSearch;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox TxtSiteIdOrName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox CboYear;
        private System.Windows.Forms.DateTimePicker DtpEndDate;
        private System.Windows.Forms.DateTimePicker DtpStartDate;
        private System.Windows.Forms.Button BtnExportExcel;
        private System.Windows.Forms.Button BtnSearchSite;
        private System.Windows.Forms.CheckBox ChkMore;
        private FarPoint.Win.Spread.FpSpread SSList;
        private FarPoint.Win.Spread.SheetView SSList_Sheet1;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.Label LblSite;
    }
}