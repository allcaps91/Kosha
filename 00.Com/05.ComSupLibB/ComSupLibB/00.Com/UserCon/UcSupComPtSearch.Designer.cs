namespace ComSupLibB
{
    partial class UcSupComPtSearch
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
            this.lblPtNo = new System.Windows.Forms.Label();
            this.txtSearch_PtInfo = new System.Windows.Forms.TextBox();
            this.txtSearch_SName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblPtNo
            // 
            this.lblPtNo.AutoSize = true;
            this.lblPtNo.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblPtNo.Location = new System.Drawing.Point(0, 7);
            this.lblPtNo.Name = "lblPtNo";
            this.lblPtNo.Size = new System.Drawing.Size(60, 17);
            this.lblPtNo.TabIndex = 0;
            this.lblPtNo.Text = "환자정보";
            this.lblPtNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtSearch_PtInfo
            // 
            this.txtSearch_PtInfo.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtSearch_PtInfo.Location = new System.Drawing.Point(63, 2);
            this.txtSearch_PtInfo.Name = "txtSearch_PtInfo";
            this.txtSearch_PtInfo.Size = new System.Drawing.Size(76, 25);
            this.txtSearch_PtInfo.TabIndex = 1;
            this.txtSearch_PtInfo.Text = "1234567890";
            // 
            // txtSearch_SName
            // 
            this.txtSearch_SName.BackColor = System.Drawing.Color.Gainsboro;
            this.txtSearch_SName.Enabled = false;
            this.txtSearch_SName.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtSearch_SName.Location = new System.Drawing.Point(142, 2);
            this.txtSearch_SName.Name = "txtSearch_SName";
            this.txtSearch_SName.ReadOnly = true;
            this.txtSearch_SName.Size = new System.Drawing.Size(106, 25);
            this.txtSearch_SName.TabIndex = 2;
            this.txtSearch_SName.Text = "가나다라마바사";
            // 
            // UcSupComPtSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.txtSearch_SName);
            this.Controls.Add(this.txtSearch_PtInfo);
            this.Controls.Add(this.lblPtNo);
            this.Name = "UcSupComPtSearch";
            this.Size = new System.Drawing.Size(252, 30);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.TextBox txtSearch_PtInfo;
        public System.Windows.Forms.TextBox txtSearch_SName;
        public System.Windows.Forms.Label lblPtNo;
    }
}
