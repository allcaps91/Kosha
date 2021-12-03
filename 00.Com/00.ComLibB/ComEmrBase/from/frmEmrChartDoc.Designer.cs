namespace ComEmrBase
{
    partial class frmEmrChartDoc
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
            this.mbtnSaveTemp = new System.Windows.Forms.Button();
            this.mbtnExit = new System.Windows.Forms.Button();
            this.mbtnDelete = new System.Windows.Forms.Button();
            this.mbtnSave = new System.Windows.Forms.Button();
            this.mbtnPrint = new System.Windows.Forms.Button();
            this.mbtnClear = new System.Windows.Forms.Button();
            this.lblChartDate = new System.Windows.Forms.Label();
            this.dtMedFrDate = new System.Windows.Forms.DateTimePicker();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.panTopMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTopMenu
            // 
            this.panTopMenu.AutoScroll = true;
            this.panTopMenu.BackColor = System.Drawing.Color.White;
            this.panTopMenu.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTopMenu.Controls.Add(this.mbtnSaveTemp);
            this.panTopMenu.Controls.Add(this.mbtnExit);
            this.panTopMenu.Controls.Add(this.mbtnDelete);
            this.panTopMenu.Controls.Add(this.mbtnSave);
            this.panTopMenu.Controls.Add(this.mbtnPrint);
            this.panTopMenu.Controls.Add(this.mbtnClear);
            this.panTopMenu.Controls.Add(this.lblChartDate);
            this.panTopMenu.Controls.Add(this.dtMedFrDate);
            this.panTopMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTopMenu.Location = new System.Drawing.Point(0, 0);
            this.panTopMenu.Name = "panTopMenu";
            this.panTopMenu.Size = new System.Drawing.Size(686, 34);
            this.panTopMenu.TabIndex = 106;
            this.panTopMenu.TabStop = true;
            // 
            // mbtnSaveTemp
            // 
            this.mbtnSaveTemp.Location = new System.Drawing.Point(342, 1);
            this.mbtnSaveTemp.Name = "mbtnSaveTemp";
            this.mbtnSaveTemp.Size = new System.Drawing.Size(74, 28);
            this.mbtnSaveTemp.TabIndex = 33;
            this.mbtnSaveTemp.Text = "임시저장";
            this.mbtnSaveTemp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.mbtnSaveTemp.UseVisualStyleBackColor = true;
            this.mbtnSaveTemp.Visible = false;
            // 
            // mbtnExit
            // 
            this.mbtnExit.Location = new System.Drawing.Point(613, 1);
            this.mbtnExit.Name = "mbtnExit";
            this.mbtnExit.Size = new System.Drawing.Size(62, 28);
            this.mbtnExit.TabIndex = 32;
            this.mbtnExit.Text = "닫  기";
            this.mbtnExit.UseVisualStyleBackColor = true;
            this.mbtnExit.Visible = false;
            // 
            // mbtnDelete
            // 
            this.mbtnDelete.Location = new System.Drawing.Point(502, 1);
            this.mbtnDelete.Name = "mbtnDelete";
            this.mbtnDelete.Size = new System.Drawing.Size(62, 28);
            this.mbtnDelete.TabIndex = 31;
            this.mbtnDelete.Text = "삭  제";
            this.mbtnDelete.UseVisualStyleBackColor = true;
            this.mbtnDelete.Visible = false;
            // 
            // mbtnSave
            // 
            this.mbtnSave.Location = new System.Drawing.Point(422, 1);
            this.mbtnSave.Name = "mbtnSave";
            this.mbtnSave.Size = new System.Drawing.Size(74, 28);
            this.mbtnSave.TabIndex = 30;
            this.mbtnSave.Text = "인증저장";
            this.mbtnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.mbtnSave.UseVisualStyleBackColor = true;
            this.mbtnSave.Visible = false;
            // 
            // mbtnPrint
            // 
            this.mbtnPrint.Location = new System.Drawing.Point(206, 1);
            this.mbtnPrint.Name = "mbtnPrint";
            this.mbtnPrint.Size = new System.Drawing.Size(62, 28);
            this.mbtnPrint.TabIndex = 29;
            this.mbtnPrint.Text = "출  력";
            this.mbtnPrint.UseVisualStyleBackColor = true;
            this.mbtnPrint.Visible = false;
            // 
            // mbtnClear
            // 
            this.mbtnClear.Location = new System.Drawing.Point(274, 1);
            this.mbtnClear.Name = "mbtnClear";
            this.mbtnClear.Size = new System.Drawing.Size(62, 28);
            this.mbtnClear.TabIndex = 28;
            this.mbtnClear.Text = "Clear";
            this.mbtnClear.UseVisualStyleBackColor = true;
            this.mbtnClear.Visible = false;
            // 
            // lblChartDate
            // 
            this.lblChartDate.AutoSize = true;
            this.lblChartDate.Location = new System.Drawing.Point(7, 9);
            this.lblChartDate.Name = "lblChartDate";
            this.lblChartDate.Size = new System.Drawing.Size(53, 12);
            this.lblChartDate.TabIndex = 17;
            this.lblChartDate.Text = "작성일자";
            // 
            // dtMedFrDate
            // 
            this.dtMedFrDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtMedFrDate.Location = new System.Drawing.Point(60, 5);
            this.dtMedFrDate.Name = "dtMedFrDate";
            this.dtMedFrDate.Size = new System.Drawing.Size(98, 21);
            this.dtMedFrDate.TabIndex = 16;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 34);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(686, 756);
            this.webBrowser1.TabIndex = 107;
            // 
            // frmEmrChartDoc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(686, 790);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.panTopMenu);
            this.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "frmEmrChartDoc";
            this.Text = "frmEmrChartDoc";
            this.Load += new System.EventHandler(this.frmEmrChartDoc_Load);
            this.panTopMenu.ResumeLayout(false);
            this.panTopMenu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private mtsPanel15.mPanel panTopMenu;
        public System.Windows.Forms.Button mbtnSaveTemp;
        public System.Windows.Forms.Button mbtnExit;
        public System.Windows.Forms.Button mbtnDelete;
        public System.Windows.Forms.Button mbtnSave;
        public System.Windows.Forms.Button mbtnPrint;
        public System.Windows.Forms.Button mbtnClear;
        public System.Windows.Forms.Label lblChartDate;
        public System.Windows.Forms.DateTimePicker dtMedFrDate;
        private System.Windows.Forms.WebBrowser webBrowser1;
    }
}