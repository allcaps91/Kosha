namespace ComEmrBase
{
    partial class frmScanPageView
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
            this.picBig = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picBig)).BeginInit();
            this.SuspendLayout();
            // 
            // picBig
            // 
            this.picBig.BackColor = System.Drawing.Color.White;
            this.picBig.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picBig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBig.Location = new System.Drawing.Point(0, 0);
            this.picBig.Name = "picBig";
            this.picBig.Size = new System.Drawing.Size(925, 665);
            this.picBig.TabIndex = 49;
            this.picBig.TabStop = false;
            this.picBig.Visible = false;
            this.picBig.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picBig_MouseUp);
            // 
            // frmScanPageView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 665);
            this.Controls.Add(this.picBig);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmScanPageView";
            this.Text = "frmScanPageView";
            this.Load += new System.EventHandler(this.frmScanPageView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picBig)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picBig;
    }
}