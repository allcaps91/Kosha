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
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType13 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType14 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType15 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType16 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType17 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType18 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType19 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.SS1 = new FarPoint.Win.Spread.FpSpread();
            this.SS1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.btnView = new System.Windows.Forms.Button();
            this.SS2 = new FarPoint.Win.Spread.FpSpread();
            this.SS2_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.SS3 = new FarPoint.Win.Spread.FpSpread();
            this.SS3_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
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
            this.SS1.Size = new System.Drawing.Size(726, 165);
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
            textCellType1.ReadOnly = true;
            textCellType1.WordWrap = true;
            this.SS1_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.SS1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(0).Label = "라이선스";
            this.SS1_Sheet1.Columns.Get(0).Width = 125F;
            textCellType2.ReadOnly = true;
            textCellType2.WordWrap = true;
            this.SS1_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.SS1_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS1_Sheet1.Columns.Get(1).Label = "회사명";
            this.SS1_Sheet1.Columns.Get(1).Width = 204F;
            textCellType3.ReadOnly = true;
            this.SS1_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.SS1_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(2).Label = "설치수량";
            this.SS1_Sheet1.Columns.Get(2).Width = 88F;
            textCellType4.ReadOnly = true;
            this.SS1_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.SS1_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(3).Label = "오늘접속";
            this.SS1_Sheet1.Columns.Get(3).Width = 79F;
            textCellType5.ReadOnly = true;
            this.SS1_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.SS1_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(4).Label = "미접속";
            this.SS1_Sheet1.Columns.Get(4).Width = 78F;
            textCellType6.ReadOnly = true;
            this.SS1_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.SS1_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(5).Label = "업데이트 누락";
            this.SS1_Sheet1.Columns.Get(5).Width = 94F;
            this.SS1_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SS1_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(736, 3);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(194, 29);
            this.btnView.TabIndex = 6;
            this.btnView.Text = "사용자정보 검색";
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
            this.SS2.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SS2_CellDoubleClick);
            // 
            // SS2_Sheet1
            // 
            this.SS2_Sheet1.Reset();
            this.SS2_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS2_Sheet1.ColumnCount = 9;
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
            this.SS2_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "변경전";
            textCellType7.ReadOnly = true;
            this.SS2_Sheet1.Columns.Get(0).CellType = textCellType7;
            this.SS2_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS2_Sheet1.Columns.Get(0).Label = "Mac";
            this.SS2_Sheet1.Columns.Get(0).Width = 110F;
            textCellType8.ReadOnly = true;
            this.SS2_Sheet1.Columns.Get(1).CellType = textCellType8;
            this.SS2_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS2_Sheet1.Columns.Get(1).Label = "라이선스";
            this.SS2_Sheet1.Columns.Get(1).Width = 109F;
            textCellType9.ReadOnly = true;
            this.SS2_Sheet1.Columns.Get(2).CellType = textCellType9;
            this.SS2_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS2_Sheet1.Columns.Get(2).Label = "Ver";
            this.SS2_Sheet1.Columns.Get(2).Width = 81F;
            textCellType10.ReadOnly = true;
            this.SS2_Sheet1.Columns.Get(3).CellType = textCellType10;
            this.SS2_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS2_Sheet1.Columns.Get(3).Label = "IP";
            this.SS2_Sheet1.Columns.Get(3).Width = 87F;
            textCellType11.ReadOnly = true;
            this.SS2_Sheet1.Columns.Get(4).CellType = textCellType11;
            this.SS2_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS2_Sheet1.Columns.Get(4).Label = "설치일";
            this.SS2_Sheet1.Columns.Get(4).Width = 106F;
            textCellType12.ReadOnly = true;
            this.SS2_Sheet1.Columns.Get(5).CellType = textCellType12;
            this.SS2_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS2_Sheet1.Columns.Get(5).Label = "최종접속";
            this.SS2_Sheet1.Columns.Get(5).Width = 110F;
            textCellType13.ReadOnly = true;
            this.SS2_Sheet1.Columns.Get(6).CellType = textCellType13;
            this.SS2_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS2_Sheet1.Columns.Get(6).Label = "윈도우버전";
            this.SS2_Sheet1.Columns.Get(6).Width = 130F;
            this.SS2_Sheet1.Columns.Get(7).Label = "참고사항";
            this.SS2_Sheet1.Columns.Get(7).Width = 146F;
            textCellType14.ReadOnly = true;
            textCellType14.WordWrap = true;
            this.SS2_Sheet1.Columns.Get(8).CellType = textCellType14;
            this.SS2_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS2_Sheet1.Columns.Get(8).Label = "변경전";
            this.SS2_Sheet1.Columns.Get(8).Visible = false;
            this.SS2_Sheet1.Columns.Get(8).Width = 77F;
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
            textCellType15.ReadOnly = true;
            textCellType15.WordWrap = true;
            this.SS3_Sheet1.Columns.Get(0).CellType = textCellType15;
            this.SS3_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS3_Sheet1.Columns.Get(0).Label = "전송시각";
            this.SS3_Sheet1.Columns.Get(0).Width = 142F;
            textCellType16.ReadOnly = true;
            textCellType16.WordWrap = true;
            this.SS3_Sheet1.Columns.Get(1).CellType = textCellType16;
            this.SS3_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS3_Sheet1.Columns.Get(1).Label = "라이선스";
            this.SS3_Sheet1.Columns.Get(1).Width = 121F;
            textCellType17.ReadOnly = true;
            this.SS3_Sheet1.Columns.Get(2).CellType = textCellType17;
            this.SS3_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS3_Sheet1.Columns.Get(2).Label = "Mac";
            this.SS3_Sheet1.Columns.Get(2).Width = 107F;
            textCellType18.ReadOnly = true;
            this.SS3_Sheet1.Columns.Get(3).CellType = textCellType18;
            this.SS3_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS3_Sheet1.Columns.Get(3).Label = "IP";
            this.SS3_Sheet1.Columns.Get(3).Width = 104F;
            textCellType19.ReadOnly = true;
            this.SS3_Sheet1.Columns.Get(4).CellType = textCellType19;
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
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(738, 135);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(200, 33);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "참고사항 변경내역 저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(753, 117);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 15);
            this.label1.TabIndex = 10;
            this.label1.Text = "▶PC삭제: Mac 칼럼 더블클릭";
            // 
            // FrmUserInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(942, 686);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSave);
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
            this.PerformLayout();

        }

        #endregion

        private FarPoint.Win.Spread.FpSpread SS1;
        private FarPoint.Win.Spread.SheetView SS1_Sheet1;
        private System.Windows.Forms.Button btnView;
        private FarPoint.Win.Spread.FpSpread SS2;
        private FarPoint.Win.Spread.SheetView SS2_Sheet1;
        private FarPoint.Win.Spread.FpSpread SS3;
        private FarPoint.Win.Spread.SheetView SS3_Sheet1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
    }
}