namespace ComEmrBase
{
    partial class frmEmrBaseProgressImage
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
            this.webImage = new System.Windows.Forms.WebBrowser();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnSaveImag = new System.Windows.Forms.Button();
            this.txtChartTime = new System.Windows.Forms.MaskedTextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.dtpChartDate = new System.Windows.Forms.DateTimePicker();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panTitle.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // webImage
            // 
            this.webImage.Dock = System.Windows.Forms.DockStyle.Left;
            this.webImage.Location = new System.Drawing.Point(10, 39);
            this.webImage.MinimumSize = new System.Drawing.Size(20, 20);
            this.webImage.Name = "webImage";
            this.webImage.Size = new System.Drawing.Size(697, 739);
            this.webImage.TabIndex = 33;
            // 
            // panTitle
            // 
            this.panTitle.Controls.Add(this.btnSaveImag);
            this.panTitle.Controls.Add(this.txtChartTime);
            this.panTitle.Controls.Add(this.Label1);
            this.panTitle.Controls.Add(this.dtpChartDate);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(708, 35);
            this.panTitle.TabIndex = 5;
            // 
            // btnSaveImag
            // 
            this.btnSaveImag.Location = new System.Drawing.Point(275, 3);
            this.btnSaveImag.Name = "btnSaveImag";
            this.btnSaveImag.Size = new System.Drawing.Size(69, 28);
            this.btnSaveImag.TabIndex = 27;
            this.btnSaveImag.Text = "저  장";
            this.btnSaveImag.UseVisualStyleBackColor = true;
            this.btnSaveImag.Visible = false;
            this.btnSaveImag.Click += new System.EventHandler(this.btnSaveImag_Click);
            // 
            // txtChartTime
            // 
            this.txtChartTime.Location = new System.Drawing.Point(174, 5);
            this.txtChartTime.Mask = "90:00";
            this.txtChartTime.Name = "txtChartTime";
            this.txtChartTime.Size = new System.Drawing.Size(40, 25);
            this.txtChartTime.TabIndex = 20;
            this.txtChartTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtChartTime.ValidatingType = typeof(System.DateTime);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(7, 9);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(60, 17);
            this.Label1.TabIndex = 19;
            this.Label1.Text = "작성일자";
            // 
            // dtpChartDate
            // 
            this.dtpChartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpChartDate.Location = new System.Drawing.Point(67, 5);
            this.dtpChartDate.Name = "dtpChartDate";
            this.dtpChartDate.Size = new System.Drawing.Size(107, 25);
            this.dtpChartDate.TabIndex = 18;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.panTitle);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(710, 39);
            this.panel3.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 39);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 755);
            this.panel1.TabIndex = 34;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(10, 778);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(700, 16);
            this.panel2.TabIndex = 35;
            // 
            // frmEmrBaseProgressImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(710, 794);
            this.Controls.Add(this.webImage);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmEmrBaseProgressImage";
            this.Text = "frmEmrBaseProgressImage";
            this.Load += new System.EventHandler(this.FrmEmrBaseProgressImage_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.WebBrowser webImage;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnSaveImag;
        internal System.Windows.Forms.MaskedTextBox txtChartTime;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.DateTimePicker dtpChartDate;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}