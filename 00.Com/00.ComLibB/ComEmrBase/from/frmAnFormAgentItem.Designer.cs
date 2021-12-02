namespace ComEmrBase
{
    partial class frmAnFormAgentItem
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
            this.PanItemsAdd = new System.Windows.Forms.Panel();
            this.SsItemsAdd = new FarPoint.Win.Spread.FpSpread();
            this.SsItemsAdd_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel51 = new System.Windows.Forms.Panel();
            this.TxtAgentSearch = new System.Windows.Forms.TextBox();
            this.BtnAgentSearch = new System.Windows.Forms.Button();
            this.SsAgentItems = new FarPoint.Win.Spread.FpSpread();
            this.SsAgentItems_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel53 = new System.Windows.Forms.Panel();
            this.label30 = new System.Windows.Forms.Label();
            this.BtnAgentAdd = new System.Windows.Forms.Button();
            this.BtnAgentClose = new System.Windows.Forms.Button();
            this.PanItemsAdd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SsItemsAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SsItemsAdd_Sheet1)).BeginInit();
            this.panel51.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SsAgentItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SsAgentItems_Sheet1)).BeginInit();
            this.panel53.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanItemsAdd
            // 
            this.PanItemsAdd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanItemsAdd.Controls.Add(this.SsItemsAdd);
            this.PanItemsAdd.Controls.Add(this.panel51);
            this.PanItemsAdd.Controls.Add(this.SsAgentItems);
            this.PanItemsAdd.Controls.Add(this.panel53);
            this.PanItemsAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanItemsAdd.Location = new System.Drawing.Point(0, 0);
            this.PanItemsAdd.Name = "PanItemsAdd";
            this.PanItemsAdd.Size = new System.Drawing.Size(484, 734);
            this.PanItemsAdd.TabIndex = 38;
            this.PanItemsAdd.Visible = false;
            // 
            // SsItemsAdd
            // 
            this.SsItemsAdd.AccessibleDescription = "SsItemsAdd, Sheet1, Row 0, Column 0, I0000015889";
            this.SsItemsAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SsItemsAdd.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SsItemsAdd.Location = new System.Drawing.Point(0, 382);
            this.SsItemsAdd.Name = "SsItemsAdd";
            this.SsItemsAdd.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SsItemsAdd_Sheet1});
            this.SsItemsAdd.Size = new System.Drawing.Size(482, 350);
            this.SsItemsAdd.TabIndex = 35;
            this.SsItemsAdd.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SsItemsAdd_CellDoubleClick);
            // 
            // SsItemsAdd_Sheet1
            // 
            this.SsItemsAdd_Sheet1.Reset();
            this.SsItemsAdd_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SsItemsAdd_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SsItemsAdd_Sheet1.ColumnCount = 2;
            this.SsItemsAdd_Sheet1.RowCount = 6;
            this.SsItemsAdd_Sheet1.Cells.Get(0, 0).Value = "I0000015889";
            this.SsItemsAdd_Sheet1.Cells.Get(0, 1).Value = "N2O";
            this.SsItemsAdd_Sheet1.Cells.Get(1, 0).Value = "I0000022307";
            this.SsItemsAdd_Sheet1.Cells.Get(1, 1).Value = "O2";
            this.SsItemsAdd_Sheet1.Cells.Get(2, 0).Value = "I0000038141";
            this.SsItemsAdd_Sheet1.Cells.Get(2, 1).Value = "Sevofran";
            this.SsItemsAdd_Sheet1.Cells.Get(3, 0).Value = "I0000038142";
            this.SsItemsAdd_Sheet1.Cells.Get(3, 1).Value = "Prfol500mg";
            this.SsItemsAdd_Sheet1.Cells.Get(4, 0).Value = "I0000038143";
            this.SsItemsAdd_Sheet1.Cells.Get(4, 1).Value = "Prfol200mg";
            this.SsItemsAdd_Sheet1.Cells.Get(5, 0).Value = "I0000035948";
            this.SsItemsAdd_Sheet1.Cells.Get(5, 1).Value = "Remiva";
            this.SsItemsAdd_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SsItemsAdd_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SsItemsAdd_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SsItemsAdd_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SsItemsAdd_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "항목명";
            this.SsItemsAdd_Sheet1.Columns.Get(0).Locked = true;
            this.SsItemsAdd_Sheet1.Columns.Get(0).Visible = false;
            this.SsItemsAdd_Sheet1.Columns.Get(0).Width = 81F;
            this.SsItemsAdd_Sheet1.Columns.Get(1).Label = "항목명";
            this.SsItemsAdd_Sheet1.Columns.Get(1).Locked = true;
            this.SsItemsAdd_Sheet1.Columns.Get(1).Width = 300F;
            this.SsItemsAdd_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SsItemsAdd_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SsItemsAdd_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SsItemsAdd_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SsItemsAdd_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SsItemsAdd_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel51
            // 
            this.panel51.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel51.Controls.Add(this.TxtAgentSearch);
            this.panel51.Controls.Add(this.BtnAgentSearch);
            this.panel51.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel51.Location = new System.Drawing.Point(0, 342);
            this.panel51.Name = "panel51";
            this.panel51.Size = new System.Drawing.Size(482, 40);
            this.panel51.TabIndex = 37;
            // 
            // TxtAgentSearch
            // 
            this.TxtAgentSearch.Location = new System.Drawing.Point(14, 9);
            this.TxtAgentSearch.Name = "TxtAgentSearch";
            this.TxtAgentSearch.Size = new System.Drawing.Size(263, 21);
            this.TxtAgentSearch.TabIndex = 109;
            // 
            // BtnAgentSearch
            // 
            this.BtnAgentSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnAgentSearch.Location = new System.Drawing.Point(405, 0);
            this.BtnAgentSearch.Name = "BtnAgentSearch";
            this.BtnAgentSearch.Size = new System.Drawing.Size(75, 38);
            this.BtnAgentSearch.TabIndex = 108;
            this.BtnAgentSearch.Text = "조회";
            this.BtnAgentSearch.UseVisualStyleBackColor = true;
            this.BtnAgentSearch.Click += new System.EventHandler(this.BtnAgentSearch_Click);
            // 
            // SsAgentItems
            // 
            this.SsAgentItems.AccessibleDescription = "SsItemsAdd, Sheet1, Row 0, Column 0, I0000015889";
            this.SsAgentItems.Dock = System.Windows.Forms.DockStyle.Top;
            this.SsAgentItems.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SsAgentItems.Location = new System.Drawing.Point(0, 34);
            this.SsAgentItems.Name = "SsAgentItems";
            this.SsAgentItems.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SsAgentItems_Sheet1});
            this.SsAgentItems.Size = new System.Drawing.Size(482, 308);
            this.SsAgentItems.TabIndex = 38;
            // 
            // SsAgentItems_Sheet1
            // 
            this.SsAgentItems_Sheet1.Reset();
            this.SsAgentItems_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SsAgentItems_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SsAgentItems_Sheet1.ColumnCount = 2;
            this.SsAgentItems_Sheet1.RowCount = 6;
            this.SsAgentItems_Sheet1.Cells.Get(0, 0).Value = "I0000015889";
            this.SsAgentItems_Sheet1.Cells.Get(0, 1).Value = "N2O";
            this.SsAgentItems_Sheet1.Cells.Get(1, 0).Value = "I0000022307";
            this.SsAgentItems_Sheet1.Cells.Get(1, 1).Value = "O2";
            this.SsAgentItems_Sheet1.Cells.Get(2, 0).Value = "I0000038141";
            this.SsAgentItems_Sheet1.Cells.Get(2, 1).Value = "Sevofran";
            this.SsAgentItems_Sheet1.Cells.Get(3, 0).Value = "I0000038142";
            this.SsAgentItems_Sheet1.Cells.Get(3, 1).Value = "Prfol500mg";
            this.SsAgentItems_Sheet1.Cells.Get(4, 0).Value = "I0000038143";
            this.SsAgentItems_Sheet1.Cells.Get(4, 1).Value = "Prfol200mg";
            this.SsAgentItems_Sheet1.Cells.Get(5, 0).Value = "I0000035948";
            this.SsAgentItems_Sheet1.Cells.Get(5, 1).Value = "Remiva";
            this.SsAgentItems_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SsAgentItems_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SsAgentItems_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SsAgentItems_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SsAgentItems_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "항목명";
            this.SsAgentItems_Sheet1.Columns.Get(0).Locked = true;
            this.SsAgentItems_Sheet1.Columns.Get(0).Visible = false;
            this.SsAgentItems_Sheet1.Columns.Get(0).Width = 81F;
            this.SsAgentItems_Sheet1.Columns.Get(1).Label = "항목명";
            this.SsAgentItems_Sheet1.Columns.Get(1).Locked = true;
            this.SsAgentItems_Sheet1.Columns.Get(1).Width = 300F;
            this.SsAgentItems_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SsAgentItems_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SsAgentItems_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SsAgentItems_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SsAgentItems_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SsAgentItems_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel53
            // 
            this.panel53.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel53.Controls.Add(this.label30);
            this.panel53.Controls.Add(this.BtnAgentAdd);
            this.panel53.Controls.Add(this.BtnAgentClose);
            this.panel53.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel53.Location = new System.Drawing.Point(0, 0);
            this.panel53.Name = "panel53";
            this.panel53.Size = new System.Drawing.Size(482, 34);
            this.panel53.TabIndex = 36;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label30.Location = new System.Drawing.Point(12, 11);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(73, 12);
            this.label30.TabIndex = 109;
            this.label30.Text = "Agent 항목";
            // 
            // BtnAgentAdd
            // 
            this.BtnAgentAdd.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnAgentAdd.Location = new System.Drawing.Point(330, 0);
            this.BtnAgentAdd.Name = "BtnAgentAdd";
            this.BtnAgentAdd.Size = new System.Drawing.Size(75, 32);
            this.BtnAgentAdd.TabIndex = 107;
            this.BtnAgentAdd.Text = "저장";
            this.BtnAgentAdd.UseVisualStyleBackColor = true;
            this.BtnAgentAdd.Click += new System.EventHandler(this.BtnAgentAdd_Click);
            // 
            // BtnAgentClose
            // 
            this.BtnAgentClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnAgentClose.Location = new System.Drawing.Point(405, 0);
            this.BtnAgentClose.Name = "BtnAgentClose";
            this.BtnAgentClose.Size = new System.Drawing.Size(75, 32);
            this.BtnAgentClose.TabIndex = 108;
            this.BtnAgentClose.Text = "닫기";
            this.BtnAgentClose.UseVisualStyleBackColor = true;
            this.BtnAgentClose.Click += new System.EventHandler(this.BtnAgentClose_Click);
            // 
            // AnFormAgentItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 734);
            this.Controls.Add(this.PanItemsAdd);
            this.Name = "AnFormAgentItem";
            this.Text = "Agent 항목";
            this.Load += new System.EventHandler(this.AnFormAgentItem_Load);
            this.PanItemsAdd.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SsItemsAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SsItemsAdd_Sheet1)).EndInit();
            this.panel51.ResumeLayout(false);
            this.panel51.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SsAgentItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SsAgentItems_Sheet1)).EndInit();
            this.panel53.ResumeLayout(false);
            this.panel53.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanItemsAdd;
        private FarPoint.Win.Spread.FpSpread SsItemsAdd;
        private FarPoint.Win.Spread.SheetView SsItemsAdd_Sheet1;
        private System.Windows.Forms.Panel panel51;
        private System.Windows.Forms.TextBox TxtAgentSearch;
        private System.Windows.Forms.Button BtnAgentSearch;
        private FarPoint.Win.Spread.FpSpread SsAgentItems;
        private FarPoint.Win.Spread.SheetView SsAgentItems_Sheet1;
        private System.Windows.Forms.Panel panel53;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Button BtnAgentAdd;
        private System.Windows.Forms.Button BtnAgentClose;
    }
}