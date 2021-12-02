namespace ComLibB
{
    partial class frmHoliday
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHoliday));
            this.orbDate = new System.Windows.Forms.GroupBox();
            this.btnSuch = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.ssHoliday = new FarPoint.Win.Spread.FpSpread();
            this.ssHoliday_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.orbDate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssHoliday)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssHoliday_Sheet1)).BeginInit();
            this.panTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // orbDate
            // 
            this.orbDate.Controls.Add(this.btnSuch);
            this.orbDate.Controls.Add(this.dateTimePicker1);
            this.orbDate.Location = new System.Drawing.Point(11, 36);
            this.orbDate.Name = "orbDate";
            this.orbDate.Size = new System.Drawing.Size(190, 49);
            this.orbDate.TabIndex = 54;
            this.orbDate.TabStop = false;
            this.orbDate.Text = "작업년월";
            // 
            // btnSuch
            // 
            this.btnSuch.BackColor = System.Drawing.Color.Transparent;
            this.btnSuch.Location = new System.Drawing.Point(112, 9);
            this.btnSuch.Name = "btnSuch";
            this.btnSuch.Size = new System.Drawing.Size(72, 30);
            this.btnSuch.TabIndex = 57;
            this.btnSuch.Text = "조회";
            this.btnSuch.UseVisualStyleBackColor = false;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(8, 14);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(88, 21);
            this.dateTimePicker1.TabIndex = 56;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // ssHoliday
            // 
            this.ssHoliday.AccessibleDescription = "ssHoliday, Sheet1, Row 0, Column 0, ";
            this.ssHoliday.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssHoliday.Location = new System.Drawing.Point(12, 91);
            this.ssHoliday.Name = "ssHoliday";
            this.ssHoliday.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssHoliday_Sheet1});
            this.ssHoliday.Size = new System.Drawing.Size(566, 279);
            this.ssHoliday.TabIndex = 55;
            this.ssHoliday.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssHoliday.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpSpread1_CellClick);
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnCancel);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Controls.Add(this.btnSave);
            this.panTitle.Controls.Add(this.btnClose);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(604, 34);
            this.panTitle.TabIndex = 56;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Location = new System.Drawing.Point(451, 1);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 30);
            this.btnCancel.TabIndex = 58;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(99, 16);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "공휴일 등록";
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.Location = new System.Drawing.Point(377, 1);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 30);
            this.btnSave.TabIndex = 57;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Location = new System.Drawing.Point(525, 1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(72, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "닫기";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // frmHoliday
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(604, 379);
            this.Controls.Add(this.panTitle);
            this.Controls.Add(this.ssHoliday);
            this.Controls.Add(this.orbDate);
            this.Name = "frmHoliday";
            this.Text = "공휴일등록";
            this.orbDate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssHoliday)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssHoliday_Sheet1)).EndInit();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox orbDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private FarPoint.Win.Spread.FpSpread ssHoliday;
        private FarPoint.Win.Spread.SheetView ssHoliday_Sheet1;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSuch;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
    }
}