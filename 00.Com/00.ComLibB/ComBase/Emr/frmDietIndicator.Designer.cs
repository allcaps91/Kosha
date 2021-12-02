namespace ComBase
{
    partial class frmDietIndicator
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("BorderEx420636615805998415704", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder1 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text764636615805998571944", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder2 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Font965636615805998571944");
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.Spread.InputMap ssView_InputMapWhenFocusedNormal;
            FarPoint.Win.Spread.InputMap ssView_InputMapWhenFocusedReadOnly;
            FarPoint.Win.Spread.InputMap ssView_InputMapWhenFocusedRowMode;
            FarPoint.Win.Spread.InputMap ssView_InputMapWhenAncestorOfFocusedNormal;
            FarPoint.Win.Spread.InputMap ssView_InputMapWhenAncestorOfFocusedReadOnly;
            FarPoint.Win.Spread.InputMap ssView_InputMapWhenAncestorOfFocusedRowMode;
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            ssView_InputMapWhenFocusedNormal = new FarPoint.Win.Spread.InputMap();
            ssView_InputMapWhenFocusedNormal.Parent = new FarPoint.Win.Spread.InputMap();
            ssView_InputMapWhenFocusedReadOnly = new FarPoint.Win.Spread.InputMap();
            ssView_InputMapWhenFocusedReadOnly.Parent = new FarPoint.Win.Spread.InputMap();
            ssView_InputMapWhenFocusedRowMode = new FarPoint.Win.Spread.InputMap();
            ssView_InputMapWhenFocusedRowMode.Parent = new FarPoint.Win.Spread.InputMap();
            ssView_InputMapWhenAncestorOfFocusedNormal = new FarPoint.Win.Spread.InputMap();
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent = new FarPoint.Win.Spread.InputMap();
            ssView_InputMapWhenAncestorOfFocusedReadOnly = new FarPoint.Win.Spread.InputMap();
            ssView_InputMapWhenAncestorOfFocusedReadOnly.Parent = new FarPoint.Win.Spread.InputMap();
            ssView_InputMapWhenAncestorOfFocusedRowMode = new FarPoint.Win.Spread.InputMap();
            ssView_InputMapWhenAncestorOfFocusedRowMode.Parent = new FarPoint.Win.Spread.InputMap();
            this.panTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
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
            this.panTitle.Size = new System.Drawing.Size(632, 34);
            this.panTitle.TabIndex = 12;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(231, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "영양초기평가/재평가 기본지표";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(553, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(632, 330);
            this.panel1.TabIndex = 13;
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.EditModePermanent = true;
            this.ssView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssView.Location = new System.Drawing.Point(0, 34);
            this.ssView.Name = "ssView";
            namedStyle1.Border = complexBorder1;
            namedStyle1.Font = new System.Drawing.Font("맑은 고딕", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle2.Border = complexBorder2;
            textCellType1.MaxLength = 32000;
            textCellType1.Multiline = true;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("맑은 고딕", 9F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle3.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3});
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(632, 330);
            this.ssView.TabIndex = 47;
            this.ssView.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssView.TextTipAppearance = tipAppearance1;
            this.ssView.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            ssView_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Return, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.StartEditing);
            ssView_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Tab, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.None);
            ssView_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Tab, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            ssView_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Left, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.None);
            ssView_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Right, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.None);
            ssView_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Left, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            ssView_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Right, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            ssView_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Left, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            ssView_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Right, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            ssView_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Left, ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            ssView_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Right, ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            ssView_InputMapWhenFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Back, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.StartEditing);
            ssView_InputMapWhenFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Return, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.StartEditing);
            ssView_InputMapWhenFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke('='), FarPoint.Win.Spread.SpreadActions.StartEditingFormula);
            ssView_InputMapWhenFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.C, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardCopy);
            ssView_InputMapWhenFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.V, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardPasteAll);
            ssView_InputMapWhenFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.X, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardCut);
            ssView_InputMapWhenFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Insert, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardCopy);
            ssView_InputMapWhenFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Delete, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardCut);
            ssView_InputMapWhenFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Insert, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardPasteAll);
            ssView_InputMapWhenFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.F4, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.ShowSubEditor);
            ssView_InputMapWhenFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Space, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.SelectRow);
            ssView_InputMapWhenFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Z, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.Undo);
            ssView_InputMapWhenFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Y, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.Redo);
            this.ssView.SetInputMap(FarPoint.Win.Spread.InputMapMode.WhenFocused, FarPoint.Win.Spread.OperationMode.Normal, ssView_InputMapWhenFocusedNormal);
            ssView_InputMapWhenFocusedReadOnly.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Tab, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.None);
            ssView_InputMapWhenFocusedReadOnly.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Tab, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            ssView_InputMapWhenFocusedReadOnly.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.C, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardCopy);
            ssView_InputMapWhenFocusedReadOnly.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Insert, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardCopy);
            ssView_InputMapWhenFocusedReadOnly.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Z, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.Undo);
            ssView_InputMapWhenFocusedReadOnly.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Y, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.Redo);
            this.ssView.SetInputMap(FarPoint.Win.Spread.InputMapMode.WhenFocused, FarPoint.Win.Spread.OperationMode.ReadOnly, ssView_InputMapWhenFocusedReadOnly);
            ssView_InputMapWhenFocusedRowMode.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Return, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.StartEditing);
            ssView_InputMapWhenFocusedRowMode.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Left, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.None);
            ssView_InputMapWhenFocusedRowMode.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Right, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.None);
            ssView_InputMapWhenFocusedRowMode.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Left, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            ssView_InputMapWhenFocusedRowMode.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Right, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            ssView_InputMapWhenFocusedRowMode.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Left, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            ssView_InputMapWhenFocusedRowMode.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Right, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            ssView_InputMapWhenFocusedRowMode.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Left, ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            ssView_InputMapWhenFocusedRowMode.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Right, ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            ssView_InputMapWhenFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Return, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.StartEditing);
            ssView_InputMapWhenFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke('='), FarPoint.Win.Spread.SpreadActions.StartEditingFormula);
            ssView_InputMapWhenFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Down, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ComboShowList);
            ssView_InputMapWhenFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Up, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ComboShowList);
            ssView_InputMapWhenFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.F4, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.ShowSubEditor);
            ssView_InputMapWhenFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Z, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.Undo);
            ssView_InputMapWhenFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Y, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.Redo);
            this.ssView.SetInputMap(FarPoint.Win.Spread.InputMapMode.WhenFocused, FarPoint.Win.Spread.OperationMode.RowMode, ssView_InputMapWhenFocusedRowMode);
            ssView_InputMapWhenAncestorOfFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Return, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.StopEditing);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Up, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToPreviousRow);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Down, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextRow);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Left, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToPreviousColumnVisual);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Right, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextColumnVisual);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Up, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ExtendToPreviousRow);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Down, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ExtendToNextRow);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Left, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ExtendToPreviousColumnVisual);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Right, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ExtendToNextColumnVisual);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Up, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.MoveToPreviousRow);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Down, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.MoveToNextRow);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Left, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.MoveToPreviousColumnVisual);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Right, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.MoveToNextColumnVisual);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Up, ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ExtendToPreviousRow);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Down, ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ExtendToNextRow);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Left, ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ExtendToPreviousColumnVisual);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Right, ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ExtendToNextColumnVisual);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.PageUp, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToPreviousPageOfRows);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Next, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextPageOfRows);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.PageUp, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.MoveToPreviousPageOfColumns);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Next, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.MoveToNextPageOfColumns);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.PageUp, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ExtendToPreviousPageOfRows);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Next, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ExtendToNextPageOfRows);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.PageUp, ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ExtendToPreviousPageOfColumns);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Next, ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ExtendToNextPageOfColumns);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Home, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToFirstColumn);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.End, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToLastColumn);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Home, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.MoveToFirstCell);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.End, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.MoveToLastCell);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Home, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ExtendToFirstColumn);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.End, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ExtendToLastColumn);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Home, ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ExtendToFirstCell);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.End, ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ExtendToLastCell);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Space, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.SelectColumn);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Space, ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                    | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.SelectSheet);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Escape, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.CancelEditing);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Return, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.StopEditing);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Tab, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextColumnWrap);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Tab, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.MoveToPreviousColumnWrap);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.F2, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.ClearCell);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.F3, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.DateTimeNow);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.F4, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.ShowSubEditor);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Down, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ComboShowList);
            ssView_InputMapWhenAncestorOfFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Up, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ComboShowList);
            this.ssView.SetInputMap(FarPoint.Win.Spread.InputMapMode.WhenAncestorOfFocused, FarPoint.Win.Spread.OperationMode.Normal, ssView_InputMapWhenAncestorOfFocusedNormal);
            ssView_InputMapWhenAncestorOfFocusedReadOnly.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Tab, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.None);
            ssView_InputMapWhenAncestorOfFocusedReadOnly.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Tab, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.None);
            ssView_InputMapWhenAncestorOfFocusedReadOnly.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Up, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.ScrollToPreviousRow);
            ssView_InputMapWhenAncestorOfFocusedReadOnly.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Down, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.ScrollToNextRow);
            ssView_InputMapWhenAncestorOfFocusedReadOnly.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Left, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.ScrollToPreviousColumnVisual);
            ssView_InputMapWhenAncestorOfFocusedReadOnly.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Right, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.ScrollToNextColumnVisual);
            ssView_InputMapWhenAncestorOfFocusedReadOnly.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.PageUp, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.ScrollToPreviousPageOfRows);
            ssView_InputMapWhenAncestorOfFocusedReadOnly.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Next, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.ScrollToNextPageOfRows);
            ssView_InputMapWhenAncestorOfFocusedReadOnly.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.PageUp, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ScrollToPreviousPageOfColumns);
            ssView_InputMapWhenAncestorOfFocusedReadOnly.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Next, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ScrollToNextPageOfColumns);
            ssView_InputMapWhenAncestorOfFocusedReadOnly.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Home, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.ScrollToFirstColumn);
            ssView_InputMapWhenAncestorOfFocusedReadOnly.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.End, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.ScrollToLastColumn);
            ssView_InputMapWhenAncestorOfFocusedReadOnly.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Home, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ScrollToFirstCell);
            ssView_InputMapWhenAncestorOfFocusedReadOnly.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.End, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ScrollToLastCell);
            this.ssView.SetInputMap(FarPoint.Win.Spread.InputMapMode.WhenAncestorOfFocused, FarPoint.Win.Spread.OperationMode.ReadOnly, ssView_InputMapWhenAncestorOfFocusedReadOnly);
            ssView_InputMapWhenAncestorOfFocusedRowMode.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Return, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.StopEditing);
            ssView_InputMapWhenAncestorOfFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Up, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToPreviousRow);
            ssView_InputMapWhenAncestorOfFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Down, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextRow);
            ssView_InputMapWhenAncestorOfFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Left, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToPreviousColumnVisual);
            ssView_InputMapWhenAncestorOfFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Right, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextColumnVisual);
            ssView_InputMapWhenAncestorOfFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Up, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.MoveToPreviousRow);
            ssView_InputMapWhenAncestorOfFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Down, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.MoveToNextRow);
            ssView_InputMapWhenAncestorOfFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Left, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.MoveToPreviousColumnVisual);
            ssView_InputMapWhenAncestorOfFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Right, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.MoveToNextColumnVisual);
            ssView_InputMapWhenAncestorOfFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.PageUp, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToPreviousPageOfRows);
            ssView_InputMapWhenAncestorOfFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Next, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextPageOfRows);
            ssView_InputMapWhenAncestorOfFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.PageUp, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.MoveToPreviousPageOfColumns);
            ssView_InputMapWhenAncestorOfFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Next, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.MoveToNextPageOfColumns);
            ssView_InputMapWhenAncestorOfFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Home, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToFirstColumn);
            ssView_InputMapWhenAncestorOfFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.End, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToLastColumn);
            ssView_InputMapWhenAncestorOfFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Home, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.MoveToFirstCell);
            ssView_InputMapWhenAncestorOfFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.End, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.MoveToLastCell);
            ssView_InputMapWhenAncestorOfFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Escape, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.CancelEditing);
            ssView_InputMapWhenAncestorOfFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Return, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.StopEditing);
            ssView_InputMapWhenAncestorOfFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Tab, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.MoveToNextColumnWrap);
            ssView_InputMapWhenAncestorOfFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Tab, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.MoveToPreviousColumnWrap);
            ssView_InputMapWhenAncestorOfFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.F2, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.ClearCell);
            ssView_InputMapWhenAncestorOfFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.F3, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.DateTimeNow);
            ssView_InputMapWhenAncestorOfFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.F4, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.ShowSubEditor);
            ssView_InputMapWhenAncestorOfFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Down, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ComboShowList);
            ssView_InputMapWhenAncestorOfFocusedRowMode.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Up, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ComboShowList);
            this.ssView.SetInputMap(FarPoint.Win.Spread.InputMapMode.WhenAncestorOfFocused, FarPoint.Win.Spread.OperationMode.RowMode, ssView_InputMapWhenAncestorOfFocusedRowMode);
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 9;
            this.ssView_Sheet1.RowCount = 10;
            this.ssView_Sheet1.Cells.Get(0, 0).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssView_Sheet1.Cells.Get(0, 0).Locked = true;
            this.ssView_Sheet1.Cells.Get(0, 0).Value = "구분";
            this.ssView_Sheet1.Cells.Get(0, 1).ColumnSpan = 2;
            this.ssView_Sheet1.Cells.Get(0, 1).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssView_Sheet1.Cells.Get(0, 1).Locked = true;
            this.ssView_Sheet1.Cells.Get(0, 1).Value = "지표";
            this.ssView_Sheet1.Cells.Get(0, 2).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssView_Sheet1.Cells.Get(0, 2).Locked = true;
            this.ssView_Sheet1.Cells.Get(0, 3).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssView_Sheet1.Cells.Get(0, 3).Locked = true;
            this.ssView_Sheet1.Cells.Get(0, 3).Value = "1점";
            this.ssView_Sheet1.Cells.Get(0, 4).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssView_Sheet1.Cells.Get(0, 4).Locked = true;
            this.ssView_Sheet1.Cells.Get(0, 4).Value = "2점";
            this.ssView_Sheet1.Cells.Get(0, 5).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssView_Sheet1.Cells.Get(0, 5).Locked = true;
            this.ssView_Sheet1.Cells.Get(0, 5).Value = "3점";
            this.ssView_Sheet1.Cells.Get(0, 6).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssView_Sheet1.Cells.Get(0, 6).Locked = true;
            this.ssView_Sheet1.Cells.Get(0, 6).Value = "양호군";
            this.ssView_Sheet1.Cells.Get(0, 7).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssView_Sheet1.Cells.Get(0, 7).Locked = true;
            this.ssView_Sheet1.Cells.Get(0, 7).Value = "중위험군";
            this.ssView_Sheet1.Cells.Get(0, 8).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssView_Sheet1.Cells.Get(0, 8).Locked = true;
            this.ssView_Sheet1.Cells.Get(0, 8).Value = "고위험군";
            this.ssView_Sheet1.Cells.Get(1, 0).Locked = true;
            this.ssView_Sheet1.Cells.Get(1, 0).RowSpan = 9;
            this.ssView_Sheet1.Cells.Get(1, 0).Value = "기본 지표\r\n(초기평가,\r\n재평가)";
            this.ssView_Sheet1.Cells.Get(1, 1).ColumnSpan = 2;
            this.ssView_Sheet1.Cells.Get(1, 1).Locked = true;
            this.ssView_Sheet1.Cells.Get(1, 1).Value = "PIBW";
            this.ssView_Sheet1.Cells.Get(1, 2).Locked = true;
            this.ssView_Sheet1.Cells.Get(1, 3).Locked = true;
            this.ssView_Sheet1.Cells.Get(1, 3).Value = "80~89%";
            this.ssView_Sheet1.Cells.Get(1, 4).Locked = true;
            this.ssView_Sheet1.Cells.Get(1, 4).Value = "70~79%";
            this.ssView_Sheet1.Cells.Get(1, 5).Locked = true;
            this.ssView_Sheet1.Cells.Get(1, 5).Value = "<70%";
            this.ssView_Sheet1.Cells.Get(1, 6).Locked = true;
            this.ssView_Sheet1.Cells.Get(1, 6).RowSpan = 9;
            this.ssView_Sheet1.Cells.Get(1, 6).Value = "0~7점에\r\n해당하는\r\n환자";
            this.ssView_Sheet1.Cells.Get(1, 7).Locked = true;
            this.ssView_Sheet1.Cells.Get(1, 7).RowSpan = 9;
            this.ssView_Sheet1.Cells.Get(1, 7).Value = "8~14점\r\n에\r\n해당하는\r\n환자,\r\n점수와\r\n상관없이\r\nTube\r\nFeeding\r\n중인\r\n환자";
            this.ssView_Sheet1.Cells.Get(1, 8).Locked = true;
            this.ssView_Sheet1.Cells.Get(1, 8).RowSpan = 9;
            this.ssView_Sheet1.Cells.Get(1, 8).Value = "15점\r\n이상에\r\n해당하는\r\n환자,\r\n점수와\r\n상관없이\r\nTPN\r\n중인\r\n환자";
            this.ssView_Sheet1.Cells.Get(2, 0).Locked = true;
            this.ssView_Sheet1.Cells.Get(2, 1).ColumnSpan = 2;
            this.ssView_Sheet1.Cells.Get(2, 1).Locked = true;
            this.ssView_Sheet1.Cells.Get(2, 1).Value = "Albumin";
            this.ssView_Sheet1.Cells.Get(2, 2).Locked = true;
            this.ssView_Sheet1.Cells.Get(2, 3).Locked = true;
            this.ssView_Sheet1.Cells.Get(2, 3).Value = "2.8~3.0";
            this.ssView_Sheet1.Cells.Get(2, 4).Locked = true;
            this.ssView_Sheet1.Cells.Get(2, 4).Value = "2.1~2.7";
            this.ssView_Sheet1.Cells.Get(2, 5).Locked = true;
            this.ssView_Sheet1.Cells.Get(2, 5).Value = "<2.1";
            this.ssView_Sheet1.Cells.Get(2, 6).Locked = true;
            this.ssView_Sheet1.Cells.Get(2, 7).Locked = true;
            this.ssView_Sheet1.Cells.Get(2, 8).Locked = true;
            this.ssView_Sheet1.Cells.Get(3, 0).Locked = true;
            this.ssView_Sheet1.Cells.Get(3, 1).Locked = true;
            this.ssView_Sheet1.Cells.Get(3, 1).RowSpan = 2;
            this.ssView_Sheet1.Cells.Get(3, 1).Value = "Hb";
            this.ssView_Sheet1.Cells.Get(3, 2).Locked = true;
            this.ssView_Sheet1.Cells.Get(3, 2).Value = "여자";
            this.ssView_Sheet1.Cells.Get(3, 3).Locked = true;
            this.ssView_Sheet1.Cells.Get(3, 3).Value = "10~11.9";
            this.ssView_Sheet1.Cells.Get(3, 4).Locked = true;
            this.ssView_Sheet1.Cells.Get(3, 4).Value = "8~9.9";
            this.ssView_Sheet1.Cells.Get(3, 5).Locked = true;
            this.ssView_Sheet1.Cells.Get(3, 5).Value = "<8";
            this.ssView_Sheet1.Cells.Get(3, 6).Locked = true;
            this.ssView_Sheet1.Cells.Get(3, 7).Locked = true;
            this.ssView_Sheet1.Cells.Get(3, 8).Locked = true;
            this.ssView_Sheet1.Cells.Get(4, 0).Locked = true;
            this.ssView_Sheet1.Cells.Get(4, 1).Locked = true;
            this.ssView_Sheet1.Cells.Get(4, 2).Locked = true;
            this.ssView_Sheet1.Cells.Get(4, 2).Value = "남자";
            this.ssView_Sheet1.Cells.Get(4, 3).Locked = true;
            this.ssView_Sheet1.Cells.Get(4, 3).Value = "13.9~12";
            this.ssView_Sheet1.Cells.Get(4, 4).Locked = true;
            this.ssView_Sheet1.Cells.Get(4, 4).Value = "9~11.9";
            this.ssView_Sheet1.Cells.Get(4, 5).Locked = true;
            this.ssView_Sheet1.Cells.Get(4, 5).Value = "<9";
            this.ssView_Sheet1.Cells.Get(4, 6).Locked = true;
            this.ssView_Sheet1.Cells.Get(4, 7).Locked = true;
            this.ssView_Sheet1.Cells.Get(4, 8).Locked = true;
            this.ssView_Sheet1.Cells.Get(5, 0).Locked = true;
            this.ssView_Sheet1.Cells.Get(5, 1).ColumnSpan = 2;
            this.ssView_Sheet1.Cells.Get(5, 1).Locked = true;
            this.ssView_Sheet1.Cells.Get(5, 1).Value = "TLC";
            this.ssView_Sheet1.Cells.Get(5, 2).Locked = true;
            this.ssView_Sheet1.Cells.Get(5, 3).Locked = true;
            this.ssView_Sheet1.Cells.Get(5, 3).Value = "1200~1500";
            this.ssView_Sheet1.Cells.Get(5, 4).Locked = true;
            this.ssView_Sheet1.Cells.Get(5, 4).Value = "800~1200";
            this.ssView_Sheet1.Cells.Get(5, 5).Locked = true;
            this.ssView_Sheet1.Cells.Get(5, 5).Value = "<800";
            this.ssView_Sheet1.Cells.Get(5, 6).Locked = true;
            this.ssView_Sheet1.Cells.Get(5, 7).Locked = true;
            this.ssView_Sheet1.Cells.Get(5, 8).Locked = true;
            this.ssView_Sheet1.Cells.Get(6, 0).Locked = true;
            this.ssView_Sheet1.Cells.Get(6, 1).ColumnSpan = 2;
            this.ssView_Sheet1.Cells.Get(6, 1).Locked = true;
            this.ssView_Sheet1.Cells.Get(6, 1).Value = "Total\r\nCholesterol";
            this.ssView_Sheet1.Cells.Get(6, 2).Locked = true;
            this.ssView_Sheet1.Cells.Get(6, 3).Locked = true;
            this.ssView_Sheet1.Cells.Get(6, 3).Value = "200~239";
            this.ssView_Sheet1.Cells.Get(6, 4).Locked = true;
            this.ssView_Sheet1.Cells.Get(6, 4).Value = "240~299";
            this.ssView_Sheet1.Cells.Get(6, 5).Locked = true;
            this.ssView_Sheet1.Cells.Get(6, 5).Value = ">300";
            this.ssView_Sheet1.Cells.Get(6, 6).Locked = true;
            this.ssView_Sheet1.Cells.Get(6, 7).Locked = true;
            this.ssView_Sheet1.Cells.Get(6, 8).Locked = true;
            this.ssView_Sheet1.Cells.Get(7, 0).Locked = true;
            this.ssView_Sheet1.Cells.Get(7, 1).ColumnSpan = 2;
            this.ssView_Sheet1.Cells.Get(7, 1).Locked = true;
            this.ssView_Sheet1.Cells.Get(7, 1).Value = "나이";
            this.ssView_Sheet1.Cells.Get(7, 2).Locked = true;
            this.ssView_Sheet1.Cells.Get(7, 3).Locked = true;
            this.ssView_Sheet1.Cells.Get(7, 3).Value = ">=70세";
            this.ssView_Sheet1.Cells.Get(7, 4).Locked = true;
            this.ssView_Sheet1.Cells.Get(7, 5).Locked = true;
            this.ssView_Sheet1.Cells.Get(7, 6).Locked = true;
            this.ssView_Sheet1.Cells.Get(7, 7).Locked = true;
            this.ssView_Sheet1.Cells.Get(7, 8).Locked = true;
            this.ssView_Sheet1.Cells.Get(8, 0).Locked = true;
            this.ssView_Sheet1.Cells.Get(8, 1).ColumnSpan = 2;
            this.ssView_Sheet1.Cells.Get(8, 1).Locked = true;
            this.ssView_Sheet1.Cells.Get(8, 1).Value = "식사처방";
            this.ssView_Sheet1.Cells.Get(8, 2).Locked = true;
            this.ssView_Sheet1.Cells.Get(8, 3).Locked = true;
            this.ssView_Sheet1.Cells.Get(8, 3).Value = "치료식 섭취";
            this.ssView_Sheet1.Cells.Get(8, 4).Locked = true;
            this.ssView_Sheet1.Cells.Get(8, 4).Value = "Tube\r\nFeeding";
            this.ssView_Sheet1.Cells.Get(8, 5).Locked = true;
            this.ssView_Sheet1.Cells.Get(8, 5).Value = "TPN";
            this.ssView_Sheet1.Cells.Get(8, 6).Locked = true;
            this.ssView_Sheet1.Cells.Get(8, 7).Locked = true;
            this.ssView_Sheet1.Cells.Get(8, 8).Locked = true;
            this.ssView_Sheet1.Cells.Get(9, 0).Locked = true;
            this.ssView_Sheet1.Cells.Get(9, 1).ColumnSpan = 2;
            this.ssView_Sheet1.Cells.Get(9, 1).Locked = true;
            this.ssView_Sheet1.Cells.Get(9, 1).Value = "식사 시 문제점\r\n(구토, 설사,\r\n변비, 연하곤란)";
            this.ssView_Sheet1.Cells.Get(9, 2).Locked = true;
            this.ssView_Sheet1.Cells.Get(9, 3).Locked = true;
            this.ssView_Sheet1.Cells.Get(9, 3).Value = "1개";
            this.ssView_Sheet1.Cells.Get(9, 4).Locked = true;
            this.ssView_Sheet1.Cells.Get(9, 4).Value = "2~3개";
            this.ssView_Sheet1.Cells.Get(9, 5).Locked = true;
            this.ssView_Sheet1.Cells.Get(9, 5).Value = "4개";
            this.ssView_Sheet1.Cells.Get(9, 6).Locked = true;
            this.ssView_Sheet1.Cells.Get(9, 7).Locked = true;
            this.ssView_Sheet1.Cells.Get(9, 8).Locked = true;
            this.ssView_Sheet1.ColumnFooter.Columns.Default.Resizable = false;
            this.ssView_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnHeader.Visible = false;
            this.ssView_Sheet1.Columns.Default.Resizable = false;
            this.ssView_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.Columns.Get(3).Width = 89F;
            this.ssView_Sheet1.Columns.Get(4).Width = 89F;
            this.ssView_Sheet1.Columns.Get(5).Width = 89F;
            this.ssView_Sheet1.DefaultStyleName = "Text764636615805998571944";
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.RowHeader.Rows.Default.Resizable = false;
            this.ssView_Sheet1.RowHeader.Visible = false;
            this.ssView_Sheet1.Rows.Default.Height = 26F;
            this.ssView_Sheet1.Rows.Default.Resizable = false;
            this.ssView_Sheet1.Rows.Get(6).Height = 46F;
            this.ssView_Sheet1.Rows.Get(8).Height = 42F;
            this.ssView_Sheet1.Rows.Get(9).Height = 55F;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmDietIndicator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 364);
            this.Controls.Add(this.ssView);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmDietIndicator";
            this.Text = "frmDietIndicator";
            this.Load += new System.EventHandler(this.frmDietIndicator_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel1;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
    }
}