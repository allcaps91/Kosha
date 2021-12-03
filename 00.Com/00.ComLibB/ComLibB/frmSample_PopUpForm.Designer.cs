namespace ComLibB
{
    partial class frmSample_PopUpForm
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle0 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ssSpread = new FarPoint.Win.Spread.FpSpread();
            this.ssSpread_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitleSub2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblTitleSub1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.panTitle2 = new System.Windows.Forms.Panel();
            this.chkDelYNb = new System.Windows.Forms.CheckBox();
            this.txtGRPFORMNOb = new System.Windows.Forms.TextBox();
            this.lblItem1 = new System.Windows.Forms.Label();
            this.lblItem0 = new System.Windows.Forms.Label();
            this.txtDISPSEQb = new System.Windows.Forms.TextBox();
            this.txtGRPFORMNAMEb = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panTitleSub1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panTitle0.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssSpread)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssSpread_Sheet1)).BeginInit();
            this.panTitleSub0.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.panTitleSub2.SuspendLayout();
            this.panTitle2.SuspendLayout();
            this.panTitleSub1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle0
            // 
            this.panTitle0.BackColor = System.Drawing.Color.White;
            this.panTitle0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle0.Controls.Add(this.btnExit);
            this.panTitle0.Controls.Add(this.lblTitle);
            this.panTitle0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle0.ForeColor = System.Drawing.Color.White;
            this.panTitle0.Location = new System.Drawing.Point(0, 0);
            this.panTitle0.Name = "panTitle0";
            this.panTitle0.Size = new System.Drawing.Size(964, 38);
            this.panTitle0.TabIndex = 73;
            // 
            // btnExit
            // 
            this.btnExit.AutoSize = true;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(867, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(79, 29);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "닫  기";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(5, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(122, 21);
            this.lblTitle.TabIndex = 14;
            this.lblTitle.Text = "서식지그룹관리";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ssSpread);
            this.panel1.Controls.Add(this.panTitleSub0);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 38);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(2);
            this.panel1.Size = new System.Drawing.Size(301, 600);
            this.panel1.TabIndex = 74;
            // 
            // ssSpread
            // 
            this.ssSpread.AccessibleDescription = "";
            this.ssSpread.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssSpread.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssSpread.Location = new System.Drawing.Point(2, 29);
            this.ssSpread.Name = "ssSpread";
            this.ssSpread.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssSpread_Sheet1});
            this.ssSpread.Size = new System.Drawing.Size(297, 569);
            this.ssSpread.TabIndex = 77;
            // 
            // ssSpread_Sheet1
            // 
            this.ssSpread_Sheet1.Reset();
            this.ssSpread_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssSpread_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            ssSpread_Sheet1.ColumnCount = 2;
            ssSpread_Sheet1.RowCount = 1;
            this.ssSpread_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "코드";
            this.ssSpread_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "그 룹 명";
            this.ssSpread_Sheet1.Columns.Get(0).CellType = textCellType5;
            this.ssSpread_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSpread_Sheet1.Columns.Get(0).Label = "코드";
            this.ssSpread_Sheet1.Columns.Get(1).CellType = textCellType6;
            this.ssSpread_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSpread_Sheet1.Columns.Get(1).Label = "그 룹 명";
            this.ssSpread_Sheet1.Columns.Get(1).Width = 290F;
            this.ssSpread_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ssSpread_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssSpread_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(2, 2);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(297, 27);
            this.panTitleSub0.TabIndex = 75;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(12, 3);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(47, 17);
            this.lblTitleSub0.TabIndex = 21;
            this.lblTitleSub0.Text = "대분류";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ssView);
            this.panel2.Controls.Add(this.panTitleSub2);
            this.panel2.Controls.Add(this.panTitle2);
            this.panel2.Controls.Add(this.panTitleSub1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(301, 38);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(2);
            this.panel2.Size = new System.Drawing.Size(663, 600);
            this.panel2.TabIndex = 75;
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssView.Location = new System.Drawing.Point(2, 116);
            this.ssView.Name = "ssView";
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(659, 482);
            this.ssView.TabIndex = 78;
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            ssView_Sheet1.ColumnCount = 2;
            ssView_Sheet1.RowCount = 1;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "코드";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "그 룹 명";
            this.ssView_Sheet1.Columns.Get(0).CellType = textCellType7;
            this.ssView_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(0).Label = "코드";
            this.ssView_Sheet1.Columns.Get(1).CellType = textCellType8;
            this.ssView_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(1).Label = "그 룹 명";
            this.ssView_Sheet1.Columns.Get(1).Width = 290F;
            this.ssView_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panTitleSub2
            // 
            this.panTitleSub2.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panTitleSub2.Controls.Add(this.panel4);
            this.panTitleSub2.Controls.Add(this.lblTitleSub1);
            this.panTitleSub2.Controls.Add(this.btnSave);
            this.panTitleSub2.Controls.Add(this.btnDown);
            this.panTitleSub2.Controls.Add(this.btnUp);
            this.panTitleSub2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub2.Location = new System.Drawing.Point(2, 89);
            this.panTitleSub2.Name = "panTitleSub2";
            this.panTitleSub2.Size = new System.Drawing.Size(659, 27);
            this.panTitleSub2.TabIndex = 77;
            // 
            // panel4
            // 
            this.panel4.Location = new System.Drawing.Point(0, 23);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(881, 414);
            this.panel4.TabIndex = 6;
            // 
            // lblTitleSub1
            // 
            this.lblTitleSub1.AutoSize = true;
            this.lblTitleSub1.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub1.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub1.Location = new System.Drawing.Point(12, 4);
            this.lblTitleSub1.Name = "lblTitleSub1";
            this.lblTitleSub1.Size = new System.Drawing.Size(60, 17);
            this.lblTitleSub1.TabIndex = 95;
            this.lblTitleSub1.Text = "순서정렬";
            this.lblTitleSub1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.AutoSize = true;
            this.btnSave.Location = new System.Drawing.Point(607, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(48, 22);
            this.btnSave.TabIndex = 93;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.AutoSize = true;
            this.btnDown.Location = new System.Drawing.Point(560, 0);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(48, 22);
            this.btnDown.TabIndex = 92;
            this.btnDown.Text = "아래";
            this.btnDown.UseVisualStyleBackColor = true;
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.AutoSize = true;
            this.btnUp.Location = new System.Drawing.Point(513, 0);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(48, 22);
            this.btnUp.TabIndex = 91;
            this.btnUp.Text = "위";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // panTitle2
            // 
            this.panTitle2.BackColor = System.Drawing.Color.White;
            this.panTitle2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle2.Controls.Add(this.chkDelYNb);
            this.panTitle2.Controls.Add(this.txtGRPFORMNOb);
            this.panTitle2.Controls.Add(this.lblItem1);
            this.panTitle2.Controls.Add(this.lblItem0);
            this.panTitle2.Controls.Add(this.txtDISPSEQb);
            this.panTitle2.Controls.Add(this.txtGRPFORMNAMEb);
            this.panTitle2.Controls.Add(this.label2);
            this.panTitle2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle2.Location = new System.Drawing.Point(2, 29);
            this.panTitle2.Name = "panTitle2";
            this.panTitle2.Size = new System.Drawing.Size(659, 60);
            this.panTitle2.TabIndex = 76;
            // 
            // chkDelYNb
            // 
            this.chkDelYNb.AutoSize = true;
            this.chkDelYNb.Location = new System.Drawing.Point(382, 8);
            this.chkDelYNb.Name = "chkDelYNb";
            this.chkDelYNb.Size = new System.Drawing.Size(48, 16);
            this.chkDelYNb.TabIndex = 90;
            this.chkDelYNb.Text = "중지";
            this.chkDelYNb.UseVisualStyleBackColor = true;
            // 
            // txtGRPFORMNOb
            // 
            this.txtGRPFORMNOb.Enabled = false;
            this.txtGRPFORMNOb.Location = new System.Drawing.Point(77, 6);
            this.txtGRPFORMNOb.Name = "txtGRPFORMNOb";
            this.txtGRPFORMNOb.Size = new System.Drawing.Size(101, 21);
            this.txtGRPFORMNOb.TabIndex = 87;
            // 
            // lblItem1
            // 
            this.lblItem1.BackColor = System.Drawing.Color.RoyalBlue;
            this.lblItem1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblItem1.ForeColor = System.Drawing.Color.White;
            this.lblItem1.Location = new System.Drawing.Point(204, 6);
            this.lblItem1.Name = "lblItem1";
            this.lblItem1.Size = new System.Drawing.Size(66, 21);
            this.lblItem1.TabIndex = 88;
            this.lblItem1.Text = "표시순서";
            this.lblItem1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblItem0
            // 
            this.lblItem0.BackColor = System.Drawing.Color.RoyalBlue;
            this.lblItem0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblItem0.ForeColor = System.Drawing.Color.White;
            this.lblItem0.Location = new System.Drawing.Point(7, 6);
            this.lblItem0.Name = "lblItem0";
            this.lblItem0.Size = new System.Drawing.Size(66, 21);
            this.lblItem0.TabIndex = 86;
            this.lblItem0.Text = "그룹코드";
            this.lblItem0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtDISPSEQb
            // 
            this.txtDISPSEQb.Location = new System.Drawing.Point(275, 6);
            this.txtDISPSEQb.Name = "txtDISPSEQb";
            this.txtDISPSEQb.Size = new System.Drawing.Size(101, 21);
            this.txtDISPSEQb.TabIndex = 89;
            // 
            // txtGRPFORMNAMEb
            // 
            this.txtGRPFORMNAMEb.Location = new System.Drawing.Point(77, 31);
            this.txtGRPFORMNAMEb.Name = "txtGRPFORMNAMEb";
            this.txtGRPFORMNAMEb.Size = new System.Drawing.Size(349, 21);
            this.txtGRPFORMNAMEb.TabIndex = 85;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.RoyalBlue;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.ForeColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(7, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 21);
            this.label2.TabIndex = 84;
            this.label2.Text = "그룹명칭";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitleSub1
            // 
            this.panTitleSub1.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panTitleSub1.Controls.Add(this.label1);
            this.panTitleSub1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub1.Location = new System.Drawing.Point(2, 2);
            this.panTitleSub1.Name = "panTitleSub1";
            this.panTitleSub1.Size = new System.Drawing.Size(659, 27);
            this.panTitleSub1.TabIndex = 75;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 17);
            this.label1.TabIndex = 21;
            this.label1.Text = "대분류";
            // 
            // frmSample_PopUpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(964, 638);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle0);
            this.Name = "frmSample_PopUpForm";
            this.Text = "샘플폼-팦업";
            this.Load += new System.EventHandler(this.frmSample_PopUpForm_Load);
            this.panTitle0.ResumeLayout(false);
            this.panTitle0.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssSpread)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssSpread_Sheet1)).EndInit();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.panTitleSub2.ResumeLayout(false);
            this.panTitleSub2.PerformLayout();
            this.panTitle2.ResumeLayout(false);
            this.panTitle2.PerformLayout();
            this.panTitleSub1.ResumeLayout(false);
            this.panTitleSub1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle0;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private FarPoint.Win.Spread.FpSpread ssSpread;
        private FarPoint.Win.Spread.SheetView ssSpread_Sheet1;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panel2;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
        private System.Windows.Forms.Panel panTitleSub2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblTitleSub1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Panel panTitle2;
        private System.Windows.Forms.CheckBox chkDelYNb;
        internal System.Windows.Forms.TextBox txtGRPFORMNOb;
        internal System.Windows.Forms.Label lblItem1;
        internal System.Windows.Forms.Label lblItem0;
        internal System.Windows.Forms.TextBox txtDISPSEQb;
        internal System.Windows.Forms.TextBox txtGRPFORMNAMEb;
        internal System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panTitleSub1;
        private System.Windows.Forms.Label label1;
    }
}