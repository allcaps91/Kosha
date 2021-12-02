namespace ComEmrBase
{
    partial class frmEmrBaseProgressOcsNew
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
            this.btnSearchRmk = new System.Windows.Forms.Button();
            this.rdoText = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.mbtnSheet = new System.Windows.Forms.Button();
            this.rdoView = new System.Windows.Forms.RadioButton();
            this.rdoImg = new System.Windows.Forms.RadioButton();
            this.panText = new System.Windows.Forms.Panel();
            this.panImg = new System.Windows.Forms.Panel();
            this.panView = new System.Windows.Forms.Panel();
            this.panMain = new System.Windows.Forms.Panel();
            this.panFormSearch = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSearchRmk
            // 
            this.btnSearchRmk.BackColor = System.Drawing.Color.White;
            this.btnSearchRmk.Location = new System.Drawing.Point(414, 1);
            this.btnSearchRmk.Name = "btnSearchRmk";
            this.btnSearchRmk.Size = new System.Drawing.Size(101, 30);
            this.btnSearchRmk.TabIndex = 34;
            this.btnSearchRmk.Text = "특이사항";
            this.btnSearchRmk.UseVisualStyleBackColor = false;
            this.btnSearchRmk.Click += new System.EventHandler(this.BtnSearchRmk_Click);
            // 
            // rdoText
            // 
            this.rdoText.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoText.AutoSize = true;
            this.rdoText.Location = new System.Drawing.Point(88, 3);
            this.rdoText.Name = "rdoText";
            this.rdoText.Size = new System.Drawing.Size(88, 27);
            this.rdoText.TabIndex = 35;
            this.rdoText.TabStop = true;
            this.rdoText.Text = "텍스트 작성";
            this.rdoText.UseVisualStyleBackColor = true;
            this.rdoText.CheckedChanged += new System.EventHandler(this.RdoText_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.mbtnSheet);
            this.panel1.Controls.Add(this.rdoView);
            this.panel1.Controls.Add(this.rdoImg);
            this.panel1.Controls.Add(this.rdoText);
            this.panel1.Controls.Add(this.btnSearchRmk);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(710, 34);
            this.panel1.TabIndex = 36;
            // 
            // mbtnSheet
            // 
            this.mbtnSheet.Location = new System.Drawing.Point(269, 3);
            this.mbtnSheet.Name = "mbtnSheet";
            this.mbtnSheet.Size = new System.Drawing.Size(85, 28);
            this.mbtnSheet.TabIndex = 36;
            this.mbtnSheet.Text = "사용자서식";
            this.mbtnSheet.UseVisualStyleBackColor = true;
            this.mbtnSheet.Click += new System.EventHandler(this.mbtnSheet_Click);
            // 
            // rdoView
            // 
            this.rdoView.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoView.AutoSize = true;
            this.rdoView.Location = new System.Drawing.Point(12, 3);
            this.rdoView.Name = "rdoView";
            this.rdoView.Size = new System.Drawing.Size(70, 27);
            this.rdoView.TabIndex = 35;
            this.rdoView.TabStop = true;
            this.rdoView.Text = "연속보기";
            this.rdoView.UseVisualStyleBackColor = true;
            this.rdoView.CheckedChanged += new System.EventHandler(this.RdoView_CheckedChanged);
            // 
            // rdoImg
            // 
            this.rdoImg.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoImg.AutoSize = true;
            this.rdoImg.Location = new System.Drawing.Point(176, 3);
            this.rdoImg.Name = "rdoImg";
            this.rdoImg.Size = new System.Drawing.Size(88, 27);
            this.rdoImg.TabIndex = 35;
            this.rdoImg.TabStop = true;
            this.rdoImg.Text = "이미지 작성";
            this.rdoImg.UseVisualStyleBackColor = true;
            this.rdoImg.CheckedChanged += new System.EventHandler(this.RdoImg_CheckedChanged);
            // 
            // panText
            // 
            this.panText.BackColor = System.Drawing.SystemColors.Control;
            this.panText.Location = new System.Drawing.Point(26, 25);
            this.panText.Name = "panText";
            this.panText.Size = new System.Drawing.Size(92, 75);
            this.panText.TabIndex = 37;
            // 
            // panImg
            // 
            this.panImg.AutoScroll = true;
            this.panImg.BackColor = System.Drawing.SystemColors.Control;
            this.panImg.Location = new System.Drawing.Point(136, 25);
            this.panImg.Name = "panImg";
            this.panImg.Size = new System.Drawing.Size(92, 75);
            this.panImg.TabIndex = 37;
            // 
            // panView
            // 
            this.panView.AutoScroll = true;
            this.panView.BackColor = System.Drawing.SystemColors.Control;
            this.panView.Location = new System.Drawing.Point(243, 25);
            this.panView.Name = "panView";
            this.panView.Size = new System.Drawing.Size(92, 75);
            this.panView.TabIndex = 37;
            // 
            // panMain
            // 
            this.panMain.BackColor = System.Drawing.SystemColors.Control;
            this.panMain.Controls.Add(this.panText);
            this.panMain.Controls.Add(this.panView);
            this.panMain.Controls.Add(this.panImg);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(0, 34);
            this.panMain.Name = "panMain";
            this.panMain.Size = new System.Drawing.Size(710, 760);
            this.panMain.TabIndex = 38;
            // 
            // panFormSearch
            // 
            this.panFormSearch.Location = new System.Drawing.Point(88, 340);
            this.panFormSearch.Name = "panFormSearch";
            this.panFormSearch.Size = new System.Drawing.Size(255, 114);
            this.panFormSearch.TabIndex = 39;
            this.panFormSearch.Visible = false;
            // 
            // frmEmrBaseProgressOcsNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 794);
            this.Controls.Add(this.panFormSearch);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "frmEmrBaseProgressOcsNew";
            this.Text = "frmEmrBaseProgressOcsNew";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmEmrBaseProgressOcsNew_FormClosed);
            this.Load += new System.EventHandler(this.FrmEmrBaseProgressOcsNew_Load);
            this.Resize += new System.EventHandler(this.frmEmrBaseProgressOcsNew_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSearchRmk;
        private System.Windows.Forms.RadioButton rdoText;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rdoView;
        private System.Windows.Forms.RadioButton rdoImg;
        private System.Windows.Forms.Panel panText;
        private System.Windows.Forms.Panel panImg;
        private System.Windows.Forms.Panel panView;
        private System.Windows.Forms.Panel panMain;
        private System.Windows.Forms.Button mbtnSheet;
        private System.Windows.Forms.Panel panFormSearch;
    }
}