namespace ComLibB
{
    partial class FrmMedDurCheckCancel
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCheckCancel = new System.Windows.Forms.Button();
            this.cReasonCd = new System.Windows.Forms.ComboBox();
            this.cReasonMsg = new System.Windows.Forms.TextBox();
            this.cGrantNo = new System.Windows.Forms.TextBox();
            this.cPrscAdmSym = new System.Windows.Forms.TextBox();
            this.cPrscDd = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(248, 196);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 41);
            this.btnCancel.TabIndex = 42;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCheckCancel
            // 
            this.btnCheckCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCheckCancel.BackColor = System.Drawing.Color.White;
            this.btnCheckCancel.Location = new System.Drawing.Point(145, 196);
            this.btnCheckCancel.Name = "btnCheckCancel";
            this.btnCheckCancel.Size = new System.Drawing.Size(95, 41);
            this.btnCheckCancel.TabIndex = 41;
            this.btnCheckCancel.Text = "점검취소요청";
            this.btnCheckCancel.UseVisualStyleBackColor = false;
            this.btnCheckCancel.Click += new System.EventHandler(this.btnCheckCancel_Click);
            // 
            // cReasonCd
            // 
            this.cReasonCd.FormattingEnabled = true;
            this.cReasonCd.Location = new System.Drawing.Point(124, 132);
            this.cReasonCd.Name = "cReasonCd";
            this.cReasonCd.Size = new System.Drawing.Size(308, 20);
            this.cReasonCd.TabIndex = 40;
            this.cReasonCd.Click += new System.EventHandler(this.cReasonCd_Click);
            // 
            // cReasonMsg
            // 
            this.cReasonMsg.Location = new System.Drawing.Point(124, 161);
            this.cReasonMsg.Name = "cReasonMsg";
            this.cReasonMsg.Size = new System.Drawing.Size(308, 21);
            this.cReasonMsg.TabIndex = 39;
            // 
            // cGrantNo
            // 
            this.cGrantNo.Location = new System.Drawing.Point(124, 102);
            this.cGrantNo.Name = "cGrantNo";
            this.cGrantNo.Size = new System.Drawing.Size(194, 21);
            this.cGrantNo.TabIndex = 38;
            // 
            // cPrscAdmSym
            // 
            this.cPrscAdmSym.Location = new System.Drawing.Point(124, 72);
            this.cPrscAdmSym.Name = "cPrscAdmSym";
            this.cPrscAdmSym.Size = new System.Drawing.Size(194, 21);
            this.cPrscAdmSym.TabIndex = 37;
            // 
            // cPrscDd
            // 
            this.cPrscDd.Location = new System.Drawing.Point(124, 42);
            this.cPrscDd.Name = "cPrscDd";
            this.cPrscDd.Size = new System.Drawing.Size(194, 21);
            this.cPrscDd.TabIndex = 36;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(67, 165);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 35;
            this.label5.Text = "취소사유";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(43, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 34;
            this.label4.Text = "취소사유코드";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 33;
            this.label3.Text = "처방전교부번호";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 12);
            this.label2.TabIndex = 32;
            this.label2.Text = "처방전발행기관기호";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(67, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 31;
            this.label1.Text = "처방일자";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(7, 4);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(74, 21);
            this.lblTitle.TabIndex = 30;
            this.lblTitle.Text = "점검취소";
            // 
            // FrmMedDurCheckCancel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(442, 247);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCheckCancel);
            this.Controls.Add(this.cReasonCd);
            this.Controls.Add(this.cReasonMsg);
            this.Controls.Add(this.cGrantNo);
            this.Controls.Add(this.cPrscAdmSym);
            this.Controls.Add(this.cPrscDd);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTitle);
            this.Name = "FrmMedDurCheckCancel";
            this.Text = "DUR 점검취소";
            this.Load += new System.EventHandler(this.FrmMedDurCheckCancel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCheckCancel;
        private System.Windows.Forms.ComboBox cReasonCd;
        private System.Windows.Forms.TextBox cReasonMsg;
        private System.Windows.Forms.TextBox cGrantNo;
        private System.Windows.Forms.TextBox cPrscAdmSym;
        private System.Windows.Forms.TextBox cPrscDd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTitle;
    }
}