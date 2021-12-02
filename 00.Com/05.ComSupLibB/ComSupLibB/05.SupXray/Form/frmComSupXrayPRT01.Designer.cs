namespace ComSupLibB.SupXray
{
    partial class frmComSupXrayPRT01
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
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("ko-KR", false);
            FarPoint.Win.ComplexBorder complexBorder1 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder2 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder3 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder4 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder5 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder6 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ssPrt = new FarPoint.Win.Spread.FpSpread();
            this.ssPrt_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panbtn1 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel9 = new System.Windows.Forms.Panel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.panel8 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel20 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel18 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblOut = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.line1 = new DevComponents.DotNetBar.Controls.Line();
            this.line2 = new DevComponents.DotNetBar.Controls.Line();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel10 = new System.Windows.Forms.Panel();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.line3 = new DevComponents.DotNetBar.Controls.Line();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPano = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSName = new System.Windows.Forms.TextBox();
            this.txtXName = new System.Windows.Forms.TextBox();
            this.txtXCode = new System.Windows.Forms.TextBox();
            this.panTitleSub0.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssPrt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssPrt_Sheet1)).BeginInit();
            this.panbtn1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 0);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(806, 36);
            this.panTitleSub0.TabIndex = 105;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 7);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(140, 17);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "영상 판독결과 및 인쇄";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.ssPrt);
            this.panel2.Controls.Add(this.panbtn1);
            this.panel2.Controls.Add(this.panel7);
            this.panel2.Controls.Add(this.panel18);
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 36);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(806, 42);
            this.panel2.TabIndex = 106;
            // 
            // ssPrt
            // 
            this.ssPrt.AccessibleDescription = "ssPrt, Sheet1, Row 0, Column 0, 방사선 촬영 결과지";
            this.ssPrt.Dock = System.Windows.Forms.DockStyle.Right;
            this.ssPrt.Location = new System.Drawing.Point(768, 0);
            this.ssPrt.Name = "ssPrt";
            this.ssPrt.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssPrt_Sheet1});
            this.ssPrt.Size = new System.Drawing.Size(36, 40);
            this.ssPrt.TabIndex = 12;
            this.ssPrt.Visible = false;
            this.ssPrt.SetViewportTopRow(0, 0, 5);
            this.ssPrt.SetActiveViewport(0, -1, 0);
            // 
            // ssPrt_Sheet1
            // 
            this.ssPrt_Sheet1.Reset();
            this.ssPrt_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssPrt_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssPrt_Sheet1.ColumnCount = 6;
            this.ssPrt_Sheet1.RowCount = 6;
            this.ssPrt_Sheet1.Cells.Get(0, 0).ColumnSpan = 6;
            this.ssPrt_Sheet1.Cells.Get(0, 0).Font = new System.Drawing.Font("굴림", 20F);
            this.ssPrt_Sheet1.Cells.Get(0, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(0, 0).Value = "방사선 촬영 결과지";
            this.ssPrt_Sheet1.Cells.Get(0, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.ssPrt_Sheet1.Cells.Get(0, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(0, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(0, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(0, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(0, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(0, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(0, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(0, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(1, 0).ColumnSpan = 6;
            this.ssPrt_Sheet1.Cells.Get(1, 0).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.Cells.Get(1, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssPrt_Sheet1.Cells.Get(1, 0).Value = "=================================================================================" +
    "==";
            this.ssPrt_Sheet1.Cells.Get(1, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(1, 1).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.Cells.Get(1, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(1, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(1, 2).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.Cells.Get(1, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(1, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(1, 3).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.Cells.Get(1, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(1, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(1, 4).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.Cells.Get(1, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(1, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(1, 5).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.Cells.Get(2, 0).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.Cells.Get(2, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssPrt_Sheet1.Cells.Get(2, 0).Value = "등록번호 :";
            this.ssPrt_Sheet1.Cells.Get(2, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(2, 1).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.Cells.Get(2, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssPrt_Sheet1.Cells.Get(2, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(2, 2).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.Cells.Get(2, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssPrt_Sheet1.Cells.Get(2, 2).Value = "성　　명 :";
            this.ssPrt_Sheet1.Cells.Get(2, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(2, 3).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.Cells.Get(2, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssPrt_Sheet1.Cells.Get(2, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(2, 4).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.Cells.Get(2, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssPrt_Sheet1.Cells.Get(2, 4).Value = "성    별 :";
            this.ssPrt_Sheet1.Cells.Get(2, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(2, 5).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.Cells.Get(2, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssPrt_Sheet1.Cells.Get(3, 0).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.Cells.Get(3, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssPrt_Sheet1.Cells.Get(3, 0).Value = "의 뢰 과 :";
            this.ssPrt_Sheet1.Cells.Get(3, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(3, 1).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.Cells.Get(3, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssPrt_Sheet1.Cells.Get(3, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.ssPrt_Sheet1.Cells.Get(3, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.ssPrt_Sheet1.Cells.Get(3, 1).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.ssPrt_Sheet1.Cells.Get(3, 1).ParseFormatString = "n";
            this.ssPrt_Sheet1.Cells.Get(3, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(3, 2).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.Cells.Get(3, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssPrt_Sheet1.Cells.Get(3, 2).Value = "의    사 :";
            this.ssPrt_Sheet1.Cells.Get(3, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(3, 3).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.Cells.Get(3, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssPrt_Sheet1.Cells.Get(3, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(3, 4).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.Cells.Get(3, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssPrt_Sheet1.Cells.Get(3, 4).Value = "검사요청일 :";
            this.ssPrt_Sheet1.Cells.Get(3, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(3, 5).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.Cells.Get(3, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssPrt_Sheet1.Cells.Get(3, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(4, 0).Border = complexBorder1;
            this.ssPrt_Sheet1.Cells.Get(4, 0).ColumnSpan = 6;
            this.ssPrt_Sheet1.Cells.Get(4, 0).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.Cells.Get(4, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssPrt_Sheet1.Cells.Get(4, 0).Value = "=================================================================================" +
    "==";
            this.ssPrt_Sheet1.Cells.Get(4, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(4, 1).Border = complexBorder2;
            this.ssPrt_Sheet1.Cells.Get(4, 1).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.Cells.Get(4, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(4, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(4, 2).Border = complexBorder3;
            this.ssPrt_Sheet1.Cells.Get(4, 2).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.Cells.Get(4, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssPrt_Sheet1.Cells.Get(4, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(4, 3).Border = complexBorder4;
            this.ssPrt_Sheet1.Cells.Get(4, 3).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.Cells.Get(4, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(4, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(4, 4).Border = complexBorder5;
            this.ssPrt_Sheet1.Cells.Get(4, 4).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.Cells.Get(4, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssPrt_Sheet1.Cells.Get(4, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(4, 5).Border = complexBorder6;
            this.ssPrt_Sheet1.Cells.Get(4, 5).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.Cells.Get(4, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(4, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(5, 0).ColumnSpan = 6;
            this.ssPrt_Sheet1.Cells.Get(5, 0).Font = new System.Drawing.Font("굴림", 9F);
            this.ssPrt_Sheet1.Cells.Get(5, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssPrt_Sheet1.Cells.Get(5, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPrt_Sheet1.Cells.Get(5, 1).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.Cells.Get(5, 2).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.Cells.Get(5, 3).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.Cells.Get(5, 4).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.Cells.Get(5, 5).Font = new System.Drawing.Font("굴림체", 10F);
            this.ssPrt_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPrt_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPrt_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssPrt_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPrt_Sheet1.ColumnHeader.Visible = false;
            this.ssPrt_Sheet1.Columns.Get(0).Width = 77F;
            this.ssPrt_Sheet1.Columns.Get(1).Width = 115F;
            this.ssPrt_Sheet1.Columns.Get(2).Width = 86F;
            this.ssPrt_Sheet1.Columns.Get(3).Width = 107F;
            this.ssPrt_Sheet1.Columns.Get(4).Width = 90F;
            this.ssPrt_Sheet1.Columns.Get(5).Width = 111F;
            this.ssPrt_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssPrt_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssPrt_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssPrt_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssPrt_Sheet1.FrozenRowCount = 5;
            this.ssPrt_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.None);
            this.ssPrt_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssPrt_Sheet1.RowHeader.Visible = false;
            this.ssPrt_Sheet1.Rows.Get(0).Height = 58F;
            this.ssPrt_Sheet1.Rows.Get(1).Height = 25F;
            this.ssPrt_Sheet1.Rows.Get(2).Height = 25F;
            this.ssPrt_Sheet1.Rows.Get(3).Height = 25F;
            this.ssPrt_Sheet1.Rows.Get(4).Height = 28F;
            this.ssPrt_Sheet1.Rows.Get(5).Height = 27F;
            this.ssPrt_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.None);
            this.ssPrt_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panbtn1
            // 
            this.panbtn1.Controls.Add(this.btnSearch);
            this.panbtn1.Controls.Add(this.panel9);
            this.panbtn1.Controls.Add(this.btnPrint);
            this.panbtn1.Controls.Add(this.panel8);
            this.panbtn1.Controls.Add(this.btnExit);
            this.panbtn1.Controls.Add(this.panel20);
            this.panbtn1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panbtn1.Location = new System.Drawing.Point(519, 0);
            this.panbtn1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panbtn1.Name = "panbtn1";
            this.panbtn1.Padding = new System.Windows.Forms.Padding(3);
            this.panbtn1.Size = new System.Drawing.Size(246, 40);
            this.panbtn1.TabIndex = 37;
            // 
            // btnSearch
            // 
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Location = new System.Drawing.Point(6, 3);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(70, 34);
            this.btnSearch.TabIndex = 34;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Visible = false;
            // 
            // panel9
            // 
            this.panel9.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel9.Location = new System.Drawing.Point(76, 3);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(2, 34);
            this.panel9.TabIndex = 168;
            // 
            // btnPrint
            // 
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrint.Location = new System.Drawing.Point(78, 3);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(91, 34);
            this.btnPrint.TabIndex = 30;
            this.btnPrint.Text = "판독결과지";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // panel8
            // 
            this.panel8.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel8.Location = new System.Drawing.Point(169, 3);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(2, 34);
            this.panel8.TabIndex = 167;
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(171, 3);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(70, 34);
            this.btnExit.TabIndex = 29;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // panel20
            // 
            this.panel20.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel20.Location = new System.Drawing.Point(241, 3);
            this.panel20.Name = "panel20";
            this.panel20.Size = new System.Drawing.Size(2, 34);
            this.panel20.TabIndex = 166;
            // 
            // panel7
            // 
            this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel7.Location = new System.Drawing.Point(502, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(17, 40);
            this.panel7.TabIndex = 45;
            // 
            // panel18
            // 
            this.panel18.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel18.Location = new System.Drawing.Point(490, 0);
            this.panel18.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel18.Name = "panel18";
            this.panel18.Size = new System.Drawing.Size(12, 40);
            this.panel18.TabIndex = 43;
            // 
            // panel6
            // 
            this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel6.Location = new System.Drawing.Point(404, 0);
            this.panel6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(86, 40);
            this.panel6.TabIndex = 42;
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel5.Location = new System.Drawing.Point(383, 0);
            this.panel5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(21, 40);
            this.panel5.TabIndex = 41;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblOut);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(270, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(113, 40);
            this.panel3.TabIndex = 39;
            // 
            // lblOut
            // 
            this.lblOut.AutoSize = true;
            this.lblOut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.lblOut.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblOut.ForeColor = System.Drawing.Color.Black;
            this.lblOut.Location = new System.Drawing.Point(8, 4);
            this.lblOut.Name = "lblOut";
            this.lblOut.Size = new System.Drawing.Size(97, 30);
            this.lblOut.TabIndex = 4;
            this.lblOut.Text = "외주판독";
            this.lblOut.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(270, 40);
            this.panel1.TabIndex = 40;
            // 
            // line1
            // 
            this.line1.Dock = System.Windows.Forms.DockStyle.Top;
            this.line1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.line1.Location = new System.Drawing.Point(0, 78);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(806, 8);
            this.line1.TabIndex = 123;
            this.line1.Text = "line1";
            this.line1.Thickness = 5;
            // 
            // line2
            // 
            this.line2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.line2.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.line2.Location = new System.Drawing.Point(0, 721);
            this.line2.Name = "line2";
            this.line2.Size = new System.Drawing.Size(806, 8);
            this.line2.TabIndex = 124;
            this.line2.Text = "line2";
            this.line2.Thickness = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel10);
            this.groupBox1.Controls.Add(this.line3);
            this.groupBox1.Controls.Add(this.panel4);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 86);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(806, 635);
            this.groupBox1.TabIndex = 125;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "환자정보 및 검사정보";
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.txtResult);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel10.Location = new System.Drawing.Point(3, 104);
            this.panel10.Name = "panel10";
            this.panel10.Padding = new System.Windows.Forms.Padding(5);
            this.panel10.Size = new System.Drawing.Size(800, 528);
            this.panel10.TabIndex = 125;
            // 
            // txtResult
            // 
            this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResult.Location = new System.Drawing.Point(5, 5);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(790, 518);
            this.txtResult.TabIndex = 2;
            // 
            // line3
            // 
            this.line3.Dock = System.Windows.Forms.DockStyle.Top;
            this.line3.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.line3.Location = new System.Drawing.Point(3, 96);
            this.line3.Name = "line3";
            this.line3.Size = new System.Drawing.Size(800, 8);
            this.line3.TabIndex = 124;
            this.line3.Text = "line3";
            this.line3.Thickness = 5;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.txtPano);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.txtSName);
            this.panel4.Controls.Add(this.txtXName);
            this.panel4.Controls.Add(this.txtXCode);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(3, 21);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(800, 75);
            this.panel4.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(10, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "등록번호";
            // 
            // txtPano
            // 
            this.txtPano.Location = new System.Drawing.Point(74, 14);
            this.txtPano.Name = "txtPano";
            this.txtPano.ReadOnly = true;
            this.txtPano.Size = new System.Drawing.Size(100, 25);
            this.txtPano.TabIndex = 5;
            this.txtPano.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(8, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "검사코드";
            // 
            // txtSName
            // 
            this.txtSName.Location = new System.Drawing.Point(180, 15);
            this.txtSName.Name = "txtSName";
            this.txtSName.ReadOnly = true;
            this.txtSName.Size = new System.Drawing.Size(100, 25);
            this.txtSName.TabIndex = 2;
            this.txtSName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtXName
            // 
            this.txtXName.Location = new System.Drawing.Point(180, 44);
            this.txtXName.Name = "txtXName";
            this.txtXName.ReadOnly = true;
            this.txtXName.Size = new System.Drawing.Size(455, 25);
            this.txtXName.TabIndex = 1;
            // 
            // txtXCode
            // 
            this.txtXCode.Location = new System.Drawing.Point(74, 44);
            this.txtXCode.Name = "txtXCode";
            this.txtXCode.ReadOnly = true;
            this.txtXCode.Size = new System.Drawing.Size(100, 25);
            this.txtXCode.TabIndex = 0;
            this.txtXCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // frmComSupXrayPRT01
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(806, 729);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.line2);
            this.Controls.Add(this.line1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panTitleSub0);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmComSupXrayPRT01";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmComSupXrayPRT01";
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssPrt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssPrt_Sheet1)).EndInit();
            this.panbtn1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panbtn1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel20;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel18;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel1;
        private DevComponents.DotNetBar.Controls.Line line1;
        private DevComponents.DotNetBar.Controls.Line line2;
        private System.Windows.Forms.Label lblOut;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.TextBox txtResult;
        private DevComponents.DotNetBar.Controls.Line line3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSName;
        private System.Windows.Forms.TextBox txtXName;
        private System.Windows.Forms.TextBox txtXCode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPano;
        private FarPoint.Win.Spread.FpSpread ssPrt;
        private FarPoint.Win.Spread.SheetView ssPrt_Sheet1;
    }
}