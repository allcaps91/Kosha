namespace ComHpcLibB
{
    partial class frmHcDocScheWeek
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
            this.panSub02 = new System.Windows.Forms.Panel();
            this.SSList = new FarPoint.Win.Spread.FpSpread();
            this.SSList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panSub01 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panSub02.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).BeginInit();
            this.panSub01.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // panSub02
            // 
            this.panSub02.BackColor = System.Drawing.Color.White;
            this.panSub02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSub02.Controls.Add(this.SSList);
            this.panSub02.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panSub02.Location = new System.Drawing.Point(0, 70);
            this.panSub02.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panSub02.Name = "panSub02";
            this.panSub02.Size = new System.Drawing.Size(639, 445);
            this.panSub02.TabIndex = 35;
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
            this.SSList.HorizontalScrollBar.TabIndex = 143;
            this.SSList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SSList.Location = new System.Drawing.Point(0, 0);
            this.SSList.Name = "SSList";
            this.SSList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSList_Sheet1});
            this.SSList.Size = new System.Drawing.Size(637, 443);
            this.SSList.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.SSList.TabIndex = 143;
            this.SSList.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SSList.VerticalScrollBar.Name = "";
            flatScrollBarRenderer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SSList.VerticalScrollBar.Renderer = flatScrollBarRenderer2;
            this.SSList.VerticalScrollBar.TabIndex = 144;
            this.SSList.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // SSList_Sheet1
            // 
            this.SSList_Sheet1.Reset();
            this.SSList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSList_Sheet1.ColumnCount = 21;
            this.SSList_Sheet1.ColumnHeader.RowCount = 2;
            this.SSList_Sheet1.RowCount = 1;
            this.SSList_Sheet1.Cells.Get(0, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Cells.Get(0, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Cells.Get(0, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 0).RowSpan = 2;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "진료과";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 1).RowSpan = 2;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "의사";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 2).RowSpan = 2;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "의사명";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 3).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "월";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 6).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "화";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 9).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "수";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 12).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "목";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 15).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 15).Value = "금";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 18).ColumnSpan = 3;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 18).Value = "토";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 3).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 4).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 5).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 6).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 7).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 8).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 9).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 10).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 11).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 12).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 13).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 14).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 15).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 16).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 17).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 18).Value = "AM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 19).Value = "PM";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(1, 20).Value = "야간";
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.Rows.Get(0).Height = 31F;
            this.SSList_Sheet1.Columns.Get(3).Label = "AM";
            this.SSList_Sheet1.Columns.Get(3).Width = 34F;
            this.SSList_Sheet1.Columns.Get(4).Label = "PM";
            this.SSList_Sheet1.Columns.Get(4).Width = 34F;
            this.SSList_Sheet1.Columns.Get(5).Label = "야간";
            this.SSList_Sheet1.Columns.Get(5).Width = 34F;
            this.SSList_Sheet1.Columns.Get(6).Label = "AM";
            this.SSList_Sheet1.Columns.Get(6).Width = 34F;
            this.SSList_Sheet1.Columns.Get(7).Label = "PM";
            this.SSList_Sheet1.Columns.Get(7).Width = 34F;
            this.SSList_Sheet1.Columns.Get(8).Label = "야간";
            this.SSList_Sheet1.Columns.Get(8).Width = 34F;
            this.SSList_Sheet1.Columns.Get(9).Label = "AM";
            this.SSList_Sheet1.Columns.Get(9).Locked = false;
            this.SSList_Sheet1.Columns.Get(9).Width = 34F;
            this.SSList_Sheet1.Columns.Get(10).Label = "PM";
            this.SSList_Sheet1.Columns.Get(10).Width = 34F;
            this.SSList_Sheet1.Columns.Get(11).Label = "야간";
            this.SSList_Sheet1.Columns.Get(11).Width = 34F;
            this.SSList_Sheet1.Columns.Get(12).Label = "AM";
            this.SSList_Sheet1.Columns.Get(12).Width = 34F;
            this.SSList_Sheet1.Columns.Get(13).Label = "PM";
            this.SSList_Sheet1.Columns.Get(13).Width = 34F;
            this.SSList_Sheet1.Columns.Get(14).Label = "야간";
            this.SSList_Sheet1.Columns.Get(14).Width = 34F;
            this.SSList_Sheet1.Columns.Get(15).Label = "AM";
            this.SSList_Sheet1.Columns.Get(15).Width = 34F;
            this.SSList_Sheet1.Columns.Get(16).Label = "PM";
            this.SSList_Sheet1.Columns.Get(16).Width = 34F;
            this.SSList_Sheet1.Columns.Get(17).Label = "야간";
            this.SSList_Sheet1.Columns.Get(17).Width = 34F;
            this.SSList_Sheet1.Columns.Get(18).Label = "AM";
            this.SSList_Sheet1.Columns.Get(18).Width = 34F;
            this.SSList_Sheet1.Columns.Get(19).Label = "PM";
            this.SSList_Sheet1.Columns.Get(19).Width = 34F;
            this.SSList_Sheet1.Columns.Get(20).Label = "야간";
            this.SSList_Sheet1.Columns.Get(20).Width = 34F;
            this.SSList_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.SSList_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.SSList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SSList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSList_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.SSList_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.Rows.Get(0).Height = 24F;
            this.SSList_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.SSList_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panSub01
            // 
            this.panSub01.BackColor = System.Drawing.Color.White;
            this.panSub01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSub01.Controls.Add(this.label5);
            this.panSub01.Controls.Add(this.btnSave);
            this.panSub01.Controls.Add(this.label4);
            this.panSub01.Controls.Add(this.label3);
            this.panSub01.Controls.Add(this.label2);
            this.panSub01.Controls.Add(this.label1);
            this.panSub01.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSub01.Location = new System.Drawing.Point(0, 35);
            this.panSub01.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panSub01.Name = "panSub01";
            this.panSub01.Size = new System.Drawing.Size(639, 35);
            this.panSub01.TabIndex = 34;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.LightPink;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label5.Location = new System.Drawing.Point(295, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 25);
            this.label5.TabIndex = 24;
            this.label5.Text = "9.OFF";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave.Location = new System.Drawing.Point(555, 0);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(82, 33);
            this.btnSave.TabIndex = 23;
            this.btnSave.Text = "저 장(&S)";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Location = new System.Drawing.Point(224, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 25);
            this.label4.TabIndex = 3;
            this.label4.Text = "4.휴진";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.DarkOrange;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Location = new System.Drawing.Point(153, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "3.특검";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Gold;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(82, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "2.수술";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Aqua;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(11, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "1.진료";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(639, 35);
            this.panTitle.TabIndex = 33;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(555, 0);
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
            this.lblTitle.Size = new System.Drawing.Size(182, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "검진의사 요일별 스케쥴";
            // 
            // frmHcDocScheWeek
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(639, 515);
            this.Controls.Add(this.panSub02);
            this.Controls.Add(this.panSub01);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHcDocScheWeek";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "요일별 스케줄 관리";
            this.panSub02.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).EndInit();
            this.panSub01.ResumeLayout(false);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panSub02;
        private System.Windows.Forms.Panel panSub01;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private FarPoint.Win.Spread.FpSpread SSList;
        private FarPoint.Win.Spread.SheetView SSList_Sheet1;
    }
}