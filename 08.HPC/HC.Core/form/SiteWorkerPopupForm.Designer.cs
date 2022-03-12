namespace HC_OSHA
{
    partial class SiteWorkerPopupForm
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
            this.SSWorkerList = new FarPoint.Win.Spread.FpSpread();
            this.SSWorkerList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.awd = new ComBase.Mvc.UserControls.ContentTitle();
            this.BtnConfirm = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.TxtNAME = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSWorkerList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSWorkerList_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // SSWorkerList
            // 
            this.SSWorkerList.AccessibleDescription = "";
            this.SSWorkerList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSWorkerList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSWorkerList.Location = new System.Drawing.Point(0, 34);
            this.SSWorkerList.Margin = new System.Windows.Forms.Padding(3, 14, 3, 4);
            this.SSWorkerList.Name = "SSWorkerList";
            this.SSWorkerList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSWorkerList_Sheet1});
            this.SSWorkerList.Size = new System.Drawing.Size(800, 416);
            this.SSWorkerList.TabIndex = 3;
            // 
            // SSWorkerList_Sheet1
            // 
            this.SSWorkerList_Sheet1.Reset();
            this.SSWorkerList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSWorkerList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSWorkerList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSWorkerList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSWorkerList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSWorkerList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSWorkerList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSWorkerList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSWorkerList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSWorkerList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSWorkerList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // awd
            // 
            this.awd.Dock = System.Windows.Forms.DockStyle.Top;
            this.awd.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.awd.Location = new System.Drawing.Point(0, 0);
            this.awd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.awd.Name = "awd";
            this.awd.Size = new System.Drawing.Size(800, 34);
            this.awd.TabIndex = 9;
            this.awd.TitleText = "근로자 목록";
            this.awd.Load += new System.EventHandler(this.awd_Load);
            // 
            // BtnConfirm
            // 
            this.BtnConfirm.Location = new System.Drawing.Point(683, 2);
            this.BtnConfirm.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnConfirm.Name = "BtnConfirm";
            this.BtnConfirm.Size = new System.Drawing.Size(105, 28);
            this.BtnConfirm.TabIndex = 2;
            this.BtnConfirm.Text = "근로자 선택";
            this.BtnConfirm.UseVisualStyleBackColor = true;
            this.BtnConfirm.Click += new System.EventHandler(this.BtnConfirm_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(148, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 17);
            this.label1.TabIndex = 15;
            this.label1.Text = "성명";
            // 
            // BtnSearch
            // 
            this.BtnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSearch.Location = new System.Drawing.Point(284, 4);
            this.BtnSearch.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(75, 28);
            this.BtnSearch.TabIndex = 1;
            this.BtnSearch.Text = "검색";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // TxtNAME
            // 
            this.TxtNAME.Location = new System.Drawing.Point(184, 5);
            this.TxtNAME.Name = "TxtNAME";
            this.TxtNAME.Size = new System.Drawing.Size(95, 25);
            this.TxtNAME.TabIndex = 0;
            // 
            // SiteWorkerPopupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TxtNAME);
            this.Controls.Add(this.BtnSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnConfirm);
            this.Controls.Add(this.SSWorkerList);
            this.Controls.Add(this.awd);
            this.Name = "SiteWorkerPopupForm";
            this.Text = "SiteWorkerPopupForm";
            this.Load += new System.EventHandler(this.SiteWorkerPopupForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSWorkerList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSWorkerList_Sheet1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FarPoint.Win.Spread.FpSpread SSWorkerList;
        private FarPoint.Win.Spread.SheetView SSWorkerList_Sheet1;
        private ComBase.Mvc.UserControls.ContentTitle awd;
        private System.Windows.Forms.Button BtnConfirm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.TextBox TxtNAME;
    }
}