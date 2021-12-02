namespace HC_OSHA
{
    partial class OshaSiteList
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
            this.TxtName = new System.Windows.Forms.TextBox();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.SSOshaList = new FarPoint.Win.Spread.FpSpread();
            this.SSOshaList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panSpace = new System.Windows.Forms.Panel();
            this.panBody = new System.Windows.Forms.Panel();
            this.panTitle = new System.Windows.Forms.Panel();
            this.ChkSchedule = new System.Windows.Forms.CheckBox();
            this.CboManager = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.SSOshaList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSOshaList_Sheet1)).BeginInit();
            this.panBody.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // TxtName
            // 
            this.TxtName.Location = new System.Drawing.Point(3, 33);
            this.TxtName.Name = "TxtName";
            this.TxtName.Size = new System.Drawing.Size(121, 25);
            this.TxtName.TabIndex = 0;
            this.TxtName.TextChanged += new System.EventHandler(this.TxtName_TextChanged);
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(127, 33);
            this.BtnSearch.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(65, 25);
            this.BtnSearch.TabIndex = 4;
            this.BtnSearch.Text = "검색(&S)";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // SSOshaList
            // 
            this.SSOshaList.AccessibleDescription = "";
            this.SSOshaList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSOshaList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSOshaList.Location = new System.Drawing.Point(0, 0);
            this.SSOshaList.Name = "SSOshaList";
            this.SSOshaList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSOshaList_Sheet1});
            this.SSOshaList.Size = new System.Drawing.Size(197, 380);
            this.SSOshaList.TabIndex = 0;
            this.SSOshaList.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSOshaList_CellClick);
            this.SSOshaList.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSOshaList_CellDoubleClick);
            this.SSOshaList.SetActiveViewport(0, -1, -1);
            // 
            // SSOshaList_Sheet1
            // 
            this.SSOshaList_Sheet1.Reset();
            this.SSOshaList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSOshaList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSOshaList_Sheet1.ColumnCount = 0;
            this.SSOshaList_Sheet1.RowCount = 0;
            this.SSOshaList_Sheet1.ActiveColumnIndex = -1;
            this.SSOshaList_Sheet1.ActiveRowIndex = -1;
            this.SSOshaList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSOshaList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panSpace
            // 
            this.panSpace.BackColor = System.Drawing.SystemColors.Control;
            this.panSpace.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSpace.Location = new System.Drawing.Point(0, 65);
            this.panSpace.Name = "panSpace";
            this.panSpace.Size = new System.Drawing.Size(197, 5);
            this.panSpace.TabIndex = 27;
            // 
            // panBody
            // 
            this.panBody.BackColor = System.Drawing.Color.White;
            this.panBody.Controls.Add(this.SSOshaList);
            this.panBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panBody.Location = new System.Drawing.Point(0, 70);
            this.panBody.Name = "panBody";
            this.panBody.Size = new System.Drawing.Size(197, 380);
            this.panBody.TabIndex = 4;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panTitle.Controls.Add(this.ChkSchedule);
            this.panTitle.Controls.Add(this.CboManager);
            this.panTitle.Controls.Add(this.BtnSearch);
            this.panTitle.Controls.Add(this.TxtName);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(197, 65);
            this.panTitle.TabIndex = 28;
            // 
            // ChkSchedule
            // 
            this.ChkSchedule.AutoSize = true;
            this.ChkSchedule.Checked = true;
            this.ChkSchedule.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkSchedule.Location = new System.Drawing.Point(132, 6);
            this.ChkSchedule.Name = "ChkSchedule";
            this.ChkSchedule.Size = new System.Drawing.Size(53, 21);
            this.ChkSchedule.TabIndex = 7;
            this.ChkSchedule.Text = "일정";
            this.ChkSchedule.UseVisualStyleBackColor = true;
            this.ChkSchedule.CheckedChanged += new System.EventHandler(this.ChkSchedule_CheckedChanged);
            // 
            // CboManager
            // 
            this.CboManager.FormattingEnabled = true;
            this.CboManager.Location = new System.Drawing.Point(3, 4);
            this.CboManager.Name = "CboManager";
            this.CboManager.Size = new System.Drawing.Size(121, 25);
            this.CboManager.TabIndex = 6;
            this.CboManager.SelectedIndexChanged += new System.EventHandler(this.CboManager_SelectedIndexChanged);
            // 
            // OshaSiteList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panBody);
            this.Controls.Add(this.panSpace);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "OshaSiteList";
            this.Size = new System.Drawing.Size(197, 450);
            this.Load += new System.EventHandler(this.OshaSiteList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SSOshaList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSOshaList_Sheet1)).EndInit();
            this.panBody.ResumeLayout(false);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox TxtName;
        private System.Windows.Forms.Button BtnSearch;
        private FarPoint.Win.Spread.FpSpread SSOshaList;
        private FarPoint.Win.Spread.SheetView SSOshaList_Sheet1;
        private System.Windows.Forms.Panel panBody;
        private System.Windows.Forms.Panel panSpace;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.ComboBox CboManager;
        private System.Windows.Forms.CheckBox ChkSchedule;
    }
}
