namespace HC_Core
{
    partial class MacrowordForm
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
            this.contentTitle1 = new ComBase.Mvc.UserControls.ContentTitle();
            this.panMacroword = new System.Windows.Forms.Panel();
            this.panRight = new System.Windows.Forms.Panel();
            this.TxtContent2 = new System.Windows.Forms.TextBox();
            this.numDispSeq = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtTitle = new System.Windows.Forms.TextBox();
            this.TxtContent = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ChkOverWrite = new System.Windows.Forms.CheckBox();
            this.BtnNew = new System.Windows.Forms.Button();
            this.BtnSave = new System.Windows.Forms.Button();
            this.BtnDelete = new System.Windows.Forms.Button();
            this.SSList = new FarPoint.Win.Spread.FpSpread();
            this.SSList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panMacroword.SuspendLayout();
            this.panRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDispSeq)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // contentTitle1
            // 
            this.contentTitle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.contentTitle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.contentTitle1.Location = new System.Drawing.Point(0, 0);
            this.contentTitle1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.contentTitle1.Name = "contentTitle1";
            this.contentTitle1.Size = new System.Drawing.Size(1126, 38);
            this.contentTitle1.TabIndex = 0;
            this.contentTitle1.TitleText = "상용구";
            // 
            // panMacroword
            // 
            this.panMacroword.BackColor = System.Drawing.SystemColors.Window;
            this.panMacroword.Controls.Add(this.panRight);
            this.panMacroword.Controls.Add(this.panel1);
            this.panMacroword.Controls.Add(this.SSList);
            this.panMacroword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMacroword.Location = new System.Drawing.Point(0, 38);
            this.panMacroword.Name = "panMacroword";
            this.panMacroword.Size = new System.Drawing.Size(1126, 678);
            this.panMacroword.TabIndex = 1;
            this.panMacroword.Paint += new System.Windows.Forms.PaintEventHandler(this.PanMacroword_Paint);
            // 
            // panRight
            // 
            this.panRight.Controls.Add(this.TxtContent2);
            this.panRight.Controls.Add(this.numDispSeq);
            this.panRight.Controls.Add(this.label3);
            this.panRight.Controls.Add(this.label2);
            this.panRight.Controls.Add(this.label1);
            this.panRight.Controls.Add(this.TxtTitle);
            this.panRight.Controls.Add(this.TxtContent);
            this.panRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panRight.Location = new System.Drawing.Point(266, 49);
            this.panRight.Name = "panRight";
            this.panRight.Size = new System.Drawing.Size(860, 629);
            this.panRight.TabIndex = 140;
            // 
            // TxtContent2
            // 
            this.TxtContent2.Location = new System.Drawing.Point(19, 385);
            this.TxtContent2.Multiline = true;
            this.TxtContent2.Name = "TxtContent2";
            this.TxtContent2.Size = new System.Drawing.Size(813, 233);
            this.TxtContent2.TabIndex = 141;
            // 
            // numDispSeq
            // 
            this.numDispSeq.Location = new System.Drawing.Point(357, 23);
            this.numDispSeq.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numDispSeq.Name = "numDispSeq";
            this.numDispSeq.Size = new System.Drawing.Size(72, 25);
            this.numDispSeq.TabIndex = 140;
            this.numDispSeq.Tag = "DISPSEQ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(354, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 17);
            this.label3.TabIndex = 139;
            this.label3.Text = "순서";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 17);
            this.label2.TabIndex = 138;
            this.label2.Text = "상용구 내용";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 17);
            this.label1.TabIndex = 137;
            this.label1.Text = "상용구 제목";
            // 
            // TxtTitle
            // 
            this.TxtTitle.Location = new System.Drawing.Point(19, 23);
            this.TxtTitle.Name = "TxtTitle";
            this.TxtTitle.Size = new System.Drawing.Size(330, 25);
            this.TxtTitle.TabIndex = 135;
            // 
            // TxtContent
            // 
            this.TxtContent.Location = new System.Drawing.Point(19, 80);
            this.TxtContent.Multiline = true;
            this.TxtContent.Name = "TxtContent";
            this.TxtContent.Size = new System.Drawing.Size(813, 299);
            this.TxtContent.TabIndex = 132;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ChkOverWrite);
            this.panel1.Controls.Add(this.BtnNew);
            this.panel1.Controls.Add(this.BtnSave);
            this.panel1.Controls.Add(this.BtnDelete);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(266, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(860, 49);
            this.panel1.TabIndex = 139;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel1_Paint);
            // 
            // ChkOverWrite
            // 
            this.ChkOverWrite.AutoSize = true;
            this.ChkOverWrite.Checked = true;
            this.ChkOverWrite.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkOverWrite.Location = new System.Drawing.Point(32, 15);
            this.ChkOverWrite.Name = "ChkOverWrite";
            this.ChkOverWrite.Size = new System.Drawing.Size(105, 21);
            this.ChkOverWrite.TabIndex = 138;
            this.ChkOverWrite.Text = "커서위치복사";
            this.ChkOverWrite.UseVisualStyleBackColor = true;
            // 
            // BtnNew
            // 
            this.BtnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnNew.Location = new System.Drawing.Point(692, 7);
            this.BtnNew.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnNew.Name = "BtnNew";
            this.BtnNew.Size = new System.Drawing.Size(75, 27);
            this.BtnNew.TabIndex = 137;
            this.BtnNew.Text = "화면정리";
            this.BtnNew.UseVisualStyleBackColor = true;
            this.BtnNew.Click += new System.EventHandler(this.BtnNew_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSave.Location = new System.Drawing.Point(773, 7);
            this.BtnSave.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 27);
            this.BtnSave.TabIndex = 133;
            this.BtnSave.Text = "저장";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnDelete
            // 
            this.BtnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnDelete.Location = new System.Drawing.Point(611, 7);
            this.BtnDelete.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(75, 27);
            this.BtnDelete.TabIndex = 136;
            this.BtnDelete.Text = "삭제";
            this.BtnDelete.UseVisualStyleBackColor = true;
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // SSList
            // 
            this.SSList.AccessibleDescription = "";
            this.SSList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSList.Dock = System.Windows.Forms.DockStyle.Left;
            this.SSList.Location = new System.Drawing.Point(0, 0);
            this.SSList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SSList.Name = "SSList";
            this.SSList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSList_Sheet1});
            this.SSList.Size = new System.Drawing.Size(266, 678);
            this.SSList.TabIndex = 134;
            this.SSList.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSList_CellClick);
            this.SSList.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSList_CellDoubleClick);
            this.SSList.SetActiveViewport(0, -1, -1);
            // 
            // SSList_Sheet1
            // 
            this.SSList_Sheet1.Reset();
            this.SSList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSList_Sheet1.ColumnCount = 0;
            this.SSList_Sheet1.RowCount = 0;
            this.SSList_Sheet1.ActiveColumnIndex = -1;
            this.SSList_Sheet1.ActiveRowIndex = -1;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // MacrowordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1126, 716);
            this.Controls.Add(this.panMacroword);
            this.Controls.Add(this.contentTitle1);
            this.Name = "MacrowordForm";
            this.Text = "MacrowordForm";
            this.Load += new System.EventHandler(this.MacrowordForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panMacroword.ResumeLayout(false);
            this.panRight.ResumeLayout(false);
            this.panRight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDispSeq)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ComBase.Mvc.UserControls.ContentTitle contentTitle1;
        private System.Windows.Forms.Panel panMacroword;
        private System.Windows.Forms.Button BtnNew;
        private System.Windows.Forms.Button BtnDelete;
        private FarPoint.Win.Spread.FpSpread SSList;
        private FarPoint.Win.Spread.SheetView SSList_Sheet1;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.CheckBox ChkOverWrite;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panRight;
        private System.Windows.Forms.TextBox TxtTitle;
        private System.Windows.Forms.TextBox TxtContent;
        private System.Windows.Forms.NumericUpDown numDispSeq;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtContent2;
    }
}