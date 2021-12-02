namespace ComBase.Mvc.UserControls
{
    partial class HorizSpace
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
            this.panSpace = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panSpace
            // 
            this.panSpace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panSpace.Location = new System.Drawing.Point(0, 0);
            this.panSpace.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panSpace.Name = "panSpace";
            this.panSpace.Size = new System.Drawing.Size(250, 5);
            this.panSpace.TabIndex = 27;
            // 
            // HorizSpace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panSpace);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "HorizSpace";
            this.Size = new System.Drawing.Size(250, 5);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panSpace;
    }
}
