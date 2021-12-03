namespace HC_Measurement
{
    partial class frmHcChkCard02
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
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer1 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer2 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            this.panUCodes = new System.Windows.Forms.Panel();
            this.ssGONG = new FarPoint.Win.Spread.FpSpread();
            this.ssGONG_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.chkDel_Gong = new System.Windows.Forms.CheckBox();
            this.btnAdd1_Add = new System.Windows.Forms.Button();
            this.btnAdd1_Ins = new System.Windows.Forms.Button();
            this.btnDel_Gong = new System.Windows.Forms.Button();
            this.btnSave_Gong = new System.Windows.Forms.Button();
            this.panUCodes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssGONG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssGONG_Sheet1)).BeginInit();
            this.panel7.SuspendLayout();
            this.panel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // panUCodes
            // 
            this.panUCodes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panUCodes.Controls.Add(this.ssGONG);
            this.panUCodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panUCodes.Location = new System.Drawing.Point(0, 53);
            this.panUCodes.Name = "panUCodes";
            this.panUCodes.Size = new System.Drawing.Size(874, 887);
            this.panUCodes.TabIndex = 190;
            // 
            // ssGONG
            // 
            this.ssGONG.AccessibleDescription = "ssGONG, Sheet1, Row 0, Column 0, ";
            this.ssGONG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssGONG.EditModeReplace = true;
            this.ssGONG.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssGONG.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssGONG.HorizontalScrollBar.Renderer = flatScrollBarRenderer1;
            this.ssGONG.HorizontalScrollBar.TabIndex = 1;
            this.ssGONG.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssGONG.Location = new System.Drawing.Point(0, 0);
            this.ssGONG.Name = "ssGONG";
            this.ssGONG.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssGONG_Sheet1});
            this.ssGONG.Size = new System.Drawing.Size(872, 885);
            this.ssGONG.TabIndex = 166;
            this.ssGONG.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssGONG.VerticalScrollBar.Name = "";
            flatScrollBarRenderer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssGONG.VerticalScrollBar.Renderer = flatScrollBarRenderer2;
            this.ssGONG.VerticalScrollBar.TabIndex = 2;
            this.ssGONG.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // ssGONG_Sheet1
            // 
            this.ssGONG_Sheet1.Reset();
            this.ssGONG_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssGONG_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssGONG_Sheet1.ColumnCount = 5;
            this.ssGONG_Sheet1.RowCount = 1;
            this.ssGONG_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssGONG_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssGONG_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.ssGONG_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssGONG_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssGONG_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssGONG_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.ssGONG_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssGONG_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssGONG_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssGONG_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.ssGONG_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssGONG_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssGONG_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssGONG_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.ssGONG_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssGONG_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssGONG_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssGONG_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.ssGONG_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssGONG_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssGONG_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssGONG_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.ssGONG_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssGONG_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.ssGONG_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssGONG_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssGONG_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssGONG_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.ssGONG_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssGONG_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssGONG_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssGONG_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.ssGONG_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssGONG_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.ssGONG_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.label3);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.ForeColor = System.Drawing.Color.White;
            this.panel7.Location = new System.Drawing.Point(0, 29);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(874, 24);
            this.panel7.TabIndex = 189;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(250, 22);
            this.label3.TabIndex = 4;
            this.label3.Text = " 공정 및 유해인자별 측정계획";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.chkDel_Gong);
            this.panel8.Controls.Add(this.btnAdd1_Add);
            this.panel8.Controls.Add(this.btnAdd1_Ins);
            this.panel8.Controls.Add(this.btnDel_Gong);
            this.panel8.Controls.Add(this.btnSave_Gong);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(874, 29);
            this.panel8.TabIndex = 188;
            // 
            // chkDel_Gong
            // 
            this.chkDel_Gong.AutoSize = true;
            this.chkDel_Gong.ForeColor = System.Drawing.Color.DarkRed;
            this.chkDel_Gong.Location = new System.Drawing.Point(343, 5);
            this.chkDel_Gong.Name = "chkDel_Gong";
            this.chkDel_Gong.Size = new System.Drawing.Size(79, 21);
            this.chkDel_Gong.TabIndex = 32;
            this.chkDel_Gong.Text = "삭제포함";
            this.chkDel_Gong.UseVisualStyleBackColor = true;
            // 
            // btnAdd1_Add
            // 
            this.btnAdd1_Add.BackColor = System.Drawing.SystemColors.Control;
            this.btnAdd1_Add.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAdd1_Add.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAdd1_Add.Location = new System.Drawing.Point(538, 0);
            this.btnAdd1_Add.Name = "btnAdd1_Add";
            this.btnAdd1_Add.Size = new System.Drawing.Size(82, 29);
            this.btnAdd1_Add.TabIndex = 31;
            this.btnAdd1_Add.Text = "추가";
            this.btnAdd1_Add.UseVisualStyleBackColor = false;
            // 
            // btnAdd1_Ins
            // 
            this.btnAdd1_Ins.BackColor = System.Drawing.SystemColors.Control;
            this.btnAdd1_Ins.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAdd1_Ins.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAdd1_Ins.Location = new System.Drawing.Point(620, 0);
            this.btnAdd1_Ins.Name = "btnAdd1_Ins";
            this.btnAdd1_Ins.Size = new System.Drawing.Size(82, 29);
            this.btnAdd1_Ins.TabIndex = 30;
            this.btnAdd1_Ins.Text = "삽입";
            this.btnAdd1_Ins.UseVisualStyleBackColor = false;
            // 
            // btnDel_Gong
            // 
            this.btnDel_Gong.BackColor = System.Drawing.SystemColors.Control;
            this.btnDel_Gong.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDel_Gong.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDel_Gong.ForeColor = System.Drawing.Color.DarkRed;
            this.btnDel_Gong.Location = new System.Drawing.Point(702, 0);
            this.btnDel_Gong.Name = "btnDel_Gong";
            this.btnDel_Gong.Size = new System.Drawing.Size(82, 29);
            this.btnDel_Gong.TabIndex = 29;
            this.btnDel_Gong.Text = "삭제";
            this.btnDel_Gong.UseVisualStyleBackColor = false;
            // 
            // btnSave_Gong
            // 
            this.btnSave_Gong.BackColor = System.Drawing.SystemColors.Control;
            this.btnSave_Gong.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave_Gong.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave_Gong.Location = new System.Drawing.Point(784, 0);
            this.btnSave_Gong.Name = "btnSave_Gong";
            this.btnSave_Gong.Size = new System.Drawing.Size(90, 29);
            this.btnSave_Gong.TabIndex = 25;
            this.btnSave_Gong.Text = "저장";
            this.btnSave_Gong.UseVisualStyleBackColor = false;
            // 
            // frmHcChkCard02
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 940);
            this.Controls.Add(this.panUCodes);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panel8);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHcChkCard02";
            this.Text = "frmHcChkCard02";
            this.panUCodes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssGONG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssGONG_Sheet1)).EndInit();
            this.panel7.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panUCodes;
        private FarPoint.Win.Spread.FpSpread ssGONG;
        private FarPoint.Win.Spread.SheetView ssGONG_Sheet1;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.CheckBox chkDel_Gong;
        private System.Windows.Forms.Button btnAdd1_Add;
        private System.Windows.Forms.Button btnAdd1_Ins;
        private System.Windows.Forms.Button btnDel_Gong;
        private System.Windows.Forms.Button btnSave_Gong;
    }
}