namespace ComSupLibB.Com
{
    partial class frmComSupSTS01
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
            FarPoint.Win.Spread.EnhancedFocusIndicatorRenderer enhancedFocusIndicatorRenderer1 = new FarPoint.Win.Spread.EnhancedFocusIndicatorRenderer();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer1 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer2 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            FarPoint.Win.EmptyBorder emptyBorder1 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder2 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder3 = new FarPoint.Win.EmptyBorder();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panMain = new System.Windows.Forms.Panel();
            this.panBody1 = new System.Windows.Forms.Panel();
            this.ssList1 = new FarPoint.Win.Spread.FpSpread();
            this.ssList1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.line2 = new DevComponents.DotNetBar.Controls.Line();
            this.line1 = new DevComponents.DotNetBar.Controls.Line();
            this.panFoot = new System.Windows.Forms.Panel();
            this.panHead = new System.Windows.Forms.Panel();
            this.chkYear = new System.Windows.Forms.CheckBox();
            this.panPano = new System.Windows.Forms.Panel();
            this.uc1 = new ComSupLibB.UcSupComPtSearch();
            this.txtJumin = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panbtn1 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel14 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.panTitleSub0.SuspendLayout();
            this.panMain.SuspendLayout();
            this.panBody1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList1_Sheet1)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panHead.SuspendLayout();
            this.panPano.SuspendLayout();
            this.panbtn1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 0);
            this.panTitleSub0.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(711, 32);
            this.panTitleSub0.TabIndex = 100;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(7, 6);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(96, 17);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "환자 종합 정보";
            // 
            // panMain
            // 
            this.panMain.Controls.Add(this.panBody1);
            this.panMain.Controls.Add(this.line2);
            this.panMain.Controls.Add(this.line1);
            this.panMain.Controls.Add(this.panFoot);
            this.panMain.Controls.Add(this.panHead);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(0, 32);
            this.panMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panMain.Name = "panMain";
            this.panMain.Size = new System.Drawing.Size(711, 424);
            this.panMain.TabIndex = 101;
            // 
            // panBody1
            // 
            this.panBody1.Controls.Add(this.ssList1);
            this.panBody1.Controls.Add(this.panel3);
            this.panBody1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBody1.Location = new System.Drawing.Point(0, 50);
            this.panBody1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panBody1.Name = "panBody1";
            this.panBody1.Size = new System.Drawing.Size(711, 353);
            this.panBody1.TabIndex = 121;
            // 
            // ssList1
            // 
            this.ssList1.AccessibleDescription = "";
            this.ssList1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ssList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssList1.FocusRenderer = enhancedFocusIndicatorRenderer1;
            this.ssList1.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssList1.HorizontalScrollBar.Name = "";
            this.ssList1.HorizontalScrollBar.Renderer = enhancedScrollBarRenderer1;
            this.ssList1.HorizontalScrollBar.TabIndex = 1;
            this.ssList1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssList1.Location = new System.Drawing.Point(0, 30);
            this.ssList1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ssList1.Name = "ssList1";
            this.ssList1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList1_Sheet1});
            this.ssList1.Size = new System.Drawing.Size(711, 323);
            this.ssList1.TabIndex = 7;
            this.ssList1.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssList1.VerticalScrollBar.Name = "";
            this.ssList1.VerticalScrollBar.Renderer = enhancedScrollBarRenderer2;
            this.ssList1.VerticalScrollBar.TabIndex = 16;
            // 
            // ssList1_Sheet1
            // 
            this.ssList1_Sheet1.Reset();
            this.ssList1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssList1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssList1_Sheet1.ColumnCount = 3;
            this.ssList1_Sheet1.RowCount = 1;
            this.ssList1_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterEnhanced";
            this.ssList1_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerEnhanced";
            this.ssList1_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderEnhanced";
            this.ssList1_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.Columns.Get(0).Border = emptyBorder1;
            this.ssList1_Sheet1.Columns.Get(1).Border = emptyBorder2;
            this.ssList1_Sheet1.Columns.Get(2).Border = emptyBorder3;
            this.ssList1_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.ssList1_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarEnhanced";
            this.ssList1_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderEnhanced";
            this.ssList1_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ssList1_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Black);
            this.ssList1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList1_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderEnhanced";
            this.ssList1_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.SheetCornerStyle.Parent = "CornerEnhanced";
            this.ssList1_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Black);
            this.ssList1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(711, 30);
            this.panel3.TabIndex = 0;
            this.panel3.Visible = false;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(711, 30);
            this.panel4.TabIndex = 101;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(8, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "접수 및 검사예약 정보";
            // 
            // line2
            // 
            this.line2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.line2.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.line2.Location = new System.Drawing.Point(0, 403);
            this.line2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.line2.Name = "line2";
            this.line2.Size = new System.Drawing.Size(711, 11);
            this.line2.TabIndex = 120;
            this.line2.Text = "line2";
            this.line2.Thickness = 5;
            // 
            // line1
            // 
            this.line1.Dock = System.Windows.Forms.DockStyle.Top;
            this.line1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.line1.Location = new System.Drawing.Point(0, 39);
            this.line1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(711, 11);
            this.line1.TabIndex = 119;
            this.line1.Text = "line1";
            this.line1.Thickness = 5;
            // 
            // panFoot
            // 
            this.panFoot.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panFoot.Location = new System.Drawing.Point(0, 414);
            this.panFoot.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panFoot.Name = "panFoot";
            this.panFoot.Size = new System.Drawing.Size(711, 10);
            this.panFoot.TabIndex = 1;
            this.panFoot.Visible = false;
            // 
            // panHead
            // 
            this.panHead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panHead.Controls.Add(this.chkYear);
            this.panHead.Controls.Add(this.panPano);
            this.panHead.Controls.Add(this.panbtn1);
            this.panHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHead.Location = new System.Drawing.Point(0, 0);
            this.panHead.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panHead.Name = "panHead";
            this.panHead.Size = new System.Drawing.Size(711, 39);
            this.panHead.TabIndex = 0;
            // 
            // chkYear
            // 
            this.chkYear.AutoSize = true;
            this.chkYear.Checked = true;
            this.chkYear.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkYear.Location = new System.Drawing.Point(424, 13);
            this.chkYear.Name = "chkYear";
            this.chkYear.Size = new System.Drawing.Size(99, 21);
            this.chkYear.TabIndex = 138;
            this.chkYear.Text = "6개월자료만";
            this.chkYear.UseVisualStyleBackColor = true;
            // 
            // panPano
            // 
            this.panPano.Controls.Add(this.uc1);
            this.panPano.Controls.Add(this.txtJumin);
            this.panPano.Controls.Add(this.label5);
            this.panPano.Dock = System.Windows.Forms.DockStyle.Left;
            this.panPano.Location = new System.Drawing.Point(0, 0);
            this.panPano.Name = "panPano";
            this.panPano.Padding = new System.Windows.Forms.Padding(3);
            this.panPano.Size = new System.Drawing.Size(417, 37);
            this.panPano.TabIndex = 137;
            // 
            // uc1
            // 
            this.uc1.AutoSize = true;
            this.uc1.BackColor = System.Drawing.Color.White;
            this.uc1.Location = new System.Drawing.Point(3, 2);
            this.uc1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.uc1.Name = "uc1";
            this.uc1.pPSMH_LPoint = new System.Drawing.Point(0, 0);
            this.uc1.PSMH_TITLE_VISIBLE = true;
            this.uc1.PSMH_TYPE = ComSupLibB.UcSupComPtSearch.enmType.PTINFO;
            this.uc1.Size = new System.Drawing.Size(251, 33);
            this.uc1.TabIndex = 8;
            // 
            // txtJumin
            // 
            this.txtJumin.Enabled = false;
            this.txtJumin.Location = new System.Drawing.Point(314, 5);
            this.txtJumin.Name = "txtJumin";
            this.txtJumin.Size = new System.Drawing.Size(95, 25);
            this.txtJumin.TabIndex = 133;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(252, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 17);
            this.label5.TabIndex = 132;
            this.label5.Text = "주민번호";
            // 
            // panbtn1
            // 
            this.panbtn1.Controls.Add(this.btnSearch);
            this.panbtn1.Controls.Add(this.panel14);
            this.panbtn1.Controls.Add(this.btnExit);
            this.panbtn1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panbtn1.Location = new System.Drawing.Point(530, 0);
            this.panbtn1.Name = "panbtn1";
            this.panbtn1.Padding = new System.Windows.Forms.Padding(3);
            this.panbtn1.Size = new System.Drawing.Size(179, 37);
            this.panbtn1.TabIndex = 118;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Location = new System.Drawing.Point(22, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 31);
            this.btnSearch.TabIndex = 38;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // panel14
            // 
            this.panel14.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel14.Location = new System.Drawing.Point(94, 3);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(10, 31);
            this.panel14.TabIndex = 160;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(104, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 31);
            this.btnExit.TabIndex = 29;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // frmComSupSTS01
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(711, 456);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.panTitleSub0);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmComSupSTS01";
            this.Text = "frmComSupSTS01";
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panMain.ResumeLayout(false);
            this.panBody1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList1_Sheet1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panHead.ResumeLayout(false);
            this.panHead.PerformLayout();
            this.panPano.ResumeLayout(false);
            this.panPano.PerformLayout();
            this.panbtn1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panMain;
        private System.Windows.Forms.Panel panHead;
        private System.Windows.Forms.Panel panFoot;
        private System.Windows.Forms.Panel panBody1;
        private DevComponents.DotNetBar.Controls.Line line2;
        private DevComponents.DotNetBar.Controls.Line line1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panbtn1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panel14;
        private FarPoint.Win.Spread.FpSpread ssList1;
        private FarPoint.Win.Spread.SheetView ssList1_Sheet1;
        private System.Windows.Forms.Panel panPano;
        private System.Windows.Forms.TextBox txtJumin;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkYear;
        private UcSupComPtSearch uc1;
    }
}