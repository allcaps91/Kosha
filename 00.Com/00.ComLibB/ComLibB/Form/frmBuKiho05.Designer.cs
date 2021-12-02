namespace ComLibB
{
    partial class frmBuKiho05
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose ();
            }
            base.Dispose (disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.pan0 = new System.Windows.Forms.Panel();
            this.pan1 = new System.Windows.Forms.Panel();
            this.pan4 = new System.Windows.Forms.Panel();
            this.lblMenu = new System.Windows.Forms.Label();
            this.pan3 = new System.Windows.Forms.Panel();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.pan2 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.txtdata = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblItem0 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pan0.SuspendLayout();
            this.pan1.SuspendLayout();
            this.pan4.SuspendLayout();
            this.pan3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.pan2.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // pan0
            // 
            this.pan0.Controls.Add(this.pan1);
            this.pan0.Controls.Add(this.panTitle);
            this.pan0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pan0.Location = new System.Drawing.Point(0, 0);
            this.pan0.Name = "pan0";
            this.pan0.Size = new System.Drawing.Size(666, 568);
            this.pan0.TabIndex = 1;
            // 
            // pan1
            // 
            this.pan1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pan1.Controls.Add(this.pan4);
            this.pan1.Controls.Add(this.pan3);
            this.pan1.Controls.Add(this.pan2);
            this.pan1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pan1.Location = new System.Drawing.Point(0, 34);
            this.pan1.Name = "pan1";
            this.pan1.Size = new System.Drawing.Size(666, 534);
            this.pan1.TabIndex = 13;
            // 
            // pan4
            // 
            this.pan4.BackColor = System.Drawing.Color.White;
            this.pan4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pan4.Controls.Add(this.lblMenu);
            this.pan4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pan4.Location = new System.Drawing.Point(0, 476);
            this.pan4.Name = "pan4";
            this.pan4.Size = new System.Drawing.Size(662, 54);
            this.pan4.TabIndex = 20;
            // 
            // lblMenu
            // 
            this.lblMenu.AutoSize = true;
            this.lblMenu.Location = new System.Drawing.Point(97, 5);
            this.lblMenu.Name = "lblMenu";
            this.lblMenu.Size = new System.Drawing.Size(87, 12);
            this.lblMenu.TabIndex = 25;
            this.lblMenu.Text = "찾으실 명칭은?";
            // 
            // pan3
            // 
            this.pan3.BackColor = System.Drawing.Color.White;
            this.pan3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pan3.Controls.Add(this.ssView);
            this.pan3.Dock = System.Windows.Forms.DockStyle.Top;
            this.pan3.Location = new System.Drawing.Point(0, 36);
            this.pan3.Name = "pan3";
            this.pan3.Size = new System.Drawing.Size(662, 440);
            this.pan3.TabIndex = 19;
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.Location = new System.Drawing.Point(0, 0);
            this.ssView.Name = "ssView";
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(658, 436);
            this.ssView.TabIndex = 46;
            this.ssView.LeaveCell += new FarPoint.Win.Spread.LeaveCellEventHandler(this.ssView_LeaveCell);
            this.ssView.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssView_CellDoubleClick);
            this.ssView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ssView_KeyDown);
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 2;
            this.ssView_Sheet1.RowCount = 1;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "코드";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "코드명칭";
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssView_Sheet1.Columns.Get(0).CellType = textCellType7;
            this.ssView_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(0).Label = "코드";
            this.ssView_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(1).CellType = textCellType8;
            this.ssView_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(1).Label = "코드명칭";
            this.ssView_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(1).Width = 542F;
            this.ssView_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssView_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // pan2
            // 
            this.pan2.BackColor = System.Drawing.Color.White;
            this.pan2.Controls.Add(this.btnExit);
            this.pan2.Controls.Add(this.txtdata);
            this.pan2.Controls.Add(this.btnSearch);
            this.pan2.Controls.Add(this.lblItem0);
            this.pan2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pan2.Location = new System.Drawing.Point(0, 0);
            this.pan2.Name = "pan2";
            this.pan2.Size = new System.Drawing.Size(662, 36);
            this.pan2.TabIndex = 17;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(587, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // txtdata
            // 
            this.txtdata.Location = new System.Drawing.Point(101, 8);
            this.txtdata.Name = "txtdata";
            this.txtdata.Size = new System.Drawing.Size(100, 21);
            this.txtdata.TabIndex = 31;
            this.txtdata.EnabledChanged += new System.EventHandler(this.txtdata_EnabledChanged);
            this.txtdata.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtdata_KeyDown);
            this.txtdata.Leave += new System.EventHandler(this.txtdata_Leave);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Location = new System.Drawing.Point(515, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 27;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblItem0
            // 
            this.lblItem0.AutoSize = true;
            this.lblItem0.Location = new System.Drawing.Point(8, 11);
            this.lblItem0.Name = "lblItem0";
            this.lblItem0.Size = new System.Drawing.Size(87, 12);
            this.lblItem0.TabIndex = 24;
            this.lblItem0.Text = "찾으실 명칭은?";
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(666, 34);
            this.panTitle.TabIndex = 11;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(82, 16);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "코드 찾기";
            // 
            // frmBuKiho05
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 568);
            this.Controls.Add(this.pan0);
            this.Name = "frmBuKiho05";
            this.Text = "코드 찾기";
            this.Load += new System.EventHandler(this.frmBuKiho05_Load);
            this.pan0.ResumeLayout(false);
            this.pan1.ResumeLayout(false);
            this.pan4.ResumeLayout(false);
            this.pan4.PerformLayout();
            this.pan3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.pan2.ResumeLayout(false);
            this.pan2.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pan0;
        private System.Windows.Forms.Panel pan1;
        private System.Windows.Forms.Panel pan3;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
        private System.Windows.Forms.Panel pan2;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox txtdata;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblItem0;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pan4;
        private System.Windows.Forms.Label lblMenu;
    }
}