namespace HC.Core
{
    partial class UI_Guide
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
            this.BtnSave = new System.Windows.Forms.Button();
            this.panTop = new System.Windows.Forms.Panel();
            this.BtnCodeSave = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panFooter = new System.Windows.Forms.Panel();
            this.panBody = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.txtExSort = new System.Windows.Forms.NumericUpDown();
            this.cboTongBun = new System.Windows.Forms.ComboBox();
            this.txtHName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panSpace = new System.Windows.Forms.Panel();
            this.tableBody = new System.Windows.Forms.TableLayoutPanel();
            this.panCodeButtons = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.BtnCodeAdd = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.SSGroupCodeList = new FarPoint.Win.Spread.FpSpread();
            this.SSGroupCodeList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtExSort)).BeginInit();
            this.panCodeButtons.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSGroupCodeList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSGroupCodeList_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(668, 312);
            this.BtnSave.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 27);
            this.BtnSave.TabIndex = 0;
            this.BtnSave.Text = "저장(&S)";
            this.BtnSave.UseVisualStyleBackColor = true;
            // 
            // panTop
            // 
            this.panTop.Controls.Add(this.BtnCodeSave);
            this.panTop.Controls.Add(this.comboBox1);
            this.panTop.Controls.Add(this.lblTitle);
            this.panTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTop.Location = new System.Drawing.Point(0, 0);
            this.panTop.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.panTop.Name = "panTop";
            this.panTop.Size = new System.Drawing.Size(1264, 107);
            this.panTop.TabIndex = 1;
            // 
            // BtnCodeSave
            // 
            this.BtnCodeSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCodeSave.Location = new System.Drawing.Point(638, 35);
            this.BtnCodeSave.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnCodeSave.Name = "BtnCodeSave";
            this.BtnCodeSave.Size = new System.Drawing.Size(75, 28);
            this.BtnCodeSave.TabIndex = 9;
            this.BtnCodeSave.Text = "저장(&S)";
            this.BtnCodeSave.UseVisualStyleBackColor = true;
            this.BtnCodeSave.Click += new System.EventHandler(this.BtnCodeSave_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(322, 61);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 25);
            this.comboBox1.TabIndex = 6;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(7, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(74, 21);
            this.lblTitle.TabIndex = 5;
            this.lblTitle.Text = "폼타이틀";
            // 
            // panFooter
            // 
            this.panFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panFooter.Location = new System.Drawing.Point(0, 495);
            this.panFooter.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.panFooter.Name = "panFooter";
            this.panFooter.Size = new System.Drawing.Size(1264, 142);
            this.panFooter.TabIndex = 2;
            // 
            // panBody
            // 
            this.panBody.Location = new System.Drawing.Point(0, 107);
            this.panBody.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.panBody.Name = "panBody";
            this.panBody.Padding = new System.Windows.Forms.Padding(5);
            this.panBody.Size = new System.Drawing.Size(1264, 90);
            this.panBody.TabIndex = 3;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(777, 313);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(116, 25);
            this.textBox1.TabIndex = 25;
            // 
            // txtExSort
            // 
            this.txtExSort.Location = new System.Drawing.Point(678, 376);
            this.txtExSort.Name = "txtExSort";
            this.txtExSort.Size = new System.Drawing.Size(176, 25);
            this.txtExSort.TabIndex = 24;
            this.txtExSort.Tag = "EXSORT";
            // 
            // cboTongBun
            // 
            this.cboTongBun.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTongBun.FormattingEnabled = true;
            this.cboTongBun.Location = new System.Drawing.Point(341, 375);
            this.cboTongBun.Name = "cboTongBun";
            this.cboTongBun.Size = new System.Drawing.Size(321, 25);
            this.cboTongBun.TabIndex = 23;
            this.cboTongBun.Tag = "TONGBUN";
            // 
            // txtHName
            // 
            this.txtHName.Location = new System.Drawing.Point(341, 313);
            this.txtHName.Name = "txtHName";
            this.txtHName.Size = new System.Drawing.Size(321, 25);
            this.txtHName.TabIndex = 22;
            this.txtHName.Tag = "HNAME";
            this.txtHName.Text = "txtHName";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(231, 375);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(3);
            this.label7.Size = new System.Drawing.Size(104, 25);
            this.label7.TabIndex = 21;
            this.label7.Text = "통계분류";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panSpace
            // 
            this.panSpace.Location = new System.Drawing.Point(250, 249);
            this.panSpace.Name = "panSpace";
            this.panSpace.Size = new System.Drawing.Size(715, 5);
            this.panSpace.TabIndex = 26;
            // 
            // tableBody
            // 
            this.tableBody.ColumnCount = 2;
            this.tableBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableBody.Location = new System.Drawing.Point(939, 360);
            this.tableBody.Name = "tableBody";
            this.tableBody.RowCount = 2;
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableBody.Size = new System.Drawing.Size(200, 100);
            this.tableBody.TabIndex = 27;
            // 
            // panCodeButtons
            // 
            this.panCodeButtons.BackColor = System.Drawing.Color.White;
            this.panCodeButtons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panCodeButtons.Controls.Add(this.label2);
            this.panCodeButtons.Controls.Add(this.BtnCodeAdd);
            this.panCodeButtons.Location = new System.Drawing.Point(71, 264);
            this.panCodeButtons.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.panCodeButtons.Name = "panCodeButtons";
            this.panCodeButtons.Size = new System.Drawing.Size(719, 39);
            this.panCodeButtons.TabIndex = 28;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(12, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "부제목";
            // 
            // BtnCodeAdd
            // 
            this.BtnCodeAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCodeAdd.Location = new System.Drawing.Point(553, 4);
            this.BtnCodeAdd.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnCodeAdd.Name = "BtnCodeAdd";
            this.BtnCodeAdd.Size = new System.Drawing.Size(75, 28);
            this.BtnCodeAdd.TabIndex = 6;
            this.BtnCodeAdd.Text = "추가(&A)";
            this.BtnCodeAdd.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(275, 316);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(715, 5);
            this.panel1.TabIndex = 29;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(45, 456);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(932, 31);
            this.panel2.TabIndex = 30;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "부제목";
            // 
            // SSGroupCodeList
            // 
            this.SSGroupCodeList.AccessibleDescription = "";
            this.SSGroupCodeList.Location = new System.Drawing.Point(25, 326);
            this.SSGroupCodeList.Name = "SSGroupCodeList";
            this.SSGroupCodeList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSGroupCodeList_Sheet1});
            this.SSGroupCodeList.Size = new System.Drawing.Size(200, 100);
            this.SSGroupCodeList.TabIndex = 31;
            // 
            // SSGroupCodeList_Sheet1
            // 
            this.SSGroupCodeList_Sheet1.Reset();
            this.SSGroupCodeList_Sheet1.SheetName = "Sheet1";
            // 
            // UI_Guide
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1264, 637);
            this.Controls.Add(this.SSGroupCodeList);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panCodeButtons);
            this.Controls.Add(this.tableBody);
            this.Controls.Add(this.panSpace);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.panBody);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.panFooter);
            this.Controls.Add(this.txtExSort);
            this.Controls.Add(this.panTop);
            this.Controls.Add(this.txtHName);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.cboTongBun);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "UI_Guide";
            this.Text = "UI_Guide";
            this.Load += new System.EventHandler(this.UI_Guide_Load);
            this.panTop.ResumeLayout(false);
            this.panTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtExSort)).EndInit();
            this.panCodeButtons.ResumeLayout(false);
            this.panCodeButtons.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSGroupCodeList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSGroupCodeList_Sheet1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Panel panTop;
        private System.Windows.Forms.Panel panFooter;
        private System.Windows.Forms.Panel panBody;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.NumericUpDown txtExSort;
        private System.Windows.Forms.ComboBox cboTongBun;
        private System.Windows.Forms.TextBox txtHName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panSpace;
        private System.Windows.Forms.TableLayoutPanel tableBody;
        private System.Windows.Forms.Panel panCodeButtons;
        private System.Windows.Forms.Button BtnCodeSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtnCodeAdd;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private FarPoint.Win.Spread.FpSpread SSGroupCodeList;
        private FarPoint.Win.Spread.SheetView SSGroupCodeList_Sheet1;
    }
}