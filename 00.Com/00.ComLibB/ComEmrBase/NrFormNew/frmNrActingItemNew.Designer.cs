namespace ComEmrBase
{
    partial class frmNrActingItemNew
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType21 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType22 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType23 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType24 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType25 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType26 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType27 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType28 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType29 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType30 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panTime = new System.Windows.Forms.Panel();
            this.cboActTime = new System.Windows.Forms.ComboBox();
            this.lblSeqNo = new System.Windows.Forms.Label();
            this.panItem = new System.Windows.Forms.Panel();
            this.cboActItem = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.optItem = new System.Windows.Forms.RadioButton();
            this.optTime = new System.Windows.Forms.RadioButton();
            this.mbtnExit = new System.Windows.Forms.Button();
            this.mbtnDelete = new System.Windows.Forms.Button();
            this.mbtnSave = new System.Windows.Forms.Button();
            this.mbtnClear = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cboValue1 = new System.Windows.Forms.ComboBox();
            this.cboValue2 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTime = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ssActing = new FarPoint.Win.Spread.FpSpread();
            this.ssActing_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1.SuspendLayout();
            this.panTime.SuspendLayout();
            this.panItem.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssActing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssActing_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(105)))), ((int)(((byte)(72)))));
            this.panel1.Controls.Add(this.panTime);
            this.panel1.Controls.Add(this.panItem);
            this.panel1.Controls.Add(this.optItem);
            this.panel1.Controls.Add(this.optTime);
            this.panel1.Controls.Add(this.mbtnExit);
            this.panel1.Controls.Add(this.mbtnDelete);
            this.panel1.Controls.Add(this.mbtnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1107, 31);
            this.panel1.TabIndex = 3;
            // 
            // panTime
            // 
            this.panTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(105)))), ((int)(((byte)(72)))));
            this.panTime.Controls.Add(this.cboActTime);
            this.panTime.Controls.Add(this.lblSeqNo);
            this.panTime.Location = new System.Drawing.Point(515, 5);
            this.panTime.Name = "panTime";
            this.panTime.Size = new System.Drawing.Size(152, 26);
            this.panTime.TabIndex = 85;
            // 
            // cboActTime
            // 
            this.cboActTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboActTime.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboActTime.FormattingEnabled = true;
            this.cboActTime.Location = new System.Drawing.Point(67, 2);
            this.cboActTime.Name = "cboActTime";
            this.cboActTime.Size = new System.Drawing.Size(67, 21);
            this.cboActTime.TabIndex = 77;
            this.cboActTime.SelectedIndexChanged += new System.EventHandler(this.cboActTime_SelectedIndexChanged);
            // 
            // lblSeqNo
            // 
            this.lblSeqNo.AutoSize = true;
            this.lblSeqNo.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSeqNo.ForeColor = System.Drawing.Color.White;
            this.lblSeqNo.Location = new System.Drawing.Point(6, 6);
            this.lblSeqNo.Name = "lblSeqNo";
            this.lblSeqNo.Size = new System.Drawing.Size(57, 12);
            this.lblSeqNo.TabIndex = 0;
            this.lblSeqNo.Text = "수행시간";
            // 
            // panItem
            // 
            this.panItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(105)))), ((int)(((byte)(72)))));
            this.panItem.Controls.Add(this.cboActItem);
            this.panItem.Controls.Add(this.label5);
            this.panItem.Location = new System.Drawing.Point(143, 5);
            this.panItem.Name = "panItem";
            this.panItem.Size = new System.Drawing.Size(366, 26);
            this.panItem.TabIndex = 86;
            // 
            // cboActItem
            // 
            this.cboActItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboActItem.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboActItem.FormattingEnabled = true;
            this.cboActItem.Location = new System.Drawing.Point(67, 2);
            this.cboActItem.Name = "cboActItem";
            this.cboActItem.Size = new System.Drawing.Size(296, 21);
            this.cboActItem.TabIndex = 77;
            this.cboActItem.SelectedIndexChanged += new System.EventHandler(this.cboActItem_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(6, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "수행항목";
            // 
            // optItem
            // 
            this.optItem.AutoSize = true;
            this.optItem.ForeColor = System.Drawing.Color.White;
            this.optItem.Location = new System.Drawing.Point(73, 5);
            this.optItem.Name = "optItem";
            this.optItem.Size = new System.Drawing.Size(65, 21);
            this.optItem.TabIndex = 86;
            this.optItem.TabStop = true;
            this.optItem.Text = "항목별";
            this.optItem.UseVisualStyleBackColor = true;
            this.optItem.CheckedChanged += new System.EventHandler(this.optItem_CheckedChanged);
            // 
            // optTime
            // 
            this.optTime.AutoSize = true;
            this.optTime.ForeColor = System.Drawing.Color.White;
            this.optTime.Location = new System.Drawing.Point(8, 5);
            this.optTime.Name = "optTime";
            this.optTime.Size = new System.Drawing.Size(65, 21);
            this.optTime.TabIndex = 85;
            this.optTime.TabStop = true;
            this.optTime.Text = "시간별";
            this.optTime.UseVisualStyleBackColor = true;
            this.optTime.CheckedChanged += new System.EventHandler(this.optTime_CheckedChanged);
            // 
            // mbtnExit
            // 
            this.mbtnExit.Location = new System.Drawing.Point(867, 1);
            this.mbtnExit.Name = "mbtnExit";
            this.mbtnExit.Size = new System.Drawing.Size(62, 28);
            this.mbtnExit.TabIndex = 3;
            this.mbtnExit.Text = "닫기";
            this.mbtnExit.UseVisualStyleBackColor = true;
            this.mbtnExit.Click += new System.EventHandler(this.mbtnExit_Click);
            // 
            // mbtnDelete
            // 
            this.mbtnDelete.Location = new System.Drawing.Point(743, 1);
            this.mbtnDelete.Name = "mbtnDelete";
            this.mbtnDelete.Size = new System.Drawing.Size(62, 28);
            this.mbtnDelete.TabIndex = 3;
            this.mbtnDelete.Text = "삭제";
            this.mbtnDelete.UseVisualStyleBackColor = true;
            this.mbtnDelete.Click += new System.EventHandler(this.mbtnDelete_Click);
            // 
            // mbtnSave
            // 
            this.mbtnSave.Location = new System.Drawing.Point(805, 1);
            this.mbtnSave.Name = "mbtnSave";
            this.mbtnSave.Size = new System.Drawing.Size(62, 28);
            this.mbtnSave.TabIndex = 3;
            this.mbtnSave.Text = "저장";
            this.mbtnSave.UseVisualStyleBackColor = true;
            this.mbtnSave.Click += new System.EventHandler(this.mbtnSave_Click);
            // 
            // mbtnClear
            // 
            this.mbtnClear.Location = new System.Drawing.Point(154, 389);
            this.mbtnClear.Name = "mbtnClear";
            this.mbtnClear.Size = new System.Drawing.Size(62, 28);
            this.mbtnClear.TabIndex = 3;
            this.mbtnClear.Text = "Clear";
            this.mbtnClear.UseVisualStyleBackColor = true;
            this.mbtnClear.Visible = false;
            this.mbtnClear.Click += new System.EventHandler(this.mbtnClear_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "수 행";
            // 
            // cboValue1
            // 
            this.cboValue1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboValue1.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboValue1.FormattingEnabled = true;
            this.cboValue1.Location = new System.Drawing.Point(48, 47);
            this.cboValue1.Name = "cboValue1";
            this.cboValue1.Size = new System.Drawing.Size(259, 21);
            this.cboValue1.TabIndex = 78;
            this.cboValue1.SelectedIndexChanged += new System.EventHandler(this.cboValue1_SelectedIndexChanged);
            // 
            // cboValue2
            // 
            this.cboValue2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboValue2.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboValue2.FormattingEnabled = true;
            this.cboValue2.Location = new System.Drawing.Point(48, 74);
            this.cboValue2.Name = "cboValue2";
            this.cboValue2.Size = new System.Drawing.Size(259, 21);
            this.cboValue2.TabIndex = 80;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 17);
            this.label2.TabIndex = 79;
            this.label2.Text = "수 행";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 17);
            this.label3.TabIndex = 81;
            this.label3.Text = "수 행";
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(48, 100);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(259, 25);
            this.txtValue.TabIndex = 82;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 17);
            this.label4.TabIndex = 83;
            this.label4.Text = "수행시간";
            // 
            // txtTime
            // 
            this.txtTime.Location = new System.Drawing.Point(69, 11);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(65, 25);
            this.txtTime.TabIndex = 84;
            this.txtTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.txtTime);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.cboValue1);
            this.panel2.Controls.Add(this.txtValue);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.mbtnClear);
            this.panel2.Controls.Add(this.cboValue2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(790, 31);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(317, 508);
            this.panel2.TabIndex = 85;
            // 
            // ssActing
            // 
            this.ssActing.AccessibleDescription = "ssActing, Sheet1, Row 0, Column 0, ";
            this.ssActing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssActing.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssActing.Location = new System.Drawing.Point(0, 31);
            this.ssActing.Name = "ssActing";
            this.ssActing.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssActing_Sheet1});
            this.ssActing.Size = new System.Drawing.Size(790, 508);
            this.ssActing.TabIndex = 86;
            this.ssActing.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssActing_CellClick);
            // 
            // ssActing_Sheet1
            // 
            this.ssActing_Sheet1.Reset();
            this.ssActing_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssActing_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssActing_Sheet1.ColumnCount = 10;
            this.ssActing_Sheet1.RowCount = 1;
            this.ssActing_Sheet1.ActiveSkin = new FarPoint.Win.Spread.SheetSkin("CustomSkin1", System.Drawing.Color.White, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.LightGray, FarPoint.Win.Spread.GridLines.Both, System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(220)))), ((int)(((byte)(227))))), System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, false, false, false, false, false, true, false, true, "HeaderDefault", "HeaderDefault", "HeaderDefault", "DataAreaDefault", "HeaderDefault");
            this.ssActing_Sheet1.Cells.Get(0, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssActing_Sheet1.Cells.Get(0, 2).Value = "99:99";
            this.ssActing_Sheet1.Cells.Get(0, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssActing_Sheet1.Cells.Get(0, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssActing_Sheet1.Cells.Get(0, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssActing_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssActing_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssActing_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssActing_Sheet1.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.ssActing_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssActing_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssActing_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssActing_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssActing_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssActing_Sheet1.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.ssActing_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssActing_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "구 분";
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "항 목";
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "시간";
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "실시자";
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "수행내용";
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "수행내용";
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "수행내용";
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "ITEMCD";
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "EMRNO";
            this.ssActing_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "NRACTSEQ";
            this.ssActing_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssActing_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(220)))), ((int)(((byte)(227)))));
            this.ssActing_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssActing_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssActing_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssActing_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssActing_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssActing_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssActing_Sheet1.ColumnHeader.Rows.Get(0).Height = 28F;
            this.ssActing_Sheet1.Columns.Get(0).CellType = textCellType21;
            this.ssActing_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssActing_Sheet1.Columns.Get(0).Label = "구 분";
            this.ssActing_Sheet1.Columns.Get(0).Locked = true;
            this.ssActing_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssActing_Sheet1.Columns.Get(0).Width = 170F;
            this.ssActing_Sheet1.Columns.Get(1).CellType = textCellType22;
            this.ssActing_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssActing_Sheet1.Columns.Get(1).Label = "항 목";
            this.ssActing_Sheet1.Columns.Get(1).Locked = true;
            this.ssActing_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssActing_Sheet1.Columns.Get(1).Width = 170F;
            this.ssActing_Sheet1.Columns.Get(2).CellType = textCellType23;
            this.ssActing_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssActing_Sheet1.Columns.Get(2).Label = "시간";
            this.ssActing_Sheet1.Columns.Get(2).Locked = true;
            this.ssActing_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssActing_Sheet1.Columns.Get(2).Width = 46F;
            this.ssActing_Sheet1.Columns.Get(3).CellType = textCellType24;
            this.ssActing_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssActing_Sheet1.Columns.Get(3).Label = "실시자";
            this.ssActing_Sheet1.Columns.Get(3).Locked = true;
            this.ssActing_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssActing_Sheet1.Columns.Get(3).Width = 55F;
            this.ssActing_Sheet1.Columns.Get(4).CellType = textCellType25;
            this.ssActing_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssActing_Sheet1.Columns.Get(4).Label = "수행내용";
            this.ssActing_Sheet1.Columns.Get(4).Locked = true;
            this.ssActing_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssActing_Sheet1.Columns.Get(4).Width = 85F;
            this.ssActing_Sheet1.Columns.Get(5).CellType = textCellType26;
            this.ssActing_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssActing_Sheet1.Columns.Get(5).Label = "수행내용";
            this.ssActing_Sheet1.Columns.Get(5).Locked = true;
            this.ssActing_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssActing_Sheet1.Columns.Get(5).Width = 85F;
            this.ssActing_Sheet1.Columns.Get(6).CellType = textCellType27;
            this.ssActing_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssActing_Sheet1.Columns.Get(6).Label = "수행내용";
            this.ssActing_Sheet1.Columns.Get(6).Locked = true;
            this.ssActing_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssActing_Sheet1.Columns.Get(6).Width = 157F;
            this.ssActing_Sheet1.Columns.Get(7).CellType = textCellType28;
            this.ssActing_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssActing_Sheet1.Columns.Get(7).Label = "ITEMCD";
            this.ssActing_Sheet1.Columns.Get(7).Locked = true;
            this.ssActing_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssActing_Sheet1.Columns.Get(7).Visible = false;
            this.ssActing_Sheet1.Columns.Get(7).Width = 110F;
            this.ssActing_Sheet1.Columns.Get(8).CellType = textCellType29;
            this.ssActing_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssActing_Sheet1.Columns.Get(8).Label = "EMRNO";
            this.ssActing_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssActing_Sheet1.Columns.Get(8).Visible = false;
            this.ssActing_Sheet1.Columns.Get(8).Width = 110F;
            this.ssActing_Sheet1.Columns.Get(9).CellType = textCellType30;
            this.ssActing_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssActing_Sheet1.Columns.Get(9).Label = "NRACTSEQ";
            this.ssActing_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssActing_Sheet1.Columns.Get(9).Visible = false;
            this.ssActing_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssActing_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssActing_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.ssActing_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssActing_Sheet1.FilterBar.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssActing_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssActing_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssActing_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssActing_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssActing_Sheet1.FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssActing_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssActing_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssActing_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(220)))), ((int)(((byte)(227)))));
            this.ssActing_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssActing_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssActing_Sheet1.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.ssActing_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssActing_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssActing_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssActing_Sheet1.RowHeader.Visible = false;
            this.ssActing_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(220)))), ((int)(((byte)(227)))));
            this.ssActing_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssActing_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssActing_Sheet1.SheetCornerStyle.Parent = "HeaderDefault";
            this.ssActing_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssActing_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.ssActing_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmNrActingItemNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1107, 539);
            this.Controls.Add(this.ssActing);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmNrActingItemNew";
            this.Text = "frmNrActingItemNew";
            this.Load += new System.EventHandler(this.frmNrActingItemNew_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panTime.ResumeLayout(false);
            this.panTime.PerformLayout();
            this.panItem.ResumeLayout(false);
            this.panItem.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssActing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssActing_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.ComboBox cboActTime;
        private System.Windows.Forms.Button mbtnExit;
        private System.Windows.Forms.Button mbtnDelete;
        private System.Windows.Forms.Button mbtnSave;
        private System.Windows.Forms.Button mbtnClear;
        private System.Windows.Forms.Label lblSeqNo;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox cboValue1;
        public System.Windows.Forms.ComboBox cboValue2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTime;
        private System.Windows.Forms.RadioButton optItem;
        private System.Windows.Forms.RadioButton optTime;
        private System.Windows.Forms.Panel panTime;
        private System.Windows.Forms.Panel panItem;
        public System.Windows.Forms.ComboBox cboActItem;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel2;
        private FarPoint.Win.Spread.FpSpread ssActing;
        private FarPoint.Win.Spread.SheetView ssActing_Sheet1;
    }
}