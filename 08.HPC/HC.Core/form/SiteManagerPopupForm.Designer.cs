namespace HC_OSHA
{
    partial class SiteManagerPopupForm
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
            this.SSWorkerList.Location = new System.Drawing.Point(0, 32);
            this.SSWorkerList.Margin = new System.Windows.Forms.Padding(3, 20, 3, 6);
            this.SSWorkerList.Name = "SSWorkerList";
            this.SSWorkerList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSWorkerList_Sheet1});
            this.SSWorkerList.Size = new System.Drawing.Size(434, 190);
            this.SSWorkerList.TabIndex = 17;
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
            this.awd.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.awd.Name = "awd";
            this.awd.Size = new System.Drawing.Size(434, 32);
            this.awd.TabIndex = 18;
            this.awd.TitleText = "메일 발송 대상자";
            // 
            // BtnConfirm
            // 
            this.BtnConfirm.Location = new System.Drawing.Point(327, 0);
            this.BtnConfirm.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.BtnConfirm.Name = "BtnConfirm";
            this.BtnConfirm.Size = new System.Drawing.Size(68, 29);
            this.BtnConfirm.TabIndex = 19;
            this.BtnConfirm.Text = "선택";
            this.BtnConfirm.UseVisualStyleBackColor = true;
            this.BtnConfirm.Click += new System.EventHandler(this.BtnConfirm_Click_1);
            // 
            // SiteManagerPopupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 222);
            this.Controls.Add(this.BtnConfirm);
            this.Controls.Add(this.SSWorkerList);
            this.Controls.Add(this.awd);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SiteManagerPopupForm";
            this.Text = "SiteManagerPopupForm";
            this.Load += new System.EventHandler(this.SiteManagerPopupForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSWorkerList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSWorkerList_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private FarPoint.Win.Spread.FpSpread SSWorkerList;
        private FarPoint.Win.Spread.SheetView SSWorkerList_Sheet1;
        private ComBase.Mvc.UserControls.ContentTitle awd;
        private System.Windows.Forms.Button BtnConfirm;
    }
}