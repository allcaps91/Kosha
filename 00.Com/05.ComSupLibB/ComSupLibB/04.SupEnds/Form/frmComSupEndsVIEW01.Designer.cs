namespace ComSupLibB.SupEnds
{
    partial class frmComSupEndsVIEW01
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
            FarPoint.Win.EmptyBorder emptyBorder1 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder2 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder3 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder4 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder5 = new FarPoint.Win.EmptyBorder();
            this.panheader4 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dtpTDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.panbtn1 = new System.Windows.Forms.Panel();
            this.btnSearch1 = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.panel20 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.line1 = new DevComponents.DotNetBar.Controls.Line();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ssList = new FarPoint.Win.Spread.FpSpread();
            this.ssList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panheader4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panbtn1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panheader4
            // 
            this.panheader4.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panheader4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panheader4.Controls.Add(this.label2);
            this.panheader4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panheader4.Location = new System.Drawing.Point(0, 0);
            this.panheader4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panheader4.Name = "panheader4";
            this.panheader4.Size = new System.Drawing.Size(823, 37);
            this.panheader4.TabIndex = 133;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(4, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 17);
            this.label2.TabIndex = 41;
            this.label2.Text = "내시경 전화통보 명단";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panbtn1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 37);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(823, 44);
            this.panel1.TabIndex = 137;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dtpTDate);
            this.panel3.Controls.Add(this.dtpFDate);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(2, 7, 3, 7);
            this.panel3.Size = new System.Drawing.Size(270, 44);
            this.panel3.TabIndex = 31;
            // 
            // dtpTDate
            // 
            this.dtpTDate.CustomFormat = "yyyy-MM-dd";
            this.dtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTDate.Location = new System.Drawing.Point(169, 8);
            this.dtpTDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpTDate.Name = "dtpTDate";
            this.dtpTDate.Size = new System.Drawing.Size(87, 25);
            this.dtpTDate.TabIndex = 8;
            // 
            // dtpFDate
            // 
            this.dtpFDate.CustomFormat = "yyyy-MM-dd";
            this.dtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFDate.Location = new System.Drawing.Point(70, 8);
            this.dtpFDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpFDate.Name = "dtpFDate";
            this.dtpFDate.Size = new System.Drawing.Size(89, 25);
            this.dtpFDate.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 17);
            this.label1.TabIndex = 9;
            this.label1.Text = "조회기간";
            // 
            // panbtn1
            // 
            this.panbtn1.Controls.Add(this.btnSearch1);
            this.panbtn1.Controls.Add(this.panel5);
            this.panbtn1.Controls.Add(this.btnPrint);
            this.panbtn1.Controls.Add(this.panel20);
            this.panbtn1.Controls.Add(this.btnExit);
            this.panbtn1.Controls.Add(this.panel4);
            this.panbtn1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panbtn1.Location = new System.Drawing.Point(574, 0);
            this.panbtn1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panbtn1.Name = "panbtn1";
            this.panbtn1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panbtn1.Size = new System.Drawing.Size(249, 44);
            this.panbtn1.TabIndex = 30;
            // 
            // btnSearch1
            // 
            this.btnSearch1.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch1.Location = new System.Drawing.Point(30, 4);
            this.btnSearch1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearch1.Name = "btnSearch1";
            this.btnSearch1.Size = new System.Drawing.Size(70, 36);
            this.btnSearch1.TabIndex = 34;
            this.btnSearch1.Text = "조회";
            this.btnSearch1.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(100, 4);
            this.panel5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(2, 36);
            this.panel5.TabIndex = 168;
            // 
            // btnPrint
            // 
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrint.Location = new System.Drawing.Point(102, 4);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(70, 36);
            this.btnPrint.TabIndex = 30;
            this.btnPrint.Text = "인쇄";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // panel20
            // 
            this.panel20.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel20.Location = new System.Drawing.Point(172, 4);
            this.panel20.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel20.Name = "panel20";
            this.panel20.Size = new System.Drawing.Size(2, 36);
            this.panel20.TabIndex = 166;
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(174, 4);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(70, 36);
            this.btnExit.TabIndex = 29;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(244, 4);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(2, 36);
            this.panel4.TabIndex = 167;
            // 
            // line1
            // 
            this.line1.Dock = System.Windows.Forms.DockStyle.Top;
            this.line1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.line1.Location = new System.Drawing.Point(0, 81);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(823, 8);
            this.line1.TabIndex = 139;
            this.line1.Text = "line1";
            this.line1.Thickness = 5;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ssList);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 89);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(823, 415);
            this.panel2.TabIndex = 140;
            // 
            // ssList
            // 
            this.ssList.AccessibleDescription = "";
            this.ssList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ssList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssList.Location = new System.Drawing.Point(0, 0);
            this.ssList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ssList.Name = "ssList";
            this.ssList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList_Sheet1});
            this.ssList.Size = new System.Drawing.Size(823, 415);
            this.ssList.TabIndex = 2;
            // 
            // ssList_Sheet1
            // 
            this.ssList_Sheet1.Reset();
            this.ssList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssList_Sheet1.ColumnCount = 5;
            this.ssList_Sheet1.RowCount = 1;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.Columns.Get(0).Border = emptyBorder1;
            this.ssList_Sheet1.Columns.Get(1).Border = emptyBorder2;
            this.ssList_Sheet1.Columns.Get(2).Border = emptyBorder3;
            this.ssList_Sheet1.Columns.Get(3).Border = emptyBorder4;
            this.ssList_Sheet1.Columns.Get(4).Border = emptyBorder5;
            this.ssList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ssList_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Silver);
            this.ssList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Silver);
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmComSupEndsVIEW01
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(823, 504);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.line1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panheader4);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmComSupEndsVIEW01";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmComSupEndsViewTelList";
            this.panheader4.ResumeLayout(false);
            this.panheader4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panbtn1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panheader4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DateTimePicker dtpTDate;
        private System.Windows.Forms.DateTimePicker dtpFDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panbtn1;
        private System.Windows.Forms.Button btnSearch1;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel20;
        private System.Windows.Forms.Panel panel4;
        private DevComponents.DotNetBar.Controls.Line line1;
        private System.Windows.Forms.Panel panel2;
        private FarPoint.Win.Spread.FpSpread ssList;
        private FarPoint.Win.Spread.SheetView ssList_Sheet1;
    }
}