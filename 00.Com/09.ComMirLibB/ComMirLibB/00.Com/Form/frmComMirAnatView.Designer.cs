namespace ComMirLibB.Com
{
    partial class frmComMirAnatView
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color385636501385449532561", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text465636501385449532561", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static580636501385449532561");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Static652636501385449532561");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panOpt = new System.Windows.Forms.Panel();
            this.grbYYMM = new System.Windows.Forms.GroupBox();
            this.dtpEDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpSDate = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblBi = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.grbIO = new System.Windows.Forms.GroupBox();
            this.lblName = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtPano = new System.Windows.Forms.TextBox();
            this.collapsibleSplitContainer1 = new DevComponents.DotNetBar.Controls.CollapsibleSplitContainer();
            this.ssMain = new FarPoint.Win.Spread.FpSpread();
            this.ssMain_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.panTitle.SuspendLayout();
            this.panOpt.SuspendLayout();
            this.grbYYMM.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grbIO.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.collapsibleSplitContainer1)).BeginInit();
            this.collapsibleSplitContainer1.Panel1.SuspendLayout();
            this.collapsibleSplitContainer1.Panel2.SuspendLayout();
            this.collapsibleSplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMain_Sheet1)).BeginInit();
            this.panTitleSub0.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Padding = new System.Windows.Forms.Padding(1);
            this.panTitle.Size = new System.Drawing.Size(1073, 34);
            this.panTitle.TabIndex = 96;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(6, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(106, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "임상병리조회";
            // 
            // panOpt
            // 
            this.panOpt.BackColor = System.Drawing.Color.White;
            this.panOpt.Controls.Add(this.btnExit);
            this.panOpt.Controls.Add(this.grbYYMM);
            this.panOpt.Controls.Add(this.groupBox1);
            this.panOpt.Controls.Add(this.btnSearch);
            this.panOpt.Controls.Add(this.grbIO);
            this.panOpt.Controls.Add(this.groupBox3);
            this.panOpt.Dock = System.Windows.Forms.DockStyle.Top;
            this.panOpt.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.panOpt.Location = new System.Drawing.Point(0, 34);
            this.panOpt.Name = "panOpt";
            this.panOpt.Padding = new System.Windows.Forms.Padding(1);
            this.panOpt.Size = new System.Drawing.Size(1073, 74);
            this.panOpt.TabIndex = 97;
            // 
            // grbYYMM
            // 
            this.grbYYMM.Controls.Add(this.dtpEDate);
            this.grbYYMM.Controls.Add(this.label2);
            this.grbYYMM.Controls.Add(this.dtpSDate);
            this.grbYYMM.Location = new System.Drawing.Point(362, 8);
            this.grbYYMM.Name = "grbYYMM";
            this.grbYYMM.Size = new System.Drawing.Size(269, 55);
            this.grbYYMM.TabIndex = 21;
            this.grbYYMM.TabStop = false;
            // 
            // dtpEDate
            // 
            this.dtpEDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEDate.Location = new System.Drawing.Point(146, 20);
            this.dtpEDate.Name = "dtpEDate";
            this.dtpEDate.Size = new System.Drawing.Size(108, 25);
            this.dtpEDate.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(121, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 26);
            this.label2.TabIndex = 2;
            this.label2.Text = "~";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dtpSDate
            // 
            this.dtpSDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpSDate.Location = new System.Drawing.Point(13, 20);
            this.dtpSDate.Name = "dtpSDate";
            this.dtpSDate.Size = new System.Drawing.Size(108, 25);
            this.dtpSDate.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblBi);
            this.groupBox1.Location = new System.Drawing.Point(273, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(81, 55);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "종류";
            // 
            // lblBi
            // 
            this.lblBi.Location = new System.Drawing.Point(3, 21);
            this.lblBi.Name = "lblBi";
            this.lblBi.Size = new System.Drawing.Size(75, 26);
            this.lblBi.TabIndex = 1;
            this.lblBi.Text = "lblBi";
            this.lblBi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(895, 21);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(71, 32);
            this.btnSearch.TabIndex = 17;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // grbIO
            // 
            this.grbIO.Controls.Add(this.lblName);
            this.grbIO.Location = new System.Drawing.Point(152, 8);
            this.grbIO.Name = "grbIO";
            this.grbIO.Size = new System.Drawing.Size(114, 55);
            this.grbIO.TabIndex = 20;
            this.grbIO.TabStop = false;
            this.grbIO.Text = "성명";
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(3, 21);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(108, 26);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "lblName";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtPano);
            this.groupBox3.Location = new System.Drawing.Point(11, 8);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(135, 55);
            this.groupBox3.TabIndex = 23;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "등록번호";
            // 
            // txtPano
            // 
            this.txtPano.Location = new System.Drawing.Point(6, 21);
            this.txtPano.Name = "txtPano";
            this.txtPano.Size = new System.Drawing.Size(123, 25);
            this.txtPano.TabIndex = 0;
            // 
            // collapsibleSplitContainer1
            // 
            this.collapsibleSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.collapsibleSplitContainer1.Location = new System.Drawing.Point(0, 108);
            this.collapsibleSplitContainer1.Name = "collapsibleSplitContainer1";
            // 
            // collapsibleSplitContainer1.Panel1
            // 
            this.collapsibleSplitContainer1.Panel1.Controls.Add(this.ssMain);
            this.collapsibleSplitContainer1.Panel1.Controls.Add(this.panTitleSub0);
            // 
            // collapsibleSplitContainer1.Panel2
            // 
            this.collapsibleSplitContainer1.Panel2.Controls.Add(this.txtResult);
            this.collapsibleSplitContainer1.Panel2.Controls.Add(this.panel1);
            this.collapsibleSplitContainer1.Size = new System.Drawing.Size(1073, 589);
            this.collapsibleSplitContainer1.SplitterDistance = 362;
            this.collapsibleSplitContainer1.SplitterWidth = 20;
            this.collapsibleSplitContainer1.TabIndex = 98;
            // 
            // ssMain
            // 
            this.ssMain.AccessibleDescription = "";
            this.ssMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssMain.Location = new System.Drawing.Point(0, 28);
            this.ssMain.Name = "ssMain";
            namedStyle1.Font = new System.Drawing.Font("굴림체", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType1.MaxLength = 60;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("굴림체", 9F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType2.Static = true;
            namedStyle3.CellType = textCellType2;
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.Renderer = textCellType2;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType3.Static = true;
            namedStyle4.CellType = textCellType3;
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.Renderer = textCellType3;
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMain.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4});
            this.ssMain.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssMain_Sheet1});
            this.ssMain.Size = new System.Drawing.Size(362, 561);
            this.ssMain.TabIndex = 98;
            this.ssMain.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssMain.TextTipAppearance = tipAppearance1;
            // 
            // ssMain_Sheet1
            // 
            this.ssMain_Sheet1.Reset();
            this.ssMain_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssMain_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssMain_Sheet1.ColumnCount = 8;
            this.ssMain_Sheet1.RowCount = 1;
            this.ssMain_Sheet1.ColumnFooter.Columns.Default.Resizable = true;
            this.ssMain_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "일자";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "검사명";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "P";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "구분";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "PacsUID";
            this.ssMain_Sheet1.ColumnHeader.Columns.Default.Resizable = true;
            this.ssMain_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssMain_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssMain_Sheet1.Columns.Default.Resizable = true;
            this.ssMain_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssMain_Sheet1.Columns.Get(0).Label = "일자";
            this.ssMain_Sheet1.Columns.Get(0).StyleName = "Static580636501385449532561";
            this.ssMain_Sheet1.Columns.Get(0).Width = 78F;
            this.ssMain_Sheet1.Columns.Get(1).Label = "검사명";
            this.ssMain_Sheet1.Columns.Get(1).StyleName = "Static580636501385449532561";
            this.ssMain_Sheet1.Columns.Get(1).Width = 260F;
            this.ssMain_Sheet1.Columns.Get(2).Label = "P";
            this.ssMain_Sheet1.Columns.Get(2).StyleName = "Static652636501385449532561";
            this.ssMain_Sheet1.Columns.Get(2).Width = 29F;
            this.ssMain_Sheet1.Columns.Get(3).Label = "구분";
            this.ssMain_Sheet1.Columns.Get(3).StyleName = "Static652636501385449532561";
            this.ssMain_Sheet1.Columns.Get(3).Width = 58F;
            this.ssMain_Sheet1.Columns.Get(4).StyleName = "Static580636501385449532561";
            this.ssMain_Sheet1.Columns.Get(4).Width = 40F;
            this.ssMain_Sheet1.Columns.Get(5).StyleName = "Static580636501385449532561";
            this.ssMain_Sheet1.Columns.Get(5).Width = 39F;
            this.ssMain_Sheet1.Columns.Get(6).StyleName = "Static580636501385449532561";
            this.ssMain_Sheet1.Columns.Get(6).Width = 51F;
            this.ssMain_Sheet1.Columns.Get(7).Label = "PacsUID";
            this.ssMain_Sheet1.Columns.Get(7).StyleName = "Static652636501385449532561";
            this.ssMain_Sheet1.Columns.Get(7).Width = 63F;
            this.ssMain_Sheet1.DefaultStyleName = "Text465636501385449532561";
            this.ssMain_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssMain_Sheet1.RowHeader.Rows.Default.Resizable = false;
            this.ssMain_Sheet1.RowHeader.Visible = false;
            this.ssMain_Sheet1.Rows.Default.Resizable = false;
            this.ssMain_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 0);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(362, 28);
            this.panTitleSub0.TabIndex = 97;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(4, 2);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(37, 19);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "검사";
            // 
            // txtResult
            // 
            this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResult.Location = new System.Drawing.Point(0, 28);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(691, 561);
            this.txtResult.TabIndex = 98;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(691, 28);
            this.panel1.TabIndex = 97;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(4, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "결과";
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnExit.Location = new System.Drawing.Point(980, 20);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 32);
            this.btnExit.TabIndex = 25;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // frmComMirAnatView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1073, 697);
            this.Controls.Add(this.collapsibleSplitContainer1);
            this.Controls.Add(this.panOpt);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmComMirAnatView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmComMirAnatView";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panOpt.ResumeLayout(false);
            this.grbYYMM.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.grbIO.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.collapsibleSplitContainer1.Panel1.ResumeLayout(false);
            this.collapsibleSplitContainer1.Panel2.ResumeLayout(false);
            this.collapsibleSplitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.collapsibleSplitContainer1)).EndInit();
            this.collapsibleSplitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMain_Sheet1)).EndInit();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panOpt;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox grbIO;
        private System.Windows.Forms.GroupBox groupBox3;
        private DevComponents.DotNetBar.Controls.CollapsibleSplitContainer collapsibleSplitContainer1;
        private System.Windows.Forms.GroupBox grbYYMM;
        private System.Windows.Forms.DateTimePicker dtpEDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpSDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblBi;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtPano;
        private FarPoint.Win.Spread.FpSpread ssMain;
        private FarPoint.Win.Spread.SheetView ssMain_Sheet1;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnExit;
    }
}