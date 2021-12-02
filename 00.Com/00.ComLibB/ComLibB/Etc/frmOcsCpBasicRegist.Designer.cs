namespace ComLibB
{
    partial class frmOcsCpBasicRegist
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
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer2 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            FarPoint.Win.Spread.CellType.TextCellType textCellType16 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType17 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType18 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType19 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType20 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType21 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType22 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType23 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType24 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType25 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType26 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType27 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType28 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.DateTimeCellType dateTimeCellType2 = new FarPoint.Win.Spread.CellType.DateTimeCellType();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOcsCpBasicRegist));
            FarPoint.Win.Spread.CellType.TextCellType textCellType29 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType2 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType30 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ssCp = new FarPoint.Win.Spread.FpSpread();
            this.ssCp_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.cboCP = new System.Windows.Forms.ComboBox();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssCp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssCp_Sheet1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnSave);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1528, 34);
            this.panTitle.TabIndex = 13;
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(1380, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 30);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(1452, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 9;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(138, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "CP 기초코드 관리";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ssCp);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1528, 617);
            this.panel1.TabIndex = 14;
            // 
            // ssCp
            // 
            this.ssCp.AccessibleDescription = "ssCp, Sheet1, Row 0, Column 0, ";
            this.ssCp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssCp.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssCp.Location = new System.Drawing.Point(0, 34);
            this.ssCp.Name = "ssCp";
            this.ssCp.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssCp_Sheet1});
            this.ssCp.Size = new System.Drawing.Size(1528, 583);
            this.ssCp.TabIndex = 24;
            this.ssCp.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssCp.VerticalScrollBar.Name = "";
            this.ssCp.VerticalScrollBar.Renderer = enhancedScrollBarRenderer2;
            this.ssCp.VerticalScrollBar.TabIndex = 10;
            // 
            // ssCp_Sheet1
            // 
            this.ssCp_Sheet1.Reset();
            this.ssCp_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssCp_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssCp_Sheet1.ColumnCount = 17;
            this.ssCp_Sheet1.RowCount = 1;
            this.ssCp_Sheet1.Cells.Get(0, 13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssCp_Sheet1.Cells.Get(0, 13).Value = new System.DateTime(2018, 11, 3, 0, 0, 0, 0);
            this.ssCp_Sheet1.Cells.Get(0, 13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCp_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssCp_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssCp_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssCp_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssCp_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "코드";
            this.ssCp_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "명칭";
            this.ssCp_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "명칭(확장)";
            this.ssCp_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "문자값1";
            this.ssCp_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "문자값2";
            this.ssCp_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "문자값3";
            this.ssCp_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "문자값4";
            this.ssCp_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "숫자값1";
            this.ssCp_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "숫자값2";
            this.ssCp_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "숫자값3";
            this.ssCp_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "숫자값4";
            this.ssCp_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "비고1";
            this.ssCp_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "비고2";
            this.ssCp_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "적용일자";
            this.ssCp_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "종료일자";
            this.ssCp_Sheet1.ColumnHeader.Cells.Get(0, 15).Value = "사용";
            this.ssCp_Sheet1.ColumnHeader.Cells.Get(0, 16).Value = "신규";
            this.ssCp_Sheet1.ColumnHeader.Rows.Get(0).Height = 33F;
            this.ssCp_Sheet1.Columns.Get(0).CellType = textCellType16;
            this.ssCp_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssCp_Sheet1.Columns.Get(0).Label = "코드";
            this.ssCp_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(0).Width = 150F;
            this.ssCp_Sheet1.Columns.Get(1).CellType = textCellType17;
            this.ssCp_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssCp_Sheet1.Columns.Get(1).Label = "명칭";
            this.ssCp_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(1).Width = 358F;
            this.ssCp_Sheet1.Columns.Get(2).CellType = textCellType18;
            this.ssCp_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssCp_Sheet1.Columns.Get(2).Label = "명칭(확장)";
            this.ssCp_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(2).Width = 90F;
            this.ssCp_Sheet1.Columns.Get(3).CellType = textCellType19;
            this.ssCp_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(3).Label = "문자값1";
            this.ssCp_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(4).CellType = textCellType20;
            this.ssCp_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(4).Label = "문자값2";
            this.ssCp_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(5).CellType = textCellType21;
            this.ssCp_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(5).Label = "문자값3";
            this.ssCp_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(6).CellType = textCellType22;
            this.ssCp_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(6).Label = "문자값4";
            this.ssCp_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(7).CellType = textCellType23;
            this.ssCp_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(7).Label = "숫자값1";
            this.ssCp_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(8).CellType = textCellType24;
            this.ssCp_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(8).Label = "숫자값2";
            this.ssCp_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(9).CellType = textCellType25;
            this.ssCp_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(9).Label = "숫자값3";
            this.ssCp_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(10).CellType = textCellType26;
            this.ssCp_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(10).Label = "숫자값4";
            this.ssCp_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(11).CellType = textCellType27;
            this.ssCp_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssCp_Sheet1.Columns.Get(11).Label = "비고1";
            this.ssCp_Sheet1.Columns.Get(11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(11).Width = 99F;
            this.ssCp_Sheet1.Columns.Get(12).CellType = textCellType28;
            this.ssCp_Sheet1.Columns.Get(12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssCp_Sheet1.Columns.Get(12).Label = "비고2";
            this.ssCp_Sheet1.Columns.Get(12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(12).Width = 82F;
            dateTimeCellType2.Calendar = new System.Globalization.GregorianCalendar(System.Globalization.GregorianCalendarTypes.Localized);
            dateTimeCellType2.CalendarSurroundingDaysColor = System.Drawing.SystemColors.GrayText;
            dateTimeCellType2.MaximumTime = System.TimeSpan.Parse("23:59:59.9999999");
            dateTimeCellType2.TimeDefault = new System.DateTime(2021, 8, 25, 11, 35, 37, 0);
            this.ssCp_Sheet1.Columns.Get(13).CellType = dateTimeCellType2;
            this.ssCp_Sheet1.Columns.Get(13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(13).Label = "적용일자";
            this.ssCp_Sheet1.Columns.Get(13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(13).Width = 79F;
            this.ssCp_Sheet1.Columns.Get(14).CellType = textCellType29;
            this.ssCp_Sheet1.Columns.Get(14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(14).Label = "종료일자";
            this.ssCp_Sheet1.Columns.Get(14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(14).Width = 79F;
            this.ssCp_Sheet1.Columns.Get(15).CellType = checkBoxCellType2;
            this.ssCp_Sheet1.Columns.Get(15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(15).Label = "사용";
            this.ssCp_Sheet1.Columns.Get(15).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(15).Width = 30F;
            this.ssCp_Sheet1.Columns.Get(16).CellType = textCellType30;
            this.ssCp_Sheet1.Columns.Get(16).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(16).Label = "신규";
            this.ssCp_Sheet1.Columns.Get(16).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssCp_Sheet1.Columns.Get(16).Visible = false;
            this.ssCp_Sheet1.Columns.Get(16).Width = 30F;
            this.ssCp_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssCp_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssCp_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssCp_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssCp_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssCp_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel2.Controls.Add(this.btnDown);
            this.panel2.Controls.Add(this.btnUp);
            this.panel2.Controls.Add(this.btnDelete);
            this.panel2.Controls.Add(this.btnAdd);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.cboCP);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1528, 34);
            this.panel2.TabIndex = 23;
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(513, 2);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(72, 30);
            this.btnDown.TabIndex = 22;
            this.btnDown.Text = "▼ 아래";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(441, 2);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(72, 30);
            this.btnUp.TabIndex = 22;
            this.btnUp.Text = "▲ 위로";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(351, 2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(72, 30);
            this.btnDelete.TabIndex = 22;
            this.btnDelete.Text = "삭제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(279, 2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(72, 30);
            this.btnAdd.TabIndex = 22;
            this.btnAdd.Text = "추가";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.RoyalBlue;
            this.label6.Font = new System.Drawing.Font("맑은 고딕", 10.75F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(12, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 20);
            this.label6.TabIndex = 20;
            this.label6.Text = "CP 관리";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboCP
            // 
            this.cboCP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCP.FormattingEnabled = true;
            this.cboCP.Location = new System.Drawing.Point(75, 4);
            this.cboCP.Name = "cboCP";
            this.cboCP.Size = new System.Drawing.Size(185, 25);
            this.cboCP.TabIndex = 21;
            this.cboCP.SelectedIndexChanged += new System.EventHandler(this.cboCP_SelectedIndexChanged);
            // 
            // frmOcsCpBasicRegist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1528, 651);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmOcsCpBasicRegist";
            this.Text = "frmOcsCpBasicRegist";
            this.Activated += new System.EventHandler(this.frmOcsCpBasicRegist_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmOcsCpBasicRegist_FormClosed);
            this.Load += new System.EventHandler(this.frmOcsCpBasicRegist_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssCp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssCp_Sheet1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboCP;
        private FarPoint.Win.Spread.FpSpread ssCp;
        private FarPoint.Win.Spread.SheetView ssCp_Sheet1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDelete;
    }
}