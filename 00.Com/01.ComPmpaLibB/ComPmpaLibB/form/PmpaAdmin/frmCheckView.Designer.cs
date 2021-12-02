namespace ComPmpaLibB
{
    partial class frmCheckView
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
            this.panMsg = new System.Windows.Forms.Panel();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.lblServiece = new System.Windows.Forms.Label();
            this.panMain = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cboAmtGbn = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtCredit = new System.Windows.Forms.TextBox();
            this.txtDate = new System.Windows.Forms.TextBox();
            this.txtAmt = new System.Windows.Forms.TextBox();
            this.txtBill = new System.Windows.Forms.TextBox();
            this.label62 = new System.Windows.Forms.Label();
            this.label61 = new System.Windows.Forms.Label();
            this.label60 = new System.Windows.Forms.Label();
            this.label59 = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.panTitle.SuspendLayout();
            this.panMsg.SuspendLayout();
            this.panMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Margin = new System.Windows.Forms.Padding(2);
            this.panTitle.Name = "panTitle";
            this.panTitle.Padding = new System.Windows.Forms.Padding(3);
            this.panTitle.Size = new System.Drawing.Size(448, 42);
            this.panTitle.TabIndex = 20;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(333, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(108, 32);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.White;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(6, 7);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(74, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "수표조회";
            // 
            // panMsg
            // 
            this.panMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panMsg.Controls.Add(this.txtMsg);
            this.panMsg.Controls.Add(this.lblServiece);
            this.panMsg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panMsg.Location = new System.Drawing.Point(0, 255);
            this.panMsg.Name = "panMsg";
            this.panMsg.Padding = new System.Windows.Forms.Padding(3);
            this.panMsg.Size = new System.Drawing.Size(448, 34);
            this.panMsg.TabIndex = 22;
            // 
            // txtMsg
            // 
            this.txtMsg.BackColor = System.Drawing.Color.Wheat;
            this.txtMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMsg.Location = new System.Drawing.Point(3, 3);
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(364, 25);
            this.txtMsg.TabIndex = 9;
            // 
            // lblServiece
            // 
            this.lblServiece.BackColor = System.Drawing.Color.Silver;
            this.lblServiece.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblServiece.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblServiece.Location = new System.Drawing.Point(367, 3);
            this.lblServiece.Name = "lblServiece";
            this.lblServiece.Size = new System.Drawing.Size(76, 26);
            this.lblServiece.TabIndex = 8;
            this.lblServiece.Text = "Off-Line";
            this.lblServiece.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panMain
            // 
            this.panMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panMain.Controls.Add(this.label1);
            this.panMain.Controls.Add(this.btnCancel);
            this.panMain.Controls.Add(this.cboAmtGbn);
            this.panMain.Controls.Add(this.btnSearch);
            this.panMain.Controls.Add(this.txtCredit);
            this.panMain.Controls.Add(this.txtDate);
            this.panMain.Controls.Add(this.txtAmt);
            this.panMain.Controls.Add(this.txtBill);
            this.panMain.Controls.Add(this.label62);
            this.panMain.Controls.Add(this.label61);
            this.panMain.Controls.Add(this.label60);
            this.panMain.Controls.Add(this.label59);
            this.panMain.Controls.Add(this.label58);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(0, 42);
            this.panMain.Name = "panMain";
            this.panMain.Padding = new System.Windows.Forms.Padding(3);
            this.panMain.Size = new System.Drawing.Size(448, 213);
            this.panMain.TabIndex = 23;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(293, 125);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 17);
            this.label1.TabIndex = 25;
            this.label1.Text = "(ex: YYYYMMDD)";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(309, 59);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(115, 32);
            this.btnCancel.TabIndex = 24;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // cboAmtGbn
            // 
            this.cboAmtGbn.FormattingEnabled = true;
            this.cboAmtGbn.Location = new System.Drawing.Point(156, 56);
            this.cboAmtGbn.Name = "cboAmtGbn";
            this.cboAmtGbn.Size = new System.Drawing.Size(131, 25);
            this.cboAmtGbn.TabIndex = 23;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(309, 23);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(115, 32);
            this.btnSearch.TabIndex = 22;
            this.btnSearch.Text = "수표조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // txtCredit
            // 
            this.txtCredit.Enabled = false;
            this.txtCredit.Location = new System.Drawing.Point(156, 155);
            this.txtCredit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtCredit.Name = "txtCredit";
            this.txtCredit.Size = new System.Drawing.Size(131, 25);
            this.txtCredit.TabIndex = 21;
            // 
            // txtDate
            // 
            this.txtDate.Location = new System.Drawing.Point(156, 122);
            this.txtDate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtDate.MaxLength = 8;
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(131, 25);
            this.txtDate.TabIndex = 20;
            this.txtDate.Text = "20121101";
            this.txtDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtAmt
            // 
            this.txtAmt.Location = new System.Drawing.Point(156, 89);
            this.txtAmt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtAmt.Name = "txtAmt";
            this.txtAmt.Size = new System.Drawing.Size(131, 25);
            this.txtAmt.TabIndex = 19;
            this.txtAmt.Text = "100000";
            this.txtAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtBill
            // 
            this.txtBill.Location = new System.Drawing.Point(156, 23);
            this.txtBill.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBill.Name = "txtBill";
            this.txtBill.Size = new System.Drawing.Size(131, 25);
            this.txtBill.TabIndex = 17;
            this.txtBill.Text = "K09876543210987";
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Location = new System.Drawing.Point(20, 158);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(112, 17);
            this.label62.TabIndex = 16;
            this.label62.Text = "수표계좌일련번호";
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Location = new System.Drawing.Point(20, 125);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(86, 17);
            this.label61.TabIndex = 15;
            this.label61.Text = "수표발행일자";
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Location = new System.Drawing.Point(20, 59);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(60, 17);
            this.label60.TabIndex = 14;
            this.label60.Text = "권종코드";
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Location = new System.Drawing.Point(20, 92);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(60, 17);
            this.label59.TabIndex = 13;
            this.label59.Text = "수표금액";
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Location = new System.Drawing.Point(20, 26);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(133, 17);
            this.label58.TabIndex = 12;
            this.label58.Text = "수표정보(수표하단부)";
            // 
            // frmCheckView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 289);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.panMsg);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmCheckView";
            this.Text = "수표조회";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panMsg.ResumeLayout(false);
            this.panMsg.PerformLayout();
            this.panMain.ResumeLayout(false);
            this.panMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        public System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panMsg;
        private System.Windows.Forms.Label lblServiece;
        private System.Windows.Forms.Panel panMain;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtCredit;
        private System.Windows.Forms.TextBox txtDate;
        private System.Windows.Forms.TextBox txtAmt;
        private System.Windows.Forms.TextBox txtBill;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.ComboBox cboAmtGbn;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
    }
}