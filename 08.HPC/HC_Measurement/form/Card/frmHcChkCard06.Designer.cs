namespace HC_Measurement
{
    partial class frmHcChkCard06
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
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer1 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer2 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer3 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer4 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            this.panUCodes = new System.Windows.Forms.Panel();
            this.ssUCD = new FarPoint.Win.Spread.FpSpread();
            this.ssUCD_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.expandableSplitter1 = new DevComponents.DotNetBar.ExpandableSplitter();
            this.ssGONG = new FarPoint.Win.Spread.FpSpread();
            this.ssGONG_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.chkDel = new System.Windows.Forms.CheckBox();
            this.btnAdd_Add = new System.Windows.Forms.Button();
            this.btnAdd_Ins = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panUCodes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssUCD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssUCD_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssGONG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssGONG_Sheet1)).BeginInit();
            this.panel7.SuspendLayout();
            this.panel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // panUCodes
            // 
            this.panUCodes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panUCodes.Controls.Add(this.ssUCD);
            this.panUCodes.Controls.Add(this.expandableSplitter1);
            this.panUCodes.Controls.Add(this.ssGONG);
            this.panUCodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panUCodes.Location = new System.Drawing.Point(0, 53);
            this.panUCodes.Name = "panUCodes";
            this.panUCodes.Size = new System.Drawing.Size(906, 887);
            this.panUCodes.TabIndex = 193;
            // 
            // ssUCD
            // 
            this.ssUCD.AccessibleDescription = "ssUCD, Sheet1, Row 0, Column 0, ";
            this.ssUCD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssUCD.EditModeReplace = true;
            this.ssUCD.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssUCD.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssUCD.HorizontalScrollBar.Renderer = flatScrollBarRenderer1;
            this.ssUCD.HorizontalScrollBar.TabIndex = 1;
            this.ssUCD.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssUCD.Location = new System.Drawing.Point(0, 246);
            this.ssUCD.Name = "ssUCD";
            this.ssUCD.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssUCD_Sheet1});
            this.ssUCD.Size = new System.Drawing.Size(904, 639);
            this.ssUCD.TabIndex = 172;
            this.ssUCD.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssUCD.VerticalScrollBar.Name = "";
            flatScrollBarRenderer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssUCD.VerticalScrollBar.Renderer = flatScrollBarRenderer2;
            this.ssUCD.VerticalScrollBar.TabIndex = 2;
            this.ssUCD.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // ssUCD_Sheet1
            // 
            this.ssUCD_Sheet1.Reset();
            this.ssUCD_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssUCD_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssUCD_Sheet1.ColumnCount = 5;
            this.ssUCD_Sheet1.RowCount = 1;
            this.ssUCD_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssUCD_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssUCD_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.ssUCD_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssUCD_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssUCD_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssUCD_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.ssUCD_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssUCD_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssUCD_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssUCD_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.ssUCD_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssUCD_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssUCD_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssUCD_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.ssUCD_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssUCD_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssUCD_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssUCD_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.ssUCD_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssUCD_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssUCD_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssUCD_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.ssUCD_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssUCD_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.ssUCD_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssUCD_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssUCD_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssUCD_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.ssUCD_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssUCD_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssUCD_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssUCD_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.ssUCD_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssUCD_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.ssUCD_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // expandableSplitter1
            // 
            this.expandableSplitter1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandableSplitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.expandableSplitter1.ExpandableControl = this.ssGONG;
            this.expandableSplitter1.ExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.ExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.GripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.GripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter1.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(151)))), ((int)(((byte)(61)))));
            this.expandableSplitter1.HotBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(184)))), ((int)(((byte)(94)))));
            this.expandableSplitter1.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this.expandableSplitter1.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this.expandableSplitter1.HotExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.HotExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.HotGripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.HotGripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter1.Location = new System.Drawing.Point(0, 236);
            this.expandableSplitter1.Name = "expandableSplitter1";
            this.expandableSplitter1.Size = new System.Drawing.Size(904, 10);
            this.expandableSplitter1.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007;
            this.expandableSplitter1.TabIndex = 171;
            this.expandableSplitter1.TabStop = false;
            // 
            // ssGONG
            // 
            this.ssGONG.AccessibleDescription = "ssGONG, Sheet1, Row 0, Column 0, ";
            this.ssGONG.Dock = System.Windows.Forms.DockStyle.Top;
            this.ssGONG.EditModeReplace = true;
            this.ssGONG.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssGONG.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssGONG.HorizontalScrollBar.Renderer = flatScrollBarRenderer3;
            this.ssGONG.HorizontalScrollBar.TabIndex = 1;
            this.ssGONG.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssGONG.Location = new System.Drawing.Point(0, 0);
            this.ssGONG.Name = "ssGONG";
            this.ssGONG.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssGONG_Sheet1});
            this.ssGONG.Size = new System.Drawing.Size(904, 236);
            this.ssGONG.TabIndex = 170;
            this.ssGONG.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssGONG.VerticalScrollBar.Name = "";
            flatScrollBarRenderer4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssGONG.VerticalScrollBar.Renderer = flatScrollBarRenderer4;
            this.ssGONG.VerticalScrollBar.TabIndex = 2;
            this.ssGONG.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // ssGONG_Sheet1
            // 
            this.ssGONG_Sheet1.Reset();
            this.ssGONG_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssGONG_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssGONG_Sheet1.ColumnCount = 5;
            this.ssGONG_Sheet1.RowCount = 1;
            this.ssGONG_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssGONG_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssGONG_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.ssGONG_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssGONG_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssGONG_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssGONG_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.ssGONG_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssGONG_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssGONG_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssGONG_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.ssGONG_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssGONG_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssGONG_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssGONG_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.ssGONG_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssGONG_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssGONG_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssGONG_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.ssGONG_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssGONG_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssGONG_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssGONG_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.ssGONG_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssGONG_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.ssGONG_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssGONG_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssGONG_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssGONG_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.ssGONG_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssGONG_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssGONG_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssGONG_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.ssGONG_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssGONG_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.ssGONG_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.label3);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.ForeColor = System.Drawing.Color.White;
            this.panel7.Location = new System.Drawing.Point(0, 29);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(906, 24);
            this.panel7.TabIndex = 192;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(250, 22);
            this.label3.TabIndex = 4;
            this.label3.Text = " 작업환경측정 측정결과입력 (소음제외)";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.chkDel);
            this.panel8.Controls.Add(this.btnAdd_Add);
            this.panel8.Controls.Add(this.btnAdd_Ins);
            this.panel8.Controls.Add(this.btnDelete);
            this.panel8.Controls.Add(this.btnSave);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(906, 29);
            this.panel8.TabIndex = 191;
            // 
            // chkDel
            // 
            this.chkDel.AutoSize = true;
            this.chkDel.ForeColor = System.Drawing.Color.DarkRed;
            this.chkDel.Location = new System.Drawing.Point(343, 5);
            this.chkDel.Name = "chkDel";
            this.chkDel.Size = new System.Drawing.Size(79, 21);
            this.chkDel.TabIndex = 32;
            this.chkDel.Text = "삭제포함";
            this.chkDel.UseVisualStyleBackColor = true;
            // 
            // btnAdd_Add
            // 
            this.btnAdd_Add.BackColor = System.Drawing.SystemColors.Control;
            this.btnAdd_Add.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAdd_Add.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAdd_Add.Location = new System.Drawing.Point(570, 0);
            this.btnAdd_Add.Name = "btnAdd_Add";
            this.btnAdd_Add.Size = new System.Drawing.Size(82, 29);
            this.btnAdd_Add.TabIndex = 31;
            this.btnAdd_Add.Text = "추가";
            this.btnAdd_Add.UseVisualStyleBackColor = false;
            // 
            // btnAdd_Ins
            // 
            this.btnAdd_Ins.BackColor = System.Drawing.SystemColors.Control;
            this.btnAdd_Ins.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAdd_Ins.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAdd_Ins.Location = new System.Drawing.Point(652, 0);
            this.btnAdd_Ins.Name = "btnAdd_Ins";
            this.btnAdd_Ins.Size = new System.Drawing.Size(82, 29);
            this.btnAdd_Ins.TabIndex = 30;
            this.btnAdd_Ins.Text = "삽입";
            this.btnAdd_Ins.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.SystemColors.Control;
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDelete.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDelete.ForeColor = System.Drawing.Color.DarkRed;
            this.btnDelete.Location = new System.Drawing.Point(734, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(82, 29);
            this.btnDelete.TabIndex = 29;
            this.btnDelete.Text = "삭제";
            this.btnDelete.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave.Location = new System.Drawing.Point(816, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 29);
            this.btnSave.TabIndex = 25;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // frmHcChkCard06
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(906, 940);
            this.Controls.Add(this.panUCodes);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panel8);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHcChkCard06";
            this.Text = "frmHcChkCard06";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panUCodes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssUCD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssUCD_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssGONG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssGONG_Sheet1)).EndInit();
            this.panel7.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panUCodes;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.CheckBox chkDel;
        private System.Windows.Forms.Button btnAdd_Add;
        private System.Windows.Forms.Button btnAdd_Ins;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private FarPoint.Win.Spread.FpSpread ssUCD;
        private FarPoint.Win.Spread.SheetView ssUCD_Sheet1;
        private DevComponents.DotNetBar.ExpandableSplitter expandableSplitter1;
        private FarPoint.Win.Spread.FpSpread ssGONG;
        private FarPoint.Win.Spread.SheetView ssGONG_Sheet1;
    }
}