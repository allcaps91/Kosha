namespace HC_Core
{
    partial class SignPadForm
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
            this.PanWeb = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.PanWeb.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanWeb
            // 
            this.PanWeb.Controls.Add(this.button1);
            this.PanWeb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanWeb.Location = new System.Drawing.Point(0, 0);
            this.PanWeb.Name = "PanWeb";
            this.PanWeb.Size = new System.Drawing.Size(684, 311);
            this.PanWeb.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(344, 94);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SignPadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 311);
            this.Controls.Add(this.PanWeb);
            this.MaximumSize = new System.Drawing.Size(700, 350);
            this.Name = "SignPadForm";
            this.Text = "SignPadForm";
            this.PanWeb.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanWeb;
        private System.Windows.Forms.Button button1;
    }
}