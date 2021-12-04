namespace HC_OSHA
{
    partial class FrmPassChange
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
            this.panJikwon = new System.Windows.Forms.Panel();
            this.lblSabunInfo = new System.Windows.Forms.Label();
            this.lblLTD02 = new System.Windows.Forms.Label();
            this.txtPass1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPass2 = new System.Windows.Forms.TextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.panJikwon.SuspendLayout();
            this.SuspendLayout();
            // 
            // panJikwon
            // 
            this.panJikwon.BackColor = System.Drawing.Color.RoyalBlue;
            this.panJikwon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panJikwon.Controls.Add(this.lblSabunInfo);
            this.panJikwon.Dock = System.Windows.Forms.DockStyle.Top;
            this.panJikwon.ForeColor = System.Drawing.Color.White;
            this.panJikwon.Location = new System.Drawing.Point(0, 0);
            this.panJikwon.Name = "panJikwon";
            this.panJikwon.Size = new System.Drawing.Size(275, 19);
            this.panJikwon.TabIndex = 39;
            // 
            // lblSabunInfo
            // 
            this.lblSabunInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSabunInfo.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSabunInfo.Location = new System.Drawing.Point(0, 0);
            this.lblSabunInfo.Name = "lblSabunInfo";
            this.lblSabunInfo.Size = new System.Drawing.Size(250, 17);
            this.lblSabunInfo.TabIndex = 4;
            this.lblSabunInfo.Text = "123456 제갈공명 비밀번호 변경";
            this.lblSabunInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLTD02
            // 
            this.lblLTD02.BackColor = System.Drawing.Color.LightBlue;
            this.lblLTD02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLTD02.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLTD02.Location = new System.Drawing.Point(14, 47);
            this.lblLTD02.Name = "lblLTD02";
            this.lblLTD02.Size = new System.Drawing.Size(105, 23);
            this.lblLTD02.TabIndex = 79;
            this.lblLTD02.Text = "변경할 비밀번호";
            this.lblLTD02.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPass1
            // 
            this.txtPass1.Location = new System.Drawing.Point(125, 45);
            this.txtPass1.Name = "txtPass1";
            this.txtPass1.PasswordChar = '*';
            this.txtPass1.Size = new System.Drawing.Size(140, 25);
            this.txtPass1.TabIndex = 80;
            this.txtPass1.Tag = "MAILCODE";
            this.txtPass1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LightBlue;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(14, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 23);
            this.label1.TabIndex = 81;
            this.label1.Text = "비밀번호 확인";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPass2
            // 
            this.txtPass2.Location = new System.Drawing.Point(125, 82);
            this.txtPass2.Name = "txtPass2";
            this.txtPass2.PasswordChar = '*';
            this.txtPass2.Size = new System.Drawing.Size(140, 25);
            this.txtPass2.TabIndex = 82;
            this.txtPass2.Tag = "";
            this.txtPass2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(181, 134);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(82, 32);
            this.btnExit.TabIndex = 83;
            this.btnExit.Text = "취소";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave.Location = new System.Drawing.Point(14, 134);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(121, 32);
            this.btnSave.TabIndex = 84;
            this.btnSave.Text = "비밀번호 변경";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // FrmPassChange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 179);
            this.ControlBox = false;
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.txtPass2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPass1);
            this.Controls.Add(this.lblLTD02);
            this.Controls.Add(this.panJikwon);
            this.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "FrmPassChange";
            this.Text = "비밀번호 변경";
            this.Load += new System.EventHandler(this.FrmPassChange_Load);
            this.panJikwon.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panJikwon;
        private System.Windows.Forms.Label lblSabunInfo;
        private System.Windows.Forms.Label lblLTD02;
        private System.Windows.Forms.TextBox txtPass1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPass2;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSave;
    }
}