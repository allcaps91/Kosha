namespace ComEmrBase
{
    partial class frmEmrPrintFlowSheet
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
            this.panSearch = new System.Windows.Forms.Panel();
            this.Label1 = new System.Windows.Forms.Label();
            this.chkDesc = new System.Windows.Forms.CheckBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.mbtnPrint = new System.Windows.Forms.Button();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFrDate = new System.Windows.Forms.DateTimePicker();
            this.mbtnSearchAll = new System.Windows.Forms.Button();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panSearch
            // 
            this.panSearch.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panSearch.Controls.Add(this.Label1);
            this.panSearch.Controls.Add(this.chkDesc);
            this.panSearch.Controls.Add(this.Label3);
            this.panSearch.Controls.Add(this.mbtnPrint);
            this.panSearch.Controls.Add(this.dtpEndDate);
            this.panSearch.Controls.Add(this.dtpFrDate);
            this.panSearch.Controls.Add(this.mbtnSearchAll);
            this.panSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSearch.Location = new System.Drawing.Point(0, 0);
            this.panSearch.Name = "panSearch";
            this.panSearch.Size = new System.Drawing.Size(650, 36);
            this.panSearch.TabIndex = 26;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.BackColor = System.Drawing.Color.White;
            this.Label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label1.Location = new System.Drawing.Point(166, 10);
            this.Label1.Name = "Label1";
            this.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label1.Size = new System.Drawing.Size(17, 17);
            this.Label1.TabIndex = 91;
            this.Label1.Text = "~";
            // 
            // chkDesc
            // 
            this.chkDesc.AutoSize = true;
            this.chkDesc.Location = new System.Drawing.Point(284, 8);
            this.chkDesc.Name = "chkDesc";
            this.chkDesc.Size = new System.Drawing.Size(53, 21);
            this.chkDesc.TabIndex = 90;
            this.chkDesc.Text = "역순";
            this.chkDesc.UseVisualStyleBackColor = true;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.BackColor = System.Drawing.Color.White;
            this.Label3.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label3.Location = new System.Drawing.Point(8, 10);
            this.Label3.Name = "Label3";
            this.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label3.Size = new System.Drawing.Size(68, 17);
            this.Label3.TabIndex = 89;
            this.Label3.Text = "조회기간 :";
            // 
            // mbtnPrint
            // 
            this.mbtnPrint.Location = new System.Drawing.Point(421, 1);
            this.mbtnPrint.Name = "mbtnPrint";
            this.mbtnPrint.Size = new System.Drawing.Size(83, 30);
            this.mbtnPrint.TabIndex = 88;
            this.mbtnPrint.Text = "출 력";
            this.mbtnPrint.UseVisualStyleBackColor = true;
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new System.Drawing.Point(183, 6);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(90, 25);
            this.dtpEndDate.TabIndex = 87;
            // 
            // dtpFrDate
            // 
            this.dtpFrDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrDate.Location = new System.Drawing.Point(76, 6);
            this.dtpFrDate.Name = "dtpFrDate";
            this.dtpFrDate.Size = new System.Drawing.Size(90, 25);
            this.dtpFrDate.TabIndex = 86;
            // 
            // mbtnSearchAll
            // 
            this.mbtnSearchAll.BackColor = System.Drawing.SystemColors.Control;
            this.mbtnSearchAll.Cursor = System.Windows.Forms.Cursors.Default;
            this.mbtnSearchAll.ForeColor = System.Drawing.SystemColors.ControlText;
            this.mbtnSearchAll.Location = new System.Drawing.Point(338, 1);
            this.mbtnSearchAll.Name = "mbtnSearchAll";
            this.mbtnSearchAll.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.mbtnSearchAll.Size = new System.Drawing.Size(83, 30);
            this.mbtnSearchAll.TabIndex = 85;
            this.mbtnSearchAll.Text = "조  회";
            this.mbtnSearchAll.UseVisualStyleBackColor = true;
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.Location = new System.Drawing.Point(0, 36);
            this.ssView.Name = "ssView";
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(650, 567);
            this.ssView.TabIndex = 27;
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 0;
            this.ssView_Sheet1.RowCount = 0;
            this.ssView_Sheet1.ActiveColumnIndex = -1;
            this.ssView_Sheet1.ActiveRowIndex = -1;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssView_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmEmrPrintFlowSheet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(650, 603);
            this.Controls.Add(this.ssView);
            this.Controls.Add(this.panSearch);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmEmrPrintFlowSheet";
            this.Text = "frmEmrPrintFlowSheet";
            this.Load += new System.EventHandler(this.frmEmrPrintFlowSheet_Load);
            this.panSearch.ResumeLayout(false);
            this.panSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panSearch;
        public System.Windows.Forms.Label Label1;
        private System.Windows.Forms.CheckBox chkDesc;
        public System.Windows.Forms.Label Label3;
        private System.Windows.Forms.Button mbtnPrint;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.DateTimePicker dtpFrDate;
        public System.Windows.Forms.Button mbtnSearchAll;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
    }
}