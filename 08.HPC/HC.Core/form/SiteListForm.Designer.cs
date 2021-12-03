namespace HC_Core
{
    partial class SiteListForm
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
            this.panBody = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panList = new System.Windows.Forms.Panel();
            this.SSSiteList = new FarPoint.Win.Spread.FpSpread();
            this.SSSiteList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panSpace = new System.Windows.Forms.Panel();
            this.panSearch = new System.Windows.Forms.Panel();
            this.CboManager = new System.Windows.Forms.ComboBox();
            this.BtnOk = new System.Windows.Forms.Button();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.TxtIdOrName = new System.Windows.Forms.TextBox();
            this.panTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panBody.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSSiteList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSSiteList_Sheet1)).BeginInit();
            this.panSearch.SuspendLayout();
            this.panTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // panBody
            // 
            this.panBody.Controls.Add(this.panel2);
            this.panBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBody.Location = new System.Drawing.Point(0, 42);
            this.panBody.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panBody.Name = "panBody";
            this.panBody.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panBody.Size = new System.Drawing.Size(536, 595);
            this.panBody.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.panList);
            this.panel2.Controls.Add(this.panSearch);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 4);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(530, 587);
            this.panel2.TabIndex = 5;
            // 
            // panList
            // 
            this.panList.Controls.Add(this.SSSiteList);
            this.panList.Controls.Add(this.panSpace);
            this.panList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panList.Location = new System.Drawing.Point(0, 38);
            this.panList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panList.Name = "panList";
            this.panList.Size = new System.Drawing.Size(530, 549);
            this.panList.TabIndex = 3;
            // 
            // SSSiteList
            // 
            this.SSSiteList.AccessibleDescription = "";
            this.SSSiteList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSSiteList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSSiteList.Location = new System.Drawing.Point(0, 7);
            this.SSSiteList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SSSiteList.Name = "SSSiteList";
            this.SSSiteList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSSiteList_Sheet1});
            this.SSSiteList.Size = new System.Drawing.Size(530, 542);
            this.SSSiteList.TabIndex = 0;
            this.SSSiteList.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSSiteList_CellClick);
            this.SSSiteList.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSSiteList_CellDoubleClick);
            // 
            // SSSiteList_Sheet1
            // 
            this.SSSiteList_Sheet1.Reset();
            this.SSSiteList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSSiteList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSSiteList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSSiteList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSSiteList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSSiteList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSSiteList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSSiteList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSSiteList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSSiteList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSSiteList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panSpace
            // 
            this.panSpace.BackColor = System.Drawing.SystemColors.Control;
            this.panSpace.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSpace.Location = new System.Drawing.Point(0, 0);
            this.panSpace.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panSpace.Name = "panSpace";
            this.panSpace.Size = new System.Drawing.Size(530, 7);
            this.panSpace.TabIndex = 27;
            // 
            // panSearch
            // 
            this.panSearch.BackColor = System.Drawing.Color.White;
            this.panSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSearch.Controls.Add(this.CboManager);
            this.panSearch.Controls.Add(this.BtnOk);
            this.panSearch.Controls.Add(this.BtnSearch);
            this.panSearch.Controls.Add(this.TxtIdOrName);
            this.panSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSearch.Location = new System.Drawing.Point(0, 0);
            this.panSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panSearch.Name = "panSearch";
            this.panSearch.Size = new System.Drawing.Size(530, 38);
            this.panSearch.TabIndex = 2;
            // 
            // CboManager
            // 
            this.CboManager.FormattingEnabled = true;
            this.CboManager.Location = new System.Drawing.Point(9, 6);
            this.CboManager.Name = "CboManager";
            this.CboManager.Size = new System.Drawing.Size(121, 25);
            this.CboManager.TabIndex = 24;
            this.CboManager.SelectedIndexChanged += new System.EventHandler(this.CboManager_SelectedIndexChanged);
            // 
            // BtnOk
            // 
            this.BtnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOk.Location = new System.Drawing.Point(457, 4);
            this.BtnOk.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(65, 28);
            this.BtnOk.TabIndex = 23;
            this.BtnOk.Text = "확인";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(288, 5);
            this.BtnSearch.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(65, 28);
            this.BtnSearch.TabIndex = 4;
            this.BtnSearch.Text = "검색(&S)";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // TxtIdOrName
            // 
            this.TxtIdOrName.Location = new System.Drawing.Point(138, 6);
            this.TxtIdOrName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TxtIdOrName.Name = "TxtIdOrName";
            this.TxtIdOrName.Size = new System.Drawing.Size(145, 25);
            this.TxtIdOrName.TabIndex = 0;
            // 
            // panTop
            // 
            this.panTop.Controls.Add(this.lblTitle);
            this.panTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTop.Location = new System.Drawing.Point(0, 0);
            this.panTop.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.panTop.Name = "panTop";
            this.panTop.Size = new System.Drawing.Size(536, 42);
            this.panTop.TabIndex = 5;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(4, 11);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(96, 21);
            this.lblTitle.TabIndex = 6;
            this.lblTitle.Text = "사업장 목록";
            // 
            // SiteListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 637);
            this.Controls.Add(this.panBody);
            this.Controls.Add(this.panTop);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SiteListForm";
            this.Text = "SiteFormList";
            this.Load += new System.EventHandler(this.SiteListForm_Load);
            this.panBody.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSSiteList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSSiteList_Sheet1)).EndInit();
            this.panSearch.ResumeLayout(false);
            this.panSearch.PerformLayout();
            this.panTop.ResumeLayout(false);
            this.panTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panBody;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panList;
        private FarPoint.Win.Spread.FpSpread SSSiteList;
        private FarPoint.Win.Spread.SheetView SSSiteList_Sheet1;
        private System.Windows.Forms.Panel panSpace;
        private System.Windows.Forms.Panel panSearch;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.TextBox TxtIdOrName;
        private System.Windows.Forms.Panel panTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.ComboBox CboManager;
    }
}