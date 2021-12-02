namespace ComPmpaLibB
{
    partial class frmPmpaShowApproval
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
            this.components = new System.ComponentModel.Container();
            this.Timer_JanAmt = new System.Windows.Forms.Timer(this.components);
            this.Timer_Approval = new System.Windows.Forms.Timer(this.components);
            this.Timer_Cancel = new System.Windows.Forms.Timer(this.components);
            this.Timer_AutoExit = new System.Windows.Forms.Timer(this.components);
            this.pnlMsg = new System.Windows.Forms.Panel();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblMsg = new System.Windows.Forms.Label();
            this.pnlHead = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.pnlMsg.SuspendLayout();
            this.pnlHead.SuspendLayout();
            this.SuspendLayout();
            // 
            // Timer_JanAmt
            // 
            this.Timer_JanAmt.Interval = 1000;
            this.Timer_JanAmt.Tick += new System.EventHandler(this.Timer_JanAmt_Tick);
            // 
            // Timer_Approval
            // 
            this.Timer_Approval.Interval = 1000;
            this.Timer_Approval.Tick += new System.EventHandler(this.Timer_Approval_Tick);
            // 
            // Timer_Cancel
            // 
            this.Timer_Cancel.Interval = 1000;
            this.Timer_Cancel.Tick += new System.EventHandler(this.Timer_Cancel_Tick);
            // 
            // Timer_AutoExit
            // 
            this.Timer_AutoExit.Interval = 1000;
            this.Timer_AutoExit.Tick += new System.EventHandler(this.Timer_AutoExit_Tick);
            // 
            // pnlMsg
            // 
            this.pnlMsg.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.pnlMsg.Controls.Add(this.lblTime);
            this.pnlMsg.Controls.Add(this.lblMsg);
            this.pnlMsg.ForeColor = System.Drawing.Color.White;
            this.pnlMsg.Location = new System.Drawing.Point(36, 36);
            this.pnlMsg.Name = "pnlMsg";
            this.pnlMsg.Size = new System.Drawing.Size(304, 46);
            this.pnlMsg.TabIndex = 0;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTime.ForeColor = System.Drawing.Color.Yellow;
            this.lblTime.Location = new System.Drawing.Point(271, 16);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(12, 12);
            this.lblTime.TabIndex = 1;
            this.lblTime.Text = "1";
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Location = new System.Drawing.Point(23, 16);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(69, 12);
            this.lblMsg.TabIndex = 0;
            this.lblMsg.Text = "승인 작업중";
            // 
            // pnlHead
            // 
            this.pnlHead.BackColor = System.Drawing.Color.RoyalBlue;
            this.pnlHead.Controls.Add(this.label7);
            this.pnlHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHead.ForeColor = System.Drawing.Color.White;
            this.pnlHead.ImeMode = System.Windows.Forms.ImeMode.KatakanaHalf;
            this.pnlHead.Location = new System.Drawing.Point(0, 0);
            this.pnlHead.Name = "pnlHead";
            this.pnlHead.Size = new System.Drawing.Size(364, 30);
            this.pnlHead.TabIndex = 198;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(14, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(111, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "의료급여 승인/취소";
            // 
            // frmPmpaShowApproval
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 98);
            this.ControlBox = false;
            this.Controls.Add(this.pnlHead);
            this.Controls.Add(this.pnlMsg);
            this.Name = "frmPmpaShowApproval";
            this.Text = " ";
            this.pnlMsg.ResumeLayout(false);
            this.pnlMsg.PerformLayout();
            this.pnlHead.ResumeLayout(false);
            this.pnlHead.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer Timer_JanAmt;
        private System.Windows.Forms.Timer Timer_Approval;
        private System.Windows.Forms.Timer Timer_Cancel;
        private System.Windows.Forms.Timer Timer_AutoExit;
        private System.Windows.Forms.Panel pnlMsg;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Panel pnlHead;
        private System.Windows.Forms.Label label7;
    }
}