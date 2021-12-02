namespace ComEmrBase
{
    partial class usFormTopMenu
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
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblChartTime = new System.Windows.Forms.Label();
            this.lblChartDate = new System.Windows.Forms.Label();
            this.dtMedFrDate = new System.Windows.Forms.DateTimePicker();
            this.mbtnClear = new System.Windows.Forms.Button();
            this.mbtnPrint = new System.Windows.Forms.Button();
            this.mbtnSave = new System.Windows.Forms.Button();
            this.mbtnDelete = new System.Windows.Forms.Button();
            this.mbtnExit = new System.Windows.Forms.Button();
            this.mbtnTime = new System.Windows.Forms.Button();
            this.txtMedFrTime = new System.Windows.Forms.ComboBox();
            this.lblPrntYn = new System.Windows.Forms.Label();
            this.lblMcrtMcNo = new System.Windows.Forms.Label();
            this.mbtnSaveTemp = new System.Windows.Forms.Button();
            this.mbtnPrintNull = new System.Windows.Forms.Button();
            this.mbtnComplete = new System.Windows.Forms.Button();
            this.mbtnAuthority = new System.Windows.Forms.Button();
            this.mbtnHisSearch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblChartTime
            // 
            this.lblChartTime.AutoSize = true;
            this.lblChartTime.Location = new System.Drawing.Point(163, 11);
            this.lblChartTime.Name = "lblChartTime";
            this.lblChartTime.Size = new System.Drawing.Size(29, 12);
            this.lblChartTime.TabIndex = 16;
            this.lblChartTime.Text = "시간";
            // 
            // lblChartDate
            // 
            this.lblChartDate.AutoSize = true;
            this.lblChartDate.Location = new System.Drawing.Point(7, 11);
            this.lblChartDate.Name = "lblChartDate";
            this.lblChartDate.Size = new System.Drawing.Size(53, 12);
            this.lblChartDate.TabIndex = 15;
            this.lblChartDate.Text = "작성일자";
            // 
            // dtMedFrDate
            // 
            this.dtMedFrDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtMedFrDate.Location = new System.Drawing.Point(60, 7);
            this.dtMedFrDate.Name = "dtMedFrDate";
            this.dtMedFrDate.Size = new System.Drawing.Size(98, 21);
            this.dtMedFrDate.TabIndex = 14;
            this.dtMedFrDate.TabStop = false;
            // 
            // mbtnClear
            // 
            this.mbtnClear.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.mbtnClear.Location = new System.Drawing.Point(358, 3);
            this.mbtnClear.Name = "mbtnClear";
            this.mbtnClear.Size = new System.Drawing.Size(68, 28);
            this.mbtnClear.TabIndex = 18;
            this.mbtnClear.TabStop = false;
            this.mbtnClear.Text = "Clear";
            this.mbtnClear.UseVisualStyleBackColor = true;
            this.mbtnClear.Visible = false;
            this.mbtnClear.Click += new System.EventHandler(this.mbtnClear_Click);
            // 
            // mbtnPrint
            // 
            this.mbtnPrint.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.mbtnPrint.Location = new System.Drawing.Point(291, 3);
            this.mbtnPrint.Name = "mbtnPrint";
            this.mbtnPrint.Size = new System.Drawing.Size(62, 28);
            this.mbtnPrint.TabIndex = 19;
            this.mbtnPrint.TabStop = false;
            this.mbtnPrint.Text = "출력";
            this.mbtnPrint.UseVisualStyleBackColor = true;
            this.mbtnPrint.Visible = false;
            this.mbtnPrint.Click += new System.EventHandler(this.mbtnPrint_Click);
            // 
            // mbtnSave
            // 
            this.mbtnSave.BackColor = System.Drawing.Color.Pink;
            this.mbtnSave.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.mbtnSave.Location = new System.Drawing.Point(549, 3);
            this.mbtnSave.Name = "mbtnSave";
            this.mbtnSave.Size = new System.Drawing.Size(68, 28);
            this.mbtnSave.TabIndex = 20;
            this.mbtnSave.TabStop = false;
            this.mbtnSave.Text = "인증저장";
            this.mbtnSave.UseVisualStyleBackColor = false;
            this.mbtnSave.Visible = false;
            this.mbtnSave.Click += new System.EventHandler(this.mbtnSave_Click);
            // 
            // mbtnDelete
            // 
            this.mbtnDelete.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.mbtnDelete.Location = new System.Drawing.Point(425, 3);
            this.mbtnDelete.Name = "mbtnDelete";
            this.mbtnDelete.Size = new System.Drawing.Size(62, 28);
            this.mbtnDelete.TabIndex = 21;
            this.mbtnDelete.TabStop = false;
            this.mbtnDelete.Text = "삭 제";
            this.mbtnDelete.UseVisualStyleBackColor = true;
            this.mbtnDelete.Visible = false;
            this.mbtnDelete.Click += new System.EventHandler(this.mbtnDelete_Click);
            // 
            // mbtnExit
            // 
            this.mbtnExit.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.mbtnExit.Location = new System.Drawing.Point(695, 3);
            this.mbtnExit.Name = "mbtnExit";
            this.mbtnExit.Size = new System.Drawing.Size(62, 28);
            this.mbtnExit.TabIndex = 22;
            this.mbtnExit.TabStop = false;
            this.mbtnExit.Text = "닫  기";
            this.mbtnExit.UseVisualStyleBackColor = true;
            this.mbtnExit.Visible = false;
            this.mbtnExit.Click += new System.EventHandler(this.mbtnExit_Click);
            // 
            // mbtnTime
            // 
            this.mbtnTime.Location = new System.Drawing.Point(260, 3);
            this.mbtnTime.Name = "mbtnTime";
            this.mbtnTime.Size = new System.Drawing.Size(27, 28);
            this.mbtnTime.TabIndex = 23;
            this.mbtnTime.TabStop = false;
            this.mbtnTime.Text = "T";
            this.mbtnTime.UseVisualStyleBackColor = true;
            this.mbtnTime.Click += new System.EventHandler(this.mbtnTime_Click);
            // 
            // txtMedFrTime
            // 
            this.txtMedFrTime.FormattingEnabled = true;
            this.txtMedFrTime.Location = new System.Drawing.Point(194, 7);
            this.txtMedFrTime.Name = "txtMedFrTime";
            this.txtMedFrTime.Size = new System.Drawing.Size(60, 20);
            this.txtMedFrTime.TabIndex = 24;
            this.txtMedFrTime.TabStop = false;
            this.txtMedFrTime.Text = "00:00";
            // 
            // lblPrntYn
            // 
            this.lblPrntYn.BackColor = System.Drawing.Color.Red;
            this.lblPrntYn.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblPrntYn.ForeColor = System.Drawing.Color.White;
            this.lblPrntYn.Location = new System.Drawing.Point(619, 3);
            this.lblPrntYn.Name = "lblPrntYn";
            this.lblPrntYn.Size = new System.Drawing.Size(66, 30);
            this.lblPrntYn.TabIndex = 25;
            this.lblPrntYn.Text = "사본발급   완료";
            this.lblPrntYn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblPrntYn.Visible = false;
            // 
            // lblMcrtMcNo
            // 
            this.lblMcrtMcNo.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblMcrtMcNo.ForeColor = System.Drawing.Color.Blue;
            this.lblMcrtMcNo.Location = new System.Drawing.Point(161, 7);
            this.lblMcrtMcNo.Name = "lblMcrtMcNo";
            this.lblMcrtMcNo.Size = new System.Drawing.Size(85, 21);
            this.lblMcrtMcNo.TabIndex = 26;
            this.lblMcrtMcNo.Text = "9999-9999";
            this.lblMcrtMcNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMcrtMcNo.Visible = false;
            // 
            // mbtnSaveTemp
            // 
            this.mbtnSaveTemp.ForeColor = System.Drawing.Color.Black;
            this.mbtnSaveTemp.Location = new System.Drawing.Point(487, 3);
            this.mbtnSaveTemp.Name = "mbtnSaveTemp";
            this.mbtnSaveTemp.Size = new System.Drawing.Size(62, 28);
            this.mbtnSaveTemp.TabIndex = 27;
            this.mbtnSaveTemp.TabStop = false;
            this.mbtnSaveTemp.Text = "임시저장";
            this.mbtnSaveTemp.UseVisualStyleBackColor = true;
            this.mbtnSaveTemp.Visible = false;
            this.mbtnSaveTemp.Click += new System.EventHandler(this.mbtnSaveTemp_Click);
            // 
            // mbtnPrintNull
            // 
            this.mbtnPrintNull.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.mbtnPrintNull.Location = new System.Drawing.Point(301, 18);
            this.mbtnPrintNull.Name = "mbtnPrintNull";
            this.mbtnPrintNull.Size = new System.Drawing.Size(61, 28);
            this.mbtnPrintNull.TabIndex = 28;
            this.mbtnPrintNull.TabStop = false;
            this.mbtnPrintNull.Text = "빈서식";
            this.mbtnPrintNull.UseVisualStyleBackColor = true;
            this.mbtnPrintNull.Visible = false;
            this.mbtnPrintNull.Click += new System.EventHandler(this.mbtnPrintNull_Click);
            // 
            // mbtnComplete
            // 
            this.mbtnComplete.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.mbtnComplete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.mbtnComplete.Location = new System.Drawing.Point(362, 18);
            this.mbtnComplete.Name = "mbtnComplete";
            this.mbtnComplete.Size = new System.Drawing.Size(68, 28);
            this.mbtnComplete.TabIndex = 18;
            this.mbtnComplete.TabStop = false;
            this.mbtnComplete.Text = "미검수";
            this.mbtnComplete.UseVisualStyleBackColor = true;
            this.mbtnComplete.Visible = false;
            this.mbtnComplete.Click += new System.EventHandler(this.mbtnComplete_Click);
            // 
            // mbtnAuthority
            // 
            this.mbtnAuthority.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(243)))), ((int)(((byte)(188)))));
            this.mbtnAuthority.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.mbtnAuthority.ForeColor = System.Drawing.Color.Black;
            this.mbtnAuthority.Location = new System.Drawing.Point(414, 18);
            this.mbtnAuthority.Name = "mbtnAuthority";
            this.mbtnAuthority.Size = new System.Drawing.Size(62, 28);
            this.mbtnAuthority.TabIndex = 18;
            this.mbtnAuthority.TabStop = false;
            this.mbtnAuthority.Text = "권한";
            this.mbtnAuthority.UseVisualStyleBackColor = false;
            this.mbtnAuthority.Visible = false;
            this.mbtnAuthority.Click += new System.EventHandler(this.mbtnAuthority_Click);
            // 
            // mbtnHisSearch
            // 
            this.mbtnHisSearch.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Bold);
            this.mbtnHisSearch.ForeColor = System.Drawing.Color.Black;
            this.mbtnHisSearch.Location = new System.Drawing.Point(477, 18);
            this.mbtnHisSearch.Name = "mbtnHisSearch";
            this.mbtnHisSearch.Size = new System.Drawing.Size(68, 28);
            this.mbtnHisSearch.TabIndex = 27;
            this.mbtnHisSearch.TabStop = false;
            this.mbtnHisSearch.Text = "변경이력";
            this.mbtnHisSearch.UseVisualStyleBackColor = true;
            this.mbtnHisSearch.Visible = false;
            this.mbtnHisSearch.Click += new System.EventHandler(this.mbtnHisSearch_Click);
            // 
            // usFormTopMenu
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.mbtnAuthority);
            this.Controls.Add(this.mbtnPrintNull);
            this.Controls.Add(this.mbtnHisSearch);
            this.Controls.Add(this.mbtnSaveTemp);
            this.Controls.Add(this.lblMcrtMcNo);
            this.Controls.Add(this.lblPrntYn);
            this.Controls.Add(this.txtMedFrTime);
            this.Controls.Add(this.mbtnTime);
            this.Controls.Add(this.mbtnExit);
            this.Controls.Add(this.mbtnDelete);
            this.Controls.Add(this.mbtnSave);
            this.Controls.Add(this.mbtnPrint);
            this.Controls.Add(this.mbtnComplete);
            this.Controls.Add(this.mbtnClear);
            this.Controls.Add(this.lblChartTime);
            this.Controls.Add(this.lblChartDate);
            this.Controls.Add(this.dtMedFrDate);
            this.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "usFormTopMenu";
            this.Size = new System.Drawing.Size(761, 34);
            this.Load += new System.EventHandler(this.usFormTopMenu_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lblChartTime;
        public System.Windows.Forms.Label lblChartDate;
        public System.Windows.Forms.DateTimePicker dtMedFrDate;
        public System.Windows.Forms.Button mbtnClear;
        public System.Windows.Forms.Button mbtnPrint;
        public System.Windows.Forms.Button mbtnSave;
        public System.Windows.Forms.Button mbtnDelete;
        public System.Windows.Forms.Button mbtnExit;
        public System.Windows.Forms.Button mbtnTime;
        public System.Windows.Forms.ComboBox txtMedFrTime;
        public System.Windows.Forms.Label lblPrntYn;
        public System.Windows.Forms.Label lblMcrtMcNo;
        public System.Windows.Forms.Button mbtnSaveTemp;
        public System.Windows.Forms.Button mbtnPrintNull;
        public System.Windows.Forms.Button mbtnComplete;
        public System.Windows.Forms.Button mbtnAuthority;
        public System.Windows.Forms.Button mbtnHisSearch;
    }
}
