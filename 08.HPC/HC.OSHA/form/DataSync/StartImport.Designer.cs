namespace HC_OSHA
{
    partial class StartImport
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtnGetSync = new System.Windows.Forms.Button();
            this.RdoIsSyncN = new System.Windows.Forms.RadioButton();
            this.RdoIsSyncY = new System.Windows.Forms.RadioButton();
            this.RdoAll = new System.Windows.Forms.RadioButton();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.DtpendDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.DtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.SSDataSyncList = new FarPoint.Win.Spread.FpSpread();
            this.SSDataSyncList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSDataSyncList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSDataSyncList_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.BtnGetSync);
            this.panel1.Controls.Add(this.RdoIsSyncN);
            this.panel1.Controls.Add(this.RdoIsSyncY);
            this.panel1.Controls.Add(this.RdoAll);
            this.panel1.Controls.Add(this.BtnSearch);
            this.panel1.Controls.Add(this.DtpendDate);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.DtpStartDate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1111, 47);
            this.panel1.TabIndex = 2;
            // 
            // BtnGetSync
            // 
            this.BtnGetSync.Location = new System.Drawing.Point(842, 9);
            this.BtnGetSync.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnGetSync.Name = "BtnGetSync";
            this.BtnGetSync.Size = new System.Drawing.Size(149, 28);
            this.BtnGetSync.TabIndex = 119;
            this.BtnGetSync.Text = "노트북 DB 올리기";
            this.BtnGetSync.UseVisualStyleBackColor = true;
            this.BtnGetSync.Click += new System.EventHandler(this.BtnGetSync_Click);
            // 
            // RdoIsSyncN
            // 
            this.RdoIsSyncN.AutoSize = true;
            this.RdoIsSyncN.Location = new System.Drawing.Point(455, 9);
            this.RdoIsSyncN.Name = "RdoIsSyncN";
            this.RdoIsSyncN.Size = new System.Drawing.Size(65, 21);
            this.RdoIsSyncN.TabIndex = 118;
            this.RdoIsSyncN.TabStop = true;
            this.RdoIsSyncN.Text = "미완료";
            this.RdoIsSyncN.UseVisualStyleBackColor = true;
            this.RdoIsSyncN.CheckedChanged += new System.EventHandler(this.RdoIsSyncN_CheckedChanged);
            // 
            // RdoIsSyncY
            // 
            this.RdoIsSyncY.AutoSize = true;
            this.RdoIsSyncY.Location = new System.Drawing.Point(397, 9);
            this.RdoIsSyncY.Name = "RdoIsSyncY";
            this.RdoIsSyncY.Size = new System.Drawing.Size(52, 21);
            this.RdoIsSyncY.TabIndex = 117;
            this.RdoIsSyncY.TabStop = true;
            this.RdoIsSyncY.Text = "완료";
            this.RdoIsSyncY.UseVisualStyleBackColor = true;
            // 
            // RdoAll
            // 
            this.RdoAll.AutoSize = true;
            this.RdoAll.Location = new System.Drawing.Point(339, 9);
            this.RdoAll.Name = "RdoAll";
            this.RdoAll.Size = new System.Drawing.Size(52, 21);
            this.RdoAll.TabIndex = 116;
            this.RdoAll.TabStop = true;
            this.RdoAll.Text = "전체";
            this.RdoAll.UseVisualStyleBackColor = true;
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(538, 7);
            this.BtnSearch.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(75, 28);
            this.BtnSearch.TabIndex = 115;
            this.BtnSearch.Text = "검색";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // DtpendDate
            // 
            this.DtpendDate.CustomFormat = "yyyy-MM-dd";
            this.DtpendDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpendDate.Location = new System.Drawing.Point(214, 9);
            this.DtpendDate.Name = "DtpendDate";
            this.DtpendDate.Size = new System.Drawing.Size(108, 25);
            this.DtpendDate.TabIndex = 113;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(19, 9);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(4);
            this.label2.Size = new System.Drawing.Size(75, 25);
            this.label2.TabIndex = 111;
            this.label2.Text = "일자";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DtpStartDate
            // 
            this.DtpStartDate.CustomFormat = "yyyy-MM-dd";
            this.DtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpStartDate.Location = new System.Drawing.Point(100, 9);
            this.DtpStartDate.Name = "DtpStartDate";
            this.DtpStartDate.Size = new System.Drawing.Size(108, 25);
            this.DtpStartDate.TabIndex = 112;
            // 
            // SSDataSyncList
            // 
            this.SSDataSyncList.AccessibleDescription = "SSOshaList, Sheet1, Row 0, Column 0, HIC_USERS";
            this.SSDataSyncList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSDataSyncList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSDataSyncList.Location = new System.Drawing.Point(0, 47);
            this.SSDataSyncList.Name = "SSDataSyncList";
            this.SSDataSyncList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSDataSyncList_Sheet1});
            this.SSDataSyncList.Size = new System.Drawing.Size(1111, 403);
            this.SSDataSyncList.TabIndex = 4;
            // 
            // SSDataSyncList_Sheet1
            // 
            this.SSDataSyncList_Sheet1.Reset();
            this.SSDataSyncList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSDataSyncList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSDataSyncList_Sheet1.ColumnCount = 0;
            this.SSDataSyncList_Sheet1.RowCount = 0;
            this.SSDataSyncList_Sheet1.ActiveColumnIndex = -1;
            this.SSDataSyncList_Sheet1.ActiveRowIndex = -1;
            this.SSDataSyncList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSDataSyncList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSDataSyncList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSDataSyncList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSDataSyncList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSDataSyncList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSDataSyncList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSDataSyncList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSDataSyncList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSDataSyncList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1006, 9);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 28);
            this.button1.TabIndex = 120;
            this.button1.Text = "닫기";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // StartImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1111, 450);
            this.Controls.Add(this.SSDataSyncList);
            this.Controls.Add(this.panel1);
            this.Name = "StartImport";
            this.Text = "노트북에서 원내서버로 DB 올리기";
            this.Load += new System.EventHandler(this.StartImport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSDataSyncList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSDataSyncList_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private FarPoint.Win.Spread.FpSpread SSDataSyncList;
        private FarPoint.Win.Spread.SheetView SSDataSyncList_Sheet1;
        private System.Windows.Forms.DateTimePicker DtpendDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker DtpStartDate;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.RadioButton RdoIsSyncN;
        private System.Windows.Forms.RadioButton RdoIsSyncY;
        private System.Windows.Forms.RadioButton RdoAll;
        private System.Windows.Forms.Button BtnGetSync;
        private System.Windows.Forms.Button button1;
    }
}