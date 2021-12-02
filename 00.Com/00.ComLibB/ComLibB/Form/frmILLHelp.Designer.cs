namespace ComLibB
{
    partial class frmILLHelp
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType13 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType14 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType15 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.ssSangbyeong = new FarPoint.Win.Spread.FpSpread();
            this.ssSangbyeong_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnPrint2 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.optPart1 = new System.Windows.Forms.RadioButton();
            this.optPart0 = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.optSort1 = new System.Windows.Forms.RadioButton();
            this.optSort0 = new System.Windows.Forms.RadioButton();
            this.lblSort = new System.Windows.Forms.Label();
            this.optSort2 = new System.Windows.Forms.RadioButton();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.cboViewClass = new System.Windows.Forms.ComboBox();
            this.lblViewClass = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssSangbyeong)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssSangbyeong_Sheet1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(639, 34);
            this.panTitle.TabIndex = 84;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(565, 2);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(9, 7);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(74, 21);
            this.lblTitle.TabIndex = 15;
            this.lblTitle.Text = "상병조회";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ssSangbyeong
            // 
            this.ssSangbyeong.AccessibleDescription = "ssSangbyeong, Sheet1, Row 0, Column 0, ";
            this.ssSangbyeong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssSangbyeong.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssSangbyeong.Location = new System.Drawing.Point(0, 174);
            this.ssSangbyeong.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ssSangbyeong.Name = "ssSangbyeong";
            this.ssSangbyeong.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssSangbyeong_Sheet1});
            this.ssSangbyeong.Size = new System.Drawing.Size(639, 333);
            this.ssSangbyeong.TabIndex = 2;
            this.ssSangbyeong.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssSangbyeong_CellDoubleClick);
            // 
            // ssSangbyeong_Sheet1
            // 
            this.ssSangbyeong_Sheet1.Reset();
            this.ssSangbyeong_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssSangbyeong_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssSangbyeong_Sheet1.ColumnCount = 5;
            this.ssSangbyeong_Sheet1.RowCount = 1;
            this.ssSangbyeong_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "상병코드";
            this.ssSangbyeong_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "종류";
            this.ssSangbyeong_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "한글명칭";
            this.ssSangbyeong_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "영문명칭";
            this.ssSangbyeong_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "Header";
            this.ssSangbyeong_Sheet1.ColumnHeader.Rows.Get(0).Height = 24F;
            this.ssSangbyeong_Sheet1.Columns.Get(0).CellType = textCellType11;
            this.ssSangbyeong_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSangbyeong_Sheet1.Columns.Get(0).Label = "상병코드";
            this.ssSangbyeong_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSangbyeong_Sheet1.Columns.Get(0).Width = 80F;
            this.ssSangbyeong_Sheet1.Columns.Get(1).CellType = textCellType12;
            this.ssSangbyeong_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSangbyeong_Sheet1.Columns.Get(1).Label = "종류";
            this.ssSangbyeong_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSangbyeong_Sheet1.Columns.Get(1).Width = 40F;
            this.ssSangbyeong_Sheet1.Columns.Get(2).CellType = textCellType13;
            this.ssSangbyeong_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssSangbyeong_Sheet1.Columns.Get(2).Label = "한글명칭";
            this.ssSangbyeong_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSangbyeong_Sheet1.Columns.Get(2).Width = 220F;
            this.ssSangbyeong_Sheet1.Columns.Get(3).CellType = textCellType14;
            this.ssSangbyeong_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssSangbyeong_Sheet1.Columns.Get(3).Label = "영문명칭";
            this.ssSangbyeong_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSangbyeong_Sheet1.Columns.Get(3).Width = 240F;
            this.ssSangbyeong_Sheet1.Columns.Get(4).CellType = textCellType15;
            this.ssSangbyeong_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSangbyeong_Sheet1.Columns.Get(4).Label = "Header";
            this.ssSangbyeong_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSangbyeong_Sheet1.Columns.Get(4).Width = 120F;
            this.ssSangbyeong_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ssSangbyeong_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssSangbyeong_Sheet1.RowHeader.Columns.Get(0).Width = 38F;
            this.ssSangbyeong_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btnPrint2);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.btnView);
            this.panel1.Controls.Add(this.txtSearch);
            this.panel1.Controls.Add(this.cboViewClass);
            this.panel1.Controls.Add(this.lblViewClass);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 66);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(639, 108);
            this.panel1.TabIndex = 1;
            // 
            // btnPrint2
            // 
            this.btnPrint2.Location = new System.Drawing.Point(380, 68);
            this.btnPrint2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPrint2.Name = "btnPrint2";
            this.btnPrint2.Size = new System.Drawing.Size(140, 33);
            this.btnPrint2.TabIndex = 11;
            this.btnPrint2.Text = "고속프린트로 전송";
            this.btnPrint2.UseVisualStyleBackColor = true;
            this.btnPrint2.Click += new System.EventHandler(this.btnPrint2_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.optPart1);
            this.panel3.Controls.Add(this.optPart0);
            this.panel3.Location = new System.Drawing.Point(259, 68);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(120, 33);
            this.panel3.TabIndex = 10;
            // 
            // optPart1
            // 
            this.optPart1.AutoSize = true;
            this.optPart1.Location = new System.Drawing.Point(58, 5);
            this.optPart1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optPart1.Name = "optPart1";
            this.optPart1.Size = new System.Drawing.Size(55, 23);
            this.optPart1.TabIndex = 5;
            this.optPart1.TabStop = true;
            this.optPart1.Text = "단면";
            this.optPart1.UseVisualStyleBackColor = true;
            // 
            // optPart0
            // 
            this.optPart0.AutoSize = true;
            this.optPart0.Location = new System.Drawing.Point(5, 5);
            this.optPart0.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optPart0.Name = "optPart0";
            this.optPart0.Size = new System.Drawing.Size(55, 23);
            this.optPart0.TabIndex = 4;
            this.optPart0.TabStop = true;
            this.optPart0.Text = "양면";
            this.optPart0.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.optSort1);
            this.panel2.Controls.Add(this.optSort0);
            this.panel2.Controls.Add(this.lblSort);
            this.panel2.Controls.Add(this.optSort2);
            this.panel2.Location = new System.Drawing.Point(173, 6);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(264, 35);
            this.panel2.TabIndex = 9;
            // 
            // optSort1
            // 
            this.optSort1.AutoSize = true;
            this.optSort1.Location = new System.Drawing.Point(123, 6);
            this.optSort1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optSort1.Name = "optSort1";
            this.optSort1.Size = new System.Drawing.Size(69, 23);
            this.optSort1.TabIndex = 4;
            this.optSort1.Text = "한글명";
            this.optSort1.UseVisualStyleBackColor = true;
            // 
            // optSort0
            // 
            this.optSort0.AutoSize = true;
            this.optSort0.Checked = true;
            this.optSort0.Location = new System.Drawing.Point(68, 6);
            this.optSort0.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optSort0.Name = "optSort0";
            this.optSort0.Size = new System.Drawing.Size(55, 23);
            this.optSort0.TabIndex = 3;
            this.optSort0.TabStop = true;
            this.optSort0.Text = "코드";
            this.optSort0.UseVisualStyleBackColor = true;
            // 
            // lblSort
            // 
            this.lblSort.AutoSize = true;
            this.lblSort.Location = new System.Drawing.Point(3, 8);
            this.lblSort.Name = "lblSort";
            this.lblSort.Size = new System.Drawing.Size(65, 19);
            this.lblSort.TabIndex = 2;
            this.lblSort.Text = "검색조건";
            // 
            // optSort2
            // 
            this.optSort2.AutoSize = true;
            this.optSort2.Location = new System.Drawing.Point(192, 6);
            this.optSort2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optSort2.Name = "optSort2";
            this.optSort2.Size = new System.Drawing.Size(69, 23);
            this.optSort2.TabIndex = 5;
            this.optSort2.Text = "영문명";
            this.optSort2.UseVisualStyleBackColor = true;
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(184, 68);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 33);
            this.btnPrint.TabIndex = 8;
            this.btnPrint.Text = "자료인쇄";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(109, 68);
            this.btnView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(75, 33);
            this.btnView.TabIndex = 7;
            this.btnView.Text = "자료조회";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(5, 72);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(100, 25);
            this.txtSearch.TabIndex = 6;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // cboViewClass
            // 
            this.cboViewClass.FormattingEnabled = true;
            this.cboViewClass.Location = new System.Drawing.Point(46, 11);
            this.cboViewClass.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboViewClass.Name = "cboViewClass";
            this.cboViewClass.Size = new System.Drawing.Size(121, 25);
            this.cboViewClass.TabIndex = 1;
            // 
            // lblViewClass
            // 
            this.lblViewClass.AutoSize = true;
            this.lblViewClass.Location = new System.Drawing.Point(9, 14);
            this.lblViewClass.Name = "lblViewClass";
            this.lblViewClass.Size = new System.Drawing.Size(37, 19);
            this.lblViewClass.TabIndex = 0;
            this.lblViewClass.Text = "종류";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.lblTitleSub0);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 34);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(639, 32);
            this.panel4.TabIndex = 85;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(9, 5);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(70, 19);
            this.lblTitleSub0.TabIndex = 23;
            this.lblTitleSub0.Text = "상병 조회";
            this.lblTitleSub0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmILLHelp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(639, 507);
            this.ControlBox = false;
            this.Controls.Add(this.ssSangbyeong);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmILLHelp";
            this.Text = "상병조회";
            this.Load += new System.EventHandler(this.frmILLHelp_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssSangbyeong)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssSangbyeong_Sheet1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private FarPoint.Win.Spread.FpSpread ssSangbyeong;
        private FarPoint.Win.Spread.SheetView ssSangbyeong_Sheet1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton optPart1;
        private System.Windows.Forms.RadioButton optPart0;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton optSort1;
        private System.Windows.Forms.RadioButton optSort0;
        private System.Windows.Forms.Label lblSort;
        private System.Windows.Forms.RadioButton optSort2;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ComboBox cboViewClass;
        private System.Windows.Forms.Label lblViewClass;
        private System.Windows.Forms.Button btnPrint2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblTitleSub0;
    }
}