namespace ComEmrBase
{
    partial class frmEmrAnItem
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
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType1 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType2 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ssItem = new FarPoint.Win.Spread.FpSpread();
            this.ssItem_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtWord = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.ssItemList = new FarPoint.Win.Spread.FpSpread();
            this.ssItemList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssItem_Sheet1)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssItemList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssItemList_Sheet1)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnSave);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(543, 34);
            this.panTitle.TabIndex = 14;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.Location = new System.Drawing.Point(389, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 30);
            this.btnSave.TabIndex = 29;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(467, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 28;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(160, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "마취기록지 항목설정";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ssItem);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(251, 658);
            this.panel1.TabIndex = 15;
            // 
            // ssItem
            // 
            this.ssItem.AccessibleDescription = "ssItem, Sheet1, Row 0, Column 0, ";
            this.ssItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssItem.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssItem.Location = new System.Drawing.Point(0, 35);
            this.ssItem.Name = "ssItem";
            this.ssItem.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssItem_Sheet1});
            this.ssItem.Size = new System.Drawing.Size(251, 623);
            this.ssItem.TabIndex = 0;
            this.ssItem.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            // 
            // ssItem_Sheet1
            // 
            this.ssItem_Sheet1.Reset();
            this.ssItem_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssItem_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssItem_Sheet1.ColumnCount = 4;
            this.ssItem_Sheet1.RowCount = 0;
            this.ssItem_Sheet1.ActiveColumnIndex = -1;
            this.ssItem_Sheet1.ActiveRowIndex = -1;
            this.ssItem_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "선택";
            this.ssItem_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "항목명";
            this.ssItem_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "단위";
            this.ssItem_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "코드";
            this.ssItem_Sheet1.Columns.Get(0).CellType = checkBoxCellType1;
            this.ssItem_Sheet1.Columns.Get(0).Label = "선택";
            this.ssItem_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssItem_Sheet1.Columns.Get(0).Width = 20F;
            this.ssItem_Sheet1.Columns.Get(1).CellType = textCellType1;
            this.ssItem_Sheet1.Columns.Get(1).Label = "항목명";
            this.ssItem_Sheet1.Columns.Get(1).Locked = true;
            this.ssItem_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssItem_Sheet1.Columns.Get(1).Width = 120F;
            this.ssItem_Sheet1.Columns.Get(2).CellType = textCellType2;
            this.ssItem_Sheet1.Columns.Get(2).Label = "단위";
            this.ssItem_Sheet1.Columns.Get(2).Locked = true;
            this.ssItem_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssItem_Sheet1.Columns.Get(2).Width = 50F;
            this.ssItem_Sheet1.Columns.Get(3).CellType = textCellType3;
            this.ssItem_Sheet1.Columns.Get(3).Label = "코드";
            this.ssItem_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssItem_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnSearch);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.txtWord);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(251, 35);
            this.panel4.TabIndex = 1;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(173, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 30);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "항목명";
            // 
            // txtWord
            // 
            this.txtWord.Location = new System.Drawing.Point(47, 7);
            this.txtWord.Name = "txtWord";
            this.txtWord.Size = new System.Drawing.Size(120, 21);
            this.txtWord.TabIndex = 0;
            this.txtWord.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtWord_KeyDown);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnDelete);
            this.panel2.Controls.Add(this.btnAdd);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(251, 34);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(49, 658);
            this.panel2.TabIndex = 16;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(0, 162);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(50, 30);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "◀";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(0, 92);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(50, 30);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "▶";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // ssItemList
            // 
            this.ssItemList.AccessibleDescription = "ssItemList, Sheet1, Row 0, Column 0, ";
            this.ssItemList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssItemList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssItemList.Location = new System.Drawing.Point(300, 69);
            this.ssItemList.Name = "ssItemList";
            this.ssItemList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssItemList_Sheet1});
            this.ssItemList.Size = new System.Drawing.Size(243, 623);
            this.ssItemList.TabIndex = 17;
            this.ssItemList.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssItemList.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssItemList_CellClick);
            // 
            // ssItemList_Sheet1
            // 
            this.ssItemList_Sheet1.Reset();
            this.ssItemList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssItemList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssItemList_Sheet1.ColumnCount = 7;
            this.ssItemList_Sheet1.RowCount = 30;
            this.ssItemList_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "선택";
            this.ssItemList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "항목";
            this.ssItemList_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "단위";
            this.ssItemList_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "코드";
            this.ssItemList_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "Row";
            this.ssItemList_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "IsWrite";
            this.ssItemList_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "IsView";
            this.ssItemList_Sheet1.Columns.Get(0).CellType = checkBoxCellType2;
            this.ssItemList_Sheet1.Columns.Get(0).Label = "선택";
            this.ssItemList_Sheet1.Columns.Get(0).Width = 20F;
            this.ssItemList_Sheet1.Columns.Get(1).Label = "항목";
            this.ssItemList_Sheet1.Columns.Get(1).Width = 120F;
            this.ssItemList_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssItemList_Sheet1.Columns.Get(3).Label = "코드";
            this.ssItemList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssItemList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnDown);
            this.panel3.Controls.Add(this.btnUp);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(300, 34);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(243, 35);
            this.panel3.TabIndex = 18;
            // 
            // btnDown
            // 
            this.btnDown.BackColor = System.Drawing.Color.Transparent;
            this.btnDown.Location = new System.Drawing.Point(81, 3);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(32, 30);
            this.btnDown.TabIndex = 31;
            this.btnDown.Text = "▼";
            this.btnDown.UseVisualStyleBackColor = false;
            this.btnDown.Click += new System.EventHandler(this.BtnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.BackColor = System.Drawing.Color.Transparent;
            this.btnUp.Location = new System.Drawing.Point(43, 3);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(32, 30);
            this.btnUp.TabIndex = 30;
            this.btnUp.Text = "▲";
            this.btnUp.UseVisualStyleBackColor = false;
            this.btnUp.Click += new System.EventHandler(this.BtnUp_Click);
            // 
            // frmEmrAnItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 692);
            this.Controls.Add(this.ssItemList);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Name = "frmEmrAnItem";
            this.Text = "frmEmrAnItem";
            this.Load += new System.EventHandler(this.FrmEmrAnItem_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssItem_Sheet1)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssItemList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssItemList_Sheet1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private FarPoint.Win.Spread.FpSpread ssItem;
        private FarPoint.Win.Spread.SheetView ssItem_Sheet1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private FarPoint.Win.Spread.SheetView ssItemList_Sheet1;
        private System.Windows.Forms.Button btnSave;
        public FarPoint.Win.Spread.FpSpread ssItemList;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtWord;
    }
}