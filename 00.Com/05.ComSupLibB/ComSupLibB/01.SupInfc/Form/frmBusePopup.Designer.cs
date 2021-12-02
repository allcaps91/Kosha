namespace ComSupLibB.SupInfc
{
    partial class frmBusePopup
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.ssMain = new FarPoint.Win.Spread.FpSpread();
            this.ssMain_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtBuName = new System.Windows.Forms.TextBox();
            this.btnDeptSearch = new System.Windows.Forms.Button();
            this.lblDateTitle = new System.Windows.Forms.Label();
            this.txtPtInfo = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnUserSearch = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ssUser = new FarPoint.Win.Spread.FpSpread();
            this.ssUser_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ssMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMain_Sheet1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssUser_Sheet1)).BeginInit();
            this.panTitle.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // ssMain
            // 
            this.ssMain.AccessibleDescription = "ssMain, Sheet1, Row 0, Column 0, ";
            this.ssMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssMain.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssMain.Location = new System.Drawing.Point(0, 76);
            this.ssMain.Name = "ssMain";
            this.ssMain.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssMain_Sheet1});
            this.ssMain.Size = new System.Drawing.Size(335, 457);
            this.ssMain.TabIndex = 14;
            this.ssMain.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssMain_CellClick);
            // 
            // ssMain_Sheet1
            // 
            this.ssMain_Sheet1.Reset();
            this.ssMain_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssMain_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssMain_Sheet1.ColumnCount = 2;
            this.ssMain_Sheet1.RowCount = 10;
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "부서코드";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "부서명";
            this.ssMain_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssMain_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssMain_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssMain_Sheet1.Columns.Get(0).Label = "부서코드";
            this.ssMain_Sheet1.Columns.Get(0).Locked = true;
            this.ssMain_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMain_Sheet1.Columns.Get(0).Width = 69F;
            this.ssMain_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssMain_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssMain_Sheet1.Columns.Get(1).Label = "부서명";
            this.ssMain_Sheet1.Columns.Get(1).Locked = true;
            this.ssMain_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMain_Sheet1.Columns.Get(1).Width = 200F;
            this.ssMain_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;
            this.ssMain_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssMain_Sheet1.Rows.Default.Height = 25F;
            this.ssMain_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.Single;
            this.ssMain_Sheet1.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            this.ssMain_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ssMain);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(335, 533);
            this.panel1.TabIndex = 15;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.txtBuName);
            this.panel2.Controls.Add(this.btnDeptSearch);
            this.panel2.Controls.Add(this.lblDateTitle);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(335, 76);
            this.panel2.TabIndex = 0;
            // 
            // txtBuName
            // 
            this.txtBuName.Location = new System.Drawing.Point(52, 42);
            this.txtBuName.Name = "txtBuName";
            this.txtBuName.Size = new System.Drawing.Size(200, 21);
            this.txtBuName.TabIndex = 2;
            this.txtBuName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            // 
            // btnDeptSearch
            // 
            this.btnDeptSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeptSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnDeptSearch.Location = new System.Drawing.Point(261, 38);
            this.btnDeptSearch.Name = "btnDeptSearch";
            this.btnDeptSearch.Size = new System.Drawing.Size(72, 30);
            this.btnDeptSearch.TabIndex = 6;
            this.btnDeptSearch.Text = "조회";
            this.btnDeptSearch.UseVisualStyleBackColor = false;
            this.btnDeptSearch.Click += new System.EventHandler(this.btnDeptSearch_Click);
            // 
            // lblDateTitle
            // 
            this.lblDateTitle.AutoSize = true;
            this.lblDateTitle.Location = new System.Drawing.Point(5, 47);
            this.lblDateTitle.Name = "lblDateTitle";
            this.lblDateTitle.Size = new System.Drawing.Size(41, 12);
            this.lblDateTitle.TabIndex = 1;
            this.lblDateTitle.Text = "부서명";
            // 
            // txtPtInfo
            // 
            this.txtPtInfo.Location = new System.Drawing.Point(64, 42);
            this.txtPtInfo.Name = "txtPtInfo";
            this.txtPtInfo.Size = new System.Drawing.Size(219, 21);
            this.txtPtInfo.TabIndex = 3;
            this.txtPtInfo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.btnUserSearch);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.txtPtInfo);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(335, 34);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(382, 76);
            this.panel3.TabIndex = 16;
            // 
            // btnUserSearch
            // 
            this.btnUserSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUserSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnUserSearch.Location = new System.Drawing.Point(298, 38);
            this.btnUserSearch.Name = "btnUserSearch";
            this.btnUserSearch.Size = new System.Drawing.Size(72, 30);
            this.btnUserSearch.TabIndex = 17;
            this.btnUserSearch.Text = "조회";
            this.btnUserSearch.UseVisualStyleBackColor = false;
            this.btnUserSearch.Click += new System.EventHandler(this.btnUserSearch_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "ID/성명";
            // 
            // ssUser
            // 
            this.ssUser.AccessibleDescription = "ssMain, Sheet1, Row 0, Column 0, ";
            this.ssUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssUser.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssUser.Location = new System.Drawing.Point(335, 110);
            this.ssUser.Name = "ssUser";
            this.ssUser.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssUser_Sheet1});
            this.ssUser.Size = new System.Drawing.Size(382, 457);
            this.ssUser.TabIndex = 17;
            this.ssUser.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssUser_CellDoubleClick);
            // 
            // ssUser_Sheet1
            // 
            this.ssUser_Sheet1.Reset();
            this.ssUser_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssUser_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssUser_Sheet1.ColumnCount = 4;
            this.ssUser_Sheet1.RowCount = 10;
            this.ssUser_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssUser_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssUser_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssUser_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssUser_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "부서코드";
            this.ssUser_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "부서";
            this.ssUser_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "ID";
            this.ssUser_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "성명";
            this.ssUser_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssUser_Sheet1.Columns.Get(0).CellType = textCellType3;
            this.ssUser_Sheet1.Columns.Get(0).Label = "부서코드";
            this.ssUser_Sheet1.Columns.Get(0).Visible = false;
            this.ssUser_Sheet1.Columns.Get(1).CellType = textCellType4;
            this.ssUser_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssUser_Sheet1.Columns.Get(1).Label = "부서";
            this.ssUser_Sheet1.Columns.Get(1).Locked = true;
            this.ssUser_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssUser_Sheet1.Columns.Get(1).Width = 170F;
            this.ssUser_Sheet1.Columns.Get(2).CellType = textCellType5;
            this.ssUser_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssUser_Sheet1.Columns.Get(2).Label = "ID";
            this.ssUser_Sheet1.Columns.Get(2).Locked = true;
            this.ssUser_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssUser_Sheet1.Columns.Get(3).Label = "성명";
            this.ssUser_Sheet1.Columns.Get(3).Width = 80F;
            this.ssUser_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssUser_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssUser_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssUser_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssUser_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;
            this.ssUser_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssUser_Sheet1.Rows.Default.Height = 25F;
            this.ssUser_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.Single;
            this.ssUser_Sheet1.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            this.ssUser_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(717, 34);
            this.panTitle.TabIndex = 20;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(639, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel4.Controls.Add(this.label2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(335, 30);
            this.panel4.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(335, 30);
            this.label2.TabIndex = 0;
            this.label2.Text = "부서조회";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel5.Controls.Add(this.label3);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(382, 30);
            this.panel5.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(382, 30);
            this.label3.TabIndex = 0;
            this.label3.Text = "사용자 조회";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(10, 7);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(99, 16);
            this.lblTitle.TabIndex = 15;
            this.lblTitle.Text = "사용자 조회";
            this.lblTitle.Visible = false;
            // 
            // frmBusePopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(717, 567);
            this.Controls.Add(this.ssUser);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.MinimumSize = new System.Drawing.Size(547, 489);
            this.Name = "frmBusePopup";
            this.Text = "사용자찾기";
            this.Load += new System.EventHandler(this.frmBusePopup_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ssMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMain_Sheet1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssUser_Sheet1)).EndInit();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private FarPoint.Win.Spread.FpSpread ssMain;
        private FarPoint.Win.Spread.SheetView ssMain_Sheet1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnDeptSearch;
        private System.Windows.Forms.TextBox txtPtInfo;
        private System.Windows.Forms.TextBox txtBuName;
        private System.Windows.Forms.Label lblDateTitle;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnUserSearch;
        private System.Windows.Forms.Label label1;
        private FarPoint.Win.Spread.FpSpread ssUser;
        private FarPoint.Win.Spread.SheetView ssUser_Sheet1;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblTitle;
    }
}