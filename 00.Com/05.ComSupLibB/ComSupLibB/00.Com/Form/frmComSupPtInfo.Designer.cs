namespace ComSupLibB.Com
{
    partial class frmComSupPtInfo
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
            FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer enhancedRowHeaderRenderer1 = new FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer();
            FarPoint.Win.Spread.EnhancedFocusIndicatorRenderer enhancedFocusIndicatorRenderer1 = new FarPoint.Win.Spread.EnhancedFocusIndicatorRenderer();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer1 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer2 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            this.ss_Main = new FarPoint.Win.Spread.FpSpread();
            this.ss_Main_Sheet1 = new FarPoint.Win.Spread.SheetView();
            ((System.ComponentModel.ISupportInitialize)(this.ss_Main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss_Main_Sheet1)).BeginInit();
            this.SuspendLayout();
            enhancedRowHeaderRenderer1.BackColor = System.Drawing.SystemColors.Control;
            enhancedRowHeaderRenderer1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            enhancedRowHeaderRenderer1.ForeColor = System.Drawing.SystemColors.ControlText;
            enhancedRowHeaderRenderer1.Name = "enhancedRowHeaderRenderer1";
            enhancedRowHeaderRenderer1.PictureZoomEffect = false;
            enhancedRowHeaderRenderer1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            enhancedRowHeaderRenderer1.TextRotationAngle = 0D;
            enhancedRowHeaderRenderer1.ZoomFactor = 1F;
            // 
            // ss_Main
            // 
            this.ss_Main.AccessibleDescription = "fpSpread1, Sheet1, Row 0, Column 0, ";
            this.ss_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ss_Main.FocusRenderer = enhancedFocusIndicatorRenderer1;
            this.ss_Main.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ss_Main.HorizontalScrollBar.Name = "";
            this.ss_Main.HorizontalScrollBar.Renderer = enhancedScrollBarRenderer1;
            this.ss_Main.HorizontalScrollBar.TabIndex = 6;
            this.ss_Main.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ss_Main.Location = new System.Drawing.Point(0, 0);
            this.ss_Main.Name = "ss_Main";
            this.ss_Main.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ss_Main_Sheet1});
            this.ss_Main.Size = new System.Drawing.Size(792, 398);
            this.ss_Main.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;
            this.ss_Main.TabIndex = 113;
            this.ss_Main.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ss_Main.VerticalScrollBar.Name = "";
            this.ss_Main.VerticalScrollBar.Renderer = enhancedScrollBarRenderer2;
            this.ss_Main.VerticalScrollBar.TabIndex = 7;
            this.ss_Main.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // ss_Main_Sheet1
            // 
            this.ss_Main_Sheet1.Reset();
            this.ss_Main_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ss_Main_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ss_Main_Sheet1.ColumnCount = 8;
            this.ss_Main_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ss_Main_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss_Main_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterEnhanced";
            this.ss_Main_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss_Main_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ss_Main_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss_Main_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerEnhanced";
            this.ss_Main_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss_Main_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "등록번호";
            this.ss_Main_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "성명";
            this.ss_Main_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "주민번호";
            this.ss_Main_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "성별";
            this.ss_Main_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "나이";
            this.ss_Main_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "주소";
            this.ss_Main_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "최종내원일";
            this.ss_Main_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "진료과";
            this.ss_Main_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ss_Main_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss_Main_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderEnhanced";
            this.ss_Main_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss_Main_Sheet1.ColumnHeader.Rows.Get(0).Height = 38F;
            this.ss_Main_Sheet1.Columns.Get(0).Label = "등록번호";
            this.ss_Main_Sheet1.Columns.Get(0).Width = 102F;
            this.ss_Main_Sheet1.Columns.Get(1).Label = "성명";
            this.ss_Main_Sheet1.Columns.Get(1).Width = 100F;
            this.ss_Main_Sheet1.Columns.Get(2).Label = "주민번호";
            this.ss_Main_Sheet1.Columns.Get(2).Width = 106F;
            this.ss_Main_Sheet1.Columns.Get(3).Label = "성별";
            this.ss_Main_Sheet1.Columns.Get(3).Width = 26F;
            this.ss_Main_Sheet1.Columns.Get(4).Label = "나이";
            this.ss_Main_Sheet1.Columns.Get(4).Width = 38F;
            this.ss_Main_Sheet1.Columns.Get(5).Label = "주소";
            this.ss_Main_Sheet1.Columns.Get(5).Width = 177F;
            this.ss_Main_Sheet1.Columns.Get(6).Label = "최종내원일";
            this.ss_Main_Sheet1.Columns.Get(6).Width = 67F;
            this.ss_Main_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ss_Main_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss_Main_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarEnhanced";
            this.ss_Main_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss_Main_Sheet1.FilterBarHeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(236)))), ((int)(((byte)(247)))));
            this.ss_Main_Sheet1.FilterBarHeaderStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ss_Main_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss_Main_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss_Main_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderEnhanced";
            this.ss_Main_Sheet1.FilterBarHeaderStyle.Renderer = enhancedRowHeaderRenderer1;
            this.ss_Main_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss_Main_Sheet1.FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ss_Main_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ss_Main_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            this.ss_Main_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ss_Main_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ss_Main_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss_Main_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderEnhanced";
            this.ss_Main_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss_Main_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ss_Main_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss_Main_Sheet1.SheetCornerStyle.Parent = "CornerEnhanced";
            this.ss_Main_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss_Main_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmComSupPtInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 398);
            this.Controls.Add(this.ss_Main);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmComSupPtInfo";
            this.Text = "환자정보";
            ((System.ComponentModel.ISupportInitialize)(this.ss_Main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss_Main_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private FarPoint.Win.Spread.FpSpread ss_Main;
        private FarPoint.Win.Spread.SheetView ss_Main_Sheet1;
    }
}