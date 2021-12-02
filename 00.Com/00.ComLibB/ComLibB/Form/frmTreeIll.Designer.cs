namespace ComLibB
{
    partial class frmTreeIll
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnKor = new System.Windows.Forms.Button();
            this.btnEng = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblName = new System.Windows.Forms.Label();
            this.trvView = new System.Windows.Forms.TreeView();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(571, 34);
            this.panTitle.TabIndex = 48;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(125, 16);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "상병코드 TREE";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(496, 2);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnKor
            // 
            this.btnKor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKor.BackColor = System.Drawing.Color.Transparent;
            this.btnKor.Location = new System.Drawing.Point(496, 1);
            this.btnKor.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnKor.Name = "btnKor";
            this.btnKor.Size = new System.Drawing.Size(72, 30);
            this.btnKor.TabIndex = 46;
            this.btnKor.Text = "한글명";
            this.btnKor.UseVisualStyleBackColor = false;
            this.btnKor.Click += new System.EventHandler(this.btnKor_Click);
            // 
            // btnEng
            // 
            this.btnEng.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEng.BackColor = System.Drawing.Color.Transparent;
            this.btnEng.Location = new System.Drawing.Point(424, 1);
            this.btnEng.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnEng.Name = "btnEng";
            this.btnEng.Size = new System.Drawing.Size(72, 30);
            this.btnEng.TabIndex = 47;
            this.btnEng.Text = "영어명";
            this.btnEng.UseVisualStyleBackColor = false;
            this.btnEng.Click += new System.EventHandler(this.btnEng_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.BackColor = System.Drawing.Color.Transparent;
            this.btnSelect.Location = new System.Drawing.Point(352, 1);
            this.btnSelect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(72, 30);
            this.btnSelect.TabIndex = 48;
            this.btnSelect.Text = "선택";
            this.btnSelect.UseVisualStyleBackColor = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblName);
            this.panel1.Controls.Add(this.btnSelect);
            this.panel1.Controls.Add(this.btnKor);
            this.panel1.Controls.Add(this.btnEng);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(571, 32);
            this.panel1.TabIndex = 49;
            // 
            // lblName
            // 
            this.lblName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblName.Font = new System.Drawing.Font("굴림", 11F);
            this.lblName.Location = new System.Drawing.Point(6, 2);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(340, 28);
            this.lblName.TabIndex = 49;
            // 
            // trvView
            // 
            this.trvView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvView.Location = new System.Drawing.Point(0, 66);
            this.trvView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.trvView.Name = "trvView";
            this.trvView.Size = new System.Drawing.Size(571, 467);
            this.trvView.TabIndex = 50;
            this.trvView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.trvView_NodeMouseClick);
            // 
            // frmTreeIll
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 533);
            this.Controls.Add(this.trvView);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmTreeIll";
            this.Text = "상병코드 TREE";
            this.Load += new System.EventHandler(this.frmTreeIll_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnKor;
        private System.Windows.Forms.Button btnEng;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TreeView trvView;
    }
}