namespace HC_OSHA
{
    partial class StartExport
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtnExportSite = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.BtnExport = new System.Windows.Forms.Button();
            this.SSTableList = new FarPoint.Win.Spread.FpSpread();
            this.SSTableList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.SSSiteList = new FarPoint.Win.Spread.FpSpread();
            this.SSSiteList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.SSRelationList = new FarPoint.Win.Spread.FpSpread();
            this.SSRelationList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.oshaSiteList1 = new HC_OSHA.OshaSiteList();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSTableList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSTableList_Sheet1)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSSiteList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSSiteList_Sheet1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSRelationList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSRelationList_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.BtnExportSite);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.BtnExport);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1143, 50);
            this.panel1.TabIndex = 1;
            // 
            // BtnExportSite
            // 
            this.BtnExportSite.Location = new System.Drawing.Point(419, 11);
            this.BtnExportSite.Name = "BtnExportSite";
            this.BtnExportSite.Size = new System.Drawing.Size(126, 28);
            this.BtnExportSite.TabIndex = 6;
            this.BtnExportSite.Text = "사업장 가져오기";
            this.BtnExportSite.UseVisualStyleBackColor = true;
            this.BtnExportSite.Click += new System.EventHandler(this.BtnExportSite_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(389, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "가져올 사업장이 보이지 않을경우 사업장 가져오기를 실행하세요";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1032, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 28);
            this.button1.TabIndex = 4;
            this.button1.Text = "닫기";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // BtnExport
            // 
            this.BtnExport.Location = new System.Drawing.Point(728, 11);
            this.BtnExport.Name = "BtnExport";
            this.BtnExport.Size = new System.Drawing.Size(229, 28);
            this.BtnExport.TabIndex = 3;
            this.BtnExport.Text = "보건관리DB  노트북으로 가져오기";
            this.BtnExport.UseVisualStyleBackColor = true;
            this.BtnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // SSTableList
            // 
            this.SSTableList.AccessibleDescription = "SSOshaList, Sheet1, Row 0, Column 0, HIC_USERS";
            this.SSTableList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSTableList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSTableList.Location = new System.Drawing.Point(900, 50);
            this.SSTableList.Name = "SSTableList";
            this.SSTableList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSTableList_Sheet1});
            this.SSTableList.Size = new System.Drawing.Size(243, 678);
            this.SSTableList.TabIndex = 3;
            this.SSTableList.SetActiveViewport(0, -1, -1);
            // 
            // SSTableList_Sheet1
            // 
            this.SSTableList_Sheet1.Reset();
            this.SSTableList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSTableList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSTableList_Sheet1.ColumnCount = 0;
            this.SSTableList_Sheet1.RowCount = 0;
            this.SSTableList_Sheet1.ActiveColumnIndex = -1;
            this.SSTableList_Sheet1.ActiveRowIndex = -1;
            this.SSTableList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSTableList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSTableList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSTableList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSTableList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSTableList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSTableList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSTableList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSTableList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSTableList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.SSTableList);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1143, 728);
            this.panel3.TabIndex = 5;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.BtnAdd);
            this.panel4.Controls.Add(this.SSSiteList);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(291, 50);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(609, 678);
            this.panel4.TabIndex = 7;
            // 
            // BtnAdd
            // 
            this.BtnAdd.Location = new System.Drawing.Point(34, 176);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(98, 63);
            this.BtnAdd.TabIndex = 6;
            this.BtnAdd.Text = "사업장 추가";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // SSSiteList
            // 
            this.SSSiteList.AccessibleDescription = "SSOshaList, Sheet1, Row 0, Column 0, HIC_USERS";
            this.SSSiteList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSSiteList.Location = new System.Drawing.Point(171, 47);
            this.SSSiteList.Name = "SSSiteList";
            this.SSSiteList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSSiteList_Sheet1});
            this.SSSiteList.Size = new System.Drawing.Size(408, 391);
            this.SSSiteList.TabIndex = 5;
            // 
            // SSSiteList_Sheet1
            // 
            this.SSSiteList_Sheet1.Reset();
            this.SSSiteList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSSiteList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSSiteList_Sheet1.ColumnCount = 0;
            this.SSSiteList_Sheet1.RowCount = 0;
            this.SSSiteList_Sheet1.ActiveColumnIndex = -1;
            this.SSSiteList_Sheet1.ActiveRowIndex = -1;
            this.SSSiteList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSSiteList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSSiteList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSSiteList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSSiteList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSSiteList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSSiteList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSSiteList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSSiteList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSSiteList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(168, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(179, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "노트북으로 내려 받을 사업장";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.SSRelationList);
            this.panel2.Controls.Add(this.oshaSiteList1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 50);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(291, 678);
            this.panel2.TabIndex = 4;
            // 
            // SSRelationList
            // 
            this.SSRelationList.AccessibleDescription = "SSOshaList, Sheet1, Row 0, Column 0, HIC_USERS";
            this.SSRelationList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSRelationList.Location = new System.Drawing.Point(12, 398);
            this.SSRelationList.Name = "SSRelationList";
            this.SSRelationList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSRelationList_Sheet1});
            this.SSRelationList.Size = new System.Drawing.Size(260, 227);
            this.SSRelationList.TabIndex = 7;
            // 
            // SSRelationList_Sheet1
            // 
            this.SSRelationList_Sheet1.Reset();
            this.SSRelationList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSRelationList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSRelationList_Sheet1.ColumnCount = 0;
            this.SSRelationList_Sheet1.RowCount = 0;
            this.SSRelationList_Sheet1.ActiveColumnIndex = -1;
            this.SSRelationList_Sheet1.ActiveRowIndex = -1;
            this.SSRelationList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSRelationList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSRelationList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSRelationList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSRelationList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSRelationList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSRelationList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSRelationList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSRelationList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSRelationList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // oshaSiteList1
            // 
            this.oshaSiteList1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
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
            hC_OSHA_SITE_MODEL1.NAME = null;
            hC_OSHA_SITE_MODEL1.PARENTSITE_ID = ((long)(0));
            hC_OSHA_SITE_MODEL1.PARENTSITE_NAME = null;
            hC_OSHA_SITE_MODEL1.RowStatus = ComBase.Mvc.RowStatus.None;
            hC_OSHA_SITE_MODEL1.TEL = null;
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
            this.oshaSiteList1.GetSite = hC_OSHA_SITE_MODEL1;
            this.oshaSiteList1.Location = new System.Drawing.Point(12, 7);
            this.oshaSiteList1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.oshaSiteList1.Name = "oshaSiteList1";
            this.oshaSiteList1.Size = new System.Drawing.Size(260, 375);
            this.oshaSiteList1.TabIndex = 114;
            this.oshaSiteList1.CellClick += new HC_OSHA.OshaSiteList.CellDoubleClickEventHandler(this.oshaSiteList1_CellClick);
            this.oshaSiteList1.Load += new System.EventHandler(this.oshaSiteList1_Load);
            // 
            // StartExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1143, 728);
            this.Controls.Add(this.panel3);
            this.Name = "StartExport";
            this.Text = "노트북으로 DB 가져오기";
            this.Load += new System.EventHandler(this.StartExport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSTableList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSTableList_Sheet1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSSiteList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSSiteList_Sheet1)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSRelationList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSRelationList_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private FarPoint.Win.Spread.FpSpread SSTableList;
        private FarPoint.Win.Spread.SheetView SSTableList_Sheet1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button BtnExport;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private FarPoint.Win.Spread.FpSpread SSSiteList;
        private FarPoint.Win.Spread.SheetView SSSiteList_Sheet1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.Button BtnExportSite;
        private FarPoint.Win.Spread.FpSpread SSRelationList;
        private FarPoint.Win.Spread.SheetView SSRelationList_Sheet1;
        private OshaSiteList oshaSiteList1;
    }
}