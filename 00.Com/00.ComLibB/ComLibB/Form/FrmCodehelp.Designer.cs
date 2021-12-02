namespace ComLibB
{
    partial class FrmCodehelp
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.pan1 = new System.Windows.Forms.Panel();
            this.pan3 = new System.Windows.Forms.Panel();
            this.lblMsg = new System.Windows.Forms.Label();
            this.pan2 = new System.Windows.Forms.Panel();
            this.ssSuga = new FarPoint.Win.Spread.FpSpread();
            this.ssSuga_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.trvList = new System.Windows.Forms.TreeView();
            this.pan0 = new System.Windows.Forms.Panel();
            this.grbView = new System.Windows.Forms.GroupBox();
            this.txtData = new System.Windows.Forms.TextBox();
            this.grbBun = new System.Windows.Forms.GroupBox();
            this.optBun5 = new System.Windows.Forms.RadioButton();
            this.optBun4 = new System.Windows.Forms.RadioButton();
            this.optBun3 = new System.Windows.Forms.RadioButton();
            this.optBun2 = new System.Windows.Forms.RadioButton();
            this.optBun1 = new System.Windows.Forms.RadioButton();
            this.optBun0 = new System.Windows.Forms.RadioButton();
            this.grbGbn = new System.Windows.Forms.GroupBox();
            this.optGbn2 = new System.Windows.Forms.RadioButton();
            this.optGbn1 = new System.Windows.Forms.RadioButton();
            this.optGbn0 = new System.Windows.Forms.RadioButton();
            this.lblTitlesub = new System.Windows.Forms.Label();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.grbJong = new System.Windows.Forms.GroupBox();
            this.optJong5 = new System.Windows.Forms.RadioButton();
            this.optJong4 = new System.Windows.Forms.RadioButton();
            this.optJong3 = new System.Windows.Forms.RadioButton();
            this.optJong2 = new System.Windows.Forms.RadioButton();
            this.optJong1 = new System.Windows.Forms.RadioButton();
            this.optJong0 = new System.Windows.Forms.RadioButton();
            this.pan1.SuspendLayout();
            this.pan3.SuspendLayout();
            this.pan2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssSuga)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssSuga_Sheet1)).BeginInit();
            this.pan0.SuspendLayout();
            this.grbView.SuspendLayout();
            this.grbBun.SuspendLayout();
            this.grbGbn.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.grbJong.SuspendLayout();
            this.SuspendLayout();
            // 
            // pan1
            // 
            this.pan1.Controls.Add(this.pan3);
            this.pan1.Controls.Add(this.pan2);
            this.pan1.Controls.Add(this.pan0);
            this.pan1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pan1.Location = new System.Drawing.Point(0, 62);
            this.pan1.Name = "pan1";
            this.pan1.Size = new System.Drawing.Size(483, 468);
            this.pan1.TabIndex = 15;
            // 
            // pan3
            // 
            this.pan3.Controls.Add(this.lblMsg);
            this.pan3.Dock = System.Windows.Forms.DockStyle.Top;
            this.pan3.Location = new System.Drawing.Point(0, 424);
            this.pan3.Name = "pan3";
            this.pan3.Size = new System.Drawing.Size(483, 43);
            this.pan3.TabIndex = 2;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblMsg.Location = new System.Drawing.Point(215, 12);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(55, 16);
            this.lblMsg.TabIndex = 0;
            this.lblMsg.Text = "label1";
            // 
            // pan2
            // 
            this.pan2.Controls.Add(this.ssSuga);
            this.pan2.Controls.Add(this.trvList);
            this.pan2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pan2.Location = new System.Drawing.Point(0, 96);
            this.pan2.Name = "pan2";
            this.pan2.Size = new System.Drawing.Size(483, 328);
            this.pan2.TabIndex = 1;
            // 
            // ssSuga
            // 
            this.ssSuga.AccessibleDescription = "ssSuga, Sheet1, Row 0, Column 0, ";
            this.ssSuga.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssSuga.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssSuga.Location = new System.Drawing.Point(0, 0);
            this.ssSuga.Name = "ssSuga";
            this.ssSuga.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssSuga_Sheet1});
            this.ssSuga.Size = new System.Drawing.Size(483, 328);
            this.ssSuga.TabIndex = 1;
            this.ssSuga.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssSuga.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssSuga_CellDoubleClick);
            this.ssSuga.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ssSuga_KeyPress);
            // 
            // ssSuga_Sheet1
            // 
            this.ssSuga_Sheet1.Reset();
            this.ssSuga_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssSuga_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssSuga_Sheet1.ColumnCount = 3;
            this.ssSuga_Sheet1.RowCount = 1;
            this.ssSuga_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "xViewCode1";
            this.ssSuga_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "xViewCode2";
            this.ssSuga_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "xViewName";
            this.ssSuga_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssSuga_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSuga_Sheet1.Columns.Get(0).Label = "xViewCode1";
            this.ssSuga_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSuga_Sheet1.Columns.Get(0).Width = 90F;
            this.ssSuga_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssSuga_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSuga_Sheet1.Columns.Get(1).Label = "xViewCode2";
            this.ssSuga_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSuga_Sheet1.Columns.Get(1).Width = 90F;
            this.ssSuga_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssSuga_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSuga_Sheet1.Columns.Get(2).Label = "xViewName";
            this.ssSuga_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSuga_Sheet1.Columns.Get(2).Width = 90F;
            this.ssSuga_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssSuga_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // trvList
            // 
            this.trvList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvList.Location = new System.Drawing.Point(0, 0);
            this.trvList.Name = "trvList";
            this.trvList.Size = new System.Drawing.Size(483, 328);
            this.trvList.TabIndex = 0;
            // 
            // pan0
            // 
            this.pan0.BackColor = System.Drawing.Color.White;
            this.pan0.Controls.Add(this.grbView);
            this.pan0.Controls.Add(this.grbBun);
            this.pan0.Controls.Add(this.grbGbn);
            this.pan0.Dock = System.Windows.Forms.DockStyle.Top;
            this.pan0.Location = new System.Drawing.Point(0, 0);
            this.pan0.Name = "pan0";
            this.pan0.Size = new System.Drawing.Size(483, 96);
            this.pan0.TabIndex = 0;
            // 
            // grbView
            // 
            this.grbView.Controls.Add(this.txtData);
            this.grbView.Location = new System.Drawing.Point(276, 6);
            this.grbView.Name = "grbView";
            this.grbView.Size = new System.Drawing.Size(200, 85);
            this.grbView.TabIndex = 44;
            this.grbView.TabStop = false;
            this.grbView.Text = "찾을 자료는?";
            // 
            // txtData
            // 
            this.txtData.Location = new System.Drawing.Point(23, 36);
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(154, 21);
            this.txtData.TabIndex = 0;
            this.txtData.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtData_KeyPress);
            // 
            // grbBun
            // 
            this.grbBun.Controls.Add(this.grbJong);
            this.grbBun.Controls.Add(this.optBun5);
            this.grbBun.Controls.Add(this.optBun4);
            this.grbBun.Controls.Add(this.optBun3);
            this.grbBun.Controls.Add(this.optBun2);
            this.grbBun.Controls.Add(this.optBun1);
            this.grbBun.Controls.Add(this.optBun0);
            this.grbBun.Location = new System.Drawing.Point(112, 6);
            this.grbBun.Name = "grbBun";
            this.grbBun.Size = new System.Drawing.Size(158, 85);
            this.grbBun.TabIndex = 43;
            this.grbBun.TabStop = false;
            this.grbBun.Text = "수가분류";
            // 
            // optBun5
            // 
            this.optBun5.AutoSize = true;
            this.optBun5.Location = new System.Drawing.Point(94, 64);
            this.optBun5.Name = "optBun5";
            this.optBun5.Size = new System.Drawing.Size(47, 16);
            this.optBun5.TabIndex = 48;
            this.optBun5.TabStop = true;
            this.optBun5.Text = "기타";
            this.optBun5.UseVisualStyleBackColor = true;
            // 
            // optBun4
            // 
            this.optBun4.AutoSize = true;
            this.optBun4.Location = new System.Drawing.Point(94, 42);
            this.optBun4.Name = "optBun4";
            this.optBun4.Size = new System.Drawing.Size(59, 16);
            this.optBun4.TabIndex = 47;
            this.optBun4.TabStop = true;
            this.optBun4.Text = "방사선";
            this.optBun4.UseVisualStyleBackColor = true;
            // 
            // optBun3
            // 
            this.optBun3.AutoSize = true;
            this.optBun3.Location = new System.Drawing.Point(94, 20);
            this.optBun3.Name = "optBun3";
            this.optBun3.Size = new System.Drawing.Size(59, 16);
            this.optBun3.TabIndex = 46;
            this.optBun3.TabStop = true;
            this.optBun3.Text = "감사료";
            this.optBun3.UseVisualStyleBackColor = true;
            // 
            // optBun2
            // 
            this.optBun2.AutoSize = true;
            this.optBun2.Location = new System.Drawing.Point(9, 64);
            this.optBun2.Name = "optBun2";
            this.optBun2.Size = new System.Drawing.Size(79, 16);
            this.optBun2.TabIndex = 45;
            this.optBun2.TabStop = true;
            this.optBun2.Text = "처치, 수술";
            this.optBun2.UseVisualStyleBackColor = true;
            // 
            // optBun1
            // 
            this.optBun1.AutoSize = true;
            this.optBun1.Location = new System.Drawing.Point(9, 42);
            this.optBun1.Name = "optBun1";
            this.optBun1.Size = new System.Drawing.Size(67, 16);
            this.optBun1.TabIndex = 44;
            this.optBun1.TabStop = true;
            this.optBun1.Text = "약, 주사";
            this.optBun1.UseVisualStyleBackColor = true;
            // 
            // optBun0
            // 
            this.optBun0.AutoSize = true;
            this.optBun0.Location = new System.Drawing.Point(9, 20);
            this.optBun0.Name = "optBun0";
            this.optBun0.Size = new System.Drawing.Size(79, 16);
            this.optBun0.TabIndex = 43;
            this.optBun0.TabStop = true;
            this.optBun0.Text = "진찰, 입원";
            this.optBun0.UseVisualStyleBackColor = true;
            // 
            // grbGbn
            // 
            this.grbGbn.Controls.Add(this.optGbn2);
            this.grbGbn.Controls.Add(this.optGbn1);
            this.grbGbn.Controls.Add(this.optGbn0);
            this.grbGbn.Location = new System.Drawing.Point(12, 6);
            this.grbGbn.Name = "grbGbn";
            this.grbGbn.Size = new System.Drawing.Size(94, 85);
            this.grbGbn.TabIndex = 42;
            this.grbGbn.TabStop = false;
            this.grbGbn.Text = "찾기방법";
            // 
            // optGbn2
            // 
            this.optGbn2.AutoSize = true;
            this.optGbn2.Location = new System.Drawing.Point(12, 64);
            this.optGbn2.Name = "optGbn2";
            this.optGbn2.Size = new System.Drawing.Size(71, 16);
            this.optGbn2.TabIndex = 42;
            this.optGbn2.TabStop = true;
            this.optGbn2.Text = "조합코드";
            this.optGbn2.UseVisualStyleBackColor = true;
            // 
            // optGbn1
            // 
            this.optGbn1.AutoSize = true;
            this.optGbn1.Location = new System.Drawing.Point(12, 42);
            this.optGbn1.Name = "optGbn1";
            this.optGbn1.Size = new System.Drawing.Size(71, 16);
            this.optGbn1.TabIndex = 41;
            this.optGbn1.TabStop = true;
            this.optGbn1.Text = "조합코드";
            this.optGbn1.UseVisualStyleBackColor = true;
            // 
            // optGbn0
            // 
            this.optGbn0.AutoSize = true;
            this.optGbn0.Location = new System.Drawing.Point(12, 20);
            this.optGbn0.Name = "optGbn0";
            this.optGbn0.Size = new System.Drawing.Size(71, 16);
            this.optGbn0.TabIndex = 40;
            this.optGbn0.TabStop = true;
            this.optGbn0.Text = "조합명칭";
            this.optGbn0.UseVisualStyleBackColor = true;
            // 
            // lblTitlesub
            // 
            this.lblTitlesub.AutoSize = true;
            this.lblTitlesub.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitlesub.ForeColor = System.Drawing.Color.White;
            this.lblTitlesub.Location = new System.Drawing.Point(8, 6);
            this.lblTitlesub.Name = "lblTitlesub";
            this.lblTitlesub.Size = new System.Drawing.Size(62, 12);
            this.lblTitlesub.TabIndex = 0;
            this.lblTitlesub.Text = "찾기 옵션";
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitlesub);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(483, 28);
            this.panTitleSub0.TabIndex = 14;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Location = new System.Drawing.Point(404, 1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(72, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "닫 기";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(82, 16);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "코드 찾기";
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Controls.Add(this.btnClose);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(483, 34);
            this.panTitle.TabIndex = 13;
            // 
            // grbJong
            // 
            this.grbJong.Controls.Add(this.optJong5);
            this.grbJong.Controls.Add(this.optJong4);
            this.grbJong.Controls.Add(this.optJong3);
            this.grbJong.Controls.Add(this.optJong2);
            this.grbJong.Controls.Add(this.optJong1);
            this.grbJong.Controls.Add(this.optJong0);
            this.grbJong.Location = new System.Drawing.Point(0, 0);
            this.grbJong.Name = "grbJong";
            this.grbJong.Size = new System.Drawing.Size(158, 85);
            this.grbJong.TabIndex = 44;
            this.grbJong.TabStop = false;
            this.grbJong.Text = "작업선택";
            // 
            // optJong5
            // 
            this.optJong5.AutoSize = true;
            this.optJong5.Location = new System.Drawing.Point(83, 64);
            this.optJong5.Name = "optJong5";
            this.optJong5.Size = new System.Drawing.Size(47, 16);
            this.optJong5.TabIndex = 48;
            this.optJong5.TabStop = true;
            this.optJong5.Text = "전체";
            this.optJong5.UseVisualStyleBackColor = true;
            // 
            // optJong4
            // 
            this.optJong4.AutoSize = true;
            this.optJong4.Location = new System.Drawing.Point(83, 42);
            this.optJong4.Name = "optJong4";
            this.optJong4.Size = new System.Drawing.Size(59, 16);
            this.optJong4.TabIndex = 47;
            this.optJong4.TabStop = true;
            this.optJong4.Text = "계약처";
            this.optJong4.UseVisualStyleBackColor = true;
            // 
            // optJong3
            // 
            this.optJong3.AutoSize = true;
            this.optJong3.Location = new System.Drawing.Point(83, 20);
            this.optJong3.Name = "optJong3";
            this.optJong3.Size = new System.Drawing.Size(71, 16);
            this.optJong3.TabIndex = 46;
            this.optJong3.TabStop = true;
            this.optJong3.Text = "의료보호";
            this.optJong3.UseVisualStyleBackColor = true;
            // 
            // optJong2
            // 
            this.optJong2.AutoSize = true;
            this.optJong2.Location = new System.Drawing.Point(9, 64);
            this.optJong2.Name = "optJong2";
            this.optJong2.Size = new System.Drawing.Size(75, 16);
            this.optJong2.TabIndex = 45;
            this.optJong2.TabStop = true;
            this.optJong2.Text = "직장 조합";
            this.optJong2.UseVisualStyleBackColor = true;
            // 
            // optJong1
            // 
            this.optJong1.AutoSize = true;
            this.optJong1.Location = new System.Drawing.Point(9, 42);
            this.optJong1.Name = "optJong1";
            this.optJong1.Size = new System.Drawing.Size(75, 16);
            this.optJong1.TabIndex = 44;
            this.optJong1.TabStop = true;
            this.optJong1.Text = "지역 조합";
            this.optJong1.UseVisualStyleBackColor = true;
            // 
            // optJong0
            // 
            this.optJong0.AutoSize = true;
            this.optJong0.Location = new System.Drawing.Point(9, 20);
            this.optJong0.Name = "optJong0";
            this.optJong0.Size = new System.Drawing.Size(59, 16);
            this.optJong0.TabIndex = 43;
            this.optJong0.TabStop = true;
            this.optJong0.Text = "공무원";
            this.optJong0.UseVisualStyleBackColor = true;
            // 
            // FrmCodehelp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(483, 530);
            this.Controls.Add(this.pan1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Name = "FrmCodehelp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "코드찾기";
            this.Activated += new System.EventHandler(this.FrmCodehelp_Activated);
            this.Load += new System.EventHandler(this.FrmCodehelp_Load);
            this.pan1.ResumeLayout(false);
            this.pan3.ResumeLayout(false);
            this.pan3.PerformLayout();
            this.pan2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssSuga)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssSuga_Sheet1)).EndInit();
            this.pan0.ResumeLayout(false);
            this.grbView.ResumeLayout(false);
            this.grbView.PerformLayout();
            this.grbBun.ResumeLayout(false);
            this.grbBun.PerformLayout();
            this.grbGbn.ResumeLayout(false);
            this.grbGbn.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.grbJong.ResumeLayout(false);
            this.grbJong.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pan1;
        private System.Windows.Forms.Panel pan2;
        private System.Windows.Forms.Panel pan0;
        private System.Windows.Forms.GroupBox grbView;
        private System.Windows.Forms.GroupBox grbBun;
        private System.Windows.Forms.GroupBox grbGbn;
        private System.Windows.Forms.Panel pan3;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.TreeView trvList;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.RadioButton optBun5;
        private System.Windows.Forms.RadioButton optBun4;
        private System.Windows.Forms.RadioButton optBun3;
        private System.Windows.Forms.RadioButton optBun2;
        private System.Windows.Forms.RadioButton optBun1;
        private System.Windows.Forms.RadioButton optBun0;
        private System.Windows.Forms.RadioButton optGbn2;
        private System.Windows.Forms.RadioButton optGbn1;
        private System.Windows.Forms.RadioButton optGbn0;
        private System.Windows.Forms.Label lblTitlesub;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panTitle;
        private FarPoint.Win.Spread.FpSpread ssSuga;
        private FarPoint.Win.Spread.SheetView ssSuga_Sheet1;
        private System.Windows.Forms.GroupBox grbJong;
        private System.Windows.Forms.RadioButton optJong5;
        private System.Windows.Forms.RadioButton optJong4;
        private System.Windows.Forms.RadioButton optJong3;
        private System.Windows.Forms.RadioButton optJong2;
        private System.Windows.Forms.RadioButton optJong1;
        private System.Windows.Forms.RadioButton optJong0;
    }
}