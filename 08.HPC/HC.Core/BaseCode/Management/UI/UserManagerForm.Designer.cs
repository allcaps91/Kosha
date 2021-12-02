namespace HC.Core.BaseCode.Management.UI
{
    partial class UserManagerForm
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
            this.tableBody = new System.Windows.Forms.TableLayoutPanel();
            this.panUserList = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.BtnSave = new System.Windows.Forms.Button();
            this.SSUserList = new FarPoint.Win.Spread.FpSpread();
            this.SSUserList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.contentTitle2 = new ComBase.Mvc.UserControls.ContentTitle();
            this.panOcsUser = new System.Windows.Forms.Panel();
            this.SSViewUserList = new FarPoint.Win.Spread.FpSpread();
            this.SSViewUserList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.panSpace = new System.Windows.Forms.Panel();
            this.awd = new ComBase.Mvc.UserControls.ContentTitle();
            this.formTItle1 = new ComBase.Mvc.UserControls.FormTItle();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.tableBody.SuspendLayout();
            this.panUserList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSUserList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSUserList_Sheet1)).BeginInit();
            this.panOcsUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSViewUserList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSViewUserList_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableBody
            // 
            this.tableBody.ColumnCount = 2;
            this.tableBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.33228F));
            this.tableBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.66772F));
            this.tableBody.Controls.Add(this.panUserList, 1, 0);
            this.tableBody.Controls.Add(this.panOcsUser, 0, 0);
            this.tableBody.Location = new System.Drawing.Point(0, 32);
            this.tableBody.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableBody.Name = "tableBody";
            this.tableBody.RowCount = 1;
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableBody.Size = new System.Drawing.Size(1264, 530);
            this.tableBody.TabIndex = 10;
            // 
            // panUserList
            // 
            this.panUserList.Controls.Add(this.button1);
            this.panUserList.Controls.Add(this.BtnSave);
            this.panUserList.Controls.Add(this.SSUserList);
            this.panUserList.Controls.Add(this.panel1);
            this.panUserList.Controls.Add(this.contentTitle2);
            this.panUserList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panUserList.Location = new System.Drawing.Point(576, 2);
            this.panUserList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panUserList.Name = "panUserList";
            this.panUserList.Size = new System.Drawing.Size(685, 526);
            this.panUserList.TabIndex = 9;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(394, 6);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 25);
            this.button1.TabIndex = 8;
            this.button1.Text = "추가";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSave.Location = new System.Drawing.Point(548, 2);
            this.BtnSave.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 25);
            this.BtnSave.TabIndex = 5;
            this.BtnSave.Text = "저장(&S)";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // SSUserList
            // 
            this.SSUserList.AccessibleDescription = "";
            this.SSUserList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSUserList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSUserList.Location = new System.Drawing.Point(0, 36);
            this.SSUserList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SSUserList.Name = "SSUserList";
            this.SSUserList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSUserList_Sheet1});
            this.SSUserList.Size = new System.Drawing.Size(685, 490);
            this.SSUserList.TabIndex = 7;
            // 
            // SSUserList_Sheet1
            // 
            this.SSUserList_Sheet1.Reset();
            this.SSUserList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSUserList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSUserList_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSUserList_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSUserList_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.SSUserList_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSUserList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSUserList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSUserList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSUserList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSUserList_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSUserList_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSUserList_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.SSUserList_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSUserList_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSUserList_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSUserList_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.SSUserList_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSUserList_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSUserList_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSUserList_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.SSUserList_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSUserList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSUserList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSUserList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSUserList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSUserList_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSUserList_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSUserList_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSUserList_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSUserList_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSUserList_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSUserList_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.SSUserList_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSUserList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(685, 2);
            this.panel1.TabIndex = 8;
            // 
            // contentTitle2
            // 
            this.contentTitle2.Dock = System.Windows.Forms.DockStyle.Top;
            this.contentTitle2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.contentTitle2.Location = new System.Drawing.Point(0, 0);
            this.contentTitle2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.contentTitle2.Name = "contentTitle2";
            this.contentTitle2.Size = new System.Drawing.Size(685, 34);
            this.contentTitle2.TabIndex = 11;
            this.contentTitle2.TitleText = "사용자목록";
            // 
            // panOcsUser
            // 
            this.panOcsUser.Controls.Add(this.SSViewUserList);
            this.panOcsUser.Controls.Add(this.BtnAdd);
            this.panOcsUser.Controls.Add(this.panSpace);
            this.panOcsUser.Controls.Add(this.awd);
            this.panOcsUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panOcsUser.Location = new System.Drawing.Point(3, 2);
            this.panOcsUser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panOcsUser.Name = "panOcsUser";
            this.panOcsUser.Size = new System.Drawing.Size(567, 526);
            this.panOcsUser.TabIndex = 9;
            // 
            // SSViewUserList
            // 
            this.SSViewUserList.AccessibleDescription = "";
            this.SSViewUserList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSViewUserList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSViewUserList.Location = new System.Drawing.Point(0, 34);
            this.SSViewUserList.Margin = new System.Windows.Forms.Padding(3, 14, 3, 4);
            this.SSViewUserList.Name = "SSViewUserList";
            this.SSViewUserList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSViewUserList_Sheet1});
            this.SSViewUserList.Size = new System.Drawing.Size(567, 492);
            this.SSViewUserList.TabIndex = 2;
            // 
            // SSViewUserList_Sheet1
            // 
            this.SSViewUserList_Sheet1.Reset();
            this.SSViewUserList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSViewUserList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSViewUserList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSViewUserList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSViewUserList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSViewUserList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSViewUserList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSViewUserList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSViewUserList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSViewUserList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSViewUserList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // BtnAdd
            // 
            this.BtnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnAdd.Location = new System.Drawing.Point(483, 2);
            this.BtnAdd.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(75, 25);
            this.BtnAdd.TabIndex = 6;
            this.BtnAdd.Text = "추가(&A)";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // panSpace
            // 
            this.panSpace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panSpace.Location = new System.Drawing.Point(0, 34);
            this.panSpace.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panSpace.Name = "panSpace";
            this.panSpace.Size = new System.Drawing.Size(567, 492);
            this.panSpace.TabIndex = 5;
            // 
            // awd
            // 
            this.awd.Dock = System.Windows.Forms.DockStyle.Top;
            this.awd.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.awd.Location = new System.Drawing.Point(0, 0);
            this.awd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.awd.Name = "awd";
            this.awd.Size = new System.Drawing.Size(567, 34);
            this.awd.TabIndex = 7;
            this.awd.TitleText = "인사정보";
            // 
            // formTItle1
            // 
            this.formTItle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.formTItle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.formTItle1.Location = new System.Drawing.Point(0, 0);
            this.formTItle1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.formTItle1.Name = "formTItle1";
            this.formTItle1.Size = new System.Drawing.Size(1264, 31);
            this.formTItle1.TabIndex = 11;
            this.formTItle1.TitleText = "사용자 관리";
            // 
            // UserManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 985);
            this.Controls.Add(this.tableBody);
            this.Controls.Add(this.formTItle1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.Name = "UserManagerForm";
            this.Text = "사용자관리";
            this.Load += new System.EventHandler(this.UserManagerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.tableBody.ResumeLayout(false);
            this.panUserList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSUserList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSUserList_Sheet1)).EndInit();
            this.panOcsUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSViewUserList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSViewUserList_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panUserList;
        private FarPoint.Win.Spread.FpSpread SSUserList;
        private FarPoint.Win.Spread.SheetView SSUserList_Sheet1;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Panel panOcsUser;
        private FarPoint.Win.Spread.FpSpread SSViewUserList;
        private FarPoint.Win.Spread.SheetView SSViewUserList_Sheet1;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.TableLayoutPanel tableBody;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panSpace;
        private System.Windows.Forms.Panel panel1;
        private ComBase.Mvc.UserControls.ContentTitle contentTitle2;
        private ComBase.Mvc.UserControls.ContentTitle awd;
        private ComBase.Mvc.UserControls.FormTItle formTItle1;
    }
}