namespace HcAdmin
{
    partial class FrmSignCopy
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtID2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cboDb2 = new System.Windows.Forms.ComboBox();
            this.txtID1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboDb1 = new System.Windows.Forms.ComboBox();
            this.btnCheck = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtID2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cboDb2);
            this.panel1.Controls.Add(this.txtID1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cboDb1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(410, 133);
            this.panel1.TabIndex = 2;
            // 
            // txtID2
            // 
            this.txtID2.Location = new System.Drawing.Point(177, 92);
            this.txtID2.Name = "txtID2";
            this.txtID2.Size = new System.Drawing.Size(76, 22);
            this.txtID2.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(126, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "원본 싸인 DB의 ID";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "2. 복사 싸인 DB";
            // 
            // cboDb2
            // 
            this.cboDb2.FormattingEnabled = true;
            this.cboDb2.Location = new System.Drawing.Point(131, 65);
            this.cboDb2.Name = "cboDb2";
            this.cboDb2.Size = new System.Drawing.Size(237, 21);
            this.cboDb2.TabIndex = 4;
            // 
            // txtID1
            // 
            this.txtID1.Location = new System.Drawing.Point(177, 36);
            this.txtID1.Name = "txtID1";
            this.txtID1.Size = new System.Drawing.Size(76, 22);
            this.txtID1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "원본 싸인 DB의 ID";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "1. 원본 싸인 DB";
            // 
            // cboDb1
            // 
            this.cboDb1.FormattingEnabled = true;
            this.cboDb1.Location = new System.Drawing.Point(131, 9);
            this.cboDb1.Name = "cboDb1";
            this.cboDb1.Size = new System.Drawing.Size(237, 21);
            this.cboDb1.TabIndex = 0;
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(12, 151);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(94, 29);
            this.btnCheck.TabIndex = 3;
            this.btnCheck.Text = "자료점검";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Enabled = false;
            this.btnCopy.Location = new System.Drawing.Point(128, 151);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(94, 29);
            this.btnCopy.TabIndex = 4;
            this.btnCopy.Text = "싸인복사";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(328, 151);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(94, 29);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // txtMsg
            // 
            this.txtMsg.Location = new System.Drawing.Point(12, 186);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMsg.Size = new System.Drawing.Size(410, 202);
            this.txtMsg.TabIndex = 6;
            // 
            // FrmSignCopy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 399);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("돋움체", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSignCopy";
            this.Text = "보건담당자 싸인 복사";
            this.Load += new System.EventHandler(this.FrmSignCopy_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtID2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboDb2;
        private System.Windows.Forms.TextBox txtID1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboDb1;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox txtMsg;
    }
}