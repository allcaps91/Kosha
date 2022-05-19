namespace HC_OSHA
{
    partial class SiteMSDSListForm
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
            this.btnPdf = new System.Windows.Forms.Button();
            this.BtnApply = new System.Windows.Forms.Button();
            this.btnExcel = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.LblSite = new System.Windows.Forms.Label();
            this.BtnMange = new System.Windows.Forms.Button();
            this.SSMSDSList = new FarPoint.Win.Spread.FpSpread();
            this.sheetView1 = new FarPoint.Win.Spread.SheetView();
            this.contentTitle3 = new ComBase.Mvc.UserControls.ContentTitle();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSMSDSList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sheetView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panBody
            // 
            this.panBody.Controls.Add(this.btnPdf);
            this.panBody.Controls.Add(this.BtnApply);
            this.panBody.Controls.Add(this.btnExcel);
            this.panBody.Controls.Add(this.btnExit);
            this.panBody.Controls.Add(this.LblSite);
            this.panBody.Controls.Add(this.BtnMange);
            this.panBody.Controls.Add(this.SSMSDSList);
            this.panBody.Controls.Add(this.contentTitle3);
            this.panBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBody.Location = new System.Drawing.Point(0, 0);
            this.panBody.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panBody.Name = "panBody";
            this.panBody.Size = new System.Drawing.Size(1264, 871);
            this.panBody.TabIndex = 0;
            // 
            // btnPdf
            // 
            this.btnPdf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPdf.Location = new System.Drawing.Point(863, 3);
            this.btnPdf.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.btnPdf.Name = "btnPdf";
            this.btnPdf.Size = new System.Drawing.Size(75, 28);
            this.btnPdf.TabIndex = 37;
            this.btnPdf.Text = "pdf저장";
            this.btnPdf.UseVisualStyleBackColor = true;
            this.btnPdf.Click += new System.EventHandler(this.btnPdf_Click);
            // 
            // BtnApply
            // 
            this.BtnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnApply.Location = new System.Drawing.Point(701, 3);
            this.BtnApply.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.BtnApply.Name = "BtnApply";
            this.BtnApply.Size = new System.Drawing.Size(75, 28);
            this.BtnApply.TabIndex = 36;
            this.BtnApply.Text = "불러오기";
            this.BtnApply.UseVisualStyleBackColor = true;
            this.BtnApply.Visible = false;
            this.BtnApply.Click += new System.EventHandler(this.BtnApply_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.Location = new System.Drawing.Point(782, 2);
            this.btnExcel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 28);
            this.btnExcel.TabIndex = 35;
            this.btnExcel.Text = "엑셀저장";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(1105, 2);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 28);
            this.btnExit.TabIndex = 34;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // LblSite
            // 
            this.LblSite.AutoSize = true;
            this.LblSite.Location = new System.Drawing.Point(189, 9);
            this.LblSite.Name = "LblSite";
            this.LblSite.Size = new System.Drawing.Size(43, 17);
            this.LblSite.TabIndex = 13;
            this.LblSite.Text = "label1";
            // 
            // BtnMange
            // 
            this.BtnMange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnMange.Location = new System.Drawing.Point(1024, 2);
            this.BtnMange.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.BtnMange.Name = "BtnMange";
            this.BtnMange.Size = new System.Drawing.Size(75, 28);
            this.BtnMange.TabIndex = 12;
            this.BtnMange.Text = "관리";
            this.BtnMange.UseVisualStyleBackColor = true;
            this.BtnMange.Click += new System.EventHandler(this.BtnMange_Click);
            // 
            // SSMSDSList
            // 
            this.SSMSDSList.AccessibleDescription = "SSMSDSList, Sheet1, Row 0, Column 0, ";
            this.SSMSDSList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSMSDSList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSMSDSList.Location = new System.Drawing.Point(0, 44);
            this.SSMSDSList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SSMSDSList.Name = "SSMSDSList";
            this.SSMSDSList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.sheetView1});
            this.SSMSDSList.Size = new System.Drawing.Size(1264, 827);
            this.SSMSDSList.TabIndex = 5;
            // 
            // sheetView1
            // 
            this.sheetView1.Reset();
            this.sheetView1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.sheetView1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.sheetView1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.sheetView1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.sheetView1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.sheetView1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.sheetView1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.sheetView1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.sheetView1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.sheetView1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.sheetView1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.sheetView1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.sheetView1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.sheetView1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.sheetView1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.sheetView1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.sheetView1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.sheetView1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.sheetView1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.sheetView1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.sheetView1.RowHeader.Columns.Default.Resizable = false;
            this.sheetView1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // contentTitle3
            // 
            this.contentTitle3.Dock = System.Windows.Forms.DockStyle.Top;
            this.contentTitle3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.contentTitle3.Location = new System.Drawing.Point(0, 0);
            this.contentTitle3.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.contentTitle3.Name = "contentTitle3";
            this.contentTitle3.Size = new System.Drawing.Size(1264, 44);
            this.contentTitle3.TabIndex = 4;
            this.contentTitle3.TitleText = "화학물질 MSDS 목록 현황";
            this.contentTitle3.Load += new System.EventHandler(this.contentTitle3_Load);
            // 
            // SiteMSDSListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 871);
            this.Controls.Add(this.panBody);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SiteMSDSListForm";
            this.Text = "SiteMSDSListForm";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panBody.ResumeLayout(false);
            this.panBody.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSMSDSList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sheetView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panBody;
        private FarPoint.Win.Spread.FpSpread SSMSDSList;
        private FarPoint.Win.Spread.SheetView sheetView1;
        private ComBase.Mvc.UserControls.ContentTitle contentTitle3;
        private System.Windows.Forms.Button BtnMange;
        private System.Windows.Forms.Label LblSite;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.Button BtnApply;
        private System.Windows.Forms.Button btnPdf;
    }
}