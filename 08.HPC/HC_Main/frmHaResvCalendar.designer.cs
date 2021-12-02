namespace HC_Main
{
    partial class frmHaResvCalendar
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblFormTitle = new System.Windows.Forms.Label();
            this.panMain = new System.Windows.Forms.Panel();
            this.panRemark = new System.Windows.Forms.Panel();
            this.panExCount = new System.Windows.Forms.Panel();
            this.panCal = new System.Windows.Forms.Panel();
            this.paList = new System.Windows.Forms.Panel();
            this.panResv = new System.Windows.Forms.Panel();
            this.panDailyCount = new System.Windows.Forms.Panel();
            this.panSub01 = new System.Windows.Forms.Panel();
            this.panSearch = new System.Windows.Forms.Panel();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel4.SuspendLayout();
            this.panMain.SuspendLayout();
            this.panCal.SuspendLayout();
            this.paList.SuspendLayout();
            this.panSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.btnSave);
            this.panel4.Controls.Add(this.btnExit);
            this.panel4.Controls.Add(this.lblFormTitle);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1264, 33);
            this.panel4.TabIndex = 11;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave.Location = new System.Drawing.Point(1102, 0);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 31);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "저장(&S)";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(1182, 0);
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
            this.lblFormTitle.Size = new System.Drawing.Size(112, 21);
            this.lblFormTitle.TabIndex = 7;
            this.lblFormTitle.Text = "종검 예약달력";
            // 
            // panMain
            // 
            this.panMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panMain.Controls.Add(this.panRemark);
            this.panMain.Controls.Add(this.panExCount);
            this.panMain.Controls.Add(this.panCal);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.panMain.Location = new System.Drawing.Point(0, 33);
            this.panMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panMain.Name = "panMain";
            this.panMain.Size = new System.Drawing.Size(1264, 438);
            this.panMain.TabIndex = 12;
            // 
            // panRemark
            // 
            this.panRemark.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panRemark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panRemark.Location = new System.Drawing.Point(1046, 0);
            this.panRemark.Name = "panRemark";
            this.panRemark.Size = new System.Drawing.Size(216, 436);
            this.panRemark.TabIndex = 2;
            // 
            // panExCount
            // 
            this.panExCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panExCount.Dock = System.Windows.Forms.DockStyle.Left;
            this.panExCount.Location = new System.Drawing.Point(616, 0);
            this.panExCount.Name = "panExCount";
            this.panExCount.Size = new System.Drawing.Size(430, 436);
            this.panExCount.TabIndex = 1;
            // 
            // panCal
            // 
            this.panCal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panCal.Controls.Add(this.panSearch);
            this.panCal.Dock = System.Windows.Forms.DockStyle.Left;
            this.panCal.Location = new System.Drawing.Point(0, 0);
            this.panCal.Name = "panCal";
            this.panCal.Size = new System.Drawing.Size(616, 436);
            this.panCal.TabIndex = 0;
            // 
            // paList
            // 
            this.paList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.paList.Controls.Add(this.panResv);
            this.paList.Controls.Add(this.panDailyCount);
            this.paList.Controls.Add(this.panSub01);
            this.paList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paList.Location = new System.Drawing.Point(0, 471);
            this.paList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.paList.Name = "paList";
            this.paList.Size = new System.Drawing.Size(1264, 350);
            this.paList.TabIndex = 13;
            // 
            // panResv
            // 
            this.panResv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panResv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panResv.Location = new System.Drawing.Point(1046, 30);
            this.panResv.Name = "panResv";
            this.panResv.Size = new System.Drawing.Size(216, 318);
            this.panResv.TabIndex = 3;
            // 
            // panDailyCount
            // 
            this.panDailyCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panDailyCount.Dock = System.Windows.Forms.DockStyle.Left;
            this.panDailyCount.Location = new System.Drawing.Point(0, 30);
            this.panDailyCount.Name = "panDailyCount";
            this.panDailyCount.Size = new System.Drawing.Size(1046, 318);
            this.panDailyCount.TabIndex = 2;
            // 
            // panSub01
            // 
            this.panSub01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSub01.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSub01.Location = new System.Drawing.Point(0, 0);
            this.panSub01.Name = "panSub01";
            this.panSub01.Size = new System.Drawing.Size(1262, 30);
            this.panSub01.TabIndex = 1;
            // 
            // panSearch
            // 
            this.panSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSearch.Controls.Add(this.button1);
            this.panSearch.Controls.Add(this.BtnSearch);
            this.panSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSearch.Location = new System.Drawing.Point(0, 0);
            this.panSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panSearch.Name = "panSearch";
            this.panSearch.Size = new System.Drawing.Size(614, 33);
            this.panSearch.TabIndex = 12;
            // 
            // BtnSearch
            // 
            this.BtnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnSearch.Location = new System.Drawing.Point(542, 0);
            this.BtnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(70, 31);
            this.BtnSearch.TabIndex = 2;
            this.BtnSearch.Text = "검색";
            this.BtnSearch.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.Location = new System.Drawing.Point(409, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 31);
            this.button1.TabIndex = 3;
            this.button1.Text = "Developer Tool";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            // 
            // frmHaResvCalendar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 821);
            this.Controls.Add(this.paList);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.panel4);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHaResvCalendar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "종합검진 예약달력";
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panMain.ResumeLayout(false);
            this.panCal.ResumeLayout(false);
            this.paList.ResumeLayout(false);
            this.panSearch.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Panel panMain;
        private System.Windows.Forms.Panel panRemark;
        private System.Windows.Forms.Panel panExCount;
        private System.Windows.Forms.Panel panCal;
        private System.Windows.Forms.Panel paList;
        private System.Windows.Forms.Panel panResv;
        private System.Windows.Forms.Panel panDailyCount;
        private System.Windows.Forms.Panel panSub01;
        private System.Windows.Forms.Panel panSearch;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button BtnSearch;
    }
}