namespace ComSupLibB.SupFnEx
{
    partial class frmComSupFnExVIEW03
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
            this.panel11 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panpic = new System.Windows.Forms.Panel();
            this.metroTilePan1 = new DevComponents.DotNetBar.Metro.MetroTilePanel();
            this.itemContainer1 = new DevComponents.DotNetBar.ItemContainer();
            this.panel11.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.panpic.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panel11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel11.Controls.Add(this.label12);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel11.Location = new System.Drawing.Point(0, 34);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(1186, 32);
            this.panel11.TabIndex = 99;
            this.panel11.Visible = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(7, 8);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 0;
            this.label12.Text = "등록관리";
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1186, 34);
            this.panTitle.TabIndex = 98;
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(1112, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(70, 30);
            this.btnExit.TabIndex = 30;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(167, 16);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "생체현미경 사진보기";
            // 
            // panpic
            // 
            this.panpic.AutoScroll = true;
            this.panpic.BackColor = System.Drawing.Color.PowderBlue;
            this.panpic.Controls.Add(this.metroTilePan1);
            this.panpic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panpic.Location = new System.Drawing.Point(0, 66);
            this.panpic.Name = "panpic";
            this.panpic.Size = new System.Drawing.Size(1186, 695);
            this.panpic.TabIndex = 100;
            // 
            // metroTilePan1
            // 
            // 
            // 
            // 
            this.metroTilePan1.BackgroundStyle.Class = "MetroTilePanel";
            this.metroTilePan1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroTilePan1.ContainerControlProcessDialogKey = true;
            this.metroTilePan1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTilePan1.DragDropSupport = true;
            this.metroTilePan1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.itemContainer1});
            this.metroTilePan1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.metroTilePan1.Location = new System.Drawing.Point(0, 0);
            this.metroTilePan1.Name = "metroTilePan1";
            this.metroTilePan1.ReserveLeftSpace = false;
            this.metroTilePan1.Size = new System.Drawing.Size(1186, 695);
            this.metroTilePan1.TabIndex = 1;
            this.metroTilePan1.Text = "metroTilePanel1";
            // 
            // itemContainer1
            // 
            // 
            // 
            // 
            this.itemContainer1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer1.Name = "itemContainer1";
            // 
            // 
            // 
            this.itemContainer1.TitleStyle.Class = "MetroTileGroupTitle";
            this.itemContainer1.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer1.TitleText = "NVC Images";
            // 
            // frmComSupFnExVIEW03
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1186, 761);
            this.Controls.Add(this.panpic);
            this.Controls.Add(this.panel11);
            this.Controls.Add(this.panTitle);
            this.Name = "frmComSupFnExVIEW03";
            this.Text = "frmSupFnExVIEW03";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panpic.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panpic;
        private DevComponents.DotNetBar.Metro.MetroTilePanel metroTilePan1;
        public DevComponents.DotNetBar.ItemContainer itemContainer1;
    }
}