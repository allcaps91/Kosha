namespace ComLibB
{
    partial class frmMibiPOPUP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMibiPOPUP));
            this.pan0 = new System.Windows.Forms.Panel();
            this.picWiming = new System.Windows.Forms.PictureBox();
            this.btnidentify = new System.Windows.Forms.Button();
            this.pan0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWiming)).BeginInit();
            this.SuspendLayout();
            // 
            // pan0
            // 
            this.pan0.BackColor = System.Drawing.Color.White;
            this.pan0.Controls.Add(this.picWiming);
            this.pan0.Controls.Add(this.btnidentify);
            this.pan0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pan0.Location = new System.Drawing.Point(0, 0);
            this.pan0.Name = "pan0";
            this.pan0.Size = new System.Drawing.Size(1008, 261);
            this.pan0.TabIndex = 0;
            // 
            // picWiming
            // 
            this.picWiming.ErrorImage = ((System.Drawing.Image)(resources.GetObject("picWiming.ErrorImage")));
            this.picWiming.Image = ((System.Drawing.Image)(resources.GetObject("picWiming.Image")));
            this.picWiming.InitialImage = ((System.Drawing.Image)(resources.GetObject("picWiming.InitialImage")));
            this.picWiming.Location = new System.Drawing.Point(12, 49);
            this.picWiming.Name = "picWiming";
            this.picWiming.Size = new System.Drawing.Size(984, 82);
            this.picWiming.TabIndex = 3;
            this.picWiming.TabStop = false;
            // 
            // btnidentify
            // 
            this.btnidentify.Location = new System.Drawing.Point(462, 161);
            this.btnidentify.Name = "btnidentify";
            this.btnidentify.Size = new System.Drawing.Size(109, 53);
            this.btnidentify.TabIndex = 1;
            this.btnidentify.Text = "확인";
            this.btnidentify.UseVisualStyleBackColor = true;
            this.btnidentify.Click += new System.EventHandler(this.btnidentify_Click);
            // 
            // frmMibiPOPUP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 261);
            this.Controls.Add(this.pan0);
            this.Name = "frmMibiPOPUP";
            this.Text = "재원자 미비차트 확인";
            this.pan0.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picWiming)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pan0;
        private System.Windows.Forms.Button btnidentify;
        private System.Windows.Forms.PictureBox picWiming;
    }
}