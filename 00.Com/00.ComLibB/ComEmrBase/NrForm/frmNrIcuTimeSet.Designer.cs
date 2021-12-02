namespace ComEmrBase
{
    partial class frmNrIcuTimeSet
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
            this.txtSTime = new System.Windows.Forms.ComboBox();
            this.lblChartTime = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTime = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.mbtnSave = new System.Windows.Forms.Button();
            this.mbtnExit = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSTime1 = new System.Windows.Forms.ComboBox();
            this.txtETime1 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtETime = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // txtSTime
            // 
            this.txtSTime.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtSTime.FormattingEnabled = true;
            this.txtSTime.Location = new System.Drawing.Point(67, 12);
            this.txtSTime.Name = "txtSTime";
            this.txtSTime.Size = new System.Drawing.Size(42, 20);
            this.txtSTime.TabIndex = 26;
            this.txtSTime.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSTime_KeyDown);
            // 
            // lblChartTime
            // 
            this.lblChartTime.AutoSize = true;
            this.lblChartTime.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblChartTime.Location = new System.Drawing.Point(10, 16);
            this.lblChartTime.Name = "lblChartTime";
            this.lblChartTime.Size = new System.Drawing.Size(57, 12);
            this.lblChartTime.TabIndex = 25;
            this.lblChartTime.Text = "시작시간";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(10, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 12);
            this.label1.TabIndex = 27;
            this.label1.Text = "종료시간";
            // 
            // txtTime
            // 
            this.txtTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtTime.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtTime.FormattingEnabled = true;
            this.txtTime.Items.AddRange(new object[] {
            "1",
            "5",
            "10",
            "15",
            "30",
            "60",
            "120"});
            this.txtTime.Location = new System.Drawing.Point(67, 68);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(94, 20);
            this.txtTime.TabIndex = 30;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(10, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 12);
            this.label2.TabIndex = 29;
            this.label2.Text = "설정";
            // 
            // mbtnSave
            // 
            this.mbtnSave.Location = new System.Drawing.Point(10, 94);
            this.mbtnSave.Name = "mbtnSave";
            this.mbtnSave.Size = new System.Drawing.Size(68, 30);
            this.mbtnSave.TabIndex = 31;
            this.mbtnSave.Text = "저 장";
            this.mbtnSave.UseVisualStyleBackColor = true;
            this.mbtnSave.Click += new System.EventHandler(this.mbtnSave_Click);
            // 
            // mbtnExit
            // 
            this.mbtnExit.Location = new System.Drawing.Point(93, 94);
            this.mbtnExit.Name = "mbtnExit";
            this.mbtnExit.Size = new System.Drawing.Size(68, 30);
            this.mbtnExit.TabIndex = 32;
            this.mbtnExit.Text = "닫 기";
            this.mbtnExit.UseVisualStyleBackColor = true;
            this.mbtnExit.Click += new System.EventHandler(this.mbtnExit_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(109, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(10, 12);
            this.label3.TabIndex = 33;
            this.label3.Text = ":";
            // 
            // txtSTime1
            // 
            this.txtSTime1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtSTime1.FormattingEnabled = true;
            this.txtSTime1.Location = new System.Drawing.Point(119, 12);
            this.txtSTime1.Name = "txtSTime1";
            this.txtSTime1.Size = new System.Drawing.Size(42, 20);
            this.txtSTime1.TabIndex = 34;
            // 
            // txtETime1
            // 
            this.txtETime1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtETime1.FormattingEnabled = true;
            this.txtETime1.Location = new System.Drawing.Point(119, 38);
            this.txtETime1.Name = "txtETime1";
            this.txtETime1.Size = new System.Drawing.Size(42, 20);
            this.txtETime1.TabIndex = 37;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(109, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(10, 12);
            this.label4.TabIndex = 36;
            this.label4.Text = ":";
            // 
            // txtETime
            // 
            this.txtETime.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtETime.FormattingEnabled = true;
            this.txtETime.Location = new System.Drawing.Point(67, 38);
            this.txtETime.Name = "txtETime";
            this.txtETime.Size = new System.Drawing.Size(42, 20);
            this.txtETime.TabIndex = 35;
            this.txtETime.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtETime_KeyDown);
            // 
            // frmNrIcuTimeSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(175, 130);
            this.ControlBox = false;
            this.Controls.Add(this.txtETime1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtETime);
            this.Controls.Add(this.txtSTime1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.mbtnExit);
            this.Controls.Add(this.mbtnSave);
            this.Controls.Add(this.txtTime);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSTime);
            this.Controls.Add(this.lblChartTime);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmNrIcuTimeSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "구간 시간 설정";
            this.Load += new System.EventHandler(this.frmNrIcuTimeSet_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ComboBox txtSTime;
        public System.Windows.Forms.Label lblChartTime;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox txtTime;
        public System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button mbtnSave;
        private System.Windows.Forms.Button mbtnExit;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.ComboBox txtSTime1;
        public System.Windows.Forms.ComboBox txtETime1;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.ComboBox txtETime;
    }
}