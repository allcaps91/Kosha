namespace ComHpcLibB.UseCon
{
    partial class conHaExamRes
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
            this.panSubExam = new System.Windows.Forms.Panel();
            this.txtRes = new System.Windows.Forms.TextBox();
            this.panel9 = new System.Windows.Forms.Panel();
            this.lblExTitle = new System.Windows.Forms.Label();
            this.lblExCD = new System.Windows.Forms.Label();
            this.panSubExam.SuspendLayout();
            this.panel9.SuspendLayout();
            this.SuspendLayout();
            // 
            // panSubExam
            // 
            this.panSubExam.AutoSize = true;
            this.panSubExam.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSubExam.Controls.Add(this.txtRes);
            this.panSubExam.Controls.Add(this.panel9);
            this.panSubExam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panSubExam.Location = new System.Drawing.Point(0, 0);
            this.panSubExam.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.panSubExam.Name = "panSubExam";
            this.panSubExam.Size = new System.Drawing.Size(418, 89);
            this.panSubExam.TabIndex = 1;
            // 
            // txtRes
            // 
            this.txtRes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRes.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtRes.Location = new System.Drawing.Point(0, 32);
            this.txtRes.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtRes.Multiline = true;
            this.txtRes.Name = "txtRes";
            this.txtRes.Size = new System.Drawing.Size(416, 55);
            this.txtRes.TabIndex = 6;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.lblExTitle);
            this.panel9.Controls.Add(this.lblExCD);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(0, 0);
            this.panel9.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(416, 32);
            this.panel9.TabIndex = 5;
            // 
            // lblExTitle
            // 
            this.lblExTitle.BackColor = System.Drawing.Color.LightGray;
            this.lblExTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblExTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblExTitle.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.lblExTitle.Location = new System.Drawing.Point(0, 0);
            this.lblExTitle.Name = "lblExTitle";
            this.lblExTitle.Size = new System.Drawing.Size(353, 32);
            this.lblExTitle.TabIndex = 7;
            this.lblExTitle.Text = "◈ 초음파 - 뇌혈류 ◈";
            this.lblExTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblExCD
            // 
            this.lblExCD.BackColor = System.Drawing.Color.LightGray;
            this.lblExCD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblExCD.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblExCD.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.lblExCD.Location = new System.Drawing.Point(353, 0);
            this.lblExCD.Name = "lblExCD";
            this.lblExCD.Size = new System.Drawing.Size(63, 32);
            this.lblExCD.TabIndex = 6;
            this.lblExCD.Text = "[ TX92 ]";
            this.lblExCD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // conHaExamRes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panSubExam);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "conHaExamRes";
            this.Size = new System.Drawing.Size(418, 89);
            this.panSubExam.ResumeLayout(false);
            this.panSubExam.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel9;
        public System.Windows.Forms.Label lblExTitle;
        public System.Windows.Forms.Label lblExCD;
        public System.Windows.Forms.TextBox txtRes;
        public System.Windows.Forms.Panel panSubExam;
    }
}
