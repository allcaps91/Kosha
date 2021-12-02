namespace ComHpcLibB
{
    partial class frmHcPendListReg
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
            FarPoint.Win.Spread.EnhancedFocusIndicatorRenderer enhancedFocusIndicatorRenderer1 = new FarPoint.Win.Spread.EnhancedFocusIndicatorRenderer();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer1 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer2 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblFormTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtSayu = new System.Windows.Forms.TextBox();
            this.txtExamName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ssPatInfo = new FarPoint.Win.Spread.FpSpread();
            this.ssPatInfo_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssPatInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssPatInfo_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnDelete);
            this.panel4.Controls.Add(this.btnSave);
            this.panel4.Controls.Add(this.btnExit);
            this.panel4.Controls.Add(this.lblFormTitle);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(581, 36);
            this.panel4.TabIndex = 3;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.White;
            this.btnDelete.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDelete.Location = new System.Drawing.Point(448, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(62, 29);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "삭제(&X)";
            this.btnDelete.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave.Location = new System.Drawing.Point(383, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(62, 29);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "저장(&S)";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(513, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(62, 29);
            this.btnExit.TabIndex = 9;
            this.btnExit.Text = "닫기(&X)";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.AutoSize = true;
            this.lblFormTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblFormTitle.Location = new System.Drawing.Point(7, 7);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(112, 21);
            this.lblFormTitle.TabIndex = 7;
            this.lblFormTitle.Text = "보류대장 등록";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtSayu);
            this.panel1.Controls.Add(this.txtExamName);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.ssPatInfo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 36);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(581, 128);
            this.panel1.TabIndex = 4;
            // 
            // txtSayu
            // 
            this.txtSayu.Location = new System.Drawing.Point(103, 94);
            this.txtSayu.Name = "txtSayu";
            this.txtSayu.Size = new System.Drawing.Size(470, 25);
            this.txtSayu.TabIndex = 2;
            // 
            // txtExamName
            // 
            this.txtExamName.Location = new System.Drawing.Point(103, 62);
            this.txtExamName.Name = "txtExamName";
            this.txtExamName.Size = new System.Drawing.Size(470, 25);
            this.txtExamName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "누락 사유";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "누락 검사명";
            // 
            // ssPatInfo
            // 
            this.ssPatInfo.AccessibleDescription = "ssPatInfo, Sheet1, Row 0, Column 0, ";
            this.ssPatInfo.FocusRenderer = enhancedFocusIndicatorRenderer1;
            this.ssPatInfo.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssPatInfo.HorizontalScrollBar.Name = "";
            this.ssPatInfo.HorizontalScrollBar.Renderer = enhancedScrollBarRenderer1;
            this.ssPatInfo.HorizontalScrollBar.TabIndex = 18;
            this.ssPatInfo.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssPatInfo.Location = new System.Drawing.Point(3, 3);
            this.ssPatInfo.Name = "ssPatInfo";
            this.ssPatInfo.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssPatInfo_Sheet1});
            this.ssPatInfo.Size = new System.Drawing.Size(576, 54);
            this.ssPatInfo.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;
            this.ssPatInfo.TabIndex = 0;
            this.ssPatInfo.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssPatInfo.VerticalScrollBar.Name = "";
            this.ssPatInfo.VerticalScrollBar.Renderer = enhancedScrollBarRenderer2;
            this.ssPatInfo.VerticalScrollBar.TabIndex = 19;
            this.ssPatInfo.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            // 
            // ssPatInfo_Sheet1
            // 
            this.ssPatInfo_Sheet1.Reset();
            this.ssPatInfo_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssPatInfo_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssPatInfo_Sheet1.ColumnCount = 6;
            this.ssPatInfo_Sheet1.RowCount = 1;
            this.ssPatInfo_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPatInfo_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPatInfo_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterEnhanced";
            this.ssPatInfo_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPatInfo_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPatInfo_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPatInfo_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerEnhanced";
            this.ssPatInfo_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPatInfo_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "접수번호";
            this.ssPatInfo_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "성명";
            this.ssPatInfo_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "나이";
            this.ssPatInfo_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "사업체명";
            this.ssPatInfo_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "검진일자";
            this.ssPatInfo_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "검진종류";
            this.ssPatInfo_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPatInfo_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPatInfo_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderEnhanced";
            this.ssPatInfo_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPatInfo_Sheet1.ColumnHeader.Rows.Get(0).Height = 25F;
            this.ssPatInfo_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssPatInfo_Sheet1.Columns.Get(0).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssPatInfo_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(0).Label = "접수번호";
            this.ssPatInfo_Sheet1.Columns.Get(0).Locked = true;
            this.ssPatInfo_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(0).Width = 91F;
            this.ssPatInfo_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssPatInfo_Sheet1.Columns.Get(1).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssPatInfo_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(1).Label = "성명";
            this.ssPatInfo_Sheet1.Columns.Get(1).Locked = true;
            this.ssPatInfo_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(1).Width = 75F;
            this.ssPatInfo_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssPatInfo_Sheet1.Columns.Get(2).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssPatInfo_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssPatInfo_Sheet1.Columns.Get(2).Label = "나이";
            this.ssPatInfo_Sheet1.Columns.Get(2).Locked = true;
            this.ssPatInfo_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(2).Width = 47F;
            this.ssPatInfo_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssPatInfo_Sheet1.Columns.Get(3).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssPatInfo_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssPatInfo_Sheet1.Columns.Get(3).Label = "사업체명";
            this.ssPatInfo_Sheet1.Columns.Get(3).Locked = true;
            this.ssPatInfo_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(3).Width = 137F;
            this.ssPatInfo_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.ssPatInfo_Sheet1.Columns.Get(4).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssPatInfo_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(4).Label = "검진일자";
            this.ssPatInfo_Sheet1.Columns.Get(4).Locked = true;
            this.ssPatInfo_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(4).Width = 95F;
            this.ssPatInfo_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.ssPatInfo_Sheet1.Columns.Get(5).Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssPatInfo_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssPatInfo_Sheet1.Columns.Get(5).Label = "검진종류";
            this.ssPatInfo_Sheet1.Columns.Get(5).Locked = true;
            this.ssPatInfo_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(5).Width = 123F;
            this.ssPatInfo_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPatInfo_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPatInfo_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarEnhanced";
            this.ssPatInfo_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPatInfo_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPatInfo_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPatInfo_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderEnhanced";
            this.ssPatInfo_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPatInfo_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ssPatInfo_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
            this.ssPatInfo_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssPatInfo_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPatInfo_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPatInfo_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderEnhanced";
            this.ssPatInfo_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPatInfo_Sheet1.RowHeader.Visible = false;
            this.ssPatInfo_Sheet1.Rows.Get(0).Height = 26F;
            this.ssPatInfo_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPatInfo_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPatInfo_Sheet1.SheetCornerStyle.Parent = "CornerEnhanced";
            this.ssPatInfo_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPatInfo_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmHcPendListReg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(581, 164);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHcPendListReg";
            this.Text = "보류대장 등록";
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssPatInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssPatInfo_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtSayu;
        private System.Windows.Forms.TextBox txtExamName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private FarPoint.Win.Spread.FpSpread ssPatInfo;
        private FarPoint.Win.Spread.SheetView ssPatInfo_Sheet1;
    }
}