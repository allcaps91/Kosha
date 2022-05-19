namespace HC_OSHA
{
    partial class FrmMsdsReport
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
            this.btnClose = new System.Windows.Forms.Button();
            this.SSList = new FarPoint.Win.Spread.FpSpread();
            this.SSList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panSearch = new System.Windows.Forms.Panel();
            this.txtView = new System.Windows.Forms.TextBox();
            this.TxtLtdcode = new System.Windows.Forms.TextBox();
            this.BtnSearchSite = new System.Windows.Forms.Button();
            this.lblLTD02 = new System.Windows.Forms.Label();
            this.btnExcel = new System.Windows.Forms.Button();
            this.BtnPrint = new System.Windows.Forms.Button();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.formTItle1 = new ComBase.Mvc.UserControls.FormTItle();
            this.RdoCasNo = new System.Windows.Forms.RadioButton();
            this.RdoJepum = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).BeginInit();
            this.panSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(988, 0);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 35);
            this.btnClose.TabIndex = 24;
            this.btnClose.Text = "닫기";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // SSList
            // 
            this.SSList.AccessibleDescription = "SSList, Sheet1, Row 0, Column 0, ";
            this.SSList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSList.Location = new System.Drawing.Point(0, 79);
            this.SSList.Name = "SSList";
            this.SSList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSList_Sheet1});
            this.SSList.Size = new System.Drawing.Size(1121, 682);
            this.SSList.TabIndex = 23;
            // 
            // SSList_Sheet1
            // 
            this.SSList_Sheet1.Reset();
            this.SSList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSList_Sheet1.ColumnCount = 0;
            this.SSList_Sheet1.RowCount = 0;
            this.SSList_Sheet1.ActiveColumnIndex = -1;
            this.SSList_Sheet1.ActiveRowIndex = -1;
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.SSList_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.SSList_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.SSList_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
            this.SSList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSList_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSList_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.SSList_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panSearch
            // 
            this.panSearch.BackColor = System.Drawing.Color.White;
            this.panSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSearch.Controls.Add(this.button1);
            this.panSearch.Controls.Add(this.RdoCasNo);
            this.panSearch.Controls.Add(this.RdoJepum);
            this.panSearch.Controls.Add(this.txtView);
            this.panSearch.Controls.Add(this.TxtLtdcode);
            this.panSearch.Controls.Add(this.BtnSearchSite);
            this.panSearch.Controls.Add(this.lblLTD02);
            this.panSearch.Controls.Add(this.btnExcel);
            this.panSearch.Controls.Add(this.BtnPrint);
            this.panSearch.Controls.Add(this.BtnSearch);
            this.panSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSearch.Location = new System.Drawing.Point(0, 35);
            this.panSearch.Name = "panSearch";
            this.panSearch.Size = new System.Drawing.Size(1121, 44);
            this.panSearch.TabIndex = 21;
            // 
            // txtView
            // 
            this.txtView.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.txtView.Location = new System.Drawing.Point(543, 6);
            this.txtView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtView.Name = "txtView";
            this.txtView.Size = new System.Drawing.Size(131, 25);
            this.txtView.TabIndex = 25;
            // 
            // TxtLtdcode
            // 
            this.TxtLtdcode.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.TxtLtdcode.Location = new System.Drawing.Point(67, 9);
            this.TxtLtdcode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TxtLtdcode.Name = "TxtLtdcode";
            this.TxtLtdcode.Size = new System.Drawing.Size(174, 25);
            this.TxtLtdcode.TabIndex = 1;
            // 
            // BtnSearchSite
            // 
            this.BtnSearchSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSearchSite.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.BtnSearchSite.Location = new System.Drawing.Point(267, 10);
            this.BtnSearchSite.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearchSite.Name = "BtnSearchSite";
            this.BtnSearchSite.Size = new System.Drawing.Size(50, 28);
            this.BtnSearchSite.TabIndex = 142;
            this.BtnSearchSite.Text = "찾기";
            this.BtnSearchSite.UseVisualStyleBackColor = true;
            // 
            // lblLTD02
            // 
            this.lblLTD02.BackColor = System.Drawing.Color.LightBlue;
            this.lblLTD02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLTD02.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.lblLTD02.Location = new System.Drawing.Point(11, 8);
            this.lblLTD02.Name = "lblLTD02";
            this.lblLTD02.Size = new System.Drawing.Size(50, 28);
            this.lblLTD02.TabIndex = 143;
            this.lblLTD02.Text = "회사";
            this.lblLTD02.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnExcel
            // 
            this.btnExcel.Location = new System.Drawing.Point(891, 6);
            this.btnExcel.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 28);
            this.btnExcel.TabIndex = 4;
            this.btnExcel.Text = "엑셀";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // BtnPrint
            // 
            this.BtnPrint.Location = new System.Drawing.Point(810, 6);
            this.BtnPrint.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnPrint.Name = "BtnPrint";
            this.BtnPrint.Size = new System.Drawing.Size(75, 28);
            this.BtnPrint.TabIndex = 3;
            this.BtnPrint.Text = "인쇄(&P)";
            this.BtnPrint.UseVisualStyleBackColor = true;
            this.BtnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(680, 6);
            this.BtnSearch.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(75, 28);
            this.BtnSearch.TabIndex = 2;
            this.BtnSearch.Text = "검색(&F)";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // formTItle1
            // 
            this.formTItle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.formTItle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.formTItle1.Location = new System.Drawing.Point(0, 0);
            this.formTItle1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.formTItle1.Name = "formTItle1";
            this.formTItle1.Size = new System.Drawing.Size(1121, 35);
            this.formTItle1.TabIndex = 22;
            this.formTItle1.TitleText = "화학물질 MSDS 목록 현황";
            // 
            // RdoCasNo
            // 
            this.RdoCasNo.AutoSize = true;
            this.RdoCasNo.Location = new System.Drawing.Point(470, 14);
            this.RdoCasNo.Name = "RdoCasNo";
            this.RdoCasNo.Size = new System.Drawing.Size(62, 16);
            this.RdoCasNo.TabIndex = 145;
            this.RdoCasNo.Text = "CasNo";
            this.RdoCasNo.UseVisualStyleBackColor = true;
            // 
            // RdoJepum
            // 
            this.RdoJepum.AutoSize = true;
            this.RdoJepum.Checked = true;
            this.RdoJepum.Location = new System.Drawing.Point(390, 14);
            this.RdoJepum.Name = "RdoJepum";
            this.RdoJepum.Size = new System.Drawing.Size(59, 16);
            this.RdoJepum.TabIndex = 144;
            this.RdoJepum.TabStop = true;
            this.RdoJepum.Text = "물질명";
            this.RdoJepum.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(972, 6);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 28);
            this.button1.TabIndex = 146;
            this.button1.Text = "PDF저장";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FrmMsdsReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1121, 761);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.SSList);
            this.Controls.Add(this.panSearch);
            this.Controls.Add(this.formTItle1);
            this.Name = "FrmMsdsReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmMsdsReport";
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).EndInit();
            this.panSearch.ResumeLayout(false);
            this.panSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private FarPoint.Win.Spread.FpSpread SSList;
        private FarPoint.Win.Spread.SheetView SSList_Sheet1;
        private System.Windows.Forms.Panel panSearch;
        private System.Windows.Forms.TextBox TxtLtdcode;
        private System.Windows.Forms.Button BtnSearchSite;
        private System.Windows.Forms.Label lblLTD02;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.Button BtnPrint;
        private System.Windows.Forms.Button BtnSearch;
        private ComBase.Mvc.UserControls.FormTItle formTItle1;
        private System.Windows.Forms.TextBox txtView;
        private System.Windows.Forms.RadioButton RdoCasNo;
        private System.Windows.Forms.RadioButton RdoJepum;
        private System.Windows.Forms.Button button1;
    }
}