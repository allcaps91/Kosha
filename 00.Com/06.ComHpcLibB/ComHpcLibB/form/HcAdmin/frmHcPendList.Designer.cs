namespace ComHpcLibB
{
    partial class frmHcPendList
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
            FarPoint.Win.Spread.FlatFocusIndicatorRenderer flatFocusIndicatorRenderer1 = new FarPoint.Win.Spread.FlatFocusIndicatorRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer1 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer2 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panSub05 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rdoGubun3 = new System.Windows.Forms.RadioButton();
            this.rdoGubun2 = new System.Windows.Forms.RadioButton();
            this.rdoGubun1 = new System.Windows.Forms.RadioButton();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSName = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkChul1 = new System.Windows.Forms.CheckBox();
            this.chkChul0 = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rdoJob2 = new System.Windows.Forms.RadioButton();
            this.rdoJob1 = new System.Windows.Forms.RadioButton();
            this.rdoJob0 = new System.Windows.Forms.RadioButton();
            this.panDate = new System.Windows.Forms.Panel();
            this.dtpTDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panMain = new System.Windows.Forms.Panel();
            this.SS1 = new FarPoint.Win.Spread.FpSpread();
            this.SS1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.Progress = new DevComponents.DotNetBar.Controls.CircularProgress();
            this.panTitle.SuspendLayout();
            this.panSub05.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panDate.SuspendLayout();
            this.panMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1264, 38);
            this.panTitle.TabIndex = 15;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(11, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(112, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "보류대장 관리";
            // 
            // panSub05
            // 
            this.panSub05.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSub05.Controls.Add(this.panel3);
            this.panSub05.Controls.Add(this.btnSearch);
            this.panSub05.Controls.Add(this.txtSName);
            this.panSub05.Controls.Add(this.panel2);
            this.panSub05.Controls.Add(this.panel1);
            this.panSub05.Controls.Add(this.panDate);
            this.panSub05.Controls.Add(this.label1);
            this.panSub05.Controls.Add(this.btnSave);
            this.panSub05.Controls.Add(this.btnPrint);
            this.panSub05.Controls.Add(this.btnExit);
            this.panSub05.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSub05.Location = new System.Drawing.Point(0, 38);
            this.panSub05.Name = "panSub05";
            this.panSub05.Size = new System.Drawing.Size(1264, 42);
            this.panSub05.TabIndex = 16;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.rdoGubun3);
            this.panel3.Controls.Add(this.rdoGubun2);
            this.panel3.Controls.Add(this.rdoGubun1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(675, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(161, 40);
            this.panel3.TabIndex = 38;
            // 
            // rdoGubun3
            // 
            this.rdoGubun3.AutoSize = true;
            this.rdoGubun3.Location = new System.Drawing.Point(110, 8);
            this.rdoGubun3.Name = "rdoGubun3";
            this.rdoGubun3.Size = new System.Drawing.Size(52, 21);
            this.rdoGubun3.TabIndex = 2;
            this.rdoGubun3.Text = "종검";
            this.rdoGubun3.UseVisualStyleBackColor = true;
            // 
            // rdoGubun2
            // 
            this.rdoGubun2.AutoSize = true;
            this.rdoGubun2.Location = new System.Drawing.Point(57, 8);
            this.rdoGubun2.Name = "rdoGubun2";
            this.rdoGubun2.Size = new System.Drawing.Size(52, 21);
            this.rdoGubun2.TabIndex = 1;
            this.rdoGubun2.Text = "일검";
            this.rdoGubun2.UseVisualStyleBackColor = true;
            // 
            // rdoGubun1
            // 
            this.rdoGubun1.AutoSize = true;
            this.rdoGubun1.Checked = true;
            this.rdoGubun1.Location = new System.Drawing.Point(3, 8);
            this.rdoGubun1.Name = "rdoGubun1";
            this.rdoGubun1.Size = new System.Drawing.Size(52, 21);
            this.rdoGubun1.TabIndex = 0;
            this.rdoGubun1.TabStop = true;
            this.rdoGubun1.Text = "전체";
            this.rdoGubun1.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(990, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(68, 40);
            this.btnSearch.TabIndex = 37;
            this.btnSearch.Text = "조 회(&V)";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // txtSName
            // 
            this.txtSName.BackColor = System.Drawing.Color.White;
            this.txtSName.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtSName.Location = new System.Drawing.Point(841, 8);
            this.txtSName.Name = "txtSName";
            this.txtSName.Size = new System.Drawing.Size(115, 25);
            this.txtSName.TabIndex = 36;
            this.txtSName.Text = "김수한무거";
            this.txtSName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.chkChul1);
            this.panel2.Controls.Add(this.chkChul0);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(500, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(175, 40);
            this.panel2.TabIndex = 35;
            // 
            // chkChul1
            // 
            this.chkChul1.AutoSize = true;
            this.chkChul1.Checked = true;
            this.chkChul1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkChul1.Location = new System.Drawing.Point(90, 9);
            this.chkChul1.Name = "chkChul1";
            this.chkChul1.Size = new System.Drawing.Size(79, 21);
            this.chkChul1.TabIndex = 36;
            this.chkChul1.Text = "출장검진";
            this.chkChul1.UseVisualStyleBackColor = true;
            // 
            // chkChul0
            // 
            this.chkChul0.AutoSize = true;
            this.chkChul0.Checked = true;
            this.chkChul0.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkChul0.Location = new System.Drawing.Point(5, 9);
            this.chkChul0.Name = "chkChul0";
            this.chkChul0.Size = new System.Drawing.Size(79, 21);
            this.chkChul0.TabIndex = 35;
            this.chkChul0.Text = "내원검진";
            this.chkChul0.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.rdoJob2);
            this.panel1.Controls.Add(this.rdoJob1);
            this.panel1.Controls.Add(this.rdoJob0);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(310, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(190, 40);
            this.panel1.TabIndex = 33;
            // 
            // rdoJob2
            // 
            this.rdoJob2.AutoSize = true;
            this.rdoJob2.Checked = true;
            this.rdoJob2.Location = new System.Drawing.Point(119, 8);
            this.rdoJob2.Name = "rdoJob2";
            this.rdoJob2.Size = new System.Drawing.Size(65, 21);
            this.rdoJob2.TabIndex = 2;
            this.rdoJob2.TabStop = true;
            this.rdoJob2.Text = "미완료";
            this.rdoJob2.UseVisualStyleBackColor = true;
            // 
            // rdoJob1
            // 
            this.rdoJob1.AutoSize = true;
            this.rdoJob1.Location = new System.Drawing.Point(61, 8);
            this.rdoJob1.Name = "rdoJob1";
            this.rdoJob1.Size = new System.Drawing.Size(52, 21);
            this.rdoJob1.TabIndex = 1;
            this.rdoJob1.Text = "완료";
            this.rdoJob1.UseVisualStyleBackColor = true;
            // 
            // rdoJob0
            // 
            this.rdoJob0.AutoSize = true;
            this.rdoJob0.Location = new System.Drawing.Point(3, 8);
            this.rdoJob0.Name = "rdoJob0";
            this.rdoJob0.Size = new System.Drawing.Size(52, 21);
            this.rdoJob0.TabIndex = 0;
            this.rdoJob0.Text = "전체";
            this.rdoJob0.UseVisualStyleBackColor = true;
            // 
            // panDate
            // 
            this.panDate.Controls.Add(this.dtpTDate);
            this.panDate.Controls.Add(this.dtpFDate);
            this.panDate.Dock = System.Windows.Forms.DockStyle.Left;
            this.panDate.Location = new System.Drawing.Point(70, 0);
            this.panDate.Name = "panDate";
            this.panDate.Size = new System.Drawing.Size(240, 40);
            this.panDate.TabIndex = 32;
            // 
            // dtpTDate
            // 
            this.dtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTDate.Location = new System.Drawing.Point(121, 7);
            this.dtpTDate.Name = "dtpTDate";
            this.dtpTDate.Size = new System.Drawing.Size(109, 25);
            this.dtpTDate.TabIndex = 35;
            // 
            // dtpFDate
            // 
            this.dtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFDate.Location = new System.Drawing.Point(6, 7);
            this.dtpFDate.Name = "dtpFDate";
            this.dtpFDate.Size = new System.Drawing.Size(109, 25);
            this.dtpFDate.TabIndex = 34;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 40);
            this.label1.TabIndex = 29;
            this.label1.Text = "검진일자";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave.Location = new System.Drawing.Point(1058, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(68, 40);
            this.btnSave.TabIndex = 28;
            this.btnSave.Text = "저 장(&O)";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnPrint
            // 
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrint.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPrint.Location = new System.Drawing.Point(1126, 0);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(68, 40);
            this.btnPrint.TabIndex = 23;
            this.btnPrint.Text = "인 쇄(&P)";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(1194, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(68, 40);
            this.btnExit.TabIndex = 22;
            this.btnExit.Text = "닫 기(&X)";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // panMain
            // 
            this.panMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panMain.Controls.Add(this.Progress);
            this.panMain.Controls.Add(this.SS1);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(0, 80);
            this.panMain.Name = "panMain";
            this.panMain.Padding = new System.Windows.Forms.Padding(3);
            this.panMain.Size = new System.Drawing.Size(1264, 701);
            this.panMain.TabIndex = 17;
            // 
            // SS1
            // 
            this.SS1.AccessibleDescription = "";
            this.SS1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SS1.FocusRenderer = flatFocusIndicatorRenderer1;
            this.SS1.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SS1.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SS1.HorizontalScrollBar.Renderer = flatScrollBarRenderer1;
            this.SS1.HorizontalScrollBar.TabIndex = 31;
            this.SS1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SS1.Location = new System.Drawing.Point(3, 3);
            this.SS1.Name = "SS1";
            this.SS1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS1_Sheet1});
            this.SS1.Size = new System.Drawing.Size(1256, 693);
            this.SS1.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.SS1.TabIndex = 1;
            this.SS1.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SS1.VerticalScrollBar.Name = "";
            flatScrollBarRenderer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SS1.VerticalScrollBar.Renderer = flatScrollBarRenderer2;
            this.SS1.VerticalScrollBar.TabIndex = 32;
            this.SS1.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // SS1_Sheet1
            // 
            this.SS1_Sheet1.Reset();
            this.SS1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS1_Sheet1.ColumnCount = 5;
            this.SS1_Sheet1.RowCount = 1;
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.SS1_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.SS1_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SS1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS1_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.SS1_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.SS1_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // Progress
            // 
            this.Progress.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.Progress.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.Progress.Location = new System.Drawing.Point(531, 249);
            this.Progress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Progress.Name = "Progress";
            this.Progress.ProgressBarType = DevComponents.DotNetBar.eCircularProgressType.Dot;
            this.Progress.ProgressColor = System.Drawing.Color.Gray;
            this.Progress.Size = new System.Drawing.Size(200, 200);
            this.Progress.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeXP;
            this.Progress.TabIndex = 138;
            this.Progress.Visible = false;
            // 
            // frmHcPendList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 781);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.panSub05);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHcPendList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "보류대장";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panSub05.ResumeLayout(false);
            this.panSub05.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panDate.ResumeLayout(false);
            this.panMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panSub05;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panMain;
        private FarPoint.Win.Spread.FpSpread SS1;
        private FarPoint.Win.Spread.SheetView SS1_Sheet1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSName;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox chkChul1;
        private System.Windows.Forms.CheckBox chkChul0;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rdoJob2;
        private System.Windows.Forms.RadioButton rdoJob1;
        private System.Windows.Forms.RadioButton rdoJob0;
        private System.Windows.Forms.Panel panDate;
        private System.Windows.Forms.DateTimePicker dtpTDate;
        private System.Windows.Forms.DateTimePicker dtpFDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton rdoGubun3;
        private System.Windows.Forms.RadioButton rdoGubun2;
        private System.Windows.Forms.RadioButton rdoGubun1;
        private DevComponents.DotNetBar.Controls.CircularProgress Progress;
    }
}