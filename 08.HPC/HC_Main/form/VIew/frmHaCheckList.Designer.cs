namespace HC_Main
{
    partial class frmHaCheckList
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
            FarPoint.Win.Spread.FlatFocusIndicatorRenderer flatFocusIndicatorRenderer1 = new FarPoint.Win.Spread.FlatFocusIndicatorRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer1 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer2 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panSub01 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkSel4 = new System.Windows.Forms.CheckBox();
            this.chkSel3 = new System.Windows.Forms.CheckBox();
            this.chkSel2 = new System.Windows.Forms.CheckBox();
            this.chkSel1 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtSName = new System.Windows.Forms.TextBox();
            this.panSub03 = new System.Windows.Forms.Panel();
            this.btnLtdHelp = new System.Windows.Forms.Button();
            this.txtLtdName = new System.Windows.Forms.TextBox();
            this.cboJong = new System.Windows.Forms.ComboBox();
            this.panSub05 = new System.Windows.Forms.Panel();
            this.lblSub03 = new System.Windows.Forms.Label();
            this.lblSub02 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.panSub02 = new System.Windows.Forms.Panel();
            this.dtpTDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFDate = new System.Windows.Forms.DateTimePicker();
            this.lblSub01 = new System.Windows.Forms.Label();
            this.panMain = new System.Windows.Forms.Panel();
            this.SSList = new FarPoint.Win.Spread.FpSpread();
            this.SSList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panTitle.SuspendLayout();
            this.panSub01.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panSub03.SuspendLayout();
            this.panSub05.SuspendLayout();
            this.panSub02.SuspendLayout();
            this.panMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1185, 40);
            this.panTitle.TabIndex = 15;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(1101, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(82, 38);
            this.btnExit.TabIndex = 20;
            this.btnExit.Text = "닫 기(&X)";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(3, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(173, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "종검수검자 Check List";
            // 
            // panSub01
            // 
            this.panSub01.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panSub01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSub01.Controls.Add(this.groupBox2);
            this.panSub01.Controls.Add(this.groupBox1);
            this.panSub01.Controls.Add(this.panSub03);
            this.panSub01.Controls.Add(this.panSub05);
            this.panSub01.Controls.Add(this.btnSearch);
            this.panSub01.Controls.Add(this.btnPrint);
            this.panSub01.Controls.Add(this.panSub02);
            this.panSub01.Controls.Add(this.lblSub01);
            this.panSub01.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSub01.Location = new System.Drawing.Point(0, 40);
            this.panSub01.Name = "panSub01";
            this.panSub01.Padding = new System.Windows.Forms.Padding(1);
            this.panSub01.Size = new System.Drawing.Size(1185, 65);
            this.panSub01.TabIndex = 134;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkSel4);
            this.groupBox2.Controls.Add(this.chkSel3);
            this.groupBox2.Controls.Add(this.chkSel2);
            this.groupBox2.Controls.Add(this.chkSel1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.Location = new System.Drawing.Point(612, 1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(255, 61);
            this.groupBox2.TabIndex = 61;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "검색조건";
            // 
            // chkSel4
            // 
            this.chkSel4.AutoSize = true;
            this.chkSel4.Location = new System.Drawing.Point(196, 26);
            this.chkSel4.Name = "chkSel4";
            this.chkSel4.Size = new System.Drawing.Size(53, 21);
            this.chkSel4.TabIndex = 3;
            this.chkSel4.Text = "발송";
            this.chkSel4.UseVisualStyleBackColor = true;
            // 
            // chkSel3
            // 
            this.chkSel3.AutoSize = true;
            this.chkSel3.Location = new System.Drawing.Point(137, 26);
            this.chkSel3.Name = "chkSel3";
            this.chkSel3.Size = new System.Drawing.Size(53, 21);
            this.chkSel3.TabIndex = 2;
            this.chkSel3.Text = "출력";
            this.chkSel3.UseVisualStyleBackColor = true;
            // 
            // chkSel2
            // 
            this.chkSel2.AutoSize = true;
            this.chkSel2.Location = new System.Drawing.Point(78, 26);
            this.chkSel2.Name = "chkSel2";
            this.chkSel2.Size = new System.Drawing.Size(53, 21);
            this.chkSel2.TabIndex = 1;
            this.chkSel2.Text = "판정";
            this.chkSel2.UseVisualStyleBackColor = true;
            // 
            // chkSel1
            // 
            this.chkSel1.AutoSize = true;
            this.chkSel1.Location = new System.Drawing.Point(6, 26);
            this.chkSel1.Name = "chkSel1";
            this.chkSel1.Size = new System.Drawing.Size(66, 21);
            this.chkSel1.TabIndex = 0;
            this.chkSel1.Text = "가판정";
            this.chkSel1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSName);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(455, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(157, 61);
            this.groupBox1.TabIndex = 60;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "수검자명";
            // 
            // txtSName
            // 
            this.txtSName.Location = new System.Drawing.Point(6, 24);
            this.txtSName.Name = "txtSName";
            this.txtSName.Size = new System.Drawing.Size(145, 25);
            this.txtSName.TabIndex = 2;
            this.txtSName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panSub03
            // 
            this.panSub03.Controls.Add(this.btnLtdHelp);
            this.panSub03.Controls.Add(this.txtLtdName);
            this.panSub03.Controls.Add(this.cboJong);
            this.panSub03.Dock = System.Windows.Forms.DockStyle.Left;
            this.panSub03.Location = new System.Drawing.Point(251, 1);
            this.panSub03.Name = "panSub03";
            this.panSub03.Size = new System.Drawing.Size(204, 61);
            this.panSub03.TabIndex = 56;
            // 
            // btnLtdHelp
            // 
            this.btnLtdHelp.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLtdHelp.Location = new System.Drawing.Point(177, 32);
            this.btnLtdHelp.Name = "btnLtdHelp";
            this.btnLtdHelp.Size = new System.Drawing.Size(24, 25);
            this.btnLtdHelp.TabIndex = 95;
            this.btnLtdHelp.Text = "&H";
            this.btnLtdHelp.UseVisualStyleBackColor = true;
            // 
            // txtLtdName
            // 
            this.txtLtdName.Location = new System.Drawing.Point(3, 32);
            this.txtLtdName.Name = "txtLtdName";
            this.txtLtdName.Size = new System.Drawing.Size(171, 25);
            this.txtLtdName.TabIndex = 94;
            this.txtLtdName.Tag = "";
            this.txtLtdName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cboJong
            // 
            this.cboJong.FormattingEnabled = true;
            this.cboJong.Location = new System.Drawing.Point(3, 3);
            this.cboJong.Name = "cboJong";
            this.cboJong.Size = new System.Drawing.Size(198, 25);
            this.cboJong.TabIndex = 0;
            // 
            // panSub05
            // 
            this.panSub05.Controls.Add(this.lblSub03);
            this.panSub05.Controls.Add(this.lblSub02);
            this.panSub05.Dock = System.Windows.Forms.DockStyle.Left;
            this.panSub05.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.panSub05.Location = new System.Drawing.Point(159, 1);
            this.panSub05.Name = "panSub05";
            this.panSub05.Padding = new System.Windows.Forms.Padding(1);
            this.panSub05.Size = new System.Drawing.Size(92, 61);
            this.panSub05.TabIndex = 55;
            // 
            // lblSub03
            // 
            this.lblSub03.BackColor = System.Drawing.Color.LightBlue;
            this.lblSub03.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSub03.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSub03.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSub03.Location = new System.Drawing.Point(1, 29);
            this.lblSub03.Name = "lblSub03";
            this.lblSub03.Size = new System.Drawing.Size(90, 31);
            this.lblSub03.TabIndex = 48;
            this.lblSub03.Text = "회사코드";
            this.lblSub03.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSub02
            // 
            this.lblSub02.BackColor = System.Drawing.Color.LightBlue;
            this.lblSub02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSub02.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSub02.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSub02.Location = new System.Drawing.Point(1, 1);
            this.lblSub02.Name = "lblSub02";
            this.lblSub02.Size = new System.Drawing.Size(90, 28);
            this.lblSub02.TabIndex = 46;
            this.lblSub02.Text = "검진종류";
            this.lblSub02.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(1018, 1);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(82, 61);
            this.btnSearch.TabIndex = 54;
            this.btnSearch.Text = "조 회(&V)";
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.White;
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrint.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPrint.Location = new System.Drawing.Point(1100, 1);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(82, 61);
            this.btnPrint.TabIndex = 53;
            this.btnPrint.Text = "인 쇄(&P)";
            this.btnPrint.UseVisualStyleBackColor = false;
            // 
            // panSub02
            // 
            this.panSub02.Controls.Add(this.dtpTDate);
            this.panSub02.Controls.Add(this.dtpFDate);
            this.panSub02.Dock = System.Windows.Forms.DockStyle.Left;
            this.panSub02.Location = new System.Drawing.Point(64, 1);
            this.panSub02.Name = "panSub02";
            this.panSub02.Size = new System.Drawing.Size(95, 61);
            this.panSub02.TabIndex = 44;
            // 
            // dtpTDate
            // 
            this.dtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTDate.Location = new System.Drawing.Point(3, 32);
            this.dtpTDate.Name = "dtpTDate";
            this.dtpTDate.Size = new System.Drawing.Size(88, 25);
            this.dtpTDate.TabIndex = 1;
            // 
            // dtpFDate
            // 
            this.dtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFDate.Location = new System.Drawing.Point(3, 3);
            this.dtpFDate.Name = "dtpFDate";
            this.dtpFDate.Size = new System.Drawing.Size(88, 25);
            this.dtpFDate.TabIndex = 0;
            // 
            // lblSub01
            // 
            this.lblSub01.BackColor = System.Drawing.Color.LightBlue;
            this.lblSub01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSub01.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSub01.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSub01.Location = new System.Drawing.Point(1, 1);
            this.lblSub01.Name = "lblSub01";
            this.lblSub01.Size = new System.Drawing.Size(63, 61);
            this.lblSub01.TabIndex = 43;
            this.lblSub01.Text = "접수일자";
            this.lblSub01.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panMain
            // 
            this.panMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panMain.Controls.Add(this.SSList);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(0, 105);
            this.panMain.Name = "panMain";
            this.panMain.Padding = new System.Windows.Forms.Padding(1);
            this.panMain.Size = new System.Drawing.Size(1185, 706);
            this.panMain.TabIndex = 136;
            // 
            // SSList
            // 
            this.SSList.AccessibleDescription = "SSList, Sheet1, Row 0, Column 0, ";
            this.SSList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSList.FocusRenderer = flatFocusIndicatorRenderer1;
            this.SSList.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SSList.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SSList.HorizontalScrollBar.Renderer = flatScrollBarRenderer1;
            this.SSList.HorizontalScrollBar.TabIndex = 75;
            this.SSList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SSList.Location = new System.Drawing.Point(1, 1);
            this.SSList.Name = "SSList";
            this.SSList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSList_Sheet1});
            this.SSList.Size = new System.Drawing.Size(1181, 702);
            this.SSList.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.SSList.TabIndex = 140;
            this.SSList.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SSList.VerticalScrollBar.Name = "";
            flatScrollBarRenderer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SSList.VerticalScrollBar.Renderer = flatScrollBarRenderer2;
            this.SSList.VerticalScrollBar.TabIndex = 76;
            this.SSList.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // SSList_Sheet1
            // 
            this.SSList_Sheet1.Reset();
            this.SSList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSList_Sheet1.ColumnCount = 5;
            this.SSList_Sheet1.RowCount = 1;
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.SSList_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.SSList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SSList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSList_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.SSList_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.Rows.Get(0).Height = 24F;
            this.SSList_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.SSList_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmHaCheckList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1185, 811);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.panSub01);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHaCheckList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "종검 수검자 Check List";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panSub01.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panSub03.ResumeLayout(false);
            this.panSub03.PerformLayout();
            this.panSub05.ResumeLayout(false);
            this.panSub02.ResumeLayout(false);
            this.panMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panSub01;
        private System.Windows.Forms.Panel panSub02;
        private System.Windows.Forms.DateTimePicker dtpTDate;
        private System.Windows.Forms.DateTimePicker dtpFDate;
        private System.Windows.Forms.Label lblSub01;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panSub03;
        private System.Windows.Forms.ComboBox cboJong;
        private System.Windows.Forms.Panel panSub05;
        private System.Windows.Forms.Label lblSub03;
        private System.Windows.Forms.Label lblSub02;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkSel4;
        private System.Windows.Forms.CheckBox chkSel3;
        private System.Windows.Forms.CheckBox chkSel2;
        private System.Windows.Forms.CheckBox chkSel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtSName;
        private System.Windows.Forms.Button btnLtdHelp;
        private System.Windows.Forms.TextBox txtLtdName;
        private System.Windows.Forms.Panel panMain;
        private FarPoint.Win.Spread.FpSpread SSList;
        private FarPoint.Win.Spread.SheetView SSList_Sheet1;
    }
}