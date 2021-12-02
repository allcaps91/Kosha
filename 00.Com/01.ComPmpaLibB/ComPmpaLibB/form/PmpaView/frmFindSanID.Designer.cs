namespace ComPmpaLibB
{
    partial class frmFindSanID
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
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType2 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.SS1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.SS1 = new FarPoint.Win.Spread.FpSpread();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.CmdDel = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSangho = new System.Windows.Forms.TextBox();
            this.TxtPano = new System.Windows.Forms.TextBox();
            this.CmdView = new System.Windows.Forms.Button();
            this.TxtName = new System.Windows.Forms.TextBox();
            this.panTitle = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).BeginInit();
            this.panTitleSub0.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // SS1_Sheet1
            // 
            this.SS1_Sheet1.Reset();
            this.SS1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS1_Sheet1.ColumnCount = 12;
            this.SS1_Sheet1.RowCount = 1;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "D";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "PANO";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "성명";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "사고일자";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "게시일자";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "종결일자";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "COPR.NAME";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "사고접수번호";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "ROWID";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "SABUN";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "ROWID";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "변경여부";
            this.SS1_Sheet1.Columns.Get(0).CellType = checkBoxCellType2;
            this.SS1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(0).Label = "D";
            this.SS1_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(0).Width = 24F;
            this.SS1_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.SS1_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(1).Label = "PANO";
            this.SS1_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(1).Width = 87F;
            this.SS1_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(2).Label = "성명";
            this.SS1_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(2).Width = 102F;
            this.SS1_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS1_Sheet1.Columns.Get(3).Label = "사고일자";
            this.SS1_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(3).Width = 94F;
            this.SS1_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(4).Label = "게시일자";
            this.SS1_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(4).Width = 82F;
            this.SS1_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS1_Sheet1.Columns.Get(5).Label = "종결일자";
            this.SS1_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(5).Width = 77F;
            this.SS1_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS1_Sheet1.Columns.Get(6).Label = "COPR.NAME";
            this.SS1_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(6).Width = 140F;
            this.SS1_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS1_Sheet1.Columns.Get(7).Label = "사고접수번호";
            this.SS1_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(7).Width = 120F;
            this.SS1_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS1_Sheet1.Columns.Get(8).Label = "ROWID";
            this.SS1_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(8).Visible = false;
            this.SS1_Sheet1.Columns.Get(8).Width = 63F;
            this.SS1_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SS1_Sheet1.Columns.Get(9).Label = "SABUN";
            this.SS1_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(9).Width = 81F;
            this.SS1_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(10).Label = "ROWID";
            this.SS1_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(10).Visible = false;
            this.SS1_Sheet1.Columns.Get(10).Width = 48F;
            this.SS1_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(11).Label = "변경여부";
            this.SS1_Sheet1.Columns.Get(11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(11).Visible = false;
            this.SS1_Sheet1.Columns.Get(11).Width = 57F;
            this.SS1_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SS1_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SS1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS1_Sheet1.Rows.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(64))))));
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // SS1
            // 
            this.SS1.AccessibleDescription = "SS1, Sheet1, Row 0, Column 0, ";
            this.SS1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SS1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.SS1.Location = new System.Drawing.Point(0, 85);
            this.SS1.Name = "SS1";
            this.SS1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS1_Sheet1});
            this.SS1.Size = new System.Drawing.Size(928, 392);
            this.SS1.TabIndex = 28;
            this.SS1.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SS1_CellClick);
            this.SS1.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SS1_CellDoubleClick);
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.CmdDel);
            this.panTitleSub0.Controls.Add(this.btnExit);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 47);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Padding = new System.Windows.Forms.Padding(3);
            this.panTitleSub0.Size = new System.Drawing.Size(928, 38);
            this.panTitleSub0.TabIndex = 25;
            // 
            // CmdDel
            // 
            this.CmdDel.BackColor = System.Drawing.Color.Transparent;
            this.CmdDel.Dock = System.Windows.Forms.DockStyle.Right;
            this.CmdDel.Location = new System.Drawing.Point(738, 3);
            this.CmdDel.Name = "CmdDel";
            this.CmdDel.Size = new System.Drawing.Size(92, 28);
            this.CmdDel.TabIndex = 20;
            this.CmdDel.Text = "삭제";
            this.CmdDel.UseVisualStyleBackColor = false;
            this.CmdDel.Click += new System.EventHandler(this.CmdDel_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(830, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(91, 28);
            this.btnExit.TabIndex = 19;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(254, 45);
            this.panel3.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(41, 10);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(171, 21);
            this.textBox1.TabIndex = 2;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel7
            // 
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel7.Location = new System.Drawing.Point(254, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(254, 45);
            this.panel7.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 15);
            this.label5.TabIndex = 0;
            // 
            // txtSangho
            // 
            this.txtSangho.Location = new System.Drawing.Point(67, 10);
            this.txtSangho.Name = "txtSangho";
            this.txtSangho.Size = new System.Drawing.Size(171, 21);
            this.txtSangho.TabIndex = 1;
            this.txtSangho.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtPano
            // 
            this.TxtPano.Location = new System.Drawing.Point(65, 12);
            this.TxtPano.Name = "TxtPano";
            this.TxtPano.Size = new System.Drawing.Size(84, 21);
            this.TxtPano.TabIndex = 22;
            this.TxtPano.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtPano.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtPano_KeyPress);
            // 
            // CmdView
            // 
            this.CmdView.BackColor = System.Drawing.Color.Transparent;
            this.CmdView.Dock = System.Windows.Forms.DockStyle.Right;
            this.CmdView.Location = new System.Drawing.Point(829, 3);
            this.CmdView.Name = "CmdView";
            this.CmdView.Size = new System.Drawing.Size(92, 37);
            this.CmdView.TabIndex = 23;
            this.CmdView.Text = "조회";
            this.CmdView.UseVisualStyleBackColor = false;
            this.CmdView.Click += new System.EventHandler(this.CmdView_Click);
            // 
            // TxtName
            // 
            this.TxtName.Location = new System.Drawing.Point(268, 12);
            this.TxtName.Name = "TxtName";
            this.TxtName.Size = new System.Drawing.Size(96, 21);
            this.TxtName.TabIndex = 24;
            this.TxtName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtName_KeyPress);
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.label3);
            this.panTitle.Controls.Add(this.label2);
            this.panTitle.Controls.Add(this.TxtName);
            this.panTitle.Controls.Add(this.CmdView);
            this.panTitle.Controls.Add(this.TxtPano);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Padding = new System.Windows.Forms.Padding(3);
            this.panTitle.Size = new System.Drawing.Size(928, 47);
            this.panTitle.TabIndex = 24;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(229, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 12);
            this.label3.TabIndex = 26;
            this.label3.Text = "성 명";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 25;
            this.label2.Text = "등록번호";
            // 
            // frmFindSanID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(928, 477);
            this.Controls.Add(this.SS1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Name = "frmFindSanID";
            this.Text = "frmFindSanID";
            this.Load += new System.EventHandler(this.frmFindSanID_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).EndInit();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private FarPoint.Win.Spread.SheetView SS1_Sheet1;
        private FarPoint.Win.Spread.FpSpread SS1;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Button CmdDel;
        public System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSangho;
        private System.Windows.Forms.TextBox TxtPano;
        private System.Windows.Forms.Button CmdView;
        private System.Windows.Forms.TextBox TxtName;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}