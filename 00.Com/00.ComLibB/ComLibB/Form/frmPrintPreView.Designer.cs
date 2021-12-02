namespace ComLibB
{
    partial class frmPrintPreView
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
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.txtFind = new System.Windows.Forms.Panel();
            this.txtCnt = new System.Windows.Forms.TextBox();
            this.lbl = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTPage = new System.Windows.Forms.TextBox();
            this.txtFPage = new System.Windows.Forms.TextBox();
            this.txtPage = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.txtRate = new System.Windows.Forms.TextBox();
            this.btnSetting = new System.Windows.Forms.Button();
            this.btnBig = new System.Windows.Forms.Button();
            this.btnPrePage = new System.Windows.Forms.Button();
            this.btnSmall = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ptbSpreadPreview = new System.Windows.Forms.PictureBox();
            this.panTitleSub0.SuspendLayout();
            this.txtFind.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptbSpreadPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(1064, 28);
            this.panTitleSub0.TabIndex = 86;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(12, 9);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(93, 12);
            this.lblTitleSub0.TabIndex = 22;
            this.lblTitleSub0.Text = "미리보기 & 인쇄";
            this.lblTitleSub0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtFind
            // 
            this.txtFind.BackColor = System.Drawing.Color.White;
            this.txtFind.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtFind.Controls.Add(this.txtCnt);
            this.txtFind.Controls.Add(this.lbl);
            this.txtFind.Controls.Add(this.label4);
            this.txtFind.Controls.Add(this.txtTPage);
            this.txtFind.Controls.Add(this.txtFPage);
            this.txtFind.Controls.Add(this.txtPage);
            this.txtFind.Controls.Add(this.label3);
            this.txtFind.Controls.Add(this.label2);
            this.txtFind.Controls.Add(this.label1);
            this.txtFind.Controls.Add(this.btnNextPage);
            this.txtFind.Controls.Add(this.txtRate);
            this.txtFind.Controls.Add(this.btnSetting);
            this.txtFind.Controls.Add(this.btnBig);
            this.txtFind.Controls.Add(this.btnPrePage);
            this.txtFind.Controls.Add(this.btnSmall);
            this.txtFind.Controls.Add(this.btnPrint);
            this.txtFind.Controls.Add(this.btnExit);
            this.txtFind.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtFind.ForeColor = System.Drawing.Color.White;
            this.txtFind.Location = new System.Drawing.Point(0, 0);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new System.Drawing.Size(1064, 34);
            this.txtFind.TabIndex = 85;
            // 
            // txtCnt
            // 
            this.txtCnt.Location = new System.Drawing.Point(857, 5);
            this.txtCnt.Name = "txtCnt";
            this.txtCnt.Size = new System.Drawing.Size(39, 21);
            this.txtCnt.TabIndex = 95;
            this.txtCnt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCnt_KeyPress);
            this.txtCnt.Leave += new System.EventHandler(this.txtCnt_Leave);
            // 
            // lbl
            // 
            this.lbl.AutoSize = true;
            this.lbl.ForeColor = System.Drawing.Color.Black;
            this.lbl.Location = new System.Drawing.Point(587, 9);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(61, 12);
            this.lbl.TabIndex = 94;
            this.lbl.Text = "인쇄범위 :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(790, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 12);
            this.label4.TabIndex = 97;
            this.label4.Text = "인쇄매수 :";
            // 
            // txtTPage
            // 
            this.txtTPage.Location = new System.Drawing.Point(729, 5);
            this.txtTPage.Name = "txtTPage";
            this.txtTPage.Size = new System.Drawing.Size(39, 21);
            this.txtTPage.TabIndex = 96;
            this.txtTPage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTPage_KeyPress);
            this.txtTPage.Leave += new System.EventHandler(this.txtTPage_Leave);
            // 
            // txtFPage
            // 
            this.txtFPage.Location = new System.Drawing.Point(664, 5);
            this.txtFPage.Name = "txtFPage";
            this.txtFPage.Size = new System.Drawing.Size(39, 21);
            this.txtFPage.TabIndex = 91;
            this.txtFPage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFPage_KeyPress);
            this.txtFPage.Leave += new System.EventHandler(this.txtFPage_Leave);
            // 
            // txtPage
            // 
            this.txtPage.Location = new System.Drawing.Point(489, 5);
            this.txtPage.Name = "txtPage";
            this.txtPage.Size = new System.Drawing.Size(39, 21);
            this.txtPage.TabIndex = 94;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(709, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 12);
            this.label3.TabIndex = 95;
            this.label3.Text = "~";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(425, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 12);
            this.label2.TabIndex = 93;
            this.label2.Text = "현재Page";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(349, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 12);
            this.label1.TabIndex = 91;
            this.label1.Text = "%";
            // 
            // btnNextPage
            // 
            this.btnNextPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNextPage.AutoSize = true;
            this.btnNextPage.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnNextPage.Location = new System.Drawing.Point(168, 1);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(72, 30);
            this.btnNextPage.TabIndex = 87;
            this.btnNextPage.Text = "다음Page";
            this.btnNextPage.UseVisualStyleBackColor = true;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // txtRate
            // 
            this.txtRate.Location = new System.Drawing.Point(307, 5);
            this.txtRate.Name = "txtRate";
            this.txtRate.Size = new System.Drawing.Size(36, 21);
            this.txtRate.TabIndex = 90;
            this.txtRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRate_KeyPress);
            // 
            // btnSetting
            // 
            this.btnSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetting.AutoSize = true;
            this.btnSetting.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSetting.Location = new System.Drawing.Point(3, 1);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(79, 30);
            this.btnSetting.TabIndex = 87;
            this.btnSetting.Text = "프린터설정";
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // btnBig
            // 
            this.btnBig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBig.AutoSize = true;
            this.btnBig.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnBig.Location = new System.Drawing.Point(370, 2);
            this.btnBig.Name = "btnBig";
            this.btnBig.Size = new System.Drawing.Size(43, 30);
            this.btnBig.TabIndex = 89;
            this.btnBig.Text = "확대";
            this.btnBig.UseVisualStyleBackColor = true;
            this.btnBig.Click += new System.EventHandler(this.btnBig_Click);
            // 
            // btnPrePage
            // 
            this.btnPrePage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrePage.AutoSize = true;
            this.btnPrePage.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPrePage.Location = new System.Drawing.Point(93, 1);
            this.btnPrePage.Name = "btnPrePage";
            this.btnPrePage.Size = new System.Drawing.Size(72, 30);
            this.btnPrePage.TabIndex = 86;
            this.btnPrePage.Text = "이전Page";
            this.btnPrePage.UseVisualStyleBackColor = true;
            this.btnPrePage.Click += new System.EventHandler(this.btnPrePage_Click);
            // 
            // btnSmall
            // 
            this.btnSmall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSmall.AutoSize = true;
            this.btnSmall.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSmall.Location = new System.Drawing.Point(258, 2);
            this.btnSmall.Name = "btnSmall";
            this.btnSmall.Size = new System.Drawing.Size(43, 30);
            this.btnSmall.TabIndex = 88;
            this.btnSmall.Text = "축소";
            this.btnSmall.UseVisualStyleBackColor = true;
            this.btnSmall.Click += new System.EventHandler(this.btnSmall_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.AutoSize = true;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPrint.Location = new System.Drawing.Point(907, 1);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 30);
            this.btnPrint.TabIndex = 85;
            this.btnPrint.Text = "인 쇄";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(985, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 83;
            this.btnExit.Text = "닫 기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ptbSpreadPreview);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1064, 684);
            this.panel1.TabIndex = 87;
            // 
            // ptbSpreadPreview
            // 
            this.ptbSpreadPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ptbSpreadPreview.Location = new System.Drawing.Point(0, 0);
            this.ptbSpreadPreview.Name = "ptbSpreadPreview";
            this.ptbSpreadPreview.Size = new System.Drawing.Size(1064, 684);
            this.ptbSpreadPreview.TabIndex = 0;
            this.ptbSpreadPreview.TabStop = false;
            // 
            // frmPrintPreView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1064, 746);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.txtFind);
            this.Name = "frmPrintPreView";
            this.Text = "미리보기 & 인쇄";
            this.Load += new System.EventHandler(this.frmPrintPreView_Load);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.txtFind.ResumeLayout(false);
            this.txtFind.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ptbSpreadPreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel txtFind;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.TextBox txtRate;
        private System.Windows.Forms.Button btnSetting;
        private System.Windows.Forms.Button btnBig;
        private System.Windows.Forms.Button btnPrePage;
        private System.Windows.Forms.Button btnSmall;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox txtCnt;
        private System.Windows.Forms.Label lbl;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTPage;
        private System.Windows.Forms.TextBox txtFPage;
        private System.Windows.Forms.TextBox txtPage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox ptbSpreadPreview;
    }
}