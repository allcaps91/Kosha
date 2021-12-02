namespace ComNurLibB
{
    partial class frmDCActing
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
            this.radioCase3 = new System.Windows.Forms.RadioButton();
            this.TxtRemark = new System.Windows.Forms.TextBox();
            this.radioCase4 = new System.Windows.Forms.RadioButton();
            this.radioCase2 = new System.Windows.Forms.RadioButton();
            this.radioCase1 = new System.Windows.Forms.RadioButton();
            this.panel29 = new System.Windows.Forms.Panel();
            this.label23 = new System.Windows.Forms.Label();
            this.BtnSave = new System.Windows.Forms.Button();
            this.panel29.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioCase3
            // 
            this.radioCase3.AutoSize = true;
            this.radioCase3.Location = new System.Drawing.Point(8, 101);
            this.radioCase3.Name = "radioCase3";
            this.radioCase3.Size = new System.Drawing.Size(47, 16);
            this.radioCase3.TabIndex = 84;
            this.radioCase3.Text = "거부";
            this.radioCase3.UseVisualStyleBackColor = true;
            // 
            // TxtRemark
            // 
            this.TxtRemark.Location = new System.Drawing.Point(68, 138);
            this.TxtRemark.Name = "TxtRemark";
            this.TxtRemark.Size = new System.Drawing.Size(179, 21);
            this.TxtRemark.TabIndex = 82;
            // 
            // radioCase4
            // 
            this.radioCase4.AutoSize = true;
            this.radioCase4.Location = new System.Drawing.Point(8, 138);
            this.radioCase4.Name = "radioCase4";
            this.radioCase4.Size = new System.Drawing.Size(47, 16);
            this.radioCase4.TabIndex = 81;
            this.radioCase4.Text = "기타";
            this.radioCase4.UseVisualStyleBackColor = true;
            // 
            // radioCase2
            // 
            this.radioCase2.AutoSize = true;
            this.radioCase2.Location = new System.Drawing.Point(8, 71);
            this.radioCase2.Name = "radioCase2";
            this.radioCase2.Size = new System.Drawing.Size(137, 16);
            this.radioCase2.TabIndex = 80;
            this.radioCase2.Text = "금식-수술/시술/검사";
            this.radioCase2.UseVisualStyleBackColor = true;
            // 
            // radioCase1
            // 
            this.radioCase1.AutoSize = true;
            this.radioCase1.Checked = true;
            this.radioCase1.Location = new System.Drawing.Point(8, 37);
            this.radioCase1.Name = "radioCase1";
            this.radioCase1.Size = new System.Drawing.Size(75, 16);
            this.radioCase1.TabIndex = 79;
            this.radioCase1.TabStop = true;
            this.radioCase1.Text = "처방 변경";
            this.radioCase1.UseVisualStyleBackColor = true;
            // 
            // panel29
            // 
            this.panel29.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel29.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel29.Controls.Add(this.label23);
            this.panel29.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel29.Location = new System.Drawing.Point(0, 0);
            this.panel29.Name = "panel29";
            this.panel29.Size = new System.Drawing.Size(260, 28);
            this.panel29.TabIndex = 78;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label23.ForeColor = System.Drawing.Color.White;
            this.label23.Location = new System.Drawing.Point(8, 4);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(48, 15);
            this.label23.TabIndex = 0;
            this.label23.Text = "DC사유";
            // 
            // BtnSave
            // 
            this.BtnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSave.BackColor = System.Drawing.Color.Transparent;
            this.BtnSave.Location = new System.Drawing.Point(8, 185);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(239, 30);
            this.BtnSave.TabIndex = 85;
            this.BtnSave.Text = "선    택";
            this.BtnSave.UseVisualStyleBackColor = false;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // frmDCActing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(260, 227);
            this.ControlBox = false;
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.radioCase3);
            this.Controls.Add(this.TxtRemark);
            this.Controls.Add(this.radioCase4);
            this.Controls.Add(this.radioCase2);
            this.Controls.Add(this.radioCase1);
            this.Controls.Add(this.panel29);
            this.Name = "frmDCActing";
            this.Text = "DC사유관리";
            this.Load += new System.EventHandler(this.frmDCActing_Load);
            this.panel29.ResumeLayout(false);
            this.panel29.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioCase3;
        private System.Windows.Forms.TextBox TxtRemark;
        private System.Windows.Forms.RadioButton radioCase4;
        private System.Windows.Forms.RadioButton radioCase2;
        private System.Windows.Forms.RadioButton radioCase1;
        private System.Windows.Forms.Panel panel29;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button BtnSave;
    }
}