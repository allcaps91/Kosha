namespace ComLibB
{
    partial class frmSmsPay
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
            this.pan = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.gbJob = new System.Windows.Forms.GroupBox();
            this.txtTimeCnt = new System.Windows.Forms.TextBox();
            this.CboTimeCycle = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.TxtEDateSend = new System.Windows.Forms.TextBox();
            this.panel10 = new System.Windows.Forms.Panel();
            this.lblShow = new System.Windows.Forms.Label();
            this.panel11 = new System.Windows.Forms.Panel();
            this.lbl_P_STS = new System.Windows.Forms.Label();
            this.TmrFlow = new System.Windows.Forms.Timer(this.components);
            this.TmrAction = new System.Windows.Forms.Timer(this.components);
            this.pan.SuspendLayout();
            this.gbJob.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel11.SuspendLayout();
            this.SuspendLayout();
            // 
            // pan
            // 
            this.pan.BackColor = System.Drawing.Color.RoyalBlue;
            this.pan.Controls.Add(this.btnClose);
            this.pan.Controls.Add(this.lblTitleSub0);
            this.pan.Dock = System.Windows.Forms.DockStyle.Top;
            this.pan.Location = new System.Drawing.Point(0, 0);
            this.pan.Name = "pan";
            this.pan.Size = new System.Drawing.Size(318, 28);
            this.pan.TabIndex = 158;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(243, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(69, 23);
            this.btnClose.TabIndex = 151;
            this.btnClose.Text = "종료";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(2, 9);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(57, 12);
            this.lblTitleSub0.TabIndex = 22;
            this.lblTitleSub0.Text = "조회옵션";
            this.lblTitleSub0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gbJob
            // 
            this.gbJob.Controls.Add(this.txtTimeCnt);
            this.gbJob.Controls.Add(this.CboTimeCycle);
            this.gbJob.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gbJob.Location = new System.Drawing.Point(0, 30);
            this.gbJob.Name = "gbJob";
            this.gbJob.Size = new System.Drawing.Size(156, 50);
            this.gbJob.TabIndex = 159;
            this.gbJob.TabStop = false;
            this.gbJob.Text = "전송주기(단위 : 초)";
            // 
            // txtTimeCnt
            // 
            this.txtTimeCnt.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtTimeCnt.Location = new System.Drawing.Point(109, 16);
            this.txtTimeCnt.Name = "txtTimeCnt";
            this.txtTimeCnt.Size = new System.Drawing.Size(36, 27);
            this.txtTimeCnt.TabIndex = 151;
            this.txtTimeCnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // CboTimeCycle
            // 
            this.CboTimeCycle.DropDownWidth = 96;
            this.CboTimeCycle.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.CboTimeCycle.FormattingEnabled = true;
            this.CboTimeCycle.Location = new System.Drawing.Point(9, 16);
            this.CboTimeCycle.Name = "CboTimeCycle";
            this.CboTimeCycle.Size = new System.Drawing.Size(94, 28);
            this.CboTimeCycle.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.TxtEDateSend);
            this.groupBox2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox2.Location = new System.Drawing.Point(159, 30);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(153, 50);
            this.groupBox2.TabIndex = 160;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "마지막 전송시간";
            // 
            // TxtEDateSend
            // 
            this.TxtEDateSend.Location = new System.Drawing.Point(7, 17);
            this.TxtEDateSend.Name = "TxtEDateSend";
            this.TxtEDateSend.Size = new System.Drawing.Size(135, 25);
            this.TxtEDateSend.TabIndex = 151;
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.Silver;
            this.panel10.Controls.Add(this.lblShow);
            this.panel10.Location = new System.Drawing.Point(4, 86);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(308, 38);
            this.panel10.TabIndex = 163;
            // 
            // lblShow
            // 
            this.lblShow.AutoSize = true;
            this.lblShow.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblShow.Location = new System.Drawing.Point(77, 3);
            this.lblShow.Name = "lblShow";
            this.lblShow.Size = new System.Drawing.Size(161, 30);
            this.lblShow.TabIndex = 1;
            this.lblShow.Text = "메세지 대기중...";
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.panel11.Controls.Add(this.lbl_P_STS);
            this.panel11.Location = new System.Drawing.Point(4, 129);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(308, 38);
            this.panel11.TabIndex = 164;
            // 
            // lbl_P_STS
            // 
            this.lbl_P_STS.AutoSize = true;
            this.lbl_P_STS.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_P_STS.ForeColor = System.Drawing.Color.Blue;
            this.lbl_P_STS.Location = new System.Drawing.Point(86, 3);
            this.lbl_P_STS.Name = "lbl_P_STS";
            this.lbl_P_STS.Size = new System.Drawing.Size(111, 32);
            this.lbl_P_STS.TabIndex = 1;
            this.lbl_P_STS.Text = "문자구분";
            // 
            // TmrFlow
            // 
            this.TmrFlow.Interval = 1000;
            this.TmrFlow.Tick += new System.EventHandler(this.TmrFlow_Tick);
            // 
            // TmrAction
            // 
            this.TmrAction.Tick += new System.EventHandler(this.TmrAction_Tick);
            // 
            // frmSmsPay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(318, 173);
            this.Controls.Add(this.panel11);
            this.Controls.Add(this.panel10);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gbJob);
            this.Controls.Add(this.pan);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmSmsPay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "급여승인문자 자동전송(2021-08-20 10:00)";
            this.Load += new System.EventHandler(this.frmSmsPay_Load);
            this.pan.ResumeLayout(false);
            this.pan.PerformLayout();
            this.gbJob.ResumeLayout(false);
            this.gbJob.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pan;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.GroupBox gbJob;
        private System.Windows.Forms.TextBox txtTimeCnt;
        private System.Windows.Forms.ComboBox CboTimeCycle;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox TxtEDateSend;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Label lblShow;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Label lbl_P_STS;
        private System.Windows.Forms.Timer TmrFlow;
        private System.Windows.Forms.Timer TmrAction;
    }
}