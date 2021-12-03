namespace HC_OSHA
{
    partial class frmCharge
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
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType1 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            this.SSList = new FarPoint.Win.Spread.FpSpread();
            this.SSList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panSearch = new System.Windows.Forms.Panel();
            this.QuaterChargeCheckBox = new System.Windows.Forms.Panel();
            this.ChkQ12 = new System.Windows.Forms.CheckBox();
            this.ChkQ11 = new System.Windows.Forms.CheckBox();
            this.ChkQ10 = new System.Windows.Forms.CheckBox();
            this.ChkQ9 = new System.Windows.Forms.CheckBox();
            this.ChkQ8 = new System.Windows.Forms.CheckBox();
            this.ChkQ7 = new System.Windows.Forms.CheckBox();
            this.ChkQ6 = new System.Windows.Forms.CheckBox();
            this.ChkQ5 = new System.Windows.Forms.CheckBox();
            this.ChkQ1 = new System.Windows.Forms.CheckBox();
            this.ChkQ4 = new System.Windows.Forms.CheckBox();
            this.ChkQ2 = new System.Windows.Forms.CheckBox();
            this.ChkQ3 = new System.Windows.Forms.CheckBox();
            this.ChkQuarterCharge = new System.Windows.Forms.CheckBox();
            this.BtnDelete = new System.Windows.Forms.Button();
            this.lblTotalCount = new System.Windows.Forms.Label();
            this.lblChildCount = new System.Windows.Forms.Label();
            this.CboMonth = new System.Windows.Forms.ComboBox();
            this.lblCount = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnBuild = new System.Windows.Forms.Button();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.DtpBdate = new System.Windows.Forms.DateTimePicker();
            this.label15 = new System.Windows.Forms.Label();
            this.formTItle1 = new ComBase.Mvc.UserControls.FormTItle();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).BeginInit();
            this.panSearch.SuspendLayout();
            this.QuaterChargeCheckBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // SSList
            // 
            this.SSList.AccessibleDescription = "";
            this.SSList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSList.Location = new System.Drawing.Point(0, 105);
            this.SSList.Name = "SSList";
            this.SSList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSList_Sheet1});
            this.SSList.Size = new System.Drawing.Size(1049, 486);
            this.SSList.TabIndex = 6;
            this.SSList.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSList_CellClick);
            // 
            // SSList_Sheet1
            // 
            this.SSList_Sheet1.Reset();
            this.SSList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSList_Sheet1.ColumnCount = 1;
            this.SSList_Sheet1.RowCount = 0;
            this.SSList_Sheet1.ActiveColumnIndex = -1;
            this.SSList_Sheet1.ActiveRowIndex = -1;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 0).CellType = checkBoxCellType1;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panSearch
            // 
            this.panSearch.BackColor = System.Drawing.Color.White;
            this.panSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSearch.Controls.Add(this.QuaterChargeCheckBox);
            this.panSearch.Controls.Add(this.ChkQuarterCharge);
            this.panSearch.Controls.Add(this.BtnDelete);
            this.panSearch.Controls.Add(this.lblTotalCount);
            this.panSearch.Controls.Add(this.lblChildCount);
            this.panSearch.Controls.Add(this.CboMonth);
            this.panSearch.Controls.Add(this.lblCount);
            this.panSearch.Controls.Add(this.label1);
            this.panSearch.Controls.Add(this.BtnBuild);
            this.panSearch.Controls.Add(this.BtnSearch);
            this.panSearch.Controls.Add(this.DtpBdate);
            this.panSearch.Controls.Add(this.label15);
            this.panSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSearch.Location = new System.Drawing.Point(0, 35);
            this.panSearch.Name = "panSearch";
            this.panSearch.Size = new System.Drawing.Size(1049, 70);
            this.panSearch.TabIndex = 4;
            // 
            // QuaterChargeCheckBox
            // 
            this.QuaterChargeCheckBox.Controls.Add(this.ChkQ12);
            this.QuaterChargeCheckBox.Controls.Add(this.ChkQ11);
            this.QuaterChargeCheckBox.Controls.Add(this.ChkQ10);
            this.QuaterChargeCheckBox.Controls.Add(this.ChkQ9);
            this.QuaterChargeCheckBox.Controls.Add(this.ChkQ8);
            this.QuaterChargeCheckBox.Controls.Add(this.ChkQ7);
            this.QuaterChargeCheckBox.Controls.Add(this.ChkQ6);
            this.QuaterChargeCheckBox.Controls.Add(this.ChkQ5);
            this.QuaterChargeCheckBox.Controls.Add(this.ChkQ1);
            this.QuaterChargeCheckBox.Controls.Add(this.ChkQ4);
            this.QuaterChargeCheckBox.Controls.Add(this.ChkQ2);
            this.QuaterChargeCheckBox.Controls.Add(this.ChkQ3);
            this.QuaterChargeCheckBox.Location = new System.Drawing.Point(131, 35);
            this.QuaterChargeCheckBox.Name = "QuaterChargeCheckBox";
            this.QuaterChargeCheckBox.Size = new System.Drawing.Size(684, 28);
            this.QuaterChargeCheckBox.TabIndex = 88;
            // 
            // ChkQ12
            // 
            this.ChkQ12.AutoSize = true;
            this.ChkQ12.Location = new System.Drawing.Point(611, 6);
            this.ChkQ12.Name = "ChkQ12";
            this.ChkQ12.Size = new System.Drawing.Size(54, 21);
            this.ChkQ12.TabIndex = 22;
            this.ChkQ12.Text = "12월";
            this.ChkQ12.UseVisualStyleBackColor = true;
            // 
            // ChkQ11
            // 
            this.ChkQ11.AutoSize = true;
            this.ChkQ11.Location = new System.Drawing.Point(551, 6);
            this.ChkQ11.Name = "ChkQ11";
            this.ChkQ11.Size = new System.Drawing.Size(54, 21);
            this.ChkQ11.TabIndex = 21;
            this.ChkQ11.Text = "11월";
            this.ChkQ11.UseVisualStyleBackColor = true;
            // 
            // ChkQ10
            // 
            this.ChkQ10.AutoSize = true;
            this.ChkQ10.Location = new System.Drawing.Point(491, 6);
            this.ChkQ10.Name = "ChkQ10";
            this.ChkQ10.Size = new System.Drawing.Size(54, 21);
            this.ChkQ10.TabIndex = 20;
            this.ChkQ10.Text = "10월";
            this.ChkQ10.UseVisualStyleBackColor = true;
            // 
            // ChkQ9
            // 
            this.ChkQ9.AutoSize = true;
            this.ChkQ9.Location = new System.Drawing.Point(438, 6);
            this.ChkQ9.Name = "ChkQ9";
            this.ChkQ9.Size = new System.Drawing.Size(47, 21);
            this.ChkQ9.TabIndex = 19;
            this.ChkQ9.Text = "9월";
            this.ChkQ9.UseVisualStyleBackColor = true;
            // 
            // ChkQ8
            // 
            this.ChkQ8.AutoSize = true;
            this.ChkQ8.Location = new System.Drawing.Point(385, 6);
            this.ChkQ8.Name = "ChkQ8";
            this.ChkQ8.Size = new System.Drawing.Size(47, 21);
            this.ChkQ8.TabIndex = 18;
            this.ChkQ8.Text = "8월";
            this.ChkQ8.UseVisualStyleBackColor = true;
            // 
            // ChkQ7
            // 
            this.ChkQ7.AutoSize = true;
            this.ChkQ7.Location = new System.Drawing.Point(332, 6);
            this.ChkQ7.Name = "ChkQ7";
            this.ChkQ7.Size = new System.Drawing.Size(47, 21);
            this.ChkQ7.TabIndex = 17;
            this.ChkQ7.Text = "7월";
            this.ChkQ7.UseVisualStyleBackColor = true;
            // 
            // ChkQ6
            // 
            this.ChkQ6.AutoSize = true;
            this.ChkQ6.Location = new System.Drawing.Point(279, 6);
            this.ChkQ6.Name = "ChkQ6";
            this.ChkQ6.Size = new System.Drawing.Size(47, 21);
            this.ChkQ6.TabIndex = 16;
            this.ChkQ6.Text = "6월";
            this.ChkQ6.UseVisualStyleBackColor = true;
            // 
            // ChkQ5
            // 
            this.ChkQ5.AutoSize = true;
            this.ChkQ5.Location = new System.Drawing.Point(226, 6);
            this.ChkQ5.Name = "ChkQ5";
            this.ChkQ5.Size = new System.Drawing.Size(47, 21);
            this.ChkQ5.TabIndex = 15;
            this.ChkQ5.Text = "5월";
            this.ChkQ5.UseVisualStyleBackColor = true;
            // 
            // ChkQ1
            // 
            this.ChkQ1.AutoSize = true;
            this.ChkQ1.Location = new System.Drawing.Point(6, 6);
            this.ChkQ1.Name = "ChkQ1";
            this.ChkQ1.Size = new System.Drawing.Size(47, 21);
            this.ChkQ1.TabIndex = 11;
            this.ChkQ1.Text = "1월";
            this.ChkQ1.UseVisualStyleBackColor = true;
            // 
            // ChkQ4
            // 
            this.ChkQ4.AutoSize = true;
            this.ChkQ4.Location = new System.Drawing.Point(173, 6);
            this.ChkQ4.Name = "ChkQ4";
            this.ChkQ4.Size = new System.Drawing.Size(47, 21);
            this.ChkQ4.TabIndex = 14;
            this.ChkQ4.Text = "4월";
            this.ChkQ4.UseVisualStyleBackColor = true;
            // 
            // ChkQ2
            // 
            this.ChkQ2.AutoSize = true;
            this.ChkQ2.Location = new System.Drawing.Point(67, 6);
            this.ChkQ2.Name = "ChkQ2";
            this.ChkQ2.Size = new System.Drawing.Size(47, 21);
            this.ChkQ2.TabIndex = 12;
            this.ChkQ2.Text = "2월";
            this.ChkQ2.UseVisualStyleBackColor = true;
            // 
            // ChkQ3
            // 
            this.ChkQ3.AutoSize = true;
            this.ChkQ3.Location = new System.Drawing.Point(120, 6);
            this.ChkQ3.Name = "ChkQ3";
            this.ChkQ3.Size = new System.Drawing.Size(47, 21);
            this.ChkQ3.TabIndex = 13;
            this.ChkQ3.Text = "3월";
            this.ChkQ3.UseVisualStyleBackColor = true;
            // 
            // ChkQuarterCharge
            // 
            this.ChkQuarterCharge.AutoSize = true;
            this.ChkQuarterCharge.Location = new System.Drawing.Point(11, 42);
            this.ChkQuarterCharge.Name = "ChkQuarterCharge";
            this.ChkQuarterCharge.Size = new System.Drawing.Size(110, 21);
            this.ChkQuarterCharge.TabIndex = 87;
            this.ChkQuarterCharge.Text = "분기청구 여부";
            this.ChkQuarterCharge.UseVisualStyleBackColor = true;
            this.ChkQuarterCharge.CheckedChanged += new System.EventHandler(this.ChkQuarterCharge_CheckedChanged);
            // 
            // BtnDelete
            // 
            this.BtnDelete.Location = new System.Drawing.Point(840, 6);
            this.BtnDelete.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(75, 28);
            this.BtnDelete.TabIndex = 85;
            this.BtnDelete.Text = "미수삭제";
            this.BtnDelete.UseVisualStyleBackColor = true;
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // lblTotalCount
            // 
            this.lblTotalCount.AutoSize = true;
            this.lblTotalCount.Location = new System.Drawing.Point(704, 13);
            this.lblTotalCount.Name = "lblTotalCount";
            this.lblTotalCount.Size = new System.Drawing.Size(54, 17);
            this.lblTotalCount.TabIndex = 86;
            this.lblTotalCount.Text = "총: 0 건";
            // 
            // lblChildCount
            // 
            this.lblChildCount.AutoSize = true;
            this.lblChildCount.Location = new System.Drawing.Point(617, 13);
            this.lblChildCount.Name = "lblChildCount";
            this.lblChildCount.Size = new System.Drawing.Size(67, 17);
            this.lblChildCount.TabIndex = 85;
            this.lblChildCount.Text = "하청: 0 건";
            // 
            // CboMonth
            // 
            this.CboMonth.FormattingEnabled = true;
            this.CboMonth.Location = new System.Drawing.Point(92, 8);
            this.CboMonth.Name = "CboMonth";
            this.CboMonth.Size = new System.Drawing.Size(103, 25);
            this.CboMonth.TabIndex = 84;
            this.CboMonth.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(526, 13);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(67, 17);
            this.lblCount.TabIndex = 84;
            this.lblCount.Text = "원청: 0 건";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(215, 8);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(3);
            this.label1.Size = new System.Drawing.Size(75, 25);
            this.label1.TabIndex = 83;
            this.label1.Text = "미수일자";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnBuild
            // 
            this.BtnBuild.Location = new System.Drawing.Point(932, 7);
            this.BtnBuild.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnBuild.Name = "BtnBuild";
            this.BtnBuild.Size = new System.Drawing.Size(75, 28);
            this.BtnBuild.TabIndex = 82;
            this.BtnBuild.Text = "미수형성";
            this.BtnBuild.UseVisualStyleBackColor = true;
            this.BtnBuild.Click += new System.EventHandler(this.BtnBuild_Click);
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(426, 6);
            this.BtnSearch.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(75, 28);
            this.BtnSearch.TabIndex = 80;
            this.BtnSearch.Text = "검색";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // DtpBdate
            // 
            this.DtpBdate.CustomFormat = "";
            this.DtpBdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpBdate.Location = new System.Drawing.Point(296, 8);
            this.DtpBdate.Name = "DtpBdate";
            this.DtpBdate.Size = new System.Drawing.Size(107, 25);
            this.DtpBdate.TabIndex = 84;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label15.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.Location = new System.Drawing.Point(11, 8);
            this.label15.Name = "label15";
            this.label15.Padding = new System.Windows.Forms.Padding(3);
            this.label15.Size = new System.Drawing.Size(75, 25);
            this.label15.TabIndex = 78;
            this.label15.Text = "작업년월";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // formTItle1
            // 
            this.formTItle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.formTItle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.formTItle1.Location = new System.Drawing.Point(0, 0);
            this.formTItle1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.formTItle1.Name = "formTItle1";
            this.formTItle1.Size = new System.Drawing.Size(1049, 35);
            this.formTItle1.TabIndex = 5;
            this.formTItle1.TitleText = "미수 자동 형성 ";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(962, 5);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 28);
            this.button1.TabIndex = 83;
            this.button1.Text = "닫기";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmCharge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1049, 591);
            this.Controls.Add(this.SSList);
            this.Controls.Add(this.panSearch);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.formTItle1);
            this.Name = "frmCharge";
            this.Text = "미수형성";
            this.Load += new System.EventHandler(this.frmCharge_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).EndInit();
            this.panSearch.ResumeLayout(false);
            this.panSearch.PerformLayout();
            this.QuaterChargeCheckBox.ResumeLayout(false);
            this.QuaterChargeCheckBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private FarPoint.Win.Spread.FpSpread SSList;
        private FarPoint.Win.Spread.SheetView SSList_Sheet1;
        private System.Windows.Forms.Panel panSearch;
        private System.Windows.Forms.Button BtnBuild;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.Label label15;
        private ComBase.Mvc.UserControls.FormTItle formTItle1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker DtpBdate;
        private System.Windows.Forms.ComboBox CboMonth;
        private System.Windows.Forms.Button BtnDelete;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Label lblChildCount;
        private System.Windows.Forms.Label lblTotalCount;
        private System.Windows.Forms.CheckBox ChkQuarterCharge;
        private System.Windows.Forms.Panel QuaterChargeCheckBox;
        private System.Windows.Forms.CheckBox ChkQ12;
        private System.Windows.Forms.CheckBox ChkQ11;
        private System.Windows.Forms.CheckBox ChkQ10;
        private System.Windows.Forms.CheckBox ChkQ9;
        private System.Windows.Forms.CheckBox ChkQ8;
        private System.Windows.Forms.CheckBox ChkQ7;
        private System.Windows.Forms.CheckBox ChkQ6;
        private System.Windows.Forms.CheckBox ChkQ5;
        private System.Windows.Forms.CheckBox ChkQ1;
        private System.Windows.Forms.CheckBox ChkQ4;
        private System.Windows.Forms.CheckBox ChkQ2;
        private System.Windows.Forms.CheckBox ChkQ3;
    }
}