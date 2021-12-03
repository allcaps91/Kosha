namespace ComHpcLibB
{
    partial class frmHcPermission
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
            this.components = new System.ComponentModel.Container();
            FarPoint.Win.Spread.FlatFocusIndicatorRenderer flatFocusIndicatorRenderer1 = new FarPoint.Win.Spread.FlatFocusIndicatorRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer1 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer2 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.CellType.ButtonCellType buttonCellType1 = new FarPoint.Win.Spread.CellType.ButtonCellType();
            FarPoint.Win.Spread.CellType.ButtonCellType buttonCellType2 = new FarPoint.Win.Spread.CellType.ButtonCellType();
            FarPoint.Win.Spread.CellType.ButtonCellType buttonCellType3 = new FarPoint.Win.Spread.CellType.ButtonCellType();
            FarPoint.Win.Spread.CellType.ButtonCellType buttonCellType4 = new FarPoint.Win.Spread.CellType.ButtonCellType();
            FarPoint.Win.Spread.CellType.ButtonCellType buttonCellType5 = new FarPoint.Win.Spread.CellType.ButtonCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.ButtonCellType buttonCellType6 = new FarPoint.Win.Spread.CellType.ButtonCellType();
            this.ssConsent = new FarPoint.Win.Spread.FpSpread();
            this.ssConsent_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ssConsent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssConsent_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // ssConsent
            // 
            this.ssConsent.AccessibleDescription = "ssConsent, Sheet1, Row 0, Column 0, 개인정보";
            this.ssConsent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssConsent.FocusRenderer = flatFocusIndicatorRenderer1;
            this.ssConsent.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssConsent.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssConsent.HorizontalScrollBar.Renderer = flatScrollBarRenderer1;
            this.ssConsent.HorizontalScrollBar.TabIndex = 40;
            this.ssConsent.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssConsent.Location = new System.Drawing.Point(0, 0);
            this.ssConsent.Name = "ssConsent";
            this.ssConsent.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssConsent_Sheet1});
            this.ssConsent.Size = new System.Drawing.Size(181, 116);
            this.ssConsent.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.ssConsent.TabIndex = 0;
            this.ssConsent.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssConsent.VerticalScrollBar.Name = "";
            flatScrollBarRenderer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssConsent.VerticalScrollBar.Renderer = flatScrollBarRenderer2;
            this.ssConsent.VerticalScrollBar.TabIndex = 41;
            this.ssConsent.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            // 
            // ssConsent_Sheet1
            // 
            this.ssConsent_Sheet1.Reset();
            this.ssConsent_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssConsent_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssConsent_Sheet1.ColumnCount = 4;
            this.ssConsent_Sheet1.RowCount = 5;
            this.ssConsent_Sheet1.Cells.Get(0, 0).Value = "개인정보";
            this.ssConsent_Sheet1.Cells.Get(0, 1).Value = "2020-07-30";
            buttonCellType1.ButtonColor2 = System.Drawing.SystemColors.ButtonFace;
            buttonCellType1.Text = "보기";
            this.ssConsent_Sheet1.Cells.Get(0, 2).CellType = buttonCellType1;
            this.ssConsent_Sheet1.Cells.Get(0, 2).Value = 0;
            this.ssConsent_Sheet1.Cells.Get(0, 3).Value = "D50";
            this.ssConsent_Sheet1.Cells.Get(1, 0).Value = "내시경";
            this.ssConsent_Sheet1.Cells.Get(1, 1).Value = "2020-07-30";
            buttonCellType2.ButtonColor2 = System.Drawing.SystemColors.ButtonFace;
            buttonCellType2.Text = "보기";
            this.ssConsent_Sheet1.Cells.Get(1, 2).CellType = buttonCellType2;
            this.ssConsent_Sheet1.Cells.Get(1, 3).Value = "D51";
            this.ssConsent_Sheet1.Cells.Get(2, 0).Value = "정보활용";
            this.ssConsent_Sheet1.Cells.Get(2, 1).Value = "2020-07-30";
            buttonCellType3.ButtonColor2 = System.Drawing.SystemColors.ButtonFace;
            buttonCellType3.Text = "보기";
            this.ssConsent_Sheet1.Cells.Get(2, 2).CellType = buttonCellType3;
            this.ssConsent_Sheet1.Cells.Get(2, 3).Value = "D52";
            this.ssConsent_Sheet1.Cells.Get(3, 0).Value = "검진동시";
            this.ssConsent_Sheet1.Cells.Get(3, 1).Value = "2020-07-30";
            buttonCellType4.ButtonColor2 = System.Drawing.SystemColors.ButtonFace;
            buttonCellType4.Text = "보기";
            this.ssConsent_Sheet1.Cells.Get(3, 2).CellType = buttonCellType4;
            this.ssConsent_Sheet1.Cells.Get(3, 2).Value = 0;
            this.ssConsent_Sheet1.Cells.Get(3, 3).Value = "D53";
            this.ssConsent_Sheet1.Cells.Get(4, 0).Value = "건강진단표";
            this.ssConsent_Sheet1.Cells.Get(4, 1).Value = "2020-07-30";
            buttonCellType5.ButtonColor2 = System.Drawing.SystemColors.ButtonFace;
            buttonCellType5.Text = "보기";
            this.ssConsent_Sheet1.Cells.Get(4, 2).CellType = buttonCellType5;
            this.ssConsent_Sheet1.Cells.Get(4, 3).Value = "D54";
            this.ssConsent_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssConsent_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssConsent_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.ssConsent_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssConsent_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssConsent_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssConsent_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.ssConsent_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssConsent_Sheet1.ColumnHeader.Cells.Get(0, 0).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssConsent_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "동의서명";
            this.ssConsent_Sheet1.ColumnHeader.Cells.Get(0, 1).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssConsent_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "작성일자";
            this.ssConsent_Sheet1.ColumnHeader.Cells.Get(0, 2).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssConsent_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "보기";
            this.ssConsent_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssConsent_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssConsent_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.ssConsent_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssConsent_Sheet1.ColumnHeader.Rows.Get(0).Height = 28F;
            this.ssConsent_Sheet1.ColumnHeader.Visible = false;
            this.ssConsent_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssConsent_Sheet1.Columns.Get(0).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.ssConsent_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssConsent_Sheet1.Columns.Get(0).Label = "동의서명";
            this.ssConsent_Sheet1.Columns.Get(0).Locked = true;
            this.ssConsent_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssConsent_Sheet1.Columns.Get(0).Width = 68F;
            this.ssConsent_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssConsent_Sheet1.Columns.Get(1).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.ssConsent_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssConsent_Sheet1.Columns.Get(1).Label = "작성일자";
            this.ssConsent_Sheet1.Columns.Get(1).Locked = true;
            this.ssConsent_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssConsent_Sheet1.Columns.Get(1).Width = 74F;
            buttonCellType6.ButtonColor2 = System.Drawing.SystemColors.ButtonFace;
            this.ssConsent_Sheet1.Columns.Get(2).CellType = buttonCellType6;
            this.ssConsent_Sheet1.Columns.Get(2).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.ssConsent_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssConsent_Sheet1.Columns.Get(2).Label = "보기";
            this.ssConsent_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssConsent_Sheet1.Columns.Get(2).Width = 34F;
            this.ssConsent_Sheet1.Columns.Get(3).Visible = false;
            this.ssConsent_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssConsent_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssConsent_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.ssConsent_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssConsent_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssConsent_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssConsent_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.ssConsent_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssConsent_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.ssConsent_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssConsent_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssConsent_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssConsent_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.ssConsent_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssConsent_Sheet1.RowHeader.Visible = false;
            this.ssConsent_Sheet1.Rows.Get(0).Height = 23F;
            this.ssConsent_Sheet1.Rows.Get(1).Height = 23F;
            this.ssConsent_Sheet1.Rows.Get(2).Height = 23F;
            this.ssConsent_Sheet1.Rows.Get(3).Height = 23F;
            this.ssConsent_Sheet1.Rows.Get(4).Height = 23F;
            this.ssConsent_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssConsent_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssConsent_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.ssConsent_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssConsent_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.ssConsent_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // timer1
            // 
            this.timer1.Interval = 5000;
            // 
            // frmHcPermission
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(181, 116);
            this.Controls.Add(this.ssConsent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmHcPermission";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "동의서보기";
            ((System.ComponentModel.ISupportInitialize)(this.ssConsent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssConsent_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private FarPoint.Win.Spread.FpSpread ssConsent;
        private FarPoint.Win.Spread.SheetView ssConsent_Sheet1;
        private System.Windows.Forms.Timer timer1;
    }
}