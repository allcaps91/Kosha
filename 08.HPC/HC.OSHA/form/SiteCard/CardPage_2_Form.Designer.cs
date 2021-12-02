namespace HC_OSHA
{
    partial class CardPage_2_Form
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.TxtPrint = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnPrint = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.BtnGet = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.BtnGetData = new System.Windows.Forms.Button();
            this.panDiagram = new System.Windows.Forms.Panel();
            this.TxtTASKDIAGRAM = new System.Windows.Forms.TextBox();
            this.BtnDelete = new System.Windows.Forms.Button();
            this.BtnSave = new System.Windows.Forms.Button();
            this.contentTitle1 = new ComBase.Mvc.UserControls.ContentTitle();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.SSList2 = new FarPoint.Win.Spread.FpSpread();
            this.SSList2_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.BtnSave2 = new System.Windows.Forms.Button();
            this.contentTitle2 = new ComBase.Mvc.UserControls.ContentTitle();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panDiagram.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSList2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList2_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(872, 631);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.TxtPrint);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(864, 601);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "인쇄보기";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // TxtPrint
            // 
            this.TxtPrint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TxtPrint.Location = new System.Drawing.Point(3, 39);
            this.TxtPrint.MaxLength = 32767000;
            this.TxtPrint.Multiline = true;
            this.TxtPrint.Name = "TxtPrint";
            this.TxtPrint.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtPrint.Size = new System.Drawing.Size(858, 559);
            this.TxtPrint.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.BtnPrint);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(858, 36);
            this.panel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(16, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(328, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "4. 업무(작업) 개요(2) - 업무(작업)절차도";
            // 
            // BtnPrint
            // 
            this.BtnPrint.Location = new System.Drawing.Point(610, 5);
            this.BtnPrint.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.BtnPrint.Name = "BtnPrint";
            this.BtnPrint.Size = new System.Drawing.Size(65, 27);
            this.BtnPrint.TabIndex = 6;
            this.BtnPrint.Text = "인쇄(&P)";
            this.BtnPrint.UseVisualStyleBackColor = true;
            this.BtnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.BtnGet);
            this.tabPage2.Controls.Add(this.label15);
            this.tabPage2.Controls.Add(this.BtnGetData);
            this.tabPage2.Controls.Add(this.panDiagram);
            this.tabPage2.Controls.Add(this.BtnDelete);
            this.tabPage2.Controls.Add(this.BtnSave);
            this.tabPage2.Controls.Add(this.contentTitle1);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(864, 601);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "업무작업개요(2)";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // BtnGet
            // 
            this.BtnGet.Location = new System.Drawing.Point(534, 7);
            this.BtnGet.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnGet.Name = "BtnGet";
            this.BtnGet.Size = new System.Drawing.Size(87, 28);
            this.BtnGet.TabIndex = 111;
            this.BtnGet.Text = "관리감독자";
            this.BtnGet.UseVisualStyleBackColor = true;
            this.BtnGet.Click += new System.EventHandler(this.BtnGet_Click);
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label15.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.Location = new System.Drawing.Point(157, 7);
            this.label15.Name = "label15";
            this.label15.Padding = new System.Windows.Forms.Padding(3);
            this.label15.Size = new System.Drawing.Size(179, 25);
            this.label15.TabIndex = 108;
            this.label15.Text = "업무(작업) 절차도";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnGetData
            // 
            this.BtnGetData.Location = new System.Drawing.Point(443, 7);
            this.BtnGetData.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnGetData.Name = "BtnGetData";
            this.BtnGetData.Size = new System.Drawing.Size(75, 28);
            this.BtnGetData.TabIndex = 110;
            this.BtnGetData.Text = "추가항목 가져오기";
            this.BtnGetData.UseVisualStyleBackColor = true;
            this.BtnGetData.Click += new System.EventHandler(this.BtnGetData_Click);
            // 
            // panDiagram
            // 
            this.panDiagram.Controls.Add(this.TxtTASKDIAGRAM);
            this.panDiagram.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panDiagram.Location = new System.Drawing.Point(3, 41);
            this.panDiagram.Name = "panDiagram";
            this.panDiagram.Size = new System.Drawing.Size(858, 557);
            this.panDiagram.TabIndex = 109;
            // 
            // TxtTASKDIAGRAM
            // 
            this.TxtTASKDIAGRAM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TxtTASKDIAGRAM.Location = new System.Drawing.Point(0, 0);
            this.TxtTASKDIAGRAM.MaxLength = 32767000;
            this.TxtTASKDIAGRAM.Multiline = true;
            this.TxtTASKDIAGRAM.Name = "TxtTASKDIAGRAM";
            this.TxtTASKDIAGRAM.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtTASKDIAGRAM.Size = new System.Drawing.Size(858, 557);
            this.TxtTASKDIAGRAM.TabIndex = 0;
            // 
            // BtnDelete
            // 
            this.BtnDelete.Location = new System.Drawing.Point(663, 7);
            this.BtnDelete.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(75, 28);
            this.BtnDelete.TabIndex = 106;
            this.BtnDelete.Text = "삭제";
            this.BtnDelete.UseVisualStyleBackColor = true;
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(744, 7);
            this.BtnSave.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 28);
            this.BtnSave.TabIndex = 105;
            this.BtnSave.Text = "저장(&S)";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // contentTitle1
            // 
            this.contentTitle1.Dock = System.Windows.Forms.DockStyle.Top;
            this.contentTitle1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.contentTitle1.Location = new System.Drawing.Point(3, 3);
            this.contentTitle1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.contentTitle1.Name = "contentTitle1";
            this.contentTitle1.Size = new System.Drawing.Size(858, 38);
            this.contentTitle1.TabIndex = 104;
            this.contentTitle1.TitleText = "4. 업무(작업)개요(2)";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.SSList2);
            this.tabPage3.Controls.Add(this.BtnAdd);
            this.tabPage3.Controls.Add(this.BtnSave2);
            this.tabPage3.Controls.Add(this.contentTitle2);
            this.tabPage3.Location = new System.Drawing.Point(4, 26);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(864, 601);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "관리감독자";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // SSList2
            // 
            this.SSList2.AccessibleDescription = "";
            this.SSList2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SSList2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSList2.Location = new System.Drawing.Point(0, 38);
            this.SSList2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SSList2.Name = "SSList2";
            this.SSList2.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSList2_Sheet1});
            this.SSList2.Size = new System.Drawing.Size(864, 563);
            this.SSList2.TabIndex = 112;
            // 
            // SSList2_Sheet1
            // 
            this.SSList2_Sheet1.Reset();
            this.SSList2_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSList2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSList2_Sheet1.ColumnCount = 1;
            this.SSList2_Sheet1.RowCount = 1;
            this.SSList2_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList2_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList2_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSList2_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList2_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList2_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList2_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSList2_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList2_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSList2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // BtnAdd
            // 
            this.BtnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnAdd.Location = new System.Drawing.Point(529, 2);
            this.BtnAdd.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(75, 28);
            this.BtnAdd.TabIndex = 111;
            this.BtnAdd.Text = "추가";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // BtnSave2
            // 
            this.BtnSave2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSave2.Location = new System.Drawing.Point(610, 2);
            this.BtnSave2.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSave2.Name = "BtnSave2";
            this.BtnSave2.Size = new System.Drawing.Size(75, 28);
            this.BtnSave2.TabIndex = 110;
            this.BtnSave2.Text = "저장(&S)";
            this.BtnSave2.UseVisualStyleBackColor = true;
            this.BtnSave2.Click += new System.EventHandler(this.BtnSave2_Click);
            // 
            // contentTitle2
            // 
            this.contentTitle2.Dock = System.Windows.Forms.DockStyle.Top;
            this.contentTitle2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.contentTitle2.Location = new System.Drawing.Point(0, 0);
            this.contentTitle2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.contentTitle2.Name = "contentTitle2";
            this.contentTitle2.Size = new System.Drawing.Size(864, 38);
            this.contentTitle2.TabIndex = 107;
            this.contentTitle2.TitleText = "";
            // 
            // CardPage_2_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 631);
            this.Controls.Add(this.tabControl1);
            this.Name = "CardPage_2_Form";
            this.Text = "업무작업개요 1";
            this.Load += new System.EventHandler(this.CardPage_2_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.panDiagram.ResumeLayout(false);
            this.panDiagram.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSList2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList2_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button BtnPrint;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button BtnSave;
        private ComBase.Mvc.UserControls.ContentTitle contentTitle1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button BtnDelete;
        private System.Windows.Forms.Panel panDiagram;
        private System.Windows.Forms.TabPage tabPage3;
        private ComBase.Mvc.UserControls.ContentTitle contentTitle2;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.Button BtnSave2;
        private FarPoint.Win.Spread.FpSpread SSList2;
        private FarPoint.Win.Spread.SheetView SSList2_Sheet1;
        private System.Windows.Forms.Button BtnGetData;
        private System.Windows.Forms.TextBox TxtTASKDIAGRAM;
        private System.Windows.Forms.TextBox TxtPrint;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnGet;
    }
}