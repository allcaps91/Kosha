namespace HC_Core
{
    partial class AutoCompleteMacro
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
            this.SSMacro = new FarPoint.Win.Spread.FpSpread();
            this.SSMacro_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.BtnMacro = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.SSMacro)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSMacro_Sheet1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // SSMacro
            // 
            this.SSMacro.AccessibleDescription = "";
            this.SSMacro.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSMacro.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.SSMacro.Location = new System.Drawing.Point(0, 0);
            this.SSMacro.Name = "SSMacro";
            this.SSMacro.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSMacro_Sheet1});
            this.SSMacro.Size = new System.Drawing.Size(234, 147);
            this.SSMacro.TabIndex = 3;
            this.SSMacro.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SSMacro_CellDoubleClick);
            // 
            // SSMacro_Sheet1
            // 
            this.SSMacro_Sheet1.Reset();
            this.SSMacro_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSMacro_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSMacro_Sheet1.ColumnCount = 0;
            this.SSMacro_Sheet1.RowCount = 0;
            this.SSMacro_Sheet1.ActiveColumnIndex = -1;
            this.SSMacro_Sheet1.ActiveRowIndex = -1;
            this.SSMacro_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMacro_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSMacro_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSMacro_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMacro_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMacro_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSMacro_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSMacro_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMacro_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSMacro_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // BtnMacro
            // 
            this.BtnMacro.Location = new System.Drawing.Point(3, 6);
            this.BtnMacro.Name = "BtnMacro";
            this.BtnMacro.Size = new System.Drawing.Size(75, 22);
            this.BtnMacro.TabIndex = 4;
            this.BtnMacro.Text = "상용구";
            this.BtnMacro.UseVisualStyleBackColor = true;
            this.BtnMacro.Click += new System.EventHandler(this.BtnMacro_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.BtnMacro);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(234, 34);
            this.panel1.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(156, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 22);
            this.button1.TabIndex = 5;
            this.button1.Text = "닫기";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.SSMacro);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 34);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(234, 147);
            this.panel2.TabIndex = 6;
            // 
            // AutoCompleteMacro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "AutoCompleteMacro";
            this.Size = new System.Drawing.Size(234, 181);
            this.Load += new System.EventHandler(this.AutoCompleteMacro_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SSMacro)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSMacro_Sheet1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public FarPoint.Win.Spread.FpSpread SSMacro;
        public FarPoint.Win.Spread.SheetView SSMacro_Sheet1;
        private System.Windows.Forms.Button BtnMacro;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button1;
    }
}
