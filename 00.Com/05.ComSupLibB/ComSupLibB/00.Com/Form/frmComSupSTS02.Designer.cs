namespace ComSupLibB.Com
{
    partial class frmComSupSTS02
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
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbl_PTINFO = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.ss_PTINFO = new FarPoint.Win.Spread.FpSpread();
            this.ss_PTINFO_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.expandableSplitter1 = new DevComponents.DotNetBar.ExpandableSplitter();
            this.pan_frmComSupSTS01 = new System.Windows.Forms.Panel();
            this.barProgress = new DevComponents.DotNetBar.Controls.CircularProgress();
            this.panTitleSub0.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss_PTINFO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss_PTINFO_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.panel1);
            this.panTitleSub0.Controls.Add(this.btnExit);
            this.panTitleSub0.Controls.Add(this.panel3);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 0);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Padding = new System.Windows.Forms.Padding(1);
            this.panTitleSub0.Size = new System.Drawing.Size(1259, 35);
            this.panTitleSub0.TabIndex = 132;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbl_PTINFO);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(101, 1);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.panel1.Size = new System.Drawing.Size(209, 29);
            this.panel1.TabIndex = 26;
            // 
            // lbl_PTINFO
            // 
            this.lbl_PTINFO.AutoSize = true;
            this.lbl_PTINFO.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbl_PTINFO.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_PTINFO.ForeColor = System.Drawing.Color.White;
            this.lbl_PTINFO.Location = new System.Drawing.Point(3, 5);
            this.lbl_PTINFO.Name = "lbl_PTINFO";
            this.lbl_PTINFO.Size = new System.Drawing.Size(83, 12);
            this.lbl_PTINFO.TabIndex = 1;
            this.lbl_PTINFO.Text = "환자진료정보";
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(1204, 1);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(50, 29);
            this.btnExit.TabIndex = 25;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblTitleSub0);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(1, 1);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.panel3.Size = new System.Drawing.Size(100, 29);
            this.panel3.TabIndex = 24;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(3, 5);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(96, 12);
            this.lblTitleSub0.TabIndex = 1;
            this.lblTitleSub0.Text = "환자종합스케쥴";
            // 
            // ss_PTINFO
            // 
            this.ss_PTINFO.AccessibleDescription = "fpSpread1, Sheet1, Row 0, Column 0, ";
            this.ss_PTINFO.Dock = System.Windows.Forms.DockStyle.Left;
            this.ss_PTINFO.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ss_PTINFO.Location = new System.Drawing.Point(0, 35);
            this.ss_PTINFO.Name = "ss_PTINFO";
            this.ss_PTINFO.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ss_PTINFO_Sheet1});
            this.ss_PTINFO.Size = new System.Drawing.Size(609, 186);
            this.ss_PTINFO.TabIndex = 133;
            this.ss_PTINFO.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // ss_PTINFO_Sheet1
            // 
            this.ss_PTINFO_Sheet1.Reset();
            this.ss_PTINFO_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ss_PTINFO_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ss_PTINFO_Sheet1.ColumnCount = 20;
            this.ss_PTINFO_Sheet1.RowCount = 7;
            this.ss_PTINFO_Sheet1.ColumnHeader.Visible = false;
            this.ss_PTINFO_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ss_PTINFO_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ss_PTINFO_Sheet1.RowHeader.Visible = false;
            this.ss_PTINFO_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.None);
            this.ss_PTINFO_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // expandableSplitter1
            // 
            this.expandableSplitter1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandableSplitter1.ExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.ExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.GripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.GripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter1.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(151)))), ((int)(((byte)(61)))));
            this.expandableSplitter1.HotBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(184)))), ((int)(((byte)(94)))));
            this.expandableSplitter1.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this.expandableSplitter1.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this.expandableSplitter1.HotExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.HotExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.HotGripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.HotGripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter1.Location = new System.Drawing.Point(609, 35);
            this.expandableSplitter1.Name = "expandableSplitter1";
            this.expandableSplitter1.Size = new System.Drawing.Size(6, 186);
            this.expandableSplitter1.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007;
            this.expandableSplitter1.TabIndex = 134;
            this.expandableSplitter1.TabStop = false;
            // 
            // pan_frmComSupSTS01
            // 
            this.pan_frmComSupSTS01.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pan_frmComSupSTS01.Location = new System.Drawing.Point(615, 35);
            this.pan_frmComSupSTS01.Name = "pan_frmComSupSTS01";
            this.pan_frmComSupSTS01.Size = new System.Drawing.Size(644, 186);
            this.pan_frmComSupSTS01.TabIndex = 135;
            // 
            // barProgress
            // 
            this.barProgress.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.barProgress.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.barProgress.Location = new System.Drawing.Point(422, 46);
            this.barProgress.Name = "barProgress";
            this.barProgress.Size = new System.Drawing.Size(172, 145);
            this.barProgress.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeXP;
            this.barProgress.TabIndex = 136;
            // 
            // frmComSupSTS02
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1259, 221);
            this.Controls.Add(this.barProgress);
            this.Controls.Add(this.pan_frmComSupSTS01);
            this.Controls.Add(this.expandableSplitter1);
            this.Controls.Add(this.ss_PTINFO);
            this.Controls.Add(this.panTitleSub0);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmComSupSTS02";
            this.Text = "frmComSupSTS02";
            this.panTitleSub0.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss_PTINFO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss_PTINFO_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Button btnExit;
        private FarPoint.Win.Spread.FpSpread ss_PTINFO;
        private FarPoint.Win.Spread.SheetView ss_PTINFO_Sheet1;
        private DevComponents.DotNetBar.ExpandableSplitter expandableSplitter1;
        private System.Windows.Forms.Panel pan_frmComSupSTS01;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl_PTINFO;
        private DevComponents.DotNetBar.Controls.CircularProgress barProgress;
    }
}