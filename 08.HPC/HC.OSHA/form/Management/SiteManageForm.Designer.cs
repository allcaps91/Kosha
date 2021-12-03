namespace HC_OSHA
{
    partial class SiteManageForm
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
            HC.OSHA.Model.HC_ESTIMATE_MODEL hC_ESTIMATE_MODEL1 = new HC.OSHA.Model.HC_ESTIMATE_MODEL();
            HC.OSHA.Model.HC_OSHA_SITE_MODEL hC_OSHA_SITE_MODEL1 = new HC.OSHA.Model.HC_OSHA_SITE_MODEL();
            this.PanEstimate = new System.Windows.Forms.Panel();
            this.BtnDeleteParent = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.TxtBlueFeMale = new System.Windows.Forms.TextBox();
            this.TxtBlueMale = new System.Windows.Forms.TextBox();
            this.label52 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.TxtWhiteFeMale = new System.Windows.Forms.TextBox();
            this.TxtWhiteMale = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label63 = new System.Windows.Forms.Label();
            this.TxtPRINTDATE = new System.Windows.Forms.TextBox();
            this.label62 = new System.Windows.Forms.Label();
            this.TxtSENDMAILDATE = new System.Windows.Forms.TextBox();
            this.label61 = new System.Windows.Forms.Label();
            this.TxtRemark = new System.Windows.Forms.TextBox();
            this.label60 = new System.Windows.Forms.Label();
            this.NumOFFICIALFEE = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label59 = new System.Windows.Forms.Label();
            this.label50 = new System.Windows.Forms.Label();
            this.TxtParentSiteName = new System.Windows.Forms.TextBox();
            this.BtnSearchParent = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.TxtParentSiteId = new System.Windows.Forms.TextBox();
            this.BtnSaveEstimate = new System.Windows.Forms.Button();
            this.NumMONTHLYFEE = new System.Windows.Forms.NumericUpDown();
            this.BtnDeleteEstimate = new System.Windows.Forms.Button();
            this.btnNewEstimate = new System.Windows.Forms.Button();
            this.NumWORKERTOTALCOUNT = new System.Windows.Forms.NumericUpDown();
            this.DtpESTIMATEDATE = new System.Windows.Forms.DateTimePicker();
            this.DtpSTARTDATE = new System.Windows.Forms.DateTimePicker();
            this.NumSITEFEE = new System.Windows.Forms.NumericUpDown();
            this.BtnLoadExcel = new System.Windows.Forms.Button();
            this.SSEstimate = new FarPoint.Win.Spread.FpSpread();
            this.SSEstimate_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.BtnPrintEstimate = new System.Windows.Forms.Button();
            this.BtnSendMailEstimate = new System.Windows.Forms.Button();
            this.BtnSiteConfitm = new System.Windows.Forms.Button();
            this.TxtSiteName = new System.Windows.Forms.TextBox();
            this.TxtOshaSiteId = new System.Windows.Forms.TextBox();
            this.OshaSiteEstimateList = new HC_OSHA.OshaSiteEstimateList();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.PanContract = new System.Windows.Forms.Panel();
            this.BtnClear = new System.Windows.Forms.Button();
            this.BtnDeleteContract = new System.Windows.Forms.Button();
            this.BtnLastContract = new System.Windows.Forms.Button();
            this.BtnWorkerAdd = new System.Windows.Forms.Button();
            this.BtnSaveContract = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label58 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.TxtISSPECIALDATA = new System.Windows.Forms.TextBox();
            this.label44 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.ChkISSPECIAL = new System.Windows.Forms.CheckBox();
            this.ChkISBRAINTEST = new System.Windows.Forms.CheckBox();
            this.ChkISSTRESS = new System.Windows.Forms.CheckBox();
            this.ChkISSPACEPROGRAM = new System.Windows.Forms.CheckBox();
            this.ChkISEARPROGRAM = new System.Windows.Forms.CheckBox();
            this.ChkISSKELETON = new System.Windows.Forms.CheckBox();
            this.ChkISWEM = new System.Windows.Forms.CheckBox();
            this.ChkISCOMMITTEE = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.TxtISWEMDATA = new System.Windows.Forms.TextBox();
            this.TxtISBRAINTESTDATE = new System.Windows.Forms.TextBox();
            this.TxtISEARPROGRAMDATE = new System.Windows.Forms.TextBox();
            this.TxtISSKELETONDATE = new System.Windows.Forms.TextBox();
            this.TxtISSTRESSDATE = new System.Windows.Forms.TextBox();
            this.TxtISSPACEPROGRAMDATE = new System.Windows.Forms.TextBox();
            this.SSWorkerList = new FarPoint.Win.Spread.FpSpread();
            this.SSWorkerList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.label47 = new System.Windows.Forms.Label();
            this.NumMANAGEWORKERCOUNT = new System.Windows.Forms.NumericUpDown();
            this.label41 = new System.Windows.Forms.Label();
            this.NumMANAGEENGINEERCOUNT = new System.Windows.Forms.NumericUpDown();
            this.NumMANAGENURSECOUNT = new System.Windows.Forms.NumericUpDown();
            this.label32 = new System.Windows.Forms.Label();
            this.NumMANAGEDOCTORCOUNT = new System.Windows.Forms.NumericUpDown();
            this.DtpMANAGEDOCTORSTARTDATE = new System.Windows.Forms.DateTimePicker();
            this.DtpMANAGEENGINEERSTARTDATE = new System.Windows.Forms.DateTimePicker();
            this.DtpMANAGENURSESTARTDATE = new System.Windows.Forms.DateTimePicker();
            this.label31 = new System.Windows.Forms.Label();
            this.CboMANAGEENGINEER = new System.Windows.Forms.ComboBox();
            this.CboManageNurse = new System.Windows.Forms.ComboBox();
            this.label30 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.CboManageDoctor = new System.Windows.Forms.ComboBox();
            this.label28 = new System.Windows.Forms.Label();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.TxtWORKMEETTIME = new System.Windows.Forms.TextBox();
            this.TxtWORKENDTIME = new System.Windows.Forms.TextBox();
            this.TxtWORKSTARTTIME = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.TxtWORKETCTIME = new System.Windows.Forms.TextBox();
            this.TxtWORKEDUTIME = new System.Windows.Forms.TextBox();
            this.TxtWORKRESTTIME = new System.Windows.Forms.TextBox();
            this.TxtWORKLUANCHTIME = new System.Windows.Forms.TextBox();
            this.TxtWORKROTATIONTIME = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.DtpTERMINATEDATE = new System.Windows.Forms.DateTimePicker();
            this.DtpDECLAREDAY = new System.Windows.Forms.DateTimePicker();
            this.label53 = new System.Windows.Forms.Label();
            this.TxtSPECIALCONTRACT = new System.Windows.Forms.TextBox();
            this.NumCOMMISSION = new System.Windows.Forms.NumericUpDown();
            this.label39 = new System.Windows.Forms.Label();
            this.TxtVISITWEEK = new System.Windows.Forms.TextBox();
            this.TxtVISITDAY = new System.Windows.Forms.TextBox();
            this.label38 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.DtpCONTRACTENDDATE = new System.Windows.Forms.DateTimePicker();
            this.DtpCONTRACTSTARTDATE = new System.Windows.Forms.DateTimePicker();
            this.label33 = new System.Windows.Forms.Label();
            this.DtpCONTRACTDATE = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.RdoISLABOR_1 = new System.Windows.Forms.RadioButton();
            this.RdoISLABOR_0 = new System.Windows.Forms.RadioButton();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.RdoISPRODUCTTYPE_1 = new System.Windows.Forms.RadioButton();
            this.RdoISPRODUCTTYPE_0 = new System.Windows.Forms.RadioButton();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.RdoBUILDINGTYPE_1 = new System.Windows.Forms.RadioButton();
            this.RdoBUILDINGTYPE_0 = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.RdoISROTATION_1 = new System.Windows.Forms.RadioButton();
            this.RdoISROTATION_0 = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.RdoPosition_3 = new System.Windows.Forms.RadioButton();
            this.RdoPosition_2 = new System.Windows.Forms.RadioButton();
            this.RdoPosition_1 = new System.Windows.Forms.RadioButton();
            this.RdoPosition_0 = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.NumContractWORKERTOTALCOUNT = new System.Windows.Forms.NumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.NumWORKERWHITEFEMALECOUNT = new System.Windows.Forms.NumericUpDown();
            this.NumWORKERBLUEFEMALECOUNT = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.NumWORKERWHITEMALECOUNT = new System.Windows.Forms.NumericUpDown();
            this.NumWORKERBLUEMALECOUNT = new System.Windows.Forms.NumericUpDown();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panBottmo = new System.Windows.Forms.Panel();
            this.SSChildPrice = new FarPoint.Win.Spread.FpSpread();
            this.SSChildPrice_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label65 = new System.Windows.Forms.Label();
            this.TxtTotalPrice = new System.Windows.Forms.TextBox();
            this.label64 = new System.Windows.Forms.Label();
            this.TxtTotalUnitPrice = new System.Windows.Forms.TextBox();
            this.label55 = new System.Windows.Forms.Label();
            this.TxtTotalWorkerCount = new System.Windows.Forms.TextBox();
            this.label54 = new System.Windows.Forms.Label();
            this.BtnSaveChildPrice = new System.Windows.Forms.Button();
            this.panTop = new System.Windows.Forms.Panel();
            this.SSPrice = new FarPoint.Win.Spread.FpSpread();
            this.SSPrice_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.PanPrice = new System.Windows.Forms.Panel();
            this.ChkQuarterCharge = new System.Windows.Forms.CheckBox();
            this.ChkCharge = new System.Windows.Forms.CheckBox();
            this.NumTOTALPRICE = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.NumUNITTOTALPRICE = new System.Windows.Forms.NumericUpDown();
            this.BtnNewPrice = new System.Windows.Forms.Button();
            this.ChkIsBill = new System.Windows.Forms.CheckBox();
            this.ChkIsFix = new System.Windows.Forms.CheckBox();
            this.BtnDeletePrice = new System.Windows.Forms.Button();
            this.BtnSavePrice = new System.Windows.Forms.Button();
            this.label40 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.NumUNITPRICE = new System.Windows.Forms.NumericUpDown();
            this.NumPriceWORKERTOTALCOUNT = new System.Windows.Forms.NumericUpDown();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.BtnPdf = new System.Windows.Forms.Button();
            this.label57 = new System.Windows.Forms.Label();
            this.txtReceiveEmail = new System.Windows.Forms.TextBox();
            this.label56 = new System.Windows.Forms.Label();
            this.txtHalthManager = new System.Windows.Forms.TextBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.formTItle1 = new ComBase.Mvc.UserControls.FormTItle();
            this.oshaSiteLastTree = new HC_OSHA.OshaSiteLastTree();
            this.lblSitename = new System.Windows.Forms.Label();
            this.panTab = new System.Windows.Forms.Panel();
            this.panLeft = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.PanEstimate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumOFFICIALFEE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumMONTHLYFEE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumWORKERTOTALCOUNT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumSITEFEE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSEstimate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSEstimate_Sheet1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.PanContract.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSWorkerList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSWorkerList_Sheet1)).BeginInit();
            this.groupBox11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumMANAGEWORKERCOUNT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumMANAGEENGINEERCOUNT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumMANAGENURSECOUNT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumMANAGEDOCTORCOUNT)).BeginInit();
            this.groupBox10.SuspendLayout();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumCOMMISSION)).BeginInit();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumContractWORKERTOTALCOUNT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumWORKERWHITEFEMALECOUNT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumWORKERBLUEFEMALECOUNT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumWORKERWHITEMALECOUNT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumWORKERBLUEMALECOUNT)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.panBottmo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSChildPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSChildPrice_Sheet1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSPrice_Sheet1)).BeginInit();
            this.PanPrice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumTOTALPRICE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUNITTOTALPRICE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUNITPRICE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumPriceWORKERTOTALCOUNT)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.panTab.SuspendLayout();
            this.panLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanEstimate
            // 
            this.PanEstimate.BackColor = System.Drawing.Color.White;
            this.PanEstimate.Controls.Add(this.BtnDeleteParent);
            this.PanEstimate.Controls.Add(this.label11);
            this.PanEstimate.Controls.Add(this.label51);
            this.PanEstimate.Controls.Add(this.TxtBlueFeMale);
            this.PanEstimate.Controls.Add(this.TxtBlueMale);
            this.PanEstimate.Controls.Add(this.label52);
            this.PanEstimate.Controls.Add(this.label49);
            this.PanEstimate.Controls.Add(this.label48);
            this.PanEstimate.Controls.Add(this.TxtWhiteFeMale);
            this.PanEstimate.Controls.Add(this.TxtWhiteMale);
            this.PanEstimate.Controls.Add(this.label3);
            this.PanEstimate.Controls.Add(this.label63);
            this.PanEstimate.Controls.Add(this.TxtPRINTDATE);
            this.PanEstimate.Controls.Add(this.label62);
            this.PanEstimate.Controls.Add(this.TxtSENDMAILDATE);
            this.PanEstimate.Controls.Add(this.label61);
            this.PanEstimate.Controls.Add(this.TxtRemark);
            this.PanEstimate.Controls.Add(this.label60);
            this.PanEstimate.Controls.Add(this.NumOFFICIALFEE);
            this.PanEstimate.Controls.Add(this.label9);
            this.PanEstimate.Controls.Add(this.label8);
            this.PanEstimate.Controls.Add(this.label6);
            this.PanEstimate.Controls.Add(this.label59);
            this.PanEstimate.Controls.Add(this.label50);
            this.PanEstimate.Controls.Add(this.TxtParentSiteName);
            this.PanEstimate.Controls.Add(this.BtnSearchParent);
            this.PanEstimate.Controls.Add(this.label4);
            this.PanEstimate.Controls.Add(this.TxtParentSiteId);
            this.PanEstimate.Controls.Add(this.BtnSaveEstimate);
            this.PanEstimate.Controls.Add(this.NumMONTHLYFEE);
            this.PanEstimate.Controls.Add(this.BtnDeleteEstimate);
            this.PanEstimate.Controls.Add(this.btnNewEstimate);
            this.PanEstimate.Controls.Add(this.NumWORKERTOTALCOUNT);
            this.PanEstimate.Controls.Add(this.DtpESTIMATEDATE);
            this.PanEstimate.Controls.Add(this.DtpSTARTDATE);
            this.PanEstimate.Controls.Add(this.NumSITEFEE);
            this.PanEstimate.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanEstimate.Location = new System.Drawing.Point(3, 3);
            this.PanEstimate.Name = "PanEstimate";
            this.PanEstimate.Size = new System.Drawing.Size(1217, 227);
            this.PanEstimate.TabIndex = 1;
            // 
            // BtnDeleteParent
            // 
            this.BtnDeleteParent.Location = new System.Drawing.Point(387, 9);
            this.BtnDeleteParent.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnDeleteParent.Name = "BtnDeleteParent";
            this.BtnDeleteParent.Size = new System.Drawing.Size(124, 27);
            this.BtnDeleteParent.TabIndex = 100;
            this.BtnDeleteParent.Text = "하청 해제";
            this.BtnDeleteParent.UseVisualStyleBackColor = true;
            this.BtnDeleteParent.Click += new System.EventHandler(this.BtnDeleteParent_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(740, 142);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(21, 17);
            this.label11.TabIndex = 99;
            this.label11.Text = "여";
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(652, 142);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(21, 17);
            this.label51.TabIndex = 98;
            this.label51.Text = "남";
            // 
            // TxtBlueFeMale
            // 
            this.TxtBlueFeMale.Location = new System.Drawing.Point(764, 142);
            this.TxtBlueFeMale.Name = "TxtBlueFeMale";
            this.TxtBlueFeMale.Size = new System.Drawing.Size(47, 25);
            this.TxtBlueFeMale.TabIndex = 97;
            this.TxtBlueFeMale.Tag = "BLUEFEMALE";
            // 
            // TxtBlueMale
            // 
            this.TxtBlueMale.Location = new System.Drawing.Point(677, 142);
            this.TxtBlueMale.Name = "TxtBlueMale";
            this.TxtBlueMale.Size = new System.Drawing.Size(47, 25);
            this.TxtBlueMale.TabIndex = 96;
            this.TxtBlueMale.Tag = "BLUEMALE";
            // 
            // label52
            // 
            this.label52.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label52.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label52.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label52.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label52.Location = new System.Drawing.Point(655, 114);
            this.label52.Name = "label52";
            this.label52.Padding = new System.Windows.Forms.Padding(3);
            this.label52.Size = new System.Drawing.Size(210, 25);
            this.label52.TabIndex = 95;
            this.label52.Text = "생산";
            this.label52.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(537, 142);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(21, 17);
            this.label49.TabIndex = 94;
            this.label49.Text = "여";
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(449, 142);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(21, 17);
            this.label48.TabIndex = 93;
            this.label48.Text = "남";
            // 
            // TxtWhiteFeMale
            // 
            this.TxtWhiteFeMale.Location = new System.Drawing.Point(561, 142);
            this.TxtWhiteFeMale.Name = "TxtWhiteFeMale";
            this.TxtWhiteFeMale.Size = new System.Drawing.Size(47, 25);
            this.TxtWhiteFeMale.TabIndex = 92;
            this.TxtWhiteFeMale.Tag = "WHITEFEMALE";
            // 
            // TxtWhiteMale
            // 
            this.TxtWhiteMale.Location = new System.Drawing.Point(474, 142);
            this.TxtWhiteMale.Name = "TxtWhiteMale";
            this.TxtWhiteMale.Size = new System.Drawing.Size(47, 25);
            this.TxtWhiteMale.TabIndex = 91;
            this.TxtWhiteMale.Tag = "WHITEMALE";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(452, 114);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(3);
            this.label3.Size = new System.Drawing.Size(197, 25);
            this.label3.TabIndex = 89;
            this.label3.Text = "사무";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label63
            // 
            this.label63.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label63.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label63.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label63.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label63.Location = new System.Drawing.Point(352, 187);
            this.label63.Name = "label63";
            this.label63.Padding = new System.Windows.Forms.Padding(3);
            this.label63.Size = new System.Drawing.Size(96, 25);
            this.label63.TabIndex = 88;
            this.label63.Text = "견적출력일시";
            this.label63.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TxtPRINTDATE
            // 
            this.TxtPRINTDATE.Enabled = false;
            this.TxtPRINTDATE.Location = new System.Drawing.Point(452, 188);
            this.TxtPRINTDATE.Name = "TxtPRINTDATE";
            this.TxtPRINTDATE.Size = new System.Drawing.Size(225, 25);
            this.TxtPRINTDATE.TabIndex = 64;
            // 
            // label62
            // 
            this.label62.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label62.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label62.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label62.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label62.Location = new System.Drawing.Point(3, 187);
            this.label62.Name = "label62";
            this.label62.Padding = new System.Windows.Forms.Padding(3);
            this.label62.Size = new System.Drawing.Size(96, 25);
            this.label62.TabIndex = 87;
            this.label62.Text = "메일발송일시";
            this.label62.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TxtSENDMAILDATE
            // 
            this.TxtSENDMAILDATE.Location = new System.Drawing.Point(116, 187);
            this.TxtSENDMAILDATE.Name = "TxtSENDMAILDATE";
            this.TxtSENDMAILDATE.Size = new System.Drawing.Size(230, 25);
            this.TxtSENDMAILDATE.TabIndex = 63;
            // 
            // label61
            // 
            this.label61.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label61.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label61.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label61.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label61.Location = new System.Drawing.Point(3, 114);
            this.label61.Name = "label61";
            this.label61.Padding = new System.Windows.Forms.Padding(3);
            this.label61.Size = new System.Drawing.Size(96, 25);
            this.label61.TabIndex = 86;
            this.label61.Text = "비고";
            this.label61.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TxtRemark
            // 
            this.TxtRemark.Location = new System.Drawing.Point(116, 114);
            this.TxtRemark.Multiline = true;
            this.TxtRemark.Name = "TxtRemark";
            this.TxtRemark.Size = new System.Drawing.Size(325, 62);
            this.TxtRemark.TabIndex = 46;
            // 
            // label60
            // 
            this.label60.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label60.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label60.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label60.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label60.Location = new System.Drawing.Point(656, 83);
            this.label60.Name = "label60";
            this.label60.Padding = new System.Windows.Forms.Padding(3);
            this.label60.Size = new System.Drawing.Size(96, 25);
            this.label60.TabIndex = 85;
            this.label60.Text = "공표수수료";
            this.label60.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NumOFFICIALFEE
            // 
            this.NumOFFICIALFEE.Location = new System.Drawing.Point(758, 83);
            this.NumOFFICIALFEE.Name = "NumOFFICIALFEE";
            this.NumOFFICIALFEE.Size = new System.Drawing.Size(107, 25);
            this.NumOFFICIALFEE.TabIndex = 57;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label9.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(452, 83);
            this.label9.Name = "label9";
            this.label9.Padding = new System.Windows.Forms.Padding(3);
            this.label9.Size = new System.Drawing.Size(96, 25);
            this.label9.TabIndex = 84;
            this.label9.Text = "월별수수료";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label8.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(3, 83);
            this.label8.Name = "label8";
            this.label8.Padding = new System.Windows.Forms.Padding(3);
            this.label8.Size = new System.Drawing.Size(96, 25);
            this.label8.TabIndex = 83;
            this.label8.Text = "총근로자수";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label6.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(230, 81);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(3);
            this.label6.Size = new System.Drawing.Size(96, 25);
            this.label6.TabIndex = 82;
            this.label6.Text = "적용단가";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label59
            // 
            this.label59.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label59.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label59.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label59.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label59.Location = new System.Drawing.Point(230, 52);
            this.label59.Name = "label59";
            this.label59.Padding = new System.Windows.Forms.Padding(3);
            this.label59.Size = new System.Drawing.Size(96, 25);
            this.label59.TabIndex = 81;
            this.label59.Text = "업무시작일";
            this.label59.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label50
            // 
            this.label50.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label50.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label50.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label50.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label50.Location = new System.Drawing.Point(3, 54);
            this.label50.Name = "label50";
            this.label50.Padding = new System.Windows.Forms.Padding(3);
            this.label50.Size = new System.Drawing.Size(96, 25);
            this.label50.TabIndex = 80;
            this.label50.Text = "발행일";
            this.label50.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TxtParentSiteName
            // 
            this.TxtParentSiteName.Location = new System.Drawing.Point(172, 10);
            this.TxtParentSiteName.Name = "TxtParentSiteName";
            this.TxtParentSiteName.ReadOnly = true;
            this.TxtParentSiteName.Size = new System.Drawing.Size(149, 25);
            this.TxtParentSiteName.TabIndex = 79;
            // 
            // BtnSearchParent
            // 
            this.BtnSearchParent.Location = new System.Drawing.Point(323, 9);
            this.BtnSearchParent.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearchParent.Name = "BtnSearchParent";
            this.BtnSearchParent.Size = new System.Drawing.Size(58, 27);
            this.BtnSearchParent.TabIndex = 78;
            this.BtnSearchParent.Text = "검색";
            this.BtnSearchParent.UseVisualStyleBackColor = true;
            this.BtnSearchParent.Click += new System.EventHandler(this.BtnSearchParent_Click);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(3, 10);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(3);
            this.label4.Size = new System.Drawing.Size(96, 25);
            this.label4.TabIndex = 77;
            this.label4.Text = "원청 사업장";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TxtParentSiteId
            // 
            this.TxtParentSiteId.Location = new System.Drawing.Point(102, 10);
            this.TxtParentSiteId.Name = "TxtParentSiteId";
            this.TxtParentSiteId.ReadOnly = true;
            this.TxtParentSiteId.Size = new System.Drawing.Size(65, 25);
            this.TxtParentSiteId.TabIndex = 76;
            // 
            // BtnSaveEstimate
            // 
            this.BtnSaveEstimate.Location = new System.Drawing.Point(898, 7);
            this.BtnSaveEstimate.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSaveEstimate.Name = "BtnSaveEstimate";
            this.BtnSaveEstimate.Size = new System.Drawing.Size(75, 28);
            this.BtnSaveEstimate.TabIndex = 11;
            this.BtnSaveEstimate.Text = "저장(&S)";
            this.BtnSaveEstimate.UseVisualStyleBackColor = true;
            this.BtnSaveEstimate.Click += new System.EventHandler(this.BtnEstimateSave_Click);
            // 
            // NumMONTHLYFEE
            // 
            this.NumMONTHLYFEE.Enabled = false;
            this.NumMONTHLYFEE.Location = new System.Drawing.Point(552, 83);
            this.NumMONTHLYFEE.Name = "NumMONTHLYFEE";
            this.NumMONTHLYFEE.Size = new System.Drawing.Size(98, 25);
            this.NumMONTHLYFEE.TabIndex = 60;
            // 
            // BtnDeleteEstimate
            // 
            this.BtnDeleteEstimate.Location = new System.Drawing.Point(817, 7);
            this.BtnDeleteEstimate.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnDeleteEstimate.Name = "BtnDeleteEstimate";
            this.BtnDeleteEstimate.Size = new System.Drawing.Size(75, 28);
            this.BtnDeleteEstimate.TabIndex = 16;
            this.BtnDeleteEstimate.Text = "삭제";
            this.BtnDeleteEstimate.UseVisualStyleBackColor = true;
            this.BtnDeleteEstimate.Click += new System.EventHandler(this.BtnDeleteEstimate_Click);
            // 
            // btnNewEstimate
            // 
            this.btnNewEstimate.Location = new System.Drawing.Point(725, 7);
            this.btnNewEstimate.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.btnNewEstimate.Name = "btnNewEstimate";
            this.btnNewEstimate.Size = new System.Drawing.Size(86, 28);
            this.btnNewEstimate.TabIndex = 12;
            this.btnNewEstimate.Text = "화면정리";
            this.btnNewEstimate.UseVisualStyleBackColor = true;
            this.btnNewEstimate.Click += new System.EventHandler(this.btnNewEstimate_Click);
            // 
            // NumWORKERTOTALCOUNT
            // 
            this.NumWORKERTOTALCOUNT.Location = new System.Drawing.Point(118, 83);
            this.NumWORKERTOTALCOUNT.Name = "NumWORKERTOTALCOUNT";
            this.NumWORKERTOTALCOUNT.Size = new System.Drawing.Size(106, 25);
            this.NumWORKERTOTALCOUNT.TabIndex = 59;
            this.NumWORKERTOTALCOUNT.ValueChanged += new System.EventHandler(this.NumWORKERTOTALCOUNT_ValueChanged);
            // 
            // DtpESTIMATEDATE
            // 
            this.DtpESTIMATEDATE.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpESTIMATEDATE.Location = new System.Drawing.Point(116, 52);
            this.DtpESTIMATEDATE.Name = "DtpESTIMATEDATE";
            this.DtpESTIMATEDATE.Size = new System.Drawing.Size(106, 25);
            this.DtpESTIMATEDATE.TabIndex = 0;
            // 
            // DtpSTARTDATE
            // 
            this.DtpSTARTDATE.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.DtpSTARTDATE.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpSTARTDATE.Location = new System.Drawing.Point(332, 52);
            this.DtpSTARTDATE.Name = "DtpSTARTDATE";
            this.DtpSTARTDATE.Size = new System.Drawing.Size(107, 25);
            this.DtpSTARTDATE.TabIndex = 3;
            // 
            // NumSITEFEE
            // 
            this.NumSITEFEE.Location = new System.Drawing.Point(332, 83);
            this.NumSITEFEE.Name = "NumSITEFEE";
            this.NumSITEFEE.Size = new System.Drawing.Size(109, 25);
            this.NumSITEFEE.TabIndex = 58;
            this.NumSITEFEE.ValueChanged += new System.EventHandler(this.NumSITEFEE_ValueChanged);
            // 
            // BtnLoadExcel
            // 
            this.BtnLoadExcel.Location = new System.Drawing.Point(618, 231);
            this.BtnLoadExcel.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnLoadExcel.Name = "BtnLoadExcel";
            this.BtnLoadExcel.Size = new System.Drawing.Size(82, 28);
            this.BtnLoadExcel.TabIndex = 67;
            this.BtnLoadExcel.Text = "견적서 test";
            this.BtnLoadExcel.UseVisualStyleBackColor = true;
            this.BtnLoadExcel.Visible = false;
            this.BtnLoadExcel.Click += new System.EventHandler(this.BtnLoadExcel_Click);
            // 
            // SSEstimate
            // 
            this.SSEstimate.AccessibleDescription = "";
            this.SSEstimate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSEstimate.Location = new System.Drawing.Point(3, 262);
            this.SSEstimate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SSEstimate.Name = "SSEstimate";
            this.SSEstimate.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSEstimate_Sheet1});
            this.SSEstimate.Size = new System.Drawing.Size(979, 607);
            this.SSEstimate.TabIndex = 66;
            this.SSEstimate.SetActiveViewport(0, -1, -1);
            // 
            // SSEstimate_Sheet1
            // 
            this.SSEstimate_Sheet1.Reset();
            this.SSEstimate_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSEstimate_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSEstimate_Sheet1.ColumnCount = 0;
            this.SSEstimate_Sheet1.RowCount = 0;
            this.SSEstimate_Sheet1.ActiveColumnIndex = -1;
            this.SSEstimate_Sheet1.ActiveRowIndex = -1;
            this.SSEstimate_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSEstimate_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSEstimate_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSEstimate_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSEstimate_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSEstimate_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSEstimate_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSEstimate_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSEstimate_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSEstimate_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // BtnPrintEstimate
            // 
            this.BtnPrintEstimate.Location = new System.Drawing.Point(905, 231);
            this.BtnPrintEstimate.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnPrintEstimate.Name = "BtnPrintEstimate";
            this.BtnPrintEstimate.Size = new System.Drawing.Size(75, 28);
            this.BtnPrintEstimate.TabIndex = 14;
            this.BtnPrintEstimate.Text = "인쇄";
            this.BtnPrintEstimate.UseVisualStyleBackColor = true;
            this.BtnPrintEstimate.Click += new System.EventHandler(this.BtnPrintEstimate_Click);
            // 
            // BtnSendMailEstimate
            // 
            this.BtnSendMailEstimate.Location = new System.Drawing.Point(824, 231);
            this.BtnSendMailEstimate.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSendMailEstimate.Name = "BtnSendMailEstimate";
            this.BtnSendMailEstimate.Size = new System.Drawing.Size(75, 28);
            this.BtnSendMailEstimate.TabIndex = 15;
            this.BtnSendMailEstimate.Text = "메일발송";
            this.BtnSendMailEstimate.UseVisualStyleBackColor = true;
            this.BtnSendMailEstimate.Click += new System.EventHandler(this.BtnSendMailEstimate_Click);
            // 
            // BtnSiteConfitm
            // 
            this.BtnSiteConfitm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSiteConfitm.Location = new System.Drawing.Point(1125, 5);
            this.BtnSiteConfitm.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSiteConfitm.Name = "BtnSiteConfitm";
            this.BtnSiteConfitm.Size = new System.Drawing.Size(75, 28);
            this.BtnSiteConfitm.TabIndex = 27;
            this.BtnSiteConfitm.Text = "확인";
            this.BtnSiteConfitm.UseVisualStyleBackColor = true;
            this.BtnSiteConfitm.Visible = false;
            this.BtnSiteConfitm.Click += new System.EventHandler(this.BtnSiteConfirm_Click);
            // 
            // TxtSiteName
            // 
            this.TxtSiteName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.TxtSiteName.Location = new System.Drawing.Point(838, 8);
            this.TxtSiteName.Name = "TxtSiteName";
            this.TxtSiteName.ReadOnly = true;
            this.TxtSiteName.Size = new System.Drawing.Size(269, 25);
            this.TxtSiteName.TabIndex = 26;
            this.TxtSiteName.Visible = false;
            // 
            // TxtOshaSiteId
            // 
            this.TxtOshaSiteId.Location = new System.Drawing.Point(767, 8);
            this.TxtOshaSiteId.Name = "TxtOshaSiteId";
            this.TxtOshaSiteId.ReadOnly = true;
            this.TxtOshaSiteId.Size = new System.Drawing.Size(67, 25);
            this.TxtOshaSiteId.TabIndex = 24;
            this.TxtOshaSiteId.Visible = false;
            // 
            // OshaSiteEstimateList
            // 
            this.OshaSiteEstimateList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OshaSiteEstimateList.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
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
            this.OshaSiteEstimateList.GetEstimateModel = hC_ESTIMATE_MODEL1;
            this.OshaSiteEstimateList.Location = new System.Drawing.Point(0, 586);
            this.OshaSiteEstimateList.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.OshaSiteEstimateList.Name = "OshaSiteEstimateList";
            this.OshaSiteEstimateList.Size = new System.Drawing.Size(216, 379);
            this.OshaSiteEstimateList.TabIndex = 29;
            this.OshaSiteEstimateList.CellDoubleClick += new HC_OSHA.OshaSiteEstimateList.CellDoubleClickEventHandler(this.OshaSiteEstimateList_CellDoubleClick);
            this.OshaSiteEstimateList.Load += new System.EventHandler(this.OshaSiteEstimateList_Load);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ItemSize = new System.Drawing.Size(100, 28);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1231, 965);
            this.tabControl1.TabIndex = 34;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.PanContract);
            this.tabPage2.Location = new System.Drawing.Point(4, 32);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1223, 929);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "사업장 현황";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // PanContract
            // 
            this.PanContract.AutoScroll = true;
            this.PanContract.BackColor = System.Drawing.Color.White;
            this.PanContract.Controls.Add(this.BtnClear);
            this.PanContract.Controls.Add(this.BtnDeleteContract);
            this.PanContract.Controls.Add(this.BtnLastContract);
            this.PanContract.Controls.Add(this.BtnWorkerAdd);
            this.PanContract.Controls.Add(this.BtnSaveContract);
            this.PanContract.Controls.Add(this.groupBox1);
            this.PanContract.Controls.Add(this.SSWorkerList);
            this.PanContract.Controls.Add(this.groupBox11);
            this.PanContract.Controls.Add(this.groupBox10);
            this.PanContract.Controls.Add(this.groupBox9);
            this.PanContract.Controls.Add(this.groupBox8);
            this.PanContract.Controls.Add(this.groupBox7);
            this.PanContract.Controls.Add(this.groupBox6);
            this.PanContract.Controls.Add(this.groupBox5);
            this.PanContract.Controls.Add(this.groupBox4);
            this.PanContract.Controls.Add(this.groupBox3);
            this.PanContract.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanContract.Location = new System.Drawing.Point(3, 3);
            this.PanContract.Name = "PanContract";
            this.PanContract.Size = new System.Drawing.Size(1217, 923);
            this.PanContract.TabIndex = 32;
            this.PanContract.Paint += new System.Windows.Forms.PaintEventHandler(this.PanContract_Paint);
            // 
            // BtnClear
            // 
            this.BtnClear.Location = new System.Drawing.Point(810, 4);
            this.BtnClear.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(75, 28);
            this.BtnClear.TabIndex = 82;
            this.BtnClear.Text = "화면정리";
            this.BtnClear.UseVisualStyleBackColor = true;
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // BtnDeleteContract
            // 
            this.BtnDeleteContract.Location = new System.Drawing.Point(729, 4);
            this.BtnDeleteContract.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnDeleteContract.Name = "BtnDeleteContract";
            this.BtnDeleteContract.Size = new System.Drawing.Size(75, 28);
            this.BtnDeleteContract.TabIndex = 81;
            this.BtnDeleteContract.Text = "삭제";
            this.BtnDeleteContract.UseVisualStyleBackColor = true;
            this.BtnDeleteContract.Click += new System.EventHandler(this.BtnDeleteContract_Click);
            // 
            // BtnLastContract
            // 
            this.BtnLastContract.Location = new System.Drawing.Point(553, 4);
            this.BtnLastContract.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnLastContract.Name = "BtnLastContract";
            this.BtnLastContract.Size = new System.Drawing.Size(152, 28);
            this.BtnLastContract.TabIndex = 80;
            this.BtnLastContract.Text = "작년 현황 복사하기";
            this.BtnLastContract.UseVisualStyleBackColor = true;
            this.BtnLastContract.Click += new System.EventHandler(this.BtnLastContract_Click);
            // 
            // BtnWorkerAdd
            // 
            this.BtnWorkerAdd.Location = new System.Drawing.Point(10, 425);
            this.BtnWorkerAdd.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnWorkerAdd.Name = "BtnWorkerAdd";
            this.BtnWorkerAdd.Size = new System.Drawing.Size(175, 28);
            this.BtnWorkerAdd.TabIndex = 79;
            this.BtnWorkerAdd.Text = "사업장 직원 정보 가져오기";
            this.BtnWorkerAdd.UseVisualStyleBackColor = true;
            this.BtnWorkerAdd.Click += new System.EventHandler(this.BtnWorkerAdd_Click);
            // 
            // BtnSaveContract
            // 
            this.BtnSaveContract.Location = new System.Drawing.Point(891, 4);
            this.BtnSaveContract.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSaveContract.Name = "BtnSaveContract";
            this.BtnSaveContract.Size = new System.Drawing.Size(75, 28);
            this.BtnSaveContract.TabIndex = 79;
            this.BtnSaveContract.Text = "저장(&S)";
            this.BtnSaveContract.UseVisualStyleBackColor = true;
            this.BtnSaveContract.Click += new System.EventHandler(this.BtnSaveContract_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label58);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.TxtISSPECIALDATA);
            this.groupBox1.Controls.Add(this.label44);
            this.groupBox1.Controls.Add(this.label45);
            this.groupBox1.Controls.Add(this.label43);
            this.groupBox1.Controls.Add(this.label42);
            this.groupBox1.Controls.Add(this.label46);
            this.groupBox1.Controls.Add(this.ChkISSPECIAL);
            this.groupBox1.Controls.Add(this.ChkISBRAINTEST);
            this.groupBox1.Controls.Add(this.ChkISSTRESS);
            this.groupBox1.Controls.Add(this.ChkISSPACEPROGRAM);
            this.groupBox1.Controls.Add(this.ChkISEARPROGRAM);
            this.groupBox1.Controls.Add(this.ChkISSKELETON);
            this.groupBox1.Controls.Add(this.ChkISWEM);
            this.groupBox1.Controls.Add(this.ChkISCOMMITTEE);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.TxtISWEMDATA);
            this.groupBox1.Controls.Add(this.TxtISBRAINTESTDATE);
            this.groupBox1.Controls.Add(this.TxtISEARPROGRAMDATE);
            this.groupBox1.Controls.Add(this.TxtISSKELETONDATE);
            this.groupBox1.Controls.Add(this.TxtISSTRESSDATE);
            this.groupBox1.Controls.Add(this.TxtISSPACEPROGRAMDATE);
            this.groupBox1.Location = new System.Drawing.Point(7, 650);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(955, 185);
            this.groupBox1.TabIndex = 70;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "추가항목";
            // 
            // label58
            // 
            this.label58.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label58.Location = new System.Drawing.Point(704, 19);
            this.label58.Name = "label58";
            this.label58.Padding = new System.Windows.Forms.Padding(3);
            this.label58.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label58.Size = new System.Drawing.Size(89, 25);
            this.label58.TabIndex = 79;
            this.label58.Text = "참고사항";
            this.label58.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(704, 50);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(238, 127);
            this.textBox1.TabIndex = 78;
            this.textBox1.Tag = "REMARK";
            // 
            // TxtISSPECIALDATA
            // 
            this.TxtISSPECIALDATA.Location = new System.Drawing.Point(162, 152);
            this.TxtISSPECIALDATA.Name = "TxtISSPECIALDATA";
            this.TxtISSPECIALDATA.Size = new System.Drawing.Size(536, 25);
            this.TxtISSPECIALDATA.TabIndex = 77;
            // 
            // label44
            // 
            this.label44.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label44.Location = new System.Drawing.Point(211, 53);
            this.label44.Name = "label44";
            this.label44.Padding = new System.Windows.Forms.Padding(3);
            this.label44.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label44.Size = new System.Drawing.Size(54, 25);
            this.label44.TabIndex = 76;
            this.label44.Text = "실시일";
            this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label45
            // 
            this.label45.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label45.Location = new System.Drawing.Point(211, 117);
            this.label45.Name = "label45";
            this.label45.Padding = new System.Windows.Forms.Padding(3);
            this.label45.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label45.Size = new System.Drawing.Size(54, 25);
            this.label45.TabIndex = 76;
            this.label45.Text = "실시일";
            this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label43
            // 
            this.label43.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label43.Location = new System.Drawing.Point(211, 83);
            this.label43.Name = "label43";
            this.label43.Padding = new System.Windows.Forms.Padding(3);
            this.label43.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label43.Size = new System.Drawing.Size(54, 25);
            this.label43.TabIndex = 76;
            this.label43.Text = "실시일";
            this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label42
            // 
            this.label42.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label42.Location = new System.Drawing.Point(560, 51);
            this.label42.Name = "label42";
            this.label42.Padding = new System.Windows.Forms.Padding(3);
            this.label42.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label42.Size = new System.Drawing.Size(56, 25);
            this.label42.TabIndex = 75;
            this.label42.Text = "실시일";
            this.label42.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label46
            // 
            this.label46.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label46.Location = new System.Drawing.Point(559, 82);
            this.label46.Name = "label46";
            this.label46.Padding = new System.Windows.Forms.Padding(3);
            this.label46.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label46.Size = new System.Drawing.Size(53, 25);
            this.label46.TabIndex = 75;
            this.label46.Text = "실시일";
            this.label46.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ChkISSPECIAL
            // 
            this.ChkISSPECIAL.AutoSize = true;
            this.ChkISSPECIAL.Location = new System.Drawing.Point(11, 154);
            this.ChkISSPECIAL.Name = "ChkISSPECIAL";
            this.ChkISSPECIAL.Size = new System.Drawing.Size(131, 21);
            this.ChkISSPECIAL.TabIndex = 73;
            this.ChkISSPECIAL.Text = "특별관리물질취급";
            this.ChkISSPECIAL.UseVisualStyleBackColor = true;
            // 
            // ChkISBRAINTEST
            // 
            this.ChkISBRAINTEST.AutoSize = true;
            this.ChkISBRAINTEST.Location = new System.Drawing.Point(10, 123);
            this.ChkISBRAINTEST.Name = "ChkISBRAINTEST";
            this.ChkISBRAINTEST.Size = new System.Drawing.Size(196, 21);
            this.ChkISBRAINTEST.TabIndex = 73;
            this.ChkISBRAINTEST.Text = "뇌심혈관질환위험도평가대상";
            this.ChkISBRAINTEST.UseVisualStyleBackColor = true;
            // 
            // ChkISSTRESS
            // 
            this.ChkISSTRESS.AutoSize = true;
            this.ChkISSTRESS.Location = new System.Drawing.Point(362, 83);
            this.ChkISSTRESS.Name = "ChkISSTRESS";
            this.ChkISSTRESS.Size = new System.Drawing.Size(162, 21);
            this.ChkISSTRESS.TabIndex = 74;
            this.ChkISSTRESS.Text = "직무스트레스평가 대상";
            this.ChkISSTRESS.UseVisualStyleBackColor = true;
            // 
            // ChkISSPACEPROGRAM
            // 
            this.ChkISSPACEPROGRAM.AutoSize = true;
            this.ChkISSPACEPROGRAM.Location = new System.Drawing.Point(362, 54);
            this.ChkISSPACEPROGRAM.Name = "ChkISSPACEPROGRAM";
            this.ChkISSPACEPROGRAM.Size = new System.Drawing.Size(193, 21);
            this.ChkISSPACEPROGRAM.TabIndex = 74;
            this.ChkISSPACEPROGRAM.Text = "밀폐공간보건프로그램 대상 ";
            this.ChkISSPACEPROGRAM.UseVisualStyleBackColor = true;
            // 
            // ChkISEARPROGRAM
            // 
            this.ChkISEARPROGRAM.AutoSize = true;
            this.ChkISEARPROGRAM.Location = new System.Drawing.Point(10, 88);
            this.ChkISEARPROGRAM.Name = "ChkISEARPROGRAM";
            this.ChkISEARPROGRAM.Size = new System.Drawing.Size(162, 21);
            this.ChkISEARPROGRAM.TabIndex = 73;
            this.ChkISEARPROGRAM.Text = "청력보존프로그램 대상";
            this.ChkISEARPROGRAM.UseVisualStyleBackColor = true;
            // 
            // ChkISSKELETON
            // 
            this.ChkISSKELETON.AutoSize = true;
            this.ChkISSKELETON.Location = new System.Drawing.Point(10, 56);
            this.ChkISSKELETON.Name = "ChkISSKELETON";
            this.ChkISSKELETON.Size = new System.Drawing.Size(193, 21);
            this.ChkISSKELETON.TabIndex = 73;
            this.ChkISSKELETON.Text = "근골격계 유해요인조사 대상";
            this.ChkISSKELETON.UseVisualStyleBackColor = true;
            // 
            // ChkISWEM
            // 
            this.ChkISWEM.AutoSize = true;
            this.ChkISWEM.Location = new System.Drawing.Point(10, 26);
            this.ChkISWEM.Name = "ChkISWEM";
            this.ChkISWEM.Size = new System.Drawing.Size(136, 21);
            this.ChkISWEM.TabIndex = 72;
            this.ChkISWEM.Text = "작업환경측정 대상";
            this.ChkISWEM.UseVisualStyleBackColor = true;
            // 
            // ChkISCOMMITTEE
            // 
            this.ChkISCOMMITTEE.AutoSize = true;
            this.ChkISCOMMITTEE.Location = new System.Drawing.Point(362, 23);
            this.ChkISCOMMITTEE.Name = "ChkISCOMMITTEE";
            this.ChkISCOMMITTEE.Size = new System.Drawing.Size(175, 21);
            this.ChkISCOMMITTEE.TabIndex = 71;
            this.ChkISCOMMITTEE.Text = "산업안전보건위원회 설치";
            this.ChkISCOMMITTEE.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(285, 21);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(3);
            this.label7.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label7.Size = new System.Drawing.Size(79, 25);
            this.label7.TabIndex = 70;
            this.label7.Text = "에서 측정";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TxtISWEMDATA
            // 
            this.TxtISWEMDATA.Location = new System.Drawing.Point(165, 23);
            this.TxtISWEMDATA.Name = "TxtISWEMDATA";
            this.TxtISWEMDATA.Size = new System.Drawing.Size(106, 25);
            this.TxtISWEMDATA.TabIndex = 67;
            // 
            // TxtISBRAINTESTDATE
            // 
            this.TxtISBRAINTESTDATE.Location = new System.Drawing.Point(271, 119);
            this.TxtISBRAINTESTDATE.Name = "TxtISBRAINTESTDATE";
            this.TxtISBRAINTESTDATE.Size = new System.Drawing.Size(85, 25);
            this.TxtISBRAINTESTDATE.TabIndex = 67;
            // 
            // TxtISEARPROGRAMDATE
            // 
            this.TxtISEARPROGRAMDATE.Location = new System.Drawing.Point(271, 85);
            this.TxtISEARPROGRAMDATE.Name = "TxtISEARPROGRAMDATE";
            this.TxtISEARPROGRAMDATE.Size = new System.Drawing.Size(85, 25);
            this.TxtISEARPROGRAMDATE.TabIndex = 67;
            // 
            // TxtISSKELETONDATE
            // 
            this.TxtISSKELETONDATE.Location = new System.Drawing.Point(271, 54);
            this.TxtISSKELETONDATE.Name = "TxtISSKELETONDATE";
            this.TxtISSKELETONDATE.Size = new System.Drawing.Size(85, 25);
            this.TxtISSKELETONDATE.TabIndex = 67;
            // 
            // TxtISSTRESSDATE
            // 
            this.TxtISSTRESSDATE.Location = new System.Drawing.Point(623, 82);
            this.TxtISSTRESSDATE.Name = "TxtISSTRESSDATE";
            this.TxtISSTRESSDATE.Size = new System.Drawing.Size(75, 25);
            this.TxtISSTRESSDATE.TabIndex = 68;
            // 
            // TxtISSPACEPROGRAMDATE
            // 
            this.TxtISSPACEPROGRAMDATE.Location = new System.Drawing.Point(623, 50);
            this.TxtISSPACEPROGRAMDATE.Name = "TxtISSPACEPROGRAMDATE";
            this.TxtISSPACEPROGRAMDATE.Size = new System.Drawing.Size(75, 25);
            this.TxtISSPACEPROGRAMDATE.TabIndex = 68;
            // 
            // SSWorkerList
            // 
            this.SSWorkerList.AccessibleDescription = "";
            this.SSWorkerList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSWorkerList.Location = new System.Drawing.Point(10, 459);
            this.SSWorkerList.Name = "SSWorkerList";
            this.SSWorkerList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSWorkerList_Sheet1});
            this.SSWorkerList.Size = new System.Drawing.Size(955, 184);
            this.SSWorkerList.TabIndex = 66;
            this.SSWorkerList.SetActiveViewport(0, -1, -1);
            // 
            // SSWorkerList_Sheet1
            // 
            this.SSWorkerList_Sheet1.Reset();
            this.SSWorkerList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSWorkerList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSWorkerList_Sheet1.ColumnCount = 0;
            this.SSWorkerList_Sheet1.RowCount = 0;
            this.SSWorkerList_Sheet1.ActiveColumnIndex = -1;
            this.SSWorkerList_Sheet1.ActiveRowIndex = -1;
            this.SSWorkerList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSWorkerList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSWorkerList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSWorkerList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSWorkerList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSWorkerList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSWorkerList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSWorkerList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSWorkerList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSWorkerList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.label47);
            this.groupBox11.Controls.Add(this.NumMANAGEWORKERCOUNT);
            this.groupBox11.Controls.Add(this.label41);
            this.groupBox11.Controls.Add(this.NumMANAGEENGINEERCOUNT);
            this.groupBox11.Controls.Add(this.NumMANAGENURSECOUNT);
            this.groupBox11.Controls.Add(this.label32);
            this.groupBox11.Controls.Add(this.NumMANAGEDOCTORCOUNT);
            this.groupBox11.Controls.Add(this.DtpMANAGEDOCTORSTARTDATE);
            this.groupBox11.Controls.Add(this.DtpMANAGEENGINEERSTARTDATE);
            this.groupBox11.Controls.Add(this.DtpMANAGENURSESTARTDATE);
            this.groupBox11.Controls.Add(this.label31);
            this.groupBox11.Controls.Add(this.CboMANAGEENGINEER);
            this.groupBox11.Controls.Add(this.CboManageNurse);
            this.groupBox11.Controls.Add(this.label30);
            this.groupBox11.Controls.Add(this.label29);
            this.groupBox11.Controls.Add(this.CboManageDoctor);
            this.groupBox11.Controls.Add(this.label28);
            this.groupBox11.Location = new System.Drawing.Point(10, 243);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(431, 180);
            this.groupBox11.TabIndex = 63;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "담당요원 및 방문주기";
            // 
            // label47
            // 
            this.label47.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label47.Location = new System.Drawing.Point(15, 144);
            this.label47.Name = "label47";
            this.label47.Padding = new System.Windows.Forms.Padding(3);
            this.label47.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label47.Size = new System.Drawing.Size(66, 25);
            this.label47.TabIndex = 73;
            this.label47.Text = "총인원";
            this.label47.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumMANAGEWORKERCOUNT
            // 
            this.NumMANAGEWORKERCOUNT.Location = new System.Drawing.Point(87, 144);
            this.NumMANAGEWORKERCOUNT.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NumMANAGEWORKERCOUNT.Name = "NumMANAGEWORKERCOUNT";
            this.NumMANAGEWORKERCOUNT.Size = new System.Drawing.Size(121, 25);
            this.NumMANAGEWORKERCOUNT.TabIndex = 72;
            // 
            // label41
            // 
            this.label41.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label41.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label41.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label41.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label41.Location = new System.Drawing.Point(86, 20);
            this.label41.Name = "label41";
            this.label41.Padding = new System.Windows.Forms.Padding(3);
            this.label41.Size = new System.Drawing.Size(121, 25);
            this.label41.TabIndex = 71;
            this.label41.Text = "담당요원";
            this.label41.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NumMANAGEENGINEERCOUNT
            // 
            this.NumMANAGEENGINEERCOUNT.Location = new System.Drawing.Point(343, 108);
            this.NumMANAGEENGINEERCOUNT.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NumMANAGEENGINEERCOUNT.Name = "NumMANAGEENGINEERCOUNT";
            this.NumMANAGEENGINEERCOUNT.Size = new System.Drawing.Size(71, 25);
            this.NumMANAGEENGINEERCOUNT.TabIndex = 70;
            // 
            // NumMANAGENURSECOUNT
            // 
            this.NumMANAGENURSECOUNT.Location = new System.Drawing.Point(343, 77);
            this.NumMANAGENURSECOUNT.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NumMANAGENURSECOUNT.Name = "NumMANAGENURSECOUNT";
            this.NumMANAGENURSECOUNT.Size = new System.Drawing.Size(71, 25);
            this.NumMANAGENURSECOUNT.TabIndex = 69;
            // 
            // label32
            // 
            this.label32.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label32.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label32.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label32.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label32.Location = new System.Drawing.Point(343, 20);
            this.label32.Name = "label32";
            this.label32.Padding = new System.Windows.Forms.Padding(3);
            this.label32.Size = new System.Drawing.Size(71, 25);
            this.label32.TabIndex = 68;
            this.label32.Text = "방문주기";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NumMANAGEDOCTORCOUNT
            // 
            this.NumMANAGEDOCTORCOUNT.Location = new System.Drawing.Point(343, 48);
            this.NumMANAGEDOCTORCOUNT.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NumMANAGEDOCTORCOUNT.Name = "NumMANAGEDOCTORCOUNT";
            this.NumMANAGEDOCTORCOUNT.Size = new System.Drawing.Size(71, 25);
            this.NumMANAGEDOCTORCOUNT.TabIndex = 67;
            // 
            // DtpMANAGEDOCTORSTARTDATE
            // 
            this.DtpMANAGEDOCTORSTARTDATE.CustomFormat = "yyyy-MM";
            this.DtpMANAGEDOCTORSTARTDATE.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpMANAGEDOCTORSTARTDATE.Location = new System.Drawing.Point(224, 47);
            this.DtpMANAGEDOCTORSTARTDATE.Name = "DtpMANAGEDOCTORSTARTDATE";
            this.DtpMANAGEDOCTORSTARTDATE.Size = new System.Drawing.Size(107, 25);
            this.DtpMANAGEDOCTORSTARTDATE.TabIndex = 66;
            // 
            // DtpMANAGEENGINEERSTARTDATE
            // 
            this.DtpMANAGEENGINEERSTARTDATE.CustomFormat = "yyyy-MM";
            this.DtpMANAGEENGINEERSTARTDATE.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpMANAGEENGINEERSTARTDATE.Location = new System.Drawing.Point(224, 108);
            this.DtpMANAGEENGINEERSTARTDATE.Name = "DtpMANAGEENGINEERSTARTDATE";
            this.DtpMANAGEENGINEERSTARTDATE.Size = new System.Drawing.Size(107, 25);
            this.DtpMANAGEENGINEERSTARTDATE.TabIndex = 65;
            // 
            // DtpMANAGENURSESTARTDATE
            // 
            this.DtpMANAGENURSESTARTDATE.CustomFormat = "yyyy-MM";
            this.DtpMANAGENURSESTARTDATE.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpMANAGENURSESTARTDATE.Location = new System.Drawing.Point(224, 77);
            this.DtpMANAGENURSESTARTDATE.Name = "DtpMANAGENURSESTARTDATE";
            this.DtpMANAGENURSESTARTDATE.Size = new System.Drawing.Size(107, 25);
            this.DtpMANAGENURSESTARTDATE.TabIndex = 64;
            // 
            // label31
            // 
            this.label31.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label31.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label31.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label31.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label31.Location = new System.Drawing.Point(224, 20);
            this.label31.Name = "label31";
            this.label31.Padding = new System.Windows.Forms.Padding(3);
            this.label31.Size = new System.Drawing.Size(107, 25);
            this.label31.TabIndex = 61;
            this.label31.Text = "시작년월";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CboMANAGEENGINEER
            // 
            this.CboMANAGEENGINEER.FormattingEnabled = true;
            this.CboMANAGEENGINEER.Location = new System.Drawing.Point(86, 107);
            this.CboMANAGEENGINEER.Name = "CboMANAGEENGINEER";
            this.CboMANAGEENGINEER.Size = new System.Drawing.Size(121, 25);
            this.CboMANAGEENGINEER.TabIndex = 45;
            // 
            // CboManageNurse
            // 
            this.CboManageNurse.FormattingEnabled = true;
            this.CboManageNurse.Location = new System.Drawing.Point(86, 77);
            this.CboManageNurse.Name = "CboManageNurse";
            this.CboManageNurse.Size = new System.Drawing.Size(121, 25);
            this.CboManageNurse.TabIndex = 44;
            // 
            // label30
            // 
            this.label30.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label30.Location = new System.Drawing.Point(13, 106);
            this.label30.Name = "label30";
            this.label30.Padding = new System.Windows.Forms.Padding(3);
            this.label30.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label30.Size = new System.Drawing.Size(66, 25);
            this.label30.TabIndex = 43;
            this.label30.Text = "산업위생";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label29
            // 
            this.label29.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label29.Location = new System.Drawing.Point(13, 78);
            this.label29.Name = "label29";
            this.label29.Padding = new System.Windows.Forms.Padding(3);
            this.label29.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label29.Size = new System.Drawing.Size(66, 25);
            this.label29.TabIndex = 42;
            this.label29.Text = "간호사";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CboManageDoctor
            // 
            this.CboManageDoctor.FormattingEnabled = true;
            this.CboManageDoctor.Location = new System.Drawing.Point(86, 47);
            this.CboManageDoctor.Name = "CboManageDoctor";
            this.CboManageDoctor.Size = new System.Drawing.Size(121, 25);
            this.CboManageDoctor.TabIndex = 41;
            // 
            // label28
            // 
            this.label28.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label28.Location = new System.Drawing.Point(14, 48);
            this.label28.Name = "label28";
            this.label28.Padding = new System.Windows.Forms.Padding(3);
            this.label28.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label28.Size = new System.Drawing.Size(66, 25);
            this.label28.TabIndex = 40;
            this.label28.Text = "의사";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.TxtWORKMEETTIME);
            this.groupBox10.Controls.Add(this.TxtWORKENDTIME);
            this.groupBox10.Controls.Add(this.TxtWORKSTARTTIME);
            this.groupBox10.Controls.Add(this.label27);
            this.groupBox10.Controls.Add(this.TxtWORKETCTIME);
            this.groupBox10.Controls.Add(this.TxtWORKEDUTIME);
            this.groupBox10.Controls.Add(this.TxtWORKRESTTIME);
            this.groupBox10.Controls.Add(this.TxtWORKLUANCHTIME);
            this.groupBox10.Controls.Add(this.TxtWORKROTATIONTIME);
            this.groupBox10.Controls.Add(this.label26);
            this.groupBox10.Controls.Add(this.label25);
            this.groupBox10.Controls.Add(this.label24);
            this.groupBox10.Controls.Add(this.label23);
            this.groupBox10.Controls.Add(this.label22);
            this.groupBox10.Controls.Add(this.label21);
            this.groupBox10.Controls.Add(this.label14);
            this.groupBox10.Controls.Add(this.label2);
            this.groupBox10.Location = new System.Drawing.Point(456, 218);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(510, 237);
            this.groupBox10.TabIndex = 62;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "사업장 업무일정";
            // 
            // TxtWORKMEETTIME
            // 
            this.TxtWORKMEETTIME.Location = new System.Drawing.Point(409, 24);
            this.TxtWORKMEETTIME.Name = "TxtWORKMEETTIME";
            this.TxtWORKMEETTIME.Size = new System.Drawing.Size(84, 25);
            this.TxtWORKMEETTIME.TabIndex = 85;
            this.TxtWORKMEETTIME.Tag = "WORKMEETTIME";
            // 
            // TxtWORKENDTIME
            // 
            this.TxtWORKENDTIME.Location = new System.Drawing.Point(243, 24);
            this.TxtWORKENDTIME.Name = "TxtWORKENDTIME";
            this.TxtWORKENDTIME.Size = new System.Drawing.Size(84, 25);
            this.TxtWORKENDTIME.TabIndex = 84;
            this.TxtWORKENDTIME.Tag = "WORKENDTIME";
            // 
            // TxtWORKSTARTTIME
            // 
            this.TxtWORKSTARTTIME.Location = new System.Drawing.Point(72, 24);
            this.TxtWORKSTARTTIME.Name = "TxtWORKSTARTTIME";
            this.TxtWORKSTARTTIME.Size = new System.Drawing.Size(84, 25);
            this.TxtWORKSTARTTIME.TabIndex = 83;
            this.TxtWORKSTARTTIME.Tag = "WORKSTARTTIME";
            // 
            // label27
            // 
            this.label27.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label27.Location = new System.Drawing.Point(430, 164);
            this.label27.Name = "label27";
            this.label27.Padding = new System.Windows.Forms.Padding(3);
            this.label27.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label27.Size = new System.Drawing.Size(66, 25);
            this.label27.TabIndex = 49;
            this.label27.Text = "(시간/월)";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TxtWORKETCTIME
            // 
            this.TxtWORKETCTIME.Location = new System.Drawing.Point(72, 200);
            this.TxtWORKETCTIME.Name = "TxtWORKETCTIME";
            this.TxtWORKETCTIME.Size = new System.Drawing.Size(433, 25);
            this.TxtWORKETCTIME.TabIndex = 48;
            // 
            // TxtWORKEDUTIME
            // 
            this.TxtWORKEDUTIME.Location = new System.Drawing.Point(73, 165);
            this.TxtWORKEDUTIME.Name = "TxtWORKEDUTIME";
            this.TxtWORKEDUTIME.Size = new System.Drawing.Size(349, 25);
            this.TxtWORKEDUTIME.TabIndex = 48;
            // 
            // TxtWORKRESTTIME
            // 
            this.TxtWORKRESTTIME.Location = new System.Drawing.Point(73, 131);
            this.TxtWORKRESTTIME.Name = "TxtWORKRESTTIME";
            this.TxtWORKRESTTIME.Size = new System.Drawing.Size(432, 25);
            this.TxtWORKRESTTIME.TabIndex = 48;
            // 
            // TxtWORKLUANCHTIME
            // 
            this.TxtWORKLUANCHTIME.Location = new System.Drawing.Point(72, 98);
            this.TxtWORKLUANCHTIME.Name = "TxtWORKLUANCHTIME";
            this.TxtWORKLUANCHTIME.Size = new System.Drawing.Size(433, 25);
            this.TxtWORKLUANCHTIME.TabIndex = 48;
            // 
            // TxtWORKROTATIONTIME
            // 
            this.TxtWORKROTATIONTIME.Location = new System.Drawing.Point(72, 63);
            this.TxtWORKROTATIONTIME.Name = "TxtWORKROTATIONTIME";
            this.TxtWORKROTATIONTIME.Size = new System.Drawing.Size(433, 25);
            this.TxtWORKROTATIONTIME.TabIndex = 48;
            // 
            // label26
            // 
            this.label26.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label26.Location = new System.Drawing.Point(6, 197);
            this.label26.Name = "label26";
            this.label26.Padding = new System.Windows.Forms.Padding(3);
            this.label26.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label26.Size = new System.Drawing.Size(66, 25);
            this.label26.TabIndex = 39;
            this.label26.Text = "기     타";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label25
            // 
            this.label25.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label25.Location = new System.Drawing.Point(6, 167);
            this.label25.Name = "label25";
            this.label25.Padding = new System.Windows.Forms.Padding(3);
            this.label25.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label25.Size = new System.Drawing.Size(66, 25);
            this.label25.TabIndex = 39;
            this.label25.Text = "교육시간";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label24
            // 
            this.label24.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label24.Location = new System.Drawing.Point(6, 137);
            this.label24.Name = "label24";
            this.label24.Padding = new System.Windows.Forms.Padding(3);
            this.label24.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label24.Size = new System.Drawing.Size(66, 25);
            this.label24.TabIndex = 39;
            this.label24.Text = "휴식시간";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label23
            // 
            this.label23.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label23.Location = new System.Drawing.Point(5, 98);
            this.label23.Name = "label23";
            this.label23.Padding = new System.Windows.Forms.Padding(3);
            this.label23.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label23.Size = new System.Drawing.Size(66, 25);
            this.label23.TabIndex = 39;
            this.label23.Text = "점심시간";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label22
            // 
            this.label22.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label22.Location = new System.Drawing.Point(5, 62);
            this.label22.Name = "label22";
            this.label22.Padding = new System.Windows.Forms.Padding(3);
            this.label22.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label22.Size = new System.Drawing.Size(66, 25);
            this.label22.TabIndex = 39;
            this.label22.Text = "교대시간";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label21
            // 
            this.label21.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label21.Location = new System.Drawing.Point(339, 25);
            this.label21.Name = "label21";
            this.label21.Padding = new System.Windows.Forms.Padding(3);
            this.label21.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label21.Size = new System.Drawing.Size(66, 25);
            this.label21.TabIndex = 38;
            this.label21.Text = "조회시간";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.Location = new System.Drawing.Point(174, 25);
            this.label14.Name = "label14";
            this.label14.Padding = new System.Windows.Forms.Padding(3);
            this.label14.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label14.Size = new System.Drawing.Size(66, 25);
            this.label14.TabIndex = 36;
            this.label14.Text = "종료시간";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(3, 25);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(3);
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label2.Size = new System.Drawing.Size(66, 25);
            this.label2.TabIndex = 34;
            this.label2.Text = "시작시간";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.DtpTERMINATEDATE);
            this.groupBox9.Controls.Add(this.DtpDECLAREDAY);
            this.groupBox9.Controls.Add(this.label53);
            this.groupBox9.Controls.Add(this.TxtSPECIALCONTRACT);
            this.groupBox9.Controls.Add(this.NumCOMMISSION);
            this.groupBox9.Controls.Add(this.label39);
            this.groupBox9.Controls.Add(this.TxtVISITWEEK);
            this.groupBox9.Controls.Add(this.TxtVISITDAY);
            this.groupBox9.Controls.Add(this.label38);
            this.groupBox9.Controls.Add(this.label37);
            this.groupBox9.Controls.Add(this.label36);
            this.groupBox9.Controls.Add(this.label35);
            this.groupBox9.Controls.Add(this.label34);
            this.groupBox9.Controls.Add(this.DtpCONTRACTENDDATE);
            this.groupBox9.Controls.Add(this.DtpCONTRACTSTARTDATE);
            this.groupBox9.Controls.Add(this.label33);
            this.groupBox9.Controls.Add(this.DtpCONTRACTDATE);
            this.groupBox9.Controls.Add(this.label1);
            this.groupBox9.Location = new System.Drawing.Point(7, 31);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(958, 100);
            this.groupBox9.TabIndex = 63;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "계약 및 근무일";
            // 
            // DtpTERMINATEDATE
            // 
            this.DtpTERMINATEDATE.Checked = false;
            this.DtpTERMINATEDATE.CustomFormat = "yyyy-MM-dd";
            this.DtpTERMINATEDATE.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpTERMINATEDATE.Location = new System.Drawing.Point(619, 23);
            this.DtpTERMINATEDATE.Name = "DtpTERMINATEDATE";
            this.DtpTERMINATEDATE.ShowCheckBox = true;
            this.DtpTERMINATEDATE.Size = new System.Drawing.Size(107, 25);
            this.DtpTERMINATEDATE.TabIndex = 82;
            // 
            // DtpDECLAREDAY
            // 
            this.DtpDECLAREDAY.CustomFormat = "yyyy-MM-dd";
            this.DtpDECLAREDAY.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpDECLAREDAY.Location = new System.Drawing.Point(810, 23);
            this.DtpDECLAREDAY.Name = "DtpDECLAREDAY";
            this.DtpDECLAREDAY.Size = new System.Drawing.Size(108, 25);
            this.DtpDECLAREDAY.TabIndex = 81;
            this.DtpDECLAREDAY.Tag = "";
            // 
            // label53
            // 
            this.label53.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label53.Location = new System.Drawing.Point(537, 22);
            this.label53.Name = "label53";
            this.label53.Padding = new System.Windows.Forms.Padding(3);
            this.label53.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label53.Size = new System.Drawing.Size(82, 25);
            this.label53.TabIndex = 80;
            this.label53.Text = "계약해지일";
            this.label53.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TxtSPECIALCONTRACT
            // 
            this.TxtSPECIALCONTRACT.Location = new System.Drawing.Point(503, 61);
            this.TxtSPECIALCONTRACT.Name = "TxtSPECIALCONTRACT";
            this.TxtSPECIALCONTRACT.Size = new System.Drawing.Size(415, 25);
            this.TxtSPECIALCONTRACT.TabIndex = 78;
            // 
            // NumCOMMISSION
            // 
            this.NumCOMMISSION.Location = new System.Drawing.Point(341, 60);
            this.NumCOMMISSION.Maximum = new decimal(new int[] {
            999000,
            0,
            0,
            0});
            this.NumCOMMISSION.Name = "NumCOMMISSION";
            this.NumCOMMISSION.Size = new System.Drawing.Size(71, 25);
            this.NumCOMMISSION.TabIndex = 61;
            // 
            // label39
            // 
            this.label39.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label39.Location = new System.Drawing.Point(294, 59);
            this.label39.Name = "label39";
            this.label39.Padding = new System.Windows.Forms.Padding(3);
            this.label39.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label39.Size = new System.Drawing.Size(49, 25);
            this.label39.TabIndex = 76;
            this.label39.Text = "1인당";
            this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TxtVISITWEEK
            // 
            this.TxtVISITWEEK.Location = new System.Drawing.Point(70, 59);
            this.TxtVISITWEEK.Name = "TxtVISITWEEK";
            this.TxtVISITWEEK.Size = new System.Drawing.Size(66, 25);
            this.TxtVISITWEEK.TabIndex = 75;
            // 
            // TxtVISITDAY
            // 
            this.TxtVISITDAY.Location = new System.Drawing.Point(172, 60);
            this.TxtVISITDAY.Name = "TxtVISITDAY";
            this.TxtVISITDAY.Size = new System.Drawing.Size(62, 25);
            this.TxtVISITDAY.TabIndex = 74;
            // 
            // label38
            // 
            this.label38.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label38.Location = new System.Drawing.Point(434, 60);
            this.label38.Name = "label38";
            this.label38.Padding = new System.Windows.Forms.Padding(3);
            this.label38.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label38.Size = new System.Drawing.Size(66, 25);
            this.label38.TabIndex = 73;
            this.label38.Text = "특약사항";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label37
            // 
            this.label37.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label37.Location = new System.Drawing.Point(750, 23);
            this.label37.Name = "label37";
            this.label37.Padding = new System.Windows.Forms.Padding(3);
            this.label37.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label37.Size = new System.Drawing.Size(54, 25);
            this.label37.TabIndex = 72;
            this.label37.Text = "선임일";
            this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label36
            // 
            this.label36.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label36.Location = new System.Drawing.Point(142, 60);
            this.label36.Name = "label36";
            this.label36.Padding = new System.Windows.Forms.Padding(3);
            this.label36.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label36.Size = new System.Drawing.Size(28, 25);
            this.label36.TabIndex = 71;
            this.label36.Text = "주";
            this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label35
            // 
            this.label35.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label35.Location = new System.Drawing.Point(240, 60);
            this.label35.Name = "label35";
            this.label35.Padding = new System.Windows.Forms.Padding(3);
            this.label35.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label35.Size = new System.Drawing.Size(48, 25);
            this.label35.TabIndex = 70;
            this.label35.Text = "요일";
            this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label34
            // 
            this.label34.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label34.Location = new System.Drawing.Point(13, 60);
            this.label34.Name = "label34";
            this.label34.Padding = new System.Windows.Forms.Padding(3);
            this.label34.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label34.Size = new System.Drawing.Size(54, 25);
            this.label34.TabIndex = 69;
            this.label34.Text = "근무일";
            this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // DtpCONTRACTENDDATE
            // 
            this.DtpCONTRACTENDDATE.CustomFormat = "yyyy-MM-dd";
            this.DtpCONTRACTENDDATE.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpCONTRACTENDDATE.Location = new System.Drawing.Point(403, 23);
            this.DtpCONTRACTENDDATE.Name = "DtpCONTRACTENDDATE";
            this.DtpCONTRACTENDDATE.Size = new System.Drawing.Size(105, 25);
            this.DtpCONTRACTENDDATE.TabIndex = 68;
            // 
            // DtpCONTRACTSTARTDATE
            // 
            this.DtpCONTRACTSTARTDATE.CustomFormat = "yyyy-MM-dd";
            this.DtpCONTRACTSTARTDATE.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpCONTRACTSTARTDATE.Location = new System.Drawing.Point(281, 23);
            this.DtpCONTRACTSTARTDATE.Name = "DtpCONTRACTSTARTDATE";
            this.DtpCONTRACTSTARTDATE.Size = new System.Drawing.Size(107, 25);
            this.DtpCONTRACTSTARTDATE.TabIndex = 67;
            // 
            // label33
            // 
            this.label33.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label33.Location = new System.Drawing.Point(211, 23);
            this.label33.Name = "label33";
            this.label33.Padding = new System.Windows.Forms.Padding(3);
            this.label33.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label33.Size = new System.Drawing.Size(66, 25);
            this.label33.TabIndex = 36;
            this.label33.Text = "계약기간";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // DtpCONTRACTDATE
            // 
            this.DtpCONTRACTDATE.Checked = false;
            this.DtpCONTRACTDATE.CustomFormat = "yyyy-MM-dd";
            this.DtpCONTRACTDATE.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpCONTRACTDATE.Location = new System.Drawing.Point(70, 23);
            this.DtpCONTRACTDATE.Name = "DtpCONTRACTDATE";
            this.DtpCONTRACTDATE.Size = new System.Drawing.Size(126, 25);
            this.DtpCONTRACTDATE.TabIndex = 32;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(13, 23);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(3);
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(54, 25);
            this.label1.TabIndex = 33;
            this.label1.Text = "계약일";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.RdoISLABOR_1);
            this.groupBox8.Controls.Add(this.RdoISLABOR_0);
            this.groupBox8.Location = new System.Drawing.Point(127, 186);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(101, 51);
            this.groupBox8.TabIndex = 65;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "노동조합";
            // 
            // RdoISLABOR_1
            // 
            this.RdoISLABOR_1.AutoSize = true;
            this.RdoISLABOR_1.Location = new System.Drawing.Point(59, 20);
            this.RdoISLABOR_1.Name = "RdoISLABOR_1";
            this.RdoISLABOR_1.Size = new System.Drawing.Size(39, 21);
            this.RdoISLABOR_1.TabIndex = 1;
            this.RdoISLABOR_1.TabStop = true;
            this.RdoISLABOR_1.Text = "무";
            this.RdoISLABOR_1.UseVisualStyleBackColor = true;
            // 
            // RdoISLABOR_0
            // 
            this.RdoISLABOR_0.AutoSize = true;
            this.RdoISLABOR_0.Location = new System.Drawing.Point(10, 20);
            this.RdoISLABOR_0.Name = "RdoISLABOR_0";
            this.RdoISLABOR_0.Size = new System.Drawing.Size(39, 21);
            this.RdoISLABOR_0.TabIndex = 0;
            this.RdoISLABOR_0.TabStop = true;
            this.RdoISLABOR_0.Text = "유";
            this.RdoISLABOR_0.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.RdoISPRODUCTTYPE_1);
            this.groupBox7.Controls.Add(this.RdoISPRODUCTTYPE_0);
            this.groupBox7.Location = new System.Drawing.Point(239, 187);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(129, 51);
            this.groupBox7.TabIndex = 64;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "생산방식";
            // 
            // RdoISPRODUCTTYPE_1
            // 
            this.RdoISPRODUCTTYPE_1.AutoSize = true;
            this.RdoISPRODUCTTYPE_1.Location = new System.Drawing.Point(70, 20);
            this.RdoISPRODUCTTYPE_1.Name = "RdoISPRODUCTTYPE_1";
            this.RdoISPRODUCTTYPE_1.Size = new System.Drawing.Size(52, 21);
            this.RdoISPRODUCTTYPE_1.TabIndex = 1;
            this.RdoISPRODUCTTYPE_1.TabStop = true;
            this.RdoISPRODUCTTYPE_1.Text = "하청";
            this.RdoISPRODUCTTYPE_1.UseVisualStyleBackColor = true;
            // 
            // RdoISPRODUCTTYPE_0
            // 
            this.RdoISPRODUCTTYPE_0.AutoSize = true;
            this.RdoISPRODUCTTYPE_0.Location = new System.Drawing.Point(10, 20);
            this.RdoISPRODUCTTYPE_0.Name = "RdoISPRODUCTTYPE_0";
            this.RdoISPRODUCTTYPE_0.Size = new System.Drawing.Size(52, 21);
            this.RdoISPRODUCTTYPE_0.TabIndex = 0;
            this.RdoISPRODUCTTYPE_0.TabStop = true;
            this.RdoISPRODUCTTYPE_0.Text = "독립";
            this.RdoISPRODUCTTYPE_0.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.RdoBUILDINGTYPE_1);
            this.groupBox6.Controls.Add(this.RdoBUILDINGTYPE_0);
            this.groupBox6.Location = new System.Drawing.Point(312, 133);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(129, 51);
            this.groupBox6.TabIndex = 63;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "건물소유";
            // 
            // RdoBUILDINGTYPE_1
            // 
            this.RdoBUILDINGTYPE_1.AutoSize = true;
            this.RdoBUILDINGTYPE_1.Location = new System.Drawing.Point(70, 20);
            this.RdoBUILDINGTYPE_1.Name = "RdoBUILDINGTYPE_1";
            this.RdoBUILDINGTYPE_1.Size = new System.Drawing.Size(52, 21);
            this.RdoBUILDINGTYPE_1.TabIndex = 1;
            this.RdoBUILDINGTYPE_1.TabStop = true;
            this.RdoBUILDINGTYPE_1.Text = "임대";
            this.RdoBUILDINGTYPE_1.UseVisualStyleBackColor = true;
            // 
            // RdoBUILDINGTYPE_0
            // 
            this.RdoBUILDINGTYPE_0.AutoSize = true;
            this.RdoBUILDINGTYPE_0.Location = new System.Drawing.Point(10, 20);
            this.RdoBUILDINGTYPE_0.Name = "RdoBUILDINGTYPE_0";
            this.RdoBUILDINGTYPE_0.Size = new System.Drawing.Size(52, 21);
            this.RdoBUILDINGTYPE_0.TabIndex = 0;
            this.RdoBUILDINGTYPE_0.TabStop = true;
            this.RdoBUILDINGTYPE_0.Text = "자가";
            this.RdoBUILDINGTYPE_0.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.RdoISROTATION_1);
            this.groupBox5.Controls.Add(this.RdoISROTATION_0);
            this.groupBox5.Location = new System.Drawing.Point(10, 186);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(106, 51);
            this.groupBox5.TabIndex = 62;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "교대제";
            // 
            // RdoISROTATION_1
            // 
            this.RdoISROTATION_1.AutoSize = true;
            this.RdoISROTATION_1.Location = new System.Drawing.Point(61, 20);
            this.RdoISROTATION_1.Name = "RdoISROTATION_1";
            this.RdoISROTATION_1.Size = new System.Drawing.Size(39, 21);
            this.RdoISROTATION_1.TabIndex = 1;
            this.RdoISROTATION_1.TabStop = true;
            this.RdoISROTATION_1.Text = "무";
            this.RdoISROTATION_1.UseVisualStyleBackColor = true;
            // 
            // RdoISROTATION_0
            // 
            this.RdoISROTATION_0.AutoSize = true;
            this.RdoISROTATION_0.Location = new System.Drawing.Point(10, 20);
            this.RdoISROTATION_0.Name = "RdoISROTATION_0";
            this.RdoISROTATION_0.Size = new System.Drawing.Size(39, 21);
            this.RdoISROTATION_0.TabIndex = 0;
            this.RdoISROTATION_0.TabStop = true;
            this.RdoISROTATION_0.Text = "유";
            this.RdoISROTATION_0.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.RdoPosition_3);
            this.groupBox4.Controls.Add(this.RdoPosition_2);
            this.groupBox4.Controls.Add(this.RdoPosition_1);
            this.groupBox4.Controls.Add(this.RdoPosition_0);
            this.groupBox4.Location = new System.Drawing.Point(9, 134);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(296, 51);
            this.groupBox4.TabIndex = 61;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "위치";
            // 
            // RdoPosition_3
            // 
            this.RdoPosition_3.AutoSize = true;
            this.RdoPosition_3.Location = new System.Drawing.Point(240, 20);
            this.RdoPosition_3.Name = "RdoPosition_3";
            this.RdoPosition_3.Size = new System.Drawing.Size(52, 21);
            this.RdoPosition_3.TabIndex = 3;
            this.RdoPosition_3.TabStop = true;
            this.RdoPosition_3.Text = "기타";
            this.RdoPosition_3.UseVisualStyleBackColor = true;
            // 
            // RdoPosition_2
            // 
            this.RdoPosition_2.AutoSize = true;
            this.RdoPosition_2.Location = new System.Drawing.Point(166, 20);
            this.RdoPosition_2.Name = "RdoPosition_2";
            this.RdoPosition_2.Size = new System.Drawing.Size(65, 21);
            this.RdoPosition_2.TabIndex = 2;
            this.RdoPosition_2.TabStop = true;
            this.RdoPosition_2.Text = "도심내";
            this.RdoPosition_2.UseVisualStyleBackColor = true;
            // 
            // RdoPosition_1
            // 
            this.RdoPosition_1.AutoSize = true;
            this.RdoPosition_1.Location = new System.Drawing.Point(77, 20);
            this.RdoPosition_1.Name = "RdoPosition_1";
            this.RdoPosition_1.Size = new System.Drawing.Size(78, 21);
            this.RdoPosition_1.TabIndex = 1;
            this.RdoPosition_1.TabStop = true;
            this.RdoPosition_1.Text = "농공단지";
            this.RdoPosition_1.UseVisualStyleBackColor = true;
            // 
            // RdoPosition_0
            // 
            this.RdoPosition_0.AutoSize = true;
            this.RdoPosition_0.Location = new System.Drawing.Point(10, 20);
            this.RdoPosition_0.Name = "RdoPosition_0";
            this.RdoPosition_0.Size = new System.Drawing.Size(52, 21);
            this.RdoPosition_0.TabIndex = 0;
            this.RdoPosition_0.TabStop = true;
            this.RdoPosition_0.Text = "공단";
            this.RdoPosition_0.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.NumContractWORKERTOTALCOUNT);
            this.groupBox3.Controls.Add(this.label20);
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.NumWORKERWHITEFEMALECOUNT);
            this.groupBox3.Controls.Add(this.NumWORKERBLUEFEMALECOUNT);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Controls.Add(this.label19);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.NumWORKERWHITEMALECOUNT);
            this.groupBox3.Controls.Add(this.NumWORKERBLUEMALECOUNT);
            this.groupBox3.Location = new System.Drawing.Point(456, 131);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(509, 87);
            this.groupBox3.TabIndex = 37;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "근로자수";
            // 
            // NumContractWORKERTOTALCOUNT
            // 
            this.NumContractWORKERTOTALCOUNT.Location = new System.Drawing.Point(410, 56);
            this.NumContractWORKERTOTALCOUNT.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NumContractWORKERTOTALCOUNT.Name = "NumContractWORKERTOTALCOUNT";
            this.NumContractWORKERTOTALCOUNT.Size = new System.Drawing.Size(52, 25);
            this.NumContractWORKERTOTALCOUNT.TabIndex = 60;
            // 
            // label20
            // 
            this.label20.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label20.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label20.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label20.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label20.Location = new System.Drawing.Point(410, 21);
            this.label20.Name = "label20";
            this.label20.Padding = new System.Windows.Forms.Padding(3);
            this.label20.Size = new System.Drawing.Size(52, 25);
            this.label20.TabIndex = 59;
            this.label20.Text = "총원";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label17
            // 
            this.label17.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label17.Location = new System.Drawing.Point(304, 54);
            this.label17.Name = "label17";
            this.label17.Padding = new System.Windows.Forms.Padding(3);
            this.label17.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label17.Size = new System.Drawing.Size(29, 25);
            this.label17.TabIndex = 57;
            this.label17.Text = "여";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label16
            // 
            this.label16.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label16.Location = new System.Drawing.Point(104, 54);
            this.label16.Name = "label16";
            this.label16.Padding = new System.Windows.Forms.Padding(3);
            this.label16.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label16.Size = new System.Drawing.Size(29, 25);
            this.label16.TabIndex = 52;
            this.label16.Text = "여";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumWORKERWHITEFEMALECOUNT
            // 
            this.NumWORKERWHITEFEMALECOUNT.Location = new System.Drawing.Point(135, 56);
            this.NumWORKERWHITEFEMALECOUNT.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NumWORKERWHITEFEMALECOUNT.Name = "NumWORKERWHITEFEMALECOUNT";
            this.NumWORKERWHITEFEMALECOUNT.Size = new System.Drawing.Size(52, 25);
            this.NumWORKERWHITEFEMALECOUNT.TabIndex = 53;
            // 
            // NumWORKERBLUEFEMALECOUNT
            // 
            this.NumWORKERBLUEFEMALECOUNT.Location = new System.Drawing.Point(339, 56);
            this.NumWORKERBLUEFEMALECOUNT.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NumWORKERBLUEFEMALECOUNT.Name = "NumWORKERBLUEFEMALECOUNT";
            this.NumWORKERBLUEFEMALECOUNT.Size = new System.Drawing.Size(52, 25);
            this.NumWORKERBLUEFEMALECOUNT.TabIndex = 58;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label15.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.Location = new System.Drawing.Point(8, 21);
            this.label15.Name = "label15";
            this.label15.Padding = new System.Windows.Forms.Padding(3);
            this.label15.Size = new System.Drawing.Size(179, 25);
            this.label15.TabIndex = 51;
            this.label15.Text = "사무직";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label18.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label18.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label18.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label18.Location = new System.Drawing.Point(212, 21);
            this.label18.Name = "label18";
            this.label18.Padding = new System.Windows.Forms.Padding(3);
            this.label18.Size = new System.Drawing.Size(179, 25);
            this.label18.TabIndex = 56;
            this.label18.Text = "비사무직";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label19
            // 
            this.label19.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label19.Location = new System.Drawing.Point(212, 54);
            this.label19.Name = "label19";
            this.label19.Padding = new System.Windows.Forms.Padding(3);
            this.label19.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label19.Size = new System.Drawing.Size(29, 25);
            this.label19.TabIndex = 54;
            this.label19.Text = "남";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.Location = new System.Drawing.Point(8, 54);
            this.label13.Name = "label13";
            this.label13.Padding = new System.Windows.Forms.Padding(3);
            this.label13.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label13.Size = new System.Drawing.Size(29, 25);
            this.label13.TabIndex = 48;
            this.label13.Text = "남";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumWORKERWHITEMALECOUNT
            // 
            this.NumWORKERWHITEMALECOUNT.Location = new System.Drawing.Point(39, 56);
            this.NumWORKERWHITEMALECOUNT.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NumWORKERWHITEMALECOUNT.Name = "NumWORKERWHITEMALECOUNT";
            this.NumWORKERWHITEMALECOUNT.Size = new System.Drawing.Size(52, 25);
            this.NumWORKERWHITEMALECOUNT.TabIndex = 49;
            // 
            // NumWORKERBLUEMALECOUNT
            // 
            this.NumWORKERBLUEMALECOUNT.Location = new System.Drawing.Point(243, 56);
            this.NumWORKERBLUEMALECOUNT.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NumWORKERBLUEMALECOUNT.Name = "NumWORKERBLUEMALECOUNT";
            this.NumWORKERBLUEMALECOUNT.Size = new System.Drawing.Size(52, 25);
            this.NumWORKERBLUEMALECOUNT.TabIndex = 55;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.panBottmo);
            this.tabPage3.Controls.Add(this.panTop);
            this.tabPage3.Location = new System.Drawing.Point(4, 32);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1223, 929);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "계   약";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // panBottmo
            // 
            this.panBottmo.Controls.Add(this.SSChildPrice);
            this.panBottmo.Controls.Add(this.panel2);
            this.panBottmo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBottmo.Location = new System.Drawing.Point(3, 406);
            this.panBottmo.Name = "panBottmo";
            this.panBottmo.Size = new System.Drawing.Size(1217, 520);
            this.panBottmo.TabIndex = 68;
            // 
            // SSChildPrice
            // 
            this.SSChildPrice.AccessibleDescription = "";
            this.SSChildPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSChildPrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSChildPrice.Location = new System.Drawing.Point(0, 54);
            this.SSChildPrice.Name = "SSChildPrice";
            this.SSChildPrice.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSChildPrice_Sheet1});
            this.SSChildPrice.Size = new System.Drawing.Size(1217, 466);
            this.SSChildPrice.TabIndex = 68;
            this.SSChildPrice.Change += new FarPoint.Win.Spread.ChangeEventHandler(this.SSChildPrice_Change);
            // 
            // SSChildPrice_Sheet1
            // 
            this.SSChildPrice_Sheet1.Reset();
            this.SSChildPrice_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSChildPrice_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSChildPrice_Sheet1.ColumnCount = 0;
            this.SSChildPrice_Sheet1.RowCount = 0;
            this.SSChildPrice_Sheet1.ActiveColumnIndex = -1;
            this.SSChildPrice_Sheet1.ActiveRowIndex = -1;
            this.SSChildPrice_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSChildPrice_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSChildPrice_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSChildPrice_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSChildPrice_Sheet1.ColumnHeader.Rows.Get(0).Height = 38F;
            this.SSChildPrice_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSChildPrice_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSChildPrice_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSChildPrice_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSChildPrice_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSChildPrice_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label65);
            this.panel2.Controls.Add(this.TxtTotalPrice);
            this.panel2.Controls.Add(this.label64);
            this.panel2.Controls.Add(this.TxtTotalUnitPrice);
            this.panel2.Controls.Add(this.label55);
            this.panel2.Controls.Add(this.TxtTotalWorkerCount);
            this.panel2.Controls.Add(this.label54);
            this.panel2.Controls.Add(this.BtnSaveChildPrice);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1217, 54);
            this.panel2.TabIndex = 70;
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label65.Location = new System.Drawing.Point(6, 19);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(73, 17);
            this.label65.TabIndex = 97;
            this.label65.Text = "원하청계약";
            // 
            // TxtTotalPrice
            // 
            this.TxtTotalPrice.Location = new System.Drawing.Point(645, 16);
            this.TxtTotalPrice.Name = "TxtTotalPrice";
            this.TxtTotalPrice.ReadOnly = true;
            this.TxtTotalPrice.Size = new System.Drawing.Size(100, 25);
            this.TxtTotalPrice.TabIndex = 96;
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.Location = new System.Drawing.Point(579, 19);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(60, 17);
            this.label64.TabIndex = 95;
            this.label64.Text = "계약금액";
            // 
            // TxtTotalUnitPrice
            // 
            this.TxtTotalUnitPrice.Location = new System.Drawing.Point(453, 16);
            this.TxtTotalUnitPrice.Name = "TxtTotalUnitPrice";
            this.TxtTotalUnitPrice.ReadOnly = true;
            this.TxtTotalUnitPrice.Size = new System.Drawing.Size(100, 25);
            this.TxtTotalUnitPrice.TabIndex = 94;
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(387, 19);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(60, 17);
            this.label55.TabIndex = 93;
            this.label55.Text = "계산금액";
            // 
            // TxtTotalWorkerCount
            // 
            this.TxtTotalWorkerCount.Location = new System.Drawing.Point(276, 16);
            this.TxtTotalWorkerCount.Name = "TxtTotalWorkerCount";
            this.TxtTotalWorkerCount.ReadOnly = true;
            this.TxtTotalWorkerCount.Size = new System.Drawing.Size(100, 25);
            this.TxtTotalWorkerCount.TabIndex = 92;
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Location = new System.Drawing.Point(236, 19);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(34, 17);
            this.label54.TabIndex = 91;
            this.label54.Text = "인원";
            // 
            // BtnSaveChildPrice
            // 
            this.BtnSaveChildPrice.Location = new System.Drawing.Point(791, 13);
            this.BtnSaveChildPrice.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSaveChildPrice.Name = "BtnSaveChildPrice";
            this.BtnSaveChildPrice.Size = new System.Drawing.Size(75, 28);
            this.BtnSaveChildPrice.TabIndex = 90;
            this.BtnSaveChildPrice.Text = "저장(&S)";
            this.BtnSaveChildPrice.UseVisualStyleBackColor = true;
            this.BtnSaveChildPrice.Click += new System.EventHandler(this.BtnSaveChildPrice_Click);
            // 
            // panTop
            // 
            this.panTop.Controls.Add(this.SSPrice);
            this.panTop.Controls.Add(this.PanPrice);
            this.panTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTop.Location = new System.Drawing.Point(3, 3);
            this.panTop.Name = "panTop";
            this.panTop.Size = new System.Drawing.Size(1217, 403);
            this.panTop.TabIndex = 69;
            // 
            // SSPrice
            // 
            this.SSPrice.AccessibleDescription = "";
            this.SSPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSPrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSPrice.Location = new System.Drawing.Point(0, 81);
            this.SSPrice.Name = "SSPrice";
            this.SSPrice.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSPrice_Sheet1});
            this.SSPrice.Size = new System.Drawing.Size(1217, 322);
            this.SSPrice.TabIndex = 67;
            this.SSPrice.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSPrice_CellDoubleClick);
            // 
            // SSPrice_Sheet1
            // 
            this.SSPrice_Sheet1.Reset();
            this.SSPrice_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSPrice_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSPrice_Sheet1.ColumnCount = 0;
            this.SSPrice_Sheet1.RowCount = 0;
            this.SSPrice_Sheet1.ActiveColumnIndex = -1;
            this.SSPrice_Sheet1.ActiveRowIndex = -1;
            this.SSPrice_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSPrice_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSPrice_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSPrice_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSPrice_Sheet1.ColumnHeader.Rows.Get(0).Height = 38F;
            this.SSPrice_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSPrice_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSPrice_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSPrice_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSPrice_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSPrice_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // PanPrice
            // 
            this.PanPrice.Controls.Add(this.ChkQuarterCharge);
            this.PanPrice.Controls.Add(this.ChkCharge);
            this.PanPrice.Controls.Add(this.NumTOTALPRICE);
            this.PanPrice.Controls.Add(this.label12);
            this.PanPrice.Controls.Add(this.NumUNITTOTALPRICE);
            this.PanPrice.Controls.Add(this.BtnNewPrice);
            this.PanPrice.Controls.Add(this.ChkIsBill);
            this.PanPrice.Controls.Add(this.ChkIsFix);
            this.PanPrice.Controls.Add(this.BtnDeletePrice);
            this.PanPrice.Controls.Add(this.BtnSavePrice);
            this.PanPrice.Controls.Add(this.label40);
            this.PanPrice.Controls.Add(this.label10);
            this.PanPrice.Controls.Add(this.label5);
            this.PanPrice.Controls.Add(this.NumUNITPRICE);
            this.PanPrice.Controls.Add(this.NumPriceWORKERTOTALCOUNT);
            this.PanPrice.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanPrice.Location = new System.Drawing.Point(0, 0);
            this.PanPrice.Name = "PanPrice";
            this.PanPrice.Size = new System.Drawing.Size(1217, 81);
            this.PanPrice.TabIndex = 0;
            // 
            // ChkQuarterCharge
            // 
            this.ChkQuarterCharge.AutoSize = true;
            this.ChkQuarterCharge.Location = new System.Drawing.Point(673, 51);
            this.ChkQuarterCharge.Name = "ChkQuarterCharge";
            this.ChkQuarterCharge.Size = new System.Drawing.Size(110, 21);
            this.ChkQuarterCharge.TabIndex = 91;
            this.ChkQuarterCharge.Text = "분기청구 여부";
            this.ChkQuarterCharge.UseVisualStyleBackColor = true;
            // 
            // ChkCharge
            // 
            this.ChkCharge.AutoSize = true;
            this.ChkCharge.Location = new System.Drawing.Point(673, 19);
            this.ChkCharge.Name = "ChkCharge";
            this.ChkCharge.Size = new System.Drawing.Size(193, 21);
            this.ChkCharge.TabIndex = 90;
            this.ChkCharge.Text = "청구에서 원청으로 보이도록";
            this.ChkCharge.UseVisualStyleBackColor = true;
            // 
            // NumTOTALPRICE
            // 
            this.NumTOTALPRICE.Location = new System.Drawing.Point(466, 48);
            this.NumTOTALPRICE.Maximum = new decimal(new int[] {
            999000,
            0,
            0,
            0});
            this.NumTOTALPRICE.Name = "NumTOTALPRICE";
            this.NumTOTALPRICE.Size = new System.Drawing.Size(71, 25);
            this.NumTOTALPRICE.TabIndex = 89;
            this.NumTOTALPRICE.Tag = "";
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label12.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.Location = new System.Drawing.Point(466, 19);
            this.label12.Name = "label12";
            this.label12.Padding = new System.Windows.Forms.Padding(3);
            this.label12.Size = new System.Drawing.Size(71, 25);
            this.label12.TabIndex = 88;
            this.label12.Text = "계약금액";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NumUNITTOTALPRICE
            // 
            this.NumUNITTOTALPRICE.Location = new System.Drawing.Point(206, 48);
            this.NumUNITTOTALPRICE.Maximum = new decimal(new int[] {
            999000,
            0,
            0,
            0});
            this.NumUNITTOTALPRICE.Name = "NumUNITTOTALPRICE";
            this.NumUNITTOTALPRICE.Size = new System.Drawing.Size(71, 25);
            this.NumUNITTOTALPRICE.TabIndex = 87;
            this.NumUNITTOTALPRICE.Tag = "";
            this.NumUNITTOTALPRICE.ValueChanged += new System.EventHandler(this.NumUNITTOTALPRICE_ValueChanged);
            // 
            // BtnNewPrice
            // 
            this.BtnNewPrice.Location = new System.Drawing.Point(980, 40);
            this.BtnNewPrice.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnNewPrice.Name = "BtnNewPrice";
            this.BtnNewPrice.Size = new System.Drawing.Size(75, 28);
            this.BtnNewPrice.TabIndex = 86;
            this.BtnNewPrice.Text = "화면정리";
            this.BtnNewPrice.UseVisualStyleBackColor = true;
            this.BtnNewPrice.Click += new System.EventHandler(this.BtnNewPrice_Click);
            // 
            // ChkIsBill
            // 
            this.ChkIsBill.AutoSize = true;
            this.ChkIsBill.Location = new System.Drawing.Point(301, 47);
            this.ChkIsBill.Name = "ChkIsBill";
            this.ChkIsBill.Size = new System.Drawing.Size(144, 21);
            this.ChkIsBill.TabIndex = 85;
            this.ChkIsBill.Text = "계산서인원단가표시";
            this.ChkIsBill.UseVisualStyleBackColor = true;
            // 
            // ChkIsFix
            // 
            this.ChkIsFix.AutoSize = true;
            this.ChkIsFix.Location = new System.Drawing.Point(553, 48);
            this.ChkIsFix.Name = "ChkIsFix";
            this.ChkIsFix.Size = new System.Drawing.Size(79, 21);
            this.ChkIsFix.TabIndex = 84;
            this.ChkIsFix.Text = "정액계약";
            this.ChkIsFix.UseVisualStyleBackColor = true;
            // 
            // BtnDeletePrice
            // 
            this.BtnDeletePrice.Location = new System.Drawing.Point(1061, 40);
            this.BtnDeletePrice.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnDeletePrice.Name = "BtnDeletePrice";
            this.BtnDeletePrice.Size = new System.Drawing.Size(75, 28);
            this.BtnDeletePrice.TabIndex = 83;
            this.BtnDeletePrice.Text = "삭제";
            this.BtnDeletePrice.UseVisualStyleBackColor = true;
            this.BtnDeletePrice.Click += new System.EventHandler(this.BtnDeletePrice_Click);
            // 
            // BtnSavePrice
            // 
            this.BtnSavePrice.Location = new System.Drawing.Point(1142, 40);
            this.BtnSavePrice.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSavePrice.Name = "BtnSavePrice";
            this.BtnSavePrice.Size = new System.Drawing.Size(75, 28);
            this.BtnSavePrice.TabIndex = 82;
            this.BtnSavePrice.Text = "저장(&S)";
            this.BtnSavePrice.UseVisualStyleBackColor = true;
            this.BtnSavePrice.Click += new System.EventHandler(this.BtnSavePrice_Click);
            // 
            // label40
            // 
            this.label40.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label40.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label40.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label40.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label40.Location = new System.Drawing.Point(206, 19);
            this.label40.Name = "label40";
            this.label40.Padding = new System.Windows.Forms.Padding(3);
            this.label40.Size = new System.Drawing.Size(71, 25);
            this.label40.TabIndex = 75;
            this.label40.Text = "계산금액";
            this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label10.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(115, 19);
            this.label10.Name = "label10";
            this.label10.Padding = new System.Windows.Forms.Padding(3);
            this.label10.Size = new System.Drawing.Size(71, 25);
            this.label10.TabIndex = 73;
            this.label10.Text = "단가";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(20, 19);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(3);
            this.label5.Size = new System.Drawing.Size(71, 25);
            this.label5.TabIndex = 72;
            this.label5.Text = "인원";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NumUNITPRICE
            // 
            this.NumUNITPRICE.Location = new System.Drawing.Point(115, 47);
            this.NumUNITPRICE.Maximum = new decimal(new int[] {
            999000,
            0,
            0,
            0});
            this.NumUNITPRICE.Name = "NumUNITPRICE";
            this.NumUNITPRICE.Size = new System.Drawing.Size(71, 25);
            this.NumUNITPRICE.TabIndex = 69;
            this.NumUNITPRICE.ValueChanged += new System.EventHandler(this.NumUNITPRICE_ValueChanged);
            this.NumUNITPRICE.KeyUp += new System.Windows.Forms.KeyEventHandler(this.NumUNITPRICE_KeyUp);
            // 
            // NumPriceWORKERTOTALCOUNT
            // 
            this.NumPriceWORKERTOTALCOUNT.Location = new System.Drawing.Point(20, 47);
            this.NumPriceWORKERTOTALCOUNT.Maximum = new decimal(new int[] {
            999000,
            0,
            0,
            0});
            this.NumPriceWORKERTOTALCOUNT.Name = "NumPriceWORKERTOTALCOUNT";
            this.NumPriceWORKERTOTALCOUNT.Size = new System.Drawing.Size(71, 25);
            this.NumPriceWORKERTOTALCOUNT.TabIndex = 68;
            this.NumPriceWORKERTOTALCOUNT.ValueChanged += new System.EventHandler(this.NumPriceWORKERTOTALCOUNT_ValueChanged);
            this.NumPriceWORKERTOTALCOUNT.KeyUp += new System.Windows.Forms.KeyEventHandler(this.NumPriceWORKERTOTALCOUNT_KeyUp);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.BtnPdf);
            this.tabPage1.Controls.Add(this.label57);
            this.tabPage1.Controls.Add(this.txtReceiveEmail);
            this.tabPage1.Controls.Add(this.label56);
            this.tabPage1.Controls.Add(this.txtHalthManager);
            this.tabPage1.Controls.Add(this.PanEstimate);
            this.tabPage1.Controls.Add(this.BtnLoadExcel);
            this.tabPage1.Controls.Add(this.SSEstimate);
            this.tabPage1.Controls.Add(this.BtnSendMailEstimate);
            this.tabPage1.Controls.Add(this.BtnPrintEstimate);
            this.tabPage1.Location = new System.Drawing.Point(4, 32);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1223, 929);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "견   적 ";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // BtnPdf
            // 
            this.BtnPdf.Location = new System.Drawing.Point(743, 231);
            this.BtnPdf.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnPdf.Name = "BtnPdf";
            this.BtnPdf.Size = new System.Drawing.Size(75, 28);
            this.BtnPdf.TabIndex = 84;
            this.BtnPdf.Text = "PDF 저장";
            this.BtnPdf.UseVisualStyleBackColor = true;
            this.BtnPdf.Click += new System.EventHandler(this.BtnPdf_Click);
            // 
            // label57
            // 
            this.label57.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label57.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label57.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label57.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label57.Location = new System.Drawing.Point(207, 233);
            this.label57.Name = "label57";
            this.label57.Padding = new System.Windows.Forms.Padding(3);
            this.label57.Size = new System.Drawing.Size(96, 25);
            this.label57.TabIndex = 83;
            this.label57.Text = "받는사람";
            this.label57.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtReceiveEmail
            // 
            this.txtReceiveEmail.Location = new System.Drawing.Point(309, 233);
            this.txtReceiveEmail.Name = "txtReceiveEmail";
            this.txtReceiveEmail.Size = new System.Drawing.Size(205, 25);
            this.txtReceiveEmail.TabIndex = 82;
            // 
            // label56
            // 
            this.label56.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label56.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label56.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label56.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label56.Location = new System.Drawing.Point(3, 233);
            this.label56.Name = "label56";
            this.label56.Padding = new System.Windows.Forms.Padding(3);
            this.label56.Size = new System.Drawing.Size(96, 25);
            this.label56.TabIndex = 80;
            this.label56.Text = "보건담당자";
            this.label56.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtHalthManager
            // 
            this.txtHalthManager.Location = new System.Drawing.Point(103, 233);
            this.txtHalthManager.Name = "txtHalthManager";
            this.txtHalthManager.Size = new System.Drawing.Size(98, 25);
            this.txtHalthManager.TabIndex = 57;
            // 
            // tabPage5
            // 
            this.tabPage5.Location = new System.Drawing.Point(4, 32);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(1223, 929);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "원하청 현황";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 32);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1223, 929);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "원청 현황";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // formTItle1
            // 
            this.formTItle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.formTItle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.formTItle1.Location = new System.Drawing.Point(0, 0);
            this.formTItle1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.formTItle1.Name = "formTItle1";
            this.formTItle1.Size = new System.Drawing.Size(1447, 33);
            this.formTItle1.TabIndex = 5;
            this.formTItle1.TitleText = "사업장 견적 및 계약";
            this.formTItle1.Load += new System.EventHandler(this.formTItle1_Load);
            // 
            // oshaSiteLastTree
            // 
            this.oshaSiteLastTree.Dock = System.Windows.Forms.DockStyle.Top;
            this.oshaSiteLastTree.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
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
            this.oshaSiteLastTree.GetSite = hC_OSHA_SITE_MODEL1;
            this.oshaSiteLastTree.IsCheckbox = false;
            this.oshaSiteLastTree.Location = new System.Drawing.Point(0, 0);
            this.oshaSiteLastTree.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.oshaSiteLastTree.Name = "oshaSiteLastTree";
            this.oshaSiteLastTree.Size = new System.Drawing.Size(216, 586);
            this.oshaSiteLastTree.TabIndex = 30;
            this.oshaSiteLastTree.NodeClick += new HC_OSHA.OshaSiteLastTree.SiteTreeViewNodeMouseClickEventHandler(this.OshaSiteLastTree_NodeClick);
            this.oshaSiteLastTree.Load += new System.EventHandler(this.oshaSiteLastTree_Load_1);
            // 
            // lblSitename
            // 
            this.lblSitename.AutoSize = true;
            this.lblSitename.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSitename.Location = new System.Drawing.Point(211, 7);
            this.lblSitename.Name = "lblSitename";
            this.lblSitename.Size = new System.Drawing.Size(60, 17);
            this.lblSitename.TabIndex = 35;
            this.lblSitename.Text = "사업장명";
            // 
            // panTab
            // 
            this.panTab.Controls.Add(this.tabControl1);
            this.panTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panTab.Location = new System.Drawing.Point(216, 33);
            this.panTab.Name = "panTab";
            this.panTab.Size = new System.Drawing.Size(1231, 965);
            this.panTab.TabIndex = 36;
            // 
            // panLeft
            // 
            this.panLeft.Controls.Add(this.OshaSiteEstimateList);
            this.panLeft.Controls.Add(this.oshaSiteLastTree);
            this.panLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panLeft.Location = new System.Drawing.Point(0, 33);
            this.panLeft.Name = "panLeft";
            this.panLeft.Size = new System.Drawing.Size(216, 965);
            this.panLeft.TabIndex = 37;
            // 
            // SiteManageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1447, 998);
            this.Controls.Add(this.panTab);
            this.Controls.Add(this.panLeft);
            this.Controls.Add(this.lblSitename);
            this.Controls.Add(this.BtnSiteConfitm);
            this.Controls.Add(this.TxtSiteName);
            this.Controls.Add(this.TxtOshaSiteId);
            this.Controls.Add(this.formTItle1);
            this.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.Name = "SiteManageForm";
            this.Text = " ";
            this.Load += new System.EventHandler(this.SiteMangerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.PanEstimate.ResumeLayout(false);
            this.PanEstimate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumOFFICIALFEE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumMONTHLYFEE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumWORKERTOTALCOUNT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumSITEFEE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSEstimate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSEstimate_Sheet1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.PanContract.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSWorkerList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSWorkerList_Sheet1)).EndInit();
            this.groupBox11.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NumMANAGEWORKERCOUNT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumMANAGEENGINEERCOUNT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumMANAGENURSECOUNT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumMANAGEDOCTORCOUNT)).EndInit();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumCOMMISSION)).EndInit();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NumContractWORKERTOTALCOUNT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumWORKERWHITEFEMALECOUNT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumWORKERBLUEFEMALECOUNT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumWORKERWHITEMALECOUNT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumWORKERBLUEMALECOUNT)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.panBottmo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSChildPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSChildPrice_Sheet1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSPrice_Sheet1)).EndInit();
            this.PanPrice.ResumeLayout(false);
            this.PanPrice.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumTOTALPRICE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUNITTOTALPRICE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUNITPRICE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumPriceWORKERTOTALCOUNT)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panTab.ResumeLayout(false);
            this.panLeft.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox TxtOshaSiteId;
        private System.Windows.Forms.TextBox TxtSiteName;
        private ComBase.Mvc.UserControls.FormTItle formTItle1;
        private System.Windows.Forms.Panel PanEstimate;
        private System.Windows.Forms.DateTimePicker DtpESTIMATEDATE;
        private System.Windows.Forms.Button BtnSaveEstimate;
        private System.Windows.Forms.Button btnNewEstimate;
        private System.Windows.Forms.Button BtnPrintEstimate;
        private System.Windows.Forms.Button BtnSendMailEstimate;
        private System.Windows.Forms.Button BtnDeleteEstimate;
        private System.Windows.Forms.DateTimePicker DtpSTARTDATE;
        private System.Windows.Forms.TextBox TxtRemark;
        private System.Windows.Forms.Button BtnSiteConfitm;
        private System.Windows.Forms.Panel PanContract;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker DtpCONTRACTDATE;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown NumWORKERWHITEMALECOUNT;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.RadioButton RdoISLABOR_1;
        private System.Windows.Forms.RadioButton RdoISLABOR_0;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.RadioButton RdoISPRODUCTTYPE_1;
        private System.Windows.Forms.RadioButton RdoISPRODUCTTYPE_0;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.RadioButton RdoBUILDINGTYPE_1;
        private System.Windows.Forms.RadioButton RdoBUILDINGTYPE_0;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton RdoISROTATION_1;
        private System.Windows.Forms.RadioButton RdoISROTATION_0;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton RdoPosition_3;
        private System.Windows.Forms.RadioButton RdoPosition_2;
        private System.Windows.Forms.RadioButton RdoPosition_1;
        private System.Windows.Forms.RadioButton RdoPosition_0;
        private System.Windows.Forms.NumericUpDown NumContractWORKERTOTALCOUNT;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown NumWORKERWHITEFEMALECOUNT;
        private System.Windows.Forms.NumericUpDown NumWORKERBLUEFEMALECOUNT;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.NumericUpDown NumWORKERBLUEMALECOUNT;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.ComboBox CboMANAGEENGINEER;
        private System.Windows.Forms.ComboBox CboManageNurse;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.ComboBox CboManageDoctor;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox TxtWORKETCTIME;
        private System.Windows.Forms.TextBox TxtWORKEDUTIME;
        private System.Windows.Forms.TextBox TxtWORKRESTTIME;
        private System.Windows.Forms.TextBox TxtWORKLUANCHTIME;
        private System.Windows.Forms.TextBox TxtWORKROTATIONTIME;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown NumMANAGEDOCTORCOUNT;
        private System.Windows.Forms.DateTimePicker DtpMANAGEDOCTORSTARTDATE;
        private System.Windows.Forms.DateTimePicker DtpMANAGEENGINEERSTARTDATE;
        private System.Windows.Forms.DateTimePicker DtpMANAGENURSESTARTDATE;
        private System.Windows.Forms.NumericUpDown NumCOMMISSION;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.TextBox TxtVISITWEEK;
        private System.Windows.Forms.TextBox TxtVISITDAY;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.DateTimePicker DtpCONTRACTENDDATE;
        private System.Windows.Forms.DateTimePicker DtpCONTRACTSTARTDATE;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.NumericUpDown NumWORKERTOTALCOUNT;
        private System.Windows.Forms.NumericUpDown NumSITEFEE;
        private System.Windows.Forms.NumericUpDown NumOFFICIALFEE;
        private System.Windows.Forms.NumericUpDown NumMONTHLYFEE;
        private System.Windows.Forms.TextBox TxtSPECIALCONTRACT;
        private FarPoint.Win.Spread.FpSpread SSWorkerList;
        private FarPoint.Win.Spread.SheetView SSWorkerList_Sheet1;
        private System.Windows.Forms.TextBox TxtPRINTDATE;
        private System.Windows.Forms.TextBox TxtSENDMAILDATE;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.NumericUpDown NumMANAGEENGINEERCOUNT;
        private System.Windows.Forms.NumericUpDown NumMANAGENURSECOUNT;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox TxtISWEMDATA;
        private System.Windows.Forms.TextBox TxtISSKELETONDATE;
        private System.Windows.Forms.TextBox TxtISSPACEPROGRAMDATE;
        private System.Windows.Forms.Button BtnDeleteContract;
        private System.Windows.Forms.Button BtnLastContract;
        private System.Windows.Forms.Button BtnSaveContract;
        private System.Windows.Forms.TextBox TxtISSPECIALDATA;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.CheckBox ChkISSPECIAL;
        private System.Windows.Forms.CheckBox ChkISBRAINTEST;
        private System.Windows.Forms.CheckBox ChkISSTRESS;
        private System.Windows.Forms.CheckBox ChkISSPACEPROGRAM;
        private System.Windows.Forms.CheckBox ChkISEARPROGRAM;
        private System.Windows.Forms.CheckBox ChkISSKELETON;
        private System.Windows.Forms.CheckBox ChkISWEM;
        private System.Windows.Forms.CheckBox ChkISCOMMITTEE;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox TxtISBRAINTESTDATE;
        private System.Windows.Forms.TextBox TxtISEARPROGRAMDATE;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.TextBox TxtISSTRESSDATE;
        private System.Windows.Forms.Button BtnWorkerAdd;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.NumericUpDown NumMANAGEWORKERCOUNT;
        private System.Windows.Forms.Button BtnLoadExcel;
        private FarPoint.Win.Spread.FpSpread SSEstimate;
        private FarPoint.Win.Spread.SheetView SSEstimate_Sheet1;
        private OshaSiteEstimateList OshaSiteEstimateList;
        private System.Windows.Forms.TextBox TxtParentSiteName;
        private System.Windows.Forms.Button BtnSearchParent;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TxtParentSiteId;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel PanPrice;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown NumUNITPRICE;
        private System.Windows.Forms.NumericUpDown NumPriceWORKERTOTALCOUNT;
        private FarPoint.Win.Spread.FpSpread SSPrice;
        private FarPoint.Win.Spread.SheetView SSPrice_Sheet1;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Button BtnDeletePrice;
        private System.Windows.Forms.Button BtnSavePrice;
        private System.Windows.Forms.CheckBox ChkIsBill;
        private System.Windows.Forms.CheckBox ChkIsFix;
        private System.Windows.Forms.TextBox txtReceiveEmail;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.TextBox txtHalthManager;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.Button BtnNewPrice;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown NumUNITTOTALPRICE;
        private OshaSiteLastTree oshaSiteLastTree;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.NumericUpDown NumTOTALPRICE;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.TextBox TxtWORKMEETTIME;
        private System.Windows.Forms.TextBox TxtWORKENDTIME;
        private System.Windows.Forms.TextBox TxtWORKSTARTTIME;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.TextBox TxtBlueFeMale;
        private System.Windows.Forms.TextBox TxtBlueMale;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.TextBox TxtWhiteFeMale;
        private System.Windows.Forms.TextBox TxtWhiteMale;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Button BtnPdf;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.TabPage tabPage4;
        private FarPoint.Win.Spread.FpSpread SSChildPrice;
        private FarPoint.Win.Spread.SheetView SSChildPrice_Sheet1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button BtnSaveChildPrice;
        private System.Windows.Forms.TextBox TxtTotalPrice;
        private System.Windows.Forms.Label label64;
        private System.Windows.Forms.TextBox TxtTotalUnitPrice;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.TextBox TxtTotalWorkerCount;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.DateTimePicker DtpDECLAREDAY;
        private System.Windows.Forms.DateTimePicker DtpTERMINATEDATE;
        private System.Windows.Forms.Label lblSitename;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Panel panTab;
        private System.Windows.Forms.Panel panLeft;
        private System.Windows.Forms.Button BtnDeleteParent;
        private System.Windows.Forms.Panel panBottmo;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.Panel panTop;
        private System.Windows.Forms.CheckBox ChkCharge;
        private System.Windows.Forms.CheckBox ChkQuarterCharge;
    }
}