namespace ComBase
{
    partial class frmPainScore3
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
            FarPoint.Win.Spread.NamedStyle namedStyle7 = new FarPoint.Win.Spread.NamedStyle("BorderEx401636536975595512848", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder3 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.NamedStyle namedStyle8 = new FarPoint.Win.Spread.NamedStyle("Text545636536975595512848", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder4 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle9 = new FarPoint.Win.Spread.NamedStyle("Static654636536975595512848");
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle10 = new FarPoint.Win.Spread.NamedStyle("Static713636536975595512848");
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle11 = new FarPoint.Win.Spread.NamedStyle("Font876636536975595668732");
            FarPoint.Win.Spread.NamedStyle namedStyle12 = new FarPoint.Win.Spread.NamedStyle("Static894636536975595668732");
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance2 = new FarPoint.Win.Spread.TipAppearance();
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSend = new System.Windows.Forms.Button();
            this.lblItem0 = new System.Windows.Forms.Label();
            this.txtJumsu = new System.Windows.Forms.TextBox();
            this.lblItem1 = new System.Windows.Forms.Label();
            this.panTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(681, 34);
            this.panTitle.TabIndex = 12;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(220, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "NIPS(Neonatal Infant Pain)";
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssView.Location = new System.Drawing.Point(0, 72);
            this.ssView.Name = "ssView";
            namedStyle7.Border = complexBorder3;
            namedStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            namedStyle7.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle7.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle7.Parent = "DataAreaDefault";
            namedStyle7.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle8.Border = complexBorder4;
            textCellType5.MaxLength = 32000;
            namedStyle8.CellType = textCellType5;
            namedStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            namedStyle8.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle8.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle8.Parent = "DataAreaDefault";
            namedStyle8.Renderer = textCellType5;
            namedStyle8.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType6.Static = true;
            textCellType6.WordWrap = true;
            namedStyle9.CellType = textCellType6;
            namedStyle9.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle9.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle9.Renderer = textCellType6;
            namedStyle9.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType7.Static = true;
            textCellType7.WordWrap = true;
            namedStyle10.CellType = textCellType7;
            namedStyle10.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle10.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle10.Renderer = textCellType7;
            namedStyle10.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            namedStyle11.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle11.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle11.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType8.Static = true;
            namedStyle12.CellType = textCellType8;
            namedStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            namedStyle12.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle12.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle12.Renderer = textCellType8;
            namedStyle12.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle7,
            namedStyle8,
            namedStyle9,
            namedStyle10,
            namedStyle11,
            namedStyle12});
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(681, 384);
            this.ssView.TabIndex = 47;
            this.ssView.TabStripRatio = 0.6D;
            tipAppearance2.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance2.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssView.TextTipAppearance = tipAppearance2;
            this.ssView.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssView_CellDoubleClick);
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 6;
            this.ssView_Sheet1.RowCount = 6;
            this.ssView_Sheet1.Cells.Get(0, 0).Value = "얼굴 표정";
            this.ssView_Sheet1.Cells.Get(0, 1).Value = " 편안해 보인다";
            this.ssView_Sheet1.Cells.Get(0, 2).Value = " 긴장되어 있다";
            this.ssView_Sheet1.Cells.Get(0, 4).RowSpan = 5;
            this.ssView_Sheet1.Cells.Get(1, 0).Value = "울음";
            this.ssView_Sheet1.Cells.Get(1, 1).Value = " 조용하다~\r\n 울지 않는다";
            this.ssView_Sheet1.Cells.Get(1, 2).Value = " 경한 신음소리\r\n 가끔씩 운다";
            this.ssView_Sheet1.Cells.Get(1, 3).Value = " 크게 울며 점점소리가  커지면서 지속적으로 운다";
            this.ssView_Sheet1.Cells.Get(2, 0).Value = "숨쉬는 패턴";
            this.ssView_Sheet1.Cells.Get(2, 1).Value = " 편안하다";
            this.ssView_Sheet1.Cells.Get(2, 2).Value = " 숨쉬는 패턴이 불규칙적이고 \r\n 빠르고 gagging 있고 숨을 멈춘다";
            this.ssView_Sheet1.Cells.Get(3, 0).Value = "팔";
            this.ssView_Sheet1.Cells.Get(3, 1).Value = " 편안하다\r\n 근육 강직이 없다\r\n 이따금씩 움직인다";
            this.ssView_Sheet1.Cells.Get(3, 2).Value = " 신전/굴곡, 긴장, 쭉 뻗은 팔\r\n 강직이 있거나 빠른 신전/굴곡";
            this.ssView_Sheet1.Cells.Get(4, 0).Value = "다리";
            this.ssView_Sheet1.Cells.Get(4, 1).Value = " 편안하다\r\n 근육 강직이 없다\r\n 이따금씩 움직인다";
            this.ssView_Sheet1.Cells.Get(4, 2).Value = " 신전/굴곡, 긴장, 쭉 뻗은 팔\r\n 강직이 있거나 빠른 신전/굴곡";
            this.ssView_Sheet1.Cells.Get(5, 0).Value = "깨어 있는 \r\n정도";
            this.ssView_Sheet1.Cells.Get(5, 1).Value = " 자거나 깨어 있다\r\n 조용. 평화롭다\r\n 자거나 혹은 깨어 있을 때\r\n 보채지 않는다.";
            this.ssView_Sheet1.Cells.Get(5, 2).Value = " 보챈다. 의식 명료\r\n 가만히 있지 않고 몸부림 친다";
            this.ssView_Sheet1.ColumnFooter.Columns.Default.Resizable = false;
            this.ssView_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssView_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = " ";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "0점";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "1점";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "2점";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "점수";
            this.ssView_Sheet1.ColumnHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssView_Sheet1.Columns.Default.Resizable = false;
            this.ssView_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.Columns.Get(0).Label = " ";
            this.ssView_Sheet1.Columns.Get(0).StyleName = "Static654636536975595512848";
            this.ssView_Sheet1.Columns.Get(0).Width = 70F;
            this.ssView_Sheet1.Columns.Get(1).Label = "0점";
            this.ssView_Sheet1.Columns.Get(1).StyleName = "Static713636536975595512848";
            this.ssView_Sheet1.Columns.Get(1).Width = 153F;
            this.ssView_Sheet1.Columns.Get(2).Label = "1점";
            this.ssView_Sheet1.Columns.Get(2).StyleName = "Static713636536975595512848";
            this.ssView_Sheet1.Columns.Get(2).Width = 210F;
            this.ssView_Sheet1.Columns.Get(3).Label = "2점";
            this.ssView_Sheet1.Columns.Get(3).StyleName = "Static713636536975595512848";
            this.ssView_Sheet1.Columns.Get(3).Width = 165F;
            this.ssView_Sheet1.Columns.Get(4).Width = 2F;
            this.ssView_Sheet1.Columns.Get(5).Label = "점수";
            this.ssView_Sheet1.Columns.Get(5).StyleName = "Static894636536975595668732";
            this.ssView_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.ssView_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.FilterBar.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssView_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssView_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.RowHeader.Rows.Default.Resizable = false;
            this.ssView_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.RowHeader.Visible = false;
            this.ssView_Sheet1.Rows.Default.Height = 40F;
            this.ssView_Sheet1.Rows.Default.Resizable = false;
            this.ssView_Sheet1.Rows.Get(0).Height = 45F;
            this.ssView_Sheet1.Rows.Get(1).Height = 45F;
            this.ssView_Sheet1.Rows.Get(2).Height = 45F;
            this.ssView_Sheet1.Rows.Get(3).Height = 61F;
            this.ssView_Sheet1.Rows.Get(4).Height = 61F;
            this.ssView_Sheet1.Rows.Get(5).Height = 79F;
            this.ssView_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssView_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(602, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.txtJumsu);
            this.panel3.Controls.Add(this.lblItem1);
            this.panel3.Controls.Add(this.btnSend);
            this.panel3.Controls.Add(this.lblItem0);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 34);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(681, 38);
            this.panel3.TabIndex = 48;
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.BackColor = System.Drawing.Color.Transparent;
            this.btnSend.Location = new System.Drawing.Point(604, 4);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(72, 30);
            this.btnSend.TabIndex = 26;
            this.btnSend.Text = "완료";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lblItem0
            // 
            this.lblItem0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblItem0.Location = new System.Drawing.Point(11, 6);
            this.lblItem0.Name = "lblItem0";
            this.lblItem0.Size = new System.Drawing.Size(319, 26);
            this.lblItem0.TabIndex = 24;
            this.lblItem0.Text = "※ NIPS(Neonatal infant pain) 신생아 통증 도구표";
            this.lblItem0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtJumsu
            // 
            this.txtJumsu.Location = new System.Drawing.Point(539, 7);
            this.txtJumsu.Name = "txtJumsu";
            this.txtJumsu.Size = new System.Drawing.Size(59, 25);
            this.txtJumsu.TabIndex = 34;
            this.txtJumsu.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblItem1
            // 
            this.lblItem1.AutoSize = true;
            this.lblItem1.Location = new System.Drawing.Point(505, 11);
            this.lblItem1.Name = "lblItem1";
            this.lblItem1.Size = new System.Drawing.Size(34, 17);
            this.lblItem1.TabIndex = 33;
            this.lblItem1.Text = "총점";
            // 
            // frmPainScore3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 456);
            this.Controls.Add(this.ssView);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmPainScore3";
            this.Text = "NIPS(Neonatal Infant Pain)";
            this.Load += new System.EventHandler(this.frmPainScore3_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label lblItem0;
        private System.Windows.Forms.TextBox txtJumsu;
        private System.Windows.Forms.Label lblItem1;
    }
}