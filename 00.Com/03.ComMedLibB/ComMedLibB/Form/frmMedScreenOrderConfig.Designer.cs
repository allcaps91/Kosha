namespace ComMedLibB
{
    partial class frmMedScreenOrderConfig
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType3 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType4 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType7 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType8 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType9 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType10 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType11 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType12 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType13 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType14 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType15 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType16 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType17 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType18 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.collapsibleSplitContainer1 = new DevComponents.DotNetBar.Controls.CollapsibleSplitContainer();
            this.ssEnvConfig = new FarPoint.Win.Spread.FpSpread();
            this.ssEnvConfig_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.lblSabun = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.ssEnvExceptDrug = new FarPoint.Win.Spread.FpSpread();
            this.ssEnvExceptDrug_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnExceptDrug = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.collapsibleSplitContainer1)).BeginInit();
            this.collapsibleSplitContainer1.Panel1.SuspendLayout();
            this.collapsibleSplitContainer1.Panel2.SuspendLayout();
            this.collapsibleSplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssEnvConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssEnvConfig_Sheet1)).BeginInit();
            this.panTitleSub0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssEnvExceptDrug)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssEnvExceptDrug_Sheet1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1135, 34);
            this.panTitle.TabIndex = 14;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.Location = new System.Drawing.Point(1040, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(91, 30);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(140, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "SCREEN 환경설정";
            // 
            // collapsibleSplitContainer1
            // 
            this.collapsibleSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.collapsibleSplitContainer1.Location = new System.Drawing.Point(0, 34);
            this.collapsibleSplitContainer1.Name = "collapsibleSplitContainer1";
            this.collapsibleSplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // collapsibleSplitContainer1.Panel1
            // 
            this.collapsibleSplitContainer1.Panel1.Controls.Add(this.ssEnvConfig);
            this.collapsibleSplitContainer1.Panel1.Controls.Add(this.panTitleSub0);
            // 
            // collapsibleSplitContainer1.Panel2
            // 
            this.collapsibleSplitContainer1.Panel2.Controls.Add(this.ssEnvExceptDrug);
            this.collapsibleSplitContainer1.Panel2.Controls.Add(this.panel2);
            this.collapsibleSplitContainer1.Size = new System.Drawing.Size(1135, 579);
            this.collapsibleSplitContainer1.SplitterDistance = 363;
            this.collapsibleSplitContainer1.SplitterWidth = 20;
            this.collapsibleSplitContainer1.TabIndex = 1;
            // 
            // ssEnvConfig
            // 
            this.ssEnvConfig.AccessibleDescription = "ssEnvConfig, Sheet1, Row 0, Column 0, ";
            this.ssEnvConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssEnvConfig.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssEnvConfig.Location = new System.Drawing.Point(0, 29);
            this.ssEnvConfig.Name = "ssEnvConfig";
            this.ssEnvConfig.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssEnvConfig_Sheet1});
            this.ssEnvConfig.Size = new System.Drawing.Size(1135, 334);
            this.ssEnvConfig.TabIndex = 21;
            this.ssEnvConfig.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssEnvConfig.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssEnvConfig_CellClick);
            this.ssEnvConfig.ButtonClicked += new FarPoint.Win.Spread.EditorNotifyEventHandler(this.ssEnvConfig_ButtonClicked);
            // 
            // ssEnvConfig_Sheet1
            // 
            this.ssEnvConfig_Sheet1.Reset();
            this.ssEnvConfig_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssEnvConfig_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssEnvConfig_Sheet1.ColumnCount = 15;
            this.ssEnvConfig_Sheet1.ColumnHeader.RowCount = 2;
            this.ssEnvConfig_Sheet1.RowCount = 1;
            this.ssEnvConfig_Sheet1.Cells.Get(0, 2).Value = false;
            this.ssEnvConfig_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssEnvConfig_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssEnvConfig_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssEnvConfig_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvConfig_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssEnvConfig_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssEnvConfig_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssEnvConfig_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 0).RowSpan = 2;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "MODULEID";
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 1).RowSpan = 2;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "모듈명";
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 2).RowSpan = 2;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "병원필수\r\n적용여부";
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 3).RowSpan = 2;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "ALERT\r\n여부";
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 4).RowSpan = 2;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "INSTCD";
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 5).RowSpan = 2;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "검토등급";
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 6).RowSpan = 2;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "중복허용\r\n일수";
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 7).RowSpan = 2;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "표준체중";
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 8).ColumnSpan = 2;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "가임기연령";
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 10).ColumnSpan = 2;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "용량검토가중치";
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 12).RowSpan = 2;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "MODULEGUBUN";
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 13).RowSpan = 2;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "REFID";
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 14).RowSpan = 2;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "REFID2";
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(1, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(1, 8).Value = "최소(세)";
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(1, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(1, 9).Value = "최대(세)";
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(1, 10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(1, 10).Value = "최소(%)";
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(1, 11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvConfig_Sheet1.ColumnHeader.Cells.Get(1, 11).Value = "최대(%)";
            this.ssEnvConfig_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssEnvConfig_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssEnvConfig_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssEnvConfig_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvConfig_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssEnvConfig_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvConfig_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvConfig_Sheet1.Columns.Get(0).Width = 82F;
            textCellType10.BackgroundImage = new FarPoint.Win.Picture(null, FarPoint.Win.RenderStyle.Normal, System.Drawing.Color.Empty, 0, FarPoint.Win.HorizontalAlignment.Center, FarPoint.Win.VerticalAlignment.Center);
            textCellType10.ReadOnly = true;
            this.ssEnvConfig_Sheet1.Columns.Get(1).CellType = textCellType10;
            this.ssEnvConfig_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssEnvConfig_Sheet1.Columns.Get(1).Locked = true;
            this.ssEnvConfig_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvConfig_Sheet1.Columns.Get(1).Width = 212F;
            checkBoxCellType3.BackgroundImage = new FarPoint.Win.Picture(null, FarPoint.Win.RenderStyle.Normal, System.Drawing.Color.Empty, 0, FarPoint.Win.HorizontalAlignment.Center, FarPoint.Win.VerticalAlignment.Center);
            this.ssEnvConfig_Sheet1.Columns.Get(2).CellType = checkBoxCellType3;
            this.ssEnvConfig_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvConfig_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvConfig_Sheet1.Columns.Get(2).Width = 74F;
            checkBoxCellType4.BackgroundImage = new FarPoint.Win.Picture(null, FarPoint.Win.RenderStyle.Normal, System.Drawing.Color.Empty, 0, FarPoint.Win.HorizontalAlignment.Center, FarPoint.Win.VerticalAlignment.Center);
            this.ssEnvConfig_Sheet1.Columns.Get(3).CellType = checkBoxCellType4;
            this.ssEnvConfig_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvConfig_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvConfig_Sheet1.Columns.Get(3).Width = 71F;
            this.ssEnvConfig_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvConfig_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvConfig_Sheet1.Columns.Get(5).CellType = textCellType11;
            this.ssEnvConfig_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvConfig_Sheet1.Columns.Get(5).Locked = true;
            this.ssEnvConfig_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvConfig_Sheet1.Columns.Get(5).Width = 85F;
            numberCellType7.DecimalPlaces = 0;
            numberCellType7.FixedPoint = false;
            numberCellType7.MaximumValue = 10000000D;
            numberCellType7.MinimumValue = -10000000D;
            this.ssEnvConfig_Sheet1.Columns.Get(6).CellType = numberCellType7;
            this.ssEnvConfig_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvConfig_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvConfig_Sheet1.Columns.Get(6).Width = 85F;
            numberCellType8.DecimalPlaces = 0;
            numberCellType8.FixedPoint = false;
            numberCellType8.MaximumValue = 10000000D;
            numberCellType8.MinimumValue = -10000000D;
            this.ssEnvConfig_Sheet1.Columns.Get(7).CellType = numberCellType8;
            this.ssEnvConfig_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvConfig_Sheet1.Columns.Get(7).Locked = true;
            this.ssEnvConfig_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvConfig_Sheet1.Columns.Get(7).Width = 85F;
            numberCellType9.DecimalPlaces = 0;
            numberCellType9.FixedPoint = false;
            numberCellType9.MaximumValue = 10000000D;
            numberCellType9.MinimumValue = -10000000D;
            this.ssEnvConfig_Sheet1.Columns.Get(8).CellType = numberCellType9;
            this.ssEnvConfig_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvConfig_Sheet1.Columns.Get(8).Label = "최소(세)";
            this.ssEnvConfig_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvConfig_Sheet1.Columns.Get(8).Width = 85F;
            numberCellType10.DecimalPlaces = 0;
            numberCellType10.FixedPoint = false;
            numberCellType10.MaximumValue = 10000000D;
            numberCellType10.MinimumValue = -10000000D;
            this.ssEnvConfig_Sheet1.Columns.Get(9).CellType = numberCellType10;
            this.ssEnvConfig_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvConfig_Sheet1.Columns.Get(9).Label = "최대(세)";
            this.ssEnvConfig_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvConfig_Sheet1.Columns.Get(9).Width = 85F;
            numberCellType11.DecimalPlaces = 0;
            numberCellType11.FixedPoint = false;
            numberCellType11.MaximumValue = 10000000D;
            numberCellType11.MinimumValue = -10000000D;
            this.ssEnvConfig_Sheet1.Columns.Get(10).CellType = numberCellType11;
            this.ssEnvConfig_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvConfig_Sheet1.Columns.Get(10).Label = "최소(%)";
            this.ssEnvConfig_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvConfig_Sheet1.Columns.Get(10).Width = 85F;
            numberCellType12.DecimalPlaces = 0;
            numberCellType12.FixedPoint = false;
            numberCellType12.MaximumValue = 10000000D;
            numberCellType12.MinimumValue = -10000000D;
            this.ssEnvConfig_Sheet1.Columns.Get(11).CellType = numberCellType12;
            this.ssEnvConfig_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvConfig_Sheet1.Columns.Get(11).Label = "최대(%)";
            this.ssEnvConfig_Sheet1.Columns.Get(11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvConfig_Sheet1.Columns.Get(11).Width = 85F;
            this.ssEnvConfig_Sheet1.Columns.Get(12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvConfig_Sheet1.Columns.Get(12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvConfig_Sheet1.Columns.Get(12).Width = 117F;
            this.ssEnvConfig_Sheet1.Columns.Get(13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvConfig_Sheet1.Columns.Get(13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvConfig_Sheet1.Columns.Get(14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvConfig_Sheet1.Columns.Get(14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvConfig_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssEnvConfig_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssEnvConfig_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.ssEnvConfig_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvConfig_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssEnvConfig_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssEnvConfig_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.ssEnvConfig_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvConfig_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssEnvConfig_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssEnvConfig_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssEnvConfig_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvConfig_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssEnvConfig_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssEnvConfig_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssEnvConfig_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssEnvConfig_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvConfig_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssEnvConfig_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssEnvConfig_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssEnvConfig_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvConfig_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.label2);
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Controls.Add(this.lblSabun);
            this.panTitleSub0.Controls.Add(this.btnSave);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 0);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(1135, 29);
            this.panTitleSub0.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(827, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 25);
            this.label2.TabIndex = 8;
            this.label2.Text = "유저사번:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 4);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(43, 15);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "설정값";
            // 
            // lblSabun
            // 
            this.lblSabun.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblSabun.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSabun.ForeColor = System.Drawing.Color.White;
            this.lblSabun.Location = new System.Drawing.Point(885, 0);
            this.lblSabun.Name = "lblSabun";
            this.lblSabun.Size = new System.Drawing.Size(118, 25);
            this.lblSabun.TabIndex = 9;
            this.lblSabun.Text = "공백";
            this.lblSabun.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(1003, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(128, 25);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "환경설정값 저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ssEnvExceptDrug
            // 
            this.ssEnvExceptDrug.AccessibleDescription = "fpSpread2, Sheet1, Row 0, Column 0, ";
            this.ssEnvExceptDrug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssEnvExceptDrug.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssEnvExceptDrug.Location = new System.Drawing.Point(0, 31);
            this.ssEnvExceptDrug.Name = "ssEnvExceptDrug";
            this.ssEnvExceptDrug.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssEnvExceptDrug_Sheet1});
            this.ssEnvExceptDrug.Size = new System.Drawing.Size(1135, 165);
            this.ssEnvExceptDrug.TabIndex = 23;
            this.ssEnvExceptDrug.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // ssEnvExceptDrug_Sheet1
            // 
            this.ssEnvExceptDrug_Sheet1.Reset();
            this.ssEnvExceptDrug_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssEnvExceptDrug_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssEnvExceptDrug_Sheet1.ColumnCount = 7;
            this.ssEnvExceptDrug_Sheet1.RowCount = 1;
            this.ssEnvExceptDrug_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssEnvExceptDrug_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssEnvExceptDrug_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssEnvExceptDrug_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvExceptDrug_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssEnvExceptDrug_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssEnvExceptDrug_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssEnvExceptDrug_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "ModuleID";
            this.ssEnvExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "상태";
            this.ssEnvExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssEnvExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "모듈명";
            this.ssEnvExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssEnvExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "약품명";
            this.ssEnvExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssEnvExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "약품코드";
            this.ssEnvExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssEnvExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "성분명";
            this.ssEnvExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssEnvExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnvExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "등록일자";
            this.ssEnvExceptDrug_Sheet1.ColumnHeader.Cells.Get(0, 6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssEnvExceptDrug_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssEnvExceptDrug_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssEnvExceptDrug_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssEnvExceptDrug_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvExceptDrug_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssEnvExceptDrug_Sheet1.Columns.Get(0).CellType = textCellType12;
            this.ssEnvExceptDrug_Sheet1.Columns.Get(0).Label = "ModuleID";
            this.ssEnvExceptDrug_Sheet1.Columns.Get(0).Locked = true;
            this.ssEnvExceptDrug_Sheet1.Columns.Get(0).Width = 77F;
            this.ssEnvExceptDrug_Sheet1.Columns.Get(1).CellType = textCellType13;
            this.ssEnvExceptDrug_Sheet1.Columns.Get(1).Label = "상태";
            this.ssEnvExceptDrug_Sheet1.Columns.Get(1).Locked = true;
            this.ssEnvExceptDrug_Sheet1.Columns.Get(1).Width = 82F;
            this.ssEnvExceptDrug_Sheet1.Columns.Get(2).CellType = textCellType14;
            this.ssEnvExceptDrug_Sheet1.Columns.Get(2).Label = "모듈명";
            this.ssEnvExceptDrug_Sheet1.Columns.Get(2).Locked = true;
            this.ssEnvExceptDrug_Sheet1.Columns.Get(2).Width = 152F;
            this.ssEnvExceptDrug_Sheet1.Columns.Get(3).CellType = textCellType15;
            this.ssEnvExceptDrug_Sheet1.Columns.Get(3).Label = "약품명";
            this.ssEnvExceptDrug_Sheet1.Columns.Get(3).Locked = true;
            this.ssEnvExceptDrug_Sheet1.Columns.Get(3).Width = 231F;
            this.ssEnvExceptDrug_Sheet1.Columns.Get(4).CellType = textCellType16;
            this.ssEnvExceptDrug_Sheet1.Columns.Get(4).Label = "약품코드";
            this.ssEnvExceptDrug_Sheet1.Columns.Get(4).Locked = true;
            this.ssEnvExceptDrug_Sheet1.Columns.Get(4).Width = 102F;
            this.ssEnvExceptDrug_Sheet1.Columns.Get(5).CellType = textCellType17;
            this.ssEnvExceptDrug_Sheet1.Columns.Get(5).Label = "성분명";
            this.ssEnvExceptDrug_Sheet1.Columns.Get(5).Locked = true;
            this.ssEnvExceptDrug_Sheet1.Columns.Get(5).Width = 228F;
            this.ssEnvExceptDrug_Sheet1.Columns.Get(6).CellType = textCellType18;
            this.ssEnvExceptDrug_Sheet1.Columns.Get(6).Label = "등록일자";
            this.ssEnvExceptDrug_Sheet1.Columns.Get(6).Locked = true;
            this.ssEnvExceptDrug_Sheet1.Columns.Get(6).Width = 191F;
            this.ssEnvExceptDrug_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssEnvExceptDrug_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssEnvExceptDrug_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.ssEnvExceptDrug_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvExceptDrug_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssEnvExceptDrug_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssEnvExceptDrug_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.ssEnvExceptDrug_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvExceptDrug_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssEnvExceptDrug_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssEnvExceptDrug_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssEnvExceptDrug_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvExceptDrug_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssEnvExceptDrug_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssEnvExceptDrug_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssEnvExceptDrug_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssEnvExceptDrug_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvExceptDrug_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssEnvExceptDrug_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssEnvExceptDrug_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssEnvExceptDrug_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssEnvExceptDrug_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.btnExceptDrug);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1135, 31);
            this.panel2.TabIndex = 22;
            // 
            // btnExceptDrug
            // 
            this.btnExceptDrug.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExceptDrug.Location = new System.Drawing.Point(1003, 0);
            this.btnExceptDrug.Name = "btnExceptDrug";
            this.btnExceptDrug.Size = new System.Drawing.Size(128, 27);
            this.btnExceptDrug.TabIndex = 6;
            this.btnExceptDrug.Text = "제외약품 변경";
            this.btnExceptDrug.UseVisualStyleBackColor = true;
            this.btnExceptDrug.Click += new System.EventHandler(this.btnExceptDrug_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(8, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(171, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "모듈별 제외약품 (모듈명 클릭)";
            // 
            // frmMedScreenOrderConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1135, 613);
            this.Controls.Add(this.collapsibleSplitContainer1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmMedScreenOrderConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmMedScreenOrderConfig";
            this.Load += new System.EventHandler(this.frmMedScreenOrderConfig_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.collapsibleSplitContainer1.Panel1.ResumeLayout(false);
            this.collapsibleSplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.collapsibleSplitContainer1)).EndInit();
            this.collapsibleSplitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssEnvConfig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssEnvConfig_Sheet1)).EndInit();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssEnvExceptDrug)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssEnvExceptDrug_Sheet1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private DevComponents.DotNetBar.Controls.CollapsibleSplitContainer collapsibleSplitContainer1;
        private FarPoint.Win.Spread.FpSpread ssEnvConfig;
        private FarPoint.Win.Spread.SheetView ssEnvConfig_Sheet1;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Label lblSabun;
        private System.Windows.Forms.Button btnSave;
        private FarPoint.Win.Spread.FpSpread ssEnvExceptDrug;
        private FarPoint.Win.Spread.SheetView ssEnvExceptDrug_Sheet1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnExceptDrug;
        private System.Windows.Forms.Label label1;
    }
}