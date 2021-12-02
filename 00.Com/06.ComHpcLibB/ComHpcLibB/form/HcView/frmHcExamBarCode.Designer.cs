namespace ComHpcLibB
{
    partial class frmHcExamBarCode
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
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSpecNo = new System.Windows.Forms.TextBox();
            this.lblBar = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnBarPrint = new System.Windows.Forms.Button();
            this.btnUSB = new System.Windows.Forms.Button();
            this.btnUSBT = new System.Windows.Forms.Button();
            this.btnBarMini = new System.Windows.Forms.Button();
            this.Pic = new System.Windows.Forms.PictureBox();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pic)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Padding = new System.Windows.Forms.Padding(1);
            this.panTitle.Size = new System.Drawing.Size(323, 38);
            this.panTitle.TabIndex = 23;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(238, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(82, 34);
            this.btnExit.TabIndex = 35;
            this.btnExit.Text = "닫 기(&X)";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(6, 7);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(180, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "검체번호 Barcode 인쇄";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnBarMini);
            this.panel1.Controls.Add(this.btnUSBT);
            this.panel1.Controls.Add(this.btnUSB);
            this.panel1.Controls.Add(this.btnBarPrint);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.lblBar);
            this.panel1.Controls.Add(this.Pic);
            this.panel1.Controls.Add(this.txtSpecNo);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(323, 174);
            this.panel1.TabIndex = 24;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "검체번호";
            // 
            // txtSpecNo
            // 
            this.txtSpecNo.Location = new System.Drawing.Point(66, 8);
            this.txtSpecNo.Name = "txtSpecNo";
            this.txtSpecNo.Size = new System.Drawing.Size(130, 21);
            this.txtSpecNo.TabIndex = 1;
            // 
            // lblBar
            // 
            this.lblBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBar.Location = new System.Drawing.Point(66, 76);
            this.lblBar.Name = "lblBar";
            this.lblBar.Size = new System.Drawing.Size(130, 23);
            this.lblBar.TabIndex = 3;
            this.lblBar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(66, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(247, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "바코드 프린트 드라이브를 활용하는 폼";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnBarPrint
            // 
            this.btnBarPrint.BackColor = System.Drawing.Color.White;
            this.btnBarPrint.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnBarPrint.Location = new System.Drawing.Point(203, 4);
            this.btnBarPrint.Name = "btnBarPrint";
            this.btnBarPrint.Size = new System.Drawing.Size(112, 28);
            this.btnBarPrint.TabIndex = 36;
            this.btnBarPrint.Text = "BarPrint";
            this.btnBarPrint.UseVisualStyleBackColor = false;
            this.btnBarPrint.Visible = false;
            // 
            // btnUSB
            // 
            this.btnUSB.BackColor = System.Drawing.Color.White;
            this.btnUSB.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnUSB.Location = new System.Drawing.Point(203, 39);
            this.btnUSB.Name = "btnUSB";
            this.btnUSB.Size = new System.Drawing.Size(112, 28);
            this.btnUSB.TabIndex = 37;
            this.btnUSB.Text = "USB Print";
            this.btnUSB.UseVisualStyleBackColor = false;
            this.btnUSB.Visible = false;
            // 
            // btnUSBT
            // 
            this.btnUSBT.BackColor = System.Drawing.Color.White;
            this.btnUSBT.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnUSBT.Location = new System.Drawing.Point(203, 74);
            this.btnUSBT.Name = "btnUSBT";
            this.btnUSBT.Size = new System.Drawing.Size(112, 28);
            this.btnUSBT.TabIndex = 38;
            this.btnUSBT.Text = "USB Print 통합";
            this.btnUSBT.UseVisualStyleBackColor = false;
            // 
            // btnBarMini
            // 
            this.btnBarMini.BackColor = System.Drawing.Color.White;
            this.btnBarMini.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnBarMini.Location = new System.Drawing.Point(203, 137);
            this.btnBarMini.Name = "btnBarMini";
            this.btnBarMini.Size = new System.Drawing.Size(112, 28);
            this.btnBarMini.TabIndex = 39;
            this.btnBarMini.Text = "USB Mini";
            this.btnBarMini.UseVisualStyleBackColor = false;
            this.btnBarMini.Visible = false;
            // 
            // Pic
            // 
            this.Pic.Location = new System.Drawing.Point(66, 41);
            this.Pic.Name = "Pic";
            this.Pic.Size = new System.Drawing.Size(130, 23);
            this.Pic.TabIndex = 2;
            this.Pic.TabStop = false;
            // 
            // frmHcExamBarCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(323, 212);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Name = "frmHcExamBarCode";
            this.Text = "frmHcExamBarCode";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnBarMini;
        private System.Windows.Forms.Button btnUSBT;
        private System.Windows.Forms.Button btnUSB;
        private System.Windows.Forms.Button btnBarPrint;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblBar;
        private System.Windows.Forms.PictureBox Pic;
        private System.Windows.Forms.TextBox txtSpecNo;
        private System.Windows.Forms.Label label1;
    }
}