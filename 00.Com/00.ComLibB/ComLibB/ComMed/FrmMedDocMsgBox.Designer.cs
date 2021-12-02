namespace ComLibB
{
    partial class FrmMedDocMsgBox
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
            this.RText = new System.Windows.Forms.RichTextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.Lbl_Black = new System.Windows.Forms.Label();
            this.btnBlack = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // RText
            // 
            this.RText.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.RText.Location = new System.Drawing.Point(1, 1);
            this.RText.Name = "RText";
            this.RText.Size = new System.Drawing.Size(911, 587);
            this.RText.TabIndex = 0;
            this.RText.Text = "";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnExit.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(416, 594);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 75);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "닫기(&X)";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // Lbl_Black
            // 
            this.Lbl_Black.BackColor = System.Drawing.Color.Black;
            this.Lbl_Black.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Lbl_Black.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Lbl_Black.ForeColor = System.Drawing.Color.White;
            this.Lbl_Black.Location = new System.Drawing.Point(4, 594);
            this.Lbl_Black.Name = "Lbl_Black";
            this.Lbl_Black.Size = new System.Drawing.Size(406, 74);
            this.Lbl_Black.TabIndex = 5;
            this.Lbl_Black.Text = "Information !!";
            this.Lbl_Black.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Lbl_Black.Visible = false;
            // 
            // btnBlack
            // 
            this.btnBlack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBlack.BackColor = System.Drawing.Color.White;
            this.btnBlack.Location = new System.Drawing.Point(9, 85);
            this.btnBlack.Name = "btnBlack";
            this.btnBlack.Size = new System.Drawing.Size(113, 30);
            this.btnBlack.TabIndex = 6;
            this.btnBlack.Text = "내용보기";
            this.btnBlack.UseVisualStyleBackColor = false;
            this.btnBlack.Visible = false;
            this.btnBlack.Click += new System.EventHandler(this.btnBlack_Click);
            // 
            // FrmMedDocMsgBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(915, 669);
            this.Controls.Add(this.btnBlack);
            this.Controls.Add(this.Lbl_Black);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.RText);
            this.Name = "FrmMedDocMsgBox";
            this.Text = "메시지 전달 사항";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmDocMsgBox_FormClosing);
            this.Load += new System.EventHandler(this.FrmDocMsgBox_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox RText;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label Lbl_Black;
        private System.Windows.Forms.Button btnBlack;
    }
}