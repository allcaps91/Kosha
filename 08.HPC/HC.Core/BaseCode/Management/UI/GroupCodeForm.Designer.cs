namespace HC.Core.BaseCode.Management.UI
{
    partial class GroupCodeForm
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
            this.contentTitleGroupCode = new ComBase.Mvc.UserControls.ContentTitle();
            this.SSGroupCodeList = new FarPoint.Win.Spread.FpSpread();
            this.SSGroupCodeList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.BtnGroupCodeSave = new System.Windows.Forms.Button();
            this.BtnGroupCodeAdd = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSGroupCodeList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSGroupCodeList_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // formTItle1
            // 
            this.formTItle1.BackColor = System.Drawing.SystemColors.Control;
            this.formTItle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.formTItle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.formTItle1.Location = new System.Drawing.Point(0, 0);
            this.formTItle1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.formTItle1.Name = "formTItle1";
            this.formTItle1.Size = new System.Drawing.Size(800, 35);
            this.formTItle1.TabIndex = 7;
            this.formTItle1.TitleText = "그룹코드 관리";
            // 
            // contentTitleGroupCode
            // 
            this.contentTitleGroupCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.contentTitleGroupCode.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.contentTitleGroupCode.Location = new System.Drawing.Point(0, 35);
            this.contentTitleGroupCode.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.contentTitleGroupCode.Name = "contentTitleGroupCode";
            this.contentTitleGroupCode.Size = new System.Drawing.Size(800, 38);
            this.contentTitleGroupCode.TabIndex = 29;
            this.contentTitleGroupCode.TitleText = "그룹코드";
            // 
            // SSGroupCodeList
            // 
            this.SSGroupCodeList.AccessibleDescription = "";
            this.SSGroupCodeList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSGroupCodeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSGroupCodeList.Location = new System.Drawing.Point(0, 73);
            this.SSGroupCodeList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SSGroupCodeList.Name = "SSGroupCodeList";
            this.SSGroupCodeList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSGroupCodeList_Sheet1});
            this.SSGroupCodeList.Size = new System.Drawing.Size(800, 377);
            this.SSGroupCodeList.TabIndex = 30;
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
            // BtnGroupCodeSave
            // 
            this.BtnGroupCodeSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnGroupCodeSave.Location = new System.Drawing.Point(720, 37);
            this.BtnGroupCodeSave.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnGroupCodeSave.Name = "BtnGroupCodeSave";
            this.BtnGroupCodeSave.Size = new System.Drawing.Size(75, 28);
            this.BtnGroupCodeSave.TabIndex = 32;
            this.BtnGroupCodeSave.Text = "저장(&S)";
            this.BtnGroupCodeSave.UseVisualStyleBackColor = true;
            this.BtnGroupCodeSave.Click += new System.EventHandler(this.BtnGroupCodeSave_Click);
            // 
            // BtnGroupCodeAdd
            // 
            this.BtnGroupCodeAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnGroupCodeAdd.Location = new System.Drawing.Point(640, 37);
            this.BtnGroupCodeAdd.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnGroupCodeAdd.Name = "BtnGroupCodeAdd";
            this.BtnGroupCodeAdd.Size = new System.Drawing.Size(75, 28);
            this.BtnGroupCodeAdd.TabIndex = 31;
            this.BtnGroupCodeAdd.Text = "추가(&A)";
            this.BtnGroupCodeAdd.UseVisualStyleBackColor = true;
            this.BtnGroupCodeAdd.Click += new System.EventHandler(this.BtnGroupCodeAdd_Click);
            // 
            // GroupCodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BtnGroupCodeSave);
            this.Controls.Add(this.BtnGroupCodeAdd);
            this.Controls.Add(this.SSGroupCodeList);
            this.Controls.Add(this.contentTitleGroupCode);
            this.Controls.Add(this.formTItle1);
            this.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.Name = "GroupCodeForm";
            this.Text = "GroupCodeForm";
            this.Load += new System.EventHandler(this.GroupCodeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSGroupCodeList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSGroupCodeList_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ComBase.Mvc.UserControls.FormTItle formTItle1;
        private ComBase.Mvc.UserControls.ContentTitle contentTitleGroupCode;
        private FarPoint.Win.Spread.FpSpread SSGroupCodeList;
        private FarPoint.Win.Spread.SheetView SSGroupCodeList_Sheet1;
        private System.Windows.Forms.Button BtnGroupCodeSave;
        private System.Windows.Forms.Button BtnGroupCodeAdd;
    }
}