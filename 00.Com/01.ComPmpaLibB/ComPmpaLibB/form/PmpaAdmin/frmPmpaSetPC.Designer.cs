namespace ComPmpaLibB
{
    partial class frmSetPmpaPC
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
            this.pnlFill = new System.Windows.Forms.Panel();
            this.grbSign = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.optCard0 = new System.Windows.Forms.RadioButton();
            this.txtPad = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.grbWhite = new System.Windows.Forms.GroupBox();
            this.txtY = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtX = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.grbLocation = new System.Windows.Forms.GroupBox();
            this.cboLocation = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboBand = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlHead = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.pnlFill.SuspendLayout();
            this.grbSign.SuspendLayout();
            this.grbWhite.SuspendLayout();
            this.grbLocation.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.pnlHead.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFill
            // 
            this.pnlFill.BackColor = System.Drawing.SystemColors.Window;
            this.pnlFill.Controls.Add(this.grbSign);
            this.pnlFill.Controls.Add(this.grbWhite);
            this.pnlFill.Controls.Add(this.grbLocation);
            this.pnlFill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFill.Location = new System.Drawing.Point(0, 82);
            this.pnlFill.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlFill.Name = "pnlFill";
            this.pnlFill.Size = new System.Drawing.Size(399, 380);
            this.pnlFill.TabIndex = 179;
            // 
            // grbSign
            // 
            this.grbSign.Controls.Add(this.label6);
            this.grbSign.Controls.Add(this.optCard0);
            this.grbSign.Controls.Add(this.txtPad);
            this.grbSign.Controls.Add(this.label3);
            this.grbSign.Location = new System.Drawing.Point(13, 252);
            this.grbSign.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grbSign.Name = "grbSign";
            this.grbSign.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grbSign.Size = new System.Drawing.Size(376, 108);
            this.grbSign.TabIndex = 4;
            this.grbSign.TabStop = false;
            this.grbSign.Text = "서명방식";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(13, 70);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 17);
            this.label6.TabIndex = 165;
            this.label6.Text = "카드사";
            // 
            // optCard0
            // 
            this.optCard0.AutoSize = true;
            this.optCard0.BackColor = System.Drawing.Color.Transparent;
            this.optCard0.Checked = true;
            this.optCard0.Location = new System.Drawing.Point(78, 68);
            this.optCard0.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optCard0.Name = "optCard0";
            this.optCard0.Size = new System.Drawing.Size(136, 21);
            this.optCard0.TabIndex = 164;
            this.optCard0.TabStop = true;
            this.optCard0.Text = "DAOU(다우데이터)";
            this.optCard0.UseVisualStyleBackColor = false;
            // 
            // txtPad
            // 
            this.txtPad.Location = new System.Drawing.Point(78, 35);
            this.txtPad.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPad.MaxLength = 1;
            this.txtPad.Name = "txtPad";
            this.txtPad.Size = new System.Drawing.Size(59, 25);
            this.txtPad.TabIndex = 154;
            this.txtPad.Text = "P";
            this.txtPad.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPad.Leave += new System.EventHandler(this.txtPad_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(16, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(324, 17);
            this.label3.TabIndex = 162;
            this.label3.Text = "서명방법                    (SignPad:P, Tablet:T, 혼합:S)";
            // 
            // grbWhite
            // 
            this.grbWhite.Controls.Add(this.txtY);
            this.grbWhite.Controls.Add(this.label5);
            this.grbWhite.Controls.Add(this.txtX);
            this.grbWhite.Controls.Add(this.label4);
            this.grbWhite.Location = new System.Drawing.Point(12, 133);
            this.grbWhite.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grbWhite.Name = "grbWhite";
            this.grbWhite.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grbWhite.Size = new System.Drawing.Size(376, 111);
            this.grbWhite.TabIndex = 3;
            this.grbWhite.TabStop = false;
            this.grbWhite.Text = "영수증여백";
            // 
            // txtY
            // 
            this.txtY.Location = new System.Drawing.Point(79, 73);
            this.txtY.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtY.MaxLength = 4;
            this.txtY.Name = "txtY";
            this.txtY.Size = new System.Drawing.Size(59, 25);
            this.txtY.TabIndex = 153;
            this.txtY.Text = "10";
            this.txtY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtY_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(17, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 17);
            this.label5.TabIndex = 152;
            this.label5.Text = "Y Margin";
            // 
            // txtX
            // 
            this.txtX.Location = new System.Drawing.Point(79, 40);
            this.txtX.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtX.MaxLength = 4;
            this.txtX.Name = "txtX";
            this.txtX.Size = new System.Drawing.Size(59, 25);
            this.txtX.TabIndex = 152;
            this.txtX.Text = "10";
            this.txtX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(17, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 17);
            this.label4.TabIndex = 150;
            this.label4.Text = "X Margin";
            // 
            // grbLocation
            // 
            this.grbLocation.Controls.Add(this.cboLocation);
            this.grbLocation.Controls.Add(this.label2);
            this.grbLocation.Controls.Add(this.cboBand);
            this.grbLocation.Controls.Add(this.label1);
            this.grbLocation.Location = new System.Drawing.Point(12, 18);
            this.grbLocation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grbLocation.Name = "grbLocation";
            this.grbLocation.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grbLocation.Size = new System.Drawing.Size(377, 107);
            this.grbLocation.TabIndex = 2;
            this.grbLocation.TabStop = false;
            this.grbLocation.Text = "위치확인";
            // 
            // cboLocation
            // 
            this.cboLocation.FormattingEnabled = true;
            this.cboLocation.Location = new System.Drawing.Point(168, 67);
            this.cboLocation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboLocation.Name = "cboLocation";
            this.cboLocation.Size = new System.Drawing.Size(181, 25);
            this.cboLocation.TabIndex = 151;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(17, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(153, 17);
            this.label2.TabIndex = 151;
            this.label2.Text = "접수/수납 프로그램 위치";
            // 
            // cboBand
            // 
            this.cboBand.FormattingEnabled = true;
            this.cboBand.Location = new System.Drawing.Point(168, 34);
            this.cboBand.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboBand.Name = "cboBand";
            this.cboBand.Size = new System.Drawing.Size(181, 25);
            this.cboBand.TabIndex = 150;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(17, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 17);
            this.label1.TabIndex = 149;
            this.label1.Text = "환자인식밴드 위치";
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.SystemColors.Window;
            this.pnlTop.Controls.Add(this.btnOk);
            this.pnlTop.Controls.Add(this.btnExit);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 42);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(10, 14, 10, 14);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(3);
            this.pnlTop.Size = new System.Drawing.Size(399, 40);
            this.pnlTop.TabIndex = 178;
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.SystemColors.Window;
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnOk.Location = new System.Drawing.Point(226, 3);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(85, 34);
            this.btnOk.TabIndex = 150;
            this.btnOk.Text = "저장";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.SystemColors.Window;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(311, 3);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(85, 34);
            this.btnExit.TabIndex = 149;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // pnlHead
            // 
            this.pnlHead.BackColor = System.Drawing.Color.RoyalBlue;
            this.pnlHead.Controls.Add(this.label7);
            this.pnlHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHead.ForeColor = System.Drawing.Color.White;
            this.pnlHead.Location = new System.Drawing.Point(0, 0);
            this.pnlHead.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlHead.Name = "pnlHead";
            this.pnlHead.Size = new System.Drawing.Size(399, 42);
            this.pnlHead.TabIndex = 177;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(14, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 17);
            this.label7.TabIndex = 0;
            this.label7.Text = "환경설정";
            // 
            // frmSetPmpaPC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(399, 462);
            this.ControlBox = false;
            this.Controls.Add(this.pnlFill);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlHead);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmSetPmpaPC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "   ";
            this.pnlFill.ResumeLayout(false);
            this.grbSign.ResumeLayout(false);
            this.grbSign.PerformLayout();
            this.grbWhite.ResumeLayout(false);
            this.grbWhite.PerformLayout();
            this.grbLocation.ResumeLayout(false);
            this.grbLocation.PerformLayout();
            this.pnlTop.ResumeLayout(false);
            this.pnlHead.ResumeLayout(false);
            this.pnlHead.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlFill;
        private System.Windows.Forms.GroupBox grbSign;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton optCard0;
        private System.Windows.Forms.TextBox txtPad;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox grbWhite;
        private System.Windows.Forms.TextBox txtY;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtX;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox grbLocation;
        private System.Windows.Forms.ComboBox cboLocation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboBand;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Button btnOk;
        public System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel pnlHead;
        private System.Windows.Forms.Label label7;
    }
}