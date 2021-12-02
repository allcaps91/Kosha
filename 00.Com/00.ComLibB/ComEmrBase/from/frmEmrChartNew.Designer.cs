namespace ComEmrBase
{
    partial class frmEmrChartNew
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.panCoading = new mtsPanel15.mPanel();
            this.panCoading.SuspendLayout();
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
            this.panChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panChart.Location = new System.Drawing.Point(850, 34);
            this.panChart.Name = "panChart";
            this.panChart.Size = new System.Drawing.Size(0, 728);
            this.panChart.TabIndex = 107;
            this.panChart.TabStop = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.White;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Font = new System.Drawing.Font("굴림체", 11F);
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(850, 728);
            this.richTextBox1.TabIndex = 110;
            this.richTextBox1.Text = "";
            this.richTextBox1.Visible = false;
            // 
            // panCoading
            // 
            this.panCoading.Controls.Add(this.richTextBox1);
            this.panCoading.Dock = System.Windows.Forms.DockStyle.Left;
            this.panCoading.Location = new System.Drawing.Point(0, 34);
            this.panCoading.Name = "panCoading";
            this.panCoading.Size = new System.Drawing.Size(850, 728);
            this.panCoading.TabIndex = 112;
            this.panCoading.TabStop = true;
            this.panCoading.Visible = false;
            // 
            // frmEmrChartNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(621, 762);
            this.Controls.Add(this.panChart);
            this.Controls.Add(this.panCoading);
            this.Controls.Add(this.panTopMenu);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmEmrChartNew";
            this.Text = "frmEmrChartNew";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmEmrChartNew_FormClosed);
            this.Load += new System.EventHandler(this.frmEmrChartNew_Load);
            this.panCoading.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private mtsPanel15.mPanel panTopMenu;
        private mtsPanel15.mPanel panChart;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private mtsPanel15.mPanel panCoading;
    }
}