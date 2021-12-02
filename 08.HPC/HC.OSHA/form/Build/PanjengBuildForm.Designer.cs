namespace HC_OSHA
{
    partial class PanjengBuildForm
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
            this.SSSiteList = new FarPoint.Win.Spread.FpSpread();
            this.SSSiteList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.TxtSiteId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.DtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.CboYear = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.label15 = new System.Windows.Forms.Label();
            this.BtnBuild = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSSiteList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSSiteList_Sheet1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // SSSiteList
            // 
            this.SSSiteList.AccessibleDescription = "";
            this.SSSiteList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSSiteList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSSiteList.Location = new System.Drawing.Point(0, 134);
            this.SSSiteList.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.SSSiteList.Name = "SSSiteList";
            this.SSSiteList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSSiteList_Sheet1});
            this.SSSiteList.Size = new System.Drawing.Size(452, 316);
            this.SSSiteList.TabIndex = 124;
            this.SSSiteList.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSWorkerList_CellClick);
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
            // panel1
            // 
            this.panel1.Controls.Add(this.SSSiteList);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(452, 450);
            this.panel1.TabIndex = 125;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.TxtSiteId);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.BtnSearch);
            this.panel2.Controls.Add(this.DtpEndDate);
            this.panel2.Controls.Add(this.CboYear);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.DtpStartDate);
            this.panel2.Controls.Add(this.label15);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(452, 134);
            this.panel2.TabIndex = 126;
            // 
            // TxtSiteId
            // 
            this.TxtSiteId.Location = new System.Drawing.Point(103, 86);
            this.TxtSiteId.Name = "TxtSiteId";
            this.TxtSiteId.Size = new System.Drawing.Size(220, 25);
            this.TxtSiteId.TabIndex = 92;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(22, 85);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(3);
            this.label2.Size = new System.Drawing.Size(75, 25);
            this.label2.TabIndex = 91;
            this.label2.Text = "사업장";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(345, 50);
            this.BtnSearch.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(75, 61);
            this.BtnSearch.TabIndex = 90;
            this.BtnSearch.Text = "검색(&F)";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // DtpEndDate
            // 
            this.DtpEndDate.CustomFormat = "";
            this.DtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpEndDate.Location = new System.Drawing.Point(216, 49);
            this.DtpEndDate.Name = "DtpEndDate";
            this.DtpEndDate.Size = new System.Drawing.Size(107, 25);
            this.DtpEndDate.TabIndex = 89;
            // 
            // CboYear
            // 
            this.CboYear.FormattingEnabled = true;
            this.CboYear.Location = new System.Drawing.Point(103, 12);
            this.CboYear.Name = "CboYear";
            this.CboYear.Size = new System.Drawing.Size(103, 25);
            this.CboYear.TabIndex = 87;
            this.CboYear.SelectedIndexChanged += new System.EventHandler(this.CboYear_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(22, 49);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(3);
            this.label1.Size = new System.Drawing.Size(75, 25);
            this.label1.TabIndex = 86;
            this.label1.Text = "검진기간";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DtpStartDate
            // 
            this.DtpStartDate.CustomFormat = "";
            this.DtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpStartDate.Location = new System.Drawing.Point(103, 49);
            this.DtpStartDate.Name = "DtpStartDate";
            this.DtpStartDate.Size = new System.Drawing.Size(107, 25);
            this.DtpStartDate.TabIndex = 88;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label15.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.Location = new System.Drawing.Point(22, 12);
            this.label15.Name = "label15";
            this.label15.Padding = new System.Windows.Forms.Padding(3);
            this.label15.Size = new System.Drawing.Size(75, 25);
            this.label15.TabIndex = 85;
            this.label15.Text = "검진년도";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnBuild
            // 
            this.BtnBuild.Location = new System.Drawing.Point(458, 49);
            this.BtnBuild.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnBuild.Name = "BtnBuild";
            this.BtnBuild.Size = new System.Drawing.Size(75, 61);
            this.BtnBuild.TabIndex = 93;
            this.BtnBuild.Text = "빌드";
            this.BtnBuild.UseVisualStyleBackColor = true;
            this.BtnBuild.Click += new System.EventHandler(this.BtnBuild_Click);
            // 
            // PanjengBuildForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 450);
            this.Controls.Add(this.BtnBuild);
            this.Controls.Add(this.panel1);
            this.Name = "PanjengBuildForm";
            this.Text = "대행 질병유소견자 사후관리 빌드";
            this.Load += new System.EventHandler(this.PanjengBuildForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSSiteList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSSiteList_Sheet1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private FarPoint.Win.Spread.FpSpread SSSiteList;
        private FarPoint.Win.Spread.SheetView SSSiteList_Sheet1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox CboYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker DtpStartDate;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.DateTimePicker DtpEndDate;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.TextBox TxtSiteId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtnBuild;
    }
}