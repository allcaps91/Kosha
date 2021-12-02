namespace ComEmrBase
{
    partial class frmEmrBaseEmrChartOld
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
            this.panTopMenu = new mtsPanel15.mPanel();
            this.mbtnPrint = new System.Windows.Forms.Button();
            this.lblServerDate = new System.Windows.Forms.Label();
            this.lblPrntYn = new System.Windows.Forms.Label();
            this.txtMedFrTime = new System.Windows.Forms.ComboBox();
            this.mbtnTime = new System.Windows.Forms.Button();
            this.mbtnExit = new System.Windows.Forms.Button();
            this.mbtnDelete = new System.Windows.Forms.Button();
            this.mbtnSave = new System.Windows.Forms.Button();
            this.lblChartTime = new System.Windows.Forms.Label();
            this.lblChartDate = new System.Windows.Forms.Label();
            this.dtMedFrDate = new System.Windows.Forms.DateTimePicker();
            this.webEMR = new System.Windows.Forms.WebBrowser();
            this.panTopMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTopMenu
            // 
            this.panTopMenu.BackColor = System.Drawing.Color.White;
            this.panTopMenu.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTopMenu.Controls.Add(this.mbtnPrint);
            this.panTopMenu.Controls.Add(this.lblServerDate);
            this.panTopMenu.Controls.Add(this.lblPrntYn);
            this.panTopMenu.Controls.Add(this.txtMedFrTime);
            this.panTopMenu.Controls.Add(this.mbtnTime);
            this.panTopMenu.Controls.Add(this.mbtnExit);
            this.panTopMenu.Controls.Add(this.mbtnDelete);
            this.panTopMenu.Controls.Add(this.mbtnSave);
            this.panTopMenu.Controls.Add(this.lblChartTime);
            this.panTopMenu.Controls.Add(this.lblChartDate);
            this.panTopMenu.Controls.Add(this.dtMedFrDate);
            this.panTopMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTopMenu.Location = new System.Drawing.Point(0, 0);
            this.panTopMenu.Name = "panTopMenu";
            this.panTopMenu.Size = new System.Drawing.Size(832, 40);
            this.panTopMenu.TabIndex = 107;
            this.panTopMenu.TabStop = true;
            // 
            // mbtnPrint
            // 
            this.mbtnPrint.Location = new System.Drawing.Point(483, 3);
            this.mbtnPrint.Name = "mbtnPrint";
            this.mbtnPrint.Size = new System.Drawing.Size(64, 28);
            this.mbtnPrint.TabIndex = 40;
            this.mbtnPrint.Text = "출력";
            this.mbtnPrint.UseVisualStyleBackColor = true;
            this.mbtnPrint.Click += new System.EventHandler(this.mbtnPrint_Click);
            // 
            // lblServerDate
            // 
            this.lblServerDate.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblServerDate.ForeColor = System.Drawing.Color.Blue;
            this.lblServerDate.Location = new System.Drawing.Point(675, 3);
            this.lblServerDate.Name = "lblServerDate";
            this.lblServerDate.Size = new System.Drawing.Size(143, 31);
            this.lblServerDate.TabIndex = 39;
            this.lblServerDate.Text = "9999-9999";
            this.lblServerDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblServerDate.Visible = false;
            // 
            // lblPrntYn
            // 
            this.lblPrntYn.BackColor = System.Drawing.Color.Red;
            this.lblPrntYn.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblPrntYn.ForeColor = System.Drawing.Color.White;
            this.lblPrntYn.Location = new System.Drawing.Point(625, 5);
            this.lblPrntYn.Name = "lblPrntYn";
            this.lblPrntYn.Size = new System.Drawing.Size(44, 25);
            this.lblPrntYn.TabIndex = 38;
            this.lblPrntYn.Text = "출력";
            this.lblPrntYn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblPrntYn.Visible = false;
            // 
            // txtMedFrTime
            // 
            this.txtMedFrTime.FormattingEnabled = true;
            this.txtMedFrTime.Location = new System.Drawing.Point(222, 5);
            this.txtMedFrTime.Name = "txtMedFrTime";
            this.txtMedFrTime.Size = new System.Drawing.Size(57, 25);
            this.txtMedFrTime.TabIndex = 37;
            this.txtMedFrTime.Text = "00:00";
            // 
            // mbtnTime
            // 
            this.mbtnTime.Location = new System.Drawing.Point(282, 3);
            this.mbtnTime.Name = "mbtnTime";
            this.mbtnTime.Size = new System.Drawing.Size(27, 28);
            this.mbtnTime.TabIndex = 36;
            this.mbtnTime.Text = "T";
            this.mbtnTime.UseVisualStyleBackColor = true;
            this.mbtnTime.Click += new System.EventHandler(this.mbtnTime_Click);
            // 
            // mbtnExit
            // 
            this.mbtnExit.Location = new System.Drawing.Point(555, 3);
            this.mbtnExit.Name = "mbtnExit";
            this.mbtnExit.Size = new System.Drawing.Size(64, 28);
            this.mbtnExit.TabIndex = 35;
            this.mbtnExit.Text = "닫  기";
            this.mbtnExit.UseVisualStyleBackColor = true;
            this.mbtnExit.Click += new System.EventHandler(this.mbtnExit_Click);
            // 
            // mbtnDelete
            // 
            this.mbtnDelete.Location = new System.Drawing.Point(411, 3);
            this.mbtnDelete.Name = "mbtnDelete";
            this.mbtnDelete.Size = new System.Drawing.Size(64, 28);
            this.mbtnDelete.TabIndex = 34;
            this.mbtnDelete.Text = "삭  제";
            this.mbtnDelete.UseVisualStyleBackColor = true;
            this.mbtnDelete.Visible = false;
            this.mbtnDelete.Click += new System.EventHandler(this.mbtnDelete_Click);
            // 
            // mbtnSave
            // 
            this.mbtnSave.Location = new System.Drawing.Point(339, 3);
            this.mbtnSave.Name = "mbtnSave";
            this.mbtnSave.Size = new System.Drawing.Size(64, 28);
            this.mbtnSave.TabIndex = 33;
            this.mbtnSave.Text = "저  장";
            this.mbtnSave.UseVisualStyleBackColor = true;
            this.mbtnSave.Visible = false;
            this.mbtnSave.Click += new System.EventHandler(this.mbtnSave_Click);
            // 
            // lblChartTime
            // 
            this.lblChartTime.AutoSize = true;
            this.lblChartTime.Location = new System.Drawing.Point(188, 9);
            this.lblChartTime.Name = "lblChartTime";
            this.lblChartTime.Size = new System.Drawing.Size(34, 17);
            this.lblChartTime.TabIndex = 30;
            this.lblChartTime.Text = "시간";
            // 
            // lblChartDate
            // 
            this.lblChartDate.AutoSize = true;
            this.lblChartDate.Location = new System.Drawing.Point(9, 9);
            this.lblChartDate.Name = "lblChartDate";
            this.lblChartDate.Size = new System.Drawing.Size(60, 17);
            this.lblChartDate.TabIndex = 29;
            this.lblChartDate.Text = "작성일자";
            // 
            // dtMedFrDate
            // 
            this.dtMedFrDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtMedFrDate.Location = new System.Drawing.Point(69, 5);
            this.dtMedFrDate.Name = "dtMedFrDate";
            this.dtMedFrDate.Size = new System.Drawing.Size(105, 25);
            this.dtMedFrDate.TabIndex = 28;
            // 
            // webEMR
            // 
            this.webEMR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webEMR.Location = new System.Drawing.Point(0, 40);
            this.webEMR.MinimumSize = new System.Drawing.Size(20, 20);
            this.webEMR.Name = "webEMR";
            this.webEMR.Size = new System.Drawing.Size(832, 922);
            this.webEMR.TabIndex = 108;
            // 
            // frmEmrBaseEmrChartOld
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(832, 962);
            this.Controls.Add(this.webEMR);
            this.Controls.Add(this.panTopMenu);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmEmrBaseEmrChartOld";
            this.Text = "정형화 서식지 - Old";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmEmrBaseEmrChartOld_FormClosed);
            this.Load += new System.EventHandler(this.frmEmrBaseEmrChartOld_Load);
            this.panTopMenu.ResumeLayout(false);
            this.panTopMenu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private mtsPanel15.mPanel panTopMenu;
        public System.Windows.Forms.Label lblServerDate;
        public System.Windows.Forms.Label lblPrntYn;
        public System.Windows.Forms.ComboBox txtMedFrTime;
        public System.Windows.Forms.Button mbtnTime;
        public System.Windows.Forms.Button mbtnExit;
        public System.Windows.Forms.Button mbtnDelete;
        public System.Windows.Forms.Button mbtnSave;
        public System.Windows.Forms.Label lblChartTime;
        public System.Windows.Forms.Label lblChartDate;
        public System.Windows.Forms.DateTimePicker dtMedFrDate;
        private System.Windows.Forms.WebBrowser webEMR;
        public System.Windows.Forms.Button mbtnPrint;
    }
}