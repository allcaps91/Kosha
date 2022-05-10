namespace HC_Core
{
    partial class MsdsForm
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
            this.panLeft = new System.Windows.Forms.Panel();
            this.panKoshaBody = new System.Windows.Forms.Panel();
            this.SSKoshaList = new FarPoint.Win.Spread.FpSpread();
            this.SSKoshaList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panKoshaSearch = new System.Windows.Forms.Panel();
            this.RdoCasNo = new System.Windows.Forms.RadioButton();
            this.RdoName = new System.Windows.Forms.RadioButton();
            this.BtnSearchKosha = new System.Windows.Forms.Button();
            this.TxtSearchKoshaWord = new System.Windows.Forms.TextBox();
            this.contentTitle1 = new ComBase.Mvc.UserControls.ContentTitle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panRightTop = new System.Windows.Forms.Panel();
            this.SSMSDSList = new FarPoint.Win.Spread.FpSpread();
            this.SSMSDSList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panMSDSSearch = new System.Windows.Forms.Panel();
            this.RdoMsdsCasNo = new System.Windows.Forms.RadioButton();
            this.RdoMsdsName = new System.Windows.Forms.RadioButton();
            this.BtnSearchMsds = new System.Windows.Forms.Button();
            this.TxtSearchMsdsWord = new System.Windows.Forms.TextBox();
            this.contentTitle3 = new ComBase.Mvc.UserControls.ContentTitle();
            this.panRightBottom = new System.Windows.Forms.Panel();
            this.panMsds = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.GhsPicture = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtChemId = new System.Windows.Forms.TextBox();
            this.BtnDeleteMsds = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtPSM_MATERIAL = new System.Windows.Forms.TextBox();
            this.TxtName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtPERMISSION_MATERIAL = new System.Windows.Forms.TextBox();
            this.TxtCasNo = new System.Windows.Forms.TextBox();
            this.TxtSTANDARD_MATERIAL = new System.Windows.Forms.TextBox();
            this.TxtSPECIALMANAGE_MATERIAL = new System.Windows.Forms.TextBox();
            this.TxtMANAGETARGET_MATERIAL = new System.Windows.Forms.TextBox();
            this.TxtSPECIALHEALTH_MATERIAL = new System.Windows.Forms.TextBox();
            this.TxtWEM_MATERIAL = new System.Windows.Forms.TextBox();
            this.ChkSPECIALMANAGE_MATERIAL = new System.Windows.Forms.CheckBox();
            this.ChkMANAGETARGET_MATERIAL = new System.Windows.Forms.CheckBox();
            this.ChkSPECIALHEALTH_MATERIAL = new System.Windows.Forms.CheckBox();
            this.ChkSTANDARD_MATERIAL = new System.Windows.Forms.CheckBox();
            this.ChkPERMISSION_MATERIAL = new System.Windows.Forms.CheckBox();
            this.ChkPSM_MATERIAL = new System.Windows.Forms.CheckBox();
            this.ChkWEM_MATERIAL = new System.Windows.Forms.CheckBox();
            this.ChkEXPOSURE_MATERIAL = new System.Windows.Forms.CheckBox();
            this.TxtEXPOSURE_MATERIAL = new System.Windows.Forms.TextBox();
            this.BtnSaveMsds = new System.Windows.Forms.Button();
            this.contentTitle2 = new ComBase.Mvc.UserControls.ContentTitle();
            this.btnExit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panLeft.SuspendLayout();
            this.panKoshaBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSKoshaList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSKoshaList_Sheet1)).BeginInit();
            this.panKoshaSearch.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panRightTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSMSDSList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSMSDSList_Sheet1)).BeginInit();
            this.panMSDSSearch.SuspendLayout();
            this.panRightBottom.SuspendLayout();
            this.panMsds.SuspendLayout();
            this.SuspendLayout();
            // 
            // formTItle1
            // 
            this.formTItle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.formTItle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.formTItle1.Location = new System.Drawing.Point(0, 0);
            this.formTItle1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.formTItle1.Name = "formTItle1";
            this.formTItle1.Size = new System.Drawing.Size(1274, 35);
            this.formTItle1.TabIndex = 0;
            this.formTItle1.TitleText = "MSDS(KOSHA)";
            // 
            // panLeft
            // 
            this.panLeft.Controls.Add(this.panKoshaBody);
            this.panLeft.Controls.Add(this.contentTitle1);
            this.panLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panLeft.Location = new System.Drawing.Point(3, 3);
            this.panLeft.Name = "panLeft";
            this.tableLayoutPanel1.SetRowSpan(this.panLeft, 2);
            this.panLeft.Size = new System.Drawing.Size(406, 830);
            this.panLeft.TabIndex = 1;
            // 
            // panKoshaBody
            // 
            this.panKoshaBody.BackColor = System.Drawing.Color.White;
            this.panKoshaBody.Controls.Add(this.SSKoshaList);
            this.panKoshaBody.Controls.Add(this.panKoshaSearch);
            this.panKoshaBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panKoshaBody.Location = new System.Drawing.Point(0, 38);
            this.panKoshaBody.Name = "panKoshaBody";
            this.panKoshaBody.Size = new System.Drawing.Size(406, 792);
            this.panKoshaBody.TabIndex = 2;
            // 
            // SSKoshaList
            // 
            this.SSKoshaList.AccessibleDescription = "";
            this.SSKoshaList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSKoshaList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSKoshaList.Location = new System.Drawing.Point(0, 49);
            this.SSKoshaList.Name = "SSKoshaList";
            this.SSKoshaList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSKoshaList_Sheet1});
            this.SSKoshaList.Size = new System.Drawing.Size(406, 743);
            this.SSKoshaList.TabIndex = 2;
            this.SSKoshaList.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSKoshaList_CellDoubleClick);
            this.SSKoshaList.SetActiveViewport(0, -1, -1);
            // 
            // SSKoshaList_Sheet1
            // 
            this.SSKoshaList_Sheet1.Reset();
            this.SSKoshaList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSKoshaList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSKoshaList_Sheet1.ColumnCount = 0;
            this.SSKoshaList_Sheet1.RowCount = 0;
            this.SSKoshaList_Sheet1.ActiveColumnIndex = -1;
            this.SSKoshaList_Sheet1.ActiveRowIndex = -1;
            this.SSKoshaList_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSKoshaList_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSKoshaList_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.SSKoshaList_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSKoshaList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSKoshaList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSKoshaList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSKoshaList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSKoshaList_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSKoshaList_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSKoshaList_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.SSKoshaList_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSKoshaList_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSKoshaList_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSKoshaList_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.SSKoshaList_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSKoshaList_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSKoshaList_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSKoshaList_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.SSKoshaList_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSKoshaList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSKoshaList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSKoshaList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSKoshaList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSKoshaList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSKoshaList_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSKoshaList_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSKoshaList_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSKoshaList_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSKoshaList_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSKoshaList_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSKoshaList_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.SSKoshaList_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSKoshaList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panKoshaSearch
            // 
            this.panKoshaSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panKoshaSearch.Controls.Add(this.RdoCasNo);
            this.panKoshaSearch.Controls.Add(this.RdoName);
            this.panKoshaSearch.Controls.Add(this.BtnSearchKosha);
            this.panKoshaSearch.Controls.Add(this.TxtSearchKoshaWord);
            this.panKoshaSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panKoshaSearch.Location = new System.Drawing.Point(0, 0);
            this.panKoshaSearch.Name = "panKoshaSearch";
            this.panKoshaSearch.Size = new System.Drawing.Size(406, 49);
            this.panKoshaSearch.TabIndex = 4;
            // 
            // RdoCasNo
            // 
            this.RdoCasNo.AutoSize = true;
            this.RdoCasNo.Location = new System.Drawing.Point(75, 10);
            this.RdoCasNo.Name = "RdoCasNo";
            this.RdoCasNo.Size = new System.Drawing.Size(65, 21);
            this.RdoCasNo.TabIndex = 13;
            this.RdoCasNo.Text = "CasNo";
            this.RdoCasNo.UseVisualStyleBackColor = true;
            // 
            // RdoName
            // 
            this.RdoName.AutoSize = true;
            this.RdoName.Checked = true;
            this.RdoName.Location = new System.Drawing.Point(8, 10);
            this.RdoName.Name = "RdoName";
            this.RdoName.Size = new System.Drawing.Size(65, 21);
            this.RdoName.TabIndex = 12;
            this.RdoName.TabStop = true;
            this.RdoName.Text = "물질명";
            this.RdoName.UseVisualStyleBackColor = true;
            // 
            // BtnSearchKosha
            // 
            this.BtnSearchKosha.Location = new System.Drawing.Point(316, 7);
            this.BtnSearchKosha.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearchKosha.Name = "BtnSearchKosha";
            this.BtnSearchKosha.Size = new System.Drawing.Size(75, 28);
            this.BtnSearchKosha.TabIndex = 10;
            this.BtnSearchKosha.Text = "검 색(&F)";
            this.BtnSearchKosha.UseVisualStyleBackColor = true;
            this.BtnSearchKosha.Click += new System.EventHandler(this.BtnSearchKosha_Click);
            // 
            // TxtSearchKoshaWord
            // 
            this.TxtSearchKoshaWord.Location = new System.Drawing.Point(148, 9);
            this.TxtSearchKoshaWord.Name = "TxtSearchKoshaWord";
            this.TxtSearchKoshaWord.Size = new System.Drawing.Size(162, 25);
            this.TxtSearchKoshaWord.TabIndex = 1;
            // 
            // contentTitle1
            // 
            this.contentTitle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.contentTitle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.contentTitle1.Location = new System.Drawing.Point(0, 0);
            this.contentTitle1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.contentTitle1.Name = "contentTitle1";
            this.contentTitle1.Size = new System.Drawing.Size(406, 38);
            this.contentTitle1.TabIndex = 0;
            this.contentTitle1.TitleText = "MSDS KOSHA 검색";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35759F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.6424F));
            this.tableLayoutPanel1.Controls.Add(this.panLeft, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panRightTop, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panRightBottom, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 35);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 49.2823F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.7177F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1274, 836);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panRightTop
            // 
            this.panRightTop.Controls.Add(this.SSMSDSList);
            this.panRightTop.Controls.Add(this.panMSDSSearch);
            this.panRightTop.Controls.Add(this.contentTitle3);
            this.panRightTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panRightTop.Location = new System.Drawing.Point(415, 3);
            this.panRightTop.Name = "panRightTop";
            this.panRightTop.Size = new System.Drawing.Size(856, 406);
            this.panRightTop.TabIndex = 3;
            // 
            // SSMSDSList
            // 
            this.SSMSDSList.AccessibleDescription = "";
            this.SSMSDSList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSMSDSList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSMSDSList.Location = new System.Drawing.Point(0, 87);
            this.SSMSDSList.Name = "SSMSDSList";
            this.SSMSDSList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSMSDSList_Sheet1});
            this.SSMSDSList.Size = new System.Drawing.Size(856, 319);
            this.SSMSDSList.TabIndex = 6;
            this.SSMSDSList.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSMSDSList_CellDoubleClick);
            // 
            // SSMSDSList_Sheet1
            // 
            this.SSMSDSList_Sheet1.Reset();
            this.SSMSDSList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSMSDSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSMSDSList_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMSDSList_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSMSDSList_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.SSMSDSList_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMSDSList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMSDSList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSMSDSList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSMSDSList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMSDSList_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMSDSList_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSMSDSList_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.SSMSDSList_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMSDSList_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMSDSList_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSMSDSList_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.SSMSDSList_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMSDSList_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMSDSList_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSMSDSList_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.SSMSDSList_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMSDSList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMSDSList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSMSDSList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSMSDSList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMSDSList_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMSDSList_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSMSDSList_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSMSDSList_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMSDSList_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMSDSList_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSMSDSList_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.SSMSDSList_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMSDSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panMSDSSearch
            // 
            this.panMSDSSearch.BackColor = System.Drawing.Color.White;
            this.panMSDSSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panMSDSSearch.Controls.Add(this.RdoMsdsCasNo);
            this.panMSDSSearch.Controls.Add(this.RdoMsdsName);
            this.panMSDSSearch.Controls.Add(this.BtnSearchMsds);
            this.panMSDSSearch.Controls.Add(this.TxtSearchMsdsWord);
            this.panMSDSSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panMSDSSearch.Location = new System.Drawing.Point(0, 38);
            this.panMSDSSearch.Name = "panMSDSSearch";
            this.panMSDSSearch.Size = new System.Drawing.Size(856, 49);
            this.panMSDSSearch.TabIndex = 7;
            // 
            // RdoMsdsCasNo
            // 
            this.RdoMsdsCasNo.AutoSize = true;
            this.RdoMsdsCasNo.Location = new System.Drawing.Point(93, 10);
            this.RdoMsdsCasNo.Name = "RdoMsdsCasNo";
            this.RdoMsdsCasNo.Size = new System.Drawing.Size(65, 21);
            this.RdoMsdsCasNo.TabIndex = 17;
            this.RdoMsdsCasNo.Text = "CasNo";
            this.RdoMsdsCasNo.UseVisualStyleBackColor = true;
            this.RdoMsdsCasNo.CheckedChanged += new System.EventHandler(this.RdoMsdsCasNo_CheckedChanged);
            // 
            // RdoMsdsName
            // 
            this.RdoMsdsName.AutoSize = true;
            this.RdoMsdsName.Checked = true;
            this.RdoMsdsName.Location = new System.Drawing.Point(13, 10);
            this.RdoMsdsName.Name = "RdoMsdsName";
            this.RdoMsdsName.Size = new System.Drawing.Size(65, 21);
            this.RdoMsdsName.TabIndex = 16;
            this.RdoMsdsName.TabStop = true;
            this.RdoMsdsName.Text = "물질명";
            this.RdoMsdsName.UseVisualStyleBackColor = true;
            this.RdoMsdsName.CheckedChanged += new System.EventHandler(this.RdoMsdsName_CheckedChanged);
            // 
            // BtnSearchMsds
            // 
            this.BtnSearchMsds.Location = new System.Drawing.Point(412, 9);
            this.BtnSearchMsds.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearchMsds.Name = "BtnSearchMsds";
            this.BtnSearchMsds.Size = new System.Drawing.Size(75, 28);
            this.BtnSearchMsds.TabIndex = 15;
            this.BtnSearchMsds.Text = "검 색(&F)";
            this.BtnSearchMsds.UseVisualStyleBackColor = true;
            this.BtnSearchMsds.Click += new System.EventHandler(this.BtnSearchMsds_Click);
            // 
            // TxtSearchMsdsWord
            // 
            this.TxtSearchMsdsWord.Location = new System.Drawing.Point(195, 10);
            this.TxtSearchMsdsWord.Name = "TxtSearchMsdsWord";
            this.TxtSearchMsdsWord.Size = new System.Drawing.Size(211, 25);
            this.TxtSearchMsdsWord.TabIndex = 14;
            this.TxtSearchMsdsWord.TextChanged += new System.EventHandler(this.TxtSearchMsdsWord_TextChanged);
            // 
            // contentTitle3
            // 
            this.contentTitle3.Dock = System.Windows.Forms.DockStyle.Top;
            this.contentTitle3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.contentTitle3.Location = new System.Drawing.Point(0, 0);
            this.contentTitle3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.contentTitle3.Name = "contentTitle3";
            this.contentTitle3.Size = new System.Drawing.Size(856, 38);
            this.contentTitle3.TabIndex = 5;
            this.contentTitle3.TitleText = "MSDS DB 목록";
            // 
            // panRightBottom
            // 
            this.panRightBottom.Controls.Add(this.panMsds);
            this.panRightBottom.Controls.Add(this.contentTitle2);
            this.panRightBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panRightBottom.Location = new System.Drawing.Point(415, 415);
            this.panRightBottom.Name = "panRightBottom";
            this.panRightBottom.Size = new System.Drawing.Size(856, 418);
            this.panRightBottom.TabIndex = 2;
            // 
            // panMsds
            // 
            this.panMsds.BackColor = System.Drawing.Color.White;
            this.panMsds.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panMsds.Controls.Add(this.label4);
            this.panMsds.Controls.Add(this.GhsPicture);
            this.panMsds.Controls.Add(this.label1);
            this.panMsds.Controls.Add(this.TxtChemId);
            this.panMsds.Controls.Add(this.BtnDeleteMsds);
            this.panMsds.Controls.Add(this.label2);
            this.panMsds.Controls.Add(this.TxtPSM_MATERIAL);
            this.panMsds.Controls.Add(this.TxtName);
            this.panMsds.Controls.Add(this.label3);
            this.panMsds.Controls.Add(this.TxtPERMISSION_MATERIAL);
            this.panMsds.Controls.Add(this.TxtCasNo);
            this.panMsds.Controls.Add(this.TxtSTANDARD_MATERIAL);
            this.panMsds.Controls.Add(this.TxtSPECIALMANAGE_MATERIAL);
            this.panMsds.Controls.Add(this.TxtMANAGETARGET_MATERIAL);
            this.panMsds.Controls.Add(this.TxtSPECIALHEALTH_MATERIAL);
            this.panMsds.Controls.Add(this.TxtWEM_MATERIAL);
            this.panMsds.Controls.Add(this.ChkSPECIALMANAGE_MATERIAL);
            this.panMsds.Controls.Add(this.ChkMANAGETARGET_MATERIAL);
            this.panMsds.Controls.Add(this.ChkSPECIALHEALTH_MATERIAL);
            this.panMsds.Controls.Add(this.ChkSTANDARD_MATERIAL);
            this.panMsds.Controls.Add(this.ChkPERMISSION_MATERIAL);
            this.panMsds.Controls.Add(this.ChkPSM_MATERIAL);
            this.panMsds.Controls.Add(this.ChkWEM_MATERIAL);
            this.panMsds.Controls.Add(this.ChkEXPOSURE_MATERIAL);
            this.panMsds.Controls.Add(this.TxtEXPOSURE_MATERIAL);
            this.panMsds.Controls.Add(this.BtnSaveMsds);
            this.panMsds.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMsds.Location = new System.Drawing.Point(0, 38);
            this.panMsds.Name = "panMsds";
            this.panMsds.Size = new System.Drawing.Size(856, 380);
            this.panMsds.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(87, 287);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 17);
            this.label4.TabIndex = 51;
            this.label4.Text = "그림문자";
            // 
            // GhsPicture
            // 
            this.GhsPicture.Location = new System.Drawing.Point(91, 309);
            this.GhsPicture.Name = "GhsPicture";
            this.GhsPicture.Size = new System.Drawing.Size(651, 52);
            this.GhsPicture.TabIndex = 50;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(88, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 17);
            this.label1.TabIndex = 17;
            this.label1.Text = "ChemId";
            // 
            // TxtChemId
            // 
            this.TxtChemId.Location = new System.Drawing.Point(157, 18);
            this.TxtChemId.Name = "TxtChemId";
            this.TxtChemId.ReadOnly = true;
            this.TxtChemId.Size = new System.Drawing.Size(105, 25);
            this.TxtChemId.TabIndex = 15;
            // 
            // BtnDeleteMsds
            // 
            this.BtnDeleteMsds.Location = new System.Drawing.Point(586, 12);
            this.BtnDeleteMsds.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnDeleteMsds.Name = "BtnDeleteMsds";
            this.BtnDeleteMsds.Size = new System.Drawing.Size(75, 28);
            this.BtnDeleteMsds.TabIndex = 46;
            this.BtnDeleteMsds.Text = "삭제(&S)";
            this.BtnDeleteMsds.UseVisualStyleBackColor = true;
            this.BtnDeleteMsds.Click += new System.EventHandler(this.BtnDeleteMsds_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(88, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 17);
            this.label2.TabIndex = 20;
            this.label2.Text = "물질명";
            // 
            // TxtPSM_MATERIAL
            // 
            this.TxtPSM_MATERIAL.Location = new System.Drawing.Point(621, 251);
            this.TxtPSM_MATERIAL.Name = "TxtPSM_MATERIAL";
            this.TxtPSM_MATERIAL.Size = new System.Drawing.Size(121, 25);
            this.TxtPSM_MATERIAL.TabIndex = 45;
            // 
            // TxtName
            // 
            this.TxtName.Location = new System.Drawing.Point(157, 61);
            this.TxtName.Name = "TxtName";
            this.TxtName.Size = new System.Drawing.Size(297, 25);
            this.TxtName.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(294, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 17);
            this.label3.TabIndex = 21;
            this.label3.Text = "CasNo";
            // 
            // TxtPERMISSION_MATERIAL
            // 
            this.TxtPERMISSION_MATERIAL.Location = new System.Drawing.Point(249, 255);
            this.TxtPERMISSION_MATERIAL.Name = "TxtPERMISSION_MATERIAL";
            this.TxtPERMISSION_MATERIAL.Size = new System.Drawing.Size(115, 25);
            this.TxtPERMISSION_MATERIAL.TabIndex = 44;
            // 
            // TxtCasNo
            // 
            this.TxtCasNo.Location = new System.Drawing.Point(347, 18);
            this.TxtCasNo.Name = "TxtCasNo";
            this.TxtCasNo.ReadOnly = true;
            this.TxtCasNo.Size = new System.Drawing.Size(107, 25);
            this.TxtCasNo.TabIndex = 19;
            // 
            // TxtSTANDARD_MATERIAL
            // 
            this.TxtSTANDARD_MATERIAL.Location = new System.Drawing.Point(621, 210);
            this.TxtSTANDARD_MATERIAL.Name = "TxtSTANDARD_MATERIAL";
            this.TxtSTANDARD_MATERIAL.Size = new System.Drawing.Size(121, 25);
            this.TxtSTANDARD_MATERIAL.TabIndex = 43;
            // 
            // TxtSPECIALMANAGE_MATERIAL
            // 
            this.TxtSPECIALMANAGE_MATERIAL.Location = new System.Drawing.Point(249, 206);
            this.TxtSPECIALMANAGE_MATERIAL.Name = "TxtSPECIALMANAGE_MATERIAL";
            this.TxtSPECIALMANAGE_MATERIAL.Size = new System.Drawing.Size(115, 25);
            this.TxtSPECIALMANAGE_MATERIAL.TabIndex = 42;
            // 
            // TxtMANAGETARGET_MATERIAL
            // 
            this.TxtMANAGETARGET_MATERIAL.Location = new System.Drawing.Point(621, 167);
            this.TxtMANAGETARGET_MATERIAL.Name = "TxtMANAGETARGET_MATERIAL";
            this.TxtMANAGETARGET_MATERIAL.Size = new System.Drawing.Size(121, 25);
            this.TxtMANAGETARGET_MATERIAL.TabIndex = 41;
            // 
            // TxtSPECIALHEALTH_MATERIAL
            // 
            this.TxtSPECIALHEALTH_MATERIAL.Location = new System.Drawing.Point(249, 167);
            this.TxtSPECIALHEALTH_MATERIAL.Name = "TxtSPECIALHEALTH_MATERIAL";
            this.TxtSPECIALHEALTH_MATERIAL.Size = new System.Drawing.Size(115, 25);
            this.TxtSPECIALHEALTH_MATERIAL.TabIndex = 40;
            // 
            // TxtWEM_MATERIAL
            // 
            this.TxtWEM_MATERIAL.Location = new System.Drawing.Point(621, 122);
            this.TxtWEM_MATERIAL.Name = "TxtWEM_MATERIAL";
            this.TxtWEM_MATERIAL.Size = new System.Drawing.Size(121, 25);
            this.TxtWEM_MATERIAL.TabIndex = 39;
            // 
            // ChkSPECIALMANAGE_MATERIAL
            // 
            this.ChkSPECIALMANAGE_MATERIAL.AutoSize = true;
            this.ChkSPECIALMANAGE_MATERIAL.Location = new System.Drawing.Point(91, 208);
            this.ChkSPECIALMANAGE_MATERIAL.Name = "ChkSPECIALMANAGE_MATERIAL";
            this.ChkSPECIALMANAGE_MATERIAL.Size = new System.Drawing.Size(105, 21);
            this.ChkSPECIALMANAGE_MATERIAL.TabIndex = 38;
            this.ChkSPECIALMANAGE_MATERIAL.Text = "특별관리물질";
            this.ChkSPECIALMANAGE_MATERIAL.UseVisualStyleBackColor = true;
            // 
            // ChkMANAGETARGET_MATERIAL
            // 
            this.ChkMANAGETARGET_MATERIAL.AutoSize = true;
            this.ChkMANAGETARGET_MATERIAL.Location = new System.Drawing.Point(458, 167);
            this.ChkMANAGETARGET_MATERIAL.Name = "ChkMANAGETARGET_MATERIAL";
            this.ChkMANAGETARGET_MATERIAL.Size = new System.Drawing.Size(131, 21);
            this.ChkMANAGETARGET_MATERIAL.TabIndex = 37;
            this.ChkMANAGETARGET_MATERIAL.Text = "관리대상유해물질";
            this.ChkMANAGETARGET_MATERIAL.UseVisualStyleBackColor = true;
            // 
            // ChkSPECIALHEALTH_MATERIAL
            // 
            this.ChkSPECIALHEALTH_MATERIAL.AutoSize = true;
            this.ChkSPECIALHEALTH_MATERIAL.Location = new System.Drawing.Point(91, 169);
            this.ChkSPECIALHEALTH_MATERIAL.Name = "ChkSPECIALHEALTH_MATERIAL";
            this.ChkSPECIALHEALTH_MATERIAL.Size = new System.Drawing.Size(157, 21);
            this.ChkSPECIALHEALTH_MATERIAL.TabIndex = 36;
            this.ChkSPECIALHEALTH_MATERIAL.Text = "특수건강진단대상물질";
            this.ChkSPECIALHEALTH_MATERIAL.UseVisualStyleBackColor = true;
            // 
            // ChkSTANDARD_MATERIAL
            // 
            this.ChkSTANDARD_MATERIAL.AutoSize = true;
            this.ChkSTANDARD_MATERIAL.Location = new System.Drawing.Point(458, 210);
            this.ChkSTANDARD_MATERIAL.Name = "ChkSTANDARD_MATERIAL";
            this.ChkSTANDARD_MATERIAL.Size = new System.Drawing.Size(131, 21);
            this.ChkSTANDARD_MATERIAL.TabIndex = 35;
            this.ChkSTANDARD_MATERIAL.Text = "허용기준설정물질";
            this.ChkSTANDARD_MATERIAL.UseVisualStyleBackColor = true;
            // 
            // ChkPERMISSION_MATERIAL
            // 
            this.ChkPERMISSION_MATERIAL.AutoSize = true;
            this.ChkPERMISSION_MATERIAL.Location = new System.Drawing.Point(91, 257);
            this.ChkPERMISSION_MATERIAL.Name = "ChkPERMISSION_MATERIAL";
            this.ChkPERMISSION_MATERIAL.Size = new System.Drawing.Size(131, 21);
            this.ChkPERMISSION_MATERIAL.TabIndex = 34;
            this.ChkPERMISSION_MATERIAL.Text = "허가대상유해물질";
            this.ChkPERMISSION_MATERIAL.UseVisualStyleBackColor = true;
            // 
            // ChkPSM_MATERIAL
            // 
            this.ChkPSM_MATERIAL.AutoSize = true;
            this.ChkPSM_MATERIAL.Location = new System.Drawing.Point(458, 255);
            this.ChkPSM_MATERIAL.Name = "ChkPSM_MATERIAL";
            this.ChkPSM_MATERIAL.Size = new System.Drawing.Size(131, 21);
            this.ChkPSM_MATERIAL.TabIndex = 33;
            this.ChkPSM_MATERIAL.Text = "PSM제출대상물질";
            this.ChkPSM_MATERIAL.UseVisualStyleBackColor = true;
            // 
            // ChkWEM_MATERIAL
            // 
            this.ChkWEM_MATERIAL.AutoSize = true;
            this.ChkWEM_MATERIAL.Location = new System.Drawing.Point(458, 124);
            this.ChkWEM_MATERIAL.Name = "ChkWEM_MATERIAL";
            this.ChkWEM_MATERIAL.Size = new System.Drawing.Size(157, 21);
            this.ChkWEM_MATERIAL.TabIndex = 32;
            this.ChkWEM_MATERIAL.Text = "작업환경측정대상물질";
            this.ChkWEM_MATERIAL.UseVisualStyleBackColor = true;
            // 
            // ChkEXPOSURE_MATERIAL
            // 
            this.ChkEXPOSURE_MATERIAL.AutoSize = true;
            this.ChkEXPOSURE_MATERIAL.Location = new System.Drawing.Point(91, 126);
            this.ChkEXPOSURE_MATERIAL.Name = "ChkEXPOSURE_MATERIAL";
            this.ChkEXPOSURE_MATERIAL.Size = new System.Drawing.Size(131, 21);
            this.ChkEXPOSURE_MATERIAL.TabIndex = 31;
            this.ChkEXPOSURE_MATERIAL.Text = "노출기준설정물질";
            this.ChkEXPOSURE_MATERIAL.UseVisualStyleBackColor = true;
            // 
            // TxtEXPOSURE_MATERIAL
            // 
            this.TxtEXPOSURE_MATERIAL.Location = new System.Drawing.Point(249, 124);
            this.TxtEXPOSURE_MATERIAL.Name = "TxtEXPOSURE_MATERIAL";
            this.TxtEXPOSURE_MATERIAL.Size = new System.Drawing.Size(115, 25);
            this.TxtEXPOSURE_MATERIAL.TabIndex = 30;
            // 
            // BtnSaveMsds
            // 
            this.BtnSaveMsds.Location = new System.Drawing.Point(667, 12);
            this.BtnSaveMsds.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSaveMsds.Name = "BtnSaveMsds";
            this.BtnSaveMsds.Size = new System.Drawing.Size(75, 28);
            this.BtnSaveMsds.TabIndex = 16;
            this.BtnSaveMsds.Text = "저장(&S)";
            this.BtnSaveMsds.UseVisualStyleBackColor = true;
            this.BtnSaveMsds.Click += new System.EventHandler(this.BtnSaveMsds_Click);
            // 
            // contentTitle2
            // 
            this.contentTitle2.Dock = System.Windows.Forms.DockStyle.Top;
            this.contentTitle2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.contentTitle2.Location = new System.Drawing.Point(0, 0);
            this.contentTitle2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.contentTitle2.Name = "contentTitle2";
            this.contentTitle2.Size = new System.Drawing.Size(856, 38);
            this.contentTitle2.TabIndex = 0;
            this.contentTitle2.TitleText = "MSDS DB 등록";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(1194, 4);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 28);
            this.btnExit.TabIndex = 30;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click_1);
            // 
            // MsdsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1274, 871);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.formTItle1);
            this.Name = "MsdsForm";
            this.Text = "Msds 기초 코드";
            this.Load += new System.EventHandler(this.MsdsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panLeft.ResumeLayout(false);
            this.panKoshaBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSKoshaList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSKoshaList_Sheet1)).EndInit();
            this.panKoshaSearch.ResumeLayout(false);
            this.panKoshaSearch.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panRightTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSMSDSList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSMSDSList_Sheet1)).EndInit();
            this.panMSDSSearch.ResumeLayout(false);
            this.panMSDSSearch.PerformLayout();
            this.panRightBottom.ResumeLayout(false);
            this.panMsds.ResumeLayout(false);
            this.panMsds.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ComBase.Mvc.UserControls.FormTItle formTItle1;
        private System.Windows.Forms.Panel panLeft;
        private System.Windows.Forms.Panel panKoshaBody;
        private FarPoint.Win.Spread.FpSpread SSKoshaList;
        private FarPoint.Win.Spread.SheetView SSKoshaList_Sheet1;
        private System.Windows.Forms.Panel panKoshaSearch;
        private System.Windows.Forms.TextBox TxtSearchKoshaWord;
        private ComBase.Mvc.UserControls.ContentTitle contentTitle1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panRightBottom;
        private System.Windows.Forms.Panel panMsds;
        private ComBase.Mvc.UserControls.ContentTitle contentTitle2;
        private System.Windows.Forms.Panel panRightTop;
        private FarPoint.Win.Spread.FpSpread SSMSDSList;
        private FarPoint.Win.Spread.SheetView SSMSDSList_Sheet1;
        private System.Windows.Forms.Panel panMSDSSearch;
        private ComBase.Mvc.UserControls.ContentTitle contentTitle3;
        private System.Windows.Forms.Button BtnSearchKosha;
        private System.Windows.Forms.RadioButton RdoCasNo;
        private System.Windows.Forms.RadioButton RdoName;
        private System.Windows.Forms.RadioButton RdoMsdsCasNo;
        private System.Windows.Forms.RadioButton RdoMsdsName;
        private System.Windows.Forms.Button BtnSearchMsds;
        private System.Windows.Forms.TextBox TxtSearchMsdsWord;
        private System.Windows.Forms.TextBox TxtEXPOSURE_MATERIAL;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtCasNo;
        private System.Windows.Forms.TextBox TxtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnSaveMsds;
        private System.Windows.Forms.TextBox TxtChemId;
        private System.Windows.Forms.TextBox TxtPSM_MATERIAL;
        private System.Windows.Forms.TextBox TxtPERMISSION_MATERIAL;
        private System.Windows.Forms.TextBox TxtSTANDARD_MATERIAL;
        private System.Windows.Forms.TextBox TxtSPECIALMANAGE_MATERIAL;
        private System.Windows.Forms.TextBox TxtMANAGETARGET_MATERIAL;
        private System.Windows.Forms.TextBox TxtSPECIALHEALTH_MATERIAL;
        private System.Windows.Forms.TextBox TxtWEM_MATERIAL;
        private System.Windows.Forms.CheckBox ChkSPECIALMANAGE_MATERIAL;
        private System.Windows.Forms.CheckBox ChkMANAGETARGET_MATERIAL;
        private System.Windows.Forms.CheckBox ChkSPECIALHEALTH_MATERIAL;
        private System.Windows.Forms.CheckBox ChkSTANDARD_MATERIAL;
        private System.Windows.Forms.CheckBox ChkPERMISSION_MATERIAL;
        private System.Windows.Forms.CheckBox ChkPSM_MATERIAL;
        private System.Windows.Forms.CheckBox ChkWEM_MATERIAL;
        private System.Windows.Forms.CheckBox ChkEXPOSURE_MATERIAL;
        private System.Windows.Forms.Button BtnDeleteMsds;
        private System.Windows.Forms.FlowLayoutPanel GhsPicture;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnExit;
    }
}