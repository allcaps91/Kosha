namespace ComEmrBase
{
    partial class frmEmrBaseNurseMemo
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.cboFont = new System.Windows.Forms.ComboBox();
            this.cboSize = new System.Windows.Forms.ComboBox();
            this.btnTool4 = new System.Windows.Forms.Button();
            this.btnTool3 = new System.Windows.Forms.Button();
            this.btnTool2 = new System.Windows.Forms.Button();
            this.btnTool1 = new System.Windows.Forms.Button();
            this.btnTool0 = new System.Windows.Forms.Button();
            this.btnSearch2 = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dtpBdate = new System.Windows.Forms.DateTimePicker();
            this.mbtnNextTot = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.mbtnBeforeTot = new System.Windows.Forms.Button();
            this.btnPaste = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtRich2 = new System.Windows.Forms.RichTextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.txtRich3 = new System.Windows.Forms.RichTextBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panNight = new System.Windows.Forms.Panel();
            this.txtRich = new System.Windows.Forms.RichTextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panNight.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cboFont);
            this.panel1.Controls.Add(this.cboSize);
            this.panel1.Controls.Add(this.btnTool4);
            this.panel1.Controls.Add(this.btnTool3);
            this.panel1.Controls.Add(this.btnTool2);
            this.panel1.Controls.Add(this.btnTool1);
            this.panel1.Controls.Add(this.btnTool0);
            this.panel1.Controls.Add(this.btnSearch2);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.dtpBdate);
            this.panel1.Controls.Add(this.mbtnNextTot);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.mbtnBeforeTot);
            this.panel1.Controls.Add(this.btnPaste);
            this.panel1.Controls.Add(this.btnCopy);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(507, 81);
            this.panel1.TabIndex = 4;
            // 
            // cboFont
            // 
            this.cboFont.Font = new System.Drawing.Font("굴림", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboFont.FormattingEnabled = true;
            this.cboFont.Location = new System.Drawing.Point(302, 43);
            this.cboFont.Name = "cboFont";
            this.cboFont.Size = new System.Drawing.Size(120, 25);
            this.cboFont.TabIndex = 107;
            this.cboFont.Visible = false;
            this.cboFont.SelectedIndexChanged += new System.EventHandler(this.cboFont_SelectedIndexChanged);
            // 
            // cboSize
            // 
            this.cboSize.Font = new System.Drawing.Font("굴림", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboSize.FormattingEnabled = true;
            this.cboSize.Location = new System.Drawing.Point(428, 42);
            this.cboSize.Name = "cboSize";
            this.cboSize.Size = new System.Drawing.Size(72, 25);
            this.cboSize.TabIndex = 105;
            this.cboSize.Visible = false;
            this.cboSize.SelectedIndexChanged += new System.EventHandler(this.cboSize_SelectedIndexChanged);
            // 
            // btnTool4
            // 
            this.btnTool4.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.btnTool4.ForeColor = System.Drawing.Color.Black;
            this.btnTool4.Location = new System.Drawing.Point(288, 8);
            this.btnTool4.Name = "btnTool4";
            this.btnTool4.Size = new System.Drawing.Size(45, 30);
            this.btnTool4.TabIndex = 98;
            this.btnTool4.Text = "색상";
            this.btnTool4.UseVisualStyleBackColor = true;
            this.btnTool4.Click += new System.EventHandler(this.btnTool0_Click);
            // 
            // btnTool3
            // 
            this.btnTool3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Underline))));
            this.btnTool3.Location = new System.Drawing.Point(257, 8);
            this.btnTool3.Name = "btnTool3";
            this.btnTool3.Size = new System.Drawing.Size(30, 30);
            this.btnTool3.TabIndex = 98;
            this.btnTool3.Text = "가";
            this.btnTool3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTool3.UseVisualStyleBackColor = true;
            this.btnTool3.Click += new System.EventHandler(this.btnTool0_Click);
            // 
            // btnTool2
            // 
            this.btnTool2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Strikeout))));
            this.btnTool2.Location = new System.Drawing.Point(227, 8);
            this.btnTool2.Name = "btnTool2";
            this.btnTool2.Size = new System.Drawing.Size(30, 30);
            this.btnTool2.TabIndex = 99;
            this.btnTool2.Text = "가";
            this.btnTool2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTool2.UseVisualStyleBackColor = true;
            this.btnTool2.Click += new System.EventHandler(this.btnTool0_Click);
            // 
            // btnTool1
            // 
            this.btnTool1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnTool1.Location = new System.Drawing.Point(197, 8);
            this.btnTool1.Name = "btnTool1";
            this.btnTool1.Size = new System.Drawing.Size(30, 30);
            this.btnTool1.TabIndex = 100;
            this.btnTool1.Text = "가";
            this.btnTool1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTool1.UseVisualStyleBackColor = true;
            this.btnTool1.Click += new System.EventHandler(this.btnTool0_Click);
            // 
            // btnTool0
            // 
            this.btnTool0.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Italic);
            this.btnTool0.Location = new System.Drawing.Point(167, 8);
            this.btnTool0.Name = "btnTool0";
            this.btnTool0.Size = new System.Drawing.Size(30, 30);
            this.btnTool0.TabIndex = 101;
            this.btnTool0.Text = "가";
            this.btnTool0.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTool0.UseVisualStyleBackColor = true;
            this.btnTool0.Click += new System.EventHandler(this.btnTool0_Click);
            // 
            // btnSearch2
            // 
            this.btnSearch2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnSearch2.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSearch2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSearch2.Location = new System.Drawing.Point(226, 42);
            this.btnSearch2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearch2.Name = "btnSearch2";
            this.btnSearch2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnSearch2.Size = new System.Drawing.Size(70, 26);
            this.btnSearch2.TabIndex = 97;
            this.btnSearch2.Text = "이전내역";
            this.btnSearch2.UseVisualStyleBackColor = false;
            this.btnSearch2.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSave.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSave.Location = new System.Drawing.Point(82, 42);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnSave.Size = new System.Drawing.Size(72, 26);
            this.btnSave.TabIndex = 97;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dtpBdate
            // 
            this.dtpBdate.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtpBdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpBdate.Location = new System.Drawing.Point(37, 11);
            this.dtpBdate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpBdate.Name = "dtpBdate";
            this.dtpBdate.Size = new System.Drawing.Size(97, 25);
            this.dtpBdate.TabIndex = 93;
            this.dtpBdate.ValueChanged += new System.EventHandler(this.dtpFrDateTot_ValueChanged);
            // 
            // mbtnNextTot
            // 
            this.mbtnNextTot.Font = new System.Drawing.Font("굴림", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.mbtnNextTot.Location = new System.Drawing.Point(134, 10);
            this.mbtnNextTot.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mbtnNextTot.Name = "mbtnNextTot";
            this.mbtnNextTot.Size = new System.Drawing.Size(27, 26);
            this.mbtnNextTot.TabIndex = 94;
            this.mbtnNextTot.Text = "|▶";
            this.mbtnNextTot.UseVisualStyleBackColor = true;
            this.mbtnNextTot.Click += new System.EventHandler(this.mbtnNextTot_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnExit.Location = new System.Drawing.Point(154, 42);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 26);
            this.btnExit.TabIndex = 33;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // mbtnBeforeTot
            // 
            this.mbtnBeforeTot.Font = new System.Drawing.Font("굴림", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.mbtnBeforeTot.Location = new System.Drawing.Point(10, 10);
            this.mbtnBeforeTot.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mbtnBeforeTot.Name = "mbtnBeforeTot";
            this.mbtnBeforeTot.Size = new System.Drawing.Size(27, 26);
            this.mbtnBeforeTot.TabIndex = 95;
            this.mbtnBeforeTot.Text = "◀|";
            this.mbtnBeforeTot.UseVisualStyleBackColor = true;
            this.mbtnBeforeTot.Click += new System.EventHandler(this.mbtnBeforeTot_Click);
            // 
            // btnPaste
            // 
            this.btnPaste.BackColor = System.Drawing.Color.White;
            this.btnPaste.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnPaste.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnPaste.Location = new System.Drawing.Point(418, 8);
            this.btnPaste.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnPaste.Size = new System.Drawing.Size(79, 30);
            this.btnPaste.TabIndex = 96;
            this.btnPaste.Text = "붙여넣기";
            this.btnPaste.UseVisualStyleBackColor = false;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.BackColor = System.Drawing.Color.White;
            this.btnCopy.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnCopy.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCopy.Location = new System.Drawing.Point(339, 8);
            this.btnCopy.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnCopy.Size = new System.Drawing.Size(79, 30);
            this.btnCopy.TabIndex = 96;
            this.btnCopy.Text = "복사";
            this.btnCopy.UseVisualStyleBackColor = false;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSearch.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSearch.Location = new System.Drawing.Point(10, 42);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnSearch.Size = new System.Drawing.Size(72, 26);
            this.btnSearch.TabIndex = 96;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel5, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panNight, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 81);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33444F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33444F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33112F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(507, 453);
            this.tableLayoutPanel1.TabIndex = 8;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtRich2);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 154);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(501, 145);
            this.panel2.TabIndex = 8;
            // 
            // txtRich2
            // 
            this.txtRich2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRich2.Location = new System.Drawing.Point(0, 32);
            this.txtRich2.Name = "txtRich2";
            this.txtRich2.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtRich2.Size = new System.Drawing.Size(501, 113);
            this.txtRich2.TabIndex = 7;
            this.txtRich2.Text = "";
            this.txtRich2.Click += new System.EventHandler(this.txtRich_Click);
            this.txtRich2.Enter += new System.EventHandler(this.txtRich_Enter);
            this.txtRich2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRich_KeyDown_1);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(501, 32);
            this.panel4.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(10, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Day";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.txtRich3);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 305);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(501, 145);
            this.panel5.TabIndex = 11;
            // 
            // txtRich3
            // 
            this.txtRich3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRich3.Location = new System.Drawing.Point(0, 32);
            this.txtRich3.Name = "txtRich3";
            this.txtRich3.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtRich3.Size = new System.Drawing.Size(501, 113);
            this.txtRich3.TabIndex = 7;
            this.txtRich3.Text = "";
            this.txtRich3.Click += new System.EventHandler(this.txtRich_Click);
            this.txtRich3.Enter += new System.EventHandler(this.txtRich_Enter);
            this.txtRich3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRich_KeyDown_1);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label3);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(501, 32);
            this.panel6.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(10, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Evening";
            // 
            // panNight
            // 
            this.panNight.Controls.Add(this.txtRich);
            this.panNight.Controls.Add(this.panel3);
            this.panNight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panNight.Location = new System.Drawing.Point(3, 3);
            this.panNight.Name = "panNight";
            this.panNight.Size = new System.Drawing.Size(501, 145);
            this.panNight.TabIndex = 6;
            // 
            // txtRich
            // 
            this.txtRich.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRich.Location = new System.Drawing.Point(0, 32);
            this.txtRich.Name = "txtRich";
            this.txtRich.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtRich.Size = new System.Drawing.Size(501, 113);
            this.txtRich.TabIndex = 7;
            this.txtRich.Text = "";
            this.txtRich.Click += new System.EventHandler(this.txtRich_Click);
            this.txtRich.Enter += new System.EventHandler(this.txtRich_Enter);
            this.txtRich.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRich_KeyDown_1);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(501, 32);
            this.panel3.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(10, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Night";
            // 
            // frmEmrBaseNurseMemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 534);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.Name = "frmEmrBaseNurseMemo";
            this.Text = "frmEmrBaseNurseMemo";
            this.Load += new System.EventHandler(this.frmEmrBaseNurseMemo_Load);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panNight.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker dtpBdate;
        private System.Windows.Forms.Button mbtnNextTot;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button mbtnBeforeTot;
        public System.Windows.Forms.Button btnSearch;
        public System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox cboSize;
        private System.Windows.Forms.Button btnTool3;
        private System.Windows.Forms.Button btnTool2;
        private System.Windows.Forms.Button btnTool1;
        private System.Windows.Forms.Button btnTool0;
        private System.Windows.Forms.ComboBox cboFont;
        private System.Windows.Forms.Button btnTool4;
        public System.Windows.Forms.Button btnSearch2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RichTextBox txtRich2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panNight;
        private System.Windows.Forms.RichTextBox txtRich;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.RichTextBox txtRich3;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.Button btnCopy;
        public System.Windows.Forms.Button btnPaste;
    }
}