namespace HC_OSHA
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
            this.lblSiteName = new System.Windows.Forms.Label();
            this.BtnExit = new System.Windows.Forms.Button();
            this.SSWorkerList = new FarPoint.Win.Spread.FpSpread();
            this.SSWorkerList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panSearch = new System.Windows.Forms.Panel();
            this.CboDept = new System.Windows.Forms.ComboBox();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.TxtNameOrPano = new System.Windows.Forms.TextBox();
            this.BtnSave = new System.Windows.Forms.Button();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.contentTitle1 = new ComBase.Mvc.UserControls.ContentTitle();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSWorkerList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSWorkerList_Sheet1)).BeginInit();
            this.panSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // panRight
            // 
            this.panRight.Controls.Add(this.lblSiteName);
            this.panRight.Controls.Add(this.BtnExit);
            this.panRight.Controls.Add(this.SSWorkerList);
            this.panRight.Controls.Add(this.panSearch);
            this.panRight.Controls.Add(this.contentTitle1);
            this.panRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panRight.Location = new System.Drawing.Point(0, 0);
            this.panRight.Name = "panRight";
            this.panRight.Size = new System.Drawing.Size(1264, 871);
            this.panRight.TabIndex = 1;
            // 
            // lblSiteName
            // 
            this.lblSiteName.AutoSize = true;
            this.lblSiteName.Location = new System.Drawing.Point(115, 8);
            this.lblSiteName.Name = "lblSiteName";
            this.lblSiteName.Size = new System.Drawing.Size(130, 17);
            this.lblSiteName.TabIndex = 17;
            this.lblSiteName.Text = "사업장을 선택하세요";
            // 
            // BtnExit
            // 
            this.BtnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnExit.Location = new System.Drawing.Point(1185, 2);
            this.BtnExit.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(75, 28);
            this.BtnExit.TabIndex = 16;
            this.BtnExit.Text = "닫 기(&S)";
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // SSWorkerList
            // 
            this.SSWorkerList.AccessibleDescription = "";
            this.SSWorkerList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSWorkerList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSWorkerList.Location = new System.Drawing.Point(0, 80);
            this.SSWorkerList.Name = "SSWorkerList";
            this.SSWorkerList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSWorkerList_Sheet1});
            this.SSWorkerList.Size = new System.Drawing.Size(1264, 791);
            this.SSWorkerList.TabIndex = 2;
            // 
            // SSWorkerList_Sheet1
            // 
            this.SSWorkerList_Sheet1.Reset();
            this.SSWorkerList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSWorkerList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSWorkerList_Sheet1.ColumnCount = 1;
            this.SSWorkerList_Sheet1.RowCount = 1;
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
            // panSearch
            // 
            this.panSearch.BackColor = System.Drawing.Color.White;
            this.panSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSearch.Controls.Add(this.label1);
            this.panSearch.Controls.Add(this.label2);
            this.panSearch.Controls.Add(this.CboDept);
            this.panSearch.Controls.Add(this.BtnSearch);
            this.panSearch.Controls.Add(this.TxtNameOrPano);
            this.panSearch.Controls.Add(this.BtnSave);
            this.panSearch.Controls.Add(this.BtnAdd);
            this.panSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSearch.Location = new System.Drawing.Point(0, 38);
            this.panSearch.Name = "panSearch";
            this.panSearch.Size = new System.Drawing.Size(1264, 42);
            this.panSearch.TabIndex = 1;
            this.panSearch.Paint += new System.Windows.Forms.PaintEventHandler(this.panSearch_Paint);
            // 
            // CboDept
            // 
            this.CboDept.FormattingEnabled = true;
            this.CboDept.Location = new System.Drawing.Point(53, 8);
            this.CboDept.Name = "CboDept";
            this.CboDept.Size = new System.Drawing.Size(203, 25);
            this.CboDept.TabIndex = 0;
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(459, 8);
            this.BtnSearch.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(75, 28);
            this.BtnSearch.TabIndex = 2;
            this.BtnSearch.Text = "검색";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click_1);
            // 
            // TxtNameOrPano
            // 
            this.TxtNameOrPano.Location = new System.Drawing.Point(337, 8);
            this.TxtNameOrPano.Name = "TxtNameOrPano";
            this.TxtNameOrPano.Size = new System.Drawing.Size(100, 25);
            this.TxtNameOrPano.TabIndex = 1;
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(683, 7);
            this.BtnSave.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 28);
            this.BtnSave.TabIndex = 4;
            this.BtnSave.Text = "저 장(&S)";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnAdd
            // 
            this.BtnAdd.Location = new System.Drawing.Point(602, 7);
            this.BtnAdd.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(75, 28);
            this.BtnAdd.TabIndex = 3;
            this.BtnAdd.Text = "추 가";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
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
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(289, 10);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(3);
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label2.Size = new System.Drawing.Size(42, 25);
            this.label2.TabIndex = 35;
            this.label2.Text = "성명";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(11, 5);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(3);
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(42, 25);
            this.label1.TabIndex = 36;
            this.label1.Text = "부서";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SiteWorkerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 871);
            this.Controls.Add(this.panRight);
            this.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.Name = "SiteWorkerForm";
            this.Text = "근로자 관리";
            this.Load += new System.EventHandler(this.SiteWorkerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panRight.ResumeLayout(false);
            this.panRight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSWorkerList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSWorkerList_Sheet1)).EndInit();
            this.panSearch.ResumeLayout(false);
            this.panSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panRight;
        private System.Windows.Forms.Panel panSearch;
        private ComBase.Mvc.UserControls.ContentTitle contentTitle1;
        private FarPoint.Win.Spread.FpSpread SSWorkerList;
        private FarPoint.Win.Spread.SheetView SSWorkerList_Sheet1;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Label lblSiteName;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.TextBox TxtNameOrPano;
        private System.Windows.Forms.ComboBox CboDept;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}