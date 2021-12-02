namespace ComEmrBase
{
    partial class frmEmrChart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEmrChart));
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblFormName = new System.Windows.Forms.Label();
            this.picIconBar0 = new System.Windows.Forms.PictureBox();
            this.panOption = new System.Windows.Forms.Panel();
            this.panOptionDtl = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.optMcrRpl = new System.Windows.Forms.RadioButton();
            this.optMcrAdd = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.optMcrUser = new System.Windows.Forms.RadioButton();
            this.optMcrDept = new System.Windows.Forms.RadioButton();
            this.optMcrAll = new System.Windows.Forms.RadioButton();
            this.panEmrMain = new System.Windows.Forms.Panel();
            this.panTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIconBar0)).BeginInit();
            this.panOption.SuspendLayout();
            this.panOptionDtl.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblFormName);
            this.panTitle.Controls.Add(this.picIconBar0);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(663, 29);
            this.panTitle.TabIndex = 11;
            this.panTitle.Visible = false;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(585, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 26);
            this.btnExit.TabIndex = 79;
            this.btnExit.Text = "닫 기";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // lblFormName
            // 
            this.lblFormName.AutoSize = true;
            this.lblFormName.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblFormName.Location = new System.Drawing.Point(29, 8);
            this.lblFormName.Name = "lblFormName";
            this.lblFormName.Size = new System.Drawing.Size(70, 12);
            this.lblFormName.TabIndex = 68;
            this.lblFormName.Text = "전체서식지";
            // 
            // picIconBar0
            // 
            this.picIconBar0.Image = ((System.Drawing.Image)(resources.GetObject("picIconBar0.Image")));
            this.picIconBar0.InitialImage = null;
            this.picIconBar0.Location = new System.Drawing.Point(7, 6);
            this.picIconBar0.Name = "picIconBar0";
            this.picIconBar0.Size = new System.Drawing.Size(16, 16);
            this.picIconBar0.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picIconBar0.TabIndex = 69;
            this.picIconBar0.TabStop = false;
            // 
            // panOption
            // 
            this.panOption.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panOption.Controls.Add(this.panOptionDtl);
            this.panOption.Dock = System.Windows.Forms.DockStyle.Right;
            this.panOption.Location = new System.Drawing.Point(574, 29);
            this.panOption.Name = "panOption";
            this.panOption.Size = new System.Drawing.Size(89, 734);
            this.panOption.TabIndex = 12;
            // 
            // panOptionDtl
            // 
            this.panOptionDtl.Controls.Add(this.panel3);
            this.panOptionDtl.Controls.Add(this.panel2);
            this.panOptionDtl.Location = new System.Drawing.Point(3, 4);
            this.panOptionDtl.Name = "panOptionDtl";
            this.panOptionDtl.Size = new System.Drawing.Size(79, 139);
            this.panOptionDtl.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.optMcrRpl);
            this.panel3.Controls.Add(this.optMcrAdd);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 80);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(79, 59);
            this.panel3.TabIndex = 1;
            // 
            // optMcrRpl
            // 
            this.optMcrRpl.AutoSize = true;
            this.optMcrRpl.Location = new System.Drawing.Point(8, 31);
            this.optMcrRpl.Name = "optMcrRpl";
            this.optMcrRpl.Size = new System.Drawing.Size(67, 21);
            this.optMcrRpl.TabIndex = 4;
            this.optMcrRpl.TabStop = true;
            this.optMcrRpl.Text = "변   경";
            this.optMcrRpl.UseVisualStyleBackColor = true;
            this.optMcrRpl.CheckedChanged += new System.EventHandler(this.optMcrRpl_CheckedChanged);
            // 
            // optMcrAdd
            // 
            this.optMcrAdd.AutoSize = true;
            this.optMcrAdd.Location = new System.Drawing.Point(8, 9);
            this.optMcrAdd.Name = "optMcrAdd";
            this.optMcrAdd.Size = new System.Drawing.Size(67, 21);
            this.optMcrAdd.TabIndex = 3;
            this.optMcrAdd.TabStop = true;
            this.optMcrAdd.Text = "추   가";
            this.optMcrAdd.UseVisualStyleBackColor = true;
            this.optMcrAdd.CheckedChanged += new System.EventHandler(this.optMcrAdd_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.optMcrUser);
            this.panel2.Controls.Add(this.optMcrDept);
            this.panel2.Controls.Add(this.optMcrAll);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(79, 80);
            this.panel2.TabIndex = 0;
            // 
            // optMcrUser
            // 
            this.optMcrUser.AutoSize = true;
            this.optMcrUser.Location = new System.Drawing.Point(8, 54);
            this.optMcrUser.Name = "optMcrUser";
            this.optMcrUser.Size = new System.Drawing.Size(65, 21);
            this.optMcrUser.TabIndex = 2;
            this.optMcrUser.TabStop = true;
            this.optMcrUser.Text = "사용자";
            this.optMcrUser.UseVisualStyleBackColor = true;
            this.optMcrUser.CheckedChanged += new System.EventHandler(this.optMcrUser_CheckedChanged);
            // 
            // optMcrDept
            // 
            this.optMcrDept.AutoSize = true;
            this.optMcrDept.Location = new System.Drawing.Point(8, 32);
            this.optMcrDept.Name = "optMcrDept";
            this.optMcrDept.Size = new System.Drawing.Size(67, 21);
            this.optMcrDept.TabIndex = 1;
            this.optMcrDept.TabStop = true;
            this.optMcrDept.Text = "과   별";
            this.optMcrDept.UseVisualStyleBackColor = true;
            this.optMcrDept.CheckedChanged += new System.EventHandler(this.optMcrDept_CheckedChanged);
            // 
            // optMcrAll
            // 
            this.optMcrAll.AutoSize = true;
            this.optMcrAll.Location = new System.Drawing.Point(8, 10);
            this.optMcrAll.Name = "optMcrAll";
            this.optMcrAll.Size = new System.Drawing.Size(67, 21);
            this.optMcrAll.TabIndex = 0;
            this.optMcrAll.TabStop = true;
            this.optMcrAll.Text = "전   체";
            this.optMcrAll.UseVisualStyleBackColor = true;
            this.optMcrAll.CheckedChanged += new System.EventHandler(this.optMcrAll_CheckedChanged);
            // 
            // panEmrMain
            // 
            this.panEmrMain.AutoScroll = true;
            this.panEmrMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panEmrMain.Location = new System.Drawing.Point(0, 29);
            this.panEmrMain.Name = "panEmrMain";
            this.panEmrMain.Size = new System.Drawing.Size(574, 734);
            this.panEmrMain.TabIndex = 13;
            // 
            // frmEmrChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(663, 763);
            this.Controls.Add(this.panEmrMain);
            this.Controls.Add(this.panOption);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmEmrChart";
            this.Text = "frmEmrChart";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmEmrChart_FormClosing);
            this.Load += new System.EventHandler(this.frmEmrChart_Load);
            this.Resize += new System.EventHandler(this.frmEmrChart_Resize);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIconBar0)).EndInit();
            this.panOption.ResumeLayout(false);
            this.panOptionDtl.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblFormName;
        private System.Windows.Forms.PictureBox picIconBar0;
        private System.Windows.Forms.Panel panOption;
        private System.Windows.Forms.Panel panOptionDtl;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton optMcrRpl;
        private System.Windows.Forms.RadioButton optMcrAdd;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton optMcrUser;
        private System.Windows.Forms.RadioButton optMcrDept;
        private System.Windows.Forms.RadioButton optMcrAll;
        private System.Windows.Forms.Panel panEmrMain;
        private System.Windows.Forms.Button btnExit;
    }
}