namespace ComLibB
{
    partial class FrmMedViewSlipSub
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
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType1 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType13 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType14 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType15 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType16 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.ssOrdCode = new FarPoint.Win.Spread.FpSpread();
            this.ssOrdCode_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssOrdCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssOrdCode_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(420, 37);
            this.panel1.TabIndex = 0;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(359, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(55, 28);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(4, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(55, 28);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "<입력>";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // ssOrdCode
            // 
            this.ssOrdCode.AccessibleDescription = "ssOrdCode, Sheet1, Row 0, Column 0, ";
            this.ssOrdCode.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssOrdCode.Location = new System.Drawing.Point(3, 46);
            this.ssOrdCode.Name = "ssOrdCode";
            this.ssOrdCode.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssOrdCode_Sheet1});
            this.ssOrdCode.Size = new System.Drawing.Size(419, 572);
            this.ssOrdCode.TabIndex = 1;
            this.ssOrdCode.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssOrdCode_CellClick);
            this.ssOrdCode.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssOrdCode_CellDoubleClick);
            this.ssOrdCode.ButtonClicked += new FarPoint.Win.Spread.EditorNotifyEventHandler(this.ssOrdCode_ButtonClicked);
            // 
            // ssOrdCode_Sheet1
            // 
            this.ssOrdCode_Sheet1.Reset();
            this.ssOrdCode_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssOrdCode_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssOrdCode_Sheet1.ColumnCount = 17;
            this.ssOrdCode_Sheet1.RowCount = 50;
            this.ssOrdCode_Sheet1.RowHeader.ColumnCount = 0;
            this.ssOrdCode_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = " ";
            this.ssOrdCode_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "처방명";
            this.ssOrdCode_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "OrderCode";
            this.ssOrdCode_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "입력가능여부";
            this.ssOrdCode_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "GbInfo";
            this.ssOrdCode_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "금액입력";
            this.ssOrdCode_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "Bun";
            this.ssOrdCode_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "NextCode";
            this.ssOrdCode_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "SuCode";
            this.ssOrdCode_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "Info";
            this.ssOrdCode_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "GbSpec";
            this.ssOrdCode_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "DosCode";
            this.ssOrdCode_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "SlipNo";
            this.ssOrdCode_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "SpecName";
            this.ssOrdCode_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "GbImIv";
            this.ssOrdCode_Sheet1.ColumnHeader.Cells.Get(0, 15).Value = "입력순위";
            this.ssOrdCode_Sheet1.ColumnHeader.Cells.Get(0, 16).Value = "SubRate";
            checkBoxCellType1.BackgroundImage = new FarPoint.Win.Picture(null, FarPoint.Win.RenderStyle.Normal, System.Drawing.Color.Empty, 0, FarPoint.Win.HorizontalAlignment.Center, FarPoint.Win.VerticalAlignment.Center);
            this.ssOrdCode_Sheet1.Columns.Get(0).CellType = checkBoxCellType1;
            this.ssOrdCode_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(0).Label = " ";
            this.ssOrdCode_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(0).Width = 26F;
            this.ssOrdCode_Sheet1.Columns.Get(1).CellType = textCellType1;
            this.ssOrdCode_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssOrdCode_Sheet1.Columns.Get(1).Label = "처방명";
            this.ssOrdCode_Sheet1.Columns.Get(1).Locked = true;
            this.ssOrdCode_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(1).Width = 372F;
            this.ssOrdCode_Sheet1.Columns.Get(2).CellType = textCellType2;
            this.ssOrdCode_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(2).Label = "OrderCode";
            this.ssOrdCode_Sheet1.Columns.Get(2).Locked = true;
            this.ssOrdCode_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(2).Width = 72F;
            this.ssOrdCode_Sheet1.Columns.Get(3).CellType = textCellType3;
            this.ssOrdCode_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(3).Label = "입력가능여부";
            this.ssOrdCode_Sheet1.Columns.Get(3).Locked = true;
            this.ssOrdCode_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(3).Width = 79F;
            this.ssOrdCode_Sheet1.Columns.Get(4).CellType = textCellType4;
            this.ssOrdCode_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(4).Label = "GbInfo";
            this.ssOrdCode_Sheet1.Columns.Get(4).Locked = true;
            this.ssOrdCode_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(5).CellType = textCellType5;
            this.ssOrdCode_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(5).Label = "금액입력";
            this.ssOrdCode_Sheet1.Columns.Get(5).Locked = true;
            this.ssOrdCode_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(6).CellType = textCellType6;
            this.ssOrdCode_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(6).Label = "Bun";
            this.ssOrdCode_Sheet1.Columns.Get(6).Locked = true;
            this.ssOrdCode_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(7).CellType = textCellType7;
            this.ssOrdCode_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(7).Label = "NextCode";
            this.ssOrdCode_Sheet1.Columns.Get(7).Locked = true;
            this.ssOrdCode_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(7).Width = 66F;
            this.ssOrdCode_Sheet1.Columns.Get(8).CellType = textCellType8;
            this.ssOrdCode_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(8).Label = "SuCode";
            this.ssOrdCode_Sheet1.Columns.Get(8).Locked = true;
            this.ssOrdCode_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(9).CellType = textCellType9;
            this.ssOrdCode_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(9).Label = "Info";
            this.ssOrdCode_Sheet1.Columns.Get(9).Locked = true;
            this.ssOrdCode_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(10).CellType = textCellType10;
            this.ssOrdCode_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(10).Label = "GbSpec";
            this.ssOrdCode_Sheet1.Columns.Get(10).Locked = true;
            this.ssOrdCode_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(11).CellType = textCellType11;
            this.ssOrdCode_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(11).Label = "DosCode";
            this.ssOrdCode_Sheet1.Columns.Get(11).Locked = true;
            this.ssOrdCode_Sheet1.Columns.Get(11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(12).CellType = textCellType12;
            this.ssOrdCode_Sheet1.Columns.Get(12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(12).Label = "SlipNo";
            this.ssOrdCode_Sheet1.Columns.Get(12).Locked = true;
            this.ssOrdCode_Sheet1.Columns.Get(12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(13).CellType = textCellType13;
            this.ssOrdCode_Sheet1.Columns.Get(13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(13).Label = "SpecName";
            this.ssOrdCode_Sheet1.Columns.Get(13).Locked = true;
            this.ssOrdCode_Sheet1.Columns.Get(13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(13).Width = 77F;
            this.ssOrdCode_Sheet1.Columns.Get(14).CellType = textCellType14;
            this.ssOrdCode_Sheet1.Columns.Get(14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(14).Label = "GbImIv";
            this.ssOrdCode_Sheet1.Columns.Get(14).Locked = true;
            this.ssOrdCode_Sheet1.Columns.Get(14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(15).CellType = textCellType15;
            this.ssOrdCode_Sheet1.Columns.Get(15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(15).Label = "입력순위";
            this.ssOrdCode_Sheet1.Columns.Get(15).Locked = true;
            this.ssOrdCode_Sheet1.Columns.Get(15).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(16).CellType = textCellType16;
            this.ssOrdCode_Sheet1.Columns.Get(16).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssOrdCode_Sheet1.Columns.Get(16).Label = "SubRate";
            this.ssOrdCode_Sheet1.Columns.Get(16).Locked = true;
            this.ssOrdCode_Sheet1.Columns.Get(16).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssOrdCode_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            this.ssOrdCode_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssOrdCode_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(4, 625);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(423, 27);
            this.label1.TabIndex = 2;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmMedViewSlipSub
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(430, 654);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ssOrdCode);
            this.Controls.Add(this.panel1);
            this.Name = "FrmMedViewSlipSub";
            this.Text = "오더 Slip 조회";
            this.Load += new System.EventHandler(this.FrmMedViewSlipSub_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssOrdCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssOrdCode_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOK;
        private FarPoint.Win.Spread.FpSpread ssOrdCode;
        private FarPoint.Win.Spread.SheetView ssOrdCode_Sheet1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnExit;
    }
}