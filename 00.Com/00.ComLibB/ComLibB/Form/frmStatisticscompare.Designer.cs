namespace ComLibB
{
    partial class frmStatisticscompare
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
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitlesun0 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnSuch = new System.Windows.Forms.Button();
            this.lblTitle0 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ss0 = new FarPoint.Win.Spread.FpSpread();
            this.ss0_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.combo0 = new System.Windows.Forms.ComboBox();
            this.lblitem1 = new System.Windows.Forms.Label();
            this.txtyyyy1 = new System.Windows.Forms.TextBox();
            this.txtyyyy0 = new System.Windows.Forms.TextBox();
            this.lblitem0 = new System.Windows.Forms.Label();
            this.panTitleSub0.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss0_Sheet1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitlesun0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(785, 28);
            this.panTitleSub0.TabIndex = 17;
            // 
            // lblTitlesun0
            // 
            this.lblTitlesun0.AutoSize = true;
            this.lblTitlesun0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitlesun0.ForeColor = System.Drawing.Color.White;
            this.lblTitlesun0.Location = new System.Drawing.Point(8, 6);
            this.lblTitlesun0.Name = "lblTitlesun0";
            this.lblTitlesun0.Size = new System.Drawing.Size(31, 12);
            this.lblTitlesun0.TabIndex = 0;
            this.lblTitlesun0.Text = "통계";
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnSuch);
            this.panTitle.Controls.Add(this.lblTitle0);
            this.panTitle.Controls.Add(this.btnClose);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(785, 34);
            this.panTitle.TabIndex = 16;
            // 
            // btnSuch
            // 
            this.btnSuch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSuch.BackColor = System.Drawing.Color.Transparent;
            this.btnSuch.Location = new System.Drawing.Point(628, 1);
            this.btnSuch.Name = "btnSuch";
            this.btnSuch.Size = new System.Drawing.Size(72, 30);
            this.btnSuch.TabIndex = 28;
            this.btnSuch.Text = "조회";
            this.btnSuch.UseVisualStyleBackColor = false;
            // 
            // lblTitle0
            // 
            this.lblTitle0.AutoSize = true;
            this.lblTitle0.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle0.Location = new System.Drawing.Point(5, 8);
            this.lblTitle0.Name = "lblTitle0";
            this.lblTitle0.Size = new System.Drawing.Size(110, 16);
            this.lblTitle0.TabIndex = 4;
            this.lblTitle0.Text = "전원통계비교";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Location = new System.Drawing.Point(706, 1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(72, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "닫기";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(785, 529);
            this.panel1.TabIndex = 18;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.ss0);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 28);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(785, 501);
            this.panel3.TabIndex = 1;
            // 
            // ss0
            // 
            this.ss0.AccessibleDescription = "";
            this.ss0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ss0.Location = new System.Drawing.Point(0, 0);
            this.ss0.Name = "ss0";
            this.ss0.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ss0_Sheet1});
            this.ss0.Size = new System.Drawing.Size(785, 501);
            this.ss0.TabIndex = 0;
            // 
            // ss0_Sheet1
            // 
            this.ss0_Sheet1.Reset();
            this.ss0_Sheet1.SheetName = "Sheet1";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.combo0);
            this.panel2.Controls.Add(this.lblitem1);
            this.panel2.Controls.Add(this.txtyyyy1);
            this.panel2.Controls.Add(this.txtyyyy0);
            this.panel2.Controls.Add(this.lblitem0);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(785, 28);
            this.panel2.TabIndex = 0;
            // 
            // combo0
            // 
            this.combo0.FormattingEnabled = true;
            this.combo0.Location = new System.Drawing.Point(410, 4);
            this.combo0.Name = "combo0";
            this.combo0.Size = new System.Drawing.Size(272, 20);
            this.combo0.TabIndex = 4;
            // 
            // lblitem1
            // 
            this.lblitem1.AutoSize = true;
            this.lblitem1.Location = new System.Drawing.Point(350, 7);
            this.lblitem1.Name = "lblitem1";
            this.lblitem1.Size = new System.Drawing.Size(53, 12);
            this.lblitem1.TabIndex = 3;
            this.lblitem1.Text = "조회기간";
            // 
            // txtyyyy1
            // 
            this.txtyyyy1.Location = new System.Drawing.Point(184, 4);
            this.txtyyyy1.Name = "txtyyyy1";
            this.txtyyyy1.Size = new System.Drawing.Size(100, 21);
            this.txtyyyy1.TabIndex = 2;
            // 
            // txtyyyy0
            // 
            this.txtyyyy0.Location = new System.Drawing.Point(78, 4);
            this.txtyyyy0.Name = "txtyyyy0";
            this.txtyyyy0.Size = new System.Drawing.Size(100, 21);
            this.txtyyyy0.TabIndex = 1;
            // 
            // lblitem0
            // 
            this.lblitem0.AutoSize = true;
            this.lblitem0.Location = new System.Drawing.Point(8, 7);
            this.lblitem0.Name = "lblitem0";
            this.lblitem0.Size = new System.Drawing.Size(53, 12);
            this.lblitem0.TabIndex = 0;
            this.lblitem0.Text = "조회기간";
            // 
            // frmStatisticscompare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 591);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Name = "frmStatisticscompare";
            this.Text = "frmStatisticscompare";
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ss0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss0_Sheet1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitlesun0;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnSuch;
        private System.Windows.Forms.Label lblTitle0;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox combo0;
        private System.Windows.Forms.Label lblitem1;
        private System.Windows.Forms.TextBox txtyyyy1;
        private System.Windows.Forms.TextBox txtyyyy0;
        private System.Windows.Forms.Label lblitem0;
        private FarPoint.Win.Spread.FpSpread ss0;
        private FarPoint.Win.Spread.SheetView ss0_Sheet1;
    }
}