namespace ComSupLibB
{
    partial class frmInfectionSabun
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
            FarPoint.Win.Spread.CellType.SliderCellType sliderCellType1 = new FarPoint.Win.Spread.CellType.SliderCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ssUser = new FarPoint.Win.Spread.FpSpread();
            this.ssUser_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblTitleSub2 = new System.Windows.Forms.Label();
            this.txtPtInfo = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssUser_Sheet1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(341, 34);
            this.panTitle.TabIndex = 12;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(74, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "사번검색";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ssUser);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(341, 448);
            this.panel1.TabIndex = 13;
            // 
            // ssUser
            // 
            this.ssUser.AccessibleDescription = "ssUser, Sheet1, Row 0, Column 0, ";
            this.ssUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssUser.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssUser.Location = new System.Drawing.Point(0, 34);
            this.ssUser.Name = "ssUser";
            this.ssUser.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssUser_Sheet1});
            this.ssUser.Size = new System.Drawing.Size(341, 414);
            this.ssUser.TabIndex = 15;
            this.ssUser.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssUser_CellDoubleClick);
            // 
            // ssUser_Sheet1
            // 
            this.ssUser_Sheet1.Reset();
            this.ssUser_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssUser_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssUser_Sheet1.ColumnCount = 6;
            this.ssUser_Sheet1.RowCount = 1;
            this.ssUser_Sheet1.Cells.Get(0, 2).Value = "999999";
            this.ssUser_Sheet1.Cells.Get(0, 3).Value = "홍홍홍홍";
            this.ssUser_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssUser_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssUser_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssUser_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssUser_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssUser_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssUser_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssUser_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssUser_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssUser_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssUser_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssUser_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssUser_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "BUSE";
            this.ssUser_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "부서";
            this.ssUser_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "ID";
            this.ssUser_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "성명";
            this.ssUser_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "JOBGROUP";
            this.ssUser_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "BASNAME";
            this.ssUser_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssUser_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssUser_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssUser_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssUser_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssUser_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssUser_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssUser_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssUser_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssUser_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssUser_Sheet1.Columns.Get(0).Label = "BUSE";
            this.ssUser_Sheet1.Columns.Get(0).Locked = true;
            this.ssUser_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssUser_Sheet1.Columns.Get(0).Visible = false;
            this.ssUser_Sheet1.Columns.Get(0).Width = 115F;
            this.ssUser_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssUser_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssUser_Sheet1.Columns.Get(1).Label = "부서";
            this.ssUser_Sheet1.Columns.Get(1).Locked = true;
            this.ssUser_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssUser_Sheet1.Columns.Get(1).Width = 166F;
            this.ssUser_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssUser_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssUser_Sheet1.Columns.Get(2).Label = "ID";
            this.ssUser_Sheet1.Columns.Get(2).Locked = true;
            this.ssUser_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssUser_Sheet1.Columns.Get(2).Width = 52F;
            this.ssUser_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssUser_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssUser_Sheet1.Columns.Get(3).Label = "성명";
            this.ssUser_Sheet1.Columns.Get(3).Locked = true;
            this.ssUser_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssUser_Sheet1.Columns.Get(3).Width = 71F;
            this.ssUser_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.ssUser_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssUser_Sheet1.Columns.Get(4).Label = "JOBGROUP";
            this.ssUser_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssUser_Sheet1.Columns.Get(4).Visible = false;
            this.ssUser_Sheet1.Columns.Get(4).Width = 72F;
            sliderCellType1.TickColor = System.Drawing.Color.Black;
            this.ssUser_Sheet1.Columns.Get(5).CellType = sliderCellType1;
            this.ssUser_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssUser_Sheet1.Columns.Get(5).Label = "BASNAME";
            this.ssUser_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssUser_Sheet1.Columns.Get(5).Visible = false;
            this.ssUser_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssUser_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssUser_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.ssUser_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssUser_Sheet1.FilterBar.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssUser_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssUser_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssUser_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssUser_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssUser_Sheet1.FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssUser_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssUser_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssUser_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssUser_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssUser_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssUser_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssUser_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssUser_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssUser_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssUser_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssUser_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssUser_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssUser_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssUser_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblTitleSub2);
            this.panel2.Controls.Add(this.txtPtInfo);
            this.panel2.Controls.Add(this.btnSearch);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(341, 34);
            this.panel2.TabIndex = 14;
            // 
            // lblTitleSub2
            // 
            this.lblTitleSub2.AutoSize = true;
            this.lblTitleSub2.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub2.ForeColor = System.Drawing.Color.Black;
            this.lblTitleSub2.Location = new System.Drawing.Point(12, 10);
            this.lblTitleSub2.Name = "lblTitleSub2";
            this.lblTitleSub2.Size = new System.Drawing.Size(49, 15);
            this.lblTitleSub2.TabIndex = 102;
            this.lblTitleSub2.Text = "ID/성명";
            // 
            // txtPtInfo
            // 
            this.txtPtInfo.Location = new System.Drawing.Point(67, 5);
            this.txtPtInfo.Name = "txtPtInfo";
            this.txtPtInfo.Size = new System.Drawing.Size(100, 25);
            this.txtPtInfo.TabIndex = 32;
            this.txtPtInfo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPtInfo_KeyDown);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Location = new System.Drawing.Point(266, 2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 28;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // frmInfectionSabun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(341, 482);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmInfectionSabun";
            this.Text = "frmInfectionSabun";
            this.Load += new System.EventHandler(this.frmInfectionSabun_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssUser_Sheet1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtPtInfo;
        private System.Windows.Forms.Label lblTitleSub2;
        private FarPoint.Win.Spread.FpSpread ssUser;
        private FarPoint.Win.Spread.SheetView ssUser_Sheet1;
    }
}