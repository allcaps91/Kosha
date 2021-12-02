namespace ComEmrBase
{
    partial class frmEasTabletViewer
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
            this.BtnDev = new System.Windows.Forms.Button();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnEmptyPrint = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnDev
            // 
            this.BtnDev.Location = new System.Drawing.Point(9, 10);
            this.BtnDev.Name = "BtnDev";
            this.BtnDev.Size = new System.Drawing.Size(75, 23);
            this.BtnDev.TabIndex = 0;
            this.BtnDev.Text = "개발자";
            this.BtnDev.UseVisualStyleBackColor = true;
            this.BtnDev.Visible = false;
            this.BtnDev.Click += new System.EventHandler(this.Button1_Click);
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.BtnDev);
            this.panTitle.Controls.Add(this.btnSave);
            this.panTitle.Controls.Add(this.btnEmptyPrint);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1158, 44);
            this.panTitle.TabIndex = 42;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.Location = new System.Drawing.Point(611, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(79, 30);
            this.btnSave.TabIndex = 39;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnEmptyPrint
            // 
            this.btnEmptyPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnEmptyPrint.Location = new System.Drawing.Point(217, 5);
            this.btnEmptyPrint.Name = "btnEmptyPrint";
            this.btnEmptyPrint.Size = new System.Drawing.Size(81, 30);
            this.btnEmptyPrint.TabIndex = 37;
            this.btnEmptyPrint.Text = "출력";
            this.btnEmptyPrint.UseVisualStyleBackColor = false;
            this.btnEmptyPrint.Visible = false;
            this.btnEmptyPrint.Click += new System.EventHandler(this.btnEmptyPrint_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 44);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1158, 473);
            this.panel1.TabIndex = 70;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel1_Paint);
            // 
            // frmEasTabletViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1158, 517);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Name = "frmEasTabletViewer";
            this.Text = "전자동의서 태블릿모니터";
            this.panTitle.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnDev;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnEmptyPrint;
        private System.Windows.Forms.Panel panel1;
    }
}