namespace HC.Core.BaseCode.Management.UI
{
    partial class CodeManaerForm
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
            this.panGroupCode = new System.Windows.Forms.Panel();
            this.SSGroupCodeList = new FarPoint.Win.Spread.FpSpread();
            this.SSGroupCodeList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panSpace = new System.Windows.Forms.Panel();
            this.contentTitleGroupCode = new ComBase.Mvc.UserControls.ContentTitle();
            this.tableBody = new System.Windows.Forms.TableLayoutPanel();
            this.panCode = new System.Windows.Forms.Panel();
            this.BtnCodeSave = new System.Windows.Forms.Button();
            this.BtnCodeAdd = new System.Windows.Forms.Button();
            this.SSCodeList = new FarPoint.Win.Spread.FpSpread();
            this.SSCodeList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panSpace2 = new System.Windows.Forms.Panel();
            this.contentTitle1 = new ComBase.Mvc.UserControls.ContentTitle();
            this.formTItle1 = new ComBase.Mvc.UserControls.FormTItle();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panGroupCode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSGroupCodeList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSGroupCodeList_Sheet1)).BeginInit();
            this.tableBody.SuspendLayout();
            this.panCode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSCodeList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSCodeList_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panGroupCode
            // 
            this.panGroupCode.Controls.Add(this.SSGroupCodeList);
            this.panGroupCode.Controls.Add(this.panSpace);
            this.panGroupCode.Controls.Add(this.contentTitleGroupCode);
            this.panGroupCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panGroupCode.Location = new System.Drawing.Point(3, 3);
            this.panGroupCode.Name = "panGroupCode";
            this.panGroupCode.Size = new System.Drawing.Size(424, 596);
            this.panGroupCode.TabIndex = 10;
            // 
            // SSGroupCodeList
            // 
            this.SSGroupCodeList.AccessibleDescription = "";
            this.SSGroupCodeList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSGroupCodeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSGroupCodeList.Location = new System.Drawing.Point(0, 41);
            this.SSGroupCodeList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SSGroupCodeList.Name = "SSGroupCodeList";
            this.SSGroupCodeList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSGroupCodeList_Sheet1});
            this.SSGroupCodeList.Size = new System.Drawing.Size(424, 555);
            this.SSGroupCodeList.TabIndex = 2;
            this.SSGroupCodeList.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSGroupCodeList_CellDoubleClick);
            // 
            // SSGroupCodeList_Sheet1
            // 
            this.SSGroupCodeList_Sheet1.Reset();
            this.SSGroupCodeList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSGroupCodeList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSGroupCodeList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSGroupCodeList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSGroupCodeList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSGroupCodeList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSGroupCodeList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSGroupCodeList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSGroupCodeList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSGroupCodeList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSGroupCodeList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panSpace
            // 
            this.panSpace.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSpace.Location = new System.Drawing.Point(0, 38);
            this.panSpace.Name = "panSpace";
            this.panSpace.Size = new System.Drawing.Size(424, 3);
            this.panSpace.TabIndex = 27;
            // 
            // contentTitleGroupCode
            // 
            this.contentTitleGroupCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.contentTitleGroupCode.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.contentTitleGroupCode.Location = new System.Drawing.Point(0, 0);
            this.contentTitleGroupCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.contentTitleGroupCode.Name = "contentTitleGroupCode";
            this.contentTitleGroupCode.Size = new System.Drawing.Size(424, 38);
            this.contentTitleGroupCode.TabIndex = 28;
            this.contentTitleGroupCode.TitleText = "그룹코드";
            // 
            // tableBody
            // 
            this.tableBody.ColumnCount = 2;
            this.tableBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 430F));
            this.tableBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableBody.Controls.Add(this.panCode, 0, 0);
            this.tableBody.Controls.Add(this.panGroupCode, 0, 0);
            this.tableBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableBody.Location = new System.Drawing.Point(0, 35);
            this.tableBody.Name = "tableBody";
            this.tableBody.RowCount = 1;
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableBody.Size = new System.Drawing.Size(1137, 602);
            this.tableBody.TabIndex = 5;
            // 
            // panCode
            // 
            this.panCode.Controls.Add(this.BtnCodeSave);
            this.panCode.Controls.Add(this.BtnCodeAdd);
            this.panCode.Controls.Add(this.SSCodeList);
            this.panCode.Controls.Add(this.panSpace2);
            this.panCode.Controls.Add(this.contentTitle1);
            this.panCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panCode.Location = new System.Drawing.Point(433, 3);
            this.panCode.Name = "panCode";
            this.panCode.Size = new System.Drawing.Size(701, 596);
            this.panCode.TabIndex = 11;
            // 
            // BtnCodeSave
            // 
            this.BtnCodeSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCodeSave.Location = new System.Drawing.Point(623, 2);
            this.BtnCodeSave.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnCodeSave.Name = "BtnCodeSave";
            this.BtnCodeSave.Size = new System.Drawing.Size(75, 28);
            this.BtnCodeSave.TabIndex = 9;
            this.BtnCodeSave.Text = "저장(&S)";
            this.BtnCodeSave.UseVisualStyleBackColor = true;
            this.BtnCodeSave.Click += new System.EventHandler(this.BtnCodeSave_Click);
            // 
            // BtnCodeAdd
            // 
            this.BtnCodeAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCodeAdd.Location = new System.Drawing.Point(543, 2);
            this.BtnCodeAdd.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnCodeAdd.Name = "BtnCodeAdd";
            this.BtnCodeAdd.Size = new System.Drawing.Size(75, 28);
            this.BtnCodeAdd.TabIndex = 6;
            this.BtnCodeAdd.Text = "추가(&A)";
            this.BtnCodeAdd.UseVisualStyleBackColor = true;
            this.BtnCodeAdd.Click += new System.EventHandler(this.BtnCodeAdd_Click);
            // 
            // SSCodeList
            // 
            this.SSCodeList.AccessibleDescription = "";
            this.SSCodeList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSCodeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSCodeList.Location = new System.Drawing.Point(0, 41);
            this.SSCodeList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SSCodeList.Name = "SSCodeList";
            this.SSCodeList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSCodeList_Sheet1});
            this.SSCodeList.Size = new System.Drawing.Size(701, 555);
            this.SSCodeList.TabIndex = 2;
            // 
            // SSCodeList_Sheet1
            // 
            this.SSCodeList_Sheet1.Reset();
            this.SSCodeList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSCodeList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSCodeList_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSCodeList_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSCodeList_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.SSCodeList_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSCodeList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSCodeList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSCodeList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSCodeList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSCodeList_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSCodeList_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSCodeList_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.SSCodeList_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSCodeList_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSCodeList_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSCodeList_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.SSCodeList_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSCodeList_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSCodeList_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSCodeList_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.SSCodeList_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSCodeList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSCodeList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSCodeList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSCodeList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSCodeList_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSCodeList_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSCodeList_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSCodeList_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSCodeList_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSCodeList_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSCodeList_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.SSCodeList_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSCodeList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panSpace2
            // 
            this.panSpace2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSpace2.Location = new System.Drawing.Point(0, 38);
            this.panSpace2.Name = "panSpace2";
            this.panSpace2.Size = new System.Drawing.Size(701, 3);
            this.panSpace2.TabIndex = 27;
            // 
            // contentTitle1
            // 
            this.contentTitle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.contentTitle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.contentTitle1.Location = new System.Drawing.Point(0, 0);
            this.contentTitle1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.contentTitle1.Name = "contentTitle1";
            this.contentTitle1.Size = new System.Drawing.Size(701, 38);
            this.contentTitle1.TabIndex = 28;
            this.contentTitle1.TitleText = "코드관리";
            // 
            // formTItle1
            // 
            this.formTItle1.BackColor = System.Drawing.SystemColors.Control;
            this.formTItle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.formTItle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.formTItle1.Location = new System.Drawing.Point(0, 0);
            this.formTItle1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.formTItle1.Name = "formTItle1";
            this.formTItle1.Size = new System.Drawing.Size(1137, 35);
            this.formTItle1.TabIndex = 6;
            this.formTItle1.TitleText = "코드 관리";
            // 
            // CodeManaerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1137, 637);
            this.Controls.Add(this.tableBody);
            this.Controls.Add(this.formTItle1);
            this.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.Name = "CodeManaerForm";
            this.Text = "코드관리";
            this.Load += new System.EventHandler(this.CodeManaerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panGroupCode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSGroupCodeList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSGroupCodeList_Sheet1)).EndInit();
            this.tableBody.ResumeLayout(false);
            this.panCode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSCodeList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSCodeList_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panGroupCode;
        private FarPoint.Win.Spread.FpSpread SSGroupCodeList;
        private FarPoint.Win.Spread.SheetView SSGroupCodeList_Sheet1;
        private System.Windows.Forms.TableLayoutPanel tableBody;
        private System.Windows.Forms.Panel panSpace;
        private ComBase.Mvc.UserControls.FormTItle formTItle1;
        private ComBase.Mvc.UserControls.ContentTitle contentTitleGroupCode;
        private System.Windows.Forms.Panel panCode;
        private System.Windows.Forms.Button BtnCodeSave;
        private FarPoint.Win.Spread.FpSpread SSCodeList;
        private FarPoint.Win.Spread.SheetView SSCodeList_Sheet1;
        private System.Windows.Forms.Panel panSpace2;
        private System.Windows.Forms.Button BtnCodeAdd;
        private ComBase.Mvc.UserControls.ContentTitle contentTitle1;
    }
}