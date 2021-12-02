namespace HC_OSHA
{
    partial class OshaSiteLastTree
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.BtnSearch = new System.Windows.Forms.Button();
            this.TxtName = new System.Windows.Forms.TextBox();
            this.panBody = new System.Windows.Forms.Panel();
            this.SiteTreeView = new System.Windows.Forms.TreeView();
            this.panSpace = new System.Windows.Forms.Panel();
            this.panTitle = new System.Windows.Forms.Panel();
            this.CboManager = new System.Windows.Forms.ComboBox();
            this.panBody.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(127, 1);
            this.BtnSearch.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(65, 56);
            this.BtnSearch.TabIndex = 4;
            this.BtnSearch.Text = "검색(&S)";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // TxtName
            // 
            this.TxtName.Location = new System.Drawing.Point(3, 30);
            this.TxtName.Name = "TxtName";
            this.TxtName.Size = new System.Drawing.Size(121, 25);
            this.TxtName.TabIndex = 0;
            this.TxtName.TextChanged += new System.EventHandler(this.TxtName_TextChanged);
            this.TxtName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtName_KeyUp);
            // 
            // panBody
            // 
            this.panBody.BackColor = System.Drawing.Color.White;
            this.panBody.Controls.Add(this.SiteTreeView);
            this.panBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBody.Location = new System.Drawing.Point(0, 66);
            this.panBody.Name = "panBody";
            this.panBody.Size = new System.Drawing.Size(200, 384);
            this.panBody.TabIndex = 29;
            // 
            // SiteTreeView
            // 
            this.SiteTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SiteTreeView.Location = new System.Drawing.Point(0, 0);
            this.SiteTreeView.Name = "SiteTreeView";
            this.SiteTreeView.Size = new System.Drawing.Size(200, 384);
            this.SiteTreeView.TabIndex = 0;
            this.SiteTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.SiteTreeView_AfterCheck);
            this.SiteTreeView.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.SiteTreeView_BeforeSelect);
            this.SiteTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.SiteTreeView_NodeMouseClick);
            // 
            // panSpace
            // 
            this.panSpace.BackColor = System.Drawing.SystemColors.Control;
            this.panSpace.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSpace.Location = new System.Drawing.Point(0, 61);
            this.panSpace.Name = "panSpace";
            this.panSpace.Size = new System.Drawing.Size(200, 5);
            this.panSpace.TabIndex = 30;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panTitle.Controls.Add(this.CboManager);
            this.panTitle.Controls.Add(this.BtnSearch);
            this.panTitle.Controls.Add(this.TxtName);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(200, 61);
            this.panTitle.TabIndex = 31;
            // 
            // CboManager
            // 
            this.CboManager.FormattingEnabled = true;
            this.CboManager.Location = new System.Drawing.Point(3, 2);
            this.CboManager.Name = "CboManager";
            this.CboManager.Size = new System.Drawing.Size(121, 25);
            this.CboManager.TabIndex = 5;
            this.CboManager.SelectedIndexChanged += new System.EventHandler(this.CboManager_SelectedIndexChanged);
            // 
            // OshaSiteLastTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panBody);
            this.Controls.Add(this.panSpace);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "OshaSiteLastTree";
            this.Size = new System.Drawing.Size(200, 450);
            this.Load += new System.EventHandler(this.OshaSiteLastTree_Load);
            this.panBody.ResumeLayout(false);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.TextBox TxtName;
        private System.Windows.Forms.Panel panBody;
        private System.Windows.Forms.Panel panSpace;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.TreeView SiteTreeView;
        private System.Windows.Forms.ComboBox CboManager;
    }
}
