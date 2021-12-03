namespace HC_Bill
{
    partial class frmMDI_Admin_Old
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
            this.panSubTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.collapsibleSplitContainer1 = new DevComponents.DotNetBar.Controls.CollapsibleSplitContainer();
            this.menuTree = new DevComponents.AdvTree.AdvTree();
            this.Job1 = new DevComponents.AdvTree.Node();
            this.Job1_1 = new DevComponents.AdvTree.Node();
            this.Job1_2 = new DevComponents.AdvTree.Node();
            this.Job1_8 = new DevComponents.AdvTree.Node();
            this.Job1_3 = new DevComponents.AdvTree.Node();
            this.Job1_5 = new DevComponents.AdvTree.Node();
            this.Job1_9 = new DevComponents.AdvTree.Node();
            this.Job1_11 = new DevComponents.AdvTree.Node();
            this.Job1_10 = new DevComponents.AdvTree.Node();
            this.Job1_12 = new DevComponents.AdvTree.Node();
            this.Job1_4 = new DevComponents.AdvTree.Node();
            this.Job1_4_1 = new DevComponents.AdvTree.Node();
            this.Job1_4_2 = new DevComponents.AdvTree.Node();
            this.Job1_6 = new DevComponents.AdvTree.Node();
            this.Job1_6_1 = new DevComponents.AdvTree.Node();
            this.Job1_6_2 = new DevComponents.AdvTree.Node();
            this.Job1_7 = new DevComponents.AdvTree.Node();
            this.Job1_7_1 = new DevComponents.AdvTree.Node();
            this.Job1_7_2 = new DevComponents.AdvTree.Node();
            this.Job1_7_3 = new DevComponents.AdvTree.Node();
            this.Job4 = new DevComponents.AdvTree.Node();
            this.Job4_1 = new DevComponents.AdvTree.Node();
            this.Job2 = new DevComponents.AdvTree.Node();
            this.Job2_1 = new DevComponents.AdvTree.Node();
            this.Job3 = new DevComponents.AdvTree.Node();
            this.Job3_1 = new DevComponents.AdvTree.Node();
            this.Job_Exit = new DevComponents.AdvTree.Node();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.tabForm = new DevComponents.DotNetBar.SuperTabControl();
            this.panSubTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.collapsibleSplitContainer1)).BeginInit();
            this.collapsibleSplitContainer1.Panel1.SuspendLayout();
            this.collapsibleSplitContainer1.Panel2.SuspendLayout();
            this.collapsibleSplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.menuTree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabForm)).BeginInit();
            this.SuspendLayout();
            // 
            // panSubTitle
            // 
            this.panSubTitle.BackColor = System.Drawing.Color.RoyalBlue;
            this.panSubTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSubTitle.Controls.Add(this.lblTitle);
            this.panSubTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSubTitle.ForeColor = System.Drawing.Color.White;
            this.panSubTitle.Location = new System.Drawing.Point(0, 0);
            this.panSubTitle.Name = "panSubTitle";
            this.panSubTitle.Size = new System.Drawing.Size(1264, 29);
            this.panSubTitle.TabIndex = 41;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(6, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(174, 17);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "건강증진센터 행정업무 메인";
            // 
            // collapsibleSplitContainer1
            // 
            this.collapsibleSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.collapsibleSplitContainer1.Location = new System.Drawing.Point(0, 29);
            this.collapsibleSplitContainer1.Name = "collapsibleSplitContainer1";
            // 
            // collapsibleSplitContainer1.Panel1
            // 
            this.collapsibleSplitContainer1.Panel1.Controls.Add(this.menuTree);
            // 
            // collapsibleSplitContainer1.Panel2
            // 
            this.collapsibleSplitContainer1.Panel2.Controls.Add(this.tabForm);
            this.collapsibleSplitContainer1.Size = new System.Drawing.Size(1264, 792);
            this.collapsibleSplitContainer1.SplitterDistance = 259;
            this.collapsibleSplitContainer1.SplitterWidth = 20;
            this.collapsibleSplitContainer1.TabIndex = 42;
            // 
            // menuTree
            // 
            this.menuTree.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.menuTree.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.menuTree.BackgroundStyle.Class = "TreeBorderKey";
            this.menuTree.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.menuTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuTree.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.menuTree.Location = new System.Drawing.Point(0, 0);
            this.menuTree.Name = "menuTree";
            this.menuTree.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.Job1,
            this.Job4,
            this.Job2,
            this.Job3,
            this.Job_Exit});
            this.menuTree.NodesConnector = this.nodeConnector1;
            this.menuTree.NodeStyle = this.elementStyle1;
            this.menuTree.PathSeparator = ";";
            this.menuTree.Size = new System.Drawing.Size(259, 792);
            this.menuTree.Styles.Add(this.elementStyle1);
            this.menuTree.TabIndex = 1;
            this.menuTree.Text = "advTree1";
            // 
            // Job1
            // 
            this.Job1.Expanded = true;
            this.Job1.Image = global::HC_Bill.Properties.Resources.close;
            this.Job1.ImageExpanded = global::HC_Bill.Properties.Resources.open;
            this.Job1.Name = "Job1";
            this.Job1.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.Job1_1,
            this.Job1_2,
            this.Job1_8,
            this.Job1_3,
            this.Job1_5,
            this.Job1_9,
            this.Job1_11,
            this.Job1_10,
            this.Job1_12,
            this.Job1_4,
            this.Job1_6,
            this.Job1_7});
            this.Job1.Text = "기초코드";
            // 
            // Job1_1
            // 
            this.Job1_1.Expanded = true;
            this.Job1_1.Image = global::HC_Bill.Properties.Resources.page;
            this.Job1_1.Name = "Job1_1";
            this.Job1_1.Text = "검진기초코드";
            // 
            // Job1_2
            // 
            this.Job1_2.Expanded = true;
            this.Job1_2.Image = global::HC_Bill.Properties.Resources.page;
            this.Job1_2.Name = "Job1_2";
            this.Job1_2.Text = "검사항목코드";
            // 
            // Job1_8
            // 
            this.Job1_8.Expanded = true;
            this.Job1_8.Image = global::HC_Bill.Properties.Resources.page;
            this.Job1_8.Name = "Job1_8";
            this.Job1_8.Text = "결과값코드";
            // 
            // Job1_3
            // 
            this.Job1_3.Expanded = true;
            this.Job1_3.Image = global::HC_Bill.Properties.Resources.page;
            this.Job1_3.Name = "Job1_3";
            this.Job1_3.Text = "사업장코드";
            // 
            // Job1_5
            // 
            this.Job1_5.Expanded = true;
            this.Job1_5.Image = global::HC_Bill.Properties.Resources.page;
            this.Job1_5.Name = "Job1_5";
            this.Job1_5.Text = "종검감액코드";
            // 
            // Job1_9
            // 
            this.Job1_9.Expanded = true;
            this.Job1_9.Image = global::HC_Bill.Properties.Resources.page;
            this.Job1_9.Name = "Job1_9";
            this.Job1_9.Text = "판정의사코드";
            // 
            // Job1_11
            // 
            this.Job1_11.Expanded = true;
            this.Job1_11.Image = global::HC_Bill.Properties.Resources.page;
            this.Job1_11.Name = "Job1_11";
            this.Job1_11.Text = "취급물질등록";
            // 
            // Job1_10
            // 
            this.Job1_10.Expanded = true;
            this.Job1_10.Image = global::HC_Bill.Properties.Resources.page;
            this.Job1_10.Name = "Job1_10";
            this.Job1_10.Text = "사업장별 취급물질 등록";
            // 
            // Job1_12
            // 
            this.Job1_12.Expanded = true;
            this.Job1_12.Image = global::HC_Bill.Properties.Resources.page;
            this.Job1_12.Name = "Job1_12";
            this.Job1_12.Text = "검진수가 일괄변경";
            // 
            // Job1_4
            // 
            this.Job1_4.Expanded = true;
            this.Job1_4.Image = global::HC_Bill.Properties.Resources.close;
            this.Job1_4.ImageExpanded = global::HC_Bill.Properties.Resources.open;
            this.Job1_4.Name = "Job1_4";
            this.Job1_4.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.Job1_4_1,
            this.Job1_4_2});
            this.Job1_4.Text = "검진종류";
            // 
            // Job1_4_1
            // 
            this.Job1_4_1.Expanded = true;
            this.Job1_4_1.Image = global::HC_Bill.Properties.Resources.page;
            this.Job1_4_1.Name = "Job1_4_1";
            this.Job1_4_1.Text = "일반검진";
            // 
            // Job1_4_2
            // 
            this.Job1_4_2.Expanded = true;
            this.Job1_4_2.Image = global::HC_Bill.Properties.Resources.page;
            this.Job1_4_2.Name = "Job1_4_2";
            this.Job1_4_2.Text = "종합검진";
            // 
            // Job1_6
            // 
            this.Job1_6.Expanded = true;
            this.Job1_6.Image = global::HC_Bill.Properties.Resources.close;
            this.Job1_6.ImageExpanded = global::HC_Bill.Properties.Resources.open;
            this.Job1_6.Name = "Job1_6";
            this.Job1_6.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.Job1_6_1,
            this.Job1_6_2});
            this.Job1_6.Text = "그룹코드";
            // 
            // Job1_6_1
            // 
            this.Job1_6_1.Expanded = true;
            this.Job1_6_1.Image = global::HC_Bill.Properties.Resources.page;
            this.Job1_6_1.Name = "Job1_6_1";
            this.Job1_6_1.Text = "일반검진";
            // 
            // Job1_6_2
            // 
            this.Job1_6_2.Expanded = true;
            this.Job1_6_2.Image = global::HC_Bill.Properties.Resources.page;
            this.Job1_6_2.Name = "Job1_6_2";
            this.Job1_6_2.Text = "종합검진";
            // 
            // Job1_7
            // 
            this.Job1_7.Expanded = true;
            this.Job1_7.Image = global::HC_Bill.Properties.Resources.close;
            this.Job1_7.ImageExpanded = global::HC_Bill.Properties.Resources.open;
            this.Job1_7.Name = "Job1_7";
            this.Job1_7.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.Job1_7_1,
            this.Job1_7_2,
            this.Job1_7_3});
            this.Job1_7.Text = "그룹코드검사항목";
            // 
            // Job1_7_1
            // 
            this.Job1_7_1.Expanded = true;
            this.Job1_7_1.Image = global::HC_Bill.Properties.Resources.page;
            this.Job1_7_1.Name = "Job1_7_1";
            this.Job1_7_1.Text = "일반검진";
            // 
            // Job1_7_2
            // 
            this.Job1_7_2.Expanded = true;
            this.Job1_7_2.Image = global::HC_Bill.Properties.Resources.page;
            this.Job1_7_2.Name = "Job1_7_2";
            this.Job1_7_2.Text = "종합검진";
            // 
            // Job1_7_3
            // 
            this.Job1_7_3.Expanded = true;
            this.Job1_7_3.Image = global::HC_Bill.Properties.Resources.page;
            this.Job1_7_3.Name = "Job1_7_3";
            this.Job1_7_3.Text = "취급물질별";
            // 
            // Job4
            // 
            this.Job4.Expanded = true;
            this.Job4.Image = global::HC_Bill.Properties.Resources.close;
            this.Job4.ImageExpanded = global::HC_Bill.Properties.Resources.open;
            this.Job4.Name = "Job4";
            this.Job4.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.Job4_1});
            this.Job4.Text = "행정업무";
            // 
            // Job4_1
            // 
            this.Job4_1.Expanded = true;
            this.Job4_1.Image = global::HC_Bill.Properties.Resources.page;
            this.Job4_1.Name = "Job4_1";
            this.Job4_1.Text = "행정업무1";
            // 
            // Job2
            // 
            this.Job2.Expanded = true;
            this.Job2.Image = global::HC_Bill.Properties.Resources.close;
            this.Job2.ImageExpanded = global::HC_Bill.Properties.Resources.open;
            this.Job2.Name = "Job2";
            this.Job2.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.Job2_1});
            this.Job2.Text = "공단청구";
            // 
            // Job2_1
            // 
            this.Job2_1.Expanded = true;
            this.Job2_1.Image = global::HC_Bill.Properties.Resources.find;
            this.Job2_1.Name = "Job2_1";
            this.Job2_1.Text = "메뉴 준비중";
            // 
            // Job3
            // 
            this.Job3.Expanded = true;
            this.Job3.Image = global::HC_Bill.Properties.Resources.close;
            this.Job3.ImageExpanded = global::HC_Bill.Properties.Resources.open;
            this.Job3.Name = "Job3";
            this.Job3.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.Job3_1});
            this.Job3.Text = "미수관리";
            // 
            // Job3_1
            // 
            this.Job3_1.Expanded = true;
            this.Job3_1.Image = global::HC_Bill.Properties.Resources.find;
            this.Job3_1.Name = "Job3_1";
            this.Job3_1.Text = "메뉴 준비중";
            // 
            // Job_Exit
            // 
            this.Job_Exit.Expanded = true;
            this.Job_Exit.Image = global::HC_Bill.Properties.Resources.delete;
            this.Job_Exit.Name = "Job_Exit";
            this.Job_Exit.Text = "종료";
            // 
            // nodeConnector1
            // 
            this.nodeConnector1.LineColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle1
            // 
            this.elementStyle1.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle1.Name = "elementStyle1";
            this.elementStyle1.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // tabForm
            // 
            this.tabForm.CloseButtonOnTabsVisible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tabForm.ControlBox.CloseBox.Name = "";
            // 
            // 
            // 
            this.tabForm.ControlBox.MenuBox.Name = "";
            this.tabForm.ControlBox.Name = "";
            this.tabForm.ControlBox.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.tabForm.ControlBox.MenuBox,
            this.tabForm.ControlBox.CloseBox});
            this.tabForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabForm.Location = new System.Drawing.Point(0, 0);
            this.tabForm.Name = "tabForm";
            this.tabForm.ReorderTabsEnabled = true;
            this.tabForm.SelectedTabFont = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.tabForm.SelectedTabIndex = -1;
            this.tabForm.Size = new System.Drawing.Size(985, 792);
            this.tabForm.TabFont = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tabForm.TabIndex = 8;
            this.tabForm.TabStyle = DevComponents.DotNetBar.eSuperTabStyle.Office2010BackstageBlue;
            this.tabForm.Text = "superTabControl1";
            // 
            // frmMDI_Admin_Old
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 821);
            this.Controls.Add(this.collapsibleSplitContainer1);
            this.Controls.Add(this.panSubTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmMDI_Admin_Old";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "검진 행정업무 메인";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panSubTitle.ResumeLayout(false);
            this.panSubTitle.PerformLayout();
            this.collapsibleSplitContainer1.Panel1.ResumeLayout(false);
            this.collapsibleSplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.collapsibleSplitContainer1)).EndInit();
            this.collapsibleSplitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.menuTree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabForm)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panSubTitle;
        private System.Windows.Forms.Label lblTitle;
        private DevComponents.DotNetBar.Controls.CollapsibleSplitContainer collapsibleSplitContainer1;
        private DevComponents.AdvTree.AdvTree menuTree;
        private DevComponents.AdvTree.Node Job1;
        private DevComponents.AdvTree.Node Job2;
        private DevComponents.AdvTree.Node Job3;
        private DevComponents.AdvTree.Node Job4;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private DevComponents.AdvTree.Node Job1_1;
        private DevComponents.AdvTree.Node Job_Exit;
        private DevComponents.AdvTree.Node Job1_2;
        private DevComponents.AdvTree.Node Job1_3;
        private DevComponents.AdvTree.Node Job1_4;
        private DevComponents.AdvTree.Node Job1_4_1;
        private DevComponents.AdvTree.Node Job1_4_2;
        private DevComponents.AdvTree.Node Job1_5;
        private DevComponents.AdvTree.Node Job1_6;
        private DevComponents.AdvTree.Node Job1_6_1;
        private DevComponents.AdvTree.Node Job1_6_2;
        private DevComponents.AdvTree.Node Job1_7;
        private DevComponents.AdvTree.Node Job1_7_1;
        private DevComponents.AdvTree.Node Job1_7_2;
        private DevComponents.AdvTree.Node Job1_7_3;
        private DevComponents.AdvTree.Node Job1_8;
        private DevComponents.AdvTree.Node Job1_9;
        private DevComponents.AdvTree.Node Job1_11;
        private DevComponents.AdvTree.Node Job1_10;
        private DevComponents.DotNetBar.SuperTabControl tabForm;
        private DevComponents.AdvTree.Node Job4_1;
        private DevComponents.AdvTree.Node Job1_12;
        private DevComponents.AdvTree.Node Job2_1;
        private DevComponents.AdvTree.Node Job3_1;
    }
}