namespace ComPmpaLibB
{
    partial class frmPmpaViewDoctor
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
            FarPoint.Win.Spread.NamedStyle namedStyle9 = new FarPoint.Win.Spread.NamedStyle("BorderEx402636383209149505092", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder9 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.NamedStyle namedStyle10 = new FarPoint.Win.Spread.NamedStyle("Static506636383209149545203", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder10 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance5 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.EmptyBorder emptyBorder21 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder22 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder23 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder24 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder25 = new FarPoint.Win.EmptyBorder();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.txtDrName = new System.Windows.Forms.TextBox();
            this.ssList = new FarPoint.Win.Spread.FpSpread();
            this.ssList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitleSub0.SuspendLayout();
            this.panTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(410, 28);
            this.panTitleSub0.TabIndex = 114;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(12, 6);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(65, 17);
            this.lblTitleSub0.TabIndex = 22;
            this.lblTitleSub0.Text = "조회 결과";
            this.lblTitleSub0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.txtDrName);
            this.panTitle.Controls.Add(this.btnView);
            this.panTitle.Controls.Add(this.label15);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(410, 34);
            this.panTitle.TabIndex = 113;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.ForeColor = System.Drawing.Color.Black;
            this.label15.Location = new System.Drawing.Point(9, 7);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(60, 17);
            this.label15.TabIndex = 83;
            this.label15.Text = "의사명 : ";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(332, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // btnView
            // 
            this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnView.AutoSize = true;
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnView.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnView.Location = new System.Drawing.Point(257, 1);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(72, 30);
            this.btnView.TabIndex = 84;
            this.btnView.Text = "조회";
            this.btnView.UseVisualStyleBackColor = true;
            // 
            // txtDrName
            // 
            this.txtDrName.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.txtDrName.Location = new System.Drawing.Point(75, 5);
            this.txtDrName.Name = "txtDrName";
            this.txtDrName.Size = new System.Drawing.Size(100, 21);
            this.txtDrName.TabIndex = 116;
            this.txtDrName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDrName_KeyPress);
            // 
            // ssList
            // 
            this.ssList.AccessibleDescription = "";
            this.ssList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssList.Location = new System.Drawing.Point(0, 62);
            this.ssList.Name = "ssList";
            namedStyle9.Border = complexBorder9;
            namedStyle9.Font = new System.Drawing.Font("굴림", 11F);
            namedStyle9.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle9.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle9.Parent = "DataAreaDefault";
            namedStyle9.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle10.Border = complexBorder10;
            textCellType5.Static = true;
            namedStyle10.CellType = textCellType5;
            namedStyle10.Font = new System.Drawing.Font("굴림", 11F);
            namedStyle10.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle10.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle10.Parent = "DataAreaDefault";
            namedStyle10.Renderer = textCellType5;
            namedStyle10.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle9,
            namedStyle10});
            this.ssList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList_Sheet1});
            this.ssList.Size = new System.Drawing.Size(410, 199);
            this.ssList.TabIndex = 115;
            this.ssList.TabStripRatio = 0.6D;
            tipAppearance5.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance5.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssList.TextTipAppearance = tipAppearance5;
            this.ssList.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // ssList_Sheet1
            // 
            this.ssList_Sheet1.Reset();
            this.ssList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssList_Sheet1.ColumnCount = 5;
            this.ssList_Sheet1.RowCount = 1;
            this.ssList_Sheet1.Cells.Get(0, 0).Value = "7910";
            this.ssList_Sheet1.Cells.Get(0, 1).Value = "EE";
            this.ssList_Sheet1.Cells.Get(0, 2).Value = "사공순덕";
            this.ssList_Sheet1.Cells.Get(0, 3).Value = "799852";
            this.ssList_Sheet1.Cells.Get(0, 4).Value = "Y";
            this.ssList_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "의사코드";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "진료과";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "의사명";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "면허번호";
            this.ssList_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "선택유무";
            this.ssList_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssList_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ssList_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssList_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.ColumnHeader.Rows.Get(0).Height = 34F;
            this.ssList_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssList_Sheet1.Columns.Get(0).Border = emptyBorder21;
            this.ssList_Sheet1.Columns.Get(0).Label = "의사코드";
            this.ssList_Sheet1.Columns.Get(0).Width = 80F;
            this.ssList_Sheet1.Columns.Get(1).Border = emptyBorder22;
            this.ssList_Sheet1.Columns.Get(1).Label = "진료과";
            this.ssList_Sheet1.Columns.Get(1).Width = 59F;
            this.ssList_Sheet1.Columns.Get(2).Border = emptyBorder23;
            this.ssList_Sheet1.Columns.Get(2).Label = "의사명";
            this.ssList_Sheet1.Columns.Get(2).Width = 93F;
            this.ssList_Sheet1.Columns.Get(3).Border = emptyBorder24;
            this.ssList_Sheet1.Columns.Get(3).Label = "면허번호";
            this.ssList_Sheet1.Columns.Get(3).Width = 87F;
            this.ssList_Sheet1.Columns.Get(4).Border = emptyBorder25;
            this.ssList_Sheet1.Columns.Get(4).Label = "선택유무";
            this.ssList_Sheet1.Columns.Get(4).Width = 48F;
            this.ssList_Sheet1.DefaultStyleName = "Static506636383209149545203";
            this.ssList_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Black);
            this.ssList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ssList_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssList_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.Rows.Default.Height = 24F;
            this.ssList_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ssList_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssList_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Black);
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmPmpaViewDoctor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(410, 261);
            this.Controls.Add(this.ssList);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Name = "frmPmpaViewDoctor";
            this.Text = "의사정보 조회";
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.TextBox txtDrName;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Label label15;
        public System.Windows.Forms.Button btnExit;
        private FarPoint.Win.Spread.FpSpread ssList;
        private FarPoint.Win.Spread.SheetView ssList_Sheet1;
    }
}