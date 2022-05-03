namespace HC_OSHA
{
    partial class FrmExcelUpload7
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
            this.TxtLtdcode = new System.Windows.Forms.TextBox();
            this.BtnSearchSite = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lblLTD02 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnJob1 = new System.Windows.Forms.Button();
            this.SSExcel = new FarPoint.Win.Spread.FpSpread();
            this.SSExcel_Sheet1 = new FarPoint.Win.Spread.SheetView();
            ((System.ComponentModel.ISupportInitialize)(this.SSExcel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSExcel_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // TxtLtdcode
            // 
            this.TxtLtdcode.Location = new System.Drawing.Point(180, 8);
            this.TxtLtdcode.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.TxtLtdcode.Name = "TxtLtdcode";
            this.TxtLtdcode.Size = new System.Drawing.Size(140, 25);
            this.TxtLtdcode.TabIndex = 166;
            // 
            // BtnSearchSite
            // 
            this.BtnSearchSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSearchSite.Location = new System.Drawing.Point(338, 5);
            this.BtnSearchSite.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            this.BtnSearchSite.Name = "BtnSearchSite";
            this.BtnSearchSite.Size = new System.Drawing.Size(56, 29);
            this.BtnSearchSite.TabIndex = 165;
            this.BtnSearchSite.Text = "검색";
            this.BtnSearchSite.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.LightBlue;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(413, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 25);
            this.label4.TabIndex = 164;
            this.label4.Text = "작업년도";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLTD02
            // 
            this.lblLTD02.BackColor = System.Drawing.Color.LightBlue;
            this.lblLTD02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLTD02.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLTD02.Location = new System.Drawing.Point(136, 8);
            this.lblLTD02.Name = "lblLTD02";
            this.lblLTD02.Size = new System.Drawing.Size(42, 25);
            this.lblLTD02.TabIndex = 163;
            this.lblLTD02.Text = "회사";
            this.lblLTD02.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.RoyalBlue;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(3, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 29);
            this.label2.TabIndex = 162;
            this.label2.Text = "등록할 엑셀파일";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnJob1
            // 
            this.btnJob1.Location = new System.Drawing.Point(614, 4);
            this.btnJob1.Name = "btnJob1";
            this.btnJob1.Size = new System.Drawing.Size(100, 29);
            this.btnJob1.TabIndex = 161;
            this.btnJob1.Text = "엑셀파일 읽기";
            this.btnJob1.UseVisualStyleBackColor = true;
            this.btnJob1.Click += new System.EventHandler(this.btnJob1_Click);
            // 
            // SSExcel
            // 
            this.SSExcel.AccessibleDescription = "SSOshaList, Sheet1, Row 0, Column 0, HIC_USERS";
            this.SSExcel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSExcel.Location = new System.Drawing.Point(3, 44);
            this.SSExcel.Name = "SSExcel";
            this.SSExcel.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSExcel_Sheet1});
            this.SSExcel.Size = new System.Drawing.Size(896, 474);
            this.SSExcel.TabIndex = 160;
            // 
            // SSExcel_Sheet1
            // 
            this.SSExcel_Sheet1.Reset();
            this.SSExcel_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSExcel_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSExcel_Sheet1.ColumnCount = 0;
            this.SSExcel_Sheet1.RowCount = 0;
            this.SSExcel_Sheet1.ActiveColumnIndex = -1;
            this.SSExcel_Sheet1.ActiveRowIndex = -1;
            this.SSExcel_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSExcel_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSExcel_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSExcel_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSExcel_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSExcel_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSExcel_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSExcel_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSExcel_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSExcel_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // FrmExcelUpload7
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1027, 834);
            this.Controls.Add(this.TxtLtdcode);
            this.Controls.Add(this.BtnSearchSite);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblLTD02);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnJob1);
            this.Controls.Add(this.SSExcel);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmExcelUpload7";
            this.Text = "기업건강지수 업로드";
            ((System.ComponentModel.ISupportInitialize)(this.SSExcel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSExcel_Sheet1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TxtLtdcode;
        private System.Windows.Forms.Button BtnSearchSite;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblLTD02;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnJob1;
        private FarPoint.Win.Spread.FpSpread SSExcel;
        private FarPoint.Win.Spread.SheetView SSExcel_Sheet1;
    }
}