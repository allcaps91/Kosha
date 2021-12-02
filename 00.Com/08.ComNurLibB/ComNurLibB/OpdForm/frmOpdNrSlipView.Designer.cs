namespace ComNurLibB
{
    partial class frmOpdNrSlipView
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panDisp = new System.Windows.Forms.Panel();
            this.chkIpwon = new System.Windows.Forms.CheckBox();
            this.cboDEPT = new System.Windows.Forms.ComboBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.dtpTDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.panMain = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ssView2 = new FarPoint.Win.Spread.FpSpread();
            this.ssView2_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ssView1 = new FarPoint.Win.Spread.FpSpread();
            this.ssView1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitleSub0.SuspendLayout();
            this.panDisp.SuspendLayout();
            this.panMain.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView2_Sheet1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 0);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(1216, 28);
            this.panTitleSub0.TabIndex = 16;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(5, 1);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(253, 19);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "외래 진료과별 특정코드 수납내역 확인";
            // 
            // panDisp
            // 
            this.panDisp.Controls.Add(this.chkIpwon);
            this.panDisp.Controls.Add(this.cboDEPT);
            this.panDisp.Controls.Add(this.btnExit);
            this.panDisp.Controls.Add(this.btnSearch);
            this.panDisp.Controls.Add(this.btnPrint);
            this.panDisp.Controls.Add(this.dtpTDate);
            this.panDisp.Controls.Add(this.dtpFDate);
            this.panDisp.Controls.Add(this.label1);
            this.panDisp.Controls.Add(this.btnSave);
            this.panDisp.Controls.Add(this.label5);
            this.panDisp.Dock = System.Windows.Forms.DockStyle.Top;
            this.panDisp.Location = new System.Drawing.Point(0, 28);
            this.panDisp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panDisp.Name = "panDisp";
            this.panDisp.Size = new System.Drawing.Size(1216, 38);
            this.panDisp.TabIndex = 24;
            // 
            // chkIpwon
            // 
            this.chkIpwon.AutoSize = true;
            this.chkIpwon.Location = new System.Drawing.Point(505, 10);
            this.chkIpwon.Name = "chkIpwon";
            this.chkIpwon.Size = new System.Drawing.Size(160, 16);
            this.chkIpwon.TabIndex = 16;
            this.chkIpwon.Text = "외래 처방 입원 전송 내역";
            this.chkIpwon.UseVisualStyleBackColor = true;
            // 
            // cboDEPT
            // 
            this.cboDEPT.FormattingEnabled = true;
            this.cboDEPT.Location = new System.Drawing.Point(65, 8);
            this.cboDEPT.Name = "cboDEPT";
            this.cboDEPT.Size = new System.Drawing.Size(121, 20);
            this.cboDEPT.TabIndex = 15;
            this.cboDEPT.SelectedIndexChanged += new System.EventHandler(this.cboDEPT_SelectedIndexChanged);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(1132, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 14;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(913, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 13;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPrint.Location = new System.Drawing.Point(1059, 3);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 30);
            this.btnPrint.TabIndex = 12;
            this.btnPrint.Text = "출력";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // dtpTDate
            // 
            this.dtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTDate.Location = new System.Drawing.Point(392, 8);
            this.dtpTDate.Name = "dtpTDate";
            this.dtpTDate.Size = new System.Drawing.Size(98, 21);
            this.dtpTDate.TabIndex = 10;
            // 
            // dtpFDate
            // 
            this.dtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFDate.Location = new System.Drawing.Point(288, 8);
            this.dtpFDate.Name = "dtpFDate";
            this.dtpFDate.Size = new System.Drawing.Size(98, 21);
            this.dtpFDate.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(222, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 17);
            this.label1.TabIndex = 9;
            this.label1.Text = "의뢰일자";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave.Location = new System.Drawing.Point(986, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 30);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(12, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 17);
            this.label5.TabIndex = 4;
            this.label5.Text = "진료과";
            // 
            // panMain
            // 
            this.panMain.Controls.Add(this.panel2);
            this.panMain.Controls.Add(this.panel1);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(0, 66);
            this.panMain.Name = "panMain";
            this.panMain.Size = new System.Drawing.Size(1216, 505);
            this.panMain.TabIndex = 25;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ssView2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(288, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(928, 505);
            this.panel2.TabIndex = 1;
            // 
            // ssView2
            // 
            this.ssView2.AccessibleDescription = "ssView2, Sheet1, Row 0, Column 0,  ";
            this.ssView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView2.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssView2.Location = new System.Drawing.Point(0, 0);
            this.ssView2.Name = "ssView2";
            this.ssView2.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView2_Sheet1});
            this.ssView2.Size = new System.Drawing.Size(928, 505);
            this.ssView2.TabIndex = 1;
            this.ssView2.ButtonClicked += new FarPoint.Win.Spread.EditorNotifyEventHandler(this.ssView2_ButtonClicked);
            // 
            // ssView2_Sheet1
            // 
            this.ssView2_Sheet1.Reset();
            this.ssView2_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView2_Sheet1.ColumnCount = 8;
            this.ssView2_Sheet1.RowCount = 1;
            this.ssView2_Sheet1.Cells.Get(0, 0).Value = " ";
            this.ssView2_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView2_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView2_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssView2_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView2_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "등록번호";
            this.ssView2_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "성명";
            this.ssView2_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "수량";
            this.ssView2_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "오더코드";
            this.ssView2_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "오더명칭";
            this.ssView2_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "처방일자";
            this.ssView2_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = " ";
            this.ssView2_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "외래수납여부";
            this.ssView2_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssView2_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(0).Label = "등록번호";
            this.ssView2_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(0).Width = 100F;
            this.ssView2_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssView2_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(1).Label = "성명";
            this.ssView2_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(1).Width = 80F;
            this.ssView2_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssView2_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(2).Label = "수량";
            this.ssView2_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(2).Width = 50F;
            this.ssView2_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssView2_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(3).Label = "오더코드";
            this.ssView2_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(3).Width = 80F;
            this.ssView2_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.ssView2_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssView2_Sheet1.Columns.Get(4).Label = "오더명칭";
            this.ssView2_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(4).Width = 320F;
            this.ssView2_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.ssView2_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(5).Label = "처방일자";
            this.ssView2_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(5).Width = 80F;
            this.ssView2_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(6).Label = " ";
            this.ssView2_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(7).CellType = textCellType7;
            this.ssView2_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(7).Label = "외래수납여부";
            this.ssView2_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(7).Width = 100F;
            this.ssView2_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView2_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView2_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssView2_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView2_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ssView1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(288, 505);
            this.panel1.TabIndex = 0;
            // 
            // ssView1
            // 
            this.ssView1.AccessibleDescription = "ssView1, Sheet1, Row 0, Column 0, ";
            this.ssView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssView1.Location = new System.Drawing.Point(0, 0);
            this.ssView1.Name = "ssView1";
            this.ssView1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView1_Sheet1});
            this.ssView1.Size = new System.Drawing.Size(288, 505);
            this.ssView1.TabIndex = 1;
            this.ssView1.LeaveCell += new FarPoint.Win.Spread.LeaveCellEventHandler(this.ssView1_LeaveCell);
            // 
            // ssView1_Sheet1
            // 
            this.ssView1_Sheet1.Reset();
            this.ssView1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView1_Sheet1.ColumnCount = 2;
            this.ssView1_Sheet1.RowCount = 1;
            this.ssView1_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssView1_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "코드";
            this.ssView1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "명칭";
            this.ssView1_Sheet1.Columns.Get(0).Label = "코드";
            this.ssView1_Sheet1.Columns.Get(0).Width = 80F;
            this.ssView1_Sheet1.Columns.Get(1).Label = "명칭";
            this.ssView1_Sheet1.Columns.Get(1).Width = 150F;
            this.ssView1_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView1_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView1_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssView1_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmOpdNrSlipView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1216, 571);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.panDisp);
            this.Controls.Add(this.panTitleSub0);
            this.Name = "frmOpdNrSlipView";
            this.Text = "frmOpdNrSlipView";
            this.Load += new System.EventHandler(this.frmOpdNrSlipView_Load);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panDisp.ResumeLayout(false);
            this.panDisp.PerformLayout();
            this.panMain.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView2_Sheet1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panDisp;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.DateTimePicker dtpFDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panMain;
        private System.Windows.Forms.Panel panel2;
        private FarPoint.Win.Spread.FpSpread ssView2;
        private FarPoint.Win.Spread.SheetView ssView2_Sheet1;
        private System.Windows.Forms.Panel panel1;
        private FarPoint.Win.Spread.FpSpread ssView1;
        private FarPoint.Win.Spread.SheetView ssView1_Sheet1;
        private System.Windows.Forms.CheckBox chkIpwon;
        private System.Windows.Forms.ComboBox cboDEPT;
        private System.Windows.Forms.DateTimePicker dtpTDate;
    }
}