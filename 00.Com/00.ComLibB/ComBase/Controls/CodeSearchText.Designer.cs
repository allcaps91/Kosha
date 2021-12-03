namespace ComBase.Controls
{
    partial class CodeSearchText
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
            this.components = new System.ComponentModel.Container();
            this.TxtCode = new System.Windows.Forms.TextBox();
            this.TxtName = new System.Windows.Forms.TextBox();
            this.SSSearch = new FarPoint.Win.Spread.FpSpread();
            this.SSSearch_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.SSSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSSearch_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // TxtCode
            // 
            this.TxtCode.Dock = System.Windows.Forms.DockStyle.Left;
            this.TxtCode.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.TxtCode.Location = new System.Drawing.Point(0, 0);
            this.TxtCode.Name = "TxtCode";
            this.TxtCode.Size = new System.Drawing.Size(100, 25);
            this.TxtCode.TabIndex = 0;
            this.TxtCode.TextChanged += new System.EventHandler(this.TxtCode_TextChanged);
            this.TxtCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtCode_KeyDown);
            // 
            // TxtName
            // 
            this.TxtName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TxtName.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.TxtName.Location = new System.Drawing.Point(105, 0);
            this.TxtName.Name = "TxtName";
            this.TxtName.Size = new System.Drawing.Size(195, 25);
            this.TxtName.TabIndex = 1;
            this.TxtName.TextChanged += new System.EventHandler(this.TxtName_TextChanged);
            // 
            // SSSearch
            // 
            this.SSSearch.AccessibleDescription = "";
            this.SSSearch.Location = new System.Drawing.Point(3, 27);
            this.SSSearch.Name = "SSSearch";
            this.SSSearch.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSSearch_Sheet1});
            this.SSSearch.Size = new System.Drawing.Size(385, 244);
            this.SSSearch.TabIndex = 2;
            this.SSSearch.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSSearch_CellDoubleClick);
            this.SSSearch.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.SSSearch_PreviewKeyDown);
            // 
            // SSSearch_Sheet1
            // 
            this.SSSearch_Sheet1.Reset();
            this.SSSearch_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSSearch_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSSearch_Sheet1.ColumnCount = 0;
            this.SSSearch_Sheet1.RowCount = 0;
            this.SSSearch_Sheet1.ActiveColumnIndex = -1;
            this.SSSearch_Sheet1.ActiveRowIndex = -1;
            this.SSSearch_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSSearch_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(100, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 25);
            this.panel1.TabIndex = 3;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // CodeSearchText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TxtName);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.SSSearch);
            this.Controls.Add(this.TxtCode);
            this.Name = "CodeSearchText";
            this.Size = new System.Drawing.Size(300, 25);
            this.Load += new System.EventHandler(this.GelSearchText_Load);
            this.Enter += new System.EventHandler(this.CodeSearchText_Enter);
            ((System.ComponentModel.ISupportInitialize)(this.SSSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSSearch_Sheet1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public FarPoint.Win.Spread.FpSpread SSSearch;
        public FarPoint.Win.Spread.SheetView SSSearch_Sheet1;
        public System.Windows.Forms.TextBox TxtCode;
        public System.Windows.Forms.TextBox TxtName;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer timer1;
    }
}
