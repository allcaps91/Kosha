namespace HC_OSHA.form.Etc
{
    partial class SiteMSDSManagerForm
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
            this.formTItle1 = new ComBase.Mvc.UserControls.FormTItle();
            this.panSearch = new System.Windows.Forms.Panel();
            this.RdoMsdsCasNo = new System.Windows.Forms.RadioButton();
            this.RdoMsdsName = new System.Windows.Forms.RadioButton();
            this.BtnSearchMsds = new System.Windows.Forms.Button();
            this.TxtSearchMsdsWord = new System.Windows.Forms.TextBox();
            this.SSMSDSList = new FarPoint.Win.Spread.FpSpread();
            this.SSMSDSList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSMSDSList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSMSDSList_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // formTItle1
            // 
            this.formTItle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.formTItle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.formTItle1.Location = new System.Drawing.Point(0, 0);
            this.formTItle1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.formTItle1.Name = "formTItle1";
            this.formTItle1.Size = new System.Drawing.Size(1020, 35);
            this.formTItle1.TabIndex = 5;
            this.formTItle1.TitleText = "화학물질 MSDS 관리";
            // 
            // panSearch
            // 
            this.panSearch.BackColor = System.Drawing.Color.White;
            this.panSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSearch.Controls.Add(this.RdoMsdsCasNo);
            this.panSearch.Controls.Add(this.RdoMsdsName);
            this.panSearch.Controls.Add(this.BtnSearchMsds);
            this.panSearch.Controls.Add(this.TxtSearchMsdsWord);
            this.panSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSearch.Location = new System.Drawing.Point(0, 35);
            this.panSearch.Name = "panSearch";
            this.panSearch.Size = new System.Drawing.Size(1020, 48);
            this.panSearch.TabIndex = 6;
            // 
            // RdoMsdsCasNo
            // 
            this.RdoMsdsCasNo.AutoSize = true;
            this.RdoMsdsCasNo.Location = new System.Drawing.Point(121, 13);
            this.RdoMsdsCasNo.Name = "RdoMsdsCasNo";
            this.RdoMsdsCasNo.Size = new System.Drawing.Size(65, 21);
            this.RdoMsdsCasNo.TabIndex = 21;
            this.RdoMsdsCasNo.Text = "CasNo";
            this.RdoMsdsCasNo.UseVisualStyleBackColor = true;
            // 
            // RdoMsdsName
            // 
            this.RdoMsdsName.AutoSize = true;
            this.RdoMsdsName.Checked = true;
            this.RdoMsdsName.Location = new System.Drawing.Point(41, 13);
            this.RdoMsdsName.Name = "RdoMsdsName";
            this.RdoMsdsName.Size = new System.Drawing.Size(65, 21);
            this.RdoMsdsName.TabIndex = 20;
            this.RdoMsdsName.TabStop = true;
            this.RdoMsdsName.Text = "물질명";
            this.RdoMsdsName.UseVisualStyleBackColor = true;
            // 
            // BtnSearchMsds
            // 
            this.BtnSearchMsds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSearchMsds.Location = new System.Drawing.Point(928, 11);
            this.BtnSearchMsds.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearchMsds.Name = "BtnSearchMsds";
            this.BtnSearchMsds.Size = new System.Drawing.Size(75, 28);
            this.BtnSearchMsds.TabIndex = 19;
            this.BtnSearchMsds.Text = "검 색(&F)";
            this.BtnSearchMsds.UseVisualStyleBackColor = true;
            // 
            // TxtSearchMsdsWord
            // 
            this.TxtSearchMsdsWord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtSearchMsdsWord.Location = new System.Drawing.Point(711, 13);
            this.TxtSearchMsdsWord.Name = "TxtSearchMsdsWord";
            this.TxtSearchMsdsWord.Size = new System.Drawing.Size(211, 25);
            this.TxtSearchMsdsWord.TabIndex = 18;
            // 
            // SSMSDSList
            // 
            this.SSMSDSList.AccessibleDescription = "";
            this.SSMSDSList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSMSDSList.Location = new System.Drawing.Point(42, 110);
            this.SSMSDSList.Name = "SSMSDSList";
            this.SSMSDSList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSMSDSList_Sheet1});
            this.SSMSDSList.Size = new System.Drawing.Size(857, 167);
            this.SSMSDSList.TabIndex = 7;
            // 
            // SSMSDSList_Sheet1
            // 
            this.SSMSDSList_Sheet1.Reset();
            this.SSMSDSList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSMSDSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSMSDSList_Sheet1.ColumnCount = 0;
            this.SSMSDSList_Sheet1.RowCount = 0;
            this.SSMSDSList_Sheet1.ActiveColumnIndex = -1;
            this.SSMSDSList_Sheet1.ActiveRowIndex = -1;
            this.SSMSDSList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMSDSList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSMSDSList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSMSDSList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMSDSList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMSDSList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSMSDSList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSMSDSList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMSDSList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSMSDSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // SiteMSDSManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1020, 718);
            this.Controls.Add(this.SSMSDSList);
            this.Controls.Add(this.panSearch);
            this.Controls.Add(this.formTItle1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SiteMSDSManagerForm";
            this.Text = "SiteMSDSManagerForm";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panSearch.ResumeLayout(false);
            this.panSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSMSDSList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSMSDSList_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private ComBase.Mvc.UserControls.FormTItle formTItle1;
        private System.Windows.Forms.Panel panSearch;
        private System.Windows.Forms.RadioButton RdoMsdsCasNo;
        private System.Windows.Forms.RadioButton RdoMsdsName;
        private System.Windows.Forms.Button BtnSearchMsds;
        private System.Windows.Forms.TextBox TxtSearchMsdsWord;
        private FarPoint.Win.Spread.FpSpread SSMSDSList;
        private FarPoint.Win.Spread.SheetView SSMSDSList_Sheet1;
    }
}