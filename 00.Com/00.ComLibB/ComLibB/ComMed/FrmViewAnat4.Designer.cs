namespace ComLibB
{
    partial class FrmViewAnat4
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnRegist = new System.Windows.Forms.Button();
            this.rtxtRequest = new System.Windows.Forms.RichTextBox();
            this.lblMayakTitle = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(160, 21);
            this.lblTitle.TabIndex = 5;
            this.lblTitle.Text = "마약처방전 주요증상";
            // 
            // btnRegist
            // 
            this.btnRegist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRegist.BackColor = System.Drawing.Color.White;
            this.btnRegist.Location = new System.Drawing.Point(251, 6);
            this.btnRegist.Name = "btnRegist";
            this.btnRegist.Size = new System.Drawing.Size(72, 30);
            this.btnRegist.TabIndex = 27;
            this.btnRegist.Text = "확인";
            this.btnRegist.UseVisualStyleBackColor = false;
            this.btnRegist.Click += new System.EventHandler(this.btnRegist_Click);
            // 
            // rtxtRequest
            // 
            this.rtxtRequest.Location = new System.Drawing.Point(7, 62);
            this.rtxtRequest.Name = "rtxtRequest";
            this.rtxtRequest.Size = new System.Drawing.Size(394, 121);
            this.rtxtRequest.TabIndex = 28;
            this.rtxtRequest.Text = "";
            // 
            // lblMayakTitle
            // 
            this.lblMayakTitle.AutoSize = true;
            this.lblMayakTitle.BackColor = System.Drawing.Color.RoyalBlue;
            this.lblMayakTitle.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblMayakTitle.ForeColor = System.Drawing.Color.White;
            this.lblMayakTitle.Location = new System.Drawing.Point(8, 43);
            this.lblMayakTitle.Name = "lblMayakTitle";
            this.lblMayakTitle.Size = new System.Drawing.Size(130, 17);
            this.lblMayakTitle.TabIndex = 29;
            this.lblMayakTitle.Text = "마약처방전 주요증상";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(329, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 30);
            this.btnCancel.TabIndex = 30;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FrmViewAnat4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(408, 189);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblMayakTitle);
            this.Controls.Add(this.rtxtRequest);
            this.Controls.Add(this.btnRegist);
            this.Controls.Add(this.lblTitle);
            this.Name = "FrmViewAnat4";
            this.Text = "FrmViewAnat4";
            this.Load += new System.EventHandler(this.FrmViewAnat4_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnRegist;
        private System.Windows.Forms.RichTextBox rtxtRequest;
        private System.Windows.Forms.Label lblMayakTitle;
        private System.Windows.Forms.Button btnCancel;
    }
}