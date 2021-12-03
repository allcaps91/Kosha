namespace ComPmpaLibB
{
    partial class frmPmpaReMirBuildSTS
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose ();
            }
            base.Dispose (disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            this.panmain = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblyyyy = new System.Windows.Forms.Panel();
            this.panSub = new System.Windows.Forms.Panel();
            this.cboyyyy = new System.Windows.Forms.ComboBox();
            this.panSSmain = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.panmain.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.lblyyyy.SuspendLayout();
            this.panSub.SuspendLayout();
            this.panSSmain.SuspendLayout();
            this.SuspendLayout();
            // 
            // panmain
            // 
            this.panmain.BackColor = System.Drawing.Color.RoyalBlue;
            this.panmain.Controls.Add(this.lblTitleSub0);
            this.panmain.Dock = System.Windows.Forms.DockStyle.Top;
            this.panmain.Location = new System.Drawing.Point(0, 34);
            this.panmain.Name = "panmain";
            this.panmain.Size = new System.Drawing.Size(302, 28);
            this.panmain.TabIndex = 191;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(4, 6);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(73, 17);
            this.lblTitleSub0.TabIndex = 22;
            this.lblTitleSub0.Text = "EDI 청구월";
            this.lblTitleSub0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.label15);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(302, 34);
            this.panTitle.TabIndex = 190;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.ForeColor = System.Drawing.Color.Black;
            this.label15.Location = new System.Drawing.Point(3, 5);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(156, 21);
            this.label15.TabIndex = 83;
            this.label15.Text = "이의신청 자료 Build";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnExit
            // 
            this.btnExit.AutoSize = true;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(226, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btncansel_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.AutoSize = true;
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Location = new System.Drawing.Point(230, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 32);
            this.btnSearch.TabIndex = 77;
            this.btnSearch.Text = "자료형성";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnView_Click);
            // 
            // lblyyyy
            // 
            this.lblyyyy.BackColor = System.Drawing.SystemColors.Window;
            this.lblyyyy.Controls.Add(this.panSub);
            this.lblyyyy.Controls.Add(this.btnSearch);
            this.lblyyyy.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblyyyy.Location = new System.Drawing.Point(0, 62);
            this.lblyyyy.Name = "lblyyyy";
            this.lblyyyy.Size = new System.Drawing.Size(302, 32);
            this.lblyyyy.TabIndex = 192;
            // 
            // panSub
            // 
            this.panSub.Controls.Add(this.cboyyyy);
            this.panSub.Dock = System.Windows.Forms.DockStyle.Left;
            this.panSub.Location = new System.Drawing.Point(0, 0);
            this.panSub.Name = "panSub";
            this.panSub.Size = new System.Drawing.Size(191, 32);
            this.panSub.TabIndex = 90;
            // 
            // cboyyyy
            // 
            this.cboyyyy.FormattingEnabled = true;
            this.cboyyyy.Location = new System.Drawing.Point(9, 6);
            this.cboyyyy.Name = "cboyyyy";
            this.cboyyyy.Size = new System.Drawing.Size(121, 20);
            this.cboyyyy.TabIndex = 0;
            // 
            // panSSmain
            // 
            this.panSSmain.Controls.Add(this.progressBar1);
            this.panSSmain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panSSmain.Location = new System.Drawing.Point(0, 94);
            this.panSSmain.Name = "panSSmain";
            this.panSSmain.Size = new System.Drawing.Size(302, 25);
            this.panSSmain.TabIndex = 194;
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(302, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // frmPmpaReMirBuildSTS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 119);
            this.Controls.Add(this.panSSmain);
            this.Controls.Add(this.lblyyyy);
            this.Controls.Add(this.panmain);
            this.Controls.Add(this.panTitle);
            this.Name = "frmPmpaReMirBuildSTS";
            this.Text = "이의신청 자료 Build";
            this.Load += new System.EventHandler(this.frmPmpaReMirBuildSTS_Load);
            this.panmain.ResumeLayout(false);
            this.panmain.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.lblyyyy.ResumeLayout(false);
            this.lblyyyy.PerformLayout();
            this.panSub.ResumeLayout(false);
            this.panSSmain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panmain;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label label15;
        public System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel lblyyyy;
        private System.Windows.Forms.Panel panSub;
        private System.Windows.Forms.Panel panSSmain;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ComboBox cboyyyy;
    }
}