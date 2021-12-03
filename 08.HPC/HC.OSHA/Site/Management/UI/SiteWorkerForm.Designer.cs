namespace HC.OSHA.Site.Management.UI
{
    partial class SiteWorkerForm
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
            this.panRight = new System.Windows.Forms.Panel();
            this.SSWorkerList = new FarPoint.Win.Spread.FpSpread();
            this.SSWorkerList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panSearch = new System.Windows.Forms.Panel();
            this.BtnSave = new System.Windows.Forms.Button();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.contentTitle1 = new ComBase.Mvc.UserControls.ContentTitle();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSWorkerList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSWorkerList_Sheet1)).BeginInit();
            this.panSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // panRight
            // 
            this.panRight.Controls.Add(this.SSWorkerList);
            this.panRight.Controls.Add(this.panSearch);
            this.panRight.Controls.Add(this.contentTitle1);
            this.panRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panRight.Location = new System.Drawing.Point(0, 0);
            this.panRight.Name = "panRight";
            this.panRight.Size = new System.Drawing.Size(1264, 985);
            this.panRight.TabIndex = 1;
            // 
            // SSWorkerList
            // 
            this.SSWorkerList.AccessibleDescription = "";
            this.SSWorkerList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSWorkerList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSWorkerList.Location = new System.Drawing.Point(0, 107);
            this.SSWorkerList.Name = "SSWorkerList";
            this.SSWorkerList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSWorkerList_Sheet1});
            this.SSWorkerList.Size = new System.Drawing.Size(1264, 878);
            this.SSWorkerList.TabIndex = 2;
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
            // panSearch
            // 
            this.panSearch.BackColor = System.Drawing.Color.White;
            this.panSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSearch.Controls.Add(this.BtnSave);
            this.panSearch.Controls.Add(this.BtnAdd);
            this.panSearch.Controls.Add(this.BtnSearch);
            this.panSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSearch.Location = new System.Drawing.Point(0, 38);
            this.panSearch.Name = "panSearch";
            this.panSearch.Size = new System.Drawing.Size(1264, 69);
            this.panSearch.TabIndex = 1;
            // 
            // BtnSave
            // 
            this.BtnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSave.Location = new System.Drawing.Point(1117, 22);
            this.BtnSave.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 28);
            this.BtnSave.TabIndex = 15;
            this.BtnSave.Text = "저 장(&S)";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnAdd
            // 
            this.BtnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnAdd.Location = new System.Drawing.Point(1036, 22);
            this.BtnAdd.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(75, 28);
            this.BtnAdd.TabIndex = 14;
            this.BtnAdd.Text = "추 가";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // BtnSearch
            // 
            this.BtnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSearch.Location = new System.Drawing.Point(955, 22);
            this.BtnSearch.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(75, 28);
            this.BtnSearch.TabIndex = 12;
            this.BtnSearch.Text = "검 색(&F)";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // contentTitle1
            // 
            this.contentTitle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.contentTitle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.contentTitle1.Location = new System.Drawing.Point(0, 0);
            this.contentTitle1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.contentTitle1.Name = "contentTitle1";
            this.contentTitle1.Size = new System.Drawing.Size(1264, 38);
            this.contentTitle1.TabIndex = 0;
            this.contentTitle1.TitleText = "근로자 목록";
            // 
            // SiteWorkerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 985);
            this.Controls.Add(this.panRight);
            this.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.Name = "SiteWorkerForm";
            this.Text = "근로자 관리";
            this.Load += new System.EventHandler(this.SiteWorkerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSWorkerList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSWorkerList_Sheet1)).EndInit();
            this.panSearch.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panRight;
        private System.Windows.Forms.Panel panSearch;
        private ComBase.Mvc.UserControls.ContentTitle contentTitle1;
        private System.Windows.Forms.Button BtnSearch;
        private FarPoint.Win.Spread.FpSpread SSWorkerList;
        private FarPoint.Win.Spread.SheetView SSWorkerList_Sheet1;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Button BtnAdd;
    }
}