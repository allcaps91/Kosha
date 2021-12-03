namespace ComHpcLibB
{
    partial class frmHcDocSche
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
            this.btnShow = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panSub02 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panSub03 = new System.Windows.Forms.Panel();
            this.SSList = new FarPoint.Win.Spread.FpSpread();
            this.SSList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.btnPrint = new System.Windows.Forms.Button();
            this.panSub01 = new System.Windows.Forms.Panel();
            this.grbDept = new System.Windows.Forms.GroupBox();
            this.cboDEPT = new System.Windows.Forms.ComboBox();
            this.grbYYMM = new System.Windows.Forms.GroupBox();
            this.cboYYMM = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.panTitle.SuspendLayout();
            this.panSub02.SuspendLayout();
            this.panSub03.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).BeginInit();
            this.panSub01.SuspendLayout();
            this.grbDept.SuspendLayout();
            this.grbYYMM.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panTitle.Controls.Add(this.btnShow);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1063, 35);
            this.panTitle.TabIndex = 35;
            // 
            // btnShow
            // 
            this.btnShow.BackColor = System.Drawing.Color.White;
            this.btnShow.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnShow.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnShow.Location = new System.Drawing.Point(859, 0);
            this.btnShow.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(120, 33);
            this.btnShow.TabIndex = 21;
            this.btnShow.Text = "요일별 스케쥴";
            this.btnShow.UseVisualStyleBackColor = false;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(979, 0);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(82, 33);
            this.btnExit.TabIndex = 20;
            this.btnExit.Text = "종 료(&X)";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(3, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(128, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "검진의사 스케쥴";
            // 
            // panSub02
            // 
            this.panSub02.BackColor = System.Drawing.Color.White;
            this.panSub02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSub02.Controls.Add(this.label11);
            this.panSub02.Controls.Add(this.label10);
            this.panSub02.Controls.Add(this.label9);
            this.panSub02.Controls.Add(this.label8);
            this.panSub02.Controls.Add(this.label7);
            this.panSub02.Controls.Add(this.label6);
            this.panSub02.Controls.Add(this.label5);
            this.panSub02.Controls.Add(this.label4);
            this.panSub02.Controls.Add(this.label3);
            this.panSub02.Controls.Add(this.label2);
            this.panSub02.Controls.Add(this.label1);
            this.panSub02.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSub02.Location = new System.Drawing.Point(0, 85);
            this.panSub02.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panSub02.Name = "panSub02";
            this.panSub02.Size = new System.Drawing.Size(1063, 35);
            this.panSub02.TabIndex = 37;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label11.Location = new System.Drawing.Point(808, 3);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(99, 25);
            this.label11.TabIndex = 27;
            this.label11.Text = "C.채용상담";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label10.Location = new System.Drawing.Point(703, 3);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(99, 25);
            this.label10.TabIndex = 26;
            this.label10.Text = "B.출장검진";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.LightPink;
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label9.Location = new System.Drawing.Point(571, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 25);
            this.label9.TabIndex = 25;
            this.label9.Text = "9.OFF";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label8.Location = new System.Drawing.Point(500, 3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 25);
            this.label8.TabIndex = 11;
            this.label8.Text = "8.기타";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label7.Location = new System.Drawing.Point(429, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 25);
            this.label7.TabIndex = 10;
            this.label7.Text = "7.출장";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label6.Location = new System.Drawing.Point(358, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 25);
            this.label6.TabIndex = 9;
            this.label6.Text = "6.휴가";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label5.Location = new System.Drawing.Point(287, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 25);
            this.label5.TabIndex = 8;
            this.label5.Text = "5.학회";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Location = new System.Drawing.Point(216, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 25);
            this.label4.TabIndex = 7;
            this.label4.Text = "4.휴진";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.DarkOrange;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Location = new System.Drawing.Point(145, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 25);
            this.label3.TabIndex = 6;
            this.label3.Text = "3.특검";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Gold;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(74, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 25);
            this.label2.TabIndex = 5;
            this.label2.Text = "2.수술";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Aqua;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "1.진료";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panSub03
            // 
            this.panSub03.BackColor = System.Drawing.Color.White;
            this.panSub03.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSub03.Controls.Add(this.SSList);
            this.panSub03.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panSub03.Location = new System.Drawing.Point(0, 120);
            this.panSub03.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panSub03.Name = "panSub03";
            this.panSub03.Size = new System.Drawing.Size(1063, 517);
            this.panSub03.TabIndex = 38;
            // 
            // SSList
            // 
            this.SSList.AccessibleDescription = "SSList, Sheet1, Row 0, Column 0, ";
            this.SSList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSList.FocusRenderer = flatFocusIndicatorRenderer1;
            this.SSList.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SSList.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SSList.HorizontalScrollBar.Renderer = flatScrollBarRenderer1;
            this.SSList.HorizontalScrollBar.TabIndex = 222;
            this.SSList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SSList.Location = new System.Drawing.Point(0, 0);
            this.SSList.Name = "SSList";
            this.SSList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSList_Sheet1});
            this.SSList.Size = new System.Drawing.Size(1061, 515);
            this.SSList.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.SSList.TabIndex = 143;
            this.SSList.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SSList.VerticalScrollBar.Name = "";
            flatScrollBarRenderer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SSList.VerticalScrollBar.Renderer = flatScrollBarRenderer2;
            this.SSList.VerticalScrollBar.TabIndex = 223;
            this.SSList.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SSList.SetViewportLeftColumn(0, 0, 2);
            this.SSList.SetActiveViewport(0, 0, -1);
            // 
            // SSList_Sheet1
            // 
            this.SSList_Sheet1.Reset();
            this.SSList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSList_Sheet1.ColumnCount = 97;
            this.SSList_Sheet1.ColumnHeader.RowCount = 3;
            this.SSList_Sheet1.RowCount = 0;
            this.SSList_Sheet1.ActiveColumnIndex = -1;
            this.SSList_Sheet1.ActiveRowIndex = -1;
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 0).RowSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "진료과";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 1).RowSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "의사명";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 2).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 5).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 8).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 11).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 14).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 17).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 17).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 20).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 20).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 23).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 23).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 26).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 26).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 29).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 29).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 32).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 32).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 35).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 35).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 38).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 38).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 41).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 41).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 44).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 44).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 47).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 47).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 50).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 50).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 53).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 53).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 56).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 56).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 59).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 59).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 62).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 62).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 65).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 65).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 68).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 68).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 71).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 71).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 74).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 74).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 77).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 77).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 80).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 80).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 83).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 83).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 86).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 86).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 89).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 89).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 92).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 92).Value = "일수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 2).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 2).Value = "요일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 3).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 4).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 5).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 5).Value = "요일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 6).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 7).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 8).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 8).Value = "요일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 9).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 10).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 11).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 11).Value = "요일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 12).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 13).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 14).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 14).Value = "요일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 15).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 16).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 17).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 17).Value = "요일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 18).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 19).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 20).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 20).Value = "요일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 21).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 22).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 23).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 23).Value = "요일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 24).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 25).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 26).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 26).Value = "요일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 27).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 28).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 29).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 29).Value = "요일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 30).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 31).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 32).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 32).Value = "요일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 33).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 34).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 35).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 35).Value = "요일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 36).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 37).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 38).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 38).Value = "요일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 39).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 40).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 41).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 41).Value = "요일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 42).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 43).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 44).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 44).Value = "요일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 45).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 46).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 47).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 47).Value = "요일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 48).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 49).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 50).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 50).Value = "요일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 51).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 52).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 53).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 53).Value = "요일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 54).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 55).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 56).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 56).Value = "요일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 57).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 58).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 59).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 59).Value = "요일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 60).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 61).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 62).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 62).Value = "요일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 63).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 64).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 65).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 65).Value = "요일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 66).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 67).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 68).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 68).Value = "요일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 69).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 70).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 71).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 71).Value = "요일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 72).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 73).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 74).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 74).Value = "요일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 75).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 76).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 77).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 77).Value = "요일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 78).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 79).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 80).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 81).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 82).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 83).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 84).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 85).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 86).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 87).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 88).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 89).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 90).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 91).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 92).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 93).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 94).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 2).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 2).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 3).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 4).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 5).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 5).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 6).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 7).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 8).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 8).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 9).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 10).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 11).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 11).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 12).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 13).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 14).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 14).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 15).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 16).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 17).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 17).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 18).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 19).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 20).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 20).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 21).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 22).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 23).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 23).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 24).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 25).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 26).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 26).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 27).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 28).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 29).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 29).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 30).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 31).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 32).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 32).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 33).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 34).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 35).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 35).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 36).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 37).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 38).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 38).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 39).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 40).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 41).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 41).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 42).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 43).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 44).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 44).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 45).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 46).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 47).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 47).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 48).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 49).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 50).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 50).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 51).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 52).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 53).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 53).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 54).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 55).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 56).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 56).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 57).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 58).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 59).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 59).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 60).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 61).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 62).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 62).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 63).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 64).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 65).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 65).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 66).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 67).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 68).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 68).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 69).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 70).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 71).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 71).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 72).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 73).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 74).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 74).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 75).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 76).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 77).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 77).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 78).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 79).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 80).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 80).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 81).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 82).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 83).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 83).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 84).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 85).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 86).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 86).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 87).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 88).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 89).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 89).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 90).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 91).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 92).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 92).VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 93).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(2, 94).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.Rows.Get(0).Height = 31F;
            this.SSList_Sheet1.ColumnHeader.Rows.Get(2).Height = 39F;
            this.SSList_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(2).Label = "AM";
            this.SSList_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(2).Width = 34F;
            this.SSList_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(3).Label = "PM";
            this.SSList_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(3).Width = 34F;
            this.SSList_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(4).Label = "야간";
            this.SSList_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(4).Width = 34F;
            this.SSList_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(5).Label = "AM";
            this.SSList_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(5).Width = 34F;
            this.SSList_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(6).Label = "PM";
            this.SSList_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(6).Width = 34F;
            this.SSList_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(7).Label = "야간";
            this.SSList_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(7).Width = 34F;
            this.SSList_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(8).Label = "AM";
            this.SSList_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(8).Width = 34F;
            this.SSList_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(9).Label = "PM";
            this.SSList_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(9).Width = 34F;
            this.SSList_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(10).Label = "야간";
            this.SSList_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(10).Width = 34F;
            this.SSList_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(11).Label = "AM";
            this.SSList_Sheet1.Columns.Get(11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(11).Width = 34F;
            this.SSList_Sheet1.Columns.Get(12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(12).Label = "PM";
            this.SSList_Sheet1.Columns.Get(12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(12).Width = 34F;
            this.SSList_Sheet1.Columns.Get(13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(13).Label = "야간";
            this.SSList_Sheet1.Columns.Get(13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(13).Width = 34F;
            this.SSList_Sheet1.Columns.Get(14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(14).Label = "AM";
            this.SSList_Sheet1.Columns.Get(14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(14).Width = 34F;
            this.SSList_Sheet1.Columns.Get(15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(15).Label = "PM";
            this.SSList_Sheet1.Columns.Get(15).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(15).Width = 34F;
            this.SSList_Sheet1.Columns.Get(16).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(16).Label = "야간";
            this.SSList_Sheet1.Columns.Get(16).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(16).Width = 34F;
            this.SSList_Sheet1.Columns.Get(17).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(17).Label = "AM";
            this.SSList_Sheet1.Columns.Get(17).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(17).Width = 34F;
            this.SSList_Sheet1.Columns.Get(18).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(18).Label = "PM";
            this.SSList_Sheet1.Columns.Get(18).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(18).Width = 34F;
            this.SSList_Sheet1.Columns.Get(19).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(19).Label = "야간";
            this.SSList_Sheet1.Columns.Get(19).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(19).Width = 34F;
            this.SSList_Sheet1.Columns.Get(20).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(20).Label = "AM";
            this.SSList_Sheet1.Columns.Get(20).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(20).Width = 34F;
            this.SSList_Sheet1.Columns.Get(21).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(21).Label = "PM";
            this.SSList_Sheet1.Columns.Get(21).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(21).Width = 34F;
            this.SSList_Sheet1.Columns.Get(22).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(22).Label = "야간";
            this.SSList_Sheet1.Columns.Get(22).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(22).Width = 34F;
            this.SSList_Sheet1.Columns.Get(23).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(23).Label = "AM";
            this.SSList_Sheet1.Columns.Get(23).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(23).Width = 34F;
            this.SSList_Sheet1.Columns.Get(24).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(24).Label = "PM";
            this.SSList_Sheet1.Columns.Get(24).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(24).Width = 34F;
            this.SSList_Sheet1.Columns.Get(25).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(25).Label = "야간";
            this.SSList_Sheet1.Columns.Get(25).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(25).Width = 34F;
            this.SSList_Sheet1.Columns.Get(26).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(26).Label = "AM";
            this.SSList_Sheet1.Columns.Get(26).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(26).Width = 34F;
            this.SSList_Sheet1.Columns.Get(27).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(27).Label = "PM";
            this.SSList_Sheet1.Columns.Get(27).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(27).Width = 34F;
            this.SSList_Sheet1.Columns.Get(28).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(28).Label = "야간";
            this.SSList_Sheet1.Columns.Get(28).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(28).Width = 34F;
            this.SSList_Sheet1.Columns.Get(29).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(29).Label = "AM";
            this.SSList_Sheet1.Columns.Get(29).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(29).Width = 34F;
            this.SSList_Sheet1.Columns.Get(30).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(30).Label = "PM";
            this.SSList_Sheet1.Columns.Get(30).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(30).Width = 34F;
            this.SSList_Sheet1.Columns.Get(31).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(31).Label = "야간";
            this.SSList_Sheet1.Columns.Get(31).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(31).Width = 34F;
            this.SSList_Sheet1.Columns.Get(32).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(32).Label = "AM";
            this.SSList_Sheet1.Columns.Get(32).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(32).Width = 34F;
            this.SSList_Sheet1.Columns.Get(33).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(33).Label = "PM";
            this.SSList_Sheet1.Columns.Get(33).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(33).Width = 34F;
            this.SSList_Sheet1.Columns.Get(34).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(34).Label = "야간";
            this.SSList_Sheet1.Columns.Get(34).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(34).Width = 34F;
            this.SSList_Sheet1.Columns.Get(35).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(35).Label = "AM";
            this.SSList_Sheet1.Columns.Get(35).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(35).Width = 34F;
            this.SSList_Sheet1.Columns.Get(36).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(36).Label = "PM";
            this.SSList_Sheet1.Columns.Get(36).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(36).Width = 34F;
            this.SSList_Sheet1.Columns.Get(37).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(37).Label = "야간";
            this.SSList_Sheet1.Columns.Get(37).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(37).Width = 34F;
            this.SSList_Sheet1.Columns.Get(38).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(38).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(38).Width = 34F;
            this.SSList_Sheet1.Columns.Get(39).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(39).Label = "PM";
            this.SSList_Sheet1.Columns.Get(39).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(39).Width = 34F;
            this.SSList_Sheet1.Columns.Get(40).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(40).Label = "야간";
            this.SSList_Sheet1.Columns.Get(40).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(40).Width = 34F;
            this.SSList_Sheet1.Columns.Get(41).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(41).Label = "AM";
            this.SSList_Sheet1.Columns.Get(41).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(41).Width = 34F;
            this.SSList_Sheet1.Columns.Get(42).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(42).Label = "PM";
            this.SSList_Sheet1.Columns.Get(42).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(42).Width = 34F;
            this.SSList_Sheet1.Columns.Get(43).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(43).Label = "야간";
            this.SSList_Sheet1.Columns.Get(43).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(43).Width = 34F;
            this.SSList_Sheet1.Columns.Get(44).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(44).Label = "AM";
            this.SSList_Sheet1.Columns.Get(44).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(44).Width = 34F;
            this.SSList_Sheet1.Columns.Get(45).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(45).Label = "PM";
            this.SSList_Sheet1.Columns.Get(45).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(45).Width = 34F;
            this.SSList_Sheet1.Columns.Get(46).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(46).Label = "야간";
            this.SSList_Sheet1.Columns.Get(46).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(46).Width = 34F;
            this.SSList_Sheet1.Columns.Get(47).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(47).Label = "AM";
            this.SSList_Sheet1.Columns.Get(47).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(47).Width = 34F;
            this.SSList_Sheet1.Columns.Get(48).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(48).Label = "PM";
            this.SSList_Sheet1.Columns.Get(48).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(48).Width = 34F;
            this.SSList_Sheet1.Columns.Get(49).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(49).Label = "야간";
            this.SSList_Sheet1.Columns.Get(49).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(49).Width = 34F;
            this.SSList_Sheet1.Columns.Get(50).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(50).Label = "AM";
            this.SSList_Sheet1.Columns.Get(50).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(50).Width = 34F;
            this.SSList_Sheet1.Columns.Get(51).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(51).Label = "PM";
            this.SSList_Sheet1.Columns.Get(51).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(51).Width = 34F;
            this.SSList_Sheet1.Columns.Get(52).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(52).Label = "야간";
            this.SSList_Sheet1.Columns.Get(52).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(52).Width = 34F;
            this.SSList_Sheet1.Columns.Get(53).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(53).Label = "AM";
            this.SSList_Sheet1.Columns.Get(53).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(53).Width = 34F;
            this.SSList_Sheet1.Columns.Get(54).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(54).Label = "PM";
            this.SSList_Sheet1.Columns.Get(54).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(54).Width = 34F;
            this.SSList_Sheet1.Columns.Get(55).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(55).Label = "야간";
            this.SSList_Sheet1.Columns.Get(55).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(55).Width = 34F;
            this.SSList_Sheet1.Columns.Get(56).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(56).Label = "AM";
            this.SSList_Sheet1.Columns.Get(56).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(56).Width = 34F;
            this.SSList_Sheet1.Columns.Get(57).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(57).Label = "PM";
            this.SSList_Sheet1.Columns.Get(57).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(57).Width = 34F;
            this.SSList_Sheet1.Columns.Get(58).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(58).Label = "야간";
            this.SSList_Sheet1.Columns.Get(58).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(58).Width = 34F;
            this.SSList_Sheet1.Columns.Get(59).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(59).Label = "AM";
            this.SSList_Sheet1.Columns.Get(59).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(59).Width = 34F;
            this.SSList_Sheet1.Columns.Get(60).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(60).Label = "PM";
            this.SSList_Sheet1.Columns.Get(60).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(60).Width = 34F;
            this.SSList_Sheet1.Columns.Get(61).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(61).Label = "야간";
            this.SSList_Sheet1.Columns.Get(61).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(61).Width = 34F;
            this.SSList_Sheet1.Columns.Get(62).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(62).Label = "AM";
            this.SSList_Sheet1.Columns.Get(62).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(62).Width = 34F;
            this.SSList_Sheet1.Columns.Get(63).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(63).Label = "PM";
            this.SSList_Sheet1.Columns.Get(63).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(63).Width = 34F;
            this.SSList_Sheet1.Columns.Get(64).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(64).Label = "야간";
            this.SSList_Sheet1.Columns.Get(64).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(64).Width = 34F;
            this.SSList_Sheet1.Columns.Get(65).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(65).Label = "AM";
            this.SSList_Sheet1.Columns.Get(65).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(65).Width = 34F;
            this.SSList_Sheet1.Columns.Get(66).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(66).Label = "PM";
            this.SSList_Sheet1.Columns.Get(66).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(66).Width = 34F;
            this.SSList_Sheet1.Columns.Get(67).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(67).Label = "야간";
            this.SSList_Sheet1.Columns.Get(67).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(67).Width = 34F;
            this.SSList_Sheet1.Columns.Get(68).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(68).Label = "AM";
            this.SSList_Sheet1.Columns.Get(68).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(68).Width = 34F;
            this.SSList_Sheet1.Columns.Get(69).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(69).Label = "PM";
            this.SSList_Sheet1.Columns.Get(69).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(69).Width = 34F;
            this.SSList_Sheet1.Columns.Get(70).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(70).Label = "야간";
            this.SSList_Sheet1.Columns.Get(70).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(70).Width = 34F;
            this.SSList_Sheet1.Columns.Get(71).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(71).Label = "AM";
            this.SSList_Sheet1.Columns.Get(71).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(71).Width = 34F;
            this.SSList_Sheet1.Columns.Get(72).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(72).Label = "PM";
            this.SSList_Sheet1.Columns.Get(72).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(72).Width = 34F;
            this.SSList_Sheet1.Columns.Get(73).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(73).Label = "야간";
            this.SSList_Sheet1.Columns.Get(73).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(73).Width = 34F;
            this.SSList_Sheet1.Columns.Get(74).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(74).Label = "AM";
            this.SSList_Sheet1.Columns.Get(74).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(74).Width = 34F;
            this.SSList_Sheet1.Columns.Get(75).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(75).Label = "PM";
            this.SSList_Sheet1.Columns.Get(75).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(75).Width = 34F;
            this.SSList_Sheet1.Columns.Get(76).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(76).Label = "야간";
            this.SSList_Sheet1.Columns.Get(76).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(76).Width = 34F;
            this.SSList_Sheet1.Columns.Get(77).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(77).Label = "AM";
            this.SSList_Sheet1.Columns.Get(77).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(77).Width = 34F;
            this.SSList_Sheet1.Columns.Get(78).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(78).Label = "PM";
            this.SSList_Sheet1.Columns.Get(78).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(78).Width = 34F;
            this.SSList_Sheet1.Columns.Get(79).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(79).Label = "야간";
            this.SSList_Sheet1.Columns.Get(79).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(79).Width = 34F;
            this.SSList_Sheet1.Columns.Get(80).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(80).Label = "AM";
            this.SSList_Sheet1.Columns.Get(80).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(80).Width = 34F;
            this.SSList_Sheet1.Columns.Get(81).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(81).Label = "PM";
            this.SSList_Sheet1.Columns.Get(81).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(81).Width = 34F;
            this.SSList_Sheet1.Columns.Get(82).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(82).Label = "야간";
            this.SSList_Sheet1.Columns.Get(82).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(82).Width = 34F;
            this.SSList_Sheet1.Columns.Get(83).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(83).Label = "AM";
            this.SSList_Sheet1.Columns.Get(83).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(83).Width = 34F;
            this.SSList_Sheet1.Columns.Get(84).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(84).Label = "PM";
            this.SSList_Sheet1.Columns.Get(84).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(84).Width = 34F;
            this.SSList_Sheet1.Columns.Get(85).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(85).Label = "야간";
            this.SSList_Sheet1.Columns.Get(85).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(85).Width = 34F;
            this.SSList_Sheet1.Columns.Get(86).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(86).Label = "AM";
            this.SSList_Sheet1.Columns.Get(86).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(86).Width = 34F;
            this.SSList_Sheet1.Columns.Get(87).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(87).Label = "PM";
            this.SSList_Sheet1.Columns.Get(87).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(87).Width = 34F;
            this.SSList_Sheet1.Columns.Get(88).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(88).Label = "야간";
            this.SSList_Sheet1.Columns.Get(88).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(88).Width = 34F;
            this.SSList_Sheet1.Columns.Get(89).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(89).Label = "AM";
            this.SSList_Sheet1.Columns.Get(89).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(89).Width = 34F;
            this.SSList_Sheet1.Columns.Get(90).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(90).Label = "PM";
            this.SSList_Sheet1.Columns.Get(90).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(90).Width = 34F;
            this.SSList_Sheet1.Columns.Get(91).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(91).Label = "야간";
            this.SSList_Sheet1.Columns.Get(91).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(91).Width = 34F;
            this.SSList_Sheet1.Columns.Get(92).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(92).Label = "AM";
            this.SSList_Sheet1.Columns.Get(92).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(92).Width = 34F;
            this.SSList_Sheet1.Columns.Get(93).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(93).Label = "PM";
            this.SSList_Sheet1.Columns.Get(93).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(93).Width = 34F;
            this.SSList_Sheet1.Columns.Get(94).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(94).Label = "야간";
            this.SSList_Sheet1.Columns.Get(94).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(94).Width = 34F;
            this.SSList_Sheet1.Columns.Get(95).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(95).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.SSList_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.SSList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.FrozenColumnCount = 2;
            this.SSList_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SSList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSList_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.SSList_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.SSList_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.White;
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrint.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPrint.Location = new System.Drawing.Point(979, 0);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(82, 48);
            this.btnPrint.TabIndex = 23;
            this.btnPrint.Text = "인 쇄(&P)";
            this.btnPrint.UseVisualStyleBackColor = false;
            // 
            // panSub01
            // 
            this.panSub01.BackColor = System.Drawing.Color.White;
            this.panSub01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSub01.Controls.Add(this.grbDept);
            this.panSub01.Controls.Add(this.grbYYMM);
            this.panSub01.Controls.Add(this.btnSearch);
            this.panSub01.Controls.Add(this.btnSave);
            this.panSub01.Controls.Add(this.btnCancel);
            this.panSub01.Controls.Add(this.btnDelete);
            this.panSub01.Controls.Add(this.btnPrint);
            this.panSub01.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSub01.Location = new System.Drawing.Point(0, 35);
            this.panSub01.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panSub01.Name = "panSub01";
            this.panSub01.Size = new System.Drawing.Size(1063, 50);
            this.panSub01.TabIndex = 36;
            // 
            // grbDept
            // 
            this.grbDept.Controls.Add(this.cboDEPT);
            this.grbDept.Dock = System.Windows.Forms.DockStyle.Left;
            this.grbDept.Location = new System.Drawing.Point(169, 0);
            this.grbDept.Name = "grbDept";
            this.grbDept.Size = new System.Drawing.Size(169, 48);
            this.grbDept.TabIndex = 63;
            this.grbDept.TabStop = false;
            this.grbDept.Text = "진료과";
            this.grbDept.Visible = false;
            // 
            // cboDEPT
            // 
            this.cboDEPT.FormattingEnabled = true;
            this.cboDEPT.Location = new System.Drawing.Point(3, 22);
            this.cboDEPT.Name = "cboDEPT";
            this.cboDEPT.Size = new System.Drawing.Size(157, 25);
            this.cboDEPT.TabIndex = 1;
            // 
            // grbYYMM
            // 
            this.grbYYMM.Controls.Add(this.cboYYMM);
            this.grbYYMM.Dock = System.Windows.Forms.DockStyle.Left;
            this.grbYYMM.Location = new System.Drawing.Point(0, 0);
            this.grbYYMM.Name = "grbYYMM";
            this.grbYYMM.Size = new System.Drawing.Size(169, 48);
            this.grbYYMM.TabIndex = 62;
            this.grbYYMM.TabStop = false;
            this.grbYYMM.Text = "진료년월";
            // 
            // cboYYMM
            // 
            this.cboYYMM.FormattingEnabled = true;
            this.cboYYMM.Location = new System.Drawing.Point(3, 22);
            this.cboYYMM.Name = "cboYYMM";
            this.cboYYMM.Size = new System.Drawing.Size(157, 25);
            this.cboYYMM.TabIndex = 1;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(651, 0);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(82, 48);
            this.btnSearch.TabIndex = 27;
            this.btnSearch.Text = "조 회(&V)";
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave.Location = new System.Drawing.Point(733, 0);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(82, 48);
            this.btnSave.TabIndex = 26;
            this.btnSave.Text = "저 장(&S)";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCancel.Location = new System.Drawing.Point(815, 0);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(82, 48);
            this.btnCancel.TabIndex = 25;
            this.btnCancel.Text = "취 소(&C)";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.White;
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDelete.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDelete.Location = new System.Drawing.Point(897, 0);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(82, 48);
            this.btnDelete.TabIndex = 24;
            this.btnDelete.Text = "삭 제(&D)";
            this.btnDelete.UseVisualStyleBackColor = false;
            // 
            // frmHcDocSche
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1063, 637);
            this.Controls.Add(this.panSub03);
            this.Controls.Add(this.panSub02);
            this.Controls.Add(this.panSub01);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHcDocSche";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "검진의사 스케쥴 조회";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panSub02.ResumeLayout(false);
            this.panSub03.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).EndInit();
            this.panSub01.ResumeLayout(false);
            this.grbDept.ResumeLayout(false);
            this.grbYYMM.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panSub02;
        private System.Windows.Forms.Panel panSub03;
        private FarPoint.Win.Spread.FpSpread SSList;
        private FarPoint.Win.Spread.SheetView SSList_Sheet1;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Panel panSub01;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.GroupBox grbDept;
        private System.Windows.Forms.ComboBox cboDEPT;
        private System.Windows.Forms.GroupBox grbYYMM;
        private System.Windows.Forms.ComboBox cboYYMM;
        private System.Windows.Forms.Button btnShow;
    }
}