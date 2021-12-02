namespace ComEmrBase
{
    partial class frmEmrSugaLink
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.ComboBoxCellType comboBoxCellType1 = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTop = new System.Windows.Forms.Panel();
            this.ssItem = new FarPoint.Win.Spread.FpSpread();
            this.ssItem_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rdoAll = new System.Windows.Forms.RadioButton();
            this.rdoSave = new System.Windows.Forms.RadioButton();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ssForm = new FarPoint.Win.Spread.FpSpread();
            this.ssForm_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssItem_Sheet1)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssForm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssForm_Sheet1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTop
            // 
            this.panTop.Controls.Add(this.ssItem);
            this.panTop.Controls.Add(this.panel3);
            this.panTop.Dock = System.Windows.Forms.DockStyle.Left;
            this.panTop.Location = new System.Drawing.Point(217, 0);
            this.panTop.Name = "panTop";
            this.panTop.Size = new System.Drawing.Size(822, 691);
            this.panTop.TabIndex = 0;
            // 
            // ssItem
            // 
            this.ssItem.AccessibleDescription = "ssItem, Sheet1, Row 0, Column 0, ";
            this.ssItem.Dock = System.Windows.Forms.DockStyle.Left;
            this.ssItem.Location = new System.Drawing.Point(0, 38);
            this.ssItem.Name = "ssItem";
            this.ssItem.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssItem_Sheet1});
            this.ssItem.Size = new System.Drawing.Size(425, 653);
            this.ssItem.TabIndex = 5;
            this.ssItem.Change += new FarPoint.Win.Spread.ChangeEventHandler(this.ssItem_Change);
            // 
            // ssItem_Sheet1
            // 
            this.ssItem_Sheet1.Reset();
            this.ssItem_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssItem_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssItem_Sheet1.ColumnCount = 6;
            this.ssItem_Sheet1.RowCount = 1;
            this.ssItem_Sheet1.Cells.Get(0, 5).Value = "002.행위";
            this.ssItem_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssItem_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssItem_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssItem_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssItem_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = " ";
            this.ssItem_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "대분류";
            this.ssItem_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "분류";
            this.ssItem_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "아이템 코드";
            this.ssItem_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "아이템 이름";
            this.ssItem_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "오더구분";
            this.ssItem_Sheet1.ColumnHeader.Rows.Get(0).Height = 38F;
            this.ssItem_Sheet1.Columns.Get(0).CellType = checkBoxCellType1;
            this.ssItem_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssItem_Sheet1.Columns.Get(0).Label = " ";
            this.ssItem_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssItem_Sheet1.Columns.Get(0).Width = 19F;
            this.ssItem_Sheet1.Columns.Get(1).CellType = textCellType1;
            this.ssItem_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssItem_Sheet1.Columns.Get(1).Label = "대분류";
            this.ssItem_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssItem_Sheet1.Columns.Get(1).Visible = false;
            this.ssItem_Sheet1.Columns.Get(1).Width = 80F;
            textCellType2.MaxLength = 3000;
            textCellType2.Multiline = true;
            textCellType2.WordWrap = true;
            this.ssItem_Sheet1.Columns.Get(2).CellType = textCellType2;
            this.ssItem_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssItem_Sheet1.Columns.Get(2).Label = "분류";
            this.ssItem_Sheet1.Columns.Get(2).Locked = true;
            this.ssItem_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssItem_Sheet1.Columns.Get(2).Width = 80F;
            this.ssItem_Sheet1.Columns.Get(3).CellType = textCellType3;
            this.ssItem_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssItem_Sheet1.Columns.Get(3).Label = "아이템 코드";
            this.ssItem_Sheet1.Columns.Get(3).Locked = true;
            this.ssItem_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssItem_Sheet1.Columns.Get(3).Visible = false;
            this.ssItem_Sheet1.Columns.Get(3).Width = 100F;
            textCellType4.MaxLength = 2550;
            textCellType4.Multiline = true;
            textCellType4.WordWrap = true;
            this.ssItem_Sheet1.Columns.Get(4).CellType = textCellType4;
            this.ssItem_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssItem_Sheet1.Columns.Get(4).Label = "아이템 이름";
            this.ssItem_Sheet1.Columns.Get(4).Locked = true;
            this.ssItem_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssItem_Sheet1.Columns.Get(4).Width = 200F;
            comboBoxCellType1.AllowEditorVerticalAlign = true;
            comboBoxCellType1.ButtonAlign = FarPoint.Win.ButtonAlign.Right;
            comboBoxCellType1.Editable = true;
            comboBoxCellType1.Items = new string[] {
        "000.일당",
        "001.행위"};
            this.ssItem_Sheet1.Columns.Get(5).CellType = comboBoxCellType1;
            this.ssItem_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssItem_Sheet1.Columns.Get(5).Label = "오더구분";
            this.ssItem_Sheet1.Columns.Get(5).Locked = false;
            this.ssItem_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssItem_Sheet1.Columns.Get(5).Width = 69F;
            this.ssItem_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssItem_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssItem_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssItem_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssItem_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssItem_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.rdoAll);
            this.panel3.Controls.Add(this.rdoSave);
            this.panel3.Controls.Add(this.btnDelete);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(822, 38);
            this.panel3.TabIndex = 4;
            // 
            // rdoAll
            // 
            this.rdoAll.AutoSize = true;
            this.rdoAll.Location = new System.Drawing.Point(279, 11);
            this.rdoAll.Name = "rdoAll";
            this.rdoAll.Size = new System.Drawing.Size(99, 16);
            this.rdoAll.TabIndex = 8;
            this.rdoAll.TabStop = true;
            this.rdoAll.Text = "전체항목 보기";
            this.rdoAll.UseVisualStyleBackColor = true;
            this.rdoAll.CheckedChanged += new System.EventHandler(this.rdoSave_CheckedChanged);
            // 
            // rdoSave
            // 
            this.rdoSave.AutoSize = true;
            this.rdoSave.Location = new System.Drawing.Point(148, 11);
            this.rdoSave.Name = "rdoSave";
            this.rdoSave.Size = new System.Drawing.Size(115, 16);
            this.rdoSave.TabIndex = 8;
            this.rdoSave.TabStop = true;
            this.rdoSave.Text = "저장한 항목 보기";
            this.rdoSave.UseVisualStyleBackColor = true;
            this.rdoSave.CheckedChanged += new System.EventHandler(this.rdoSave_CheckedChanged);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(675, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(72, 32);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "삭제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(747, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 32);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(603, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 32);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label1.Location = new System.Drawing.Point(6, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "아이템 리스트";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ssForm);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(217, 691);
            this.panel1.TabIndex = 1;
            // 
            // ssForm
            // 
            this.ssForm.AccessibleDescription = "ssForm, Sheet1, Row 0, Column 0, ";
            this.ssForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssForm.Location = new System.Drawing.Point(0, 38);
            this.ssForm.Name = "ssForm";
            this.ssForm.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssForm_Sheet1});
            this.ssForm.Size = new System.Drawing.Size(217, 653);
            this.ssForm.TabIndex = 5;
            this.ssForm.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssForm_CellClick);
            // 
            // ssForm_Sheet1
            // 
            this.ssForm_Sheet1.Reset();
            this.ssForm_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssForm_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssForm_Sheet1.ColumnCount = 2;
            this.ssForm_Sheet1.RowCount = 1;
            this.ssForm_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "번호";
            this.ssForm_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "명칭";
            this.ssForm_Sheet1.ColumnHeader.Rows.Get(0).Height = 27F;
            this.ssForm_Sheet1.Columns.Get(0).CellType = textCellType5;
            this.ssForm_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssForm_Sheet1.Columns.Get(0).Label = "번호";
            this.ssForm_Sheet1.Columns.Get(0).Locked = true;
            this.ssForm_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssForm_Sheet1.Columns.Get(0).Visible = false;
            this.ssForm_Sheet1.Columns.Get(0).Width = 71F;
            textCellType6.MaxLength = 2550;
            textCellType6.Multiline = true;
            textCellType6.WordWrap = true;
            this.ssForm_Sheet1.Columns.Get(1).CellType = textCellType6;
            this.ssForm_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssForm_Sheet1.Columns.Get(1).Label = "명칭";
            this.ssForm_Sheet1.Columns.Get(1).Locked = true;
            this.ssForm_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssForm_Sheet1.Columns.Get(1).Width = 160F;
            this.ssForm_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssForm_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.label3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(217, 38);
            this.panel2.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(217, 38);
            this.label3.TabIndex = 3;
            this.label3.Text = "기록지 목록";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmEmrSugaLink
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1042, 691);
            this.Controls.Add(this.panTop);
            this.Controls.Add(this.panel1);
            this.Name = "frmEmrSugaLink";
            this.Text = "frmEmrSugaLink";
            this.Load += new System.EventHandler(this.frmEmrSugaLink_Load);
            this.panTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssItem_Sheet1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssForm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssForm_Sheet1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTop;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private FarPoint.Win.Spread.FpSpread ssForm;
        private FarPoint.Win.Spread.SheetView ssForm_Sheet1;
        private FarPoint.Win.Spread.FpSpread ssItem;
        private FarPoint.Win.Spread.SheetView ssItem_Sheet1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.RadioButton rdoAll;
        private System.Windows.Forms.RadioButton rdoSave;
    }
}