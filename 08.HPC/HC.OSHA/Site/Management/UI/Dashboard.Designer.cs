namespace HC.OSHA.Site.ETC.UI
{
    partial class Dashboard
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
            HC.OSHA.Site.Management.Model.HC_ESTIMATE_MODEL hC_ESTIMATE_MODEL2 = new HC.OSHA.Site.Management.Model.HC_ESTIMATE_MODEL();
            this.formTItle1 = new ComBase.Mvc.UserControls.FormTItle();
            this.tableBody = new System.Windows.Forms.TableLayoutPanel();
            this.panLeftTop = new System.Windows.Forms.Panel();
            this.oshaSiteList = new HC.OSHA.Site.Management.UI.OshaSiteList();
            this.panLeftBottom = new System.Windows.Forms.Panel();
            this.oshaSiteEstimateList = new HC.OSHA.Site.Management.UI.OshaSiteEstimateList();
            this.BtnWorker = new System.Windows.Forms.Button();
            this.BtnMSDS = new System.Windows.Forms.Button();
            this.panFrame = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.tableBody.SuspendLayout();
            this.panLeftTop.SuspendLayout();
            this.panLeftBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // formTItle1
            // 
            this.formTItle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.formTItle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.formTItle1.Location = new System.Drawing.Point(0, 0);
            this.formTItle1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.formTItle1.Name = "formTItle1";
            this.formTItle1.Size = new System.Drawing.Size(1264, 35);
            this.formTItle1.TabIndex = 0;
            this.formTItle1.TitleText = "DashBoard";
            // 
            // tableBody
            // 
            this.tableBody.ColumnCount = 2;
            this.tableBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.96519F));
            this.tableBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 79.03481F));
            this.tableBody.Controls.Add(this.panLeftTop, 0, 0);
            this.tableBody.Controls.Add(this.panLeftBottom, 0, 1);
            this.tableBody.Controls.Add(this.panFrame, 1, 0);
            this.tableBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableBody.Location = new System.Drawing.Point(0, 35);
            this.tableBody.Name = "tableBody";
            this.tableBody.RowCount = 2;
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 41.32553F));
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 58.67447F));
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableBody.Size = new System.Drawing.Size(1264, 1026);
            this.tableBody.TabIndex = 1;
            this.tableBody.Paint += new System.Windows.Forms.PaintEventHandler(this.TableBody_Paint);
            // 
            // panLeftTop
            // 
            this.panLeftTop.Controls.Add(this.oshaSiteList);
            this.panLeftTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panLeftTop.Location = new System.Drawing.Point(3, 3);
            this.panLeftTop.Name = "panLeftTop";
            this.panLeftTop.Size = new System.Drawing.Size(258, 417);
            this.panLeftTop.TabIndex = 3;
            // 
            // oshaSiteList
            // 
            this.oshaSiteList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.oshaSiteList.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.oshaSiteList.GetSite = null;
            this.oshaSiteList.Location = new System.Drawing.Point(0, 0);
            this.oshaSiteList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.oshaSiteList.Name = "oshaSiteList";
            this.oshaSiteList.Size = new System.Drawing.Size(258, 417);
            this.oshaSiteList.TabIndex = 0;
            this.oshaSiteList.CellDoubleClick += new HC.OSHA.Site.Management.UI.OshaSiteList.CellDoubleClickEventHandler(this.OshaSiteList_CellDoubleClick);
            this.oshaSiteList.Load += new System.EventHandler(this.OshaSiteList_Load);
            // 
            // panLeftBottom
            // 
            this.panLeftBottom.Controls.Add(this.oshaSiteEstimateList);
            this.panLeftBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panLeftBottom.Location = new System.Drawing.Point(3, 426);
            this.panLeftBottom.Name = "panLeftBottom";
            this.panLeftBottom.Size = new System.Drawing.Size(258, 597);
            this.panLeftBottom.TabIndex = 6;
            // 
            // oshaSiteEstimateList
            // 
            this.oshaSiteEstimateList.Dock = System.Windows.Forms.DockStyle.Fill;
            hC_ESTIMATE_MODEL2.CONTRACTDATE = null;
            hC_ESTIMATE_MODEL2.CONTRACTENDDATE = null;
            hC_ESTIMATE_MODEL2.ContractPeriod = null;
            hC_ESTIMATE_MODEL2.CONTRACTSTARTDATE = null;
            hC_ESTIMATE_MODEL2.ESTIMATEDATE = null;
            hC_ESTIMATE_MODEL2.ID = ((long)(0));
            hC_ESTIMATE_MODEL2.ISCONTRACT = null;
            hC_ESTIMATE_MODEL2.RowStatus = ComBase.Mvc.RowStatus.None;
            this.oshaSiteEstimateList.GetEstimateModel = hC_ESTIMATE_MODEL2;
            this.oshaSiteEstimateList.Location = new System.Drawing.Point(0, 0);
            this.oshaSiteEstimateList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.oshaSiteEstimateList.Name = "oshaSiteEstimateList";
            this.oshaSiteEstimateList.Size = new System.Drawing.Size(258, 597);
            this.oshaSiteEstimateList.TabIndex = 1;
            this.oshaSiteEstimateList.CellDoubleClick += new HC.OSHA.Site.Management.UI.OshaSiteEstimateList.CellDoubleClickEventHandler(this.OshaSiteEstimateList_CellDoubleClick);
            // 
            // BtnWorker
            // 
            this.BtnWorker.Location = new System.Drawing.Point(701, 8);
            this.BtnWorker.Name = "BtnWorker";
            this.BtnWorker.Size = new System.Drawing.Size(88, 23);
            this.BtnWorker.TabIndex = 4;
            this.BtnWorker.Text = "근로자 관리";
            this.BtnWorker.UseVisualStyleBackColor = true;
            this.BtnWorker.Click += new System.EventHandler(this.BtnWorker_Click);
            // 
            // BtnMSDS
            // 
            this.BtnMSDS.Location = new System.Drawing.Point(810, 8);
            this.BtnMSDS.Name = "BtnMSDS";
            this.BtnMSDS.Size = new System.Drawing.Size(75, 23);
            this.BtnMSDS.TabIndex = 4;
            this.BtnMSDS.Text = "화학물질관리";
            this.BtnMSDS.UseVisualStyleBackColor = true;
            this.BtnMSDS.Click += new System.EventHandler(this.BtnMSDS_Click);
            // 
            // panFrame
            // 
            this.panFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panFrame.Location = new System.Drawing.Point(267, 3);
            this.panFrame.Name = "panFrame";
            this.tableBody.SetRowSpan(this.panFrame, 2);
            this.panFrame.Size = new System.Drawing.Size(994, 1020);
            this.panFrame.TabIndex = 7;
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 1061);
            this.Controls.Add(this.BtnMSDS);
            this.Controls.Add(this.BtnWorker);
            this.Controls.Add(this.tableBody);
            this.Controls.Add(this.formTItle1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Dashboard";
            this.Text = "대시보드";
            this.Load += new System.EventHandler(this.Dashboard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.tableBody.ResumeLayout(false);
            this.panLeftTop.ResumeLayout(false);
            this.panLeftBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComBase.Mvc.UserControls.FormTItle formTItle1;
        private System.Windows.Forms.TableLayoutPanel tableBody;
        private Management.UI.OshaSiteList oshaSiteList;
        private Management.UI.OshaSiteEstimateList oshaSiteEstimateList;
        private System.Windows.Forms.Button BtnMSDS;
        private System.Windows.Forms.Button BtnWorker;
        private System.Windows.Forms.Panel panLeftTop;
        private System.Windows.Forms.Panel panLeftBottom;
        private System.Windows.Forms.Panel panFrame;
    }
}