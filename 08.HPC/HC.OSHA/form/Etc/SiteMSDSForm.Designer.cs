namespace HC_OSHA
{
    partial class SiteMSDSForm
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
            this.formTItle1 = new ComBase.Mvc.UserControls.FormTItle();
            this.tableBody = new System.Windows.Forms.TableLayoutPanel();
            this.panBottomLeft = new System.Windows.Forms.Panel();
            this.panMsdsBoby = new System.Windows.Forms.Panel();
            this.SSMSDSList = new FarPoint.Win.Spread.FpSpread();
            this.SSMSDSList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panSearch = new System.Windows.Forms.Panel();
            this.RdoMsdsCasNo = new System.Windows.Forms.RadioButton();
            this.RdoMsdsName = new System.Windows.Forms.RadioButton();
            this.BtnSearchMsds = new System.Windows.Forms.Button();
            this.TxtSearchMsdsWord = new System.Windows.Forms.TextBox();
            this.contentTitle3 = new ComBase.Mvc.UserControls.ContentTitle();
            this.panTop = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.SSProduct = new FarPoint.Win.Spread.FpSpread();
            this.SSProduct_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.BtnSaveProduct = new System.Windows.Forms.Button();
            this.BtnAddProduct = new System.Windows.Forms.Button();
            this.contentTitle1 = new ComBase.Mvc.UserControls.ContentTitle();
            this.panBottomRight = new System.Windows.Forms.Panel();
            this.panProductMsdsBody = new System.Windows.Forms.Panel();
            this.SSProductMsds = new FarPoint.Win.Spread.FpSpread();
            this.SSProductMsds_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panSiteProduct = new System.Windows.Forms.Panel();
            this.TxtProductName = new System.Windows.Forms.TextBox();
            this.BtnSaveMsds = new System.Windows.Forms.Button();
            this.LblProductName = new System.Windows.Forms.Label();
            this.contentTitle2 = new ComBase.Mvc.UserControls.ContentTitle();
            this.btnExit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.tableBody.SuspendLayout();
            this.panBottomLeft.SuspendLayout();
            this.panMsdsBoby.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSMSDSList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSMSDSList_Sheet1)).BeginInit();
            this.panSearch.SuspendLayout();
            this.panTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSProduct)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSProduct_Sheet1)).BeginInit();
            this.panBottomRight.SuspendLayout();
            this.panProductMsdsBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSProductMsds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSProductMsds_Sheet1)).BeginInit();
            this.panSiteProduct.SuspendLayout();
            this.SuspendLayout();
            // 
            // formTItle1
            // 
            this.formTItle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.formTItle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.formTItle1.Location = new System.Drawing.Point(0, 0);
            this.formTItle1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.formTItle1.Name = "formTItle1";
            this.formTItle1.Size = new System.Drawing.Size(1264, 35);
            this.formTItle1.TabIndex = 0;
            this.formTItle1.TitleText = "화학물질 MSDS 관리";
            // 
            // tableBody
            // 
            this.tableBody.ColumnCount = 2;
            this.tableBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 666F));
            this.tableBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableBody.Controls.Add(this.panBottomLeft, 0, 1);
            this.tableBody.Controls.Add(this.panTop, 0, 0);
            this.tableBody.Controls.Add(this.panBottomRight, 1, 1);
            this.tableBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableBody.Location = new System.Drawing.Point(0, 35);
            this.tableBody.Name = "tableBody";
            this.tableBody.RowCount = 2;
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 579F));
            this.tableBody.Size = new System.Drawing.Size(1264, 836);
            this.tableBody.TabIndex = 1;
            // 
            // panBottomLeft
            // 
            this.panBottomLeft.Controls.Add(this.panMsdsBoby);
            this.panBottomLeft.Controls.Add(this.contentTitle3);
            this.panBottomLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBottomLeft.Location = new System.Drawing.Point(3, 260);
            this.panBottomLeft.Name = "panBottomLeft";
            this.panBottomLeft.Size = new System.Drawing.Size(660, 573);
            this.panBottomLeft.TabIndex = 2;
            // 
            // panMsdsBoby
            // 
            this.panMsdsBoby.Controls.Add(this.SSMSDSList);
            this.panMsdsBoby.Controls.Add(this.panSearch);
            this.panMsdsBoby.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMsdsBoby.Location = new System.Drawing.Point(0, 38);
            this.panMsdsBoby.Name = "panMsdsBoby";
            this.panMsdsBoby.Size = new System.Drawing.Size(660, 535);
            this.panMsdsBoby.TabIndex = 3;
            // 
            // SSMSDSList
            // 
            this.SSMSDSList.AccessibleDescription = "";
            this.SSMSDSList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSMSDSList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSMSDSList.Location = new System.Drawing.Point(0, 48);
            this.SSMSDSList.Name = "SSMSDSList";
            this.SSMSDSList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSMSDSList_Sheet1});
            this.SSMSDSList.Size = new System.Drawing.Size(660, 487);
            this.SSMSDSList.TabIndex = 2;
            this.SSMSDSList.SetActiveViewport(0, -1, -1);
            // 
            // SSMSDSList_Sheet1
            // 
            this.SSMSDSList_Sheet1.Reset();
            this.SSMSDSList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSMSDSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSMSDSList_Sheet1.ColumnCount = 0;
            this.SSMSDSList_Sheet1.RowCount = 0;
            this.SSMSDSList_Sheet1.ActiveColumnIndex = -1;
            this.SSMSDSList_Sheet1.ActiveRowIndex = -1;
            this.SSMSDSList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMSDSList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSMSDSList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSMSDSList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMSDSList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMSDSList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSMSDSList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSMSDSList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMSDSList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSMSDSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panSearch
            // 
            this.panSearch.BackColor = System.Drawing.Color.White;
            this.panSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSearch.Controls.Add(this.RdoMsdsCasNo);
            this.panSearch.Controls.Add(this.RdoMsdsName);
            this.panSearch.Controls.Add(this.BtnSearchMsds);
            this.panSearch.Controls.Add(this.TxtSearchMsdsWord);
            this.panSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSearch.Location = new System.Drawing.Point(0, 0);
            this.panSearch.Name = "panSearch";
            this.panSearch.Size = new System.Drawing.Size(660, 48);
            this.panSearch.TabIndex = 3;
            // 
            // RdoMsdsCasNo
            // 
            this.RdoMsdsCasNo.AutoSize = true;
            this.RdoMsdsCasNo.Location = new System.Drawing.Point(121, 13);
            this.RdoMsdsCasNo.Name = "RdoMsdsCasNo";
            this.RdoMsdsCasNo.Size = new System.Drawing.Size(65, 21);
            this.RdoMsdsCasNo.TabIndex = 21;
            this.RdoMsdsCasNo.Text = "CasNo";
            this.RdoMsdsCasNo.UseVisualStyleBackColor = true;
            // 
            // RdoMsdsName
            // 
            this.RdoMsdsName.AutoSize = true;
            this.RdoMsdsName.Checked = true;
            this.RdoMsdsName.Location = new System.Drawing.Point(41, 13);
            this.RdoMsdsName.Name = "RdoMsdsName";
            this.RdoMsdsName.Size = new System.Drawing.Size(65, 21);
            this.RdoMsdsName.TabIndex = 20;
            this.RdoMsdsName.TabStop = true;
            this.RdoMsdsName.Text = "물질명";
            this.RdoMsdsName.UseVisualStyleBackColor = true;
            // 
            // BtnSearchMsds
            // 
            this.BtnSearchMsds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSearchMsds.Location = new System.Drawing.Point(568, 11);
            this.BtnSearchMsds.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearchMsds.Name = "BtnSearchMsds";
            this.BtnSearchMsds.Size = new System.Drawing.Size(75, 28);
            this.BtnSearchMsds.TabIndex = 19;
            this.BtnSearchMsds.Text = "검 색(&F)";
            this.BtnSearchMsds.UseVisualStyleBackColor = true;
            this.BtnSearchMsds.Click += new System.EventHandler(this.BtnSearchMsds_Click);
            // 
            // TxtSearchMsdsWord
            // 
            this.TxtSearchMsdsWord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtSearchMsdsWord.Location = new System.Drawing.Point(351, 13);
            this.TxtSearchMsdsWord.Name = "TxtSearchMsdsWord";
            this.TxtSearchMsdsWord.Size = new System.Drawing.Size(211, 25);
            this.TxtSearchMsdsWord.TabIndex = 18;
            // 
            // contentTitle3
            // 
            this.contentTitle3.Dock = System.Windows.Forms.DockStyle.Top;
            this.contentTitle3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.contentTitle3.Location = new System.Drawing.Point(0, 0);
            this.contentTitle3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.contentTitle3.Name = "contentTitle3";
            this.contentTitle3.Size = new System.Drawing.Size(660, 38);
            this.contentTitle3.TabIndex = 1;
            this.contentTitle3.TitleText = "② MSDS DB 검색";
            // 
            // panTop
            // 
            this.tableBody.SetColumnSpan(this.panTop, 2);
            this.panTop.Controls.Add(this.label1);
            this.panTop.Controls.Add(this.SSProduct);
            this.panTop.Controls.Add(this.BtnSaveProduct);
            this.panTop.Controls.Add(this.BtnAddProduct);
            this.panTop.Controls.Add(this.contentTitle1);
            this.panTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panTop.Location = new System.Drawing.Point(3, 3);
            this.panTop.Name = "panTop";
            this.panTop.Size = new System.Drawing.Size(1258, 251);
            this.panTop.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(119, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 17);
            this.label1.TabIndex = 12;
            this.label1.Text = "제품명 필수";
            // 
            // SSProduct
            // 
            this.SSProduct.AccessibleDescription = "";
            this.SSProduct.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSProduct.Location = new System.Drawing.Point(0, 38);
            this.SSProduct.Name = "SSProduct";
            this.SSProduct.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSProduct_Sheet1});
            this.SSProduct.Size = new System.Drawing.Size(1258, 213);
            this.SSProduct.TabIndex = 1;
            this.SSProduct.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSProduct_CellDoubleClick);
            this.SSProduct.SetActiveViewport(0, -1, -1);
            // 
            // SSProduct_Sheet1
            // 
            this.SSProduct_Sheet1.Reset();
            this.SSProduct_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSProduct_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSProduct_Sheet1.ColumnCount = 0;
            this.SSProduct_Sheet1.RowCount = 0;
            this.SSProduct_Sheet1.ActiveColumnIndex = -1;
            this.SSProduct_Sheet1.ActiveRowIndex = -1;
            this.SSProduct_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSProduct_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSProduct_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSProduct_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSProduct_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSProduct_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSProduct_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSProduct_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSProduct_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSProduct_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // BtnSaveProduct
            // 
            this.BtnSaveProduct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSaveProduct.Location = new System.Drawing.Point(1171, 2);
            this.BtnSaveProduct.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSaveProduct.Name = "BtnSaveProduct";
            this.BtnSaveProduct.Size = new System.Drawing.Size(75, 28);
            this.BtnSaveProduct.TabIndex = 11;
            this.BtnSaveProduct.Text = "저장(&S)";
            this.BtnSaveProduct.UseVisualStyleBackColor = true;
            this.BtnSaveProduct.Click += new System.EventHandler(this.BtnSaveProduct_Click);
            // 
            // BtnAddProduct
            // 
            this.BtnAddProduct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnAddProduct.Location = new System.Drawing.Point(1090, 2);
            this.BtnAddProduct.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnAddProduct.Name = "BtnAddProduct";
            this.BtnAddProduct.Size = new System.Drawing.Size(75, 28);
            this.BtnAddProduct.TabIndex = 10;
            this.BtnAddProduct.Text = "추가(&A)";
            this.BtnAddProduct.UseVisualStyleBackColor = true;
            this.BtnAddProduct.Click += new System.EventHandler(this.BtnAddProduct_Click);
            // 
            // contentTitle1
            // 
            this.contentTitle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.contentTitle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.contentTitle1.Location = new System.Drawing.Point(0, 0);
            this.contentTitle1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.contentTitle1.Name = "contentTitle1";
            this.contentTitle1.Size = new System.Drawing.Size(1258, 38);
            this.contentTitle1.TabIndex = 0;
            this.contentTitle1.TitleText = "① 제품 관리";
            // 
            // panBottomRight
            // 
            this.panBottomRight.Controls.Add(this.panProductMsdsBody);
            this.panBottomRight.Controls.Add(this.contentTitle2);
            this.panBottomRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBottomRight.Location = new System.Drawing.Point(669, 260);
            this.panBottomRight.Name = "panBottomRight";
            this.panBottomRight.Size = new System.Drawing.Size(592, 573);
            this.panBottomRight.TabIndex = 1;
            // 
            // panProductMsdsBody
            // 
            this.panProductMsdsBody.Controls.Add(this.SSProductMsds);
            this.panProductMsdsBody.Controls.Add(this.panSiteProduct);
            this.panProductMsdsBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panProductMsdsBody.Location = new System.Drawing.Point(0, 38);
            this.panProductMsdsBody.Name = "panProductMsdsBody";
            this.panProductMsdsBody.Size = new System.Drawing.Size(592, 535);
            this.panProductMsdsBody.TabIndex = 14;
            // 
            // SSProductMsds
            // 
            this.SSProductMsds.AccessibleDescription = "";
            this.SSProductMsds.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSProductMsds.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSProductMsds.Location = new System.Drawing.Point(0, 48);
            this.SSProductMsds.Name = "SSProductMsds";
            this.SSProductMsds.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSProductMsds_Sheet1});
            this.SSProductMsds.Size = new System.Drawing.Size(592, 487);
            this.SSProductMsds.TabIndex = 2;
            this.SSProductMsds.SetActiveViewport(0, -1, -1);
            // 
            // SSProductMsds_Sheet1
            // 
            this.SSProductMsds_Sheet1.Reset();
            this.SSProductMsds_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSProductMsds_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSProductMsds_Sheet1.ColumnCount = 0;
            this.SSProductMsds_Sheet1.RowCount = 0;
            this.SSProductMsds_Sheet1.ActiveColumnIndex = -1;
            this.SSProductMsds_Sheet1.ActiveRowIndex = -1;
            this.SSProductMsds_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSProductMsds_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSProductMsds_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSProductMsds_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSProductMsds_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSProductMsds_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSProductMsds_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSProductMsds_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSProductMsds_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSProductMsds_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panSiteProduct
            // 
            this.panSiteProduct.BackColor = System.Drawing.Color.White;
            this.panSiteProduct.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSiteProduct.Controls.Add(this.TxtProductName);
            this.panSiteProduct.Controls.Add(this.BtnSaveMsds);
            this.panSiteProduct.Controls.Add(this.LblProductName);
            this.panSiteProduct.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSiteProduct.Location = new System.Drawing.Point(0, 0);
            this.panSiteProduct.Name = "panSiteProduct";
            this.panSiteProduct.Size = new System.Drawing.Size(592, 48);
            this.panSiteProduct.TabIndex = 4;
            // 
            // TxtProductName
            // 
            this.TxtProductName.Location = new System.Drawing.Point(71, 10);
            this.TxtProductName.Name = "TxtProductName";
            this.TxtProductName.ReadOnly = true;
            this.TxtProductName.Size = new System.Drawing.Size(340, 25);
            this.TxtProductName.TabIndex = 19;
            // 
            // BtnSaveMsds
            // 
            this.BtnSaveMsds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSaveMsds.Location = new System.Drawing.Point(504, 9);
            this.BtnSaveMsds.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSaveMsds.Name = "BtnSaveMsds";
            this.BtnSaveMsds.Size = new System.Drawing.Size(75, 28);
            this.BtnSaveMsds.TabIndex = 13;
            this.BtnSaveMsds.Text = "저장(&S)";
            this.BtnSaveMsds.UseVisualStyleBackColor = true;
            this.BtnSaveMsds.Click += new System.EventHandler(this.BtnSaveMsds_Click);
            // 
            // LblProductName
            // 
            this.LblProductName.AutoSize = true;
            this.LblProductName.Location = new System.Drawing.Point(18, 13);
            this.LblProductName.Name = "LblProductName";
            this.LblProductName.Size = new System.Drawing.Size(47, 17);
            this.LblProductName.TabIndex = 0;
            this.LblProductName.Text = "제품명";
            // 
            // contentTitle2
            // 
            this.contentTitle2.Dock = System.Windows.Forms.DockStyle.Top;
            this.contentTitle2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.contentTitle2.Location = new System.Drawing.Point(0, 0);
            this.contentTitle2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.contentTitle2.Name = "contentTitle2";
            this.contentTitle2.Size = new System.Drawing.Size(592, 38);
            this.contentTitle2.TabIndex = 1;
            this.contentTitle2.TitleText = "③ 물질 관리";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(1174, 4);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 28);
            this.btnExit.TabIndex = 35;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // SiteMSDSForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 871);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.tableBody);
            this.Controls.Add(this.formTItle1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SiteMSDSForm";
            this.Text = "SiteMSDSForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SiteMSDSForm_FormClosing);
            this.Load += new System.EventHandler(this.SiteMSDSForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.tableBody.ResumeLayout(false);
            this.panBottomLeft.ResumeLayout(false);
            this.panMsdsBoby.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSMSDSList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSMSDSList_Sheet1)).EndInit();
            this.panSearch.ResumeLayout(false);
            this.panSearch.PerformLayout();
            this.panTop.ResumeLayout(false);
            this.panTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSProduct)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSProduct_Sheet1)).EndInit();
            this.panBottomRight.ResumeLayout(false);
            this.panProductMsdsBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSProductMsds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSProductMsds_Sheet1)).EndInit();
            this.panSiteProduct.ResumeLayout(false);
            this.panSiteProduct.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ComBase.Mvc.UserControls.FormTItle formTItle1;
        private System.Windows.Forms.TableLayoutPanel tableBody;
        private System.Windows.Forms.Panel panTop;
        private ComBase.Mvc.UserControls.ContentTitle contentTitle1;
        private System.Windows.Forms.Panel panBottomRight;
        private ComBase.Mvc.UserControls.ContentTitle contentTitle2;
        private FarPoint.Win.Spread.FpSpread SSProduct;
        private FarPoint.Win.Spread.SheetView SSProduct_Sheet1;
        private System.Windows.Forms.Button BtnSaveProduct;
        private System.Windows.Forms.Button BtnAddProduct;
        private System.Windows.Forms.Button BtnSaveMsds;
        private FarPoint.Win.Spread.FpSpread SSProductMsds;
        private FarPoint.Win.Spread.SheetView SSProductMsds_Sheet1;
        private System.Windows.Forms.Panel panBottomLeft;
        private FarPoint.Win.Spread.FpSpread SSMSDSList;
        private FarPoint.Win.Spread.SheetView SSMSDSList_Sheet1;
        private ComBase.Mvc.UserControls.ContentTitle contentTitle3;
        private System.Windows.Forms.Panel panMsdsBoby;
        private System.Windows.Forms.Panel panSearch;
        private System.Windows.Forms.RadioButton RdoMsdsCasNo;
        private System.Windows.Forms.RadioButton RdoMsdsName;
        private System.Windows.Forms.Button BtnSearchMsds;
        private System.Windows.Forms.TextBox TxtSearchMsdsWord;
        private System.Windows.Forms.Panel panProductMsdsBody;
        private System.Windows.Forms.Panel panSiteProduct;
        private System.Windows.Forms.TextBox TxtProductName;
        private System.Windows.Forms.Label LblProductName;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label1;
    }
}