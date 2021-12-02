namespace ComNurLibB
{
    partial class frmNrCodeTongBuild
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
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.grbDate = new System.Windows.Forms.GroupBox();
            this.cboDate = new System.Windows.Forms.ComboBox();
            this.grbWork = new System.Windows.Forms.GroupBox();
            this.chkGun = new System.Windows.Forms.CheckBox();
            this.chkW = new System.Windows.Forms.CheckBox();
            this.chkInjection = new System.Windows.Forms.CheckBox();
            this.chkPRN = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtBlue = new System.Windows.Forms.TextBox();
            this.lblComp = new System.Windows.Forms.Label();
            this.txtRed = new System.Windows.Forms.TextBox();
            this.lblError = new System.Windows.Forms.Label();
            this.grbMessage = new System.Windows.Forms.GroupBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.ssLog = new FarPoint.Win.Spread.FpSpread();
            this.ssLog_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            this.grbDate.SuspendLayout();
            this.grbWork.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.grbMessage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssLog_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Controls.Add(this.btnStart);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(697, 34);
            this.panTitle.TabIndex = 36;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(621, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 39;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(183, 16);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "간호부 각종 통계 build";
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.BackColor = System.Drawing.Color.Transparent;
            this.btnStart.Location = new System.Drawing.Point(543, 0);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(72, 30);
            this.btnStart.TabIndex = 40;
            this.btnStart.Text = "시작";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // grbDate
            // 
            this.grbDate.Controls.Add(this.cboDate);
            this.grbDate.Location = new System.Drawing.Point(12, 40);
            this.grbDate.Name = "grbDate";
            this.grbDate.Size = new System.Drawing.Size(167, 81);
            this.grbDate.TabIndex = 37;
            this.grbDate.TabStop = false;
            this.grbDate.Text = "작업년월";
            // 
            // cboDate
            // 
            this.cboDate.FormattingEnabled = true;
            this.cboDate.Location = new System.Drawing.Point(23, 35);
            this.cboDate.Name = "cboDate";
            this.cboDate.Size = new System.Drawing.Size(121, 20);
            this.cboDate.TabIndex = 2;
            this.cboDate.SelectedIndexChanged += new System.EventHandler(this.cboDate_SelectedIndexChanged);
            this.cboDate.Enter += new System.EventHandler(this.cboDate_Enter);
            this.cboDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboDate_KeyPress);
            // 
            // grbWork
            // 
            this.grbWork.Controls.Add(this.chkGun);
            this.grbWork.Controls.Add(this.chkW);
            this.grbWork.Controls.Add(this.chkInjection);
            this.grbWork.Controls.Add(this.chkPRN);
            this.grbWork.Location = new System.Drawing.Point(185, 40);
            this.grbWork.Name = "grbWork";
            this.grbWork.Size = new System.Drawing.Size(145, 133);
            this.grbWork.TabIndex = 38;
            this.grbWork.TabStop = false;
            this.grbWork.Text = "작업구분";
            // 
            // chkGun
            // 
            this.chkGun.AutoSize = true;
            this.chkGun.Location = new System.Drawing.Point(25, 104);
            this.chkGun.Name = "chkGun";
            this.chkGun.Size = new System.Drawing.Size(82, 16);
            this.chkGun.TabIndex = 42;
            this.chkGun.Text = "4.근무형태";
            this.chkGun.UseVisualStyleBackColor = true;
            // 
            // chkW
            // 
            this.chkW.AutoSize = true;
            this.chkW.Location = new System.Drawing.Point(25, 20);
            this.chkW.Name = "chkW";
            this.chkW.Size = new System.Drawing.Size(86, 16);
            this.chkW.TabIndex = 39;
            this.chkW.Text = "1.각 병동별";
            this.chkW.UseVisualStyleBackColor = true;
            // 
            // chkInjection
            // 
            this.chkInjection.AutoSize = true;
            this.chkInjection.Location = new System.Drawing.Point(25, 76);
            this.chkInjection.Name = "chkInjection";
            this.chkInjection.Size = new System.Drawing.Size(110, 16);
            this.chkInjection.TabIndex = 41;
            this.chkInjection.Text = "3.주사실 통계 B";
            this.chkInjection.UseVisualStyleBackColor = true;
            // 
            // chkPRN
            // 
            this.chkPRN.AutoSize = true;
            this.chkPRN.Location = new System.Drawing.Point(25, 48);
            this.chkPRN.Name = "chkPRN";
            this.chkPRN.Size = new System.Drawing.Size(118, 16);
            this.chkPRN.TabIndex = 40;
            this.chkPRN.Text = "2.외래 통계 Build";
            this.chkPRN.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtBlue);
            this.groupBox3.Controls.Add(this.lblComp);
            this.groupBox3.Controls.Add(this.txtRed);
            this.groupBox3.Controls.Add(this.lblError);
            this.groupBox3.Location = new System.Drawing.Point(12, 127);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(167, 46);
            this.groupBox3.TabIndex = 38;
            this.groupBox3.TabStop = false;
            // 
            // txtBlue
            // 
            this.txtBlue.BackColor = System.Drawing.Color.Blue;
            this.txtBlue.Location = new System.Drawing.Point(84, 15);
            this.txtBlue.Name = "txtBlue";
            this.txtBlue.Size = new System.Drawing.Size(19, 21);
            this.txtBlue.TabIndex = 41;
            // 
            // lblComp
            // 
            this.lblComp.AutoSize = true;
            this.lblComp.Location = new System.Drawing.Point(103, 21);
            this.lblComp.Name = "lblComp";
            this.lblComp.Size = new System.Drawing.Size(61, 12);
            this.lblComp.TabIndex = 43;
            this.lblComp.Text = "Build 완료";
            // 
            // txtRed
            // 
            this.txtRed.BackColor = System.Drawing.Color.Red;
            this.txtRed.Location = new System.Drawing.Point(2, 15);
            this.txtRed.Name = "txtRed";
            this.txtRed.Size = new System.Drawing.Size(19, 21);
            this.txtRed.TabIndex = 40;
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Location = new System.Drawing.Point(21, 21);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(64, 12);
            this.lblError.TabIndex = 42;
            this.lblError.Text = "Bulid Error";
            // 
            // grbMessage
            // 
            this.grbMessage.Controls.Add(this.txtMessage);
            this.grbMessage.Location = new System.Drawing.Point(12, 179);
            this.grbMessage.Name = "grbMessage";
            this.grbMessage.Size = new System.Drawing.Size(316, 81);
            this.grbMessage.TabIndex = 39;
            this.grbMessage.TabStop = false;
            this.grbMessage.Text = "Message";
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(15, 33);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(293, 21);
            this.txtMessage.TabIndex = 44;
            // 
            // ssLog
            // 
            this.ssLog.AccessibleDescription = "fpSpread1, Sheet1, Row 0, Column 0, ";
            this.ssLog.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssLog.Location = new System.Drawing.Point(349, 40);
            this.ssLog.Name = "ssLog";
            this.ssLog.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssLog_Sheet1});
            this.ssLog.Size = new System.Drawing.Size(342, 219);
            this.ssLog.TabIndex = 40;
            // 
            // ssLog_Sheet1
            // 
            this.ssLog_Sheet1.Reset();
            this.ssLog_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssLog_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssLog_Sheet1.ColumnCount = 2;
            this.ssLog_Sheet1.RowCount = 10;
            this.ssLog_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "작업년월";
            this.ssLog_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "작업로그";
            this.ssLog_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssLog_Sheet1.Columns.Get(0).Label = "작업년월";
            this.ssLog_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssLog_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssLog_Sheet1.Columns.Get(1).Label = "작업로그";
            this.ssLog_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssLog_Sheet1.Columns.Get(1).Width = 234F;
            this.ssLog_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssLog_Sheet1.RowHeader.Columns.Get(0).Width = 27F;
            this.ssLog_Sheet1.Rows.Get(0).Height = 22F;
            this.ssLog_Sheet1.Rows.Get(1).Height = 22F;
            this.ssLog_Sheet1.Rows.Get(2).Height = 22F;
            this.ssLog_Sheet1.Rows.Get(3).Height = 22F;
            this.ssLog_Sheet1.Rows.Get(4).Height = 22F;
            this.ssLog_Sheet1.Rows.Get(5).Height = 22F;
            this.ssLog_Sheet1.Rows.Get(6).Height = 22F;
            this.ssLog_Sheet1.Rows.Get(7).Height = 22F;
            this.ssLog_Sheet1.Rows.Get(8).Height = 22F;
            this.ssLog_Sheet1.Rows.Get(9).Height = 22F;
            this.ssLog_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmNrCodeTongBuild
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(697, 266);
            this.Controls.Add(this.ssLog);
            this.Controls.Add(this.grbMessage);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.grbWork);
            this.Controls.Add(this.grbDate);
            this.Controls.Add(this.panTitle);
            this.Name = "frmNrCodeTongBuild";
            this.Text = "간호부 각종 통계 build";
            this.Load += new System.EventHandler(this.frmNrCodeTongBuild_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.grbDate.ResumeLayout(false);
            this.grbWork.ResumeLayout(false);
            this.grbWork.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.grbMessage.ResumeLayout(false);
            this.grbMessage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssLog_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox grbDate;
        private System.Windows.Forms.GroupBox grbWork;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cboDate;
        private System.Windows.Forms.CheckBox chkGun;
        private System.Windows.Forms.CheckBox chkW;
        private System.Windows.Forms.CheckBox chkInjection;
        private System.Windows.Forms.CheckBox chkPRN;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.GroupBox grbMessage;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.TextBox txtBlue;
        private System.Windows.Forms.Label lblComp;
        private System.Windows.Forms.TextBox txtRed;
        private System.Windows.Forms.Label lblError;
        private FarPoint.Win.Spread.FpSpread ssLog;
        private FarPoint.Win.Spread.SheetView ssLog_Sheet1;
    }
}