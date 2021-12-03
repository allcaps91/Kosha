namespace ComEmrBase
{
    partial class frmEmrProgressView
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
            this.panChart = new System.Windows.Forms.Panel();
            this.txtProgress = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.chkSortAs2 = new System.Windows.Forms.CheckBox();
            this.dtpEEDATE = new System.Windows.Forms.DateTimePicker();
            this.dtpSSDATE = new System.Windows.Forms.DateTimePicker();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panChart.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panChart
            // 
            this.panChart.Controls.Add(this.txtProgress);
            this.panChart.Controls.Add(this.panel1);
            this.panChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panChart.Location = new System.Drawing.Point(0, 0);
            this.panChart.Name = "panChart";
            this.panChart.Size = new System.Drawing.Size(834, 799);
            this.panChart.TabIndex = 1;
            // 
            // txtProgress
            // 
            this.txtProgress.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtProgress.Location = new System.Drawing.Point(0, 35);
            this.txtProgress.Name = "txtProgress";
            this.txtProgress.Size = new System.Drawing.Size(834, 764);
            this.txtProgress.TabIndex = 1;
            this.txtProgress.Text = "";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dtpEEDATE);
            this.panel1.Controls.Add(this.dtpSSDATE);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.chkSortAs2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(834, 35);
            this.panel1.TabIndex = 2;
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(325, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(52, 28);
            this.btnPrint.TabIndex = 41;
            this.btnPrint.Text = "출 력";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // chkSortAs2
            // 
            this.chkSortAs2.AutoSize = true;
            this.chkSortAs2.Location = new System.Drawing.Point(213, 10);
            this.chkSortAs2.Name = "chkSortAs2";
            this.chkSortAs2.Size = new System.Drawing.Size(60, 16);
            this.chkSortAs2.TabIndex = 39;
            this.chkSortAs2.Text = "순정렬";
            this.chkSortAs2.UseVisualStyleBackColor = true;
            // 
            // dtpEEDATE
            // 
            this.dtpEEDATE.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEEDATE.Location = new System.Drawing.Point(110, 7);
            this.dtpEEDATE.Name = "dtpEEDATE";
            this.dtpEEDATE.Size = new System.Drawing.Size(97, 21);
            this.dtpEEDATE.TabIndex = 43;
            // 
            // dtpSSDATE
            // 
            this.dtpSSDATE.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpSSDATE.Location = new System.Drawing.Point(12, 7);
            this.dtpSSDATE.Name = "dtpSSDATE";
            this.dtpSSDATE.Size = new System.Drawing.Size(97, 21);
            this.dtpSSDATE.TabIndex = 42;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(273, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(52, 28);
            this.btnSearch.TabIndex = 41;
            this.btnSearch.Text = "조 회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // frmEmrProgressView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 799);
            this.Controls.Add(this.panChart);
            this.Name = "frmEmrProgressView";
            this.Text = "frmEmrProgressView";
            this.Load += new System.EventHandler(this.frmEmrProgressView_Load);
            this.panChart.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panChart;
        private System.Windows.Forms.RichTextBox txtProgress;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkSortAs2;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.DateTimePicker dtpEEDATE;
        private System.Windows.Forms.DateTimePicker dtpSSDATE;
        private System.Windows.Forms.Button btnSearch;
    }
}