namespace HC_Sangdam
{
    partial class frmHcSangLivingHabitPrescriptionModify
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
            FarPoint.Win.Spread.FlatFocusIndicatorRenderer flatFocusIndicatorRenderer1 = new FarPoint.Win.Spread.FlatFocusIndicatorRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer1 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer2 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnMenuSave = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkDementia = new System.Windows.Forms.CheckBox();
            this.chkDepress = new System.Windows.Forms.CheckBox();
            this.chkHabit5 = new System.Windows.Forms.CheckBox();
            this.chkHabit4 = new System.Windows.Forms.CheckBox();
            this.chkHabit3 = new System.Windows.Forms.CheckBox();
            this.chkHabit2 = new System.Windows.Forms.CheckBox();
            this.chkHabit1 = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ssPatInfo = new FarPoint.Win.Spread.FpSpread();
            this.ssPatInfo_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtWrtNo = new System.Windows.Forms.TextBox();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssPatInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssPatInfo_Sheet1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panTitle.Controls.Add(this.btnMenuSave);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(736, 35);
            this.panTitle.TabIndex = 27;
            // 
            // btnMenuSave
            // 
            this.btnMenuSave.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnMenuSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMenuSave.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnMenuSave.Location = new System.Drawing.Point(570, 0);
            this.btnMenuSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnMenuSave.Name = "btnMenuSave";
            this.btnMenuSave.Size = new System.Drawing.Size(82, 33);
            this.btnMenuSave.TabIndex = 21;
            this.btnMenuSave.Text = "저장(&S)";
            this.btnMenuSave.UseVisualStyleBackColor = false;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(652, 0);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(82, 33);
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
            this.lblTitle.Size = new System.Drawing.Size(220, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "생활습관개선 상담수정(임시)";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.ssPatInfo);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(736, 110);
            this.panel1.TabIndex = 28;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.chkDementia);
            this.panel2.Controls.Add(this.chkDepress);
            this.panel2.Controls.Add(this.chkHabit5);
            this.panel2.Controls.Add(this.chkHabit4);
            this.panel2.Controls.Add(this.chkHabit3);
            this.panel2.Controls.Add(this.chkHabit2);
            this.panel2.Controls.Add(this.chkHabit1);
            this.panel2.Location = new System.Drawing.Point(143, 70);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(584, 31);
            this.panel2.TabIndex = 49;
            // 
            // chkDementia
            // 
            this.chkDementia.AutoSize = true;
            this.chkDementia.Location = new System.Drawing.Point(462, 5);
            this.chkDementia.Name = "chkDementia";
            this.chkDementia.Size = new System.Drawing.Size(105, 21);
            this.chkDementia.TabIndex = 6;
            this.chkDementia.Text = "인지기능장애";
            this.chkDementia.UseVisualStyleBackColor = true;
            // 
            // chkDepress
            // 
            this.chkDepress.AutoSize = true;
            this.chkDepress.Location = new System.Drawing.Point(375, 5);
            this.chkDepress.Name = "chkDepress";
            this.chkDepress.Size = new System.Drawing.Size(66, 21);
            this.chkDepress.TabIndex = 5;
            this.chkDepress.Text = "우울증";
            this.chkDepress.UseVisualStyleBackColor = true;
            // 
            // chkHabit5
            // 
            this.chkHabit5.AutoSize = true;
            this.chkHabit5.Location = new System.Drawing.Point(301, 5);
            this.chkHabit5.Name = "chkHabit5";
            this.chkHabit5.Size = new System.Drawing.Size(53, 21);
            this.chkHabit5.TabIndex = 4;
            this.chkHabit5.Text = "영양";
            this.chkHabit5.UseVisualStyleBackColor = true;
            // 
            // chkHabit4
            // 
            this.chkHabit4.AutoSize = true;
            this.chkHabit4.Location = new System.Drawing.Point(227, 5);
            this.chkHabit4.Name = "chkHabit4";
            this.chkHabit4.Size = new System.Drawing.Size(53, 21);
            this.chkHabit4.TabIndex = 3;
            this.chkHabit4.Text = "비만";
            this.chkHabit4.UseVisualStyleBackColor = true;
            // 
            // chkHabit3
            // 
            this.chkHabit3.AutoSize = true;
            this.chkHabit3.Location = new System.Drawing.Point(153, 5);
            this.chkHabit3.Name = "chkHabit3";
            this.chkHabit3.Size = new System.Drawing.Size(53, 21);
            this.chkHabit3.TabIndex = 2;
            this.chkHabit3.Text = "운동";
            this.chkHabit3.UseVisualStyleBackColor = true;
            // 
            // chkHabit2
            // 
            this.chkHabit2.AutoSize = true;
            this.chkHabit2.Location = new System.Drawing.Point(79, 5);
            this.chkHabit2.Name = "chkHabit2";
            this.chkHabit2.Size = new System.Drawing.Size(53, 21);
            this.chkHabit2.TabIndex = 1;
            this.chkHabit2.Text = "흡연";
            this.chkHabit2.UseVisualStyleBackColor = true;
            // 
            // chkHabit1
            // 
            this.chkHabit1.AutoSize = true;
            this.chkHabit1.Location = new System.Drawing.Point(5, 5);
            this.chkHabit1.Name = "chkHabit1";
            this.chkHabit1.Size = new System.Drawing.Size(53, 21);
            this.chkHabit1.TabIndex = 0;
            this.chkHabit1.Text = "음주";
            this.chkHabit1.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.LightBlue;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(11, 75);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(116, 24);
            this.label7.TabIndex = 48;
            this.label7.Text = "생활습관개선";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ssPatInfo
            // 
            this.ssPatInfo.AccessibleDescription = "";
            this.ssPatInfo.FocusRenderer = flatFocusIndicatorRenderer1;
            this.ssPatInfo.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssPatInfo.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer1.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            flatScrollBarRenderer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            flatScrollBarRenderer1.TrackBarBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(219)))), ((int)(((byte)(219)))));
            this.ssPatInfo.HorizontalScrollBar.Renderer = flatScrollBarRenderer1;
            this.ssPatInfo.HorizontalScrollBar.TabIndex = 8;
            this.ssPatInfo.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssPatInfo.Location = new System.Drawing.Point(141, 9);
            this.ssPatInfo.Name = "ssPatInfo";
            this.ssPatInfo.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssPatInfo_Sheet1});
            this.ssPatInfo.Size = new System.Drawing.Size(587, 54);
            this.ssPatInfo.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2016Colorful;
            this.ssPatInfo.TabIndex = 3;
            this.ssPatInfo.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssPatInfo.VerticalScrollBar.Name = "";
            flatScrollBarRenderer2.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            flatScrollBarRenderer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            flatScrollBarRenderer2.TrackBarBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(219)))), ((int)(((byte)(219)))));
            this.ssPatInfo.VerticalScrollBar.Renderer = flatScrollBarRenderer2;
            this.ssPatInfo.VerticalScrollBar.TabIndex = 9;
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
            this.ssPatInfo_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlatOffice2016Colorful";
            this.ssPatInfo_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPatInfo_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPatInfo_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPatInfo_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlatOffice2016Colorful";
            this.ssPatInfo_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPatInfo_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "등록번호";
            this.ssPatInfo_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "성명";
            this.ssPatInfo_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "나이";
            this.ssPatInfo_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "사업체명";
            this.ssPatInfo_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "접수일자";
            this.ssPatInfo_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "검진종류";
            this.ssPatInfo_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPatInfo_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPatInfo_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlatOffice2016Colorful";
            this.ssPatInfo_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPatInfo_Sheet1.ColumnHeader.Rows.Get(0).Height = 25F;
            this.ssPatInfo_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssPatInfo_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(0).Label = "등록번호";
            this.ssPatInfo_Sheet1.Columns.Get(0).Locked = true;
            this.ssPatInfo_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(0).Width = 80F;
            this.ssPatInfo_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssPatInfo_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(1).Label = "성명";
            this.ssPatInfo_Sheet1.Columns.Get(1).Locked = true;
            this.ssPatInfo_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(1).Width = 70F;
            this.ssPatInfo_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssPatInfo_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(2).Label = "나이";
            this.ssPatInfo_Sheet1.Columns.Get(2).Locked = true;
            this.ssPatInfo_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(2).Width = 42F;
            this.ssPatInfo_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssPatInfo_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssPatInfo_Sheet1.Columns.Get(3).Label = "사업체명";
            this.ssPatInfo_Sheet1.Columns.Get(3).Locked = true;
            this.ssPatInfo_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(3).Width = 138F;
            this.ssPatInfo_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.ssPatInfo_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(4).Label = "접수일자";
            this.ssPatInfo_Sheet1.Columns.Get(4).Locked = true;
            this.ssPatInfo_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(4).Width = 101F;
            this.ssPatInfo_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.ssPatInfo_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssPatInfo_Sheet1.Columns.Get(5).Label = "검진종류";
            this.ssPatInfo_Sheet1.Columns.Get(5).Locked = true;
            this.ssPatInfo_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPatInfo_Sheet1.Columns.Get(5).Width = 152F;
            this.ssPatInfo_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPatInfo_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPatInfo_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlatOffice2016Colorful";
            this.ssPatInfo_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPatInfo_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPatInfo_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPatInfo_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlatOffice2016Colorful";
            this.ssPatInfo_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPatInfo_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ssPatInfo_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
            this.ssPatInfo_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssPatInfo_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPatInfo_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPatInfo_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlatOffice2016Colorful";
            this.ssPatInfo_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPatInfo_Sheet1.RowHeader.Visible = false;
            this.ssPatInfo_Sheet1.Rows.Get(0).Height = 29F;
            this.ssPatInfo_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPatInfo_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPatInfo_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlatOffice2016Colorful";
            this.ssPatInfo_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPatInfo_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtWrtNo);
            this.groupBox1.Location = new System.Drawing.Point(7, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(127, 58);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "접수번호";
            // 
            // txtWrtNo
            // 
            this.txtWrtNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtWrtNo.Location = new System.Drawing.Point(7, 22);
            this.txtWrtNo.MaxLength = 8;
            this.txtWrtNo.Name = "txtWrtNo";
            this.txtWrtNo.Size = new System.Drawing.Size(113, 25);
            this.txtWrtNo.TabIndex = 0;
            this.txtWrtNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // frmHcSangLivingHabitPrescriptionModify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(736, 145);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHcSangLivingHabitPrescriptionModify";
            this.Text = "생활습관개선 상담수정(임시)";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssPatInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssPatInfo_Sheet1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnMenuSave;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private FarPoint.Win.Spread.FpSpread ssPatInfo;
        private FarPoint.Win.Spread.SheetView ssPatInfo_Sheet1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtWrtNo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox chkDementia;
        private System.Windows.Forms.CheckBox chkDepress;
        private System.Windows.Forms.CheckBox chkHabit5;
        private System.Windows.Forms.CheckBox chkHabit4;
        private System.Windows.Forms.CheckBox chkHabit3;
        private System.Windows.Forms.CheckBox chkHabit2;
        private System.Windows.Forms.CheckBox chkHabit1;
        private System.Windows.Forms.Label label7;
    }
}