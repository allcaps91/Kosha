namespace ComLibB
{
    partial class frmClinicDept
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType25 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType26 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType27 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType28 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ssDept = new FarPoint.Win.Spread.FpSpread();
            this.ssDept_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.btnView = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.orbSort = new System.Windows.Forms.GroupBox();
            this.optSeo = new System.Windows.Forms.RadioButton();
            this.optCode = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtNameE = new System.Windows.Forms.TextBox();
            this.txtNameK = new System.Windows.Forms.TextBox();
            this.txtRank = new System.Windows.Forms.TextBox();
            this.lblSeoyeol = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.lblEngName = new System.Windows.Forms.Label();
            this.lblDeptCode = new System.Windows.Forms.Label();
            this.lblKorName = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssDept)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssDept_Sheet1)).BeginInit();
            this.orbSort.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ssDept);
            this.panel2.Location = new System.Drawing.Point(0, 242);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(597, 331);
            this.panel2.TabIndex = 3;
            // 
            // ssDept
            // 
            this.ssDept.AccessibleDescription = "ssDept, Sheet1, Row 0, Column 0, ";
            this.ssDept.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssDept.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssDept.Location = new System.Drawing.Point(0, 0);
            this.ssDept.Name = "ssDept";
            this.ssDept.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssDept_Sheet1});
            this.ssDept.Size = new System.Drawing.Size(597, 331);
            this.ssDept.TabIndex = 43;
            this.ssDept.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssDept.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssDept_CellDoubleClick);
            // 
            // ssDept_Sheet1
            // 
            this.ssDept_Sheet1.Reset();
            this.ssDept_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssDept_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssDept_Sheet1.ColumnCount = 4;
            this.ssDept_Sheet1.RowCount = 1;
            this.ssDept_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "서열";
            this.ssDept_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "코드";
            this.ssDept_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "한글명칭";
            this.ssDept_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "영문명칭";
            this.ssDept_Sheet1.Columns.Get(0).CellType = textCellType25;
            this.ssDept_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssDept_Sheet1.Columns.Get(0).Label = "서열";
            this.ssDept_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssDept_Sheet1.Columns.Get(0).Width = 35F;
            this.ssDept_Sheet1.Columns.Get(1).CellType = textCellType26;
            this.ssDept_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssDept_Sheet1.Columns.Get(1).Label = "코드";
            this.ssDept_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssDept_Sheet1.Columns.Get(1).Width = 40F;
            this.ssDept_Sheet1.Columns.Get(2).CellType = textCellType27;
            this.ssDept_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssDept_Sheet1.Columns.Get(2).Label = "한글명칭";
            this.ssDept_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssDept_Sheet1.Columns.Get(2).Width = 230F;
            this.ssDept_Sheet1.Columns.Get(3).CellType = textCellType28;
            this.ssDept_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssDept_Sheet1.Columns.Get(3).Label = "영문명칭";
            this.ssDept_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssDept_Sheet1.Columns.Get(3).Width = 230F;
            this.ssDept_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssDept_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // btnView
            // 
            this.btnView.BackColor = System.Drawing.Color.Transparent;
            this.btnView.ForeColor = System.Drawing.Color.Black;
            this.btnView.Location = new System.Drawing.Point(178, 0);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(72, 30);
            this.btnView.TabIndex = 45;
            this.btnView.Text = "조 회";
            this.btnView.UseVisualStyleBackColor = false;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.ForeColor = System.Drawing.Color.Black;
            this.btnPrint.Location = new System.Drawing.Point(256, 0);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 30);
            this.btnPrint.TabIndex = 44;
            this.btnPrint.Text = "출 력";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // orbSort
            // 
            this.orbSort.Controls.Add(this.optSeo);
            this.orbSort.Controls.Add(this.optCode);
            this.orbSort.Location = new System.Drawing.Point(3, 2);
            this.orbSort.Name = "orbSort";
            this.orbSort.Size = new System.Drawing.Size(144, 30);
            this.orbSort.TabIndex = 42;
            this.orbSort.TabStop = false;
            this.orbSort.Text = "SORT";
            // 
            // optSeo
            // 
            this.optSeo.AutoSize = true;
            this.optSeo.ForeColor = System.Drawing.Color.Black;
            this.optSeo.Location = new System.Drawing.Point(12, 11);
            this.optSeo.Name = "optSeo";
            this.optSeo.Size = new System.Drawing.Size(59, 16);
            this.optSeo.TabIndex = 40;
            this.optSeo.TabStop = true;
            this.optSeo.Text = "서열순";
            this.optSeo.UseVisualStyleBackColor = true;
            // 
            // optCode
            // 
            this.optCode.AutoSize = true;
            this.optCode.ForeColor = System.Drawing.Color.Black;
            this.optCode.Location = new System.Drawing.Point(77, 11);
            this.optCode.Name = "optCode";
            this.optCode.Size = new System.Drawing.Size(59, 16);
            this.optCode.TabIndex = 41;
            this.optCode.TabStop = true;
            this.optCode.Text = "코드순";
            this.optCode.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtNameE);
            this.panel1.Controls.Add(this.txtNameK);
            this.panel1.Controls.Add(this.txtRank);
            this.panel1.Controls.Add(this.lblSeoyeol);
            this.panel1.Controls.Add(this.txtCode);
            this.panel1.Controls.Add(this.lblEngName);
            this.panel1.Controls.Add(this.lblDeptCode);
            this.panel1.Controls.Add(this.lblKorName);
            this.panel1.Location = new System.Drawing.Point(0, 58);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(597, 156);
            this.panel1.TabIndex = 2;
            // 
            // txtNameE
            // 
            this.txtNameE.Location = new System.Drawing.Point(99, 91);
            this.txtNameE.Name = "txtNameE";
            this.txtNameE.Size = new System.Drawing.Size(370, 21);
            this.txtNameE.TabIndex = 36;
            this.txtNameE.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNameE_KeyPress);
            // 
            // txtNameK
            // 
            this.txtNameK.Location = new System.Drawing.Point(99, 60);
            this.txtNameK.Name = "txtNameK";
            this.txtNameK.Size = new System.Drawing.Size(370, 21);
            this.txtNameK.TabIndex = 35;
            this.txtNameK.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNameK_KeyPress);
            // 
            // txtRank
            // 
            this.txtRank.Location = new System.Drawing.Point(369, 28);
            this.txtRank.Name = "txtRank";
            this.txtRank.Size = new System.Drawing.Size(100, 21);
            this.txtRank.TabIndex = 34;
            this.txtRank.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRank_KeyPress);
            // 
            // lblSeoyeol
            // 
            this.lblSeoyeol.AutoSize = true;
            this.lblSeoyeol.Location = new System.Drawing.Point(294, 32);
            this.lblSeoyeol.Name = "lblSeoyeol";
            this.lblSeoyeol.Size = new System.Drawing.Size(69, 12);
            this.lblSeoyeol.TabIndex = 33;
            this.lblSeoyeol.Text = "출력시 서열";
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(99, 29);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(100, 21);
            this.txtCode.TabIndex = 32;
            this.txtCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCode_KeyPress);
            // 
            // lblEngName
            // 
            this.lblEngName.AutoSize = true;
            this.lblEngName.Location = new System.Drawing.Point(12, 95);
            this.lblEngName.Name = "lblEngName";
            this.lblEngName.Size = new System.Drawing.Size(81, 12);
            this.lblEngName.TabIndex = 29;
            this.lblEngName.Text = "영     문     명";
            // 
            // lblDeptCode
            // 
            this.lblDeptCode.AutoSize = true;
            this.lblDeptCode.Location = new System.Drawing.Point(12, 33);
            this.lblDeptCode.Name = "lblDeptCode";
            this.lblDeptCode.Size = new System.Drawing.Size(81, 12);
            this.lblDeptCode.TabIndex = 26;
            this.lblDeptCode.Text = "진 료 과 코 드";
            // 
            // lblKorName
            // 
            this.lblKorName.AutoSize = true;
            this.lblKorName.Location = new System.Drawing.Point(12, 64);
            this.lblKorName.Name = "lblKorName";
            this.lblKorName.Size = new System.Drawing.Size(81, 12);
            this.lblKorName.TabIndex = 25;
            this.lblKorName.Text = "한     글     명";
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Transparent;
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Location = new System.Drawing.Point(366, 1);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(72, 30);
            this.btnDelete.TabIndex = 38;
            this.btnDelete.Text = "삭 제";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.AllowDrop = true;
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Location = new System.Drawing.Point(290, 1);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 30);
            this.btnSave.TabIndex = 37;
            this.btnSave.Text = "저 장";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.Controls.Add(this.label1);
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(596, 28);
            this.panTitleSub0.TabIndex = 86;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(6, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 12);
            this.label1.TabIndex = 23;
            this.label1.Text = "등록부분";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(12, 9);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(0, 12);
            this.lblTitleSub0.TabIndex = 22;
            this.lblTitleSub0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnCancel);
            this.panTitle.Controls.Add(this.btnDelete);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.btnSave);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(596, 34);
            this.panTitle.TabIndex = 85;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.AutoSize = true;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(442, 1);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 30);
            this.btnCancel.TabIndex = 78;
            this.btnCancel.Text = "취 소";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(518, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "닫 기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(3, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(133, 16);
            this.lblTitle.TabIndex = 14;
            this.lblTitle.Text = "진료과코드 등록";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.btnView);
            this.panel3.Controls.Add(this.btnPrint);
            this.panel3.Controls.Add(this.orbSort);
            this.panel3.ForeColor = System.Drawing.Color.White;
            this.panel3.Location = new System.Drawing.Point(0, 205);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(597, 34);
            this.panel3.TabIndex = 87;
            // 
            // frmClinicDept
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(596, 590);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "frmClinicDept";
            this.Text = "진료과코드 등록";
            this.Load += new System.EventHandler(this.frmClinicDept_Load);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssDept)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssDept_Sheet1)).EndInit();
            this.orbSort.ResumeLayout(false);
            this.orbSort.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private FarPoint.Win.Spread.FpSpread ssDept;
        private FarPoint.Win.Spread.SheetView ssDept_Sheet1;
        private System.Windows.Forms.GroupBox orbSort;
        private System.Windows.Forms.RadioButton optSeo;
        private System.Windows.Forms.RadioButton optCode;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtNameE;
        private System.Windows.Forms.TextBox txtNameK;
        private System.Windows.Forms.TextBox txtRank;
        private System.Windows.Forms.Label lblSeoyeol;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label lblEngName;
        private System.Windows.Forms.Label lblDeptCode;
        private System.Windows.Forms.Label lblKorName;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
    }
}