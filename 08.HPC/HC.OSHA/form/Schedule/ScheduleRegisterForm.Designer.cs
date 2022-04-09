namespace HC_OSHA
{
    partial class ScheduleRegisterForm
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
            HC.OSHA.Model.HC_OSHA_SITE_MODEL hC_OSHA_SITE_MODEL1 = new HC.OSHA.Model.HC_OSHA_SITE_MODEL();
            HC.OSHA.Model.HC_ESTIMATE_MODEL hC_ESTIMATE_MODEL1 = new HC.OSHA.Model.HC_ESTIMATE_MODEL();
            HC.OSHA.Model.HC_ESTIMATE_MODEL hC_ESTIMATE_MODEL2 = new HC.OSHA.Model.HC_ESTIMATE_MODEL();
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("ko-KR", false);
            this.formTItle1 = new ComBase.Mvc.UserControls.FormTItle();
            this.tableBody = new System.Windows.Forms.TableLayoutPanel();
            this.panLeftTop = new System.Windows.Forms.Panel();
            this.panSiteBody = new System.Windows.Forms.Panel();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.TabSite = new System.Windows.Forms.TabPage();
            this.OshaSiteLastTree = new HC_OSHA.OshaSiteLastTree();
            this.oshaSiteEstimateList1 = new HC_OSHA.OshaSiteEstimateList();
            this.horizSpace1 = new ComBase.Mvc.UserControls.HorizSpace();
            this.oshaSiteEstimateList2 = new HC_OSHA.OshaSiteEstimateList();
            this.TabUnvisit = new System.Windows.Forms.TabPage();
            this.SSUnVisit = new FarPoint.Win.Spread.FpSpread();
            this.SSUnVisit_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panSearchUnvisit = new System.Windows.Forms.Panel();
            this.label35 = new System.Windows.Forms.Label();
            this.TxtSearchUnvisitSiteIdOrName = new System.Windows.Forms.TextBox();
            this.LblUnvisitCount = new System.Windows.Forms.Label();
            this.DtpSearchUnVisitStartDate = new System.Windows.Forms.DateTimePicker();
            this.CboSearchUnVisitUserId = new System.Windows.Forms.ComboBox();
            this.DtpSearchUnVisitEndDate = new System.Windows.Forms.DateTimePicker();
            this.label18 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.BtnSearchUnVisit = new System.Windows.Forms.Button();
            this.TabVisit = new System.Windows.Forms.TabPage();
            this.SSVisit = new FarPoint.Win.Spread.FpSpread();
            this.SSVisit_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panSearchVisit = new System.Windows.Forms.Panel();
            this.label34 = new System.Windows.Forms.Label();
            this.TxtSearchVisitSiteIdOrName = new System.Windows.Forms.TextBox();
            this.LblVisitCount = new System.Windows.Forms.Label();
            this.CboSearchVisitUserId = new System.Windows.Forms.ComboBox();
            this.DtpSearchVisitStartDate = new System.Windows.Forms.DateTimePicker();
            this.DtpSearchVisitEndDate = new System.Windows.Forms.DateTimePicker();
            this.label29 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.BtnSearchVisit = new System.Windows.Forms.Button();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.SSPreCharge = new FarPoint.Win.Spread.FpSpread();
            this.SSPreCharge_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.CboSearchPreVisitUserId = new System.Windows.Forms.ComboBox();
            this.DtpSearchPreVisitStartDate = new System.Windows.Forms.DateTimePicker();
            this.DtpSearchPreVisitEndDate = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.BtnSearchPreVisit = new System.Windows.Forms.Button();
            this.contentTitle2 = new ComBase.Mvc.UserControls.ContentTitle();
            this.panRight = new System.Windows.Forms.Panel();
            this.BtnShowCalendar = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.SSScheduleList = new FarPoint.Win.Spread.FpSpread();
            this.SSScheduleList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExcel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.CboMonth = new System.Windows.Forms.ComboBox();
            this.ChkSearchSchedule = new System.Windows.Forms.CheckBox();
            this.BtnPrintSchedule = new System.Windows.Forms.Button();
            this.label24 = new System.Windows.Forms.Label();
            this.CboSearchScheduleVisitUserId = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CboSearchScheduleVisitUserId2 = new System.Windows.Forms.ComboBox();
            this.BtnSearchSchedule = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.PanPrice = new System.Windows.Forms.Panel();
            this.ChkIsPreCharge = new System.Windows.Forms.CheckBox();
            this.NumChargePrice = new System.Windows.Forms.NumericUpDown();
            this.label33 = new System.Windows.Forms.Label();
            this.ChkIsFix = new System.Windows.Forms.CheckBox();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.SSVisitPriceList = new FarPoint.Win.Spread.FpSpread();
            this.SSVisitPriceList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.NumVisitTOTALPRICE = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.NumVisitUNITTOALPRICE = new System.Windows.Forms.NumericUpDown();
            this.label25 = new System.Windows.Forms.Label();
            this.ChkISKUKGO = new System.Windows.Forms.CheckBox();
            this.ChkISFEE = new System.Windows.Forms.CheckBox();
            this.NumVisitUNITPRICE = new System.Windows.Forms.NumericUpDown();
            this.label23 = new System.Windows.Forms.Label();
            this.NumVisitWORKERCOUNT = new System.Windows.Forms.NumericUpDown();
            this.panVisit = new System.Windows.Forms.Panel();
            this.txtEndTime = new System.Windows.Forms.TextBox();
            this.txtStartTime = new System.Windows.Forms.TextBox();
            this.BtnGetVisit = new System.Windows.Forms.Button();
            this.BtnSaveVisit = new System.Windows.Forms.Button();
            this.BtnVisitDelete = new System.Windows.Forms.Button();
            this.TxtTakeHourAndMinuteText = new System.Windows.Forms.TextBox();
            this.contentTitle3 = new ComBase.Mvc.UserControls.ContentTitle();
            this.label15 = new System.Windows.Forms.Label();
            this.TxtVisitREMARK = new System.Windows.Forms.TextBox();
            this.CboVISITTYPE = new System.Windows.Forms.ComboBox();
            this.label30 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.CboVISITDOCTOR = new System.Windows.Forms.ComboBox();
            this.label26 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.CboVISITUSER = new System.Windows.Forms.ComboBox();
            this.TxtTakeHourAndMinute = new System.Windows.Forms.TextBox();
            this.DtpVISITDATETIME = new System.Windows.Forms.DateTimePicker();
            this.label17 = new System.Windows.Forms.Label();
            this.TabEduPage = new System.Windows.Forms.TabPage();
            this.TabCommitteePage = new System.Windows.Forms.TabPage();
            this.TabInformation = new System.Windows.Forms.TabPage();
            this.TabAccident = new System.Windows.Forms.TabPage();
            this.TabInOut = new System.Windows.Forms.TabPage();
            this.TabReceipt = new System.Windows.Forms.TabPage();
            this.BtnSaveSchedule = new System.Windows.Forms.Button();
            this.BtnDeleteSchedule = new System.Windows.Forms.Button();
            this.panSchedule = new System.Windows.Forms.Panel();
            this.TxtVISITPLACE = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtVISITTIME = new System.Windows.Forms.TextBox();
            this.ChkDoctor = new System.Windows.Forms.CheckBox();
            this.LblSiteName = new System.Windows.Forms.Label();
            this.TxtMODIFIEDUSER = new System.Windows.Forms.TextBox();
            this.TxtMODIFIED = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.TxtREMARK = new System.Windows.Forms.TextBox();
            this.CboVISITUSERID = new System.Windows.Forms.ComboBox();
            this.CboVISITMANAGERID = new System.Windows.Forms.ComboBox();
            this.DtpVISITRESERVEDATE = new System.Windows.Forms.DateTimePicker();
            this.contentTitle1 = new ComBase.Mvc.UserControls.ContentTitle();
            this.BtnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.tableBody.SuspendLayout();
            this.panLeftTop.SuspendLayout();
            this.panSiteBody.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.TabSite.SuspendLayout();
            this.TabUnvisit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSUnVisit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSUnVisit_Sheet1)).BeginInit();
            this.panSearchUnvisit.SuspendLayout();
            this.TabVisit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSVisit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSVisit_Sheet1)).BeginInit();
            this.panSearchVisit.SuspendLayout();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSPreCharge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSPreCharge_Sheet1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panRight.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSScheduleList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSScheduleList_Sheet1)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.PanPrice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumChargePrice)).BeginInit();
            this.tabControl3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSVisitPriceList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSVisitPriceList_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumVisitTOTALPRICE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumVisitUNITTOALPRICE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumVisitUNITPRICE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumVisitWORKERCOUNT)).BeginInit();
            this.panVisit.SuspendLayout();
            this.panSchedule.SuspendLayout();
            this.SuspendLayout();
            // 
            // formTItle1
            // 
            this.formTItle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.formTItle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.formTItle1.Location = new System.Drawing.Point(0, 0);
            this.formTItle1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.formTItle1.Name = "formTItle1";
            this.formTItle1.Size = new System.Drawing.Size(1516, 35);
            this.formTItle1.TabIndex = 0;
            this.formTItle1.TitleText = "방문관리";
            // 
            // tableBody
            // 
            this.tableBody.ColumnCount = 2;
            this.tableBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 424F));
            this.tableBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableBody.Controls.Add(this.panLeftTop, 0, 0);
            this.tableBody.Controls.Add(this.panRight, 1, 0);
            this.tableBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableBody.Location = new System.Drawing.Point(0, 35);
            this.tableBody.Name = "tableBody";
            this.tableBody.RowCount = 1;
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 836F));
            this.tableBody.Size = new System.Drawing.Size(1516, 836);
            this.tableBody.TabIndex = 1;
            // 
            // panLeftTop
            // 
            this.panLeftTop.Controls.Add(this.panSiteBody);
            this.panLeftTop.Controls.Add(this.contentTitle2);
            this.panLeftTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panLeftTop.Location = new System.Drawing.Point(3, 3);
            this.panLeftTop.Name = "panLeftTop";
            this.panLeftTop.Size = new System.Drawing.Size(418, 830);
            this.panLeftTop.TabIndex = 4;
            // 
            // panSiteBody
            // 
            this.panSiteBody.Controls.Add(this.tabControl2);
            this.panSiteBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panSiteBody.Location = new System.Drawing.Point(0, 38);
            this.panSiteBody.Name = "panSiteBody";
            this.panSiteBody.Size = new System.Drawing.Size(418, 792);
            this.panSiteBody.TabIndex = 1;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.TabSite);
            this.tabControl2.Controls.Add(this.TabUnvisit);
            this.tabControl2.Controls.Add(this.TabVisit);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(418, 792);
            this.tabControl2.TabIndex = 5;
            this.tabControl2.SelectedIndexChanged += new System.EventHandler(this.TabControl2_SelectedIndexChanged);
            // 
            // TabSite
            // 
            this.TabSite.Controls.Add(this.OshaSiteLastTree);
            this.TabSite.Controls.Add(this.oshaSiteEstimateList1);
            this.TabSite.Controls.Add(this.horizSpace1);
            this.TabSite.Controls.Add(this.oshaSiteEstimateList2);
            this.TabSite.Location = new System.Drawing.Point(4, 26);
            this.TabSite.Name = "TabSite";
            this.TabSite.Padding = new System.Windows.Forms.Padding(3);
            this.TabSite.Size = new System.Drawing.Size(410, 762);
            this.TabSite.TabIndex = 0;
            this.TabSite.Text = "사업장";
            this.TabSite.UseVisualStyleBackColor = true;
            // 
            // OshaSiteLastTree
            // 
            this.OshaSiteLastTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OshaSiteLastTree.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            hC_OSHA_SITE_MODEL1.ADDRESS = null;
            hC_OSHA_SITE_MODEL1.BIZCREATEDATE = null;
            hC_OSHA_SITE_MODEL1.BIZJIDOWON = null;
            hC_OSHA_SITE_MODEL1.BIZJONG = null;
            hC_OSHA_SITE_MODEL1.BIZKIHO = null;
            hC_OSHA_SITE_MODEL1.BIZNUMBER = null;
            hC_OSHA_SITE_MODEL1.BIZTYPE = null;
            hC_OSHA_SITE_MODEL1.CEONAME = null;
            hC_OSHA_SITE_MODEL1.ComboDisplay = null;
            hC_OSHA_SITE_MODEL1.EMAIL = null;
            hC_OSHA_SITE_MODEL1.FAX = null;
            hC_OSHA_SITE_MODEL1.HASCHILD = null;
            hC_OSHA_SITE_MODEL1.ID = ((long)(0));
            hC_OSHA_SITE_MODEL1.INDUSTRIALNUMBER = null;
            hC_OSHA_SITE_MODEL1.INSURANCE = null;
            hC_OSHA_SITE_MODEL1.ISACTIVE = null;
            hC_OSHA_SITE_MODEL1.LABOR = null;
            hC_OSHA_SITE_MODEL1.LASTMODIFIED = null;
            hC_OSHA_SITE_MODEL1.MANAGEDOCTORCOUNT = ((long)(0));
            hC_OSHA_SITE_MODEL1.MANAGEDOCTORSTARTDATE = null;
            hC_OSHA_SITE_MODEL1.MANAGEENGINEERCOUNT = ((long)(0));
            hC_OSHA_SITE_MODEL1.MANAGEENGINEERSTARTDATE = null;
            hC_OSHA_SITE_MODEL1.MANAGENURSECOUNT = ((long)(0));
            hC_OSHA_SITE_MODEL1.MANAGENURSESTARTDATE = null;
            hC_OSHA_SITE_MODEL1.NAME = null;
            hC_OSHA_SITE_MODEL1.PARENTSITE_ID = ((long)(0));
            hC_OSHA_SITE_MODEL1.PARENTSITE_NAME = null;
            hC_OSHA_SITE_MODEL1.RowStatus = ComBase.Mvc.RowStatus.None;
            hC_OSHA_SITE_MODEL1.SITE_ID = ((long)(0));
            hC_OSHA_SITE_MODEL1.TEL = null;
            hC_OSHA_SITE_MODEL1.VISITDAY = ((long)(0));
            hC_OSHA_SITE_MODEL1.VISITWEEK = ((long)(0));
            hC_OSHA_SITE_MODEL1.WORKERTOTALCOUNT = ((long)(0));
            hC_OSHA_SITE_MODEL1.zTemp1 = null;
            hC_OSHA_SITE_MODEL1.zTemp10 = null;
            hC_OSHA_SITE_MODEL1.zTemp11 = null;
            hC_OSHA_SITE_MODEL1.zTemp12 = null;
            hC_OSHA_SITE_MODEL1.zTemp13 = null;
            hC_OSHA_SITE_MODEL1.zTemp14 = null;
            hC_OSHA_SITE_MODEL1.zTemp15 = null;
            hC_OSHA_SITE_MODEL1.zTemp16 = null;
            hC_OSHA_SITE_MODEL1.zTemp17 = null;
            hC_OSHA_SITE_MODEL1.zTemp18 = null;
            hC_OSHA_SITE_MODEL1.zTemp19 = null;
            hC_OSHA_SITE_MODEL1.zTemp2 = null;
            hC_OSHA_SITE_MODEL1.zTemp20 = null;
            hC_OSHA_SITE_MODEL1.zTemp3 = null;
            hC_OSHA_SITE_MODEL1.zTemp4 = null;
            hC_OSHA_SITE_MODEL1.zTemp5 = null;
            hC_OSHA_SITE_MODEL1.zTemp6 = null;
            hC_OSHA_SITE_MODEL1.zTemp7 = null;
            hC_OSHA_SITE_MODEL1.zTemp8 = null;
            hC_OSHA_SITE_MODEL1.zTemp9 = null;
            this.OshaSiteLastTree.GetSite = hC_OSHA_SITE_MODEL1;
            this.OshaSiteLastTree.IsCheckbox = false;
            this.OshaSiteLastTree.Location = new System.Drawing.Point(3, 8);
            this.OshaSiteLastTree.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.OshaSiteLastTree.Name = "OshaSiteLastTree";
            this.OshaSiteLastTree.Size = new System.Drawing.Size(404, 544);
            this.OshaSiteLastTree.TabIndex = 4;
            this.OshaSiteLastTree.NodeClick += new HC_OSHA.OshaSiteLastTree.SiteTreeViewNodeMouseClickEventHandler(this.OshaSiteLastTree_NodeClick);
            this.OshaSiteLastTree.Load += new System.EventHandler(this.OshaSiteLastTree_Load);
            // 
            // oshaSiteEstimateList1
            // 
            this.oshaSiteEstimateList1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.oshaSiteEstimateList1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            hC_ESTIMATE_MODEL1.ComboDisplay = null;
            hC_ESTIMATE_MODEL1.CONTRACTDATE = null;
            hC_ESTIMATE_MODEL1.CONTRACTENDDATE = null;
            hC_ESTIMATE_MODEL1.ContractPeriod = null;
            hC_ESTIMATE_MODEL1.CONTRACTSTARTDATE = null;
            hC_ESTIMATE_MODEL1.ESTIMATEDATE = null;
            hC_ESTIMATE_MODEL1.ID = ((long)(0));
            hC_ESTIMATE_MODEL1.ISCONTRACT = null;
            hC_ESTIMATE_MODEL1.RowStatus = ComBase.Mvc.RowStatus.None;
            hC_ESTIMATE_MODEL1.zTemp1 = null;
            hC_ESTIMATE_MODEL1.zTemp10 = null;
            hC_ESTIMATE_MODEL1.zTemp11 = null;
            hC_ESTIMATE_MODEL1.zTemp12 = null;
            hC_ESTIMATE_MODEL1.zTemp13 = null;
            hC_ESTIMATE_MODEL1.zTemp14 = null;
            hC_ESTIMATE_MODEL1.zTemp15 = null;
            hC_ESTIMATE_MODEL1.zTemp16 = null;
            hC_ESTIMATE_MODEL1.zTemp17 = null;
            hC_ESTIMATE_MODEL1.zTemp18 = null;
            hC_ESTIMATE_MODEL1.zTemp19 = null;
            hC_ESTIMATE_MODEL1.zTemp2 = null;
            hC_ESTIMATE_MODEL1.zTemp20 = null;
            hC_ESTIMATE_MODEL1.zTemp3 = null;
            hC_ESTIMATE_MODEL1.zTemp4 = null;
            hC_ESTIMATE_MODEL1.zTemp5 = null;
            hC_ESTIMATE_MODEL1.zTemp6 = null;
            hC_ESTIMATE_MODEL1.zTemp7 = null;
            hC_ESTIMATE_MODEL1.zTemp8 = null;
            hC_ESTIMATE_MODEL1.zTemp9 = null;
            this.oshaSiteEstimateList1.GetEstimateModel = hC_ESTIMATE_MODEL1;
            this.oshaSiteEstimateList1.Location = new System.Drawing.Point(3, 552);
            this.oshaSiteEstimateList1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.oshaSiteEstimateList1.Name = "oshaSiteEstimateList1";
            this.oshaSiteEstimateList1.Size = new System.Drawing.Size(404, 207);
            this.oshaSiteEstimateList1.TabIndex = 1;
            this.oshaSiteEstimateList1.CellDoubleClick += new HC_OSHA.OshaSiteEstimateList.CellDoubleClickEventHandler(this.OshaSiteEstimateList1_CellDoubleClick);
            // 
            // horizSpace1
            // 
            this.horizSpace1.BackColor = System.Drawing.SystemColors.Control;
            this.horizSpace1.Dock = System.Windows.Forms.DockStyle.Top;
            this.horizSpace1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.horizSpace1.Location = new System.Drawing.Point(3, 3);
            this.horizSpace1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.horizSpace1.Name = "horizSpace1";
            this.horizSpace1.Size = new System.Drawing.Size(404, 5);
            this.horizSpace1.TabIndex = 3;
            // 
            // oshaSiteEstimateList2
            // 
            this.oshaSiteEstimateList2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            hC_ESTIMATE_MODEL2.ComboDisplay = null;
            hC_ESTIMATE_MODEL2.CONTRACTDATE = null;
            hC_ESTIMATE_MODEL2.CONTRACTENDDATE = null;
            hC_ESTIMATE_MODEL2.ContractPeriod = null;
            hC_ESTIMATE_MODEL2.CONTRACTSTARTDATE = null;
            hC_ESTIMATE_MODEL2.ESTIMATEDATE = null;
            hC_ESTIMATE_MODEL2.ID = ((long)(0));
            hC_ESTIMATE_MODEL2.ISCONTRACT = null;
            hC_ESTIMATE_MODEL2.RowStatus = ComBase.Mvc.RowStatus.None;
            hC_ESTIMATE_MODEL2.zTemp1 = null;
            hC_ESTIMATE_MODEL2.zTemp10 = null;
            hC_ESTIMATE_MODEL2.zTemp11 = null;
            hC_ESTIMATE_MODEL2.zTemp12 = null;
            hC_ESTIMATE_MODEL2.zTemp13 = null;
            hC_ESTIMATE_MODEL2.zTemp14 = null;
            hC_ESTIMATE_MODEL2.zTemp15 = null;
            hC_ESTIMATE_MODEL2.zTemp16 = null;
            hC_ESTIMATE_MODEL2.zTemp17 = null;
            hC_ESTIMATE_MODEL2.zTemp18 = null;
            hC_ESTIMATE_MODEL2.zTemp19 = null;
            hC_ESTIMATE_MODEL2.zTemp2 = null;
            hC_ESTIMATE_MODEL2.zTemp20 = null;
            hC_ESTIMATE_MODEL2.zTemp3 = null;
            hC_ESTIMATE_MODEL2.zTemp4 = null;
            hC_ESTIMATE_MODEL2.zTemp5 = null;
            hC_ESTIMATE_MODEL2.zTemp6 = null;
            hC_ESTIMATE_MODEL2.zTemp7 = null;
            hC_ESTIMATE_MODEL2.zTemp8 = null;
            hC_ESTIMATE_MODEL2.zTemp9 = null;
            this.oshaSiteEstimateList2.GetEstimateModel = hC_ESTIMATE_MODEL2;
            this.oshaSiteEstimateList2.Location = new System.Drawing.Point(3, 1000);
            this.oshaSiteEstimateList2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.oshaSiteEstimateList2.Name = "oshaSiteEstimateList2";
            this.oshaSiteEstimateList2.Size = new System.Drawing.Size(257, 0);
            this.oshaSiteEstimateList2.TabIndex = 2;
            // 
            // TabUnvisit
            // 
            this.TabUnvisit.Controls.Add(this.SSUnVisit);
            this.TabUnvisit.Controls.Add(this.panSearchUnvisit);
            this.TabUnvisit.Location = new System.Drawing.Point(4, 26);
            this.TabUnvisit.Name = "TabUnvisit";
            this.TabUnvisit.Padding = new System.Windows.Forms.Padding(3);
            this.TabUnvisit.Size = new System.Drawing.Size(410, 762);
            this.TabUnvisit.TabIndex = 1;
            this.TabUnvisit.Text = "미방문";
            this.TabUnvisit.UseVisualStyleBackColor = true;
            // 
            // SSUnVisit
            // 
            this.SSUnVisit.AccessibleDescription = "SSUnVisit, Sheet1, Row 0, Column 0, 123213";
            this.SSUnVisit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSUnVisit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSUnVisit.Location = new System.Drawing.Point(3, 130);
            this.SSUnVisit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SSUnVisit.Name = "SSUnVisit";
            this.SSUnVisit.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSUnVisit_Sheet1});
            this.SSUnVisit.Size = new System.Drawing.Size(404, 629);
            this.SSUnVisit.TabIndex = 31;
            this.SSUnVisit.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSUnVisit_CellDoubleClick);
            // 
            // SSUnVisit_Sheet1
            // 
            this.SSUnVisit_Sheet1.Reset();
            this.SSUnVisit_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSUnVisit_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSUnVisit_Sheet1.Cells.Get(0, 0).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SSUnVisit_Sheet1.Cells.Get(0, 0).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.SSUnVisit_Sheet1.Cells.Get(0, 0).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.SSUnVisit_Sheet1.Cells.Get(0, 0).ParseFormatString = "n";
            this.SSUnVisit_Sheet1.Cells.Get(0, 0).Value = 123213;
            this.SSUnVisit_Sheet1.Cells.Get(0, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SSUnVisit_Sheet1.Cells.Get(0, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.SSUnVisit_Sheet1.Cells.Get(0, 1).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.SSUnVisit_Sheet1.Cells.Get(0, 1).ParseFormatString = "n";
            this.SSUnVisit_Sheet1.Cells.Get(0, 1).Value = 2323;
            this.SSUnVisit_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSUnVisit_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSUnVisit_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSUnVisit_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSUnVisit_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSUnVisit_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSUnVisit_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSUnVisit_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSUnVisit_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSUnVisit_Sheet1.Rows.Get(0).ForeColor = System.Drawing.Color.Yellow;
            this.SSUnVisit_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panSearchUnvisit
            // 
            this.panSearchUnvisit.Controls.Add(this.label35);
            this.panSearchUnvisit.Controls.Add(this.TxtSearchUnvisitSiteIdOrName);
            this.panSearchUnvisit.Controls.Add(this.LblUnvisitCount);
            this.panSearchUnvisit.Controls.Add(this.DtpSearchUnVisitStartDate);
            this.panSearchUnvisit.Controls.Add(this.CboSearchUnVisitUserId);
            this.panSearchUnvisit.Controls.Add(this.DtpSearchUnVisitEndDate);
            this.panSearchUnvisit.Controls.Add(this.label18);
            this.panSearchUnvisit.Controls.Add(this.label27);
            this.panSearchUnvisit.Controls.Add(this.BtnSearchUnVisit);
            this.panSearchUnvisit.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSearchUnvisit.Location = new System.Drawing.Point(3, 3);
            this.panSearchUnvisit.Name = "panSearchUnvisit";
            this.panSearchUnvisit.Size = new System.Drawing.Size(404, 127);
            this.panSearchUnvisit.TabIndex = 107;
            // 
            // label35
            // 
            this.label35.BackColor = System.Drawing.SystemColors.Window;
            this.label35.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label35.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label35.Location = new System.Drawing.Point(2, 63);
            this.label35.Name = "label35";
            this.label35.Padding = new System.Windows.Forms.Padding(4);
            this.label35.Size = new System.Drawing.Size(58, 25);
            this.label35.TabIndex = 116;
            this.label35.Text = "사업장";
            this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TxtSearchUnvisitSiteIdOrName
            // 
            this.TxtSearchUnvisitSiteIdOrName.Location = new System.Drawing.Point(67, 64);
            this.TxtSearchUnvisitSiteIdOrName.Name = "TxtSearchUnvisitSiteIdOrName";
            this.TxtSearchUnvisitSiteIdOrName.Size = new System.Drawing.Size(183, 25);
            this.TxtSearchUnvisitSiteIdOrName.TabIndex = 109;
            this.TxtSearchUnvisitSiteIdOrName.TextChanged += new System.EventHandler(this.TxtSearchUnvisitSiteIdOrName_TextChanged);
            // 
            // LblUnvisitCount
            // 
            this.LblUnvisitCount.BackColor = System.Drawing.Color.White;
            this.LblUnvisitCount.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LblUnvisitCount.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LblUnvisitCount.Location = new System.Drawing.Point(3, 92);
            this.LblUnvisitCount.Name = "LblUnvisitCount";
            this.LblUnvisitCount.Padding = new System.Windows.Forms.Padding(4);
            this.LblUnvisitCount.Size = new System.Drawing.Size(59, 25);
            this.LblUnvisitCount.TabIndex = 108;
            this.LblUnvisitCount.Text = "0건";
            this.LblUnvisitCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DtpSearchUnVisitStartDate
            // 
            this.DtpSearchUnVisitStartDate.Checked = false;
            this.DtpSearchUnVisitStartDate.CustomFormat = "yyyy-MM-dd";
            this.DtpSearchUnVisitStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpSearchUnVisitStartDate.Location = new System.Drawing.Point(67, 35);
            this.DtpSearchUnVisitStartDate.Name = "DtpSearchUnVisitStartDate";
            this.DtpSearchUnVisitStartDate.Size = new System.Drawing.Size(88, 25);
            this.DtpSearchUnVisitStartDate.TabIndex = 107;
            // 
            // CboSearchUnVisitUserId
            // 
            this.CboSearchUnVisitUserId.FormattingEnabled = true;
            this.CboSearchUnVisitUserId.Location = new System.Drawing.Point(68, 4);
            this.CboSearchUnVisitUserId.Name = "CboSearchUnVisitUserId";
            this.CboSearchUnVisitUserId.Size = new System.Drawing.Size(182, 25);
            this.CboSearchUnVisitUserId.TabIndex = 65;
            this.CboSearchUnVisitUserId.SelectedIndexChanged += new System.EventHandler(this.CboSearchUnVisitUserId_SelectedIndexChanged);
            // 
            // DtpSearchUnVisitEndDate
            // 
            this.DtpSearchUnVisitEndDate.Checked = false;
            this.DtpSearchUnVisitEndDate.CustomFormat = "yyyy-MM-dd";
            this.DtpSearchUnVisitEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpSearchUnVisitEndDate.Location = new System.Drawing.Point(162, 35);
            this.DtpSearchUnVisitEndDate.Name = "DtpSearchUnVisitEndDate";
            this.DtpSearchUnVisitEndDate.Size = new System.Drawing.Size(88, 25);
            this.DtpSearchUnVisitEndDate.TabIndex = 106;
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.White;
            this.label18.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label18.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label18.Location = new System.Drawing.Point(2, 28);
            this.label18.Name = "label18";
            this.label18.Padding = new System.Windows.Forms.Padding(4);
            this.label18.Size = new System.Drawing.Size(58, 25);
            this.label18.TabIndex = 105;
            this.label18.Text = "방문월";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label27
            // 
            this.label27.BackColor = System.Drawing.Color.White;
            this.label27.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label27.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label27.Location = new System.Drawing.Point(3, 3);
            this.label27.Name = "label27";
            this.label27.Padding = new System.Windows.Forms.Padding(4);
            this.label27.Size = new System.Drawing.Size(58, 25);
            this.label27.TabIndex = 105;
            this.label27.Text = "담당자";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BtnSearchUnVisit
            // 
            this.BtnSearchUnVisit.Location = new System.Drawing.Point(66, 92);
            this.BtnSearchUnVisit.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearchUnVisit.Name = "BtnSearchUnVisit";
            this.BtnSearchUnVisit.Size = new System.Drawing.Size(184, 25);
            this.BtnSearchUnVisit.TabIndex = 84;
            this.BtnSearchUnVisit.Text = " 검색";
            this.BtnSearchUnVisit.UseVisualStyleBackColor = true;
            this.BtnSearchUnVisit.Click += new System.EventHandler(this.BtnSearchUnVisit_Click);
            // 
            // TabVisit
            // 
            this.TabVisit.Controls.Add(this.SSVisit);
            this.TabVisit.Controls.Add(this.panSearchVisit);
            this.TabVisit.Location = new System.Drawing.Point(4, 26);
            this.TabVisit.Name = "TabVisit";
            this.TabVisit.Padding = new System.Windows.Forms.Padding(3);
            this.TabVisit.Size = new System.Drawing.Size(410, 762);
            this.TabVisit.TabIndex = 2;
            this.TabVisit.Text = "방문";
            this.TabVisit.UseVisualStyleBackColor = true;
            // 
            // SSVisit
            // 
            this.SSVisit.AccessibleDescription = "";
            this.SSVisit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSVisit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSVisit.Location = new System.Drawing.Point(3, 129);
            this.SSVisit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SSVisit.Name = "SSVisit";
            this.SSVisit.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSVisit_Sheet1});
            this.SSVisit.Size = new System.Drawing.Size(404, 630);
            this.SSVisit.TabIndex = 113;
            this.SSVisit.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSVisit_CellDoubleClick);
            // 
            // SSVisit_Sheet1
            // 
            this.SSVisit_Sheet1.Reset();
            this.SSVisit_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSVisit_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSVisit_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSVisit_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSVisit_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSVisit_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSVisit_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSVisit_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSVisit_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSVisit_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSVisit_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panSearchVisit
            // 
            this.panSearchVisit.Controls.Add(this.label34);
            this.panSearchVisit.Controls.Add(this.TxtSearchVisitSiteIdOrName);
            this.panSearchVisit.Controls.Add(this.LblVisitCount);
            this.panSearchVisit.Controls.Add(this.CboSearchVisitUserId);
            this.panSearchVisit.Controls.Add(this.DtpSearchVisitStartDate);
            this.panSearchVisit.Controls.Add(this.DtpSearchVisitEndDate);
            this.panSearchVisit.Controls.Add(this.label29);
            this.panSearchVisit.Controls.Add(this.label28);
            this.panSearchVisit.Controls.Add(this.BtnSearchVisit);
            this.panSearchVisit.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSearchVisit.Location = new System.Drawing.Point(3, 3);
            this.panSearchVisit.Name = "panSearchVisit";
            this.panSearchVisit.Size = new System.Drawing.Size(404, 126);
            this.panSearchVisit.TabIndex = 114;
            // 
            // label34
            // 
            this.label34.BackColor = System.Drawing.SystemColors.Window;
            this.label34.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label34.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label34.Location = new System.Drawing.Point(5, 65);
            this.label34.Name = "label34";
            this.label34.Padding = new System.Windows.Forms.Padding(4);
            this.label34.Size = new System.Drawing.Size(58, 25);
            this.label34.TabIndex = 115;
            this.label34.Text = "사업장";
            this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TxtSearchVisitSiteIdOrName
            // 
            this.TxtSearchVisitSiteIdOrName.Location = new System.Drawing.Point(68, 65);
            this.TxtSearchVisitSiteIdOrName.Name = "TxtSearchVisitSiteIdOrName";
            this.TxtSearchVisitSiteIdOrName.Size = new System.Drawing.Size(183, 25);
            this.TxtSearchVisitSiteIdOrName.TabIndex = 114;
            this.TxtSearchVisitSiteIdOrName.TextChanged += new System.EventHandler(this.TxtSearchVisitSiteIdOrName_TextChanged);
            // 
            // LblVisitCount
            // 
            this.LblVisitCount.BackColor = System.Drawing.Color.White;
            this.LblVisitCount.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LblVisitCount.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LblVisitCount.Location = new System.Drawing.Point(4, 93);
            this.LblVisitCount.Name = "LblVisitCount";
            this.LblVisitCount.Padding = new System.Windows.Forms.Padding(4);
            this.LblVisitCount.Size = new System.Drawing.Size(59, 25);
            this.LblVisitCount.TabIndex = 113;
            this.LblVisitCount.Text = "0건";
            this.LblVisitCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CboSearchVisitUserId
            // 
            this.CboSearchVisitUserId.FormattingEnabled = true;
            this.CboSearchVisitUserId.Location = new System.Drawing.Point(69, 5);
            this.CboSearchVisitUserId.Name = "CboSearchVisitUserId";
            this.CboSearchVisitUserId.Size = new System.Drawing.Size(182, 25);
            this.CboSearchVisitUserId.TabIndex = 108;
            this.CboSearchVisitUserId.SelectedIndexChanged += new System.EventHandler(this.CboSearchVisitUserId_SelectedIndexChanged);
            // 
            // DtpSearchVisitStartDate
            // 
            this.DtpSearchVisitStartDate.Checked = false;
            this.DtpSearchVisitStartDate.CustomFormat = "yyyy-MM-dd";
            this.DtpSearchVisitStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpSearchVisitStartDate.Location = new System.Drawing.Point(68, 36);
            this.DtpSearchVisitStartDate.Name = "DtpSearchVisitStartDate";
            this.DtpSearchVisitStartDate.Size = new System.Drawing.Size(88, 25);
            this.DtpSearchVisitStartDate.TabIndex = 107;
            // 
            // DtpSearchVisitEndDate
            // 
            this.DtpSearchVisitEndDate.Checked = false;
            this.DtpSearchVisitEndDate.CustomFormat = "yyyy-MM-dd";
            this.DtpSearchVisitEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpSearchVisitEndDate.Location = new System.Drawing.Point(163, 36);
            this.DtpSearchVisitEndDate.Name = "DtpSearchVisitEndDate";
            this.DtpSearchVisitEndDate.Size = new System.Drawing.Size(88, 25);
            this.DtpSearchVisitEndDate.TabIndex = 112;
            // 
            // label29
            // 
            this.label29.BackColor = System.Drawing.Color.White;
            this.label29.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label29.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label29.Location = new System.Drawing.Point(3, 29);
            this.label29.Name = "label29";
            this.label29.Padding = new System.Windows.Forms.Padding(4);
            this.label29.Size = new System.Drawing.Size(58, 25);
            this.label29.TabIndex = 111;
            this.label29.Text = "방문월";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label28
            // 
            this.label28.BackColor = System.Drawing.Color.White;
            this.label28.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label28.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label28.Location = new System.Drawing.Point(4, 4);
            this.label28.Name = "label28";
            this.label28.Padding = new System.Windows.Forms.Padding(4);
            this.label28.Size = new System.Drawing.Size(58, 25);
            this.label28.TabIndex = 110;
            this.label28.Text = "담당자";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BtnSearchVisit
            // 
            this.BtnSearchVisit.Location = new System.Drawing.Point(68, 93);
            this.BtnSearchVisit.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearchVisit.Name = "BtnSearchVisit";
            this.BtnSearchVisit.Size = new System.Drawing.Size(183, 25);
            this.BtnSearchVisit.TabIndex = 109;
            this.BtnSearchVisit.Text = "검색";
            this.BtnSearchVisit.UseVisualStyleBackColor = true;
            this.BtnSearchVisit.Click += new System.EventHandler(this.BtnSearchVisit_Click);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.SSPreCharge);
            this.tabPage5.Controls.Add(this.panel2);
            this.tabPage5.Location = new System.Drawing.Point(4, 26);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(410, 762);
            this.tabPage5.TabIndex = 3;
            this.tabPage5.Text = "선청구";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // SSPreCharge
            // 
            this.SSPreCharge.AccessibleDescription = "";
            this.SSPreCharge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSPreCharge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSPreCharge.Location = new System.Drawing.Point(3, 103);
            this.SSPreCharge.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SSPreCharge.Name = "SSPreCharge";
            this.SSPreCharge.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSPreCharge_Sheet1});
            this.SSPreCharge.Size = new System.Drawing.Size(404, 656);
            this.SSPreCharge.TabIndex = 116;
            this.SSPreCharge.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSPreCharge_CellDoubleClick);
            // 
            // SSPreCharge_Sheet1
            // 
            this.SSPreCharge_Sheet1.Reset();
            this.SSPreCharge_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSPreCharge_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSPreCharge_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSPreCharge_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSPreCharge_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSPreCharge_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSPreCharge_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSPreCharge_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSPreCharge_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSPreCharge_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSPreCharge_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.CboSearchPreVisitUserId);
            this.panel2.Controls.Add(this.DtpSearchPreVisitStartDate);
            this.panel2.Controls.Add(this.DtpSearchPreVisitEndDate);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label32);
            this.panel2.Controls.Add(this.BtnSearchPreVisit);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(404, 100);
            this.panel2.TabIndex = 115;
            // 
            // CboSearchPreVisitUserId
            // 
            this.CboSearchPreVisitUserId.FormattingEnabled = true;
            this.CboSearchPreVisitUserId.Location = new System.Drawing.Point(69, 5);
            this.CboSearchPreVisitUserId.Name = "CboSearchPreVisitUserId";
            this.CboSearchPreVisitUserId.Size = new System.Drawing.Size(182, 25);
            this.CboSearchPreVisitUserId.TabIndex = 108;
            this.CboSearchPreVisitUserId.SelectedIndexChanged += new System.EventHandler(this.CboSearchPreVisitUserId_SelectedIndexChanged);
            // 
            // DtpSearchPreVisitStartDate
            // 
            this.DtpSearchPreVisitStartDate.Checked = false;
            this.DtpSearchPreVisitStartDate.CustomFormat = "yyyy-MM-dd";
            this.DtpSearchPreVisitStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpSearchPreVisitStartDate.Location = new System.Drawing.Point(68, 36);
            this.DtpSearchPreVisitStartDate.Name = "DtpSearchPreVisitStartDate";
            this.DtpSearchPreVisitStartDate.Size = new System.Drawing.Size(88, 25);
            this.DtpSearchPreVisitStartDate.TabIndex = 107;
            // 
            // DtpSearchPreVisitEndDate
            // 
            this.DtpSearchPreVisitEndDate.Checked = false;
            this.DtpSearchPreVisitEndDate.CustomFormat = "yyyy-MM-dd";
            this.DtpSearchPreVisitEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpSearchPreVisitEndDate.Location = new System.Drawing.Point(163, 36);
            this.DtpSearchPreVisitEndDate.Name = "DtpSearchPreVisitEndDate";
            this.DtpSearchPreVisitEndDate.Size = new System.Drawing.Size(88, 25);
            this.DtpSearchPreVisitEndDate.TabIndex = 112;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.White;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(3, 29);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(4);
            this.label5.Size = new System.Drawing.Size(58, 25);
            this.label5.TabIndex = 111;
            this.label5.Text = "방문월";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label32
            // 
            this.label32.BackColor = System.Drawing.Color.White;
            this.label32.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label32.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label32.Location = new System.Drawing.Point(4, 4);
            this.label32.Name = "label32";
            this.label32.Padding = new System.Windows.Forms.Padding(4);
            this.label32.Size = new System.Drawing.Size(58, 25);
            this.label32.TabIndex = 110;
            this.label32.Text = "담당자";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BtnSearchPreVisit
            // 
            this.BtnSearchPreVisit.Location = new System.Drawing.Point(68, 68);
            this.BtnSearchPreVisit.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearchPreVisit.Name = "BtnSearchPreVisit";
            this.BtnSearchPreVisit.Size = new System.Drawing.Size(183, 25);
            this.BtnSearchPreVisit.TabIndex = 109;
            this.BtnSearchPreVisit.Text = "검색";
            this.BtnSearchPreVisit.UseVisualStyleBackColor = true;
            this.BtnSearchPreVisit.Click += new System.EventHandler(this.BtnSearchPreVisit_Click);
            // 
            // contentTitle2
            // 
            this.contentTitle2.Dock = System.Windows.Forms.DockStyle.Top;
            this.contentTitle2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.contentTitle2.Location = new System.Drawing.Point(0, 0);
            this.contentTitle2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.contentTitle2.Name = "contentTitle2";
            this.contentTitle2.Size = new System.Drawing.Size(418, 38);
            this.contentTitle2.TabIndex = 0;
            this.contentTitle2.TitleText = "사업장";
            // 
            // panRight
            // 
            this.panRight.Controls.Add(this.BtnShowCalendar);
            this.panRight.Controls.Add(this.btnNew);
            this.panRight.Controls.Add(this.tabControl1);
            this.panRight.Controls.Add(this.BtnSaveSchedule);
            this.panRight.Controls.Add(this.BtnDeleteSchedule);
            this.panRight.Controls.Add(this.panSchedule);
            this.panRight.Controls.Add(this.contentTitle1);
            this.panRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panRight.Location = new System.Drawing.Point(427, 3);
            this.panRight.Name = "panRight";
            this.panRight.Size = new System.Drawing.Size(1086, 830);
            this.panRight.TabIndex = 3;
            // 
            // BtnShowCalendar
            // 
            this.BtnShowCalendar.Location = new System.Drawing.Point(444, 2);
            this.BtnShowCalendar.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnShowCalendar.Name = "BtnShowCalendar";
            this.BtnShowCalendar.Size = new System.Drawing.Size(75, 28);
            this.BtnShowCalendar.TabIndex = 107;
            this.BtnShowCalendar.Text = "달력보기";
            this.BtnShowCalendar.UseVisualStyleBackColor = true;
            this.BtnShowCalendar.Click += new System.EventHandler(this.BtnShowCalendar_Click);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(525, 2);
            this.btnNew.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 28);
            this.btnNew.TabIndex = 106;
            this.btnNew.Text = "화면정리";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.TabEduPage);
            this.tabControl1.Controls.Add(this.TabCommitteePage);
            this.tabControl1.Controls.Add(this.TabInformation);
            this.tabControl1.Controls.Add(this.TabAccident);
            this.tabControl1.Controls.Add(this.TabInOut);
            this.tabControl1.Controls.Add(this.TabReceipt);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 218);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1086, 612);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.SSScheduleList);
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(1078, 582);
            this.tabPage2.TabIndex = 7;
            this.tabPage2.Text = "방문일정 목록";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // SSScheduleList
            // 
            this.SSScheduleList.AccessibleDescription = "";
            this.SSScheduleList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSScheduleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSScheduleList.Location = new System.Drawing.Point(0, 54);
            this.SSScheduleList.Name = "SSScheduleList";
            this.SSScheduleList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSScheduleList_Sheet1});
            this.SSScheduleList.Size = new System.Drawing.Size(1078, 528);
            this.SSScheduleList.TabIndex = 4;
            this.SSScheduleList.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSScheduleList_CellDoubleClick);
            // 
            // SSScheduleList_Sheet1
            // 
            this.SSScheduleList_Sheet1.Reset();
            this.SSScheduleList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSScheduleList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSScheduleList_Sheet1.ColumnCount = 0;
            this.SSScheduleList_Sheet1.RowCount = 0;
            this.SSScheduleList_Sheet1.ActiveColumnIndex = -1;
            this.SSScheduleList_Sheet1.ActiveRowIndex = -1;
            this.SSScheduleList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSScheduleList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSScheduleList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSScheduleList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSScheduleList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSScheduleList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSScheduleList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSScheduleList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSScheduleList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSScheduleList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnExcel);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.CboMonth);
            this.panel1.Controls.Add(this.ChkSearchSchedule);
            this.panel1.Controls.Add(this.BtnPrintSchedule);
            this.panel1.Controls.Add(this.label24);
            this.panel1.Controls.Add(this.CboSearchScheduleVisitUserId);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.CboSearchScheduleVisitUserId2);
            this.panel1.Controls.Add(this.BtnSearchSchedule);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1078, 54);
            this.panel1.TabIndex = 113;
            // 
            // btnExcel
            // 
            this.btnExcel.Location = new System.Drawing.Point(758, 16);
            this.btnExcel.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 28);
            this.btnExcel.TabIndex = 117;
            this.btnExcel.Text = "엑셀";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(437, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 17);
            this.label1.TabIndex = 116;
            this.label1.Text = "방문월";
            // 
            // CboMonth
            // 
            this.CboMonth.FormattingEnabled = true;
            this.CboMonth.Location = new System.Drawing.Point(486, 16);
            this.CboMonth.Name = "CboMonth";
            this.CboMonth.Size = new System.Drawing.Size(84, 25);
            this.CboMonth.TabIndex = 108;
            // 
            // ChkSearchSchedule
            // 
            this.ChkSearchSchedule.AutoSize = true;
            this.ChkSearchSchedule.Checked = true;
            this.ChkSearchSchedule.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkSearchSchedule.Location = new System.Drawing.Point(19, 18);
            this.ChkSearchSchedule.Name = "ChkSearchSchedule";
            this.ChkSearchSchedule.Size = new System.Drawing.Size(53, 21);
            this.ChkSearchSchedule.TabIndex = 114;
            this.ChkSearchSchedule.Text = "전체";
            this.ChkSearchSchedule.UseVisualStyleBackColor = true;
            // 
            // BtnPrintSchedule
            // 
            this.BtnPrintSchedule.Location = new System.Drawing.Point(677, 16);
            this.BtnPrintSchedule.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnPrintSchedule.Name = "BtnPrintSchedule";
            this.BtnPrintSchedule.Size = new System.Drawing.Size(75, 28);
            this.BtnPrintSchedule.TabIndex = 113;
            this.BtnPrintSchedule.Text = "인쇄";
            this.BtnPrintSchedule.UseVisualStyleBackColor = true;
            this.BtnPrintSchedule.Click += new System.EventHandler(this.BtnPrintSchedule_Click);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(257, 19);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(47, 17);
            this.label24.TabIndex = 112;
            this.label24.Text = "동행자";
            // 
            // CboSearchScheduleVisitUserId
            // 
            this.CboSearchScheduleVisitUserId.FormattingEnabled = true;
            this.CboSearchScheduleVisitUserId.Location = new System.Drawing.Point(144, 16);
            this.CboSearchScheduleVisitUserId.Name = "CboSearchScheduleVisitUserId";
            this.CboSearchScheduleVisitUserId.Size = new System.Drawing.Size(108, 25);
            this.CboSearchScheduleVisitUserId.TabIndex = 106;
            this.CboSearchScheduleVisitUserId.SelectedIndexChanged += new System.EventHandler(this.CboSearchScheduleVisitUserId_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(91, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 17);
            this.label3.TabIndex = 110;
            this.label3.Text = "방문자";
            // 
            // CboSearchScheduleVisitUserId2
            // 
            this.CboSearchScheduleVisitUserId2.FormattingEnabled = true;
            this.CboSearchScheduleVisitUserId2.Location = new System.Drawing.Point(309, 16);
            this.CboSearchScheduleVisitUserId2.Name = "CboSearchScheduleVisitUserId2";
            this.CboSearchScheduleVisitUserId2.Size = new System.Drawing.Size(108, 25);
            this.CboSearchScheduleVisitUserId2.TabIndex = 107;
            // 
            // BtnSearchSchedule
            // 
            this.BtnSearchSchedule.Location = new System.Drawing.Point(596, 16);
            this.BtnSearchSchedule.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearchSchedule.Name = "BtnSearchSchedule";
            this.BtnSearchSchedule.Size = new System.Drawing.Size(75, 28);
            this.BtnSearchSchedule.TabIndex = 110;
            this.BtnSearchSchedule.Text = "검색";
            this.BtnSearchSchedule.UseVisualStyleBackColor = true;
            this.BtnSearchSchedule.Click += new System.EventHandler(this.BtnSearchSchedule_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.PanPrice);
            this.tabPage1.Controls.Add(this.panVisit);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1078, 582);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "방문등록";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // PanPrice
            // 
            this.PanPrice.Controls.Add(this.ChkIsPreCharge);
            this.PanPrice.Controls.Add(this.NumChargePrice);
            this.PanPrice.Controls.Add(this.label33);
            this.PanPrice.Controls.Add(this.ChkIsFix);
            this.PanPrice.Controls.Add(this.tabControl3);
            this.PanPrice.Controls.Add(this.NumVisitTOTALPRICE);
            this.PanPrice.Controls.Add(this.label2);
            this.PanPrice.Controls.Add(this.label31);
            this.PanPrice.Controls.Add(this.label22);
            this.PanPrice.Controls.Add(this.NumVisitUNITTOALPRICE);
            this.PanPrice.Controls.Add(this.label25);
            this.PanPrice.Controls.Add(this.ChkISKUKGO);
            this.PanPrice.Controls.Add(this.ChkISFEE);
            this.PanPrice.Controls.Add(this.NumVisitUNITPRICE);
            this.PanPrice.Controls.Add(this.label23);
            this.PanPrice.Controls.Add(this.NumVisitWORKERCOUNT);
            this.PanPrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanPrice.Location = new System.Drawing.Point(3, 173);
            this.PanPrice.Name = "PanPrice";
            this.PanPrice.Size = new System.Drawing.Size(1072, 406);
            this.PanPrice.TabIndex = 116;
            // 
            // ChkIsPreCharge
            // 
            this.ChkIsPreCharge.AutoSize = true;
            this.ChkIsPreCharge.Enabled = false;
            this.ChkIsPreCharge.Location = new System.Drawing.Point(14, 241);
            this.ChkIsPreCharge.Name = "ChkIsPreCharge";
            this.ChkIsPreCharge.Size = new System.Drawing.Size(92, 21);
            this.ChkIsPreCharge.TabIndex = 97;
            this.ChkIsPreCharge.Text = "선청구여부";
            this.ChkIsPreCharge.UseVisualStyleBackColor = true;
            // 
            // NumChargePrice
            // 
            this.NumChargePrice.BackColor = System.Drawing.Color.White;
            this.NumChargePrice.Enabled = false;
            this.NumChargePrice.Location = new System.Drawing.Point(135, 195);
            this.NumChargePrice.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.NumChargePrice.Minimum = new decimal(new int[] {
            9999999,
            0,
            0,
            -2147483648});
            this.NumChargePrice.Name = "NumChargePrice";
            this.NumChargePrice.ReadOnly = true;
            this.NumChargePrice.Size = new System.Drawing.Size(121, 25);
            this.NumChargePrice.TabIndex = 96;
            // 
            // label33
            // 
            this.label33.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label33.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label33.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label33.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label33.Location = new System.Drawing.Point(8, 195);
            this.label33.Name = "label33";
            this.label33.Padding = new System.Windows.Forms.Padding(4);
            this.label33.Size = new System.Drawing.Size(123, 25);
            this.label33.TabIndex = 126;
            this.label33.Text = "선청구된금액";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ChkIsFix
            // 
            this.ChkIsFix.AutoSize = true;
            this.ChkIsFix.Location = new System.Drawing.Point(134, 36);
            this.ChkIsFix.Name = "ChkIsFix";
            this.ChkIsFix.Size = new System.Drawing.Size(53, 21);
            this.ChkIsFix.TabIndex = 124;
            this.ChkIsFix.Text = "정액";
            this.ChkIsFix.UseVisualStyleBackColor = true;
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage4);
            this.tabControl3.Location = new System.Drawing.Point(276, 37);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(784, 281);
            this.tabControl3.TabIndex = 123;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.SSVisitPriceList);
            this.tabPage4.Location = new System.Drawing.Point(4, 26);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(776, 251);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "수수료 발생 내역";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // SSVisitPriceList
            // 
            this.SSVisitPriceList.AccessibleDescription = "";
            this.SSVisitPriceList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSVisitPriceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSVisitPriceList.Location = new System.Drawing.Point(3, 3);
            this.SSVisitPriceList.Name = "SSVisitPriceList";
            this.SSVisitPriceList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSVisitPriceList_Sheet1});
            this.SSVisitPriceList.Size = new System.Drawing.Size(770, 245);
            this.SSVisitPriceList.TabIndex = 120;
            // 
            // SSVisitPriceList_Sheet1
            // 
            this.SSVisitPriceList_Sheet1.Reset();
            this.SSVisitPriceList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSVisitPriceList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSVisitPriceList_Sheet1.ColumnCount = 0;
            this.SSVisitPriceList_Sheet1.RowCount = 0;
            this.SSVisitPriceList_Sheet1.ActiveColumnIndex = -1;
            this.SSVisitPriceList_Sheet1.ActiveRowIndex = -1;
            this.SSVisitPriceList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSVisitPriceList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSVisitPriceList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSVisitPriceList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSVisitPriceList_Sheet1.ColumnHeader.Rows.Get(0).Height = 38F;
            this.SSVisitPriceList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSVisitPriceList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSVisitPriceList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSVisitPriceList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSVisitPriceList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSVisitPriceList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // NumVisitTOTALPRICE
            // 
            this.NumVisitTOTALPRICE.BackColor = System.Drawing.Color.White;
            this.NumVisitTOTALPRICE.Location = new System.Drawing.Point(135, 160);
            this.NumVisitTOTALPRICE.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.NumVisitTOTALPRICE.Minimum = new decimal(new int[] {
            9999999,
            0,
            0,
            -2147483648});
            this.NumVisitTOTALPRICE.Name = "NumVisitTOTALPRICE";
            this.NumVisitTOTALPRICE.Size = new System.Drawing.Size(121, 25);
            this.NumVisitTOTALPRICE.TabIndex = 95;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(8, 160);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(4);
            this.label2.Size = new System.Drawing.Size(123, 25);
            this.label2.TabIndex = 121;
            this.label2.Text = "발생금액";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label31
            // 
            this.label31.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label31.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label31.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label31.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label31.Location = new System.Drawing.Point(8, 2);
            this.label31.Name = "label31";
            this.label31.Padding = new System.Windows.Forms.Padding(4);
            this.label31.Size = new System.Drawing.Size(861, 25);
            this.label31.TabIndex = 86;
            this.label31.Text = "보건대행수수료";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label22
            // 
            this.label22.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label22.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label22.Location = new System.Drawing.Point(8, 64);
            this.label22.Name = "label22";
            this.label22.Padding = new System.Windows.Forms.Padding(4);
            this.label22.Size = new System.Drawing.Size(123, 25);
            this.label22.TabIndex = 85;
            this.label22.Text = "계약인원";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NumVisitUNITTOALPRICE
            // 
            this.NumVisitUNITTOALPRICE.BackColor = System.Drawing.Color.White;
            this.NumVisitUNITTOALPRICE.Location = new System.Drawing.Point(135, 129);
            this.NumVisitUNITTOALPRICE.Maximum = new decimal(new int[] {
            1215752191,
            23,
            0,
            0});
            this.NumVisitUNITTOALPRICE.Minimum = new decimal(new int[] {
            -727379969,
            232,
            0,
            -2147483648});
            this.NumVisitUNITTOALPRICE.Name = "NumVisitUNITTOALPRICE";
            this.NumVisitUNITTOALPRICE.Size = new System.Drawing.Size(121, 25);
            this.NumVisitUNITTOALPRICE.TabIndex = 94;
            // 
            // label25
            // 
            this.label25.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label25.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label25.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label25.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label25.Location = new System.Drawing.Point(8, 129);
            this.label25.Name = "label25";
            this.label25.Padding = new System.Windows.Forms.Padding(4);
            this.label25.Size = new System.Drawing.Size(123, 25);
            this.label25.TabIndex = 85;
            this.label25.Text = "계산금액";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ChkISKUKGO
            // 
            this.ChkISKUKGO.AutoSize = true;
            this.ChkISKUKGO.Location = new System.Drawing.Point(202, 35);
            this.ChkISKUKGO.Name = "ChkISKUKGO";
            this.ChkISKUKGO.Size = new System.Drawing.Size(53, 21);
            this.ChkISKUKGO.TabIndex = 111;
            this.ChkISKUKGO.Text = "국고";
            this.ChkISKUKGO.UseVisualStyleBackColor = true;
            // 
            // ChkISFEE
            // 
            this.ChkISFEE.AutoSize = true;
            this.ChkISFEE.Location = new System.Drawing.Point(10, 37);
            this.ChkISFEE.Name = "ChkISFEE";
            this.ChkISFEE.Size = new System.Drawing.Size(97, 21);
            this.ChkISFEE.TabIndex = 110;
            this.ChkISFEE.Text = "수수료 발생";
            this.ChkISFEE.UseVisualStyleBackColor = true;
            this.ChkISFEE.CheckedChanged += new System.EventHandler(this.ChkISFEE_CheckedChanged);
            this.ChkISFEE.Click += new System.EventHandler(this.ChkISFEE_Click);
            // 
            // NumVisitUNITPRICE
            // 
            this.NumVisitUNITPRICE.BackColor = System.Drawing.Color.White;
            this.NumVisitUNITPRICE.Location = new System.Drawing.Point(135, 95);
            this.NumVisitUNITPRICE.Maximum = new decimal(new int[] {
            1215752191,
            23,
            0,
            0});
            this.NumVisitUNITPRICE.Minimum = new decimal(new int[] {
            1215752191,
            23,
            0,
            -2147483648});
            this.NumVisitUNITPRICE.Name = "NumVisitUNITPRICE";
            this.NumVisitUNITPRICE.Size = new System.Drawing.Size(121, 25);
            this.NumVisitUNITPRICE.TabIndex = 93;
            this.NumVisitUNITPRICE.ValueChanged += new System.EventHandler(this.NumVisitUNITPRICE_ValueChanged);
            this.NumVisitUNITPRICE.KeyUp += new System.Windows.Forms.KeyEventHandler(this.NumVisitUNITPRICE_KeyUp);
            // 
            // label23
            // 
            this.label23.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label23.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label23.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label23.Location = new System.Drawing.Point(8, 95);
            this.label23.Name = "label23";
            this.label23.Padding = new System.Windows.Forms.Padding(4);
            this.label23.Size = new System.Drawing.Size(123, 25);
            this.label23.TabIndex = 85;
            this.label23.Text = "계약단가";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NumVisitWORKERCOUNT
            // 
            this.NumVisitWORKERCOUNT.BackColor = System.Drawing.Color.White;
            this.NumVisitWORKERCOUNT.Location = new System.Drawing.Point(135, 64);
            this.NumVisitWORKERCOUNT.Maximum = new decimal(new int[] {
            1215752191,
            23,
            0,
            0});
            this.NumVisitWORKERCOUNT.Minimum = new decimal(new int[] {
            1215752191,
            23,
            0,
            -2147483648});
            this.NumVisitWORKERCOUNT.Name = "NumVisitWORKERCOUNT";
            this.NumVisitWORKERCOUNT.Size = new System.Drawing.Size(121, 25);
            this.NumVisitWORKERCOUNT.TabIndex = 92;
            this.NumVisitWORKERCOUNT.ValueChanged += new System.EventHandler(this.NumVisitWORKERCOUNT_ValueChanged);
            this.NumVisitWORKERCOUNT.KeyUp += new System.Windows.Forms.KeyEventHandler(this.NumVisitWORKERCOUNT_KeyUp);
            // 
            // panVisit
            // 
            this.panVisit.Controls.Add(this.txtEndTime);
            this.panVisit.Controls.Add(this.txtStartTime);
            this.panVisit.Controls.Add(this.BtnGetVisit);
            this.panVisit.Controls.Add(this.BtnSaveVisit);
            this.panVisit.Controls.Add(this.BtnVisitDelete);
            this.panVisit.Controls.Add(this.TxtTakeHourAndMinuteText);
            this.panVisit.Controls.Add(this.contentTitle3);
            this.panVisit.Controls.Add(this.label15);
            this.panVisit.Controls.Add(this.TxtVisitREMARK);
            this.panVisit.Controls.Add(this.CboVISITTYPE);
            this.panVisit.Controls.Add(this.label30);
            this.panVisit.Controls.Add(this.label21);
            this.panVisit.Controls.Add(this.CboVISITDOCTOR);
            this.panVisit.Controls.Add(this.label26);
            this.panVisit.Controls.Add(this.label20);
            this.panVisit.Controls.Add(this.label19);
            this.panVisit.Controls.Add(this.label16);
            this.panVisit.Controls.Add(this.CboVISITUSER);
            this.panVisit.Controls.Add(this.TxtTakeHourAndMinute);
            this.panVisit.Controls.Add(this.DtpVISITDATETIME);
            this.panVisit.Controls.Add(this.label17);
            this.panVisit.Dock = System.Windows.Forms.DockStyle.Top;
            this.panVisit.Location = new System.Drawing.Point(3, 3);
            this.panVisit.Name = "panVisit";
            this.panVisit.Size = new System.Drawing.Size(1072, 170);
            this.panVisit.TabIndex = 113;
            // 
            // txtEndTime
            // 
            this.txtEndTime.Location = new System.Drawing.Point(405, 72);
            this.txtEndTime.Name = "txtEndTime";
            this.txtEndTime.Size = new System.Drawing.Size(74, 25);
            this.txtEndTime.TabIndex = 87;
            this.txtEndTime.TextChanged += new System.EventHandler(this.txtEndTime_TextChanged);
            // 
            // txtStartTime
            // 
            this.txtStartTime.Location = new System.Drawing.Point(145, 73);
            this.txtStartTime.Name = "txtStartTime";
            this.txtStartTime.Size = new System.Drawing.Size(74, 25);
            this.txtStartTime.TabIndex = 86;
            this.txtStartTime.TextChanged += new System.EventHandler(this.txtStartTime_TextChanged);
            // 
            // BtnGetVisit
            // 
            this.BtnGetVisit.Location = new System.Drawing.Point(524, 3);
            this.BtnGetVisit.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnGetVisit.Name = "BtnGetVisit";
            this.BtnGetVisit.Size = new System.Drawing.Size(75, 28);
            this.BtnGetVisit.TabIndex = 98;
            this.BtnGetVisit.Text = "가져오기";
            this.BtnGetVisit.UseVisualStyleBackColor = true;
            this.BtnGetVisit.Click += new System.EventHandler(this.BtnGetVisit_Click);
            // 
            // BtnSaveVisit
            // 
            this.BtnSaveVisit.Location = new System.Drawing.Point(686, 3);
            this.BtnSaveVisit.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSaveVisit.Name = "BtnSaveVisit";
            this.BtnSaveVisit.Size = new System.Drawing.Size(75, 28);
            this.BtnSaveVisit.TabIndex = 100;
            this.BtnSaveVisit.Text = "저장(&S)";
            this.BtnSaveVisit.UseVisualStyleBackColor = true;
            this.BtnSaveVisit.Click += new System.EventHandler(this.BtnSaveVisit_Click);
            // 
            // BtnVisitDelete
            // 
            this.BtnVisitDelete.Location = new System.Drawing.Point(605, 3);
            this.BtnVisitDelete.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnVisitDelete.Name = "BtnVisitDelete";
            this.BtnVisitDelete.Size = new System.Drawing.Size(75, 28);
            this.BtnVisitDelete.TabIndex = 99;
            this.BtnVisitDelete.Text = "삭제";
            this.BtnVisitDelete.UseVisualStyleBackColor = true;
            this.BtnVisitDelete.Click += new System.EventHandler(this.BtnVisitDelete_Click);
            // 
            // TxtTakeHourAndMinuteText
            // 
            this.TxtTakeHourAndMinuteText.Location = new System.Drawing.Point(663, 73);
            this.TxtTakeHourAndMinuteText.Name = "TxtTakeHourAndMinuteText";
            this.TxtTakeHourAndMinuteText.ReadOnly = true;
            this.TxtTakeHourAndMinuteText.Size = new System.Drawing.Size(121, 25);
            this.TxtTakeHourAndMinuteText.TabIndex = 118;
            // 
            // contentTitle3
            // 
            this.contentTitle3.Dock = System.Windows.Forms.DockStyle.Top;
            this.contentTitle3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.contentTitle3.Location = new System.Drawing.Point(0, 0);
            this.contentTitle3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.contentTitle3.Name = "contentTitle3";
            this.contentTitle3.Size = new System.Drawing.Size(1072, 35);
            this.contentTitle3.TabIndex = 117;
            this.contentTitle3.TitleText = "방문 등록";
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label15.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label15.Location = new System.Drawing.Point(19, 42);
            this.label15.Name = "label15";
            this.label15.Padding = new System.Windows.Forms.Padding(4);
            this.label15.Size = new System.Drawing.Size(122, 25);
            this.label15.TabIndex = 86;
            this.label15.Text = "방문일";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TxtVisitREMARK
            // 
            this.TxtVisitREMARK.Location = new System.Drawing.Point(145, 135);
            this.TxtVisitREMARK.Name = "TxtVisitREMARK";
            this.TxtVisitREMARK.Size = new System.Drawing.Size(637, 25);
            this.TxtVisitREMARK.TabIndex = 91;
            // 
            // CboVISITTYPE
            // 
            this.CboVISITTYPE.FormattingEnabled = true;
            this.CboVISITTYPE.Location = new System.Drawing.Point(663, 102);
            this.CboVISITTYPE.Name = "CboVISITTYPE";
            this.CboVISITTYPE.Size = new System.Drawing.Size(121, 25);
            this.CboVISITTYPE.TabIndex = 90;
            // 
            // label30
            // 
            this.label30.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label30.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label30.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label30.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label30.Location = new System.Drawing.Point(534, 73);
            this.label30.Name = "label30";
            this.label30.Padding = new System.Windows.Forms.Padding(4);
            this.label30.Size = new System.Drawing.Size(123, 25);
            this.label30.TabIndex = 109;
            this.label30.Text = "소요시간";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label21
            // 
            this.label21.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label21.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label21.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label21.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label21.Location = new System.Drawing.Point(534, 102);
            this.label21.Name = "label21";
            this.label21.Padding = new System.Windows.Forms.Padding(4);
            this.label21.Size = new System.Drawing.Size(123, 25);
            this.label21.TabIndex = 109;
            this.label21.Text = "방문구분";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CboVISITDOCTOR
            // 
            this.CboVISITDOCTOR.FormattingEnabled = true;
            this.CboVISITDOCTOR.Location = new System.Drawing.Point(406, 103);
            this.CboVISITDOCTOR.Name = "CboVISITDOCTOR";
            this.CboVISITDOCTOR.Size = new System.Drawing.Size(121, 25);
            this.CboVISITDOCTOR.TabIndex = 89;
            // 
            // label26
            // 
            this.label26.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label26.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label26.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label26.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label26.Location = new System.Drawing.Point(19, 134);
            this.label26.Name = "label26";
            this.label26.Padding = new System.Windows.Forms.Padding(4);
            this.label26.Size = new System.Drawing.Size(122, 25);
            this.label26.TabIndex = 107;
            this.label26.Text = "방문일지 기타";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label20
            // 
            this.label20.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label20.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label20.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label20.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label20.Location = new System.Drawing.Point(277, 103);
            this.label20.Name = "label20";
            this.label20.Padding = new System.Windows.Forms.Padding(4);
            this.label20.Size = new System.Drawing.Size(123, 25);
            this.label20.TabIndex = 107;
            this.label20.Text = "의  사";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label19.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label19.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label19.Location = new System.Drawing.Point(19, 104);
            this.label19.Name = "label19";
            this.label19.Padding = new System.Windows.Forms.Padding(4);
            this.label19.Size = new System.Drawing.Size(122, 25);
            this.label19.TabIndex = 88;
            this.label19.Text = "방문자";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label16.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label16.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label16.Location = new System.Drawing.Point(276, 73);
            this.label16.Name = "label16";
            this.label16.Padding = new System.Windows.Forms.Padding(4);
            this.label16.Size = new System.Drawing.Size(123, 25);
            this.label16.TabIndex = 104;
            this.label16.Text = "방문종료시간";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CboVISITUSER
            // 
            this.CboVISITUSER.FormattingEnabled = true;
            this.CboVISITUSER.Location = new System.Drawing.Point(145, 104);
            this.CboVISITUSER.Name = "CboVISITUSER";
            this.CboVISITUSER.Size = new System.Drawing.Size(121, 25);
            this.CboVISITUSER.TabIndex = 88;
            // 
            // TxtTakeHourAndMinute
            // 
            this.TxtTakeHourAndMinute.Location = new System.Drawing.Point(790, 73);
            this.TxtTakeHourAndMinute.Name = "TxtTakeHourAndMinute";
            this.TxtTakeHourAndMinute.ReadOnly = true;
            this.TxtTakeHourAndMinute.Size = new System.Drawing.Size(121, 25);
            this.TxtTakeHourAndMinute.TabIndex = 96;
            // 
            // DtpVISITDATETIME
            // 
            this.DtpVISITDATETIME.CustomFormat = "yyyy-MM-dd";
            this.DtpVISITDATETIME.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpVISITDATETIME.Location = new System.Drawing.Point(145, 42);
            this.DtpVISITDATETIME.Name = "DtpVISITDATETIME";
            this.DtpVISITDATETIME.Size = new System.Drawing.Size(121, 25);
            this.DtpVISITDATETIME.TabIndex = 85;
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label17.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label17.Location = new System.Drawing.Point(19, 73);
            this.label17.Name = "label17";
            this.label17.Padding = new System.Windows.Forms.Padding(4);
            this.label17.Size = new System.Drawing.Size(122, 25);
            this.label17.TabIndex = 101;
            this.label17.Text = "방문시작시간";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TabEduPage
            // 
            this.TabEduPage.Location = new System.Drawing.Point(4, 26);
            this.TabEduPage.Name = "TabEduPage";
            this.TabEduPage.Padding = new System.Windows.Forms.Padding(3);
            this.TabEduPage.Size = new System.Drawing.Size(1078, 582);
            this.TabEduPage.TabIndex = 1;
            this.TabEduPage.Text = "보건교육지원";
            this.TabEduPage.UseVisualStyleBackColor = true;
            this.TabEduPage.Click += new System.EventHandler(this.TabEduPage_Click);
            // 
            // TabCommitteePage
            // 
            this.TabCommitteePage.Location = new System.Drawing.Point(4, 26);
            this.TabCommitteePage.Name = "TabCommitteePage";
            this.TabCommitteePage.Padding = new System.Windows.Forms.Padding(3);
            this.TabCommitteePage.Size = new System.Drawing.Size(1078, 582);
            this.TabCommitteePage.TabIndex = 2;
            this.TabCommitteePage.Text = "산업안전보건위원회대장";
            this.TabCommitteePage.UseVisualStyleBackColor = true;
            // 
            // TabInformation
            // 
            this.TabInformation.Location = new System.Drawing.Point(4, 26);
            this.TabInformation.Name = "TabInformation";
            this.TabInformation.Padding = new System.Windows.Forms.Padding(3);
            this.TabInformation.Size = new System.Drawing.Size(1078, 582);
            this.TabInformation.TabIndex = 3;
            this.TabInformation.Text = "정보자료제공";
            this.TabInformation.UseVisualStyleBackColor = true;
            // 
            // TabAccident
            // 
            this.TabAccident.Location = new System.Drawing.Point(4, 26);
            this.TabAccident.Name = "TabAccident";
            this.TabAccident.Padding = new System.Windows.Forms.Padding(3);
            this.TabAccident.Size = new System.Drawing.Size(1078, 582);
            this.TabAccident.TabIndex = 4;
            this.TabAccident.Text = "산업재해";
            this.TabAccident.UseVisualStyleBackColor = true;
            // 
            // TabInOut
            // 
            this.TabInOut.Location = new System.Drawing.Point(4, 26);
            this.TabInOut.Name = "TabInOut";
            this.TabInOut.Padding = new System.Windows.Forms.Padding(3);
            this.TabInOut.Size = new System.Drawing.Size(1078, 582);
            this.TabInOut.TabIndex = 5;
            this.TabInOut.Text = "입퇴사자";
            this.TabInOut.UseVisualStyleBackColor = true;
            // 
            // TabReceipt
            // 
            this.TabReceipt.Location = new System.Drawing.Point(4, 26);
            this.TabReceipt.Name = "TabReceipt";
            this.TabReceipt.Padding = new System.Windows.Forms.Padding(3);
            this.TabReceipt.Size = new System.Drawing.Size(1078, 582);
            this.TabReceipt.TabIndex = 6;
            this.TabReceipt.Text = "수령증발급";
            this.TabReceipt.UseVisualStyleBackColor = true;
            // 
            // BtnSaveSchedule
            // 
            this.BtnSaveSchedule.Location = new System.Drawing.Point(693, 2);
            this.BtnSaveSchedule.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSaveSchedule.Name = "BtnSaveSchedule";
            this.BtnSaveSchedule.Size = new System.Drawing.Size(75, 28);
            this.BtnSaveSchedule.TabIndex = 82;
            this.BtnSaveSchedule.Text = "저장(&S)";
            this.BtnSaveSchedule.UseVisualStyleBackColor = true;
            this.BtnSaveSchedule.Click += new System.EventHandler(this.BtnSaveSchedule_Click);
            // 
            // BtnDeleteSchedule
            // 
            this.BtnDeleteSchedule.Location = new System.Drawing.Point(612, 2);
            this.BtnDeleteSchedule.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnDeleteSchedule.Name = "BtnDeleteSchedule";
            this.BtnDeleteSchedule.Size = new System.Drawing.Size(75, 28);
            this.BtnDeleteSchedule.TabIndex = 83;
            this.BtnDeleteSchedule.Text = "삭제";
            this.BtnDeleteSchedule.UseVisualStyleBackColor = true;
            this.BtnDeleteSchedule.Click += new System.EventHandler(this.BtnDeleteSchedule_Click);
            // 
            // panSchedule
            // 
            this.panSchedule.BackColor = System.Drawing.Color.White;
            this.panSchedule.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSchedule.Controls.Add(this.TxtVISITPLACE);
            this.panSchedule.Controls.Add(this.label6);
            this.panSchedule.Controls.Add(this.txtVISITTIME);
            this.panSchedule.Controls.Add(this.ChkDoctor);
            this.panSchedule.Controls.Add(this.LblSiteName);
            this.panSchedule.Controls.Add(this.TxtMODIFIEDUSER);
            this.panSchedule.Controls.Add(this.TxtMODIFIED);
            this.panSchedule.Controls.Add(this.label14);
            this.panSchedule.Controls.Add(this.label7);
            this.panSchedule.Controls.Add(this.label12);
            this.panSchedule.Controls.Add(this.label11);
            this.panSchedule.Controls.Add(this.label4);
            this.panSchedule.Controls.Add(this.label9);
            this.panSchedule.Controls.Add(this.label8);
            this.panSchedule.Controls.Add(this.TxtREMARK);
            this.panSchedule.Controls.Add(this.CboVISITUSERID);
            this.panSchedule.Controls.Add(this.CboVISITMANAGERID);
            this.panSchedule.Controls.Add(this.DtpVISITRESERVEDATE);
            this.panSchedule.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSchedule.Location = new System.Drawing.Point(0, 38);
            this.panSchedule.Name = "panSchedule";
            this.panSchedule.Size = new System.Drawing.Size(1086, 180);
            this.panSchedule.TabIndex = 6;
            this.panSchedule.Paint += new System.Windows.Forms.PaintEventHandler(this.panSchedule_Paint);
            // 
            // TxtVISITPLACE
            // 
            this.TxtVISITPLACE.Location = new System.Drawing.Point(546, 47);
            this.TxtVISITPLACE.Name = "TxtVISITPLACE";
            this.TxtVISITPLACE.Size = new System.Drawing.Size(128, 25);
            this.TxtVISITPLACE.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(470, 47);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(4);
            this.label6.Size = new System.Drawing.Size(70, 25);
            this.label6.TabIndex = 106;
            this.label6.Text = "방문장소";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtVISITTIME
            // 
            this.txtVISITTIME.Location = new System.Drawing.Point(344, 48);
            this.txtVISITTIME.Name = "txtVISITTIME";
            this.txtVISITTIME.Size = new System.Drawing.Size(74, 25);
            this.txtVISITTIME.TabIndex = 1;
            // 
            // ChkDoctor
            // 
            this.ChkDoctor.AutoSize = true;
            this.ChkDoctor.Location = new System.Drawing.Point(470, 83);
            this.ChkDoctor.Name = "ChkDoctor";
            this.ChkDoctor.Size = new System.Drawing.Size(123, 21);
            this.ChkDoctor.TabIndex = 4;
            this.ChkDoctor.Text = "동행자 의사설정";
            this.ChkDoctor.UseVisualStyleBackColor = true;
            this.ChkDoctor.CheckedChanged += new System.EventHandler(this.ChkDoctor_CheckedChanged);
            // 
            // LblSiteName
            // 
            this.LblSiteName.AutoSize = true;
            this.LblSiteName.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LblSiteName.Location = new System.Drawing.Point(5, 7);
            this.LblSiteName.Name = "LblSiteName";
            this.LblSiteName.Size = new System.Drawing.Size(149, 20);
            this.LblSiteName.TabIndex = 105;
            this.LblSiteName.Text = "사업장을 선택하세요";
            // 
            // TxtMODIFIEDUSER
            // 
            this.TxtMODIFIEDUSER.Location = new System.Drawing.Point(517, 141);
            this.TxtMODIFIEDUSER.Name = "TxtMODIFIEDUSER";
            this.TxtMODIFIEDUSER.ReadOnly = true;
            this.TxtMODIFIEDUSER.Size = new System.Drawing.Size(121, 25);
            this.TxtMODIFIEDUSER.TabIndex = 103;
            this.TxtMODIFIEDUSER.Visible = false;
            // 
            // TxtMODIFIED
            // 
            this.TxtMODIFIED.Location = new System.Drawing.Point(136, 141);
            this.TxtMODIFIED.Name = "TxtMODIFIED";
            this.TxtMODIFIED.ReadOnly = true;
            this.TxtMODIFIED.Size = new System.Drawing.Size(246, 25);
            this.TxtMODIFIED.TabIndex = 102;
            this.TxtMODIFIED.Visible = false;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label14.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label14.Location = new System.Drawing.Point(387, 141);
            this.label14.Name = "label14";
            this.label14.Padding = new System.Windows.Forms.Padding(4);
            this.label14.Size = new System.Drawing.Size(122, 25);
            this.label14.TabIndex = 101;
            this.label14.Text = "수정자";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label14.Visible = false;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(9, 141);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(4);
            this.label7.Size = new System.Drawing.Size(121, 25);
            this.label7.TabIndex = 101;
            this.label7.Text = "수정일시";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label7.Visible = false;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label12.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label12.Location = new System.Drawing.Point(259, 48);
            this.label12.Name = "label12";
            this.label12.Padding = new System.Windows.Forms.Padding(4);
            this.label12.Size = new System.Drawing.Size(80, 25);
            this.label12.TabIndex = 89;
            this.label12.Text = "방문시간*";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label11.Location = new System.Drawing.Point(9, 79);
            this.label11.Name = "label11";
            this.label11.Padding = new System.Windows.Forms.Padding(4);
            this.label11.Size = new System.Drawing.Size(123, 25);
            this.label11.TabIndex = 88;
            this.label11.Text = "방문자*";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(259, 79);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(4);
            this.label4.Size = new System.Drawing.Size(80, 25);
            this.label4.TabIndex = 87;
            this.label4.Text = "동행자";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label9.Location = new System.Drawing.Point(9, 48);
            this.label9.Name = "label9";
            this.label9.Padding = new System.Windows.Forms.Padding(4);
            this.label9.Size = new System.Drawing.Size(123, 25);
            this.label9.TabIndex = 84;
            this.label9.Text = "방문예정일*";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(9, 110);
            this.label8.Name = "label8";
            this.label8.Padding = new System.Windows.Forms.Padding(4);
            this.label8.Size = new System.Drawing.Size(123, 25);
            this.label8.TabIndex = 68;
            this.label8.Text = "참고사항";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TxtREMARK
            // 
            this.TxtREMARK.Location = new System.Drawing.Point(136, 110);
            this.TxtREMARK.Name = "TxtREMARK";
            this.TxtREMARK.Size = new System.Drawing.Size(631, 25);
            this.TxtREMARK.TabIndex = 5;
            // 
            // CboVISITUSERID
            // 
            this.CboVISITUSERID.FormattingEnabled = true;
            this.CboVISITUSERID.Location = new System.Drawing.Point(136, 79);
            this.CboVISITUSERID.Name = "CboVISITUSERID";
            this.CboVISITUSERID.Size = new System.Drawing.Size(108, 25);
            this.CboVISITUSERID.TabIndex = 2;
            // 
            // CboVISITMANAGERID
            // 
            this.CboVISITMANAGERID.FormattingEnabled = true;
            this.CboVISITMANAGERID.Location = new System.Drawing.Point(344, 80);
            this.CboVISITMANAGERID.Name = "CboVISITMANAGERID";
            this.CboVISITMANAGERID.Size = new System.Drawing.Size(121, 25);
            this.CboVISITMANAGERID.TabIndex = 3;
            this.CboVISITMANAGERID.Click += new System.EventHandler(this.CboVISITMANAGERID_Click);
            // 
            // DtpVISITRESERVEDATE
            // 
            this.DtpVISITRESERVEDATE.CustomFormat = "yyyy-MM-dd";
            this.DtpVISITRESERVEDATE.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpVISITRESERVEDATE.Location = new System.Drawing.Point(136, 48);
            this.DtpVISITRESERVEDATE.Name = "DtpVISITRESERVEDATE";
            this.DtpVISITRESERVEDATE.Size = new System.Drawing.Size(108, 25);
            this.DtpVISITRESERVEDATE.TabIndex = 0;
            // 
            // contentTitle1
            // 
            this.contentTitle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.contentTitle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.contentTitle1.Location = new System.Drawing.Point(0, 0);
            this.contentTitle1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.contentTitle1.Name = "contentTitle1";
            this.contentTitle1.Size = new System.Drawing.Size(1086, 38);
            this.contentTitle1.TabIndex = 4;
            this.contentTitle1.TitleText = "방문 일정";
            // 
            // BtnClose
            // 
            this.BtnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnClose.Location = new System.Drawing.Point(1429, 4);
            this.BtnClose.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(75, 28);
            this.BtnClose.TabIndex = 108;
            this.BtnClose.Text = "닫기";
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // ScheduleRegisterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1516, 871);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.tableBody);
            this.Controls.Add(this.formTItle1);
            this.Name = "ScheduleRegisterForm";
            this.Text = "ScheduleRegisterForm";
            this.Load += new System.EventHandler(this.ScheduleRegisterForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.tableBody.ResumeLayout(false);
            this.panLeftTop.ResumeLayout(false);
            this.panSiteBody.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.TabSite.ResumeLayout(false);
            this.TabUnvisit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSUnVisit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSUnVisit_Sheet1)).EndInit();
            this.panSearchUnvisit.ResumeLayout(false);
            this.panSearchUnvisit.PerformLayout();
            this.TabVisit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSVisit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSVisit_Sheet1)).EndInit();
            this.panSearchVisit.ResumeLayout(false);
            this.panSearchVisit.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSPreCharge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSPreCharge_Sheet1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panRight.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSScheduleList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSScheduleList_Sheet1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.PanPrice.ResumeLayout(false);
            this.PanPrice.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumChargePrice)).EndInit();
            this.tabControl3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSVisitPriceList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSVisitPriceList_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumVisitTOTALPRICE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumVisitUNITTOALPRICE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumVisitUNITPRICE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumVisitWORKERCOUNT)).EndInit();
            this.panVisit.ResumeLayout(false);
            this.panVisit.PerformLayout();
            this.panSchedule.ResumeLayout(false);
            this.panSchedule.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ComBase.Mvc.UserControls.FormTItle formTItle1;
        private System.Windows.Forms.TableLayoutPanel tableBody;
        private System.Windows.Forms.Panel panRight;
        private System.Windows.Forms.DateTimePicker DtpVISITRESERVEDATE;
        private ComBase.Mvc.UserControls.ContentTitle contentTitle1;
        private System.Windows.Forms.Panel panSchedule;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage TabEduPage;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TxtREMARK;
        private System.Windows.Forms.ComboBox CboVISITUSERID;
        private System.Windows.Forms.ComboBox CboVISITMANAGERID;
        private System.Windows.Forms.Button BtnDeleteSchedule;
        private System.Windows.Forms.Button BtnSaveSchedule;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TxtMODIFIEDUSER;
        private System.Windows.Forms.TextBox TxtMODIFIED;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panLeftTop;
        private System.Windows.Forms.Panel panSiteBody;
        private ComBase.Mvc.UserControls.ContentTitle contentTitle2;
        private System.Windows.Forms.TextBox TxtVisitREMARK;
        private System.Windows.Forms.CheckBox ChkISKUKGO;
        private System.Windows.Forms.CheckBox ChkISFEE;
        private System.Windows.Forms.ComboBox CboVISITTYPE;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.ComboBox CboVISITDOCTOR;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button BtnVisitDelete;
        private System.Windows.Forms.Button BtnSaveVisit;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.DateTimePicker DtpVISITDATETIME;
        private System.Windows.Forms.TextBox TxtTakeHourAndMinute;
        private System.Windows.Forms.ComboBox CboVISITUSER;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.NumericUpDown NumVisitUNITTOALPRICE;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.NumericUpDown NumVisitUNITPRICE;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.NumericUpDown NumVisitWORKERCOUNT;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Panel panVisit;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage TabSite;
        private System.Windows.Forms.TabPage TabUnvisit;
        private System.Windows.Forms.TabPage TabVisit;
        private FarPoint.Win.Spread.FpSpread SSUnVisit;
        private FarPoint.Win.Spread.SheetView SSUnVisit_Sheet1;
        private System.Windows.Forms.ComboBox CboSearchUnVisitUserId;
        private System.Windows.Forms.Button BtnSearchUnVisit;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.DateTimePicker DtpSearchUnVisitEndDate;
        private FarPoint.Win.Spread.FpSpread SSVisit;
        private FarPoint.Win.Spread.SheetView SSVisit_Sheet1;
        private System.Windows.Forms.DateTimePicker DtpSearchVisitEndDate;
        private System.Windows.Forms.ComboBox CboSearchVisitUserId;
        private System.Windows.Forms.Button BtnSearchVisit;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.DateTimePicker DtpSearchVisitStartDate;
        private System.Windows.Forms.Label LblSiteName;
        private System.Windows.Forms.Panel panSearchUnvisit;
        private System.Windows.Forms.Panel panSearchVisit;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.TabPage TabCommitteePage;
        private System.Windows.Forms.TabPage TabInformation;
        private System.Windows.Forms.TabPage TabAccident;
        private System.Windows.Forms.TabPage TabInOut;
        private System.Windows.Forms.TabPage TabReceipt;
        private HC_OSHA.OshaSiteEstimateList oshaSiteEstimateList2;
        private HC_OSHA.OshaSiteEstimateList oshaSiteEstimateList1;
        private ComBase.Mvc.UserControls.HorizSpace horizSpace1;
        private OshaSiteLastTree OshaSiteLastTree;
        private System.Windows.Forms.Panel PanPrice;
        private System.Windows.Forms.Button btnNew;
        private ComBase.Mvc.UserControls.ContentTitle contentTitle3;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.TextBox TxtTakeHourAndMinuteText;
        private System.Windows.Forms.NumericUpDown NumVisitTOTALPRICE;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtnGetVisit;
        private System.Windows.Forms.DateTimePicker DtpSearchUnVisitStartDate;
        private System.Windows.Forms.Button BtnShowCalendar;
        private System.Windows.Forms.TabPage tabPage2;
        private FarPoint.Win.Spread.FpSpread SSScheduleList;
        private FarPoint.Win.Spread.SheetView SSScheduleList_Sheet1;
        private System.Windows.Forms.Button BtnSearchSchedule;
        private System.Windows.Forms.ComboBox CboSearchScheduleVisitUserId2;
        private System.Windows.Forms.ComboBox CboSearchScheduleVisitUserId;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button BtnPrintSchedule;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage4;
        private FarPoint.Win.Spread.FpSpread SSVisitPriceList;
        private FarPoint.Win.Spread.SheetView SSVisitPriceList_Sheet1;
        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.TabPage tabPage5;
        private FarPoint.Win.Spread.FpSpread SSPreCharge;
        private FarPoint.Win.Spread.SheetView SSPreCharge_Sheet1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox CboSearchPreVisitUserId;
        private System.Windows.Forms.DateTimePicker DtpSearchPreVisitStartDate;
        private System.Windows.Forms.DateTimePicker DtpSearchPreVisitEndDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Button BtnSearchPreVisit;
        private System.Windows.Forms.CheckBox ChkSearchSchedule;
        private System.Windows.Forms.CheckBox ChkDoctor;
        private System.Windows.Forms.Label LblUnvisitCount;
        private System.Windows.Forms.Label LblVisitCount;
        private System.Windows.Forms.CheckBox ChkIsFix;
        private System.Windows.Forms.NumericUpDown NumChargePrice;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TextBox TxtSearchUnvisitSiteIdOrName;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.TextBox TxtSearchVisitSiteIdOrName;
        private System.Windows.Forms.CheckBox ChkIsPreCharge;
        private System.Windows.Forms.TextBox txtVISITTIME;
        private System.Windows.Forms.TextBox txtEndTime;
        private System.Windows.Forms.TextBox txtStartTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CboMonth;
        private System.Windows.Forms.TextBox TxtVISITPLACE;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnExcel;
    }
}