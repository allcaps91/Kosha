namespace HC_IF
{
    partial class frmHcKiosk_ResultView
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
            this.components = new System.ComponentModel.Container();
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.FlatFocusIndicatorRenderer flatFocusIndicatorRenderer1 = new FarPoint.Win.Spread.FlatFocusIndicatorRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer1 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer2 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panBlank1 = new System.Windows.Forms.Panel();
            this.txtWRTNO = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblSName = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ssChk_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.ssChk = new FarPoint.Win.Spread.FpSpread();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panRemark = new DevComponents.DotNetBar.PanelEx();
            this.panMsg = new DevComponents.DotNetBar.PanelEx();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panTitle.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssChk_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssChk)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panTitle.Controls.Add(this.lblSName);
            this.panTitle.Controls.Add(this.panel1);
            this.panTitle.Controls.Add(this.txtWRTNO);
            this.panTitle.Controls.Add(this.panBlank1);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(498, 41);
            this.panTitle.TabIndex = 24;
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(399, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(97, 39);
            this.btnExit.TabIndex = 29;
            this.btnExit.Text = "닫 기(&X)";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.LightBlue;
            this.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(99, 39);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "접수번호";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panBlank1
            // 
            this.panBlank1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panBlank1.Location = new System.Drawing.Point(99, 0);
            this.panBlank1.Name = "panBlank1";
            this.panBlank1.Size = new System.Drawing.Size(10, 39);
            this.panBlank1.TabIndex = 31;
            // 
            // txtWRTNO
            // 
            this.txtWRTNO.BackColor = System.Drawing.Color.LemonChiffon;
            this.txtWRTNO.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtWRTNO.Font = new System.Drawing.Font("굴림체", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtWRTNO.Location = new System.Drawing.Point(109, 0);
            this.txtWRTNO.Name = "txtWRTNO";
            this.txtWRTNO.Size = new System.Drawing.Size(135, 39);
            this.txtWRTNO.TabIndex = 32;
            this.txtWRTNO.Text = "12345678";
            this.txtWRTNO.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(244, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 39);
            this.panel1.TabIndex = 33;
            // 
            // lblSName
            // 
            this.lblSName.BackColor = System.Drawing.Color.Wheat;
            this.lblSName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSName.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSName.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSName.Location = new System.Drawing.Point(254, 0);
            this.lblSName.Name = "lblSName";
            this.lblSName.Size = new System.Drawing.Size(139, 39);
            this.lblSName.TabIndex = 34;
            this.lblSName.Text = "홍길동(36/M)";
            this.lblSName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ssChk);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(0, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(498, 641);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "당일 검진 항목 체크";
            // 
            // ssChk_Sheet1
            // 
            this.ssChk_Sheet1.Reset();
            this.ssChk_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssChk_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssChk_Sheet1.ColumnCount = 4;
            this.ssChk_Sheet1.RowCount = 3;
            this.ssChk_Sheet1.Cells.Get(0, 0).Value = "동맹경화협착검사";
            this.ssChk_Sheet1.Cells.Get(0, 1).Value = "수검";
            this.ssChk_Sheet1.Cells.Get(0, 2).Value = "999";
            this.ssChk_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssChk_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssChk_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.ssChk_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssChk_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssChk_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssChk_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.ssChk_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssChk_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "구분";
            this.ssChk_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "상태";
            this.ssChk_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "대기";
            this.ssChk_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "Part";
            this.ssChk_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssChk_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssChk_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.ssChk_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssChk_Sheet1.ColumnHeader.Rows.Get(0).Height = 30F;
            this.ssChk_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssChk_Sheet1.Columns.Get(0).Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.ssChk_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssChk_Sheet1.Columns.Get(0).Label = "구분";
            this.ssChk_Sheet1.Columns.Get(0).Locked = true;
            this.ssChk_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssChk_Sheet1.Columns.Get(0).Width = 251F;
            this.ssChk_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssChk_Sheet1.Columns.Get(1).Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.ssChk_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssChk_Sheet1.Columns.Get(1).Label = "상태";
            this.ssChk_Sheet1.Columns.Get(1).Locked = true;
            this.ssChk_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssChk_Sheet1.Columns.Get(1).Width = 112F;
            this.ssChk_Sheet1.Columns.Get(2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(222)))), ((int)(((byte)(243)))));
            this.ssChk_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssChk_Sheet1.Columns.Get(2).Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.ssChk_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssChk_Sheet1.Columns.Get(2).Label = "대기";
            this.ssChk_Sheet1.Columns.Get(2).Locked = true;
            this.ssChk_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssChk_Sheet1.Columns.Get(2).Width = 112F;
            this.ssChk_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssChk_Sheet1.Columns.Get(3).Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.ssChk_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssChk_Sheet1.Columns.Get(3).Label = "Part";
            this.ssChk_Sheet1.Columns.Get(3).Locked = true;
            this.ssChk_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssChk_Sheet1.Columns.Get(3).Width = 73F;
            this.ssChk_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssChk_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssChk_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.ssChk_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssChk_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssChk_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssChk_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.ssChk_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssChk_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.ssChk_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssChk_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssChk_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssChk_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.ssChk_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssChk_Sheet1.RowHeader.Visible = false;
            this.ssChk_Sheet1.Rows.Get(0).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(1).Height = 32F;
            this.ssChk_Sheet1.Rows.Get(2).Height = 32F;
            this.ssChk_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssChk_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssChk_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.ssChk_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssChk_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.ssChk_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // ssChk
            // 
            this.ssChk.AccessibleDescription = "fpSpread1, Sheet1, Row 0, Column 0, 동맹경화협착검사";
            this.ssChk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssChk.FocusRenderer = flatFocusIndicatorRenderer1;
            this.ssChk.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssChk.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssChk.HorizontalScrollBar.Renderer = flatScrollBarRenderer1;
            this.ssChk.HorizontalScrollBar.TabIndex = 22;
            this.ssChk.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssChk.Location = new System.Drawing.Point(3, 25);
            this.ssChk.Name = "ssChk";
            this.ssChk.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssChk_Sheet1});
            this.ssChk.Size = new System.Drawing.Size(492, 613);
            this.ssChk.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.ssChk.TabIndex = 34;
            this.ssChk.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssChk.VerticalScrollBar.Name = "";
            flatScrollBarRenderer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssChk.VerticalScrollBar.Renderer = flatScrollBarRenderer2;
            this.ssChk.VerticalScrollBar.TabIndex = 23;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.panRemark);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 682);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(498, 44);
            this.panel2.TabIndex = 34;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 42);
            this.label2.TabIndex = 5;
            this.label2.Text = "참고사항";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panRemark
            // 
            this.panRemark.CanvasColor = System.Drawing.SystemColors.Control;
            this.panRemark.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panRemark.DisabledBackColor = System.Drawing.Color.Empty;
            this.panRemark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panRemark.Location = new System.Drawing.Point(45, 0);
            this.panRemark.Name = "panRemark";
            this.panRemark.Size = new System.Drawing.Size(451, 42);
            this.panRemark.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panRemark.Style.BackColor1.Color = System.Drawing.Color.FloralWhite;
            this.panRemark.Style.BackColor2.Color = System.Drawing.Color.Tan;
            this.panRemark.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panRemark.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panRemark.Style.ForeColor.Color = System.Drawing.Color.Black;
            this.panRemark.Style.GradientAngle = 90;
            this.panRemark.TabIndex = 6;
            this.panRemark.Text = "참고사항";
            // 
            // panMsg
            // 
            this.panMsg.CanvasColor = System.Drawing.SystemColors.Control;
            this.panMsg.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panMsg.DisabledBackColor = System.Drawing.Color.Empty;
            this.panMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMsg.Location = new System.Drawing.Point(0, 726);
            this.panMsg.Name = "panMsg";
            this.panMsg.Size = new System.Drawing.Size(498, 41);
            this.panMsg.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panMsg.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panMsg.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panMsg.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panMsg.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panMsg.Style.ForeColor.Color = System.Drawing.Color.Black;
            this.panMsg.Style.GradientAngle = 90;
            this.panMsg.TabIndex = 38;
            this.panMsg.Text = "panMsg";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            // 
            // frmHcKiosk_ResultView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 767);
            this.Controls.Add(this.panMsg);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHcKiosk_ResultView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Acting 결과조회";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssChk_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssChk)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblSName;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtWRTNO;
        private System.Windows.Forms.Panel panBlank1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private FarPoint.Win.Spread.FpSpread ssChk;
        private FarPoint.Win.Spread.SheetView ssChk_Sheet1;
        private System.Windows.Forms.Panel panel2;
        private DevComponents.DotNetBar.PanelEx panRemark;
        private System.Windows.Forms.Label label2;
        private DevComponents.DotNetBar.PanelEx panMsg;
        private System.Windows.Forms.Timer timer1;
    }
}