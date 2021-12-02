namespace ComPmpaLibB
{
    partial class frmPmpaEntryCaution
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
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType1 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType13 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType14 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType15 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType16 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.pnlBody = new System.Windows.Forms.Panel();
            this.ssList = new FarPoint.Win.Spread.FpSpread();
            this.ssList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.rdoGubun2 = new System.Windows.Forms.RadioButton();
            this.rdoGubun1 = new System.Windows.Forms.RadioButton();
            this.rdoGubun0 = new System.Windows.Forms.RadioButton();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnBuild = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlHead = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            ssList_InputMapWhenFocusedNormal = new FarPoint.Win.Spread.InputMap();
            this.pnlBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.pnlHead.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBody
            // 
            this.pnlBody.BackColor = System.Drawing.SystemColors.Window;
            this.pnlBody.Controls.Add(this.ssList);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(0, 70);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(1173, 551);
            this.pnlBody.TabIndex = 197;
            // 
            // ssList
            // 
            this.ssList.AccessibleDescription = "ssList, Sheet1, Row 0, Column 0, ";
            this.ssList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ssList.FocusRenderer = enhancedFocusIndicatorRenderer1;
            this.ssList.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssList.HorizontalScrollBar.Name = "";
            this.ssList.HorizontalScrollBar.Renderer = enhancedScrollBarRenderer1;
            this.ssList.HorizontalScrollBar.TabIndex = 67;
            this.ssList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssList.Location = new System.Drawing.Point(12, 13);
            this.ssList.Name = "ssList";
            this.ssList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList_Sheet1});
            this.ssList.Size = new System.Drawing.Size(1149, 526);
            this.ssList.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;
            this.ssList.TabIndex = 166;
            this.ssList.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssList.VerticalScrollBar.Name = "";
            this.ssList.VerticalScrollBar.Renderer = enhancedScrollBarRenderer2;
            this.ssList.VerticalScrollBar.TabIndex = 68;
            this.ssList.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            ssList_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Return, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextColumn);
            this.ssList.SetInputMap(FarPoint.Win.Spread.InputMapMode.WhenFocused, FarPoint.Win.Spread.OperationMode.Normal, ssList_InputMapWhenFocusedNormal);
            this.ssList.SetViewportLeftColumn(0, 0, 6);
            this.ssList.SetActiveViewport(0, 0, -1);
            // 
            // ssList_Sheet1
            // 
            this.ssList_Sheet1.Reset();
            this.ssList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssList_Sheet1.ColumnCount = 22;
            this.ssList_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterEnhanced";
            this.ssList_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerEnhanced";
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "선택";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "년도";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "등록번호";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "고객명";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "보험";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "주민등록번호";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "연락처(환자)";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "휴대폰(환자)";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "연락처(보호자)";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "최종내원일";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "총점";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "사용점수";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "주소";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "정보동의";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "구분";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 15).Value = "작업자";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 16).Value = "최초작업일자";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 20).Value = "rowid";
            this.ssList_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderEnhanced";
            this.ssList_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            checkBoxCellType1.BackgroundImage = new FarPoint.Win.Picture(null, FarPoint.Win.RenderStyle.Normal, System.Drawing.Color.Empty, 0, FarPoint.Win.HorizontalAlignment.Center, FarPoint.Win.VerticalAlignment.Center);
            this.ssList_Sheet1.Columns.Get(0).CellType = checkBoxCellType1;
            this.ssList_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(0).Label = "선택";
            this.ssList_Sheet1.Columns.Get(0).Locked = false;
            this.ssList_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(0).Width = 38F;
            this.ssList_Sheet1.Columns.Get(1).CellType = textCellType1;
            this.ssList_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(1).Label = "년도";
            this.ssList_Sheet1.Columns.Get(1).Locked = true;
            this.ssList_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(1).Width = 53F;
            this.ssList_Sheet1.Columns.Get(2).CellType = textCellType2;
            this.ssList_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(2).Label = "등록번호";
            this.ssList_Sheet1.Columns.Get(2).Locked = true;
            this.ssList_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(2).Width = 69F;
            this.ssList_Sheet1.Columns.Get(3).CellType = textCellType3;
            this.ssList_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssList_Sheet1.Columns.Get(3).Label = "고객명";
            this.ssList_Sheet1.Columns.Get(3).Locked = true;
            this.ssList_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(3).Width = 84F;
            this.ssList_Sheet1.Columns.Get(4).CellType = textCellType4;
            this.ssList_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssList_Sheet1.Columns.Get(4).Label = "보험";
            this.ssList_Sheet1.Columns.Get(4).Locked = true;
            this.ssList_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(4).Width = 51F;
            this.ssList_Sheet1.Columns.Get(5).CellType = textCellType5;
            this.ssList_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(5).Label = "주민등록번호";
            this.ssList_Sheet1.Columns.Get(5).Locked = true;
            this.ssList_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(5).Width = 118F;
            this.ssList_Sheet1.Columns.Get(6).CellType = textCellType6;
            this.ssList_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssList_Sheet1.Columns.Get(6).Label = "연락처(환자)";
            this.ssList_Sheet1.Columns.Get(6).Locked = true;
            this.ssList_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(6).Width = 102F;
            this.ssList_Sheet1.Columns.Get(7).CellType = textCellType7;
            this.ssList_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssList_Sheet1.Columns.Get(7).Label = "휴대폰(환자)";
            this.ssList_Sheet1.Columns.Get(7).Locked = true;
            this.ssList_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(7).Width = 102F;
            this.ssList_Sheet1.Columns.Get(8).CellType = textCellType8;
            this.ssList_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssList_Sheet1.Columns.Get(8).Label = "연락처(보호자)";
            this.ssList_Sheet1.Columns.Get(8).Locked = true;
            this.ssList_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(8).Width = 102F;
            this.ssList_Sheet1.Columns.Get(9).CellType = textCellType9;
            this.ssList_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(9).Label = "최종내원일";
            this.ssList_Sheet1.Columns.Get(9).Locked = true;
            this.ssList_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(9).Width = 70F;
            this.ssList_Sheet1.Columns.Get(10).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ssList_Sheet1.Columns.Get(10).CellType = textCellType10;
            this.ssList_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssList_Sheet1.Columns.Get(10).Label = "총점";
            this.ssList_Sheet1.Columns.Get(10).Locked = true;
            this.ssList_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(11).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ssList_Sheet1.Columns.Get(11).CellType = textCellType11;
            this.ssList_Sheet1.Columns.Get(11).Label = "사용점수";
            this.ssList_Sheet1.Columns.Get(11).Locked = true;
            this.ssList_Sheet1.Columns.Get(12).CellType = textCellType12;
            this.ssList_Sheet1.Columns.Get(12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssList_Sheet1.Columns.Get(12).Label = "주소";
            this.ssList_Sheet1.Columns.Get(12).Locked = true;
            this.ssList_Sheet1.Columns.Get(12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(12).Width = 217F;
            this.ssList_Sheet1.Columns.Get(13).CellType = textCellType13;
            this.ssList_Sheet1.Columns.Get(13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssList_Sheet1.Columns.Get(13).Label = "정보동의";
            this.ssList_Sheet1.Columns.Get(13).Locked = true;
            this.ssList_Sheet1.Columns.Get(13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(13).Width = 58F;
            this.ssList_Sheet1.Columns.Get(14).CellType = textCellType14;
            this.ssList_Sheet1.Columns.Get(14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(14).Label = "구분";
            this.ssList_Sheet1.Columns.Get(14).Locked = true;
            this.ssList_Sheet1.Columns.Get(14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(14).Width = 45F;
            this.ssList_Sheet1.Columns.Get(15).CellType = textCellType15;
            this.ssList_Sheet1.Columns.Get(15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssList_Sheet1.Columns.Get(15).Label = "작업자";
            this.ssList_Sheet1.Columns.Get(15).Locked = true;
            this.ssList_Sheet1.Columns.Get(15).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(15).Width = 77F;
            this.ssList_Sheet1.Columns.Get(16).CellType = textCellType16;
            this.ssList_Sheet1.Columns.Get(16).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(16).Label = "최초작업일자";
            this.ssList_Sheet1.Columns.Get(16).Locked = true;
            this.ssList_Sheet1.Columns.Get(16).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList_Sheet1.Columns.Get(16).Width = 80F;
            this.ssList_Sheet1.Columns.Get(17).Visible = false;
            this.ssList_Sheet1.Columns.Get(18).Visible = false;
            this.ssList_Sheet1.Columns.Get(19).Visible = false;
            this.ssList_Sheet1.Columns.Get(20).Label = "rowid";
            this.ssList_Sheet1.Columns.Get(20).Visible = false;
            this.ssList_Sheet1.Columns.Get(21).Visible = false;
            this.ssList_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarEnhanced";
            this.ssList_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderEnhanced";
            this.ssList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.FrozenColumnCount = 6;
            this.ssList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderEnhanced";
            this.ssList_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.SheetCornerStyle.Parent = "CornerEnhanced";
            this.ssList_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.SystemColors.Window;
            this.pnlTop.Controls.Add(this.rdoGubun2);
            this.pnlTop.Controls.Add(this.rdoGubun1);
            this.pnlTop.Controls.Add(this.rdoGubun0);
            this.pnlTop.Controls.Add(this.btnSearch);
            this.pnlTop.Controls.Add(this.btnBuild);
            this.pnlTop.Controls.Add(this.btnDelete);
            this.pnlTop.Controls.Add(this.btnPrint);
            this.pnlTop.Controls.Add(this.btnExit);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 30);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(10);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(4);
            this.pnlTop.Size = new System.Drawing.Size(1173, 40);
            this.pnlTop.TabIndex = 196;
            // 
            // rdoGubun2
            // 
            this.rdoGubun2.AutoSize = true;
            this.rdoGubun2.Location = new System.Drawing.Point(176, 12);
            this.rdoGubun2.Name = "rdoGubun2";
            this.rdoGubun2.Size = new System.Drawing.Size(71, 16);
            this.rdoGubun2.TabIndex = 201;
            this.rdoGubun2.Text = "추천대상";
            this.rdoGubun2.UseVisualStyleBackColor = true;
            // 
            // rdoGubun1
            // 
            this.rdoGubun1.AutoSize = true;
            this.rdoGubun1.Location = new System.Drawing.Point(84, 12);
            this.rdoGubun1.Name = "rdoGubun1";
            this.rdoGubun1.Size = new System.Drawing.Size(71, 16);
            this.rdoGubun1.TabIndex = 200;
            this.rdoGubun1.Text = "기본대상";
            this.rdoGubun1.UseVisualStyleBackColor = true;
            // 
            // rdoGubun0
            // 
            this.rdoGubun0.AutoSize = true;
            this.rdoGubun0.Checked = true;
            this.rdoGubun0.Location = new System.Drawing.Point(23, 12);
            this.rdoGubun0.Name = "rdoGubun0";
            this.rdoGubun0.Size = new System.Drawing.Size(47, 16);
            this.rdoGubun0.TabIndex = 199;
            this.rdoGubun0.TabStop = true;
            this.rdoGubun0.Text = "전체";
            this.rdoGubun0.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.SystemColors.Window;
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Location = new System.Drawing.Point(752, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(83, 32);
            this.btnSearch.TabIndex = 198;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnBuild
            // 
            this.btnBuild.BackColor = System.Drawing.SystemColors.Window;
            this.btnBuild.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnBuild.Location = new System.Drawing.Point(835, 4);
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(83, 32);
            this.btnBuild.TabIndex = 197;
            this.btnBuild.Text = "고객생성";
            this.btnBuild.UseVisualStyleBackColor = true;
            this.btnBuild.Click += new System.EventHandler(this.btnBuild_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.SystemColors.Window;
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDelete.Location = new System.Drawing.Point(918, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(83, 32);
            this.btnDelete.TabIndex = 196;
            this.btnDelete.Text = "삭제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.SystemColors.Window;
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrint.Location = new System.Drawing.Point(1001, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(83, 32);
            this.btnPrint.TabIndex = 193;
            this.btnPrint.Text = "출력";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.SystemColors.Window;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(1084, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(85, 32);
            this.btnExit.TabIndex = 191;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // pnlHead
            // 
            this.pnlHead.BackColor = System.Drawing.Color.RoyalBlue;
            this.pnlHead.Controls.Add(this.label7);
            this.pnlHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHead.ForeColor = System.Drawing.Color.White;
            this.pnlHead.ImeMode = System.Windows.Forms.ImeMode.KatakanaHalf;
            this.pnlHead.Location = new System.Drawing.Point(0, 0);
            this.pnlHead.Name = "pnlHead";
            this.pnlHead.Size = new System.Drawing.Size(1173, 30);
            this.pnlHead.TabIndex = 198;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(14, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "특별고객 명단관리";
            // 
            // frmPmpaEntryCaution
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1173, 621);
            this.ControlBox = false;
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlHead);
            this.Name = "frmPmpaEntryCaution";
            this.Text = " ";
            this.pnlBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlHead.ResumeLayout(false);
            this.pnlHead.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBody;
        private FarPoint.Win.Spread.FpSpread ssList;
        private FarPoint.Win.Spread.SheetView ssList_Sheet1;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnPrint;
        public System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnBuild;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.RadioButton rdoGubun2;
        private System.Windows.Forms.RadioButton rdoGubun1;
        private System.Windows.Forms.RadioButton rdoGubun0;
        private System.Windows.Forms.Panel pnlHead;
        private System.Windows.Forms.Label label7;
    }
}