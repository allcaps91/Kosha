namespace HC_OSHA
{
    partial class SitePriceManageForm
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
            FarPoint.Win.Spread.InputMap SSPrice_InputMapWhenFocusedNormal;
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblCount = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.RdoAll = new System.Windows.Forms.RadioButton();
            this.RdoGbGukgo = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.TxtNameOrCode = new System.Windows.Forms.TextBox();
            this.SSPrice = new FarPoint.Win.Spread.FpSpread();
            this.SSPrice_Sheet1 = new FarPoint.Win.Spread.SheetView();
            SSPrice_InputMapWhenFocusedNormal = new FarPoint.Win.Spread.InputMap();
            SSPrice_InputMapWhenFocusedNormal.Parent = new FarPoint.Win.Spread.InputMap();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSPrice_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblCount);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.BtnSearch);
            this.panel1.Controls.Add(this.TxtNameOrCode);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1054, 58);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(428, 15);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(24, 17);
            this.lblCount.TabIndex = 26;
            this.lblCount.Text = "총:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.RdoAll);
            this.panel2.Controls.Add(this.RdoGbGukgo);
            this.panel2.Location = new System.Drawing.Point(257, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(144, 43);
            this.panel2.TabIndex = 25;
            // 
            // RdoAll
            // 
            this.RdoAll.AutoSize = true;
            this.RdoAll.Checked = true;
            this.RdoAll.Location = new System.Drawing.Point(14, 10);
            this.RdoAll.Name = "RdoAll";
            this.RdoAll.Size = new System.Drawing.Size(52, 21);
            this.RdoAll.TabIndex = 22;
            this.RdoAll.TabStop = true;
            this.RdoAll.Text = "전체";
            this.RdoAll.UseVisualStyleBackColor = true;
            this.RdoAll.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // RdoGbGukgo
            // 
            this.RdoGbGukgo.AutoSize = true;
            this.RdoGbGukgo.Location = new System.Drawing.Point(76, 10);
            this.RdoGbGukgo.Name = "RdoGbGukgo";
            this.RdoGbGukgo.Size = new System.Drawing.Size(52, 21);
            this.RdoGbGukgo.TabIndex = 24;
            this.RdoGbGukgo.Text = "국고";
            this.RdoGbGukgo.UseVisualStyleBackColor = true;
            this.RdoGbGukgo.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 17);
            this.label1.TabIndex = 21;
            this.label1.Text = "회사명";
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(689, 11);
            this.BtnSearch.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(75, 28);
            this.BtnSearch.TabIndex = 20;
            this.BtnSearch.Text = "검색";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // TxtNameOrCode
            // 
            this.TxtNameOrCode.Location = new System.Drawing.Point(92, 11);
            this.TxtNameOrCode.Name = "TxtNameOrCode";
            this.TxtNameOrCode.Size = new System.Drawing.Size(159, 25);
            this.TxtNameOrCode.TabIndex = 19;
            // 
            // SSPrice
            // 
            this.SSPrice.AccessibleDescription = "";
            this.SSPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSPrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSPrice.Location = new System.Drawing.Point(0, 58);
            this.SSPrice.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SSPrice.Name = "SSPrice";
            this.SSPrice.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSPrice_Sheet1});
            this.SSPrice.Size = new System.Drawing.Size(1054, 579);
            this.SSPrice.TabIndex = 107;
            SSPrice_InputMapWhenFocusedNormal.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.C, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardCopyValues);
            SSPrice_InputMapWhenFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Back, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.StartEditing);
            SSPrice_InputMapWhenFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Return, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.StartEditing);
            SSPrice_InputMapWhenFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke('='), FarPoint.Win.Spread.SpreadActions.StartEditingFormula);
            SSPrice_InputMapWhenFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.C, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardCopy);
            SSPrice_InputMapWhenFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.V, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardPasteAll);
            SSPrice_InputMapWhenFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.X, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardCut);
            SSPrice_InputMapWhenFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Insert, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardCopy);
            SSPrice_InputMapWhenFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Delete, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardCut);
            SSPrice_InputMapWhenFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Insert, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.ClipboardPasteAll);
            SSPrice_InputMapWhenFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.F4, System.Windows.Forms.Keys.None), FarPoint.Win.Spread.SpreadActions.ShowSubEditor);
            SSPrice_InputMapWhenFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Space, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.SelectRow);
            SSPrice_InputMapWhenFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Z, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.Undo);
            SSPrice_InputMapWhenFocusedNormal.Parent.Put(new FarPoint.Win.Spread.Keystroke(System.Windows.Forms.Keys.Y, ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.None)))), FarPoint.Win.Spread.SpreadActions.Redo);
            this.SSPrice.SetInputMap(FarPoint.Win.Spread.InputMapMode.WhenFocused, FarPoint.Win.Spread.OperationMode.Normal, SSPrice_InputMapWhenFocusedNormal);
            // 
            // SSPrice_Sheet1
            // 
            this.SSPrice_Sheet1.Reset();
            this.SSPrice_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSPrice_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSPrice_Sheet1.ColumnCount = 1;
            this.SSPrice_Sheet1.RowCount = 1;
            this.SSPrice_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSPrice_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSPrice_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSPrice_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSPrice_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSPrice_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSPrice_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSPrice_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSPrice_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSPrice_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // SitePriceManageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1054, 637);
            this.Controls.Add(this.SSPrice);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SitePriceManageForm";
            this.Text = "사업장 계약금액";
            this.Load += new System.EventHandler(this.SitePriceManageForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSPrice_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private FarPoint.Win.Spread.FpSpread SSPrice;
        private FarPoint.Win.Spread.SheetView SSPrice_Sheet1;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.TextBox TxtNameOrCode;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton RdoGbGukgo;
        private System.Windows.Forms.RadioButton RdoAll;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCount;
    }
}