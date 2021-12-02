namespace ComHpcLibB
{
    partial class frmHcAmRecommendationsCommon
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
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType1 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.ComboBoxCellType comboBoxCellType1 = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
            FarPoint.Win.Spread.CellType.ComboBoxCellType comboBoxCellType2 = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblHelp = new System.Windows.Forms.Label();
            this.btnLSogen1 = new System.Windows.Forms.Button();
            this.btnBSogen = new System.Windows.Forms.Button();
            this.btnWSogen = new System.Windows.Forms.Button();
            this.btnSogen = new System.Windows.Forms.Button();
            this.btnColon = new System.Windows.Forms.Button();
            this.btnStmch = new System.Windows.Forms.Button();
            this.btnSeq = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.SS1 = new FarPoint.Win.Spread.FpSpread();
            this.SS1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panTitle.Controls.Add(this.btnSave);
            this.panTitle.Controls.Add(this.btnCancel);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1055, 35);
            this.panTitle.TabIndex = 23;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave.Location = new System.Drawing.Point(744, 0);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(103, 33);
            this.btnSave.TabIndex = 22;
            this.btnSave.Text = "저장(&O)";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCancel.Location = new System.Drawing.Point(847, 0);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(103, 33);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "저장안함(&C)";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(950, 0);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(103, 33);
            this.btnExit.TabIndex = 20;
            this.btnExit.Text = "닫 기(&X)";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(3, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(166, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "암판정 권고사항 등록";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblHelp);
            this.panel1.Controls.Add(this.btnLSogen1);
            this.panel1.Controls.Add(this.btnBSogen);
            this.panel1.Controls.Add(this.btnWSogen);
            this.panel1.Controls.Add(this.btnSogen);
            this.panel1.Controls.Add(this.btnColon);
            this.panel1.Controls.Add(this.btnStmch);
            this.panel1.Controls.Add(this.btnSeq);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1055, 44);
            this.panel1.TabIndex = 24;
            // 
            // lblHelp
            // 
            this.lblHelp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblHelp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblHelp.Location = new System.Drawing.Point(659, 6);
            this.lblHelp.Name = "lblHelp";
            this.lblHelp.Size = new System.Drawing.Size(389, 30);
            this.lblHelp.TabIndex = 30;
            this.lblHelp.Text = " 권고사항은 최대 600자를 넘을수 없습니다.";
            this.lblHelp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnLSogen1
            // 
            this.btnLSogen1.BackColor = System.Drawing.Color.White;
            this.btnLSogen1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLSogen1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnLSogen1.Location = new System.Drawing.Point(569, 5);
            this.btnLSogen1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnLSogen1.Name = "btnLSogen1";
            this.btnLSogen1.Size = new System.Drawing.Size(83, 33);
            this.btnLSogen1.TabIndex = 29;
            this.btnLSogen1.Text = "폐암판정";
            this.btnLSogen1.UseVisualStyleBackColor = false;
            // 
            // btnBSogen
            // 
            this.btnBSogen.BackColor = System.Drawing.Color.White;
            this.btnBSogen.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnBSogen.Location = new System.Drawing.Point(391, 5);
            this.btnBSogen.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBSogen.Name = "btnBSogen";
            this.btnBSogen.Size = new System.Drawing.Size(83, 33);
            this.btnBSogen.TabIndex = 28;
            this.btnBSogen.Text = "유방암판정";
            this.btnBSogen.UseVisualStyleBackColor = false;
            // 
            // btnWSogen
            // 
            this.btnWSogen.BackColor = System.Drawing.Color.White;
            this.btnWSogen.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnWSogen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btnWSogen.Location = new System.Drawing.Point(480, 5);
            this.btnWSogen.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnWSogen.Name = "btnWSogen";
            this.btnWSogen.Size = new System.Drawing.Size(83, 33);
            this.btnWSogen.TabIndex = 27;
            this.btnWSogen.Text = "자궁암판정";
            this.btnWSogen.UseVisualStyleBackColor = false;
            // 
            // btnSogen
            // 
            this.btnSogen.BackColor = System.Drawing.Color.White;
            this.btnSogen.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSogen.ForeColor = System.Drawing.Color.Blue;
            this.btnSogen.Location = new System.Drawing.Point(302, 5);
            this.btnSogen.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSogen.Name = "btnSogen";
            this.btnSogen.Size = new System.Drawing.Size(83, 33);
            this.btnSogen.TabIndex = 26;
            this.btnSogen.Text = "간암판정";
            this.btnSogen.UseVisualStyleBackColor = false;
            // 
            // btnColon
            // 
            this.btnColon.BackColor = System.Drawing.Color.White;
            this.btnColon.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnColon.ForeColor = System.Drawing.Color.Green;
            this.btnColon.Location = new System.Drawing.Point(213, 5);
            this.btnColon.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnColon.Name = "btnColon";
            this.btnColon.Size = new System.Drawing.Size(83, 33);
            this.btnColon.TabIndex = 25;
            this.btnColon.Text = "대장암판정";
            this.btnColon.UseVisualStyleBackColor = false;
            // 
            // btnStmch
            // 
            this.btnStmch.BackColor = System.Drawing.Color.White;
            this.btnStmch.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnStmch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnStmch.Location = new System.Drawing.Point(124, 5);
            this.btnStmch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnStmch.Name = "btnStmch";
            this.btnStmch.Size = new System.Drawing.Size(83, 33);
            this.btnStmch.TabIndex = 24;
            this.btnStmch.Text = "위암판정";
            this.btnStmch.UseVisualStyleBackColor = false;
            // 
            // btnSeq
            // 
            this.btnSeq.BackColor = System.Drawing.Color.White;
            this.btnSeq.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSeq.Location = new System.Drawing.Point(5, 5);
            this.btnSeq.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSeq.Name = "btnSeq";
            this.btnSeq.Size = new System.Drawing.Size(103, 33);
            this.btnSeq.TabIndex = 23;
            this.btnSeq.Text = "자동코드부여";
            this.btnSeq.UseVisualStyleBackColor = false;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.SS1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 79);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1055, 540);
            this.panel2.TabIndex = 25;
            // 
            // SS1
            // 
            this.SS1.AccessibleDescription = "SS1, Sheet1, Row 0, Column 0, ";
            this.SS1.FocusRenderer = enhancedFocusIndicatorRenderer1;
            this.SS1.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SS1.HorizontalScrollBar.Name = "";
            this.SS1.HorizontalScrollBar.Renderer = enhancedScrollBarRenderer1;
            this.SS1.HorizontalScrollBar.TabIndex = 32;
            this.SS1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.SS1.Location = new System.Drawing.Point(7, 5);
            this.SS1.Name = "SS1";
            this.SS1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS1_Sheet1});
            this.SS1.Size = new System.Drawing.Size(1041, 529);
            this.SS1.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;
            this.SS1.TabIndex = 0;
            this.SS1.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SS1.VerticalScrollBar.Name = "";
            this.SS1.VerticalScrollBar.Renderer = enhancedScrollBarRenderer2;
            this.SS1.VerticalScrollBar.TabIndex = 33;
            this.SS1.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // SS1_Sheet1
            // 
            this.SS1_Sheet1.Reset();
            this.SS1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS1_Sheet1.ColumnCount = 6;
            this.SS1_Sheet1.RowCount = 50;
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterEnhanced";
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerEnhanced";
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "코드순서";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "판정구분";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "검사구분";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "권고사항";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "RowId";
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderEnhanced";
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.Rows.Get(0).Height = 25F;
            this.SS1_Sheet1.Columns.Get(0).CellType = checkBoxCellType1;
            this.SS1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(0).Width = 24F;
            this.SS1_Sheet1.Columns.Get(1).BackColor = System.Drawing.Color.LightCyan;
            this.SS1_Sheet1.Columns.Get(1).CellType = textCellType1;
            this.SS1_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(1).Label = "코드순서";
            this.SS1_Sheet1.Columns.Get(1).Locked = true;
            this.SS1_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(1).Width = 72F;
            this.SS1_Sheet1.Columns.Get(2).BackColor = System.Drawing.Color.LightGoldenrodYellow;
            comboBoxCellType1.AllowEditorVerticalAlign = true;
            comboBoxCellType1.ButtonAlign = FarPoint.Win.ButtonAlign.Right;
            this.SS1_Sheet1.Columns.Get(2).CellType = comboBoxCellType1;
            this.SS1_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS1_Sheet1.Columns.Get(2).Label = "판정구분";
            this.SS1_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(2).Width = 166F;
            comboBoxCellType2.AllowEditorVerticalAlign = true;
            comboBoxCellType2.ButtonAlign = FarPoint.Win.ButtonAlign.Right;
            this.SS1_Sheet1.Columns.Get(3).CellType = comboBoxCellType2;
            this.SS1_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS1_Sheet1.Columns.Get(3).Label = "검사구분";
            this.SS1_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(3).Width = 134F;
            textCellType2.Multiline = true;
            textCellType2.WordWrap = true;
            this.SS1_Sheet1.Columns.Get(4).CellType = textCellType2;
            this.SS1_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS1_Sheet1.Columns.Get(4).Label = "권고사항";
            this.SS1_Sheet1.Columns.Get(4).Locked = true;
            this.SS1_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(4).Width = 589F;
            this.SS1_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS1_Sheet1.Columns.Get(5).Label = "RowId";
            this.SS1_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(5).Width = 118F;
            this.SS1_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarEnhanced";
            this.SS1_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderEnhanced";
            this.SS1_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.SS1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS1_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderEnhanced";
            this.SS1_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.SheetCornerStyle.Parent = "CornerEnhanced";
            this.SS1_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmHcAmRecommendationsCommon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1055, 619);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHcAmRecommendationsCommon";
            this.Text = "암판정 권고사항 등록";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblHelp;
        private System.Windows.Forms.Button btnLSogen1;
        private System.Windows.Forms.Button btnBSogen;
        private System.Windows.Forms.Button btnWSogen;
        private System.Windows.Forms.Button btnSogen;
        private System.Windows.Forms.Button btnColon;
        private System.Windows.Forms.Button btnStmch;
        private System.Windows.Forms.Button btnSeq;
        private System.Windows.Forms.Panel panel2;
        private FarPoint.Win.Spread.FpSpread SS1;
        private FarPoint.Win.Spread.SheetView SS1_Sheet1;
    }
}