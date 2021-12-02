namespace ComLibB
{
    partial class frmMsymHelp
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
            this.panTitle = new System.Windows.Forms.Panel();
            this.txtData = new System.Windows.Forms.TextBox();
            this.optJong = new System.Windows.Forms.RadioButton();
            this.optCode = new System.Windows.Forms.RadioButton();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ssSCode = new FarPoint.Win.Spread.FpSpread();
            this.ssSCode_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssSCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssSCode_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.txtData);
            this.panTitle.Controls.Add(this.optJong);
            this.panTitle.Controls.Add(this.optCode);
            this.panTitle.Controls.Add(this.btnSearch);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.btnCancel);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(621, 34);
            this.panTitle.TabIndex = 75;
            // 
            // txtData
            // 
            this.txtData.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtData.Location = new System.Drawing.Point(266, 5);
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(100, 22);
            this.txtData.TabIndex = 77;
            this.txtData.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtData_KeyPress);
            // 
            // optJong
            // 
            this.optJong.AutoSize = true;
            this.optJong.BackColor = System.Drawing.Color.White;
            this.optJong.ForeColor = System.Drawing.Color.Black;
            this.optJong.Location = new System.Drawing.Point(152, 8);
            this.optJong.Name = "optJong";
            this.optJong.Size = new System.Drawing.Size(47, 16);
            this.optJong.TabIndex = 77;
            this.optJong.TabStop = true;
            this.optJong.Text = "명칭";
            this.optJong.UseVisualStyleBackColor = false;
            // 
            // optCode
            // 
            this.optCode.AutoSize = true;
            this.optCode.BackColor = System.Drawing.Color.White;
            this.optCode.ForeColor = System.Drawing.Color.Black;
            this.optCode.Location = new System.Drawing.Point(205, 8);
            this.optCode.Name = "optCode";
            this.optCode.Size = new System.Drawing.Size(47, 16);
            this.optCode.TabIndex = 78;
            this.optCode.TabStop = true;
            this.optCode.Text = "코드";
            this.optCode.UseVisualStyleBackColor = false;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.AutoSize = true;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Location = new System.Drawing.Point(386, 1);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 78;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(542, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "닫  기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.AutoSize = true;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(464, 1);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 30);
            this.btnCancel.TabIndex = 77;
            this.btnCancel.Text = "취 소";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(3, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(116, 16);
            this.lblTitle.TabIndex = 14;
            this.lblTitle.Text = "표준코드 찾기";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(621, 28);
            this.panTitleSub0.TabIndex = 76;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(12, 9);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(88, 12);
            this.lblTitleSub0.TabIndex = 22;
            this.lblTitleSub0.Text = "표준코드 찾기";
            this.lblTitleSub0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ssSCode);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(621, 275);
            this.panel1.TabIndex = 77;
            // 
            // ssSCode
            // 
            this.ssSCode.AccessibleDescription = "ssSCode, Sheet1, Row 0, Column 0, ";
            this.ssSCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssSCode.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssSCode.Location = new System.Drawing.Point(0, 0);
            this.ssSCode.Name = "ssSCode";
            this.ssSCode.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssSCode_Sheet1});
            this.ssSCode.Size = new System.Drawing.Size(621, 275);
            this.ssSCode.TabIndex = 0;
            this.ssSCode.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // ssSCode_Sheet1
            // 
            this.ssSCode_Sheet1.Reset();
            this.ssSCode_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssSCode_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssSCode_Sheet1.ColumnCount = 2;
            this.ssSCode_Sheet1.RowCount = 1;
            this.ssSCode_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "상병코드";
            this.ssSCode_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "상 병 명 칭";
            this.ssSCode_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssSCode_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSCode_Sheet1.Columns.Get(0).Label = "상병코드";
            this.ssSCode_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSCode_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssSCode_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSCode_Sheet1.Columns.Get(1).Label = "상 병 명 칭";
            this.ssSCode_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSCode_Sheet1.Columns.Get(1).Width = 500F;
            this.ssSCode_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssSCode_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmMsymHelp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(621, 337);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Name = "frmMsymHelp";
            this.Text = "표준코드찾기";
            this.Load += new System.EventHandler(this.frmMsymHelp_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssSCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssSCode_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RadioButton optJong;
        private System.Windows.Forms.RadioButton optCode;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.Panel panel1;
        private FarPoint.Win.Spread.FpSpread ssSCode;
        private FarPoint.Win.Spread.SheetView ssSCode_Sheet1;
        public System.Windows.Forms.Button btnExit;
    }
}