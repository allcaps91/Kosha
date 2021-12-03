namespace ComEmrBase
{
    partial class frmEmrLogin
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
            this.panUserChk = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtConfirmPw2 = new System.Windows.Forms.TextBox();
            this.txtConfirmID2 = new System.Windows.Forms.TextBox();
            this.btnExitUserChk = new System.Windows.Forms.Button();
            this.btnAppUserChk = new System.Windows.Forms.Button();
            this.txtBloodDate = new System.Windows.Forms.TextBox();
            this.txtBloodNo = new System.Windows.Forms.TextBox();
            this.txtBloodGBN = new System.Windows.Forms.TextBox();
            this.txtBlood = new System.Windows.Forms.TextBox();
            this.txtPano = new System.Windows.Forms.TextBox();
            this.chk5 = new System.Windows.Forms.CheckBox();
            this.chk4 = new System.Windows.Forms.CheckBox();
            this.chk3 = new System.Windows.Forms.CheckBox();
            this.chk2 = new System.Windows.Forms.CheckBox();
            this.chk1 = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panPLTC = new System.Windows.Forms.Panel();
            this.ssBlood = new FarPoint.Win.Spread.FpSpread();
            this.ssBlood_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panUserChk.SuspendLayout();
            this.panPLTC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssBlood)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssBlood_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panUserChk
            // 
            this.panUserChk.Controls.Add(this.label8);
            this.panUserChk.Controls.Add(this.label7);
            this.panUserChk.Controls.Add(this.txtConfirmPw2);
            this.panUserChk.Controls.Add(this.txtConfirmID2);
            this.panUserChk.Controls.Add(this.btnExitUserChk);
            this.panUserChk.Controls.Add(this.btnAppUserChk);
            this.panUserChk.Controls.Add(this.txtBloodDate);
            this.panUserChk.Controls.Add(this.txtBloodNo);
            this.panUserChk.Controls.Add(this.txtBloodGBN);
            this.panUserChk.Controls.Add(this.txtBlood);
            this.panUserChk.Controls.Add(this.txtPano);
            this.panUserChk.Controls.Add(this.chk5);
            this.panUserChk.Controls.Add(this.chk4);
            this.panUserChk.Controls.Add(this.chk3);
            this.panUserChk.Controls.Add(this.chk2);
            this.panUserChk.Controls.Add(this.chk1);
            this.panUserChk.Controls.Add(this.label3);
            this.panUserChk.Dock = System.Windows.Forms.DockStyle.Left;
            this.panUserChk.Location = new System.Drawing.Point(0, 0);
            this.panUserChk.Name = "panUserChk";
            this.panUserChk.Size = new System.Drawing.Size(262, 284);
            this.panUserChk.TabIndex = 114;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("나눔고딕", 9F);
            this.label8.Location = new System.Drawing.Point(95, 240);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 14);
            this.label8.TabIndex = 115;
            this.label8.Text = "비번";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("나눔고딕", 9F);
            this.label7.Location = new System.Drawing.Point(22, 240);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 14);
            this.label7.TabIndex = 115;
            this.label7.Text = "아이디";
            // 
            // txtConfirmPw2
            // 
            this.txtConfirmPw2.Font = new System.Drawing.Font("나눔고딕", 9F);
            this.txtConfirmPw2.Location = new System.Drawing.Point(79, 257);
            this.txtConfirmPw2.Name = "txtConfirmPw2";
            this.txtConfirmPw2.PasswordChar = '*';
            this.txtConfirmPw2.Size = new System.Drawing.Size(60, 21);
            this.txtConfirmPw2.TabIndex = 1;
            this.txtConfirmPw2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtConfirmPw2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtConfirmPw2_KeyDown);
            // 
            // txtConfirmID2
            // 
            this.txtConfirmID2.Font = new System.Drawing.Font("나눔고딕", 9F);
            this.txtConfirmID2.Location = new System.Drawing.Point(13, 257);
            this.txtConfirmID2.Name = "txtConfirmID2";
            this.txtConfirmID2.Size = new System.Drawing.Size(60, 21);
            this.txtConfirmID2.TabIndex = 0;
            this.txtConfirmID2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtConfirmID2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtConfirmID2_KeyDown);
            // 
            // btnExitUserChk
            // 
            this.btnExitUserChk.Font = new System.Drawing.Font("나눔고딕", 9F);
            this.btnExitUserChk.Location = new System.Drawing.Point(137, 198);
            this.btnExitUserChk.Name = "btnExitUserChk";
            this.btnExitUserChk.Size = new System.Drawing.Size(120, 30);
            this.btnExitUserChk.TabIndex = 5;
            this.btnExitUserChk.TabStop = false;
            this.btnExitUserChk.Text = "닫기";
            this.btnExitUserChk.UseVisualStyleBackColor = true;
            this.btnExitUserChk.Click += new System.EventHandler(this.btnExitUserChk_Click);
            // 
            // btnAppUserChk
            // 
            this.btnAppUserChk.Enabled = false;
            this.btnAppUserChk.Font = new System.Drawing.Font("나눔고딕", 9F);
            this.btnAppUserChk.Location = new System.Drawing.Point(11, 198);
            this.btnAppUserChk.Name = "btnAppUserChk";
            this.btnAppUserChk.Size = new System.Drawing.Size(120, 30);
            this.btnAppUserChk.TabIndex = 4;
            this.btnAppUserChk.TabStop = false;
            this.btnAppUserChk.Text = "적용";
            this.btnAppUserChk.UseVisualStyleBackColor = true;
            this.btnAppUserChk.Click += new System.EventHandler(this.btnAppUserChk_Click);
            // 
            // txtBloodDate
            // 
            this.txtBloodDate.Location = new System.Drawing.Point(90, 158);
            this.txtBloodDate.Name = "txtBloodDate";
            this.txtBloodDate.Size = new System.Drawing.Size(109, 21);
            this.txtBloodDate.TabIndex = 2;
            this.txtBloodDate.TabStop = false;
            // 
            // txtBloodNo
            // 
            this.txtBloodNo.Location = new System.Drawing.Point(90, 125);
            this.txtBloodNo.Name = "txtBloodNo";
            this.txtBloodNo.Size = new System.Drawing.Size(109, 21);
            this.txtBloodNo.TabIndex = 2;
            this.txtBloodNo.TabStop = false;
            // 
            // txtBloodGBN
            // 
            this.txtBloodGBN.Location = new System.Drawing.Point(90, 92);
            this.txtBloodGBN.Name = "txtBloodGBN";
            this.txtBloodGBN.Size = new System.Drawing.Size(109, 21);
            this.txtBloodGBN.TabIndex = 2;
            this.txtBloodGBN.TabStop = false;
            // 
            // txtBlood
            // 
            this.txtBlood.Location = new System.Drawing.Point(90, 59);
            this.txtBlood.Name = "txtBlood";
            this.txtBlood.Size = new System.Drawing.Size(109, 21);
            this.txtBlood.TabIndex = 2;
            this.txtBlood.TabStop = false;
            // 
            // txtPano
            // 
            this.txtPano.Location = new System.Drawing.Point(90, 26);
            this.txtPano.Name = "txtPano";
            this.txtPano.Size = new System.Drawing.Size(109, 21);
            this.txtPano.TabIndex = 2;
            this.txtPano.TabStop = false;
            // 
            // chk5
            // 
            this.chk5.AutoSize = true;
            this.chk5.Location = new System.Drawing.Point(11, 160);
            this.chk5.Name = "chk5";
            this.chk5.Size = new System.Drawing.Size(72, 16);
            this.chk5.TabIndex = 1;
            this.chk5.TabStop = false;
            this.chk5.Text = "유효기간";
            this.chk5.UseVisualStyleBackColor = true;
            // 
            // chk4
            // 
            this.chk4.AutoSize = true;
            this.chk4.Location = new System.Drawing.Point(11, 127);
            this.chk4.Name = "chk4";
            this.chk4.Size = new System.Drawing.Size(72, 16);
            this.chk4.TabIndex = 1;
            this.chk4.TabStop = false;
            this.chk4.Text = "혈액번호";
            this.chk4.UseVisualStyleBackColor = true;
            // 
            // chk3
            // 
            this.chk3.AutoSize = true;
            this.chk3.Location = new System.Drawing.Point(11, 94);
            this.chk3.Name = "chk3";
            this.chk3.Size = new System.Drawing.Size(72, 16);
            this.chk3.TabIndex = 1;
            this.chk3.TabStop = false;
            this.chk3.Text = "혈액종류";
            this.chk3.UseVisualStyleBackColor = true;
            // 
            // chk2
            // 
            this.chk2.AutoSize = true;
            this.chk2.Location = new System.Drawing.Point(11, 61);
            this.chk2.Name = "chk2";
            this.chk2.Size = new System.Drawing.Size(60, 16);
            this.chk2.TabIndex = 1;
            this.chk2.TabStop = false;
            this.chk2.Text = "혈액형";
            this.chk2.UseVisualStyleBackColor = true;
            // 
            // chk1
            // 
            this.chk1.AutoSize = true;
            this.chk1.Location = new System.Drawing.Point(11, 28);
            this.chk1.Name = "chk1";
            this.chk1.Size = new System.Drawing.Size(72, 16);
            this.chk1.TabIndex = 1;
            this.chk1.TabStop = false;
            this.chk1.Text = "환자번호";
            this.chk1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("나눔고딕", 9F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(8, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 14);
            this.label3.TabIndex = 0;
            this.label3.Text = "모두 확인 해야 적용 가능";
            // 
            // panPLTC
            // 
            this.panPLTC.Controls.Add(this.ssBlood);
            this.panPLTC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panPLTC.Location = new System.Drawing.Point(262, 0);
            this.panPLTC.Name = "panPLTC";
            this.panPLTC.Size = new System.Drawing.Size(2, 284);
            this.panPLTC.TabIndex = 115;
            this.panPLTC.Visible = false;
            // 
            // ssBlood
            // 
            this.ssBlood.AccessibleDescription = "ssBlood, Sheet1, Row 0, Column 0, ";
            this.ssBlood.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssBlood.Location = new System.Drawing.Point(0, 0);
            this.ssBlood.Name = "ssBlood";
            this.ssBlood.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssBlood_Sheet1});
            this.ssBlood.Size = new System.Drawing.Size(2, 284);
            this.ssBlood.TabIndex = 6;
            // 
            // ssBlood_Sheet1
            // 
            this.ssBlood_Sheet1.Reset();
            this.ssBlood_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssBlood_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssBlood_Sheet1.ColumnCount = 2;
            this.ssBlood_Sheet1.RowCount = 1;
            this.ssBlood_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssBlood_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssBlood_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssBlood_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssBlood_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "혈액번호";
            this.ssBlood_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "유효기간";
            this.ssBlood_Sheet1.ColumnHeader.Rows.Get(0).Height = 27F;
            textCellType1.MaxLength = 2550;
            textCellType1.Multiline = true;
            textCellType1.WordWrap = true;
            this.ssBlood_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssBlood_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssBlood_Sheet1.Columns.Get(0).Label = "혈액번호";
            this.ssBlood_Sheet1.Columns.Get(0).Locked = true;
            this.ssBlood_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssBlood_Sheet1.Columns.Get(0).Width = 80F;
            this.ssBlood_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssBlood_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssBlood_Sheet1.Columns.Get(1).Label = "유효기간";
            this.ssBlood_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssBlood_Sheet1.Columns.Get(1).Width = 80F;
            this.ssBlood_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssBlood_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssBlood_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssBlood_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssBlood_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssBlood_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmEmrLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 284);
            this.Controls.Add(this.panPLTC);
            this.Controls.Add(this.panUserChk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmEmrLogin";
            this.Text = "수혈 기록지 이중확인";
            this.Load += new System.EventHandler(this.frmEmrLogin_Load);
            this.panUserChk.ResumeLayout(false);
            this.panUserChk.PerformLayout();
            this.panPLTC.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssBlood)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssBlood_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panUserChk;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtConfirmPw2;
        private System.Windows.Forms.TextBox txtConfirmID2;
        private System.Windows.Forms.Button btnExitUserChk;
        private System.Windows.Forms.Button btnAppUserChk;
        private System.Windows.Forms.TextBox txtBloodDate;
        private System.Windows.Forms.TextBox txtBloodNo;
        private System.Windows.Forms.TextBox txtBloodGBN;
        private System.Windows.Forms.TextBox txtBlood;
        private System.Windows.Forms.TextBox txtPano;
        private System.Windows.Forms.CheckBox chk5;
        private System.Windows.Forms.CheckBox chk4;
        private System.Windows.Forms.CheckBox chk3;
        private System.Windows.Forms.CheckBox chk2;
        private System.Windows.Forms.CheckBox chk1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panPLTC;
        private FarPoint.Win.Spread.FpSpread ssBlood;
        private FarPoint.Win.Spread.SheetView ssBlood_Sheet1;
    }
}