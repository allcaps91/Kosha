namespace ComLibB
{
    partial class frmPanoView
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
            this.components = new System.ComponentModel.Container();
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.tab = new DevComponents.DotNetBar.TabControl();
            this.tab1 = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabControlPanel1 = new DevComponents.DotNetBar.TabControlPanel();
            this.tabItem2 = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabControlPanel2 = new DevComponents.DotNetBar.TabControlPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSname = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ssEnrollmentNum = new FarPoint.Win.Spread.FpSpread();
            this.ssEnrollmentNum_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.txtJumin1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtJumin2 = new System.Windows.Forms.TextBox();
            this.panTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tab)).BeginInit();
            this.tab.SuspendLayout();
            this.tabControlPanel1.SuspendLayout();
            this.tabControlPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssEnrollmentNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssEnrollmentNum_Sheet1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(606, 34);
            this.panTitle.TabIndex = 89;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(529, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(9, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(116, 16);
            this.lblTitle.TabIndex = 15;
            this.lblTitle.Text = "등록번호 찾기";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tab
            // 
            this.tab.BackColor = System.Drawing.Color.White;
            this.tab.CanReorderTabs = true;
            this.tab.ColorScheme.TabBackground = System.Drawing.Color.White;
            this.tab.ColorScheme.TabBackground2 = System.Drawing.Color.White;
            this.tab.ColorScheme.TabItemBackground = System.Drawing.Color.CornflowerBlue;
            this.tab.ColorScheme.TabItemBackground2 = System.Drawing.Color.CornflowerBlue;
            this.tab.ColorScheme.TabItemSelectedBackground = System.Drawing.Color.CornflowerBlue;
            this.tab.ColorScheme.TabItemSelectedBackground2 = System.Drawing.Color.CornflowerBlue;
            this.tab.ColorScheme.TabItemSelectedText = System.Drawing.Color.White;
            this.tab.ColorScheme.TabItemText = System.Drawing.Color.White;
            this.tab.ColorScheme.TabPanelBackground = System.Drawing.Color.White;
            this.tab.ColorScheme.TabPanelBackground2 = System.Drawing.Color.White;
            this.tab.Controls.Add(this.tabControlPanel1);
            this.tab.Controls.Add(this.tabControlPanel2);
            this.tab.Dock = System.Windows.Forms.DockStyle.Top;
            this.tab.Location = new System.Drawing.Point(0, 34);
            this.tab.Name = "tab";
            this.tab.SelectedTabFont = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.tab.SelectedTabIndex = 0;
            this.tab.Size = new System.Drawing.Size(606, 54);
            this.tab.TabIndex = 90;
            this.tab.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tab.Tabs.Add(this.tab1);
            this.tab.Tabs.Add(this.tabItem2);
            this.tab.Text = "tabControl1";
            // 
            // tab1
            // 
            this.tab1.AttachedControl = this.tabControlPanel1;
            this.tab1.Name = "tab1";
            this.tab1.Text = "수진자 명별 조회";
            // 
            // tabControlPanel1
            // 
            this.tabControlPanel1.Controls.Add(this.panel1);
            this.tabControlPanel1.DisabledBackColor = System.Drawing.Color.Empty;
            this.tabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel1.Location = new System.Drawing.Point(0, 26);
            this.tabControlPanel1.Name = "tabControlPanel1";
            this.tabControlPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel1.Size = new System.Drawing.Size(606, 28);
            this.tabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel1.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right) 
            | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel1.Style.GradientAngle = 90;
            this.tabControlPanel1.TabIndex = 1;
            this.tabControlPanel1.TabItem = this.tab1;
            // 
            // tabItem2
            // 
            this.tabItem2.AttachedControl = this.tabControlPanel2;
            this.tabItem2.Name = "tabItem2";
            this.tabItem2.Text = "주민번호별 조회";
            // 
            // tabControlPanel2
            // 
            this.tabControlPanel2.Controls.Add(this.panel2);
            this.tabControlPanel2.DisabledBackColor = System.Drawing.Color.Empty;
            this.tabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel2.Location = new System.Drawing.Point(0, 26);
            this.tabControlPanel2.Name = "tabControlPanel2";
            this.tabControlPanel2.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel2.Size = new System.Drawing.Size(606, 28);
            this.tabControlPanel2.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel2.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel2.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel2.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right) 
            | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel2.Style.GradientAngle = 90;
            this.tabControlPanel2.TabIndex = 2;
            this.tabControlPanel2.TabItem = this.tabItem2;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtSname);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(1, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(604, 28);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(9, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "수진자명";
            // 
            // txtSname
            // 
            this.txtSname.Location = new System.Drawing.Point(62, 3);
            this.txtSname.Name = "txtSname";
            this.txtSname.Size = new System.Drawing.Size(100, 21);
            this.txtSname.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(162, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "2글자 이상을 입력해 주세요!!";
            // 
            // ssEnrollmentNum
            // 
            this.ssEnrollmentNum.AccessibleDescription = "";
            this.ssEnrollmentNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssEnrollmentNum.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssEnrollmentNum.Location = new System.Drawing.Point(0, 88);
            this.ssEnrollmentNum.Name = "ssEnrollmentNum";
            this.ssEnrollmentNum.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssEnrollmentNum_Sheet1});
            this.ssEnrollmentNum.Size = new System.Drawing.Size(606, 301);
            this.ssEnrollmentNum.TabIndex = 1;
            // 
            // ssEnrollmentNum_Sheet1
            // 
            this.ssEnrollmentNum_Sheet1.Reset();
            this.ssEnrollmentNum_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssEnrollmentNum_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            ssEnrollmentNum_Sheet1.ColumnCount = 9;
            ssEnrollmentNum_Sheet1.RowCount = 1;
            this.ssEnrollmentNum_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "병록번호";
            this.ssEnrollmentNum_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "수진자명";
            this.ssEnrollmentNum_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "성별";
            this.ssEnrollmentNum_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "주민번호";
            this.ssEnrollmentNum_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "보호자명";
            this.ssEnrollmentNum_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "환자 구분";
            this.ssEnrollmentNum_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "전화번호";
            this.ssEnrollmentNum_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "최종내원";
            this.ssEnrollmentNum_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "과";
            this.ssEnrollmentNum_Sheet1.ColumnHeader.Rows.Get(0).Height = 24F;
            this.ssEnrollmentNum_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssEnrollmentNum_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnrollmentNum_Sheet1.Columns.Get(0).Label = "병록번호";
            this.ssEnrollmentNum_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssEnrollmentNum_Sheet1.Columns.Get(0).Width = 55F;
            this.ssEnrollmentNum_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssEnrollmentNum_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnrollmentNum_Sheet1.Columns.Get(1).Label = "수진자명";
            this.ssEnrollmentNum_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssEnrollmentNum_Sheet1.Columns.Get(1).Width = 55F;
            this.ssEnrollmentNum_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssEnrollmentNum_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnrollmentNum_Sheet1.Columns.Get(2).Label = "성별";
            this.ssEnrollmentNum_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssEnrollmentNum_Sheet1.Columns.Get(2).Width = 30F;
            this.ssEnrollmentNum_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssEnrollmentNum_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnrollmentNum_Sheet1.Columns.Get(3).Label = "주민번호";
            this.ssEnrollmentNum_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssEnrollmentNum_Sheet1.Columns.Get(3).Width = 100F;
            this.ssEnrollmentNum_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.ssEnrollmentNum_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnrollmentNum_Sheet1.Columns.Get(4).Label = "보호자명";
            this.ssEnrollmentNum_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssEnrollmentNum_Sheet1.Columns.Get(4).Width = 55F;
            this.ssEnrollmentNum_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.ssEnrollmentNum_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnrollmentNum_Sheet1.Columns.Get(5).Label = "환자 구분";
            this.ssEnrollmentNum_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssEnrollmentNum_Sheet1.Columns.Get(5).Width = 30F;
            this.ssEnrollmentNum_Sheet1.Columns.Get(6).CellType = textCellType7;
            this.ssEnrollmentNum_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnrollmentNum_Sheet1.Columns.Get(6).Label = "전화번호";
            this.ssEnrollmentNum_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssEnrollmentNum_Sheet1.Columns.Get(6).Width = 100F;
            this.ssEnrollmentNum_Sheet1.Columns.Get(7).CellType = textCellType8;
            this.ssEnrollmentNum_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnrollmentNum_Sheet1.Columns.Get(7).Label = "최종내원";
            this.ssEnrollmentNum_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssEnrollmentNum_Sheet1.Columns.Get(7).Width = 100F;
            this.ssEnrollmentNum_Sheet1.Columns.Get(8).CellType = textCellType9;
            this.ssEnrollmentNum_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssEnrollmentNum_Sheet1.Columns.Get(8).Label = "과";
            this.ssEnrollmentNum_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssEnrollmentNum_Sheet1.Columns.Get(8).Width = 20F;
            this.ssEnrollmentNum_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ssEnrollmentNum_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssEnrollmentNum_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.txtJumin2);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.txtJumin1);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(1, 1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(604, 28);
            this.panel2.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "주민번호";
            // 
            // txtJumin1
            // 
            this.txtJumin1.Location = new System.Drawing.Point(62, 3);
            this.txtJumin1.Name = "txtJumin1";
            this.txtJumin1.Size = new System.Drawing.Size(100, 21);
            this.txtJumin1.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(162, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = " - ";
            // 
            // txtJumin2
            // 
            this.txtJumin2.Location = new System.Drawing.Point(181, 3);
            this.txtJumin2.Name = "txtJumin2";
            this.txtJumin2.Size = new System.Drawing.Size(100, 21);
            this.txtJumin2.TabIndex = 3;
            // 
            // frmPanoView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(606, 389);
            this.ControlBox = false;
            this.Controls.Add(this.ssEnrollmentNum);
            this.Controls.Add(this.tab);
            this.Controls.Add(this.panTitle);
            this.Name = "frmPanoView";
            this.Text = "등록번호 찾기";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tab)).EndInit();
            this.tab.ResumeLayout(false);
            this.tabControlPanel1.ResumeLayout(false);
            this.tabControlPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssEnrollmentNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssEnrollmentNum_Sheet1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private DevComponents.DotNetBar.TabControl tab;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel1;
        private DevComponents.DotNetBar.TabItem tab1;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel2;
        private DevComponents.DotNetBar.TabItem tabItem2;
        private FarPoint.Win.Spread.FpSpread ssEnrollmentNum;
        private FarPoint.Win.Spread.SheetView ssEnrollmentNum_Sheet1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSname;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtJumin2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtJumin1;
        private System.Windows.Forms.Label label3;
    }
}