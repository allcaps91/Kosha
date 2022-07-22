namespace HcAdmin
{
    partial class FrmUserInfo
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType20 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType21 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType22 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType23 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType24 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType25 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType26 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType27 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType28 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType29 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType30 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType31 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType32 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType33 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType34 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType35 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType36 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType37 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType38 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.SS1 = new FarPoint.Win.Spread.FpSpread();
            this.SS1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.btnView = new System.Windows.Forms.Button();
            this.SS2 = new FarPoint.Win.Spread.FpSpread();
            this.SS2_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.SS3 = new FarPoint.Win.Spread.FpSpread();
            this.SS3_Sheet1 = new FarPoint.Win.Spread.SheetView();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS2_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS3_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // SS1
            // 
            this.SS1.AccessibleDescription = "SS1, Sheet1, Row 0, Column 0, ";
            this.SS1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SS1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.SS1.Location = new System.Drawing.Point(3, 3);
            this.SS1.Name = "SS1";
            this.SS1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS1_Sheet1});
            this.SS1.Size = new System.Drawing.Size(653, 165);
            this.SS1.TabIndex = 5;
            this.SS1.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SS1_CellDoubleClick);
            // 
            // SS1_Sheet1
            // 
            this.SS1_Sheet1.Reset();
            this.SS1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS1_Sheet1.ColumnCount = 6;
            this.SS1_Sheet1.RowCount = 20;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "라이선스";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "회사명";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "설치수량";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "오늘접속";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "미접속";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "업데이트 누락";
            textCellType20.ReadOnly = true;
            textCellType20.WordWrap = true;
            this.SS1_Sheet1.Columns.Get(0).CellType = textCellType20;
            this.SS1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(0).Label = "라이선스";
            this.SS1_Sheet1.Columns.Get(0).Width = 125F;
            textCellType21.ReadOnly = true;
            textCellType21.WordWrap = true;
            this.SS1_Sheet1.Columns.Get(1).CellType = textCellType21;
            this.SS1_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS1_Sheet1.Columns.Get(1).Label = "회사명";
            this.SS1_Sheet1.Columns.Get(1).Width = 167F;
            textCellType22.ReadOnly = true;
            this.SS1_Sheet1.Columns.Get(2).CellType = textCellType22;
            this.SS1_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(2).Label = "설치수량";
            this.SS1_Sheet1.Columns.Get(2).Width = 74F;
            textCellType23.ReadOnly = true;
            this.SS1_Sheet1.Columns.Get(3).CellType = textCellType23;
            this.SS1_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(3).Label = "오늘접속";
            this.SS1_Sheet1.Columns.Get(3).Width = 71F;
            textCellType24.ReadOnly = true;
            this.SS1_Sheet1.Columns.Get(4).CellType = textCellType24;
            this.SS1_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(4).Label = "미접속";
            textCellType25.ReadOnly = true;
            this.SS1_Sheet1.Columns.Get(5).CellType = textCellType25;
            this.SS1_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(5).Label = "업데이트 누락";
            this.SS1_Sheet1.Columns.Get(5).Width = 100F;
            this.SS1_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SS1_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(704, 12);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(91, 29);
            this.btnView.TabIndex = 6;
            this.btnView.Text = "자료감섹";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // SS2
            // 
            this.SS2.AccessibleDescription = "SS2, Sheet1, Row 0, Column 0, ";
            this.SS2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SS2.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.SS2.Location = new System.Drawing.Point(3, 174);
            this.SS2.Name = "SS2";
            this.SS2.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS2_Sheet1});
            this.SS2.Size = new System.Drawing.Size(935, 245);
            this.SS2.TabIndex = 7;
            // 
            // SS2_Sheet1
            // 
            this.SS2_Sheet1.Reset();
            this.SS2_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS2_Sheet1.ColumnCount = 8;
            this.SS2_Sheet1.RowCount = 20;
            this.SS2_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS2_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS2_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SS2_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS2_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "Mac";
            this.SS2_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "라이선스";
            this.SS2_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "Ver";
            this.SS2_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "IP";
            this.SS2_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "설치일";
            this.SS2_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "최종접속";
            this.SS2_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "윈도우버전";
            this.SS2_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "참고사항";
            textCellType26.ReadOnly = true;
            this.SS2_Sheet1.Columns.Get(0).CellType = textCellType26;
            this.SS2_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS2_Sheet1.Columns.Get(0).Label = "Mac";
            this.SS2_Sheet1.Columns.Get(0).Width = 110F;
            textCellType27.ReadOnly = true;
            this.SS2_Sheet1.Columns.Get(1).CellType = textCellType27;
            this.SS2_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS2_Sheet1.Columns.Get(1).Label = "라이선스";
            this.SS2_Sheet1.Columns.Get(1).Width = 109F;
            textCellType28.ReadOnly = true;
            this.SS2_Sheet1.Columns.Get(2).CellType = textCellType28;
            this.SS2_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS2_Sheet1.Columns.Get(2).Label = "Ver";
            this.SS2_Sheet1.Columns.Get(2).Width = 81F;
            textCellType29.ReadOnly = true;
            this.SS2_Sheet1.Columns.Get(3).CellType = textCellType29;
            this.SS2_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS2_Sheet1.Columns.Get(3).Label = "IP";
            this.SS2_Sheet1.Columns.Get(3).Width = 95F;
            textCellType30.ReadOnly = true;
            this.SS2_Sheet1.Columns.Get(4).CellType = textCellType30;
            this.SS2_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS2_Sheet1.Columns.Get(4).Label = "설치일";
            this.SS2_Sheet1.Columns.Get(4).Width = 121F;
            textCellType31.ReadOnly = true;
            this.SS2_Sheet1.Columns.Get(5).CellType = textCellType31;
            this.SS2_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS2_Sheet1.Columns.Get(5).Label = "최종접속";
            this.SS2_Sheet1.Columns.Get(5).Width = 126F;
            textCellType32.ReadOnly = true;
            this.SS2_Sheet1.Columns.Get(6).CellType = textCellType32;
            this.SS2_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS2_Sheet1.Columns.Get(6).Label = "윈도우버전";
            this.SS2_Sheet1.Columns.Get(6).Width = 135F;
            textCellType33.ReadOnly = true;
            textCellType33.WordWrap = true;
            this.SS2_Sheet1.Columns.Get(7).CellType = textCellType33;
            this.SS2_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS2_Sheet1.Columns.Get(7).Label = "참고사항";
            this.SS2_Sheet1.Columns.Get(7).Width = 98F;
            this.SS2_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS2_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS2_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SS2_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS2_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // SS3
            // 
            this.SS3.AccessibleDescription = "SS3, Sheet1, Row 0, Column 0, ";
            this.SS3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SS3.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.SS3.Location = new System.Drawing.Point(3, 425);
            this.SS3.Name = "SS3";
            this.SS3.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS3_Sheet1});
            this.SS3.Size = new System.Drawing.Size(935, 245);
            this.SS3.TabIndex = 8;
            // 
            // SS3_Sheet1
            // 
            this.SS3_Sheet1.Reset();
            this.SS3_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS3_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS3_Sheet1.ColumnCount = 5;
            this.SS3_Sheet1.RowCount = 20;
            this.SS3_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS3_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS3_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SS3_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS3_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "전송시각";
            this.SS3_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "라이선스";
            this.SS3_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "Mac";
            this.SS3_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "IP";
            this.SS3_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "전송로그";
            textCellType34.ReadOnly = true;
            textCellType34.WordWrap = true;
            this.SS3_Sheet1.Columns.Get(0).CellType = textCellType34;
            this.SS3_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS3_Sheet1.Columns.Get(0).Label = "전송시각";
            this.SS3_Sheet1.Columns.Get(0).Width = 142F;
            textCellType35.ReadOnly = true;
            textCellType35.WordWrap = true;
            this.SS3_Sheet1.Columns.Get(1).CellType = textCellType35;
            this.SS3_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS3_Sheet1.Columns.Get(1).Label = "라이선스";
            this.SS3_Sheet1.Columns.Get(1).Width = 121F;
            textCellType36.ReadOnly = true;
            this.SS3_Sheet1.Columns.Get(2).CellType = textCellType36;
            this.SS3_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS3_Sheet1.Columns.Get(2).Label = "Mac";
            this.SS3_Sheet1.Columns.Get(2).Width = 107F;
            textCellType37.ReadOnly = true;
            this.SS3_Sheet1.Columns.Get(3).CellType = textCellType37;
            this.SS3_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS3_Sheet1.Columns.Get(3).Label = "IP";
            this.SS3_Sheet1.Columns.Get(3).Width = 104F;
            textCellType38.ReadOnly = true;
            this.SS3_Sheet1.Columns.Get(4).CellType = textCellType38;
            this.SS3_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS3_Sheet1.Columns.Get(4).Label = "전송로그";
            this.SS3_Sheet1.Columns.Get(4).Width = 393F;
            this.SS3_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS3_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS3_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SS3_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS3_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS3_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // FrmUserInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(942, 686);
            this.Controls.Add(this.SS3);
            this.Controls.Add(this.SS2);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.SS1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmUserInfo";
            this.Text = "헬스소프트 사용자 정보";
            this.Load += new System.EventHandler(this.FrmUserInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS2_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS3_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private FarPoint.Win.Spread.FpSpread SS1;
        private FarPoint.Win.Spread.SheetView SS1_Sheet1;
        private System.Windows.Forms.Button btnView;
        private FarPoint.Win.Spread.FpSpread SS2;
        private FarPoint.Win.Spread.SheetView SS2_Sheet1;
        private FarPoint.Win.Spread.FpSpread SS3;
        private FarPoint.Win.Spread.SheetView SS3_Sheet1;
    }
}