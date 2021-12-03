namespace ComLibB
{
    partial class PmpaMirSugaJemsu
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
            this.btnJob1 = new System.Windows.Forms.Button();
            this.btnXray = new System.Windows.Forms.Button();
            this.txtState = new System.Windows.Forms.TextBox();
            this.panTitleSub0.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 0);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(451, 34);
            this.panTitleSub0.TabIndex = 13;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 5);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(112, 21);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "상대가치 점수";
            // 
            // btnJob1
            // 
            this.btnJob1.AutoSize = true;
            this.btnJob1.Location = new System.Drawing.Point(50, 62);
            this.btnJob1.Name = "btnJob1";
            this.btnJob1.Size = new System.Drawing.Size(209, 27);
            this.btnJob1.TabIndex = 14;
            this.btnJob1.Text = "상대가치 WORK를 DB에 INSERT";
            this.btnJob1.UseVisualStyleBackColor = true;
            this.btnJob1.Click += new System.EventHandler(this.btnJob1_Click);
            // 
            // btnXray
            // 
            this.btnXray.AutoSize = true;
            this.btnXray.Location = new System.Drawing.Point(50, 107);
            this.btnXray.Name = "btnXray";
            this.btnXray.Size = new System.Drawing.Size(209, 27);
            this.btnXray.TabIndex = 14;
            this.btnXray.Text = "방사선 단순촬영 상대가치 형성";
            this.btnXray.UseVisualStyleBackColor = true;
            this.btnXray.Click += new System.EventHandler(this.btnXray_Click);
            // 
            // txtState
            // 
            this.txtState.Location = new System.Drawing.Point(281, 64);
            this.txtState.Name = "txtState";
            this.txtState.ReadOnly = true;
            this.txtState.Size = new System.Drawing.Size(100, 25);
            this.txtState.TabIndex = 15;
            // 
            // PmpaMirSugaJemsu
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(451, 237);
            this.Controls.Add(this.txtState);
            this.Controls.Add(this.btnXray);
            this.Controls.Add(this.btnJob1);
            this.Controls.Add(this.panTitleSub0);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "PmpaMirSugaJemsu";
            this.Text = "상대가치 점수";
            this.Load += new System.EventHandler(this.PmpaMirSugaJemsu_Load);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Button btnJob1;
        private System.Windows.Forms.Button btnXray;
        private System.Windows.Forms.TextBox txtState;
    }
}