namespace ComEmrBase
{
    partial class frmEmrBasePictureView
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
            this.panChart = new mtsPanel15.mPanel();
            this.wbImg = new System.Windows.Forms.WebBrowser();
            this.panModify = new System.Windows.Forms.Panel();
            this.txtChartTime = new System.Windows.Forms.MaskedTextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.dtpChartDate = new System.Windows.Forms.DateTimePicker();
            this.btnSave = new System.Windows.Forms.Button();
            this.mbtnDelete = new System.Windows.Forms.Button();
            this.panChart.SuspendLayout();
            this.panModify.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTopMenu
            // 
            this.panTopMenu.AutoScroll = true;
            this.panTopMenu.BackColor = System.Drawing.Color.White;
            this.panTopMenu.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTopMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTopMenu.Location = new System.Drawing.Point(0, 0);
            this.panTopMenu.Name = "panTopMenu";
            this.panTopMenu.Size = new System.Drawing.Size(621, 34);
            this.panTopMenu.TabIndex = 106;
            this.panTopMenu.TabStop = true;
            // 
            // panChart
            // 
            this.panChart.AutoScroll = true;
            this.panChart.BackColor = System.Drawing.Color.White;
            this.panChart.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panChart.Controls.Add(this.wbImg);
            this.panChart.Controls.Add(this.panModify);
            this.panChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panChart.Location = new System.Drawing.Point(0, 34);
            this.panChart.Name = "panChart";
            this.panChart.Size = new System.Drawing.Size(621, 478);
            this.panChart.TabIndex = 108;
            this.panChart.TabStop = true;
            // 
            // wbImg
            // 
            this.wbImg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbImg.Location = new System.Drawing.Point(0, 34);
            this.wbImg.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbImg.Name = "wbImg";
            this.wbImg.Size = new System.Drawing.Size(617, 440);
            this.wbImg.TabIndex = 13;
            // 
            // panModify
            // 
            this.panModify.Controls.Add(this.mbtnDelete);
            this.panModify.Controls.Add(this.txtChartTime);
            this.panModify.Controls.Add(this.Label1);
            this.panModify.Controls.Add(this.dtpChartDate);
            this.panModify.Controls.Add(this.btnSave);
            this.panModify.Dock = System.Windows.Forms.DockStyle.Top;
            this.panModify.Location = new System.Drawing.Point(0, 0);
            this.panModify.Name = "panModify";
            this.panModify.Size = new System.Drawing.Size(617, 34);
            this.panModify.TabIndex = 0;
            // 
            // txtChartTime
            // 
            this.txtChartTime.Location = new System.Drawing.Point(174, 5);
            this.txtChartTime.Mask = "90:00";
            this.txtChartTime.Name = "txtChartTime";
            this.txtChartTime.Size = new System.Drawing.Size(40, 23);
            this.txtChartTime.TabIndex = 23;
            this.txtChartTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtChartTime.ValidatingType = typeof(System.DateTime);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(7, 9);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(55, 15);
            this.Label1.TabIndex = 22;
            this.Label1.Text = "작성일자";
            // 
            // dtpChartDate
            // 
            this.dtpChartDate.Enabled = false;
            this.dtpChartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpChartDate.Location = new System.Drawing.Point(67, 5);
            this.dtpChartDate.Name = "dtpChartDate";
            this.dtpChartDate.Size = new System.Drawing.Size(107, 23);
            this.dtpChartDate.TabIndex = 21;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(220, 1);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 30);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // mbtnDelete
            // 
            this.mbtnDelete.Location = new System.Drawing.Point(293, 1);
            this.mbtnDelete.Name = "mbtnDelete";
            this.mbtnDelete.Size = new System.Drawing.Size(72, 30);
            this.mbtnDelete.TabIndex = 35;
            this.mbtnDelete.Text = "삭  제";
            this.mbtnDelete.UseVisualStyleBackColor = true;
            this.mbtnDelete.Click += new System.EventHandler(this.MbtnDelete_Click);
            // 
            // frmEmrBasePictureView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(621, 512);
            this.Controls.Add(this.panChart);
            this.Controls.Add(this.panTopMenu);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmEmrBasePictureView";
            this.Text = "frmEmrBasePictureView";
            this.Load += new System.EventHandler(this.frmEmrBasePictureView_Load);
            this.panChart.ResumeLayout(false);
            this.panModify.ResumeLayout(false);
            this.panModify.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private mtsPanel15.mPanel panTopMenu;
        private mtsPanel15.mPanel panChart;
        private System.Windows.Forms.Panel panModify;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.WebBrowser wbImg;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.DateTimePicker dtpChartDate;
        internal System.Windows.Forms.MaskedTextBox txtChartTime;
        public System.Windows.Forms.Button mbtnDelete;
    }
}