namespace ComLibB
{
    partial class frmStatistics
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
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSuch = new System.Windows.Forms.Button();
            this.lblTitle0 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitlesub0 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pan1 = new System.Windows.Forms.Panel();
            this.lblItem1 = new System.Windows.Forms.Label();
            this.combo0 = new System.Windows.Forms.ComboBox();
            this.lblItem0 = new System.Windows.Forms.Label();
            this.panTitle.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pan1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnPrint);
            this.panTitle.Controls.Add(this.btnSuch);
            this.panTitle.Controls.Add(this.lblTitle0);
            this.panTitle.Controls.Add(this.btnClose);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(624, 34);
            this.panTitle.TabIndex = 14;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.Location = new System.Drawing.Point(467, 1);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 30);
            this.btnPrint.TabIndex = 23;
            this.btnPrint.Text = "출력";
            this.btnPrint.UseVisualStyleBackColor = false;
            // 
            // btnSuch
            // 
            this.btnSuch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSuch.BackColor = System.Drawing.Color.Transparent;
            this.btnSuch.Location = new System.Drawing.Point(389, 1);
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
            this.lblTitle0.Size = new System.Drawing.Size(42, 16);
            this.lblTitle0.TabIndex = 4;
            this.lblTitle0.Text = "차트";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Location = new System.Drawing.Point(545, 1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(72, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "닫기";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitlesub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(624, 28);
            this.panTitleSub0.TabIndex = 15;
            // 
            // lblTitlesub0
            // 
            this.lblTitlesub0.AutoSize = true;
            this.lblTitlesub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitlesub0.ForeColor = System.Drawing.Color.White;
            this.lblTitlesub0.Location = new System.Drawing.Point(8, 6);
            this.lblTitlesub0.Name = "lblTitlesub0";
            this.lblTitlesub0.Size = new System.Drawing.Size(82, 12);
            this.lblTitlesub0.TabIndex = 0;
            this.lblTitlesub0.Text = "서브 타이틀0";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pan1);
            this.panel1.Controls.Add(this.combo0);
            this.panel1.Controls.Add(this.lblItem0);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(624, 52);
            this.panel1.TabIndex = 16;
            // 
            // pan1
            // 
            this.pan1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pan1.Controls.Add(this.lblItem1);
            this.pan1.Location = new System.Drawing.Point(217, 17);
            this.pan1.Name = "pan1";
            this.pan1.Size = new System.Drawing.Size(200, 20);
            this.pan1.TabIndex = 27;
            // 
            // lblItem1
            // 
            this.lblItem1.AutoSize = true;
            this.lblItem1.Location = new System.Drawing.Point(78, 4);
            this.lblItem1.Name = "lblItem1";
            this.lblItem1.Size = new System.Drawing.Size(41, 12);
            this.lblItem1.TabIndex = 26;
            this.lblItem1.Text = "지표값";
            // 
            // combo0
            // 
            this.combo0.FormattingEnabled = true;
            this.combo0.Location = new System.Drawing.Point(79, 17);
            this.combo0.Name = "combo0";
            this.combo0.Size = new System.Drawing.Size(121, 20);
            this.combo0.TabIndex = 26;
            this.combo0.Text = "ComboChartJong";
            // 
            // lblItem0
            // 
            this.lblItem0.AutoSize = true;
            this.lblItem0.Location = new System.Drawing.Point(25, 20);
            this.lblItem0.Name = "lblItem0";
            this.lblItem0.Size = new System.Drawing.Size(53, 12);
            this.lblItem0.TabIndex = 25;
            this.lblItem0.Text = "차트종류";
            // 
            // frmStatistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(624, 718);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Name = "frmStatistics";
            this.Text = "frmStatistics";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pan1.ResumeLayout(false);
            this.pan1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnSuch;
        private System.Windows.Forms.Label lblTitle0;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitlesub0;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pan1;
        private System.Windows.Forms.Label lblItem1;
        private System.Windows.Forms.ComboBox combo0;
        private System.Windows.Forms.Label lblItem0;
    }
}