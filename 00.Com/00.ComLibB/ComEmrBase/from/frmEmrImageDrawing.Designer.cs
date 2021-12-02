namespace ComEmrBase
{
    partial class frmEmrImageDrawing
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEmrImageDrawing));
            this.panel1 = new System.Windows.Forms.Panel();
            this.mbtnPrint = new System.Windows.Forms.Button();
            this.panChartDate = new System.Windows.Forms.Panel();
            this.txtMedFrTime = new System.Windows.Forms.ComboBox();
            this.mbtnTime = new System.Windows.Forms.Button();
            this.lblChartTime = new System.Windows.Forms.Label();
            this.lblChartDate = new System.Windows.Forms.Label();
            this.dtMedFrDate = new System.Windows.Forms.DateTimePicker();
            this.mbtnClear = new System.Windows.Forms.Button();
            this.mbtnClearBack = new System.Windows.Forms.Button();
            this.mbtnSaveUserImage = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.mbtnDelete = new System.Windows.Forms.Button();
            this.mbtnSave = new System.Windows.Forms.Button();
            this.mbtnExit = new System.Windows.Forms.Button();
            this.optAll = new System.Windows.Forms.RadioButton();
            this.mbtnLoadBackImage = new System.Windows.Forms.Button();
            this.ssImage = new FarPoint.Win.Spread.FpSpread();
            this.ssImage_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.mtsImageList1 = new mtsImgList.mtsImageList();
            this.panel3 = new System.Windows.Forms.Panel();
            this.optDept = new System.Windows.Forms.RadioButton();
            this.optUse = new System.Windows.Forms.RadioButton();
            this.mbtnSmartPhonImage = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.mtsImgMain1 = new mtsImgList.mtsImgMain();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lblImageName = new System.Windows.Forms.Label();
            this.axActXForm1 = new AxProjectActXForm.AxActXForm();
            this.panel1.SuspendLayout();
            this.panChartDate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssImage_Sheet1)).BeginInit();
            this.panel2.SuspendLayout();
            this.mtsImageList1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axActXForm1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.mbtnPrint);
            this.panel1.Controls.Add(this.panChartDate);
            this.panel1.Controls.Add(this.mbtnClear);
            this.panel1.Controls.Add(this.mbtnClearBack);
            this.panel1.Controls.Add(this.mbtnSaveUserImage);
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Controls.Add(this.mbtnDelete);
            this.panel1.Controls.Add(this.mbtnSave);
            this.panel1.Controls.Add(this.mbtnExit);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(850, 38);
            this.panel1.TabIndex = 1;
            // 
            // mbtnPrint
            // 
            this.mbtnPrint.Location = new System.Drawing.Point(772, 2);
            this.mbtnPrint.Name = "mbtnPrint";
            this.mbtnPrint.Size = new System.Drawing.Size(75, 30);
            this.mbtnPrint.TabIndex = 174;
            this.mbtnPrint.Text = "출 력";
            this.mbtnPrint.UseVisualStyleBackColor = true;
            this.mbtnPrint.Visible = false;
            this.mbtnPrint.Click += new System.EventHandler(this.mbtnPrint_Click);
            // 
            // panChartDate
            // 
            this.panChartDate.Controls.Add(this.txtMedFrTime);
            this.panChartDate.Controls.Add(this.mbtnTime);
            this.panChartDate.Controls.Add(this.lblChartTime);
            this.panChartDate.Controls.Add(this.lblChartDate);
            this.panChartDate.Controls.Add(this.dtMedFrDate);
            this.panChartDate.Location = new System.Drawing.Point(0, 1);
            this.panChartDate.Name = "panChartDate";
            this.panChartDate.Size = new System.Drawing.Size(285, 33);
            this.panChartDate.TabIndex = 173;
            // 
            // txtMedFrTime
            // 
            this.txtMedFrTime.FormattingEnabled = true;
            this.txtMedFrTime.Location = new System.Drawing.Point(202, 6);
            this.txtMedFrTime.Name = "txtMedFrTime";
            this.txtMedFrTime.Size = new System.Drawing.Size(53, 20);
            this.txtMedFrTime.TabIndex = 31;
            this.txtMedFrTime.Text = "00:00";
            // 
            // mbtnTime
            // 
            this.mbtnTime.Location = new System.Drawing.Point(255, 2);
            this.mbtnTime.Name = "mbtnTime";
            this.mbtnTime.Size = new System.Drawing.Size(27, 28);
            this.mbtnTime.TabIndex = 30;
            this.mbtnTime.Text = "T";
            this.mbtnTime.UseVisualStyleBackColor = true;
            this.mbtnTime.Click += new System.EventHandler(this.mbtnTime_Click);
            // 
            // lblChartTime
            // 
            this.lblChartTime.AutoSize = true;
            this.lblChartTime.Location = new System.Drawing.Point(173, 10);
            this.lblChartTime.Name = "lblChartTime";
            this.lblChartTime.Size = new System.Drawing.Size(29, 12);
            this.lblChartTime.TabIndex = 29;
            this.lblChartTime.Text = "시간";
            // 
            // lblChartDate
            // 
            this.lblChartDate.AutoSize = true;
            this.lblChartDate.Location = new System.Drawing.Point(10, 10);
            this.lblChartDate.Name = "lblChartDate";
            this.lblChartDate.Size = new System.Drawing.Size(53, 12);
            this.lblChartDate.TabIndex = 28;
            this.lblChartDate.Text = "작성일자";
            // 
            // dtMedFrDate
            // 
            this.dtMedFrDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtMedFrDate.Location = new System.Drawing.Point(63, 6);
            this.dtMedFrDate.Name = "dtMedFrDate";
            this.dtMedFrDate.Size = new System.Drawing.Size(98, 21);
            this.dtMedFrDate.TabIndex = 27;
            // 
            // mbtnClear
            // 
            this.mbtnClear.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.mbtnClear.Location = new System.Drawing.Point(546, 2);
            this.mbtnClear.Name = "mbtnClear";
            this.mbtnClear.Size = new System.Drawing.Size(75, 30);
            this.mbtnClear.TabIndex = 172;
            this.mbtnClear.Text = "Clear";
            this.mbtnClear.UseVisualStyleBackColor = true;
            this.mbtnClear.Click += new System.EventHandler(this.mbtnClear_Click);
            // 
            // mbtnClearBack
            // 
            this.mbtnClearBack.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.mbtnClearBack.Location = new System.Drawing.Point(471, 2);
            this.mbtnClearBack.Name = "mbtnClearBack";
            this.mbtnClearBack.Size = new System.Drawing.Size(75, 30);
            this.mbtnClearBack.TabIndex = 171;
            this.mbtnClearBack.Text = "배경해제";
            this.mbtnClearBack.UseVisualStyleBackColor = true;
            this.mbtnClearBack.Click += new System.EventHandler(this.mbtnClearBack_Click);
            // 
            // mbtnSaveUserImage
            // 
            this.mbtnSaveUserImage.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.mbtnSaveUserImage.Location = new System.Drawing.Point(364, 2);
            this.mbtnSaveUserImage.Name = "mbtnSaveUserImage";
            this.mbtnSaveUserImage.Size = new System.Drawing.Size(105, 30);
            this.mbtnSaveUserImage.TabIndex = 169;
            this.mbtnSaveUserImage.Text = "상용이미지 등록";
            this.mbtnSaveUserImage.UseVisualStyleBackColor = true;
            this.mbtnSaveUserImage.Click += new System.EventHandler(this.mbtnSaveUserImage_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(10, 11);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(124, 16);
            this.lblTitle.TabIndex = 167;
            this.lblTitle.Text = "Image Drawing";
            this.lblTitle.Visible = false;
            // 
            // mbtnDelete
            // 
            this.mbtnDelete.Location = new System.Drawing.Point(696, 2);
            this.mbtnDelete.Name = "mbtnDelete";
            this.mbtnDelete.Size = new System.Drawing.Size(75, 30);
            this.mbtnDelete.TabIndex = 166;
            this.mbtnDelete.Text = "삭 제";
            this.mbtnDelete.UseVisualStyleBackColor = true;
            this.mbtnDelete.Click += new System.EventHandler(this.mbtnDelete_Click);
            // 
            // mbtnSave
            // 
            this.mbtnSave.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.mbtnSave.Location = new System.Drawing.Point(621, 2);
            this.mbtnSave.Name = "mbtnSave";
            this.mbtnSave.Size = new System.Drawing.Size(75, 30);
            this.mbtnSave.TabIndex = 165;
            this.mbtnSave.Text = "저 장";
            this.mbtnSave.UseVisualStyleBackColor = true;
            this.mbtnSave.Click += new System.EventHandler(this.mbtnSave_Click);
            // 
            // mbtnExit
            // 
            this.mbtnExit.Location = new System.Drawing.Point(772, 2);
            this.mbtnExit.Name = "mbtnExit";
            this.mbtnExit.Size = new System.Drawing.Size(75, 30);
            this.mbtnExit.TabIndex = 164;
            this.mbtnExit.Text = "닫 기";
            this.mbtnExit.UseVisualStyleBackColor = true;
            this.mbtnExit.Click += new System.EventHandler(this.mbtnExit_Click);
            // 
            // optAll
            // 
            this.optAll.AutoSize = true;
            this.optAll.Location = new System.Drawing.Point(137, 11);
            this.optAll.Name = "optAll";
            this.optAll.Size = new System.Drawing.Size(47, 16);
            this.optAll.TabIndex = 2;
            this.optAll.TabStop = true;
            this.optAll.Text = "전체";
            this.optAll.UseVisualStyleBackColor = true;
            this.optAll.Visible = false;
            this.optAll.CheckedChanged += new System.EventHandler(this.optAll_CheckedChanged);
            // 
            // mbtnLoadBackImage
            // 
            this.mbtnLoadBackImage.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.mbtnLoadBackImage.Location = new System.Drawing.Point(85, 3);
            this.mbtnLoadBackImage.Name = "mbtnLoadBackImage";
            this.mbtnLoadBackImage.Size = new System.Drawing.Size(75, 30);
            this.mbtnLoadBackImage.TabIndex = 170;
            this.mbtnLoadBackImage.Text = "배 경";
            this.mbtnLoadBackImage.UseVisualStyleBackColor = true;
            this.mbtnLoadBackImage.Visible = false;
            this.mbtnLoadBackImage.Click += new System.EventHandler(this.mbtnLoadBackImage_Click);
            // 
            // ssImage
            // 
            this.ssImage.AccessibleDescription = "ssImage, Sheet1, Row 0, Column 0, ";
            this.ssImage.Location = new System.Drawing.Point(3, 3);
            this.ssImage.Name = "ssImage";
            this.ssImage.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssImage_Sheet1});
            this.ssImage.Size = new System.Drawing.Size(76, 29);
            this.ssImage.TabIndex = 168;
            this.ssImage.Visible = false;
            // 
            // ssImage_Sheet1
            // 
            this.ssImage_Sheet1.Reset();
            this.ssImage_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssImage_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssImage_Sheet1.ColumnCount = 3;
            this.ssImage_Sheet1.RowCount = 1;
            this.ssImage_Sheet1.Columns.Get(0).CellType = textCellType7;
            this.ssImage_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssImage_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssImage_Sheet1.Columns.Get(0).Width = 115F;
            this.ssImage_Sheet1.Columns.Get(1).CellType = textCellType8;
            this.ssImage_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssImage_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssImage_Sheet1.Columns.Get(1).Width = 115F;
            this.ssImage_Sheet1.Columns.Get(2).CellType = textCellType9;
            this.ssImage_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssImage_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssImage_Sheet1.Columns.Get(2).Width = 140F;
            this.ssImage_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssImage_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.mtsImageList1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.mbtnSmartPhonImage);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(661, 38);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(189, 761);
            this.panel2.TabIndex = 2;
            // 
            // mtsImageList1
            // 
            this.mtsImageList1.AutoScroll = true;
            this.mtsImageList1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mtsImageList1.Controls.Add(this.ssImage);
            this.mtsImageList1.Controls.Add(this.mbtnLoadBackImage);
            this.mtsImageList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtsImageList1.Highlight = System.Drawing.Color.Blue;
            this.mtsImageList1.HighLightMode = mtsImgList.iHighliteMode.GlassAndBackGround;
            this.mtsImageList1.Location = new System.Drawing.Point(0, 82);
            this.mtsImageList1.Name = "mtsImageList1";
            this.mtsImageList1.SelectedColor = System.Drawing.Color.Blue;
            this.mtsImageList1.Size = new System.Drawing.Size(189, 458);
            this.mtsImageList1.TabIndex = 4;
            this.mtsImageList1.tbSize = 100;
            this.mtsImageList1.ThumbnailBack = System.Drawing.Color.Gray;
            this.mtsImageList1.ThumbnailBorder = System.Drawing.Color.Black;
            this.mtsImageList1.ThumbnailClick += new mtsImgList.mtsImageList.ThumbnailClickEventHandler(this.mtsImageList1_ThumbnailClick);
            this.mtsImageList1.ThumbnailDoubleClick += new mtsImgList.mtsImageList.ThumbnailDoubleClickEventHandler(this.mtsImageList1_ThumbnailDoubleClick);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.optAll);
            this.panel3.Controls.Add(this.optDept);
            this.panel3.Controls.Add(this.optUse);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 44);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(189, 38);
            this.panel3.TabIndex = 2;
            // 
            // optDept
            // 
            this.optDept.AutoSize = true;
            this.optDept.Location = new System.Drawing.Point(84, 11);
            this.optDept.Name = "optDept";
            this.optDept.Size = new System.Drawing.Size(35, 16);
            this.optDept.TabIndex = 1;
            this.optDept.TabStop = true;
            this.optDept.Text = "과";
            this.optDept.UseVisualStyleBackColor = true;
            this.optDept.CheckedChanged += new System.EventHandler(this.optDept_CheckedChanged);
            // 
            // optUse
            // 
            this.optUse.AutoSize = true;
            this.optUse.Location = new System.Drawing.Point(15, 11);
            this.optUse.Name = "optUse";
            this.optUse.Size = new System.Drawing.Size(47, 16);
            this.optUse.TabIndex = 0;
            this.optUse.TabStop = true;
            this.optUse.Text = "개인";
            this.optUse.UseVisualStyleBackColor = true;
            this.optUse.CheckedChanged += new System.EventHandler(this.optUse_CheckedChanged);
            // 
            // mbtnSmartPhonImage
            // 
            this.mbtnSmartPhonImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.mbtnSmartPhonImage.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.mbtnSmartPhonImage.ForeColor = System.Drawing.Color.Green;
            this.mbtnSmartPhonImage.Location = new System.Drawing.Point(0, 0);
            this.mbtnSmartPhonImage.Name = "mbtnSmartPhonImage";
            this.mbtnSmartPhonImage.Size = new System.Drawing.Size(189, 44);
            this.mbtnSmartPhonImage.TabIndex = 5;
            this.mbtnSmartPhonImage.Text = "핸드폰 촬영 이미지 보기";
            this.mbtnSmartPhonImage.UseVisualStyleBackColor = true;
            this.mbtnSmartPhonImage.Visible = false;
            this.mbtnSmartPhonImage.Click += new System.EventHandler(this.mbtnSmartPhonImage_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.mtsImgMain1);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 540);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(189, 221);
            this.panel4.TabIndex = 3;
            // 
            // mtsImgMain1
            // 
            this.mtsImgMain1.AutoScroll = true;
            this.mtsImgMain1.BackColor = System.Drawing.Color.Gray;
            this.mtsImgMain1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mtsImgMain1.Dock = System.Windows.Forms.DockStyle.Top;
            this.mtsImgMain1.Location = new System.Drawing.Point(0, 31);
            this.mtsImgMain1.Name = "mtsImgMain1";
            this.mtsImgMain1.Size = new System.Drawing.Size(189, 190);
            this.mtsImgMain1.TabIndex = 4;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel5.Controls.Add(this.lblImageName);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(189, 31);
            this.panel5.TabIndex = 3;
            // 
            // lblImageName
            // 
            this.lblImageName.AutoSize = true;
            this.lblImageName.Location = new System.Drawing.Point(5, 8);
            this.lblImageName.Name = "lblImageName";
            this.lblImageName.Size = new System.Drawing.Size(87, 12);
            this.lblImageName.TabIndex = 4;
            this.lblImageName.Text = "lblImageName";
            // 
            // axActXForm1
            // 
            this.axActXForm1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axActXForm1.Location = new System.Drawing.Point(0, 38);
            this.axActXForm1.Name = "axActXForm1";
            this.axActXForm1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axActXForm1.OcxState")));
            this.axActXForm1.Size = new System.Drawing.Size(661, 761);
            this.axActXForm1.TabIndex = 3;
            // 
            // frmEmrImageDrawing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 799);
            this.ControlBox = false;
            this.Controls.Add(this.axActXForm1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmEmrImageDrawing";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Image Drawing";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmEmrImageDrawing_FormClosed);
            this.Load += new System.EventHandler(this.frmEmrImageDrawing_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panChartDate.ResumeLayout(false);
            this.panChartDate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssImage_Sheet1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.mtsImageList1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axActXForm1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton optAll;
        private System.Windows.Forms.RadioButton optDept;
        private System.Windows.Forms.RadioButton optUse;
        private mtsImgList.mtsImageList mtsImageList1;
        private System.Windows.Forms.Button mbtnDelete;
        private System.Windows.Forms.Button mbtnSave;
        private System.Windows.Forms.Button mbtnExit;
        private System.Windows.Forms.Label lblTitle;
        private FarPoint.Win.Spread.FpSpread ssImage;
        private FarPoint.Win.Spread.SheetView ssImage_Sheet1;
        private mtsImgList.mtsImgMain mtsImgMain1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label lblImageName;
        private System.Windows.Forms.Button mbtnSaveUserImage;
        private System.Windows.Forms.Button mbtnClearBack;
        private System.Windows.Forms.Button mbtnLoadBackImage;
        private AxProjectActXForm.AxActXForm axActXForm1;
        private System.Windows.Forms.Button mbtnClear;
        private System.Windows.Forms.Panel panChartDate;
        public System.Windows.Forms.ComboBox txtMedFrTime;
        public System.Windows.Forms.Button mbtnTime;
        public System.Windows.Forms.Label lblChartTime;
        public System.Windows.Forms.Label lblChartDate;
        public System.Windows.Forms.DateTimePicker dtMedFrDate;
        private System.Windows.Forms.Button mbtnPrint;
        private System.Windows.Forms.Button mbtnSmartPhonImage;
    }
}