namespace ComLibB
{
    partial class frmDocuBuse
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
            this.panel5 = new System.Windows.Forms.Panel();
            this.LblBUSE = new System.Windows.Forms.Label();
            this.TxtBuse = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.BtnSave = new System.Windows.Forms.Button();
            this.BtnDelete = new System.Windows.Forms.Button();
            this.panTitle = new System.Windows.Forms.Panel();
            this.BtnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SsView = new FarPoint.Win.Spread.FpSpread();
            this.SsView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SsView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SsView_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.LblBUSE);
            this.panel5.Controls.Add(this.TxtBuse);
            this.panel5.Controls.Add(this.label27);
            this.panel5.Controls.Add(this.panel3);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 34);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(479, 32);
            this.panel5.TabIndex = 38;
            // 
            // LblBUSE
            // 
            this.LblBUSE.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LblBUSE.Location = new System.Drawing.Point(197, 6);
            this.LblBUSE.Name = "LblBUSE";
            this.LblBUSE.Size = new System.Drawing.Size(104, 21);
            this.LblBUSE.TabIndex = 32;
            this.LblBUSE.Text = "label1";
            this.LblBUSE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TxtBuse
            // 
            this.TxtBuse.Location = new System.Drawing.Point(83, 6);
            this.TxtBuse.Name = "TxtBuse";
            this.TxtBuse.Size = new System.Drawing.Size(113, 21);
            this.TxtBuse.TabIndex = 31;
            this.TxtBuse.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtBuse_KeyDown);
            // 
            // label27
            // 
            this.label27.BackColor = System.Drawing.Color.LightGray;
            this.label27.Location = new System.Drawing.Point(7, 6);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(75, 21);
            this.label27.TabIndex = 27;
            this.label27.Tag = "";
            this.label27.Text = "부서명";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.AutoSize = true;
            this.panel3.Controls.Add(this.BtnSave);
            this.panel3.Controls.Add(this.BtnDelete);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(319, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(1);
            this.panel3.Size = new System.Drawing.Size(160, 32);
            this.panel3.TabIndex = 17;
            // 
            // BtnSave
            // 
            this.BtnSave.AutoSize = true;
            this.BtnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BtnSave.Location = new System.Drawing.Point(1, 1);
            this.BtnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(79, 30);
            this.BtnSave.TabIndex = 17;
            this.BtnSave.Text = "저  장";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnDelete
            // 
            this.BtnDelete.AutoSize = true;
            this.BtnDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnDelete.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BtnDelete.Location = new System.Drawing.Point(80, 1);
            this.BtnDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(79, 30);
            this.BtnDelete.TabIndex = 16;
            this.BtnDelete.Text = "삭  제";
            this.BtnDelete.UseVisualStyleBackColor = true;
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.BtnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(479, 34);
            this.panTitle.TabIndex = 37;
            // 
            // BtnExit
            // 
            this.BtnExit.AutoSize = true;
            this.BtnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BtnExit.Location = new System.Drawing.Point(396, 0);
            this.BtnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(79, 30);
            this.BtnExit.TabIndex = 18;
            this.BtnExit.Text = "닫기";
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(188, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "문서관리 부서 등록 화면";
            // 
            // SsView
            // 
            this.SsView.AccessibleDescription = "fpSpread1, Sheet1, Row 0, Column 0, ";
            this.SsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SsView.Location = new System.Drawing.Point(0, 66);
            this.SsView.Name = "SsView";
            this.SsView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SsView_Sheet1});
            this.SsView.Size = new System.Drawing.Size(479, 625);
            this.SsView.TabIndex = 39;
            // 
            // SsView_Sheet1
            // 
            this.SsView_Sheet1.Reset();
            this.SsView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SsView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SsView_Sheet1.ColumnCount = 5;
            this.SsView_Sheet1.RowCount = 1;
            this.SsView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = " ";
            this.SsView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "부서코드";
            this.SsView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "부서명";
            this.SsView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "삭제일자";
            this.SsView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "정렬순서";
            this.SsView_Sheet1.ColumnHeader.Rows.Get(0).Height = 39F;
            this.SsView_Sheet1.Columns.Get(0).CellType = checkBoxCellType1;
            this.SsView_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SsView_Sheet1.Columns.Get(0).Label = " ";
            this.SsView_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SsView_Sheet1.Columns.Get(0).Width = 19F;
            this.SsView_Sheet1.Columns.Get(1).CellType = textCellType1;
            this.SsView_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SsView_Sheet1.Columns.Get(1).Label = "부서코드";
            this.SsView_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SsView_Sheet1.Columns.Get(1).Width = 67F;
            this.SsView_Sheet1.Columns.Get(2).CellType = textCellType2;
            this.SsView_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SsView_Sheet1.Columns.Get(2).Label = "부서명";
            this.SsView_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SsView_Sheet1.Columns.Get(2).Width = 179F;
            this.SsView_Sheet1.Columns.Get(3).CellType = textCellType3;
            this.SsView_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SsView_Sheet1.Columns.Get(3).Label = "삭제일자";
            this.SsView_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SsView_Sheet1.Columns.Get(3).Width = 93F;
            this.SsView_Sheet1.Columns.Get(4).CellType = textCellType4;
            this.SsView_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SsView_Sheet1.Columns.Get(4).Label = "정렬순서";
            this.SsView_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SsView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SsView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmDocuBuse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(479, 691);
            this.Controls.Add(this.SsView);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panTitle);
            this.Name = "frmDocuBuse";
            this.Text = "frmDocuBuse";
            this.Activated += new System.EventHandler(this.frmDocuBuse_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDocuBuse_FormClosed);
            this.Load += new System.EventHandler(this.frmDocuBuse_Load);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SsView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SsView_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TextBox TxtBuse;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Button BtnDelete;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label LblBUSE;
        private System.Windows.Forms.Button BtnExit;
        private FarPoint.Win.Spread.FpSpread SsView;
        private FarPoint.Win.Spread.SheetView SsView_Sheet1;
    }
}