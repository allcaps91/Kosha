namespace ComHpcLibB
{
    partial class frmHcCalendar
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
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblFormTitle = new System.Windows.Forms.Label();
            this.panMain = new System.Windows.Forms.Panel();
            this.panCal = new System.Windows.Forms.Panel();
            this.panSearch = new System.Windows.Forms.Panel();
            this.panSub01 = new System.Windows.Forms.Panel();
            this.rdoView2 = new System.Windows.Forms.RadioButton();
            this.rdoView1 = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.btnDev = new System.Windows.Forms.Button();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.panel4.SuspendLayout();
            this.panMain.SuspendLayout();
            this.panSearch.SuspendLayout();
            this.panSub01.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.btnExit);
            this.panel4.Controls.Add(this.lblFormTitle);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1224, 33);
            this.panel4.TabIndex = 12;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(1142, 0);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(80, 31);
            this.btnExit.TabIndex = 9;
            this.btnExit.Text = "닫기(&X)";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.AutoSize = true;
            this.lblFormTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblFormTitle.Location = new System.Drawing.Point(7, 5);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(176, 21);
            this.lblFormTitle.TabIndex = 7;
            this.lblFormTitle.Text = "건강증진센터 예약달력";
            // 
            // panMain
            // 
            this.panMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panMain.Controls.Add(this.panCal);
            this.panMain.Controls.Add(this.panSearch);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(0, 33);
            this.panMain.Name = "panMain";
            this.panMain.Size = new System.Drawing.Size(1224, 773);
            this.panMain.TabIndex = 13;
            // 
            // panCal
            // 
            this.panCal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panCal.Location = new System.Drawing.Point(0, 33);
            this.panCal.Name = "panCal";
            this.panCal.Size = new System.Drawing.Size(1222, 738);
            this.panCal.TabIndex = 13;
            // 
            // panSearch
            // 
            this.panSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSearch.Controls.Add(this.panSub01);
            this.panSearch.Controls.Add(this.label3);
            this.panSearch.Controls.Add(this.btnDev);
            this.panSearch.Controls.Add(this.BtnSearch);
            this.panSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSearch.Location = new System.Drawing.Point(0, 0);
            this.panSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panSearch.Name = "panSearch";
            this.panSearch.Size = new System.Drawing.Size(1222, 33);
            this.panSearch.TabIndex = 12;
            // 
            // panSub01
            // 
            this.panSub01.AutoSize = true;
            this.panSub01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSub01.Controls.Add(this.rdoView2);
            this.panSub01.Controls.Add(this.rdoView1);
            this.panSub01.Dock = System.Windows.Forms.DockStyle.Left;
            this.panSub01.Location = new System.Drawing.Point(90, 0);
            this.panSub01.Name = "panSub01";
            this.panSub01.Padding = new System.Windows.Forms.Padding(3);
            this.panSub01.Size = new System.Drawing.Size(199, 31);
            this.panSub01.TabIndex = 122;
            // 
            // rdoView2
            // 
            this.rdoView2.AutoSize = true;
            this.rdoView2.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoView2.Location = new System.Drawing.Point(81, 3);
            this.rdoView2.Name = "rdoView2";
            this.rdoView2.Size = new System.Drawing.Size(113, 23);
            this.rdoView2.TabIndex = 1;
            this.rdoView2.Text = "내원검진+학생";
            this.rdoView2.UseVisualStyleBackColor = true;
            // 
            // rdoView1
            // 
            this.rdoView1.AutoSize = true;
            this.rdoView1.Checked = true;
            this.rdoView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoView1.Location = new System.Drawing.Point(3, 3);
            this.rdoView1.Name = "rdoView1";
            this.rdoView1.Size = new System.Drawing.Size(78, 23);
            this.rdoView1.TabIndex = 0;
            this.rdoView1.TabStop = true;
            this.rdoView1.Text = "출장검진";
            this.rdoView1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.LightBlue;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 31);
            this.label3.TabIndex = 121;
            this.label3.Text = "조회구분";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnDev
            // 
            this.btnDev.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDev.Location = new System.Drawing.Point(1008, 0);
            this.btnDev.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDev.Name = "btnDev";
            this.btnDev.Size = new System.Drawing.Size(133, 31);
            this.btnDev.TabIndex = 3;
            this.btnDev.Text = "Developer Tool";
            this.btnDev.UseVisualStyleBackColor = true;
            // 
            // BtnSearch
            // 
            this.BtnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnSearch.Location = new System.Drawing.Point(1141, 0);
            this.BtnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(79, 31);
            this.BtnSearch.TabIndex = 2;
            this.BtnSearch.Text = "검색";
            this.BtnSearch.UseVisualStyleBackColor = true;
            // 
            // frmHcCalendar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1224, 806);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.panel4);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHcCalendar";
            this.Text = "건강증진센터 예약달력 (샘플)";
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panMain.ResumeLayout(false);
            this.panSearch.ResumeLayout(false);
            this.panSearch.PerformLayout();
            this.panSub01.ResumeLayout(false);
            this.panSub01.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Panel panMain;
        private System.Windows.Forms.Panel panSearch;
        private System.Windows.Forms.Button btnDev;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.Panel panCal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panSub01;
        private System.Windows.Forms.RadioButton rdoView2;
        private System.Windows.Forms.RadioButton rdoView1;
    }
}