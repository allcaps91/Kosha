namespace HC_OSHA
{
    partial class SiteMSDSListForm
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panBody = new System.Windows.Forms.Panel();
            this.SSMSDSList = new FarPoint.Win.Spread.FpSpread();
            this.SSMSDSList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.btnPdf = new System.Windows.Forms.Button();
            this.BtnApply = new System.Windows.Forms.Button();
            this.btnExcel = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.LblSite = new System.Windows.Forms.Label();
            this.BtnMange = new System.Windows.Forms.Button();
            this.contentTitle3 = new ComBase.Mvc.UserControls.ContentTitle();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSMSDSList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSMSDSList_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panBody
            // 
            this.panBody.Controls.Add(this.SSMSDSList);
            this.panBody.Controls.Add(this.btnPdf);
            this.panBody.Controls.Add(this.BtnApply);
            this.panBody.Controls.Add(this.btnExcel);
            this.panBody.Controls.Add(this.btnExit);
            this.panBody.Controls.Add(this.LblSite);
            this.panBody.Controls.Add(this.BtnMange);
            this.panBody.Controls.Add(this.contentTitle3);
            this.panBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBody.Location = new System.Drawing.Point(0, 0);
            this.panBody.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panBody.Name = "panBody";
            this.panBody.Size = new System.Drawing.Size(1264, 871);
            this.panBody.TabIndex = 0;
            // 
            // SSMSDSList
            // 
            this.SSMSDSList.AccessibleDescription = "SS1, Sheet1, Row 0, Column 0, ";
            this.SSMSDSList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSMSDSList.FocusRenderer = flatFocusIndicatorRenderer1;
            this.SSMSDSList.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SSMSDSList.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SSMSDSList.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SSMSDSList.HorizontalScrollBar.Renderer = flatScrollBarRenderer1;
            this.SSMSDSList.HorizontalScrollBar.TabIndex = 258;
            this.SSMSDSList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SSMSDSList.Location = new System.Drawing.Point(0, 44);
            this.SSMSDSList.Name = "SSMSDSList";
            this.SSMSDSList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSMSDSList_Sheet1});
            this.SSMSDSList.Size = new System.Drawing.Size(1264, 827);
            this.SSMSDSList.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.SSMSDSList.TabIndex = 162;
            this.SSMSDSList.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SSMSDSList.VerticalScrollBar.Name = "";
            flatScrollBarRenderer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SSMSDSList.VerticalScrollBar.Renderer = flatScrollBarRenderer2;
            this.SSMSDSList.VerticalScrollBar.TabIndex = 259;
            this.SSMSDSList.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // SSMSDSList_Sheet1
            // 
            this.SSMSDSList_Sheet1.Reset();
            this.SSMSDSList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSMSDSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSMSDSList_Sheet1.ColumnCount = 26;
            this.SSMSDSList_Sheet1.RowCount = 50;
            this.SSMSDSList_Sheet1.Cells.Get(3, 25).CellType = textCellType1;
            this.SSMSDSList_Sheet1.Cells.Get(3, 25).Locked = true;
            this.SSMSDSList_Sheet1.Cells.Get(3, 25).Value = "";
            this.SSMSDSList_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMSDSList_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSMSDSList_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.SSMSDSList_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMSDSList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMSDSList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSMSDSList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.SSMSDSList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMSDSList_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "취급공정";
            this.SSMSDSList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "제품명";
            this.SSMSDSList_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "권고용도";
            this.SSMSDSList_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "제조사";
            this.SSMSDSList_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "월취급량";
            this.SSMSDSList_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "단위";
            this.SSMSDSList_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "개정일자";
            this.SSMSDSList_Sheet1.ColumnHeader.Cells.Get(0, 25).Value = "비고";
            this.SSMSDSList_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMSDSList_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSMSDSList_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.SSMSDSList_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMSDSList_Sheet1.ColumnHeader.Rows.Get(0).Height = 41F;
            textCellType2.WordWrap = true;
            this.SSMSDSList_Sheet1.Columns.Get(0).CellType = textCellType2;
            this.SSMSDSList_Sheet1.Columns.Get(0).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SSMSDSList_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSMSDSList_Sheet1.Columns.Get(0).Label = "취급공정";
            this.SSMSDSList_Sheet1.Columns.Get(0).Locked = true;
            this.SSMSDSList_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.SSMSDSList_Sheet1.Columns.Get(0).Width = 64F;
            textCellType3.WordWrap = true;
            this.SSMSDSList_Sheet1.Columns.Get(1).CellType = textCellType3;
            this.SSMSDSList_Sheet1.Columns.Get(1).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SSMSDSList_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMSDSList_Sheet1.Columns.Get(1).Label = "제품명";
            this.SSMSDSList_Sheet1.Columns.Get(1).Locked = true;
            this.SSMSDSList_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.SSMSDSList_Sheet1.Columns.Get(1).Width = 82F;
            textCellType4.Multiline = true;
            textCellType4.WordWrap = true;
            this.SSMSDSList_Sheet1.Columns.Get(2).CellType = textCellType4;
            this.SSMSDSList_Sheet1.Columns.Get(2).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SSMSDSList_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SSMSDSList_Sheet1.Columns.Get(2).Label = "권고용도";
            this.SSMSDSList_Sheet1.Columns.Get(2).Locked = true;
            this.SSMSDSList_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSMSDSList_Sheet1.Columns.Get(2).Width = 79F;
            textCellType5.Multiline = true;
            textCellType5.WordWrap = true;
            this.SSMSDSList_Sheet1.Columns.Get(3).CellType = textCellType5;
            this.SSMSDSList_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSMSDSList_Sheet1.Columns.Get(3).Label = "제조사";
            this.SSMSDSList_Sheet1.Columns.Get(3).Locked = true;
            this.SSMSDSList_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSMSDSList_Sheet1.Columns.Get(3).Width = 59F;
            textCellType6.WordWrap = true;
            this.SSMSDSList_Sheet1.Columns.Get(4).CellType = textCellType6;
            this.SSMSDSList_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSMSDSList_Sheet1.Columns.Get(4).Label = "월취급량";
            this.SSMSDSList_Sheet1.Columns.Get(4).Locked = true;
            this.SSMSDSList_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMSDSList_Sheet1.Columns.Get(4).Width = 65F;
            this.SSMSDSList_Sheet1.Columns.Get(5).Label = "단위";
            this.SSMSDSList_Sheet1.Columns.Get(5).Width = 35F;
            this.SSMSDSList_Sheet1.Columns.Get(6).Label = "개정일자";
            this.SSMSDSList_Sheet1.Columns.Get(6).Width = 65F;
            textCellType7.Multiline = true;
            textCellType7.WordWrap = true;
            this.SSMSDSList_Sheet1.Columns.Get(25).CellType = textCellType7;
            this.SSMSDSList_Sheet1.Columns.Get(25).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SSMSDSList_Sheet1.Columns.Get(25).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSMSDSList_Sheet1.Columns.Get(25).Label = "비고";
            this.SSMSDSList_Sheet1.Columns.Get(25).Locked = true;
            this.SSMSDSList_Sheet1.Columns.Get(25).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMSDSList_Sheet1.Columns.Get(25).Width = 115F;
            this.SSMSDSList_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMSDSList_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSMSDSList_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.SSMSDSList_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMSDSList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMSDSList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSMSDSList_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.SSMSDSList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMSDSList_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
            this.SSMSDSList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSMSDSList_Sheet1.RowHeader.Columns.Get(0).Width = 30F;
            this.SSMSDSList_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMSDSList_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSMSDSList_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.SSMSDSList_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMSDSList_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMSDSList_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSMSDSList_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.SSMSDSList_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMSDSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // btnPdf
            // 
            this.btnPdf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPdf.Location = new System.Drawing.Point(863, 3);
            this.btnPdf.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.btnPdf.Name = "btnPdf";
            this.btnPdf.Size = new System.Drawing.Size(75, 28);
            this.btnPdf.TabIndex = 37;
            this.btnPdf.Text = "pdf저장";
            this.btnPdf.UseVisualStyleBackColor = true;
            this.btnPdf.Click += new System.EventHandler(this.btnPdf_Click);
            // 
            // BtnApply
            // 
            this.BtnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnApply.Location = new System.Drawing.Point(701, 3);
            this.BtnApply.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.BtnApply.Name = "BtnApply";
            this.BtnApply.Size = new System.Drawing.Size(75, 28);
            this.BtnApply.TabIndex = 36;
            this.BtnApply.Text = "불러오기";
            this.BtnApply.UseVisualStyleBackColor = true;
            this.BtnApply.Visible = false;
            this.BtnApply.Click += new System.EventHandler(this.BtnApply_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.Location = new System.Drawing.Point(782, 2);
            this.btnExcel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 28);
            this.btnExcel.TabIndex = 35;
            this.btnExcel.Text = "엑셀저장";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(1105, 2);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 28);
            this.btnExit.TabIndex = 34;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // LblSite
            // 
            this.LblSite.AutoSize = true;
            this.LblSite.Location = new System.Drawing.Point(189, 9);
            this.LblSite.Name = "LblSite";
            this.LblSite.Size = new System.Drawing.Size(43, 17);
            this.LblSite.TabIndex = 13;
            this.LblSite.Text = "label1";
            // 
            // BtnMange
            // 
            this.BtnMange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnMange.Location = new System.Drawing.Point(1024, 2);
            this.BtnMange.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.BtnMange.Name = "BtnMange";
            this.BtnMange.Size = new System.Drawing.Size(75, 28);
            this.BtnMange.TabIndex = 12;
            this.BtnMange.Text = "관리";
            this.BtnMange.UseVisualStyleBackColor = true;
            this.BtnMange.Click += new System.EventHandler(this.BtnMange_Click);
            // 
            // contentTitle3
            // 
            this.contentTitle3.Dock = System.Windows.Forms.DockStyle.Top;
            this.contentTitle3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.contentTitle3.Location = new System.Drawing.Point(0, 0);
            this.contentTitle3.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.contentTitle3.Name = "contentTitle3";
            this.contentTitle3.Size = new System.Drawing.Size(1264, 44);
            this.contentTitle3.TabIndex = 4;
            this.contentTitle3.TitleText = "화학물질 MSDS 목록 현황";
            this.contentTitle3.Load += new System.EventHandler(this.contentTitle3_Load);
            // 
            // SiteMSDSListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 871);
            this.Controls.Add(this.panBody);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SiteMSDSListForm";
            this.Text = "SiteMSDSListForm";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panBody.ResumeLayout(false);
            this.panBody.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSMSDSList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSMSDSList_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panBody;
        private ComBase.Mvc.UserControls.ContentTitle contentTitle3;
        private System.Windows.Forms.Button BtnMange;
        private System.Windows.Forms.Label LblSite;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.Button BtnApply;
        private System.Windows.Forms.Button btnPdf;
        private FarPoint.Win.Spread.FpSpread SSMSDSList;
        private FarPoint.Win.Spread.SheetView SSMSDSList_Sheet1;
    }
}