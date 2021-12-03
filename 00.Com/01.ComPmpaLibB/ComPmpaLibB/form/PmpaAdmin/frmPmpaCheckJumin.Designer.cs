namespace ComPmpaLibB
{
    partial class frmCheckJumin
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
            this.pnlHead = new System.Windows.Forms.Panel();
            this.chkBaby = new System.Windows.Forms.CheckBox();
            this.chkJumin = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnNhic = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSname = new System.Windows.Forms.TextBox();
            this.txtJumin2 = new System.Windows.Forms.TextBox();
            this.txtJumin1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlBody = new System.Windows.Forms.Panel();
            this.sprList = new FarPoint.Win.Spread.FpSpread();
            this.sprList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lblMsg = new System.Windows.Forms.Label();
            this.pnlHead.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.pnlBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sprList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sprList_Sheet1)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHead
            // 
            this.pnlHead.BackColor = System.Drawing.Color.RoyalBlue;
            this.pnlHead.Controls.Add(this.chkBaby);
            this.pnlHead.Controls.Add(this.chkJumin);
            this.pnlHead.Controls.Add(this.label2);
            this.pnlHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHead.ForeColor = System.Drawing.Color.White;
            this.pnlHead.Location = new System.Drawing.Point(0, 0);
            this.pnlHead.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlHead.Name = "pnlHead";
            this.pnlHead.Size = new System.Drawing.Size(690, 42);
            this.pnlHead.TabIndex = 173;
            // 
            // chkBaby
            // 
            this.chkBaby.AutoSize = true;
            this.chkBaby.BackColor = System.Drawing.Color.Transparent;
            this.chkBaby.Dock = System.Windows.Forms.DockStyle.Right;
            this.chkBaby.ForeColor = System.Drawing.Color.White;
            this.chkBaby.Location = new System.Drawing.Point(431, 0);
            this.chkBaby.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkBaby.Name = "chkBaby";
            this.chkBaby.Size = new System.Drawing.Size(123, 42);
            this.chkBaby.TabIndex = 183;
            this.chkBaby.Text = "신생아 중복허용";
            this.chkBaby.UseVisualStyleBackColor = false;
            // 
            // chkJumin
            // 
            this.chkJumin.AutoSize = true;
            this.chkJumin.BackColor = System.Drawing.Color.Transparent;
            this.chkJumin.Checked = true;
            this.chkJumin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkJumin.Dock = System.Windows.Forms.DockStyle.Right;
            this.chkJumin.ForeColor = System.Drawing.Color.White;
            this.chkJumin.Location = new System.Drawing.Point(554, 0);
            this.chkJumin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkJumin.Name = "chkJumin";
            this.chkJumin.Size = new System.Drawing.Size(136, 42);
            this.chkJumin.TabIndex = 182;
            this.chkJumin.Text = "주민번호일치 환자";
            this.chkJumin.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(14, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "신환환자 등록";
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.btnNhic);
            this.pnlTop.Controls.Add(this.btnSave);
            this.pnlTop.Controls.Add(this.btnExit);
            this.pnlTop.Controls.Add(this.label1);
            this.pnlTop.Controls.Add(this.txtSname);
            this.pnlTop.Controls.Add(this.txtJumin2);
            this.pnlTop.Controls.Add(this.txtJumin1);
            this.pnlTop.Controls.Add(this.label4);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 42);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(10, 14, 10, 14);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.pnlTop.Size = new System.Drawing.Size(690, 44);
            this.pnlTop.TabIndex = 174;
            // 
            // btnNhic
            // 
            this.btnNhic.BackColor = System.Drawing.SystemColors.Window;
            this.btnNhic.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnNhic.Location = new System.Drawing.Point(431, 6);
            this.btnNhic.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnNhic.Name = "btnNhic";
            this.btnNhic.Size = new System.Drawing.Size(85, 32);
            this.btnNhic.TabIndex = 197;
            this.btnNhic.Text = "자격검증";
            this.btnNhic.UseVisualStyleBackColor = true;
            this.btnNhic.Click += new System.EventHandler(this.btnNhic_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.SystemColors.Window;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(516, 6);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(85, 32);
            this.btnSave.TabIndex = 196;
            this.btnSave.Text = "등록";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.SystemColors.Window;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(601, 6);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(85, 32);
            this.btnExit.TabIndex = 195;
            this.btnExit.Text = "닫기(&D)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Window;
            this.label1.Location = new System.Drawing.Point(223, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 17);
            this.label1.TabIndex = 194;
            this.label1.Text = "수진자명";
            // 
            // txtSname
            // 
            this.txtSname.BackColor = System.Drawing.SystemColors.Window;
            this.txtSname.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.txtSname.Location = new System.Drawing.Point(289, 10);
            this.txtSname.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSname.Name = "txtSname";
            this.txtSname.Size = new System.Drawing.Size(126, 25);
            this.txtSname.TabIndex = 184;
            this.txtSname.Text = "박병규";
            this.txtSname.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSname_KeyPress);
            // 
            // txtJumin2
            // 
            this.txtJumin2.BackColor = System.Drawing.SystemColors.Window;
            this.txtJumin2.Location = new System.Drawing.Point(144, 10);
            this.txtJumin2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtJumin2.MaxLength = 7;
            this.txtJumin2.Name = "txtJumin2";
            this.txtJumin2.Size = new System.Drawing.Size(73, 25);
            this.txtJumin2.TabIndex = 183;
            this.txtJumin2.Text = "1234567";
            this.txtJumin2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtJumin2_KeyPress);
            // 
            // txtJumin1
            // 
            this.txtJumin1.BackColor = System.Drawing.SystemColors.Window;
            this.txtJumin1.Location = new System.Drawing.Point(79, 10);
            this.txtJumin1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtJumin1.MaxLength = 6;
            this.txtJumin1.Name = "txtJumin1";
            this.txtJumin1.Size = new System.Drawing.Size(59, 25);
            this.txtJumin1.TabIndex = 182;
            this.txtJumin1.Text = "123456";
            this.txtJumin1.TextChanged += new System.EventHandler(this.txtJumin1_TextChanged);
            this.txtJumin1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtJumin1_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.Window;
            this.label4.Location = new System.Drawing.Point(13, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 17);
            this.label4.TabIndex = 185;
            this.label4.Text = "주민번호";
            // 
            // pnlBody
            // 
            this.pnlBody.Controls.Add(this.sprList);
            this.pnlBody.Controls.Add(this.pnlBottom);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(0, 86);
            this.pnlBody.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(690, 373);
            this.pnlBody.TabIndex = 175;
            // 
            // sprList
            // 
            this.sprList.AccessibleDescription = "sprList, Sheet1, Row 0, Column 0, ";
            this.sprList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sprList.FocusRenderer = enhancedFocusIndicatorRenderer1;
            this.sprList.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.sprList.HorizontalScrollBar.Name = "";
            this.sprList.HorizontalScrollBar.Renderer = enhancedScrollBarRenderer1;
            this.sprList.HorizontalScrollBar.TabIndex = 13;
            this.sprList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.sprList.Location = new System.Drawing.Point(0, 0);
            this.sprList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.sprList.Name = "sprList";
            this.sprList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.sprList_Sheet1});
            this.sprList.Size = new System.Drawing.Size(690, 337);
            this.sprList.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;
            this.sprList.TabIndex = 2;
            this.sprList.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.sprList.VerticalScrollBar.Name = "";
            this.sprList.VerticalScrollBar.Renderer = enhancedScrollBarRenderer2;
            this.sprList.VerticalScrollBar.TabIndex = 14;
            this.sprList.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.sprList_CellDoubleClick);
            this.sprList.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.sprList_KeyPress);
            // 
            // sprList_Sheet1
            // 
            this.sprList_Sheet1.Reset();
            this.sprList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.sprList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.sprList_Sheet1.ColumnCount = 4;
            this.sprList_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.sprList_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.sprList_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterEnhanced";
            this.sprList_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.sprList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.sprList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.sprList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerEnhanced";
            this.sprList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.sprList_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "등록번호";
            this.sprList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "성명";
            this.sprList_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "주민번호";
            this.sprList_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "피보험자";
            this.sprList_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.sprList_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.sprList_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderEnhanced";
            this.sprList_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.sprList_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.sprList_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sprList_Sheet1.Columns.Get(0).Label = "등록번호";
            this.sprList_Sheet1.Columns.Get(0).Locked = true;
            this.sprList_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sprList_Sheet1.Columns.Get(0).Width = 82F;
            this.sprList_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.sprList_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.sprList_Sheet1.Columns.Get(1).Label = "성명";
            this.sprList_Sheet1.Columns.Get(1).Locked = true;
            this.sprList_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sprList_Sheet1.Columns.Get(1).Width = 161F;
            this.sprList_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.sprList_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sprList_Sheet1.Columns.Get(2).Label = "주민번호";
            this.sprList_Sheet1.Columns.Get(2).Locked = true;
            this.sprList_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sprList_Sheet1.Columns.Get(2).Width = 141F;
            this.sprList_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.sprList_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.sprList_Sheet1.Columns.Get(3).Label = "피보험자";
            this.sprList_Sheet1.Columns.Get(3).Locked = true;
            this.sprList_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sprList_Sheet1.Columns.Get(3).Width = 121F;
            this.sprList_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.sprList_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.sprList_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarEnhanced";
            this.sprList_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.sprList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.sprList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.sprList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderEnhanced";
            this.sprList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.sprList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.sprList_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.sprList_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.sprList_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderEnhanced";
            this.sprList_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.sprList_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.sprList_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.sprList_Sheet1.SheetCornerStyle.Parent = "CornerEnhanced";
            this.sprList_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.sprList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Info;
            this.pnlBottom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlBottom.Controls.Add(this.lblMsg);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 337);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(690, 36);
            this.pnlBottom.TabIndex = 0;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Location = new System.Drawing.Point(13, 10);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(48, 17);
            this.lblMsg.TabIndex = 0;
            this.lblMsg.Text = "lblMsg";
            // 
            // frmCheckJumin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(690, 459);
            this.ControlBox = false;
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlHead);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmCheckJumin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "  ";
            this.pnlHead.ResumeLayout(false);
            this.pnlHead.PerformLayout();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sprList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sprList_Sheet1)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlHead;
        private System.Windows.Forms.CheckBox chkBaby;
        private System.Windows.Forms.CheckBox chkJumin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.TextBox txtSname;
        private System.Windows.Forms.TextBox txtJumin2;
        private System.Windows.Forms.TextBox txtJumin1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        public System.Windows.Forms.Button btnExit;
        private FarPoint.Win.Spread.FpSpread sprList;
        private FarPoint.Win.Spread.SheetView sprList_Sheet1;
        private System.Windows.Forms.Button btnNhic;
    }
}