namespace HC_OSHA.form.Visit
{
    partial class VIsitDocumentBatchForm
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
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType2 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.GeneralCellType generalCellType1 = new FarPoint.Win.Spread.CellType.GeneralCellType();
            this.TxtDocNumber = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DtpDate = new System.Windows.Forms.DateTimePicker();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.contentTitle1 = new ComBase.Mvc.UserControls.ContentTitle();
            this.SSList = new FarPoint.Win.Spread.FpSpread();
            this.SSList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.btnPRint = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSendMail = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkMailSend = new System.Windows.Forms.CheckBox();
            this.CboManager = new System.Windows.Forms.ComboBox();
            this.txtAttach = new System.Windows.Forms.TextBox();
            this.btnAttach = new System.Windows.Forms.Button();
            this.CboMonth = new System.Windows.Forms.ComboBox();
            this.BtnExportPdf = new System.Windows.Forms.Button();
            this.formulaProvider1 = new FarPoint.Win.Spread.FormulaProvider();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formulaProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // TxtDocNumber
            // 
            this.TxtDocNumber.Location = new System.Drawing.Point(214, 8);
            this.TxtDocNumber.Name = "TxtDocNumber";
            this.TxtDocNumber.Size = new System.Drawing.Size(70, 25);
            this.TxtDocNumber.TabIndex = 1;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label15.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label15.Location = new System.Drawing.Point(5, 8);
            this.label15.Name = "label15";
            this.label15.Padding = new System.Windows.Forms.Padding(4);
            this.label15.Size = new System.Drawing.Size(57, 25);
            this.label15.TabIndex = 106;
            this.label15.Text = "방문월";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(140, 8);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(4);
            this.label1.Size = new System.Drawing.Size(70, 25);
            this.label1.TabIndex = 108;
            this.label1.Text = "문서번호";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(289, 10);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(4);
            this.label2.Size = new System.Drawing.Size(70, 25);
            this.label2.TabIndex = 109;
            this.label2.Text = "시행일자";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DtpDate
            // 
            this.DtpDate.CustomFormat = "yyyy-MM-dd";
            this.DtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpDate.Location = new System.Drawing.Point(362, 11);
            this.DtpDate.Name = "DtpDate";
            this.DtpDate.Size = new System.Drawing.Size(104, 25);
            this.DtpDate.TabIndex = 2;
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(600, 9);
            this.BtnSearch.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(90, 28);
            this.BtnSearch.TabIndex = 4;
            this.BtnSearch.Text = "검색";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // contentTitle1
            // 
            this.contentTitle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.contentTitle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.contentTitle1.Location = new System.Drawing.Point(0, 0);
            this.contentTitle1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.contentTitle1.Name = "contentTitle1";
            this.contentTitle1.Size = new System.Drawing.Size(1264, 38);
            this.contentTitle1.TabIndex = 113;
            this.contentTitle1.TitleText = "공문인쇄 및 메일발송";
            // 
            // SSList
            // 
            this.SSList.AccessibleDescription = "SSList, Sheet1, Row 0, Column 0, ";
            this.SSList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSList.Location = new System.Drawing.Point(0, 98);
            this.SSList.Name = "SSList";
            this.SSList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSList_Sheet1});
            this.SSList.Size = new System.Drawing.Size(1264, 773);
            this.SSList.TabIndex = 114;
            // 
            // SSList_Sheet1
            // 
            this.SSList_Sheet1.Reset();
            this.SSList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSList_Sheet1.ColumnCount = 16;
            this.SSList_Sheet1.RowCount = 0;
            this.SSList_Sheet1.ActiveColumnIndex = -1;
            this.SSList_Sheet1.ActiveRowIndex = -1;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 0).CellType = checkBoxCellType1;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "False";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "코드";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "회사명";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "방문1";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "방문2";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "방문3";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "방문4";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "방문5";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "Email1";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "Email2";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "Email3";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "Email4";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "메일 발송일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "발송자";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "계약ID";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 15).Value = "IDs";
            this.SSList_Sheet1.ColumnHeader.Rows.Get(0).Height = 43F;
            checkBoxCellType2.FocusRectangle = FarPoint.Win.FocusRectangle.Show;
            checkBoxCellType2.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
            checkBoxCellType2.ThreeState = true;
            this.SSList_Sheet1.Columns.Get(0).CellType = checkBoxCellType2;
            this.SSList_Sheet1.Columns.Get(0).Label = "False";
            this.SSList_Sheet1.Columns.Get(0).Width = 29F;
            this.SSList_Sheet1.Columns.Get(1).AllowAutoSort = true;
            this.SSList_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(1).Label = "코드";
            this.SSList_Sheet1.Columns.Get(1).Width = 61F;
            this.SSList_Sheet1.Columns.Get(2).AllowAutoSort = true;
            this.SSList_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SSList_Sheet1.Columns.Get(2).Label = "회사명";
            this.SSList_Sheet1.Columns.Get(2).Width = 153F;
            this.SSList_Sheet1.Columns.Get(3).Label = "방문1";
            this.SSList_Sheet1.Columns.Get(3).Width = 147F;
            this.SSList_Sheet1.Columns.Get(4).Label = "방문2";
            this.SSList_Sheet1.Columns.Get(4).Width = 150F;
            this.SSList_Sheet1.Columns.Get(5).Label = "방문3";
            this.SSList_Sheet1.Columns.Get(5).Width = 154F;
            this.SSList_Sheet1.Columns.Get(6).Label = "방문4";
            this.SSList_Sheet1.Columns.Get(6).Width = 142F;
            this.SSList_Sheet1.Columns.Get(7).Label = "방문5";
            this.SSList_Sheet1.Columns.Get(7).Width = 129F;
            this.SSList_Sheet1.Columns.Get(8).CellType = textCellType1;
            this.SSList_Sheet1.Columns.Get(8).Label = "Email1";
            this.SSList_Sheet1.Columns.Get(8).Width = 160F;
            this.SSList_Sheet1.Columns.Get(9).CellType = textCellType2;
            this.SSList_Sheet1.Columns.Get(9).Label = "Email2";
            this.SSList_Sheet1.Columns.Get(9).Width = 138F;
            this.SSList_Sheet1.Columns.Get(10).CellType = textCellType3;
            this.SSList_Sheet1.Columns.Get(10).Label = "Email3";
            this.SSList_Sheet1.Columns.Get(10).Width = 95F;
            this.SSList_Sheet1.Columns.Get(11).CellType = textCellType4;
            this.SSList_Sheet1.Columns.Get(11).Label = "Email4";
            this.SSList_Sheet1.Columns.Get(11).Width = 90F;
            textCellType5.BackgroundImage = new FarPoint.Win.Picture(null, FarPoint.Win.RenderStyle.Normal, System.Drawing.Color.Empty, 0, FarPoint.Win.HorizontalAlignment.Center, FarPoint.Win.VerticalAlignment.Center);
            this.SSList_Sheet1.Columns.Get(12).CellType = textCellType5;
            this.SSList_Sheet1.Columns.Get(12).Label = "메일 발송일";
            this.SSList_Sheet1.Columns.Get(12).Width = 100F;
            textCellType6.BackgroundImage = new FarPoint.Win.Picture(null, FarPoint.Win.RenderStyle.Normal, System.Drawing.Color.Empty, 0, FarPoint.Win.HorizontalAlignment.Center, FarPoint.Win.VerticalAlignment.Center);
            this.SSList_Sheet1.Columns.Get(13).CellType = textCellType6;
            this.SSList_Sheet1.Columns.Get(13).Label = "발송자";
            this.SSList_Sheet1.Columns.Get(13).Width = 80F;
            generalCellType1.ReadOnly = true;
            this.SSList_Sheet1.Columns.Get(14).CellType = generalCellType1;
            this.SSList_Sheet1.Columns.Get(14).Label = "계약ID";
            this.SSList_Sheet1.Columns.Get(14).Visible = false;
            this.SSList_Sheet1.Columns.Get(15).Label = "IDs";
            this.SSList_Sheet1.Columns.Get(15).Visible = false;
            this.SSList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // btnPRint
            // 
            this.btnPRint.Location = new System.Drawing.Point(1118, 8);
            this.btnPRint.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.btnPRint.Name = "btnPRint";
            this.btnPRint.Size = new System.Drawing.Size(56, 28);
            this.btnPRint.TabIndex = 115;
            this.btnPRint.Text = "인쇄";
            this.btnPRint.UseVisualStyleBackColor = true;
            this.btnPRint.Click += new System.EventHandler(this.btnPRint_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(1177, 2);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 28);
            this.btnClose.TabIndex = 116;
            this.btnClose.Text = "닫기";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSendMail
            // 
            this.btnSendMail.Location = new System.Drawing.Point(1180, 8);
            this.btnSendMail.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.btnSendMail.Name = "btnSendMail";
            this.btnSendMail.Size = new System.Drawing.Size(75, 28);
            this.btnSendMail.TabIndex = 117;
            this.btnSendMail.Text = "메일발송";
            this.btnSendMail.UseVisualStyleBackColor = true;
            this.btnSendMail.Click += new System.EventHandler(this.btnSendMail_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.chkMailSend);
            this.panel1.Controls.Add(this.CboManager);
            this.panel1.Controls.Add(this.txtAttach);
            this.panel1.Controls.Add(this.btnAttach);
            this.panel1.Controls.Add(this.CboMonth);
            this.panel1.Controls.Add(this.BtnExportPdf);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.btnSendMail);
            this.panel1.Controls.Add(this.btnPRint);
            this.panel1.Controls.Add(this.TxtDocNumber);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.BtnSearch);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.DtpDate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1264, 60);
            this.panel1.TabIndex = 118;
            // 
            // chkMailSend
            // 
            this.chkMailSend.AutoSize = true;
            this.chkMailSend.Location = new System.Drawing.Point(289, 38);
            this.chkMailSend.Name = "chkMailSend";
            this.chkMailSend.Size = new System.Drawing.Size(198, 21);
            this.chkMailSend.TabIndex = 128;
            this.chkMailSend.Text = "메일 미전송 방문일정만 찾기";
            this.chkMailSend.UseVisualStyleBackColor = true;
            // 
            // CboManager
            // 
            this.CboManager.FormattingEnabled = true;
            this.CboManager.Location = new System.Drawing.Point(472, 9);
            this.CboManager.Name = "CboManager";
            this.CboManager.Size = new System.Drawing.Size(122, 25);
            this.CboManager.TabIndex = 3;
            // 
            // txtAttach
            // 
            this.txtAttach.Location = new System.Drawing.Point(770, 3);
            this.txtAttach.Multiline = true;
            this.txtAttach.Name = "txtAttach";
            this.txtAttach.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAttach.Size = new System.Drawing.Size(260, 51);
            this.txtAttach.TabIndex = 127;
            // 
            // btnAttach
            // 
            this.btnAttach.Location = new System.Drawing.Point(694, 9);
            this.btnAttach.Name = "btnAttach";
            this.btnAttach.Size = new System.Drawing.Size(70, 26);
            this.btnAttach.TabIndex = 126;
            this.btnAttach.Text = "첨부파일";
            this.btnAttach.UseVisualStyleBackColor = true;
            this.btnAttach.Click += new System.EventHandler(this.btnAttach_Click);
            // 
            // CboMonth
            // 
            this.CboMonth.FormattingEnabled = true;
            this.CboMonth.Location = new System.Drawing.Point(66, 8);
            this.CboMonth.Name = "CboMonth";
            this.CboMonth.Size = new System.Drawing.Size(71, 25);
            this.CboMonth.TabIndex = 0;
            // 
            // BtnExportPdf
            // 
            this.BtnExportPdf.Location = new System.Drawing.Point(1040, 8);
            this.BtnExportPdf.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnExportPdf.Name = "BtnExportPdf";
            this.BtnExportPdf.Size = new System.Drawing.Size(72, 28);
            this.BtnExportPdf.TabIndex = 118;
            this.BtnExportPdf.Text = "PDF 저장";
            this.BtnExportPdf.UseVisualStyleBackColor = true;
            this.BtnExportPdf.Click += new System.EventHandler(this.BtnExportPdf_Click);
            // 
            // VIsitDocumentBatchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 871);
            this.Controls.Add(this.SSList);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.contentTitle1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "VIsitDocumentBatchForm";
            this.Text = "VIsitDocumentBatchForm";
            this.Load += new System.EventHandler(this.VIsitDocumentBatchForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formulaProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox TxtDocNumber;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker DtpDate;
        private System.Windows.Forms.Button BtnSearch;
        private ComBase.Mvc.UserControls.ContentTitle contentTitle1;
        private FarPoint.Win.Spread.FpSpread SSList;
        private FarPoint.Win.Spread.SheetView SSList_Sheet1;
        private System.Windows.Forms.Button btnPRint;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSendMail;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button BtnExportPdf;
        private System.Windows.Forms.ComboBox CboMonth;
        private System.Windows.Forms.TextBox txtAttach;
        private System.Windows.Forms.Button btnAttach;
        private System.Windows.Forms.ComboBox CboManager;
        private System.Windows.Forms.CheckBox chkMailSend;
        private FarPoint.Win.Spread.FormulaProvider formulaProvider1;
    }
}