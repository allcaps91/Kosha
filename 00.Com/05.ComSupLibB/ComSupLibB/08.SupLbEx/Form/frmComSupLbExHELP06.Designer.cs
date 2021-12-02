namespace ComSupLibB.SupLbEx
{
    partial class frmComSupLbExHELP06
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
            FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer enhancedRowHeaderRenderer2 = new FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer();
            FarPoint.Win.Spread.EnhancedFocusIndicatorRenderer enhancedFocusIndicatorRenderer1 = new FarPoint.Win.Spread.EnhancedFocusIndicatorRenderer();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer1 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer2 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            this.btnExit = new System.Windows.Forms.Button();
            this.panTitle = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.ss_EXAM_RESULT = new FarPoint.Win.Spread.FpSpread();
            this.ss_EXAM_RESULT_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss_EXAM_RESULT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss_EXAM_RESULT_Sheet1)).BeginInit();
            this.SuspendLayout();
            enhancedRowHeaderRenderer1.Name = "enhancedRowHeaderRenderer1";
            enhancedRowHeaderRenderer1.PictureZoomEffect = false;
            enhancedRowHeaderRenderer1.TextRotationAngle = 0D;
            enhancedRowHeaderRenderer1.ZoomFactor = 1F;
            enhancedRowHeaderRenderer2.BackColor = System.Drawing.SystemColors.Control;
            enhancedRowHeaderRenderer2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            enhancedRowHeaderRenderer2.ForeColor = System.Drawing.SystemColors.ControlText;
            enhancedRowHeaderRenderer2.Name = "enhancedRowHeaderRenderer2";
            enhancedRowHeaderRenderer2.PictureZoomEffect = false;
            enhancedRowHeaderRenderer2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            enhancedRowHeaderRenderer2.TextRotationAngle = 0D;
            enhancedRowHeaderRenderer2.ZoomFactor = 1F;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(346, 1);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(69, 38);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.Controls.Add(this.panel1);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitle.Name = "panTitle";
            this.panTitle.Padding = new System.Windows.Forms.Padding(1);
            this.panTitle.Size = new System.Drawing.Size(416, 40);
            this.panTitle.TabIndex = 13;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(1, 1);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.panel1.Size = new System.Drawing.Size(345, 38);
            this.panel1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(5, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(335, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "1234567890 홍길동 AA1234 : 이상한검사";
            // 
            // ss_EXAM_RESULT
            // 
            this.ss_EXAM_RESULT.AccessibleDescription = "";
            this.ss_EXAM_RESULT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ss_EXAM_RESULT.FocusRenderer = enhancedFocusIndicatorRenderer1;
            this.ss_EXAM_RESULT.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ss_EXAM_RESULT.HorizontalScrollBar.Name = "";
            this.ss_EXAM_RESULT.HorizontalScrollBar.Renderer = enhancedScrollBarRenderer1;
            this.ss_EXAM_RESULT.HorizontalScrollBar.TabIndex = 5;
            this.ss_EXAM_RESULT.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ss_EXAM_RESULT.Location = new System.Drawing.Point(0, 40);
            this.ss_EXAM_RESULT.Name = "ss_EXAM_RESULT";
            this.ss_EXAM_RESULT.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ss_EXAM_RESULT_Sheet1});
            this.ss_EXAM_RESULT.Size = new System.Drawing.Size(416, 326);
            this.ss_EXAM_RESULT.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;
            this.ss_EXAM_RESULT.TabIndex = 14;
            this.ss_EXAM_RESULT.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ss_EXAM_RESULT.VerticalScrollBar.Name = "";
            this.ss_EXAM_RESULT.VerticalScrollBar.Renderer = enhancedScrollBarRenderer2;
            this.ss_EXAM_RESULT.VerticalScrollBar.TabIndex = 6;
            this.ss_EXAM_RESULT.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // ss_EXAM_RESULT_Sheet1
            // 
            this.ss_EXAM_RESULT_Sheet1.Reset();
            this.ss_EXAM_RESULT_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ss_EXAM_RESULT_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ss_EXAM_RESULT_Sheet1.ColumnCount = 2;
            this.ss_EXAM_RESULT_Sheet1.RowCount = 10;
            this.ss_EXAM_RESULT_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ss_EXAM_RESULT_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss_EXAM_RESULT_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterEnhanced";
            this.ss_EXAM_RESULT_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss_EXAM_RESULT_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ss_EXAM_RESULT_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss_EXAM_RESULT_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerEnhanced";
            this.ss_EXAM_RESULT_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss_EXAM_RESULT_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ss_EXAM_RESULT_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss_EXAM_RESULT_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderEnhanced";
            this.ss_EXAM_RESULT_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss_EXAM_RESULT_Sheet1.Columns.Get(0).Width = 81F;
            this.ss_EXAM_RESULT_Sheet1.Columns.Get(1).Width = 106F;
            this.ss_EXAM_RESULT_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ss_EXAM_RESULT_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss_EXAM_RESULT_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarEnhanced";
            this.ss_EXAM_RESULT_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss_EXAM_RESULT_Sheet1.FilterBarHeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(236)))), ((int)(((byte)(247)))));
            this.ss_EXAM_RESULT_Sheet1.FilterBarHeaderStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ss_EXAM_RESULT_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss_EXAM_RESULT_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss_EXAM_RESULT_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderEnhanced";
            this.ss_EXAM_RESULT_Sheet1.FilterBarHeaderStyle.Renderer = enhancedRowHeaderRenderer2;
            this.ss_EXAM_RESULT_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss_EXAM_RESULT_Sheet1.FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ss_EXAM_RESULT_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ss_EXAM_RESULT_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ss_EXAM_RESULT_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ss_EXAM_RESULT_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss_EXAM_RESULT_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderEnhanced";
            this.ss_EXAM_RESULT_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss_EXAM_RESULT_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ss_EXAM_RESULT_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss_EXAM_RESULT_Sheet1.SheetCornerStyle.Parent = "CornerEnhanced";
            this.ss_EXAM_RESULT_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss_EXAM_RESULT_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmComSupLbExHELP06
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(416, 366);
            this.Controls.Add(this.ss_EXAM_RESULT);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmComSupLbExHELP06";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmComSupLbExHELP06";
            this.TopMost = true;
            this.panTitle.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ss_EXAM_RESULT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss_EXAM_RESULT_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panTitle;
        private FarPoint.Win.Spread.FpSpread ss_EXAM_RESULT;
        private FarPoint.Win.Spread.SheetView ss_EXAM_RESULT_Sheet1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
    }
}