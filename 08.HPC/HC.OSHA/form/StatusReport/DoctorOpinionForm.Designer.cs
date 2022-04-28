namespace HC_OSHA.form.StatusReport
{
    partial class DoctorOpinionForm
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
            this.ChkOverWrite = new System.Windows.Forms.CheckBox();
            this.BtnSave = new System.Windows.Forms.Button();
            this.ssMacroList = new FarPoint.Win.Spread.FpSpread();
            this.ssMacroList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.TxtOPINION = new System.Windows.Forms.TextBox();
            this.BtnMacro = new System.Windows.Forms.Button();
            this.label90 = new System.Windows.Forms.Label();
            this.BtnCard19 = new System.Windows.Forms.Button();
            this.BtnClear = new System.Windows.Forms.Button();
            this.TxtMemo1 = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.btnMemoRead = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMacroList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMacroList_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // ChkOverWrite
            // 
            this.ChkOverWrite.AutoSize = true;
            this.ChkOverWrite.Checked = true;
            this.ChkOverWrite.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkOverWrite.Location = new System.Drawing.Point(833, 14);
            this.ChkOverWrite.Name = "ChkOverWrite";
            this.ChkOverWrite.Size = new System.Drawing.Size(105, 21);
            this.ChkOverWrite.TabIndex = 154;
            this.ChkOverWrite.Text = "커서위치복사";
            this.ChkOverWrite.UseVisualStyleBackColor = true;
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(736, 10);
            this.BtnSave.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 28);
            this.BtnSave.TabIndex = 153;
            this.BtnSave.Text = "저장(&S)";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // ssMacroList
            // 
            this.ssMacroList.AccessibleDescription = "";
            this.ssMacroList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ssMacroList.Location = new System.Drawing.Point(830, 47);
            this.ssMacroList.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.ssMacroList.Name = "ssMacroList";
            this.ssMacroList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssMacroList_Sheet1});
            this.ssMacroList.Size = new System.Drawing.Size(384, 436);
            this.ssMacroList.TabIndex = 151;
            this.ssMacroList.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssMacroList_CellDoubleClick);
            // 
            // ssMacroList_Sheet1
            // 
            this.ssMacroList_Sheet1.Reset();
            this.ssMacroList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssMacroList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssMacroList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMacroList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMacroList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssMacroList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMacroList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMacroList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMacroList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssMacroList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMacroList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // TxtOPINION
            // 
            this.TxtOPINION.Location = new System.Drawing.Point(12, 47);
            this.TxtOPINION.Multiline = true;
            this.TxtOPINION.Name = "TxtOPINION";
            this.TxtOPINION.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TxtOPINION.Size = new System.Drawing.Size(799, 636);
            this.TxtOPINION.TabIndex = 149;
            // 
            // BtnMacro
            // 
            this.BtnMacro.Location = new System.Drawing.Point(944, 10);
            this.BtnMacro.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnMacro.Name = "BtnMacro";
            this.BtnMacro.Size = new System.Drawing.Size(104, 27);
            this.BtnMacro.TabIndex = 150;
            this.BtnMacro.Text = "상용구 관리";
            this.BtnMacro.UseVisualStyleBackColor = true;
            this.BtnMacro.Click += new System.EventHandler(this.BtnMacro_Click);
            // 
            // label90
            // 
            this.label90.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label90.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label90.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label90.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label90.Location = new System.Drawing.Point(12, 10);
            this.label90.Name = "label90";
            this.label90.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label90.Size = new System.Drawing.Size(271, 27);
            this.label90.TabIndex = 148;
            this.label90.Text = "3. 종합의견(세부업무내용, 개선의견)";
            this.label90.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BtnCard19
            // 
            this.BtnCard19.Location = new System.Drawing.Point(1054, 10);
            this.BtnCard19.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnCard19.Name = "BtnCard19";
            this.BtnCard19.Size = new System.Drawing.Size(160, 27);
            this.BtnCard19.TabIndex = 155;
            this.BtnCard19.Text = "위탁업무수행일지 관리";
            this.BtnCard19.UseVisualStyleBackColor = true;
            this.BtnCard19.Click += new System.EventHandler(this.BtnCard19_Click);
            // 
            // BtnClear
            // 
            this.BtnClear.Location = new System.Drawing.Point(289, 10);
            this.BtnClear.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(75, 28);
            this.BtnClear.TabIndex = 156;
            this.BtnClear.Text = "지우기";
            this.BtnClear.UseVisualStyleBackColor = true;
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // TxtMemo1
            // 
            this.TxtMemo1.Location = new System.Drawing.Point(830, 518);
            this.TxtMemo1.Multiline = true;
            this.TxtMemo1.Name = "TxtMemo1";
            this.TxtMemo1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TxtMemo1.Size = new System.Drawing.Size(384, 162);
            this.TxtMemo1.TabIndex = 184;
            this.TxtMemo1.TextChanged += new System.EventHandler(this.TxtMemo1_TextChanged);
            // 
            // label30
            // 
            this.label30.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label30.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label30.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label30.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label30.Location = new System.Drawing.Point(830, 490);
            this.label30.Name = "label30";
            this.label30.Padding = new System.Windows.Forms.Padding(3);
            this.label30.Size = new System.Drawing.Size(91, 25);
            this.label30.TabIndex = 183;
            this.label30.Text = "참고사항";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnMemoRead
            // 
            this.btnMemoRead.Location = new System.Drawing.Point(927, 490);
            this.btnMemoRead.Name = "btnMemoRead";
            this.btnMemoRead.Size = new System.Drawing.Size(86, 25);
            this.btnMemoRead.TabIndex = 341;
            this.btnMemoRead.Text = "다시읽기";
            this.btnMemoRead.UseVisualStyleBackColor = true;
            this.btnMemoRead.Click += new System.EventHandler(this.btnMemoRead_Click);
            // 
            // DoctorOpinionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1223, 692);
            this.Controls.Add(this.btnMemoRead);
            this.Controls.Add(this.TxtMemo1);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.BtnClear);
            this.Controls.Add(this.BtnCard19);
            this.Controls.Add(this.ChkOverWrite);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.ssMacroList);
            this.Controls.Add(this.TxtOPINION);
            this.Controls.Add(this.BtnMacro);
            this.Controls.Add(this.label90);
            this.Name = "DoctorOpinionForm";
            this.Text = "DoctorOpinionForm";
            this.Load += new System.EventHandler(this.DoctorOpinionForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMacroList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMacroList_Sheet1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox ChkOverWrite;
        private System.Windows.Forms.Button BtnSave;
        private FarPoint.Win.Spread.FpSpread ssMacroList;
        private FarPoint.Win.Spread.SheetView ssMacroList_Sheet1;
        private System.Windows.Forms.TextBox TxtOPINION;
        private System.Windows.Forms.Button BtnMacro;
        private System.Windows.Forms.Label label90;
        private System.Windows.Forms.Button BtnCard19;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.TextBox TxtMemo1;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Button btnMemoRead;
    }
}