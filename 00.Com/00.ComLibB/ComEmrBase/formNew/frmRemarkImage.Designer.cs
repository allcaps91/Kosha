namespace ComEmrBase
{
    partial class frmRemarkImage
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
            this.panImg = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panImg
            // 
            this.panImg.AutoScroll = true;
            this.panImg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panImg.Location = new System.Drawing.Point(0, 0);
            this.panImg.Name = "panImg";
            this.panImg.Size = new System.Drawing.Size(1145, 620);
            this.panImg.TabIndex = 0;
            // 
            // frmRemarkImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1145, 620);
            this.Controls.Add(this.panImg);
            this.Name = "frmRemarkImage";
            this.Text = "참고사항";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmRemarkImage_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panImg;
    }
}