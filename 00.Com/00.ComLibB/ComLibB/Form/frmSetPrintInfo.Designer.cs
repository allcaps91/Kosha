namespace ComLibB
{
    partial class frmSetPrintInfo
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
            FarPoint.Win.Spread.InputMap ssList_InputMapWhenFocusedNormal;
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnPrintTemp = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlBody = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.grpInfo = new System.Windows.Forms.GroupBox();
            this.cboUse = new System.Windows.Forms.ComboBox();
            this.cboGwange = new System.Windows.Forms.ComboBox();
            this.txtHphone = new System.Windows.Forms.TextBox();
            this.txtJumin2 = new System.Windows.Forms.TextBox();
            this.txtJumin1 = new System.Windows.Forms.TextBox();
            this.txtPage = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.ssList = new FarPoint.Win.Spread.FpSpread();
            this.ssList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.pnlHead = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.eventLog1 = new System.Diagnostics.EventLog();
            this.btnSet20 = new System.Windows.Forms.Button();
            this.btnSet21 = new System.Windows.Forms.Button();
            this.btnSet22 = new System.Windows.Forms.Button();
            this.btnSet11 = new System.Windows.Forms.Button();
            this.btnSet10 = new System.Windows.Forms.Button();
            this.btnSet12 = new System.Windows.Forms.Button();
            ssList_InputMapWhenFocusedNormal = new FarPoint.Win.Spread.InputMap();
            this.pnlBottom.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.grpInfo.SuspendLayout();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).BeginInit();
            this.pnlHead.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Window;
            this.pnlBottom.Controls.Add(this.btnPrintTemp);
            this.pnlBottom.Controls.Add(this.btnPrint);
            this.pnlBottom.Controls.Add(this.btnExit);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 447);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(10, 14, 10, 14);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Padding = new System.Windows.Forms.Padding(3);
            this.pnlBottom.Size = new System.Drawing.Size(534, 42);
            this.pnlBottom.TabIndex = 200;
            // 
            // btnPrintTemp
            // 
            this.btnPrintTemp.BackColor = System.Drawing.SystemColors.Window;
            this.btnPrintTemp.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrintTemp.Location = new System.Drawing.Point(253, 3);
            this.btnPrintTemp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPrintTemp.Name = "btnPrintTemp";
            this.btnPrintTemp.Size = new System.Drawing.Size(110, 36);
            this.btnPrintTemp.TabIndex = 200;
            this.btnPrintTemp.Text = "원무팀 업무용";
            this.btnPrintTemp.UseVisualStyleBackColor = true;
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.SystemColors.Window;
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrint.Location = new System.Drawing.Point(363, 3);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(83, 36);
            this.btnPrint.TabIndex = 199;
            this.btnPrint.Text = "출력";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.SystemColors.Window;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(446, 3);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(85, 36);
            this.btnExit.TabIndex = 170;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // pnlBody
            // 
            this.pnlBody.BackColor = System.Drawing.SystemColors.Window;
            this.pnlBody.Controls.Add(this.label1);
            this.pnlBody.Controls.Add(this.grpInfo);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(0, 169);
            this.pnlBody.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(534, 320);
            this.pnlBody.TabIndex = 199;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label1.Location = new System.Drawing.Point(310, 248);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(213, 17);
            this.label1.TabIndex = 168;
            this.label1.Text = "▽ 출력(업무용)은 저장기능이 없음";
            // 
            // grpInfo
            // 
            this.grpInfo.Controls.Add(this.btnSet12);
            this.grpInfo.Controls.Add(this.btnSet10);
            this.grpInfo.Controls.Add(this.btnSet11);
            this.grpInfo.Controls.Add(this.btnSet22);
            this.grpInfo.Controls.Add(this.btnSet21);
            this.grpInfo.Controls.Add(this.btnSet20);
            this.grpInfo.Controls.Add(this.cboUse);
            this.grpInfo.Controls.Add(this.cboGwange);
            this.grpInfo.Controls.Add(this.txtHphone);
            this.grpInfo.Controls.Add(this.txtJumin2);
            this.grpInfo.Controls.Add(this.txtJumin1);
            this.grpInfo.Controls.Add(this.txtPage);
            this.grpInfo.Controls.Add(this.txtName);
            this.grpInfo.Controls.Add(this.label9);
            this.grpInfo.Controls.Add(this.label8);
            this.grpInfo.Controls.Add(this.label6);
            this.grpInfo.Controls.Add(this.label5);
            this.grpInfo.Controls.Add(this.label3);
            this.grpInfo.Controls.Add(this.label4);
            this.grpInfo.Location = new System.Drawing.Point(12, 18);
            this.grpInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpInfo.Name = "grpInfo";
            this.grpInfo.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpInfo.Size = new System.Drawing.Size(506, 211);
            this.grpInfo.TabIndex = 0;
            this.grpInfo.TabStop = false;
            this.grpInfo.Text = "발급자 정보";
            // 
            // cboUse
            // 
            this.cboUse.FormattingEnabled = true;
            this.cboUse.Location = new System.Drawing.Point(75, 159);
            this.cboUse.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboUse.Name = "cboUse";
            this.cboUse.Size = new System.Drawing.Size(176, 25);
            this.cboUse.TabIndex = 68;
            // 
            // cboGwange
            // 
            this.cboGwange.FormattingEnabled = true;
            this.cboGwange.Location = new System.Drawing.Point(75, 119);
            this.cboGwange.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboGwange.Name = "cboGwange";
            this.cboGwange.Size = new System.Drawing.Size(176, 25);
            this.cboGwange.TabIndex = 67;
            // 
            // txtHphone
            // 
            this.txtHphone.Location = new System.Drawing.Point(353, 78);
            this.txtHphone.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtHphone.Name = "txtHphone";
            this.txtHphone.Size = new System.Drawing.Size(101, 25);
            this.txtHphone.TabIndex = 44;
            // 
            // txtJumin2
            // 
            this.txtJumin2.Location = new System.Drawing.Point(134, 78);
            this.txtJumin2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtJumin2.Name = "txtJumin2";
            this.txtJumin2.Size = new System.Drawing.Size(72, 25);
            this.txtJumin2.TabIndex = 42;
            this.txtJumin2.Text = "1234567";
            // 
            // txtJumin1
            // 
            this.txtJumin1.Location = new System.Drawing.Point(75, 78);
            this.txtJumin1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtJumin1.Name = "txtJumin1";
            this.txtJumin1.Size = new System.Drawing.Size(53, 25);
            this.txtJumin1.TabIndex = 41;
            this.txtJumin1.Text = "123456";
            // 
            // txtPage
            // 
            this.txtPage.Location = new System.Drawing.Point(353, 38);
            this.txtPage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPage.Name = "txtPage";
            this.txtPage.Size = new System.Drawing.Size(101, 25);
            this.txtPage.TabIndex = 40;
            this.txtPage.Text = "박병규";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(74, 38);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(132, 25);
            this.txtName.TabIndex = 39;
            this.txtName.Text = "박병규";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.SystemColors.Window;
            this.label9.Location = new System.Drawing.Point(39, 125);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 17);
            this.label9.TabIndex = 37;
            this.label9.Text = "관계";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.SystemColors.Window;
            this.label8.Location = new System.Drawing.Point(39, 164);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 17);
            this.label8.TabIndex = 36;
            this.label8.Text = "용도";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.SystemColors.Window;
            this.label6.Location = new System.Drawing.Point(301, 84);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 17);
            this.label6.TabIndex = 35;
            this.label6.Text = "연락처";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.SystemColors.Window;
            this.label5.Location = new System.Drawing.Point(15, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 17);
            this.label5.TabIndex = 34;
            this.label5.Text = "주민번호";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.Window;
            this.label3.Location = new System.Drawing.Point(39, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 17);
            this.label3.TabIndex = 33;
            this.label3.Text = "성명";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.Window;
            this.label4.Location = new System.Drawing.Point(289, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 17);
            this.label4.TabIndex = 32;
            this.label4.Text = "인쇄매수";
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.SystemColors.Window;
            this.pnlTop.Controls.Add(this.label2);
            this.pnlTop.Controls.Add(this.ssList);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 42);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(10, 14, 10, 14);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.pnlTop.Size = new System.Drawing.Size(534, 127);
            this.pnlTop.TabIndex = 198;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label2.Location = new System.Drawing.Point(14, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 17);
            this.label2.TabIndex = 169;
            this.label2.Text = "▽ Click시 기본정보 설정";
            // 
            // ssList
            // 
            this.ssList.AccessibleDescription = "ssList, Sheet1, Row 0, Column 0, 1";
            this.ssList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ssList.FocusRenderer = enhancedFocusIndicatorRenderer1;
            this.ssList.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssList.HorizontalScrollBar.Name = "";
            this.ssList.HorizontalScrollBar.Renderer = enhancedScrollBarRenderer1;
            this.ssList.HorizontalScrollBar.TabIndex = 92;
            this.ssList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssList.Location = new System.Drawing.Point(12, 44);
            this.ssList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ssList.Name = "ssList";
            this.ssList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList_Sheet1});
            this.ssList.Size = new System.Drawing.Size(511, 67);
            this.ssList.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;
            this.ssList.TabIndex = 167;
            this.ssList.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssList.VerticalScrollBar.Name = "";
            this.ssList.VerticalScrollBar.Renderer = enhancedScrollBarRenderer2;
            this.ssList.VerticalScrollBar.TabIndex = 93;
            this.ssList.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssList.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssList_CellClick);
            ssList_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Return, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextColumn);
            this.ssList.SetInputMap(FarPoint.Win.Spread.InputMapMode.WhenFocused, FarPoint.Win.Spread.OperationMode.Normal, ssList_InputMapWhenFocusedNormal);
            // 
            // ssList_Sheet1
            // 
            this.ssList_Sheet1.Reset();
            this.ssList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssList_Sheet1.ColumnCount = 5;
            this.ssList_Sheet1.RowCount = 1;
            this.ssList_Sheet1.Cells.Get(0, 0).Value = "1";
            this.ssList_Sheet1.Cells.Get(0, 1).Value = "2";
            this.ssList_Sheet1.Cells.Get(0, 2).Value = "3";
            this.ssList_Sheet1.Cells.Get(0, 3).Value = "4";
            this.ssList_Sheet1.Cells.Get(0, 4).Value = "5";
            this.ssList_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterEnhanced";
            this.ssList_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerEnhanced";
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "등록번호";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "성명";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "주민번호(앞)";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "주민번호(뒤)";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "휴대폰번호";
            this.ssList_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderEnhanced";
            this.ssList_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssList_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssList_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(0).Label = "등록번호";
            this.ssList_Sheet1.Columns.Get(0).Locked = true;
            this.ssList_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(0).Width = 74F;
            this.ssList_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssList_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssList_Sheet1.Columns.Get(1).Label = "성명";
            this.ssList_Sheet1.Columns.Get(1).Locked = true;
            this.ssList_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(1).Width = 125F;
            this.ssList_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssList_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(2).Label = "주민번호(앞)";
            this.ssList_Sheet1.Columns.Get(2).Locked = true;
            this.ssList_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(2).Width = 99F;
            this.ssList_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssList_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(3).Label = "주민번호(뒤)";
            this.ssList_Sheet1.Columns.Get(3).Locked = true;
            this.ssList_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(3).Width = 99F;
            this.ssList_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.ssList_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(4).Label = "휴대폰번호";
            this.ssList_Sheet1.Columns.Get(4).Locked = true;
            this.ssList_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(4).Width = 110F;
            this.ssList_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.DefaultStyle.Locked = true;
            this.ssList_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.ssList_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarEnhanced";
            this.ssList_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderEnhanced";
            this.ssList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderEnhanced";
            this.ssList_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.RowHeader.Visible = false;
            this.ssList_Sheet1.Rows.Get(0).Height = 37F;
            this.ssList_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.SheetCornerStyle.Parent = "CornerEnhanced";
            this.ssList_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // pnlHead
            // 
            this.pnlHead.BackColor = System.Drawing.Color.RoyalBlue;
            this.pnlHead.Controls.Add(this.label7);
            this.pnlHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHead.ForeColor = System.Drawing.Color.White;
            this.pnlHead.ImeMode = System.Windows.Forms.ImeMode.KatakanaHalf;
            this.pnlHead.Location = new System.Drawing.Point(0, 0);
            this.pnlHead.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlHead.Name = "pnlHead";
            this.pnlHead.Size = new System.Drawing.Size(534, 42);
            this.pnlHead.TabIndex = 197;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(14, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 17);
            this.label7.TabIndex = 0;
            this.label7.Text = "인쇄정보";
            // 
            // eventLog1
            // 
            this.eventLog1.SynchronizingObject = this;
            // 
            // btnSet20
            // 
            this.btnSet20.AutoSize = true;
            this.btnSet20.Location = new System.Drawing.Point(257, 158);
            this.btnSet20.Name = "btnSet20";
            this.btnSet20.Size = new System.Drawing.Size(70, 27);
            this.btnSet20.TabIndex = 69;
            this.btnSet20.Text = "학교제출";
            this.btnSet20.UseVisualStyleBackColor = true;
            // 
            // btnSet21
            // 
            this.btnSet21.AutoSize = true;
            this.btnSet21.Location = new System.Drawing.Point(327, 158);
            this.btnSet21.Name = "btnSet21";
            this.btnSet21.Size = new System.Drawing.Size(70, 27);
            this.btnSet21.TabIndex = 69;
            this.btnSet21.Text = "보험회사";
            this.btnSet21.UseVisualStyleBackColor = true;
            // 
            // btnSet22
            // 
            this.btnSet22.AutoSize = true;
            this.btnSet22.Location = new System.Drawing.Point(397, 158);
            this.btnSet22.Name = "btnSet22";
            this.btnSet22.Size = new System.Drawing.Size(44, 27);
            this.btnSet22.TabIndex = 69;
            this.btnSet22.Text = "기타";
            this.btnSet22.UseVisualStyleBackColor = true;
            // 
            // btnSet11
            // 
            this.btnSet11.AutoSize = true;
            this.btnSet11.Location = new System.Drawing.Point(257, 118);
            this.btnSet11.Name = "btnSet11";
            this.btnSet11.Size = new System.Drawing.Size(44, 27);
            this.btnSet11.TabIndex = 69;
            this.btnSet11.Text = "본인";
            this.btnSet11.UseVisualStyleBackColor = true;
            // 
            // btnSet10
            // 
            this.btnSet10.AutoSize = true;
            this.btnSet10.Location = new System.Drawing.Point(301, 118);
            this.btnSet10.Name = "btnSet10";
            this.btnSet10.Size = new System.Drawing.Size(57, 27);
            this.btnSet10.TabIndex = 69;
            this.btnSet10.Text = "보호자";
            this.btnSet10.UseVisualStyleBackColor = true;
            // 
            // btnSet12
            // 
            this.btnSet12.AutoSize = true;
            this.btnSet12.Location = new System.Drawing.Point(358, 118);
            this.btnSet12.Name = "btnSet12";
            this.btnSet12.Size = new System.Drawing.Size(44, 27);
            this.btnSet12.TabIndex = 69;
            this.btnSet12.Text = "기타";
            this.btnSet12.UseVisualStyleBackColor = true;
            // 
            // frmSetPrintInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 489);
            this.ControlBox = false;
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlHead);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmSetPrintInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            this.pnlBottom.ResumeLayout(false);
            this.pnlBody.ResumeLayout(false);
            this.pnlBody.PerformLayout();
            this.grpInfo.ResumeLayout(false);
            this.grpInfo.PerformLayout();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).EndInit();
            this.pnlHead.ResumeLayout(false);
            this.pnlHead.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.Panel pnlTop;
        private FarPoint.Win.Spread.FpSpread ssList;
        private FarPoint.Win.Spread.SheetView ssList_Sheet1;
        private System.Windows.Forms.Panel pnlHead;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnPrintTemp;
        private System.Windows.Forms.GroupBox grpInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPage;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtJumin2;
        private System.Windows.Forms.TextBox txtJumin1;
        private System.Windows.Forms.TextBox txtHphone;
        private System.Windows.Forms.ComboBox cboUse;
        private System.Windows.Forms.ComboBox cboGwange;
        private System.Diagnostics.EventLog eventLog1;
        private System.Windows.Forms.Button btnSet22;
        private System.Windows.Forms.Button btnSet21;
        private System.Windows.Forms.Button btnSet20;
        private System.Windows.Forms.Button btnSet12;
        private System.Windows.Forms.Button btnSet10;
        private System.Windows.Forms.Button btnSet11;
    }
}