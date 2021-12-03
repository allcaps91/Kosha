namespace ComNurLibB
{
    partial class frmAgentSend
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAgentSend));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.mnuExit = new System.Windows.Forms.ToolStripButton();
            this.mnuSendNow = new System.Windows.Forms.ToolStripDropDownButton();
            this.mnuSendNow_01 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSendNow_02 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSendNow_03 = new System.Windows.Forms.ToolStripMenuItem();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dtpEDate = new System.Windows.Forms.DateTimePicker();
            this.dtpSDate = new System.Windows.Forms.DateTimePicker();
            this.chk1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblTimer = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuExit,
            this.mnuSendNow});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(367, 25);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // mnuExit
            // 
            this.mnuExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.mnuExit.Image = ((System.Drawing.Image)(resources.GetObject("mnuExit.Image")));
            this.mnuExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(35, 22);
            this.mnuExit.Text = "종료";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuSendNow
            // 
            this.mnuSendNow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.mnuSendNow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSendNow_01,
            this.mnuSendNow_02,
            this.mnuSendNow_03});
            this.mnuSendNow.Image = ((System.Drawing.Image)(resources.GetObject("mnuSendNow.Image")));
            this.mnuSendNow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuSendNow.Name = "mnuSendNow";
            this.mnuSendNow.Size = new System.Drawing.Size(102, 22);
            this.mnuSendNow.Text = "NEDIS즉시전송";
            // 
            // mnuSendNow_01
            // 
            this.mnuSendNow_01.Name = "mnuSendNow_01";
            this.mnuSendNow_01.Size = new System.Drawing.Size(146, 22);
            this.mnuSendNow_01.Text = "응급환자처치";
            this.mnuSendNow_01.Click += new System.EventHandler(this.mnuSendNow_01_Click);
            // 
            // mnuSendNow_02
            // 
            this.mnuSendNow_02.Name = "mnuSendNow_02";
            this.mnuSendNow_02.Size = new System.Drawing.Size(146, 22);
            this.mnuSendNow_02.Text = "응급환자진단";
            this.mnuSendNow_02.Click += new System.EventHandler(this.mnuSendNow_02_Click);
            // 
            // mnuSendNow_03
            // 
            this.mnuSendNow_03.Name = "mnuSendNow_03";
            this.mnuSendNow_03.Size = new System.Drawing.Size(146, 22);
            this.mnuSendNow_03.Text = "응급환자퇴원";
            this.mnuSendNow_03.Click += new System.EventHandler(this.mnuSendNow_03_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.lblTimer);
            this.panel3.Controls.Add(this.dtpEDate);
            this.panel3.Controls.Add(this.dtpSDate);
            this.panel3.Controls.Add(this.chk1);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 25);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(367, 143);
            this.panel3.TabIndex = 22;
            // 
            // dtpEDate
            // 
            this.dtpEDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEDate.Location = new System.Drawing.Point(231, 95);
            this.dtpEDate.Name = "dtpEDate";
            this.dtpEDate.Size = new System.Drawing.Size(102, 21);
            this.dtpEDate.TabIndex = 3;
            // 
            // dtpSDate
            // 
            this.dtpSDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpSDate.Location = new System.Drawing.Point(123, 95);
            this.dtpSDate.Name = "dtpSDate";
            this.dtpSDate.Size = new System.Drawing.Size(102, 21);
            this.dtpSDate.TabIndex = 3;
            // 
            // chk1
            // 
            this.chk1.AutoSize = true;
            this.chk1.Location = new System.Drawing.Point(24, 97);
            this.chk1.Name = "chk1";
            this.chk1.Size = new System.Drawing.Size(60, 16);
            this.chk1.TabIndex = 2;
            this.chk1.Text = "테스트";
            this.chk1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LemonChiffon;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label1.Location = new System.Drawing.Point(24, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(310, 60);
            this.label1.TabIndex = 0;
            this.label1.Text = "응급의료 자료 전송중...!!!";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblTimer
            // 
            this.lblTimer.AutoSize = true;
            this.lblTimer.BackColor = System.Drawing.Color.LemonChiffon;
            this.lblTimer.ForeColor = System.Drawing.Color.Red;
            this.lblTimer.Location = new System.Drawing.Point(40, 47);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(11, 12);
            this.lblTimer.TabIndex = 4;
            this.lblTimer.Text = "0";
            // 
            // frmAgentSend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 168);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.toolStrip1);
            this.Name = "frmAgentSend";
            this.Text = "frmAgentSend";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmAgentSend_FormClosed);
            this.Load += new System.EventHandler(this.frmAgentSend_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton mnuExit;
        private System.Windows.Forms.ToolStripDropDownButton mnuSendNow;
        private System.Windows.Forms.ToolStripMenuItem mnuSendNow_01;
        private System.Windows.Forms.ToolStripMenuItem mnuSendNow_02;
        private System.Windows.Forms.ToolStripMenuItem mnuSendNow_03;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox chk1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpEDate;
        private System.Windows.Forms.DateTimePicker dtpSDate;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblTimer;
    }
}