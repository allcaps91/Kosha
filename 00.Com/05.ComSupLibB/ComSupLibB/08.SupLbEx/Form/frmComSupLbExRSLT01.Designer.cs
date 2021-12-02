namespace ComSupLibB.SupLbEx
{
    partial class frmComSupLbExRSLT01
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
            FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer enhancedRowHeaderRenderer5 = new FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer();
            FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer enhancedRowHeaderRenderer1 = new FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panMain = new System.Windows.Forms.Panel();
            this.txtFootNote = new System.Windows.Forms.TextBox();
            this.ss_EXAM_SPECODE = new FarPoint.Win.Spread.FpSpread();
            this.ss_EXAM_SPECODE_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss_EXAM_SPECODE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss_EXAM_SPECODE_Sheet1)).BeginInit();
            this.SuspendLayout();
            enhancedRowHeaderRenderer5.BackColor = System.Drawing.SystemColors.Control;
            enhancedRowHeaderRenderer5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            enhancedRowHeaderRenderer5.ForeColor = System.Drawing.SystemColors.ControlText;
            enhancedRowHeaderRenderer5.Name = "enhancedRowHeaderRenderer5";
            enhancedRowHeaderRenderer5.PictureZoomEffect = false;
            enhancedRowHeaderRenderer5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            enhancedRowHeaderRenderer5.TextRotationAngle = 0D;
            enhancedRowHeaderRenderer5.ZoomFactor = 1F;
            enhancedRowHeaderRenderer1.Name = "enhancedRowHeaderRenderer1";
            enhancedRowHeaderRenderer1.PictureZoomEffect = false;
            enhancedRowHeaderRenderer1.TextRotationAngle = 0D;
            enhancedRowHeaderRenderer1.ZoomFactor = 1F;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.Controls.Add(this.btnOk);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Padding = new System.Windows.Forms.Padding(1);
            this.panTitle.Size = new System.Drawing.Size(416, 34);
            this.panTitle.TabIndex = 14;
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Transparent;
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnOk.Location = new System.Drawing.Point(271, 1);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(72, 32);
            this.btnOk.TabIndex = 19;
            this.btnOk.Text = "확인";
            this.btnOk.UseVisualStyleBackColor = false;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(343, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 32);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(6, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(91, 16);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "Foot Note";
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(416, 28);
            this.panTitleSub0.TabIndex = 92;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 6);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(99, 12);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "Foot Note 내용";
            // 
            // panMain
            // 
            this.panMain.Controls.Add(this.ss_EXAM_SPECODE);
            this.panMain.Controls.Add(this.txtFootNote);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(0, 62);
            this.panMain.Name = "panMain";
            this.panMain.Size = new System.Drawing.Size(416, 439);
            this.panMain.TabIndex = 93;
            // 
            // txtFootNote
            // 
            this.txtFootNote.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtFootNote.Location = new System.Drawing.Point(0, 225);
            this.txtFootNote.Multiline = true;
            this.txtFootNote.Name = "txtFootNote";
            this.txtFootNote.Size = new System.Drawing.Size(416, 214);
            this.txtFootNote.TabIndex = 0;
            // 
            // ss_EXAM_SPECODE
            // 
            this.ss_EXAM_SPECODE.AccessibleDescription = "";
            this.ss_EXAM_SPECODE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ss_EXAM_SPECODE.Location = new System.Drawing.Point(0, 0);
            this.ss_EXAM_SPECODE.Name = "ss_EXAM_SPECODE";
            this.ss_EXAM_SPECODE.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ss_EXAM_SPECODE_Sheet1});
            this.ss_EXAM_SPECODE.Size = new System.Drawing.Size(416, 225);
            this.ss_EXAM_SPECODE.TabIndex = 1;
            // 
            // ss_EXAM_SPECODE_Sheet1
            // 
            this.ss_EXAM_SPECODE_Sheet1.Reset();
            this.ss_EXAM_SPECODE_Sheet1.SheetName = "Sheet1";
            // 
            // frmComSupLbExRSLT01
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(416, 501);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Name = "frmComSupLbExRSLT01";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmComSupInfcCODE01";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panMain.ResumeLayout(false);
            this.panMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss_EXAM_SPECODE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss_EXAM_SPECODE_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panMain;
        private System.Windows.Forms.TextBox txtFootNote;
        private FarPoint.Win.Spread.FpSpread ss_EXAM_SPECODE;
        private FarPoint.Win.Spread.SheetView ss_EXAM_SPECODE_Sheet1;
    }
}