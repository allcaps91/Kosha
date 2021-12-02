namespace ComSupLibB.SupLbEx
{
    partial class frmComSupLbExHELP07
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
            FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer enhancedRowHeaderRenderer1 = new FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer();
            FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer enhancedRowHeaderRenderer2 = new FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer();
            this.panTitle = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbl_PTINFO = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.txt_REMIND = new System.Windows.Forms.TextBox();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            enhancedRowHeaderRenderer1.Name = "enhancedRowHeaderRenderer1";
            enhancedRowHeaderRenderer1.PictureZoomEffect = false;
            enhancedRowHeaderRenderer1.TextRotationAngle = 0D;
            enhancedRowHeaderRenderer1.ZoomFactor = 1F;
            enhancedRowHeaderRenderer2.BackColor = System.Drawing.SystemColors.Control;
            enhancedRowHeaderRenderer2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            enhancedRowHeaderRenderer2.ForeColor = System.Drawing.SystemColors.ControlText;
            enhancedRowHeaderRenderer2.Name = "enhancedRowHeaderRenderer2";
            enhancedRowHeaderRenderer2.PictureZoomEffect = false;
            enhancedRowHeaderRenderer2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            enhancedRowHeaderRenderer2.TextRotationAngle = 0D;
            enhancedRowHeaderRenderer2.ZoomFactor = 1F;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.Controls.Add(this.btnSave);
            this.panTitle.Controls.Add(this.panel1);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(3, 3);
            this.panTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitle.Name = "panTitle";
            this.panTitle.Padding = new System.Windows.Forms.Padding(1);
            this.panTitle.Size = new System.Drawing.Size(412, 40);
            this.panTitle.TabIndex = 13;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbl_PTINFO);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(1, 1);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.panel1.Size = new System.Drawing.Size(410, 38);
            this.panel1.TabIndex = 4;
            // 
            // lbl_PTINFO
            // 
            this.lbl_PTINFO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_PTINFO.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_PTINFO.Location = new System.Drawing.Point(5, 10);
            this.lbl_PTINFO.Name = "lbl_PTINFO";
            this.lbl_PTINFO.Size = new System.Drawing.Size(400, 23);
            this.lbl_PTINFO.TabIndex = 0;
            this.lbl_PTINFO.Text = "1234567890 홍길동 AA1234 : 이상한검사";
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(342, 1);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(69, 38);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // txt_REMIND
            // 
            this.txt_REMIND.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_REMIND.Location = new System.Drawing.Point(3, 43);
            this.txt_REMIND.Multiline = true;
            this.txt_REMIND.Name = "txt_REMIND";
            this.txt_REMIND.Size = new System.Drawing.Size(412, 374);
            this.txt_REMIND.TabIndex = 14;
            // 
            // frmComSupLbExHELP07
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(418, 420);
            this.Controls.Add(this.txt_REMIND);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmComSupLbExHELP07";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmComSupLbExHELP07";
            this.TopMost = true;
            this.panTitle.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl_PTINFO;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txt_REMIND;
    }
}