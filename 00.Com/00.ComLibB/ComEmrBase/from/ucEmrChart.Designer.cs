namespace ComEmrBase
{
    partial class ucEmrChart
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
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblChartDate = new System.Windows.Forms.Label();
            this.pnlChart = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lblChartTime = new System.Windows.Forms.Label();
            this.lblComplete = new System.Windows.Forms.Label();
            this.lblDeptName = new System.Windows.Forms.Label();
            this.lblPrntYn = new System.Windows.Forms.Label();
            this.lblServerDate = new System.Windows.Forms.Label();
            this.btnCahrtComment = new System.Windows.Forms.Button();
            this.lblFormName = new System.Windows.Forms.Label();
            this.btnChartReg = new System.Windows.Forms.Button();
            this.btnChartModify = new System.Windows.Forms.Button();
            this.btnRemarkView = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panLine = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.BackColor = System.Drawing.Color.Honeydew;
            this.lblUserName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUserName.Font = new System.Drawing.Font("돋움체", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblUserName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblUserName.Location = new System.Drawing.Point(196, 1);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(94, 28);
            this.lblUserName.TabIndex = 5;
            this.lblUserName.Text = "아무개나";
            this.lblUserName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblChartDate
            // 
            this.lblChartDate.AutoSize = true;
            this.lblChartDate.BackColor = System.Drawing.Color.Honeydew;
            this.lblChartDate.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblChartDate.Font = new System.Drawing.Font("돋움체", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblChartDate.ForeColor = System.Drawing.Color.Blue;
            this.lblChartDate.Location = new System.Drawing.Point(145, 1);
            this.lblChartDate.Name = "lblChartDate";
            this.lblChartDate.Size = new System.Drawing.Size(34, 28);
            this.lblChartDate.TabIndex = 4;
            this.lblChartDate.Text = "2019-09-09";
            this.lblChartDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlChart
            // 
            this.pnlChart.BackColor = System.Drawing.Color.Transparent;
            this.pnlChart.Location = new System.Drawing.Point(0, 246);
            this.pnlChart.Name = "pnlChart";
            this.pnlChart.Size = new System.Drawing.Size(736, 365);
            this.pnlChart.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.Honeydew;
            this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            this.tableLayoutPanel2.Controls.Add(this.lblChartDate, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblChartTime, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblUserName, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblDeptName, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblPrntYn, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblComplete, 5, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 30);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(736, 30);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // lblChartTime
            // 
            this.lblChartTime.AutoSize = true;
            this.lblChartTime.BackColor = System.Drawing.Color.Honeydew;
            this.lblChartTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblChartTime.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblChartTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblChartTime.Location = new System.Drawing.Point(4, 1);
            this.lblChartTime.Name = "lblChartTime";
            this.lblChartTime.Size = new System.Drawing.Size(134, 28);
            this.lblChartTime.TabIndex = 7;
            this.lblChartTime.Text = "11:11";
            this.lblChartTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblComplete
            // 
            this.lblComplete.BackColor = System.Drawing.Color.Transparent;
            this.lblComplete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblComplete.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.lblComplete.ForeColor = System.Drawing.Color.Blue;
            this.lblComplete.Location = new System.Drawing.Point(656, 1);
            this.lblComplete.Name = "lblComplete";
            this.lblComplete.Size = new System.Drawing.Size(76, 28);
            this.lblComplete.TabIndex = 9;
            this.lblComplete.Text = "검수완료";
            this.lblComplete.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDeptName
            // 
            this.lblDeptName.AutoSize = true;
            this.lblDeptName.BackColor = System.Drawing.Color.Honeydew;
            this.lblDeptName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDeptName.Font = new System.Drawing.Font("돋움체", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDeptName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblDeptName.Location = new System.Drawing.Point(297, 1);
            this.lblDeptName.Name = "lblDeptName";
            this.lblDeptName.Size = new System.Drawing.Size(274, 28);
            this.lblDeptName.TabIndex = 6;
            this.lblDeptName.Text = "label1";
            this.lblDeptName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPrntYn
            // 
            this.lblPrntYn.BackColor = System.Drawing.Color.Red;
            this.lblPrntYn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPrntYn.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.lblPrntYn.ForeColor = System.Drawing.Color.White;
            this.lblPrntYn.Location = new System.Drawing.Point(578, 1);
            this.lblPrntYn.Name = "lblPrntYn";
            this.lblPrntYn.Size = new System.Drawing.Size(71, 28);
            this.lblPrntYn.TabIndex = 8;
            this.lblPrntYn.Text = "사본발급\r\n완료";
            this.lblPrntYn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblServerDate
            // 
            this.lblServerDate.AutoSize = true;
            this.lblServerDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblServerDate.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.lblServerDate.Location = new System.Drawing.Point(578, 1);
            this.lblServerDate.Name = "lblServerDate";
            this.lblServerDate.Size = new System.Drawing.Size(154, 28);
            this.lblServerDate.TabIndex = 4;
            this.lblServerDate.Text = "( 20190830 10:39:04 )";
            this.lblServerDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCahrtComment
            // 
            this.btnCahrtComment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCahrtComment.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCahrtComment.Location = new System.Drawing.Point(126, 4);
            this.btnCahrtComment.Name = "btnCahrtComment";
            this.btnCahrtComment.Size = new System.Drawing.Size(102, 22);
            this.btnCahrtComment.TabIndex = 1;
            this.btnCahrtComment.Text = "주석(색칠하기)";
            this.btnCahrtComment.UseVisualStyleBackColor = true;
            this.btnCahrtComment.Click += new System.EventHandler(this.BtnCahrtComment_Click);
            // 
            // lblFormName
            // 
            this.lblFormName.AutoSize = true;
            this.lblFormName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFormName.Font = new System.Drawing.Font("돋움체", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblFormName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblFormName.Location = new System.Drawing.Point(286, 1);
            this.lblFormName.Name = "lblFormName";
            this.lblFormName.Size = new System.Drawing.Size(285, 28);
            this.lblFormName.TabIndex = 0;
            this.lblFormName.Text = "label1";
            this.lblFormName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnChartReg
            // 
            this.btnChartReg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnChartReg.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnChartReg.Location = new System.Drawing.Point(65, 4);
            this.btnChartReg.Name = "btnChartReg";
            this.btnChartReg.Size = new System.Drawing.Size(54, 22);
            this.btnChartReg.TabIndex = 2;
            this.btnChartReg.Text = "☆중요";
            this.btnChartReg.UseVisualStyleBackColor = true;
            this.btnChartReg.Click += new System.EventHandler(this.BtnChartReg_Click);
            // 
            // btnChartModify
            // 
            this.btnChartModify.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnChartModify.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnChartModify.Location = new System.Drawing.Point(4, 4);
            this.btnChartModify.Name = "btnChartModify";
            this.btnChartModify.Size = new System.Drawing.Size(54, 22);
            this.btnChartModify.TabIndex = 3;
            this.btnChartModify.Text = "수정";
            this.btnChartModify.UseVisualStyleBackColor = true;
            this.btnChartModify.Click += new System.EventHandler(this.BtnChartModify_Click);
            // 
            // btnRemarkView
            // 
            this.btnRemarkView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRemarkView.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRemarkView.Location = new System.Drawing.Point(235, 4);
            this.btnRemarkView.Name = "btnRemarkView";
            this.btnRemarkView.Size = new System.Drawing.Size(44, 22);
            this.btnRemarkView.TabIndex = 0;
            this.btnRemarkView.Text = "해제";
            this.btnRemarkView.UseVisualStyleBackColor = true;
            this.btnRemarkView.Visible = false;
            this.btnRemarkView.Click += new System.EventHandler(this.btnRemarkView_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 108F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel1.Controls.Add(this.btnRemarkView, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnChartModify, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnChartReg, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblFormName, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnCahrtComment, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblServerDate, 5, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(736, 30);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // panLine
            // 
            this.panLine.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panLine.Location = new System.Drawing.Point(2, 32);
            this.panLine.Name = "panLine";
            this.panLine.Size = new System.Drawing.Size(732, 27);
            this.panLine.TabIndex = 4;
            this.panLine.Visible = false;
            // 
            // ucEmrChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.pnlChart);
            this.Controls.Add(this.panLine);
            this.Name = "ucEmrChart";
            this.Size = new System.Drawing.Size(736, 611);
            this.Load += new System.EventHandler(this.UcEmrChart_Load);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.ucEmrChart_PreviewKeyDown);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlChart;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblChartDate;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lblDeptName;
        private System.Windows.Forms.Label lblChartTime;
        private System.Windows.Forms.Label lblPrntYn;
        private System.Windows.Forms.Label lblServerDate;
        private System.Windows.Forms.Button btnCahrtComment;
        public System.Windows.Forms.Label lblFormName;
        private System.Windows.Forms.Button btnChartReg;
        private System.Windows.Forms.Button btnChartModify;
        private System.Windows.Forms.Button btnRemarkView;
        public System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panLine;
        private System.Windows.Forms.Label lblComplete;
    }
}
